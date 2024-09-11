using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField]
    private Button MouseControlButton;
    [SerializeField]
    private Button KeyboardMouseControlButton;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        RefreshControlTypeSetting();
    }

    public void SetControlMode(int controlType)
    {
        PlayerSettings.controlType = (EControlType)controlType;

        RefreshControlTypeSetting();
    }

    private void RefreshControlTypeSetting()
    {
        switch (PlayerSettings.controlType)
        {
            case EControlType.Mouse:
                MouseControlButton.image.color = Color.green;
                KeyboardMouseControlButton.image.color = Color.white;
                break;

            case EControlType.KeyboardMouse:
                MouseControlButton.image.color = Color.white;
                KeyboardMouseControlButton.image.color = Color.green;
                break;
        }
    }

    private void SaveSetting()
    {
        PlayerPrefs.SetInt("controlType", (int)PlayerSettings.controlType);
    }

    public virtual void Close()
    {
        SaveSetting();

        StartCoroutine(CoCloseAfterDelay());
    }

    protected IEnumerator CoCloseAfterDelay() {
        animator.SetTrigger("close");
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
