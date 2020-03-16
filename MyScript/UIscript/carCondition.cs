using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.IO;
public class carCondition : MonoBehaviour 
{
    public WheelCollider[] wheels;

    public InputField[] allConditions;
    public Slider[] allSliders;
     
    private int[] originalNum={0,0,0,0};
    private WheelFrictionCurve origiForwardWheel;
    private WheelFrictionCurve origiSlidwaysWheel;
    private static carCondition instance;
    public static carCondition Instance
    {
        get { return instance; }
    }
	void Start () 
    {
        instance = this;
        origiForwardWheel = getNewWheelFrictionCurve
        (
        wheels[0].forwardFriction.extremumSlip,
        wheels[0].forwardFriction.extremumValue,
        wheels[0].forwardFriction.asymptoteSlip,
        wheels[0].forwardFriction.asymptoteValue,
        wheels[0].forwardFriction.stiffness
        );
        origiSlidwaysWheel = getNewWheelFrictionCurve
        (
        wheels[0].sidewaysFriction.extremumSlip,
        wheels[0].sidewaysFriction.extremumValue,
        wheels[0].sidewaysFriction.asymptoteSlip,
        wheels[0].sidewaysFriction.asymptoteValue,
        wheels[0].sidewaysFriction.stiffness
        );
       readCarInfXML();
	}
	
	
	void Update () 
    {
		
	}
    public void recoverWheelsInf()
    {
       foreach(WheelCollider temp in wheels)
       {
           temp.forwardFriction = origiForwardWheel;
           temp.sidewaysFriction = origiSlidwaysWheel;
       }
       foreach (InputField temp in allConditions)
        {
            temp.text = "50";
        }
       foreach (Slider temp in allSliders)
       {
           temp.value = 50;
       }
    }
    public void checkAgain(int num)
    {
        foreach (char temp in allConditions[num].text)
        {
            if ((int)temp < 48 || (int)temp > 57)
            {
                allConditions[num].text = "0";
            }
        }
        allSliders[num].value = int.Parse(allConditions[num].text);
        changeWheelsAttribute(num, ((float)allSliders[num].value - 50) / 100);
    }
    public void checkInput(int num)
    {
        if (allConditions[num].text.Length >= 2)
        {
            if (int.Parse(allConditions[num].text) > 100)
            {
                allConditions[num].text = allConditions[num].text.Substring(0, 2);
            }
        }
        else
        {
            foreach (char temp in allConditions[num].text)
            {
                if ((int)temp < 48 || (int)temp > 57)
                {
                    allConditions[num].text ="0";
                }
            }
            originalNum[num] = int.Parse(allConditions[num].text);
        }
        foreach (char temp in allConditions[num].text)
        {
            if ((int)temp < 48 || (int)temp > 57)
            {
                allConditions[num].text = "0";
            }
        }
        allSliders[num].value = int.Parse(allConditions[num].text);
        changeWheelsAttribute(num, ((float)allSliders[num].value - 50) / 100);
        //print(((float)allSliders[num].value - 50) / 100);
    }
    public void sliderChange(int num)
    {
        allConditions[num].text = allSliders[num].value.ToString();
        changeWheelsAttribute(num, ((float)allSliders[num].value-50)/100);
    }
    public void changeWheelsAttribute(int num,float value)
    {
        string attributeName = "";
        switch (num)
        {
            case 0: changeFES(value); attributeName = "FES"; break;
            case 1: changeFAS(value); attributeName = "FAS"; break;
            case 2: changeSES(value); attributeName = "SES"; break;
            case 3: changeSAS(value); attributeName = "SAS"; break;
        }
        UpdateCarInfXML(PlayerInformation.Instance.account, PlayerInformation.Instance.carDriving, attributeName, value);
    }
    public void UpdateCarInfXML(string account,int id,string attriName,float value)
    {
        string carInfPath = Application.streamingAssetsPath + "/XML/PlayerCarInformation.xml";
        if (File.Exists(carInfPath))
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(carInfPath);
            XmlNodeList allInf = doc.SelectSingleNode("root").ChildNodes;
            foreach (XmlElement playerCar in allInf)
            {
                if (playerCar.GetAttribute("account") == account)
                {
                    foreach (XmlElement everyCar in playerCar.ChildNodes)
                    {
                        if (everyCar.GetAttribute("id")==id.ToString())
                        {
                            foreach (XmlAttribute temp in everyCar.Attributes)
                           {
                               if (temp.Name == attriName)
                                {
                                    temp.Value = value.ToString();
                                    doc.Save(carInfPath);
                                    return;
                                }
                           }
                        }
                    }
                }
            }
        }
    }
    public void readCarInfXML()
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
                        if (everyCar.GetAttribute("id") ==PlayerInformation.Instance.carDriving.ToString())
                        {
                            changeFES(50 + float.Parse(everyCar.GetAttribute("FES")) * 100);
                            allSliders[0].value = 50 + float.Parse(everyCar.GetAttribute("FES")) * 100;
                            allConditions[0].text = allSliders[0].value.ToString();

                            changeFAS(50 + float.Parse(everyCar.GetAttribute("FAS")) * 100);
                            allSliders[1].value = 50 + float.Parse(everyCar.GetAttribute("FAS")) * 100;
                            allConditions[1].text = allSliders[1].value.ToString();

                            changeSES(50 + float.Parse(everyCar.GetAttribute("SES")) * 100);
                            allSliders[2].value = 50 + float.Parse(everyCar.GetAttribute("SES")) * 100;
                            allConditions[2].text = allSliders[2].value.ToString();

                            changeSAS(50 + float.Parse(everyCar.GetAttribute("SAS")) * 100);
                            allSliders[3].value = 50 + float.Parse(everyCar.GetAttribute("SAS")) * 100;
                            allConditions[3].text = allSliders[3].value.ToString();
                            return;
                        }
                    }
                }
            }
        }
    }
    public WheelFrictionCurve getNewWheelFrictionCurve(float ES,float EV,float AS,float AV,float stif)
    {
        WheelFrictionCurve temp = new WheelFrictionCurve();
        temp.extremumSlip = ES;
        temp.extremumValue = EV;
        temp.asymptoteSlip = AS;
        temp.asymptoteValue = AV;
        temp.stiffness = stif;

        return temp;
    }
    public void changeFES(float offsetVale)
    {
        float newValue = origiForwardWheel.extremumSlip + offsetVale;
        if (newValue<0.1f)
        {
            newValue = 0.1f;
        }
        WheelFrictionCurve newCurve = getNewWheelFrictionCurve
       (
       newValue,
       wheels[0].forwardFriction.extremumValue,
       wheels[0].forwardFriction.asymptoteSlip,
       wheels[0].forwardFriction.asymptoteValue,
       wheels[0].forwardFriction.stiffness
       );
       foreach(WheelCollider temp in wheels)
       {
           temp.forwardFriction = newCurve;
       }
    }
    public void changeFAS(float offsetVale)
    {
        float newValue = origiForwardWheel.asymptoteSlip + offsetVale;
        if (newValue < 0.1f)
        {
            newValue = 0.1f;
        }
        WheelFrictionCurve newCurve = getNewWheelFrictionCurve
       (
       wheels[0].forwardFriction.extremumSlip,
       wheels[0].forwardFriction.extremumValue,
       newValue,
       wheels[0].forwardFriction.asymptoteValue,
       wheels[0].forwardFriction.stiffness
       );
        foreach (WheelCollider temp in wheels)
        {
            temp.forwardFriction = newCurve;
        }
    }
    public void changeSES(float offsetVale)
    {
        float newValue = origiSlidwaysWheel.extremumSlip + offsetVale;
        if (newValue < 0.1f)
        {
            newValue = 0.1f;
        }
        WheelFrictionCurve newCurve = getNewWheelFrictionCurve
       (
       newValue,
       wheels[0].sidewaysFriction.extremumValue,
       wheels[0].sidewaysFriction.asymptoteSlip,
       wheels[0].sidewaysFriction.asymptoteValue,
       wheels[0].sidewaysFriction.stiffness
       );
        foreach (WheelCollider temp in wheels)
        {
            temp.sidewaysFriction = newCurve;
        }
    }
    public void changeSAS(float offsetVale)
    {
        float newValue = origiSlidwaysWheel.asymptoteSlip + offsetVale;
        if (newValue < 0.1f)
        {
            newValue = 0.1f;
        }
        WheelFrictionCurve newCurve = getNewWheelFrictionCurve
       (
       wheels[0].sidewaysFriction.extremumSlip,
       wheels[0].sidewaysFriction.extremumValue,
       newValue,
       wheels[0].sidewaysFriction.asymptoteValue,
       wheels[0].sidewaysFriction.stiffness
       );
        foreach (WheelCollider temp in wheels)
        {
            temp.sidewaysFriction = newCurve;
        }
    }
}
