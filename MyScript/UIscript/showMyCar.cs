using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class showMyCar : MonoBehaviour 
{
   public GameObject[] myCars;
   public float dragSpeed;
   public Text tips;
   public Button BuyBtn;
   public Text DriveTips;
    Transform starPOS;
    Transform endPOS;

    GameObject CarShowing;
   
    private static showMyCar instance;
    public static showMyCar Instance
    {
        get { return instance; }
    }


	void Start () 
    {
        instance = this;
        starPOS = transform.Find("starPos");
        endPOS = transform.Find("endPos");
        instantiateCar(0);
        UIinHouse.Instance.LoadInformationInXML(0);
	}


    public void instantiateCar(int index)
    {
        CarShowing = Instantiate<GameObject>(myCars[index], transform);
        CarShowing.transform.position = endPOS.position;
        CarShowing.transform.eulerAngles = new Vector3(0, 30, 0);
        CarShowing.transform.DOMove(starPOS.position, 1.0f);
    }


	void Update () 
    {
        DragCar();
	}

    //车辆拖拽显示逻辑
    public void DragCar()
    {      
        if(Input.GetMouseButton(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) 
            {
                if (hit.collider.tag == "car")
                {
                   //print(hit.collider.name);
                    float h = Input.GetAxis("Mouse X");
                    hit.collider.transform.eulerAngles += new Vector3(0,-h*dragSpeed,0);
                }
            }          
        }
        
    }

    //按键相应换车
    public void chooseCar(int index)
    {
        if (UIinHouse.Instance.checkPlayerCarXML(PlayerInformation.Instance.account, index))
        {
            tips.text = "已拥有";
            BuyBtn.interactable = false;
            DriveTips.text = "Drive";
        }
        else
        {
            tips.text = "未拥有";
            DriveTips.text = "Inexistence";
            BuyBtn.interactable = true;
        }
        if (PlayerInformation.Instance.carDriving == index)
        {
            DriveTips.text = "Driving";
        }

        if (UIinHouse.Instance.carNum == index)
            return;
        UIinHouse.Instance.carNum = index;
        UIinHouse.Instance.LoadInformationInXML(index);
        //PlayerInformation.Instance.carDriving = index + 1;
        CarShowing.transform.eulerAngles = new Vector3(0, 30, 0);
        CarShowing.transform.DOMove(endPOS.position, 1.0f).OnComplete(() => {
            Destroy(CarShowing);
            instantiateCar(index);
        });
    }

}
