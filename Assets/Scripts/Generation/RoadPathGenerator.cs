using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Serializable]
public class newGenTemplate
{
    public Vector2Int xMinMax;
    public Vector2Int yMinMax;
    public int targetItems;
    public List<dict> prefabsToSpawn = new List<dict>();
}
public class RoadPathGenerator : MonoBehaviour
{
    private List<Generator> generators = new List<Generator>();
    private List<GameObject> areas = new List<GameObject>();
    private List<GameObject> allBorders = new List<GameObject>();
    private List<Vector3> vertices;
    private GameObject spawned = null;
    
    [Header("Generation Seed Value")]
    public int seed;
    [Header("Individual Sections")]
    public List<newGenTemplate> listGen = new List<newGenTemplate>();
    public GameObject genPrefab;
    
    [Header("Overall Site")]
    public Material groundMaterial;
    public int generatorBorder = 5;
    public int maxXSites;
    public int maxYSites;
    public int numBordersRemoved;
    public GameObject wholeSiteBorder;
    private bool val;
    
    // Start is called before the first frame update
    void Start()
    {
        spawned = new GameObject("groundParent");
        UnityEngine.Random.InitState(seed);
        addAreas();
        startGeneration();
        moveAreas();
        generateBoundingGeometry();
        generateGroundPolygon();
        removeClosestBorder();
        generateBorderPrefabs();
        spawned.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
    }

    void addAreas()
    {
        for (int i = 0; i < listGen.Count; i++){
            GameObject area = Instantiate(genPrefab);
            areas.Add(area);
            generators.Add(area.GetComponent<Generator>());
        }
    }
    void startGeneration()
    {
        for (int i = 0; i < listGen.Count; i++){
            generators[i].xSize = UnityEngine.Random.Range(listGen[i].xMinMax.x, listGen[i].xMinMax.y);
            generators[i].ySize = UnityEngine.Random.Range(listGen[i].yMinMax.x, listGen[i].yMinMax.y);
            generators[i].targetItems = listGen[i].targetItems;
            generators[i].prefabsToSpawn = listGen[i].prefabsToSpawn;
            generators[i].startGeneration(seed + i);
            foreach (GameObject border in generators[i].borderInstances) { allBorders.Add(border); }
        }
    }
    private void moveAreas()
    {
        float locationX = 0;
        float locationY = 0;
        int xinc = 0;
        int yinc = 0;
        for (int i = 0; i < areas.Count; i++)
        {
            areas[i].transform.position = new Vector3(locationX, 0, locationY);
            val = (UnityEngine.Random.value < 0.5);
            int x = generators[i].xSize;
            int y = generators[i].ySize;
            if (val && xinc < maxXSites) { 
                locationX += x + generatorBorder; 
                xinc += 1; 
            }
            if (!val && yinc < maxYSites) { 
                locationY += y + generatorBorder; 
                yinc += 1; 
            }
            else if (xinc == maxXSites) { 
                locationY += (y/2) + generatorBorder;
                locationX -= x + generatorBorder; 
                xinc = 0; 
            }
            else if (yinc == maxYSites) { 
                locationX += (x/2) + generatorBorder; 
                locationY -= y + generatorBorder; 
                yinc = 0; 
            }
        }
    }
    void generateBoundingGeometry()
    {
        List<Vector3> prefabLocations = (from item in allBorders select item.transform.position).ToList();
        vertices = ConvexHull.compute(prefabLocations);
    }
    void generateGroundPolygon()
    {
        GameObject ground = new GameObject("groundMesh");
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
        ground.transform.position = ground.transform.position + new Vector3(0, -0.01f, 0);
        
        Vector3 center = renderer.bounds.center;
        spawned.transform.position = center;
        ground.transform.parent = spawned.transform;
    }
    void removeClosestBorder()
    {
        for (int i = 0; i < generators.Count - 1; i++)
        {
            float distance = Mathf.Infinity;
            Generator start = generators[i];
            Generator end = generators[i + 1];
            GameObject border1 = null;
            GameObject border2 = null;
            int sindex = 0;
            int eindex = 0;
            bool addedBox = false;

            for (int s = 1; s < start.borderInstances.Count; s++)
            {
                for (int e = 1; e < end.borderInstances.Count; e++)
                {
                    GameObject sborder = start.borderInstances[s];
                    GameObject eborder = end.borderInstances[e];
                    float tempDist = Vector3.Distance(sborder.transform.position, eborder.transform.position);
                    if (tempDist <= distance)
                    {
                        distance = tempDist;
                        border1 = sborder;
                        border2 = eborder;
                        sindex = s;
                        eindex = e;
                    }
                }
            }
            Destroy(border1);
            Destroy(border2);
            for (int x = 0; x < numBordersRemoved; x++)
            {
                GameObject starget = start.borderInstances[sindex - x];
                if (!addedBox) { addedBox = true; Instantiate(start.box, starget.transform.position, starget.transform.rotation); }
                if (starget) { Destroy(starget); }
                GameObject etarget = end.borderInstances[eindex - x];
                if (etarget) { Destroy(etarget); }
            }
        }
    }
    void generateBorderPrefabs()
    {
        List<GameObject> borderInstances = new List<GameObject>();
        vertices.Add(vertices[0]);
        for (int i = 0; i < vertices.Count - 1; i++)
        {
            Vector2 p1 = new Vector2(vertices[i].x, vertices[i].z);
            Vector2 p2 = new Vector2(vertices[i + 1].x, vertices[i + 1].z);
            float distance = Vector2.Distance(p1, p2);

            for (float x = 0; x < distance; x += 7.5f)
            {
                float value = x / distance;
                Vector2 pos = Vector2.Lerp(p1, p2, value);
                Vector3 pos3d = new Vector3(pos.x, 0f, pos.y);
                Quaternion rotation = Quaternion.LookRotation((vertices[i + 1] - pos3d).normalized) * Quaternion.Euler(0, 90, 0);
                bool toggle = true;
                foreach (GameObject item in borderInstances)
                {
                    if (Vector3.Distance(item.transform.position, pos3d) < 4f)
                    {
                        toggle = false;
                    }
                }
                if (toggle)
                {
                    GameObject border = Instantiate(wholeSiteBorder, pos3d, rotation);
                    border.transform.parent = spawned.transform;
                    borderInstances.Add(border);
                }
            }
        }
    }
}
