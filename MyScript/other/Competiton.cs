using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.IO;
public enum RaceName
{
   _textRace,
   mainRace,
    lastRace
}

public class Competiton : MonoBehaviour 
{
//用于管理比赛相关的逻辑
    [HideInInspector]
    public  List<Transform> RaceRootList;
    public Text gameCount;
    GameObject player;
    public GameObject[] allRunWays;
    public GameObject[] AIcars;
    public GameObject AIframework;
    //根据名字获取相应的赛道
    public Dictionary<RaceName, GameObject> RaceDic;
    public Text raceTime;
    List<Transform> RaceRoot;
    GameObject carManager;

    //倒计时文本
    Text countText;

    [HideInInspector]
    public RaceName raceNow;
    [HideInInspector]
    public int AInum;

    private static Competiton instance;
    public static Competiton Instance
    {
        get { return instance; }
    }
    public void againCompition(RaceName name, int num)
    {
        gameCount.text = "0";
        raceTime.text = "0";
        foreach (Rigidbody temp in carManager.GetComponentsInChildren<Rigidbody>())
        {
            //print(temp.name);
            if (temp.name != "CarManage" && temp.tag != "Player")
            {
                Destroy(temp.gameObject);
            }
        }
        starCompition(name,num);
    }

    public void starCompition(int raceNum, int num)
    {
        if (raceNum < 0)
            return;

        if (raceNum<RaceDic.Count)
        {
            RaceRootList.Clear();
            //将赛道检查点存入容器
            foreach (Transform everyRoot in allRunWays[raceNum].GetComponentsInChildren<Transform>())
            {
                if (everyRoot.tag == "root")
                {
                    RaceRootList.Add(everyRoot);
                }

            }

            AInum = num;
            AInum = Mathf.Clamp(AInum, 1, 5);
            allRunWays[raceNum].SetActive(true);
            Transform starPOS = GameObject.Find("starPos").transform;
            Transform[] allPos = starPOS.GetComponentsInChildren<Transform>();
            for (int i = 0; i < AInum; i++)
            {
                GameObject carTemp = Instantiate<GameObject>(AIframework, allPos[i + 1]);
                GameObject carShape = Instantiate<GameObject>(AIcars[Random.Range(0, AIcars.Length)], carTemp.transform);
                carTemp.name = PlayerInformation.Instance.AInames[Random.Range(0, PlayerInformation.Instance.AInames.Length)];
                carTemp.GetComponent<AIcar>().isHaveVectort = true;
                carShape.GetComponent<BoxCollider>().enabled = false;
                
                carTemp.transform.SetParent(carManager.transform);
                carTemp.GetComponent<Rigidbody>().isKinematic = true;
            }
            player.transform.position = allPos[AInum + 1].position;
            player.transform.rotation = allPos[AInum + 1].rotation;
            //player.GetComponent<Rigidbody>().isKinematic = true;
            player.GetComponent<Car>().isAvaliable = false;
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            StartCoroutine(CountDownToBegin());
            
        }
    }

    //传一个赛道名，激活相应赛道，将车辆放置在指定位置
    public void starCompition(RaceName name,int num)
    {
        if (RaceDic.ContainsKey(name))
       {
           RaceRootList.Clear();
            //将赛道检查点存入容器
           foreach (Transform everyRoot in RaceDic[name].GetComponentsInChildren<Transform>())
           {
               if (everyRoot.tag == "root")
               {
                   RaceRootList.Add(everyRoot);
               }

           }

           raceNow = name;
           AInum = num;
           num = Mathf.Clamp(num,1,5);
           RaceDic[name].SetActive(true);
           Transform starPOS = GameObject.Find("starPos").transform;
           Transform[] allPos = starPOS.GetComponentsInChildren<Transform>();
           for (int i = 0; i < num; i++)
           {
              GameObject carTemp = Instantiate<GameObject>(AIframework, allPos[i + 1]);
              GameObject carShape = Instantiate<GameObject>(AIcars[Random.Range(0, AIcars.Length)], carTemp.transform);
              carTemp.name = PlayerInformation.Instance.AInames[Random.Range(0, PlayerInformation.Instance.AInames.Length)];
              carShape.GetComponent<BoxCollider>().enabled = false;
              carTemp.GetComponent<AIcar>().isHaveVectort = true;
              carTemp.transform.SetParent(carManager.transform);
              carTemp.GetComponent<Rigidbody>().isKinematic = true;

           }
           player.transform.position = allPos[num+1].position;
           player.transform.rotation = allPos[num + 1].rotation;
           //player.GetComponent<Rigidbody>().isKinematic = true;
           player.GetComponent<Car>().isAvaliable = false;
           player.GetComponent<Rigidbody>().velocity = Vector3.zero;
           StartCoroutine(CountDownToBegin());
       }
    }

    IEnumerator CountDownToBegin()
    {
      
        int index = 3;
        while (true)
        {
            countText.color = Color.green;
            countText.text = index.ToString();
            yield return new WaitForSeconds(1.0f);
            if (index-- <=0)
                break;
        }
        countText.gameObject.SetActive(false);
        foreach (Rigidbody temp in carManager.GetComponentsInChildren<Rigidbody>())
        {
            //print(temp.name);
            if (temp.name != "CarManage")
            {
                temp.isKinematic = false;
            }
            if(temp.tag=="Player")
            {
                player.GetComponent<Car>().isAvaliable = true;
            }
        }
    }

    public void RaceEnd()
    {
        foreach (var temp in RaceDic)
        {
            temp.Value.SetActive(false);
        }
    }
	void Start () 
    {
        Time.timeScale = 1;
        RaceRootList = new List<Transform>();
        instance = this;
        RaceRoot = new List<Transform>();
        carManager = GameObject.Find("CarManage");
        player = GameObject.FindWithTag("Player");
        player.name = PlayerInformation.Instance.playerName;
        countText = GameObject.Find("GameUI/gameUI/CountDown").GetComponent<Text>();

         RaceDic = new Dictionary<RaceName, GameObject>() 
        {
            {RaceName._textRace,allRunWays[0]},
            {RaceName.mainRace,allRunWays[1]},
            {RaceName.lastRace,allRunWays[2]}
        };

         if (PlayerInformation.Instance != null && PlayerInformation.Instance.whichRace >= 0)
        {
            AInum = Mathf.Clamp(PlayerInformation.Instance.whichRace*3, 1, 6);
            starCompition(PlayerInformation.Instance.whichRace, AInum);
            
        }
         MusicManager.Instance.PlayBGMusic();
	}
	
	
	void Update () 
    {
	}

    public void LoadRecord(RaceName raceName, string playerName,float time)
    {       
        string filePath = Application.streamingAssetsPath + "/XML/RaceRecord.xml";
        if(File.Exists(filePath))
        {
            List<string> nameRank = new List<string>() { "","",""};
            List<float> timeRank = new List<float>() { 0,0,0};
            XmlDocument myDoc = new XmlDocument();
            myDoc.Load(filePath);
            XmlNodeList nodeList = myDoc.SelectSingleNode("root").ChildNodes;
            //获得XML中赛道纪录，将纪录存入容器中
            foreach (XmlElement tempRace in nodeList)
            {
                if (tempRace.Name == raceName.ToString())
                {
                    foreach (XmlAttribute RaceInf in tempRace.Attributes)
                    {       
                        if (RaceInf.Name == "FName")
                        {
                            nameRank[0] = RaceInf.Value;
                        }
                        else if (RaceInf.Name == "SName")
                        {
                            nameRank[1] = RaceInf.Value;
                        }
                        else if (RaceInf.Name == "TName")
                        {
                            nameRank[2] = RaceInf.Value;
                        }
                        else if (RaceInf.Name == "FiresRecord")
                        {
                            timeRank[0]=float.Parse(RaceInf.Value);
                        }
                        else if (RaceInf.Name == "SecondRecord")
                        {
                            timeRank[1] = float.Parse(RaceInf.Value);
                        }
                        else if (RaceInf.Name == "ThirdRecord")
                        {
                            timeRank[2] = float.Parse(RaceInf.Value);
                        }
                    }
                }
            }
            //与传入数据相比较，若更快则存入容器中
            for (int i = 0; i < timeRank.Count; i++)
            {
                if (time < timeRank[i])
                {
                    for (int j = timeRank.Count - 1; j >= i + 1; j--)
                    {
                        timeRank[j] = timeRank[j - 1];
                        nameRank[j] = nameRank[j - 1];
                    }
                    timeRank[i] = time;
                    nameRank[i] = playerName;
                    break;
                }
            }

            foreach (XmlElement tempRace in nodeList)
            {
                if (tempRace.Name == raceName.ToString())
                {
                    foreach (XmlAttribute RaceInf in tempRace.Attributes)
                    {
                        if (RaceInf.Name == "FName")
                        {
                            RaceInf.Value = nameRank[0];
                        }
                        else if (RaceInf.Name == "SName")
                        {
                            RaceInf.Value = nameRank[1];
                        }
                        else if (RaceInf.Name == "TName")
                        {
                            RaceInf.Value = nameRank[2];
                        }
                        else if (RaceInf.Name == "FiresRecord")
                        {
                            RaceInf.Value = timeRank[0].ToString();
                        }
                        else if (RaceInf.Name == "SecondRecord")
                        {
                            RaceInf.Value = timeRank[1].ToString();
                        }
                        else if (RaceInf.Name == "ThirdRecord")
                        {
                            RaceInf.Value = timeRank[2].ToString();
                        }
                    }
                }
            }
            myDoc.Save(filePath);
            nameRank.Clear();
            timeRank.Clear();
        }
    }
}
