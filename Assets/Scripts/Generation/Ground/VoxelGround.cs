using UnityEngine;

public class VoxelGround : MonoBehaviour
{
    int groundDepth;
    bool[,,] voxelGrid;
    Bounds bounds;
    Material groundMaterial;

    public void setVariables(int groundDepth, bool[,,] voxelGrid, Bounds bounds, Material groundMaterial)
    {
        this.groundDepth = groundDepth;
        this.voxelGrid = voxelGrid;
        this.bounds = bounds;
        this.groundMaterial = groundMaterial;
    }

    public void initializeGround()
    {
        VoxelData groundData = new VoxelData();
        groundData.Data = voxelGrid;
        gameObject.AddComponent<VoxelRenderer>();
        GetComponent<VoxelRenderer>().GenerateVoxelMesh(groundData, bounds);
        GetComponent<VoxelRenderer>().UpdateMesh();
        GetComponent<MeshRenderer>().material = groundMaterial;
        gameObject.AddComponent<MeshCollider>().sharedMesh = gameObject.GetComponent<MeshFilter>().mesh;
    }


    // Start is called before the first frame update

}
