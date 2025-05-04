using ccU3DEngine;
using UnityEngine;
using System.Collections;


public class LegionDungeonBuyPage : UIFramwork
{
    /// <summary>
    /// 购买类型，目前是军团副本类型，军团战类型
    /// </summary>
    public enum EM_BuyType
    {
        Dungeon = 0,
        Battle = 1,
        CrossServerBattle = 2,
        ChaosBattle = 3, //TsuCode - CHaosBattle
    }

    private UILabel mTitleLabel;
    private UILabel mDungeonLeftTimesLabel;
    private UILabel mBuyLeftTimesLabel;
    private UILabel mSyceeCostLabel;
    private UILabel mBuyTimesLabel;

    private EM_BuyType _curType;
    private int _buyTimesLimit;
    private int _curTimes;
    private int _buyTimes;
    private int _syceeCost;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mTitleLabel = f_GetObject("TitleLabel").GetComponent<UILabel>();
        mDungeonLeftTimesLabel = f_GetObject("DungeonLeftTimesLabel").GetComponent<UILabel>();
        mBuyLeftTimesLabel = f_GetObject("BuyLeftTimesLabel").GetComponent<UILabel>();
        mSyceeCostLabel = f_GetObject("SyceeCostLabel").GetComponent<UILabel>();
        mBuyTimesLabel = f_GetObject("BuyTimesLabel").GetComponent<UILabel>();
        f_RegClickEvent("AddOneBtn", f_AddOneBtn);
        f_RegClickEvent("AddTenBtn", f_AddTenBtn);
        f_RegClickEvent("SubtractOneBtn", f_SubtractOneBtn);
        f_RegClickEvent("SubtractTenBtn", f_SubtractTenBtn);
        f_RegClickEvent("CancelBtn", f_CancelBtn);
        f_RegClickEvent("SureBtn", f_SureBtn);
        f_RegClickEvent("BlackBG", f_CancelBtn);
        f_RegClickEvent("Sprite_Close", f_CancelBtn);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        _curType = EM_BuyType.Dungeon;
        if (e != null && e is EM_BuyType)
            _curType = (EM_BuyType)e;
        int tLeftTimes = 0;
        if (_curType == EM_BuyType.Dungeon)
        {
            _buyTimesLimit = LegionMain.GetInstance().m_LegionDungeonPool.m_iDungeonBuyTimesLimit;
            _curTimes = LegionMain.GetInstance().m_LegionDungeonPool.m_iDungeonBuyTimes;
            tLeftTimes = LegionMain.GetInstance().m_LegionDungeonPool.m_iDungeonLeftTimes;
            tLeftTimes += LegionMain.GetInstance().m_LegionDungeonPool.f_GetDungeonExtraTimes(ccMath.time_t2DateTime(GameSocket.GetInstance().f_GetServerTime()));
mTitleLabel.text = "Mua lượt khiêu chiến Phó Bản";
        }
        else if (_curType == EM_BuyType.Battle)
        {
            _buyTimesLimit = LegionMain.GetInstance().m_LegionBattlePool.m_iBuyTimesLimit;
            _curTimes = LegionMain.GetInstance().m_LegionBattlePool.m_iBuyTimes;
            tLeftTimes = LegionMain.GetInstance().m_LegionBattlePool.m_iTimesLimit - LegionMain.GetInstance().m_LegionBattlePool.m_iTimes;
mTitleLabel.text = "Mua lượt khiêu chiến Quân Đoàn";
        }
        else if (_curType == EM_BuyType.CrossServerBattle)
        {
            _buyTimesLimit = Data_Pool.m_CrossServerBattlePool.BattleBuyTimesLimit;
            _curTimes = Data_Pool.m_CrossServerBattlePool.BattleBuyTimes;
            tLeftTimes = Data_Pool.m_CrossServerBattlePool.BattleLeftTimes;
mTitleLabel.text = "Mua lượt khiêu chiến Đấu Đỉnh Phong";
        }
        //TsuCode - ChaosBattle
        else if (_curType == EM_BuyType.ChaosBattle)
        {
            _buyTimesLimit = Data_Pool.m_ChaosBattlePool.BattleBuyTimesLimit;
            _curTimes = Data_Pool.m_ChaosBattlePool.BattleBuyTimes;
            tLeftTimes = Data_Pool.m_ChaosBattlePool.BattleLeftTimes;
mTitleLabel.text = "Mua lượt khiêu chiến Thí Luyện";
        }
        //---------------------
        _buyTimes = 0;
        if (!f_TimesAddOne(true))
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionDungeonBuyPage, UIMessageDef.UI_CLOSE);
            return;
        }
        f_UpdateViewByInfo();
mDungeonLeftTimesLabel.text = string.Format("Có sẵn：[fffaec]{0}", tLeftTimes);
mBuyLeftTimesLabel.text = string.Format("Tối đa：[fffaec]{0}", _buyTimesLimit - _curTimes);
        if (_curType != EM_BuyType.CrossServerBattle)
            glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
        //TsuCode - ChaosBattle
        if (_curType != EM_BuyType.ChaosBattle)
            glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
        //-----------------------
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        if (_curType != EM_BuyType.CrossServerBattle)
            glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
        //TsuCode - ChaosBattle
        if (_curType != EM_BuyType.ChaosBattle)
            glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
        //---------------------
    }

    private void f_UpdateViewByInfo()
    {
        mBuyTimesLabel.text = _buyTimes.ToString();
        mSyceeCostLabel.text = _syceeCost.ToString();
    }

    private void f_AddOneBtn(GameObject go, object value1, object value2)
    {
        f_TimesAddOne(false);
        f_UpdateViewByInfo();
    }

    private void f_AddTenBtn(GameObject go, object value1, object value2)
    {
        for (int i = 0; i < 10; i++)
        {
            if (!f_TimesAddOne(false))
                break;
        }
        f_UpdateViewByInfo();
    }

    private void f_SubtractOneBtn(GameObject go, object value1, object value2)
    {
        f_TimesSubtractOne();
        f_UpdateViewByInfo();
    }

    private void f_SubtractTenBtn(GameObject go, object value1, object value2)
    {
        for (int i = 0; i < 10; i++)
        {
            if (!f_TimesSubtractOne())
                break;
        }
        f_UpdateViewByInfo();
    }

    private bool f_TimesAddOne(bool showTip = false)
    {
        int tTimes = _curTimes + _buyTimes + 1;
        if (tTimes > _buyTimesLimit)
        {
            if (showTip)
UITool.Ui_Trip("Đã hết thời gian");
            return false;
        }
        int tSycess = 0;
        if (_curType == EM_BuyType.Dungeon)
        {
            tSycess = LegionTool.f_GetBuyDungeonTimesSyceeCost(_curTimes, _buyTimes + 1);
        }
        else if (_curType == EM_BuyType.Battle)
        {
            tSycess = LegionTool.f_GetBuyBattleTimesSyceeCost(_curTimes, _buyTimes + 1);
        }
        else if (_curType == EM_BuyType.CrossServerBattle)
        {
            tSycess = GameFormula.f_CrossServerBattleBuyTimesCost(_curTimes, _buyTimes + 1);
        }
        //TsuCode - ChaosBattle
        else if (_curType == EM_BuyType.ChaosBattle)
        {
            tSycess = GameFormula.f_ChaosBattleBuyTimesCost(_curTimes, _buyTimes + 1);
        }
        //------------
        if (!UITool.f_IsEnoughMoney(EM_MoneyType.eUserAttr_Sycee, tSycess, showTip, true, this))
            return false;
        _buyTimes++;
        _syceeCost = tSycess;
        return true;
    }

    private bool f_TimesSubtractOne()
    {
        if (_buyTimes - 1 <= 0)
            return false;
        _buyTimes--;
        if (_curType == EM_BuyType.Dungeon)
            _syceeCost = LegionTool.f_GetBuyDungeonTimesSyceeCost(_curTimes, _buyTimes);
        else if (_curType == EM_BuyType.Battle)
            _syceeCost = LegionTool.f_GetBuyBattleTimesSyceeCost(_curTimes, _buyTimes);
        else if (_curType == EM_BuyType.CrossServerBattle)
            _syceeCost = GameFormula.f_CrossServerBattleBuyTimesCost(_curTimes, _buyTimes);
        //TsuCode - ChaosBattle
        else if (_curType == EM_BuyType.ChaosBattle)
            _syceeCost = GameFormula.f_ChaosBattleBuyTimesCost(_curTimes, _buyTimes);
        //----------------
        return true;
    }

    private void f_CancelBtn(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionDungeonBuyPage, UIMessageDef.UI_CLOSE);
    }

    private void f_SureBtn(GameObject go, object value1, object value2)
    {
        if (_buyTimes > byte.MaxValue)
        {
UITool.Ui_Trip(string.Format("Chỉ có thể mua {0} lượt 1 lần", byte.MaxValue));
            return;
        }
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        UITool.f_OpenOrCloseWaitTip(true);
        if (_curType == EM_BuyType.Dungeon)
        {
            socketCallbackDt.m_ccCallbackSuc = f_Callback_BuyDungeonTimes;
            socketCallbackDt.m_ccCallbackFail = f_Callback_BuyDungeonTimes;
            LegionMain.GetInstance().m_LegionDungeonPool.f_BuyDungeonTimes((byte)_buyTimes, socketCallbackDt);
        }
        else if (_curType == EM_BuyType.Battle)
        {
            socketCallbackDt.m_ccCallbackSuc = f_Callback_BuyBattleTimes;
            socketCallbackDt.m_ccCallbackFail = f_Callback_BuyBattleTimes;
            LegionMain.GetInstance().m_LegionBattlePool.f_BuyBattleTimes((byte)_buyTimes, socketCallbackDt);
        }
        else if (_curType == EM_BuyType.CrossServerBattle)
        {
            socketCallbackDt.m_ccCallbackSuc = f_Callback_BuyBattleTimes;
            socketCallbackDt.m_ccCallbackFail = f_Callback_BuyBattleTimes;
            Data_Pool.m_CrossServerBattlePool.f_BuyTimes((byte)_buyTimes, socketCallbackDt);
        }
        //TsuCode - ChaosBattle
        else if (_curType == EM_BuyType.ChaosBattle)
        {
            socketCallbackDt.m_ccCallbackSuc = f_Callback_BuyChaosBattleTimes;
            socketCallbackDt.m_ccCallbackFail = f_Callback_BuyChaosBattleTimes;
            Data_Pool.m_ChaosBattlePool.f_BuyTimes((byte)_buyTimes, socketCallbackDt);
        }
        //------------
    }

    private void f_Callback_BuyDungeonTimes(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
UITool.Ui_Trip("Mua thành công");
        }
        else
        {
UITool.Ui_Trip("Mua thất bại, code:" + (int)result);
MessageBox.ASSERT("Buy legion PB failed,code:" + (int)result);
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionDungeonBuyPage, UIMessageDef.UI_CLOSE);
    }

    private void f_Callback_BuyBattleTimes(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
UITool.Ui_Trip("Mua thành công");
        }
        else
        {
UITool.Ui_Trip("Mua thất bại, code:" + (int)result);
MessageBox.ASSERT("Buy failed legion battle,code:" + (int)result);
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionDungeonBuyPage, UIMessageDef.UI_CLOSE);
    }

    private void f_Callback_BuyCrossServerBattleTimes(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
UITool.Ui_Trip("Mua thành công");
        }
        else
        {
UITool.Ui_Trip("Mua thất bại, code:" + (int)result);
MessageBox.ASSERT("Buying the Inter-Server Battlefield failed, code:" + (int)result);
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionDungeonBuyPage, UIMessageDef.UI_CLOSE);
    }

    private void f_ProcessByMsg_ForceClose(object value)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionDungeonBuyPage, UIMessageDef.UI_CLOSE);
    }
    //TsuCode -  ChaosBattle
    private void f_Callback_BuyChaosBattleTimes(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
UITool.Ui_Trip("Mua thành công");
        }
        else
        {
UITool.Ui_Trip("Mua thất bại, code:" + (int)result);
MessageBox.ASSERT("Purchase of Inter-Server Training Failed,code:" + (int)result);
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionDungeonBuyPage, UIMessageDef.UI_CLOSE);
    }
    //
}
