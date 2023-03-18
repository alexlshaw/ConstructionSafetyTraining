using UnityEngine;

public class Spill : MonoBehaviour
{
    ParticleSystem myParticleSystem;

    // Start is called before the first frame update
    void Start()
    {
        myParticleSystem = GetComponent<ParticleSystem>();
        myParticleSystem.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Angle(Vector3.down, transform.forward) <= 90f)
        {
            ParticleSystem.EmissionModule em = myParticleSystem.emission;
            em.enabled = true;
        }
        else
        {
            ParticleSystem.EmissionModule em = myParticleSystem.emission;
            em.enabled = false;
        }
    }


}
