using System.Collections;
using UnityEngine;

public class CraneLoad : MonoBehaviour
{
    private float time;
    private bool doRotate = true;
    private bool doChecking = true;
    private LineRenderer lr;
    private float startRotY;
    private int multiplier = 1;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
    }
    void Update()
    {
        lr.SetPosition(0, new Vector3(0, 0, 0));
        lr.SetPosition(1, new Vector3(0, (100 - transform.localPosition.y) / 4, 0));
        if (doRotate)
        {
            transform.parent.transform.Rotate(Vector3.up, 0.01f * multiplier);
        }

        time += Time.deltaTime;

        Vector3 h1start = transform.TransformPoint(new Vector3(0.75f, 0, 0));
        Vector3 h2start = transform.TransformPoint(new Vector3(-0.75f, 0, 0));
        Vector3 h3start = transform.TransformPoint(new Vector3(0, 0, 1f));
        Vector3 h4start = transform.TransformPoint(new Vector3(0, 0, -1f));
        RaycastHit h1;
        RaycastHit h2;
        RaycastHit h3;
        RaycastHit h4;
        bool r1 = Physics.Raycast(h1start, transform.TransformDirection(Vector3.down), out h1, Mathf.Infinity);
        //Debug.DrawRay(h1start, transform.TransformDirection(Vector3.down) * 100, Color.yellow, 10f);

        bool r2 = Physics.Raycast(h2start, transform.TransformDirection(Vector3.down), out h2, Mathf.Infinity);
        //Debug.DrawRay(h2start, transform.TransformDirection(Vector3.down) * 100, Color.yellow, 10f);

        bool r3 = Physics.Raycast(h3start, transform.TransformDirection(Vector3.down), out h3, Mathf.Infinity);
        //Debug.DrawRay(h3start, transform.TransformDirection(Vector3.down) * 100, Color.yellow, 10f);

        bool r4 = Physics.Raycast(h4start, transform.TransformDirection(Vector3.down), out h4, Mathf.Infinity);
        //Debug.DrawRay(h4start, transform.TransformDirection(Vector3.down) * 100, Color.yellow, 10f);

        if (!r1 && !r2 && !r3 && !r4)
        {
            multiplier *= -1;
        }

        if (time > Random.Range(5f, 10f) && doChecking)
        {
            time = 0f;
            if (r1 && r2 && r3 && r4)
            {
                if (!h1.collider.gameObject.CompareTag("Train") && !h2.collider.gameObject.CompareTag("Train") &&
                !h2.collider.gameObject.CompareTag("Train") && !h2.collider.gameObject.CompareTag("Train"))
                {
                    if (h1.distance == h2.distance && h1.distance == h3.distance && h1.distance == h4.distance)
                    {
                        StartCoroutine(waitThenMove());
                    }
                }
            }
        }
    }
    IEnumerator waitThenMove()
    {
        doRotate = false;
        doChecking = false;
        float elapsedTime = 0;
        float waitTime = 10f;
        Vector3 startPos = new Vector3(transform.localPosition.x, 90, transform.localPosition.z);
        Vector3 endPos = new Vector3(transform.localPosition.x, 2f, transform.localPosition.z);

        while (elapsedTime < waitTime)
        {
            transform.localPosition = Vector3.Lerp(startPos, endPos, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = endPos;
        yield return new WaitForSeconds(10f);

        elapsedTime = 0f;
        while (elapsedTime < waitTime)
        {
            transform.localPosition = Vector3.Lerp(endPos, startPos, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = startPos;

        doRotate = true;
        doChecking = true;
        time = 0f;
    }
}
