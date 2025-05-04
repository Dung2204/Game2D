
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

class ModelAssetImporter : AssetPostprocessor
{
    private static string environmentsDir = "Assets/Game/Environments";
    private static string RoleDir = "Assets/Game/Actors/Role";
    private static string MonsterDir = "Assets/Game/Actors/Monster";
    private static string T4M = "Assets/T4M";
    private static string T4MOBJ = "Assets/T4MOBJ";
    public void OnPreprocessModel()
    {
        ModelImporter modelImporter = assetImporter as ModelImporter;
        modelImporter.importMaterials = false;  // 在模型导入前关掉自动导入材质球，太恶心
        var obj = AssetDatabase.LoadMainAssetAtPath(modelImporter.assetPath);
        //if (null == obj)
        //{
        //    return;
        //}

        if (ImporterUtils.IsIgnoreImportRule(obj))
        {
            return;
        }

        if (modelImporter.assetPath.StartsWith(T4MOBJ) || modelImporter.assetPath.StartsWith(T4M))
        {
            return;
        }
      //  ProcessGlobalScele(modelImporter);
        ProcessIsReadable(modelImporter);
        ProcessOptimizeMesh(modelImporter);
        ProcessMeshCompression(modelImporter);
        ProcessTangents(modelImporter);
        ProcessMaterials(modelImporter);
        ProcessCollider(modelImporter);
    }

    public void OnPostprocessModel(GameObject model)
    {
        Renderer[] renders = model.GetComponentsInChildren<Renderer>();
        if (null != renders)
        {
            foreach (var render in renders)
            {
                render.sharedMaterials = new Material[render.sharedMaterials.Length];
            }
        }
        FoldAnimation(model);
    }

    private static void ProcessGlobalScele(ModelImporter assetImporter)
    {
        if (assetImporter.globalScale != 1)
        {
            return;
        }
        assetImporter.globalScale = 1;
    }

    private static void ProcessIsReadable(ModelImporter assetImporter)
    {
        if (assetImporter.assetPath.StartsWith(environmentsDir) || assetImporter.assetPath.StartsWith(RoleDir))   // 场景模型的开启状态由场景编辑那控制
        {
            return;
        }

        if (assetImporter.assetPath.StartsWith(environmentsDir))
        {
            assetImporter.isReadable = true;
        }
        else
        {
            assetImporter.isReadable = false;
        }
        
    }

    private static void ProcessOptimizeMesh(ModelImporter assetImporter)
    {
        assetImporter.optimizeMesh = true;
    }

    private static void ProcessMeshCompression(ModelImporter assetImporter)
    {
        // 角色有接缝问题不能修改模式
        if (assetImporter.assetPath.StartsWith(RoleDir))
        {
            assetImporter.meshCompression = ModelImporterMeshCompression.Off;
            return;
        }
        if (assetImporter.meshCompression < ModelImporterMeshCompression.Medium)
        {
            assetImporter.meshCompression = ModelImporterMeshCompression.Medium;
        }
    }

    private static void ProcessTangents(ModelImporter assetImporter)
    {
        assetImporter.importTangents = ModelImporterTangents.None;
    }

    private static void ProcessMaterials(ModelImporter assetImporter)
    {
        assetImporter.importMaterials = false;
    }

    private static void ProcessCollider(ModelImporter assetImporter)
    {
        assetImporter.addCollider = false;
    }

    //优化动画文件
    void FoldAnimation(GameObject g)
    {
        int accuracy = 3;
        // for skeleton animations.
        ModelImporter modelImporter = assetImporter as ModelImporter;
        if (modelImporter.assetPath.StartsWith(MonsterDir))
        {
            accuracy = 4;
        }
        modelImporter.optimizeGameObjects = true;
        modelImporter.animationCompression = ModelImporterAnimationCompression.Optimal;
        modelImporter.animationRotationError = 0.5f;
        modelImporter.animationPositionError = 0.5f;
        modelImporter.animationScaleError = 0.5f;

        List<AnimationClip> animationClipList = new List<AnimationClip>(AnimationUtility.GetAnimationClips(g));
        if (animationClipList.Count == 0)
        {
            AnimationClip[] objectList = UnityEngine.Object.FindObjectsOfType(typeof(AnimationClip)) as AnimationClip[];
            animationClipList.AddRange(objectList);
        }

        foreach (AnimationClip theAnimation in animationClipList)
        {
            try
            {
                //去除scale曲线
                foreach (EditorCurveBinding theCurveBinding in AnimationUtility.GetCurveBindings(theAnimation))
                {
                    string name = theCurveBinding.propertyName.ToLower();
                    if (name.Contains("scale"))
                    {
                        AnimationUtility.SetEditorCurve(theAnimation, theCurveBinding, null);
                    }
                }

                //浮点数精度压缩到f3
                AnimationClipCurveData[] curves = null;
                curves = AnimationUtility.GetAllCurves(theAnimation);
                Keyframe key;
                Keyframe[] keyFrames;
                for (int ii = 0; ii < curves.Length; ++ii)
                {
                    AnimationClipCurveData curveDate = curves[ii];
                    if (curveDate.curve == null || curveDate.curve.keys == null)
                    {
                        //Debug.LogWarning(string.Format("AnimationClipCurveData {0} don't have curve; Animation name {1} ", curveDate, animationPath));
                        continue;
                    }
                    keyFrames = curveDate.curve.keys;
                    for (int i = 0; i < keyFrames.Length; i++)
                    {
                        key = keyFrames[i];
                        key.value = float.Parse(key.value.ToString("f"+ accuracy));
                        key.inTangent = float.Parse(key.inTangent.ToString("f" + accuracy));
                        key.outTangent = float.Parse(key.outTangent.ToString("f" + accuracy));
                        keyFrames[i] = key;
                    }
                    curveDate.curve.keys = keyFrames;
                    theAnimation.SetCurve(curveDate.path, curveDate.type, curveDate.propertyName, curveDate.curve);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(string.Format("CompressAnimationClip Failed !!! animationPath : {0} error: {1}", assetPath, e));
            }
        }

    }
}

