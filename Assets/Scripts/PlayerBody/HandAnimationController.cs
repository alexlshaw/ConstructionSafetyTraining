using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class HandAnimationController : MonoBehaviour
{
    //public GameObject controller;

    public XRNode xrNode;
    public int offsetAngle;

    public Material handMaterial;
    public Material glovesMaterial;
    public GameObject handsModel;

    private InputDevice inputDevice;
    private Animator handAnimator;


    // Start is called before the first frame update
    void Start()
    {
        transform.localRotation = Quaternion.Euler(0, 0, offsetAngle);
        handAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!inputDevice.isValid)
        {
            inputDevice = InputDevices.GetDeviceAtXRNode(xrNode);

        }
        else
        {
            UpdateHandAnimation();
            updateGloves();
        }
    }

    void UpdateHandAnimation()
    {
        if(inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);

        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }
        if (inputDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }
    }

    void updateGloves()
    {
        if (Globals.hasGloves)
        {
            handsModel.GetComponentInChildren<SkinnedMeshRenderer>().material = glovesMaterial;
        }
        else
        {
            handsModel.GetComponentInChildren<SkinnedMeshRenderer>().material = handMaterial;

        }
    }
}
