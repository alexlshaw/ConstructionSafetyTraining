using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class dict
{
    public GameObject item;
    public float chance;
}
[Serializable]
public class Generator : MonoBehaviour
{
    public int xSize;
    public int ySize;
    public int targetItems;
    public int craneMax;
    public List<dict> prefabsToSpawn = new List<dict>();
    public List<GameObject> borderPrefabs;
    public GameObject box;
    public Quaternion borderRotationOffset = Quaternion.Euler(0, 90, 0);
    private GameObject currentPrefab;
    private GameObject ground;
    public Material groundMaterial;
    private List<Vector3> vertices = new List<Vector3>();
    private List<Vector3> prefabLocations = new List<Vector3>();
    public List<List<bool>> bool_array = new List<List<bool>>();
    public List<GameObject> borderInstances = new List<GameObject>();

    public void startGeneration(int seed)
    {
        UnityEngine.Random.InitState(seed);
        generateFilledBoolArray();
        generatePrefabs();
        generateBoundingGeometry();
        generateGroundPolygon();
        generateBorderPrefabs();
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
        for (int i = 0; i < targetItems; i++)
        {
            foreach (dict input_item in prefabsToSpawn)
            {
                float val = UnityEngine.Random.value;
                if (val < input_item.chance)
                {
                    GameObject item = input_item.item;
                    int h = UnityEngine.Random.Range(0, ySize - 1);
                    int w = UnityEngine.Random.Range(0, xSize - 1);
                    currentPrefab = item;
                    int prefabX = currentPrefab.GetComponent<GeneratedItem>().xSize + 1;
                    int prefabY = currentPrefab.GetComponent<GeneratedItem>().ySize + 1;
                    bool areaIsInUse = false;
                    for (int y = -prefabY; y < prefabY; y++)
                    {
                        for (int x = -prefabX; x < prefabX; x++)
                        {
                            if (h + y >= 0 && h + y < ySize && w + x >= 0 && w + x < xSize)
                            {
                                if (bool_array[h + y][w + x] == true)
                                {
                                    areaIsInUse = true;
                                }
                            }
                        }
                    }
                    if (areaIsInUse == false)
                    {
                        for (int y = -prefabY; y < prefabY; y++)
                        {
                            for (int x = -prefabX; x < prefabX; x++)
                            {
                                if (h + y >= 0 && h + y < ySize && w + x >= 0 && w + x < xSize)
                                {
                                    bool_array[h + y][w + x] = true;
                                }
                            }
                        }
                        Quaternion rotation = (currentPrefab.GetComponent<GeneratedItem>().randomlyRotate) ? Quaternion.Euler(0f, UnityEngine.Random.Range(0, 360 - 1), 0f) : Quaternion.identity;
                        float new_w = w + gameObject.transform.position.x;
                        float new_h = h + gameObject.transform.position.z;

                        if (currentPrefab.tag == "Crane")
                        {
                            if (craneCheck())
                            {
                                GameObject prefab = Instantiate(currentPrefab, new Vector3(new_w, 0.5f, new_h), rotation);
                                prefab.transform.parent = gameObject.transform;
                                prefabLocations.Add(new Vector3(new_w - prefabX, 0, new_h + prefabY));
                                prefabLocations.Add(new Vector3(new_w + prefabX, 0, new_h - prefabY));
                                prefabLocations.Add(new Vector3(new_w + prefabX, 0, new_h + prefabY));
                                prefabLocations.Add(new Vector3(new_w - prefabX, 0, new_h - prefabY));
                            }
                        }
                        else
                        {
                            GameObject prefab = Instantiate(currentPrefab, new Vector3(new_w, 0.5f, new_h), rotation);
                            prefab.transform.parent = gameObject.transform;
                            prefabLocations.Add(new Vector3(new_w - prefabX, 0, new_h + prefabY));
                            prefabLocations.Add(new Vector3(new_w + prefabX, 0, new_h - prefabY));
                            prefabLocations.Add(new Vector3(new_w + prefabX, 0, new_h + prefabY));
                            prefabLocations.Add(new Vector3(new_w - prefabX, 0, new_h - prefabY));
                        }

                    }
                }
            }
        }
    }
    bool craneCheck()
    {
        GameObject[] craneSearch = GameObject.FindGameObjectsWithTag("Crane");
        return craneSearch.Length < craneMax;
    }
    void generateBoundingGeometry()
    {
        vertices = ConvexHull.compute(prefabLocations);
    }

    void generateGroundPolygon()
    {
        ground = new GameObject("groundMesh");
        ground.transform.parent = gameObject.transform;
        Mesh msh = ground.AddComponent<MeshFilter>().mesh;
        List<Vector2> vertices2d = new List<Vector2>();
        foreach (Vector3 item in vertices) { vertices2d.Add(new Vector2(item.x, item.z)); }
        TriangleScript tr = new TriangleScript(vertices2d.ToArray());
        msh.vertices = vertices.ToArray();
        msh.triangles = tr.Triangulate();
        msh.RecalculateNormals();
        msh.RecalculateBounds();
        MeshCollider meshCollider = ground.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = msh;
        MeshRenderer renderer = ground.AddComponent<MeshRenderer>();
        renderer.material = groundMaterial;
    }
    void generateBorderPrefabs()
    {
        int index = UnityEngine.Random.Range(0, borderPrefabs.Count);
        vertices.Add(vertices[0]); //Add first vertex to end to create loop
        for (int i = 0; i < vertices.Count-1; i++)
        {
            Vector3 p1 = vertices[i];
            Vector3 p2 = vertices[i + 1];
            float distance = Vector3.Distance(p1, p2);

            for (float x = 0; x < distance; x += 6.05f)
            {
                float percentageOfDistance = x / distance;
                if (percentageOfDistance < 1.0) //check that current spawn is actually on the edge (between the two vertices)
                {
                    Vector3 position = Vector3.Lerp(p1, p2, percentageOfDistance);

                    bool shouldSpawn = true;
                    foreach (GameObject item in borderInstances)
                    {
                        if (Vector3.Distance(item.transform.position, position) <= 3.025) //half the spacing between border prefabs is the minimum distance from the current to any border
                        {
                            shouldSpawn = false;
                        }
                    }
                    if (shouldSpawn)
                    {
                        GameObject border = Instantiate(borderPrefabs[index], position, Quaternion.identity);
                        border.transform.LookAt(p2);
                        border.transform.parent = gameObject.transform;
                        borderInstances.Add(border);
                    }
                }
            }
        }
    }

    public void generateTracks()
    {
        List<int> startArray = new List<int>();
        Vector3[] groundVertices = ground.GetComponent<MeshFilter>().mesh.vertices;
        for (int i = 0; i < groundVertices.Length; i++)
        {
            groundVertices[i] = ground.transform.TransformPoint(groundVertices[i]);
        }
        for (int i = 0; i < groundVertices.Length - 2; i += 3)
        {
            startArray.Add(i);
            Vector3[] loop_newPoints = new Vector3[] { groundVertices[i], groundVertices[i + 1], groundVertices[i + 2] };
            Vector3 loop_averagePoint = (loop_newPoints[0] + loop_newPoints[1] + loop_newPoints[2]) / 3;
            Vector3 loop_centerPoint = ground.transform.TransformPoint(ground.GetComponent<MeshFilter>().mesh.bounds.center);
            Vector3 loop_directionToMoveAway = Vector3.Normalize(loop_averagePoint - loop_centerPoint) * 10f;
            loop_newPoints[0] += loop_directionToMoveAway + new Vector3(0, 0.01f, 0);
            loop_newPoints[1] += loop_directionToMoveAway * 1.5f + new Vector3(0, 0.01f, 0);
            loop_newPoints[2] += loop_directionToMoveAway + new Vector3(0, 0.01f, 0);
            TrackScript loop_tr = Instantiate(Resources.Load("Prefabs/TrackRenderer") as GameObject).GetComponent<TrackScript>();
            loop_tr.points = loop_newPoints;
            loop_tr.vertexAmount = 12;
            loop_tr.create();
            loop_tr.transform.parent = transform;
        }

        int index = (UnityEngine.Random.Range(0, groundVertices.Length) % groundVertices.Length);
        if (startArray.Contains(index)){ index = (index + 1) % groundVertices.Length; }
        Vector3[] newPoints = new Vector3[] { groundVertices[index], groundVertices[index + 1], groundVertices[index + 2] };
        Vector3 averagePoint = (newPoints[0] + newPoints[1] + newPoints[2]) / 3;
        Vector3 centerPoint = ground.transform.TransformPoint(ground.GetComponent<MeshFilter>().mesh.bounds.center);
        Vector3 directionToMoveAway = Vector3.Normalize(averagePoint - centerPoint) * 10f;
        newPoints[0] += directionToMoveAway + new Vector3(0, 0.01f, 0);
        newPoints[1] += directionToMoveAway * 1.5f + new Vector3(0, 0.01f, 0);
        newPoints[2] += directionToMoveAway + new Vector3(0, 0.01f, 0);
        TrackScript tr = Instantiate(Resources.Load("Prefabs/TrackRenderer") as GameObject).GetComponent<TrackScript>();
        tr.points = newPoints;
        tr.vertexAmount = 12;
        tr.create();
        tr.transform.parent = transform;

    }
}
