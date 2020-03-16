using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Xml;
using System.IO;
public enum sceneName
{
    _inGame,
    _inRank,
    _inUI
}
public class PlayerInformation : MonoBehaviour 
{
    [HideInInspector]
    public sceneName nowScene;
    [HideInInspector]
    public string playerName;
    [HideInInspector]
    public string account;
    [HideInInspector]
    public int SP;
    [HideInInspector]
    public int gameLevel;
    [HideInInspector]
    public int whichRace;
    [HideInInspector]
    public int carDriving;
    [HideInInspector]
    public carInformationTips carInformation;
    [HideInInspector]
    public string[] AInames = 
    {
        "Sam",
        "Jeck",
        "Vivian",
        "Bob",
        "Kavin",
        "Jims",
        "White"
    };
    [HideInInspector]
    public GameObject asyUI;
    private static PlayerInformation instance;
    public static PlayerInformation Instance
    {
        get { return instance; }
    }
	
    public void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        playerName = "MSY";
        SP = 99999;
        gameLevel = 9;
        whichRace = 0;
        carDriving = 0;
        nowScene = sceneName._inUI;
    }
    
	void Update () 
    {
		if(Input.GetKeyDown(KeyCode.M))
        {
            changeBGM();
        }
	}
    public void updateCarInformation(carInformationTips newCar)
    {
        carInformation.MaxMotorTorque = newCar.MaxMotorTorque;
        carInformation.MaxSpeed = newCar.MaxSpeed;
        carInformation.sliderSpeed = newCar.sliderSpeed;
        carInformation.speedUpTime = newCar.speedUpTime;
    }
    public void changeBGM()
    {
        print(nowScene);
        switch (nowScene)
        {
            case sceneName._inUI: MusicManager.Instance.PlayBGMusic(); break;
            case sceneName._inGame: MusicManager.Instance.PlayGameMusic(); break;
            case sceneName._inRank: MusicManager.Instance.PlayRankMusic(); break;
        }
    }
    public void changeScenes(string sceneName)
    {
        asyUI.SetActive(true);
        asyUI.transform.SetAsLastSibling();
        asyUI.GetComponent<AsyncLoadScene>().changeScene(sceneName);
    }
    public void backTOxml()
    {
        string filePath = Application.streamingAssetsPath + "/XML/PlayerInformation.xml";
        if (File.Exists(filePath))
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlNodeList nodeslist = doc.SelectSingleNode("root").ChildNodes;
            foreach (XmlElement everyPlayer in nodeslist)
            {
                if (everyPlayer.GetAttribute("account") == PlayerInformation.instance.account)
                {
                    print("in");
                    foreach (XmlAttribute attributes in everyPlayer.Attributes)
                    {
                        print(attributes.Name);
                        if (attributes.Name == "sp")
                        {
                            attributes.Value = PlayerInformation.instance.SP.ToString();
                            print(attributes.Value);
                        }
                        else if (attributes.Name == "gameLevel")
                        {
                            attributes.Value = PlayerInformation.instance.gameLevel.ToString();
                        }
                        
                    }
                    doc.Save(filePath);
                    return;
                }
            }
        }
        else
        {
            print("读取失败");
        }
    }
}
