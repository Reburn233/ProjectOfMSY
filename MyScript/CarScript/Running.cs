using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Running : MonoBehaviour 
{
    public float maxSpeed;
    public float breakPower;
    public float breakRotatePower;
    public float constRotate;
    public float nowel;

    public Transform wheelFL;
    public Transform wheelFR;
    public Transform wheelBL;
    public Transform wheelBR;

    private Rigidbody m_rigid;
    private Vector3 nowSpeed;
    private Vector2 nowButton;
    public float horsepower;
    public float turnRL;

    
    public bool isplayer=false;

	void Start () 
    {
        m_rigid = GetComponent<Rigidbody>();
        nowButton = new Vector2(0,0);
	}
    void FixedUpdate()
    {
        //Debug.DrawRay(m_rigid.transform.position,m_rigid.velocity, Color.red,15.0f);
        print(Vector3.Dot(m_rigid.velocity.normalized, transform.forward));
        //if (m_rigid.velocity.normalized!=transform.forward)
        //{
        //    m_rigid.velocity -= m_rigid.velocity.normalized * nowel;
        //}
       

        if (isplayer==false)
        {
            return;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            nowButton.x += 1;
           givePower(1, 0);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            nowButton.x += -1;
            givePower(-1, 0);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            
            nowButton.y += 1;
            givePower(0, 1);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
           
            nowButton.y += -1;
            givePower(0, -1);
        }

        if (Input.GetKey(KeyCode.Space) && (m_rigid.velocity.magnitude >= 0.02 || m_rigid.velocity.magnitude <= -0.02))
        {
            Vector3 breakVector = m_rigid.velocity.normalized;
            m_rigid.AddForce(-breakVector * breakPower);

            m_rigid.AddForceAtPosition(breakVector * breakRotatePower, wheelBL.position);
            m_rigid.AddForceAtPosition(breakVector * breakRotatePower, wheelBR.position);

            m_rigid.AddForceAtPosition(-breakVector * breakRotatePower, wheelFL.position);
            m_rigid.AddForceAtPosition(-breakVector * breakRotatePower, wheelFR.position);
        }

        Vector3 nowVector = m_rigid.velocity.normalized;
        float speed = Mathf.Clamp(m_rigid.velocity.magnitude,-maxSpeed,maxSpeed);
        m_rigid.velocity = speed * nowVector;
   
        //print(m_rigid.rotation.eulerAngles.x);
        //float x = Mathf.Clamp(m_rigid.rotation.eulerAngles.x, 34, 320);
        //float y = Mathf.Clamp(m_rigid.rotation.eulerAngles.y, 0, 90);
        //float z = Mathf.Clamp(m_rigid.rotation.eulerAngles.z, 0, 90);
        //m_rigid.rotation = Quaternion.Euler(new Vector3(x, m_rigid.rotation.eulerAngles.y, m_rigid.rotation.eulerAngles.z));


        nowButton = Vector2.zero;
    }
	
	void Update () 
    {
       

       
	}

    void givePower(int isForward,int isRight)
    {
        m_rigid.AddForceAtPosition(m_rigid.transform.forward * horsepower * isForward, wheelFL.position);
        m_rigid.AddForceAtPosition(m_rigid.transform.forward * horsepower * isForward, wheelFR.position);
        m_rigid.AddForceAtPosition(m_rigid.transform.forward * horsepower * isForward, wheelBL.position);
        m_rigid.AddForceAtPosition(m_rigid.transform.forward * horsepower * isForward, wheelBR.position);

        print(Vector3.Dot(m_rigid.velocity, transform.forward));
        if (Vector3.Dot(m_rigid.velocity,transform.forward)>0.2f)
        {
            m_rigid.rotation = Quaternion.Euler(new Vector3(0, constRotate, 0) * isRight + m_rigid.rotation.eulerAngles);
            m_rigid.AddForceAtPosition(isRight * m_rigid.transform.right * ((maxSpeed - nowSpeed.magnitude) / maxSpeed) * horsepower * turnRL, wheelFL.position);
            m_rigid.AddForceAtPosition(isRight * m_rigid.transform.right * ((maxSpeed - nowSpeed.magnitude) / maxSpeed) * horsepower * turnRL, wheelFR.position);

            m_rigid.AddForceAtPosition(-isRight * m_rigid.transform.right * ((maxSpeed - nowSpeed.magnitude) / maxSpeed) * horsepower * turnRL, wheelBL.position);
            m_rigid.AddForceAtPosition(-isRight * m_rigid.transform.right * ((maxSpeed - nowSpeed.magnitude) / maxSpeed) * horsepower * turnRL, wheelBR.position);
        }
        else if (Vector3.Dot(m_rigid.velocity, transform.forward) < -0.2f)
        {
            m_rigid.rotation = Quaternion.Euler(new Vector3(0, constRotate, 0) * (-isRight) + m_rigid.rotation.eulerAngles);
            m_rigid.AddForceAtPosition(-isRight * m_rigid.transform.right * ((maxSpeed - nowSpeed.magnitude) / maxSpeed) * horsepower * turnRL, wheelFL.position);
            m_rigid.AddForceAtPosition(-isRight * m_rigid.transform.right * ((maxSpeed - nowSpeed.magnitude) / maxSpeed) * horsepower * turnRL, wheelFR.position);

            m_rigid.AddForceAtPosition(isRight * m_rigid.transform.right * ((maxSpeed - nowSpeed.magnitude) / maxSpeed) * horsepower * turnRL, wheelBL.position);
            m_rigid.AddForceAtPosition(isRight * m_rigid.transform.right * ((maxSpeed - nowSpeed.magnitude) / maxSpeed) * horsepower * turnRL, wheelBR.position);
        }
        
    }

    public void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag=="car")
        {

        }
    }


}
