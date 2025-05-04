// --------------ABOUT AND COPYRIGHT----------------------
//  Copyright © 2013 SketchWork Productions Limited
//        support@sketchworkproductions.com
// -------------------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
/// This is the Time Control and Pause main Controller script and controls every element of the control.  For best use please use the supplied prefab.
/// </summary>
public class SWP_TimeGlobalController: MonoBehaviour
{		  
	enum SoundType {PauseSound, ResumeSound};

	/// <summary>
	///The events are for advanced custom use.  You can track these events on your scripts and perform any custom actions and/or time control other objects not support directly by this kit. 
	/// </summary>
	public delegate void OnGlobalPauseEvent ();
	public event OnGlobalPauseEvent OnGlobalPause;

	/// <summary>
	///The events are for advanced custom use.  You can track these events on your scripts and perform any custom actions and/or time control other objects not support directly by this kit. 
	/// </summary>
	public delegate void OnGlobalResumeEvent ();
	public event OnGlobalResumeEvent OnGlobalResume;

	/// <summary>
	/// This will allow the overall volume to dip when your game is paused. 
	/// </summary>
	public bool DipSoundOnPause;
	/// <summary>
	/// If DipSoundOnPause is enabled this is the volume that the volume will dip to once paused.
	/// </summary>
	public float DipVolumeLevel;
	/// <summary>
	/// If DipSoundOnPause is enabled this is how long in seconds it will take to dip the volume.  Set to zero for instant dip.
	/// </summary>
	public float DipTime;
	/// <summary>
	/// This holds the old time so when you resume after a pause the volume will return to the level is was playing at before.
	/// </summary>
	float fOldVolume;

	/// <summary>
	///You should not directly change this value, but you can read this to see the current speed as a percentage. 
	/// </summary>
	static public bool IsPaused = false;

	/// <summary>
	///This holds the current time since the level was loaded and takes into account if the game has been globally paused.
	/// </summary>
	static public float GlobalTimeSinceLevelLoad = 0f;

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
	public AudioClip GlobalPauseSound;
	
	/// <summary>
	///This points to the initial pause trigger SFX.
	/// </summary>
	public AudioClip GlobalResumeSound;

	/// <summary>
	///This points to the Audio Source component for playing the time control sounds.
	/// </summary>
	AudioSource asAudio;

	/// <summary>
	///This is the modifier that is used for the global pause function.  It is also static so you can access it from menu animations, etc.
	/// </summary>
	static public float TimeScaleModifier = 1000000f;
	
	/// <summary>
	///This holds a cached list of Timed Game Objects for speed
	/// </summary>
	static public SWP_InternalTimedList tlTimedGameObjectList;  //DOIT:  Awaiting implementation

	/// <summary>
	///We use Awake to cache the Audio Source component.
	/// </summary>
	void Awake ()
	{
		asAudio = GetComponent<AudioSource>();
	}

	/// <summary>
	///The start function is used to set up the cached Audio Source component.
	/// </summary>
	void Start ()
	{
		SoundVolume = Mathf.Clamp (SoundVolume, 0f, 1f);

		if (asAudio == null || !asAudio.enabled)
			EnableSound = false;
	}
	
	/// <summary>
	///Update is used to increment the GlobalTimeSinceLevelLoad variable taking into account if the game has been globally paused.
	/// </summary>
	void Update ()
	{
		if (!SWP_TimeGlobalController.IsPaused)
			SWP_TimeGlobalController.GlobalTimeSinceLevelLoad += Time.deltaTime;
		else
			SWP_TimeGlobalController.GlobalTimeSinceLevelLoad += Time.deltaTime * TimeScaleModifier;
	}

	/// <summary>
	///This is what is used to Broadcast messages to the TimeGameObjects using only the event name.
	/// </summary>
	void BroadcastEvents (string _EventName)
	{
		SWP_InternalTimedGameObject[] thisGameObject = GameObject.FindObjectsOfType (typeof(SWP_InternalTimedGameObject)) as SWP_InternalTimedGameObject[];

		for (int thisCount = 0; thisCount < thisGameObject.Length; ++thisCount)
			thisGameObject [thisCount].SendMessage (_EventName, SendMessageOptions.DontRequireReceiver);
	}

	/// <summary>
	///The main PauseTime control method.
	/// </summary>
	public void PauseGlobalTime ()
	{
		if (SWP_TimeGlobalController.IsPaused)
			return;

		BroadcastEvents ("OnGlobalPauseBroadcast");

		if (OnGlobalPause != null)
			OnGlobalPause ();

		PlayTimeControlSound (SoundType.PauseSound, SoundVolume);
		
		if (DipSoundOnPause && DipTime > 0f)
			StartCoroutine(DipAudioCoRoutine());
		else
		{
			if (DipSoundOnPause)
			{
				fOldVolume = AudioListener.volume;
				AudioListener.volume = DipVolumeLevel;
			}

			SWP_TimeGlobalController.IsPaused = true;
			
			Time.timeScale /= SWP_TimeGlobalController.TimeScaleModifier;
			//Time.fixedDeltaTime /= SWP_TimeGlobalController.TimeScaleModifier;
		}
	}

	/// <summary>
	///The main return to normal time control method.
	/// </summary>
	public void ResumeGlobalTime ()
	{
		if (!SWP_TimeGlobalController.IsPaused)
			return;
		
		Time.timeScale *= SWP_TimeGlobalController.TimeScaleModifier;
		//Time.fixedDeltaTime *= SWP_TimeGlobalController.TimeScaleModifier;

		SWP_TimeGlobalController.IsPaused = false;

		BroadcastEvents ("OnGlobalResumeBroadcast");

		if (OnGlobalResume != null)
			OnGlobalResume ();

		PlayTimeControlSound (SoundType.ResumeSound, SoundVolume);
			
		if (DipSoundOnPause && DipTime > 0f)
			StartCoroutine(ResumeAudioCoRoutine());
		else if (DipSoundOnPause)
			AudioListener.volume = fOldVolume;
	}

	/// <summary>
	///This function will use the Audio Source to play the correct sound effect if enabled and required.
	/// </summary>
	void PlayTimeControlSound (SoundType _SoundType, float _SoundVolume)
	{
		if (!EnableSound || asAudio == null || !asAudio.enabled)
			return;

		if (_SoundType == SoundType.PauseSound && GlobalPauseSound != null)
			asAudio.PlayOneShot (GlobalPauseSound, _SoundVolume);
		else if (_SoundType == SoundType.ResumeSound && GlobalResumeSound != null)
			asAudio.PlayOneShot (GlobalResumeSound, _SoundVolume);
	}
	
	/// <summary>
	///This IEnumerator gives a nice volume change when dipping on pause.
	/// </summary>
	IEnumerator DipAudioCoRoutine()
	{
		float thisProgress = 0f;
		fOldVolume = AudioListener.volume;

		while (thisProgress < 1f)
		{
			yield return new WaitForEndOfFrame();
			AudioListener.volume = Mathf.Lerp(AudioListener.volume, DipVolumeLevel, thisProgress);
			thisProgress += Time.deltaTime / 1f;
		}

		AudioListener.volume = DipVolumeLevel;

		SWP_TimeGlobalController.IsPaused = true;
		
		Time.timeScale /= SWP_TimeGlobalController.TimeScaleModifier;
		//Time.fixedDeltaTime /= SWP_TimeGlobalController.TimeScaleModifier;
	}
	
	/// <summary>
	///This IEnumerator gives a nice volume change when resuming after a pause.
	/// </summary>
	IEnumerator ResumeAudioCoRoutine()
	{
		float thisProgress = 0f;

		while (thisProgress < 1f)
		{
			yield return new WaitForEndOfFrame();
			AudioListener.volume = Mathf.Lerp(DipVolumeLevel, fOldVolume, thisProgress);
			thisProgress += Time.deltaTime / 1f;
		}
		
		AudioListener.volume = fOldVolume;
	}
}