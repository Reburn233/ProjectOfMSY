using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/************************************************************************/
/* 1、脚本要泛型单例Singleton<T>一起运行
 * 2、脚本需要挂载到游戏对象上
 * 3、所有的音频需要放在Resources\Audios路径下
/************************************************************************/


public class MusicManager : Singleton<MusicManager>
{
    [HideInInspector]
    public AudioSource m_bgSource; //背景音乐声音源
    [HideInInspector]
    public AudioSource m_effSource; //音效声音源
    //存储所有音频的容器  key是音频的名字  value是音频的片段
    List<AudioClip> m_BGclipsDic = new List<AudioClip>();
    List<AudioClip> m_GameclipsDic = new List<AudioClip>();
    List<AudioClip> m_RankclipsDic = new List<AudioClip>();
    Dictionary<string, AudioClip> m_EffectclipsDic = new Dictionary<string, AudioClip>();

    protected override void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        base.Awake();
        StartCoroutine(loadMusic());
        //AudioClip[] BGClips = Resources.LoadAll<AudioClip>("Audios/BG");
        //foreach (AudioClip temp in BGClips)
        //{
        //    if (!m_BGclipsDic.Contains(temp))
        //    {
        //        //print(temp.name);
        //        m_BGclipsDic.Add(temp);
        //    }
        //}
        //AudioClip[] GameClips = Resources.LoadAll<AudioClip>("Audios/Game");
        //foreach (AudioClip temp in BGClips)
        //{
        //    if (!m_GameclipsDic.Contains(temp))
        //    {
        //        m_GameclipsDic.Add(temp);
        //    }
        //}
        //AudioClip[] RankClips = Resources.LoadAll<AudioClip>("Audios/Rank");
        //foreach (AudioClip temp in BGClips)
        //{
        //    if (!m_RankclipsDic.Contains(temp))
        //    {
        //        m_RankclipsDic.Add(temp);
        //    }
        //}
        //AudioClip[] EffectClips = Resources.LoadAll<AudioClip>("Audios/Effect");
        //foreach (AudioClip temp in EffectClips)
        //{
        //    if (!m_EffectclipsDic.ContainsKey(temp.name))
        //    {
        //        m_EffectclipsDic.Add(temp.name, temp);
        //    }
        //}

        ////使用代码添加两个AudioSource
        //m_bgSource = gameObject.AddComponent<AudioSource>();
        //m_bgSource.playOnAwake = false;
        //m_bgSource.loop = true;
        //m_effSource = gameObject.AddComponent<AudioSource>();
        //m_effSource.playOnAwake = false;
    }

    IEnumerator loadMusic()
    {
        yield return new WaitForSeconds(5.0f);
        AudioClip[] BGClips = Resources.LoadAll<AudioClip>("Audios/BG");
        foreach (AudioClip temp in BGClips)
        {
            if (!m_BGclipsDic.Contains(temp))
            {
                //print(temp.name);
                m_BGclipsDic.Add(temp);
            }
        }
        AudioClip[] GameClips = Resources.LoadAll<AudioClip>("Audios/Game");
        foreach (AudioClip temp in BGClips)
        {
            if (!m_GameclipsDic.Contains(temp))
            {
                m_GameclipsDic.Add(temp);
            }
        }
        AudioClip[] RankClips = Resources.LoadAll<AudioClip>("Audios/Rank");
        foreach (AudioClip temp in BGClips)
        {
            if (!m_RankclipsDic.Contains(temp))
            {
                m_RankclipsDic.Add(temp);
            }
        }
        AudioClip[] EffectClips = Resources.LoadAll<AudioClip>("Audios/Effect");
        foreach (AudioClip temp in EffectClips)
        {
            if (!m_EffectclipsDic.ContainsKey(temp.name))
            {
                m_EffectclipsDic.Add(temp.name, temp);
            }
        }

        //使用代码添加两个AudioSource
        m_bgSource = gameObject.AddComponent<AudioSource>();
        m_bgSource.playOnAwake = false;
        m_bgSource.loop = true;
        m_effSource = gameObject.AddComponent<AudioSource>();
        m_effSource.playOnAwake = false;
    }
    //播放背景音乐
    public void PlayBGMusic()
    {
        if (m_bgSource != null)
        {
            m_bgSource.clip = m_BGclipsDic[Random.Range(0, m_BGclipsDic.Count)];
            m_bgSource.Play();
        }
    }
    public void PlayGameMusic()
    {
        if (m_bgSource != null)
        {
            m_bgSource.clip = m_GameclipsDic[Random.Range(0, m_BGclipsDic.Count)];
            m_bgSource.Play();
        }
    }
    public void PlayRankMusic()
    {
        if (m_bgSource != null)
        {
            m_bgSource.clip = m_RankclipsDic[Random.Range(0, m_BGclipsDic.Count)];
            m_bgSource.Play();
        }
    }

    //播放音效
    public void PlayEffMusic(string _name)
    {
        AudioClip clip;
        if (m_effSource != null && m_EffectclipsDic.TryGetValue(_name, out clip))
        {
            //print(_name);
            m_effSource.clip = m_EffectclipsDic[_name];
            m_effSource.Play();
        }
    }
    //设置背景音乐的声音大小
    public void SetBGVolume(float _value)
    {
        if(m_bgSource != null)
        {
            m_bgSource.volume = _value;
        }
    }
    //设置音效的声音大小
    public void SetEffVolume(float _value)
    {
        if (m_effSource != null)
        {
            m_effSource.volume = _value;
        }
    }
    //是否将背景音乐静音
    public void SetBGMute(bool isMute)
    {
        if (m_bgSource != null)
        {
            m_bgSource.mute = isMute;
        }
    }
    //是否将音效静音
    public void SetEffMute(bool isMute)
    {
        if (m_effSource != null)
        {
            m_effSource.mute = isMute;
        }
    }
}
