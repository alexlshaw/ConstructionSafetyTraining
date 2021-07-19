using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiteNavScript : MonoBehaviour
{
    private List<Vector3> locations = new List<Vector3>();
    public bool drawDebugLocations = true;
    private Vector3 targetLocation;
    private GameObject targetIndicator;

    public void getRandomLocations(List<Generator> generators, List<GameObject> allBorders)
    {
        foreach (Generator generator in generators)
        {
            List<List<bool>> boolArray = generator.bool_array;
            for (int outer = 0; outer < boolArray.Count; outer++)
            {
                for (int inner = 0; inner < boolArray[outer].Count; inner++)
                {
                    if (boolArray[outer][inner] == false)
                    {
                        Vector3 x0z = new Vector3(inner, 0f, outer) + generator.transform.position;
                        bool tempBoolDistanceCheck = true;
                        foreach (GameObject border in allBorders)
                        {
                            if (Vector3.Distance(x0z, border.transform.position) < 4)
                            {
                                tempBoolDistanceCheck = false;
                            }
                        }
                        if (tempBoolDistanceCheck)
                        {
                            locations.Add(x0z);
                        }
                    }
                }
            }
        }
    }
    public void pickLocation()
    {
        int locationIndex = Random.Range(0,locations.Count);
        for (int i = 0; i < locations.Count; i++)
        {
            if (drawDebugLocations && i != locationIndex)
            {
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.position = locations[i];
                sphere.GetComponent<Collider>().enabled = false;
            }
        }
        targetLocation = locations[locationIndex];
        targetIndicator = Instantiate(Resources.Load("Prefabs/TargetLocation") as GameObject, targetLocation, Quaternion.identity);

    }
    void Update()
    {
        float dist = Vector3.Distance(Camera.main.transform.position, targetLocation);
        if (dist < 4)
        {
            Destroy(targetIndicator);
            pickLocation();
        }
    }
}
