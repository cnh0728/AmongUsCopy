using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomUI : MonoBehaviour
{

    [SerializeField]
    private List<Image> crewImages;

    [SerializeField]
    private List<Button> imposterCountButtons;

    [SerializeField]
    private List<Button> maxPlayerCountButtons;

    public CreateGameRoomData roomData;

    private readonly int minPlayerCountToMakeRoom = 4;

    private int minPlayerCount;

    void Start()
    {
        for(int i = 0; i < crewImages.Count; i++)
        {
            Material materialInstance = Instantiate(crewImages[i].material);
            crewImages[i].material = materialInstance;
        }

        roomData = new CreateGameRoomData() { imposterCount = 1, maxPlayerCount = 10 };
        UpdateCrewImages();
    }

    public void UpdateMaxPlayerCount(int count)
    {
        roomData.maxPlayerCount = count;

        for(int i = 0; i < maxPlayerCountButtons.Count; i++)
        {
            if (i == count - minPlayerCountToMakeRoom)
            {
                maxPlayerCountButtons[i].image.color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                maxPlayerCountButtons[i].image.color = new Color(1f, 1f, 1f, 0f);
            }
        }

        int maxImposterCount = count >= 9 ? 3 : count >= 7 ? 2 : 1;

        if(roomData.imposterCount > maxImposterCount)
        {
            UpdateImposterCount(maxImposterCount);
        }

        for(int i= 0;i< imposterCountButtons.Count; i++)
        {
            var text = imposterCountButtons[i].GetComponentInChildren<TextMeshProUGUI>();

            if(i + 1 > maxImposterCount)
            {
                imposterCountButtons[i].interactable = false;
                text.color = Color.gray;
            }
            else
            {
                imposterCountButtons[i].interactable= true;
                text.color = Color.white;
            }
        }

        UpdateCrewImages();
    }

    public void UpdateImposterCount(int count)
    {
        roomData.imposterCount = count;

        for( int i = 0;i < imposterCountButtons.Count; i++)
        {
            if(i == count - 1)
            {
                imposterCountButtons[i].image.color = new Color(1f, 1f , 1f, 1f);
            }
            else
            {
                imposterCountButtons[i].image.color = new Color(1f, 1f, 1f, 0f);
            }
        }

        UpdateCrewImages();
    }

    private void UpdateCrewImages()
    {
        for(int i = 0;i < crewImages.Count; i++)
        {
            crewImages[i].material.SetColor("_PlayerColor", Color.white);
        }

        int imposterCount = roomData.imposterCount;
        int idx = 0;

        while(imposterCount != 0)
        {
            if(idx >= roomData.maxPlayerCount)
            {
                idx = 0;
            }

            if (crewImages[idx].material.GetColor("_PlayerColor") != Color.red && Random.Range(0, 5) == 0) //20% 확률
            {
                crewImages[idx].material.SetColor("_PlayerColor", Color.red);
                imposterCount--;
            }

            idx++;
        }

        for(int i= 0; i < crewImages.Count; i++)
        {
            if (i < roomData.maxPlayerCount)
            {
                crewImages[i].gameObject.SetActive(true);
            }
            else
            {
                crewImages[i].gameObject.SetActive(false);
            }
        }
    }

    public void CreateRoom()
    {
        var manager = NetworkManager.singleton as AmongUsRoomManager;
        //방 설정작업 처리
        manager.minPlayerCount = roomData.imposterCount <= 1 ? 4 : roomData.imposterCount == 2 ? 7 : 9;
        manager.ImposterCount = roomData.imposterCount;
        manager.maxConnections = roomData.maxPlayerCount;
        //

        manager.StartHost();

    }
}


public class CreateGameRoomData
{
    public int imposterCount;
    public int maxPlayerCount;
}