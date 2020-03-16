using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallTexi : MonoBehaviour 
{
    public GameObject[] Role;
    public int TaskNum=1;

    [HideInInspector]
    public List<Transform> allTaxiPos;

    private List<Transform> availablePos;

    //是否正在执行任务
    [HideInInspector]
    public bool isRunning;

    //是否生成了任务
    [HideInInspector]
    public bool HadInite;
	void Start () 
    {
        allTaxiPos = new List<Transform>();
        availablePos = new List<Transform>();

        isRunning = false;
        Transform[] pos= GetComponentsInChildren<Transform>();
        foreach(Transform temp in pos)
        {
            if (!allTaxiPos.Contains(temp))
              allTaxiPos.Add(temp);
            availablePos.Add(temp);
        }
	}

	void Update () 
    {
        if (isRunning || HadInite)
            return;

        for (int i = 0; i < TaskNum;i++)
        {
            int whichPos = Random.Range(0,availablePos.Count);
            Instantiate(Role[Random.Range(0,Role.Length)],availablePos[whichPos]);
            availablePos.RemoveAt(whichPos);
        }
        HadInite = true;
	}
}
