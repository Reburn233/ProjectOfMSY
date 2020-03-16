using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChangeVoice : MonoBehaviour 
{
    public Slider BGslider;
    public Slider effectSlider;

    public Text BGtext;
    public Text EffectText;

	void Start () 
    {
		
	}
	

	void Update () 
    {
		
	}
   public void BGchange()
    {
        int value =(int)BGslider.value;
        BGtext.text = "BGM:" + value.ToString() + "%";
        MusicManager.Instance.SetBGVolume((float)value/100);
        print((float)value / 100);
    }
   public void effectChange()
    {
        int value = (int)effectSlider.value;
        EffectText.text = "SOUND:" + value.ToString() + "%";
        MusicManager.Instance.SetEffVolume((float)value/100);
    }
   public void BtnPress(int num)
   {
       MusicManager.Instance.PlayEffMusic("click");
       switch (num)
       {
           case 1: BGslider.value+=1; break;
           case 2: BGslider.value -= 1; break;
           case 3: effectSlider.value+=1; break;
           case 4: effectSlider.value -= 1; break;
       }
       BGchange();
       effectChange();
   }
}
