using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OnlineUI : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField nicknameInputField;
    [SerializeField]
    private GameObject createRoomUI;

    private void OnEnable()
    {
        nicknameInputField.text = PlayerSettings.nickname;
    }

    public void OnClickCreateButton()
    {
        if (CheckNicknameEmpty()) {
            createRoomUI.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void OnClickEnterGameRoomButton()
    {
        if (CheckNicknameEmpty())
        {
            var manager = AmongUsRoomManager.singleton;

            manager.StartClient();
        }
    }

    private bool CheckNicknameEmpty()
    {
        if (nicknameInputField.text != "")
        {
            PlayerSettings.nickname = nicknameInputField.text;
            PlayerPrefs.SetString("nickname", PlayerSettings.nickname);

            return true;
        }
        else
        {
            nicknameInputField.GetComponent<Animator>().SetTrigger("on");
        }
        return false;
    }
}
