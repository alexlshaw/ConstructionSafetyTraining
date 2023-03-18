using UnityEngine;

public class PlankSocketMeshManager : MonoBehaviour
{
    GameObject socket;
    GameObject meshOnly;
    FloorController floorController;
    public bool isSocket;

    void Start()
    {
        socket = transform.Find("SocketInteractor").gameObject;
        meshOnly = transform.Find("MeshOnly").gameObject;
        floorController = GetComponentInParent<FloorController>();
        Initialise();
        GameEvents.current.onStartSecondLevel += displayMesh;
    }

    private void Initialise()
    {
        if (!floorController.transform.name.Equals("Floor0"))
        {
            isSocket = false;
            displayMesh();
        }
        else
        {
            float random = Random.Range(0f, 1f);
            if (random < floorController.socketChance)
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

    }

    public void addPlankFilled()
    {
        floorController.totalPlanksFilled += 1;
        Debug.Log(floorController.totalPlanksFilled);
        floorController.checkIfPlanksFilled();
    }
    public void removePlankFilled()
    {
        floorController.totalPlanksFilled -= 1;
        floorController.checkIfPlanksFilled();
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

    public void reset()
    {
        if(Globals.currentLevel == 1)
        {
            Initialise();
        }
    }
}
