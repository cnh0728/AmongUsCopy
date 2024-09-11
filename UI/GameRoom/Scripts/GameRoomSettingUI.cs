using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoomSettingUI : SettingUI
{
    public void ExitGameRoom() {
        var manager = AmongUsRoomManager.singleton;

        if(manager.mode == Mirror.NetworkManagerMode.Host)
        {
            manager.StopHost();
        }
        else
        {
            manager.StopClient();
        }

    }

    public void Open()
    {
        AmongUsRoomPlayer.MyRoomPlayer.myCharacter.IsMovable = false;
        AmongUsRoomPlayer.MyRoomPlayer.myCharacter.StopMoveAnimation();
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        base.Close();

        AmongUsRoomPlayer.MyRoomPlayer.myCharacter.IsMovable = true;
    }
}
