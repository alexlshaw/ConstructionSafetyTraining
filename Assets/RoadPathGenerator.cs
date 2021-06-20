using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPathGenerator : MonoBehaviour
{
    public List<Generator> areas;
    private List<Vector3> points = new List<Vector3>();
    public int seed;
    private LineRenderer lr;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        Random.InitState(seed);
        for (int i = 0; i < areas.Count; i++)
        {
            areas[i].startGeneration(seed + i);
            int index = Random.Range(0, areas[i].borderInstances.Count);
            points.Add(areas[i].borderInstances[index].transform.position);
        }
        lr.positionCount = points.Count;
        lr.SetPositions(points.ToArray());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
