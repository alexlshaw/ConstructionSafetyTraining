using UnityEngine;

public class SteeringWheel : MonoBehaviour
{
    float maxTurnAngle = 45;

    void Update()
    {
        Vector3 modified = Vector3.up * Mathf.Clamp((Input.GetAxis("Horizontal") * 100), -maxTurnAngle, maxTurnAngle);
        transform.localEulerAngles = new Vector3(modified.x, modified.y, 90f);


    }
}
