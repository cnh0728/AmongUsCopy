using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public enum GameState
{
    Playing,
    ImposterWin,
    CrewmateWin,
}

public class GameSystem : NetworkBehaviour
{
    public static GameSystem Instance;

    //게임 캐릭터가 스폰될때 스스로 시스템의 players에 등록함
    private List<IngameCharacterMover> players = new List<IngameCharacterMover>();

    [SerializeField]
    private Transform spawnTransform;

    [SerializeField]
    private float spawnDistance;

    [SyncVar]
    public float killCooldown;

    [SyncVar]
    public int killRange;

    [SyncVar]
    public int skipVotePlayerCount;

    [SyncVar]
    public float remainTime;

    [SerializeField]
    private Light2D shadowLight;

    [SerializeField]
    private Light2D lightMapLight;

    [SerializeField]
    private Light2D globalLight;

    [SerializeField]
    private float defaultGlobalLightIntensity = 0.3f;

    [SerializeField]
    private float defaultShadowLightIntensity = 0.5f;

    [SerializeField]
    private float ejectionAnimationDuration = 10f;

    [SyncVar]
    public int totalAlivePlayer;

    [SyncVar]
    public int totalEjectedPlayer;

    private Coroutine coStartingMeeting;

    private AmongUsRoomManager manager;
    public AmongUsRoomManager Manager 
    {  
        get {
            if (manager == null)
            {
                manager = NetworkManager.singleton as AmongUsRoomManager;
            }
            return manager; 
        } 
    }

    public void AddPlayer(IngameCharacterMover player)
    {
        if (!players.Contains(player))
        {
            players.Add(player);
        }
    }

    private IEnumerator CoGameReady()
    {
#if UNITY_EDITOR
        killCooldown = 5; //디버깅용
#else
        killCooldown = manager.gameRuleData.killCooldown;
#endif
        killRange = (int)Manager.gameRuleData.killRange;

        //모두 게임방에 진입할때까지 대기
        while (Manager.roomSlots.Count != players.Count)
        {
            yield return null;
        }

        //임포스터 수만큼 선정
        for (int i = 0; i < Manager.ImposterCount; i++)
        {
            var player = players[Random.Range(0, players.Count)];
            if(player.playerType != EPlayerType.ImposterAlive)
            {
                player.playerType = EPlayerType.ImposterAlive;
            }
            else
            {
                i--;
            }
        }

        AllocatePlayerToAroundSpawnPoint(players.ToArray());

        yield return new WaitForSeconds(1f);
        RpcStartGame();

        foreach(var player in players)
        {
            player.InitKillCooldown();
        }

    }

    public void CheckGameState()
    {
        int aliveCrewmate = 0;
        int aliveImposter = 0;

        foreach (var player in players) {
            if(player.playerType == EPlayerType.CrewAlive)
            {
                aliveCrewmate += 1; 
            }
            else if (player.playerType == EPlayerType.ImposterAlive)
            {
                aliveImposter += 1;
            }
        }

        //@@ Task 게이지 다채워도 임포스터 윈 해당해야함
        if (aliveCrewmate <= aliveImposter)
        {
            RPCEndGame(GameState.ImposterWin);
        }
        else if(aliveImposter <= 0)
        {
            RPCEndGame(GameState.CrewmateWin);
        }
    }

    [ClientRpc]
    private void RPCEndGame(GameState gameState)
    {
        StartCoroutine(CoEndGame(gameState));
    }

    private IEnumerator CoEndGame(GameState gameState)
    {
        IngameUIManager.Instance.OuttroUI.Open(gameState);

        yield return null;
    }

    private void AllocatePlayerToAroundSpawnPoint(IngameCharacterMover[] players)
    {
        //특정 스폰포인트를 기준으로 원을 그리며 스폰
        for (int i = 0; i < players.Length; i++)
        {
            float radian = ((2f * Mathf.PI) / players.Length) * i;
            players[i].RpcTeleport(spawnTransform.position + (new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0f) * spawnDistance));
        }
    }

    [ClientRpc] //서버와 클라이언트 둘다 사용해야함.
    private void RpcStartGame()
    {
        StartCoroutine(CoStartGameCoroutine());
    }

    private IEnumerator CoStartGameCoroutine()
    {
        yield return StartCoroutine(IngameUIManager.Instance.IngameIntroUI.CoShowIntroSequence());

        IngameCharacterMover myCharacter = null;
        foreach (var player in players)
        {
            if (player.isOwned)
            {
                myCharacter = player;
            }
        }

        foreach (var player in players)
        {
            player.SetNicknameColor(myCharacter.playerType);
        }

        yield return new WaitForSeconds(3f);
        IngameUIManager.Instance.IngameIntroUI.Close();
    }

    public List<IngameCharacterMover> GetPlayerList()
    {
        return players; 
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (isServer)
        {
            StartCoroutine(CoGameReady());
        }
    }

    public void ChangeLightMode(EPlayerType type)
    {
        if((type & EPlayerType.Ghost) != 0)
        {
            lightMapLight.lightType = Light2D.LightType.Global;
            shadowLight.intensity = 0f;
            globalLight.intensity = 1f;
        }
        else
        {
            lightMapLight.lightType = Light2D.LightType.Point;
            shadowLight.intensity = defaultShadowLightIntensity;
            globalLight.intensity = defaultGlobalLightIntensity;
        }
    }

    public void StartReportMeeting(EPlayerColor deadbodyColor)
    {
        RpcSendReportSign(deadbodyColor);
        StartCoroutine(CoMeetingProcess());
    }

    private IEnumerator CoStartMeeting(UnityAction endAction)
    {
        yield return new WaitForSeconds(3f);
        IngameUIManager.Instance.ReportUI.Close();
        IngameUIManager.Instance.MeetingUI.Open();
        IngameUIManager.Instance.MeetingUI.ChangeMeetingState(EMeetingState.Meeting);

        if(endAction != null)
        {
            endAction();
        }
    }

    private IEnumerator CoMeetingProcess()
    {
        var players = FindObjectsOfType<IngameCharacterMover>();
        foreach(var player in players)
        {
            //회의시간에는 투표를 못하게 하기위해 투표를 한 것처럼 해서 막는다
            player.isVote = true;
        }

        yield return new WaitForSeconds(3f);

        remainTime = Manager.gameRuleData.meetingsTime;
        while (remainTime > 0f)
        {
            remainTime -= Time.deltaTime;
            yield return null;
        }


        totalAlivePlayer = 0;
        foreach (var player in players) 
        {
            if((player.playerType & EPlayerType.Ghost) != EPlayerType.Ghost)
            {
                player.isVote = false;
                totalAlivePlayer += 1;
            }
            player.vote = 0;
        }

        RpcStartVoteTime();
        remainTime = Manager.gameRuleData.voteTime;
        totalEjectedPlayer = 0;
        while (remainTime > 0f)
        {
            if (totalAlivePlayer == totalEjectedPlayer)
            {
                break;
            }
            remainTime -= Time.deltaTime;
            yield return null; 
        }

        foreach(var player in players)
        {
            if(!player.isVote && (player.playerType & EPlayerType.Ghost) != EPlayerType.Ghost)
            {
                player.isVote = true;
                skipVotePlayerCount += 1;
                RpcSignSkipVote(player.playerColor);
            }
        }

        RpcEndVoteTime();

        yield return new WaitForSeconds(3f);

        StartCoroutine(CoCalculateVoteResult(players));
    }

    private class CharacterVoteComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            IngameCharacterMover xPlayer = (IngameCharacterMover)x;
            IngameCharacterMover yPlayer = (IngameCharacterMover)y;
            return xPlayer.vote <= yPlayer.vote ? 1: -1;
        }
    }

    private IEnumerator CoCalculateVoteResult(IngameCharacterMover[] players)
    {
        System.Array.Sort(players, new CharacterVoteComparer());

        int remainImposter = 0;
        foreach (var player in players)
        {
            if((player.playerType & EPlayerType.ImposterAlive) == EPlayerType.ImposterAlive)
            {
                remainImposter++;
            }
        }

        if(skipVotePlayerCount >= players[0].vote)
        {
            RpcOpenEjectionUI(false, EPlayerColor.Black, false, remainImposter);
        }
        else if(skipVotePlayerCount >= players[1].vote)
        {
            RpcOpenEjectionUI(false, EPlayerColor.Black, false, remainImposter);
        }
        else
        {
            bool isImposter = (players[0].playerType & EPlayerType.Imposter) == EPlayerType.Imposter;
            RpcOpenEjectionUI(true, players[0].playerColor, isImposter, isImposter ? remainImposter - 1 : remainImposter);

            players[0].Dead(true, players[0].playerColor);
        }

        var deadbodies = FindObjectsOfType<Deadbody>();
        for (int i = 0; i < deadbodies.Length; i++)
        {
            Destroy(deadbodies[i].gameObject);
        }

        AllocatePlayerToAroundSpawnPoint(players);

        //추방 애니메이션이 끝날때까지 대기
        yield return new WaitForSeconds(ejectionAnimationDuration);

        RpcCloseEjectionUI();
    }

    [ClientRpc]
    public void RpcOpenEjectionUI(bool isEjection, EPlayerColor ejectionPlayerColor, bool isImposter, int remainImposterCount)
    {
        IngameUIManager.Instance.EjectionUI.Open(isEjection, ejectionPlayerColor, isImposter, remainImposterCount);
        IngameUIManager.Instance.MeetingUI.Close();
    }

    [ClientRpc]
    public void RpcCloseEjectionUI()
    {
        CheckGameState();
        IngameUIManager.Instance.EjectionUI.Close();
        AmongUsRoomPlayer.MyRoomPlayer.myCharacter.IsMovable = true;
    }

    [ClientRpc]
    public void RpcStartVoteTime()
    {
        IngameUIManager.Instance.MeetingUI.ChangeMeetingState(EMeetingState.Vote);
    }

    [ClientRpc]
    public void RpcEndVoteTime()
    {
        IngameUIManager.Instance.MeetingUI.CompleteVote();
    }

    [ClientRpc]
    private void RpcSendReportSign(EPlayerColor deadbodyColor)
    {
        if(coStartingMeeting != null)
        {
            return;
        }

        IngameUIManager.Instance.ReportUI.Open(deadbodyColor);

        coStartingMeeting = StartCoroutine(CoStartMeeting(EndMeeting));
    }

    public void EndMeeting()
    {
        coStartingMeeting = null;
    }

    [ClientRpc]
    public void RpcSignVoteEject(EPlayerColor voterColor, EPlayerColor ejectColor)
    {
        IngameUIManager.Instance.MeetingUI.UpdateVote(voterColor, ejectColor);
    }

    [ClientRpc]
    public void RpcSignSkipVote(EPlayerColor skipVotePlayerColor)
    {
        IngameUIManager.Instance.MeetingUI.UpdateSkipVotePlayer(skipVotePlayerColor);
    }
}
