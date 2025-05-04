using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class RunningManEliteChallengePage : UIFramwork
{
    private UILabel mTollgateName;
    private UILabel mTollgateId;
    private Transform mRoleParent;

    private GameObject role;

    private RunningManElitePoolDT mData;

    private UILabel mFirstAwardLabel;
    private GameObject mPassedTip;
    private GameObject mAwardItem;
    private UIGrid mPassAwardGrid;
    private ResourceCommonItemComponent mPassAwardShow;

    private UILabel mEliteTimesLabel;
    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mTollgateName = f_GetObject("TollgateName").GetComponent<UILabel>();
        mTollgateId = f_GetObject("TollgateId").GetComponent<UILabel>();
        mRoleParent = f_GetObject("RoleParent").transform;

        mFirstAwardLabel = f_GetObject("FirstAwardLabel").GetComponent<UILabel>();
        mPassedTip = f_GetObject("PassedTip");
        mAwardItem = f_GetObject("RMEliteAwardItem");
        mPassAwardGrid = f_GetObject("PassAwardGrid").GetComponent<UIGrid>();
        mEliteTimesLabel = f_GetObject("EliteTimesLabel").GetComponent<UILabel>();

        f_RegClickEvent("BtnBuyTimes", f_BtnBuyTimes);
        f_RegClickEvent("BtnClose", f_BtnClose);
        f_RegClickEvent("MaskClose", f_BtnClose);
        f_RegClickEvent("BtnChallenge", f_TollgateChallengeHandle);

    }
    private int eliteLeftTimes;
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (e == null || !(e is RunningManElitePoolDT))
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(842));
        f_UpdateByInfo(e as RunningManElitePoolDT);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_THENEXTDAY_UIPROCESS, f_ProcessNextDay, this);
    }

    private void f_ProcessNextDay(object value)
    {
        if ((EM_NextDaySource)value != EM_NextDaySource.RunningManPool)
            return;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManChallengePage, UIMessageDef.UI_CLOSE);
    }

    private void f_UpdateByInfo(RunningManElitePoolDT data)
    {
        mData = data;
        mTollgateId.text = data.m_Template.iId.ToString();
        mTollgateName.text = data.m_Template.szName;
        UITool.f_CreateRoleByModeId(data.m_Template.iShowMonster, ref role, mRoleParent, 11, false);


        mFirstAwardLabel.text = data.m_Template.iAwardFirst.ToString();
        mPassedTip.SetActive(data.m_bIsPassed);
        if (mPassAwardShow == null)
            mPassAwardShow = new ResourceCommonItemComponent(mPassAwardGrid, mAwardItem);
        eliteLeftTimes = Data_Pool.m_RunningManPool.m_iEliteBuyTimes + GameParamConst.RMEliteTimesLimit - Data_Pool.m_RunningManPool.m_iEliteTimes;
mEliteTimesLabel.text = string.Format("Today's Hit: {0}", eliteLeftTimes);
        mPassAwardShow.f_Show(Data_Pool.m_AwardPool.f_GetAwardByString(data.m_Template.szAward));
    }

    private void f_TollgateChallengeHandle(GameObject go, object value1, object value2)
    {
        if (eliteLeftTimes <= 0)
        {
UITool.Ui_Trip("Đã kết thúc!");
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_CallbackChallenge;
        socketCallbackDt.m_ccCallbackFail = f_CallbackChallenge;
        Data_Pool.m_RunningManPool.f_RunningManElite((ushort)mData.m_Template.iId, mData.m_Template.iSceneId, socketCallbackDt);
    }

    private void f_CallbackChallenge(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManEliteChallengePage, UIMessageDef.UI_CLOSE);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.BattleMain);
        }
        else
        {
MessageBox.ASSERT("Passed and killed the general Tinh Anh，code：" + result);
        }
    }

    private void f_BtnClose(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManEliteChallengePage, UIMessageDef.UI_CLOSE);
    }

    private void f_BtnBuyTimes(GameObject go, object value1, object value2)
    {
        int tBuyTimes = Data_Pool.m_RunningManPool.m_iEliteBuyTimes;
        int tBuyTimesLimit = Data_Pool.m_RechargePool.f_GetCurLvVipPriValue(EM_VipPrivilege.eVip_RunningManEliteBuyTimes);
        if (tBuyTimes >= tBuyTimesLimit)
        {
UITool.Ui_Trip("Đã đạt đến giới hạn mua hàng của ngày hôm nay!");
            return;
        }
        int tSyceeCost = GameFormula.f_RMEliteBuyTimesCost(tBuyTimes);
        int tHaveSycee = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee);
        if (tHaveSycee < tSyceeCost)
        {
UITool.Ui_Trip("Không đủ KNB!");
            return;
        }
string tContent = string.Format("Đã xác nhận sử dụng {0} KNB để mua lượt khiêu chiến?\nHôm nay còn {1} lần mua", tSyceeCost, tBuyTimesLimit - tBuyTimes);
PopupMenuParams tParam = new PopupMenuParams("Nhắc lại", tContent, "Xác nhận", f_SureBuyTimes, "Hủy bỏ");
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParam);
    }

    private void f_SureBuyTimes(object result)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_CallbackBuyTimes;
        socketCallbackDt.m_ccCallbackFail = f_CallbackBuyTimes;
        Data_Pool.m_RunningManPool.f_RunningManEliteTimes(socketCallbackDt);
    }

    private void f_CallbackBuyTimes(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            eliteLeftTimes = Data_Pool.m_RunningManPool.m_iEliteBuyTimes + GameParamConst.RMEliteTimesLimit - Data_Pool.m_RunningManPool.m_iEliteTimes;
mEliteTimesLabel.text = string.Format("Today's Hit ：{0}", eliteLeftTimes);
        }
        else
        {
MessageBox.ASSERT("Buy failed");
        }
    }
}
