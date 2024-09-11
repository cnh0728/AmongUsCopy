using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameIntroUI : MonoBehaviour
{
    [SerializeField]
    private GameObject shhhObj;

    [SerializeField]
    private GameObject crewmateObj;

    [SerializeField]
    private TextMeshProUGUI playerType;

    [SerializeField]
    private Image gradientImage;

    [SerializeField]
    private IntroCharacter myCharacter;

    [SerializeField]
    private List<IntroCharacter> otherCharacters = new List<IntroCharacter>();

    [SerializeField]
    private Color crewColor;

    [SerializeField]
    public Color CrewColor { get { return crewColor; } }

    [SerializeField]
    private Color imposterColor;

    public Color ImposterColor { get { return imposterColor; } }

    [SerializeField]
    private CanvasGroup canvasGroup;

    private IngameCharacterMover myPlayer;

    public IngameCharacterMover MyPlayer { 
        get { 
            if(myPlayer == null)
            {
                var players = GameSystem.Instance.GetPlayerList();
                
                foreach (var player in players)
                {
                    if (player.isOwned)
                    {
                        myPlayer = player;
                        break;
                    }
                }
            }    
            return myPlayer;
        }
    }

    private void Awake()
    {
        shhhObj.SetActive(false);
        crewmateObj.SetActive(false);
    }

    public IEnumerator CoShowIntroSequence()
    {
        shhhObj.SetActive(true);
        shhhObj.GetComponent<Animator>().SetTrigger("on");
        yield return new WaitForSeconds(2f);
        shhhObj.GetComponent<Animator>().SetTrigger("off");
        yield return new WaitForSeconds(1f);
        shhhObj.SetActive(false);

        ShowPlayerType();
        crewmateObj.SetActive(true);
        crewmateObj.GetComponent<Animator>().SetTrigger("on");
    }

    public void Close()
    {
        StartCoroutine(CoFadeOut());
        MyPlayer.IsMovable = true;
    }

    private IEnumerator CoFadeOut()
    {
        float timer = 1f;
        
        while(timer >= 0f)
        {
            yield return null;
            timer -= Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, timer);
        }

        gameObject.SetActive(false);
    }

    public void ShowPlayerType()
    {
        var players = GameSystem.Instance.GetPlayerList();

        myCharacter.SetIntroCharacter(MyPlayer.nickname, MyPlayer.playerColor);

        string playerTypeText = "";
        Color playerTypeColor = Color.white;
        EPlayerType playerEType = EPlayerType.Crew;

        if(MyPlayer.playerType == EPlayerType.ImposterAlive)
        {
            playerTypeText = "임포스터";
            playerTypeColor = imposterColor;
            playerEType = EPlayerType.Imposter;
        }
        else
        {
            playerTypeText = "크루원";
            playerTypeColor = crewColor;
            playerEType = EPlayerType.Crew;
        }

        playerType.text = playerTypeText;
        playerType.color = playerTypeColor;
        gradientImage.color = playerTypeColor;

        int i = 0;
        foreach (var player in players)
        {
            if (!player.isOwned && (player.playerType & playerEType) == playerEType)
            {
                otherCharacters[i].SetIntroCharacter(player.nickname, player.playerColor);
                otherCharacters[i].gameObject.SetActive(true);
                i++;
            }
        }
    }
    
}
