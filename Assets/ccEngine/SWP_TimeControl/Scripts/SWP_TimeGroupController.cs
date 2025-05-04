// --------------ABOUT AND COPYRIGHT----------------------
//  Copyright © 2013 SketchWork Productions Limited
//        support@sketchworkproductions.com
// -------------------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
/// This is the Time Control and Pause main Controller script and controls every element of the control.  For best use please use the supplied prefab.
/// </summary>
public class SWP_TimeGroupController : MonoBehaviour
{
    enum SoundType { PauseSound, SlowDownSound, SpeedUpSound, BackToNormalSound };

    /// <summary>
    ///The events are for advanced custom use.  You can track these events on your scripts and perform any custom actions and/or time control other objects not support directly by this kit. 
    /// </summary>
    public delegate void OnGroupPauseEvent(SWP_InternalTimedClass _TimedClass);
    public event OnGroupPauseEvent OnGroupPause;

    /// <summary>
    ///The events are for advanced custom use.  You can track these events on your scripts and perform any custom actions and/or time control other objects not support directly by this kit. 
    /// </summary>
    public delegate void OnGroupSlowDownEvent(SWP_InternalTimedClass _TimedClass);
    public event OnGroupSlowDownEvent OnGroupSlowDown;

    /// <summary>
    ///The events are for advanced custom use.  You can track these events on your scripts and perform any custom actions and/or time control other objects not support directly by this kit. 
    /// </summary>
    public delegate void OnGroupSpeedUpEvent(SWP_InternalTimedClass _TimedClass);
    public event OnGroupSpeedUpEvent OnGroupSpeedUp;

    /// <summary>
    ///The events are for advanced custom use.  You can track these events on your scripts and perform any custom actions and/or time control other objects not support directly by this kit. 
    /// </summary>
    public delegate void OnGroupResumeEvent(SWP_InternalTimedClass _TimedClass);
    public event OnGroupResumeEvent OnGroupResume;

    /// <summary>
    ///You should not directly change this value, but you can read this to see the current speed as a percentage. 
    /// </summary>
    public float ControllerSpeedPercent = 100f;

    /// <summary>
    ///You should not directly change this value, but you can read this to see the current speed as a zero to one value.
    /// </summary>
    public float ControllerSpeedZeroToOne = 1f;

    /// <summary>
    ///This controls which group of Timed Game Objects are to be controlled by this Time Controller.  You can have multiple Time Controllers controlling different IDs for example.
    /// </summary>
    public int GroupID = 1;

    /// <summary>
    ///This controls if the time control is to trigger the appropriate sound effects.
    /// </summary>
    public bool EnableSound = true;

    /// <summary>
    ///This is the overall volume of the triggered time control sound effects.
    /// </summary>
    public float SoundVolume = 1f;

    /// <summary>
    ///This points to the initial pause trigger SFX.
    /// </summary>
    public AudioClip PauseSound;

    /// <summary>
    ///This points to the initial slow down trigger SFX.
    /// </summary>
    public AudioClip SlowDownSound;

    /// <summary>
    ///This points to the initial speed up trigger SFX.
    /// </summary>
    public AudioClip SpeedUpSound;

    /// <summary>
    ///This points to the initial return to normal time trigger SFX.
    /// </summary>
    public AudioClip ResumeSound;

    /// <summary>
    ///This points to the Audio Source component for playing the time control sounds.
    /// </summary>
    AudioSource asAudio;

    /// <summary>
    ///We use Awake to cache the Audio Source component.
    /// </summary>
    void Awake()
    {
        asAudio = GetComponent<AudioSource>();
    }

    /// <summary>
    ///The start function is used to set up the cached Audio Source component.
    /// </summary>
    void Start()
    {
        SoundVolume = Mathf.Clamp(SoundVolume, 0f, 1f);

        if (asAudio == null || !asAudio.enabled)
            EnableSound = false;
    }

    /// <summary>
    ///This is what is used to Broadcast messages to the TimeGameObjects using only the event name.
    /// </summary>
    void BroadcastEvents(string _EventName)
    {
        SWP_InternalTimedGameObject[] thisGameObject = GameObject.FindObjectsOfType(typeof(SWP_InternalTimedGameObject)) as SWP_InternalTimedGameObject[];

        for (int iCount = 0; iCount < thisGameObject.Length; ++iCount)
            thisGameObject[iCount].SendMessage(_EventName, SendMessageOptions.DontRequireReceiver);
    }

    /// <summary>
    ///This is what is used to Broadcast messages to the TimeGameObjects using only the event name and also passes the SWP_TimedClass too which contains additional information.
    /// </summary>
    void BroadcastEvents(string _EventName, object _PassedObject)
    {
        SWP_InternalTimedGameObject[] thisGameObject = GameObject.FindObjectsOfType(typeof(SWP_InternalTimedGameObject)) as SWP_InternalTimedGameObject[];

        for (int iCount = 0; iCount < thisGameObject.Length; ++iCount)
            thisGameObject[iCount].SendMessage(_EventName, _PassedObject, SendMessageOptions.DontRequireReceiver);
    }

    /// <summary>
    ///This method is used where you want to time control DeltaTime.  Just replace your Time.DeltaTime calls with a call to this function.
    /// </summary>
    public float TimedDeltaTime()
    {
        if (ControllerSpeedPercent != 0)
            return Time.deltaTime / (100f / ControllerSpeedPercent);
        else
            return 0f;
    }

    /// <summary>
    ///This sets the two controller speeds (Percentage speed and ZeroToOne speed)
    /// </summary>
    void SetControllerSpeed(float _NewSpeed)
    {
        ControllerSpeedPercent = _NewSpeed;
        ControllerSpeedZeroToOne = (ControllerSpeedPercent == 0f ? 0f : ControllerSpeedPercent / 100f);
    }

    /// <summary>
    ///The main PauseTime control method.
    /// </summary>
    public void PauseGroupTime()
    {
        SetControllerSpeed(0f);

        SWP_InternalTimedClass thisTimedClass = new SWP_InternalTimedClass(GroupID, ControllerSpeedPercent);

        BroadcastEvents("OnGroupPauseBroadcast", thisTimedClass);

        if (OnGroupPause != null)
            OnGroupPause(thisTimedClass);

        PlayTimeControlSound(SoundType.PauseSound, SoundVolume);

        Debug.Log("SlowDownGroupTime");
    }

    /// <summary>
    ///The main SlowDownTime control method pass a percentage speed where 100% is normal speed.
    /// </summary>
    public void SlowDownGroupTime(float _NewTime)
    {
        SetControllerSpeed(_NewTime);

        SWP_InternalTimedClass thisTimedClass = new SWP_InternalTimedClass(GroupID, ControllerSpeedPercent);

        BroadcastEvents("OnGroupSlowDownBroadcast", thisTimedClass);

        if (OnGroupSlowDown != null)
            OnGroupSlowDown(thisTimedClass);

        PlayTimeControlSound(SoundType.SlowDownSound, SoundVolume);
    }

    /// <summary>
    ///The main SpeedUpTime control method pass a percentage speed where 100% is normal speed.
    /// </summary>
    public void SpeedUpGroupTime(float _NewTime, bool Init=false)
    {
        SetControllerSpeed(_NewTime);

        SWP_InternalTimedClass thisTimedClass = new SWP_InternalTimedClass(GroupID, ControllerSpeedPercent);

        BroadcastEvents("OnGroupSpeedUpBroadcast", thisTimedClass);

        if (OnGroupSpeedUp != null)
            OnGroupSpeedUp(thisTimedClass);

        if (!Init)
            PlayTimeControlSound(SoundType.SpeedUpSound, SoundVolume);
    }

    /// <summary>
    ///The main return to normal time control method.
    /// </summary>
    public void ResumeGroupTime()
    {
        SetControllerSpeed(100f);

        SWP_InternalTimedClass thisTimedClass = new SWP_InternalTimedClass(GroupID, ControllerSpeedPercent);

        BroadcastEvents("OnGroupResumeBroadcast", thisTimedClass);

        if (OnGroupResume != null)
            OnGroupResume(thisTimedClass);

        PlayTimeControlSound(SoundType.BackToNormalSound, SoundVolume);
    }

    /// <summary>
    ///This function will use the Audio Source to play the correct sound effect if enabled and required.
    /// </summary>
    void PlayTimeControlSound(SoundType _SoundType, float _SoundVolume)
    {
        if (!EnableSound || asAudio == null || !asAudio.enabled)
            return;

        if (_SoundType == SoundType.PauseSound && PauseSound != null)
            asAudio.PlayOneShot(PauseSound, _SoundVolume);
        else if (_SoundType == SoundType.SlowDownSound && SlowDownSound != null)
            asAudio.PlayOneShot(SlowDownSound, _SoundVolume);
        else if (_SoundType == SoundType.SpeedUpSound && SpeedUpSound != null)
            asAudio.PlayOneShot(SpeedUpSound, _SoundVolume);
        else if (_SoundType == SoundType.BackToNormalSound && ResumeSound != null)
            asAudio.PlayOneShot(ResumeSound, _SoundVolume);
    }
}