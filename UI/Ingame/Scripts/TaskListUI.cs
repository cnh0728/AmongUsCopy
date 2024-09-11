using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TaskListUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private float offset;

    [SerializeField]
    private RectTransform TaskListUITransform;

    [SerializeField]
    private float uiOpenSpeed = 0.5f;

    private bool isOpen = true;

    private float timer;

    private Coroutine coToggleOpenUI;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(coToggleOpenUI != null)
        {
            StopCoroutine(coToggleOpenUI);
        }

        coToggleOpenUI = StartCoroutine(CoToggleOpenUI());
    }

    private IEnumerator CoToggleOpenUI()
    {
        isOpen = !isOpen;

        if(timer != 0f)
        {
            timer = uiOpenSpeed - timer;
        }

        while(timer <= uiOpenSpeed)
        {
            timer += Time.deltaTime;

            float start = isOpen ? -TaskListUITransform.sizeDelta.x : offset;
            float dest = isOpen ? offset : -TaskListUITransform.sizeDelta.x;
            TaskListUITransform.anchoredPosition = new Vector2(Mathf.Lerp(start, dest, timer/uiOpenSpeed), TaskListUITransform.anchoredPosition.y);
            yield return null;
        }
    }

}
