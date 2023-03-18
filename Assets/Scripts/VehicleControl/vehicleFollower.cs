using UnityEngine;

public class vehicleFollower : MonoBehaviour
{
    public GameObject vehicle;
    void Update()
    {
        if (vehicle != null)
        {
            transform.position = vehicle.transform.position;
            transform.rotation = vehicle.transform.rotation;
        }
    }
}
