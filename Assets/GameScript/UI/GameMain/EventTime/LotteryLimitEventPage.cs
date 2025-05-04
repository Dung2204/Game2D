using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotteryLimitEventPage : UIFramwork
{
    private SocketCallbackDT ReqeustBuyLotteryCallback = new SocketCallbackDT();
    private GameObject mRole;
    public GameObject mModelParent;
    UILabel mLbTime;
    List<EventTimePoolDT> m_List;
    private int m_SelectIndex = 0;
    private int timeLeft = 0;
    private bool _isPopupRateShow = false;

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        _isPopupRateShow = false;
        EventTimeDT pEventTimeDT = e as EventTimeDT;
        int iIdEventTime = pEventTimeDT.iId;
        m_List = new List<EventTimePoolDT>();
        //Data_Pool.m_EventTimePool.f_GetForId();
        List<NBaseSCDT> m_LotteryLimitEventDTs = glo_Main.GetInstance().m_SC_Pool.m_LotteryLimitEventSC.f_GetAll();
        for (int i = 0; i < m_LotteryLimitEventDTs.Count; i++)
        {
            LotteryLimitEventDT node = (LotteryLimitEventDT)m_LotteryLimitEventDTs[i];
            if (node.iEventTime == iIdEventTime) {
                EventTimePoolDT eventTimePoolDT = (EventTimePoolDT)Data_Pool.m_EventTimePool.f_EventTimePoolDT(iIdEventTime, node.iId);
                if(GameSocket.GetInstance().f_GetServerTime() >= eventTimePoolDT.idata1 && GameSocket.GetInstance().f_GetServerTime() < eventTimePoolDT.idata2)
                    m_List.Add(eventTimePoolDT);
            }
        }
        if(m_List.Count == 0) {
            //UITool
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LotteryLimitEventPage, UIMessageDef.UI_CLOSE);
            return;
        }
        UpdateContent();
    }
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();

    }
    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }
    protected override void InitGUI()
    {
        base.InitGUI();
        mLbTime = f_GetObject("LabelTimes").GetComponent<UILabel>();
        mModelParent = f_GetObject("ModelParent");
    }
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnRate", OnBtnRateClick);
        f_RegClickEvent("BtnReturn", OnBtnReturnClick);
        f_RegClickEvent("BtnBuyOneAdmiral", OnBtnOnceClick);
        f_RegClickEvent("BtnBuyTenAdmiral", OnBtnTenClick);
        ReqeustBuyLotteryCallback.m_ccCallbackSuc = OnRequestLotteryBuySucCallback;
        ReqeustBuyLotteryCallback.m_ccCallbackFail = OnRequestLotteryBuyFailCallback;

    } 
    private void InitMoneyUI()
    {
        List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
        listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Money);
        listMoneyType.Add(EM_MoneyType.eLimitAd);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, listMoneyType);
    }
    protected void UpdateContent()
    {
        InitMoneyUI();
        EventTimePoolDT eventTimePoolDT = m_List[m_SelectIndex];
        int lv = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        if (lv < eventTimePoolDT.m_EventTimeDT.iLevel)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LotteryLimitEventPage, UIMessageDef.UI_CLOSE);
            string str1 = "Cấp {0} mở!";
            UITool.Ui_Trip(string.Format(str1, eventTimePoolDT.m_EventTimeDT.iLevel));
            return;
        }
        timeLeft = eventTimePoolDT.idata2 - GameSocket.GetInstance().f_GetServerTime();
        TimerControl(true);
        LotteryLimitEventDT lotteryLimitEventDT = (LotteryLimitEventDT)glo_Main.GetInstance().m_SC_Pool.m_LotteryLimitEventSC.f_GetSC(eventTimePoolDT.IEventId);
        CardDT cardDT = (CardDT)glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(lotteryLimitEventDT.iCardId);
        //GameObject a = UITool.f_GetStatelObject(lotteryLimitEventDT.iCardId);
        UITool.f_CreateRoleByCardId(lotteryLimitEventDT.iCardId, ref mRole, mModelParent.transform, 3);
        InitBtnLotteryInfo(f_GetObject("BtnBuyOneAdmiral"), lotteryLimitEventDT.szOnceCost);
        InitBtnLotteryInfo(f_GetObject("BtnBuyTenAdmiral"), lotteryLimitEventDT.szTenCost);
        UILabel LabelSendAdHint = f_GetObject("LabelSendAdHint").GetComponent<UILabel>();
        
        string str = "Còn {0} lần Chiêu mộ chắc chắn nhận {1} , đã chiêu mộ {2}";
        int num = lotteryLimitEventDT.iNum - eventTimePoolDT.idata3;
        string s = eventTimePoolDT.idata3 + "/" + lotteryLimitEventDT.iNum;
        //UITool.f_GetImportantColorName((EM_Important)cardDT.iImportant)
        LabelSendAdHint.text = string.Format(str, num, cardDT.szName, s);
        UILabel LabelName = f_GetObject("LabelName").GetComponent<UILabel>();
        UISprite CampIcon = f_GetObject("CampIcon").GetComponent<UISprite>();
        CampIcon.spriteName = "IconCamp_" + cardDT.iCardCamp;
        string name = cardDT.szName;
        string tempName =name.Replace(" ", "\n");
        LabelName.text = tempName;
        f_GetObject("PopupRate").SetActive(_isPopupRateShow);
    }
    private void OnBtnRateClick(GameObject go, object obj1, object obj2)
    {
        if (_isPopupRateShow)
        {
            _isPopupRateShow = false;
            f_GetObject("PopupRate").SetActive(_isPopupRateShow);
            return;
        }
        else
        {
            _isPopupRateShow = true;
        }
        UILabel lbRate = f_GetObject("LbRate").GetComponent<UILabel>();
        lbRate.text = "";
        lbRate.text = CommonTools.f_GetTransLanguage(2341);
        f_GetObject("PopupRate").SetActive(_isPopupRateShow);
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
        timeLeft--;
        if (timeLeft <= 0)
        {
            //TimerControl(false);
            mLbTime.text = "Đã kết thúc";
            TimerControl(false);
        }
        else
        {
            mLbTime.text = "Còn : " + CommonTools.f_GetStringBySecond(timeLeft);
        }
    }
    private void InitBtnLotteryInfo(GameObject Btn,string szCost)
    {
        ResourceCommonDT cost = CommonTools.f_GetCommonResourceByResourceStr(szCost);
        int AdCount = Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(cost.mResourceId);
        UISprite IconSycee = Btn.transform.Find("IconProp/IconSycee").GetComponent<UISprite>();
        IconSycee.spriteName =  UITool.f_GetMoneySpriteName((EM_MoneyType)cost.mResourceId);
        int num = cost.mResourceNum;
        string ShowNum = num.ToString();
        ShowNum = (AdCount >= cost.mResourceNum? "[24ff00]": "[ec2626]") + ShowNum;
        Btn.transform.Find("IconProp/LabelCount").GetComponent<UILabel>().text = ShowNum;

        IconSycee.ResetAnchors();
        IconSycee.transform.localScale = Vector3.one;
    }
    private void OnBtnOnceClick(GameObject go, object obj1, object obj2)
    {
        EventTimePoolDT eventTimePoolDT = m_List[m_SelectIndex];
        if (timeLeft<=0)
        {
            // thông báo sự kiện đã kết thúc
            UITool.Ui_Trip("Sự kiện đã kết thúc");
            return;
        }
        Data_Pool.m_CardPool.f_ClearlistLastAddCardTempID();
        LotteryLimitEventDT lotteryLimitEventDT = (LotteryLimitEventDT)glo_Main.GetInstance().m_SC_Pool.m_LotteryLimitEventSC.f_GetSC(eventTimePoolDT.IEventId);
        ResourceCommonDT cost = CommonTools.f_GetCommonResourceByResourceStr(lotteryLimitEventDT.szOnceCost);
        if (UITool.f_CheckEnoughWaste((byte)cost.mResourceType, cost.mResourceId, cost.mResourceNum,0, 0,0, this))
        {
            Data_Pool.m_ShopLotteryPool.f_LotteryLimit(eventTimePoolDT.IEventTimeId, eventTimePoolDT.IEventId, 1, ReqeustBuyLotteryCallback);
            UITool.f_OpenOrCloseWaitTip(true);
        }
        
    }
    private void OnBtnTenClick(GameObject go, object obj1, object obj2)
    {
        EventTimePoolDT eventTimePoolDT = m_List[m_SelectIndex];
        if (timeLeft <= 0)
        {
            // thông báo sự kiện đã kết thúc
            UITool.Ui_Trip("Sự kiện đã kết thúc");
            return;
        }
        Data_Pool.m_CardPool.f_ClearlistLastAddCardTempID();
        LotteryLimitEventDT lotteryLimitEventDT = (LotteryLimitEventDT)glo_Main.GetInstance().m_SC_Pool.m_LotteryLimitEventSC.f_GetSC(eventTimePoolDT.IEventId);
        ResourceCommonDT cost = CommonTools.f_GetCommonResourceByResourceStr(lotteryLimitEventDT.szTenCost);
        if (UITool.f_CheckEnoughWaste((byte)cost.mResourceType, cost.mResourceId, cost.mResourceNum, 0, 0, 0, this))
        {
            Data_Pool.m_ShopLotteryPool.f_LotteryLimit(eventTimePoolDT.IEventTimeId, eventTimePoolDT.IEventId, 2, ReqeustBuyLotteryCallback);
            UITool.f_OpenOrCloseWaitTip(true);
        }
    }
    private void OnBtnReturnClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LotteryLimitEventPage, UIMessageDef.UI_CLOSE);
    }

    private void OnRequestLotteryBuySucCallback(object obj)
    {
        UpdateContent();
        //eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        //Debug.Log(teMsgOperateResult.ToString());
        List<int> listGetCard = Data_Pool.m_CardPool.f_GetlistLastAddCardTempID();
        ShowGetCardList(listGetCard);
        UITool.f_OpenOrCloseWaitTip(false);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_UPDATE_USERINFOR);
    }
    private void OnRequestLotteryBuyFailCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1116) + CommonTools.f_GetTransLanguage((int)obj));
    }

    private void ShowGetCardList(List<int> listGetCard)
    {
        ccUIBase _UI = ccUIManage.GetInstance().f_GetUIHandler(UINameConst.LotteryLimitEventPage);
        ccUIHoldPool.GetInstance().f_Hold(_UI);
        EventTimePoolDT eventTimePoolDT = m_List[m_SelectIndex];
        LotteryLimitEventDT lotteryLimitEventDT = (LotteryLimitEventDT)glo_Main.GetInstance().m_SC_Pool.m_LotteryLimitEventSC.f_GetSC(eventTimePoolDT.IEventId);
        ResourceCommonDT cost = CommonTools.f_GetCommonResourceByResourceStr(lotteryLimitEventDT.szOnceCost);
        EquipSythesis tSythesis = new EquipSythesis(listGetCard);
        LotteryLimitData data = new LotteryLimitData(tSythesis, cost);
        tSythesis.m_OnCloseCallback = (object obj) =>
        {
            OnBtnOnceClick(null, null, null);
        };
       
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainCardLimitShowPage, UIMessageDef.UI_OPEN, data);
    }
}
