using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    public static string currentState;


    private void Start()
    {
        GameEvents.current.onCompleteDigging += completeDigging;
        GameEvents.current.onFoundationFilled += completeFoundation;
        GameEvents.current.onStartSecondLevel += loadSecondLevel;
        GameEvents.current.onStartRoofLevel += loadRoofLevel;
        GameEvents.current.onRoofPlanksComplete += roofPlanks;
        GameEvents.current.onResetLevel += reset;
        currentState = "Start";
    }

    void completeDigging()
    {
        currentState = "Digging";
    }

    void completeFoundation()
    {
        currentState = "Foundation";
    }

    void loadFirstLevel()
    {
        currentState = "FirstLevel";
    }
    void loadSecondLevel()
    {
        Globals.currentLevel = 2;
        currentState = "Plaster";
    }
    void loadRoofLevel()
    {
        Globals.currentLevel = 3;
        currentState = "RoofPlank";
    }
    void roofPlanks()
    {
        currentState = "RoofTile";

    }

    private void reset()
    {
        if (Globals.currentLevel == 1)
        {
            currentState = "Digging";
        }
        if (Globals.currentLevel == 2)
        {
            currentState = "Plaster";
        }
        if (Globals.currentLevel == 3)
        {
            currentState = "RoofPlank";
        }
    }
}
