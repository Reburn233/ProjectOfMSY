using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager :MonoBehaviour
{
    private static CarManager instance;
    public static CarManager Instance
    {
        get
        {
            return instance;
        }
    }
    void Awake()
    {
        instance = this;
    }
	void Start () 
    {
		
	}
	
}
