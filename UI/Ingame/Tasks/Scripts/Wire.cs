using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wire : MonoBehaviour
{
    public EWireColor WireColor { get; private set; }

    public Wire ConnectedWire { get; protected set; }

    public bool IsConnected { get { return ConnectedWire != null; } }

    public bool IsCorrectConnected { get; protected set; }

    private RectTransform wireBody;

    [SerializeField]
    protected Image lightImage;

    [SerializeField]
    private List<Image> wireImages;

    [SerializeField]
    private Image copperImage;

    private void Start()
    {
        wireBody = GetComponent<RectTransform>();
    }

    public void SetWireColor(EWireColor wireColor)
    {
        WireColor = wireColor;
        Color color = Color.black;
        switch (WireColor)
        {
            case EWireColor.Red:
                color = Color.red;
                break;
            case EWireColor.Blue:
                color = Color.blue;
                break;
            case EWireColor.Yellow:
                color = Color.yellow;
                break;
            case EWireColor.Magenta:
                color = Color.magenta;
                break;
        }

        foreach (var image in wireImages)
        {
            image.color = color;
        }
    }

    private void CheckCorrectWire(Wire wire)
    {
        if (ConnectedWire.WireColor == WireColor)
        {
            IsCorrectConnected = true;
            lightImage.color = Color.yellow;
        }
        else
        {
            IsCorrectConnected = false;
            lightImage.color = Color.gray;
        }
    }

    public virtual void ConnectWire(Wire wire)
    {
        if(ConnectedWire != null && ConnectedWire != wire)
        {
            ConnectedWire.DisconnectWire();
        }

        if(ConnectedWire == null)
        {
            copperImage.enabled = false;
            ConnectedWire = wire;
        }

        CheckCorrectWire(wire);
    }

    public virtual void DisconnectWire()
    {
        if(ConnectedWire != null)
        {
            ConnectedWire = null;
            lightImage.color = Color.gray;
            IsCorrectConnected = false;

            copperImage.enabled = true;

            ResetTargetPosition();
        }
    }

    public void SetTargetPosition(Vector3 targetPosition, float offset, Canvas gameCanvas)
    {
        float angle = Vector2.SignedAngle(transform.position + Vector3.right
            - transform.position, targetPosition - transform.position);
        float distance = Vector2.Distance(wireBody.transform.position, targetPosition) - offset;
        wireBody.localRotation = Quaternion.Euler(0f, 0f, angle);
        wireBody.sizeDelta = new Vector2(distance * (1 / gameCanvas.transform.localScale.x), wireBody.sizeDelta.y);
    }

    public void ResetTargetPosition()
    {
        wireBody.localRotation = Quaternion.Euler(Vector3.zero);
        wireBody.sizeDelta = new Vector2(0f, wireBody.sizeDelta.y);
    }

}
