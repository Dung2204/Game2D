using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupermarketPage : UIFramwork
{
    public GameObject ItemRoot;
    private List<BtnItem> listBtnItem = new List<BtnItem>();
    private Dictionary<BtnItem, GameObject> dicBtnItem = new Dictionary<BtnItem, GameObject>();
    private Dictionary<EM_SupermarketType, GameObject> dicTypeToBtnItem = new Dictionary<EM_SupermarketType, GameObject>();

    private Dictionary<EM_SupermarketType, GameObject> dicTypeToContentObj = new Dictionary<EM_SupermarketType, GameObject>();
    private BtnItem m_curSelectBtnItem = null;
    private class BtnItem
    {
        public EM_SupermarketType m_Type;
        public string m_Name;
        public BtnItem(EM_SupermarketType type, string name)
        {
            this.m_Type = type;
            this.m_Name = name;
        }
    }
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        listBtnItem.Clear();
        dicBtnItem.Clear();
        dicTypeToBtnItem.Clear();
        InitMoneyUI();
        InitListBtn();
        InitBtnUI();
        InitRaddot();
        if (e == null)
        {
            ShowActDefault(listBtnItem[0].m_Type);
        }
        else
        {
            ShowActDefault((EM_SupermarketType)e);
        }
        //if (listBtnItem.Count > 0)
        //{
            
            
        //}
        //else
        //{
        //    // chưa mở 
        //}
    }
    private void OnPageResume(object e)//--------------------case 3
    {
        if (this.f_IsOpen())
        {
            OnBtnClick(dicBtnItem[m_curSelectBtnItem], m_curSelectBtnItem, null);
        }
    }
    // khởi tạo ds btn tab
    private void InitListBtn()
    {
        //if(Data_Pool.m_ShopEventTimePool.IsOpen())// quà hạn giờ
        //    listBtnItem.Add(new BtnItem(EM_SupermarketType.AwardTimeLimit, f_GetActName(EM_GameNameParamType.AwardTimeLimit)));
        //// quà ưu đãi
        //listBtnItem.Add(new BtnItem(EM_SupermarketType.AwardEndow, f_GetActName(EM_GameNameParamType.AwardEndow)));
        if (Data_Pool.m_ShopEventTimePool.IsOpen())// quà hạn giờ
            listBtnItem.Add(new BtnItem(EM_SupermarketType.AwardTimeLimit, "Quà Hạn Giờ"));
        // quà ưu đãi
        listBtnItem.Add(new BtnItem(EM_SupermarketType.AwardEndow, "Quà Ưu Đãi"));
        listBtnItem.Add(new BtnItem(EM_SupermarketType.AwardSeason, "Gói Ưu Đãi Mùa"));
        listBtnItem.Add(new BtnItem(EM_SupermarketType.MonthCard, CommonTools.f_GetTransLanguage(1287)));
        listBtnItem.Add(new BtnItem(EM_SupermarketType.OpenServFund, CommonTools.f_GetTransLanguage(1285)));
        if (_CheckOpenTime(EM_SupermarketType.DealsEveryDay))
        {
            listBtnItem.Add(new BtnItem(EM_SupermarketType.DealsEveryDay, f_GetActName(EM_GameNameParamType.DealsEveryDay)));
        }
        if(Data_Pool.m_WeekFundPool.f_GetCurDay() >= 0)
        {
            listBtnItem.Add(new BtnItem(EM_SupermarketType.WeekFund, CommonTools.f_GetTransLanguage(1292)));
        }
    }
    private void InitMoneyUI()
    {
        List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
        listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Money);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Energy);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, listMoneyType);
    }

    //private BtnItem GetBtnItemByActType(EM_SupermarketType actType)//-----------------------case 1
    //{
    //    switch (actType)// quà hạn giờ, quà ưu đãi
    //    {
    //        case EM_SupermarketType.AwardTimeLimit: return new BtnItem(EM_SupermarketType.AwardTimeLimit, f_GetActName(EM_GameNameParamType.AwardTimeLimit));
    //        case EM_SupermarketType.AwardEndow: return new BtnItem(EM_SupermarketType.AwardEndow, f_GetActName(EM_GameNameParamType.AwardEndow));
    //    }

    //    return null;
    //}
    private string f_GetActName(EM_GameNameParamType eM_GameNameParamType)
    {
        GameNameParamDT gameParam = glo_Main.GetInstance().m_SC_Pool.m_GameNameParamSC.f_GetSC((int)eM_GameNameParamType) as GameNameParamDT;
        return gameParam.szParam1;
    }
    private void ShowActDefault(EM_SupermarketType em_Page)
    {
        for (int i = 0; i < listBtnItem.Count; i++)
        {
            if (listBtnItem[i].m_Type == em_Page)
            {
                OnBtnClick(dicBtnItem[listBtnItem[i]], listBtnItem[i], null);
                UIProgressBar uiProgressBar = f_GetObject("ProgressBar").transform.GetComponent<UIProgressBar>();
                return;
            }
        }
    }


    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        OnPageResume(e);
        InitMoneyUI();
        UpdateReddotUI();
    }

    protected override void f_InitMessage()
    {
        base.f_InitMessage();

        f_RegClickEvent("BtnReturn", OnBtnReturnClick);
       
        Vector3 posItemRoot = ItemRoot.transform.localPosition;
        posItemRoot.x = 360 / 2 * ScreenControl.Instance.mFunctionRatio;//360为左侧菜单宽度
        float windowAspect = (float)Screen.width / (float)Screen.height;
        ItemRoot.transform.localPosition = posItemRoot;
    }
    protected override void InitRaddot()
    {
        base.InitRaddot();
        if (dicTypeToBtnItem.Count == 0)
            return;
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.ActOpenServFund, dicTypeToBtnItem[EM_SupermarketType.OpenServFund], ReddotCallback_Show_Btn_OpenServFund);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.MonthCardGet, dicTypeToBtnItem[EM_SupermarketType.MonthCard], ReddotCallback_Show_Btn_MonthCard);
        if (dicTypeToBtnItem.ContainsKey(EM_SupermarketType.DealsEveryDay))
            Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.DealsEveryDay, dicTypeToBtnItem[EM_SupermarketType.DealsEveryDay], ReddotCallback_Show_Btn_DealsEveryDay);
        UpdateReddotUI();
    }
    private void ReddotCallback_Show_Btn_MonthCard(object Obj)
    {
        int iNum = (int)Obj;
        GameObject Btn_MonthCard = dicTypeToBtnItem[EM_SupermarketType.MonthCard];
        Btn_MonthCard.transform.Find("Reddot").gameObject.SetActive(iNum > 0 ? true : false);
    }
    private void ReddotCallback_Show_Btn_OpenServFund(object Obj)
    {
        int iNum = (int)Obj;
        GameObject Btn_OpenServFund = dicTypeToBtnItem[EM_SupermarketType.OpenServFund];
        Btn_OpenServFund.transform.Find("Reddot").gameObject.SetActive(iNum > 0 ? true : false);
    }
    private void ReddotCallback_Show_Btn_DealsEveryDay(object Obj)
    {
        int iNum = (int)Obj;
        if (dicTypeToBtnItem.ContainsKey(EM_SupermarketType.DealsEveryDay))
        {
            GameObject Btn_DealsEveryDay = dicTypeToBtnItem[EM_SupermarketType.DealsEveryDay];
            if (Btn_DealsEveryDay != null)
                Btn_DealsEveryDay.transform.Find("Reddot").gameObject.SetActive(iNum > 0 ? true : false);
        }
    }
    // create btn tab
    private void InitBtnUI()
    {
        List<BasePoolDT<long>> listBtnContent = new List<BasePoolDT<long>>();
        for (int i = 0; i < listBtnItem.Count; i++)
        {
            BasePoolDT<long> item = new BasePoolDT<long>();
            item.iId = i;
            listBtnContent.Add(item);
        }
        GridUtil.f_SetGridView<BtnItem>(f_GetObject("BtnItemParent"), f_GetObject("BtnItemExample"), listBtnItem, OnShowBtnItem);
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
            dicTypeToBtnItem.Add(btnItem.m_Type, item.gameObject);
        }
        item.transform.Find("BtnBg").gameObject.SetActive(!(m_curSelectBtnItem == btnItem));
        item.transform.Find("BtnBgDown").gameObject.SetActive(m_curSelectBtnItem == btnItem);
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

    private void OnBtnReturnClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SupermarketPage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
    }
    private GameObject f_LoadViewContent(EM_SupermarketType actType, string viewContentPath, GameObject objParent)
    {
        if (dicTypeToContentObj.ContainsKey(actType))
            return null;
        GameObject oriObj = Resources.Load<GameObject>(viewContentPath);
        ScreenAdaptive.Type adaptype = ScreenAdaptive.Type.Function;
        if (oriObj.GetComponent<ScreenAdaptive>() != null)
            adaptype = oriObj.GetComponent<ScreenAdaptive>().m_type;

        GameObject ViewContent = GameObject.Instantiate(oriObj) as GameObject;
        Destroy(ViewContent.GetComponent<ScreenAdaptive>());
        ViewContent.transform.SetParent(objParent.transform);
        ViewContent.transform.localPosition = Vector3.zero;
        ViewContent.transform.localEulerAngles = Vector3.zero;
        ViewContent.transform.localScale = Vector3.one;
        ViewContent.gameObject.SetActive(true);
        ViewContent.AddComponent<ScreenAdaptive>().m_type = adaptype;
        dicTypeToContentObj.Add(actType, ViewContent);
        return ViewContent;
    }
    private void UpdateContentData(BtnItem item)
    {
        EM_SupermarketType type = item.m_Type;
        if (dicTypeToContentObj.ContainsKey(EM_SupermarketType.AwardTimeLimit))
            dicTypeToContentObj[EM_SupermarketType.AwardTimeLimit].GetComponent<AwardTimeLimitCtl>().f_DestoryView();
        if (dicTypeToContentObj.ContainsKey(EM_SupermarketType.AwardEndow))
            dicTypeToContentObj[EM_SupermarketType.AwardEndow].GetComponent<AwardEndowCtl>().f_DestoryView();
        if (dicTypeToContentObj.ContainsKey(EM_SupermarketType.AwardSeason))
            dicTypeToContentObj[EM_SupermarketType.AwardSeason].GetComponent<AwardSeasonCtl>().f_DestoryView();

        if (dicTypeToContentObj.ContainsKey(EM_SupermarketType.OpenServFund))
            dicTypeToContentObj[EM_SupermarketType.OpenServFund].GetComponent<ActOpenServFundCtl>().f_DestoryView();
        if (dicTypeToContentObj.ContainsKey(EM_SupermarketType.MonthCard))
            dicTypeToContentObj[EM_SupermarketType.MonthCard].GetComponent<ActMonthCardCtl>().f_DestoryView();
        if (dicTypeToContentObj.ContainsKey(EM_SupermarketType.DealsEveryDay))
            dicTypeToContentObj[EM_SupermarketType.DealsEveryDay].GetComponent<DealsEveryDay>().f_DestoryView();
		if(dicTypeToContentObj.ContainsKey(EM_SupermarketType.WeekFund))
            dicTypeToContentObj[EM_SupermarketType.WeekFund].GetComponent<WeekFundCtl>().f_DestoryView();
        switch (type)
        {
            case EM_SupermarketType.AwardTimeLimit://红包兑换
                f_LoadViewContent(EM_SupermarketType.AwardTimeLimit, "UI/UIPrefab/GameMain/Supermarket/AwardTimeLimit", ItemRoot);
                dicTypeToContentObj[EM_SupermarketType.AwardTimeLimit].GetComponent<AwardTimeLimitCtl>().f_ShowView(this);
                break;
            case EM_SupermarketType.AwardEndow:
                f_LoadViewContent(EM_SupermarketType.AwardEndow, "UI/UIPrefab/GameMain/Supermarket/AwardEndow", ItemRoot);
                dicTypeToContentObj[EM_SupermarketType.AwardEndow].GetComponent<AwardEndowCtl>().f_ShowView(this);
                break;
            case EM_SupermarketType.AwardSeason:
                f_LoadViewContent(EM_SupermarketType.AwardSeason, "UI/UIPrefab/GameMain/Supermarket/AwardSeason", ItemRoot);
                dicTypeToContentObj[EM_SupermarketType.AwardSeason].GetComponent<AwardSeasonCtl>().f_ShowView(this);
                break;
            //
            case EM_SupermarketType.MonthCard://月卡
                f_LoadViewContent(EM_SupermarketType.MonthCard, "UI/UIPrefab/GameMain/Activity/MonthCardExample", ItemRoot);
                dicTypeToContentObj[EM_SupermarketType.MonthCard].GetComponent<ActMonthCardCtl>().f_ShowView(this);
                break;
            case EM_SupermarketType.OpenServFund://开服基金
                f_LoadViewContent(EM_SupermarketType.OpenServFund, "UI/UIPrefab/GameMain/Activity/OpenServFundExample", ItemRoot);
                dicTypeToContentObj[EM_SupermarketType.OpenServFund].GetComponent<ActOpenServFundCtl>().f_ShowView();
                break;
            case EM_SupermarketType.DealsEveryDay:
                f_LoadViewContent(EM_SupermarketType.DealsEveryDay, "UI/UIPrefab/GameMain/NewYearAct/DealsEveryDay", ItemRoot);
                dicTypeToContentObj[EM_SupermarketType.DealsEveryDay].GetComponent<DealsEveryDay>().f_ShowView(this);
                break;
			case EM_SupermarketType.WeekFund://周基金
                f_LoadViewContent(EM_SupermarketType.WeekFund, "UI/UIPrefab/GameMain/Activity/WeekFundExample", ItemRoot);
                dicTypeToContentObj[EM_SupermarketType.WeekFund].GetComponent<WeekFundCtl>().f_ShowView(this);
                break;
        }
    }
    protected override void UpdateReddotUI()
    {
        base.UpdateReddotUI();
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.MonthCardGet);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.ActOpenServFund);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.DealsEveryDay);
    }
    protected override void On_Destory()
    {
        base.On_Destory();
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.MonthCardGet, dicTypeToBtnItem[EM_SupermarketType.MonthCard]);
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.ActOpenServFund, dicTypeToBtnItem[EM_SupermarketType.OpenServFund]);
        if (dicTypeToBtnItem.ContainsKey(EM_SupermarketType.DealsEveryDay))
            Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.DealsEveryDay, dicTypeToBtnItem[EM_SupermarketType.DealsEveryDay]);
    }
    private static bool _CheckOpenTime(EM_SupermarketType tEM_NewYearActType)
    {
        GameParamDT gameParam;
        int StarTime;
        int EndTime;
        int NowTime = GameSocket.GetInstance().f_GetServerTime();
        switch (tEM_NewYearActType)
        {
            case EM_SupermarketType.DealsEveryDay:
                return glo_Main.GetInstance().m_SC_Pool.m_NewYearDealsEveryDaySC.f_GetAll().Count > 0;
            default:
                break;
        }
        return false;
    }
}
