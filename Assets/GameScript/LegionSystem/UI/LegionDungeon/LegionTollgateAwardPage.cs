using ccU3DEngine;
using UnityEngine;
using System;
using System.Collections.Generic;

public class LegionTollgateAwardPage : UIFramwork
{
    private LegionDungeonPoolDT mCurInfo;
    private EM_CardCamp mCurType = EM_CardCamp.eCardMain;

    private GameObject _awardItemParent;
    private GameObject _awardItem;
    private List<BasePoolDT<long>> _awardList;
    private UIWrapComponent _awardWrapComponent;
    private UIWrapComponent mAwardWrapComponent
    {
        get
        {
            if (_awardWrapComponent == null)
            {
                _awardList = LegionMain.GetInstance().m_LegionDungeonPool.f_GetTollgateAwardList(mCurInfo.mChapterId, (byte)mCurType);
                _awardWrapComponent = new UIWrapComponent(260, 5, 300, 4, _awardItemParent, _awardItem, _awardList, f_AwardItemUpdate, f_AwardItemClick);
            }
            return _awardWrapComponent;
        }
    }

    private LegionTollgateAwardSelectBtn[] mSelectBtns;

    private string szCenterBgFile = "UI/TextureRemove/Legion/jt_bz_bg_a";
    private UITexture mTextureBg;

    private UILabel mTimeTip;

    private int _timeEventId = -99;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        _awardItemParent = f_GetObject("AwardItemParent");
        _awardItem = f_GetObject("AwardItem");
        mTimeTip = f_GetObject("TimeTip").GetComponent<UILabel>();
        f_RegClickEvent("SelectBtn1", f_SelectBtn, EM_CardCamp.eCardWei);
        f_RegClickEvent("SelectBtn2", f_SelectBtn, EM_CardCamp.eCardShu);
        f_RegClickEvent("SelectBtn3", f_SelectBtn, EM_CardCamp.eCardWu);
        f_RegClickEvent("SelectBtn4", f_SelectBtn, EM_CardCamp.eCardGroupHero);
        f_RegClickEvent("AwardPreviewBtn", f_AwardPreviewBtn);
        f_RegClickEvent("BackBtn", f_BackBtn);
        mSelectBtns = new LegionTollgateAwardSelectBtn[4];
        for (int i = 0; i < mSelectBtns.Length; i++)
        {
            mSelectBtns[i] = f_GetObject(string.Format("SelectBtn{0}", i + 1)).GetComponent<LegionTollgateAwardSelectBtn>();
            f_RegClickEvent(mSelectBtns[i].mSelectBtn,f_SelectBtn,(EM_CardCamp)i+1);
        }
        mTextureBg = f_GetObject("Flower").GetComponent<UITexture>();
    }

    private void f_LoadTexture()
    {
       mTextureBg.mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(szCenterBgFile);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (e == null || !(e is LegionDungeonPoolDT))
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(684));//军团副本关卡奖励界面参数错误
            return;
        }
        mCurInfo = (LegionDungeonPoolDT)e;
        f_UpdateByType(EM_CardCamp.eCardWei);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_THENEXTDAY_UIPROCESS,f_ProcessTheNextDay, this);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
        if (_timeEventId == -99)
            _timeEventId = ccTimeEvent.GetInstance().f_RegEvent(1.0f, true, null, f_UpdateBySecond);
        f_LoadTexture();
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

    private void f_ProcessTheNextDay(object value)
    {
        if ((EM_NextDaySource)value != EM_NextDaySource.LegionDungeonPool)
            return;
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(685));//宝箱过期
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionTollgateAwardPage, UIMessageDef.UI_CLOSE);
    }

    private void f_ProcessByMsg_ForceClose(object value)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionTollgateAwardPage, UIMessageDef.UI_CLOSE);
    }

    private bool mCurBoxValid = false;
    private DateTime m_NowTime;
    private TimeSpan m_NextDay2NowSpan;
    private void f_UpdateBySecond(object value)
    {
        f_SetTimeTip();
    }

    private void f_CheckBoxValid()
    {
        bool tValid = LegionMain.GetInstance().m_LegionDungeonPool.f_IsFinisChapterToday(mCurInfo.mChapterId)
                      || mCurInfo.mChapterId == LegionMain.GetInstance().m_LegionDungeonPool.m_iCurDungeonChapId;
        bool tCanGet = LegionMain.GetInstance().m_LegionDungeonPool.f_GetTollgateAwardCanGet(mCurInfo.mChapterId, (byte)mCurType);
        long tHp = mCurInfo.f_GetTollgatePoolDtByIdx((byte)mCurType - 1).mHp;
        mCurBoxValid = tValid && tCanGet && tHp <= 0;
    }

    private void f_SetTimeTip()
    {
        m_NowTime = ccMath.time_t2DateTime(GameSocket.GetInstance().f_GetServerTime());
        m_NextDay2NowSpan = GameSocket.GetInstance().mNextDayTime - m_NowTime;
        if (mCurBoxValid && m_NextDay2NowSpan.TotalSeconds > 0)//消失,请尽快领区
            mTimeTip.text = string.Format(CommonTools.f_GetTransLanguage(686), m_NextDay2NowSpan.Hours, m_NextDay2NowSpan.Minutes, m_NextDay2NowSpan.Seconds);
        else
            mTimeTip.text = string.Empty;
    }

    private void f_UpdateByType(EM_CardCamp type)
    {
        mCurType = type;
        f_UpdateSelectBtn();
        _awardList = LegionMain.GetInstance().m_LegionDungeonPool.f_GetTollgateAwardList(mCurInfo.mChapterId, (byte)mCurType);
        mAwardWrapComponent.f_UpdateList(_awardList);
        mAwardWrapComponent.f_ResetView();
         
        f_CheckBoxValid();
        f_SetTimeTip();
    }

    private void f_SelectBtn(GameObject go, object value1, object value2)
    {
        EM_CardCamp tType = (EM_CardCamp)value1;
        if (mCurType == tType)
            return;
        f_UpdateByType(tType);
    }

    private void f_UpdateSelectBtn()
    {
        for (int i = 0; i < mSelectBtns.Length; i++)
        {
            mSelectBtns[i].f_UpdateByInfo(mCurType, (EM_CardCamp)i + 1, mCurInfo.f_GetTollgatePoolDtByIdx(i));
        }
    }

    private void f_BackBtn(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionTollgateAwardPage, UIMessageDef.UI_CLOSE);
    }

    private void f_AwardPreviewBtn(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionTollgateAwardPreviewPage, UIMessageDef.UI_OPEN, mCurInfo);
    }

    private void f_AwardItemUpdate(Transform tf, BasePoolDT<long> dt)
    {
        LegionTollgateAwardItem tItem = tf.GetComponent<LegionTollgateAwardItem>();
        tItem.f_UpdateByInfo(dt);
    }

    private void f_AwardItemClick(Transform tf, BasePoolDT<long> dt)
    {
        bool tValid = LegionMain.GetInstance().m_LegionDungeonPool.f_IsFinisChapterToday(mCurInfo.mChapterId)
                      || mCurInfo.mChapterId == LegionMain.GetInstance().m_LegionDungeonPool.m_iCurDungeonChapId;
        bool canGet = LegionMain.GetInstance().m_LegionDungeonPool.f_GetTollgateAwardCanGet(mCurInfo.mChapterId, (byte)mCurType);
        LegionTollgatePoolDT tTollgatePoolDt = mCurInfo.f_GetTollgatePoolDtByIdx((byte)mCurType - 1);
        LegionTollgateAwardPoolDT poolDt = (LegionTollgateAwardPoolDT)dt;
        if (!tValid)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(687));//宝箱已过期
            return;
        }
        else if (tTollgatePoolDt.mHp > 0)
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(688), tTollgatePoolDt.mTollgateTemplate.szName));//击败{0}后,可以领区宝藏
            return;
        }
        else if (!canGet)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(689));//你已经领取过了
            return;
        }
        else if (poolDt.m_iGetPlayer != 0)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(690));//已被领取了
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_TollgateAward;
        socketCallbackDt.m_ccCallbackFail = f_Callback_TollgateAward;
        LegionMain.GetInstance().m_LegionDungeonPool.f_TollgateAward(mCurInfo.mChapterId,(byte)mCurType,(byte)poolDt.m_iIdx,socketCallbackDt);
    }

    private void f_Callback_TollgateAward(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(691));//领取成功
            f_CheckBoxValid();
            f_SetTimeTip();
            mAwardWrapComponent.f_UpdateView();
            f_UpdateSelectBtn();
        }
        else if ((int)result == (int)eMsgOperateResult.eOR_LegionTollgateBoxTimeErr)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(692));//通关前加入军团的成员才可以领取该关卡宝箱
        }
        else if ((int)result == (int)eMsgOperateResult.eOR_LegionTollgateBoxOpened)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(693));//该宝箱已被领取了
            UITool.f_OpenOrCloseWaitTip(true);
            SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
            socketCallbackDt.m_ccCallbackSuc = f_Callback_TollgateInit;
            socketCallbackDt.m_ccCallbackFail = f_Callback_TollgateInit;
            LegionMain.GetInstance().m_LegionDungeonPool.f_InitTollgate(true, mCurInfo.mChapterId, (byte)mCurType, socketCallbackDt);
        }
        else if ((int)result == (int)eMsgOperateResult.eOR_TimesLimit)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(694));//该关卡宝箱已领取过了
        }
        else
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(695) + (int)result);//宝箱领取失败
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(696) + result);
        }
    }

    private void f_Callback_TollgateInit(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result != (int)eMsgOperateResult.OR_Succeed)
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(697) + result);//初始化关卡奖励数据
        }
        mAwardWrapComponent.f_ResetView();
        f_UpdateSelectBtn();
        f_CheckBoxValid();
        f_SetTimeTip();
    }
}
