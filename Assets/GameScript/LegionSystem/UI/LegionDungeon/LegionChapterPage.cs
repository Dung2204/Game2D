using ccU3DEngine;
using UnityEngine;
using System;
using System.Collections.Generic;

public class LegionChapterPage : UIFramwork
{
    private GameObject _chapterItemParent;
    private GameObject _chapterItem;
    private List<BasePoolDT<long>> _chapterList;
    private UIWrapComponent _chapterWrapComponent;
    private UIWrapComponent mChapterWrapComponent
    {
        get
        {
            if (_chapterWrapComponent == null)
            {
                _chapterList = LegionMain.GetInstance().m_LegionDungeonPool.f_GetAll();
                _chapterWrapComponent = new UIWrapComponent(260, 2, 680, 5, _chapterItemParent, _chapterItem, _chapterList, f_ChapterItemUpdateByInfo, f_ChapterItemClick);
            }
            return _chapterWrapComponent;
        }
    }
    private UILabel mChapterIdx;
    private UILabel mChapterName; 
    private UISlider mResetHpSlider;
    private UILabel mResetHpLabel;
    private UILabel mChapterResetTip;
    private UILabel mChallengeTimesLabel;
    private UILabel mRecoverTipLabel;

    private int _timeEventId = -99;
    private LegionDungeonPoolDT mCurInfo;

    private const int ShowNumPrePage = 4;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        _chapterItemParent = f_GetObject("ChapterItemParent");
        _chapterItem = f_GetObject("ChapterItem");
        mChapterIdx = f_GetObject("ChapterIdx").GetComponent<UILabel>();
        mChapterName = f_GetObject("ChapterName").GetComponent<UILabel>();
        mResetHpSlider = f_GetObject("ResetHpSlider").GetComponent<UISlider>();
        mResetHpLabel = f_GetObject("ResetHpLabel").GetComponent<UILabel>();
        mChapterResetTip = f_GetObject("ChapterResetTip").GetComponent<UILabel>();
        mChallengeTimesLabel = f_GetObject("ChallengeTimesLabel").GetComponent<UILabel>();
        mRecoverTipLabel = f_GetObject("RecoverTipLabel").GetComponent<UILabel>();
        f_RegClickEvent("AddTimesBtn", f_AddTimesBtn);
        f_RegClickEvent("BackBtn", f_BackBtn);
        f_RegClickEvent("Btn_Reset", f_ResetBtn);
        f_RegClickEvent("Btn_PassAward", f_PassAwardBtn);
        f_RegClickEvent("Btn_DamageRank", f_DamageRankBtn);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        mCurInfo = (LegionDungeonPoolDT)LegionMain.GetInstance().m_LegionDungeonPool.f_GetForId(LegionMain.GetInstance().m_LegionDungeonPool.m_iCurDungeonChapId);
        f_UpdateByInfo(mCurInfo);
        mChapterWrapComponent.f_ViewGotoRealIdx(f_GetIdxByCurChapter(), ShowNumPrePage);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_THENEXTDAY_UIPROCESS, f_ProcessTheNextDay, this);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
        if (_timeEventId == -99)
            _timeEventId = ccTimeEvent.GetInstance().f_RegEvent(1.0f, true, null, f_UpdateBySecond);
    }
    
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_THENEXTDAY_UIPROCESS, f_ProcessTheNextDay, this);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
        if (_timeEventId != -99)
        {
            ccTimeEvent.GetInstance().f_UnRegEvent(_timeEventId);
            _timeEventId = -99;
        }
    }

    private int f_GetIdxByCurChapter()
    {
        int result = 1;
        int curChapterId = LegionMain.GetInstance().m_LegionDungeonPool.m_iCurDungeonChapId;
        LegionDungeonPoolDT tPoolDt;
        for (int i = 0; i < _chapterList.Count; i++)
        {
            tPoolDt = (LegionDungeonPoolDT)_chapterList[i];
            if (tPoolDt.mChapterId == curChapterId)
            {
                result = i;
                break;
            }
        }
        return result;
    }

    private void f_ProcessTheNextDay(object value)
    {
        if ((EM_NextDaySource)value != EM_NextDaySource.LegionDungeonPool)
            return;
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(643));//跨天刷新
        LegionMain.GetInstance().m_LegionDungeonPool.f_ExecuteAfterInitFiniChapterAndInitCurChapter(f_ExecuteUpdateAfterInit);
    }

    private void f_ProcessByMsg_ForceClose(object value)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionChapterPage, UIMessageDef.UI_CLOSE);
    }

    //跨天刷新自己
    private void f_ExecuteUpdateAfterInit(object result)
    {
        mCurInfo = (LegionDungeonPoolDT)LegionMain.GetInstance().m_LegionDungeonPool.f_GetForId(LegionMain.GetInstance().m_LegionDungeonPool.m_iCurDungeonChapId);
        f_UpdateByInfo(mCurInfo);
        mChapterWrapComponent.f_ViewGotoRealIdx(f_GetIdxByCurChapter(), ShowNumPrePage);
    }

    //单前界面显示的重置章节Id
    private int curResetChapterId = 0;

    private void f_UpdateByInfo(LegionDungeonPoolDT info)
    {
        mChapterIdx.text = string.Format(CommonTools.f_GetTransLanguage(644), info.mChapterId);//第{0}章
        mChapterName.text = string.Format("{0}", info.mChapterTemplate.szName);
        curResetChapterId = LegionMain.GetInstance().m_LegionDungeonPool.m_iResetChapterId;
        LegionDungeonPoolDT tRestInfo = (LegionDungeonPoolDT)LegionMain.GetInstance().m_LegionDungeonPool.f_GetForId(curResetChapterId);
        mResetHpSlider.value = (float)tRestInfo.mTotalHp / tRestInfo.mTotalHpMax;
        mResetHpLabel.text = string.Format("{0}/{1}", tRestInfo.mTotalHp, tRestInfo.mTotalHpMax);
        f_SetResetTip();
        
    }

    private DateTime m_NowTime;
    private TimeSpan m_NextDay2NowSpan;

    private void f_UpdateBySecond(object value)
    {
        f_SetResetTip();
    }

    private bool _inTime;
    private int _extraTimes;
    private void f_SetResetTip()
    {
        m_NowTime = ccMath.time_t2DateTime(GameSocket.GetInstance().f_GetServerTime());
        m_NextDay2NowSpan = GameSocket.GetInstance().mNextDayTime - m_NowTime;
        if (m_NextDay2NowSpan.TotalSeconds > 0)  //重置至第{0}章
            mChapterResetTip.text = string.Format(CommonTools.f_GetTransLanguage(645), m_NextDay2NowSpan.Hours, m_NextDay2NowSpan.Minutes, m_NextDay2NowSpan.Seconds, LegionMain.GetInstance().m_LegionDungeonPool.m_iResetChapterId);
        else
            mChapterResetTip.text = string.Empty;

        if (curResetChapterId != LegionMain.GetInstance().m_LegionDungeonPool.m_iResetChapterId)
        {
            curResetChapterId = LegionMain.GetInstance().m_LegionDungeonPool.m_iResetChapterId;
            LegionDungeonPoolDT tRestInfo = (LegionDungeonPoolDT)LegionMain.GetInstance().m_LegionDungeonPool.f_GetForId(curResetChapterId);
            mResetHpSlider.value = (float)tRestInfo.mTotalHp / tRestInfo.mTotalHpMax;
            mResetHpLabel.text = string.Format("{0}/{1}", tRestInfo.mTotalHp, tRestInfo.mTotalHpMax);
        }

        _inTime = LegionMain.GetInstance().m_LegionDungeonPool.f_IsInOpenTime(false);
        _extraTimes = LegionMain.GetInstance().m_LegionDungeonPool.f_GetDungeonExtraTimes(m_NowTime);
        if (_inTime)//每隔两个小时恢复一次挑战次数
            mRecoverTipLabel.text = string.Format(CommonTools.f_GetTransLanguage(646),
                LegionConst.LEGION_DUNGEON_BEGIN_TIME, LegionConst.LEGION_DUNGEON_END_TIME,
                LegionConst.LEGION_DUNGEON_TIMES_RECOVER_TIME - (m_NowTime.Hour % LegionConst.LEGION_DUNGEON_TIMES_RECOVER_TIME +1),
                59 - m_NowTime.Minute,59 - m_NowTime.Second);
        else
            mRecoverTipLabel.text = string.Empty;//挑战次数
        mChallengeTimesLabel.text = string.Format(CommonTools.f_GetTransLanguage(647), LegionMain.GetInstance().m_LegionDungeonPool.m_iDungeonLeftTimes + _extraTimes);
    }

    private void f_BackBtn(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionChapterPage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
    }

    private void f_AddTimesBtn(GameObject go, object value1, object value2)
    {
        if (LegionMain.GetInstance().m_LegionDungeonPool.m_iDungeonBuyLeftTimes <= 0)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(648));//购买次数已用完
            return;
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionDungeonBuyPage, UIMessageDef.UI_OPEN);
    }

    private void f_ResetBtn(GameObject go, object value1, object value2)
    {
        if (!LegionTool.f_IsEnoughPermission(EM_LegionOperateType.SetResetDungeonChapter))
        {
            return;
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionChapterResetPage, UIMessageDef.UI_OPEN);
    }

    private void f_PassAwardBtn(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionChapterAwardPage, UIMessageDef.UI_OPEN);
    }

    private void f_DamageRankBtn(GameObject go, object value1, object value2)
    {
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(649));//成员成绩未开放
    }

    private void f_ChapterItemUpdateByInfo(Transform tf, BasePoolDT<long> dt)
    {
        LegionChapterItem tItem = tf.GetComponent<LegionChapterItem>();
        tItem.f_UpdateByInfo(dt);
    }

    private void f_ChapterItemClick(Transform tf, BasePoolDT<long> dt)
    {
        LegionDungeonPoolDT info = (LegionDungeonPoolDT)dt;
        int chapterLimit = LegionMain.GetInstance().m_LegionInfor.mLevelTemplate.iDungeonChapter;
        if (info.mChapterId > chapterLimit)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(650));//需要提升军团等级
            return;
        }
        else if (info.mChapterId > LegionMain.GetInstance().m_LegionDungeonPool.m_iCurDungeonChapId)
        {
            LegionChapterDT chapterDt = (LegionChapterDT)glo_Main.GetInstance().m_SC_Pool.m_LegionChapterSC.f_GetSC(info.mChapterId - 1);
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(651), chapterDt != null ? chapterDt.szName : string.Empty));//需通关
            return;
        }
        else if (LegionMain.GetInstance().m_LegionDungeonPool.f_IsFinisChapterToday(info.mChapterId))
        {
            curInfo = info;
            SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
            socketCallbackDt.m_ccCallbackSuc = f_Callback_InitChapter;
            socketCallbackDt.m_ccCallbackFail = f_Callback_InitChapter;
            LegionMain.GetInstance().m_LegionDungeonPool.f_InitChapter(info.mChapterId,socketCallbackDt);
            return;
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionChapterPage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionTollgatePage, UIMessageDef.UI_OPEN, info);
    }

    private LegionDungeonPoolDT curInfo;
    private void f_Callback_InitChapter(object result)
    {
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionChapterPage, UIMessageDef.UI_CLOSE);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionTollgatePage, UIMessageDef.UI_OPEN, curInfo);
        }
        else
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(652) + result);//军团副章节数据初始化失败
        }
    }

    #region 红点
    protected override void InitRaddot()
    {
        base.InitRaddot();
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.LegionChapetAward, f_GetObject("Btn_PassAward"), ReddotCallback_Show_Btn_PassAward, true);
    }

    protected override void UpdateReddotUI()
    {
        base.UpdateReddotUI();
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.LegionChapetAward);
    }

    protected override void On_Destory()
    {
        base.On_Destory();
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.LegionChapetAward, f_GetObject("Btn_PassAward"));
    }

    private void ReddotCallback_Show_Btn_PassAward(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnHall = f_GetObject("Btn_PassAward");
        UITool.f_UpdateReddot(BtnHall, iNum, new Vector3(56, 45, 0), 401);
    }

    #endregion
}
