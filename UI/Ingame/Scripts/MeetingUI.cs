using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum EMeetingState
{
    None,
    Meeting,
    Vote,
}

public class MeetingUI : MonoBehaviour
{
    [SerializeField]
    private Transform skipVoteParentTransform;

    [SerializeField]
    private Transform playerPanelsParent;

    [SerializeField]
    private GameObject voterPrefab;

    [SerializeField]
    private GameObject skipVoteButton;

    [SerializeField]
    private GameObject skipVotePlayers;

    [SerializeField]
    private GameObject playerPanelPrefab;

    [SerializeField]
    private TextMeshProUGUI meetingTimeText;

    private EMeetingState meetingState;

    private List<MeetingPlayerPanel> meetingPlayerPanels = new List<MeetingPlayerPanel>();

    private List<Image> skipVoters = new List<Image>();

    private IngameCharacterMover myCharacter;

    private int skipVoterIndex;

    public IngameCharacterMover MyCharacter
    {
        get
        {
            if (myCharacter == null)
            {
                myCharacter = AmongUsRoomPlayer.MyRoomPlayer.myCharacter as IngameCharacterMover;
            }
            return myCharacter;
        }
    }

    private void InitPanels()
    {
        skipVoterIndex = 0;

        for (int i = 0; i < skipVoters.Count; i++)
        {
            skipVoters[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < meetingPlayerPanels.Count; i++)
        {
            meetingPlayerPanels[i].InitVotePanel();
            meetingPlayerPanels[i].gameObject.SetActive(false);
        }

        skipVotePlayers.SetActive(false);
        skipVoteButton.SetActive(true);
    }

    public void Open()
    {
        InitPanels();
        
        int index = 0;
        
        MyCharacter.IsMovable = false;
        gameObject.SetActive(true);

        if (meetingPlayerPanels.Count > index)
        {
            meetingPlayerPanels[index].gameObject.SetActive(true);
        }
        else
        {
            var panel = Instantiate(playerPanelPrefab, playerPanelsParent).GetComponent<MeetingPlayerPanel>();
            panel.SetPlayer(MyCharacter);
            meetingPlayerPanels.Add(panel);
        }

        index++;

        var players = FindObjectsOfType<IngameCharacterMover>();
        foreach (var player in players)
        {
            if(player != MyCharacter)
            {
                if(meetingPlayerPanels.Count <= index)
                {
                    var panel = Instantiate(playerPanelPrefab, playerPanelsParent).GetComponent<MeetingPlayerPanel>();
                    panel.SetPlayer(player);
                    meetingPlayerPanels.Add(panel);
                }
                else
                {
                    meetingPlayerPanels[index].SetPlayer(player);
                    meetingPlayerPanels[index].gameObject.SetActive(true);
                }

                index++;
            }
        }

    }

    public void ChangeMeetingState(EMeetingState state)
    {
        meetingState = state;
    }

    public void SelectPlayerPanel()
    {
        foreach(var panel in meetingPlayerPanels)
        {
            panel.UnSelect();
        }
    }

    public void UpdateVote(EPlayerColor voterColor, EPlayerColor ejectColor)
    {
        foreach(var panel in meetingPlayerPanels)
        {
            if(panel.targetPlayer.playerColor == ejectColor)
            {
                panel.UpdatePanel(voterColor);
            }

            if(panel.targetPlayer.playerColor == voterColor)
            {
                panel.UpdateVoteSign(true);
            }
        }
    }

    public void UpdateSkipVotePlayer(EPlayerColor skipVotePlayerColor) 
    {
        foreach(var panel in meetingPlayerPanels)
        {
            if(panel.targetPlayer.playerColor == skipVotePlayerColor)
            {
                panel.UpdateVoteSign(true);
            }
        }

        Image voter;

        if (skipVoterIndex < skipVoters.Count)
        {
            voter = skipVoters[skipVoterIndex];
        }
        else
        {
            voter = Instantiate(voterPrefab, skipVoteParentTransform).GetComponent<Image>();
            voter.material = Instantiate(voter.material);
            skipVoters.Add(voter);
        }

        voter.material.SetColor("_PlayerColor", PlayerColor.GetColor(skipVotePlayerColor));

        skipVoterIndex++;

        if (myCharacter.isVote)
        {
            skipVoteButton.SetActive(false);
        }
    }

    public void OnClickSkipButton()
    {
        if (MyCharacter.isVote)
        {
            return;
        }

        MyCharacter.CmdSkipVote();
        SelectPlayerPanel();
    }

    public void CompleteVote()
    {
        foreach(var panel in meetingPlayerPanels)
        {
            panel.OpenResult();
        }

        skipVoteButton.SetActive(false);
        skipVotePlayers.SetActive(true);
    }

    private void Update()
    {
        if(meetingState == EMeetingState.Meeting)
        {
            meetingTimeText.text = string.Format("회의시간 : {0}s", (int)Mathf.Clamp(GameSystem.Instance.remainTime, 0f, float.MaxValue));
        }
        else if(meetingState == EMeetingState.Vote)
        {
            meetingTimeText.text = string.Format("투표시간 : {0}s", (int)Mathf.Clamp(GameSystem.Instance.remainTime, 0f, float.MaxValue));
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
