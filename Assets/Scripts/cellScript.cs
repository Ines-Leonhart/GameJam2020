using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cellScript : MonoBehaviour
{
    Game Game
    {
        get
        {
            return Singleton.Get<Game>();
        }
    }

    public GameObject plantPrefab;

    public Plant Plant { get; set; }
    public bool seeded { get; set; }
    public (float, float) spawnTimeRange;

    private float period;
    private float spawnTimestamp;

    private void Start()
    {
        seeded = false;
        period = Random.Range(spawnTimeRange.Item1, spawnTimeRange.Item2);
        spawnTimestamp = Time.realtimeSinceStartup;
    }

    private void Update()
    {
        if (Game.gameStarted)
        {
            if(Plant != null)
            {
                seeded = true;
            }

            if (!seeded)
            {
                if(Time.realtimeSinceStartup - spawnTimestamp >= period)
                {
                    var plantObject = Instantiate(plantPrefab);
                    plantObject.transform.position = transform.position;
                    Plant = plantObject.GetComponent<Plant>();
                }
            }
        }
    }
}
