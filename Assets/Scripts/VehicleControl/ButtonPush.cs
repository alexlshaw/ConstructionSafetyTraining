using System.Collections;
using UnityEngine;

public class ButtonPush : MonoBehaviour
{
    private float originalPosition;
    private float finalPosition;
    public float pushSpeed = 0.000001f;
    private bool goingDown = false;
    private bool goingUp = false;
    private DiggingTesting digging;
    private void Start()
    {
        originalPosition = transform.localPosition.y;
        float dropDistance = transform.localScale.y / 2;
        finalPosition = originalPosition - dropDistance;
        digging = GetComponentInParent<DiggingTesting>();
    }
    public void startDigging()
    {
        if(!digging.digDown && !digging.digUp)
        {
            digging.digDown = true;
        }
    }

    public void unload()
    {
        digging.unload = true;
    }
    public void animateButton()
    {
        goingDown = true;
    }
    private void Update()
    {
        //Debug.Log("Down" + goingDown);
        //Debug.Log("Up" + goingUp);

        if (goingDown)
        {
            //Debug.Log("findown");

            StartCoroutine(GoingDown());
        }
        if (goingUp)
        {
            StartCoroutine(GoingUp());
        }
    }

    IEnumerator GoingDown()
    {
        float timeSinceStarted = 0f;
        while (true)
        {
            timeSinceStarted += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, finalPosition, transform.localPosition.z), timeSinceStarted);

            if (transform.localPosition.y <= finalPosition)
            {
                this.goingDown = false;
                this.goingUp = true;
                yield break;
            }

            yield return null;
        }
    }

    IEnumerator GoingUp()
    {
        float timeSinceStarted = 0f;
        while (true)
        {
            timeSinceStarted += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, originalPosition, transform.localPosition.z), timeSinceStarted);
            if (transform.localPosition.y >= originalPosition)
            {
                this.goingUp = false;
                yield break;
            }

            yield return null;
        }
    }
}
