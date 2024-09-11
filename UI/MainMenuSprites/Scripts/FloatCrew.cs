using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatCrew : MonoBehaviour
{
    public EPlayerColor playerColor;

    private SpriteRenderer spriteRenderer;
    private Vector3 direction;
    private float floatingSpeed;
    private float rotatingSpeed;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetFloatCrew(Sprite sprite, EPlayerColor playerColor, Vector3 direction, float floatingSpeed, float rotationSpeed, float size)
    {
        this.playerColor = playerColor;
        this.direction = direction;
        this.floatingSpeed = floatingSpeed;
        this.rotatingSpeed = rotationSpeed;

        spriteRenderer.sprite = sprite;
        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(playerColor));
        transform.localScale = new Vector3(size, size, size);
        spriteRenderer.sortingOrder = (int)Mathf.Lerp(1, 32767, size);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * floatingSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, 0f, rotatingSpeed));
    }
}
