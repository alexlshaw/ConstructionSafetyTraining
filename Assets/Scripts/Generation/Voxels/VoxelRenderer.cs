using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VoxelRenderer : MonoBehaviour
{

    Mesh mesh;
    List<Vector3> vertices;
    List<int> triangles;
    int vertexIndex;
    List<Vector2> uvs;
    VoxelData voxelData;
    public float scale = 1f;

    float adjScale;
    // Use this for initialization

    public void GenerateVoxelMesh(VoxelData data, Bounds bounds)
    {
        voxelData = data;
        mesh = GetComponent<MeshFilter>().mesh;
        adjScale = scale * 0.5f;
        vertices = new List<Vector3>();
        triangles = new List<int>();
        uvs = new List<Vector2>();
        vertexIndex = 0;
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        for (int z = 0; z < voxelData.Depth; z++)
        {
            for (int y = 0; y < voxelData.Height; y++)
            {
                for (int x = 0; x < voxelData.Width; x++)
                {
                    if (!voxelData.Data[x, y, z])
                    {
                        continue;
                    }
                    AddVoxelDataToChunk(new Vector3(x, y, z), bounds.min);
                }
            }
        }
    }

    bool CheckVoxel(Vector3 pos)
    {
        int x = Mathf.FloorToInt(pos.x);
        int y = Mathf.FloorToInt(pos.y);
        int z = Mathf.FloorToInt(pos.z);

        if (x < 0 || x > voxelData.Width - 1 || y < 0 || y > voxelData.Height - 1 || z < 0 || z > voxelData.Depth - 1)
            return false;

        return voxelData.Data[x, y, z];
    }

    void AddVoxelDataToChunk(Vector3 pos, Vector3 boundsMin)
    {
        for (int p = 0; p < 6; p++)
        {
            if (!CheckVoxel(pos + VoxelData.faceChecks[p]))
            {
                vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 0]]);
                vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 1]]);
                vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 2]]);
                vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 3]]);
                //vertices.Add(pos + boundsMin + VoxelData.voxelVerts[VoxelData.voxelTris[p, 0]]);
                //vertices.Add(pos + boundsMin + VoxelData.voxelVerts[VoxelData.voxelTris[p, 1]]);
                //vertices.Add(pos + boundsMin + VoxelData.voxelVerts[VoxelData.voxelTris[p, 2]]);
                //vertices.Add(pos + boundsMin + VoxelData.voxelVerts[VoxelData.voxelTris[p, 3]]);
                uvs.Add(VoxelData.voxelUvs[0]);
                uvs.Add(VoxelData.voxelUvs[1]);
                uvs.Add(VoxelData.voxelUvs[2]);
                uvs.Add(VoxelData.voxelUvs[3]);
                triangles.Add(vertexIndex);
                triangles.Add(vertexIndex + 1);
                triangles.Add(vertexIndex + 2);
                triangles.Add(vertexIndex + 2);
                triangles.Add(vertexIndex + 1);
                triangles.Add(vertexIndex + 3);
                vertexIndex += 4;
            }
        }
    }

    public void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}