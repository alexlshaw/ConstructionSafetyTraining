using UnityEngine;

public class RoofTileController : MonoBehaviour
{
    GameObject socket;
    GameObject meshOnly;
    RoofController roofController;
    public bool isSocket;

    // Start is called before the first frame update
    void Awake()
    {
        socket = transform.Find("SocketInteractor").gameObject;
        meshOnly = transform.Find("MeshOnly").gameObject;
        roofController = GetComponentInParent<RoofController>();

    }
    private void Start()
    {
        GameEvents.current.onStartRoofLevel += DeActivate;
        GameEvents.current.onRoofPlanksComplete += Activate;
        GameEvents.current.onRoofPlanksComplete += displayMesh;
    }

    private void Initialise()
    {
        float random = Random.Range(0f, 1f);
        if (random < roofController.tileChance)
        {
            isSocket = true;
            displaySocket();
        }
        else
        {
            isSocket = false;
            displayMesh();
        }

    }


    void Activate()
    {
        gameObject.SetActive(true);

    }
    void DeActivate()
    {
        gameObject.SetActive(false);
    }
    //    public void addPlasterFilled()
    //    {
    //        roofController.totalTilesFilled += 1;
    //        Debug.Log(roofController.totalTilesFilled);

    //    }
    //    public void removPlasterFilled()
    //    {
    //        roofController.totalTilesFilled -= 1;
    //        Debug.Log(roofController.totalTilesFilled);

    //    }

    void displayMesh()
    {
        meshOnly.SetActive(true);
        socket.SetActive(false);
    }

    void displaySocket()
    {
        meshOnly.SetActive(false);
        socket.SetActive(true);
    }
}
