using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EjectionUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI ejectionResultText;

    [SerializeField]
    private Image ejectionPlayer;

    [SerializeField]
    private RectTransform startPosition;

    [SerializeField]
    private RectTransform endPosition;

    [SerializeField]
    private float ejectionRotationSpeed = 1.0f;

    [SerializeField]
    private float ejectionMoveSpeed = 0.5f;

    void Start()
    {
        ejectionPlayer.material = Instantiate(ejectionPlayer.material);
    }

    public void Open(bool isEjection, EPlayerColor ejectionPlayerColor, bool isImposter, int remainImposterCount)
    {
        string text = "";
        IngameCharacterMover ejectPlayer = null;

        if (isEjection)
        {
            IngameCharacterMover[] players = FindObjectsOfType<IngameCharacterMover>();
            foreach(var player in players)
            {
                if(player.playerColor == ejectionPlayerColor)
                {
                    ejectPlayer = player;
                    break;
                }
            }
            text = string.Format("{0}�� ��������{1}\n�������Ͱ� {2}�� ���ҽ��ϴ�.",
                ejectPlayer.nickname, isImposter ? "�Դϴ�." : "�� �ƴϾ����ϴ�.", remainImposterCount);
        }
        else
        {
            text = string.Format("�ƹ��� ������� �ʾҽ��ϴ�.\n�������Ͱ� {0}�� ���ҽ��ϴ�.", remainImposterCount);
        }

        gameObject.SetActive(true);

        StartCoroutine(CoShowEjectionResult(ejectPlayer, text));
    }

    private IEnumerator CoShowEjectionResult(IngameCharacterMover ejectionPlayerMover, string text)
    {
        ejectionResultText.text = "";

        string forwardText = "";
        string backText = "";

        if (ejectionPlayerMover != null)
        {
            ejectionPlayer.material.SetColor("_PlayerColor", PlayerColor.GetColor(ejectionPlayerMover.playerColor));

            float timer = 0f;
            while (timer <= 1f)
            {
                yield return null;
                timer += Time.deltaTime * ejectionMoveSpeed;

                ejectionPlayer.rectTransform.anchoredPosition = Vector2.Lerp(startPosition.anchoredPosition, endPosition.anchoredPosition, timer);
                ejectionPlayer.rectTransform.rotation = Quaternion.Euler(ejectionPlayer.rectTransform.rotation.eulerAngles
                    + new Vector3(0f, 0f, -360f * Time.deltaTime * ejectionRotationSpeed));
            }

        }

        backText = text;

        while (backText.Length != 0)
        {
            forwardText += backText[0];
            backText = backText.Remove(0, 1);
            ejectionResultText.text = string.Format("<color=#FFFFFF>{0}</color><color=#000000>{1}</color>", forwardText, backText);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
