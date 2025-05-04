using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ClipAlphe : EditorWindow {

    [MenuItem("Tools/ClipAlphe")]
    static void Open() {
        Rect wr = new Rect(0, 0, 350, 350);
        ClipAlphe window = (ClipAlphe)EditorWindow.GetWindowWithRect(typeof(ClipAlphe), wr, true, "ClipAlphe");
        window.Show();
    }

    private Texture2D _Texture;
    private void OnGUI()
    {
        GUILayout.Space(30);
        GUILayout.Label("剔除图片多余的透明");
        _Texture=EditorGUILayout.ObjectField(_Texture,typeof(Texture2D)) as Texture2D;

        if (GUILayout.Button("剔除")) {
            Clip(_Texture);
        }

    }


    void Clip(Texture2D t) {
        int left = 0;
        int top = 0;
        int right = t.width;
        int botton = t.height;

        //左侧
        for (int i = 0; i < t.width; i++)
        {
            bool find = false;

            for (int j = 0; j < t.height; j++)
            {
                Color tColor= t.GetPixel(i,j);
                if (Mathf.Abs(tColor.a)>1e-6) {
                    find = true;
                    break;
                }
            }
            if (find) {
                left = i;
                break;
            }
        }

        //右侧
        for (int i = t.width-1; i >= 0; i--)
        {
            bool find = false;
            for (int j = 0; j < t.height; j++)
            {
                Color tColor = t.GetPixel(i, j);
                if (Mathf.Abs(tColor.a) > 1e-6)
                {
                    find = true;
                    break;
                }
            }
            if (find)
            {
                right = i;
                break;
            }
        }

        //上侧
        for (int i = 0; i < t.height; i++)
        {
            bool find = false;

            for (int j = 0; j < t.width; j++)
            {
                Color tColor = t.GetPixel(i, j);
                if (Mathf.Abs(tColor.a) > 1e-6)
                {
                    find = true;
                    break;
                }
            }
            if (find)
            {
                top = i;
                break;
            }
        }
        //下侧
        for (int i = t.height-1; i >= 0; i--)
        {
            bool find = false;
            for (int j = 0; j < t.width; j++)
            {
                Color tColor = t.GetPixel(i, j);
                if (Mathf.Abs(tColor.a) > 1e-6)
                {
                    find = true;
                    break;
                }
            }
            if (find)
            {
                botton = i;
                break;
            }
        }

        //创建新纹理
        int width = right - left;
        int height = botton - top;

        Texture2D  tTextuew= new Texture2D(width,height,TextureFormat.ARGB32,false);
        tTextuew.alphaIsTransparency = true;

        Color[] tcolors = t.GetPixels(left,top,width,height);
        tTextuew.SetPixels(0, 0, width, height, tcolors);

        tTextuew.Apply();

        _Texture = tTextuew;
        string TexturePath= AssetDatabase.GetAssetPath(t);


    }

}
