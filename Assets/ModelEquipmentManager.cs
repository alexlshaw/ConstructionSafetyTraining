using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ModelEquipmentManager: MonoBehaviour
{
    public void EquipHelmet()
    {
        Debug.Log("exit");
        Globals.hasHat = true;
    }
    public void UnequipHelmet()
    {
        Globals.hasHat = false;
        Debug.Log("enter");
    }

    public void EquipGlasses()
    {
        Globals.hasGlasses = true;
    }
    public void UnequipGlasses()
    {
        Globals.hasGlasses = false;
    }

    public void EquipMask()
    {
        Globals.hasMask = true;
    } 
    public void UnequipMask()
    {
        Globals.hasMask = false;
    }

    public void EquipVest()
    {
        Globals.hasVest = true;
    } 
    public void UnequipVest()
    {
        Globals.hasVest = false;
    }

    public void EquipGloves()
    {
        Globals.hasGloves = true;
    }
    public void UnequipGloves()
    {
        Globals.hasGloves = false;
    }

    public void EquipBoots()
    {
        Globals.hasBoots = true;
    }
    public void UnequipBoots()
    {
        Globals.hasBoots = false;
    }

}
