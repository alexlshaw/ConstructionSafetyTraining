using UnityEngine;

public class ConcreteMixer : MonoBehaviour
{
    public GameObject concrete;
    public GameObject particles;
    public GameObject wheel;

    float fill;
    float empty;
    public bool foundationFilled;
    public bool concreteMixerOn;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    ParticleSystem myParticleSystem;
    public GameObject instructionUI;

    public bool instructionsShown = false;

    private float timeElapsed;
    private float damageTimer = 5;

    // Start is called before the first frame update
    void Start()
    {
        myParticleSystem = particles.GetComponent<ParticleSystem>();
        myParticleSystem.Clear();

        concreteMixerOn = false;
        foundationFilled = false;
        fill = 0;
        empty = 0;

        instructionUI.SetActive(false);
        GameEvents.current.onStartGame += setOriginalPosition;
        GameEvents.current.onResetLevel += reset;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        checkParticleAngle();
        rotateHorizontally();
        checkFilled();
        transform.eulerAngles = new Vector3(wheel.transform.eulerAngles.x, wheel.transform.eulerAngles.y, transform.eulerAngles.z);
    }

    private void checkFilled()
    {
        if (fill > 1)
        {
            foundationFilled = true;
            fill = 0;
        }
        if (foundationFilled)
        {
            fill = 0;
            concrete.SetActive(true);
            concreteMixerOn = true;
        }
        if (empty > 1)
        {
            foundationFilled = false;
            concrete.SetActive(false);
            concreteMixerOn = false;
            empty = 0;
        }
    }

    private void rotateHorizontally()
    {
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z);
        if (concreteMixerOn)
        {
            transform.RotateAround(transform.position, transform.forward, Time.deltaTime * 10f);
        }
    }

    void reset()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        myParticleSystem = particles.GetComponent<ParticleSystem>();
        myParticleSystem.Clear();
        ParticleSystem.EmissionModule em = myParticleSystem.emission;
        em.enabled = false;

        concreteMixerOn = false;
        foundationFilled = false;
        fill = 0;
        empty = 0;
    }
    void setOriginalPosition()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }
    private void checkParticleAngle()
    {
        if (Vector3.Angle(Vector3.down, particles.transform.forward) <= 90f && foundationFilled)
        {
            ParticleSystem.EmissionModule em = myParticleSystem.emission;
            em.enabled = true;
            empty += 0.0005f;
            if (!Globals.hasMask || !Globals.hasGloves || !Globals.hasGlasses)
            {
                timeElapsed += Time.deltaTime;
                if (timeElapsed >= damageTimer)
                {
                    PointsLostHandler.dust += 0.5f;
                    GameEvents.current.TakeDamage();
                }
            }
        }
        else
        {
            ParticleSystem.EmissionModule em = myParticleSystem.emission;
            em.enabled = false;
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.name.Equals("BagConcreteParticles") && fill < 1)
        {
            fill += 0.005f;
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
