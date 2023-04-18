using UnityEngine;
using UnityEngine.XR;

public class ButtonControllerScript : MonoBehaviour
{
    public GameObject menu; // Assign in inspector

    private bool isShowing = false;

    private InputDevice leftController;
    private InputDevice rightController;
    bool leftControllerLastState = false;
    //bool rightControllerLastState = false;


    //private float jumpForce = 100;
    private CharacterController _controller;

    private void Start()
    {
        menu.SetActive(false);
        _controller = GetComponent<CharacterController>();


    }
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
                GameEvents.current.UpdateTaskList();
            }
            leftControllerLastState = primaryButtonValue;

        }
    }

}
