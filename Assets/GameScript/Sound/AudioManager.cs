using System;
using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 声音管理器
/// </summary>
public class AudioManager : MonoBehaviour
{
    public AudioSource _ButtleAudio;
    public AudioSource _Bgm;
    public AudioSource _EffectAudio;
	public AudioSource _VoiceAudio;
    /// <summary>
    /// 初始化
    /// </summary>
    void Awake()
    {
        float musicVolumn = LocalDataManager.f_GetLocalDataIfNotExitSetData<float>(LocalDataType.Float_MusicVolumn, 1);
        float effectVolumn = LocalDataManager.f_GetLocalDataIfNotExitSetData<float>(LocalDataType.Float_EffectVolumn, 1);
        StaticValue.m_isPlayMusic = musicVolumn == 1 ? true : false;
        StaticValue.m_isPlaySound = effectVolumn == 1 ? true : false;
        if (!StaticValue.m_isPlayMusic)
        {
            f_StopAudioMusic();
        }
        if (!StaticValue.m_isPlaySound)
        {
            f_StopAudioButtle();
        }
        DontDestroyOnLoad(this);
    }
    /// <summary>
    /// 播放按钮音效
    /// </summary>
    /// <param name="audioEffectType">音效类型</param>
    /// <param name="PlayTime">播放比例</param>
    public void f_PlayAudioButtle(AudioButtle audioEffectType)
    {
        AudioClip audioEffectClip = glo_Main.GetInstance().m_ResourceManager.f_GetAudioClip(0, (int)audioEffectType);
        if (audioEffectClip == null)
        {
MessageBox.ASSERT("Audio Load Failed！");
        }
        _ButtleAudio.clip = audioEffectClip;
        _ButtleAudio.loop = false;
        _ButtleAudio.volume = LocalDataManager.f_GetLocalData<float>(LocalDataType.Float_EffectVolumn);
        _ButtleAudio.pitch = 1;
        _ButtleAudio.Play();
    }
    /// <summary>
    /// 播放游戏背景音乐
    /// </summary>
    /// <param name="audioMusicType">音乐类型</param>
    /// <param name="isLoop">是否循环播放</param>
    public void f_PlayAudioMusic(AudioMusicType audioMusicType, bool isLoop = true, float volume = 0.2f)
    {
        AudioClip audioMusicClip = glo_Main.GetInstance().m_ResourceManager.f_GetAudioClip(2, (int)audioMusicType);
        if (audioMusicClip == null)
        {
MessageBox.ASSERT("Audio Load Failed！");
        }
        _Bgm.clip = audioMusicClip;
        _Bgm.loop = isLoop;
        _Bgm.volume = LocalDataManager.f_GetLocalData<float>(LocalDataType.Float_MusicVolumn) * volume;
        _Bgm.Play();
    }
    /// <summary>
    /// 播放特效音效
    /// </summary>
    /// <param name="audioEffectType">音效类型</param>
    /// <param name="PlayTime">播放比例</param>
    public void f_PlayAudioEffect(AudioEffectType audioEffectType, float PlayTime = 1, float volume = 0.5f)
    {
        AudioClip audioEffectClip = glo_Main.GetInstance().m_ResourceManager.f_GetAudioClip(1, (int)audioEffectType);
        if (audioEffectClip == null)
        {
MessageBox.ASSERT("Audio Load Failed！");
			return;
        }
        _EffectAudio.clip = audioEffectClip;
        _EffectAudio.loop = false;
        _EffectAudio.SetScheduledEndTime((double)(PlayTime * audioEffectClip.length));
        _EffectAudio.volume = LocalDataManager.f_GetLocalData<float>(LocalDataType.Float_EffectVolumn) * volume;
        _EffectAudio.pitch = 1;
        _EffectAudio.Play();
    }

    /// <summary>
    /// 剧情对话和喊招
    /// </summary>
    /// <param name="strMusicName"></param>
    /// <param name="PlayTime"></param>
    public float f_PlayAudioDialog(string strMusicName, float PlayTime = 1)
    {
        // AudioClip audioEffectClip = glo_Main.GetInstance().m_ResourceManager.f_GetAudioClipByDialog(strMusicName);
        // if (audioEffectClip == null)
        // {
// MessageBox.ASSERT("Audio Load Failed！");
            // return -99;
        // }
		// if(!_ButtleAudio.isPlaying)
		// {
			// _ButtleAudio.clip = audioEffectClip;
			// _ButtleAudio.loop = false;
			// _ButtleAudio.SetScheduledEndTime((double)(PlayTime * audioEffectClip.length));
			// _ButtleAudio.volume = LocalDataManager.f_GetLocalData<float>(LocalDataType.Float_EffectVolumn);
			// _ButtleAudio.Play();
			// _ButtleAudio.pitch = 1;
			// return audioEffectClip.length;
		// }
		return 1f;
    }

    public void f_PlayAudioEffect(string strMusicName, float PlayTime = 1)
    {
        AudioClip audioEffectClip = glo_Main.GetInstance().m_ResourceManager.f_GetAudioClip(strMusicName);
        if (audioEffectClip == null)
        {
MessageBox.ASSERT("Audio Load Failed！");
        }
        _EffectAudio.clip = audioEffectClip;
        _EffectAudio.loop = false;
        _EffectAudio.SetScheduledEndTime((double)(PlayTime * audioEffectClip.length));
        _EffectAudio.volume = LocalDataManager.f_GetLocalData<float>(LocalDataType.Float_EffectVolumn);
        _EffectAudio.pitch = 1;
        _EffectAudio.Play();
    }
	
	public void f_PlayAudioVoice(string strMusicName, float PlayTime = 1)
    {
        // AudioClip audioEffectClip = glo_Main.GetInstance().m_ResourceManager.f_GetAudioClip(strMusicName);
        // if (audioEffectClip == null)
        // {
// MessageBox.ASSERT("Audio Load Failed！");
            // return;
        // }
		// if(!_VoiceAudio.isPlaying)
		// {
			// _VoiceAudio.clip = audioEffectClip;
			// _VoiceAudio.loop = false;
			// _VoiceAudio.SetScheduledEndTime((double)(PlayTime * audioEffectClip.length));
			// _VoiceAudio.volume = LocalDataManager.f_GetLocalData<float>(LocalDataType.Float_EffectVolumn);
			// _VoiceAudio.pitch = 1;
			// _VoiceAudio.Play();
		// }
    }

    /// <summary>
    /// 播放技能音效
    /// </summary>
    /// <param name="audioEffectType">音效类型</param>
    /// <param name="PlayTime">播放比例</param>
    public void f_PlayAudioMagic(string strName, float pitch = 1f)
    {
        AudioClip audioEffectClip = glo_Main.GetInstance().m_ResourceManager.f_GetAudioMagic(strName);
        if (audioEffectClip == null)
        {
MessageBox.ASSERT("Audio Load Failed！" + strName);
            return;
        }
        else
        {
			if(!_EffectAudio.isPlaying)
			{
				_EffectAudio.clip = audioEffectClip;
				_EffectAudio.loop = false;
				_EffectAudio.SetScheduledEndTime((double)(1 * audioEffectClip.length));
				_EffectAudio.volume = LocalDataManager.f_GetLocalData<float>(LocalDataType.Float_EffectVolumn)*0.35f; //âm thanh hiệu ứng
				_EffectAudio.pitch = pitch * 1.5f;
				_EffectAudio.Play();
			}
        }
    }

    /// <summary>
    /// 停止背景音乐
    /// </summary>
    public void f_StopAudioMusic()
    {
        _Bgm.Stop();
    }
    public void f_StopAudioButtle()
    {
        _ButtleAudio.Stop();
        _EffectAudio.Stop();
		_VoiceAudio.Stop();
    }
    /// <summary>
    /// 播放背景音乐
    /// </summary>
    public void f_PlayAudioMusic()
    {
        _Bgm.volume = LocalDataManager.f_GetLocalData<float>(LocalDataType.Float_MusicVolumn);
        _Bgm.Play();
    }
    public void f_PlayAudioButtle()
    {
        _ButtleAudio.Play();
        _EffectAudio.Play();
		_VoiceAudio.Play();
    }
    /// <summary>
    /// 暂停背景音乐
    /// </summary>
    public void f_PauseAudioMusic()
    {
        _Bgm.Pause();
        _Bgm.enabled = false;
    }
    public void f_PauseAudioButtle()
    {
        _ButtleAudio.enabled = false;
        _EffectAudio.enabled = false;
		_VoiceAudio.enabled = false;
    }
    /// <summary>
    /// 恢复暂停背景音乐
    /// </summary>
    public void f_UnPauseAudioMusic()
    {
        _Bgm.volume = LocalDataManager.f_GetLocalData<float>(LocalDataType.Float_MusicVolumn);
        _Bgm.UnPause();
        _Bgm.enabled = true;
    }
    public void f_UnPauseAudioButtle()
    {
        _ButtleAudio.clip = null;
        _EffectAudio.clip = null;
		_VoiceAudio.clip = null;
        _ButtleAudio.enabled = true;
        _EffectAudio.enabled = true;
		_VoiceAudio.enabled = true;
    }
}
