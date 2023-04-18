using UnityEngine;

public class DiggableBlocks : MonoBehaviour
{
    int total;
    int currentDestroyed = 0;
    float completionThreshold = 0.7f;
    private void Start()
    {
        GameEvents.current.onStartGame += setSiteNav;
    }
    // Start is called before the first frame update
    public void setTotal(int total)
    {
        this.total = total;
        GameEvents.current.onCompleteDigging += deactivate;
        GameEvents.current.onResetLevel += activate;

    }
    void setSiteNav()
    {
        GameEvents.current.SetSiteNavLocation(transform.position);
    }
    // Update is called once per frame
    public void addDestroyed()
    {
        currentDestroyed += 1;
        if (currentDestroyed / total > completionThreshold)
        {
            GameEvents.current.CompleteDigging();
        }
    }
    public void activate()
    {
        if(Globals.currentLevel == 1)
        {
            gameObject.SetActive(true);
            currentDestroyed = 0;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
    public void deactivate()
    {
        this.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameEvents.current.onCompleteDigging -= deactivate;

    }
}
