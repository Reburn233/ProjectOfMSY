using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraFollow : MonoBehaviour 
{
    public Transform target;
    public float distance=1.5f;
    public float high = 0.9f;
    Quaternion q;
    Rigidbody targetRigi;
    Vector3 offset;
	void Start () 
    {
        targetRigi = target.GetComponent<Rigidbody>();

        q = Quaternion.Euler(0, target.eulerAngles.y, 0);
        transform.rotation = q;
        transform.position = target.position + q * new Vector3(0, high, -distance);
        //offset = transform.position - target.position;
	}

    public float m_speed = 1.0f;
	void FixedUpdate () 
    {
        Vector3 temp = new Vector3(0, targetRigi.velocity.y, 0);
        q = Quaternion.Euler(0, target.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * m_speed);
        transform.position = Vector3.Lerp(transform.position, target.position + q * new Vector3(0, high, -distance), Time.deltaTime * m_speed);

	}


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            print("c");
            distance = (distance == -0.32f) ? 1.7f : -0.32f;
            high = (high == 0.55f) ? 0.8f : 0.55f;
        }

    }
   
}
