using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
public class Car: MonoBehaviour
{
    public Sprite readyUP;
    public Sprite explodeUP;
   
    public WheelCollider FRwheel;
    public WheelCollider FLwheel;
    public WheelCollider BRwheel;
    public WheelCollider BLwheel;

    public bool isFourDrive=true;
    public float maxMotorTorque; // 最大马力
    public float maxSteeringAngle; // 最大转弯角度
    public float maxSpeed;  //最大平跑速度
    public float breakPower; //刹车制动力
    public float sliderSpeed; //氮气条
    public float maxSpeedUp;  //最大氮气加速度
    public float speedPower; //氮气加速马力
    public float speedUpTime;
    public float skyPower;
    public float brakeTorquePower;
    public float FrontRate = 0.8f;
    private float maxspeed;

    public Text showSpeed;
    public Image sliderControler;
    public Image explodePiceture;

    Rigidbody m_rigi;

    Image speedUpButton1;
    Image speedUpButton2;

    Light CarLightR;
    Light CarLightL;
    GameObject wind;
    [HideInInspector]
    public bool isAvaliable;
    [HideInInspector]
    public bool isInSky=false;

    //短涡轮是否正在冷却
    [HideInInspector]
    public bool canExplodeUP = true;

    //短涡轮是否就绪
    private bool ExplodeReady;
    public GameObject FireBack1;
    public GameObject FireBack2;
    public ParticleSystem[] fires;
    void Start()
    {
        changeCarShape();
        LoadCarAttribute();
        wind = transform.Find("Whirlwind").gameObject;
        ExplodeReady = false;
        isAvaliable = true;
        maxspeed = maxSpeed;

        m_rigi = GetComponent<Rigidbody>();
        speedUpButton1 = GameObject.FindWithTag("N2O1").GetComponent<Image>();
        speedUpButton2 = GameObject.FindWithTag("N2O2").GetComponent<Image>();
        speedUpButton1.color = Color.black;
        speedUpButton2.color = Color.black;
        CarLightR = transform.Find("Spotlight").GetComponent<Light>();
        CarLightL = transform.Find("Spotlight2").GetComponent<Light>();
    }


    public void FixedUpdate()
    {
        if (isAvaliable == false)
        {
            FRwheel.motorTorque = 0;
            FLwheel.motorTorque = 0;
            BRwheel.motorTorque = 0;
            BLwheel.motorTorque = 0;
            return;
        }
        sliderLogic();
        givePower();
        //balanceSystem(BRwheel, BLwheel);
        //balanceSystem(FRwheel, FLwheel);
        breakSpeed();
        controlInSkay();
        turbineUP();
        checkSpeed();
    }
    void checkSpeed()
    {
        if(m_rigi.velocity.magnitude<=maxspeed/4 && MusicManager.Instance.m_effSource.isPlaying==false)
        {
            //print(1);
            MusicManager.Instance.PlayEffMusic("beginSpeed");
        }
        else if (m_rigi.velocity.magnitude > maxspeed / 4 && MusicManager.Instance.m_effSource.isPlaying == false)
        {
            //print(2);
            MusicManager.Instance.PlayEffMusic("keepSpeed");
        }

        if (m_rigi.velocity.magnitude > maxspeed+1)
        {
            wind.SetActive(true);
        }
        else
        {
            wind.SetActive(false);
        }
    }
    public void changeCarShape()
    {
        if (PlayerInformation.Instance != null)
        {
            //print(PlayerInformation.Instance.carDriving);
            GameObject carShape = Instantiate<GameObject>(Resources.Load<GameObject>("CarPrefab/Car" + Mathf.Clamp(PlayerInformation.Instance.carDriving+1, 1, 15).ToString()), transform);
            carShape.GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            GameObject carShape = Instantiate<GameObject>(Resources.Load<GameObject>("CarPrefab/Car1"), transform);
            carShape.GetComponent<BoxCollider>().enabled = false;
        }
    }
    public void LoadCarAttribute()
    {
        maxMotorTorque = PlayerInformation.Instance.carInformation.MaxMotorTorque;
        maxSpeed = PlayerInformation.Instance.carInformation.MaxSpeed;
        sliderSpeed = PlayerInformation.Instance.carInformation.sliderSpeed;
        speedUpTime = PlayerInformation.Instance.carInformation.speedUpTime;
    }
    void turbineUP()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && ExplodeReady == true)
        {
            ExplodeReady = false;
            StartCoroutine(maxSpeedChange(0.1f));
            m_rigi.AddForce(transform.forward * speedPower *1000);
            //m_rigi.velocity = Vector3.Dot(m_rigi.velocity,transform.forward)*transform.forward;           
            explodePiceture.sprite = explodeUP;
            explodePiceture.DOFade(1, 0.2f).OnComplete(() =>
            {
                explodePiceture.DOFade(0, 1.0f).OnComplete(() => { explodePiceture.sprite = readyUP; StartCoroutine(explodeTime()); ExplodeReady = false; explodePiceture.sprite = readyUP;});
            });
        }

        if (canExplodeUP == false)
            return;
        Vector3 horizontalSpeed = new Vector3(m_rigi.velocity.x, 0, m_rigi.velocity.z).normalized;
        if (Vector3.Dot(horizontalSpeed, transform.forward) < 0.8 && Vector3.Dot(horizontalSpeed, transform.forward) > 0)
        {
            explodePiceture.sprite = readyUP;
            ExplodeReady = true;
            canExplodeUP = false;
            explodePiceture.DOFade(1,0.5f).OnComplete(() =>
            {
                explodePiceture.DOFade(0, 1.0f).OnComplete(() => { explodePiceture.sprite = readyUP; StartCoroutine(explodeTime()); ; ExplodeReady = false; explodePiceture.sprite = readyUP; });
            });
        }
    }
    IEnumerator explodeTime()
    {
        float expTime = 0;
        while(true)
        {
            yield return new WaitForSeconds(1.0f);
            if (expTime++>=2.0f)
            {
                canExplodeUP = true;
                break;
            }
        }
    }

    public void updateShowSpeed()
    {
        showSpeed.text = (((int)(m_rigi.velocity.magnitude * 10.0f)).ToString());
    }

    void givePower()
    {
        float motor = maxMotorTorque * Input.GetAxis("Vertical");//获取前进或者后退的输入
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");//获取水平轴的转弯输入

        BRwheel.motorTorque = motor;
        BLwheel.motorTorque = motor;
        if(isFourDrive)
        {
            FRwheel.motorTorque = motor;
            FLwheel.motorTorque = motor;
        }
        

        FRwheel.steerAngle = steering;
        FLwheel.steerAngle = steering;


        m_rigi.velocity = m_rigi.velocity.normalized * (Mathf.Clamp(m_rigi.velocity.magnitude, 0, maxSpeed));
        //checkWindclose();
    }
    void breakSpeed()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            FRwheel.brakeTorque = brakeTorquePower;
            FLwheel.brakeTorque = brakeTorquePower;
            BRwheel.brakeTorque = brakeTorquePower;
            BLwheel.brakeTorque = brakeTorquePower;
        }
        else
        {
            FRwheel.brakeTorque = 0;
            FLwheel.brakeTorque = 0;
            BRwheel.brakeTorque = 0;
            BLwheel.brakeTorque = 0;
        }
        
    }

    public void LateUpdate()
    {
        updateShowSpeed();
        keepState();
        if (isAvaliable == false)
        {
            return;
        }
        if(Input.GetKeyDown(KeyCode.PageDown))
        {
            CarLightR.intensity = Mathf.Clamp(CarLightR.intensity-5,0,30);
            CarLightL.intensity = Mathf.Clamp(CarLightL.intensity - 5, 0, 30);
        }
        else if (Input.GetKeyDown(KeyCode.PageUp))
        {
            CarLightR.intensity = Mathf.Clamp(CarLightR.intensity + 5, 0, 30);
            CarLightL.intensity = Mathf.Clamp(CarLightL.intensity + 5, 0, 30);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            m_rigi.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            speedUp(speedUpTime);
        }
       
       
    }
    void keepState()
    {
        if (FRwheel.isGrounded == false && FLwheel.isGrounded == false && BRwheel.isGrounded == false && BRwheel.isGrounded == false)
       {
           transform.eulerAngles = new Vector3(Mathf.Clamp(transform.eulerAngles.x, -5, 5), transform.eulerAngles.y, Mathf.Clamp(transform.eulerAngles.z, -5, 5));
           m_rigi.angularVelocity = new Vector3(0, m_rigi.angularVelocity.y,0);
       }
    }
    void sliderLogic()
    {
        Vector3 horizontalSpeed = new Vector3(m_rigi.velocity.x, 0, m_rigi.velocity.z).normalized;
        if ((Vector3.Dot(horizontalSpeed, transform.forward) < 0.8 && Vector3.Dot(horizontalSpeed, transform.forward) > 0) ||
              (FRwheel.isGrounded == false && FLwheel.isGrounded == false && BRwheel.isGrounded == false && BRwheel.isGrounded == false))
        {
            sliderControler.fillAmount += sliderSpeed;
            if (sliderControler.fillAmount == 1)
            {
                if (speedUpButton1.color==Color.black)
                {
                    speedUpButton1.color = Color.white;
                }
                else if (speedUpButton1.color == Color.white && speedUpButton2.color == Color.black)
                {
                    speedUpButton2.color = Color.white;
                }
               sliderControler.fillAmount = 0;
            }
        }
    }
    void speedUp(float UpTime)
    {
        if(m_rigi.velocity.magnitude>maxSpeed+1)
        return;
        if (speedUpButton2.color == Color.white)
        {
            speedUpButton2.color = Color.black;
        }
        else if (speedUpButton1.color == Color.white && speedUpButton2.color == Color.black)
        {
            speedUpButton1.color = Color.black;
        }
        else
        {
            return;
        }
        StartCoroutine(maxSpeedChange(UpTime));
    }
    IEnumerator maxSpeedChange(float UpTime)
    {
        float tempMax = maxSpeed;
        maxSpeed = maxSpeedUp;
        int timeAll = 0;
        Camera.main.fieldOfView += 5;
        foreach(var temp in fires)
        {
            temp.Play();
        }        
        wind.SetActive(true);
        while(true)
        {
          
            m_rigi.AddForce(transform.forward * speedPower);
            timeAll++;
            
            yield return new WaitForSeconds(0.1f);
            if (timeAll >= UpTime * 10)
            {
                break;
            }
        }
        Camera.main.fieldOfView -= 5;
        foreach (var temp in fires)
        {
            temp.Stop();
        }
        while(true)
        {
            yield return new WaitForSeconds(0.5f);
            if (maxSpeed-- <= tempMax)
            {
                wind.SetActive(false);               
                maxSpeed = tempMax;
                break;
            }
        }
    }

    void balanceSystem(WheelCollider TheWheelR, WheelCollider TheWheelL)
    {
        WheelHit hitL = new WheelHit();
        WheelHit hitR = new WheelHit();
        float travelL=0;
        float travelR = 0;
        float antiroll = TheWheelL.suspensionSpring.spring/100;
        bool groundedL = TheWheelL.GetGroundHit(out hitL);
        bool groundedR = TheWheelR.GetGroundHit(out hitR);

        if (groundedL)
        {
            travelL = (-TheWheelL.transform.InverseTransformPoint(hitL.point).y - TheWheelL.suspensionDistance);
        }
        else
        {
            travelL = 1.0f;
        }

        if (groundedR)
        {
            travelR = (-TheWheelR.transform.InverseTransformPoint(hitR.point).y - TheWheelR.suspensionDistance);
        }
        else
        {
            travelR = 1.0f;
        }

        var antiRollForce = (travelL - travelR) * antiroll;
        if (groundedL)
            m_rigi.AddForceAtPosition(transform.up * -antiRollForce, TheWheelL.transform.position);
        if (groundedR)
            m_rigi.AddForceAtPosition(transform.up * antiRollForce, TheWheelR.transform.position);
    }
    void controlInSkay()
    {
        Ray rayDis = new Ray(transform.position,Vector3.down);
        RaycastHit hit;
        if(Physics.Raycast(rayDis,out hit))
        {
            //print(hit.distance);
            if (hit.distance<1.0f)
            {
                return;
            }
        }
        if (FRwheel.isGrounded == false && FLwheel.isGrounded == false && BRwheel.isGrounded == false && BRwheel.isGrounded == false)
        {
            isInSky = true;
            //Time.timeScale = 0.5f;
            float h = Input.GetAxis("Horizontal") * 10;
            transform.eulerAngles += new Vector3(0, h, 0);
            m_rigi.AddForce(Vector3.forward *skyPower);
        }
        else
        {
            //Time.timeScale = 1;
            isInSky = false;
        }
    }

    //void checkWindclose()
    //{
    //    if (wind.activeInHierarchy == false)
    //        return;
    //    //print(m_rigi.velocity.magnitude + "  " + (maxSpeed * 0.5f).ToString());
    //    if (m_rigi.velocity.magnitude <= maxSpeed * 0.5f || Vector3.Dot(m_rigi.velocity,transform.forward)<=0.2f)
    //    {
    //        //print("close");
    //        wind.SetActive(false);
    //    }
    //}
}


