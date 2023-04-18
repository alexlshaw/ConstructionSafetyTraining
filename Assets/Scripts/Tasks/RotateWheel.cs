using UnityEngine;
using UnityEngine.XR;

public class RotateWheel : MonoBehaviour
{
    private InputDevice leftController;
    private InputDevice rightController;
    private bool leftHandOnWheel = false;
    private bool rightHandOnWheel = false;

    public Transform[] snappPositions;
    private int numberOfHandsOnWheel = 0;
    public float currentWheelRotation = 0;
    private float turnDampening = 250;

    public Transform directionalObject;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!leftController.isValid || !rightController.isValid)
        {
            leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        }
        //else
        //{
        //    bool primaryButtonValue;

        //    if (leftController.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonValue) && primaryButtonValue)
        //    {
        //        transform.RotateAround(transform.position, transform.right, Time.deltaTime * 60f);
        //    }

        //    if (rightController.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonValue) && primaryButtonValue)
        //    {
        //        transform.RotateAround(transform.position, transform.right, -Time.deltaTime * 60f);
        //    }
        //}


    }
}
