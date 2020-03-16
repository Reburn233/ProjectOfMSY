using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LoadAndRegisterScript : MonoBehaviour 
{
    public GameObject LoadPanel;
    public GameObject RegisterPanel;

    public InputField PlayerAccount;
    public InputField PlayerPassWorld;

    public InputField accountField;
    public InputField passWorld;
    public InputField passWorld2;
    public InputField playerName;
    public Text tips;
    public Text loadTips;
    private string filePath ;
	void Start () 
    {
        filePath = Application.streamingAssetsPath + "/XML/PlayerInformation.xml";
        MusicManager.Instance.PlayBGMusic();
	}
	
	
	void Update () 
    {

	}


    public bool checkXML(string account)
    {
        if (File.Exists(filePath))
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlNodeList allPlayer = doc.SelectSingleNode("root").ChildNodes;
            foreach(XmlElement playerInf in allPlayer)
            {
                if(playerInf.GetAttribute("account")==account)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public void addPlayerInXML(string account,string passWorld,string playerName)
    {
        if (File.Exists(filePath))
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlNode root = doc.SelectSingleNode("root");
            XmlElement information = doc.CreateElement("information");

            information.SetAttribute("playerName", playerName);
            information.SetAttribute("account", account);
            information.SetAttribute("passWorld", passWorld);
            information.SetAttribute("sp","0");
            information.SetAttribute("gameLevel", "1");

            root.AppendChild(information);
            doc.AppendChild(root);
            doc.Save(filePath);

            addPlayerCarInXML(account,0);
        }

    }
    //同步增加玩家车辆信息
    public void addPlayerCarInXML(string account,int carID)
    {
        string carInfPath = Application.streamingAssetsPath + "/XML/PlayerCarInformation.xml";
        if (File.Exists(carInfPath))
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(carInfPath);
            XmlNode root = doc.SelectSingleNode("root");
            XmlElement information = doc.CreateElement("player");
            information.SetAttribute("account", account);
            XmlElement newCar = doc.CreateElement("car");
            newCar.SetAttribute("id", carID.ToString());
            newCar.SetAttribute("FES", "0");
            newCar.SetAttribute("FAS", "0");
            newCar.SetAttribute("SES", "0");
            newCar.SetAttribute("SAS", "0");

            newCar.SetAttribute("engine", "1");
            newCar.SetAttribute("nitrogen", "1");
            newCar.SetAttribute("suspension", "1");

            newCar.SetAttribute("MaxMotorTorque", getCarAttribute(0, "MaxMotorTorque"));
            newCar.SetAttribute("MaxSpeed", getCarAttribute(0, "MaxSpeed"));
            newCar.SetAttribute("sliderSpeed", getCarAttribute(0, "sliderSpeed"));
            newCar.SetAttribute("speedUpTime", getCarAttribute(0, "speedUpTime"));

            information.AppendChild(newCar);
            root.AppendChild(information);
            doc.AppendChild(root);
            doc.Save(carInfPath);
        }
    }
    //从车辆信息中读取指定属性
    public string getCarAttribute(int id,string attributeName)
    {
        string filePath=Application.streamingAssetsPath+"/XML/CarInformation.xml";
        if (File.Exists(filePath))
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlNodeList nodelist = doc.SelectSingleNode("root").ChildNodes;
            foreach(XmlElement everyCar in nodelist)
            {
                if (everyCar.GetAttribute("id") == id.ToString())
                {
                    foreach (XmlElement everyAttribute in everyCar.ChildNodes)
                    {
                        if (everyAttribute.Name == attributeName)
                        {
                            return everyAttribute.InnerText;
                        }
                    }
                    
                }
            }
        }
        return null;
    }

    public void inputAccount()
    {
        tips.text = null;
        if (checkXML(accountField.text))
        {
            tips.text = "账号已存在";
        }
        else if (accountField.text.Length < 4 || accountField.text.Length >8)
        {
            tips.text = "账号过短";
        }
    }

    public void passWorldSame()
    {
        tips.text = null;
        string temp = tips.text;
        if (passWorld.text != passWorld2.text)
        {
            tips.text = "两次密码不一致";
        }
        else
        {
            tips.text = temp;
        }
    }

    public void LoadInXml()
    {
        MusicManager.Instance.PlayEffMusic("click");
        if (accountField.text != null && passWorld.text != null && passWorld2.text != null && playerName.text != null
            && passWorld.text == passWorld2.text && accountField.text.Length>=4
            )
        {
            PlayerInformation.Instance.account = accountField.text;
            PlayerInformation.Instance.playerName = playerName.text;
            PlayerInformation.Instance.SP = 0;
            PlayerInformation.Instance.gameLevel = 1;
            addPlayerInXML(accountField.text, passWorld.text, playerName.text);
            LoadInGame(1);
        }
    }
    public void LoadInGame(int index)
    {
        //SceneManager.LoadScene(index);
        PlayerInformation.Instance.changeScenes("mainScenes");
    }

    public void goRigister()
    {
        MusicManager.Instance.PlayEffMusic("click.mp3");
        RegisterPanel.SetActive(true);
        LoadPanel.SetActive(false);
    }

    public void checkAccountAndPass()
    {
        MusicManager.Instance.PlayEffMusic("click");
        bool ifFind = false;
        if (File.Exists(filePath))
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlNodeList allPlayer = doc.SelectSingleNode("root").ChildNodes;
            foreach (XmlElement playerInf in allPlayer)
            {
                if (playerInf.GetAttribute("account") == PlayerAccount.text && playerInf.GetAttribute("passWorld") == PlayerPassWorld.text)
                {
                    PlayerInformation.Instance.account = playerInf.GetAttribute("account");
                    PlayerInformation.Instance.playerName = playerInf.GetAttribute("playerName");
                    PlayerInformation.Instance.SP = int.Parse(playerInf.GetAttribute("sp"));
                    PlayerInformation.Instance.gameLevel = int.Parse(playerInf.GetAttribute("gameLevel"));
                    ifFind = true;
                    LoadInGame(1);
                    break;
                }
            }
        }
        if (!ifFind)
        {
            loadTips.text = "账号或密码不存在";
        }
       
    }
    public void hideTips()
    {
        loadTips.text = null;
    }
}
