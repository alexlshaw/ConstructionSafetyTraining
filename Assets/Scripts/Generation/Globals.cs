using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals
{
    public static bool isDriving = false;
    public static Dictionary<Vector2Int, List<string>> vectorToLabelDict = new Dictionary<Vector2Int, List<string>>();
    public static Dictionary<string, List<Vector2Int>> labelToVectorDict = new Dictionary<string, List<Vector2Int>>();

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
