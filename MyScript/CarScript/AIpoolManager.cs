using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;
public class AIpoolManager : MonoBehaviour 
{
    public GameObject AIframe;
    public GameObject[] aiShape;
    public Transform[] roots;

    private Vector3 initePosNow;
    private static AIpoolManager instance;
    public static AIpoolManager Instance
    {
        get { return instance; }
    }

    public SpawnPool AIpool;
    [HideInInspector]
    public GameObject AIroot;
	void Start () 
    {
        instance = this;
        AIpool = GetComponent<SpawnPool>();
        AIroot = GameObject.Find("AIRoot");
        initePosNow = Vector3.zero;
        StartCoroutine(initeCars());
	}
	
	
	void Update () 
    {
        initeAIcars();
		if(Input.GetKeyDown(KeyCode.K))
        {
            print("+1");
           GameObject newAICar= instance.AIpool.Spawn(AIframe, AIroot.transform.position, Quaternion.identity).gameObject;
           newAICar.transform.SetParent(AIroot.transform,true);
           Transform shape = newAICar.transform.Find("shape");
           if (shape==null)
          {
             GameObject newCarShape= Instantiate<GameObject>(aiShape[Random.Range(0, aiShape.Length)], newAICar.transform);
             newCarShape.name = "shape";
          }
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            foreach (Transform temp in AIroot.GetComponentsInChildren<Transform>())
            {
                if (temp.name != "AIroot")
                {
                    AIpool.Despawn(temp);
                }
            }
        }
	}

    public void initeAIcars()
    {
        for (int i = 0; i < roots.Length; i++)
        {
            if (roots[i].gameObject.activeInHierarchy)
            {
                if (i + 1 <= roots.Length - 1)
                {
                    initePosNow = roots[i + 1].position;
                }
                else
                {
                    initePosNow = roots[i].position;
                }               
                break;
            }
        }
    }
    IEnumerator initeCars()
    {
        while(true)
        {
            Quaternion q = Quaternion.Euler(0,Random.Range(0,360),0);
            GameObject newAICar = instance.AIpool.Spawn(AIframe, initePosNow, q).gameObject;
            newAICar.transform.SetParent(AIroot.transform, true);
            newAICar.transform.position = new Vector3(newAICar.transform.position.x,0.2f, newAICar.transform.position.z);
            newAICar.GetComponent<AIcar>().isHaveVectort = false;
            Transform shape = newAICar.transform.Find("shape");
            if (shape == null)
            {
                GameObject newCarShape = Instantiate<GameObject>(aiShape[Random.Range(0, aiShape.Length)], newAICar.transform);
                newCarShape.GetComponent<BoxCollider>().enabled = false;
                newCarShape.name = "shape";
            }
            yield return new WaitForSeconds(3.0f);
        }
       
    }
}
