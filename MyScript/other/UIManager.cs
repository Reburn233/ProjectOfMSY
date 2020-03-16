using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public enum UIname
{
    MainUI,
    gameUI,
    HousePanel,
    settingUI
}

public class UIManager : MonoBehaviour
{
    public List<GameObject> UIlist;
    public GameObject careerUI;
    public GameObject levelUI;
    public Text playName;
    public Text sp;
    //public List<Button> levelList;
    private static UIManager instance;
    public static UIManager Instance
    {
        get { return instance; }
    }

    void Start()
    {
        instance = this;
        playName.text = " " + PlayerInformation.Instance.playerName;
        sp.text = " SP:" + PlayerInformation.Instance.SP;
        PlayerInformation.Instance.backTOxml();
        //levelList = new List<Button>();
        UpdateLevel();
        MusicManager.Instance.PlayBGMusic();
    }

    void Update()
    {

    }

    public void UpdateLevel()
    {
        //print("UpdateLevel");
        int levelNum;
        levelNum = PlayerInformation.Instance.gameLevel;

        Transform content = levelUI.transform.Find("Viewport/Content");
        foreach (Button everyBtn in content.GetComponentsInChildren<Button>())
        {
            everyBtn.interactable = false;
            if (levelNum-- > 0)
            {
                everyBtn.interactable = true;
            }
        }
    }

    public void changeUI(string uiName)
    {
        MusicManager.Instance.PlayEffMusic("click");
        foreach(GameObject temp in UIlist)
        {
            if (temp.name == uiName)
            {
                temp.SetActive(true);
            }else
            {
                temp.SetActive(false);
            }
        }
    }

    public void chooseCareer(bool open)
    {
        MusicManager.Instance.PlayEffMusic("click");
        if (open)
        {
            careerUI.SetActive(true);
            levelUI.SetActive(false);
        }
        else
        {
            careerUI.SetActive(false);
            levelUI.SetActive(true);
        }
    }

    public void enterGame(int RaceNum)
    {
        MusicManager.Instance.PlayEffMusic("click");
        PlayerInformation.Instance.whichRace = RaceNum-1;
        Time.timeScale = 1;
        //SceneManager.LoadScene(2);
        PlayerInformation.Instance.changeScenes("Dark City");
    }

    public void exitGame()
    {
        MusicManager.Instance.PlayEffMusic("click");
        PlayerInformation.Instance.backTOxml();
        Application.Quit();
    }

    public void goEscapeRace()
    {
        MusicManager.Instance.PlayEffMusic("click");
        //SceneManager.LoadScene(3);
        PlayerInformation.Instance.changeScenes("escapeRace");
    }
}
