using UnityEngine;
using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;

public class NewYearSellingPage : UIFramwork
{

    private string szCenterBgFile = "UI/TextureRemove/NewServer/Texture_SevenDayAwardBg";
    private string szLanternBgFil = "UI/TextureRemove/Activity/Tex_Fortune";

    private UIWrapComponent _ShowItem;
    private SocketCallbackDT _SocketCallback = new SocketCallbackDT();
    private SocketCallbackDT _BuySocketCallback = new SocketCallbackDT();
    private List<BasePoolDT<long>> _NewYearSellingList = new List<BasePoolDT<long>>();

    private int Time_2Day;
    private UIWrapComponent ShowItem
    {
        get
        {
            UpdateList();
            if (_ShowItem == null)
                _ShowItem = new UIWrapComponent(202, 2, 650, 8, f_GetObject("ItemParent"), f_GetObject("Item"), _NewYearSellingList, _UpdateItem, null);
            return _ShowItem;
        }
    }
    private void _LodTexture()
    {
        //f_GetObject("CenterBg").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(szCenterBgFile);
        //f_GetObject("Lantern").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(szLanternBgFil);
    }





    public void f_ShowView(int timeEnd)
    {
        //_LodTexture();
        
        gameObject.SetActive(true);
        if (timeEnd == 0)
        {
            f_GetObject("LabelTimeLeft").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(1472);
            return;
        }
        _SocketCallback.m_ccCallbackFail = _InfoFail;
        _SocketCallback.m_ccCallbackSuc = _InfoSuc;
        Data_Pool.m_NewYearSellingPool.f_NewYearSellingInfo(_SocketCallback);

        string str = timeEnd.ToString();
        int year = int.Parse(str.Substring(0, 4));//20181224
        int month = int.Parse(str.Substring(4, 2));
        int day = int.Parse(str.Substring(6, 2));
        f_GetObject("LabelTimeLeft").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1473), day, month, year);


        //f_GetObject("SprTitleBg").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSprite(306);
        //f_GetObject("SprTitleBg").GetComponent<UI2DSprite>().MakePixelPerfect();
    }

    private void _InfoSuc(object obj)
    {
        ShowItem.f_ResetView();
        Time_2Day = ccTimeEvent.GetInstance().f_RegEvent(1f, true, null, f_2Day);
    }

    private void _InfoFail(object obj)
    {

    }

    private void _UpdateMain()
    {

    }

    /// <summary>
    /// 跨天检测
    /// </summary>
    /// <param name="obj"></param>
    private void f_2Day(object obj)
    {
        if (GameSocket.GetInstance().mNextDayTime.Ticks < GameSocket.GetInstance().f_GetServerTime())
        {
            ShowItem.f_UpdateList(_NewYearSellingList);
            ShowItem.f_ResetView();
        }
    }

    private void UpdateList()
    {
        _NewYearSellingList.Clear();

        int StarTime = 0;
        int EndTime = 0;
        int NowTime = GameSocket.GetInstance().f_GetServerTime();
        for (int i = 0; i < Data_Pool.m_NewYearSellingPool.f_GetAll().Count; i++)
        {

            NewYearSellingPoolDT tNewYearSellingPoolDT = Data_Pool.m_NewYearSellingPool.f_GetAll()[i] as NewYearSellingPoolDT;

            StarTime = f_Data2Int(tNewYearSellingPoolDT.m_NewYearSelling.iStarTime);
            EndTime = f_Data2Int(tNewYearSellingPoolDT.m_NewYearSelling.iEndTime);

            if (tNewYearSellingPoolDT.m_NewYearSelling.iRankDownLimit <= Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level))
            {
                if (NowTime >= StarTime && NowTime < EndTime)
                    _NewYearSellingList.Add(tNewYearSellingPoolDT);
            }

        }
    }

    private int f_Data2Int(int Time)
    {
        string szTime = Time.ToString();

        int Year = int.Parse(szTime.Substring(0, 4));

        int Mon = int.Parse(szTime.Substring(4, 2));

        int Day = int.Parse(szTime.Substring(6, 2));

        return (int)ccMath.f_Time_String2Int(Year, Mon, Day, 0, 0, 0);
    }
    private void _UpdateItem(Transform tran, BasePoolDT<long> tGoods)
    {
        NewYearSellingPoolDT tNewYearSellingPoolDT = (NewYearSellingPoolDT)tGoods;
        ResourceCommonItem tSellId = tran.Find("Goods").GetComponent<ResourceCommonItem>();
        ResourceCommonItem tMonery = tran.Find("Monery").GetComponent<ResourceCommonItem>();
        UILabel tBuyTime = tran.Find("BuyTime").GetComponent<UILabel>();
        UILabel tDiscount = tran.Find("Discount").GetComponent<UILabel>();
        GameObject BuyBtn = tran.Find("BuyBtn").gameObject;

        bool isDiscount = tNewYearSellingPoolDT.m_NewYearSelling.iDiscountPer > 0;

        tSellId.f_UpdateByInfo(tNewYearSellingPoolDT.m_NewYearSelling.iRescId, tNewYearSellingPoolDT.m_NewYearSelling.iSellId, tNewYearSellingPoolDT.m_NewYearSelling.iCount);
        tMonery.f_UpdateByInfo(1, 7, tNewYearSellingPoolDT.m_NewYearSelling.iDiscountPrice);

        tBuyTime.text = string.Format(CommonTools.f_GetTransLanguage(1481),
            tNewYearSellingPoolDT.m_NewYearSelling.iBuyTime - tNewYearSellingPoolDT.m_BuyTime, tNewYearSellingPoolDT.m_NewYearSelling.iBuyTime);
        tDiscount.gameObject.SetActive(isDiscount);
        tDiscount.text = ((10 - tNewYearSellingPoolDT.m_NewYearSelling.iDiscountPer)*10).ToString() + "%";

        BuyBtn.SetActive(tNewYearSellingPoolDT.m_bIsShowBtn);
        f_RegClickEvent(BuyBtn, _BuyGoods, tNewYearSellingPoolDT);
    }

    private void _BuyGoods(GameObject go, object obj1, object obj2)
    {

        NewYearSellingPoolDT tNewYearSellingPoolDT = (NewYearSellingPoolDT)obj1;

        if (tNewYearSellingPoolDT.m_NewYearSelling.iDiscountPrice > Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee))
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1482));
            return;
        }

        BuyParam tBuyParam = new BuyParam();
        tBuyParam.buyHint = CommonTools.f_GetTransLanguage(1483);
        tBuyParam.canBuyTimes = tNewYearSellingPoolDT.m_NewYearSelling.iBuyTime - tNewYearSellingPoolDT.m_BuyTime >= 99 ? 99 : tNewYearSellingPoolDT.m_NewYearSelling.iBuyTime - tNewYearSellingPoolDT.m_BuyTime;
        tBuyParam.iId = tNewYearSellingPoolDT.m_NewYearSelling.iId;
        tBuyParam.moneyType = EM_MoneyType.eUserAttr_Sycee;
        List<int> tListint = new List<int>();
        tListint.Add(tNewYearSellingPoolDT.m_NewYearSelling.iDiscountPrice);
        tBuyParam.price = tListint;
        tBuyParam.resourceCount = tNewYearSellingPoolDT.m_NewYearSelling.iCount;
        tBuyParam.resourceID = tNewYearSellingPoolDT.m_NewYearSelling.iSellId;
        tBuyParam.resourceType = (EM_ResourceType)tNewYearSellingPoolDT.m_NewYearSelling.iRescId;
        tBuyParam.title = CommonTools.f_GetTransLanguage(1484);
        tBuyParam.onConfirmBuyCallback = ConfirmBuyCallback;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.BuyPage, UIMessageDef.UI_OPEN, tBuyParam);
    }

    private void ConfirmBuyCallback(int iId, EM_ResourceType type, int resourceId, int resourceCount, int buyCount)
    {
        Data_Pool.m_AwardPool.m_GetLoginAward.Clear();
        UITool.f_OpenOrCloseWaitTip(true);
        _BuySocketCallback.m_ccCallbackSuc = _BuySuc;
        _BuySocketCallback.m_ccCallbackFail = _BuyFail;

        Data_Pool.m_NewYearSellingPool.f_NewYearSellingBuy(iId, (short)buyCount, _BuySocketCallback);
    }

    private void _BuySuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        Data_Pool.m_ReddotMessagePool.f_MsgSubtract(EM_ReddotMsgType.NewYearSelling);
        _ShowItem.f_UpdateView();
        _ShowItem.f_UpdateList(_NewYearSellingList);
        if (Data_Pool.m_AwardPool.m_GetLoginAward.Count > 0)
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN, new object[] { Data_Pool.m_AwardPool.m_GetLoginAward });
    }

    private void _BuyFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1485) + obj.ToString());
    }
    public void f_DestoryView()
    {
        ccTimeEvent.GetInstance().f_UnRegEvent(Time_2Day);
        gameObject.SetActive(false);
    }
}
