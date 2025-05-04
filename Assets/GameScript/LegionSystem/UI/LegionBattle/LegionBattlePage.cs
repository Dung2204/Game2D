using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class LegionBattlePage : UIFramwork
{
    private LegionBattleSignupSubPanel mSigupSubPanel;

    private LegionBattleItem[] mBattleItems;

    private GameObject mBtnSelfLegionSelect;
    private GameObject mBtnEnemyLegionSelect;
    private UILabel mSelfStarNum;
    private UILabel mEnemyStarNum;
    private UILabel mChallengeTimes;
    private UILabel mEndTip;
    private UILabel mEnemyLegionName;

    private LegionBattlePoolDT curData;
    //是否正在军团战期间
    private bool isBattling = false;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mSigupSubPanel = f_GetObject("SigupSubPanel").GetComponent<LegionBattleSignupSubPanel>();
        mSigupSubPanel.f_Init(this);
        mBattleItems = new LegionBattleItem[(int)EM_LegionGate.End - 1];
        for (int i = 0; i < mBattleItems.Length; i++)
        {
            mBattleItems[i] = f_GetObject(string.Format("BattleItem{0}", i + 1)).GetComponent<LegionBattleItem>();
            f_RegClickEvent(mBattleItems[i].mClickItem, f_OnBattleItemClick, (EM_LegionGate)i + 1);
        }
        mBtnSelfLegionSelect = f_GetObject("BtnSelfLegionSelect");
        mBtnEnemyLegionSelect = f_GetObject("BtnEnemyLegionSelect");
        mSelfStarNum = f_GetObject("SelfStarNum").GetComponent<UILabel>();
        mEnemyStarNum = f_GetObject("EnemyStarNum").GetComponent<UILabel>();
        mChallengeTimes = f_GetObject("ChallengeTimes").GetComponent<UILabel>();
        mEndTip = f_GetObject("EndTip").GetComponent<UILabel>();
        mEnemyLegionName = f_GetObject("EnemyLegionName").GetComponent<UILabel>();
        f_RegClickEvent("BtnSelfLegion", f_OnBtnSelfLegionClick);
        f_RegClickEvent("BtnEnemyLegion", f_OnBtnEnemyLegionClick);
        f_RegClickEvent("BtnLineup", f_OnBtnLineupClick);
        f_RegClickEvent("BtnLegionBattleAward", f_OnBtnLegionBattleAwardClick);
        f_RegClickEvent("BtnLegionBattleList", f_OnBtnLegionBattleListClick);
        f_RegClickEvent("BtnBack", f_OnBtnBack);
		f_RegClickEvent("Btn_Help", f_OnHelpBtnClick);
        f_RegClickEvent(mSigupSubPanel.mBtnCLose, f_OnSigupPanelClose);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        curServerTime = GameSocket.GetInstance().f_GetServerTime();
        LegionMain.GetInstance().m_LegionBattlePool.f_SetBattleTime(curServerTime);
        isBattling = curServerTime >= LegionMain.GetInstance().m_LegionBattlePool.m_iBeginTime && curServerTime <= LegionMain.GetInstance().m_LegionBattlePool.m_iEndTime;
        //默认打开敌方军团界面
        f_UpdateCurData(LegionMain.GetInstance().m_LegionBattlePool.m_EnemyPoolDt);
        f_LoadTexture();
        //tsuCode
        //f_RegTimeEvent();
        //
        if (e != null && e is Battle2MenuProcessParam)
        {
            Battle2MenuProcessParam tParam = (Battle2MenuProcessParam)e;
            int tGateType = (int)tParam.m_Params[0];
            //ccUIHoldPool.GetInstance().f_Hold(this);
            LegionBattleGatePageParam tParam2 = new LegionBattleGatePageParam(curData.m_bIsSelf, curData.f_GetGateNode((EM_LegionGate)tGateType), true);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionBattleGatePage, UIMessageDef.UI_OPEN, tParam2);
            tParam.f_UpdateParam(EM_Battle2MenuProcess.None);
            //return;
        }
        f_LegionBattleInit();
        f_RegTimeEvent(); //tsuComment
        mSigupSubPanel.f_Open();
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        f_ResetLegionBattleInitCD();
        f_UnregTimeEvent();
        mSigupSubPanel.f_Close();
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
    }

    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        f_RegTimeEvent();
        if (curData.m_bIsSelf)
        {
            f_UpdateCurData(LegionMain.GetInstance().m_LegionBattlePool.m_SelfPoolDt);
        }
        else
        {
            f_UpdateCurData(LegionMain.GetInstance().m_LegionBattlePool.m_EnemyPoolDt);
        }
        f_LegionBattleInit();
    }

    protected override void UI_HOLD(object e)
    {
        base.UI_HOLD(e);
        f_ResetLegionBattleInitCD();
        f_UnregTimeEvent();
        mSigupSubPanel.f_Close();
    }

    private const string szCenterBgFile = "UI/TextureRemove/Legion/Tex_LegionBattleBg";
    private void f_LoadTexture()
    {
        f_GetObject("TextureBg").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(szCenterBgFile);
    }

    private int timeEventId = -99;
    private void f_RegTimeEvent()
    {
        f_UnregTimeEvent();
        timeEventId = ccTimeEvent.GetInstance().f_RegEvent(1.0f, true, null, f_UpdateBySecond);
    }

    private void f_UnregTimeEvent()
    {
        if (timeEventId != -99)
        {
            ccTimeEvent.GetInstance().f_UnRegEvent(timeEventId);
            timeEventId = -99;
        }
    }

    private int curServerTime;
    private void f_UpdateBySecond(object value)
    {
        curServerTime = GameSocket.GetInstance().f_GetServerTime();
        if (!mSigupSubPanel)
        {
            return;
        }
        mSigupSubPanel.f_UpdateBySecond(curServerTime);
        if (curServerTime >= LegionMain.GetInstance().m_LegionBattlePool.m_iBeginTime
            && curServerTime <= LegionMain.GetInstance().m_LegionBattlePool.m_iEndTime)
        {
            if (!isBattling)
            {
                LegionMain.GetInstance().m_LegionBattlePool.f_SetBattleTime(curServerTime);
                isBattling = curServerTime >= LegionMain.GetInstance().m_LegionBattlePool.m_iBeginTime && curServerTime <= LegionMain.GetInstance().m_LegionBattlePool.m_iEndTime;
                mSelfStarNum.text = string.Format("{0}", isBattling ? LegionMain.GetInstance().m_LegionBattlePool.m_EnemyPoolDt.m_iStarNum : 0);
                mEnemyStarNum.text = string.Format("{0}", isBattling ? LegionMain.GetInstance().m_LegionBattlePool.m_SelfPoolDt.m_iStarNum : 0);
                for (int i = 0; i < mBattleItems.Length; i++)
                {
                    mBattleItems[i].f_UpdateByInfo(LegionMain.GetInstance().m_LegionBattlePool.m_State, curData.m_lLegionId,isBattling, curData.f_GetGateNode((EM_LegionGate)i + 1));
                }
                mSigupSubPanel.f_Close();
                f_LegionBattleInit();
            }
            int tTimeDis = LegionMain.GetInstance().m_LegionBattlePool.m_iEndTime - curServerTime;
            mEndTip.text = string.Format(CommonTools.f_GetTransLanguage(486), tTimeDis / 3600, tTimeDis / 60 % 60, tTimeDis % 60); 
        }
        else
        {
            if (isBattling)
            {
                LegionMain.GetInstance().m_LegionBattlePool.f_SetBattleTime(curServerTime);
                isBattling = curServerTime >= LegionMain.GetInstance().m_LegionBattlePool.m_iBeginTime && curServerTime <= LegionMain.GetInstance().m_LegionBattlePool.m_iEndTime;
                mSelfStarNum.text = string.Format("{0}", isBattling ? LegionMain.GetInstance().m_LegionBattlePool.m_EnemyPoolDt.m_iStarNum : 0);
                mEnemyStarNum.text = string.Format("{0}", isBattling ? LegionMain.GetInstance().m_LegionBattlePool.m_SelfPoolDt.m_iStarNum : 0);
                for (int i = 0; i < mBattleItems.Length; i++)
                {
                    mBattleItems[i].f_UpdateByInfo(LegionMain.GetInstance().m_LegionBattlePool.m_State, curData.m_lLegionId, isBattling, curData.f_GetGateNode((EM_LegionGate)i + 1));
                }
                mSigupSubPanel.f_Open();
            }
            int tTimeDis = LegionMain.GetInstance().m_LegionBattlePool.m_iBeginTime - curServerTime;
            if (tTimeDis > 0)
                mEndTip.text = string.Format(CommonTools.f_GetTransLanguage(487), tTimeDis / 3600, tTimeDis / 60 % 60, tTimeDis % 60);
            else
                mEndTip.text = string.Empty;
        }
    }

    private void f_UpdateCurData(LegionBattlePoolDT poolDt)
    {
        curData = poolDt;
        mBtnSelfLegionSelect.SetActive(curData.m_bIsSelf);
        mBtnEnemyLegionSelect.SetActive(!curData.m_bIsSelf);
        mSelfStarNum.text = string.Format("{0}", isBattling ? LegionMain.GetInstance().m_LegionBattlePool.m_EnemyPoolDt.m_iStarNum : 0);
        mEnemyStarNum.text = string.Format("{0}", isBattling ? LegionMain.GetInstance().m_LegionBattlePool.m_SelfPoolDt.m_iStarNum : 0);
        for (int i = 0; i < mBattleItems.Length; i++)
        {
            mBattleItems[i].f_UpdateByInfo(LegionMain.GetInstance().m_LegionBattlePool.m_State, curData.m_lLegionId, isBattling,curData.f_GetGateNode((EM_LegionGate)i + 1));
        }
        mChallengeTimes.text = string.Format(CommonTools.f_GetTransLanguage(488), LegionMain.GetInstance().m_LegionBattlePool.m_iTimesLimit - LegionMain.GetInstance().m_LegionBattlePool.m_iTimes);
        mEnemyLegionName.gameObject.SetActive(!curData.m_bIsSelf 
            && isBattling 
            && LegionMain.GetInstance().m_LegionBattlePool.m_State != EM_LegionBattleState.eLegionBattle_Init 
            && LegionMain.GetInstance().m_LegionBattlePool.m_State != EM_LegionBattleState.eLegionBattle_Matching);        
        mEnemyLegionName.text = string.IsNullOrEmpty(curData.m_szLegionName)? CommonTools.f_GetTransLanguage(489) : curData.m_szLegionName;
        f_UpdateBySecond(null);
    }

    #region 按钮

    private void f_OnBtnBack(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionBattlePage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
    }
    private void f_OnSigupPanelClose(GameObject go, object value1, object value2)
    {
        if (LegionMain.GetInstance().m_LegionBattlePool.m_iSignupTime != 0)
        {
            mSigupSubPanel.f_Close();
        }
        else
        {
            //UITool.Ui_Trip("军团战报名了，才可以关闭报名界面");
            mSigupSubPanel.f_Close();
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionBattlePage, UIMessageDef.UI_CLOSE);
            ccUIHoldPool.GetInstance().f_UnHold();
        }
    }

    private void f_OnBtnSelfLegionClick(GameObject go, object value1, object value2)
    {
        if (curData.m_bIsSelf)
            return;
        f_UpdateCurData(LegionMain.GetInstance().m_LegionBattlePool.m_SelfPoolDt);
    }

    private void f_OnBtnEnemyLegionClick(GameObject go, object value1, object value2)
    {
        if (!curData.m_bIsSelf)
            return;
        f_UpdateCurData(LegionMain.GetInstance().m_LegionBattlePool.m_EnemyPoolDt);
    }

    private void f_OnBtnLegionBattleAwardClick(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionBattleAwardPage, UIMessageDef.UI_OPEN);
    }

    private void f_OnBtnLegionBattleListClick(GameObject go, object value1, object value2)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_LegionBattleTable;
        socketCallbackDt.m_ccCallbackFail = f_Callback_LegionBattleTable;
        LegionMain.GetInstance().m_LegionBattlePool.f_LegionBattleTable(socketCallbackDt);
    } 

    private void f_OnBtnLineupClick(GameObject go, object value1, object value2)
    {
        //打开布阵
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ClothArrayPage, UIMessageDef.UI_OPEN);
    }

    private EM_LegionGate curClickGate = EM_LegionGate.Invalid;
    private void f_OnBattleItemClick(GameObject go, object value1, object value2)
    {
        int tNow = GameSocket.GetInstance().f_GetServerTime();
        LegionMain.GetInstance().m_LegionBattlePool.f_SetBattleTime(tNow);
        if (LegionMain.GetInstance().m_LegionBattlePool.m_State == EM_LegionBattleState.eLegionBattle_Init
            || LegionMain.GetInstance().m_LegionBattlePool.m_State == EM_LegionBattleState.eLegionBattle_Matching)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(490));
            return;
        }
        else if (tNow < LegionMain.GetInstance().m_LegionBattlePool.m_iBeginTime
            || tNow > LegionMain.GetInstance().m_LegionBattlePool.m_iEndTime)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(491));
            return;
        }
        else if (curData.m_lLegionId == 0)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(492));
            return;
        } 
        UITool.f_OpenOrCloseWaitTip(true);
        curClickGate = (EM_LegionGate)value1;
        int tFlag = curData.m_bIsSelf ? 0 : 1;
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_DefenceList;
        socketCallbackDt.m_ccCallbackFail = f_Callback_DefenceList;
        LegionMain.GetInstance().m_LegionBattlePool.f_LegionBattleDefenceList(tFlag, (int)curClickGate, socketCallbackDt);
    }

    #endregion

    private void f_Callback_DefenceList(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            //ccUIHoldPool.GetInstance().f_Hold(this); 
            LegionBattleGatePageParam tParam = new LegionBattleGatePageParam(curData.m_bIsSelf, curData.f_GetGateNode(curClickGate), false);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionBattleGatePage, UIMessageDef.UI_OPEN, tParam);
        }
        else
        {
            MessageBox.ASSERT("LegionBattleDefenceList is error,code:" + result);
        }
    } 
	
	private void f_OnHelpBtnClick(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonHelpPage, UIMessageDef.UI_OPEN, 20);
    }

    private void f_Callback_LegionBattleInit(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result != (int)eMsgOperateResult.OR_Succeed)
        {
            MessageBox.ASSERT("LegionBattleInit error,code:" + result);
        }
        LegionMain.GetInstance().m_LegionBattlePool.f_SetBattleTime(curServerTime);
        isBattling = curServerTime >= LegionMain.GetInstance().m_LegionBattlePool.m_iBeginTime && curServerTime <= LegionMain.GetInstance().m_LegionBattlePool.m_iEndTime;
        mSigupSubPanel.f_Open();
        if (curData.m_bIsSelf)
        {
            f_UpdateCurData(LegionMain.GetInstance().m_LegionBattlePool.m_SelfPoolDt);
        }
        else
        {
            f_UpdateCurData(LegionMain.GetInstance().m_LegionBattlePool.m_EnemyPoolDt);
        }
        if (LegionMain.GetInstance().m_LegionBattlePool.m_State == EM_LegionBattleState.eLegionBattle_Init
            || LegionMain.GetInstance().m_LegionBattlePool.m_State == EM_LegionBattleState.eLegionBattle_Matching)
        {
            f_ResetLegionBattleInitCD();
            timeEvent_InitId = ccTimeEvent.GetInstance().f_RegEvent(LegionBattleInitCD, false,null, f_OnLegionBattleInitCD);
        }
    }

    private void f_Callback_LegionBattleTable(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result != (int)eMsgOperateResult.OR_Succeed)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(493));
            MessageBox.ASSERT("LegionBattleTable is error,code:" + result);
            return;
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionBattleListPage, UIMessageDef.UI_OPEN);
    }

    private void f_ProcessByMsg_ForceClose(object value)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionBattlePage, UIMessageDef.UI_CLOSE);
    }

    /// <summary>
    /// 军团战斗初始化
    /// </summary>
    private void f_LegionBattleInit()
    {
        f_ResetLegionBattleInitCD();
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_LegionBattleInit;
        socketCallbackDt.m_ccCallbackFail = f_Callback_LegionBattleInit;
        LegionMain.GetInstance().m_LegionBattlePool.f_LegionBattleInit(socketCallbackDt);
    }

    private const float LegionBattleInitCD = 10.0f;
    private int timeEvent_InitId = 0;
    private void f_OnLegionBattleInitCD(object value)
    {
        f_ResetLegionBattleInitCD();
        f_LegionBattleInit(); 
    }

    private void f_ResetLegionBattleInitCD()
    {
        if (timeEvent_InitId != 0)
        {
            ccTimeEvent.GetInstance().f_UnRegEvent(timeEvent_InitId);
            timeEvent_InitId = 0;
        }
    }
}
