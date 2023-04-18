using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    void Awake()
    {
        current = this;
    }

    public event Action onSetOriginalPositions;
    public event Action onResetLevel;
    public event Action onResetVehiclePosition;
    public event Action onResetPlayerPosition;

    public event Action onVehicleTipped;
    public event Action onUpdateTaskList;
    public event Action onUpdateEquipment;

    public event Action<Vector3> onSetSiteNavLocation;

    public event Action onStartGame;
    public event Action onCompleteDigging;
    public event Action onFoundationFilled;
    public event Action onFirstFloorFinished;
    public event Action onStartSecondLevel;
    public event Action onSecondLevelFinished;
    public event Action onStartRoofLevel;
    public event Action onRoofPlanksComplete;
    public event Action onRoofLevelFinished;
    public event Action onGameOver;

    public event Action onNailsLevel;
    public event Action onTakeDamage;

    public event Action onUpdateFloorCount;
    public void TakeDamage()
    {
        onTakeDamage?.Invoke();
    }
    public void RoofPlanksFinished()
    {
        onRoofPlanksComplete?.Invoke();
    }
    public void StartRoofLevel()
    {
        onStartRoofLevel?.Invoke();
    }
    public void RoofLevelFinished()
    {
        onRoofLevelFinished?.Invoke();
    }
    public void SecondFloorFinished()
    {
        onSecondLevelFinished?.Invoke();
    }
    public void FirstFloorFinished()
    {
        onFirstFloorFinished?.Invoke();
    }
    public void SetSiteNavLocation(Vector3 location)
    {
        onSetSiteNavLocation?.Invoke(location);
    }
    public void UpdateEquipment()
    {
        onUpdateEquipment?.Invoke();
    }
    public void UpdateTaskList()
    {
        onUpdateTaskList?.Invoke();
    }
    public void NailsLevel()
    {
        onNailsLevel?.Invoke();
    }
    public void StartSecondLevel()
    {
        onStartSecondLevel?.Invoke();
    }
    public void UpdateFloorCount()
    {
        onUpdateFloorCount?.Invoke();
    }
    public void ResetPlayerPositon()
    {
        onResetPlayerPosition.Invoke();
    }
    public void ResetVehiclePosition()
    {
        onResetVehiclePosition?.Invoke();
    }
    public void VehicleTipped()
    {
        onVehicleTipped?.Invoke();
    }
    public void CompleteDigging()
    {
        onCompleteDigging?.Invoke();
    }

    public void FoundationFilled()
    {
        onFoundationFilled?.Invoke();
    }

    public void StartGame()
    {
        onStartGame?.Invoke();
    }

    public void GameOver()
    {
        onGameOver?.Invoke();
    }

    public void SetOriginalPosition()
    {
        onSetOriginalPositions?.Invoke();
    }

    public void ResetLevel()
    {
        onResetLevel?.Invoke();
    }

}
