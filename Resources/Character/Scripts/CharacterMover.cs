using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class CharacterMover : NetworkBehaviour
{
    [SyncVar]
    public float speed = 2f;

    [SyncVar(hook = nameof(SetNickname_Hook))]
    public string nickname;
    [SerializeField]
    protected TextMeshProUGUI nicknameText;
    public void SetNickname_Hook(string oldValue, string newValue)
    {
        nicknameText.text = newValue;
    }

    [SerializeField]
    private float characterSize = 0.5f;

    [SerializeField]
    private float cameraSize = 2.5f;

    protected Animator animator;

    private bool isMovable;

    public bool IsMovable
    {
        get
        {
            return isMovable;
        }
        set
        {
            isMovable = value;
        }
    }

    protected SpriteRenderer spriteRenderer;

    [SyncVar(hook = nameof(SetPlayerColor_Hook))]
    //syncVar로 등록된 변수가 서버에서 변경되면 hook에 걸린 함수가 실행됨
    public EPlayerColor playerColor;
    public void SetPlayerColor_Hook(EPlayerColor oldColor, EPlayerColor newColor)
    {
        if(spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(newColor));
    }

    public void StopMoveAnimation()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        animator.SetBool("isMove", false);

    }

    public virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(playerColor));

        if (isOwned)
        {
            Camera cam = Camera.main;
            cam.transform.SetParent(transform);
            cam.transform.localPosition = new Vector3(0f, 0f, -10f);
            cam.orthographicSize = cameraSize;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        if(isOwned && isMovable)
        {
            bool isMove = false;

            if (PlayerSettings.controlType == EControlType.Mouse)
            {
                if (Input.GetMouseButton(0)) {
                    Vector3 dir = (Input.mousePosition - new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f)).normalized;
                    if (dir.x < 0f)
                    {
                        transform.localScale = new Vector3(-characterSize, characterSize, 1f);
                    }
                    else if (dir.x > 0f)
                    {
                        transform.localScale = new Vector3(characterSize, characterSize, 1f);
                    }
                    transform.position += dir * speed * Time.deltaTime;

                    isMove = dir.magnitude != 0f;
                }
            }
            else
            {
                Vector3 dir = Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f), 1f);
                if (dir.x < 0f)
                {
                    transform.localScale = new Vector3(-characterSize, characterSize, 1f);
                }
                else if (dir.x > 0f)
                {
                    transform.localScale = new Vector3(characterSize, characterSize, 1f);
                }
                transform.position += dir * speed * Time.deltaTime;

                isMove = dir.magnitude != 0f;
            }

            animator.SetBool("isMove", isMove);
        }

        if(transform.localScale.x < 0f)
        {
            nicknameText.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if(transform.localScale.x > 0f)
        {
            nicknameText.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
