using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignScript : MonoBehaviour
{
    public TMPro.TMP_Text text;
    public void Start()
    {
        GameObject[] cranes = GameObject.FindGameObjectsWithTag("Crane");
        GameObject[] stationary = GameObject.FindGameObjectsWithTag("StationaryVehicle");
        GameObject[] moving = GameObject.FindGameObjectsWithTag("MovingVehicle");
        string itemList = "";
        if (cranes.Length > 0) { itemList += "Crane(s)\n"; }
        if (stationary.Length > 0) { itemList += "Stationary vehicles\n"; }
        if (moving.Length > 0) { itemList += "Moving vehicles\n"; }
        text.text = string.Format("Site has:\n{0}\nPlease be careful!", itemList);
    }
}
