using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class gameUI : MonoBehaviour 
{
    public RectTransform settingPanelUI;
    private static gameUI instance;
    public static gameUI Instance
    {
        get { return instance; }
    }
	void Start () 
    {
        instance = this;
	}
	
	
	void Update () 
    {
		
	}

    public void exitGame()
    {
        //carCondition.Instance.recoverWheelsInf();
        SceneManager.LoadScene(1);
    }
    public void settingPanel()
    {
        if (settingPanelUI.localScale == Vector3.one)
        {
            settingPanelUI.localScale = Vector3.zero;
        }
        else
        {
            settingPanelUI.localScale = Vector3.one;
        }
    }

}
