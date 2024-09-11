using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EWireColor
{
    None = -1,
    Red,
    Blue,
    Yellow,
    Magenta,
}

public class FixWiringTaskUIManager : MonoBehaviour
{

    [SerializeField]
    private float leftOffset = 15f;

    [SerializeField]
    private float rightOffset = 40f;

    [SerializeField]
    private int wireAmount = 4;

    [SerializeField]
    private List<LeftWire> leftWires;

    [SerializeField]
    private List<RightWire> rightWires;

    private LeftWire selectedWire;

    private Canvas gameCanvas;

    // Start is called before the first frame update
    void Start()
    {
        var canvases = FindObjectsOfType<Canvas>();
        foreach(Canvas c in canvases)
        {
            if (c.CompareTag("Canvas"))
            {
                gameCanvas = c;
                break;
            }
        }
    }

    private void OnEnable()
    {
        for(int i=0; i<leftWires.Count; i++)
        {
            DisConnectWire(leftWires[i]);
        }

        List<int> numberPool = new List<int>();
        for(int i = 0; i < wireAmount; i++)
        {
            numberPool.Add(i);
        }

        int index = 0;

        while(numberPool.Count != 0)
        {
            var number = numberPool[Random.Range(0, numberPool.Count)];
            leftWires[index++].SetWireColor((EWireColor)number);
            numberPool.Remove(number);
        }

        for (int i = 0; i < wireAmount; i++)
        {
            numberPool.Add(i);
        }

        index = 0;

        while (numberPool.Count != 0)
        {
            var number = numberPool[Random.Range(0, numberPool.Count)];
            rightWires[index++].SetWireColor((EWireColor)number);
            numberPool.Remove(number);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(Input.mousePosition, Vector2.right, 1f);
            foreach (var hit in hits)
            {
                if (hit.collider != null && hit.collider.CompareTag("TaskWire"))
                {
                    var left = hit.collider.GetComponentInParent<LeftWire>();
                    if (left != null)
                    {
                        selectedWire = left;

                        DisConnectWire(selectedWire);
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (selectedWire != null)
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(Input.mousePosition, Vector2.right, 1f);
                bool isConnectWire = false;
                foreach (var hit in hits)
                {
                    if (hit.collider != null && hit.collider.CompareTag("TaskWire"))
                    {
                        var right = hit.collider.GetComponentInParent<RightWire>();
                        if (right != null)
                        {
                            selectedWire.SetTargetPosition(right.GetComponentInParent<Transform>().position, rightOffset, gameCanvas);

                            if (right.IsConnected)
                            {
                                right.ConnectedWire.DisconnectWire();
                            }

                            ConnectWire(right, selectedWire);

                            isConnectWire = true;
                        }
                    }
                }

                if (isConnectWire)
                {
                    CheckCompleteTask();
                }
                else
                {
                    selectedWire.ResetTargetPosition();
                }

                selectedWire = null;
            }
        }

        if (selectedWire != null)
        {
            selectedWire.SetTargetPosition(Input.mousePosition, leftOffset, gameCanvas);
        }
    }

    private void DisConnectWire(Wire wire)
    {
        if (wire.IsConnected)
        {
            wire.ConnectedWire.DisconnectWire();
        }

        wire.DisconnectWire();
    }

    private void ConnectWire(Wire firstWire, Wire secondWire)
    {
        secondWire.ConnectWire(firstWire);
        firstWire.ConnectWire(secondWire);
    }

    private void CheckCompleteTask()
    {
        bool isAllComplete = true;
        foreach (var wire in leftWires)
        {
            if (!wire.IsCorrectConnected)
            {
                isAllComplete = false;
                break;
            }
        }

        if (isAllComplete)
        {
            Close();
        }
    }
    public void Open()
    {
        AmongUsRoomPlayer.MyRoomPlayer.myCharacter.IsMovable = false;
        transform.parent.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    public void Close()
    {
        AmongUsRoomPlayer.MyRoomPlayer.myCharacter.IsMovable = true;
        transform.parent.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
