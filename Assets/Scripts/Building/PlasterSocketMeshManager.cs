using UnityEngine;

public class PlasterSocketMeshManager : MonoBehaviour
{
    GameObject socket;
    GameObject meshOnly;
    GameObject nailTask;

    FloorController floorController;
    NailTaskManager nailTaskManager;

    public bool isSocket;

    void Start()
    {
        socket = transform.Find("SocketInteractor").gameObject;
        meshOnly = transform.Find("MeshOnly").gameObject;
        nailTask = transform.Find("NailTasks").gameObject;

        floorController = GetComponentInParent<FloorController>();
        nailTaskManager = nailTask.GetComponent<NailTaskManager>();

        GameEvents.current.onFoundationFilled += DeActivate;
        GameEvents.current.onStartSecondLevel += Activate;
        GameEvents.current.onNailsLevel += displayMesh;
        GameEvents.current.onStartGame += DeactivateNailTask;
        GameEvents.current.onResetLevel += DeactivateNailTask;
        GameEvents.current.onResetLevel += reset;
        GameEvents.current.onStartSecondLevel += Initialise;

    }
    private void Initialise()
    {
        float random = Random.Range(0f, 1f);
        if (random < floorController.plasterChance)
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
    public void nailFinished()
    {
        Debug.Log("plasterfinished");
        floorController.totalPlastersFilled += 1;
        floorController.checkIfPlastersFilled();
    }
    void ActivateNailTask()
    {
        Debug.Log("Acivate");

        nailTask.SetActive(true);
    }
    void DeactivateNailTask()
    {
        nailTask.SetActive(false);
    }
    public void addPlasterFilled()
    {
        Debug.Log("Filled");
        ActivateNailTask();
    }
    public void removePlasterFilled()
    {
        DeactivateNailTask();
    }

    void displayMesh()
    {
        meshOnly.SetActive(true);
        socket.SetActive(false);
        isSocket = false;
    }

    void displaySocket()
    {
        meshOnly.SetActive(false);
        socket.SetActive(true);
        isSocket = true;
    }

    void reset()
    {
        Initialise();
    }
}
