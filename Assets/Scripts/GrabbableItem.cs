using UnityEngine;

public class GrabbableItem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onResetLevel += destroySelf;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void destroySelf()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameEvents.current.onResetLevel -= destroySelf;

    }
}
