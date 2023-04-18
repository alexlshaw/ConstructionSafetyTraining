using UnityEngine;
using UnityEngine.AI;

public class VehiclePathing : MonoBehaviour
{
    UnityEngine.Camera cam;
    [SerializeField]
    private Vector3 destination;
    NavMeshAgent agent;
    bool isVisible = false;

    private GameObject player;
    private float playerDistance;
    private float stoppingDistance = 3;

    // Update is called once per frame
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //path = new NavMeshPath();
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        destination = HelperFunctions.findRandomLocation("vehicleRoad");
    }

    private void Update()
    {
        agent.isStopped = false;
        if (Globals.hasHat)
        {
            playerDistance = Vector3.Distance(transform.position, player.transform.position);
            if (playerDistance < stoppingDistance)
            {
                agent.isStopped = true;
            }
        }
        
        
        if (!isVisible && ICanSee())
        {
            //Debug.Log(gameObject.GetInstanceID() + ":" + gameObject.name + ":Visible");
            isVisible = true;
        }
        if(isVisible && !ICanSee())
        {
            //Debug.Log(gameObject.GetInstanceID() + ":" + gameObject.name + ":notVisible");
            isVisible = false;
        }


        agent.destination = destination;

        if (Vector3.Distance(transform.position, destination) < 5)
        {
            destination = HelperFunctions.findRandomLocation("vehicleRoad");
        }
    }
    private bool ICanSee()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
        return GeometryUtility.TestPlanesAABB(planes, GetComponent<Collider>().bounds);
    }



}
