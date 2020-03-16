using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class SmallMapFollow : MonoBehaviour 
{

    Transform player;
	void Start () 
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
	}
	
	
	void Update () 
    {
        transform.position = new Vector3(player.position.x,transform.position.y,player.position.z);
	}
}
