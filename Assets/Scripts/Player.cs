using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    [Header("Camera Attributes")]
    public float turnXSpeed = 0.1f;
    public float turnYSpeed = 0.1f;
    public float interpSpeed = 5f;
    [Header("Interaction Variables")]
    public Animator canvasAnimator;
    private AudioSource aud;

    private float camY;
    private float camX;
    private int currentZone;
    private int multiplier = 1;
    private float bobOffset = 0;

    protected override void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        aud = GetComponent<AudioSource>();
        base.Start();
    }
    protected override void Update()
    {
        zoneTrace();
        base.Update();
    }
    public void OnJump()
    {
        if (rb && isGrounded)
        {
            rb.AddForce(0f, jumpForce, 0f, ForceMode.Impulse);
        }
    }
    public void OnMove(InputValue input)
    {
        Vector2 inputVec = input.Get<Vector2>();
        moveDirection = Camera.main.transform.TransformDirection(new Vector3(inputVec.x, 0f, inputVec.y));
        moveDirection.y = 0f;
        moveDirection.Normalize();
    }

    public void OnLook(InputValue input)
    {
        Vector2 inputVec = input.Get<Vector2>();
        if (moveDirection.magnitude != 0)
        {
            bobOffset += 0.0025f * multiplier;
            if (bobOffset > 0 || bobOffset < -0.25) { multiplier *= -1; }
        }
        else
        {
            bobOffset = 0;
        }
        camX += (inputVec.x * turnXSpeed);
        camY += (-inputVec.y * turnYSpeed);
        camY = Mathf.Clamp(camY, -90f, 90f);
        Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, Quaternion.Euler(camY, camX, 0f), Time.deltaTime * interpSpeed);
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
                if (aud.isPlaying) { aud.Stop(); }
                aud.Play();
                canvasAnimator.SetTrigger("NoGo");
            }
            else
            {
                currentZone = hitLayer.value;
                
            }
        }
    }
}
