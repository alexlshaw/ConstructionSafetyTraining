using UnityEngine;

public class PalletPickUp : MonoBehaviour
{
    public bool entered = false;
    public bool collided = false;
    GameObject fork;
    Rigidbody thisRigidBody;
    Transform originalParent;


    Vector3 originalLocalPosition;
    Quaternion originalLocalRotation;
    // Start is called before the first frame update
    private void Start()
    {

        thisRigidBody = GetComponent<Rigidbody>();
        originalParent = transform.parent;

        originalLocalPosition = transform.localPosition;
        originalLocalRotation = transform.localRotation;

        GameEvents.current.onResetLevel += resetPosition;

    }
    // Update is called once per frame
    void Update()
    {
        if (GetComponent<XRCustomGrabInteractable>().isSelected)
        {
            gameObject.layer = LayerMask.NameToLayer("GrabInteractables");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
        if (entered && collided)
        {
            transform.parent = fork.transform;
            //thisRigidBody.isKinematic = true;
            Rigidbody[] childBodies = GetComponentsInChildren<Rigidbody>();
            foreach(Rigidbody rb in childBodies)
            {
                rb.isKinematic = true;
            }
        }
        else if (entered && !collided)
        {
            originalParent.transform.position = transform.position;
            transform.parent = originalParent;
            //thisRigidBody.isKinematic = false;
            Rigidbody[] childBodies = GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in childBodies)
            {
                rb.isKinematic = true;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Equals("Fork"))
        {
            collided = true;
            fork = collision.gameObject;
        }
        if (collision.gameObject.name.Equals("voxelGround"))
        {
            collided = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name.Equals("Fork"))
        {
            collided = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("Fork"))
        {
            entered = true;
            fork = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Equals("Fork"))
        {
            entered = false;
        }
    }

    public void resetPosition()
    {
        transform.SetParent(originalParent);
        transform.localRotation = originalLocalRotation;
        transform.localPosition = originalLocalPosition;
        entered = false;
        collided = false;
    }
}
