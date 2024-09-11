using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OuttroUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI resultText;

    [SerializeField]
    private List<IntroCharacter> characterList;

    [SerializeField]
    private Image gradient;

    [SerializeField]
    private Animator animator;

    public void Open(GameState gameState)
    {
        gameObject.SetActive(true);

        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }

        animator.SetTrigger("on");

        ShowWinPlayer(gameState);
    }

    private void ShowWinPlayer(GameState gameState)
    {
        var players = GameSystem.Instance.GetPlayerList();

        string winText = "";
        Color winColor = Color.white;
        EPlayerType winType = EPlayerType.Ghost;

        if (gameState == GameState.CrewmateWin)
        {
            winColor = IngameUIManager.Instance.IngameIntroUI.CrewColor;
            winType = EPlayerType.Crew;
            winText = "크루원";
        }
        else if (gameState == GameState.ImposterWin)
        {
            winColor = IngameUIManager.Instance.IngameIntroUI.ImposterColor;
            winType = EPlayerType.Imposter;
            winText = "임포스터";
        }

        gradient.color = IngameUIManager.Instance.IngameIntroUI.CrewColor;
        resultText.text = $"{winText} 승리";

        int index = 0;

        foreach (var player in players)
        {
            if((player.playerType & winType) == winType)
            {
                characterList[index].SetIntroCharacter(player.nickname, player.playerColor);
                characterList[index].gameObject.SetActive(true);
                index++;
            }
        }
    }

    public void Close()
    {
        var manager = NetworkManager.singleton as AmongUsRoomManager;
        var player = AmongUsRoomPlayer.MyRoomPlayer;

        if (player.isServer)
        {
            manager.StopHost();
        }
        else
        {
            manager.StopClient();
        }
    }
}
