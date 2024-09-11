using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewFloater : MonoBehaviour
{
    [SerializeField]
    private GameObject crewPrefab;
    [SerializeField]
    private List<Sprite> crewSprites;

    private readonly static int crewColorCount = 12;

    private bool[] crewState = new bool[crewColorCount];
    private float timer = 0.5f;
    private float distance = 9f;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < crewColorCount; i++)
        {
            SpawnFloatingCrew((EPlayerColor)i, Random.Range(0f, distance));
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if(timer<= 0f)
        {
            SpawnFloatingCrew((EPlayerColor)Random.Range(0, crewColorCount), distance);
            timer = 1f;
        }
    }

    public void SpawnFloatingCrew(EPlayerColor playerColor, float dist)
    {
        if (!crewState[(int)playerColor])
        {
            crewState[(int)playerColor] = true;

            float angle = Random.Range(0f, 360f);
            Vector3 spawnPos = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0f) * dist;
            Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
            float floatingSpeed = Random.Range(1f, 4f);
            float rotationgSpeed = Random.Range(-1f, 1f);

            var crew = Instantiate(crewPrefab, spawnPos, Quaternion.identity).GetComponent<FloatCrew>();
            crew.SetFloatCrew(crewSprites[Random.Range(0, crewSprites.Count)], playerColor, direction, floatingSpeed, 
                rotationgSpeed, Random.Range(0.5f, 1f));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var crew = collision.GetComponent<FloatCrew>();
        if(crew != null)
        {
            crewState[(int)crew.playerColor] = false;
            Destroy(crew.gameObject);
        }
    }
}
