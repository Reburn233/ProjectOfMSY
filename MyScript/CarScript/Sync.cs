using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sync : MonoBehaviour 
{

    Transform carFramework;
	void Start () 
    {
        carFramework = transform.Find("Framework");
	}
	
	
	void FixedUpdate () 
    {
        transform.position = carFramework.position;
        transform.rotation = carFramework.rotation;
        
	}
}
