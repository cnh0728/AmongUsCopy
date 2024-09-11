using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseUI : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> ClosesUI;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        foreach (var close in ClosesUI)
        {
            close.SetActive(false);
        }
    }
}
