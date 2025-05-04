// --------------ABOUT AND COPYRIGHT----------------------
//  Copyright © 2013 SketchWork Productions Limited
//        support@sketchworkproductions.com
// -------------------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SWP_TimeGroupController))]
public class SWP_TimeGroupControllerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		SWP_TimeGroupController _TimeControllerScript = (SWP_TimeGroupController)target;

		if (SWP_TimeGlobalControllerEditor.ShowHeader)
			GetHeader();

		if (SWP_TimeGlobalControllerEditor.ShowTitles)
		{
			EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb);
			EditorGUILayout.LabelField("Time Group Controller");
			EditorGUILayout.EndHorizontal();
		}
		
		#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0
		EditorGUILayout.BeginVertical(EditorStyles.miniButtonMid);
		#else
		EditorGUILayout.BeginVertical();
		#endif
		_TimeControllerScript.GroupID = EditorGUILayout.IntSlider("Group ID", _TimeControllerScript.GroupID, 1, 50);
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb);
		#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0
		_TimeControllerScript.EnableSound = EditorGUILayout.ToggleLeft("Enable Sound", _TimeControllerScript.EnableSound);
		#else
		_TimeControllerScript.EnableSound = EditorGUILayout.Toggle("Enable Sound", _TimeControllerScript.EnableSound);
		#endif
		EditorGUILayout.EndHorizontal();
		
		if (_TimeControllerScript.EnableSound)
		{
			#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0
			EditorGUILayout.BeginVertical(EditorStyles.miniButtonMid);
			#else
			EditorGUILayout.BeginVertical();
			#endif
			_TimeControllerScript.SoundVolume = EditorGUILayout.Slider("Sound Volume", _TimeControllerScript.SoundVolume, 0f, 1f);
			_TimeControllerScript.PauseSound = (AudioClip) EditorGUILayout.ObjectField("Pause Audio", _TimeControllerScript.PauseSound, typeof(AudioClip), false);
			_TimeControllerScript.SlowDownSound = (AudioClip) EditorGUILayout.ObjectField("Slow Down Audio", _TimeControllerScript.SlowDownSound, typeof(AudioClip), false);
			_TimeControllerScript.SpeedUpSound = (AudioClip) EditorGUILayout.ObjectField("Speed Up Audio", _TimeControllerScript.SpeedUpSound, typeof(AudioClip), false);
			_TimeControllerScript.ResumeSound = (AudioClip) EditorGUILayout.ObjectField("Resume Audio", _TimeControllerScript.ResumeSound, typeof(AudioClip), false);
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

				if (GUILayout.Button("Pause"))
					_TimeControllerScript.PauseGroupTime();
				
				if (GUILayout.Button("Resume"))
					_TimeControllerScript.ResumeGroupTime();

				EditorGUILayout.EndHorizontal();		
				EditorGUILayout.BeginHorizontal();

				if (GUILayout.Button("25% Speed"))
					_TimeControllerScript.SlowDownGroupTime(25f);
				
				if (GUILayout.Button("250% Speed"))
					_TimeControllerScript.SpeedUpGroupTime(250f);

				EditorGUILayout.EndHorizontal();		

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Current Speed = " + _TimeControllerScript.ControllerSpeedPercent + "%");
				EditorGUILayout.EndHorizontal();		
			EditorGUILayout.EndVertical();	
		}
		
		if (GUI.changed)
			EditorUtility.SetDirty(_TimeControllerScript);
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
