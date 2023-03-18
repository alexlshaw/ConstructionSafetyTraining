using UnityEngine;

public class RoofPlankController : MonoBehaviour
{
    GameObject socket;
    GameObject meshOnly;
    RoofController roofController;
    public bool isSocket;

    // Start is called before the first frame update
    void Start()
    {
        socket = transform.Find("SocketInteractor").gameObject;
        meshOnly = transform.Find("MeshOnly").gameObject;
        roofController = GetComponentInParent<RoofController>();

        //GameEvents.current.onStartRoofLevel += Initialise;
        GameEvents.current.onRoofPlanksComplete += displayMesh;
        GameEvents.current.onResetLevel += reset;
    }
    public void Initialise()
    {
        float random = Random.Range(0f, 1f);
        if (random < roofController.socketChance)
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
    public void addPlankFilled()
    {
        roofController.addPlank();
    }
    public void removePlankFilled()
    {
        roofController.removePlank();
    }
    public void reset()
    {
        if (Globals.currentLevel == 3)
        {
            Initialise();
        }
    }
}
