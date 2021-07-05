using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedItem : MonoBehaviour
{
    public int xSize;
    public int ySize;
    public bool generateBorder;
    public bool randomlyRotate = true;
    public Material material;
    // Start is called before the first frame update
    void Start()
    {
        List<Vector3> points = new List<Vector3>();
        if (generateBorder){
            Vector3 topleft = new Vector3(transform.position.x - xSize, 0.1f, transform.position.z + ySize);
            points.Add(topleft);
            Vector3 bottomleft = new Vector3(transform.position.x - xSize, 0.1f, transform.position.z - ySize);
            points.Add(bottomleft);
            Vector3 topright = new Vector3(transform.position.x + xSize, 0.1f, transform.position.z + ySize);
            points.Add(topright);
            Vector3 bottomright = new Vector3(transform.position.x + xSize, 0.1f, transform.position.z - ySize);
            points.Add(bottomright);
        
            GameObject borderObject = new GameObject("Border");
            borderObject.transform.parent = gameObject.transform;

            MeshRenderer meshRenderer = borderObject.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = new Material(material);
            MeshFilter meshFilter = borderObject.AddComponent<MeshFilter>();
            Mesh mesh = new Mesh();

            mesh.vertices = points.ToArray();
            int[] tris ={0,2,3,0,3,1};
            mesh.triangles = tris;
            meshFilter.mesh = mesh;
        }
    }
}
