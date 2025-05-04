using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;
using UnityEngine;
class DealsEveryDay : UIFramwork
{
    private SocketCallbackDT tDealsEveryDay = new SocketCallbackDT();  //获取
    private UIWrapComponent tUIWrapComponent;
    private List<ccU3DEngine.BasePoolDT<long>> tData = new List<ccU3DEngine.BasePoolDT<long>>();
    //private string strTexAdsRoot = "UI/TextureRemove/Activity/Tex_ActAd";
    //UITexture tUITexture;
    private UIWrapComponent mUIWrapComponent
    {
        get
        {
            if(tUIWrapComponent == null)
            {
                tUIWrapComponent = new UIWrapComponent(0, 4, 260, 1, f_GetObject("ItemParent"), f_GetObject("Item"),
                   tData, UpdateItem, null);
            }
            return tUIWrapComponent;
        }
    }

    public void f_ShowView(UIFramwork actPage)
    {
        gameObject.SetActive(true);

        f_RegClickEvent(f_GetObject("BtnBuy7"), Btn_Get7);
        f_RegClickEvent(f_GetObject("BtnLuck"), BtnLuck);
        LoadBtnBuy7();

        tDealsEveryDay.m_ccCallbackFail = GetFail;
        tDealsEveryDay.m_ccCallbackSuc = GetSuc;
        UpdatePool();
		////My Code
		//FestivalExchangeDT tFestivalExchangeDT = glo_Main.GetInstance().m_SC_Pool.m_FestivalExchangeSC.f_GetAll()[glo_Main.GetInstance().m_SC_Pool.m_FestivalExchangeSC.f_GetAll().Count -1] as FestivalExchangeDT;
		//MessageBox.ASSERT("Begin: " + tFestivalExchangeDT.iBeginTime.ToString() + " End: " + tFestivalExchangeDT.iEndTime.ToString());
		//string BeginTime = tFestivalExchangeDT.iBeginTime + "";
		//string EndTime = tFestivalExchangeDT.iEndTime + "";
        //f_GetObject("BeginTime").GetComponent<UILabel>().text = string.Format("Event open time: {0}/{1}/{2} 0:00", BeginTime.Substring(6, 2 ), BeginTime.Substring(4, 2), BeginTime.Substring(0, 4));
		//f_GetObject("EndTime").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1473), EndTime.Substring(6, 2), EndTime.Substring(4, 2), EndTime.Substring(0, 4));
		////
        mUIWrapComponent.f_UpdateView();


    }

    private void LoadBtnBuy7()
    {
        if (!Data_Pool.m_NewYearActivityPool.CanBuyDealsEveryDay())
        {
            UITool.f_SetSpriteGray(f_GetObject("BtnBuy7"), true);
            return;
        }
    }

    //private void LoadTexture()
    //{
    //    if(tUITexture == null)
    //    {
    //        tUITexture = f_GetObject("Texture").GetComponent<UITexture>();
    //    }
    //    if(tUITexture.mainTexture == null)
    //        tUITexture.mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexAdsRoot);
    //}

    public void f_DestoryView()
    {
        gameObject.SetActive(false);
    }

    private void UpdateItem(Transform Item, BasePoolDT<long> data)
    {
        DealsEveryDayPoolDT tDealsEveryDayPoolDT = data as DealsEveryDayPoolDT;
    
        Transform tTran = Item.Find("GoodsItemGrid");
        GameObject Btn = Item.Find("Btn").gameObject;
        UILabel Sale = Item.Find("Sale").GetComponent<UILabel>();
        UILabel Label = Item.Find("Btn/Label").GetComponent<UILabel>();
        ResourceCommonItem item1 = tTran.GetChild(0).GetComponent<ResourceCommonItem>();
        ResourceCommonItem item2 = tTran.GetChild(1).GetComponent<ResourceCommonItem>();
    
        string[] tStringArr = tDealsEveryDayPoolDT.mDealsEveryDayPoolDT.szAward.Split('#');
        string[] tItem = tStringArr[0].Split(';');
        item1.f_UpdateByInfo(int.Parse(tItem[0]), int.Parse(tItem[1]), int.Parse(tItem[2]));
        if(tStringArr.Length >= 2)
        {
            tItem = tStringArr[1].Split(';');
            item2.f_UpdateByInfo(int.Parse(tItem[0]), int.Parse(tItem[1]), int.Parse(tItem[2]));
        }

        Sale.text = tDealsEveryDayPoolDT.mDealsEveryDayPoolDT.iCondition > 0 ? string.Format("+{0}%", tDealsEveryDayPoolDT.mDealsEveryDayPoolDT.szTextSale) : "";
        Label.text = Data_Pool.m_NewYearActivityPool.CanBuyDealsEveryDay() ? tDealsEveryDayPoolDT.mDealsEveryDayPoolDT.szTextCondition : tDealsEveryDayPoolDT.mDealsEveryDayPoolDT.szTextGet;
        if (tDealsEveryDayPoolDT.IsCanGet)
        {
            UITool.f_SetSpriteGray(Btn, true);
            return;
        }
        f_RegClickEvent(Btn, Btn_Get, tDealsEveryDayPoolDT);
    
    }

    private void Btn_Get(GameObject go, object obj1, object obj2)
    {
        DealsEveryDayPoolDT tDealsEveryDayPoolDT = (DealsEveryDayPoolDT)obj1;

        int uCoin = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Coin);
        if(tDealsEveryDayPoolDT.mDealsEveryDayPoolDT.iCondition == 0 || !Data_Pool.m_NewYearActivityPool.CanBuyDealsEveryDay())
        {
            UITool.f_OpenOrCloseWaitTip(true);
            Data_Pool.m_NewYearActivityPool.f_GetDealsEveryDaye(tDealsEveryDayPoolDT.mId, tDealsEveryDay);
        }
        else
        {
            if (uCoin >= tDealsEveryDayPoolDT.mDealsEveryDayPoolDT.iCondition)
            {
                PopupMenuParams tParam = new PopupMenuParams("Xác Nhận", string.Format("Dùng {0} Kim Phiếu \n mua gói {1}", tDealsEveryDayPoolDT.mDealsEveryDayPoolDT.iCondition, tDealsEveryDayPoolDT.mDealsEveryDayPoolDT.szTextCondition), "Đồng ý", f_ConfirmRecharge, "Hủy bỏ", null, tDealsEveryDayPoolDT);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParam);
            }
            else
            {
                UITool.Ui_Trip("Không đủ Kim Phiếu!");
                glo_Main.GetInstance().m_SDKCmponent.f_ShowSDKPay();
            }
        }
    }

    private void Btn_Get7(GameObject go, object obj1, object obj2)
    {
        GameParamDT param = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.DealsEveryDay);

        int uCoin = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Coin);
        if (Data_Pool.m_NewYearActivityPool.CanBuyDealsEveryDay())
        {
            if (uCoin >= param.iParam1)
            {
                PopupMenuParams tParam = new PopupMenuParams("Xác Nhận", string.Format("Dùng {0} Kim Phiếu \n mua gói {1}", param.iParam1, ""), "Đồng ý", f_ConfirmRecharge7, "Hủy bỏ", null, null);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParam);
            }
            else
            {
                UITool.Ui_Trip("Không đủ Kim Phiếu!");
                glo_Main.GetInstance().m_SDKCmponent.f_ShowSDKPay();
            }
        }
    }

    private void BtnLuck(GameObject go, object obj1, object obj2)
    {
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.NewYearActPage, UIMessageDef.UI_OPEN, EM_NewYearActType.ExclusionSpin);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SupermarketPage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ActivityPage, UIMessageDef.UI_OPEN, EM_ActivityType.ExclusionSpin);
    }

    private void f_ConfirmRecharge7(object value)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_NewYearActivityPool.f_GetDealsEveryDaye7(tDealsEveryDay);
    }

    private void f_ConfirmRecharge(object value)
    {
        DealsEveryDayPoolDT tDealsEveryDayPoolDT = (DealsEveryDayPoolDT)value;
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_NewYearActivityPool.f_GetDealsEveryDaye(tDealsEveryDayPoolDT.mId, tDealsEveryDay);
    }

    private void GetSuc(object obj)
   {
       UITool.f_OpenOrCloseWaitTip(false);
       mUIWrapComponent.f_UpdateView();
        //UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1498));
        LoadBtnBuy7();
   }
   
   private void GetFail(object obj)
   {
       UITool.f_OpenOrCloseWaitTip(false);
       UITool.Ui_Trip(CommonTools.f_GetTransLanguage((int)obj));
   }

    private void UpdatePool()
    {
        tData.Clear();
        for (int i = 1; i <= Data_Pool.m_NewYearActivityPool.mDealsEveryDayPoolDTData.Count; i++)
        {
                tData.Add(Data_Pool.m_NewYearActivityPool.mDealsEveryDayPoolDTData[i]);
        }
    }
}

