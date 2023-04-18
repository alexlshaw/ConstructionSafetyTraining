using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScaffoldController : MonoBehaviour
{
    [SerializeField]
    private ScaffoldingPart[] scaffoldingObjects;
    int totalFilled;

    public LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        totalFilled = scaffoldingObjects.Where(part => part.isFilled).ToList().Count;
    }

    public void addFilled()
    {
        totalFilled += 1;
    }
    public void removeFilled()
    {
        totalFilled -= 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player")) {
            Debug.Log("entered");
            Debug.Log(totalFilled);
            if (totalFilled != scaffoldingObjects.Length)
            {
                StartCoroutine(removeFloor());
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            Debug.Log("exitted");
            Debug.Log(totalFilled);
            StopCoroutine(removeFloor());
        }
    }
    private IEnumerator removeFloor()
    {
        int lastTotalFilled = totalFilled;
        Debug.Log("lastTotalFilled" + lastTotalFilled);
        yield return new WaitForSeconds(10);
        Debug.Log("totalFilled" + totalFilled);
        if(totalFilled == lastTotalFilled && totalFilled != scaffoldingObjects.Length)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position+new Vector3(0, 0.3f, 0), Vector3.down, out hit, Mathf.Infinity, layerMask))
            {
                if (hit.collider.gameObject.name.Equals("ScaffoldCeiling"))
                {
                    Debug.Log("hit ceiling");
                    hit.collider.gameObject.SetActive(false);
                    //GameEvents.current.TakeDamage();
                }
            }
        }
    }
}
