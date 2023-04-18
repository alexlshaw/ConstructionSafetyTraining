using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTaskManager : MonoBehaviour
{
    FloorController[] allFloors;

    public int totalFloorstoComplete;
    public int totalRoofstoComplete;

    public int totalFloorsCompleted;
    public int totalRoofsCompleted;
    // Start is called before the first frame update
    void Start()
    {
        allFloors = GetComponentsInChildren<FloorController>();

        Debug.Log("TotalFloors" + totalFloorstoComplete);
        GameEvents.current.onStartGame += Initialise;
        GameEvents.current.onFoundationFilled += Initialise;
        GameEvents.current.onStartSecondLevel += Initialise;
        GameEvents.current.onStartRoofLevel += Initialise;
        GameEvents.current.onRoofPlanksComplete += Initialise;

    }

    void Initialise()
    {
        totalFloorstoComplete = GetComponentsInChildren<FloorController>().GetLength(0);
        totalFloorsCompleted = 0;
        totalRoofstoComplete = GetComponentsInChildren<RoofController>().GetLength(0);
        totalRoofsCompleted = 0;
    }
    public void addRoofPlankCompleted()
    {
        totalRoofsCompleted += 1;
        if (totalRoofsCompleted == totalRoofstoComplete)
        {
            Debug.Log("All Roofs Plank Finished");
            GameEvents.current.RoofPlanksFinished();

        }
    }
    public void addRoofTilesCompleted()
    {
        totalRoofsCompleted += 1;
        if (totalRoofsCompleted == totalRoofstoComplete)
        {
            Debug.Log("All Roofs Tiles Finished");
            GameEvents.current.RoofLevelFinished();
        }
    }
    public void addFloorCompleted()
    {
        totalFloorsCompleted += 1;
        if(totalFloorsCompleted == totalFloorstoComplete)
        {
            if(Globals.currentLevel == 1)
            {
                GameEvents.current.FirstFloorFinished();
                Debug.Log("Building finished");
            }
            if (Globals.currentLevel == 2)
            {
                GameEvents.current.SecondFloorFinished();
                Debug.Log("SecondFloor finished");
            }
        }
    }
}
