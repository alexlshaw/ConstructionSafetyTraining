using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("ToggleObjects")]
    [SerializeField] GameObject helmet;
    [SerializeField] GameObject glasses;
    [SerializeField] GameObject mask;
    [SerializeField] GameObject vest;
    [SerializeField] GameObject gloves;
    [SerializeField] GameObject safetyBoots;
    [SerializeField] GameObject harness;

    private Toggle helmetToggle;
    private Toggle glassesToggle;
    private Toggle maskToggle;
    private Toggle vestToggle;
    private Toggle glovesToggle;
    private Toggle safetyBootsToggle;
    private Toggle harnessToggle;
    private GameObject player;

    [Header("MenuObjects")]
    //[SerializeField] GameObject HelpScreenSide;
    [SerializeField] GameObject ExcavatorHelpScreen;
    [SerializeField] GameObject MixerHelpScreen;
    [SerializeField] GameObject ForkliftHelpScreen;

    [SerializeField] GameObject InfoScreen;
    [SerializeField] GameObject HelpScreen;

    [SerializeField] GameObject HpBar;

    public List<GameObject> taskObjects;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        helmetToggle = helmet.GetComponent<Toggle>();
        glassesToggle = glasses.GetComponent<Toggle>();
        maskToggle = mask.GetComponent<Toggle>();
        vestToggle = vest.GetComponent<Toggle>();
        glovesToggle = gloves.GetComponent<Toggle>();
        safetyBootsToggle = safetyBoots.GetComponent<Toggle>();
        harnessToggle = harness.GetComponent<Toggle>();
        GameEvents.current.onStartGame += UpdateTaskList;
        GameEvents.current.onStartGame += DisableMenu;
        GameEvents.current.onUpdateTaskList += UpdateTaskList;
        GameEvents.current.onUpdateEquipment += updateToggles;
        GameEvents.current.onTakeDamage += updateSlider;
        showInfoScreen();

    }

    void updateSlider()
    {
        HpBar.GetComponent<Slider>().value = PointsLostHandler.HP / 100;
    }


    void DisableMenu()
    {
        gameObject.SetActive(false);
    }

    public void resetPosition()
    {
        GameEvents.current.ResetPlayerPositon();
    }

    public void UpdateTaskList()
    {
        Debug.Log(Globals.getCurrentTasks().Count);
        List<Task> taskList = Globals.getCurrentTasks();
        for (int i = 0; i < taskObjects.Count; i++)
        {
            TextMeshProUGUI textMesh = taskObjects[i].GetComponent<TextMeshProUGUI>();
            textMesh.text = "";
            if (i < taskList.Count)
            {
                textMesh.text = taskList[i].getTaskString();
            }
        }
    }

    public void updateToggles()
    {
        helmetToggle.SetIsOnWithoutNotify(Globals.hasHat);
        glassesToggle.SetIsOnWithoutNotify(Globals.hasGlasses);
        maskToggle.SetIsOnWithoutNotify(Globals.hasMask);
        vestToggle.SetIsOnWithoutNotify(Globals.hasVest);
        glovesToggle.SetIsOnWithoutNotify(Globals.hasGloves);
        safetyBootsToggle.SetIsOnWithoutNotify(Globals.hasBoots);
        harnessToggle.SetIsOnWithoutNotify(Globals.hasHarness);
    }
    public void showInfoScreen()
    {
        InfoScreen.SetActive(true);
        //HelpScreenSide.SetActive(false);
        HelpScreen.SetActive(false);
    }

    public void showHelpScreen()
    {
        InfoScreen.SetActive(false);
        //HelpScreenSide.SetActive(false);
        HelpScreen.SetActive(true);
    }

    public void showExcavatorHelpScreen()
    {
        //HelpScreenSide.SetActive(true);
        Instantiate(ExcavatorHelpScreen, player.transform.position + (player.transform.forward) + player.transform.up, player.transform.rotation); 
        //ExcavatorHelpScreen.SetActive(true);
        //MixerHelpScreen.SetActive(false);
        //ForkliftHelpScreen.SetActive(false);
    }
    public void showMixerHelpScreen()
    {
        //HelpScreenSide.SetActive(true);
        Instantiate(MixerHelpScreen, player.transform.position + (player.transform.forward) + player.transform.up, player.transform.rotation);
        //ExcavatorHelpScreen.SetActive(false);
        //MixerHelpScreen.SetActive(true);
        //ForkliftHelpScreen.SetActive(false);
    }
    public void showForkliftHelpScreen()
    {
        Instantiate(ForkliftHelpScreen, player.transform.position + (player.transform.forward) + player.transform.up, player.transform.rotation);
        //HelpScreenSide.SetActive(true);
        //ExcavatorHelpScreen.SetActive(false);
        //MixerHelpScreen.SetActive(false);
        //ForkliftHelpScreen.SetActive(true);
    }


}
