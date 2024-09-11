using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AmongUsRoomManager : NetworkRoomManager
{
    public GameRuleData gameRuleData;

    public int minPlayerCount;
    public int ImposterCount;

    public override void OnRoomServerConnect(NetworkConnectionToClient conn)
    {
        //새로 들어온 클라이언트를 감지했을때 동작하는 함수
        base.OnRoomServerConnect(conn);


    }

}
