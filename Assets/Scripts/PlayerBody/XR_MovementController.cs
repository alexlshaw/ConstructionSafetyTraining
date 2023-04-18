using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XR_MovementController : MonoBehaviour
{
    public XRNode inputSource;
    private InputDevice device;
    public LayerMask CeilingLayer;
    public float jumpHeight;
    public bool isJumping = false;
    private bool lastState = false;
    public float jumpSpeed = 8.0f;
    public float additionalHeight = 0.2f;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float slidingSpeed = 0.2f;
    [SerializeField] private Vector3 hitPointNormal;
    [SerializeField]
    private bool canSlide;

    [SerializeField] private bool isSliding;

    bool determineSliding()
    {
        
            if(CheckIfGrounded() && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 1f))
            {
                hitPointNormal = slopeHit.normal;
                return slopeHit.collider.gameObject.CompareTag("Roof");
            }
            else
            {
                return false;
            }
        
    }

    private XRRig rig;
    private CharacterController character;
    private ContinuousMoveProviderBase moveProvider;

    private int currentZone;
    public bool isGrounded;
    void Start()
    {
        moveProvider = GetComponent<ContinuousMoveProviderBase>();
        character = GetComponent<CharacterController>();
        rig = GetComponent<XRRig>();
        InitController();
        character.detectCollisions = false;

    }

    void InitController()
    {
        device = InputDevices.GetDeviceAtXRNode(inputSource);
    }

    private float jumpstartpos;
    private float startFallPos;
    private float endFallPos;
    // Update is called once per frame
    void FixedUpdate()
    {
        CapsuleFollowHeadset();
        zoneTrace();
        if (!device.isValid)
            InitController();
        if(CheckIfGrounded() == false && isGrounded) //different from last state
        {
            Debug.Log("Start jump");
            startFallPos = character.transform.position.y;
        }
        if (CheckIfGrounded() == true && !isGrounded) //different from last state
        {
            Debug.Log("end jump");

            endFallPos = character.transform.position.y;
            float distanceFallen = startFallPos - endFallPos;
            Debug.Log(distanceFallen);
            if(distanceFallen > 1.5)
            {
                PointsLostHandler.falls += 1;
                Debug.Log("take damage");
                GameEvents.current.TakeDamage();
            }
        }
        isGrounded = CheckIfGrounded();

        if (SecondaryButtonDown() & CheckIfGrounded())
        {
            if (isJumping == false)
            {
                moveProvider.useGravity = false;
                jumpstartpos = character.transform.position.y;
            }
            isJumping = true;

            character.slopeLimit = 90; // fixes the jump directly in fornt of thing while pressing forward. has to be reset to 45 when isJumping = false
        }

        if (isJumping)
            Jump();

        if (HitCeiling())
            isJumping = false;


        isSliding = determineSliding();
        canSlide = Globals.canSlide;
        if (Globals.canSlide && isSliding)
        {
            Vector3 moveDirection = new Vector3(hitPointNormal.x, 0, hitPointNormal.z) * slidingSpeed;
            transform.position += moveDirection;
        }
    }

    void zoneTrace()
    {
        RaycastHit hit;
        bool ray = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity);
        if (ray)
        {
            LayerMask hitLayer = hit.transform.gameObject.layer;
            if (hitLayer == LayerMask.NameToLayer("Approach") && hitLayer.value != currentZone)
            {
                Debug.Log("Approach");
                currentZone = hitLayer.value;
            }
            else if (hitLayer == LayerMask.NameToLayer("Restricted") && hitLayer.value != currentZone)
            {
                Debug.Log("Restricted");
                currentZone = hitLayer.value;
            }
            else if (hitLayer == LayerMask.NameToLayer("NoGo") && hitLayer.value != currentZone)
            {
                Debug.Log("NoGo");
                currentZone = hitLayer.value;
                PointsLostHandler.other += 1;
            }
            else
            {
                currentZone = hitLayer.value;
            }
        }
    }
    private bool CheckIfGrounded()
    {
        Vector3 rayStart = transform.TransformPoint(character.center);
        float rayLength = 1.8f + 0.1f;
        
        bool hasHit = Physics.SphereCast(rayStart, character.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
        //if (hasHit)
        //{
        //    Debug.Log(hitInfo.collider.gameObject.name);
        //}
        return hasHit;

    }

    public bool HitCeiling()
    {
        Vector3 rayStart = transform.TransformPoint(character.center);
        float rayLength = character.center.y + 0.1f;
        bool hasHit = Physics.SphereCast(rayStart, character.radius, Vector3.up, out RaycastHit hitInfo, rayLength, CeilingLayer);
        return hasHit;
    }

    private void Jump()
    {
        if (!CheckIfGrounded() && character.transform.position.y >= jumpstartpos + jumpHeight - 0.1)
        {
            isJumping = false;
            moveProvider.useGravity = true;
            character.slopeLimit = 60;
        }
        character.Move(Vector3.up * jumpSpeed * Time.smoothDeltaTime);

    }



    public bool SecondaryButtonDown()
    {
        if (device.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButtonValue) && secondaryButtonValue)
        {
            bool tempStatePrimary = secondaryButtonValue;

            if (tempStatePrimary != lastState)  //Button Down
            {
                lastState = tempStatePrimary;
                return true;
            }
        }
        else
        {
            lastState = false;
        }
        return false;
    }

    void CapsuleFollowHeadset()
    {
        character.height = rig.cameraInRigSpaceHeight + additionalHeight;
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);
        character.center = new Vector3(capsuleCenter.x, character.height / 2 + character.skinWidth, capsuleCenter.z);
    }

}