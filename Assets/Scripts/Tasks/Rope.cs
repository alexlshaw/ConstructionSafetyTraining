using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Rope : MonoBehaviour
{

    public Transform[] points;
    private LineRenderer lineRenderer;
    public Rigidbody lastSphere;
    private XRGrabInteractable grabInteractable;
    public XRSocketInteractor socketInteractor;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = points.Length;
        lastSphere = points[points.Length - 1].GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    
    private void Update()
    {
        gameObject.SetActive(!Globals.hasHarness);
        for (int i = 0; i < points.Length; ++i)
        {
            lineRenderer.SetPosition(i, points[i].position);
        }
    }
}
