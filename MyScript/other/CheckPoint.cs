using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class CheckPoint : MonoBehaviour 
{
    Dictionary<GameObject,int> completeTime;
    List<GameObject> carRank;
    List<float> timeRank;

    private int loop=1;
    private float gameTime = 0;
    public int totalLoop = 2;
    public Text timing;
    public int CountDown;
    GameObject RankPanel;
    GameObject GameUI;
    //倒计时
    Text countText;

    bool IEstart;
    float beginTime = 0;
	void Start () 
    {
        beginTime = 0;
        IEstart = false;
        carRank = new List<GameObject>();
        timeRank = new List<float>();
        completeTime = new Dictionary<GameObject, int>();
        GameUI = GameObject.Find("GameUI");
        RankPanel = GameUI.transform.Find("gameUI/RankPanel").gameObject;
        //RankPanel = GameObject.FindWithTag("Rank");
        RankPanel.SetActive(false);
        countText = GameObject.Find("CountDown").GetComponent<Text>();
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag=="AI")
        {
            if (!completeTime.ContainsKey(other.gameObject))
            {
                completeTime.Add(other.gameObject, 1);
                beginTime = Time.time;
            }
            else if (Vector3.Dot(other.GetComponent<Rigidbody>().velocity, transform.forward) > 0 && completeTime.ContainsKey(other.gameObject))
            {
                //赛车再次进入检查点
                if (completeTime[other.gameObject]++ >= totalLoop)
                {  
                    //比赛进入倒计时
                    if (carRank.Contains(other.gameObject))
                    {
                        return;
                    }
                    carRank.Add(other.gameObject);
                    timeRank.Add(float.Parse(timing.text));
                    StartCoroutine(CarStop(other.gameObject));
                    //倒计时
                    if (IEstart == false)
                    {
                        StartCoroutine(gameOver());
                        IEstart = true;
                    }
                  
                }
            }
            else
            {
                completeTime[other.gameObject]--;              
            }

        }
 
    }
    void Update()
    {
        if (beginTime!=0)
        {
            timing.text = (Time.time - beginTime).ToString().Substring(0, Mathf.Clamp(5, 0, (Time.time - beginTime).ToString().Length));
        }
    }
    IEnumerator gameOver()
    {
        countText.gameObject.SetActive(true);
        int lastTime = 0;
        while (true)
        {                    
            countText.text = (CountDown - lastTime).ToString();
            countText.DOFade(1, 0.5f).SetLoops(-1, LoopType.Yoyo);
            lastTime++;
            yield return new WaitForSeconds(1.0f);
            if (lastTime > CountDown)
            {                
                break;
            }
        }
       
        countText.color = Color.clear;
        RankPanel.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0;
        beginTime = 0;
        RankPanel.SetActive(true);
        List<GameObject> rank = new List<GameObject>();
        for (int i = 0; i < carRank.Count;i++ )
        {
            Text PlayerScore = GameObject.Find("RankPanel/Viewport/Content/Player0" + (i + 1).ToString() + "/score").GetComponent<Text>();
            PlayerScore.text ="  "+carRank[i].name + "  Time:" + timeRank[i].ToString();
            Competiton.Instance.LoadRecord(Competiton.Instance.raceNow, carRank[i].name, timeRank[i]);

            if (carRank[i].tag=="Player" && PlayerInformation.Instance!=null)
            {
                PlayerInformation.Instance.SP += ((int)Competiton.Instance.raceNow + 1) * (4 - i) * 1000;
                PlayerInformation.Instance.gameLevel++;
                PlayerInformation.Instance.backTOxml();
                
            }
        }
        RankManager.completeNum = carRank.Count;
        carRank.Clear();
        timeRank.Clear();
        completeTime.Clear();
        IEstart = false;
    }

    IEnumerator CarStop(GameObject go)
    {
        yield return new WaitForSeconds(1.0f);
        if (go!=null && go.tag == "Player")
        {
            go.GetComponent<Car>().isAvaliable = false;
        }
        else
        {
            go.GetComponent<Rigidbody>().isKinematic = true;
        }
        go.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
