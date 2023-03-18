using UnityEngine;

public class NailTaskManager : MonoBehaviour
{
    // Start is called before the first frame update
    int totalNails;
    public int nailsCompleted;


    PlasterSocketMeshManager plaster;
    void Start()
    {
        GameEvents.current.onResetLevel += reset;
        plaster = GetComponentInParent<PlasterSocketMeshManager>();
        totalNails = GetComponentsInChildren<NailTask>().GetLength(0);
        Debug.Log("totalNails" + totalNails);
        nailsCompleted = 0;
    }

    
    // Update is called once per frame
    public void checkIfFilled()
    {
        if (nailsCompleted == totalNails)
        {
            Debug.Log("nailsCompleted");

            plaster.nailFinished();
        }
    }

    public void addCompleted()
    {
        nailsCompleted += 1;
        Debug.Log("completed" + nailsCompleted);
        checkIfFilled();
    }

    public void reset()
    {
        nailsCompleted = 0;
    }
}
