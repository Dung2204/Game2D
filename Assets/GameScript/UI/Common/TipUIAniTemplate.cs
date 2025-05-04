using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 动画模板
/// </summary>
public class TipUIAniTemplate : MonoBehaviour
{
    private UITweener[] _AniArr;

    public static TipUIAniTemplate AddAni(GameObject go)
    {
        TipUIAniTemplate temp = go.GetComponent<TipUIAniTemplate>();
        if (temp == null)
        {
            temp = go.AddComponent<TipUIAniTemplate>();
            temp.InitAni(go);          
        }
        return temp;
    }

    private int _templateId = -1;
    private int _idxMaxNum = 0;
    private int _curIdx = 0;
    /// <summary>
    /// 在同一个Idx下，有多少个动画组件
    /// </summary>
    private int _curTemplateNum = 0;
    private int _templateTotalNum
    {
        get
        {
            if (_curTemplateList != null)
                return _curTemplateList.Count;
            return 0;
        }
    }

    private List<TipAniTemplateDT> _curTemplateList;
    /// <summary>
    /// 根据模板Id播放动画
    /// </summary>
    /// <param name="templateId"></param>
    public void f_PlayAni(int templateId)
    {
        _templateId = templateId;
        _idxMaxNum = glo_Main.GetInstance().m_SC_Pool.m_TipAniTemplateSC.f_GetIdxMaxCount(_templateId);
        if (_idxMaxNum < 0)
            return;
        _curIdx = 0;
        _curTemplateList = glo_Main.GetInstance().m_SC_Pool.m_TipAniTemplateSC.f_GetAniDatas(_templateId, _curIdx);
        _curTemplateNum = 0;
        for (int i = 0; i < _curTemplateList.Count; i++)
        {
            PlayUITweenerByData(_curTemplateList[i]);
        }
    }

    /// <summary>
    /// 设置动画组件并播放
    /// </summary>
    /// <param name="item"></param>
    private void PlayUITweenerByData(TipAniTemplateDT item)
    {
        UITweener tmpTweener = _AniArr[item.TweenType];
        if (tmpTweener == null)
        {
            if (item.TweenType == (byte)UITweenerType.Position)
            {
                tmpTweener = BelongObj.GetComponent<TweenPosition>() as UITweener;
                if (tmpTweener == null)
                    tmpTweener = BelongObj.AddComponent<TweenPosition>() as UITweener;
            }
            else if (item.TweenType == (byte)UITweenerType.Scale)
            {
                tmpTweener = BelongObj.GetComponent<TweenScale>() as UITweener;
                if (tmpTweener == null)
                    tmpTweener = BelongObj.AddComponent<TweenScale>() as UITweener;
            }
            else if (item.TweenType == (byte)UITweenerType.Color)
            {
                tmpTweener = BelongObj.GetComponent<TweenColor>() as UITweener;
                if (tmpTweener == null)
                    tmpTweener = BelongObj.AddComponent<TweenColor>() as UITweener;
            }
            else
            {
                return;
            }
            tmpTweener.SetOnFinished(PlayNextIdx);
        }
        item.f_SetTweener(ref tmpTweener);
        tmpTweener.ResetToBeginning();
        tmpTweener.PlayForward();
    }

    /// <summary>
    /// 播放下一个Idx动画
    /// </summary>
    private void PlayNextIdx()
    {
        _curTemplateNum++;
        if (_curTemplateNum >= _templateTotalNum && _curIdx <= _idxMaxNum)
        {
            _curIdx++;
            _curTemplateList = glo_Main.GetInstance().m_SC_Pool.m_TipAniTemplateSC.f_GetAniDatas(_templateId, _curIdx);
            _curTemplateNum = 0;
            for (int i = 0; i < _curTemplateList.Count; i++)
            {
                PlayUITweenerByData(_curTemplateList[i]);
            }
        }
    }

    /// <summary>
    /// Ani所属Obj
    /// </summary>
    public GameObject BelongObj
    {
        private set;
        get;
    }

    public void InitAni(GameObject go)
    {
        BelongObj = go;
        _AniArr = new UITweener[System.Enum.GetValues(typeof(UITweenerType)).Length];

    }
}
