using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixWiringTaskObject : MonoBehaviour
{
    [SerializeField]
    private Sprite useButtonSprite;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material = Instantiate(spriteRenderer.material);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var character = collision.GetComponent<IngameCharacterMover>();
        if (character != null && character.isOwned)
        {
            spriteRenderer.material.SetFloat("_Highlighted", 0f);
            IngameUIManager.Instance.SetUseButton(useButtonSprite, OnClickUse);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var character = collision.GetComponent<IngameCharacterMover>();
        if (character != null && character.isOwned)
        {
            spriteRenderer.material.SetFloat("_Highlighted", 1f);
            IngameUIManager.Instance.UnSetUseButton();
        }
    }

    public void OnClickUse()
    {
        IngameUIManager.Instance.FixWiringTaskUI.Open();
    }
}
