using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameRoomPlayerCounter : NetworkBehaviour
{
    [SyncVar]
    private int minPlayer;
    [SyncVar]
    private int maxPlayer;

    [SerializeField]
    private TextMeshProUGUI playerCountText;

    public void UpdatePlayerCount()
    {
        var players = FindObjectsOfType<AmongUsRoomPlayer>();
#if UNITY_EDITOR
        bool isStartable = true; //@@ µð¹ö±ë¿ë
#else
        bool isStartable = players.Length >= minPlayer; 
#endif
        playerCountText.color = isStartable ? Color.white : Color.red;
        playerCountText.text = string.Format("{0}/{1}", players.Length, maxPlayer);
        LobbyUIManger.Instance.SetInteractableStartButton(isStartable);
    }

    // Start is called before the first frame update
    void Start()
    {
        if(isServer)
        {
            var manager = NetworkManager.singleton as AmongUsRoomManager;
            minPlayer = manager.minPlayerCount;
            maxPlayer = manager.maxConnections;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
