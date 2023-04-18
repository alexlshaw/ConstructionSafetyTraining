using UnityEngine;

public class BaseController : MonoBehaviour
{

    public HingeJoint topHinge;
    public HingeJoint bottomHinge;

    // needs to decrease to extend
    float topHingeMin = -0.45f;
    float topHingeMax = -0.65f;

    // needs to increase to extend
    float bottomHingeMin = 0.45f;
    float bottomHingeMax = 0.65f;

    public float currentTop;
    public float currentBottom;

    public float speed = .25f;

    // Start is called before the first frame update
    void Start()
    {
        currentTop = topHinge.anchor.x;
        currentBottom = bottomHinge.anchor.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Keypad1) && currentTop > topHingeMax && currentBottom < bottomHingeMax)
        {
            IncreaseNumber();
        }
        else if (Input.GetKey(KeyCode.Keypad2) && currentTop < topHingeMin && currentBottom > bottomHingeMin)
        {
            DecreaseNumber();
        }

        topHinge.anchor = new Vector3(currentTop, 0, 0);
        bottomHinge.anchor = new Vector3(currentBottom, 0, 0);

    }

    public void IncreaseNumber()
    {
        currentTop -= Time.deltaTime * speed;
        currentBottom += Time.deltaTime * speed;
    }

    public void DecreaseNumber()
    {
        currentTop += Time.deltaTime * speed;
        currentBottom -= Time.deltaTime * speed;
    }
}