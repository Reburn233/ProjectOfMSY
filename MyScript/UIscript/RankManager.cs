using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class RankManager : MonoBehaviour 
{
    public static int completeNum=0;
	
	void Start () 
    {
        gameObject.SetActive(false);
	}
	
	
	void Update () 
    {
		
	}
    public void ExitPanel()
    {
        for (int i = 0; i < completeNum; i++)
        {
            Text PlayerScore = GameObject.Find("RankPanel/Viewport/Content/Player0" + (i + 1).ToString() + "/score").GetComponent<Text>();
            PlayerScore.text = null;
        }
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
    public void againRace()
    {
        Competiton.Instance.raceNow = (RaceName)PlayerInformation.Instance.whichRace;
        Competiton.Instance.againCompition(Competiton.Instance.raceNow, Competiton.Instance.AInum);
        ExitPanel();
    }
    public void BackMainMnue()
    {
        if (PlayerInformation.Instance.gameLevel==(int)Competiton.Instance.raceNow+1)
        PlayerInformation.Instance.gameLevel = Mathf.Clamp(++PlayerInformation.Instance.gameLevel,0,10);
        PlayerInformation.Instance.backTOxml();
        //print(PlayerInformation.Instance.gameLevel);
        PlayerInformation.Instance.SP += 100;
        SceneManager.LoadScene(1);
    }
    
}
