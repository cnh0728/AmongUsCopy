using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillUI : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    private Image imposterImage;

    [SerializeField]
    private Image crewImage;

    public void Open(EPlayerColor imposterColor, EPlayerColor crewColor)
    {
        AmongUsRoomPlayer.MyRoomPlayer.myCharacter.IsMovable = false;

        Material imposterMatInst = Instantiate(imposterImage.material);
        imposterImage.material = imposterMatInst;
        Material crewMatInst = Instantiate(crewImage.material);
        crewImage.material = crewMatInst;

        gameObject.SetActive(true);

        imposterImage.material.SetColor("_PlayerColor", PlayerColor.GetColor(imposterColor));
        crewImage.material.SetColor("_PlayerColor", PlayerColor.GetColor(crewColor));

        Invoke("Close", 3f);

    }

    public void Close()
    {
        gameObject.SetActive(false);
        AmongUsRoomPlayer.MyRoomPlayer.myCharacter.IsMovable = true;
    }
}
