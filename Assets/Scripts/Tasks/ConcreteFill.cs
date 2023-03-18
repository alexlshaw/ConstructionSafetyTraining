using UnityEngine;

public class ConcreteFill : MonoBehaviour
{
    public GameObject tank;
    public GameObject top;
    Foundation foundation;

    private Material fillMaterial;
    private float fillLimit = 0.2f;
    private float fill;

    // Start is called before the first frame update
    void Start()
    {
        foundation = GetComponentInParent<Foundation>();
        fillMaterial = tank.GetComponent<MeshRenderer>().material;
        fill = tank.transform.localScale.y;
        GameEvents.current.onFoundationFilled += fullFillFoundation;
        GameEvents.current.onResetLevel += UnfillFoundation;
    }

    // Update is called once per frame
    void Update()
    {
        fillMaterial.SetFloat("_FillAmount", foundation.filled);
        top.transform.localPosition = new Vector3(top.transform.localPosition.x, Mathf.Max(-fill, -foundation.filled), top.transform.localPosition.z);
    }


    private void FillFoundation()
    {
        foundation.addFilled();
    }

    private void fullFillFoundation()
    {
        fill = 0.2f;

    }
    private void UnfillFoundation()
    {
        fill = tank.transform.localScale.y;
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.name);

        if (other.name.Equals("ConcreteParticles") && fill > fillLimit)
        {
            FillFoundation();

        }

    }
}
