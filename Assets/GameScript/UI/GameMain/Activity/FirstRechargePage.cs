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
public class FirstRechargePage : UIFramwork
{
    private SocketCallbackDT QueryCallback = new SocketCallbackDT();//查询回调
    private SocketCallbackDT RequestGetCallback = new SocketCallbackDT();//领取回调
    GameObject Magic;
    SkeletonAnimation ani;
    int Time_OpenUI;
    private FirstRechargePoolDT NowSelect;
    private int NowSelectId;
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
        UITool.f_CreateMagicById(20017, ref Magic, f_GetObject("SpineParent").transform, 20, "shouchong", SpineEnd);
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
        f_RegClickEvent("Btn6", onClickBtn, 0);
        f_RegClickEvent("Btn68", onClickBtn, 1);
        f_RegClickEvent("Btn198", onClickBtn, 2);
        f_RegClickEvent("Get", OnClickGet);
        f_RegClickEvent("GoPay",GoToPay);
    }

    private void QuerySuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        Time_OpenUI = ccTimeEvent.GetInstance().f_RegEvent(0.6f, false, null, UpdateUI);
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
        if (index>=3) {
            index = 2;
        }
        NowSelectId = index;
        NowSelect = Data_Pool.m_FirstRechargePool.f_GetForId((long)index + 1) as FirstRechargePoolDT; ;
        SetBtnState(index);
        UpdateGoods(index + 1);
        UpdateDesc(index + 1);
        UpdateBtn();

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
        GridUtil.f_SetGridView<ResourceCommonDT>(f_GetObject("GoodsParent"), f_GetObject("Item"), listCommonDT, UpdateItem);
        f_GetObject("GoodsParent").transform.GetComponent<UIGrid>().Reposition();
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
        int AllMoney = Data_Pool.m_RechargePool.f_GetAllRechageMoney();
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
        FirstRechargePoolDT data;
        for (int i = 0; i < BtnArr.childCount; i++)
        {
            data = Data_Pool.m_FirstRechargePool.f_GetForId((long)i + 1) as FirstRechargePoolDT;
            BtnArr.GetChild(i).Find("Label").GetComponent<UILabel>().text = string.Format(desc, data.m_FirstRechargeDT.iCondition);
        }
    }

    private void SpineEnd(TrackEntry tTrackEntry)
    {
        if (ani.AnimationName != "shouchong_loop")
        {
            ani.state.SetAnimation(0, "shouchong_loop", true);
            //ani.loop = true;
            //ani.AnimationName = ;
        }

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
        ccUIManage.GetInstance().f_SendMsg(UINameConst.FirstRechargePage, UIMessageDef.UI_CLOSE);
    }

    private void onClickBtn(GameObject go, object obj1, object obj2)
    {
        int index = (int)obj1;

        UpdateUI(index);
    }


    private void OnClickItem(GameObject go, object obj1, object obj2)
    {
        ResourceCommonDT commonData = (ResourceCommonDT)obj1;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, commonData);
    }

    private void OnClickGet(GameObject go, object obj1, object obj2)
    {
        int AllMoney = Data_Pool.m_RechargePool.f_GetAllRechageMoney();
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
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
                new object[] { CommonTools.f_GetListAwardPoolDT(NowSelect.m_FirstRechargeDT.szAward) });
        UpdateUI((int)NowSelect.iId);
    }
    private void GetFill(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1382) + CommonTools.f_GetTransLanguage((int)obj));
    }

    private void GoToPay(GameObject go, object obj1, object obj2) {
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShowVip,UIMessageDef.UI_OPEN, ShowVip.EM_PageIndex.Recharge);

    }
}

