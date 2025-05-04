using UnityEngine;
using System.Text;

public class LegionBattleSignupSubPanel : SubPanelBase
{
    private  string BATTLE_DESC = "";
    public UILabel mDesc;
    public UILabel mBeginTime;
    public GameObject mSignupBtn;
    public GameObject mAlreadySignupTip;
    public GameObject mBtnCLose;
	//My Code
	GameParamDT LegionParam;
	int legionLvLimit;
	//

    public override void f_Init(UIFramwork parentUI)
    {
        base.f_Init(parentUI);
        BATTLE_DESC = CommonTools.f_GetTransLanguage(494);
		LegionParam = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(97) as GameParamDT);
		legionLvLimit = LegionParam.iParam1;
        f_RegClickEvent(mSignupBtn, f_OnSignupBtnClick);
        StringBuilder tSb = new StringBuilder();
        tSb.Append(CommonTools.f_GetTransLanguage(495));
        for (int i = 0; i < LegionConst.LEGION_BATTLE_BEGIN_DAYOFWEEKS.Length; i++)
        {
            tSb.Append(UITool.f_DayOfWeek2String((System.DayOfWeek)LegionConst.LEGION_BATTLE_BEGIN_DAYOFWEEKS[i]));
            if (i != LegionConst.LEGION_BATTLE_BEGIN_DAYOFWEEKS.Length - 1)
                tSb.Append("，");
        }
        mDesc.text = string.Format(BATTLE_DESC, tSb.ToString(), LegionConst.LEGION_BATTLE_END_HOUR, LegionConst.LEGION_BATTLE_END_Minute, LegionConst.LEGION_BATTLE_SIGNUP_MEMBERNUM_LIMIT);
    }

    public override void f_Open()
    {
        base.f_Open();
        mSignupBtn.SetActive(LegionMain.GetInstance().m_LegionBattlePool.m_iSignupTime == 0);
        mAlreadySignupTip.SetActive(LegionMain.GetInstance().m_LegionBattlePool.m_iSignupTime != 0);
        int tNow = GameSocket.GetInstance().f_GetServerTime();
        mPanel.SetActive(tNow < LegionMain.GetInstance().m_LegionBattlePool.m_iBeginTime || tNow > LegionMain.GetInstance().m_LegionBattlePool.m_iEndTime);
        f_UpdateBySecond(tNow);
    }

    public override void f_Close()
    {
        base.f_Close();
    }

    public void f_UpdateBySecond(int serverTime)
    {
        if (!mPanel || !mPanel.activeSelf)
            return;
        int tTimeDis = LegionMain.GetInstance().m_LegionBattlePool.m_iBeginTime - serverTime;
        if (tTimeDis >= 0)
            mBeginTime.text = string.Format(CommonTools.f_GetTransLanguage(496), tTimeDis / 3600, tTimeDis / 60 % 60, tTimeDis % 60);
        else
            mBeginTime.text = string.Empty;
    }

    private void f_OnSignupBtnClick(GameObject go, object value1, object value2)
    {
        if (!LegionTool.f_IsEnoughPermission(EM_LegionOperateType.LegionBattleSignUp, true))
        {
            return;
        }
        else if (!LegionMain.GetInstance().m_LegionBattlePool.f_IsEnableSignUp(LegionMain.GetInstance().m_LegionInfor.f_getUserLegion().f_GetProperty((int)EM_LegionProperty.Lv) , LegionMain.GetInstance().m_LegionPlayerPool.m_iMemberNum))
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(497), legionLvLimit, LegionConst.LEGION_BATTLE_SIGNUP_MEMBERNUM_LIMIT));
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_BattleSignup;
        socketCallbackDt.m_ccCallbackFail = f_Callback_BattleSignup;
        LegionMain.GetInstance().m_LegionBattlePool.f_LegionBattleApply(socketCallbackDt);
    }

    private void f_Callback_BattleSignup(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(498));
        }
        //已报名，错误码处理
        else if ((int)result == (int)eMsgOperateResult.eOR_TimesLimit)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(499));
        }
        else if ((int)result == (int)eMsgOperateResult.eOR_LegionMenNotEnough)
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(500), LegionConst.LEGION_BATTLE_SIGNUP_MEMBERNUM_LIMIT,legionLvLimit));
        }
        else
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(501));
            MessageBox.ASSERT(string.Format(CommonTools.f_GetTransLanguage(502), result));
        }
        //刷新界面
        f_Open();
    }

}
