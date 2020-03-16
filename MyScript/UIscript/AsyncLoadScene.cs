using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class AsyncLoadScene : MonoBehaviour 
{
    public Slider m_slider;
    private AsyncOperation m_async;
    private int nowPross;
    private int totalProcess=0;

    private AsyncLoadScene instance;
    public AsyncLoadScene Instance
    {
        get { return instance; }
    }


    public void Start()
    {
        //DontDestroyOnLoad(gameObject);
        instance = this;
        PlayerInformation.Instance.asyUI = gameObject;
        gameObject.SetActive(false);
    }

    public void changeScene(string sceneName)
    {
        StartCoroutine(Loading(sceneName));
    }
	void Update () 
    {
		if(m_async!=null)
        {
            if (m_async.progress <= 0.85)
            {
                totalProcess = (int)(m_async.progress * 100);
            }
            else
            {
                totalProcess = 100;
            }
            if(nowPross <totalProcess)
            {
                nowPross++;
            }
            m_slider.value = nowPross;
            if(nowPross==100)
            {
                m_async.allowSceneActivation = true;
            }
        }
	}
    IEnumerator Loading(string sceneName)
    {
        m_async = SceneManager.LoadSceneAsync(sceneName);
        m_async.allowSceneActivation = false;
        //PlayerInformation.Instance.asyUI = null;
     
        yield return m_async;
    }
}
