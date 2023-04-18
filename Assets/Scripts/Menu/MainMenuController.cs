using TMPro;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject startPage;
    [SerializeField]
    private GameObject endOfLevelPage;
    [SerializeField]
    private GameObject gameOverPage;
    [SerializeField]
    private GameObject resetVehiclePage;
    public TextMeshProUGUI textMeshPro;
    string baseText;
    public GameObject[] Stars;

    // Start is called before the first frame update
    void Start()
    {
        baseText = textMeshPro.text;
        GameEvents.current.onStartGame += showStartMenu;
        GameEvents.current.onVehicleTipped += showVehicleReset;
        GameEvents.current.onFirstFloorFinished += showFinishLevel;
        GameEvents.current.onSecondLevelFinished += showFinishLevel;
        GameEvents.current.onGameOver += showGameOver;

        GameEvents.current.onResetLevel += reset;
    }

    public void resetLevel()
    {
        GameEvents.current.ResetLevel();
        closeMenu();
    }
    public void nextLevel()
    {
        if(Globals.currentLevel == 1)
        {
            GameEvents.current.StartSecondLevel();
        }
        if (Globals.currentLevel == 2)
        {
            GameEvents.current.StartRoofLevel();
        }
        closeMenu();
    }
 
    public void closeMenu()
    {
        gameObject.SetActive(false);
    }

    void showStartMenu()
    {
        gameObject.SetActive(true);
        startPage.SetActive(true);
        endOfLevelPage.SetActive(false);
        gameOverPage.SetActive(false);
        resetVehiclePage.SetActive(false);
    }
    void showFinishLevel()
    {
        Debug.Log("Finish");
        gameObject.SetActive(true);
        startPage.SetActive(false);
        endOfLevelPage.SetActive(true);
        gameOverPage.SetActive(false);
        resetVehiclePage.SetActive(false);
        setEndOfLevelText();
        setStars();
    }

    void showGameOver()
    {
        gameObject.SetActive(true);
        startPage.SetActive(false);
        endOfLevelPage.SetActive(false);
        gameOverPage.SetActive(true);
        resetVehiclePage.SetActive(false);
        setEndOfLevelText();
        setStars();
    }
    void showVehicleReset()
    {
        gameObject.SetActive(true);
        startPage.SetActive(false);
        endOfLevelPage.SetActive(false);
        resetVehiclePage.SetActive(true);
    }

    public void resetVehiclePosition()
    {
        GameEvents.current.ResetVehiclePosition();
        closeMenu();
    }

    void setEndOfLevelText()
    {
        textMeshPro.text = baseText.Replace("num_collisions", PointsLostHandler.collisions.ToString())
            .Replace("num_falls", PointsLostHandler.falls.ToString())
            .Replace("num_tips", PointsLostHandler.tips.ToString())
            .Replace("num_asbestos", PointsLostHandler.dust.ToString()
            .Replace("num_falling", PointsLostHandler.other.ToString()));

    }
    public void reset()
    {
        closeMenu();
    }

    void setStars()
    {
        float percentage = PointsLostHandler.calculatePoints()/1000;
        Debug.Log(PointsLostHandler.calculatePoints());
        for(int i = 0; i < Stars.Length; i += 1)
        {
            Debug.Log(i);
            if(i*20 < percentage * 100)
            {
                Stars[i].SetActive(true);
            }
            else
            {
                Stars[i].SetActive(false);
            }
        }

    }
}
