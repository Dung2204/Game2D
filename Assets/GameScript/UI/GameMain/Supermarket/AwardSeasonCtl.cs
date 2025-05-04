using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwardSeasonCtl : UIFramwork
{

    private UILabel labelTime;
    List<BasePoolDT<long>> m_List = new List<BasePoolDT<long>>();
    ccUIBase actPage;
    private SocketCallbackDT ReqeustBuyCallback = new SocketCallbackDT();
   
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
        this.actPage = actPage;
        gameObject.SetActive(true);
        ReqeustBuyCallback.m_ccCallbackSuc = OnRequestBuySucCallback;
        ReqeustBuyCallback.m_ccCallbackFail = OnRequestBuyFailCallback;
        InitUI();
        UpdateContentData();
    }
    private void InitUI()
    {
        labelTime = f_GetObject("labelTime").GetComponent<UILabel>();
        
    }
    
    private int timeLeft;
    private void UpdateContentData()
    {
        UITexture Banner = f_GetObject("Banner").GetComponent<UITexture>();
        Banner.mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("Tex_Banner_Season1");
        long endTime = Data_Pool.m_ShopSeasonPool.m_iEndTime;
        timeLeft =(int)(endTime - GameSocket.GetInstance().f_GetServerTime());
        if (timeLeft <= 0)
        {
            TimerControl(false);
            labelTime.text = "Đã kết thúc";
        }
        else
        {
            TimerControl(true);
        }
        List<BasePoolDT<long>> m_List = Data_Pool.m_ShopSeasonPool.GetList();
        mUIWrapComponent.f_UpdateList(m_List);
        mUIWrapComponent.f_ResetView();
    }

    private void UpdateItem(Transform Item, BasePoolDT<long> data)
    {
        ShopSeasonAwardPoolDT tPoolDT = data as ShopSeasonAwardPoolDT;

        Transform tTran = Item.Find("GoodsItemGrid");
        UI2DSprite Icon = Item.Find("Icon").GetComponent<UI2DSprite>();
        UILabel LabelLimit = Item.Find("labelLimit").GetComponent<UILabel>();
        UILabel LabelMoney = Item.Find("labelMoney").GetComponent<UILabel>();
        Transform scroll = Item.Find("Scroll").transform;
        List<ResourceCommonDT> m_ListAward = CommonTools.f_GetListCommonDT(tPoolDT.m_PoolDT.szAward);
        // todo:setIcon
        //Icon.sprite2D = UITool.f_GetIconSprite(tPoolDT.m_PoolDT.szIcon);
        Icon.sprite2D = UITool.f_GetIconSprite(m_ListAward[0].mIcon);
        //end
        string strLimit = "Yêu cầu Vip: {0}";
        LabelLimit.text = string.Format(strLimit, tPoolDT.m_PoolDT.iVip);
        if (tPoolDT.m_PoolDT.iFree == 1)
        {
            LabelMoney.text = "Miễn phí";
        }
        else
        {
            LabelMoney.text = tPoolDT.m_PoolDT.iMoney + " VND";
        }
        
        GameObject AwardItem = scroll.Find("ResourceCommonItem").gameObject;//.GetComponent<ResourceCommonItem>();
        GameObject AwardParent = scroll.Find("GoodsItemGrid").gameObject;
        GridUtil.f_SetGridView<ResourceCommonDT>(AwardParent, AwardItem, m_ListAward, OnShowAwardItem);
        AwardParent.transform.GetComponentInParent<UIScrollView>().ResetPosition();
        UISprite bg = Item.Find("Bg").GetComponent<UISprite>();
        if (tPoolDT.idata1 >= tPoolDT.m_PoolDT.iLimit)
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
        ShopSeasonAwardPoolDT tPoolDT = (ShopSeasonAwardPoolDT)value;
        Data_Pool.m_ShopSeasonPool.f_Buy(tPoolDT.IAwardId, ReqeustBuyCallback);
        UITool.f_OpenOrCloseWaitTip(true);
    }
    private void Btn_Buy(GameObject go, object obj1, object obj2)
    {
        ShopSeasonAwardPoolDT tPoolDT = (ShopSeasonAwardPoolDT)obj1;
        if (tPoolDT.m_PoolDT.iVip > UITool.f_GetNowVipLv())
        {
            UITool.Ui_Trip("Cấp Vip không đủ.");
            return;
        }
        if (tPoolDT.idata1 >= tPoolDT.m_PoolDT.iLimit)
        {
            UITool.Ui_Trip("Đã hết lượt mua.");
            return;
        }
        if (tPoolDT.m_PoolDT.iFree == 1)
        {
            f_ConfirmBuy(tPoolDT);
            return;
        }
        List<ResourceCommonDT> listCommonDT = CommonTools.f_GetListCommonDT(tPoolDT.m_PoolDT.szCost);
        ResourceCommonDT cost = listCommonDT[0];
        int uCoin = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Coin);
        if (cost.mResourceNum > uCoin)
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
        UpdateContentData();
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