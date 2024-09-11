using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReportUI : MonoBehaviour
{
    [SerializeField]
    private Image deadbodyImage;

    public void Open(EPlayerColor deadbodyColor)
    {
        AmongUsRoomPlayer.MyRoomPlayer.myCharacter.IsMovable = false;

        Material matInst = Instantiate(deadbodyImage.material);
        deadbodyImage.material = matInst;

        gameObject.SetActive(true);

        deadbodyImage.material.SetColor("_PlayerColor", PlayerColor.GetColor(deadbodyColor));

    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void OnClickButton()
    {
        var myCharacter = AmongUsRoomPlayer.MyRoomPlayer.myCharacter as IngameCharacterMover;
        myCharacter.Report();
    }
}
