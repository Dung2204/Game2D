using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class LegionBattleChallengePage : UIFramwork
{
    private GameObject[] mChallengeBtns;
    private GameObject[] mLockTips;

    private LegionBattleGateDefenderNode curData;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mChallengeBtns = new GameObject[3];
        for (int i = 0; i < mChallengeBtns.Length; i++)
        {
            mChallengeBtns[i] = f_GetObject(string.Format("ChallengeBtn{0}",i+1));
            f_RegClickEvent(mChallengeBtns[i], f_OnChallengeBtnClick, i + 1);
        }
        mLockTips = new GameObject[3];
        for (int i = 0; i < mLockTips.Length; i++)
        {
            mLockTips[i] = f_GetObject(string.Format("LockTip{0}", i + 1));
        }
        f_RegClickEvent("BtnClose", f_OnBtnCloseClick);
        f_RegClickEvent("BlackBG", f_OnBtnCloseClick);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (e == null || !(e is LegionBattleGateDefenderNode))
        {
            MessageBox.ASSERT("LegionBattleChallengePage param error");
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionBattleChallengePage, UIMessageDef.UI_CLOSE);
            return;
        }
        curData = (LegionBattleGateDefenderNode)e;
        f_UpdateByInfo(curData);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
    }

    private void f_UpdateByInfo(LegionBattleGateDefenderNode node)
    {
        for (int i = 0; i < mChallengeBtns.Length; i++)
        {
            mChallengeBtns[i].SetActive(i+1 > node.m_iStarNum);
        }
        for (int i = 0; i < mLockTips.Length; i++)
        {
            mLockTips[i].SetActive(i+1 <= node.m_iStarNum);
			MessageBox.ASSERT("Star: " + node.m_iStarNum);
        }
    }

    private void f_OnChallengeBtnClick(GameObject go, object value1, object value2)
    {
        int tStar = (int)value1;
        int tNow = GameSocket.GetInstance().f_GetServerTime();
        LegionMain.GetInstance().m_LegionBattlePool.f_SetBattleTime(tNow);
        if (LegionMain.GetInstance().m_LegionBattlePool.m_iBeginTime <= LegionMain.GetInstance().m_LegionInfor.m_iIOTime + LegionConst.LEGION_BATTLE_CHALLENGE_TIME_LIMIT)
        {
UITool.Ui_Trip("Thời gian gia nhập quân đoàn chưa đủ 24h");
            return;
        }
        else if (tNow < LegionMain.GetInstance().m_LegionBattlePool.m_iBeginTime
            || tNow > LegionMain.GetInstance().m_LegionBattlePool.m_iEndTime)
        {
UITool.Ui_Trip("Quân đoàn chiến đã kết thúc");
            return;
        }
        else if (LegionMain.GetInstance().m_LegionBattlePool.m_iTimes >= LegionMain.GetInstance().m_LegionBattlePool.m_iTimesLimit)
        {
UITool.Ui_Trip("Đã hết thời gian khiêu chiến");
            return;
        }
        else if (curData.m_iStarNum >= curData.m_iStarMaxNum)
        {
UITool.Ui_Trip("Tối đa");
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_LegionBattleChallenge;
        socketCallbackDt.m_ccCallbackFail = f_Callback_LegionBattleChallenge;
        LegionMain.GetInstance().m_LegionBattlePool.f_LegionBattleChallenge((int)curData.m_GateType, (byte)curData.m_iIdx, (byte)tStar, socketCallbackDt);
    }

    private void f_OnBtnCloseClick(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionBattleChallengePage, UIMessageDef.UI_CLOSE);
    }

    private void f_Callback_LegionBattleChallenge(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult tResult = (eMsgOperateResult)result;
        if (tResult == eMsgOperateResult.OR_Succeed)
        {
            //挑战成功切换场景
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionBattleChallengePage, UIMessageDef.UI_CLOSE);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionBattleGatePage, UIMessageDef.UI_CLOSE);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.BattleMain);
            return;
        }
        else if (tResult == eMsgOperateResult.eOR_LegionEnemyBusy)
        {
UITool.Ui_Trip("Đã có người khác khiêu chiến");
            f_RequestDefenceListAgain();
        }
        else if (tResult == eMsgOperateResult.eOR_LegionEnemyBusy)
        {
UITool.Ui_Trip("Cần khiêu chiến mức sao cao hơn");
            f_RequestDefenceListAgain();
        }
        else if (tResult == eMsgOperateResult.eOR_LegionBattleInvalidMen)
        {
UITool.Ui_Trip("Thời gian gia nhập quân đoàn chưa đủ 24h");
        }
        else if (tResult == eMsgOperateResult.eOR_TimesLimit)
        {
UITool.Ui_Trip("Đã hết thời gian khiêu chiến");
        }
        else
        {
UITool.Ui_Trip("Phát sinh lỗi");
            MessageBox.ASSERT("LegionBattleChallenge error,code:" + result);
        }
        //失败就刷新切界面处理参数
        StaticValue.m_Battle2MenuProcessParam.f_UpdateParam(EM_Battle2MenuProcess.None);
    }

    private void f_RequestDefenceListAgain()
    {
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_DefenceList;
        socketCallbackDt.m_ccCallbackFail = f_Callback_DefenceList;
        LegionMain.GetInstance().m_LegionBattlePool.f_LegionBattleDefenceList(1, (int)curData.m_GateType, socketCallbackDt);
    }

    private void f_Callback_DefenceList(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result != (int)eMsgOperateResult.OR_Succeed)
        {
            MessageBox.ASSERT("LegionBattleDefenceList is error,code:" + result);
        }
        f_UpdateByInfo(curData);
    }

    private void f_ProcessByMsg_ForceClose(object value)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionBattleChallengePage, UIMessageDef.UI_CLOSE);
    }
}
