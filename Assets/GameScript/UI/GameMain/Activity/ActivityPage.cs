using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
using System;
/// <summary>
/// 活动页面
/// </summary>
public class ActivityPage : UIFramwork
{
    public GameObject ItemRoot;
    private static List<EM_ActivityType> listRedPointSortAct = new List<EM_ActivityType>();
    private List<ActivityBtnItem> listActivityBtnItem = new List<ActivityBtnItem>();
    private Dictionary<ActivityBtnItem, GameObject> dicAcBtnItem = new Dictionary<ActivityBtnItem, GameObject>();
    private Dictionary<EM_ActivityType, GameObject> dicAcTypeToBtnItem = new Dictionary<EM_ActivityType, GameObject>();
	//My Code
	GameParamDT AssetOpen2;
	//

    private Dictionary<EM_ActivityType, GameObject> dicActTypeToContentObj = new Dictionary<EM_ActivityType, GameObject>();
    private ActivityBtnItem m_curSelectActBtnItem = null;

    private OpenSkyFortuneTimeDT m_OpenSkyFortuneTimeDT;
    /// <summary>
    /// 活动红点分类
    /// </summary>
    /// <param name="actType">活动类型</param>
    public static void SortAct(EM_ActivityType actType)
    {
        if(!listRedPointSortAct.Contains(actType))
        {
            listRedPointSortAct.Insert(0, actType);
        }
        else
        {
            listRedPointSortAct.RemoveAt(listRedPointSortAct.IndexOf(actType));
            listRedPointSortAct.Insert(0, actType);
        }
    }
    /// <summary>
    /// 创建ActivityBtnItem
    /// </summary>
    /// <param name="actType">活动类型</param>
    /// <returns></returns>
    private ActivityBtnItem GetBtnItemByActType(EM_ActivityType actType)//--------------case1
    {
        switch(actType)
        {
            case EM_ActivityType.DaySignIn: return new ActivityBtnItem(EM_ActivityType.DaySignIn, CommonTools.f_GetTransLanguage(1276));
            case EM_ActivityType.GrandSignIn: return new ActivityBtnItem(EM_ActivityType.GrandSignIn, CommonTools.f_GetTransLanguage(1277));
            case EM_ActivityType.OnlineAward: return new ActivityBtnItem(EM_ActivityType.OnlineAward, CommonTools.f_GetTransLanguage(1278));
            case EM_ActivityType.SkyFortune:
                OpenSkyFortuneTimeDT tOpenSkyFortuneTimeDT;
                for(int i = 0; i < glo_Main.GetInstance().m_SC_Pool.m_OpenSkyFortuneTimeSC.f_GetAll().Count; i++)
                {
                    tOpenSkyFortuneTimeDT = glo_Main.GetInstance().m_SC_Pool.m_OpenSkyFortuneTimeSC.f_GetAll()[i] as OpenSkyFortuneTimeDT;
                    if(CommonTools.f_CheckTime(tOpenSkyFortuneTimeDT.iOpenTime.ToString(), tOpenSkyFortuneTimeDT.iEndTime.ToString()))
                    {
                        if(Data_Pool.m_SkyFortunePool.isInitServ)
                        {
                            m_OpenSkyFortuneTimeDT = tOpenSkyFortuneTimeDT;
                            return new ActivityBtnItem(EM_ActivityType.SkyFortune, CommonTools.f_GetTransLanguage(1279));
                        }
                    }
                }

                //int dayIndex = Data_Pool.m_SkyFortunePool.mOpenSevDay;
                //if (dayIndex == 2 || dayIndex == 8 || dayIndex == 15 || dayIndex == 30 || dayIndex == 45 || dayIndex == 60)
                //{
                //}
                break;
            case EM_ActivityType.Banquet: return new ActivityBtnItem(EM_ActivityType.Banquet, CommonTools.f_GetTransLanguage(1280));
            case EM_ActivityType.LuckySymbol: return new ActivityBtnItem(EM_ActivityType.LuckySymbol, CommonTools.f_GetTransLanguage(1281));
            case EM_ActivityType.WealthMan: return new ActivityBtnItem(EM_ActivityType.WealthMan, CommonTools.f_GetTransLanguage(1282));
            case EM_ActivityType.LoginGift:
                List<NBaseSCDT> listActLoginGiftDT = glo_Main.GetInstance().m_SC_Pool.m_ActLoginGiftSC.f_GetAll();
                for(int i = 0; i < listActLoginGiftDT.Count; i++)
                {
                    ActLoginGiftDT actLoginGiftDT = listActLoginGiftDT[i] as ActLoginGiftDT;
                    if(actLoginGiftDT.itype == 1 && CommonTools.f_CheckTime(GetTimeByTimeStr(actLoginGiftDT.szStartTime), GetTimeByTimeStr(actLoginGiftDT.szEndTime)))//需判断时间是否到
                    {
                        return new ActivityBtnItem(EM_ActivityType.LoginGift, CommonTools.f_GetTransLanguage(1283));
                    }
                }
                break;
            case EM_ActivityType.LoginGiftNewServ:
                //新服豪礼 1-8天
                if(Data_Pool.m_ActivityCommonData.IsOpenNewSvrGift())
                    return new ActivityBtnItem(EM_ActivityType.LoginGiftNewServ, CommonTools.f_GetTransLanguage(1284));
                break;
            //case EM_ActivityType.OpenServFund: return new ActivityBtnItem(EM_ActivityType.OpenServFund, CommonTools.f_GetTransLanguage(1285));
            case EM_ActivityType.OpenWelfare: return new ActivityBtnItem(EM_ActivityType.OpenWelfare, CommonTools.f_GetTransLanguage(1286));
            //case EM_ActivityType.MonthCard: return new ActivityBtnItem(EM_ActivityType.MonthCard, CommonTools.f_GetTransLanguage(1287));
            case EM_ActivityType.ExchangeCode: return new ActivityBtnItem(EM_ActivityType.ExchangeCode, CommonTools.f_GetTransLanguage(1288));
            case EM_ActivityType.TenSycee:
                if(Data_Pool.m_TenSyceePool.f_CheckOpen())
                {
                    return new ActivityBtnItem(EM_ActivityType.TenSycee, CommonTools.f_GetTransLanguage(1289));
                }
                break;
            case EM_ActivityType.VipGift: return new ActivityBtnItem(EM_ActivityType.VipGift, CommonTools.f_GetTransLanguage(1290));
            //case EM_ActivityType.FirstRecharge:
           //     if(Data_Pool.m_FirstRechargePool.f_CheckIsOpen())
             //   {
            //        return new ActivityBtnItem(EM_ActivityType.FirstRecharge, CommonTools.f_GetTransLanguage(1291));
            //    }
            //    break;
            case EM_ActivityType.WeekFund:
                if(Data_Pool.m_WeekFundPool.f_GetCurDay() >= 0)
                {
                    //return new ActivityBtnItem(EM_ActivityType.WeekFund, CommonTools.f_GetTransLanguage(1292));
                }
                break;
            case EM_ActivityType.ExclusionSpin:
                if (_CheckOpenTime(EM_ActivityType.ExclusionSpin))
                {
                    return new ActivityBtnItem(EM_ActivityType.ExclusionSpin, "Vòng Quay Loại Trừ");
                }
                break;
        }
        return null;
    }
    private string f_GetActName(EM_GameNameParamType eM_GameNameParamType)
    {
        GameNameParamDT gameParam = glo_Main.GetInstance().m_SC_Pool.m_GameNameParamSC.f_GetSC((int)eM_GameNameParamType) as GameNameParamDT;
        return gameParam.szParam1;
    }
    /// <summary>
    /// 往list添加ActBtn
    /// </summary>
    /// <param name="actType">活动类型</param>
    private void AddActItem(EM_ActivityType actType)
    {
        ActivityBtnItem item = GetBtnItemByActType(actType);
        if(item != null)
            listActivityBtnItem.Add(item);
    }
    protected override void UI_OPEN(object e)//--------------case2（有红点的先显示）
    {
        base.UI_OPEN(e);
        dicAcBtnItem.Clear();
        dicAcTypeToBtnItem.Clear();
        listActivityBtnItem.Clear();
        for(int i = 0; i < listRedPointSortAct.Count; i++)
        {
            AddActItem(listRedPointSortAct[i]);
        }
		AssetOpen2 = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(111) as GameParamDT);
        //if(!listRedPointSortAct.Contains(EM_ActivityType.FirstRecharge))
        //    AddActItem(EM_ActivityType.FirstRecharge);
	
		float windowAspect = (float)Screen.width /  (float) Screen.height ;
		MessageBox.ASSERT("" + windowAspect);
		// if(windowAspect >= 1.9)
		// {
			// f_GetObject("SideBtn").transform.localPosition = new Vector3(120f, 0f, 0f);
		// }
	
        if(!listRedPointSortAct.Contains(EM_ActivityType.GrandSignIn))
            AddActItem(EM_ActivityType.GrandSignIn);
        if(!listRedPointSortAct.Contains(EM_ActivityType.VipGift))
            AddActItem(EM_ActivityType.VipGift);
        //if(!listRedPointSortAct.Contains(EM_ActivityType.MonthCard))
        //    AddActItem(EM_ActivityType.MonthCard);
        //if(!listRedPointSortAct.Contains(EM_ActivityType.OpenServFund))
        //    AddActItem(EM_ActivityType.OpenServFund);
        if(!listRedPointSortAct.Contains(EM_ActivityType.OpenWelfare))
            AddActItem(EM_ActivityType.OpenWelfare);
        if(!listRedPointSortAct.Contains(EM_ActivityType.SkyFortune))
            AddActItem(EM_ActivityType.SkyFortune);
        if(!listRedPointSortAct.Contains(EM_ActivityType.TenSycee))
            AddActItem(EM_ActivityType.TenSycee);
        if(!listRedPointSortAct.Contains(EM_ActivityType.WealthMan))
            AddActItem(EM_ActivityType.WealthMan);

        if(!listRedPointSortAct.Contains(EM_ActivityType.DaySignIn))
            AddActItem(EM_ActivityType.DaySignIn);
        if(!listRedPointSortAct.Contains(EM_ActivityType.OnlineAward))
            AddActItem(EM_ActivityType.OnlineAward);
        if(!listRedPointSortAct.Contains(EM_ActivityType.Banquet))
            AddActItem(EM_ActivityType.Banquet);
        if(!listRedPointSortAct.Contains(EM_ActivityType.LuckySymbol))
            AddActItem(EM_ActivityType.LuckySymbol);
        if(!listRedPointSortAct.Contains(EM_ActivityType.ExchangeCode) && (AssetOpen2.iParam1 == 1 && Data_Pool.m_OnlineAwardPool.m_timeSecondToday >= AssetOpen2.iParam2))
            AddActItem(EM_ActivityType.ExchangeCode);
        if(!listRedPointSortAct.Contains(EM_ActivityType.LoginGift))
            AddActItem(EM_ActivityType.LoginGift);
        if(!listRedPointSortAct.Contains(EM_ActivityType.LoginGiftNewServ))
            AddActItem(EM_ActivityType.LoginGiftNewServ);

        if(!listRedPointSortAct.Contains(EM_ActivityType.WeekFund))
            AddActItem(EM_ActivityType.WeekFund);
        
       
        if (!listRedPointSortAct.Contains(EM_ActivityType.ExclusionSpin) && _CheckOpenTime(EM_ActivityType.ExclusionSpin))    //兑换
        {
            AddActItem(EM_ActivityType.ExclusionSpin);
        }
        InitBtnUI();
        if(e == null)
        {
            //默认选中第一个按钮
            ShowActDefault(listActivityBtnItem[0].m_ActivityType);
        }
        else
        {
            ShowActDefault((EM_ActivityType)e);
        }
        InitMoneyUI();
        InitRaddot();
    }

    /// <summary>
    /// 初始化金钱UI
    /// </summary>
    private void InitMoneyUI()
    {
        List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
        listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Money);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Energy);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, listMoneyType);
    }
    /// <summary>
    /// 显示某个活动选中状态
    /// </summary>
    /// <param name="em_ActivityPage">活动类型</param>
    private void ShowActDefault(EM_ActivityType em_ActivityPage)
    {
        for(int i = 0; i < listActivityBtnItem.Count; i++)
        {
            if(listActivityBtnItem[i].m_ActivityType == em_ActivityPage)
            {
                OnActBtnClick(dicAcBtnItem[listActivityBtnItem[i]], listActivityBtnItem[i], null);
                UIProgressBar uiProgressBar = f_GetObject("ProgressBar").transform.GetComponent<UIProgressBar>();
                uiProgressBar.value = i * 1.0f / (listActivityBtnItem.Count - 1);
                return;
            }
        }
    }
    /// <summary>
    /// 通过字符串获取日期
    /// </summary>
    /// <param name="StrTime"></param>
    /// <returns></returns>
    private DateTime GetTimeByTimeStr(string StrTime)
    {
        string[] timeArray = StrTime.Split(';');
        DateTime time = new DateTime(int.Parse(timeArray[0]), int.Parse(timeArray[1]), int.Parse(timeArray[2]), int.Parse(timeArray[3]),
            int.Parse(timeArray[4]), int.Parse(timeArray[5]));
        return time;
    }
    protected override void UI_HOLD(object e)
    {
        base.UI_HOLD(e);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
    }
    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        InitMoneyUI();
        OnPageResume(e);
        UpdateReddotUI();
    }
    /// <summary>
    /// 页面恢复
    /// </summary>
    /// <param name="e"></param>
    private void OnPageResume(object e)
    {
        if(this.f_IsOpen())
        {
            //充值页面返回刷新界面
            if(dicActTypeToContentObj.ContainsKey(EM_ActivityType.GrandSignIn))
                dicActTypeToContentObj[EM_ActivityType.GrandSignIn].GetComponent<ActGrandSignCtl>().f_ViewResume(e);
            if(dicActTypeToContentObj.ContainsKey(EM_ActivityType.MonthCard))
                dicActTypeToContentObj[EM_ActivityType.MonthCard].GetComponent<ActMonthCardCtl>().f_ViewResume(e);

            if(dicActTypeToContentObj.ContainsKey(EM_ActivityType.VipGift))
                dicActTypeToContentObj[EM_ActivityType.VipGift].GetComponent<VipGiftCtl>().f_ViewResume(e);
            if(dicActTypeToContentObj.ContainsKey(EM_ActivityType.WeekFund))
                dicActTypeToContentObj[EM_ActivityType.WeekFund].GetComponent<WeekFundCtl>().f_ViewResume(e);

            OnActBtnClick(dicAcBtnItem[m_curSelectActBtnItem], m_curSelectActBtnItem, null);
        }
    }
    private GameObject f_LoadViewContent(EM_ActivityType actType, string viewContentPath, GameObject objParent)
    {
        if(dicActTypeToContentObj.ContainsKey(actType))
            return null;
        GameObject oriObj = Resources.Load<GameObject>(viewContentPath);
        ScreenAdaptive.Type adaptype = ScreenAdaptive.Type.Function;
        if(oriObj.GetComponent<ScreenAdaptive>() != null)
            adaptype = oriObj.GetComponent<ScreenAdaptive>().m_type;

        GameObject ViewContent = GameObject.Instantiate(oriObj) as GameObject;
        Destroy(ViewContent.GetComponent<ScreenAdaptive>());
        ViewContent.transform.SetParent(objParent.transform);
        ViewContent.transform.localPosition = Vector3.zero;
        ViewContent.transform.localEulerAngles = Vector3.zero;
        ViewContent.transform.localScale = Vector3.one;
        ViewContent.gameObject.SetActive(true);
        ViewContent.AddComponent<ScreenAdaptive>().m_type = adaptype;
        dicActTypeToContentObj.Add(actType, ViewContent);
        return ViewContent;
    }
    /// <summary>
    /// 按钮内容数据更新
    /// </summary>
    private void UpdateContentData(ActivityBtnItem item)//--------------case3
    {
        EM_ActivityType actType = item.m_ActivityType;
        if(dicActTypeToContentObj.ContainsKey(EM_ActivityType.DaySignIn))
            dicActTypeToContentObj[EM_ActivityType.DaySignIn].GetComponent<ActUserSignCtl>().f_DestoryView();
        if(dicActTypeToContentObj.ContainsKey(EM_ActivityType.GrandSignIn))
            dicActTypeToContentObj[EM_ActivityType.GrandSignIn].GetComponent<ActGrandSignCtl>().f_DestoryView();

        if(dicActTypeToContentObj.ContainsKey(EM_ActivityType.Banquet))
            dicActTypeToContentObj[EM_ActivityType.Banquet].GetComponent<ActBanQuetCtl>().f_DestoryView();
        if(dicActTypeToContentObj.ContainsKey(EM_ActivityType.LuckySymbol))
            dicActTypeToContentObj[EM_ActivityType.LuckySymbol].GetComponent<ActLuckySymbolCtl>().f_DestoryView();
        if(dicActTypeToContentObj.ContainsKey(EM_ActivityType.WealthMan))
            dicActTypeToContentObj[EM_ActivityType.WealthMan].GetComponent<ActWealthManCtl>().f_DestoryView();

        if(dicActTypeToContentObj.ContainsKey(EM_ActivityType.LoginGift))
            dicActTypeToContentObj[EM_ActivityType.LoginGift].GetComponent<ActLoginGiftCtl>().f_DestoryView();
        if(dicActTypeToContentObj.ContainsKey(EM_ActivityType.LoginGiftNewServ))
            dicActTypeToContentObj[EM_ActivityType.LoginGiftNewServ].GetComponent<ActLoginGiftNewServCtl>().f_DestoryView();
        //if(dicActTypeToContentObj.ContainsKey(EM_ActivityType.OpenServFund))
        //    dicActTypeToContentObj[EM_ActivityType.OpenServFund].GetComponent<ActOpenServFundCtl>().f_DestoryView();
        if(dicActTypeToContentObj.ContainsKey(EM_ActivityType.OpenWelfare))
            dicActTypeToContentObj[EM_ActivityType.OpenWelfare].GetComponent<ActOpenWelfareCtl>().f_DestoryView();
        //if(dicActTypeToContentObj.ContainsKey(EM_ActivityType.MonthCard))
        //    dicActTypeToContentObj[EM_ActivityType.MonthCard].GetComponent<ActMonthCardCtl>().f_DestoryView();
        if(dicActTypeToContentObj.ContainsKey(EM_ActivityType.OnlineAward))
            dicActTypeToContentObj[EM_ActivityType.OnlineAward].GetComponent<OnlineAwardCtl>().f_DestoryView();
        if(dicActTypeToContentObj.ContainsKey(EM_ActivityType.SkyFortune))
            dicActTypeToContentObj[EM_ActivityType.SkyFortune].GetComponent<SkyFortuneCtl>().f_DestoryView();
        if(dicActTypeToContentObj.ContainsKey(EM_ActivityType.ExchangeCode))
            dicActTypeToContentObj[EM_ActivityType.ExchangeCode].GetComponent<ExchangeCodeCtl>().f_DestoryView();
        if(dicActTypeToContentObj.ContainsKey(EM_ActivityType.TenSycee))
            dicActTypeToContentObj[EM_ActivityType.TenSycee].GetComponent<TenSyceeCtl>().f_DestoryView();
        if(dicActTypeToContentObj.ContainsKey(EM_ActivityType.VipGift))
            dicActTypeToContentObj[EM_ActivityType.VipGift].GetComponent<VipGiftCtl>().f_DestoryView();
        //if(dicActTypeToContentObj.ContainsKey(EM_ActivityType.FirstRecharge))
        //    dicActTypeToContentObj[EM_ActivityType.FirstRecharge].GetComponent<FirstRechargeCtl>().f_DestoryView();
        if(dicActTypeToContentObj.ContainsKey(EM_ActivityType.WeekFund))
            dicActTypeToContentObj[EM_ActivityType.WeekFund].GetComponent<WeekFundCtl>().f_DestoryView();
        if (dicActTypeToContentObj.ContainsKey(EM_ActivityType.ExclusionSpin))
            dicActTypeToContentObj[EM_ActivityType.ExclusionSpin].GetComponent<ExclusionSpin>().f_DestoryView();
        switch (actType)
        {
            case EM_ActivityType.DaySignIn://每日签到
                f_LoadViewContent(EM_ActivityType.DaySignIn, "UI/UIPrefab/GameMain/Activity/UserSignExample", ItemRoot);
                dicActTypeToContentObj[EM_ActivityType.DaySignIn].GetComponent<ActUserSignCtl>().f_ShowView();
                break;
            case EM_ActivityType.GrandSignIn://豪华签到
                f_LoadViewContent(EM_ActivityType.GrandSignIn, "UI/UIPrefab/GameMain/Activity/GrandSignExample", ItemRoot);
                dicActTypeToContentObj[EM_ActivityType.GrandSignIn].GetComponent<ActGrandSignCtl>().f_ShowView(this);
                break;
            case EM_ActivityType.Banquet://铜雀台(已改名宫宴)
                f_LoadViewContent(EM_ActivityType.Banquet, "UI/UIPrefab/GameMain/Activity/ContentBanQuetObjExample", ItemRoot);
                dicActTypeToContentObj[EM_ActivityType.Banquet].GetComponent<ActBanQuetCtl>().f_ShowView();
                break;
            case EM_ActivityType.LuckySymbol://招财符
                f_LoadViewContent(EM_ActivityType.LuckySymbol, "UI/UIPrefab/GameMain/Activity/ActLuckySymbolExample", ItemRoot);
                dicActTypeToContentObj[EM_ActivityType.LuckySymbol].GetComponent<ActLuckySymbolCtl>().f_ShowView();
                break;
            case EM_ActivityType.WealthMan://迎财神
                f_LoadViewContent(EM_ActivityType.WealthMan, "UI/UIPrefab/GameMain/Activity/WealthManExample", ItemRoot);
                dicActTypeToContentObj[EM_ActivityType.WealthMan].GetComponent<ActWealthManCtl>().f_ShowView();
                break;
            case EM_ActivityType.LoginGift://登录送礼
                f_LoadViewContent(EM_ActivityType.LoginGift, "UI/UIPrefab/GameMain/Activity/LoginGiftExample", ItemRoot);
                dicActTypeToContentObj[EM_ActivityType.LoginGift].GetComponent<ActLoginGiftCtl>().f_ShowView();
                break;
            case EM_ActivityType.LoginGiftNewServ://登录送礼（新服豪礼）
                f_LoadViewContent(EM_ActivityType.LoginGiftNewServ, "UI/UIPrefab/GameMain/Activity/LoginGiftNewServExample", ItemRoot);
                dicActTypeToContentObj[EM_ActivityType.LoginGiftNewServ].GetComponent<ActLoginGiftNewServCtl>().f_ShowView();
                break;
            //case EM_ActivityType.OpenServFund://开服基金
            //    f_LoadViewContent(EM_ActivityType.OpenServFund, "UI/UIPrefab/GameMain/Activity/OpenServFundExample", ItemRoot);
            //    dicActTypeToContentObj[EM_ActivityType.OpenServFund].GetComponent<ActOpenServFundCtl>().f_ShowView();
            //    break;
            case EM_ActivityType.OpenWelfare://全民福利
                f_LoadViewContent(EM_ActivityType.OpenWelfare, "UI/UIPrefab/GameMain/Activity/OpenWelfareExample", ItemRoot);
                dicActTypeToContentObj[EM_ActivityType.OpenWelfare].GetComponent<ActOpenWelfareCtl>().f_ShowView(OnOpenWelfareGotoCallback);
                break;
            //case EM_ActivityType.MonthCard://月卡
            //    f_LoadViewContent(EM_ActivityType.MonthCard, "UI/UIPrefab/GameMain/Activity/MonthCardExample", ItemRoot);
            //    dicActTypeToContentObj[EM_ActivityType.MonthCard].GetComponent<ActMonthCardCtl>().f_ShowView(this);
            //    break;
            case EM_ActivityType.OnlineAward://在线豪礼
                f_LoadViewContent(EM_ActivityType.OnlineAward, "UI/UIPrefab/GameMain/Activity/OnlineAwardExample", ItemRoot);
                dicActTypeToContentObj[EM_ActivityType.OnlineAward].GetComponent<OnlineAwardCtl>().f_ShowView();
                break;
            case EM_ActivityType.SkyFortune://天降横财
                f_LoadViewContent(EM_ActivityType.SkyFortune, "UI/UIPrefab/GameMain/Activity/SkyFortuneExample", ItemRoot);
                dicActTypeToContentObj[EM_ActivityType.SkyFortune].GetComponent<SkyFortuneCtl>().f_ShowView(m_OpenSkyFortuneTimeDT);
                break;
            case EM_ActivityType.ExchangeCode://兑换码
                f_LoadViewContent(EM_ActivityType.ExchangeCode, "UI/UIPrefab/GameMain/Activity/ExchangeCodeExample", ItemRoot);
                dicActTypeToContentObj[EM_ActivityType.ExchangeCode].GetComponent<ExchangeCodeCtl>().f_ShowView();
                break;
            case EM_ActivityType.TenSycee://十万元宝
                f_LoadViewContent(EM_ActivityType.TenSycee, "UI/UIPrefab/GameMain/Activity/TenSyceeExample", ItemRoot);
                dicActTypeToContentObj[EM_ActivityType.TenSycee].GetComponent<TenSyceeCtl>().f_ShowView();
                break;
            case EM_ActivityType.VipGift://vip礼包
                f_LoadViewContent(EM_ActivityType.VipGift, "UI/UIPrefab/GameMain/Activity/VipGiftExample", ItemRoot);
                dicActTypeToContentObj[EM_ActivityType.VipGift].GetComponent<VipGiftCtl>().f_ShowView(this);
                break;
            //case EM_ActivityType.FirstRecharge://首充
            //    f_LoadViewContent(EM_ActivityType.FirstRecharge, "UI/UIPrefab/GameMain/Activity/FirstRechargeExample", ItemRoot);
            //    dicActTypeToContentObj[EM_ActivityType.FirstRecharge].GetComponent<FirstRechargeCtl>().f_ShowView(this);
            //    break;
            case EM_ActivityType.WeekFund://周基金
                f_LoadViewContent(EM_ActivityType.WeekFund, "UI/UIPrefab/GameMain/Activity/WeekFundExample", ItemRoot);
                dicActTypeToContentObj[EM_ActivityType.WeekFund].GetComponent<WeekFundCtl>().f_ShowView(this);
                break;
            case EM_ActivityType.ExclusionSpin:
                f_LoadViewContent(EM_ActivityType.ExclusionSpin, "UI/UIPrefab/GameMain/NewYearAct/ExclusionSpin", ItemRoot);
                dicActTypeToContentObj[EM_ActivityType.ExclusionSpin].GetComponent<ExclusionSpin>().f_ShowView(this);
                break;
        }
    }
    /// <summary>
    /// 全民福利前往按钮事件
    /// </summary>
    private void OnOpenWelfareGotoCallback(object data)
    {
        for(int i = 0; i < listActivityBtnItem.Count; i++)
        {
            ActivityBtnItem activityItem = listActivityBtnItem[i] as ActivityBtnItem;
            if(activityItem.m_ActivityType == EM_ActivityType.OpenServFund)
            {
                OnActBtnClick(dicAcBtnItem[activityItem], activityItem, null);
                break;
            }
        }
    }
    /// <summary>
    /// 活动按钮被点击
    /// </summary>
    private void OnActBtnClick(GameObject go, object obj1, object obj2)
    {
        ActivityBtnItem item = (ActivityBtnItem)obj1;
        m_curSelectActBtnItem = item;
        CloseAllBtnPressEffect();
        go.transform.Find("BtnBg").gameObject.SetActive(false);
        go.transform.Find("BtnBgDown").gameObject.SetActive(true);
        UpdateContentData(item);
    }
    /// <summary>
    /// 初始化消息
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();

        f_RegClickEvent("BtnReturn", OnBtnReturnClick);

        Vector3 posItemRoot = ItemRoot.transform.localPosition;
        posItemRoot.x = 360 / 2 * ScreenControl.Instance.mFunctionRatio;//360为左侧菜单宽度
		float windowAspect = (float)Screen.width /  (float) Screen.height ;
        ItemRoot.transform.localPosition = posItemRoot;

        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.TheNextDay, NextDay);
        //365
    }

    private void NextDay(object obj)
    {
        //刷新活动
        if (null == m_curSelectActBtnItem || null == m_Panel || !m_Panel.gameObject.activeInHierarchy)
            return;
        ShowActDefault(m_curSelectActBtnItem.m_ActivityType);
    }

    /// <summary>
    /// 关闭所有按钮选项
    /// </summary>
    private void CloseAllBtnPressEffect()
    {
        for(int i = 0; i < listActivityBtnItem.Count; i++)
        {
            if(dicAcBtnItem.ContainsKey(listActivityBtnItem[i]))
            {
                dicAcBtnItem[listActivityBtnItem[i]].transform.Find("BtnBg").gameObject.SetActive(true);
                dicAcBtnItem[listActivityBtnItem[i]].transform.Find("BtnBgDown").gameObject.SetActive(false);
            }
        }
    }
    #region 按钮相关
    /// <summary>
    /// 点击返回按钮
    /// </summary>
    private void OnBtnReturnClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ActivityPage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
    }
    #endregion
    private void InitBtnUI()
    {
        List<BasePoolDT<long>> listBtnContent = new List<BasePoolDT<long>>();
        for(int i = 0; i < listActivityBtnItem.Count; i++)
        {
            BasePoolDT<long> item = new BasePoolDT<long>();
            item.iId = i;
            listBtnContent.Add(item);
        }
        GridUtil.f_SetGridView<ActivityBtnItem>(f_GetObject("ActBtnItemParent"), f_GetObject("ActBtnItemExample"), listActivityBtnItem, OnShowActBtnItem);
        f_GetObject("ActBtnItemParent").transform.GetComponentInParent<UIScrollView>().ResetPosition();
    }
    #region 红点提示 //---------------case 4
    protected override void InitRaddot()
    {
        base.InitRaddot();
        if(dicAcTypeToBtnItem.Count == 0)//没有初始化，不执行
            return;
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.UserSign, dicAcTypeToBtnItem[EM_ActivityType.DaySignIn], ReddotCallback_Show_Btn_DaySignIn);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.GrandSign, dicAcTypeToBtnItem[EM_ActivityType.GrandSignIn], ReddotCallback_Show_Btn_GrandSignIn);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.BanQuet, dicAcTypeToBtnItem[EM_ActivityType.Banquet], ReddotCallback_Show_Btn_Banquet);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.LuckySymbolFree, dicAcTypeToBtnItem[EM_ActivityType.LuckySymbol], ReddotCallback_Show_Btn_LuckySymbol);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.WealthMan, dicAcTypeToBtnItem[EM_ActivityType.WealthMan], ReddotCallback_Show_Btn_WealthMan);
        if(dicAcTypeToBtnItem.ContainsKey(EM_ActivityType.LoginGift))
            Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.LoginGift, dicAcTypeToBtnItem[EM_ActivityType.LoginGift], ReddotCallback_Show_Btn_LoginGift);
        if(dicAcTypeToBtnItem.ContainsKey(EM_ActivityType.LoginGiftNewServ))
            Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.LoginGiftNewServ, dicAcTypeToBtnItem[EM_ActivityType.LoginGiftNewServ], ReddotCallback_Show_Btn_LoginGiftNewServ);
        //Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.MonthCardGet, dicAcTypeToBtnItem[EM_ActivityType.MonthCard], ReddotCallback_Show_Btn_MonthCard);
        //Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.ActOpenServFund, dicAcTypeToBtnItem[EM_ActivityType.OpenServFund], ReddotCallback_Show_Btn_OpenServFund);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.ActOpenWelfare, dicAcTypeToBtnItem[EM_ActivityType.OpenWelfare], ReddotCallback_Show_Btn_OpenWelfare);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.OnlineAward, dicAcTypeToBtnItem[EM_ActivityType.OnlineAward], ReddotCallback_Show_Btn_OnlineAward);
        if(dicAcTypeToBtnItem.ContainsKey(EM_ActivityType.SkyFortune))
            Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.SkyFortune, dicAcTypeToBtnItem[EM_ActivityType.SkyFortune], ReddotCallback_Show_Btn_SkyFortune);
        if(dicAcTypeToBtnItem.ContainsKey(EM_ActivityType.TenSycee))
            Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.TenSycee, dicAcTypeToBtnItem[EM_ActivityType.TenSycee], ReddotCallback_Show_Btn_TenSycee);
        if(dicAcTypeToBtnItem.ContainsKey(EM_ActivityType.VipGift))
            Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.VipGift, dicAcTypeToBtnItem[EM_ActivityType.VipGift], ReddotCallback_Show_Btn_VipGift);
        //if(dicAcTypeToBtnItem.ContainsKey(EM_ActivityType.FirstRecharge))
        //    Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.FirstRecharge, dicAcTypeToBtnItem[EM_ActivityType.FirstRecharge], ReddotCallback_Show_Btn_FirstRechage);
        if(dicAcTypeToBtnItem.ContainsKey(EM_ActivityType.WeekFund))
            Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.WeekFund, dicAcTypeToBtnItem[EM_ActivityType.WeekFund], ReddotCallback_Show_Btn_WeekFund);
        if (dicAcTypeToBtnItem.ContainsKey(EM_ActivityType.ExclusionSpin))
            Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.ExclusionSpin, dicAcTypeToBtnItem[EM_ActivityType.ExclusionSpin], ReddotCallback_Show_Btn_ExclusionSpin);
        UpdateReddotUI();
    }
    protected override void On_Destory()
    {
        base.On_Destory();
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.UserSign, dicAcTypeToBtnItem[EM_ActivityType.DaySignIn]);
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.GrandSign, dicAcTypeToBtnItem[EM_ActivityType.GrandSignIn]);
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.BanQuet, dicAcTypeToBtnItem[EM_ActivityType.Banquet]);
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.LuckySymbolFree, dicAcTypeToBtnItem[EM_ActivityType.LuckySymbol]);
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.WealthMan, dicAcTypeToBtnItem[EM_ActivityType.WealthMan]);
        if(dicAcTypeToBtnItem.ContainsKey(EM_ActivityType.LoginGift))
            Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.LoginGift, dicAcTypeToBtnItem[EM_ActivityType.LoginGift]);
        if(dicAcTypeToBtnItem.ContainsKey(EM_ActivityType.LoginGiftNewServ))
            Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.LoginGiftNewServ, dicAcTypeToBtnItem[EM_ActivityType.LoginGiftNewServ]);
        //Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.MonthCardGet, dicAcTypeToBtnItem[EM_ActivityType.MonthCard]);
        //Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.ActOpenServFund, dicAcTypeToBtnItem[EM_ActivityType.OpenServFund]);
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.ActOpenWelfare, dicAcTypeToBtnItem[EM_ActivityType.OpenWelfare]);
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.OnlineAward, dicAcTypeToBtnItem[EM_ActivityType.OnlineAward]);
        if(dicAcTypeToBtnItem.ContainsKey(EM_ActivityType.SkyFortune))
            Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.SkyFortune, dicAcTypeToBtnItem[EM_ActivityType.SkyFortune]);
        if(dicAcTypeToBtnItem.ContainsKey(EM_ActivityType.TenSycee))
            Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.TenSycee, dicAcTypeToBtnItem[EM_ActivityType.TenSycee]);
        if(dicAcTypeToBtnItem.ContainsKey(EM_ActivityType.VipGift))
            Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.VipGift, dicAcTypeToBtnItem[EM_ActivityType.VipGift]);
        //if(dicAcTypeToBtnItem.ContainsKey(EM_ActivityType.FirstRecharge))
        //    Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.FirstRecharge, dicAcTypeToBtnItem[EM_ActivityType.FirstRecharge]);
        if(dicAcTypeToBtnItem.ContainsKey(EM_ActivityType.WeekFund))
            Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.WeekFund, dicAcTypeToBtnItem[EM_ActivityType.WeekFund]);
        if (dicAcTypeToBtnItem.ContainsKey(EM_ActivityType.ExclusionSpin))
            Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.ExclusionSpin, dicAcTypeToBtnItem[EM_ActivityType.ExclusionSpin]);
        dicAcBtnItem.Clear();
        dicAcTypeToBtnItem.Clear();
        listActivityBtnItem.Clear();
    }
    protected override void UpdateReddotUI()
    {
        base.UpdateReddotUI();
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.UserSign);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.GrandSign);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.BanQuet);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.LuckySymbolFree);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.WealthMan);
        if(dicAcTypeToBtnItem.ContainsKey(EM_ActivityType.LoginGift))
            Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.LoginGift);
        if(dicAcTypeToBtnItem.ContainsKey(EM_ActivityType.LoginGiftNewServ))
            Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.LoginGiftNewServ);
        //Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.MonthCardGet);
        //Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.ActOpenServFund);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.ActOpenWelfare);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.OnlineAward);
        if(dicAcTypeToBtnItem.ContainsKey(EM_ActivityType.SkyFortune))
            Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.SkyFortune);
        if(dicAcTypeToBtnItem.ContainsKey(EM_ActivityType.TenSycee))
            Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.TenSycee);
        if(dicAcTypeToBtnItem.ContainsKey(EM_ActivityType.VipGift))
            Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.VipGift);
        //if(dicAcTypeToBtnItem.ContainsKey(EM_ActivityType.FirstRecharge))
        //    Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.FirstRecharge);
        if(dicAcTypeToBtnItem.ContainsKey(EM_ActivityType.WeekFund))
            Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.WeekFund);

        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.ExclusionSpin);
    }
    private void ReddotCallback_Show_Btn_DaySignIn(object Obj)
    {
        int iNum = (int)Obj;
        GameObject Btn_DaySignIn = dicAcTypeToBtnItem[EM_ActivityType.DaySignIn];
        Btn_DaySignIn.transform.Find("Reddot").gameObject.SetActive(iNum > 0 ? true : false);
    }
    private void ReddotCallback_Show_Btn_GrandSignIn(object Obj)
    {
        int iNum = (int)Obj;
        GameObject Btn_GrandSignIn = dicAcTypeToBtnItem[EM_ActivityType.GrandSignIn];
        Btn_GrandSignIn.transform.Find("Reddot").gameObject.SetActive(iNum > 0 ? true : false);
    }
    private void ReddotCallback_Show_Btn_Banquet(object Obj)
    {
        int iNum = (int)Obj;
        GameObject Btn_Banquet = dicAcTypeToBtnItem[EM_ActivityType.Banquet];
        Btn_Banquet.transform.Find("Reddot").gameObject.SetActive(iNum > 0 ? true : false);
    }
    private void ReddotCallback_Show_Btn_LuckySymbol(object Obj)
    {
        int iNum = (int)Obj;
        GameObject Btn_LuckySymbol = dicAcTypeToBtnItem[EM_ActivityType.LuckySymbol];
        Btn_LuckySymbol.transform.Find("Reddot").gameObject.SetActive(iNum > 0 ? true : false);
    }
    private void ReddotCallback_Show_Btn_WealthMan(object Obj)
    {
        int iNum = (int)Obj;
        GameObject Btn_WealthMan = dicAcTypeToBtnItem[EM_ActivityType.WealthMan];
        Btn_WealthMan.transform.Find("Reddot").gameObject.SetActive(iNum > 0 ? true : false);
    }
    private void ReddotCallback_Show_Btn_LoginGift(object Obj)
    {
        int iNum = (int)Obj;
        GameObject Btn_LoginGift = dicAcTypeToBtnItem[EM_ActivityType.LoginGift];
        Btn_LoginGift.transform.Find("Reddot").gameObject.SetActive(iNum > 0 ? true : false);
    }
    private void ReddotCallback_Show_Btn_LoginGiftNewServ(object Obj)
    {
        int iNum = (int)Obj;
        if (dicAcTypeToBtnItem.ContainsKey(EM_ActivityType.LoginGiftNewServ))
        {
            GameObject Btn_LoginGift = dicAcTypeToBtnItem[EM_ActivityType.LoginGiftNewServ];
            Btn_LoginGift.transform.Find("Reddot").gameObject.SetActive(iNum > 0 ? true : false);
        }
    }
    //private void ReddotCallback_Show_Btn_MonthCard(object Obj)
    //{
    //    int iNum = (int)Obj;
    //    GameObject Btn_MonthCard = dicAcTypeToBtnItem[EM_ActivityType.MonthCard];
    //    Btn_MonthCard.transform.Find("Reddot").gameObject.SetActive(iNum > 0 ? true : false);
    //}
    //private void ReddotCallback_Show_Btn_OpenServFund(object Obj)
    //{
    //    int iNum = (int)Obj;
    //    GameObject Btn_OpenServFund = dicAcTypeToBtnItem[EM_ActivityType.OpenServFund];
    //    Btn_OpenServFund.transform.Find("Reddot").gameObject.SetActive(iNum > 0 ? true : false);
    //}
    private void ReddotCallback_Show_Btn_OpenWelfare(object Obj)
    {
        int iNum = (int)Obj;
        GameObject Btn_OpenWelfare = dicAcTypeToBtnItem[EM_ActivityType.OpenWelfare];
        Btn_OpenWelfare.transform.Find("Reddot").gameObject.SetActive(iNum > 0 ? true : false);
    }
    private void ReddotCallback_Show_Btn_OnlineAward(object Obj)
    {
        int iNum = (int)Obj;
        GameObject Btn_OnlineAward = dicAcTypeToBtnItem[EM_ActivityType.OnlineAward];
        Btn_OnlineAward.transform.Find("Reddot").gameObject.SetActive(iNum > 0 ? true : false);
    }
    private void ReddotCallback_Show_Btn_SkyFortune(object Obj)
    {
        int iNum = (int)Obj;
        GameObject Btn_SkyFortune = dicAcTypeToBtnItem[EM_ActivityType.SkyFortune];
        Btn_SkyFortune.transform.Find("Reddot").gameObject.SetActive(iNum > 0 ? true : false);
    }
    private void ReddotCallback_Show_Btn_TenSycee(object Obj)
    {
        int iNum = (int)Obj;
        GameObject Btn_TenSycee = dicAcTypeToBtnItem[EM_ActivityType.TenSycee];
        Btn_TenSycee.transform.Find("Reddot").gameObject.SetActive(iNum > 0 ? true : false);
    }
    private void ReddotCallback_Show_Btn_VipGift(object Obj)
    {
        int iNum = (int)Obj;
        GameObject Btn_VipGift = dicAcTypeToBtnItem[EM_ActivityType.VipGift];
        Btn_VipGift.transform.Find("Reddot").gameObject.SetActive(iNum > 0 ? true : false);
    }
    //private void ReddotCallback_Show_Btn_FirstRechage(object Obj)
    //{
    //    int iNum = (int)Obj;
    //    if(dicAcTypeToBtnItem.ContainsKey(EM_ActivityType.FirstRecharge))
    //    {
    //        GameObject Btn_FirstRechage = dicAcTypeToBtnItem[EM_ActivityType.FirstRecharge];
    //        if(Btn_FirstRechage != null)
    //            Btn_FirstRechage.transform.Find("Reddot").gameObject.SetActive(iNum > 0 ? true : false);
    //    }
    //}
    private void ReddotCallback_Show_Btn_WeekFund(object Obj)
    {
        int iNum = (int)Obj;
        if(dicAcTypeToBtnItem.ContainsKey(EM_ActivityType.WeekFund))
        {
            GameObject Btn_WeekFund = dicAcTypeToBtnItem[EM_ActivityType.WeekFund];
            if(Btn_WeekFund != null)
                Btn_WeekFund.transform.Find("Reddot").gameObject.SetActive(iNum > 0 ? true : false);
        }
    }

    private void ReddotCallback_Show_Btn_ExclusionSpin(object Obj)
    {
        int iNum = (int)Obj;
        if (dicAcTypeToBtnItem.ContainsKey(EM_ActivityType.ExclusionSpin))
        {
            GameObject Btn = dicAcTypeToBtnItem[EM_ActivityType.ExclusionSpin];
            if (Btn != null)
                Btn.transform.Find("Reddot").gameObject.SetActive(iNum > 0 ? true : false);
        }
    }
    #endregion
    /// <summary>
    /// 显示活动按钮Item
    /// </summary>
    private void OnShowActBtnItem(GameObject item, ActivityBtnItem data)
    {
        ActivityBtnItem activityBtnItem = data;
        item.GetComponent<ActBtnItemCtl>().SetData(activityBtnItem.m_ActivityName);
        f_RegClickEvent(item.gameObject, OnActBtnClick, activityBtnItem);
        if(!dicAcBtnItem.ContainsKey(activityBtnItem))
        {
            dicAcBtnItem.Add(activityBtnItem, item.gameObject);
            dicAcTypeToBtnItem.Add(activityBtnItem.m_ActivityType, item.gameObject);
        }
        item.transform.Find("BtnBg").gameObject.SetActive(!(m_curSelectActBtnItem == activityBtnItem));
        item.transform.Find("BtnBgDown").gameObject.SetActive(m_curSelectActBtnItem == activityBtnItem);
    }

    private static bool _CheckOpenTime(EM_ActivityType tEM_NewYearActType)
    {
        GameParamDT gameParam;
        int StarTime;
        int EndTime;
        int NowTime = GameSocket.GetInstance().f_GetServerTime();
        switch (tEM_NewYearActType)
        {
            case EM_ActivityType.ExclusionSpin:
                return glo_Main.GetInstance().m_SC_Pool.m_NewYearDealsEveryDaySC.f_GetAll().Count > 0;
            default:
                break;
        }
        return false;
    }
    /// <summary>
    /// 活动按钮item
    /// </summary>
    private class ActivityBtnItem
    {
        public EM_ActivityType m_ActivityType;//活动类型
        public string m_ActivityName;//活动名称
        public ActivityBtnItem(EM_ActivityType activityType, string activityName)
        {
            this.m_ActivityType = activityType;
            this.m_ActivityName = activityName;
        }
    }
}
