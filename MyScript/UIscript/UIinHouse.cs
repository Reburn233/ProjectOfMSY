using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Xml;
using System.IO;
using UnityEngine.EventSystems;
using DG.Tweening;
[System.Serializable]
public struct carInformationTips
{
    public int id;
    public string name;
    public int cost;
    public string des;
    public int MaxMotorTorque;
    public int MaxSpeed;
    public float sliderSpeed;
    public float speedUpTime;
}
public class UIinHouse : MonoBehaviour 
{
    GameObject carContent;
    public int carNum=0;
    carInformationTips carNowInShowing;
    private static UIinHouse instance;
    public Text carDES;
    public Text sp;
    public Text moneyTips;
    public Text DriveTips;

    public Text engineLV;
    public Text N2OLV;
    public Text HangLV;

    public GameObject tipsWindow;
    private int moneyCost;
    private int refitType;
    public static UIinHouse Instance
    {
        get { return instance; }
    }

	void Start () 
    {
        sp.text = PlayerInformation.Instance.SP.ToString();
        instance = this;
        giveFrameworkAttribute();

	}
	
	
	void Update () 
    {

	}

    //返回车辆整体信息到面板上
    public void LoadInformationInXML(int carID)
    {
        string filePath = Application.streamingAssetsPath + "/XML/CarInformation.xml";
        if (File.Exists(filePath))
        {
            carDES.text = "  ";
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;
            foreach(XmlElement carInf in nodeList)
            {
                foreach(XmlAttribute tempName in carInf.Attributes)
                {
                    if (tempName.Name=="id" && tempName.Value==carID.ToString())
                    {
                        //找到对应ID的节点
                        //print(carInf.GetAttribute("carName"));
                        carDES.text = carInf.GetAttribute("carName");
                        foreach (XmlElement carAttribute in carInf.ChildNodes)
                        {
                            if (carAttribute.Name == "cost" || carAttribute.Name == "des")
                            {
                                carDES.text += "\n" + carAttribute.Name + ": " + carAttribute.InnerText.ToString();
                            }
                           
                        }
                        updateCarAttributes(carDES);
                        updateCarRefit(carID);
                        
                        return;
                    }
                }
            }

        }
    }
   
    //同步面板上车辆属性信息
    public void updateCarAttributes(Text carInf)
    {
        if(checkPlayerCarXML(PlayerInformation.Instance.account,carNum))
        {
            carInf.text += "\n" + "MaxMotorTorque:" + getPlayerCarAttribution(carNum, "MaxMotorTorque");
            carInf.text += "\n" + "MaxSpeed:" + getPlayerCarAttribution(carNum, "MaxSpeed");
            carInf.text += "\n" + "sliderSpeed:" + getPlayerCarAttribution(carNum, "sliderSpeed");
            carInf.text += "\n" + "speedUpTime:" + getPlayerCarAttribution(carNum, "speedUpTime");
        }
       
    }
    //将当前车辆的改装信息同步到面板上
    public void updateCarRefit(int carID)
    {
        if (checkPlayerCarXML(PlayerInformation.Instance.account, carID))
        {
            transform.Find("refit").gameObject.SetActive(true);          
            string carInfPath = Application.streamingAssetsPath + "/XML/PlayerCarInformation.xml";
            if (File.Exists(carInfPath))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(carInfPath);
                XmlNodeList allInf = doc.SelectSingleNode("root").ChildNodes;
                foreach (XmlElement playerCar in allInf)
                {
                    if (playerCar.GetAttribute("account") == PlayerInformation.Instance.account)
                    {
                        foreach (XmlElement everyCar in playerCar.ChildNodes)
                        {
                            if (everyCar.GetAttribute("id") == carID.ToString())
                            {
                                engineLV.text = "Lv:" + everyCar.GetAttribute("engine");
                                N2OLV.text = "Lv:" + everyCar.GetAttribute("nitrogen");
                                HangLV.text = "Lv:" + everyCar.GetAttribute("suspension");
                            }
                        }
                    }
                }
            }

        }
        else
        {
            transform.Find("refit").gameObject.SetActive(false);
            engineLV.text = "Lv:1";
            N2OLV.text = "Lv:1";
            HangLV.text = "Lv:1";
        }
    }

    //点击升级按键后执行
    public void upRefit(int type)
    {
        MusicManager.Instance.PlayEffMusic("click");
        refitType = type;
        tipsWindow.SetActive(true);
        int money = int.Parse(getCarInfInXML(carNum, "cost"));
        Text tips = tipsWindow.transform.Find("Text").GetComponent<Text>();
        tips.text = "是否花费" + (money/10).ToString()+"升级配件";
        moneyCost = money / 10;
        //updateCarRefit(carNum);
    }
    public void yesBtn()
    {
        MusicManager.Instance.PlayEffMusic("click");
        string temp = "engine";
        int lvNum = 0;
        switch (refitType)
        {
            case 0: 
                { 
                    temp = "engine"; 
                    lvNum = int.Parse(engineLV.text.Substring(3, 1)) + 1;
                    changeCarAttributeInf(carNum, "MaxMotorTorque","100");
                    changeCarAttributeInf(carNum, "MaxSpeed", "1");
                } break;
            case 1:
                { 
                    temp = "nitrogen"; 
                    lvNum = int.Parse(N2OLV.text.Substring(3, 1)) + 1;
                    changeCarAttributeInf(carNum, "speedUpTime", "0.1");
                } break;
            case 2: 
                { 
                    temp = "suspension"; 
                    lvNum = int.Parse(HangLV.text.Substring(3, 1)) + 1;
                    changeCarAttributeInf(carNum, "sliderSpeed", "0.001");
                } break;
        }

        moneyDown(moneyCost);
        moneyCost = 0;
        changePlayerCarInf(carNum, temp, lvNum.ToString());
        updateCarRefit(carNum);
        tipsWindow.SetActive(false);
        LoadInformationInXML(carNum);
    }
    public void NoBtn()
    {
        MusicManager.Instance.PlayEffMusic("click");
        moneyCost = 0;
        tipsWindow.SetActive(false);
    }
    //检查玩家是否拥有指定车辆
    public bool checkPlayerCarXML(string account,int id)
    {
        string carInfPath = Application.streamingAssetsPath + "/XML/PlayerCarInformation.xml";
        if (File.Exists(carInfPath))
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(carInfPath);
            XmlNodeList allInf = doc.SelectSingleNode("root").ChildNodes;
           foreach(XmlElement playerCar in allInf)
           {
               if (playerCar.GetAttribute("account")==account)
               {
                   foreach (XmlElement everyCar in playerCar.ChildNodes)
                   {
                       if (everyCar.GetAttribute("id")==id.ToString())
                       {
                           return true;
                       }
                   }
               }
           }
            return false;
        }
        return false;
    }

    //在XML中添加指定车辆
    public void BuyNewCar()
    {
        MusicManager.Instance.PlayEffMusic("click");
        int carCost = int.Parse(getCarInfInXML(carNum, "cost"));
        if (moneyDown(carCost)==false)
        {
            return;
        }

        string carInfPath = Application.streamingAssetsPath + "/XML/PlayerCarInformation.xml";
        if (File.Exists(carInfPath))
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(carInfPath);
            XmlNodeList allInf = doc.SelectSingleNode("root").ChildNodes;
            foreach (XmlElement playerCar in allInf)
            {
                if (playerCar.GetAttribute("account") == PlayerInformation.Instance.account)
                {
                    XmlElement car= doc.CreateElement("car");
                    playerCar.AppendChild(car);
                    car.SetAttribute("id", carNum.ToString());
                    car.SetAttribute("FES","0");
                    car.SetAttribute("FAS", "0");
                    car.SetAttribute("SES", "0");
                    car.SetAttribute("SAS", "0");
                    car.SetAttribute("engine", "1");
                    car.SetAttribute("nitrogen", "1");
                    car.SetAttribute("suspension", "1");
                    car.SetAttribute("MaxMotorTorque", getCarInfInXML(carNum, "MaxMotorTorque"));
                    car.SetAttribute("MaxSpeed", getCarInfInXML(carNum, "MaxSpeed"));
                    car.SetAttribute("sliderSpeed", getCarInfInXML(carNum, "sliderSpeed"));
                    car.SetAttribute("speedUpTime", getCarInfInXML(carNum, "speedUpTime"));
                }
            }
            doc.Save(carInfPath);
        }      
       showMyCar.Instance.chooseCar(carNum);
       updateCarRefit(carNum);
       LoadInformationInXML(carNum);
       transform.Find("refit").gameObject.SetActive(true);
    }

    //获得车辆配置文件中的指定属性
    public string getCarInfInXML(int id,string attributeName)
    {
        string carInfPath = Application.streamingAssetsPath + "/XML/CarInformation.xml";
        if (File.Exists(carInfPath))
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(carInfPath);
            XmlNodeList allInf = doc.SelectSingleNode("root").ChildNodes;
            foreach (XmlElement playerCar in allInf)
            {
                if (playerCar.GetAttribute("id") == id.ToString())
                {
                    foreach (XmlElement singleAttribute in playerCar.ChildNodes)
                    {
                        if (singleAttribute.Name == attributeName)
                        {
                            //print(singleAttribute.InnerText);
                            return singleAttribute.InnerText;
                        }
                    }
                }
            }
        }
        return null;
    }

    //获得玩家仓库配置文件中车辆的指定属性
    public string getPlayerCarAttribution(int id, string attributeName)
    {
        string carInfPath = Application.streamingAssetsPath + "/XML/PlayerCarInformation.xml";
        if (File.Exists(carInfPath))
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(carInfPath);
            XmlNodeList allInf = doc.SelectSingleNode("root").ChildNodes;
            foreach (XmlElement everyplayer in allInf)
            {
                if (everyplayer.GetAttribute("account") == PlayerInformation.Instance.account)
                {
                    foreach (XmlElement everyCar in everyplayer.ChildNodes)
                    {
                        if (everyCar.GetAttribute("id")==id.ToString())
                        {
                            return everyCar.GetAttribute(attributeName);
                        }
                    }
                }
            }
        }
        return null;
    }

    //改变车辆改装信息
    public void changePlayerCarInf(int id,string attributeName,string newValue)
    {
        if (checkPlayerCarXML(PlayerInformation.Instance.account, id))
        {
            string carInfPath = Application.streamingAssetsPath + "/XML/PlayerCarInformation.xml";
            if (File.Exists(carInfPath))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(carInfPath);
                XmlNodeList allInf = doc.SelectSingleNode("root").ChildNodes;
                foreach (XmlElement playerCar in allInf)
                {
                    if (playerCar.GetAttribute("account") == PlayerInformation.Instance.account)
                    {
                        foreach (XmlElement everyCar in playerCar.ChildNodes)
                        {
                            if (everyCar.GetAttribute("id")==id.ToString())
                            {
                                foreach (XmlAttribute temp in everyCar.Attributes)
                                {
                                    if (temp.Name == attributeName)
                                    {
                                        temp.Value = newValue;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                doc.Save(carInfPath);
            }
        }
    }

    //改变车辆属性信息
    public void changeCarAttributeInf(int id, string attributeName, string addValue)
    {
        if (checkPlayerCarXML(PlayerInformation.Instance.account, id))
        {
            string carInfPath = Application.streamingAssetsPath + "/XML/PlayerCarInformation.xml";
            if (File.Exists(carInfPath))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(carInfPath);
                XmlNodeList allInf = doc.SelectSingleNode("root").ChildNodes;
                foreach (XmlElement playerCar in allInf)
                {
                    if (playerCar.GetAttribute("account") == PlayerInformation.Instance.account)
                    {
                        foreach (XmlElement everyCar in playerCar.ChildNodes)
                        {
                            if (everyCar.GetAttribute("id") == id.ToString())
                            {
                                foreach (XmlAttribute temp in everyCar.Attributes)
                                {
                                    if (temp.Name == attributeName)
                                    {
                                        temp.Value =( float.Parse(temp.Value) + float.Parse(addValue)).ToString();
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                doc.Save(carInfPath);
            }
        }
    }
    public bool moneyDown(int cost)
    {
        if (PlayerInformation.Instance.SP - cost > 0)
        {
            PlayerInformation.Instance.SP -= cost;
            PlayerInformation.Instance.backTOxml();
            StartCoroutine(SPupAndDown(PlayerInformation.Instance.SP));
            moneyTips.text = "购买成功";
            moneyTips.DOFade(1, 0.5f).OnComplete((() => { moneyTips.DOFade(0, 0.5f);}));
            return true;
        }
        else
        {
            moneyTips.text = "SP不足";
            moneyTips.DOFade(1, 0.5f).OnComplete((() => { moneyTips.DOFade(0, 0.5f); }));
            return false;
        }
    }


    //将某文本内的数字渐变到指定值
    IEnumerator SPupAndDown(int endSp)
    {
        int unitNum = (endSp - int.Parse(sp.text))/10;
         while(true)
         {
             sp.text = (int.Parse(sp.text) + unitNum).ToString();
             yield return new WaitForSeconds(0.1f);
             if (int.Parse(sp.text) == endSp)
             {
                 break;
             }
         }
    }

    public void driveCar()
    {
        MusicManager.Instance.PlayEffMusic("click");
        if (checkPlayerCarXML(PlayerInformation.Instance.account, carNum))
        {
            DriveTips.text = "Driving";
            giveFrameworkAttribute();
        }
        else
        {
            DriveTips.text = "Inexistence";
        }
    }
    public void giveFrameworkAttribute()
    {
        PlayerInformation.Instance.carDriving = carNum;
        carNowInShowing.MaxMotorTorque = int.Parse(getPlayerCarAttribution(carNum, "MaxMotorTorque"));
        carNowInShowing.MaxSpeed = int.Parse(getPlayerCarAttribution(carNum, "MaxSpeed"));
        carNowInShowing.sliderSpeed = float.Parse(getPlayerCarAttribution(carNum, "sliderSpeed"));
        carNowInShowing.speedUpTime = float.Parse(getPlayerCarAttribution(carNum, "speedUpTime"));
        PlayerInformation.Instance.updateCarInformation(carNowInShowing);
    }

}
