using System;
using UnityEngine;

public static class HelperFunctions
{
    public static void combineChildrenMesh(GameObject gameObject)
    {
        MeshFilter[] meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            //meshFilters[i].gameObject.SetActive(false);

            i++;
        }
        Mesh combinedAllMesh = new Mesh();
        combinedAllMesh.CombineMeshes(combine);
        gameObject.AddComponent<MeshFilter>().mesh = combinedAllMesh;
        gameObject.AddComponent<MeshCollider>().sharedMesh = gameObject.GetComponent<MeshFilter>().mesh;
    }
    public static void createSphere(string dictName)
    {
        GameObject sphereParent = new GameObject(dictName + "-parent");
        foreach (Vector2Int vector in Globals.labelToVectorDict[dictName])
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = new Vector3(vector.x, 0.1f, vector.y);
            sphere.transform.parent = sphereParent.transform;
        }
    }

    public static void InitObjectRegionDict(GameObject gameObject)
    {
        if (gameObject == null)
        {
            Debug.Log("destroyed");
            return;
        }
        if (gameObject.GetComponent<MeshRenderer>() == null)
        {
            HelperFunctions.combineChildrenMesh(gameObject);

        }
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
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

    public static void InitObjectRegionDictByPosition(GameObject gameObject)
    {
        if (gameObject == null)
        {
            return;
        }

        Globals.addTagToPosition(new Vector2Int((int)gameObject.transform.position.x, (int)gameObject.transform.position.z), gameObject.name);
    }

    public static Vector3 findRandomLocation(String dictName)
    {
        int index = UnityEngine.Random.Range(0, Globals.labelToVectorDict[dictName].Count);
        Vector2 vector = Globals.labelToVectorDict[dictName][index];
        return new Vector3(vector.x, 0, vector.y);
    }
}
