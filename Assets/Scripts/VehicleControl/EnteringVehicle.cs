using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EnteringVehicle : MonoBehaviour
{
    public GameObject vehicle;
    public GameObject objectToTrack;
    public float cameraOffset;

    private GameObject player;
    private VehicleController vehicleController;
    private Vector3 originalPosition;
    private GameObject vehicleTracker;
    private XRRig xrRig;
    // Start is called before the first frame update
    private void Start()
    {
        vehicleController = vehicle.GetComponentInChildren<VehicleController>();
        player = GameObject.FindGameObjectWithTag("Player");
        xrRig = player.GetComponentInChildren<XRRig>();
        vehicleTracker = GameObject.Find("VehicleFollower");
    }
    public void toggleVehicle()
    {
        vehicleController.isDriving = !vehicleController.isDriving;
        if (vehicleController.isDriving)
        {
            //vehicleController.stopDriving();
            vehicleTracker.transform.position = objectToTrack.transform.position;
            vehicleTracker.GetComponent<vehicleFollower>().vehicle = objectToTrack;

            player.transform.parent = vehicleTracker.transform;
            originalPosition = player.transform.localPosition;
            player.GetComponent<PlayerVehicleManager>().isDriving = true;
            //player.transform.localPosition = new Vector3(0, 0, 0);
            player.transform.localRotation = Quaternion.identity;
            Vector3 relativePosition = xrRig.cameraGameObject.transform.position - objectToTrack.transform.position;
            Debug.Log(objectToTrack.transform.position);
            Debug.Log(xrRig.cameraGameObject.transform.position);

            Debug.Log(relativePosition);
            player.transform.position = player.transform.position - relativePosition;

            //xrRig.requestedTrackingOriginMode = XRRig.TrackingOriginMode.Device;
            //xrRig.cameraYOffset = 0;

        }
        else
        {
            //vehicleController.startDriving();
            player.GetComponent<PlayerVehicleManager>().isDriving = false;
            player.transform.localPosition = originalPosition;
            player.transform.localRotation = Quaternion.Euler(transform.parent.right);
            player.transform.parent = null;
            xrRig.requestedTrackingOriginMode = XRRig.TrackingOriginMode.Floor;

        }

    }

    public void Exit()
    {
        Debug.Log("Exit");
    }
}
