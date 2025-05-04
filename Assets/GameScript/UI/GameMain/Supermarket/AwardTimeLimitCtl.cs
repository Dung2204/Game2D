using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwardTimeLimitCtl : UIFramwork
{
    private List<BtnItem> listBtnItem = new List<BtnItem>();
    private Dictionary<BtnItem, GameObject> dicBtnItem = new Dictionary<BtnItem, GameObject>();
    private Dictionary<int, GameObject> dicTypeToBtnItem = new Dictionary<int, GameObject>();

    //private Dictionary<int, GameObject> dicTypeToContentObj = new Dictionary<int, GameObject>();
    private BtnItem m_curSelectBtnItem = null;

    private UILabel labelTime;
    List<BasePoolDT<long>> m_List = new List<BasePoolDT<long>>();
    ccUIBase actPage;
    private SocketCallbackDT ReqeustBuyCallback = new SocketCallbackDT();

    private class BtnItem
    {
        public int m_Id;
        public string m_Name;
        public BtnItem(int id, string name)
        {
            this.m_Id = id;
            this.m_Name = name;
        }
    }
    private UIWrapComponent tUIWrapComponent;
    private UIWrapComponent mUIWrapComponent
    {
        get
        {
            if (tUIWrapComponent == null)
            {

                tUIWrapComponent = new UIWrapComponent(450, 1, 1400, 5, f_GetObject("ItemParent"), f_GetObject("Item"),
                   m_List, UpdateItem, null);
            }
            return tUIWrapComponent;
        }
    }

    public void f_DestoryView()
    {
        gameObject.SetActive(false);
    }
    public void f_ShowView(ccUIBase actPage)
    {
        this.actPage = actPage;
        gameObject.SetActive(true);
        listBtnItem.Clear();
        dicBtnItem.Clear();
        dicTypeToBtnItem.Clear();
        ReqeustBuyCallback.m_ccCallbackSuc = OnRequestBuySucCallback;
        ReqeustBuyCallback.m_ccCallbackFail = OnRequestBuyFailCallback;
        InitUI();
        InitBtnUI();
        UILabel labelTime = f_GetObject("labelTime").GetComponent<UILabel>();
        if (listBtnItem.Count > 0)
        {
            ShowActDefault(listBtnItem[0].m_Id);
            f_GetObject("NorContent").SetActive(false);
            f_GetObject("Content").SetActive(true);
        }
        else
        {
            f_GetObject("NorContent").SetActive(true);
            f_GetObject("Content").SetActive(false);
        }
    }
    // khởi tạo List cửa hàng
    private void InitUI()
    {
        labelTime = f_GetObject("labelTime").GetComponent<UILabel>();
        List<NBaseSCDT> allShop = glo_Main.GetInstance().m_SC_Pool.m_ShopEventTimeSC.f_GetAll();
        for (int i = 0; i < allShop.Count; i++)
        {
            ShopEventTimeDT dt = allShop[i] as ShopEventTimeDT;
            //ShopEventTimePoolDT poolDt = Data_Pool.m_ShopEventTimePool.f_GetForId(dt.iId) as ShopEventTimePoolDT;
            if (Data_Pool.m_ShopEventTimePool.IsOpenById(dt.iId))
            {
                listBtnItem.Add(new BtnItem(dt.iId, dt.szName));
            }
        }
    }
    private void InitBtnUI()
    {
        List<BasePoolDT<long>> listBtnContent = new List<BasePoolDT<long>>();
        for (int i = 0; i < listBtnItem.Count; i++)
        {
            BasePoolDT<long> item = new BasePoolDT<long>();
            item.iId = i;
            listBtnContent.Add(item);
        }
        GridUtil.f_SetGridView<BtnItem>(f_GetObject("BtnItemParent"), f_GetObject("BtnItem"), listBtnItem, OnShowBtnItem);
        f_GetObject("BtnItemParent").transform.GetComponentInParent<UIScrollView>().ResetPosition();
    }
    private void OnShowBtnItem(GameObject item, BtnItem data)
    {
        BtnItem btnItem = data;
        item.GetComponent<ActBtnItemCtl>().SetData(btnItem.m_Name);
        f_RegClickEvent(item.gameObject, OnBtnClick, btnItem);
        if (!dicBtnItem.ContainsKey(btnItem))
        {
            dicBtnItem.Add(btnItem, item.gameObject);
            dicTypeToBtnItem.Add(btnItem.m_Id, item.gameObject);
        }
        item.transform.Find("BtnBg").gameObject.SetActive(!(m_curSelectBtnItem == btnItem));
        item.transform.Find("BtnBgDown").gameObject.SetActive(m_curSelectBtnItem == btnItem);
    }
    private void ShowActDefault(int page)
    {
        for (int i = 0; i < listBtnItem.Count; i++)
        {
            if (listBtnItem[i].m_Id == page)
            {
                OnBtnClick(dicBtnItem[listBtnItem[i]], listBtnItem[i], null);
                UIProgressBar uiProgressBar = f_GetObject("ProgressBar").transform.GetComponent<UIProgressBar>();
                return;
            }
        }
    }
    private void OnBtnClick(GameObject go, object obj1, object obj2)
    {
        BtnItem item = (BtnItem)obj1;
        m_curSelectBtnItem = item;
        CloseAllBtnPressEffect();
        go.transform.Find("BtnBg").gameObject.SetActive(false);
        go.transform.Find("BtnBgDown").gameObject.SetActive(true);
        UpdateContentData(item);
    }
    private void CloseAllBtnPressEffect()
    {
        for (int i = 0; i < listBtnItem.Count; i++)
        {
            if (dicBtnItem.ContainsKey(listBtnItem[i]))
            {
                dicBtnItem[listBtnItem[i]].transform.Find("BtnBg").gameObject.SetActive(true);
                dicBtnItem[listBtnItem[i]].transform.Find("BtnBgDown").gameObject.SetActive(false);
            }
        }
    }
    private int timeLeft;
    private void UpdateContentData(BtnItem item)
    {
        ShopEventTimePoolDT poolDt = Data_Pool.m_ShopEventTimePool.f_GetForId(item.m_Id) as ShopEventTimePoolDT;
        UITexture Banner = f_GetObject("Banner").GetComponent<UITexture>();
        // todo:setbaner
        Banner.mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(poolDt.m_EventTimeDT.szBanner);
        //end
        timeLeft = poolDt.idata2 - GameSocket.GetInstance().f_GetServerTime();
        if (timeLeft <= 0)
        {
            TimerControl(false);
            labelTime.text = "Đã kết thúc";
        }
        else
        {
            TimerControl(true);
        }
        List<BasePoolDT<long>> m_List = Data_Pool.m_ShopEventTimePool.GetList(item.m_Id);
        mUIWrapComponent.f_UpdateList(m_List);
        mUIWrapComponent.f_ResetView();
    }

    private void UpdateItem(Transform Item, BasePoolDT<long> data)
    {
        ShopEventTimeAwardPoolDT tPoolDT = data as ShopEventTimeAwardPoolDT;

        Transform tTran = Item.Find("GoodsItemGrid");
        UI2DSprite Icon = Item.Find("Icon").GetComponent<UI2DSprite>();
        UILabel LabelLimit = Item.Find("labelLimit").GetComponent<UILabel>();
        UILabel LabelMoney = Item.Find("labelMoney").GetComponent<UILabel>();
        Transform scroll = Item.Find("Scroll").transform;
        List<ResourceCommonDT> m_ListAward = CommonTools.f_GetListCommonDT(tPoolDT.m_AwardDT.szAward);
        // todo:setIcon
        Icon.sprite2D = UITool.f_GetIconSprite(m_ListAward[0].mIcon);
        //end
        string strLimit = "Hạn mua: {0}/{1}";
        LabelLimit.text = string.Format(strLimit, tPoolDT.idata1, tPoolDT.m_AwardDT.iLimit);
        if(tPoolDT.m_AwardDT.iFree == 1)
        {
            LabelMoney.text = "Miễn phí";
        }
        else
        {
            LabelMoney.text = tPoolDT.m_AwardDT.iMoney + " VND";
        }
        
        GameObject AwardItem = scroll.Find("ResourceCommonItem").gameObject;//.GetComponent<ResourceCommonItem>();
        GameObject AwardParent = scroll.Find("GoodsItemGrid").gameObject;
        GridUtil.f_SetGridView<ResourceCommonDT>(AwardParent, AwardItem, m_ListAward, OnShowAwardItem);
        AwardParent.transform.GetComponentInParent<UIScrollView>().ResetPosition();
        UISprite bg = Item.Find("Bg").GetComponent<UISprite>();
        if (tPoolDT.idata1 >= tPoolDT.m_AwardDT.iLimit)
        {
            bg.spriteName = "item_bg_gray";
        }
        else
        {
            bg.spriteName = "item_bg";
        }
        f_RegClickEvent(Item.gameObject, Btn_Buy, tPoolDT);

    }

    private void OnShowAwardItem(GameObject item, ResourceCommonDT data)
    {
        ResourceCommonItem _Item = item.GetComponent<ResourceCommonItem>();
        _Item.f_UpdateByInfo(data);
    }
    private void f_Confirm(object value)
    {
        ccUIHoldPool.GetInstance().f_Hold(this.actPage);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShowVip, UIMessageDef.UI_OPEN, ShowVip.EM_PageIndex.RechargeCoin);
    }
    private void f_ConfirmBuy(object value)
    {
        ShopEventTimeAwardPoolDT tPoolDT = (ShopEventTimeAwardPoolDT)value;
        Data_Pool.m_ShopEventTimePool.f_Buy(m_curSelectBtnItem.m_Id, tPoolDT.IAwardId, ReqeustBuyCallback);
        UITool.f_OpenOrCloseWaitTip(true);
    }
    private void Btn_Buy(GameObject go, object obj1, object obj2)
    {
        ShopEventTimeAwardPoolDT tPoolDT = (ShopEventTimeAwardPoolDT)obj1;
        if(tPoolDT.idata1 >= tPoolDT.m_AwardDT.iLimit)
        {
            UITool.Ui_Trip("Đã hết lượt mua.");
            return;
        }
        if(tPoolDT.m_AwardDT.iFree == 1)
        {
            f_ConfirmBuy(tPoolDT);
            return;
        }
        List<ResourceCommonDT> listCommonDT = CommonTools.f_GetListCommonDT(tPoolDT.m_AwardDT.szCost);
        ResourceCommonDT cost = listCommonDT[0];
        int uCoin = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Coin);
        if(cost.mResourceNum > uCoin)
        {
            PopupMenuParams tParam = new PopupMenuParams("Nhắc nhở", "Kim phiếu bạn không đủ cần nạp thêm.", "Đến nạp", f_Confirm, "Hủy", null, tPoolDT);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParam);
            return;
        }
        string str = "Mua vật phẩm này dùng {0} Kim Phiếu.";
        PopupMenuParams tParam1 = new PopupMenuParams("Nhắc nhở", string.Format(str, cost.mResourceNum), "Mua", f_ConfirmBuy, "Hủy", null, tPoolDT);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParam1);
    }
    private void OnRequestBuySucCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        //UpdateContentData(m_curSelectBtnItem);
        mUIWrapComponent.f_UpdateView();
    }
    private void OnRequestBuyFailCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1116) + CommonTools.f_GetTransLanguage((int)obj));
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
            labelTime.text = "Đã kết thúc";
            TimerControl(false);
        }
        else
        {
            labelTime.text = "Còn : " + CommonTools.f_GetStringBySecond(timeLeft);
        }
    }
}
