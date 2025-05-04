using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class FinPrefabs : EditorWindow {
    [MenuItem("Tools/FindPrefabsForSpriteName")]
    static void OpenWindow() {
        Rect wr = new Rect(0,0,350,350);
        FinPrefabs window = (FinPrefabs)EditorWindow.GetWindowWithRect(typeof(FinPrefabs),wr,true, "FindPrefabsForSpriteName");
        window.Show();
    }

    private string SpriteName;
    //private bool isSet
    string PrefabsNamePath = Application.dataPath;
    private void OnGUI()
    {
        GUILayout.Space(30);
        EditorGUILayout.LabelField("  根据SpriteName（图片名字）遍历全部Prefabs查找Prefabs ");
        EditorGUILayout.LabelField("  并生成Txt在PrefabsNamePath目录下");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("SpriteName");
        SpriteName = EditorGUILayout.TextField(SpriteName);
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("查找")) {
            FindPrefabsForSpriteName(SpriteName);
        }
    }
    void FindPrefabsForSpriteName(string SpriteName) {
        string tmpstr = string.Empty;
        PrefabsNamePath = PrefabsNamePath + "/" + SpriteName + ".txt";
        if (File.Exists(PrefabsNamePath))
        {
            File.Delete(PrefabsNamePath);
        }
        StreamWriter PrefabsName = File.CreateText(PrefabsNamePath);
        Debug.Log("开始创建"+PrefabsNamePath);

        Debug.Log("开始遍历所有的Prefabs");
        string[] ids = AssetDatabase.FindAssets("t:Prefab");
        for (int i = 0; i < ids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(ids[i]);

            GameObject tttt = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;

            UISprite[] ttt = tttt.transform.GetComponentsInChildren<UISprite>();

            for (int j = 0; j < ttt.Length; j++)
            {
                if (ttt[j].spriteName == "Border_Common")
                {
                    Debug.Log(tttt.name);
                    PrefabsName.WriteLine(string.Format("Prefabs:{0}  子类名字:{1}",tttt.name,ttt[j].name));
                }
            }
        }
        PrefabsName.Close();
        Debug.Log("写入完成");
    }


    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
