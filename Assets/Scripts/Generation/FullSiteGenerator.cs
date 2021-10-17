using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using SharpConfig;
using System.IO;
using UnityEngine.AI;

public class FullSiteGenerator : MonoBehaviour
{
    private List<Generator> generators = new List<Generator>();
    private List<GameObject> areas = new List<GameObject>();
    private List<GameObject> allBorders = new List<GameObject>();
    private List<GameObject> borderInstances = new List<GameObject>();
    private List<Vector3> vertices;
    private GameObject spawned = null;
    private GameObject ground;
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
    public int groundDepth = 5;

    private Configuration cfg;
    private string filepath;
    private SiteNavScript nav;
    private bool drawDebugSpheres;
    private NavMeshSurface nms;
    private GameObject startArea;

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        UnityEngine.Random.InitState(seed);
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
        nav = gameObject.GetComponent<SiteNavScript>();
        //Debug.Log((nav != null).ToString()+(generators != null).ToString()+(allBorders != null).ToString());
        nav.drawDebugLocations = drawDebugSpheres;
        nav.getRandomLocations(generators, allBorders);
        nav.pickLocation();
        nms.BuildNavMesh();
        addRandomAI(mf.mesh.bounds.center, Quaternion.identity);
        for (int i = 0; i < generators.Count; i++)
        {
            generators[i].generateTracks();
        }
        generateEdgeItems();
        yield return new WaitForFixedUpdate();

        InitObjectRegionDict(ground);
        foreach(String key in Globals.labelToVectorDict.Keys)
        {
            Debug.Log(key);
        }

        createVoxelGround();

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

    void moveAreas()
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
        ground = new GameObject("groundMesh");
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
        vertices.Add(vertices[0]); //Add first vertex to end to create loop
        for (int i = 0; i < vertices.Count - 1; i++)
        {
            Vector3 p1 = vertices[i];
            Vector3 p2 = vertices[i + 1];
            float distance = Vector3.Distance(p1, p2);

            for (float x = 0; x < distance; x += 7.5f)
            {
                float percentageOfDistance = x / distance;
                if (percentageOfDistance < 1.0) //check that current spawn is actually on the edge (between the two vertices)
                {
                    Vector3 position = Vector3.Lerp(p1, p2, percentageOfDistance);

                    bool shouldSpawn = true;
                    foreach (GameObject item in borderInstances)
                    {
                        if (Vector3.Distance(item.transform.position, position) <= 3.75) //half the spacing between border prefabs is the minimum distance from the current to any border
                        {
                            shouldSpawn = false;
                        }
                    }
                    if (shouldSpawn)
                    {
                        GameObject border = Instantiate(wholeSiteBorder, position, Quaternion.identity);
                        border.transform.LookAt(p2);
                        border.transform.parent = spawned.transform;
                        borderInstances.Add(border);
                    }
                }
            }
        }
    }
    void generateOutsideBuilding(Vector3 p1, Vector3 p2)
    {
        Mesh meshTemp = mf.mesh;
        p1 = mf.transform.TransformPoint(p1);
        p2 = mf.transform.TransformPoint(p2);

        Vector3 pc = Vector3.Lerp(p1, p2, 0.5f);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.right, p1 - p2);
        Vector3 dir = Vector3.Cross(p2 - p1, Vector3.up).normalized;
        Vector3 rotvec = Quaternion.AngleAxis(90, Vector3.up) * ((p2 - p1).normalized);

        GameObject item = Instantiate(Resources.Load("Prefabs/outsideBuilding") as GameObject, pc + (rotvec * 15) - (Vector3.up * 2.5f), rotation);
    }


    void generateTrainTracks(Vector3 p1, Vector3 p2)
    {
        Mesh meshTemp = mf.mesh;
        p1 = mf.transform.TransformPoint(p1);
        p2 = mf.transform.TransformPoint(p2);

        Vector3 pc = Vector3.Lerp(p1, p2, 0.5f);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.right, p1-p2);
        Vector3 dir = Vector3.Cross(p2-p1, Vector3.up).normalized;
        Vector3 rotvec = Quaternion.AngleAxis(90, Vector3.up) * ((p2 - p1).normalized);

        GameObject item = Instantiate(Resources.Load("Prefabs/TrainTracks") as GameObject, pc +(rotvec*20) - (Vector3.up * 2.5f), rotation);
        item.transform.localScale *= 5f;
        item.GetComponentInChildren<AudioSource>().maxDistance *= 5f;
    }
    void generateStartArea(Vector3 p1, Vector3 p2)
    {
        Mesh meshTemp = mf.mesh;
        p1 = mf.transform.TransformPoint(p1);
        p2 = mf.transform.TransformPoint(p2);

        Vector3 pc = Vector3.Lerp(p1, p2, 0.5f);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.right, p1 - p2);
        Vector3 dir = Vector3.Cross(p2 - p1, Vector3.up).normalized;
        Vector3 rotvec = Quaternion.AngleAxis(90, Vector3.up) * ((p2 - p1).normalized);
        startArea = Instantiate(Resources.Load("Prefabs/startArea") as GameObject, pc + (rotvec * 5), rotation);
        
        addPlayer(startArea.transform.position, rotation);
        addSign(pc + (rotvec * 1) + new Vector3(0, 4, 0) + startArea.transform.right * -3, rotation);
        
        foreach (GameObject border in borderInstances)
        {
            if (Vector3.Distance(startArea.transform.position, border.transform.position) < 10.0)
            {
                Destroy(border);
            }
        }
    }
    void addPlayer(Vector3 position, Quaternion rotation)
    {
        GameObject player = Instantiate(VR_Enabled ? Resources.Load("Prefabs/XR_Rig") as GameObject : Resources.Load("Prefabs/Player") as GameObject, position, rotation);
    }
    void addSign(Vector3 position, Quaternion rotation)
    {
        Instantiate(Resources.Load("Prefabs/Sign") as GameObject, position, rotation);
    }
    void addRandomAI(Vector3 position, Quaternion rotation)
    {
        GameObject AI = Instantiate(Resources.Load("Prefabs/AI_Walker_Actor") as GameObject, position, rotation);
    }
    void generateEdgeItems()
    {
        Mesh meshTemp = mf.mesh;
        bool tracks = false;
        bool spawn = false;

        for (int i = 0; i < meshTemp.vertices.Length-3; i++)
        {
            Vector3 vert1 = meshTemp.vertices[i];
            Vector3 vert2 = meshTemp.vertices[i + 1];
            if (!tracks)
            {
                generateTrainTracks(vert1, vert2);
                tracks = true;   
                generateOutsideBuilding(meshTemp.vertices[i+2], meshTemp.vertices[i+3]);
                i += 2;
            }
            else if (!spawn)
            {
                generateStartArea(vert1, vert2);
                spawn = true;
            }
            else
            {
                generateOutsideBuilding(vert1, vert2);
            }
        }
    }
    private void InitObjectRegionDict(GameObject gameObject)
    {
        MeshRenderer meshRenderer = gameObject.transform.GetComponent<MeshRenderer>();
        for (int x = (int)meshRenderer.bounds.min.x; x < meshRenderer.bounds.max.x; x++)
        {
            for (int z = (int)meshRenderer.bounds.min.z; z < meshRenderer.bounds.max.z; z++)
            {
                Vector3Int x0z = new Vector3Int(x, (int)(meshRenderer.bounds.max.y + 1f), z);
                RaycastHit[] hits = Physics.RaycastAll(x0z, Vector3.down, 10f);
                bool addToDict = false;
                foreach (RaycastHit hit in hits)
                {
                    if (hit.transform.name.Equals(gameObject.transform.name))
                    {
                        addToDict = true;
                        break;
                    }

                }
                if (addToDict)
                {
                    Globals.addTagToPosition(new Vector2Int(x, z), gameObject.name);
                }
                
            }
        }
    }

    private void createVoxelGround()
    {
        MeshRenderer meshRenderer = ground.transform.GetComponent<MeshRenderer>();
        bool[,,] voxelGrid = new bool[(int)meshRenderer.bounds.size.x + 1, groundDepth + 1, (int)meshRenderer.bounds.size.z + 1];
        //GameObject building = GameObject.Find("BuildingParent");
        //Vector3 buildingRelativePosition = building.transform.position - meshRenderer.bounds.min;
        //building.transform.position = new Vector3((int)buildingRelativePosition.x, (int)buildingRelativePosition.y, (int)buildingRelativePosition.z) + meshRenderer.bounds.min;
        //Mesh buildingMesh = building.GetComponent<MeshFilter>().mesh;
        foreach (Vector2Int vector in Globals.labelToVectorDict["groundMesh"])
        {
            for (int i = 0; i < groundDepth; i++)
            {
                Vector3 relativePosition = new Vector3(vector.x, 0, vector.y) - new Vector3(meshRenderer.bounds.min.x, 0f, meshRenderer.bounds.min.z);
                voxelGrid[(int)relativePosition.x, i, (int)relativePosition.z] = true;
            }
        }
        //RemoveGroundUnderBuilding(voxelGrid, buildingMesh, meshRenderer);
        VoxelData groundData = new VoxelData();
        groundData.Data = voxelGrid;

        GameObject voxelGround = new GameObject("voxelGround");

        voxelGround.AddComponent<VoxelRenderer>();
        voxelGround.GetComponent<VoxelRenderer>().GenerateVoxelMesh(groundData, meshRenderer.bounds);
        voxelGround.GetComponent<VoxelRenderer>().UpdateMesh();
        voxelGround.GetComponent<MeshRenderer>().material = groundMaterial;
        voxelGround.AddComponent<MeshCollider>().sharedMesh = voxelGround.GetComponent<MeshFilter>().mesh;
        voxelGround.transform.position = new Vector3(voxelGround.transform.position.x, -groundDepth + ground.transform.position.y, voxelGround.transform.position.z);
    }
    private void RemoveGroundUnderBuilding(bool[,,] voxelGrid, Mesh buildingMesh, MeshRenderer meshRenderer)
    {

        for (int x = (int)buildingMesh.bounds.min.x; x < (int)buildingMesh.bounds.max.x + 1; x++)
        {
            for (int z = (int)buildingMesh.bounds.min.z; z < (int)buildingMesh.bounds.max.z + 1; z++)
            {
                Vector3 relativePosition = new Vector3(x, 0, z) - new Vector3((int)meshRenderer.bounds.min.x, 0f, (int)meshRenderer.bounds.min.z);

                voxelGrid[(int)relativePosition.x, 0, (int)relativePosition.z] = false;
            }
        }
    }
}


