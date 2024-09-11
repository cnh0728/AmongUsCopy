using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KillButtonUI : MonoBehaviour
{
    [SerializeField]
    private Button killButton;

    [SerializeField]
    private TextMeshProUGUI cooldownText;

    private IngameCharacterMover myPlayer;

    public void Show(IngameCharacterMover player)
    {
        gameObject.SetActive(true);
        myPlayer = player;
    }

    private void Update()
    {
        if (myPlayer != null)
        {
            if (myPlayer.IsKillable)
            {
                cooldownText.text = "";
                killButton.interactable = true;
            }
            else
            {
                cooldownText.text = myPlayer.KillCooldown > 0 ? ((int)myPlayer.KillCooldown).ToString() : "";
                killButton.interactable = false;
            }

        }
    }

    public void OnClickKillButton()
    {
        myPlayer.Kill();
    }
}
