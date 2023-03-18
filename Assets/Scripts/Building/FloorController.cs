using System.Linq;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    int totalTasks;

    public float plasterChance = 0.05f;
    public float socketChance = 0.1f;

    BuildingTaskManager buildingManager;
    PlankSocketMeshManager[] totalPlanks;
    PlasterSocketMeshManager[] totalPlasters;

    public int totalPlanksToFill;
    public int totalPlanksFilled;

    public int totalPlastersToFill;
    public int totalPlastersFilled;

    public int totalTasksCompleted;
    public int totalSocketsCompleted;
    // Start is called before the first frame update
    void Start()
    {
        totalTasks = GetComponentsInChildren<NailTaskManager>().GetLength(0);
        buildingManager = GetComponentInParent<BuildingTaskManager>();

        totalPlanks = GetComponentsInChildren<PlankSocketMeshManager>();
        totalPlanksToFill = totalPlanks.Where(plank => plank.isSocket).ToList().Count;

        totalPlasters = GetComponentsInChildren<PlasterSocketMeshManager>();
        totalPlastersToFill = totalPlasters.Where(plaster => plaster.isSocket).ToList().Count;

        Debug.Log(gameObject.name + "totalPlanksToFill" + totalPlanksToFill);

        
        GameEvents.current.onResetLevel += reset;
        GameEvents.current.onStartSecondLevel += setActive;

        GameEvents.current.onFoundationFilled += Initialise;
        GameEvents.current.onStartSecondLevel += Initialise;

        totalTasksCompleted = 0;
    }

    void Initialise()
    {
        if(Globals.currentLevel == 1)
        {
            foreach (PlankSocketMeshManager plankSocketMeshManager in totalPlanks)
            {
                plankSocketMeshManager.reset();
            }
            totalPlanksToFill = totalPlanks.Where(plank => plank.isSocket).ToList().Count;
            totalPlanksFilled = 0;
            Debug.Log(gameObject.name + "totalPlanksToFill" + totalPlanksToFill);
            checkIfPlanksFilled();
        }
        if (Globals.currentLevel == 2)
        {
            foreach (PlankSocketMeshManager plankSocketMeshManager in totalPlanks)
            {
                plankSocketMeshManager.reset();
            }
            totalPlastersToFill = totalPlasters.Where(plaster => plaster.isSocket).ToList().Count;
            totalPlastersFilled = 0;
            Debug.Log(gameObject.name + "totalPlastersToFill" + totalPlastersToFill);
            checkIfPlastersFilled();
        }
        Debug.Log(buildingManager.totalFloorsCompleted + "/" + buildingManager.totalFloorstoComplete);



    }
    // Update is called once per frame
    public void checkIfPlanksFilled()
    {
        if(totalPlanksFilled == totalPlanksToFill && gameObject.activeSelf)
        {
            Debug.Log("finished floor planks");
            buildingManager.addFloorCompleted();
        }
    }
    public void checkIfPlastersFilled()
    {
        if (totalPlastersToFill == totalPlastersFilled && gameObject.activeSelf)
        {
            Debug.Log("finished floor plasters");
            buildingManager.addFloorCompleted();
        }
    }
    public void checkTasksCompleted()
    {
        if (totalTasks == totalTasksCompleted)
        {
            GameEvents.current.FirstFloorFinished();
        }
    }

    void setActive()
    {
        gameObject.SetActive(true);
    }
    void reset()
    {
        if (Globals.currentLevel == 1)
        {
            Initialise();
            gameObject.SetActive(false);
        }
    }

}
