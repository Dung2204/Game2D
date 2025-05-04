// --------------ABOUT AND COPYRIGHT----------------------
//  Copyright © 2013 SketchWork Productions Limited
//        support@sketchworkproductions.com
// -------------------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SWP_TimeGlobalController))]
public class SWP_TimeGlobalControllerEditor : Editor
{
	static public bool ShowHeader = true;
	static public bool ShowTitles = true;
	static public bool ShowQuickDebugControls = true;

	public override void OnInspectorGUI()  
	{
		SWP_TimeGlobalController _TimeControlManagerScript = (SWP_TimeGlobalController)target;  
		
		if (SWP_TimeGlobalControllerEditor.ShowHeader)
			GetHeader();
		
		if (SWP_TimeGlobalControllerEditor.ShowTitles)
		{
			EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb);
			EditorGUILayout.LabelField("Time Global Controller");
			EditorGUILayout.EndHorizontal();
		}

		#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0
		EditorGUILayout.BeginVertical(EditorStyles.miniButtonMid);
		SWP_TimeGlobalControllerEditor.ShowHeader = EditorGUILayout.ToggleLeft("Show Editor Header", SWP_TimeGlobalControllerEditor.ShowHeader);
		SWP_TimeGlobalControllerEditor.ShowTitles = EditorGUILayout.ToggleLeft("Show Editor Titles", SWP_TimeGlobalControllerEditor.ShowTitles);
		SWP_TimeGlobalControllerEditor.ShowQuickDebugControls = EditorGUILayout.ToggleLeft("Show Debug Controls", SWP_TimeGlobalControllerEditor.ShowQuickDebugControls);
		#else
		EditorGUILayout.BeginVertical();
		SWP_TimeGlobalControllerEditor.ShowHeader = EditorGUILayout.Toggle("Show Editor Header", SWP_TimeGlobalControllerEditor.ShowHeader);
		SWP_TimeGlobalControllerEditor.ShowTitles = EditorGUILayout.Toggle("Show Editor Titles", SWP_TimeGlobalControllerEditor.ShowTitles);
		SWP_TimeGlobalControllerEditor.ShowQuickDebugControls = EditorGUILayout.Toggle("Show Debug Controls", SWP_TimeGlobalControllerEditor.ShowQuickDebugControls);
		#endif
		EditorGUILayout.EndVertical();

		EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb);
		#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0
		_TimeControlManagerScript.EnableSound = EditorGUILayout.ToggleLeft("Enable Sound", _TimeControlManagerScript.EnableSound);
		#else
		_TimeControlManagerScript.EnableSound = EditorGUILayout.Toggle("Enable Sound", _TimeControlManagerScript.EnableSound);
		#endif
		EditorGUILayout.EndHorizontal();
		
		if (_TimeControlManagerScript.EnableSound)
		{
			#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0
			EditorGUILayout.BeginVertical(EditorStyles.miniButtonMid);
			#else
			EditorGUILayout.BeginVertical();
			#endif
			_TimeControlManagerScript.SoundVolume = EditorGUILayout.Slider("Sound Volume", _TimeControlManagerScript.SoundVolume, 0f, 1f);
			_TimeControlManagerScript.GlobalPauseSound = (AudioClip) EditorGUILayout.ObjectField("Pause Audio", _TimeControlManagerScript.GlobalPauseSound, typeof(AudioClip), false);
			_TimeControlManagerScript.GlobalResumeSound = (AudioClip) EditorGUILayout.ObjectField("Resume Audio", _TimeControlManagerScript.GlobalResumeSound, typeof(AudioClip), false);
			EditorGUILayout.EndVertical();
		}

		EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb);
		#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0
		_TimeControlManagerScript.DipSoundOnPause = EditorGUILayout.ToggleLeft("Dip On Pause", _TimeControlManagerScript.DipSoundOnPause);
		#else
		_TimeControlManagerScript.DipSoundOnPause = EditorGUILayout.Toggle("Dip On Pause", _TimeControlManagerScript.DipSoundOnPause);
		#endif
		EditorGUILayout.EndHorizontal();
		
		if (_TimeControlManagerScript.DipSoundOnPause)
		{
			#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0
			EditorGUILayout.BeginVertical(EditorStyles.miniButtonMid);
			#else
			EditorGUILayout.BeginVertical();
			#endif
			_TimeControlManagerScript.DipVolumeLevel = EditorGUILayout.Slider("Dip Volume", _TimeControlManagerScript.DipVolumeLevel, 0f, 1f);
			_TimeControlManagerScript.DipTime = EditorGUILayout.Slider("Time To Dip", _TimeControlManagerScript.DipTime, 0f, 1f);
			EditorGUILayout.EndVertical();
		}

		if (SWP_TimeGlobalControllerEditor.ShowQuickDebugControls)
		{
			if (SWP_TimeGlobalControllerEditor.ShowTitles)  
			{
				EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb);
				EditorGUILayout.LabelField("Quick Debug Controls");
				EditorGUILayout.EndHorizontal();
			}

			#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0
			EditorGUILayout.BeginVertical(EditorStyles.miniButtonMid);
			#else
			EditorGUILayout.BeginVertical();
			#endif
				EditorGUILayout.BeginHorizontal();

				if (GUILayout.Button("Global Pause"))
					_TimeControlManagerScript.PauseGlobalTime();

				if (GUILayout.Button("Global Resume"))
					_TimeControlManagerScript.ResumeGlobalTime();

				EditorGUILayout.EndHorizontal();		
			
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Current State = " + (SWP_TimeGlobalController.IsPaused ? " Paused" : "Not Paused"));
				EditorGUILayout.EndHorizontal();		
			EditorGUILayout.EndVertical();	
		}

		if (GUI.changed)
			EditorUtility.SetDirty(_TimeControlManagerScript);
	}

	void GetHeader()
	{
		//Texture thisTextureHeader = (Texture) Resources.LoadAssetAtPath("Assets/SWP_TimeControl/Editor/Textures/SketchWorkHeader.png", typeof(Texture));
        Texture thisTextureHeader = (Texture)AssetDatabase.LoadAssetAtPath("Assets/SWP_TimeControl/Editor/Textures/SketchWorkHeader.png", typeof(Texture));
		
		if (thisTextureHeader != null)
		{
			Rect thisRect = GUILayoutUtility.GetRect(0f, 0f);
			thisRect.width = thisTextureHeader.width;
			thisRect.height = thisTextureHeader.height;
			GUILayout.Space(thisRect.height);
			GUI.DrawTexture(thisRect, thisTextureHeader);
		}
	}
}
