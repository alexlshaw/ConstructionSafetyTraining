using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using SharpConfig;
using System.IO;
using UnityEngine.AI;

public class RoadPathGenerator : MonoBehaviour
{
    private List<Generator> generators = new List<Generator>();
    private List<GameObject> areas = new List<GameObject>();
    private List<GameObject> allBorders = new List<GameObject>();
    private List<Vector3> vertices;
    private GameObject spawned = null;
    private MeshFilter mf;
    
    [Header("Generation Seed Value")]
    public int seed;                    //cfg
    [Header("VR Settings")]
    public bool VR_Enabled = false;
    
    [Header("Overall Site")]
    public Material groundMaterial;
    public int generatorBorder;         //cfg
    public int maxXSites;               //cfg
    public int maxYSites;               //cfg
    public int numBordersRemoved;       //cfg
    public GameObject wholeSiteBorder;
    public GameObject trainTracks;

    private Configuration cfg;
    private string filepath;
    private SiteNavScript nav;
    private bool drawDebugSpheres;
    private NavMeshSurface nms;

    // Start is called before the first frame update
    void Start()
    {
        filepath = Application.streamingAssetsPath + "/config.cfg";
        if (!File.Exists(filepath)){
            Debug.Log("Config not found! Making new config...");
            makeConfig();
            saveConfig();
        }
        loadConfig();
        spawned = new GameObject("groundParent");
        UnityEngine.Random.InitState(seed);
        moveAreas();
        generateBoundingGeometry();
        generateGroundPolygon();
        removeClosestBorder();
        generateBorderPrefabs();
        spawned.transform.localScale *= 1.25f;
        generateTrainTracks();
        addPlayer();
        nav = gameObject.GetComponent<SiteNavScript>();
        //Debug.Log((nav != null).ToString()+(generators != null).ToString()+(allBorders != null).ToString());
        nav.drawDebugLocations = drawDebugSpheres;
        nav.getRandomLocations(generators, allBorders);
        nav.pickLocation();
        nms.BuildNavMesh();
    }
    void makeConfig()
    {
        cfg = new Configuration();
        cfg["Generator"]["seed"].IntValue = 0;
        cfg["Generator"]["generatorBorder"].IntValue = 10;
        cfg["Generator"]["maxXSites"].IntValue = 3;
        cfg["Generator"]["NumBordersRemoved"].IntValue = 2;
        cfg["Generator"]["CraneAmount"].IntValue = 1;
        cfg["Section"]["sectionXSizeMinMax"].IntValueArray = new int[] { 30, 40 };
        cfg["Section"]["sectionYSizeMinMax"].IntValueArray = new int[] { 30, 40 };
        cfg["Section"]["targetItemAmount"].IntValue = 5;
        cfg["Section"]["p1"].StringValueArray = new string[] { "Crane", "1" };
        cfg["VRSettings"]["Enabled"].BoolValue = false;
        cfg["NavSettings"]["debugPositionsEnabled"].BoolValue = false;
    }
    void saveConfig()
    {
        cfg.SaveToFile(filepath);
    }
    void loadConfig()
    {
        cfg = Configuration.LoadFromFile(filepath);

        var section = cfg["Generator"];
        seed = section["seed"].IntValue;
        generatorBorder = section["generatorBorder"].IntValue;
        maxXSites = section["maxXSites"].IntValue;
        numBordersRemoved = section["numBordersRemoved"].IntValue;
        int craneAmount = section["CraneAmount"].IntValue;
        int i = 0;
        foreach (var sectionOverall in cfg)
        {
            if (sectionOverall.Name.Substring(0,7).ToLower() == "section")
            {
                GameObject area = Instantiate(Resources.Load("Prefabs/GeneratorPrefab") as GameObject);
                areas.Add(area);
                generators.Add(area.GetComponent<Generator>());

                int[] x = sectionOverall["sectionXSizeMinMax"].IntValueArray;
                int[] y = sectionOverall["sectionYSizeMinMax"].IntValueArray;
                Vector2Int xMinMax = new Vector2Int(x[0], x[1]);
                Vector2Int yMinMax = new Vector2Int(y[0], y[1]);
                int targetAmount = sectionOverall["targetItemAmount"].IntValue;
                List<dict> prefabsToSpawn = new List<dict>();
                foreach (var setting in sectionOverall)
                {
                    if (setting.Name.ToLower() == "prefab")
                    {
                        string[] tempArray = setting.StringValueArray;
                        string prefabName = tempArray[0];
                        float chance = float.Parse(tempArray[1]);
                        dict tempVal = new dict();
                        tempVal.item = Resources.Load<GameObject>("Prefabs/" + prefabName);
                        tempVal.chance = chance;
                        prefabsToSpawn.Add(tempVal);
                    }
                }
                generators[i].xSize = UnityEngine.Random.Range(xMinMax.x, xMinMax.y);
                generators[i].ySize = UnityEngine.Random.Range(yMinMax.x, yMinMax.y);
                generators[i].targetItems = targetAmount;
                generators[i].prefabsToSpawn = prefabsToSpawn;
                generators[i].craneMax = craneAmount;
                generators[i].startGeneration(seed + i);
                foreach (GameObject border in generators[i].borderInstances) { allBorders.Add(border); }
                i++;
            }
        }
        VR_Enabled = cfg["VRSettings"]["Enabled"].BoolValue;
        drawDebugSpheres = cfg["NavSettings"]["debugPositionsEnabled"].BoolValue;
        Debug.Log("Loaded config!");
    }

    private void moveAreas()
    {
        float locationX = 0;
        float locationY = 0;
        int xinc = 0;
        for (int i = 0; i < generators.Count; i++)
        {
            areas[i].transform.position = new Vector3(locationX, 0, locationY);
            int x = generators[i].xSize;
            int y = generators[i].ySize;
            if (xinc < maxXSites - 1) { 
                locationX += (x + generatorBorder); 
                xinc += 1;
            }
            else {
                locationX = 0;
                locationY += (y + generatorBorder); 
                xinc = 0;
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
        ground.layer = 1;
        mf = ground.AddComponent<MeshFilter>();
        Mesh msh = mf.mesh;
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

        nms = ground.AddComponent<NavMeshSurface>();
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
            if (border1) { Destroy(border1); }
            if (border2) { Destroy(border2); }
            for (int x = 0; x < numBordersRemoved; x++)
            {
                if (sindex - x >= 0)
                {
                    GameObject starget = start.borderInstances[sindex - x];
                    if (!addedBox) { addedBox = true; Instantiate(start.box, starget.transform.position, starget.transform.rotation * Quaternion.Euler(0f, -90f, 0f)); }
                    if (starget) { Destroy(starget); }
                }
                if (eindex - x >= 0)
                {
                    GameObject etarget = end.borderInstances[eindex - x];
                    if (etarget) { Destroy(etarget); }
                }
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
    void generateTrainTracks()
    {
        Mesh meshTemp = mf.mesh;
        int index = UnityEngine.Random.Range(0, meshTemp.vertices.Length);
        Vector3 p1 = meshTemp.vertices[index];
        Vector3 p2 = meshTemp.vertices[(index + 1) % meshTemp.vertices.Length];
        p1 = mf.transform.TransformPoint(p1);
        p2 = mf.transform.TransformPoint(p2);

        Vector3 pc = Vector3.Lerp(p1, p2, 0.5f);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.right, p1-p2);
        Vector3 dir = Vector3.Cross(p2-p1, Vector3.up).normalized;
        Vector3 rotvec = Quaternion.AngleAxis(90, Vector3.up) * ((p2 - p1).normalized);

        GameObject item = Instantiate(trainTracks, pc+(rotvec*20) - (Vector3.up * 2.5f), rotation);
        item.transform.localScale *= 5f;
        item.GetComponentInChildren<AudioSource>().maxDistance *= 5f;
    }
    void addPlayer()
    {
        if (VR_Enabled)
        {
            GameObject player = Instantiate(Resources.Load("Prefabs/XR_Rig") as GameObject, mf.mesh.bounds.center, Quaternion.identity);
        }
        else
        {
            GameObject player = Instantiate(Resources.Load("Prefabs/Player") as GameObject, mf.mesh.bounds.center, Quaternion.identity);
        }
    }
}
