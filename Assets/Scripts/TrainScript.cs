using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainScript : MonoBehaviour
{
    public Transform start;
    public Transform end;
    private float locationAmount;

    // Update is called once per frame
    void Update()
    {
        locationAmount += 0.0005f;
        transform.position = Vector3.Lerp(transform.position, end.position, Time.deltaTime/2);
        if (locationAmount > 1) { transform.position = start.position; }
    }
}
