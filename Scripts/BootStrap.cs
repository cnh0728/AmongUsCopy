using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootStrap : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenuUI;

    private void Awake()
    {
        PlayerSettings.nickname = PlayerPrefs.GetString("nickname", "noName");
        PlayerSettings.controlType = (EControlType)PlayerPrefs.GetInt("controlType", 0);
    }

    private void Start()
    {
        mainMenuUI.SetActive(true);
    }


    
}
