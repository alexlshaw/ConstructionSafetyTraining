using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class InitialiseBuilding : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Globals.calculateBuildingSize();
        GameObject building = Resources.Load<GameObject>("Prefabs/" + "BuildingCreator");
        Instantiate(building);
        //StartCoroutine(buildNavMesh());

    }

    IEnumerator buildNavMesh()
    {
        yield return null;
        GameObject navMesh = GameObject.Find("navmeshHumans");

        NavMeshSurface nms = navMesh.GetComponent<NavMeshSurface>();
        //nms.agentTypeID = -1372625422;

        nms.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
