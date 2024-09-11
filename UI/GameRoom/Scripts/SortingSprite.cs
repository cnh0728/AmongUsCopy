using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingSprite : MonoBehaviour
{
    public enum ESortingtype
    {
        Static, Updating
    }

    [SerializeField]
    private ESortingtype sortingType;

    private SpriteSorter sorter;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        sorter = FindObjectOfType<SpriteSorter>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sortingOrder = sorter.GetSortingOrder(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (sortingType == ESortingtype.Updating)
        {
            spriteRenderer.sortingOrder = sorter.GetSortingOrder(gameObject);
        }
    }
}
