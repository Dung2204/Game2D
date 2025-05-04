using ccU3DEngine;
using UnityEngine;
using System;

public class LegionTollgatePage : UIFramwork
{
    private LegionTollgateItem[] mTollgateItems;
    private UILabel mCurChapterIdx;
    private UILabel mCurChapterName; 
    private UILabel mChapterResetTip;
    private UISlider mResetHpSlider;
    private UILabel mResetHpLabel;
    private UILabel mChallengeTimesLabel;
    private UILabel mRecoverTipLabel;
    private UILabel mTollgateAwardBtnLabel;

    private int _timeEventId = -99;
    private LegionDungeonPoolDT mCurInfo;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mTollgateItems = new LegionTollgateItem[4];
        for (int i = 0; i < mTollgateItems.Length; i++)
        {
            mTollgateItems[i] = f_GetObject(string.Format("TollgateItem{0}", i + 1)).GetComponent<LegionTollgateItem>();
            f_RegClickEvent(mTollgateItems[i].mItem,f_TollgateItemClick,i);
        }
        mCurChapterIdx = f_GetObject("CurChapterIdx").GetComponent<UILabel>();
        mCurChapterName = f_GetObject("CurChapterName").GetComponent<UILabel>(); 
        mResetHpSlider = f_GetObject("ResetHpSlider").GetComponent<UISlider>();
        mResetHpLabel = f_GetObject("ResetHpLabel").GetComponent<UILabel>();
        mChapterResetTip = f_GetObject("ChapterResetTip").GetComponent<UILabel>();
        mChallengeTimesLabel = f_GetObject("ChallengeTimesLabel").GetComponent<UILabel>();
        mRecoverTipLabel = f_GetObject("RecoverTipLabel").GetComponent<UILabel>();
        mTollgateAwardBtnLabel = f_GetObject("TollgateAwardBtnLabel").GetComponent<UILabel>();
        f_RegClickEvent("AddTimesBtn", f_AddTimesBtn);
        f_RegClickEvent("BackBtn", f_BackBtn);
        f_RegClickEvent("TollgateAwardBtn", f_TollgateAwardBtn);
        f_RegClickEvent("Btn_Reset", f_ResetBtn);
        f_RegClickEvent("Btn_PassAward", f_PassAwardBtn);
        f_RegClickEvent("Btn_DamageRank", f_DamageRankBtn);
    }


    private bool isOpenByBattle2Menu = false;
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        isOpenByBattle2Menu = false;
        if (e != null && e is Battle2MenuProcessParam)
        {
            Battle2MenuProcessParam tParam = (Battle2MenuProcessParam)e;
            mCurInfo = (LegionDungeonPoolDT)LegionMain.GetInstance().m_LegionDungeonPool.f_GetForId((int)tParam.m_Params[0]);
            tParam.f_UpdateParam(EM_Battle2MenuProcess.None);
            f_InitChapterInfo(true);
            isOpenByBattle2Menu = true;
        }
        else if (e != null && e is LegionDungeonPoolDT)
        {
            mCurInfo = (LegionDungeonPoolDT)e;
        }
        else
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(654)); //军团副本关卡界面传递参数错误
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionTollgatePage, UIMessageDef.UI_CLOSE);
            return;
        }
        f_LoadTexture();
        f_UpdateByInfo(mCurInfo);
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

    private const string szCenterBgFile = "UI/TextureRemove/Legion/Tex_LegionTollgate";
    private void f_LoadTexture()
    {
        // f_GetObject("TextureBg").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(szCenterBgFile);
    }

    private void f_ProcessTheNextDay(object value)
    {
        if ((EM_NextDaySource)value != EM_NextDaySource.LegionDungeonPool)
            return;
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(655));//跨天刷新
        UITool.f_OpenOrCloseWaitTip(true);
        LegionMain.GetInstance().m_LegionDungeonPool.f_ExecuteAfterInitFiniChapterAndInitCurChapter(f_ExecuteOpenChapterPageAfterInit);
    }
    //跨天关闭关卡界面，重新打开章节界面
    private void f_ExecuteOpenChapterPageAfterInit(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionTollgatePage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionChapterPage, UIMessageDef.UI_OPEN);
    }

    private void f_ProcessByMsg_ForceClose(object value)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionTollgatePage, UIMessageDef.UI_CLOSE);
    }

    //单前界面显示的重置章节Id
    private int curResetChapterId = 0;

    private void f_UpdateByInfo(LegionDungeonPoolDT info)
    {
        for (int i = 0; i < mTollgateItems.Length; i++)
        {
            mTollgateItems[i].f_UpdateByInfo(info.f_GetTollgatePoolDtByIdx(i));
        }
        mCurChapterIdx.text = string.Format(CommonTools.f_GetTransLanguage(656), mCurInfo.mChapterId);//第{0}章
        mCurChapterName.text = string.Format("{0}", mCurInfo.mChapterTemplate.szName);
        mTollgateAwardBtnLabel.text = string.Format(CommonTools.f_GetTransLanguage(657), mCurInfo.mChapterTemplate.szName);//宝藏
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
        if (m_NextDay2NowSpan.TotalSeconds > 0)//重置至第{3}章
            mChapterResetTip.text = string.Format(CommonTools.f_GetTransLanguage(658), m_NextDay2NowSpan.Hours, m_NextDay2NowSpan.Minutes, m_NextDay2NowSpan.Seconds, LegionMain.GetInstance().m_LegionDungeonPool.m_iResetChapterId);
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
            mRecoverTipLabel.text = string.Format(CommonTools.f_GetTransLanguage(659),
                LegionConst.LEGION_DUNGEON_BEGIN_TIME, LegionConst.LEGION_DUNGEON_END_TIME,
                LegionConst.LEGION_DUNGEON_TIMES_RECOVER_TIME - (m_NowTime.Hour % LegionConst.LEGION_DUNGEON_TIMES_RECOVER_TIME + 1),
                59 - m_NowTime.Minute, 59 - m_NowTime.Second);
        else
            mRecoverTipLabel.text = string.Empty;
        mChallengeTimesLabel.text = string.Format(CommonTools.f_GetTransLanguage(660), LegionMain.GetInstance().m_LegionDungeonPool.m_iDungeonLeftTimes + _extraTimes);//挑战次数
    }

    private void f_AddTimesBtn(GameObject go, object value1, object value2)
    {
        if (LegionMain.GetInstance().m_LegionDungeonPool.m_iDungeonBuyLeftTimes <= 0)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(661));//购买次数已用完
            return;
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionDungeonBuyPage, UIMessageDef.UI_OPEN);
    }

    private void f_BackBtn(GameObject go, object value1, object value2)
    {
        if (isOpenByBattle2Menu)
        {
            UITool.f_OpenOrCloseWaitTip(true);
            LegionMain.GetInstance().m_LegionDungeonPool.f_ExecuteAfterInitFiniChapterAndInitCurChapter(f_ExecuteOpenChapterPageAfterInit);
            return;
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionTollgatePage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionChapterPage, UIMessageDef.UI_OPEN);
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
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(662));//成员成绩未开放
    }

    private void f_TollgateAwardBtn(GameObject go, object value1, object value2)
    {
        if (_tollgateAwardInitWait)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(663));//等待服务器返回
            return;
        }
        if (mCurInfo.mChapterId != LegionMain.GetInstance().m_LegionDungeonPool.m_iCurDungeonChapId
            && !LegionMain.GetInstance().m_LegionDungeonPool.f_IsFinisChapterToday(mCurInfo.mChapterId))
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(664));//宝箱已过期
            return;
        }
        _tollgateAwardInitIdx = 0;
        _tollgateAwardInitWait = true;
        f_InitCurChapterTollgateInfo();
    }

    private int _tollgateAwardInitIdx = 0;
    private bool _tollgateAwardInitWait = false;
    private void f_InitCurChapterTollgateInfo(object value = null)
    {
        if (_tollgateAwardInitIdx < mCurInfo.mTotalTollgateCount)
        {
            LegionTollgatePoolDT tTollgatePoolDt = mCurInfo.f_GetTollgatePoolDtByIdx(_tollgateAwardInitIdx);
            SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
            socketCallbackDt.m_ccCallbackSuc = f_Callback_InitTollgateAward;
            socketCallbackDt.m_ccCallbackFail = f_Callback_InitTollgateAward;
            LegionMain.GetInstance().m_LegionDungeonPool.f_InitTollgate(tTollgatePoolDt.mHp <= 0, mCurInfo.mChapterId, (byte)tTollgatePoolDt.mCamp, socketCallbackDt);
        }
        else
        {
            _tollgateAwardInitWait = false;
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionTollgateAwardPage, UIMessageDef.UI_OPEN, mCurInfo);
        }
    }

    private void f_Callback_InitTollgateAward(object result)
    {
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            _tollgateAwardInitIdx++;
            ccTimeEvent.GetInstance().f_RegEvent(0.01f, false,null, f_InitCurChapterTollgateInfo);
        }
        else
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(665) + result);//初始化军团副本关卡奖励失败
            _tollgateAwardInitWait = false;
        }
    }


    private void f_TollgateItemClick(GameObject go,object value1,object value2)
    {
        if (!LegionMain.GetInstance().m_LegionDungeonPool.f_IsInOpenTime(true))
        {
            return;
        }
        int tollgateIdx = (int)value1;
        LegionTollgatePoolDT tollgatePoolDt = mCurInfo.f_GetTollgatePoolDtByIdx(tollgateIdx);
        if (tollgatePoolDt == null)
            return;
        if (tollgatePoolDt.mHp <= 0)
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(666), tollgatePoolDt.mTollgateTemplate.szName));//已击破
            return;
        }
        LegionTollgateChallengePageParam tParam = new LegionTollgateChallengePageParam(tollgatePoolDt, f_Callback_TollgateChallengeResult);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionTollgateChallengePage, UIMessageDef.UI_OPEN, tParam);
    }

    private void f_Callback_TollgateChallengeResult(object result)
    {
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionTollgatePage, UIMessageDef.UI_CLOSE);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.BattleMain);
        }
        else
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(667));//挑战数据同步,刷新界面
            //出错刷新关卡界面
            f_InitChapterInfo(false);
        }
    }

    private bool needUpdateFiniInfo = false;
    private void f_InitChapterInfo(bool needUpdateFiniInfo)
    {
        this.needUpdateFiniInfo = needUpdateFiniInfo;
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_InitChapterInfo;
        socketCallbackDt.m_ccCallbackFail = f_Callback_InitChapterInfo;
        LegionMain.GetInstance().m_LegionDungeonPool.f_InitChapter(mCurInfo.mChapterId, socketCallbackDt);
    }

    private void f_Callback_InitChapterInfo(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            f_UpdateByInfo(mCurInfo);
        }
        else
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(668) + result);//初始化章节信息失败
        }
        if (needUpdateFiniInfo)
        {
            UITool.f_OpenOrCloseWaitTip(true);
            SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
            socketCallbackDt.m_ccCallbackSuc = f_Callback_InitFiniChapter;
            socketCallbackDt.m_ccCallbackFail = f_Callback_InitFiniChapter;
            LegionMain.GetInstance().m_LegionDungeonPool.f_InitFiniChapter(socketCallbackDt);
        } 
    }

    private void f_Callback_InitFiniChapter(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            f_UpdateByInfo(mCurInfo);
        }
        else
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(669) + result);//初始化已完成章节数据失败
        }
        needUpdateFiniInfo = false;
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
