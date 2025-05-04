using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class SetModelColor {
    /// <summary>
    /// 改变颜色
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="color">颜色代码</param>
    public static void SetColor(GameObject obj, Vector4[] color) {
        var meshRenderer = obj.GetComponent<MeshRenderer>();
        if (null == meshRenderer)
        {
MessageBox.DEBUG("Cannot find MeshRenderer："+obj.name);
            return;
        }
        ResetShader(obj, "Spine/SkeletonColorMatrix");
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        mpb.SetVector("_ColorMatrixR", color[0]);
        mpb.SetVector("_ColorMatrixG", color[1]);
        mpb.SetVector("_ColorMatrixB", color[2]);
        mpb.SetVector("_ColorMatrixA", color[3]);
        meshRenderer.SetPropertyBlock(mpb);
    }

    /// <summary>
    /// 重置模型shader
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="shaderName"></param>
    public static void ResetShader(UnityEngine.Object obj, string shaderName)
    {
        List<Material> listMat = new List<Material>();
        listMat.Clear();

        if (obj is Material)
        {
            Material m = obj as Material;
            listMat.Add(m);
        }
        else if (obj is GameObject)
        {
            GameObject go = obj as GameObject;
            Renderer[] rends = go.GetComponentsInChildren<Renderer>();
            if (null != rends)
            {
                foreach (Renderer item in rends)
                {
                    Material[] materialsArr = item.sharedMaterials;
                    foreach (Material m in materialsArr)
                        listMat.Add(m);
                }
            }

        }

        for (int i = 0; i < listMat.Count; i++)
        {
            Material m = listMat[i];
            if (null == m)
                continue;
            var newShader = Shader.Find(shaderName);
            if (newShader != null)
                m.shader = newShader;
        }
    }

    /// <summary>
    /// 灰色
    /// </summary>
    public static Vector4[] gray = new Vector4[4]{
        new Vector4(0.2f, 0.7f, 0.1f, 0),
        new Vector4(0.2f, 0.7f, 0.1f, 0),
        new Vector4(0.2f, 0.7f, 0.1f, 0),
        new Vector4(0,0, 0, 1),
    };

    /// <summary>
    /// 石化
    /// </summary>
    public static Vector4[] stone = new Vector4[4]{
        new Vector4(0.2f, 0.2f, 0.2f, 0),
        new Vector4(0.2f, 0.2f, 0.2f, 0),
        new Vector4(0.2f, 0.2f, 0.2f, 0),
        new Vector4(0, 0, 0, 1),
    };

    /// <summary>
    /// 放逐
    /// </summary>
    public static Vector4[] banish = new Vector4[4]{
        new Vector4(0.27f, 0.27f, 0.27f, 0),
        new Vector4(0.36f, 0.36f, 0.36f, 0),
       new  Vector4(0.3f, 0.3f, 0.3f, 0.1f),
        new Vector4(0, 0, 0, 1),
    };

    /// <summary>
    /// 中毒
    /// </summary>
    public static Vector4[] poison = new Vector4[4]{
        new Vector4(0.8f, 0f, 0f,0.08f),
        new Vector4(0, 0.8f, 0, 0.2f),
        new Vector4(0, 0, 0.8f, 0),
        new Vector4(0, 0, 0, 1),
    };

    /// <summary>
    /// 冰封
    /// </summary>
    public static Vector4[] frozen = new Vector4[4]{
        new Vector4(0.8f, 0f, 0f,0f),
        new Vector4(0, 0.8f, 0, 0f),
        new Vector4(0, 0, 0.8f, 0.2f),
        new Vector4(0, 0, 0, 1),
    };

    /// <summary>
    /// 冰霜
    /// </summary>
    public static Vector4[] ice = new Vector4[4]{
        new Vector4(0.4f, 0.4f, 0.4f,0),
        new Vector4(0.6f, 0.6f, 0.6f, 0),
        new Vector4(0.75f, 0.75f, 0.75f, 0),
        new Vector4(0, 0, 0, 1),
    };

    /// <summary>
    /// normal
    /// </summary>
    public static Vector4[] normal = new Vector4[4]{
        new Vector4(1,0,0,0),
        new Vector4(0,1,0,0),
        new Vector4(0,0,1,0),
        new Vector4(0, 0, 0, 1),
    };

    /// <summary>
    /// 纯白色
    /// </summary>
    public static Vector4[] white = new Vector4[4]{
        new Vector4(1,0,0,0.7f),
        new Vector4(0,1,0,0.7f),
        new Vector4(0,0,1,0.7f),
        new Vector4(0, 0, 0, 1),
    };

    /// <summary>
    /// 金黄色
    /// </summary>
    public static Vector4[] gold = new Vector4[4]{
        new Vector4(1,0,0,0.9f),
        new Vector4(0,1,0,0.5f),
        new Vector4(0,0,1,0),
        new Vector4(0, 0, 0, 1),
    };

    /// <summary>
    /// 纯黑色
    /// </summary>
    public static Vector4[] black = new Vector4[4]{
        new Vector4(0,0,0,0),
        new Vector4(0,0,0,0),
        new Vector4(0,0,0,0),
        new Vector4(0, 0, 0, 1),
    };

    /// <summary>
    /// 高偏红
    /// </summary>
    public static Vector4[] red = new Vector4[4]{
        new Vector4(2,2,2,0),
        new Vector4(0,1,0,0),
        new Vector4(0,0,1,0),
        new Vector4(0, 0, 0, 1),
    };

    /// <summary>
    /// 高亮蓝
    /// </summary>
    public static Vector4[] highBlue = new Vector4[4]{
         new Vector4(0.5f,0,0,0),
        new Vector4(0,1,0,0),
        new Vector4(0,0,10,0),
        new Vector4(0, 0, 0, 1),
    };
}
