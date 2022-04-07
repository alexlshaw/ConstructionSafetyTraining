using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ButtonControllerScript : MonoBehaviour
{
    public GameObject menu; // Assign in inspector

    private bool isShowing;

    private InputDevice leftController;
    private InputDevice rightController;
    bool leftControllerLastState = false;
    bool rightControllerLastState = false;



    void Update()
    {
        if (!leftController.isValid || !rightController.isValid)
        {
            leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        }
        else
        {
            bool primaryButtonValue;

            if (leftController.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonValue) && primaryButtonValue)
            {
                if (primaryButtonValue != leftControllerLastState)
                {
                    isShowing = !isShowing;
                    menu.SetActive(isShowing);
                }

            }
            leftControllerLastState = primaryButtonValue;

        }
    }
}
