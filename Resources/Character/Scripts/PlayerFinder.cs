using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFinder : MonoBehaviour
{
    private CircleCollider2D circleColldier;

    private List<IngameCharacterMover> targets = new List<IngameCharacterMover>();
    public List<IngameCharacterMover> Targets { get { return targets; } }

    private void Awake()
    {
        circleColldier = GetComponent<CircleCollider2D>();
    }

    public void SetKillRange(float range)
    {
        circleColldier.radius = range;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<IngameCharacterMover>();
        if (player && AmongUsRoomPlayer.MyRoomPlayer.myCharacter != player)
        {
            if (!targets.Contains(player))
            {
                targets.Add(player);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.GetComponent<IngameCharacterMover>();
        if (player && AmongUsRoomPlayer.MyRoomPlayer.myCharacter != player)
        {
            if (targets.Contains(player))
            {
                targets.Remove(player);
            }
        }
    }

    public IngameCharacterMover GetFirstTarget()
    {
        float closeDist = float.MaxValue;
        IngameCharacterMover closeTarget = null;
        foreach (var target in targets)
        {
            float dist = Vector3.Distance(transform.position, target.transform.position);
            if (dist < closeDist)
            {
                closeDist = dist;
                closeTarget = target;
            }
        }

        targets.Remove(closeTarget);
        return closeTarget;
    }
}
