using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class everyChickPoint : MonoBehaviour 
{
    Text timpTips;
    
	void Start () 
    {
        timpTips = GameObject.Find("Canvas/gameUI/RaceTips/Tips").GetComponent<Text>();
       
	}
	
	
	void Update () 
    {
		
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player")
        {
            if (gameObject.name == "endPos")
            {
                escapeRace.Instance.gameOverUI.SetActive(true);
                Time.timeScale = 0;
                return;
            }
            escapeRace.Instance.addTime();
            gameObject.SetActive(false);
        }
    }

}
