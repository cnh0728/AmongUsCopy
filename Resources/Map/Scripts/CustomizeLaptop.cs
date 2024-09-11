using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizeLaptop : MonoBehaviour
{
    [SerializeField]
    private Sprite useButtonSprite;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();    
        var matInst = Instantiate(spriteRenderer.material);
        spriteRenderer.material = matInst;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var character = collision.GetComponent<CharacterMover>();
        if (character != null && character.isOwned)
        {
            spriteRenderer.material.SetFloat("_Highlighted", 0f);
            LobbyUIManger.Instance.SetUseButton(useButtonSprite, OnClickUse);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var character = collision.GetComponent<CharacterMover>();
        if (character != null && character.isOwned)
        {
            spriteRenderer.material.SetFloat("_Highlighted", 1f);
            LobbyUIManger.Instance.UnSetUseButton();
        }
    }

    public void OnClickUse()
    {
        LobbyUIManger.Instance.CustomizeUI.Open();
    }
}
