using System.Linq;
using UnityEngine;

public class RoofController : MonoBehaviour
{
    public float tileChance;
    public float socketChance;

    public float totalItemsToFill;

    RoofPlankController[] totalPlanks;
    RoofTileController[] totalTiles;

    BuildingTaskManager buildingManager;

    public int totalPlanksToFill;
    public int totalPlanksFilled;

    public int totalTilesToFill;
    public int totalTilessFilled;

    public GameObject Tiles;

    bool allFilled = false;
    // Start is called before the first frame update

    void Start()
    {
        buildingManager = GetComponentInParent<BuildingTaskManager>();

        totalItemsToFill = GetComponentsInChildren<SocketWithNameCheck>().GetLength(0);
        Debug.Log("totalItemsToFill" + totalItemsToFill);
        GameEvents.current.onStartSecondLevel += DeActivate;
        GameEvents.current.onStartRoofLevel += Initialise;
        GameEvents.current.onStartRoofLevel += Activate;
        GameEvents.current.onResetLevel += reset;

    }

    void Initialise()
    {
        totalPlanks = GetComponentsInChildren<RoofPlankController>();
        foreach(RoofPlankController plank in totalPlanks)
        {
            plank.Initialise();
        }
        totalPlanksToFill = totalPlanks.Where(plank => plank.isSocket).ToList().Count;
        if (LevelController.currentState.Equals("RoofPlank"))
        {
            checkIfPlanksFilled();
        }
        
    }

    // Update is called once per frame
    void checkIfPlanksFilled()
    {
        Debug.Log("planks"+totalPlanksFilled+"/"+totalPlanksToFill);
        if (totalPlanksFilled == totalPlanksToFill)
        {
            buildingManager.addRoofPlankCompleted();
        }
    }
    void checkIfTilesFilled()
    {
        Debug.Log("tiles" + totalTilessFilled + "/" + totalTilesToFill);

        if (totalTilesToFill == totalTilessFilled)
        {
            buildingManager.addRoofTilesCompleted();
        }
    }
    public void addPlank()
    {
        totalPlanksFilled += 1;
        checkIfPlanksFilled();
    }
    public void removePlank()
    {
        totalPlanksFilled -= 1;
    }
    public void addTile()
    {
        totalTilessFilled += 1;
        checkIfTilesFilled();
    }
    public void removeTile()
    {
        totalTilessFilled -= 1;
    }
    public void Activate()
    {
        gameObject.SetActive(true);
    }
    public void DeActivate()
    {
        gameObject.SetActive(false);
    }

    void reset()
    {
        totalPlanksFilled = 0;
        totalTilessFilled = 0;
    }

    public void setSliding()
    {
        Globals.canSlide = true;
    }
    public void setNotSliding()
    {
        Globals.canSlide = false;
    }
}
