using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DropObjects : MonoBehaviour
{
    public float maxTime = 20;
    public float minTime = 10;
    public float distanceToBuilding = 50;
    public float currentDistance;
    public List<GameObject> itemsToSpawn;

    private float spawnTime;
    private float timeElapsed;

    private GameObject building;
    // Start is called before the first frame update
    void Start()
    {
        building = GameObject.Find("BuildingCreator(Clone)");

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(building == null)
        {
            building = GameObject.Find("BuildingCreator(Clone)");

        }
        currentDistance = Vector3.Distance(building.transform.position, transform.position);
        if (currentDistance < distanceToBuilding)
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= spawnTime)
            {
                int index = Random.Range(0, itemsToSpawn.Count);
                Instantiate(itemsToSpawn[index], transform.position, itemsToSpawn[index].transform.rotation);
                if (!Globals.hasHat)
                {
                    GameEvents.current.TakeDamage();
                    PointsLostHandler.other += 1;
                }
                spawnTime = Random.Range(minTime, maxTime);
                timeElapsed = 0;
            }
        }
        
    }
}
