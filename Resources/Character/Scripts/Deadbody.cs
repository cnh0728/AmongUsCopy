using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deadbody : NetworkBehaviour
{
    private SpriteRenderer spriteRenderer;
    private EPlayerColor deadbodyColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //서버에서 불러서 클라이언트에서 동작하는 함수
    [ClientRpc]
    public void RpcSetColor(EPlayerColor color)
    {
        deadbodyColor = color;
        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(color));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<IngameCharacterMover>();
        if (player != null && player.isOwned && (player.playerType & EPlayerType.Ghost) != EPlayerType.Ghost)
        {
            IngameUIManager.Instance.ReportButtonUI.SetInteractable(true);
            var myCharacter = AmongUsRoomPlayer.MyRoomPlayer.myCharacter as IngameCharacterMover;
            myCharacter.foundDeadbodyColor = deadbodyColor;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.GetComponent<IngameCharacterMover>();
        if (player != null && player.isOwned && (player.playerType & EPlayerType.Ghost) != EPlayerType.Ghost)
        {
            IngameUIManager.Instance.ReportButtonUI.SetInteractable(false);
        }
    }
}
