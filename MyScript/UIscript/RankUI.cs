using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.IO;
public class RankUI : MonoBehaviour 
{
    public Text raceInf;
	void Start () 
    {
        LoadXMLRankInformation(1);
	}

	void Update () 
    {
		
	}
    public void LoadXMLRankInformation(int num)
    {
        string filePath = Application.streamingAssetsPath + "/XML/RaceRecord.xml";
        if (File.Exists(filePath))
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlNodeList nodelist = doc.SelectSingleNode("root").ChildNodes;
            foreach(XmlElement everyRace in nodelist)
            {
                if(everyRace.GetAttribute("id")==num.ToString())
                {
                    raceInf.text = "";
                    raceInf.text += "             " + everyRace.GetAttribute("des") + "\n";
                    raceInf.text += "FiresRecord: " + everyRace.GetAttribute("FName") + "\n";
                    raceInf.text += "       TIME: " + everyRace.GetAttribute("FiresRecord") +"s"+ "\n";

                    raceInf.text += "SecondRecord: " + everyRace.GetAttribute("SName") + "\n";
                    raceInf.text += "       TIME: " + everyRace.GetAttribute("SecondRecord") + "s" + "\n";

                    raceInf.text += "ThirdRecord: " + everyRace.GetAttribute("TName") + "\n";
                    raceInf.text += "       TIME: " + everyRace.GetAttribute("ThirdRecord") + "s" + "\n";
                    return;
                }
            }
        }
    }
    public void ExitRankUI()
    {
        gameObject.SetActive(false);
    }
}
