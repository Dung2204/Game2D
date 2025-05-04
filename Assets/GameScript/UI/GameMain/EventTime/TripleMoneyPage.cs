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
public class TripleMoneyPage : UIFramwork
{
    private SocketCallbackDT RequestGetCallback = new SocketCallbackDT();//领取回调
    private bool initUI;
    EventTimeDT eventTimeDT;
    List<NBaseSCDT> TripleMoneyDTs;
    EventTimePoolDT tPoolDataDT;
    private UILabel ActivityEndTime;   //活
    private bool mFirstPage = true;
    private UIScrollView mScrollView;
    private GameObject mAwardItemParent;
    private GameObject mAwardItem;

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

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }
    protected override void InitGUI()
    {
        base.InitGUI();
        ActivityEndTime = f_GetObject("ActivityEndTime").GetComponent<UILabel>();
    }

    private void f_OnMomnetEnds()
    {
        Vector3 constraint = mScrollView.panel.CalculateConstrainOffset(mScrollView.bounds.min, mScrollView.bounds.min);
        if (constraint.y <= 0)
        {
            mFirstPage = false;
            Data_Pool.m_RankingPowerAwardPool.f_RankList();
        }
    }

    protected override void f_Create()
    {
        _InitReference();
        base.f_Create();
    }
    private void _InitReference()
    {
        AddGOReference("Panel/Anchor-Center/BlackBg");
        AddGOReference("Panel/Anchor-Center/UI/BtnClose");
        AddGOReference("Panel/Anchor-Center/UI/Item");

        AddGOReference("Panel/Anchor-Center/UI/DayNum/ActivityEndTime");

        AddGOReference("Panel/Anchor-Center/UI/Type1Panel");
        AddGOReference("Panel/Anchor-Center/UI/Type2Panel");

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

        //glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_RANKING_POWER_LIST, OnGetSucCallback, this);

        RequestGetCallback.m_ccCallbackSuc = OnGetSucCallback;
        RequestGetCallback.m_ccCallbackFail = OnGetFailCallback;

    }

    private void OnCloseThis(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TripleMoneyPage, UIMessageDef.UI_CLOSE);
        f_DestoryView();
        if(eventTimeDT.iHold > 0)
        {
            ccUIHoldPool.GetInstance().f_UnHold();
        }
    }

    private void BtnPayType1(GameObject go, object obj1, object obj2)
    {
        Recharge((int) obj1);
    }

    void Recharge(int id)
    {
        TripleMoneyDT tripleMoneyDT = (TripleMoneyDT)glo_Main.GetInstance().m_SC_Pool.m_TripleMoneySC.f_GetSC(id);

        int uCoin = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Coin);

        EventTimePoolDT eventTimePoolDT = (EventTimePoolDT)Data_Pool.m_EventTimePool.f_EventTimePoolDT(tripleMoneyDT.iEventTime, tripleMoneyDT.iId);
        if(eventTimePoolDT.idata1 >= tripleMoneyDT.iMaxNum)
        {
UITool.Ui_Trip("Đã đạt đến giới hạn mua!");
            return;
        }


        if (uCoin >= tripleMoneyDT.iPayNum)
        {
            MessageBox.ASSERT("TsuLog: Co the mua " + uCoin + " >= " + tripleMoneyDT.iPayNum);
            //SocketCallbackDT whitelistCallbackDt = new SocketCallbackDT();
            //whitelistCallbackDt.m_ccCallbackSuc = f_OnRechargeCoin;
            //whitelistCallbackDt.m_ccCallbackFail = f_OnRechargeCoin;
            //Data_Pool.m_RechargePool.RechargeTripleMoney(tripleMoneyDT.iEventTime, tripleMoneyDT.iId, whitelistCallbackDt);
 

PopupMenuParams tParam = new PopupMenuParams("Xác Nhận", string.Format("Dùng {0} Kim Phiếu \n để mua {1}", tripleMoneyDT.iPayNum, tripleMoneyDT.szPayDesc), "Đồng ý", f_ConfirmRecharge, "Hủy bỏ" , null, new int[] { tripleMoneyDT.iEventTime, tripleMoneyDT.iId });
            ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParam);
            MessageBox.ASSERT("TsuLog: Co the mua " + uCoin + " >= " + tripleMoneyDT.iPayNum);
        }
        else
        {
            MessageBox.ASSERT("TsuLog: Khong du kiem phieu de giao dich " + uCoin + " < " + tripleMoneyDT.iPayNum);
UITool.Ui_Trip("Không đủ Kim Phiếu!");
            //glo_Main.GetInstance().m_SDKCmponent.f_Pccaccy(0, "", 0, 0, tCPOrderId, "", 0, "", "");
            glo_Main.GetInstance().m_SDKCmponent.f_ShowSDKPay();
        }

        //----------------------------------
    }

    private void f_ConfirmRecharge(object value)
    {
        int[] param = (int[])value;

        int iEventTime = param[0];
        int orderId = param[1];

        idtriple = orderId;
        SocketCallbackDT whitelistCallbackDt = new SocketCallbackDT();
        whitelistCallbackDt.m_ccCallbackSuc = f_OnRechargeCoin;
        whitelistCallbackDt.m_ccCallbackFail = f_OnRechargeCoin;
        Data_Pool.m_RechargePool.RechargeTripleMoney(iEventTime, orderId, whitelistCallbackDt);
    }

    private int idtriple; 
    private void f_OnRechargeCoin(object result)
    {
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
UITool.Ui_Trip("Giao dịch thành công");
            TripleMoneyDT tripleMoneyDT = (TripleMoneyDT)glo_Main.GetInstance().m_SC_Pool.m_TripleMoneySC.f_GetSC(idtriple);
            List<AwardPoolDT> tAwardPoolList = new List<AwardPoolDT>();
            AwardPoolDT taward = new AwardPoolDT();
            taward.f_UpdateByInfo((byte)EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_Sycee, tripleMoneyDT.iFirstPayNum + tripleMoneyDT.iSecondPayNum + tripleMoneyDT.iThirdPayNum);
            tAwardPoolList.Add(taward);

            if (tAwardPoolList.Count > 0)
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN, new object[] { tAwardPoolList });
                tAwardPoolList.Clear();
            }
            f_UpdateInfo((TripleMoneyDT)TripleMoneyDTs[0], f_GetObject("Type1Panel"));
            f_UpdateInfo((TripleMoneyDT)TripleMoneyDTs[1], f_GetObject("Type2Panel"));
        }
        else
        {
UITool.Ui_Trip("Giao dịch thất bại");
            MessageBox.ASSERT("Recharge whitelist failed,code:" + (int)result);
        }
    }

    private void f_UpdateInfo(TripleMoneyDT tripleMoneyDT, GameObject Parent)
    {

        Transform Item = f_GetObject("Item").transform;


        List<ResourceCommonDT> listCommonDT = new List<ResourceCommonDT>();

        ResourceCommonDT commonData1 = new ResourceCommonDT();
        commonData1.f_UpdateInfo((byte)EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_Sycee, tripleMoneyDT.iFirstPayNum);
commonData1.f_UpdateName("Get Basic");
        listCommonDT.Add(commonData1);
        ResourceCommonDT commonData2 = new ResourceCommonDT();
        commonData2.f_UpdateInfo((byte)EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_Sycee, tripleMoneyDT.iSecondPayNum+ tripleMoneyDT.iThirdPayNum);
commonData2.f_UpdateName("Get Offer");
        listCommonDT.Add(commonData2);
        ResourceCommonDT commonData3 = new ResourceCommonDT();
        commonData3.f_UpdateInfo((byte)EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_Sycee, tripleMoneyDT.iFirstPayNum + tripleMoneyDT.iSecondPayNum + tripleMoneyDT.iThirdPayNum);
commonData3.f_UpdateName("Total Receive");
        listCommonDT.Add(commonData3);

        UILabel Name = Parent.transform.Find("Name").GetComponent<UILabel>();
        Name.text = tripleMoneyDT.szPayDesc;

        UILabel Num = Parent.transform.Find("PayNum").GetComponent<UILabel>();
        EventTimePoolDT eventTimePoolDT = (EventTimePoolDT)Data_Pool.m_EventTimePool.f_EventTimePoolDT(tripleMoneyDT.iEventTime, tripleMoneyDT.iId);
Num.text = string.Format("Giới hạn : {0}/{1} lần", tripleMoneyDT.iMaxNum - eventTimePoolDT.idata1, tripleMoneyDT.iMaxNum);

        f_RegClickEvent(Parent.transform.Find("BtnPayType").gameObject, BtnPayType1, tripleMoneyDT.iId);
        UILabel Namebtn = Parent.transform.Find("BtnPayType/Label").GetComponent<UILabel>();
Namebtn.text = string.Format("{0} Kim Phiếu", tripleMoneyDT.iPayNum);

        GridUtil.f_SetGridView<ResourceCommonDT>(Parent.transform.Find("Grid").gameObject, Item.gameObject, listCommonDT, UpdateItem);
    }

    private void f_OnAwardItemClick(Transform tf, BasePoolDT<long> dt)
    { 
    
    }
    int iTime1;
    DateTime tDate1;
    private void UpdateContent()
    {
        TripleMoneyDTs = glo_Main.GetInstance().m_SC_Pool.m_TripleMoneySC.f_GetSCByEventTimeId(eventTimeDT.iId);
        if(TripleMoneyDTs.Count <= 0)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.TripleMoneyPage, UIMessageDef.UI_CLOSE);
Debug.Log("don't set gift id for this event time");
            return;
        }

        if (TripleMoneyDTs.Count < 2)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.TripleMoneyPage, UIMessageDef.UI_CLOSE);
Debug.Log("this event set 2 id");
            return;
        }

        int second_chenhlech = Data_Pool.m_UserData.m_CreateTime - Data_Pool.m_SevenActivityTaskPool.OpenSeverTime;
        int second_du2 = second_chenhlech % 86400;

        iTime1 =  Data_Pool.m_UserData.m_CreateTime - second_du2 + eventTimeDT.iEndTime * 86400 - GameSocket.GetInstance().f_GetServerTime();

        tDate1 = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Unspecified).AddSeconds(iTime1);

        if(iTime1 > 0)
        {
            ActivityEndTime.text = string.Format(CommonTools.f_GetTransLanguage(2112), tDate1.Day - 1, tDate1.Hour, tDate1.Minute) + tDate1.Second + CommonTools.f_GetTransLanguage(2114);
        }
        else
        {
            ActivityEndTime.text = string.Format(CommonTools.f_GetTransLanguage(2115));
        }

        f_UpdateInfo((TripleMoneyDT)TripleMoneyDTs[0], f_GetObject("Type1Panel"));
        f_UpdateInfo((TripleMoneyDT)TripleMoneyDTs[1], f_GetObject("Type2Panel"));

        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_EventTimePool.f_TripleMoneyPool(eventTimeDT.iId, RequestGetCallback);
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
        UILabel Num = tran.Find("Num").GetComponent<UILabel>();
        UILabel Name = tran.Find("Name").GetComponent<UILabel>();
        UISprite Case = tran.Find("Case").GetComponent<UISprite>();

        Icon.sprite2D = UITool.f_GetIconSprite(Data.mIcon);
        Num.text = Data.mResourceNum.ToString();
        Name.text = Data.mName.ToString();
        Case.spriteName = UITool.f_GetImporentCase(Data.mImportant);
        //CreareEffect(tran.Find("Effect").gameObject, Data.mImportant);

        f_RegClickEvent(Icon.gameObject, OnClickItem, Data);
    }

    private void CreareEffect(GameObject EquipObj, int Imporent)
    {
        string EffectName = "";
        switch ((EM_Important)Imporent)
        {
            case EM_Important.White:
            case EM_Important.Green:
                EffectName = UIEffectName.biankuangliuguang_green;
                break;
            case EM_Important.Blue:
                EffectName = UIEffectName.biankuangliuguang_bue;
                break;
            case EM_Important.Purple:
                EffectName = UIEffectName.biankuangliuguang_purple;
                break;
            case EM_Important.Oragen:
                EffectName = UIEffectName.biankuangliuguang_oragen;
                break;
            case EM_Important.Red:
                EffectName = UIEffectName.biankuangliuguang_red;
                break;
            case EM_Important.Gold:
                break;
        }

        for (int j = EquipObj.transform.childCount - 1; j >= 0; j--)//删除特效
            Destroy(EquipObj.transform.GetChild(j).gameObject);

        GameObject SetEquipEffect = UITool.f_CreateEffect_Old(EffectName, EquipObj.transform, Vector3.zero, 1f, 0, UIEffectName.UIEffectAddress1);
        SetEquipEffect.GetComponent<ParticleScaler>().TrailRenderSortingOrder = 1;
        SetEquipEffect.transform.parent.localScale = Vector3.one * 120;
        SetEquipEffect.transform.parent.localPosition = Vector3.zero;
        SetEquipEffect.transform.localPosition = Vector3.zero;
        SetEquipEffect.transform.localScale = Vector3.one;
    }
    private float updateTime = 0f;
    protected override void f_Update()
    {
        base.f_Update();
        updateTime += Time.deltaTime;
        if (updateTime > 1f)
        {
            updateTime = 0f;
            if(iTime1 > 0)
            {
                iTime1--;
                tDate1 = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Unspecified).AddSeconds(iTime1);
                ActivityEndTime.text = string.Format(CommonTools.f_GetTransLanguage(2112), tDate1.Day - 1, tDate1.Hour, tDate1.Minute) + tDate1.Second + CommonTools.f_GetTransLanguage(2114);
            }
            else
            {
                ActivityEndTime.text = string.Format(CommonTools.f_GetTransLanguage(2115));
            }
        }
    }


    private void OnClickItem(GameObject go, object obj1, object obj2)
    {
        ResourceCommonDT commonData = (ResourceCommonDT)obj1;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, commonData);
    }

    //查询成功回调
    private void OnSucCallback(object obj)
    {
        ccUIHoldPool.GetInstance().f_Hold(this);
    }

    private void OnGetSucCallback(object value)
    {
        //server đã gởi về pool

        UITool.f_OpenOrCloseWaitTip(false);

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

