using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Generator : MonoBehaviour
{
    public int xSize;
    public int ySize;
    public List<GameObject> prefabsToSpawn = new List<GameObject>();
    public GameObject borderPrefab;
    public Quaternion borderRotationOffset = Quaternion.Euler(0, 90, 0);
    public float borderSpacing = 4.5f;
    public float minimumDistanceApart = 2.5f;
    private GameObject currentPrefab;
    public Material groundMaterial;
    private List<Vector3> vertices = new List<Vector3>();
    private List<Vector2> prefabLocations = new List<Vector2>();
    private List<List<bool>> bool_array = new List<List<bool>>();
    public List<GameObject> borderInstances = new List<GameObject>();

    public void startGeneration(int seed)
    {
        Random.InitState(seed);
        generateFilledBoolArray();
        generatePrefabs();
        generateBoundingGeometry();
        generateGroundPolygon();
        generateBorderPrefabs();
        transform.position = Vector3.zero;
    }

    void generateFilledBoolArray()
    {
        for (int y = 0; y < ySize; y++)
        {
            List<bool> tempList = new List<bool>();
            for (int x = 0; x < xSize; x++)
            {
                tempList.Add(false);
            }
            bool_array.Add(tempList);
        }
    }
    void generatePrefabs()
    {
        foreach (GameObject item in prefabsToSpawn){
            int h = Random.Range(0, ySize-1);
            int w = Random.Range(0, xSize-1);
            currentPrefab = item;
            if (bool_array[h][w] == false){
                bool_array[h][w] = true;
                int prefabX = currentPrefab.GetComponent<GeneratedItem>().xSize + 1;
                int prefabY = currentPrefab.GetComponent<GeneratedItem>().ySize + 1;
                for (int y = -prefabY + 1; y < prefabY; y++){
                    for (int x = -prefabX + 1; x < prefabX; x++){
                        if (h + y >= 0 && h + y < ySize && w + x >= 0 && w + x < xSize){
                            bool_array[h + y][w + x] = true;
                        }
                    }
                }
                Quaternion rotation = Quaternion.Euler(0f, Random.Range(0, 360-1), 0f);
                float new_w = w + xSize / 2 + transform.position.x;
                float new_h = h + ySize / 2 + transform.position.z;
                Instantiate(currentPrefab, new Vector3(new_w, 0.5f, new_h), rotation);
                prefabLocations.Add(new Vector2(new_w - prefabX, new_h + prefabY));
                prefabLocations.Add(new Vector2(new_w + prefabX, new_h - prefabY));
                prefabLocations.Add(new Vector2(new_w + prefabX, new_h + prefabY));
                prefabLocations.Add(new Vector2(new_w - prefabX, new_h - prefabY));
            }
        }
    }

    void generateBoundingGeometry()
    {
        Vector2[] points = prefabLocations.ToArray();
        HashSet<Vector2> result = new HashSet<Vector2>();
        List<Vector2> collinearPoints = new List<Vector2>();
        int leftMostIndex = 0;
        for (int i = 1; i < points.Length; i++)
        {
            if (points[leftMostIndex].x > points[i].x)
                leftMostIndex = i;
        }
        result.Add(points[leftMostIndex]);
        Vector2 current = points[leftMostIndex];
        while (true)
        {
            Vector2 nextTarget = points[0];
            for (int i = 1; i < points.Length; i++)
            {
                if (points[i] == current)
                    continue;
                float x1, x2, y1, y2;
                x1 = current.x - nextTarget.x;
                x2 = current.x - points[i].x;
                y1 = current.y - nextTarget.y;
                y2 = current.y - points[i].y;
                float val = (y2 * x1) - (y1 * x2);
                if (val > 0)
                {
                    nextTarget = points[i];
                    collinearPoints = new List<Vector2>();
                }
                else if (val == 0)
                {
                    if (Vector2.Distance(current, nextTarget) < Vector2.Distance(current, points[i]))
                    {
                        collinearPoints.Add(nextTarget);
                        nextTarget = points[i];
                    }
                    else
                        collinearPoints.Add(points[i]);
                }
            }
            foreach (Vector2 t in collinearPoints)
                result.Add(t);
            if (nextTarget == points[leftMostIndex])
                break;
            result.Add(nextTarget);
            current = nextTarget;
        }
        foreach (Vector2 item in result)
        {
            vertices.Add(new Vector3(item.x, 0f, item.y));
        }
    }
    void generateGroundPolygon()
    {
        Mesh msh = new Mesh();
        List<Vector2> vertices2d = new List<Vector2>();
        foreach (Vector3 item in vertices) { vertices2d.Add(new Vector2(item.x, item.z)); }
        TriangleScript tr = new TriangleScript(vertices2d.ToArray());
        msh.vertices = vertices.ToArray();
        msh.triangles = tr.Triangulate();
        msh.RecalculateNormals();
        msh.RecalculateBounds();
        MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();
        MeshFilter filter = gameObject.AddComponent<MeshFilter>();
        renderer.material = groundMaterial;
        filter.mesh = msh;
    }
    void generateBorderPrefabs()
    {
        vertices.Add(vertices[0]);
        for (int i = 0; i < vertices.Count - 1; i++)
        {
            Vector2 p1 = new Vector2(vertices[i].x, vertices[i].z);
            Vector2 p2 = new Vector2(vertices[i + 1].x, vertices[i + 1].z);
            float distance = Vector2.Distance(p1, p2);

            for (float x = 0; x < distance; x += borderSpacing)
            {
                float value = x / distance;
                Vector2 pos = Vector2.Lerp(p1, p2, value);
                Vector3 pos3d = new Vector3(pos.x, 0f, pos.y);
                Quaternion rotation = Quaternion.LookRotation((vertices[i + 1] - pos3d).normalized) * borderRotationOffset;
                bool toggle = true;
                foreach (GameObject item in borderInstances)
                {
                    if (Vector3.Distance(item.transform.position, pos3d) < minimumDistanceApart)
                    {
                        toggle = false;
                    }
                }
                if (toggle)
                {
                    GameObject border = Instantiate(borderPrefab, pos3d, rotation);
                    borderInstances.Add(border);
                }
            }
        }
    }
}
