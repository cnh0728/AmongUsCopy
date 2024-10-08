using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeUI : MonoBehaviour
{
    [SerializeField]
    private Button colorButton;
    [SerializeField]
    private GameObject colorPanel;
    [SerializeField]
    private Button gameRuleButton;
    [SerializeField]
    private GameObject gameRulePanel;

    [SerializeField]
    private Image characterPreview;

    [SerializeField]
    private List<ColorSelect> colorSelectButtons;

    private void Start()
    {
        var matInst = Instantiate(characterPreview.material);
        characterPreview.material = matInst;

    }

    private void OnEnable()
    {
        UpdateColorButton();
        ActiveColorPanel();

        var roomSlots = (NetworkManager.singleton as AmongUsRoomManager).roomSlots;
        foreach (var player in roomSlots)
        {
            var aPlayer = player as AmongUsRoomPlayer;
            if (aPlayer.isLocalPlayer)
            {
                UpdatePreviewColor(aPlayer.playerColor);
                break;
            }
        }
    }

    public void UpdateSelectColorButton(EPlayerColor color)
    {
        colorSelectButtons[(int)color].SetInteractable(false);
    }

    public void UpdateUnSelectColorButton(EPlayerColor color)
    {
        colorSelectButtons[(int)color].SetInteractable(true);
    }

    public void ActiveColorPanel()
    {
        colorButton.image.color = new Color(0f, 0f, 0f, 0.75f);
        gameRuleButton.image.color = new Color(0f, 0f, 0f, 0.25f);

        colorPanel.SetActive(true);
        gameRulePanel.SetActive(false);
    }
    public void ActiveGameRulePanel()
    {
        colorButton.image.color = new Color(0f, 0f, 0f, 0.25f);
        gameRuleButton.image.color = new Color(0f, 0f, 0f, 0.75f);

        colorPanel.SetActive(false);
        gameRulePanel.SetActive(true);
    }


    public void UpdateColorButton()
    {
        var roomSlots = (NetworkManager.singleton as AmongUsRoomManager).roomSlots;

        for (int i = 0; i < colorSelectButtons.Count; i++)
        {
            colorSelectButtons[i].SetInteractable(true);
        }

        foreach (var player in roomSlots)
        {
            var aPlayer = player as AmongUsRoomPlayer;
            colorSelectButtons[(int)aPlayer.playerColor].SetInteractable(false);
        }
    }

    public void UpdatePreviewColor(EPlayerColor color)
    {
        characterPreview.material.SetColor("_PlayerColor", PlayerColor.GetColor(color));
    }

    public void OnClickColorButton(int index)
    {
        if (colorSelectButtons[index].isInteractable)
        {
            AmongUsRoomPlayer.MyRoomPlayer.CmdSetPlayerColor((EPlayerColor)index);
            UpdatePreviewColor((EPlayerColor)index);
        }
    }

    public void Open()
    {
        AmongUsRoomPlayer.MyRoomPlayer.myCharacter.IsMovable = false;
        AmongUsRoomPlayer.MyRoomPlayer.myCharacter.StopMoveAnimation();
        gameObject.SetActive(true);
    }

    public void Close()
    {
        AmongUsRoomPlayer.MyRoomPlayer.myCharacter.IsMovable = true;
        gameObject.SetActive(false);
    }
}
