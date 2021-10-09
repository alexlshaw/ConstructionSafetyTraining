using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class outsideBuildingScript : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        //UnityEngine.Random.InitState(0);
        float x, y, z;
        x = transform.localScale.x * Random.Range(0.5f, 1.5f);
        y = transform.localScale.y * Random.Range(1.0f, 4.0f);
        z = transform.localScale.z * Random.Range(0.5f, 1.5f);
        transform.localScale = new Vector3(x, y, z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
