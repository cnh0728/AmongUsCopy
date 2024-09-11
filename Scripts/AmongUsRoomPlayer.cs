using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmongUsRoomPlayer : NetworkRoomPlayer
{
    private static AmongUsRoomPlayer myRoomPlayer;
    public static AmongUsRoomPlayer MyRoomPlayer
    {
        get
        {
            if (myRoomPlayer == null)
            {
                var players = FindObjectsOfType<AmongUsRoomPlayer>();
                foreach (var player in players)
                {
                    if (player.isOwned)
                    {
                        myRoomPlayer = player;
                    }
                }
            }

            return myRoomPlayer;
        }
    }

    [SyncVar(hook = nameof(SetPlayerColor_Hook))]
    public EPlayerColor playerColor;
    public void SetPlayerColor_Hook(EPlayerColor oldColor, EPlayerColor newColor)
    {
        LobbyUIManger.Instance.CustomizeUI.UpdateUnSelectColorButton(oldColor);
        LobbyUIManger.Instance.CustomizeUI.UpdateSelectColorButton(newColor);
    }

    [SyncVar]
    public string nickname;

    public CharacterMover myCharacter;

    public override void Start()
    {
        base.Start();

        Init();

        if (isServer)
        {
            SpawnLobbyPlayerCharacter();
            LobbyUIManger.Instance.ActiveStartButton();
        }

        if (isLocalPlayer)
        {
            CmdSetNickname(PlayerSettings.nickname);
        }

        LobbyUIManger.Instance.GameRoomPlayerCounter.UpdatePlayerCount();
    }

    public void OnDestroy()
    {
        if (LobbyUIManger.Instance != null)
        {
            LobbyUIManger.Instance.GameRoomPlayerCounter.UpdatePlayerCount();
            LobbyUIManger.Instance.CustomizeUI.UpdateUnSelectColorButton(playerColor);
        }
    }

    private void Init()
    {
        LobbyUIManger.Instance.DisActiveStartButton();
    }

    [Command]
    public void CmdSetNickname(string nick)
    {
        nickname = nick;
        myCharacter.nickname = nick;
    }

    private void SpawnLobbyPlayerCharacter()
    {
        var roomSlots = (NetworkManager.singleton as AmongUsRoomManager).roomSlots;

        EPlayerColor color = EPlayerColor.Red;

        for (int i = 0; i <= (int)EPlayerColor.Lime; i++)
        {
            bool isFindSameColor = false;
            foreach (var roomPlayer in roomSlots)
            {
                var amongUsRoomPlayer = roomPlayer as AmongUsRoomPlayer;
                if(amongUsRoomPlayer.playerColor == (EPlayerColor)i && roomPlayer.netId != netId)
                {
                    isFindSameColor = true;
                    break;
                }
            }

            if (!isFindSameColor)
            {
                color = (EPlayerColor)i;
                break;
            }
        }

        playerColor = color;

        PosScale spawnInfo = FindObjectOfType<SpawnPositions>().GetSpawnInfo();

        var playerCharacter = Instantiate(AmongUsRoomManager.singleton.spawnPrefabs[0], spawnInfo.pos, Quaternion.identity).GetComponent<LobbyCharacterMover>();
        playerCharacter.transform.localScale = spawnInfo.scale;

        NetworkServer.Spawn(playerCharacter.gameObject, connectionToClient);
        playerCharacter.ownerNetId = netId;
        playerCharacter.playerColor = color;

    }

    [Command] //서버에서 실행시키도록 하는 함수 접두사로 반드시 Cmd가 있어야함
    public void CmdSetPlayerColor(EPlayerColor color)
    {
        playerColor = color;
        myCharacter.playerColor = color;
    }

}
