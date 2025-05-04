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
public class OnlineVoteAppPage : UIFramwork
{
    private SocketCallbackDT RequestGetCallback = new SocketCallbackDT();//领取回调
    private bool initUI;
    EventTimeDT eventTimeDT;
    VoteAppDT voteAppDT;
    EventTimePoolDT tPoolDataDT;
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (initUI)
        {
            EventTimeDT newEvent = (EventTimeDT)e;

            if (newEvent.iId != eventTimeDT.iId)
            {
                initUI = false;
            }
        }

        eventTimeDT = (EventTimeDT)e;
        Debug.Log(eventTimeDT.iId);
        UpdateContent();

    }
    protected override void f_Create()
    {
        _InitReference();
        base.f_Create();
    }
    private void _InitReference()
    {
        AddGOReference("Panel/Anchor-Center/BlackBg");
        AddGOReference("Panel/Anchor-Center/UI/GoodsParent");
        AddGOReference("Panel/Anchor-Center/UI/GoodsParent/Item");
        AddGOReference("Panel/Anchor-Center/UI/GetBtn");
        AddGOReference("Panel/Anchor-Center/UI/VoteBtn");
        AddGOReference("Panel/Anchor-Center/UI/BtnClose");

    }
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {

    }
    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
    }
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BlackBg", OnCloseThis);
        f_RegClickEvent("BtnClose", OnCloseThis);
        f_RegClickEvent("GetBtn", OnClickGet_btn);
        f_RegClickEvent("VoteBtn", OnClickVote_btn);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_VOTE_APP_AWARD, ItemChange, this);

        RequestGetCallback.m_ccCallbackSuc = OnGetSucCallback;
        RequestGetCallback.m_ccCallbackFail = OnGetFailCallback;

    }

    private void OnCloseThis(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.OnlineVoteAppPage, UIMessageDef.UI_CLOSE);
        f_DestoryView();
    }

    private void UpdateContent()
    {

        voteAppDT = (VoteAppDT)glo_Main.GetInstance().m_SC_Pool.m_VoteAppSC.f_GetSCByEventTimeId(eventTimeDT.iId)[0];
        if(voteAppDT == null)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.OnlineVoteAppPage, UIMessageDef.UI_CLOSE);
Debug.Log("don't set gift id for this event time");
            return;
        }
        List<ResourceCommonDT> listCommonDT = CommonTools.f_GetListCommonDT(voteAppDT.szAward);
        if (!initUI)
        {
            initUI = true;
        
            GridUtil.f_SetGridView<ResourceCommonDT>(f_GetObject("GoodsParent"), f_GetObject("Item"), listCommonDT, UpdateItem);

        }

        int key = InitKey(eventTimeDT.iId, voteAppDT.iId);
        tPoolDataDT = (EventTimePoolDT)Data_Pool.m_EventTimePool.f_GetForId(key);

        f_GetObject("GoodsParent").GetComponent<UIGrid>().Reposition();
        if (tPoolDataDT == null) return;
        UpdateBtnGet();
    }

    private int InitKey(int idA, int idB)
    {
        string key = idA + "" + idB;
        return int.Parse(key);
    }

    private void UpdateItem(GameObject go, ResourceCommonDT Data)
    {
        Transform tran = go.transform;

        UI2DSprite Icon = tran.Find("Icon").GetComponent<UI2DSprite>();


        Icon.sprite2D = UITool.f_GetIconSprite(Data.mIcon);

        f_RegClickEvent(Icon.gameObject, OnClickItem, Data);
    }


    private void OnClickItem(GameObject go, object obj1, object obj2)
    {
        ResourceCommonDT commonData = (ResourceCommonDT)obj1;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, commonData);
    }
    private void UpdateBtnGet()
    {
        f_GetObject("GetBtn").gameObject.SetActive(tPoolDataDT.isFinsh);
        UITool.f_SetSpriteGray(f_GetObject("GetBtn"), tPoolDataDT.idata1 >0);

        f_GetObject("VoteBtn").gameObject.SetActive(!tPoolDataDT.isFinsh);

    }

    private void OnClickGet_btn(GameObject go, object obj1, object obj2)
    {
        Data_Pool.m_EventTimePool.f_GetAward(eventTimeDT.iId, voteAppDT.iId, RequestGetCallback);
        UpdateBtnGet();
    }

    private void OnClickVote_btn(GameObject go, object obj1, object obj2)
    {
        //if (Application.platform == RuntimePlatform.IPhonePlayer)
        //{
        //    Application.OpenURL(voteAppDT.szIos);
        //}
        //else
        //{
        //    Application.OpenURL(voteAppDT.szAndroid);
        //}

        glo_Main.GetInstance().m_SDKCmponent.f_OpenStore(); //TsuCode - open store - vote app

        tPoolDataDT.isFinsh = true;
        UpdateBtnGet();
    }

    //查询成功回调
    private void OnSucCallback(object obj)
    {
        ccUIHoldPool.GetInstance().f_Hold(this);
    }



    private void ItemChange(object value)
    {
        UpdateContent();
    }

    private void OnGetSucCallback(object obj)
    {      
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { CommonTools.f_GetListAwardPoolDT(voteAppDT.szAward) });
    }

    private void OnGetFailCallback(object obj)
    {
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1274));
    }

    public void f_DestoryView()
    {
        

    }

    public bool CheckDoneEvent(int eventTimeId)
    {

        return false;
    }

}

