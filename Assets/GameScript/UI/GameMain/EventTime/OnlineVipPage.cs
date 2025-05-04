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
public class OnlineVipPage : UIFramwork
{
    private SocketCallbackDT RequestGetCallback = new SocketCallbackDT();//领取回调
    private bool initUI;
    protected override void UI_OPEN(object e)
    {
        Data_Pool.m_OnlineAwardPool.f_QueryInfo(null);
        base.UI_OPEN(e);
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
        AddGOReference("Panel/Anchor-Center/UI/Special");
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
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_ONLINE_VIP_AWARD, ItemChange, this);

        RequestGetCallback.m_ccCallbackSuc = OnGetSucCallback;
        RequestGetCallback.m_ccCallbackFail = OnGetFailCallback;

    }

    private void OnCloseThis(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.OnlineVipPage, UIMessageDef.UI_CLOSE);
        f_DestoryView();



    }

    private int timeSec;
    private List<OnlineVipAwardDT> onlineVipAwardDTs = new List<OnlineVipAwardDT>();
    GameObject Magic;
    SkeletonAnimation ani;
    GameParamDT RoleList;
    private void UpdateContent()
    {
        onlineVipAwardDTs.Clear();
        for (int i = 0; i < glo_Main.GetInstance().m_SC_Pool.m_OnlineVipAwardSC.f_GetAll().Count; i++)
        {
            OnlineVipAwardDT onlineVipAwardDT = glo_Main.GetInstance().m_SC_Pool.m_OnlineVipAwardSC.f_GetAll()[i] as OnlineVipAwardDT;
            onlineVipAwardDTs.Add(onlineVipAwardDT);
        }
        if (!initUI)
        {
            initUI = true;
            GridUtil.f_SetGridView<OnlineVipAwardDT>(f_GetObject("GoodsParent"), f_GetObject("Item"), onlineVipAwardDTs, UpdateItem);
            UITool.f_GetStatelObject(1402, f_GetObject("Special").transform, new Vector3(0, 180, 0), new Vector3(-100, 0, 0), 23, "Model", 100);
        }
       
        f_GetObject("GoodsParent").GetComponent<UIGrid>().Reposition();

        timeSec = Data_Pool.m_OnlineAwardPool.m_timeSecondToday;

        UITool.f_SetSpriteGray(f_GetObject("GetBtn"), true);
        TimerControl(true);

        //UITool.f_CreateMagicById(12101, ref Magic, , 20, "Stand", SpineEnd);
        //ani = Magic.GetComponent<SkeletonAnimation>();

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


    private void UpdateItem(GameObject go, OnlineVipAwardDT Data)
    {
        go.AddComponent<OnlineVipItem>().SetData(Data);        
    }

    private OnlineVipAwardDT OnlineVipAwardDT;
    private void OnClickGet_btn(GameObject go, object obj1, object obj2)
    {
        Data_Pool.m_OnlineVipAwardPool.f_GetAward(OnlineVipAwardDT.iId, RequestGetCallback);

    }

    //查询成功回调
    private void OnSucCallback(object obj)
    {
        ccUIHoldPool.GetInstance().f_Hold(this);
    }

    private void UpdateBtnGet(bool check)
    {
        UITool.f_SetSpriteGray(f_GetObject("GetBtn"), check);
    }

    private void TimerControl(bool isStart)
    {
        CancelInvoke("ReduceTime");
        if (isStart)
        {
            InvokeRepeating("ReduceTime", 0f, 1f);
        }
    }

    private void ReduceTime()
    {
        timeSec++;
        for (int i = 0; i < onlineVipAwardDTs.Count; i++)
        {
            int time = onlineVipAwardDTs[i].iTime * 60;
            bool check = Data_Pool.m_OnlineVipAwardPool.CheckGetAward(onlineVipAwardDTs[i].iId); 

            if(!check && time - timeSec <= 0)
            {
                OnlineVipAwardDT = onlineVipAwardDTs[i];
                UpdateBtnGet(false);
                TimerControl(false);
                break;
            }
        }
    }

    private void ItemChange(object value)
    {
        UITool.f_SetSpriteGray(f_GetObject("GetBtn"), true);
        ReduceTime();
    }

    private void OnGetSucCallback(object obj)
    {
        List<AwardPoolDT> awardList = new List<AwardPoolDT>();
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { CommonTools.f_GetListAwardPoolDT(OnlineVipAwardDT.szAward) });
    }

    private void OnGetFailCallback(object obj)
    {
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1274));
    }

    public void f_DestoryView()
    {
        //TimerControl(false);
        foreach (OnlineVipItem item in transform.GetComponentsInChildren<OnlineVipItem>())
        {
            item.f_DestoryView();
        }

    }

}

