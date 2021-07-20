﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class Player : Character
{
    [Header("Camera Attributes")]
    public float turnXSpeed = 0.1f;
    public float turnYSpeed = 0.1f;
    public float interpSpeed = 5f;
    [Header("VR Attributes")]
    public bool VR = false;
    public TrackedPoseDriver tpd;

    private float camY;
    private float camX;
    private int currentZone;
    private int multiplier = 1;
    private float bobOffset = 0;
    private Vector2 inputVecCam;
    private Quaternion inputVecCamHMD;
    private Vector2 inputVecMove;

    protected override void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (VR) { tpd.enabled = true; }
        base.Start();
    }
    protected override void Update()
    {
        zoneTrace();
        
        moveFunc();
        base.Update();
    }
    void LateUpdate()
    {
        camFunc();
    }
    public void OnJump()
    {
        if (rb && isGrounded)
        {
            rb.AddForce(0f, jumpForce, 0f, ForceMode.Impulse);
        }
    }
    public void OnMoveKB(InputValue input)
    {
        inputVecMove = input.Get<Vector2>();
    }
    public void OnMove(InputValue input)
    {
        Debug.Log("Controller input detected");
        inputVecMove = input.Get<Vector2>();
    }
    void moveFunc(){
        moveDirection = Camera.main.transform.TransformDirection(new Vector3(inputVecMove.x, 0f, inputVecMove.y));
        moveDirection.y = 0f;
        moveDirection.Normalize();
    }

    public void OnLook(InputValue input){
        inputVecCam = input.Get<Vector2>();
    }
    void camFunc(){
        if (moveDirection.magnitude != 0 && isGrounded)
        {
            bobOffset += 0.025f * multiplier;
            if (bobOffset > 0 || bobOffset < -0.25) { multiplier *= -1; }
        }
        else
        {
            bobOffset = 0;
        }
        camX += (inputVecCam.x * turnXSpeed);
        camY += (-inputVecCam.y * turnYSpeed);
        camY = Mathf.Clamp(camY, -90f, 90f);
        if (!VR)
        {
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, Quaternion.Euler(camY, camX, 0f), Time.deltaTime * interpSpeed);
        }
        Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, new Vector3(0, 1 + bobOffset, 0), Time.deltaTime * interpSpeed);
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
            }
            else
            {
                currentZone = hitLayer.value;
            }
        }
    }
}