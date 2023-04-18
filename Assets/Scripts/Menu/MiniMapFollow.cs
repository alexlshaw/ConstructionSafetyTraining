using UnityEngine;

public class MiniMapFollow : MonoBehaviour
{
    public float height;
    private GameObject Target;

    private void Start()
    {
        Target = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        if (Target == null)
        {
            Target = GameObject.FindGameObjectWithTag("Player");

        }
        else
        {
            Vector3 position = transform.position;

            position.x = Target.transform.position.x;
            position.y = height;
            position.z = Target.transform.position.z;

            transform.position = position;
            //transform.rotation = Quaternion.Euler(90, Target.transform.rotation.eulerAngles.y, 0);
        }

    }
}
