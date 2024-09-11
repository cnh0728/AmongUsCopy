using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntroCharacter : MonoBehaviour
{
    [SerializeField]
    private Image character;

    [SerializeField]
    private TextMeshProUGUI nickname;

    public void SetIntroCharacter(string nick, EPlayerColor playerColor)
    {
        var matInst = Instantiate(character.material);
        character.material = matInst;

        nickname.text = nick;
        character.material.SetColor("_PlayerColor", PlayerColor.GetColor(playerColor));
    }
}
