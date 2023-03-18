using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RightHandScripts : MonoBehaviour
{
    XRDirectInteractor directInteractor;
    // Start is called before the first frame update
    void Start()
    {
        directInteractor = GetComponent<XRDirectInteractor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (directInteractor.selectTarget != null && directInteractor.selectTarget.name.Equals("RopeEnd"))
        {
            Rope rope = directInteractor.selectTarget.GetComponentInParent<Rope>();

            if (Vector3.Distance(rope.points[0].position, rope.points[rope.points.Length - 1].position) > 2)
            {
                Debug.Log(Vector3.Distance(rope.points[0].position, rope.points[rope.points.Length - 1].position));
                directInteractor.selectTarget.interactionLayerMask = LayerMask.NameToLayer("Default");
            }
        }
    }
}
