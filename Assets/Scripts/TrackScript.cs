using System.Collections.Generic;
using UnityEngine;

public class TrackScript : MonoBehaviour
{
    public Vector3[] points = new Vector3[3];
    public int vertexAmount;

    public void create()
    {
        LineRenderer lr = GetComponentInChildren<LineRenderer>();
        List<Vector3> pointList = new List<Vector3>();
        for (float ratio = 0; ratio <= 1; ratio += 1.0f / vertexAmount)
        {
            Vector3 tangent1 = Vector3.Lerp(points[0], points[1], ratio);
            Vector3 tangent2 = Vector3.Lerp(points[1], points[2], ratio);
            Vector3 curve = Vector3.Lerp(tangent1, tangent2, ratio);
            pointList.Add(curve);
        }
        lr.positionCount = pointList.Count;
        lr.SetPositions(pointList.ToArray());
    }
}
