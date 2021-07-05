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
    public List<dict> prefabsToSpawn = new List<dict>();
    public List<GameObject> borderPrefabs;
    public GameObject box;
    public Quaternion borderRotationOffset = Quaternion.Euler(0, 90, 0);
    public float borderSpacing = 4.5f;
    public float minimumDistanceApart = 2.5f;
    private GameObject currentPrefab;
    public Material groundMaterial;
    private List<Vector3> vertices = new List<Vector3>();
    private List<Vector3> prefabLocations = new List<Vector3>();
    private List<List<bool>> bool_array = new List<List<bool>>();
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
        for (int i = 0; i < targetItems; i++){
            foreach (dict input_item in prefabsToSpawn){
                float val = UnityEngine.Random.value;
                if (val < input_item.chance){
                    GameObject item = input_item.item;
                    int h = UnityEngine.Random.Range(0, ySize - 1);
                    int w = UnityEngine.Random.Range(0, xSize - 1);
                    currentPrefab = item;
                    int prefabX = currentPrefab.GetComponent<GeneratedItem>().xSize + 1;
                    int prefabY = currentPrefab.GetComponent<GeneratedItem>().ySize + 1;
                    bool areaIsInUse = false;
                    for (int y = -prefabY; y < prefabY; y++){
                        for (int x = -prefabX; x < prefabX; x++){
                            if (h + y >= 0 && h + y < ySize && w + x >= 0 && w + x < xSize){
                                if (bool_array[h + y][w + x] == true){
                                    areaIsInUse = true;
                                }
                            }
                        }
                    }
                    if (areaIsInUse == false){
                        for (int y = -prefabY; y < prefabY; y++){
                            for (int x = -prefabX; x < prefabX; x++){
                                if (h + y >= 0 && h + y < ySize && w + x >= 0 && w + x < xSize){
                                    bool_array[h + y][w + x] = true;
                                }
                            }
                        }
                        Quaternion rotation = (currentPrefab.GetComponent<GeneratedItem>().randomlyRotate) ? Quaternion.Euler(0f, UnityEngine.Random.Range(0, 360 - 1), 0f) : Quaternion.identity;
                        float new_w = w + gameObject.transform.position.x;
                        float new_h = h + gameObject.transform.position.z;
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
    void generateBoundingGeometry()
    {
        vertices = ConvexHull.compute(prefabLocations);
    }

    void generateGroundPolygon()
    {
        GameObject ground = new GameObject("groundMesh");
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
                foreach (GameObject item in borderInstances){
                    if (Vector3.Distance(item.transform.position, pos3d) < minimumDistanceApart){
                        toggle = false;
                    }
                }
                if (toggle)
                { 
                    GameObject border = Instantiate(borderPrefabs[index], pos3d, rotation);
                    border.transform.parent = gameObject.transform;
                    borderInstances.Add(border);
                }
            }
        }
    }
}
