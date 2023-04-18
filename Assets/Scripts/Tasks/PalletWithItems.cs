using System;
using UnityEngine;

public class PalletWithItems : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject objectToSpawn;

    public Transform parentObject;
    //public GameObject plaster;

    int plankCount;
    int plasterCount;
    int cementCount;
    void Start()
    {
        if (transform.name.Contains("PalletLongPlank"))
        {
            plankCount = 50;
            GameEvents.current.onStartGame += GeneratePlanks;
            GameEvents.current.onStartRoofLevel += Deactivate;

        }
        if (transform.name.Contains("Plaster"))
        {
            plasterCount = 15;
            GameEvents.current.onStartGame += GeneratePlaster;
        }
        if (transform.name.Contains("Cement"))
        {
            cementCount = 24;
            GameEvents.current.onStartGame += GenerateCement;
        }
        if (transform.name.Contains("Roof"))
        {
            plankCount = 50;
            GameEvents.current.onStartRoofLevel += GeneratePlanks;

        }
    }
    void Deactivate()
    {
        gameObject.SetActive(false);
    }
    private void GenerateCement()
    {
        int count = 0;
        float y = 0.44f;
        while (count <= cementCount)
        {
            for( float z = -0.9f; z <= 0.8f; z += 0.8f)
            {
                for (float x = -0.5f; x <= 0.55f; x += 1)
                {
                    GameObject newplank = GameObject.Instantiate(objectToSpawn);
                    newplank.transform.SetParent(transform);
                    newplank.name = objectToSpawn.name;
                    newplank.transform.localPosition = new Vector3(x, y, z);
                    newplank.transform.localEulerAngles = new Vector3(0, 90, 0);
                    newplank.transform.parent = parentObject;
                    count += 1;
                }
            }
            
            y += 0.23f;
        }
    }

    private void GeneratePlaster()
    {
        int count = 0;
        float y = 0.55f;
        while (count <= plasterCount)
        {

            GameObject newplank = GameObject.Instantiate(objectToSpawn);
            newplank.transform.SetParent(transform);
            newplank.name = objectToSpawn.name;
            newplank.transform.localPosition = new Vector3(0, y, 0);
            newplank.transform.localEulerAngles = new Vector3(0, 0, 90);
            newplank.transform.parent = parentObject;
            count += 1;

            y += 0.11f;
        }
    }

    void GeneratePlanks()
    {
        int count = 0;
        float y = 0.55f;
        while (count <= plankCount)
        {
            for (float x = -0.95f; x <= 1f; x += 0.325f)
            {
                GameObject newplank = GameObject.Instantiate(objectToSpawn);
                newplank.transform.SetParent(transform);
                newplank.name = objectToSpawn.name;
                newplank.transform.localPosition = new Vector3(x, y, 0);
                newplank.transform.localRotation = Quaternion.identity;
                newplank.transform.parent = parentObject;
                count += 1;
            }
            y += 0.22f;
        }


    }
    // Update is called once per frame
    void Update()
    {

    }
}
