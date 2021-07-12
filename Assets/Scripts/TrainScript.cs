using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainScript : MonoBehaviour
{
    public Transform start;
    public Transform end;
    private float locationAmount = 0;
    private bool boolean = true;

    // Update is called once per frame
    void Update()
    {
        if (boolean)
        {
            locationAmount += 0.0005f;
            transform.position = Vector3.Lerp(transform.position, end.position, Time.deltaTime/2);
        }
        if (!boolean)
        {
            locationAmount += 0.0005f;
            transform.position = Vector3.Lerp(transform.position, start.position, Time.deltaTime/2);
        }
        if (locationAmount > 1 || locationAmount < -1) { boolean = !boolean; locationAmount = 0; }
    }
}
