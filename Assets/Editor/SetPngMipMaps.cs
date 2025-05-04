using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SetPngMipMaps
{

    [MenuItem("Tool/SetPngData")]
    public static void f_SetPngMipMaps()
    {
        Debug.Log("开始查找全部的Texture");
        string tip = "开始设置TextureImporter.....";
        string path = string.Empty;

        int NowIndex = 0;
        string[] Textureids = AssetDatabase.FindAssets("t:Texture");
        string[] Spriteids = AssetDatabase.FindAssets("t:Sprite");
        int Alllength = Textureids.Length + Spriteids.Length;

        EditorApplication.update = () =>
        {
            bool isCancle = EditorUtility.DisplayCancelableProgressBar("设置TextureImporter", tip + path, (float)NowIndex / Alllength);
            if (isCancle || NowIndex >= Alllength)
            {
                Debug.Log("手动取消或以完成");
                EditorUtility.ClearProgressBar();
                EditorApplication.update = null;
            }

            if (Textureids.Length > NowIndex)
            {
                path = AssetDatabase.GUIDToAssetPath(Textureids[NowIndex]);
                SetPngData(path);
                NowIndex++;
            }
            else
            {
                try
                {
                    if (NowIndex >= Alllength) {
                        Debug.Log("手动取消或以完成");
                        EditorUtility.ClearProgressBar();
                        EditorApplication.update = null;
                        return;
                    }
                    path = AssetDatabase.GUIDToAssetPath(Spriteids[NowIndex - Textureids.Length]);
                    SetPngData(path);
                    NowIndex++;
                }
                catch (System.Exception e)
                {
                    Debug.Log(e.Message + "Alllength:" + Alllength + "   Spriteids:" + Spriteids.Length+"    index:"+NowIndex);
                    throw;
                }
            }
        };
        return;
        for (int i = 0; i < Textureids.Length; i++)
        {
            path = AssetDatabase.GUIDToAssetPath(Textureids[i]);

            TextureImporter Imp = TextureImporter.GetAtPath(path) as TextureImporter;
            //AssetDatabase.LoadAssetAtPath(path,typeof(TextureImporter)) as TextureImporter;

            if (Imp == null)
            {
                Debug.LogError("获取TextureImporter错误  GUID：" + Textureids[i]);
            }

            Imp.mipmapEnabled = false;
            Imp.isReadable = false;
            AssetDatabase.ImportAsset(path);
            NowIndex++;
        }
        Debug.Log("Texture设置完成 数量：" + Textureids.Length);


        for (int i = 0; i < Spriteids.Length; i++)
        {
            path = AssetDatabase.GUIDToAssetPath(Spriteids[i]);

            TextureImporter Imp = TextureImporter.GetAtPath(path) as TextureImporter;

            if (Imp == null)
            {
                Debug.LogError("获取TextureImporter错误  GUID：" + Textureids[i]);
            }

            Imp.mipmapEnabled = false;
            Imp.isReadable = false;
            AssetDatabase.ImportAsset(path);
            NowIndex++;
        }

        Debug.Log("结束");

    }

    private static bool SetPngData(string path)
    {
        TextureImporter Imp = TextureImporter.GetAtPath(path) as TextureImporter;
        bool isSetData = false;
        if (Imp.mipmapEnabled)
        {
            Imp.mipmapEnabled = false;
            isSetData = true;
        }
        if (Imp.isReadable)
        {
            Imp.isReadable = false;
            isSetData = true;
        }
        if (Imp.wrapMode != TextureWrapMode.Clamp)
        {
            Imp.wrapMode = TextureWrapMode.Clamp;
            isSetData = true;
        }
        if (Imp.filterMode != FilterMode.Bilinear)
        {
            Imp.filterMode = FilterMode.Bilinear;
            isSetData = true;
        }
        if (isSetData)
            AssetDatabase.ImportAsset(path);
        return isSetData;
    }

    [MenuItem("Tools/进度条测试")]
    public static void Run()
    {
        int index = 0;
        int total = 500;
        EditorApplication.update = delegate ()
        {
            bool isCancle = EditorUtility.DisplayCancelableProgressBar("执行中...", "描述文字...", (float)index / total);
            ++index;
            if (isCancle || index >= total)
            {
                EditorUtility.ClearProgressBar();
                EditorApplication.update = null;
            }
        };
    }
    
    [MenuItem("Tools/清除PlayerPref缓存")]
    public static void clean()
    {
        PlayerPrefs.DeleteAll();
    }

}
