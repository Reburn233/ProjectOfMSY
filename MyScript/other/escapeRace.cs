using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class escapeRace : MonoBehaviour 
{
    public int alltime;
    public Text timeTips;
    public GameObject gameOverUI;
    public Text addTimeUI;
    public int everyChickPointTime=20;
    List<Transform> allRoot;
    private static escapeRace instance;
    public static escapeRace Instance
    {
        get { return instance; }
    }


	void Start () 
    {
        instance = this;
        allRoot = new List<Transform>();
        foreach(Transform temp in gameObject.GetComponentsInChildren<Transform>())
        {
            if (temp.tag=="CheckPoint")
            {
                allRoot.Add(temp);
            }
        }
        gameOverUI.SetActive(false);
        StartCoroutine("timeGoDown");
        //StartCoroutine(initeAIcars());
        MusicManager.Instance.PlayBGMusic();
	}
	
	
	void Update () 
    {
		
	}
    IEnumerator initeAIcars()
    {
        
        while(true)
        {
            for (int i = 0; i < allRoot.Count;i++)
            {
                print(allRoot[i].name);
                if (allRoot[i].gameObject.activeInHierarchy==true)
                {
                   Transform go= AIpoolManager.Instance.AIpool.Spawn("AI"+i.ToString(),allRoot[i].position,Quaternion.identity);
                   go.GetComponent<AIcar>().isHaveVectort = false;
                    break;
                }
            }

                AIpoolManager.Instance.AIpool.Spawn("AI"+0.ToString(),allRoot[0].position,Quaternion.identity);
            yield return new WaitForSeconds(1.0f);
        }
        
    }
    IEnumerator timeGoDown()
    {
        while(true)
        {
            if (alltime-- <=0)
            {
                gameOverUI.SetActive(true);
                Time.timeScale = 0;
                alltime = 0;
                break;
            }
            timeTips.text = alltime.ToString();
            yield return new WaitForSeconds(1.0f);
        }
        
    }

    public void againRace()
    {
        Time.timeScale = 1;
        gameOverUI.SetActive(false);
        foreach(Transform temp in allRoot)
        {
            temp.gameObject.SetActive(true);
        }
       foreach(Transform temp in gameObject.GetComponentsInChildren<Transform>())
       {
           temp.gameObject.SetActive(true);
           if (temp.name == "beginPos")
           {
               GameObject.FindWithTag("Player").transform.forward = temp.transform.forward;
               GameObject.FindWithTag("Player").transform.position = temp.position;
               GameObject.FindWithTag("Player").GetComponent<Rigidbody>().velocity = Vector3.zero;
           }
       }
       alltime = 40;
       timeTips.text = alltime.ToString();
       StopCoroutine("timeGoDown");
       StartCoroutine("timeGoDown");
    }

    public void exitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void addTime()
    {
        addTimeUI.text = "+" + everyChickPointTime.ToString()+"S";
        addTimeUI.DOFade(1, 1.0f).OnComplete(() => { addTimeUI.DOFade(0, 1.0f); alltime += everyChickPointTime; });
    }
}
