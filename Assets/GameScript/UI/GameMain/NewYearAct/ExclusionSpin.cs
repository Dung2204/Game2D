using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;
using UnityEngine;
class ExclusionSpin : UIFramwork
{
    private SocketCallbackDT tExclusionSpin = new SocketCallbackDT();  //获取
    private UIWrapComponent tUIWrapComponent1;
    private UIWrapComponent tUIWrapComponent2;
    private UIWrapComponent tUIWrapComponent3;
    private List<ccU3DEngine.BasePoolDT<long>> tData1 = new List<ccU3DEngine.BasePoolDT<long>>();
    private List<ccU3DEngine.BasePoolDT<long>> tData2 = new List<ccU3DEngine.BasePoolDT<long>>();
    private List<ccU3DEngine.BasePoolDT<long>> tData3 = new List<ccU3DEngine.BasePoolDT<long>>();
    //private string strTexAdsRoot = "UI/TextureRemove/Activity/Tex_ActAd";
    //UITexture tUITexture;
    private UIWrapComponent mUIWrapComponent1
    {
        get
        {
            if(tUIWrapComponent1 == null)
            {
                tUIWrapComponent1 = new UIWrapComponent(0, 8, 233, 1, f_GetObject("ItemParent1"), f_GetObject("Item"),
                   tData1, UpdateItem, null);
            }
            return tUIWrapComponent1;
        }
    }
    private UIWrapComponent mUIWrapComponent2
    {
        get
        {
            if (tUIWrapComponent2 == null)
            {
                tUIWrapComponent2 = new UIWrapComponent(0, 8, 233, 1, f_GetObject("ItemParent2"), f_GetObject("Item"),
                   tData2, UpdateItem, null);
            }
            return tUIWrapComponent2;
        }
    }

    private UIWrapComponent mUIWrapComponent3
    {
        get
        {
            if (tUIWrapComponent3 == null)
            {
                tUIWrapComponent3 = new UIWrapComponent(0, 8, 233, 1, f_GetObject("ItemParent3"), f_GetObject("Item"),
                   tData3, UpdateItem, null);
            }
            return tUIWrapComponent3;
        }
    }

    public void f_ShowView(UIFramwork actPage)
    {
        gameObject.SetActive(true);

        f_RegClickEvent(f_GetObject("BtnSpin"), BtnSpin);
        f_RegClickEvent(f_GetObject("BtnLuck"), BtnLuck);
		f_RegClickEvent("BtnHelp", f_OnHelpBtnClick);

        LoadBtnSpin();
        //
        tExclusionSpin.m_ccCallbackFail = GetFail;
        tExclusionSpin.m_ccCallbackSuc = GetSuc;
        UpdatePool();
		////My Code
		//FestivalExchangeDT tFestivalExchangeDT = glo_Main.GetInstance().m_SC_Pool.m_FestivalExchangeSC.f_GetAll()[glo_Main.GetInstance().m_SC_Pool.m_FestivalExchangeSC.f_GetAll().Count -1] as FestivalExchangeDT;
		//MessageBox.ASSERT("Begin: " + tFestivalExchangeDT.iBeginTime.ToString() + " End: " + tFestivalExchangeDT.iEndTime.ToString());
		//string BeginTime = tFestivalExchangeDT.iBeginTime + "";
		//string EndTime = tFestivalExchangeDT.iEndTime + "";
        //f_GetObject("BeginTime").GetComponent<UILabel>().text = string.Format("Event open time: {0}/{1}/{2} 0:00", BeginTime.Substring(6, 2 ), BeginTime.Substring(4, 2), BeginTime.Substring(0, 4));
		//f_GetObject("EndTime").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1473), EndTime.Substring(6, 2), EndTime.Substring(4, 2), EndTime.Substring(0, 4));
		////
        mUIWrapComponent1.f_UpdateView();
        mUIWrapComponent2.f_UpdateView();
        mUIWrapComponent3.f_UpdateView();


    }

    private void LoadBtnSpin()
    { 
        UITool.f_SetSpriteGray(f_GetObject("BtnSpin"), !Data_Pool.m_NewYearActivityPool.CanExclusionSpin());
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

    private void BtnLuck(GameObject go, object obj1, object obj2)
    {
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.NewYearActPage, UIMessageDef.UI_OPEN, EM_NewYearActType.DealsEveryDay);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ActivityPage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SupermarketPage, UIMessageDef.UI_OPEN, EM_SupermarketType.DealsEveryDay);
    }
    public void f_DestoryView()
    {
        gameObject.SetActive(false);
    }

    private void UpdateItem(Transform Item, BasePoolDT<long> data)
    {
        ExclusionSpinPoolDT tExclusionSpinPoolDT = data as ExclusionSpinPoolDT;
       
        GameObject Hot = Item.Find("Hot").gameObject;
        GameObject Get = Item.Find("Get").gameObject;

        ResourceCommonItem item1 = Item.Find("ResourceCommonItem").GetComponent<ResourceCommonItem>();
      
        string[] tStringArr = tExclusionSpinPoolDT.mExclusionSpinPoolDT.szAward.Split('#');
        string[] tItem = tStringArr[0].Split(';');
        item1.f_UpdateByInfo(int.Parse(tItem[0]), int.Parse(tItem[1]), int.Parse(tItem[2]));

        if (tExclusionSpinPoolDT.IsCanGet)
        {
            UITool.f_SetSpriteGray(Item.gameObject, true);
        }
        else
        {
            UITool.f_SetSpriteGray(Item.gameObject, false);
        }

        UITool.f_SetSpriteGray(Get, !tExclusionSpinPoolDT.IsCanGet);
        Get.SetActive(tExclusionSpinPoolDT.IsCanGet);
        Hot.SetActive(tExclusionSpinPoolDT.mExclusionSpinPoolDT.iHot > 0);

    }

    private void BtnSpin(GameObject go, object obj1, object obj2)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_NewYearActivityPool.f_ExclusionSpin(tExclusionSpin);
    }
	
	private void f_OnHelpBtnClick(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonHelpPage, UIMessageDef.UI_OPEN, 25);
    }

    //private void Btn_Get7(GameObject go, object obj1, object obj2)
    //{
    //    GameParamDT param = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.DealsEveryDay);
    //
    //    int uCoin = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Coin);
    //    if (Data_Pool.m_NewYearActivityPool.CanBuyDealsEveryDay())
    //    {
    //        if (uCoin >= param.iParam1)
    //        {
    //            PopupMenuParams tParam = new PopupMenuParams("Confirm", string.Format("Use {0} Voucher \n buy package {1}", param.iParam1, ""), "Confirm", f_ConfirmRecharge7, "Cancel", null, null);
    //            ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParam);
    //        }
    //        else
    //        {
    //            UITool.Ui_Trip("Not enough vouchers, please refill!");
    //            glo_Main.GetInstance().m_SDKCmponent.f_ShowSDKPay();
    //        }
    //    }
    //}

    //private void f_ConfirmRecharge7(object value)
    //{
    //    UITool.f_OpenOrCloseWaitTip(true);
    //    Data_Pool.m_NewYearActivityPool.f_GetDealsEveryDaye7(tDealsEveryDay);
    //}
    //
    //private void f_ConfirmRecharge(object value)
    //{
    //    DealsEveryDayPoolDT tDealsEveryDayPoolDT = (DealsEveryDayPoolDT)value;
    //    UITool.f_OpenOrCloseWaitTip(true);
    //    Data_Pool.m_NewYearActivityPool.f_GetDealsEveryDaye(tDealsEveryDayPoolDT.mId, tDealsEveryDay);
    //}
    //
   private void GetSuc(object obj)
   {
      UITool.f_OpenOrCloseWaitTip(false);
        //todo hieu ung spin
        tUIWrapComponent1.f_UpdateView();
        tUIWrapComponent2.f_UpdateView();
        tUIWrapComponent3.f_UpdateView();
        //UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1498));
        LoadBtnSpin();
   }

   private void GetFail(object obj)
   {
      UITool.f_OpenOrCloseWaitTip(false);
      UITool.Ui_Trip(CommonTools.f_GetTransLanguage((int)obj));
   }
   
    private void UpdatePool()
    {
        tData1.Clear();
        tData2.Clear();
        tData3.Clear();
		MessageBox.ASSERT("Exdata: " + Data_Pool.m_NewYearActivityPool.mExclusionSpinPoolDTData.Count);
        for (int i = 1; i <= Data_Pool.m_NewYearActivityPool.mExclusionSpinPoolDTData.Count; i++)
        {
            ExclusionSpinPoolDT texclusionSpinPoolDT = Data_Pool.m_NewYearActivityPool.mExclusionSpinPoolDTData[i];
            switch (texclusionSpinPoolDT.mExclusionSpinPoolDT.iPos)
            {
                case 1:
                    tData1.Add(texclusionSpinPoolDT);
                    break;
                case 2:
                    tData2.Add(texclusionSpinPoolDT);
                    break;
                case 3:
                    tData3.Add(texclusionSpinPoolDT);
                    break;
            }
        }
    }
}

