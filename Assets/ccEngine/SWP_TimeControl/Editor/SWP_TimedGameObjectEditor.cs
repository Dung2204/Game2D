// --------------ABOUT AND COPYRIGHT----------------------
//  Copyright © 2013 SketchWork Productions Limited
//        support@sketchworkproductions.com
// -------------------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SWP_TimedGameObject))]  
public class SWP_TimedGameObjectEditor : Editor
{
	public override void OnInspectorGUI()  
	{
		SWP_TimedGameObject _TimedGameObjectScript = (SWP_TimedGameObject)target;
		
		if (SWP_TimeGlobalControllerEditor.ShowHeader)
			GetHeader();
		
		if (SWP_TimeGlobalControllerEditor.ShowTitles)
		{
			EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb);
			EditorGUILayout.LabelField("Timed Object Options");
			EditorGUILayout.EndHorizontal();
		}

		DrawDefaultInspector();
		
		if (GUI.changed)
			EditorUtility.SetDirty(_TimedGameObjectScript);
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
