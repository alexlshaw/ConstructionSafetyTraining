using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Character Attributes")]
    public float movementSpeed = 5f;
    public float jumpForce = 5f;
    [Header("Ground-check Attributes")]
    public LayerMask mask;
    public Transform position;

    protected Vector3 moveDirection;
    protected Rigidbody rb;
    protected bool isGrounded;
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    protected virtual void Update()
    {
        isGrounded = Physics.CheckSphere(position.position, 0.2f, mask);
    }
    protected virtual void FixedUpdate()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector3(moveDirection.x * movementSpeed, rb.velocity.y, moveDirection.z * movementSpeed);
        }
    }
}
