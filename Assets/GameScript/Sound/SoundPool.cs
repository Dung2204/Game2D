using UnityEngine;
using System.Collections;

public class SoundPool : MonoBehaviour 
{
	string	strPathSx = "Audio/";
    //public	AudioSource		asMain;

    private AudioSource m_AsBG;

    [Range( 0f , 1f )]
    float musicVolume = 1;
    public float MusicVolume
    {
        get
        {
            return musicVolume;
        }
        set
        {
            musicVolume = value;
            m_AsBG.volume = musicVolume;
        }
    }

    [Range( 0f , 1f )]
    public float SoundVolume = 1;



    private static SoundPool _Instance = null;
    public static SoundPool GetInstance()
    {
        if (!_Instance)
        {
            GameObject glo_Main = GameObject.Find("glo_Main");
            _Instance = glo_Main.AddComponent<SoundPool>();
            if (!_Instance)
            {
                Debug.LogError("init glo_Main Fail");
            }
        }
        return _Instance;
    }

    void Awake() 
    {
        CreateSoundSource();
        GetDefaultVolimeSetting();
    }

    private void CreateSoundSource()
    {
        m_AsBG = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
    }

    private void GetDefaultVolimeSetting()
    {
        //MusicVolume = GetVolumeSetting(StaticValue.strBGM_Volume);
        //SoundVolume = GetVolumeSetting(StaticValue.strSE_Volume);
    }

    //// Update is called once per frame
    //void Update () 
    //{
	
    //}

    /// <summary>
    /// 播放指定音效
    /// </summary>
    /// <param name="strFileName"></param>
    //public void f_PlaySound(string strFileName) 
    //{
    //    if (!StaticValue.m_isPlaySound)
    //    {
    //        return;
    //    }

    //    if (strFileName == "")
    //    {
    //        return;
    //    }
    //    //string	strPath = strPathSx + ResourceSxList[idx];
    //   // _aclip = glo_Main.GetInstance().m_ResourceManager.f_LoadSound(strFileName);
    //    glo_Main.GetInstance().m_ResourceManager.f_LoadMusicSound(strFileName, Callback_PlaySound);
    //}

    //void Callback_PlaySound(object Obj, UnityEngine.Object asset)
    //{
    //    _aclip = asset as AudioClip;
    //    if (_aclip == null)
    //    {
    //        MessageBox.ASSERT("没有找到声音文件,播放失败 ");
    //    }
    //    else
    //    {
    //        AudioSource.PlayClipAtPoint(_aclip, new Vector3(0,0,-10) , SoundVolume);
    //        StaticValue.m_isPlayCharVoice = true;
    //        StartCoroutine("CharVioceCountDown");
    //    }
    //}
    //IEnumerator CharVioceCountDown()
    //{
    //    yield return new WaitForSeconds(1.5f);

    //    StaticValue.m_isPlayCharVoice = false;
    //}

  
    /// <summary>
    /// 播放指定背景音乐
    /// </summary>
    /// <param name="strFileName"></param>
    public void f_PlayMusic(string strFileName)
    {
        if (!StaticValue.m_isPlayMusic)
        {
            return;
        }
        if (strFileName == "")
        {
            return;
        }
        AudioClip tAudioClip = GetAudio(strFileName);
        m_AsBG.clip = tAudioClip;
        m_AsBG.loop = true;
        m_AsBG.volume = /*0.2f*/ MusicVolume;
        m_AsBG.Play();
    }

    //void Callback_PlayMusic(object Obj, UnityEngine.Object asset)
    //{
    //    AudioClip ac = asset as AudioClip;
    //    if (ac != null)
    //    {
    //        asBG.clip = ac;
    //        asBG.loop = true;
    //        asBG.volume = /*0.2f*/ MusicVolume;
    //        asBG.Play();
    //    }
    //}

    private AudioClip GetAudio(string strAudio)
    {
        string ppSQL = string.Format(strPathSx + strAudio);
        return (AudioClip)Resources.Load(ppSQL, typeof(AudioClip));
    }

    public void f_StopAll()
    {
        f_StopMusic();
    }

    public void f_StopMusic()
    {
        if (m_AsBG != null)
        {
            m_AsBG.Stop();
        }
    }

    public void SaveSetting ( string type , float value )
    {
        PlayerPrefs.SetFloat( type , value );
    }

    public float GetVolumeSetting ( string type )
    {
        //預設音量都為1.0f
        if ( false == PlayerPrefs.HasKey( type ) )
            SaveSetting( type , 1.0f );
        return PlayerPrefs.GetFloat( type );
    }
}

