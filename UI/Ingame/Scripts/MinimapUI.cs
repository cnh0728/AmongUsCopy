using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapUI : MonoBehaviour
{
    private enum rulerIndex
    {
        left,
        right, 
        top, 
        bottom,
    }

    [Tooltip ("left right top bottom 순서대로")]
    [SerializeField]
    private List<Transform> rulerPositions; //left right top bottom

    [SerializeField]
    private Image minimapImage;
    [SerializeField]
    private Image minimapPlayerImage;

    private CharacterMover targetPlayer;

    // Start is called before the first frame update
    void Start()
    {
        var matInst = Instantiate(minimapImage.material);
        minimapImage.material = matInst;

        targetPlayer = AmongUsRoomPlayer.MyRoomPlayer.myCharacter;
    }

    // Update is called once per frame
    void Update()
    {
        if(targetPlayer != null)
        {
            float top = rulerPositions[(int)rulerIndex.top].position.y;
            float bottom = rulerPositions[(int)rulerIndex.bottom].position.y;
            float left = rulerPositions[(int)rulerIndex.left].position.x;
            float right = rulerPositions[(int)rulerIndex.right].position.x;

            Vector2 mapArea = new Vector2(right - left, top - bottom);
            Vector2 charPos = new Vector2(targetPlayer.transform.position.x - left, targetPlayer.transform.position.y - bottom);
            Vector2 normalPos = new Vector2(charPos.x / mapArea.x, charPos.y / mapArea.y);

            minimapPlayerImage.rectTransform.anchoredPosition = 
                new Vector2(minimapImage.rectTransform.sizeDelta.x * normalPos.x, minimapImage.rectTransform.sizeDelta.y * normalPos.y);
        }
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
