using System.Collections.Generic;
using UnityEngine;

public static class Globals
{
    //Overall level
    public static int currentLevel = 1;

    //Building
    public static int buildingFloors;
    public static int buildingWidth;
    public static int buildingLength;
    public static int maxIterations = 2;

    public static int plankLong = 0;
    public static int plankShortTop;
    public static int plankShortBottom;
    public static int windowFrame;

    public static int plasterWindow;
    public static int plasterDoor;
    public static int plasterNormal;

    public static int plankRoof;
    public static int roofTile;

    public static bool isDriving = false;
    public static Dictionary<Vector2Int, List<string>> vectorToLabelDict = new Dictionary<Vector2Int, List<string>>();
    public static Dictionary<string, List<Vector2Int>> labelToVectorDict = new Dictionary<string, List<Vector2Int>>();

    public static bool hasHat = false;
    public static bool hasGlasses = false;
    public static bool hasMask = false;
    public static bool hasVest = false;
    public static bool hasGloves = false;
    public static bool hasBoots = false;
    public static bool hasHarness = false;
    public static float miniMapSize = 15;

    public static bool canSlide = true;


    public static List<Vector3> foundationLocations;
    public static Dictionary<string, Task> tasks;

    public static List<Task> getCurrentTasks()
    {
        List<Task> currentTasks = new List<Task>();
        foreach (string key in tasks.Keys)
        {
            currentTasks.Add(tasks[key]);
        }
        return currentTasks;
    }
    public static void calculateBuildingSize()
    {
        buildingWidth = Random.Range(3, 5);
        buildingLength = Random.Range(3, 5);
        buildingFloors = Random.Range(3, 4);
    }
    public static void addTagToPosition(Vector2Int position, string tag)
    {
        if (vectorToLabelDict.ContainsKey(position) && !vectorToLabelDict[position].Contains(tag))
        {
            vectorToLabelDict[position].Add(tag);
        }
        else
        {
            vectorToLabelDict[position] = new List<string>() { tag };
        }

        if (labelToVectorDict.ContainsKey(tag))
        {
            labelToVectorDict[tag].Add(position);
        }
        else
        {
            labelToVectorDict[tag] = new List<Vector2Int>() { position };
        }
    }

    public static bool positionIsTaken(Vector2Int position, string label)
    {
        return vectorToLabelDict[position].Contains(label);
    }



}
