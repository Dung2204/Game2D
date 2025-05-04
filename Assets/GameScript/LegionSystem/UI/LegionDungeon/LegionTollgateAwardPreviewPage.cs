using ccU3DEngine;
using UnityEngine;
using System;
using System.Collections.Generic;

public class LegionTollgateAwardPreviewPage : UIFramwork
{
    private LegionDungeonPoolDT mCurInfo;
    private EM_CardCamp mCurType = EM_CardCamp.eCardMain;

    private GameObject _awardCountItemParent;
    private GameObject _awardCountItem;
    private List<BasePoolDT<long>> _awardCountList;
    private UIWrapComponent _awardCountWrapComponent;
    private UIWrapComponent mAwardCountWrapComponent
    {
        get
        {
            if (_awardCountWrapComponent == null)
            {
                _awardCountList = LegionMain.GetInstance().m_LegionDungeonPool.f_GetTollgateAwardCountList(mCurInfo.mChapterId,(byte)mCurType);
                _awardCountWrapComponent = new UIWrapComponent(180, 5, 195, 2, _awardCountItemParent, _awardCountItem, _awardCountList, f_AwardCountItemUpdate, null);
            }
            return _awardCountWrapComponent;
        }
    }

    private Transform[] mSelectBtns;

    private UILabel mTimeTip;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        _awardCountItemParent = f_GetObject("AwardCountItemParent");
        _awardCountItem = f_GetObject("AwardCountItem");
        mSelectBtns = new Transform[4];
        for (int i = 0; i < mSelectBtns.Length; i++)
        {
            mSelectBtns[i] = f_GetObject(string.Format("SelectBtn{0}", i + 1)).transform;
            f_RegClickEvent(mSelectBtns[i].gameObject, f_SelectBtn, (EM_CardCamp)i + 1);
        }
        mTimeTip = f_GetObject("TimeTip").GetComponent<UILabel>();
        f_RegClickEvent("CloseBtn", f_CloseBtn);
        f_RegClickEvent("MaskClose", f_CloseBtn);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if(e == null || !(e is LegionDungeonPoolDT))
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(698));//军团副本关卡奖励预览界面参数错误
            return;
        }
        mCurInfo = (LegionDungeonPoolDT)e;
        f_UpdateByType(EM_CardCamp.eCardWei);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_THENEXTDAY_UIPROCESS, f_ProcessTheNextDay, this);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
        if (timeEventId != -99)
        {
            ccTimeEvent.GetInstance().f_UnRegEvent(timeEventId);
            timeEventId = -99;
        }
        timeEventId = ccTimeEvent.GetInstance().f_RegEvent(1.0f, true, null, f_UpdateBySecond);
    }

    private void f_UpdateTab(Transform selfTF,EM_CardCamp curCamp,EM_CardCamp selfCamp,LegionTollgatePoolDT info)
    {
        UILabel tNormalLabel = selfTF.Find("NormalLabel").GetComponent<UILabel>();
        GameObject tSelectTip = selfTF.Find("SelectTip").gameObject;
        UILabel tSelectLabel = selfTF.Find("SelectTip/SelectLabel").GetComponent<UILabel>();
        tNormalLabel.text = info.mTollgateTemplate.szName;
        tSelectLabel.text = info.mTollgateTemplate.szName;
        tSelectTip.SetActive(curCamp == selfCamp);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_THENEXTDAY_UIPROCESS, f_ProcessTheNextDay, this);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
        if (timeEventId != -99)
        {
            ccTimeEvent.GetInstance().f_UnRegEvent(timeEventId);
            timeEventId = -99;
        }
    }

    private void f_ProcessTheNextDay(object value)
    {
        if ((EM_NextDaySource)value != EM_NextDaySource.LegionDungeonPool)
            return;
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(699));//宝箱过期了
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionTollgateAwardPreviewPage, UIMessageDef.UI_CLOSE);
    }

    private void f_ProcessByMsg_ForceClose(object value)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionTollgateAwardPreviewPage, UIMessageDef.UI_CLOSE);
    }

    private void f_UpdateByType(EM_CardCamp type)
    {
        mCurType = type;
        _awardCountList = LegionMain.GetInstance().m_LegionDungeonPool.f_GetTollgateAwardCountList(mCurInfo.mChapterId,(byte)mCurType);
        mAwardCountWrapComponent.f_UpdateList(_awardCountList);
        mAwardCountWrapComponent.f_ResetView();
        for (int i = 0; i < mSelectBtns.Length; i++)
        {
            f_UpdateTab(mSelectBtns[i], mCurType, (EM_CardCamp)i + 1, mCurInfo.f_GetTollgatePoolDtByIdx(i));
        }
    }

    private void f_SelectBtn(GameObject go,object value1,object value2)
    {
        EM_CardCamp tType = (EM_CardCamp)value1;
        if(mCurType == tType)
            return;
        f_UpdateByType(tType);
    }

    private void f_CloseBtn(GameObject go,object value1,object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionTollgateAwardPreviewPage,UIMessageDef.UI_CLOSE);
    }

    private void f_AwardCountItemUpdate(Transform tf, BasePoolDT<long> dt)
    {
        LegionTollgateAwardPreviewItem tItem = tf.GetComponent<LegionTollgateAwardPreviewItem>();
        tItem.f_UpdateByInfo(dt);
    }

    private int timeEventId = -99;
    private DateTime m_NowTime;
    private TimeSpan m_NextDay2NowSpan;
    private void f_UpdateBySecond(object value)
    {
        m_NowTime = ccMath.time_t2DateTime(GameSocket.GetInstance().f_GetServerTime());
        m_NextDay2NowSpan = GameSocket.GetInstance().mNextDayTime - m_NowTime;
        if (m_NextDay2NowSpan.TotalSeconds > 0)//每击败一个关卡
            mTimeTip.text = string.Format(CommonTools.f_GetTransLanguage(700), m_NextDay2NowSpan.Hours, m_NextDay2NowSpan.Minutes, m_NextDay2NowSpan.Seconds);
        else                                 //都可以开启一份宝藏
            mTimeTip.text = string.Format(CommonTools.f_GetTransLanguage(701));
    }
}
