using UnityEngine;

public class Foundation : MonoBehaviour
{
    float total;
    public float filled;
    private float fillLimit = 0.2f;

    // Start is called before the first frame update
    private void Start()
    {
        GameEvents.current.onResetLevel += Deactivate;
        GameEvents.current.onCompleteDigging += Activate;
        GameEvents.current.onFoundationFilled += Deactivate;
        filled = 2;
    }
    public void addFilled()
    {
        filled -= 0.0005f;
        if(filled <= fillLimit)
        {
            GameEvents.current.FoundationFilled();
        }
    }

    public void setTotal()
    {
        total = transform.childCount * 0.8f;
        Debug.Log("Total" + total);
    }

    void Activate()
    {
        gameObject.SetActive(true);
    }
    void Deactivate()
    {
        filled = 2;
        gameObject.SetActive(false);
    }
}
