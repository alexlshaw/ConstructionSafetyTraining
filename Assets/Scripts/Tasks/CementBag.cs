using UnityEngine;

public class CementBag : MonoBehaviour
{
    public GameObject particles;
    ParticleSystem myParticleSystem;
    public bool selected = false;

    Transform originalParent;
    Vector3 originalLocalPosition;
    Quaternion originalLocalRotation;

    // Start is called before the first frame update
    void Start()
    {
        originalParent = transform.parent;

        myParticleSystem = particles.GetComponent<ParticleSystem>();
        myParticleSystem.Clear();
        originalLocalPosition = transform.localPosition;
        originalLocalRotation = transform.localRotation;

        GameEvents.current.onResetLevel += resetPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (selected)
        {
            if (Vector3.Angle(Vector3.down, particles.transform.forward) <= 90f)
            {
                ParticleSystem.EmissionModule em = myParticleSystem.emission;
                em.enabled = true;
            }
        }
        else
        {
            ParticleSystem.EmissionModule em = myParticleSystem.emission;
            em.enabled = false;
        }
        if (GetComponent<XRCustomGrabInteractable>().isSelected)
        {
            gameObject.layer = LayerMask.NameToLayer("GrabInteractables");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Default");

        }
    }

    public void resetPosition()
    {
        transform.SetParent(originalParent);
        transform.localRotation = originalLocalRotation;
        transform.localPosition = originalLocalPosition;
    }
    public void toggleSelected()
    {
        selected = !selected;
    }
}
