using UnityEditor;
using UnityEngine;

using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public class AniTemplateEditor : EditorWindow
{
    public const string c_FileName = "AniTemplate.bin";

    [MenuItem("GameTool/AniTemplate")]
    static void Init()
    {
        EditorWindow window =  GetWindow(typeof(AniTemplateEditor),false);
        window.Show();
    }

    private void OnEnable()
    {
        InitTemplate();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Serialize"))
        {
            SerializeTemplate();
        }

        AddUpdateID = EditorGUILayout.IntField("AddUpdateId:", AddUpdateID);
        AddUpdateIdx = EditorGUILayout.IntField("AddUpdateIdx:", AddUpdateIdx);

        if (GUILayout.Button("UpdateTemplate"))
        {
            AddOrUpdateTemplate();
        }

        RemoveId = EditorGUILayout.IntField("RemoveId:", RemoveId);
        RemoveIdx = EditorGUILayout.IntField("RemoveIdx:", RemoveIdx);
        RemoveTweenType = (UITweenerType)EditorGUILayout.EnumPopup("RemoveTweenType:",RemoveTweenType);
        if (GUILayout.Button("RemoveTemplate"))
        {
            TemplateDataListRemove(RemoveId, RemoveIdx, (int)RemoveTweenType);
        }

        //展示数据
        for (int i = 0; i < templateDataList.Count; i++)
        {
            GUILayout.Label(templateDataList[i].ToString());
            EditorGUILayout.CurveField(templateDataList[i].mAnimationCurve);
        }
    }

    private bool _bHasInit = false;
    public void InitTemplate()
    {
        if (!_bHasInit)
        {
            templateDataList = CommonTools.f_DeserializeTemplate<TipAniTemplateDT>(Application.streamingAssetsPath, c_FileName);
        } 
    }


    private void SerializeTemplate()
    {
        CommonTools.f_SerializeTemplate<TipAniTemplateDT>(Application.streamingAssetsPath,c_FileName, templateDataList);
    }

    private List<TipAniTemplateDT> templateDataList = new List<TipAniTemplateDT>();

    private int AddUpdateID, AddUpdateIdx;
    private void AddOrUpdateTemplate()
    {
        GameObject select = Selection.activeGameObject;
        Debug.Log(select);
        if (select != null)
        {
            UITweener[] tweeners = select.GetComponents<UITweener>();
            for (int i = 0; i < tweeners.Length; i++)
            {
                TipAniTemplateDT tmpTween = new TipAniTemplateDT();
                if (tmpTween.f_InitByTweener((byte)AddUpdateID, (byte)AddUpdateIdx, tweeners[i]))
                {
                    TemplateDataListUpdate(tmpTween);
                }
            }
        }
    } 
    
    /// <summary>
    /// 更新TemplateList
    /// </summary>
    /// <param name="item"></param>
    private void TemplateDataListUpdate(TipAniTemplateDT item)
    {
        TemplateDataListRemove(item.Id, item.Idx, item.TweenType);
        templateDataList.Add(item);
    }

    private int RemoveId, RemoveIdx;
    private UITweenerType RemoveTweenType;

    /// <summary>
    /// 移除TemplateList 数据
    /// </summary>
    /// <param id="id"></param>
    /// <param idx="idx"></param>
    /// <param tweenType="tweenType"></param>
    private void TemplateDataListRemove(int id, int idx, int tweenType)
    {
        int result = templateDataList.RemoveAll(delegate (TipAniTemplateDT item)
        {
            return item.Id == id && item.Idx == idx && item.TweenType == tweenType;
        });
        Debug.Log(string.Format("Remove Id:{0},Idx:{1},TweenType:{2} removeNum:{3}",id,idx,((UITweenerType)tweenType).ToString(),result));
    }

}
