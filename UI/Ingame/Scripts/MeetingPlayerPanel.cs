using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MeetingPlayerPanel : MonoBehaviour
{
    [SerializeField]
    private Image characterImage;

    [SerializeField]
    private TextMeshProUGUI nicknameText;

    [SerializeField]
    private GameObject deadPlayerBlock;

    [SerializeField]
    private GameObject reportSign;

    [SerializeField]
    private GameObject voteButtons;

    [HideInInspector]
    public IngameCharacterMover targetPlayer;

    [SerializeField]
    private GameObject voteSign;

    [SerializeField]
    private GameObject voterPrefab;

    [SerializeField]
    private Transform voterParentTransform;

    private List<Image> voters = new List<Image>();

    private int votersIndex;

    private IngameCharacterMover myCharacter;
    public IngameCharacterMover MyCharacter { 
        get 
        { 
            if (myCharacter == null)
            {
                myCharacter = AmongUsRoomPlayer.MyRoomPlayer.myCharacter as IngameCharacterMover;
            }
            return myCharacter; 
        } 
    }

    public void InitVotePanel()
    {
        votersIndex = 0;
        voteSign.SetActive(false);

        for (int i = 0; i < voters.Count; i++)
        {
            voters[i].gameObject.SetActive(false);
        }
    }

    public void UpdatePanel(EPlayerColor voterColor)
    {
        Image voter;

        if(voters.Count > votersIndex)
        {
            voter = voters[votersIndex];
        }
        else
        {
            voter = Instantiate(voterPrefab, voterParentTransform).GetComponent<Image>();        
            voter.material = Instantiate(voter.material);
        }
        voter.material.SetColor("_PlayerColor", PlayerColor.GetColor(voterColor));
        votersIndex++;
    }

    public void OpenResult()
    {
        voterParentTransform.gameObject.SetActive(true);
    }

    public void UpdateVoteSign(bool isVoted)
    {
        voteSign.SetActive(isVoted);
    }

    public void SetPlayer(IngameCharacterMover target)
    {
        Material matInst = Instantiate(characterImage.material);
        characterImage.material = matInst;

        targetPlayer = target;
        characterImage.material.SetColor("_PlayerColor", PlayerColor.GetColor(target.playerColor));
        nicknameText.text = target.nickname;

        if(((MyCharacter.playerType & EPlayerType.Imposter) == EPlayerType.Imposter) 
            && ((targetPlayer.playerType & EPlayerType.Imposter) == EPlayerType.Imposter))
        {
            nicknameText.color = IngameUIManager.Instance.IngameIntroUI.ImposterColor;
        }

        bool isDead = (targetPlayer.playerType & EPlayerType.Ghost) == EPlayerType.Ghost;

        deadPlayerBlock.SetActive(isDead);
        GetComponent<Button>().interactable = !isDead;
        reportSign.SetActive(targetPlayer.isReporter);
    }

    public void OnClickPlayerPanel()
    {
        if (myCharacter.isVote)
        {
            return;
        }

        if((MyCharacter.playerType & EPlayerType.Ghost) != EPlayerType.Ghost)
        {
            IngameUIManager.Instance.MeetingUI.SelectPlayerPanel();
            voteButtons.SetActive(true);
        }
    }

    public void Select()
    {
        myCharacter.CmdVoteEjectPlayer(targetPlayer.playerColor);
        UnSelect();
    }

    public void UnSelect()
    {
        voteButtons.SetActive(false);
    }

}
