using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
using System;
using Spine;
using Spine.Unity;
/// <summary>
/// 活动首充界面
/// </summary>
public class FirstRechargePageNew : UIFramwork
{
    private SocketCallbackDT QueryCallback = new SocketCallbackDT();//查询回调
    private SocketCallbackDT RequestGetCallback = new SocketCallbackDT();//领取回调
    GameObject Magic;
    SkeletonAnimation ani;
    int Time_OpenUI;
    private FirstRechargePoolDT NowSelect;
    private int NowSelectId;
    private int nowSelectedDayId; //TsuCode - FirstRechargeNew - NapDau
	//My Code
	GameParamDT RoleList;
	public bool AwardState = false;
	//

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        f_GetObject("UI").SetActive(false);
        UITool.f_OpenOrCloseWaitTip(true);
        QueryCallback.m_ccCallbackSuc = QuerySuc;
        QueryCallback.m_ccCallbackFail = QueryFill;
        Data_Pool.m_FirstRechargePool.f_QueryInfo(QueryCallback);
        f_LoadTexture();

    }
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        UITool.f_CreateMagicById(12101, ref Magic, f_GetObject("SpineParent").transform, 20, "Stand", SpineEnd);
		RoleList = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(98) as GameParamDT);
		InitRecruitModel(f_GetObject("Special"), RoleList.iParam1, true, new Vector3(-355, -260, 0), 0);
        ani = Magic.GetComponent<SkeletonAnimation>();
        //ani.state.Complete += SpineEnd;
    }
    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        UpdateUI(NowSelectId);
    }
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BlackBg", OnCloseThis);
        f_RegClickEvent("BtnClose", OnCloseThis);
        f_RegClickEvent("Btn6", onClickBtn, 0);
        f_RegClickEvent("Btn68", onClickBtn, 1);
        f_RegClickEvent("Btn198", onClickBtn, 2);
        f_RegClickEvent("Get", OnClickGet);
        f_RegClickEvent("GoPay", GoToPay);

        //TsuCode - FirstRechargeNew - NapDau
        f_RegClickEvent("GetBtn", OnClickGet_btn, 0);
        f_RegClickEvent("GetBtn1", OnClickGet_btn, 1);
        f_RegClickEvent("GetBtn2", OnClickGet_btn, 2);
    }

    private void QuerySuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        Time_OpenUI = ccTimeEvent.GetInstance().f_RegEvent(0.6f, false, NowSelectId, UpdateUI);
    }
    private void QueryFill(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
    }

    private void UpdateUI(object obj)
    {
        f_GetObject("UI").SetActive(true);
        int index = 0;
        if (obj == null)
        {
            index = 0;
        }
        else
        {
            index = (int)obj;
        }
        if (index >= 3)
        {
            index = 2;
        }
        NowSelectId = index;
        NowSelect = Data_Pool.m_FirstRechargePool.f_GetForId((long)index + 1) as FirstRechargePoolDT; ;
        SetBtnState(index);
        UpdateGoods(index + 1);
        UpdateDesc(index + 1);
        UpdateBtn();
		AwardState = false;
        UpdateBtnState_btn("GetBtn", index + 1, 0);
        UpdateBtnState_btn("GetBtn1", index + 1, 1);
        UpdateBtnState_btn("GetBtn2", index + 1, 2);
    }

    private void SetBtnState(int index)
    {
        Transform BtnArr = f_GetObject("Btn").transform;

        for (int i = 0; i < BtnArr.childCount; i++)
        {
            BtnArr.GetChild(i).Find("Down").gameObject.SetActive(index == i);
            BtnArr.GetChild(i).Find("Up").gameObject.SetActive(index != i);
        }

    }
    private void UpdateGoods(int index)
    {

        FirstRechargePoolDT data = Data_Pool.m_FirstRechargePool.f_GetForId((long)index) as FirstRechargePoolDT;
        List<ResourceCommonDT> listCommonDT = CommonTools.f_GetListCommonDT(data.m_FirstRechargeDT.szAward);
        List<ResourceCommonDT> listCommonDT1 = CommonTools.f_GetListCommonDT(data.m_FirstRechargeDT.szAward1);
        List<ResourceCommonDT> listCommonDT2 = CommonTools.f_GetListCommonDT(data.m_FirstRechargeDT.szAward2);

        GridUtil.f_SetGridView<ResourceCommonDT>(f_GetObject("GoodsParent"), f_GetObject("Item"), listCommonDT, UpdateItem);
        GridUtil.f_SetGridView<ResourceCommonDT>(f_GetObject("GoodsParent1"), f_GetObject("Item"), listCommonDT1, UpdateItem);
        GridUtil.f_SetGridView<ResourceCommonDT>(f_GetObject("GoodsParent2"), f_GetObject("Item"), listCommonDT2, UpdateItem);



        f_GetObject("GoodsParent").transform.GetComponent<UIGrid>().Reposition();
        f_GetObject("GoodsParent1").transform.GetComponent<UIGrid>().Reposition(); //TsuCode - FirstRechargeNew
        f_GetObject("GoodsParent2").transform.GetComponent<UIGrid>().Reposition(); //TsuCode - FirstRechargeNew
        //f_GetObject("GoodsParent").transform.GetComponentInParent<UIScrollView>().ResetPosition();
    }

    private void UpdateItem(GameObject go, ResourceCommonDT Data)
    {
        Transform tran = go.transform;

        UISprite Case = tran.Find("Case").GetComponent<UISprite>();
        UI2DSprite Icon = tran.Find("Icon").GetComponent<UI2DSprite>();
        UILabel Num = tran.Find("Num").GetComponent<UILabel>();
        UILabel Nmae = tran.Find("Name").GetComponent<UILabel>();

        string name = Data.mName;
        Case.spriteName = UITool.f_GetImporentColorName(Data.mImportant, ref name);
        Icon.sprite2D = UITool.f_GetIconSprite(Data.mIcon);
        Num.text = Data.mResourceNum.ToString();
        Nmae.text = name;

        f_RegClickEvent(Case.gameObject, OnClickItem, Data);
    }

    private void UpdateDesc(int index)
    {
        FirstRechargePoolDT data = Data_Pool.m_FirstRechargePool.f_GetForId((long)index) as FirstRechargePoolDT;
        //int AllMoney = Data_Pool.m_RechargePool.f_GetAllRechageMoney();
        int AllMoney = Data_Pool.m_RechargePool.f_GetAllRechageMoney_TsuFunc();
        string Desc = CommonTools.f_GetTransLanguage(2195);
        f_GetObject("Desc").GetComponent<UILabel>().text = string.Format(Desc, data.m_FirstRechargeDT.iCondition);
        Desc = CommonTools.f_GetTransLanguage(2196);
        f_GetObject("Desc2").GetComponent<UILabel>().text = string.Format(Desc, AllMoney, AllMoney, data.m_FirstRechargeDT.iCondition);
        f_GetObject("Get").SetActive(NowSelect.mGetTimes < 1);
        UISprite Getbox = f_GetObject("Get").GetComponent<UISprite>();
        UISprite GetLabel = f_GetObject("Get").transform.Find("Sprite").GetComponent<UISprite>();
        UITool.f_SetSpriteGray(Getbox, AllMoney < data.m_FirstRechargeDT.iCondition);
        UITool.f_SetSpriteGray(GetLabel, AllMoney < data.m_FirstRechargeDT.iCondition);

    }

    private void UpdateBtn()
    {
        Transform BtnArr = f_GetObject("Btn").transform;
        string desc = CommonTools.f_GetTransLanguage(2197);
		int num = 2273;
        FirstRechargePoolDT data;
        for (int i = 0; i < BtnArr.childCount; i++)
        {
            data = Data_Pool.m_FirstRechargePool.f_GetForId((long)i + 1) as FirstRechargePoolDT;
            // BtnArr.GetChild(i).Find("Label").GetComponent<UILabel>().text = string.Format(desc, data.m_FirstRechargeDT.iCondition);
			BtnArr.GetChild(i).Find("Label").GetComponent<UILabel>().text = "" + CommonTools.f_GetTransLanguage(num + i).Replace(" ", "\n");
        }
    }

    private void SpineEnd(TrackEntry tTrackEntry)
    {
        //if (ani.AnimationName != "shouchong_loop")
        //{
        //    ani.state.SetAnimation(0, "shouchong_loop", true);
        //    //ani.loop = true;
        //    //ani.AnimationName = ;
        //}

    }

    private void OnCloseThis(GameObject go, object obj1, object obj2)
    {
        if (Magic != null)
        {
            UITool.f_DestoryStatelObject(Magic);
            Magic = null;
            ani = null;
        }
        f_GetObject("UI").SetActive(false);
        ccTimeEvent.GetInstance().f_UnRegEvent(Time_OpenUI);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.FirstRechargePageNew, UIMessageDef.UI_CLOSE);
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.FirstRechargePage, UIMessageDef.UI_CLOSE);
    }

    private void onClickBtn(GameObject go, object obj1, object obj2)
    {
        int index = (int)obj1;
		if(index == 0)
		{
			InitRecruitModel(f_GetObject("Special"), RoleList.iParam1, true, new Vector3(-355, -260, 0), index);
		}
		if(index == 1)
		{
			InitRecruitModel(f_GetObject("Special"), RoleList.iParam2, true, new Vector3(-355, -260, 0), index);
		}
		if(index == 2)
		{
			InitRecruitModel(f_GetObject("Special"), RoleList.iParam3, true, new Vector3(-355, -260, 0), index);
		}
        UpdateUI(index);
    }


    private void OnClickItem(GameObject go, object obj1, object obj2)
    {
        ResourceCommonDT commonData = (ResourceCommonDT)obj1;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, commonData);
    }

    private void OnClickGet(GameObject go, object obj1, object obj2)
    {
        int AllMoney = Data_Pool.m_RechargePool.f_GetAllRechageMoney_TsuFunc();
        if (NowSelect.mGetTimes > 0 || AllMoney < NowSelect.m_FirstRechargeDT.iCondition)
        {
            return;
        }
        RequestGetCallback.m_ccCallbackSuc = GetSuc;
        RequestGetCallback.m_ccCallbackFail = GetFill;
        Data_Pool.m_FirstRechargePool.f_GetAward((int)NowSelect.iId, RequestGetCallback);
    }

    private void GetSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
		AwardState = false;
        UpdateBtnState_btn("GetBtn", (int)NowSelect.iId, 0);
        UpdateBtnState_btn("GetBtn1", (int)NowSelect.iId, 1);
        UpdateBtnState_btn("GetBtn2", (int)NowSelect.iId, 2);
        if (nowSelectedDayId == 0)
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
                new object[] { CommonTools.f_GetListAwardPoolDT(NowSelect.m_FirstRechargeDT.szAward) });
        if (nowSelectedDayId == 1)
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
                new object[] { CommonTools.f_GetListAwardPoolDT(NowSelect.m_FirstRechargeDT.szAward1) });
        if (nowSelectedDayId == 2)
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
                new object[] { CommonTools.f_GetListAwardPoolDT(NowSelect.m_FirstRechargeDT.szAward2) });
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
        //        new object[] { CommonTools.f_GetListAwardPoolDT(NowSelect.m_FirstRechargeDT.szAward) });
        //UpdateUI((int)NowSelect.iId);

        
    }
    private void GetFill(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1382) + CommonTools.f_GetTransLanguage((int)obj));
    }

    private void GoToPay(GameObject go, object obj1, object obj2)
    {
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShowVip, UIMessageDef.UI_OPEN, ShowVip.EM_PageIndex.Recharge);

    }

    //TsuCode- FirstRechargeNew - NapDau

    private void OnClickGet_btn(GameObject go, object obj1, object obj2)
    {
        //UITool.Ui_Trip(NowSelectId.ToString());
        MessageBox.ASSERT("TSULOG CHECK NowSlected " + NowSelectId + "-" + obj1);
        MessageBox.ASSERT("TSULOG CHECK NowSlected  day " + Data_Pool.m_FirstRechargePool.getDay(NowSelectId));
        nowSelectedDayId = (int)obj1;

        long timeDataa = Data_Pool.m_FirstRechargePool.getDay(NowSelectId) + nowSelectedDayId*86400;
        long dayData = ccMath.time_t2DateTime(timeDataa).Year * 10000 + ccMath.time_t2DateTime(timeDataa).Month * 100 + ccMath.time_t2DateTime(timeDataa).Day;

        
        int AllMoney = Data_Pool.m_RechargePool.f_GetAllRechageMoney_TsuFunc();
        //if (NowSelect.mGetTimes > 0 || AllMoney < NowSelect.m_FirstRechargeDT.iCondition)
        //{
        //    return;
        //}

        long nowTime = GameSocket.GetInstance().f_GetServerTime();
        int nowDay = ccMath.time_t2DateTime(nowTime).Year * 10000 + ccMath.time_t2DateTime(nowTime).Month * 100 + ccMath.time_t2DateTime(nowTime).Day;
        MessageBox.ASSERT("TSULOG CHECK NowSlected  DAY DAY DAY " + nowDay + "-" + dayData);
        //if (AllMoney < NowSelect.m_FirstRechargeDT.iCondition || (nowDay < Data_Pool.m_FirstRechargePool.getDay(NowSelectId) + (int)obj1))
        if (AllMoney < NowSelect.m_FirstRechargeDT.iCondition || (nowDay < dayData))
        {
UITool.Ui_Trip("Không đủ điều kiện nhận");
            return;
        }
        int ob = (int)obj1;
        RequestGetCallback.m_ccCallbackSuc = GetSuc;
        RequestGetCallback.m_ccCallbackFail = GetFill;
        Data_Pool.m_FirstRechargePool.f_GetAward_Tsu((int)NowSelect.iId, (int)obj1, RequestGetCallback);
      
        //UpdateUI(NowSelectId);
        //UpdateBtnState_btn(go.name, NowSelectId+1, ob);
    }
	
	private void InitRecruitModel(GameObject parent, int cardId, bool dir, Vector3 postion, int index)
    {
		if(index == 0)
		{
			if(parent.transform.Find("Model1") != null)
				parent.transform.Find("Model1").gameObject.SetActive(false);
			if(parent.transform.Find("Model2") != null)
				parent.transform.Find("Model2").gameObject.SetActive(false);
			if (parent.transform.Find("Model") != null)
			{
				parent.transform.Find("Model").gameObject.SetActive(true);
				return;
			}
			UITool.f_GetStatelObject(cardId, parent.transform, dir ? Vector3.zero : new Vector3(0,180,0), postion, 23, "Model", 90);
		}
		if(index == 1)
		{
			if(parent.transform.Find("Model") != null)
				parent.transform.Find("Model").gameObject.SetActive(false);
			if(parent.transform.Find("Model2") != null)
				parent.transform.Find("Model2").gameObject.SetActive(false);
			if (parent.transform.Find("Model1") != null)
			{
				parent.transform.Find("Model1").gameObject.SetActive(true);
				return;
			}
			UITool.f_GetStatelObject(cardId, parent.transform, dir ? Vector3.zero : new Vector3(0,180,0), postion, 23, "Model1", 90);
		}
		if(index == 2)
		{
			if(parent.transform.Find("Model1") != null)
				parent.transform.Find("Model1").gameObject.SetActive(false);
			if(parent.transform.Find("Model") != null)
				parent.transform.Find("Model").gameObject.SetActive(false);
			if (parent.transform.Find("Model2") != null)
			{
				parent.transform.Find("Model2").gameObject.SetActive(true);
				return;
			}
			UITool.f_GetStatelObject(cardId, parent.transform, dir ? Vector3.zero : new Vector3(0,180,0), postion, 23, "Model2", 90);
        }
    }

    private void UpdateBtnState_btn(string btnName, int index, int dayId)
    {
        int receivedIndex = 0;
        int ind = 0;
        if (index == null)
        {
            ind = 0;
        }
        else
        {
            ind = (int)index;
        }
        if (ind >= 3)
        {
            ind = 2;
        }
     

        int AllMoney = Data_Pool.m_RechargePool.f_GetAllRechageMoney_TsuFunc();

        long nowTime = GameSocket.GetInstance().f_GetServerTime();
        long nowDay = ccMath.time_t2DateTime(nowTime).Year * 10000 + ccMath.time_t2DateTime(nowTime).Month * 100 + ccMath.time_t2DateTime(nowTime).Day;
  
        FirstRechargePoolDT data = Data_Pool.m_FirstRechargePool.f_GetForId((long)index) as FirstRechargePoolDT;
        long timeDataa = Data_Pool.m_FirstRechargePool.getDay(index - 1) + dayId*86400;
    
        long dayData = ccMath.time_t2DateTime(timeDataa).Year * 10000 + ccMath.time_t2DateTime(timeDataa).Month * 100 + ccMath.time_t2DateTime(timeDataa).Day;

        switch (dayId + 1)
        {
            case 1:
                //if (data.received_1 == 1) f_GetObject(btnName).SetActive(false);
                receivedIndex = data.received_1;
                break;
            case 2:
                receivedIndex = data.received_2;
                //if (data.received_2 == 1) f_GetObject(btnName).SetActive(false);
                break;
            case 3:
                receivedIndex = data.received_3;
                //if (data.received_3 == 1) f_GetObject(btnName).SetActive(false);
                break;
        }


        //f_GetObject(btnName).SetActive(AllMoney >= data.m_FirstRechargeDT.iCondition && nowDay >= Data_Pool.m_FirstRechargePool.getDay(index-1) + dayId);
        //f_GetObject(btnName).SetActive(dayId!=data.received);
        UISprite Getbox = f_GetObject(btnName).GetComponent<UISprite>();
        //UISprite GetLabel = f_GetObject(btnName).transform.Find("Sprite").GetComponent<UISprite>();
        f_GetObject(btnName).SetActive(true);
        //UITool.f_SetSpriteGray(Getbox, AllMoney < data.m_FirstRechargeDT.iCondition || nowDay != Data_Pool.m_FirstRechargePool.getDay(index - 1) + dayId || dayId + 1 <= data.received);
        //UITool.f_SetSpriteGray(GetLabel, AllMoney < data.m_FirstRechargeDT.iCondition || nowDay != Data_Pool.m_FirstRechargePool.getDay(index - 1) + dayId || dayId + 1 <= data.received);
        UITool.f_SetSpriteGray(Getbox, AllMoney < data.m_FirstRechargeDT.iCondition || (nowDay < dayData) || receivedIndex==1);

        if (!(AllMoney < data.m_FirstRechargeDT.iCondition || (nowDay < dayData) || receivedIndex == 1))
		{
			AwardState = true;
			Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.FirstRecharge);
            Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.FirstRecharge);
		}
		AwardState = !(AllMoney < data.m_FirstRechargeDT.iCondition || (nowDay < dayData) || receivedIndex == 1);
        //UITool.f_SetSpriteGray(GetLabel, AllMoney < data.m_FirstRechargeDT.iCondition || nowDay != dayData || dayId + 1 <= data.received);
        BoxCollider box = f_GetObject(btnName).GetComponent<BoxCollider>();
        //box.enabled = !(AllMoney < data.m_FirstRechargeDT.iCondition || nowDay != Data_Pool.m_FirstRechargePool.getDay(index - 1) + dayId || dayId + 1 <= data.received);
        box.enabled = !(AllMoney < data.m_FirstRechargeDT.iCondition || (nowDay < dayData) || receivedIndex == 1);
        if (receivedIndex==1)
            f_GetObject(btnName).SetActive(false);


        // if (AllMoney < data.m_FirstRechargeDT.iCondition || nowDay != dayData || dayId + 1 <= data.received)
        // ccUIManage.GetInstance().f_SendMsg(UINameConst.MainMenu, UIMessageDef.UI_SETFRREDDOT, false);
        //UITool.f_SetSpriteGray(Getbox, true); //true isGray
        //UITool.f_SetSpriteGray(GetLabel, true);
    }

    //------------------------------------------
}

