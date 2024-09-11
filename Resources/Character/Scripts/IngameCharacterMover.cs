using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum EPlayerType //첫번째 비트가 크루원인지 임포스턴지, 두번째비트가 살아있는지
{
    Crew = 0b0,
    Imposter = 0b1,
    Ghost = 0b10,

    CrewAlive = 0b00,
    ImposterAlive = 0b01,
    CrewGhost = 0b10,
    ImposterGhost = 0b11,
}

public class IngameCharacterMover : CharacterMover
{
    [SyncVar(hook = nameof(SetPlayerType_Hook))]
    public EPlayerType playerType;

    public void SetPlayerType_Hook(EPlayerType oldValue, EPlayerType newValue)
    {
        if(isOwned && newValue == EPlayerType.ImposterAlive)
        {
            IngameUIManager.Instance.KillButtonUI.Show(this);
        }
    }

    [SerializeField]
    private PlayerFinder playerFinder;

    [SyncVar]
    private float killCooldown;
    public float KillCooldown { get { return killCooldown; } }

    public bool IsKillable { get { return killCooldown < 0f && playerFinder.Targets.Count > 0; } }

    [SyncVar]
    public bool isReporter = false;

    [SyncVar]
    public bool isVote;

    [SyncVar]
    public int vote;

    public EPlayerColor foundDeadbodyColor;

    [ClientRpc] //## 서버에서 클라이언트로 호출할 수 있게 붙여줌
    public void RpcTeleport(Vector3 position)
    {
        transform.position = position;
    }

    private void Init()
    {
        playerFinder.SetKillRange(GameSystem.Instance.killRange + 1f);

        if (isOwned)
        {
            var myRoomPlayer = AmongUsRoomPlayer.MyRoomPlayer;
            myRoomPlayer.myCharacter = this;
            CmdSetPlayerCharacter(myRoomPlayer.nickname, myRoomPlayer.playerColor);
        }
    }

    public void SetNicknameColor(EPlayerType type)
    {
        if(playerType == EPlayerType.ImposterAlive && type == EPlayerType.ImposterAlive)
        {
            nicknameText.color = IngameUIManager.Instance.IngameIntroUI.ImposterColor;
        }
    }

    public void InitKillCooldown()
    {
        if (isServer)
        {
            killCooldown = GameSystem.Instance.killCooldown;
        }
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        Init();

        StopMoveAnimation();

        GameSystem.Instance.AddPlayer(this);

    }

    // Update is called once per frame
    void Update()
    {
        if(isServer && playerType == EPlayerType.ImposterAlive)
        {
            killCooldown -= Time.deltaTime;
        }
    }

    public void Kill()
    {
        CmdKill(playerFinder.GetFirstTarget().netId);
    }

    [Command]
    private void CmdKill(uint targetNetId)
    {
        IngameCharacterMover target = null;
        foreach (var player in GameSystem.Instance.GetPlayerList())
        {
            if (player.netId == targetNetId)
            {
                target = player;
            }
        }

        if (target != null)
        {
            RpcTeleport(target.transform.position);
            target.Dead(false, playerColor);
            InitKillCooldown();
        }
    }


    [Command]
    private void CmdSetPlayerCharacter(string nickname, EPlayerColor color)
    {
        this.nickname = nickname;
        playerColor = color;
    }

    public void Dead(bool isEject, EPlayerColor imposterColor)
    {
        playerType |= EPlayerType.Ghost;
        RpcDead(isEject, imposterColor, playerColor);

        if (!isEject)
        {
            var manager = AmongUsRoomManager.singleton as AmongUsRoomManager;
            var deadbody = Instantiate(manager.spawnPrefabs[1], transform.position, transform.rotation).GetComponent<Deadbody>();
            NetworkServer.Spawn(deadbody.gameObject);
            deadbody.RpcSetColor(playerColor);
        }

        GameSystem.Instance.CheckGameState();
    }

    [ClientRpc]
    private void RpcDead(bool isEject, EPlayerColor imposterColor, EPlayerColor crewColor)
    {
        if (isOwned)
        {
            animator.SetBool("isGhost", true);
            if (!isEject)
            {
                IngameUIManager.Instance.KillUI.Open(imposterColor, crewColor);
            }

            var players = GameSystem.Instance.GetPlayerList();
            foreach (var player in players)
            {
                if((player.playerType & EPlayerType.Ghost) == EPlayerType.Ghost)
                {
                    player.SetVisibility(true);
                }
            }

            GameSystem.Instance.ChangeLightMode(playerType);
        }
        else
        {
            var myPlayer = AmongUsRoomPlayer.MyRoomPlayer.myCharacter as IngameCharacterMover;
            if(((int)myPlayer.playerType & (int)EPlayerType.Ghost) != (int)EPlayerType.Ghost)
            {
                SetVisibility(false);
            }
        }

        var collider = GetComponent<BoxCollider2D>();
        if (collider)
        {
            collider.enabled = false;
        }
    }

    public void Report()
    {
        CmdReport(foundDeadbodyColor);
    }

    [Command]
    public void CmdReport(EPlayerColor deadbodyColor)
    {
        isReporter = true;
        GameSystem.Instance.StartReportMeeting(deadbodyColor);
    }

    private void SetVisibility(bool isVisible)
    {
        if (isVisible)
        {
            spriteRenderer.enabled = true;
            nicknameText.text = nickname;
        }
        else
        {
            spriteRenderer.enabled = false;
            nicknameText.text = "";
        }
    }

    [Command]
    public void CmdVoteEjectPlayer(EPlayerColor ejectColor)
    {
        isVote = true;
        GameSystem.Instance.RpcSignVoteEject(playerColor, ejectColor);

        var players = FindObjectsOfType<IngameCharacterMover>();
        IngameCharacterMover ejectedPlayer = null;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].playerColor == ejectColor)
            {
                ejectedPlayer = players[i];
            }
        }
        ejectedPlayer.vote += 1;
        GameSystem.Instance.totalEjectedPlayer += 1;
    }

    [Command]
    public void CmdSkipVote()
    {
        isVote = true;
        GameSystem.Instance.skipVotePlayerCount += 1;
        GameSystem.Instance.totalEjectedPlayer += 1;
        GameSystem.Instance.RpcSignSkipVote(playerColor);
    }
}
