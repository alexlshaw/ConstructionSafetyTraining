using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private float moveX;
    private float moveY;
    private float lookX;
    private float lookY;
    private bool dashing;
    [Header("Camera Attributes")]
    public float turnXSpeed = 1f;
    public float turnYSpeed = 1f;
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
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
        lookX = Input.GetAxis("Mouse X");
        lookY = Input.GetAxis("Mouse Y");
        if (Input.GetButtonDown("Jump") && rb && isGrounded) { rb.AddForce(0f, jumpForce, 0f, ForceMode.Impulse); }

        moveDirection = Camera.main.transform.TransformDirection(new Vector3(moveX, 0f, moveY)).normalized;
        moveDirection.y = 0f;
        cameraFunc();
        zoneTrace();
        base.Update();
    }

    void cameraFunc()
    {
        if (moveDirection.magnitude != 0)
        {
            bobOffset += 0.00075f * multiplier;
            if (bobOffset > 0.075 || bobOffset < -0.075) { multiplier *= -1; }
        }
        else
        {
            bobOffset = 0;
        }
        camX += (lookX * turnXSpeed);
        camY += (-lookY * turnYSpeed) + bobOffset;
        camY = Mathf.Clamp(camY, -90f, 90f);
        Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, Quaternion.Euler(camY, camX, 0f), Time.deltaTime * interpSpeed);
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
