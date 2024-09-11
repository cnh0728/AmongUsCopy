using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LobbyUIManger : MonoBehaviour
{
    public static LobbyUIManger Instance;

    [SerializeField]
    private CustomizeUI customizeUI;
    public CustomizeUI CustomizeUI { get { return customizeUI; } }

    [SerializeField]
    private Button useButton;
    [SerializeField]
    private Sprite originUseButtonSprite;

    [SerializeField]
    private GameRoomPlayerCounter gameRoomPlayerCounter;
    public GameRoomPlayerCounter GameRoomPlayerCounter { get { return gameRoomPlayerCounter; } }

    [SerializeField]
    private Button startButton;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    public void SetUseButton(Sprite sprite, UnityAction action)
    {
        useButton.image.sprite = sprite;
        useButton.onClick.AddListener(action);
        useButton.interactable = true;
    }
    public void UnSetUseButton()
    {
        useButton.image.sprite = originUseButtonSprite;
        useButton.onClick.RemoveAllListeners();
        useButton.interactable = false;
    }

    public void ActiveStartButton()
    {
        startButton.gameObject.SetActive(true);
    }

    public void DisActiveStartButton()
    {
        startButton.gameObject.SetActive(false);
    }

    public void SetInteractableStartButton(bool isInteractable)
    {
        startButton.interactable = isInteractable;
    }

    public void OnClickStartButton()
    {
        var manager = NetworkManager.singleton as AmongUsRoomManager;
        manager.gameRuleData = FindObjectOfType<GameruleStore>().GameRuleData;

        var players = FindObjectsOfType<AmongUsRoomPlayer>();
        for (int i=0; i<players.Length; i++)
        {
            players[i].CmdChangeReadyState(true);
        }

        manager.ServerChangeScene(manager.GameplayScene);
    }

}
