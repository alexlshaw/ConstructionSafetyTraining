using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ForkLiftController : MonoBehaviour
{
    [SerializeField] private GameObject fork;
    [SerializeField] private float speed;
    [SerializeField] private GameObject lever;

    private float maxHeight = 2;
    private float minHeight = -0.1f;
    private HingeJoint leverHingeJoint;
    private XRGrabInteractable xRGrabInteractable;

    public GameObject instructionUI;

    public bool instructionsShown = false;

    private void Start()
    {
        leverHingeJoint = lever.GetComponent<HingeJoint>();
        xRGrabInteractable = lever.GetComponent<XRGrabInteractable>();
    }
    private void Update()
    {
        if (!xRGrabInteractable.isSelected)
        {
            lever.transform.localRotation = Quaternion.Euler(0, 0, 0);
            lever.transform.localPosition = new Vector3(0.00608999934f, 0.00458292523f, 0.00427970383f);
        }
        else
        {
            Debug.Log(leverHingeJoint.angle);
            if(fork.transform.localPosition.y< minHeight)
            {
                fork.transform.localPosition = new Vector3(fork.transform.localPosition.x, minHeight, fork.transform.localPosition.z);
            }
            if (fork.transform.localPosition.y > maxHeight)
            {
                fork.transform.localPosition = new Vector3(fork.transform.localPosition.x, maxHeight, fork.transform.localPosition.z);
            }
            if ((leverHingeJoint.angle > 10 || Input.GetKey(KeyCode.U)) && fork.transform.localPosition.y <= maxHeight)
            {
                fork.transform.position += Vector3.up * speed * Time.deltaTime;
            }
            if ((leverHingeJoint.angle < -10 || Input.GetKey(KeyCode.I)) && fork.transform.localPosition.y >= minHeight)
            {

                fork.transform.position += Vector3.down * speed * Time.deltaTime;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!instructionsShown && other.gameObject.tag.Equals("Player"))
        {
            instructionsShown = true;
            instructionUI.SetActive(true);
        }
    }
}
