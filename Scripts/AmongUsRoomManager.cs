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
        //���� ���� Ŭ���̾�Ʈ�� ���������� �����ϴ� �Լ�
        base.OnRoomServerConnect(conn);


    }

}
