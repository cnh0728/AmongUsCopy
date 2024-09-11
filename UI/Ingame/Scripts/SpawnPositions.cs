using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PosScale
{
    public Vector3 pos;
    public Vector3 scale;
}

public class SpawnPositions : MonoBehaviour
{
    [SerializeField]
    private List<Transform> spawnPositions;

    private int index;

    public PosScale GetSpawnInfo()
    {
        PosScale ret;

        ret.pos = spawnPositions[index].position;
        ret.scale = spawnPositions[index].localScale;

        index = (index + 1) %spawnPositions.Count;

        return ret;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
