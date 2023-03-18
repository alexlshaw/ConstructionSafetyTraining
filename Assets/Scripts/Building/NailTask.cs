using UnityEngine;

public class NailTask : MonoBehaviour
{
    [SerializeField]
    GameObject nail;
    [SerializeField]
    ParticleSystem sparks;

    NailTaskManager nailTaskManager;

    bool finished;

    private void Awake()
    {
        finished = false;
        nailTaskManager = GetComponentInParent<NailTaskManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onResetLevel += reset;
        ParticleSystem.EmissionModule em = sparks.emission;
        em.enabled = false;
    }

    private void moveNail(float distance)
    {
        if (!finished)
        {
            Debug.Log(distance);
            nail.transform.localPosition = nail.transform.localPosition + new Vector3(distance, 0, 0);
            if (nail.transform.localPosition.x < -0.12f)
            {
                nail.transform.localPosition = new Vector3(-0.12f, 0, 0);
                nailTaskManager.addCompleted();

                finished = true; 
                Debug.Log("NailFinished");

            }
            if (!Globals.hasGloves)
            {
                GameEvents.current.TakeDamage();
            }
        }

    }
    
    private void reset()
    {
        nail.transform.localPosition = Vector3.zero;
        ParticleSystem.EmissionModule em = sparks.emission;
        em.enabled = false;
        finished = false;
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.name == "Hammer")
        {
            ParticleSystem.EmissionModule em = sparks.emission;
            em.enabled = true;
            if (Vector3.Project(collision.gameObject.GetComponent<Rigidbody>().velocity, -transform.right).magnitude > 0.7)
            {
                moveNail(-0.02f);

            }
            else if (Vector3.Project(collision.gameObject.GetComponent<Rigidbody>().velocity, -transform.right).magnitude > 0.3)
            {
                moveNail(-0.01f);

            }
            
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        ParticleSystem.EmissionModule em = sparks.emission;
        em.enabled = false;
    }
}
