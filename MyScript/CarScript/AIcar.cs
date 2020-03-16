using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIcar : MonoBehaviour 
{
    [HideInInspector]
    public bool isHaveVectort=true;
    private int RootNum=0;
    private Transform nextRoot;
    private Transform lastRoot;

    public WheelCollider FRwheel;
    public WheelCollider FLwheel;
    public WheelCollider BRwheel;
    public WheelCollider BLwheel;
    public Transform viewPos;

    public float maxMotorTorque; // 最大马力
    public float maxSteeringAngle; // 最大转弯角度
    public float maxSpeed;  //最大平跑速度
    public float maxShift; //最大过弯速度
    public float breakPower; //刹车制动力
    public float sliderSpeed; //氮气条
    public float maxSpeedUp;  //最大氮气加速度
    public float speedPower; //氮气加速马力
    public float speedUpTime;
    public float skyPower;
    public float viewDistance=25.0f;

    Rigidbody m_rigi;
    Transform lastTransform;

    //转向与动力的比例
    private float motor;
    private float steering;

    void Start()
    {
        m_rigi = GetComponent<Rigidbody>();        
    }

  
    public void FixedUpdate()
    {
        if (isHaveVectort)
        {
            //当车辆有路点可以参照时
            //判断车辆是否能行驶
            ReSetCar();
            //更新检查点
            UpdateRoot();
            //检测是否逆向行驶
            checkVector();
        }
        
        motor = maxMotorTorque *(judgeGoBack()==true? 1:-1);//获取前进或者后退的输入
        steering = maxSteeringAngle * giveHorizontal() * (judgeGoBack() == true ? 1 : -1);//获取水平轴的转弯输入
        //避免过于频繁的小幅度转向
        if ((steering < 3 && steering > -3))   
        {
            steering = 0;
        }
        //回盘
        vectorBack();
        BRwheel.motorTorque = motor;
        BLwheel.motorTorque = motor;
        FRwheel.motorTorque = motor;
        FLwheel.motorTorque = motor;
        FRwheel.steerAngle = steering;
        FLwheel.steerAngle = steering;
       //限制速度
        m_rigi.velocity = m_rigi.velocity.normalized * 
            (Mathf.Clamp(m_rigi.velocity.magnitude, 0, maxSpeed));
        //过弯限速
        if (FRwheel.steerAngle > maxSteeringAngle / 2 || FRwheel.steerAngle < -maxSteeringAngle / 2)
        {
            m_rigi.velocity = m_rigi.velocity.normalized *
           (Mathf.Clamp(m_rigi.velocity.magnitude, 0, maxShift));
        }
      
    }


    void ReSetCar()
    {
        if (m_rigi.velocity.magnitude <= 10)
        {
            if (BRwheel.isGrounded == false && BLwheel.isGrounded == false && FLwheel.isGrounded == false && FRwheel.isGrounded == false)
            {
                transform.eulerAngles = Vector3.zero;
                if (Competiton.Instance.RaceRootList != null && Competiton.Instance.RaceRootList.Count != 0)
                {
                    transform.LookAt(Competiton.Instance.RaceRootList[(RootNum) % Competiton.Instance.RaceRootList.Count].position);
                }
                
            }

        }
    }
    void checkVector()
    {
        if (Competiton.Instance.RaceRootList == null || Competiton.Instance.RaceRootList.Count==0)
            return;
       
        Vector3 ForwardDir = Competiton.Instance.RaceRootList[(RootNum) % Competiton.Instance.RaceRootList.Count].position
            - Competiton.Instance.RaceRootList[(RootNum - 1 >= 0 ? (RootNum - 1) : Competiton.Instance.RaceRootList.Count-1) 
            % Competiton.Instance.RaceRootList.Count].position;
        if (Vector3.Dot(ForwardDir, transform.forward) <= 0 && Vector3.Dot(ForwardDir,m_rigi.velocity) <= 0)
        {
            //print(Vector3.Dot(ForwardDir, transform.forward));
            transform.eulerAngles = new Vector3(0,0,0);
            transform.LookAt(Competiton.Instance.RaceRootList[(RootNum) % Competiton.Instance.RaceRootList.Count].position);
        }
    }
    void UpdateRoot()
    {
        if (Competiton.Instance.RaceRootList == null || Competiton.Instance.RaceRootList.Count == 0)
            return;
        if (Vector3.Distance(transform.position,
            Competiton.Instance.RaceRootList[(RootNum) % Competiton.Instance.RaceRootList.Count].position) <= 5)
        {
            if (RootNum++ >= Competiton.Instance.RaceRootList.Count)
            {
                RootNum = 0;
            }
        }
    }
    void vectorBack()
    {
        //回盘逻辑
        if (Vector3.Dot(m_rigi.velocity.normalized, transform.forward) <= 0.85f)  // && m_rigi.velocity.magnitude > 1
        {
            if (Vector3.Dot(m_rigi.velocity, transform.right) > 0)
            {
                steering = maxSteeringAngle;
            }
            else if (Vector3.Dot(m_rigi.velocity, transform.right) < 0)
            {
                steering = -maxSteeringAngle;
            }
        }
    }
    float DrawRayToJudge(Vector3 temp)
    {
        Ray myRay = new Ray(viewPos.position, new Vector3(temp.x, 0, temp.z));
        RaycastHit hit;
        //Debug.DrawRay(viewPos.position, new Vector3(temp.x, 0, temp.z) * viewDistance);
        if (Physics.Raycast(myRay, out hit))    //, viewDistance
        {
            if (hit.collider.tag == "CheckPoint")
            {
                return 100;
            }
            //if (hit.distance<=4)
            //{
            //    return -100;
            //}
            return hit.distance;
        }
        return 100;
    }
    float giveHorizontal()
    {
        //若速度很小，则不进行转向判断
        if (m_rigi.velocity.magnitude < 2)
        {
            return 0;
        }
        //发射一簇扇形射线
        float rightDistance = 0;
        float LeftDistance = 0;
        for (int i = 180; i >= 0; i -= 15)
        {
            float f = Mathf.PI / 180 * i;
            //Debug.DrawRay(viewPos.position, transform.TransformDirection(new Vector3(Mathf.Cos(f), 0, Mathf.Sin(f))));
            float distance = DrawRayToJudge(transform.TransformDirection(new Vector3(Mathf.Cos(f), 0, Mathf.Sin(f))) * viewDistance);

            if (i > 90)
            {
                LeftDistance += distance;
            }
            else
            {
                rightDistance += distance;
            }

        }
        //print(rightDistance + "   " + LeftDistance);
        return (rightDistance > LeftDistance) ?
            (rightDistance - LeftDistance) / rightDistance :
            -(LeftDistance - rightDistance) / LeftDistance;


    }
    bool judgeGoBack()
    {
        if (DrawRayToJudge(transform.forward) < 1)
        {
            return false;
        }
        return true;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (isHaveVectort==true)
        {
            return;
        }
        if ((collision.collider.tag == "AI" && gameObject.GetComponent<Rigidbody>().velocity.magnitude <= 2))
        {
            AIpoolManager.Instance.AIpool.Despawn(transform);
        }
    }

    

    }




