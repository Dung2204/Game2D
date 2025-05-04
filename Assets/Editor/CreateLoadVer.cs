using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class CreateLoadVer : EditorWindow
{
    [MenuItem("Tools/LoadVer")]
    static void OpenWindow() {
        Rect wr = new Rect(0, 0,350, 350);
        CreateLoadVer window = (CreateLoadVer)EditorWindow.GetWindowWithRect(typeof(CreateLoadVer), wr, true, "LoadVer");
        window.Show();
        window.Adress = Application.streamingAssetsPath + "/LoadVer.txt";
    }

    private string strVer;
    private string AndroidMd5;
    private string IosMd5;
    private TextAsset ServerList;
    private int AutoUpdateLog;
    private int AutoUpdateLogTime;
    private int Ver;
    public string Adress ;
    private void OnGUI()
    {
        EditorGUILayout.LabelField("    这个窗体用来编辑LoadVer文件");
        EditorGUILayout.LabelField("    点击确认后自动会在StreamingAssets下生成");
        EditorGUILayout.LabelField("    LoadVer.byte文件");
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("资源版本:");
        strVer=EditorGUILayout.TextField("", strVer);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("安卓MD5:");
        AndroidMd5 = EditorGUILayout.TextField("", AndroidMd5);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("IOSMd5:");
        IosMd5 = EditorGUILayout.TextField("", IosMd5);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("ServerList:");
        ServerList = (TextAsset)EditorGUILayout.ObjectField((UnityEngine.Object)ServerList,typeof(TextAsset));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("是否自动上传日志:");
        AutoUpdateLog = EditorGUILayout.IntField(AutoUpdateLog);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("自动上传日志间隔:");
        AutoUpdateLogTime = EditorGUILayout.IntField(AutoUpdateLogTime);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("当前版本号:");
        Ver = EditorGUILayout.IntField(Ver);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("生成")) {
            _CreateLoadVer();
        }
        if (GUILayout.Button("重新生成"))
        {
            _CreateLoadVer(true);
        }
        EditorGUILayout.EndHorizontal();
    }

    private void _CreateLoadVer(bool isRedo=false) {
        

        if (isRedo) {
            if (File.Exists(Adress)) {
                File.Delete(Adress);
            }
        }

        if (!File.Exists(Adress))
        {
            StreamWriter t = File.CreateText(Adress);
            //写入版本效验以及资源MD5
            t.Write(string.Format("{0}---{1}-{2}:", strVer, IosMd5, AndroidMd5));
            //写入服务器列表
            if (ServerList!=null)
            {
                string tmpText = ServerList.text.Replace("\r", "");
                tmpText = tmpText.Replace("\n", "#");
                for (int i = 0; i < tmpText.Length; i++)
                {
                   // Debug.Log(string.Format("{0}+{1}", (byte)tmpText[i], tmpText[i]));
                }
                t.Write(tmpText + ":");
            }
            //写入服务器上传日志相关以及版本号
            t.Write(string.Format("{0}:{1}:{2}", AutoUpdateLog, AutoUpdateLogTime, Ver));
            t.Close();
            Debug.Log("生成成功");
        }
        else {
            Debug.Log("以拥有");
        }
    }



}
