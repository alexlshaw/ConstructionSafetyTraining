using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaffoldingCreator : MonoBehaviour
{
    public int dungeonWidth, dungeonLength;
    public GameObject scaffoldCorner;
    public GameObject scaffoldDefault;
    public GameObject scaffoldStairs;
    public GameObject cornerNoBarriers;
    public GameObject defaultNoFloor;

    public int scale;
    public int totalFloors;

    [Range(0,1)]
    public float stairsProbability;


    bool[,] scaffoldPositions;


    // Start is called before the first frame update
    void Start()
    {
        GameObject scaffoldingObject = new GameObject("Scaffold");
        for(int floor = 0; floor < totalFloors; floor++)
        {
            GameObject scaffoldFloor = new GameObject("scaffoldFloor" + floor);
            createScaffoldPositions(scaffoldFloor, floor);

            scaffoldFloor.transform.position += new Vector3(0, floor * scale, 0);
            scaffoldFloor.transform.SetParent(scaffoldingObject.transform);
        }

        GameObject scaffoldParent = new GameObject("scaffoldParent");

        scaffoldingObject.transform.parent = scaffoldParent.transform;

        scaffoldingObject.transform.localPosition = new Vector3((float)-(dungeonWidth + 1) / 2 * 3, scaffoldingObject.transform.localPosition.y, (float)-(dungeonLength + 1) / 2 * 3);
        scaffoldParent.transform.parent = transform;
        scaffoldParent.transform.localScale = new Vector3(1f, 1f, 1f);
        scaffoldParent.transform.localPosition = Vector3.zero;
    }

    private void createScaffoldPositions(GameObject parent, int floor)
    {
        scaffoldPositions = new bool[dungeonWidth + 2, dungeonLength + 2];
        for (int x = 0; x < dungeonWidth + 2; x++)
        {
            for (int z = 0; z < dungeonLength + 2; z++)
            {
                if (x > 0 && x < dungeonWidth + 1 && z > 0 && z < dungeonLength + 1)
                {
                    continue;
                }
                float stairsValue = Random.Range(0f, 1f);
                GameObject defaultType;
                if(floor == 0)
                {
                    defaultType = stairsValue < stairsProbability ? scaffoldStairs : scaffoldDefault;
                }
                else
                {
                    defaultType = stairsValue < stairsProbability ? scaffoldStairs : defaultNoFloor;
                }
                GameObject cornerType = floor==0 ? cornerNoBarriers : scaffoldCorner;
                if (x == 0)
                {
                    if (z == 0)
                    {
                        GameObject.Instantiate(cornerType, new Vector3(x * scale + 0.5f, 0, z * scale+0.5f), Quaternion.Euler(new Vector3(0, -90, 0))).transform.SetParent(parent.transform);
                    }
                    else if(z== dungeonLength+1)
                    {
                        GameObject.Instantiate(cornerType, new Vector3(x * scale + 0.5f, 0, z * scale - 0.5f), Quaternion.identity).transform.SetParent(parent.transform);
                    }
                    else
                    {
                        GameObject.Instantiate(defaultType, new Vector3(x * scale + 0.5f, 0, z * scale), Quaternion.Euler(new Vector3(0, -90, 0))).transform.SetParent(parent.transform);
                    }
                }
                else if (x == dungeonWidth+1)
                {
                    if (z == 0)
                    {
                        GameObject.Instantiate(cornerType, new Vector3(x * scale - 0.5f, 0, z * scale + 0.5f), Quaternion.Euler(new Vector3(0, 180, 0))).transform.SetParent(parent.transform);
                    }
                    else if (z == dungeonLength + 1)
                    {
                        GameObject.Instantiate(cornerType, new Vector3(x * scale - 0.5f, 0, z * scale - 0.5f), Quaternion.Euler(new Vector3(0, -90, 0))).transform.SetParent(parent.transform);
                    }
                    else
                    {
                        GameObject.Instantiate(defaultType, new Vector3(x * scale - 0.5f, 0, z * scale), Quaternion.Euler(new Vector3(0, 90, 0))).transform.SetParent(parent.transform);
                    }
                }
                else if (z==0)
                {
                     GameObject.Instantiate(defaultType, new Vector3(x * scale, 0, z * scale+0.5f), Quaternion.Euler(new Vector3(0, 180, 0))).transform.SetParent(parent.transform);
                }
                else if (z == dungeonLength + 1)
                {
                    GameObject.Instantiate(defaultType, new Vector3(x * scale, 0, z * scale - 0.5f), Quaternion.Euler(new Vector3(0, 0, 0))).transform.SetParent(parent.transform);
                }
                scaffoldPositions[x, z] = true;
            }
        }


    }
}
