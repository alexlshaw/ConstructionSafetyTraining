using UnityEngine;

public class DiggingTesting : MonoBehaviour
{
    BaseController baseController;
    ForeArmControl foreArmController;
    BucketControl bucketController;
    public bool digDown = false;
    public bool digUp = false;
    public bool unload = false;

    public GameObject instructionUI;
    public bool instructionsShown = false;

    private void Awake()
    {
        baseController = gameObject.GetComponentInChildren<BaseController>();
        foreArmController = gameObject.GetComponentInChildren<ForeArmControl>();
        bucketController = gameObject.GetComponentInChildren<BucketControl>();
        //GameEvents.current.onStartGame += setSiteNav;
        instructionUI.SetActive(false);

    }

    //void setSiteNav()
    //{
    //    GameEvents.current.SetSiteNavLocation(transform.position);
    //}
    // Start is called before the first frame update
    void Update()
    {
        if (digDown)
        {
            if (foreArmController.currentTop < -0.41)
            {
                foreArmController.DecreaseNumber();
            }
            else if (baseController.currentTop > -0.575)
            {
                baseController.IncreaseNumber();
            }
            else if (bucketController.currentTop > -0.58)
            {
                bucketController.IncreaseNumber();
            }
            else
            {
                digUp = true;
                digDown = false;
            }
        }
        if (digUp)
        {
            if (foreArmController.currentTop > -0.5)
            {
                foreArmController.IncreaseNumber();
            }
            else if (baseController.currentTop < -0.5)
            {
                baseController.DecreaseNumber();
            }
            else
            {
                digUp = false;
            }
        }
        if (unload)
        {
            if (bucketController.currentTop < -0.5)
            {
                bucketController.DecreaseNumber();
            }
            else
            {
                unload = false;
            }
        }
        if (Input.GetKey(KeyCode.W))
        {
            digDown = true;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            unload = true;
        }


    }
    private void OnTriggerEnter(Collider other)
    {
        if (!instructionsShown && other.gameObject.tag.Equals("Player"))
        {
            instructionsShown = true;
            instructionUI.SetActive(true);
        }
    } 
    

}
