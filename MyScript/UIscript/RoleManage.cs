using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleManage : MonoBehaviour 
{
    Animator RoleAnimator;
	
	void Start () 
    {
        RoleAnimator = GetComponentInChildren<Animator>();
        //print(RoleAnimator.gameObject.name);
	}
	
	
	void Update () 
    {
		
	}

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag=="Player")
        {

        }
    }

}
