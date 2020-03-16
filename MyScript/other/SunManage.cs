using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SunManage : MonoBehaviour 
{

    Light theSun;
	void Start () 
    {
        theSun = transform.GetComponent<Light>();
        transform.DORotate(new Vector3(180,20,0),10f).SetLoops(-1,LoopType.Yoyo);
	}
	
	
	void Update () 
    {
        theSun.intensity = (90-(transform.eulerAngles.x > 90 ? (transform.eulerAngles.x - 90) : (90 - transform.eulerAngles.x))) / 90;
	}
}
