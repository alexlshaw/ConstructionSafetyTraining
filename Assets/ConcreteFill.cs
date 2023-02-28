using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcreteFill : MonoBehaviour
{
    public GameObject tank;
    public GameObject top;


    private Material fillMaterial;
    private float fillLimit = 0.2f;
    private float fill;

    // Start is called before the first frame update
    void Start()
    {
        fillMaterial = tank.GetComponent<MeshRenderer>().material;
        fill = 1.0f;

    }

    // Update is called once per frame
    void Update()
    {
        fillMaterial.SetFloat("_FillAmount", fill);

        top.transform.localPosition = new Vector3(top.transform.localPosition.x, -fill, top.transform.localPosition.z);
    }


    private void FillFoundation()
    {
        Debug.Log("filling");
        fill = fillMaterial.GetFloat("_FillAmount") - 0.001f;
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("ConcreteParticles") && fill > fillLimit)
        {
            FillFoundation();
        }
    }
}
