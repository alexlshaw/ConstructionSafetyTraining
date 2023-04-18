using System;
using UnityEngine;
using UnityEngine.XR;

public class VehicleController : MonoBehaviour
{
    public bool isDriving = false;
    public Transform steeringWheel;
    [SerializeField]
    private float maxSpeed;

    private InputDevice leftController;
    private InputDevice rightController;

    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private bool isBraking;
    private float currentBrakeForce;
    float maxTurnAngle = 45;

    [SerializeField] private float motorForce;
    [SerializeField] private float brakeForce;
    [SerializeField] private float maxSteeringAngle;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider backLeftWheelCollider;
    [SerializeField] private WheelCollider backRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform backLeftWheelTransform;
    [SerializeField] private Transform backRightWheelTransform;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private GameObject vehicleReset;
    private Rigidbody RBody;

    bool tipped = false;
    // Update is called once per frame
    void Awake()
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            for (int j = i; j < colliders.Length; j++)
            {
                Physics.IgnoreCollision(colliders[i], colliders[j]);
            }
        }
    }


    void Start()
    {
        RBody = GetComponent<Rigidbody>();
        RBody.centerOfMass += new Vector3(0, -1f, 0);

        GameEvents.current.onCompleteDigging += setToVehicleResetPosition;
        GameEvents.current.onCompleteDigging += stopDriving;
        GameEvents.current.onFoundationFilled += setToVehicleResetPosition;
        GameEvents.current.onFoundationFilled += stopDriving;
        GameEvents.current.onSecondLevelFinished += reset;
        GameEvents.current.onSecondLevelFinished += stopDriving;
        GameEvents.current.onResetVehiclePosition += reset;
        GameEvents.current.onResetPlayerPosition += stopDriving;
        GameEvents.current.onResetLevel += reset;

        vehicleReset = GameObject.Find("VehicleReset(Clone)");
    }

    private void Update()
    {
        GetInput();

        if (isDriving && ((transform.eulerAngles.x > 25 && transform.eulerAngles.x < 335) || (Math.Abs(transform.eulerAngles.z) > 25 && transform.eulerAngles.z < 335)))
        {
            RBody.isKinematic = true;
            tipped = true;
        }
        else
        {
            RBody.isKinematic = false;
        }
        if (tipped)
        {
            GameEvents.current.VehicleTipped();
            PointsLostHandler.tips += 1;
            tipped = false;
        }
    }
    void reset()
    {

        isDriving = false;
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }

    private void setToVehicleResetPosition()
    {
        if (isDriving)
        {
            transform.position = vehicleReset.transform.position;
            transform.rotation = vehicleReset.transform.rotation;
        }
    }

    public void stopDriving()
    {
        isDriving = false;
        //RBody.isKinematic = true;

    }
    public void startDriving()
    {
        //RBody.isKinematic = false;

        isDriving = true;

    }

    void FixedUpdate()
    {

        if (!leftController.isValid || !rightController.isValid)
        {
            leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        }
        if (isDriving)
        {

            if (steeringWheel != null)
            {
                Vector3 originalRotation = steeringWheel.localEulerAngles;
                Vector3 modified = Vector3.up * Mathf.Clamp((Input.GetAxis("Horizontal") * 100), -maxTurnAngle, maxTurnAngle);
                steeringWheel.localEulerAngles = new Vector3(originalRotation.x, modified.y, originalRotation.z);

            }
            HandleMotor();
            HandleSteering();
            UpdateWheels();
        }
        else
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        bool primaryButtonValue;

        isBraking = (Input.GetKey(KeyCode.Space) || (rightController.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonValue) && primaryButtonValue));
    }
    private void HandleMotor()
    {
        if (RBody.velocity.sqrMagnitude < maxSpeed)
        {
            frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
            frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        }
        else
        {
            frontLeftWheelCollider.motorTorque = 0;
            frontRightWheelCollider.motorTorque = 0;
        }
        currentBrakeForce = isBraking ? brakeForce : 0f;
        applyBraking();
    }
    private void HandleSteering()
    {
        currentSteerAngle = maxSteeringAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }
    private void applyBraking()
    {
        frontLeftWheelCollider.brakeTorque = currentBrakeForce;
        frontRightWheelCollider.brakeTorque = currentBrakeForce;
        backLeftWheelCollider.brakeTorque = currentBrakeForce;
        backRightWheelCollider.brakeTorque = currentBrakeForce;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(backLeftWheelCollider, backLeftWheelTransform);
        UpdateSingleWheel(backRightWheelCollider, backRightWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}
