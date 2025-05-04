using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 限时优惠活动页面
/// </summary>
public class TimeDiscounPage : UIFramwork
{
    private UIWrapComponent _contentWrapComponet = null;
    private SocketCallbackDT RequestQueryCallback = new SocketCallbackDT();//查询回调
    private SocketCallbackDT BuyTimeShopCallback = new SocketCallbackDT();//购买限时商品回调
    private SocketCallbackDT GetRechargeAwardCallback = new SocketCallbackDT();//领取优惠奖励回调
    private SocketCallbackDT GetOpenSevAwardCallback = new SocketCallbackDT();//领取全民福利奖励回调
    private long buyDiscountPropId;//购买物品的id
    private long getOpenSevId;//领取全民福利的物品id
    private int hasBuyOpenServCount = 0;//已经领取全民福利的物品

    private EM_PageIndex curPageIndex = EM_PageIndex.DiscountProp;
    /// <summary>
    /// 页面开启
    /// </summary>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        f_GetObject("TimeDiscountContent").SetActive(false);
        f_GetObject("RechargeContent").SetActive(false);
        f_GetObject("OpenWelfareContent").SetActive(false);
        curPageIndex = EM_PageIndex.DiscountProp;
        Data_Pool.m_TimeDiscountPool.f_RequestInfo(RequestQueryCallback);
        UITool.f_OpenOrCloseWaitTip(true);
        f_LoadTexture();
    }
    private string strTexBgRoot = "UI/TextureRemove/MainMenu/Tex_TimeDiscountBg";
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        //加载背景图
        UITexture TexBg = f_GetObject("TexBg").GetComponent<UITexture>();
        if (TexBg.mainTexture == null)
        {
            Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot);
            TexBg.mainTexture = tTexture2D;
        }
    }
    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        Data_Pool.m_TimeDiscountPool.f_ChargeInfo(RequestQueryCallback);
        UITool.f_OpenOrCloseWaitTip(true);
    }
    /// <summary>
    /// 初始化注册消息
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnDiscount", OnTapBtnDiscountClick);
        f_RegClickEvent("BtnRecharge", OnTapBtnRechargeClick);
        f_RegClickEvent("BtnOpenWelfare", OnTapBtnOpenWelfareClick);
        f_RegClickEvent("BtnReturn", OnBtnReturnClick);
        f_RegClickEvent("BtnClose", OnBtnReturnClick);
        f_RegClickEvent("BtnGoto", OnBtnGoToClick);
        f_RegClickEvent("BtnGoto2", OnBtnGoToClick);
        f_RegClickEvent("BtnGoRecharge", OnBtnGoRechargeClick);
        f_RegClickEvent("BtnGetRecharge", OnBtnGetRechargeClick);
        RequestQueryCallback.m_ccCallbackSuc = OnQuerySucCallback;
        RequestQueryCallback.m_ccCallbackFail = OnQueryFailCallback;
        BuyTimeShopCallback.m_ccCallbackSuc = OnBuySucCallback;
        BuyTimeShopCallback.m_ccCallbackFail = OnBuyFailCallback;
        GetRechargeAwardCallback.m_ccCallbackSuc = OnGetRechargeAwardSucCallback;
        GetRechargeAwardCallback.m_ccCallbackFail = OnGetRechargeAwardFailCallback;
        GetOpenSevAwardCallback.m_ccCallbackSuc = OnGetOpenServAwardSucCallback;
        GetOpenSevAwardCallback.m_ccCallbackFail = OnGetOpenServAwardFailCallback;

        GameObject ModelParent = f_GetObject("ModelParent");
        UITool.f_GetStatelObject(1100, ModelParent.transform, Vector3.zero, Vector3.zero, 18, "Model", 180);
    }
    /// <summary>
    /// 更新全民福利
    /// </summary>
    private void UpdateOpenServContent()
    {
        if (curPageIndex != EM_PageIndex.OpenWelfare)
            return;
        f_GetObject("LabelHasGetCount").GetComponent<UILabel>().text = Data_Pool.m_TimeDiscountPool.m_BuyCount.ToString();
        List<BasePoolDT<long>> listOpenSev = Data_Pool.m_TimeDiscountPool.m_listOpenServ;
        if (_contentWrapComponet == null)
        {
            _contentWrapComponet = new UIWrapComponent(230, 1, 1474, 8, f_GetObject("OpenWelfareParent"), f_GetObject("OpenWelfareItem"), listOpenSev, OnOpenSevContentItemUpdate, null);
        }
        _contentWrapComponet.f_ResetView();
        hasBuyOpenServCount = 0;
        _contentWrapComponet.f_UpdateView();
        _contentWrapComponet.f_ViewGotoRealIdx(hasBuyOpenServCount <= 0 ? 0 : hasBuyOpenServCount - 1, 2);
    }
    /// <summary>
    /// 全民福利item更新
    /// </summary>
    private void OnOpenSevContentItemUpdate(Transform t, BasePoolDT<long> item)
    {
        TimeDiscountOpenServPoolDT dt = item as TimeDiscountOpenServPoolDT;
        TimeDiscountOpenWelfareItem ctlItem = t.GetComponent<TimeDiscountOpenWelfareItem>();
        if (Data_Pool.m_TimeDiscountPool.m_BuyCount >= dt.m_DiscountAllServDT.iGetLimit)//到达条件
        {
            ctlItem.m_ObjWaitGet.SetActive(false);
            bool isGet = dt.m_buyTimes >= dt.m_DiscountAllServDT.iAllowGetNum ? true : false;
            ctlItem.m_ObjHasGet.SetActive(isGet);
            ctlItem.m_ObjGet.SetActive(!isGet);
            if(isGet)
                hasBuyOpenServCount++;
            if (!isGet)
            {
                f_RegClickEvent(ctlItem.m_ObjGet, OnGetOpenSevAward, dt);
            }
        }
        else
        {
            ctlItem.m_ObjGet.SetActive(false);
            ctlItem.m_ObjHasGet.SetActive(false);
            ctlItem.m_ObjWaitGet.SetActive(true);
        }
        string name = dt.m_DiscountAllServDT.iCount + UITool.f_GetGoodName((EM_ResourceType)dt.m_DiscountAllServDT.iType, dt.m_DiscountAllServDT.iResID);

        ResourceCommonDT resourceCommonDT = new ResourceCommonDT();
        resourceCommonDT.f_UpdateInfo((byte)dt.m_DiscountAllServDT.iType, dt.m_DiscountAllServDT.iResID, dt.m_DiscountAllServDT.iCount);
        string borderName = UITool.f_GetImporentColorName(resourceCommonDT.mImportant, ref name);
        ctlItem.SetData(name, dt.m_DiscountAllServDT.iGetLimit, (EM_ResourceType)dt.m_DiscountAllServDT.iType, dt.m_DiscountAllServDT.iResID,
            borderName, dt.m_DiscountAllServDT.iCount);

        ResourceCommonDT commonData = new ResourceCommonDT();
        commonData.f_UpdateInfo((byte)dt.m_DiscountAllServDT.iType, dt.m_DiscountAllServDT.iResID, dt.m_DiscountAllServDT.iCount);
        f_RegClickEvent(ctlItem.m_SprIcon.gameObject, OnOpenSevContentItemIconClick, commonData);
    }

    /// <summary>
    /// 点击icon图标
    /// </summary>
    private void OnOpenSevContentItemIconClick(GameObject go, object obj1, object obj2)
    {
        ResourceCommonDT commonData = (ResourceCommonDT)obj1;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, commonData);
    }
    /// <summary>
    /// 获取全民福利奖励
    /// </summary>
    private void OnGetOpenSevAward(GameObject go,object obj1,object obj2)
    {
        TimeDiscountOpenServPoolDT dt = obj1 as TimeDiscountOpenServPoolDT;
        getOpenSevId = dt.iId;
        Data_Pool.m_TimeDiscountPool.f_OpenServGet((int)dt.iId, GetOpenSevAwardCallback);
        UITool.f_OpenOrCloseWaitTip(true);
    }
    /// <summary>
    /// 更新充值优惠信息
    /// </summary>
    private void UpdateRechageContent()
    {
        if (curPageIndex != EM_PageIndex.ReduceRecharge)
            return;
        //1.设置UI状态
        bool isOpen = Data_Pool.m_TimeDiscountPool.m_proc >= 100 ? true : false;
        if (!isOpen)
        {
            f_GetObject("RechargeOpen").SetActive(isOpen);
            f_GetObject("RechargeClose").SetActive(!isOpen);
            return;
        }
        if (Data_Pool.m_TimeDiscountPool.m_DiscountRechargeDT != null)
        {
            GameObject openParent = f_GetObject("RechargeOpen");
            f_GetObject("BtnGetRecharge").SetActive(Data_Pool.m_TimeDiscountPool.m_GetState); 
            f_GetObject("BtnGoRecharge").SetActive(!Data_Pool.m_TimeDiscountPool.m_GetState);
openParent.transform.Find("Grid/LabelLevel").GetComponent<UILabel>().text = "You are offered a deposit promotion： [F34A6BFF]" + Data_Pool.m_TimeDiscountPool.m_DiscountRechargeDT.iLevel + " KNB";
openParent.transform.Find("Grid/LabelReturnValue").GetComponent<UILabel>().text = "Extra KNB： [F5C24CFF]" + Data_Pool.m_TimeDiscountPool.m_DiscountRechargeDT.iReturn + "KNB";
            int timeHour = Data_Pool.m_TimeDiscountPool.m_DiscountRechargeDT.iDiscountTime;
            int timeHasGoneSecond = GameSocket.GetInstance().f_GetServerTime() - Data_Pool.m_TimeDiscountPool.m_StartTime;
            int SecondNow = timeHour * 60 * 60 - timeHasGoneSecond;
            int hour = SecondNow / 3600;
            int minute = (SecondNow % 3600) / 60;
openParent.transform.Find("Grid/LabelTime").GetComponent<UILabel>().text = "Promotion [FE7203FF]" + hour + "hour" + minute + "more minutes[FFFFFFFF] will end";
        }
    }
    /// <summary>
    /// 更新限时商品
    /// </summary>
    private void UpdateDiscountContent()
    {
        if (curPageIndex != EM_PageIndex.DiscountProp)
            return;
        List<BasePoolDT<long>> poolDT = Data_Pool.m_TimeDiscountPool.f_GetAll();
        if (poolDT.Count < 6)
        {
            return;
        }
        for (int i = 0; i < 6; i++)
        {
            TimeDiscountPoolDT dt = poolDT[i] as TimeDiscountPoolDT;
            GameObject item = f_GetObject("Item" + i);
            item.gameObject.SetActive(true);
            ResourceCommonDT commonData = new ResourceCommonDT();
            commonData.f_UpdateInfo((byte)(EM_ResourceType)dt.m_DiscountPropDT.iSellId, dt.m_DiscountPropDT.iRescId, dt.m_DiscountPropDT.iCount);
            string Name = commonData.mName;
            string BorderName = UITool.f_GetImporentColorName(commonData.mImportant, ref Name);
            item.GetComponent<DiscountItem>().SetData((EM_ResourceType)commonData.mResourceType, commonData.mResourceId, BorderName, Name,
                commonData.mResourceNum, dt.m_DiscountPropDT.iDiscountPer, dt.m_DiscountPropDT.iPrimeNum, dt.m_DiscountPropDT.iDiscountPrice, (EM_MoneyType)dt.m_DiscountPropDT.iResID,
                dt.m_BuyTimes);
            f_RegClickEvent(item.GetComponent<DiscountItem>().m_BtnBuy, OnBuyDiscountPropClick, dt);
            f_RegClickEvent(item.GetComponent<DiscountItem>().m_icon.gameObject, OnItemIconClick, dt);
        }
        f_GetObject("SliderDiscount").GetComponent<UISlider>().value = Data_Pool.m_TimeDiscountPool.m_proc * 1.0f / 100;
        f_GetObject("SliderDiscount").transform.Find("LabelValue").GetComponent<UILabel>().text = Data_Pool.m_TimeDiscountPool.m_proc + "/" + 100;
    }
    /// <summary>
    /// 点击icon图标
    /// </summary>
    private void OnItemIconClick(GameObject go, object obj1, object obj2)
    {
        TimeDiscountPoolDT dt = obj1 as TimeDiscountPoolDT;
        ResourceCommonDT commonData = new ResourceCommonDT();
        commonData.f_UpdateInfo((byte)dt.m_DiscountPropDT.iSellId, dt.m_DiscountPropDT.iRescId, dt.m_DiscountPropDT.iCount);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, commonData);
    }
    /// <summary>
    /// 点击购买限时折扣物品
    /// </summary>
    private void OnBuyDiscountPropClick(GameObject go, object obj1, object obj2)
    {
        TimeDiscountPoolDT dt = obj1 as TimeDiscountPoolDT;
        buyDiscountPropId = dt.iId;
        Data_Pool.m_TimeDiscountPool.f_Buy((int)dt.iId, BuyTimeShopCallback);
        UITool.f_OpenOrCloseWaitTip(true);
    }
    #region 服务器回调
    /// <summary>
    /// 信息回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnQuerySucCallback(object obj)
    {
        UpdateDiscountContent();
        UpdateRechageContent();
        UpdateOpenServContent();
        ShowContent(curPageIndex);
        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void OnQueryFailCallback(object obj)
    {
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Gửi không thành công!");
        UITool.f_OpenOrCloseWaitTip(false);
    }

    /// <summary>
    /// 购买等级礼包回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnBuySucCallback(object obj)
    {
        TimeDiscountPoolDT dt = Data_Pool.m_TimeDiscountPool.f_GetForId(buyDiscountPropId) as TimeDiscountPoolDT;
        UpdateDiscountContent();
        List<AwardPoolDT> awardList = new List<AwardPoolDT>();
        AwardPoolDT item1 = new AwardPoolDT();
        item1.f_UpdateByInfo((byte)(EM_ResourceType)dt.m_DiscountPropDT.iSellId, dt.m_DiscountPropDT.iRescId, dt.m_DiscountPropDT.iCount);
        awardList.Add(item1);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { awardList });
        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void OnBuyFailCallback(object obj)
    {
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Mua hàng không thành công!");
        UITool.f_OpenOrCloseWaitTip(false);
    }
    /// <summary>
    /// 领取充值优惠奖励回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnGetRechargeAwardSucCallback(object obj)
    {
        Data_Pool.m_TimeDiscountPool.m_proc = 0;
        UpdateDiscountContent();
        UpdateRechageContent();
        List<AwardPoolDT> awardList = new List<AwardPoolDT>();
        AwardPoolDT item1 = new AwardPoolDT();
        item1.f_UpdateByInfo((byte)(EM_ResourceType.Money), (int)EM_MoneyType.eUserAttr_Sycee, Data_Pool.m_TimeDiscountPool.m_DiscountRechargeDT.iReturn);
        awardList.Add(item1);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { awardList });
        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void OnGetRechargeAwardFailCallback(object obj)
    {
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Không thành công!");
        UITool.f_OpenOrCloseWaitTip(false);
    }
    /// <summary>
    /// 领取全民福利奖励回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnGetOpenServAwardSucCallback(object obj)
    {
        _contentWrapComponet.f_UpdateView();
        UITool.f_OpenOrCloseWaitTip(true);
        DiscountAllServDT dt = glo_Main.GetInstance().m_SC_Pool.m_DiscountAllServSC.f_GetSC((int)getOpenSevId) as DiscountAllServDT;
        List<AwardPoolDT> awardList = new List<AwardPoolDT>();
        AwardPoolDT item1 = new AwardPoolDT();
        item1.f_UpdateByInfo((byte)dt.iType, (int)dt.iResID, dt.iCount);
        awardList.Add(item1);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { awardList });
        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void OnGetOpenServAwardFailCallback(object obj)
    {
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Không thành công!");
        UITool.f_OpenOrCloseWaitTip(false);
    }
    #endregion
    /// <summary>
    /// 显示页面内容
    /// </summary>
    /// <param name="pageIndex">页码</param>
    private void ShowContent(EM_PageIndex pageIndex)
    {
        f_GetObject("TimeDiscountContent").SetActive(pageIndex == EM_PageIndex.DiscountProp);
        f_GetObject("RechargeContent").SetActive(pageIndex == EM_PageIndex.ReduceRecharge);
        f_GetObject("OpenWelfareContent").SetActive(pageIndex == EM_PageIndex.OpenWelfare);
        f_GetObject("BtnDiscount").transform.Find("BtnPress").gameObject.SetActive(pageIndex == EM_PageIndex.DiscountProp);
        f_GetObject("BtnRecharge").transform.Find("BtnPress").gameObject.SetActive(pageIndex == EM_PageIndex.ReduceRecharge);
        f_GetObject("BtnOpenWelfare").transform.Find("BtnPress").gameObject.SetActive(pageIndex == EM_PageIndex.OpenWelfare);
    }
    #region 按钮消息
    /// <summary>
    /// 点击返回按钮，关闭界面
    /// </summary>
    private void OnBtnReturnClick(GameObject go, object obj1, object obj2)
    {
        //ccUIHoldPool.GetInstance().f_UnHold();
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TimeDiscountPage, UIMessageDef.UI_CLOSE);
    }
    /// <summary>
    /// 点击充值按钮
    /// </summary>
    private void OnBtnGoRechargeClick(GameObject go, object obj1, object obj2)
    {
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShowVip, UIMessageDef.UI_OPEN);
    }
    /// <summary>
    /// 点击领取按钮
    /// </summary>
    private void OnBtnGetRechargeClick(GameObject go, object obj1, object obj2)
    {
        Data_Pool.m_TimeDiscountPool.f_ChargeGet(GetRechargeAwardCallback);
        UITool.f_OpenOrCloseWaitTip(true);
    }
    

    /// <summary>
    /// 点击前往按钮
    /// </summary>
    private void OnBtnGoToClick(GameObject go, object obj1, object obj2)
    {
        curPageIndex = EM_PageIndex.DiscountProp;
        ShowContent(curPageIndex);
    }
    /// <summary>
    /// 点击折扣道具分页按钮
    /// </summary>
    private void OnTapBtnDiscountClick(GameObject go, object obj1, object obj2)
    {
        curPageIndex = EM_PageIndex.DiscountProp;
        ShowContent(curPageIndex);
    }
    /// <summary>
    /// 点击充值优惠分页按钮
    /// </summary>
    private void OnTapBtnRechargeClick(GameObject go, object obj1, object obj2)
    {
        curPageIndex = EM_PageIndex.ReduceRecharge;
        ShowContent(curPageIndex);
        //1.查询和设置UI状态
        bool isOpen = Data_Pool.m_TimeDiscountPool.m_proc >= 100 ? true : false;
        if (isOpen)
        {
            Data_Pool.m_TimeDiscountPool.f_ChargeInfo(RequestQueryCallback);
            UITool.f_OpenOrCloseWaitTip(true);
        }
        f_GetObject("RechargeOpen").SetActive(isOpen);
        f_GetObject("RechargeClose").SetActive(!isOpen);
    }
    /// <summary>
    /// 点击全民福利分页按钮
    /// </summary>
    private void OnTapBtnOpenWelfareClick(GameObject go, object obj1, object obj2)
    {
        curPageIndex = EM_PageIndex.OpenWelfare;
        ShowContent(curPageIndex);
        UpdateOpenServContent();
        Data_Pool.m_TimeDiscountPool.f_OpenServInfo(RequestQueryCallback);
        UITool.f_OpenOrCloseWaitTip(true);
    }
    #endregion
    /// <summary>
    /// 分页类型
    /// </summary>
    private enum EM_PageIndex
    {
        DiscountProp = 1,//折扣道具分页
        ReduceRecharge = 2,//充值优惠
        OpenWelfare = 3,//全民福利
    }
}
