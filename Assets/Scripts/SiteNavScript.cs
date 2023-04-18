using System.Collections.Generic;
using UnityEngine;

public class SiteNavScript : MonoBehaviour
{
    public bool drawDebugLocations = true;
    private Vector3 targetLocation;
    private GameObject targetIndicator;
    [SerializeField]
    private List<GameObject> firstLevelNodes = new List<GameObject>();
    private List<GameObject> secondLevelNodes = new List<GameObject>();
    private List<GameObject> roofLevelNodes = new List<GameObject>();

    private void Start()
    {
        GameEvents.current.onSetSiteNavLocation += setLocation;
    }

    public void setLocation(Vector3 location)
    {
        targetLocation = location;
        targetIndicator = Instantiate(Resources.Load("Prefabs/TargetLocation") as GameObject, targetLocation, Quaternion.identity);

    }

    void Update()
    {
        if (Camera.main != null)
        {
            float dist = Vector3.Distance(Camera.main.transform.position, targetLocation);
            if (dist < 6)
            {
                Destroy(targetIndicator);
            }
        }

    }

}
