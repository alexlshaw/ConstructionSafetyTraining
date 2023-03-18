using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerVehicleManager : MonoBehaviour
{
    // Handles recentering of player model once in a vehicle, and also removes the body
    private bool lastDriving = false;
    public bool isDriving = false;
    public GameObject XRRig;
    public GameObject robotModel;
    public Animator animator;
    public Vector3 startPosition;
    public GameObject resetPositionObject;

    private int currentZone;
    public List<Collider> RagdollParts = new List<Collider>();

    private void Awake()
    {
        SetRagdollParts();
        GameEvents.current.onStartGame += setSiteNav;

    }
    private void Start()
    {
        startPosition = transform.position;
        resetPositionObject = GameObject.Find("PlayerReset(Clone)");
        GameEvents.current.onResetLevel += resetPosition;
        GameEvents.current.onCompleteDigging += resetPosition;
        GameEvents.current.onFoundationFilled += resetPosition;
        GameEvents.current.onStartSecondLevel += resetPosition;
        //StartCoroutine(die());
    }
    void setSiteNav()
    {
        GameEvents.current.SetSiteNavLocation(transform.position);
    }
    void Update()
    {
        if (lastDriving != isDriving)
        {
            lastDriving = isDriving;
            if (isDriving)
            {
                transform.localRotation = Quaternion.identity;
                robotModel.SetActive(false);
                XRRig.GetComponent<CharacterController>().enabled = false;
                XRRig.GetComponent<DeviceBasedContinuousMoveProvider>().enabled = false;
            }
            else
            {
                robotModel.SetActive(true);
                XRRig.GetComponent<CharacterController>().enabled = true;
                XRRig.GetComponent<DeviceBasedContinuousMoveProvider>().enabled = true;
            }
        }
        zoneTrace();
    }

    void SetRagdollParts()
    {
        Collider[] colliders = this.gameObject.GetComponentsInChildren<Collider>();
        foreach(Collider c in colliders)
        {
            if(c.gameObject != this.gameObject)
            {
                c.isTrigger = true;
                RagdollParts.Add(c);
            }
        }
    }
    //private IEnumerator die()
    //{
    //    yield return new WaitForSeconds(10);
    //    TurnOnRagDoll();
    //}
    void TurnOnRagDoll()
    {
        CharacterController characterController = GetComponent<CharacterController>();
        characterController.enabled = false;
        animator.enabled = false;
        foreach(Collider c in RagdollParts)
        {
            c.isTrigger = false;
        }
    }
    
    void resetPosition()
    {
        transform.SetParent(null);
        if (LevelController.currentState.Equals("Start"))
        {
            transform.position = startPosition;
            transform.rotation = Quaternion.identity;
        }
        else
        {
            transform.position = resetPositionObject.transform.position;
            transform.rotation = resetPositionObject.transform.rotation;
        }
        stopDriving();
    }
    public void stopDriving()
    {
        isDriving = false;
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
                PointsLostHandler.other += 30;
            }
            else
            {
                currentZone = hitLayer.value;
            }
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("AutoVehicle"))
        {
            Debug.Log(gameObject.GetInstanceID() + "Collision with player");
            PointsLostHandler.collisions += 1;
            GameEvents.current.TakeDamage();
        }
    }


}
