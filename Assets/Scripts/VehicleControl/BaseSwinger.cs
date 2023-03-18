using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BaseSwinger : MonoBehaviour
{
    public float torque;
    public Rigidbody rb;
    public GameObject lever;


    bool addingTorque;
    bool addingReverseTorque;
    private HingeJoint leverHingeJoint;
    private XRGrabInteractable xRGrabInteractable;
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        leverHingeJoint = lever.GetComponent<HingeJoint>();
        xRGrabInteractable = lever.GetComponent<XRGrabInteractable>();
    }

    private void Update()
    {
        if (!xRGrabInteractable.isSelected)
        {
            lever.transform.localRotation = Quaternion.identity;
            lever.transform.localPosition = new Vector3(0.242f, 0.77f, 0);
        }

        float angle = transform.localEulerAngles.y;
        angle = (angle > 180) ? angle - 360 : angle;
        if ((leverHingeJoint.angle < -10 || Input.GetKey(KeyCode.Keypad9)) && angle < 90)
        {
            transform.Rotate(Vector3.up, 20 * Time.deltaTime);
        }
        else if ((leverHingeJoint.angle > 10 || Input.GetKey(KeyCode.Keypad3)) && angle > -90)
        {
            transform.Rotate(Vector3.up, -20 * Time.deltaTime);
        }

    }
}
