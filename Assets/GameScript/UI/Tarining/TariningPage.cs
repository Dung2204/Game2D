using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;

public class TariningPage : UIFramwork
{
    private Transform ItemParent;
    private Transform Item;

    private SocketCallbackDT _OneTarining = new SocketCallbackDT();
    private int TrainingIdx;
    private ccCallback _OneTariningCallback;


    private SocketCallbackDT _GetAward = new SocketCallbackDT();
    private int GetAwardIdx;
    private ccCallback _GetAwardCallback;

    public static bool IsTween;

    private string _TariningDesc = "";//+CommonTools.f_GetTransLanguage(1619);

    private int FreeTime = 0;
    private int NeedMoney = 0;
    private int NeedSycee = 0;

    private string _BgAdress = "UI/TextureRemove/Tarining/Texture_TariningBg";

    #region 红点

    protected override void InitRaddot()
    {
        base.InitRaddot();
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.TacticalStdySkill, f_GetObject("Btn_Tactical"), Btn_TacticalRed);
    }

    protected override void UpdateReddotUI()
    {
        base.UpdateReddotUI();
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.TacticalStdySkill);
    }
    private void Btn_TacticalRed(object obj)
    {
        int iNum = (int)obj;
        GameObject BtnActivity = f_GetObject("Btn_Tactical");
        UITool.f_UpdateReddot(BtnActivity, iNum, new Vector3(55, 55, 0), 102);
    }

    #endregion


    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        InitGUI();
        UpdateMainUI();
        InitBg();
    }
    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        f_GetObject(UIEffectName.lianbingchang_lianbing_01).SetActive(false);
        InitGUI();
        UpdateMainUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        ItemParent = f_GetObject("ItemParent").transform;
        Item = f_GetObject("Item").transform;
        _TariningDesc = CommonTools.f_GetTransLanguage(1619);
        _OneTarining.m_ccCallbackFail = OneTariningFail;
        _OneTarining.m_ccCallbackSuc = OneTariningSuc;
        TrainingIdx = -99;

        _GetAward.m_ccCallbackFail = GetAwardFail;
        _GetAward.m_ccCallbackSuc = GetAwardSuc;
        GetAwardIdx = -99;

        IsTween = false;

        List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
        listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Money);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, listMoneyType);

        GameParamDT tGameParamDT = glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.OneTariningNeedMoneny) as GameParamDT;
        NeedMoney = tGameParamDT.iParam1;
        tGameParamDT = glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.SyceeTarining) as GameParamDT;
        NeedSycee = tGameParamDT.iParam1;

        f_GetObject(UIEffectName.lianbingchang_lianbing_01).SetActive(false);
    }


    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("SyceeTarining", Btn_SyceeTarining);
        f_RegClickEvent("OneKeyTarining", Btn_OneKeyTarining);
        f_RegClickEvent("OneKeyGetAward", Btn_OneKeyGet);
        f_RegClickEvent("Btn_Close", Btn_ClosePage);
        f_RegClickEvent("Btn_Tactical", Btn_OpenTactical);
		f_RegClickEvent("Btn_Help", f_OnHelpBtnClick);
    }

    private void InitBg()
    {
        // f_GetObject("Bg").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(_BgAdress);
    }
    private void UpdateMainUI()
    {
        if(ItemParent.childCount != 20)
        {
            for(int i = 0; i < ItemParent.childCount; i++)
                Destroy(ItemParent.GetChild(i).gameObject);
            for(int i = 0; i < 20; i++)
                NGUITools.AddChild(ItemParent.gameObject, Item.gameObject);
            ItemParent.GetComponent<UIGrid>().Reposition();
        }

        for(int i = 0; i < ItemParent.childCount; i++)
        {
            ItemParent.GetChild(i).gameObject.SetActive(true);
            //if (Data_Pool.m_TariningAndTacticalPool.m_TariningInfo.m_TariningInfo[i].TimeStamp != 0)
            //{
            TariningInfoItem ttt = Data_Pool.m_TariningAndTacticalPool.m_TariningInfo.m_TariningInfo[i];
            ItemParent.GetChild(i).GetComponent<TariningItem>().UpdateItem(ttt.awardType, ttt.awardId, ttt.Num, i, ttt.TimeStamp, OneTariningSend, GetAwardSend);
            //}
        }
        UpdateDesc();
        UpdateGoodsNum();
    }

    private void UpdateDesc()
    {
        int time = 10 - Data_Pool.m_TariningAndTacticalPool.m_TariningInfo.TariningTime;
        FreeTime = time <= 0 ? 0 : time;
        f_GetObject("DescLabel").GetComponent<UILabel>().text = string.Format(_TariningDesc, FreeTime, NeedMoney, NeedSycee);
        UpdateGoodsNum();
    }

    private void UpdateGoodsNum()
    {

        int GoodsID = Data_Pool.m_TariningAndTacticalPool.TariningGoodsId;
        f_GetObject("TariningGoods").GetComponent<ResourceCommonItem>().f_UpdateByInfo((int)EM_ResourceType.Good,
           GoodsID, Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(GoodsID));
    }

    #region 按钮事件

    private void Btn_ClosePage(GameObject go, object obj1, object obj2)
    {
        if(IsTween)
            return;
        ccUIHoldPool.GetInstance().f_UnHold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TariningPage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
    }
	
	private void f_OnHelpBtnClick(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonHelpPage, UIMessageDef.UI_OPEN, 23);
    }

    /// <summary>
    /// 打开阵法
    /// </summary>
    private void Btn_OpenTactical(GameObject go, object obj1, object obj2)
    {
        if(IsTween)
            return;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TacticalPage, UIMessageDef.UI_OPEN);
        ccUIHoldPool.GetInstance().f_Hold(this);
    }
    /// <summary>
    /// 元宝练兵
    /// </summary>
    private void Btn_SyceeTarining(GameObject go, object obj1, object obj2)
    {
        if(IsTween)
            return;
        bool IsMaxAward = true;

        for(int i = 0; i < Data_Pool.m_TariningAndTacticalPool.m_TariningInfo.m_TariningInfo.Length; i++)
        {
            if(Data_Pool.m_TariningAndTacticalPool.m_TariningInfo.m_TariningInfo[i].awardId == 0)
            {
                IsMaxAward = false;
                break;
            }
        }

        if(IsMaxAward)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1620));
            return;
        }

        int SyceeNum = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.SyceeTarining) as GameParamDT).iParam1;
        if(Data_Pool.m_UserData.f_GetProperty((int)EM_MoneyType.eUserAttr_Sycee) < SyceeNum)
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1621), SyceeNum));
            return;
        }
        SocketCallbackDT tt = new SocketCallbackDT();
        tt.m_ccCallbackSuc = OneKeyTariningSuc;
        tt.m_ccCallbackFail = SyceeTrainingFail;
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_TariningAndTacticalPool.f_TacticalTransSycee(tt);
    }
    /// <summary>
    /// 一键练兵
    /// </summary>
    private void Btn_OneKeyTarining(GameObject go, object obj1, object obj2)
    {
        if(IsTween)
            return;
        bool IsMaxAward = true;

        for(int i = 0; i < Data_Pool.m_TariningAndTacticalPool.m_TariningInfo.m_TariningInfo.Length; i++)
        {
            if(Data_Pool.m_TariningAndTacticalPool.m_TariningInfo.m_TariningInfo[i].awardId == 0)
            {
                IsMaxAward = false;
                break;
            }
        }

        if(IsMaxAward)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1620));
            return;
        }
        int residue = 1;
        int GoodsNum = Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(Data_Pool.m_TariningAndTacticalPool.TariningGoodsId);
        int TariningMoneyNum = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.OneTariningNeedMoneny) as GameParamDT).iParam1;
        //for(int i = 0; i < Data_Pool.m_TariningAndTacticalPool.m_TariningInfo.m_TariningInfo.Length; i++)
        //    if(Data_Pool.m_TariningAndTacticalPool.m_TariningInfo.m_TariningInfo[i].TimeStamp == 0)
        //        residue++;
        residue = residue - (FreeTime + GoodsNum);
        residue = residue <= 0 ? 0 : residue;


        if(residue * TariningMoneyNum > Data_Pool.m_UserData.f_GetProperty((int)EM_MoneyType.eUserAttr_Money) && FreeTime == 0 && GoodsNum == 0)
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1622)));
            return;
        }

		glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(AudioEffectType.Gain_Purple);
        SocketCallbackDT tt = new SocketCallbackDT();
        tt.m_ccCallbackFail = OneKeyTariningFail;
        tt.m_ccCallbackSuc = OneKeyTariningSuc;
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_TariningAndTacticalPool.f_OneKeyMoneyTrainingTransSend(tt);
    }

    /// <summary>
    /// 一键拾取
    /// </summary>
    private void Btn_OneKeyGet(GameObject go, object obj1, object obj2)
    {
        if(IsTween)
            return;
        int CanGetAwardTime = 0;
        for(int i = 0; i < Data_Pool.m_TariningAndTacticalPool.m_TariningInfo.m_TariningInfo.Length; i++)
            if(Data_Pool.m_TariningAndTacticalPool.m_TariningInfo.m_TariningInfo[i].TimeStamp != 0)
                CanGetAwardTime++;
        if(CanGetAwardTime == 0)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1623));
            return;
        }

        SocketCallbackDT tt = new SocketCallbackDT();
        tt.m_ccCallbackFail = OneKeyGetAwardFail;
        tt.m_ccCallbackSuc = OneKeyGetAwardSuc;
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_TariningAndTacticalPool.f_TacticalPickupOneKey(tt);
    }
    #endregion



    private void OneTariningSend(object obj)
    {

        int residue = 1;
        int GoodsNum = Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(Data_Pool.m_TariningAndTacticalPool.TariningGoodsId);
        int TariningMoneyNum = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.OneTariningNeedMoneny) as GameParamDT).iParam1;
        //for(int i = 0; i < Data_Pool.m_TariningAndTacticalPool.m_TariningInfo.m_TariningInfo.Length; i++)
        //    if(Data_Pool.m_TariningAndTacticalPool.m_TariningInfo.m_TariningInfo[i].TimeStamp == 0)
        //        residue++;
        residue = residue - (FreeTime + GoodsNum);
        residue = residue <= 0 ? 0 : residue;


        if(residue * TariningMoneyNum > Data_Pool.m_UserData.f_GetProperty((int)EM_MoneyType.eUserAttr_Money) && FreeTime == 0 && GoodsNum == 0)
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1622)));
            return;
        }

		glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(AudioEffectType.Gain_Whith);
        TariningItemParam idx = (TariningItemParam)obj;
        TrainingIdx = idx.idx;
        _OneTariningCallback = idx._ReturnSockect;
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_TariningAndTacticalPool.f_TrainingTransSend((byte)TrainingIdx, _OneTarining);
    }
    private void OneTariningSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1624));
        TariningInfoItem ttt = Data_Pool.m_TariningAndTacticalPool.m_TariningInfo.m_TariningInfo[TrainingIdx];
        //ItemParent.GetChild(TrainingIdx).GetComponent<TariningItem>().UpdateItem(ttt.awardType, ttt.awardId, ttt.Num, TrainingIdx, ttt.TimeStamp, OneTariningSend, GetAwardSend);
        ResourceCommonDT ttResourceCommonDT = new ResourceCommonDT();
        ttResourceCommonDT.f_UpdateInfo(ttt.awardType, ttt.awardId, ttt.Num);
        if(_OneTariningCallback != null)
            _OneTariningCallback(ttResourceCommonDT);
        UpdateDesc();
    }
    private void OneTariningFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1625) + obj.ToString());
    }



    private void GetAwardSend(object obj)
    {
        TariningItemParam idx = (TariningItemParam)obj;
        GetAwardIdx = idx.idx;
        _GetAwardCallback = idx._ReturnGetAward;

        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_TariningAndTacticalPool.f_TrainingGetAward((byte)GetAwardIdx, _GetAward);
    }
    private void GetAwardSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if(_GetAwardCallback != null)
            _GetAwardCallback(GetAwardIdx);
        Data_Pool.m_ReddotMessagePool.f_MsgSubtract(EM_ReddotMsgType.TariningGetAward);
        MessageBox.DEBUG(CommonTools.f_GetTransLanguage(1626));
    }
    private void GetAwardFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        MessageBox.DEBUG(CommonTools.f_GetTransLanguage(1627) + obj.ToString());
    }


    private void OneKeyTariningSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        for(int i = 0; i < ItemParent.childCount; i++)
        {
            TariningItem tTrainingItem = ItemParent.GetChild(i).GetComponent<TariningItem>();

            if(!tTrainingItem.OneKeyShowItem())
            {
                TariningInfoItem ttt = Data_Pool.m_TariningAndTacticalPool.m_TariningInfo.m_TariningInfo[i];
                if(ttt.TimeStamp == 0)
                    continue;
                ResourceCommonDT tResourceCommonDT = new ResourceCommonDT();
                tResourceCommonDT.f_UpdateInfo(ttt.awardType, ttt.awardId, ttt.Num);
                tTrainingItem.OneKeyTweenPlay(tResourceCommonDT);
            }
        }
        UpdateDesc();
    }
    private void OneKeyTariningFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1628) + obj.ToString());
    }

    private void OneKeyGetAwardSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        for(int i = 0; i < ItemParent.childCount; i++)
        {
            TariningItem tTrainingItem = ItemParent.GetChild(i).GetComponent<TariningItem>();

            if(tTrainingItem.OneKeyShowItem())
            {
                //TariningInfoItem ttt = Data_Pool.m_TariningAndTacticalPool.m_TariningInfo.m_TariningInfo[i];
                //ResourceCommonDT tResourceCommonDT = new ResourceCommonDT();
                //tResourceCommonDT.f_UpdateInfo(ttt.awardType, ttt.awardId, ttt.Num);
                tTrainingItem.OneKeyGetAward(null);
            }
        }
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.TariningGetAward);
    }
    private void OneKeyGetAwardFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1629) + obj);
    }

    private void SyceeTrainingFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1630) + obj.ToString());
    }
}
