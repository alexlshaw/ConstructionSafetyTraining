using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainScript : MonoBehaviour
{
    public Transform start;
    public Transform end;
    public float movementStep = 0.00025f;
    public float delayBetweenTrains = 2f;
    private float locationAmount;

    // Update is called once per frame
    void Update()
    {
        locationAmount += movementStep;
        transform.position = Vector3.Lerp(transform.position, end.position, Time.deltaTime/2);
        if (locationAmount > 1) { StartCoroutine(wait()); }
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(delayBetweenTrains);
        transform.position = start.position;
        locationAmount = 0;
    }
}
