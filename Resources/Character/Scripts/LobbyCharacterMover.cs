using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCharacterMover : CharacterMover
{
    [SyncVar(hook = nameof(SetOwnerNetId_Hook))]
    public uint ownerNetId;

    public override void Start()
    {
        base.Start();
    }

    public void SetOwnerNetId_Hook(uint preOwnerId, uint newOwnerId)
    {
        var players = FindObjectsOfType<AmongUsRoomPlayer>();
        foreach(var player in players)
        {
            if(newOwnerId == player.netId)
            {
                player.myCharacter = this;
                break;
            }
        }
    }

    public void CompleteSpawn()
    {
        if (isOwned)
        {
            IsMovable = true;
        }
    }
}
