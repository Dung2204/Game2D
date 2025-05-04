using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
using System;
/// <summary>
/// 春节活动页面
/// </summary>
public class NewYearActPage : UIFramwork
{
    public GameObject ItemRoot;
    private static List<EM_NewYearActType> listRedPointSortAct = new List<EM_NewYearActType>();
    private List<ActivityBtnItem> listActivityBtnItem = new List<ActivityBtnItem>();
    private Dictionary<ActivityBtnItem, GameObject> dicAcBtnItem = new Dictionary<ActivityBtnItem, GameObject>();
    private Dictionary<EM_NewYearActType, GameObject> dicAcTypeToBtnItem = new Dictionary<EM_NewYearActType, GameObject>();

    private Dictionary<EM_NewYearActType, GameObject> dicActTypeToContentObj = new Dictionary<EM_NewYearActType, GameObject>();
    private ActivityBtnItem m_curSelectActBtnItem = null;
	//My Code
	GameParamDT DogStepOpen;
	//
    /// <summary>
    /// 活动红点分类
    /// </summary>
    /// <param name="actType">活动类型</param>
    public static void SortAct(EM_NewYearActType actType)
    {
        if (!listRedPointSortAct.Contains(actType))
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
    private ActivityBtnItem GetBtnItemByActType(EM_NewYearActType actType)//-----------------------case 1
    {
        switch (actType)
        {
            case EM_NewYearActType.RedPacketExchange: return new ActivityBtnItem(EM_NewYearActType.RedPacketExchange, f_GetActName(EM_GameNameParamType.RedPacketExchangeName));
            case EM_NewYearActType.RedPacketTask: return new ActivityBtnItem(EM_NewYearActType.RedPacketTask, f_GetActName(EM_GameNameParamType.RedPacketTaskName));
            //case EM_NewYearActType.MammonSendGifts: return new ActivityBtnItem(EM_NewYearActType.MammonSendGifts, "财神送礼");
            case EM_NewYearActType.NewYearSelling: return new ActivityBtnItem(EM_NewYearActType.NewYearSelling, f_GetActName(EM_GameNameParamType.NewYearSellingName));
            case EM_NewYearActType.ValentinesDay: return new ActivityBtnItem(EM_NewYearActType.ValentinesDay, f_GetActName(EM_GameNameParamType.ValentinesDayName));
            case EM_NewYearActType.DogStep: return new ActivityBtnItem(EM_NewYearActType.DogStep, f_GetActName(EM_GameNameParamType.DogStepName));
            case EM_NewYearActType.RecruitGift: return new ActivityBtnItem(EM_NewYearActType.RecruitGift, f_GetActName(EM_GameNameParamType.RecruitGiftName));
            case EM_NewYearActType.SingleRecharge: return new ActivityBtnItem(EM_NewYearActType.SingleRecharge, f_GetActName(EM_GameNameParamType.SingleRechargeName));
            case EM_NewYearActType.MutiRecharge: return new ActivityBtnItem(EM_NewYearActType.MutiRecharge, f_GetActName(EM_GameNameParamType.MutiRechargeName));
            case EM_NewYearActType.TotalConst: return new ActivityBtnItem(EM_NewYearActType.TotalConst, f_GetActName(EM_GameNameParamType.TotalConstName));
            case EM_NewYearActType.MammonSendGiftsNew: return new ActivityBtnItem(EM_NewYearActType.MammonSendGiftsNew, f_GetActName(EM_GameNameParamType.MammonSendGiftsNewName));
            case EM_NewYearActType.NewYearSign: return new ActivityBtnItem(EM_NewYearActType.NewYearSign, f_GetActName(EM_GameNameParamType.NewYearSignName));
            case EM_NewYearActType.FestivalExchange: return new ActivityBtnItem(EM_NewYearActType.FestivalExchange, f_GetActName(EM_GameNameParamType.FestivalExchange));
            //case EM_NewYearActType.DealsEveryDay: return new ActivityBtnItem(EM_NewYearActType.DealsEveryDay, f_GetActName(EM_GameNameParamType.DealsEveryDay));
            //case EM_NewYearActType.ExclusionSpin: return new ActivityBtnItem(EM_NewYearActType.ExclusionSpin, f_GetActName(EM_GameNameParamType.ExclusionSpin));
        }

        return null;
    }
    /// <summary>
    /// 活动活动名称
    /// </summary>
    /// <param name="eM_GameNameParamType">参数类型</param>
    /// <returns></returns>
    private string f_GetActName(EM_GameNameParamType eM_GameNameParamType)
    {
        GameNameParamDT gameParam = glo_Main.GetInstance().m_SC_Pool.m_GameNameParamSC.f_GetSC((int)eM_GameNameParamType) as GameNameParamDT;
        return gameParam.szParam1;
    }
    /// <summary>
    /// 往list添加ActBtn
    /// </summary>
    /// <param name="actType">活动类型</param>
    private void AddActItem(EM_NewYearActType actType)
    {
        ActivityBtnItem item = GetBtnItemByActType(actType);
        if (item != null)
            listActivityBtnItem.Add(item);
    }
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        dicAcBtnItem.Clear();
        dicAcTypeToBtnItem.Clear();
        listActivityBtnItem.Clear();

		//My Code
		DogStepOpen = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(92) as GameParamDT);
		//
		
		float windowAspect = (float)Screen.width /  (float) Screen.height ;
		MessageBox.ASSERT("" + windowAspect);
		// if(windowAspect >= 1.9)
		// {
			// f_GetObject("SideBtn").transform.localPosition = new Vector3(0f, 0f, 0f);
		// }
		
        Data_Pool.m_GuidancePool.m_NowOpenUIName = UINameConst.NewYearActPage;
        for (int i = 0; i < listRedPointSortAct.Count; i++)
        {
    
            AddActItem(listRedPointSortAct[i]);
        }
        _CheckEunm();//--------------------case 2
        InitBtnUI();
        if (e == null)
        {
            //默认选中第一个按钮
            ShowActDefault(listActivityBtnItem[0].m_ActivityType);
        }
        else
        {
            ShowActDefault((EM_NewYearActType)e);
        }
		if(ScreenControl.Instance.mFunctionRatio <= 0.85f)
		{
			f_GetObject("Anchor-Left").transform.localScale = new Vector3(0.85f, 1f, 1f);
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
    private void ShowActDefault(EM_NewYearActType em_ActivityPage)
    {
        for (int i = 0; i < listActivityBtnItem.Count; i++)
        {
            if (listActivityBtnItem[i].m_ActivityType == em_ActivityPage)
            {
                OnActBtnClick(dicAcBtnItem[listActivityBtnItem[i]], listActivityBtnItem[i], null);
                UIProgressBar uiProgressBar = f_GetObject("ProgressBar").transform.GetComponent<UIProgressBar>();
                //uiProgressBar.value = i * 1.0f / (listActivityBtnItem.Count - 1);
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
    private void OnPageResume(object e)//--------------------case 3
    {
        if (this.f_IsOpen())
        {
            //if (dicActTypeToContentObj.ContainsKey(EM_NewYearActType.GrandSignIn))
            //    dicActTypeToContentObj[EM_NewYearActType.GrandSignIn].GetComponent<ActGrandSignCtl>().f_ViewResume(e);
            //if (dicActTypeToContentObj.ContainsKey(EM_NewYearActType.MonthCard))
            //    dicActTypeToContentObj[EM_NewYearActType.MonthCard].GetComponent<ActMonthCardCtl>().f_ViewResume(e);
            OnActBtnClick(dicAcBtnItem[m_curSelectActBtnItem], m_curSelectActBtnItem, null);
        }
    }
    private GameObject f_LoadViewContent(EM_NewYearActType actType, string viewContentPath, GameObject objParent)
    {
        if (dicActTypeToContentObj.ContainsKey(actType))
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
        dicActTypeToContentObj.Add(actType, ViewContent);
        return ViewContent;
    }
    /// <summary>
    /// 按钮内容数据更新
    /// </summary>
    private void UpdateContentData(ActivityBtnItem item)//----------------------case 4
    {
        EM_NewYearActType actType = item.m_ActivityType;
        if (dicActTypeToContentObj.ContainsKey(EM_NewYearActType.RedPacketExchange))
            dicActTypeToContentObj[EM_NewYearActType.RedPacketExchange].GetComponent<RedPacketExchangeCtl>().f_DestoryView();
        if (dicActTypeToContentObj.ContainsKey(EM_NewYearActType.RedPacketTask))
            dicActTypeToContentObj[EM_NewYearActType.RedPacketTask].GetComponent<RedPacketTaskCtl>().f_DestoryView();
        if (dicActTypeToContentObj.ContainsKey(EM_NewYearActType.NewYearSelling))
            dicActTypeToContentObj[EM_NewYearActType.NewYearSelling].GetComponent<NewYearSellingPage>().f_DestoryView();
        if (dicActTypeToContentObj.ContainsKey(EM_NewYearActType.MammonSendGifts))
            dicActTypeToContentObj[EM_NewYearActType.MammonSendGifts].GetComponent<MammonSendGiftsPage>().f_DestoryView();
        if (dicActTypeToContentObj.ContainsKey(EM_NewYearActType.DogStep))
            dicActTypeToContentObj[EM_NewYearActType.DogStep].GetComponent<DogNewYearPage>().f_DestoryView();
        if (dicActTypeToContentObj.ContainsKey(EM_NewYearActType.RecruitGift))
            dicActTypeToContentObj[EM_NewYearActType.RecruitGift].GetComponent<NewYearRecruitGiftCtl>().f_DestoryView();
        if (dicActTypeToContentObj.ContainsKey(EM_NewYearActType.SingleRecharge))
            dicActTypeToContentObj[EM_NewYearActType.SingleRecharge].GetComponent<SingleRechargeCtl>().f_DestoryView();
        if (dicActTypeToContentObj.ContainsKey(EM_NewYearActType.MutiRecharge))
            dicActTypeToContentObj[EM_NewYearActType.MutiRecharge].GetComponent<MutiRechargeCtl>().f_DestoryView();
        if (dicActTypeToContentObj.ContainsKey(EM_NewYearActType.TotalConst))
            dicActTypeToContentObj[EM_NewYearActType.TotalConst].GetComponent<TotalConsumptionPage>().f_DestoryView();
        if (dicActTypeToContentObj.ContainsKey(EM_NewYearActType.MammonSendGiftsNew))
            dicActTypeToContentObj[EM_NewYearActType.MammonSendGiftsNew].GetComponent<MammonSendGiftsPage2>().f_DestoryView();
        if (dicActTypeToContentObj.ContainsKey(EM_NewYearActType.NewYearSign))
            dicActTypeToContentObj[EM_NewYearActType.NewYearSign].GetComponent<NewYearSignCtl>().f_DestoryView();
        if (dicActTypeToContentObj.ContainsKey(EM_NewYearActType.FestivalExchange))
            dicActTypeToContentObj[EM_NewYearActType.FestivalExchange].GetComponent<FestivalExchange>().f_DestoryView();
        //if (dicActTypeToContentObj.ContainsKey(EM_NewYearActType.DealsEveryDay))
        //    dicActTypeToContentObj[EM_NewYearActType.DealsEveryDay].GetComponent<DealsEveryDay>().f_DestoryView();
        //if (dicActTypeToContentObj.ContainsKey(EM_NewYearActType.ExclusionSpin))
        //    dicActTypeToContentObj[EM_NewYearActType.ExclusionSpin].GetComponent<ExclusionSpin>().f_DestoryView();
        switch (actType)
        {
            case EM_NewYearActType.RedPacketExchange://红包兑换
                int timeEnd = 0;
                List<NBaseSCDT> listDT = glo_Main.GetInstance().m_SC_Pool.m_RedPacketExChangeSC.f_GetAll();
                for (int i = 0; i < listDT.Count; i++)
                {
                    RedPacketExchangeDT dt = listDT[i] as RedPacketExchangeDT;
                    bool isOpen = CommonTools.f_CheckTime(dt.iTimeBegin.ToString(), dt.iTimeEnd.ToString());
                    if (isOpen)
                    {
                        timeEnd = dt.iTimeEnd;
                        break;
                    }
                }
                f_LoadViewContent(EM_NewYearActType.RedPacketExchange, "UI/UIPrefab/GameMain/NewYearAct/RedPacketExchangeExample", ItemRoot);
                dicActTypeToContentObj[EM_NewYearActType.RedPacketExchange].GetComponent<RedPacketExchangeCtl>().f_ShowView(timeEnd);
                break;
            case EM_NewYearActType.RedPacketTask://红包任务
                int timeEndRedPacketTask = 0;
                List<NBaseSCDT> listDTRecruitGift = glo_Main.GetInstance().m_SC_Pool.m_RedPacketTaskSC.f_GetAll();
                for (int i = 0; i < listDTRecruitGift.Count; i++)
                {
                    RedPacketTaskDT taskDT = listDTRecruitGift[i] as RedPacketTaskDT;
                    if (taskDT.iTaskType != Data_Pool.m_RedPacketTaskPool.mRecruitGiftTaskID)
                    {
                        //bool isOpen = CommonTools.f_CheckTime(taskDT.iTimeBegin.ToString(), taskDT.iTimeEnd.ToString());
                        bool isOpen = CommonTools.f_CheckActIsOpenForOpenSeverTime_TsuFunc(taskDT.iTimeBegin, taskDT.iTimeEnd); //TsuCode
                        if (isOpen)
                        {
                            timeEndRedPacketTask = taskDT.iTimeEnd;
                            break;
                        }
                    }
                }
                f_LoadViewContent(EM_NewYearActType.RedPacketTask, "UI/UIPrefab/GameMain/NewYearAct/RedPacketTaskExample", ItemRoot);
                dicActTypeToContentObj[EM_NewYearActType.RedPacketTask].GetComponent<RedPacketTaskCtl>().f_ShowView(this, timeEndRedPacketTask);
                break;
            case EM_NewYearActType.NewYearSelling:
                int timeEnd6 = 0;
                List<NBaseSCDT> listDT2 = glo_Main.GetInstance().m_SC_Pool.m_NewYearSelling.f_GetAll();
                for (int i = 0; i < listDT2.Count; i++)
                {
                    NewYearSellingDT dt = listDT2[i] as NewYearSellingDT;
                    bool isOpen = CommonTools.f_CheckTime(dt.iStarTime.ToString(), dt.iEndTime.ToString());
                    if (isOpen)
                    {
                        timeEnd6 = dt.iEndTime;
                        break;
                    }
                }
                f_LoadViewContent(EM_NewYearActType.NewYearSelling, "UI/UIPrefab/GameMain/Activity/NewYearSellingPage", ItemRoot);
                dicActTypeToContentObj[EM_NewYearActType.NewYearSelling].GetComponent<NewYearSellingPage>().f_ShowView(timeEnd6);
                break;
            case EM_NewYearActType.MammonSendGifts:
                f_LoadViewContent(EM_NewYearActType.MammonSendGifts, "UI/UIPrefab/GameMain/NewYearAct/MammonSendGiftsPage", ItemRoot);
                dicActTypeToContentObj[EM_NewYearActType.MammonSendGifts].GetComponent<MammonSendGiftsPage>().f_ShowView();
                break;

            case EM_NewYearActType.DogStep:
                f_LoadViewContent(EM_NewYearActType.DogStep, "UI/UIPrefab/GameMain/NewYearAct/DogNewYearPage", ItemRoot);
                dicActTypeToContentObj[EM_NewYearActType.DogStep].GetComponent<DogNewYearPage>().f_ShowView();
                break;
            case EM_NewYearActType.RecruitGift://招募有礼
                int timeEndRecruitGift = 0;
                List<NBaseSCDT> listDTRecruitGift2 = glo_Main.GetInstance().m_SC_Pool.m_RedPacketTaskSC.f_GetAll();
                for (int i = 0; i < listDTRecruitGift2.Count; i++)
                {
                    RedPacketTaskDT taskDT = listDTRecruitGift2[i] as RedPacketTaskDT;
                    if (taskDT.iTaskType == Data_Pool.m_RedPacketTaskPool.mRecruitGiftTaskID)
                    {
                        //bool isOpen = CommonTools.f_CheckTime(taskDT.iTimeBegin.ToString(), taskDT.iTimeEnd.ToString());
                        bool isOpen = CommonTools.f_CheckActIsOpenForOpenSeverTime_TsuFunc(taskDT.iTimeBegin, taskDT.iTimeEnd); //TsuCode
                        if (isOpen)
                        {
                            timeEndRecruitGift = taskDT.iTimeEnd;
                            break;
                        }
                    }
                }
                f_LoadViewContent(EM_NewYearActType.RecruitGift, "UI/UIPrefab/GameMain/NewYearAct/NewYearRecruitGiftExample", ItemRoot);
                dicActTypeToContentObj[EM_NewYearActType.RecruitGift].GetComponent<NewYearRecruitGiftCtl>().f_ShowView(this, timeEndRecruitGift);
                break;
            case EM_NewYearActType.SingleRecharge://单笔充值福利
                int timeEnd2 = 0;
                List<NBaseSCDT> listDT3 = glo_Main.GetInstance().m_SC_Pool.m_NewYearSingleRechargeAwardSC.f_GetAll();
                for (int i = 0; i < listDT3.Count; i++)
                {
                    NewYearSingleRechargeAwardDT dt = listDT3[i] as NewYearSingleRechargeAwardDT;
                    //bool isOpen = CommonTools.f_CheckActIsOpenForOpenSeverTime(dt.iTimeBeg, dt.iTimeEnd);  //CommonTools.f_CheckTime(dt.iTimeBeg.ToString(), dt.iTimeEnd.ToString());
                    bool isOpen = CommonTools.f_CheckActIsOpenForOpenSeverTime_TsuFunc(dt.iTimeBeg, dt.iTimeEnd); //TsuCode
                    if (isOpen)
                    {
                        timeEnd2 = dt.iTimeEnd;
                        break;
                    }
                }
                f_LoadViewContent(EM_NewYearActType.SingleRecharge, "UI/UIPrefab/GameMain/NewYearAct/SingleRechargeExample", ItemRoot);
                dicActTypeToContentObj[EM_NewYearActType.SingleRecharge].GetComponent<SingleRechargeCtl>().f_ShowView(this, timeEnd2);
                break;
            case EM_NewYearActType.MutiRecharge://累计充值福利
                int timeStart3 = 0;
                int timeEnd3 = 0;
                List<NBaseSCDT> timeEnd4 = glo_Main.GetInstance().m_SC_Pool.m_NewYearMultiRechargeAwardSC.f_GetAll();
                for (int i = 0; i < timeEnd4.Count; i++)
                {
                    NewYearMultiRechargeAwardDT dt = timeEnd4[i] as NewYearMultiRechargeAwardDT;
                    //bool isOpen = CommonTools.f_CheckActIsOpenForOpenSeverTime(dt.iTimeBeg, dt.iTimeEnd);//CommonTools.f_CheckTime(dt.iTimeBeg.ToString(), dt.iTimeEnd.ToString());
                    bool isOpen = CommonTools.f_CheckActIsOpenForOpenSeverTime_TsuFunc(dt.iTimeBeg, dt.iTimeEnd);
                    if (isOpen)
                    {
                        //timeStart3 = CommonTools.f_GetActStarTimeForOpenSeverTime(dt.iTimeBeg); //dt.iTimeBeg;
                        //timeEnd3 = CommonTools.f_GetActEndTimeForOpenSeverTime(dt.iTimeEnd); //dt.iTimeEnd;
                        timeStart3 = CommonTools.f_GetActStarTimeForOpenSeverTime_Tsu(dt.iTimeBeg); //dt.iTimeBeg;
                        timeEnd3 = CommonTools.f_GetActEndTimeForOpenSeverTime_Tsu(dt.iTimeEnd); //dt.iTimeEnd;
                        break;
                        //continue; //TsuCode
                    }
                }
                f_LoadViewContent(EM_NewYearActType.MutiRecharge, "UI/UIPrefab/GameMain/NewYearAct/MutiRechargeExample", ItemRoot);
                dicActTypeToContentObj[EM_NewYearActType.MutiRecharge].GetComponent<MutiRechargeCtl>().f_ShowView(this, timeStart3, timeEnd3);
                break;
            case EM_NewYearActType.TotalConst://累计消费
                int timeEnd5 = 0;
                for (int i = 0; i < glo_Main.GetInstance().m_SC_Pool.m_NewYearSyceeConsumeSC.f_GetAll().Count; i++)
                {
                    NewYearSyceeConsumeDT tNewYearSyceeConsumeDT = glo_Main.GetInstance().m_SC_Pool.m_NewYearSyceeConsumeSC.f_GetAll()[i] as NewYearSyceeConsumeDT;
                    //bool isOpen5 = CommonTools.f_CheckActIsOpenForOpenSeverTime(tNewYearSyceeConsumeDT.iTimeBegin, tNewYearSyceeConsumeDT.iTimeEnd); //CommonTools.f_CheckTime(tNewYearSyceeConsumeDT.iTimeBegin.ToString(), tNewYearSyceeConsumeDT.iTimeEnd.ToString());
                    bool isOpen5 = CommonTools.f_CheckActIsOpenForOpenSeverTime_TsuFunc(tNewYearSyceeConsumeDT.iTimeBegin, tNewYearSyceeConsumeDT.iTimeEnd); //TsuCode
                    if (isOpen5)
                    {
                        //timeEnd5 = CommonTools.f_GetActEndTimeForOpenSeverTime(tNewYearSyceeConsumeDT.iTimeEnd); //tNewYearSyceeConsumeDT.iTimeEnd;
                        timeEnd5 = CommonTools.f_GetActEndTimeForOpenSeverTime_Tsu(tNewYearSyceeConsumeDT.iTimeEnd); //TsuCode
                        break;
                    }
                }
                f_LoadViewContent(EM_NewYearActType.TotalConst, "UI/UIPrefab/GameMain/NewYearAct/TotalConsumptionPage", ItemRoot);
                dicActTypeToContentObj[EM_NewYearActType.TotalConst].GetComponent<TotalConsumptionPage>().f_ShowView(this, timeEnd5);
                break;
            case EM_NewYearActType.MammonSendGiftsNew://财神送礼(新)
                f_LoadViewContent(EM_NewYearActType.MammonSendGiftsNew, "UI/UIPrefab/GameMain/NewYearAct/MammonSendGiftsPage2", ItemRoot);
                dicActTypeToContentObj[EM_NewYearActType.MammonSendGiftsNew].GetComponent<MammonSendGiftsPage2>().f_ShowView();
                break;
            case EM_NewYearActType.NewYearSign://新春签到
                f_LoadViewContent(EM_NewYearActType.NewYearSign, "UI/UIPrefab/GameMain/NewYearAct/NewYearSignExample", ItemRoot);
                dicActTypeToContentObj[EM_NewYearActType.NewYearSign].GetComponent<NewYearSignCtl>().f_ShowView(this);
                break;
            case EM_NewYearActType.FestivalExchange:
                f_LoadViewContent(EM_NewYearActType.FestivalExchange, "UI/UIPrefab/GameMain/NewYearAct/FestivalExchange", ItemRoot);
                dicActTypeToContentObj[EM_NewYearActType.FestivalExchange].GetComponent<FestivalExchange>().f_ShowView(this);
                break;
            //case EM_NewYearActType.DealsEveryDay:
            //    f_LoadViewContent(EM_NewYearActType.DealsEveryDay, "UI/UIPrefab/GameMain/NewYearAct/DealsEveryDay", ItemRoot);
            //    dicActTypeToContentObj[EM_NewYearActType.DealsEveryDay].GetComponent<DealsEveryDay>().f_ShowView(this);
            //    break;
            //case EM_NewYearActType.ExclusionSpin:
            //    f_LoadViewContent(EM_NewYearActType.ExclusionSpin, "UI/UIPrefab/GameMain/NewYearAct/ExclusionSpin", ItemRoot);
            //    dicActTypeToContentObj[EM_NewYearActType.ExclusionSpin].GetComponent<ExclusionSpin>().f_ShowView(this);
            //    break;
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
        ItemRoot.transform.localPosition = posItemRoot;
        //365
    }
    /// <summary>
    /// 关闭所有按钮选项
    /// </summary>
    private void CloseAllBtnPressEffect()
    {
        for (int i = 0; i < listActivityBtnItem.Count; i++)
        {
            if (dicAcBtnItem.ContainsKey(listActivityBtnItem[i]))
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
        ccUIManage.GetInstance().f_SendMsg(UINameConst.NewYearActPage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
    }
    #endregion
    private void InitBtnUI()
    {
        List<BasePoolDT<long>> listBtnContent = new List<BasePoolDT<long>>();
        for (int i = 0; i < listActivityBtnItem.Count; i++)
        {
            BasePoolDT<long> item = new BasePoolDT<long>();
            item.iId = i;
            listBtnContent.Add(item);
        }
        GridUtil.f_SetGridView<ActivityBtnItem>(f_GetObject("ActBtnItemParent"), f_GetObject("ActBtnItemExample"), listActivityBtnItem, OnShowActBtnItem);
        f_GetObject("ActBtnItemParent").transform.GetComponentInParent<UIScrollView>().ResetPosition();
    }
    #region 红点提示
    protected override void InitRaddot()//-----------------------case 5(红点)
    {
        base.InitRaddot();
        if (dicAcTypeToBtnItem.Count == 0)//没有初始化，不执行
            return;
        if (dicAcTypeToBtnItem.ContainsKey(EM_NewYearActType.SingleRecharge))
            Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.SingleRecharge, dicAcTypeToBtnItem[EM_NewYearActType.SingleRecharge], ReddotCallback_Show_Btn_SingleRecharge);
        if (dicAcTypeToBtnItem.ContainsKey(EM_NewYearActType.MutiRecharge))
            Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.MutiRecharge, dicAcTypeToBtnItem[EM_NewYearActType.MutiRecharge], ReddotCallback_Show_Btn_MutiRecharge);
        if (dicAcTypeToBtnItem.ContainsKey(EM_NewYearActType.RedPacketTask))
            Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.RedPacketTask, dicAcTypeToBtnItem[EM_NewYearActType.RedPacketTask], ReddotCallback_Show_Btn_RedPacketTask);
        if (dicAcTypeToBtnItem.ContainsKey(EM_NewYearActType.RecruitGift))
            Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.RecruitGift, dicAcTypeToBtnItem[EM_NewYearActType.RecruitGift], ReddotCallback_Show_Btn_RecruitGift);
        if (dicAcTypeToBtnItem.ContainsKey(EM_NewYearActType.RedPacketExchange))
            Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.RedPacketExchange, dicAcTypeToBtnItem[EM_NewYearActType.RedPacketExchange], ReddotCallback_Show_Btn_RedPacketExchange);
        if (dicAcTypeToBtnItem.ContainsKey(EM_NewYearActType.TotalConst))
            Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.TotalConst, dicAcTypeToBtnItem[EM_NewYearActType.TotalConst], ReddotCallback_Show_Btn_TotalConst);
        if (dicAcTypeToBtnItem.ContainsKey(EM_NewYearActType.NewYearSelling))
            Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.NewYearSelling, dicAcTypeToBtnItem[EM_NewYearActType.NewYearSelling], ReddotCallback_Show_Btn_NewYearSelling);
        if (dicAcTypeToBtnItem.ContainsKey(EM_NewYearActType.MammonSendGiftsNew))
            Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.MammonSendGifts, dicAcTypeToBtnItem[EM_NewYearActType.MammonSendGiftsNew], ReddotCallback_Show_Btn_MammonSendGifts);
        if (dicAcTypeToBtnItem.ContainsKey(EM_NewYearActType.NewYearSign))
            Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.NewYearSign, dicAcTypeToBtnItem[EM_NewYearActType.NewYearSign], ReddotCallback_Show_Btn_NewYearSign);
        //if (dicAcTypeToBtnItem.ContainsKey(EM_NewYearActType.DealsEveryDay))
        //    Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.DealsEveryDay, dicAcTypeToBtnItem[EM_NewYearActType.DealsEveryDay], ReddotCallback_Show_Btn_DealsEveryDay);
        //if (dicAcTypeToBtnItem.ContainsKey(EM_NewYearActType.ExclusionSpin))
        //    Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.ExclusionSpin, dicAcTypeToBtnItem[EM_NewYearActType.ExclusionSpin], ReddotCallback_Show_Btn_ExclusionSpin);
        UpdateReddotUI();
    }
    protected override void On_Destory()
    {
        base.On_Destory();
        if (dicAcTypeToBtnItem.ContainsKey(EM_NewYearActType.SingleRecharge))
            Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.SingleRecharge, dicAcTypeToBtnItem[EM_NewYearActType.SingleRecharge]);
        if (dicAcTypeToBtnItem.ContainsKey(EM_NewYearActType.MutiRecharge))
            Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.MutiRecharge, dicAcTypeToBtnItem[EM_NewYearActType.MutiRecharge]);
        if (dicAcTypeToBtnItem.ContainsKey(EM_NewYearActType.RedPacketTask))
            Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.RedPacketTask, dicAcTypeToBtnItem[EM_NewYearActType.RedPacketTask]);
        if (dicAcTypeToBtnItem.ContainsKey(EM_NewYearActType.RecruitGift))
            Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.RecruitGift, dicAcTypeToBtnItem[EM_NewYearActType.RecruitGift]);
        if (dicAcTypeToBtnItem.ContainsKey(EM_NewYearActType.RedPacketExchange))
            Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.RedPacketExchange, dicAcTypeToBtnItem[EM_NewYearActType.RedPacketExchange]);
        if (dicAcTypeToBtnItem.ContainsKey(EM_NewYearActType.TotalConst))
            Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.TotalConst, dicAcTypeToBtnItem[EM_NewYearActType.TotalConst]);
        if (dicAcTypeToBtnItem.ContainsKey(EM_NewYearActType.NewYearSelling))
            Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.NewYearSelling, dicAcTypeToBtnItem[EM_NewYearActType.NewYearSelling]);
        if (dicAcTypeToBtnItem.ContainsKey(EM_NewYearActType.MammonSendGiftsNew))
            Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.MammonSendGifts, dicAcTypeToBtnItem[EM_NewYearActType.MammonSendGiftsNew]);
        if (dicAcTypeToBtnItem.ContainsKey(EM_NewYearActType.NewYearSign))
            Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.NewYearSign, dicAcTypeToBtnItem[EM_NewYearActType.NewYearSign]);
        //if (dicAcTypeToBtnItem.ContainsKey(EM_NewYearActType.DealsEveryDay))
        //    Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.DealsEveryDay, dicAcTypeToBtnItem[EM_NewYearActType.DealsEveryDay]);
        //if (dicAcTypeToBtnItem.ContainsKey(EM_NewYearActType.ExclusionSpin))
        //    Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.ExclusionSpin, dicAcTypeToBtnItem[EM_NewYearActType.ExclusionSpin]);
        dicAcBtnItem.Clear();
        dicAcTypeToBtnItem.Clear();
        listActivityBtnItem.Clear();
    }
    protected override void UpdateReddotUI()
    {
        base.UpdateReddotUI();
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.SingleRecharge);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.MutiRecharge);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.RedPacketTask);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.RecruitGift);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.RedPacketExchange);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.TotalConst);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.NewYearSelling);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.MammonSendGifts);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.NewYearSign);
        //Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.DealsEveryDay);
        //Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.ExclusionSpin);

    }
    private void ReddotCallback_Show_Btn_SingleRecharge(object Obj)
    {
        int iNum = (int)Obj;
        if (dicAcTypeToBtnItem.ContainsKey(EM_NewYearActType.SingleRecharge))
        {
            GameObject Btn_SingleRecharge = dicAcTypeToBtnItem[EM_NewYearActType.SingleRecharge];
            if (Btn_SingleRecharge != null)
                Btn_SingleRecharge.transform.Find("Reddot").gameObject.SetActive(iNum > 0 ? true : false);
        }
    }
    private void ReddotCallback_Show_Btn_MutiRecharge(object Obj)
    {
        int iNum = (int)Obj;
        if (dicAcTypeToBtnItem.ContainsKey(EM_NewYearActType.MutiRecharge))
        {
            GameObject Btn_MutiRecharge = dicAcTypeToBtnItem[EM_NewYearActType.MutiRecharge];
            if (Btn_MutiRecharge != null)
                Btn_MutiRecharge.transform.Find("Reddot").gameObject.SetActive(iNum > 0 ? true : false);
        }
    }
    private void ReddotCallback_Show_Btn_RedPacketTask(object Obj)
    {
        int iNum = (int)Obj;
        if (dicAcTypeToBtnItem.ContainsKey(EM_NewYearActType.RedPacketTask))
        {
            GameObject Btn_RedPacketTask = dicAcTypeToBtnItem[EM_NewYearActType.RedPacketTask];
            if (Btn_RedPacketTask != null)
                Btn_RedPacketTask.transform.Find("Reddot").gameObject.SetActive(iNum > 0 ? true : false);
        }
    }
    private void ReddotCallback_Show_Btn_RecruitGift(object Obj)
    {
        int iNum = (int)Obj;
        if (dicAcTypeToBtnItem.ContainsKey(EM_NewYearActType.RecruitGift))
        {
            GameObject Btn_RecruitGift = dicAcTypeToBtnItem[EM_NewYearActType.RecruitGift];
            if (Btn_RecruitGift != null)
                Btn_RecruitGift.transform.Find("Reddot").gameObject.SetActive(iNum > 0 ? true : false);
        }
    }
    private void ReddotCallback_Show_Btn_RedPacketExchange(object Obj)
    {
        int iNum = (int)Obj;
        if (dicAcTypeToBtnItem.ContainsKey(EM_NewYearActType.RedPacketExchange))
        {
            GameObject Btn_RedPacketExchange = dicAcTypeToBtnItem[EM_NewYearActType.RedPacketExchange];
            if (Btn_RedPacketExchange != null)
                Btn_RedPacketExchange.transform.Find("Reddot").gameObject.SetActive(iNum > 0 ? true : false);
        }
    }
    private void ReddotCallback_Show_Btn_TotalConst(object Obj)
    {
        int iNum = (int)Obj;
        if (dicAcTypeToBtnItem.ContainsKey(EM_NewYearActType.TotalConst))
        {
            GameObject Btn_TotalConst = dicAcTypeToBtnItem[EM_NewYearActType.TotalConst];
            if (Btn_TotalConst != null)
                Btn_TotalConst.transform.Find("Reddot").gameObject.SetActive(iNum > 0 ? true : false);
        }
    }
    private void ReddotCallback_Show_Btn_NewYearSelling(object Obj)
    {
        int iNum = (int)Obj;
        if (dicAcTypeToBtnItem.ContainsKey(EM_NewYearActType.NewYearSelling))
        {
            GameObject Btn_TotalConst = dicAcTypeToBtnItem[EM_NewYearActType.NewYearSelling];
            if (Btn_TotalConst != null)
                Btn_TotalConst.transform.Find("Reddot").gameObject.SetActive(iNum > 0 ? true : false);
        }
    }

    private void ReddotCallback_Show_Btn_MammonSendGifts(object Obj)
    {
        int iNum = (int)Obj;
        if (dicAcTypeToBtnItem.ContainsKey(EM_NewYearActType.MammonSendGiftsNew))
        {
            GameObject Btn_TotalConst = dicAcTypeToBtnItem[EM_NewYearActType.MammonSendGiftsNew];
            if (Btn_TotalConst != null)
                Btn_TotalConst.transform.Find("Reddot").gameObject.SetActive(iNum > 0 ? true : false);
        }
    }
    private void ReddotCallback_Show_Btn_NewYearSign(object Obj)
    {
        int iNum = (int)Obj;
        if (dicAcTypeToBtnItem.ContainsKey(EM_NewYearActType.NewYearSign))
        {
            GameObject Btn_NewYearSign = dicAcTypeToBtnItem[EM_NewYearActType.NewYearSign];
            if (Btn_NewYearSign != null)
                Btn_NewYearSign.transform.Find("Reddot").gameObject.SetActive(iNum > 0 ? true : false);
        }
    }

    //private void ReddotCallback_Show_Btn_DealsEveryDay(object Obj)
    //{
    //    int iNum = (int)Obj;
    //    if (dicAcTypeToBtnItem.ContainsKey(EM_NewYearActType.DealsEveryDay))
    //    {
    //        GameObject Btn_DealsEveryDay = dicAcTypeToBtnItem[EM_NewYearActType.DealsEveryDay];
    //        if (Btn_DealsEveryDay != null)
    //            Btn_DealsEveryDay.transform.Find("Reddot").gameObject.SetActive(iNum > 0 ? true : false);
    //    }
    //}

    //private void ReddotCallback_Show_Btn_ExclusionSpin(object Obj)
    //{
    //    int iNum = (int)Obj;
    //    if (dicAcTypeToBtnItem.ContainsKey(EM_NewYearActType.ExclusionSpin))
    //    {
    //        GameObject Btn = dicAcTypeToBtnItem[EM_NewYearActType.ExclusionSpin];
    //        if (Btn != null)
    //            Btn.transform.Find("Reddot").gameObject.SetActive(iNum > 0 ? true : false);
    //    }
    //}
    #endregion
    /// <summary>
    /// 显示活动按钮Item
    /// </summary>
    private void OnShowActBtnItem(GameObject item, ActivityBtnItem data)
    {
        ActivityBtnItem activityBtnItem = data;
        item.GetComponent<ActBtnItemCtl>().SetData(activityBtnItem.m_ActivityName);
        f_RegClickEvent(item.gameObject, OnActBtnClick, activityBtnItem);
        if (!dicAcBtnItem.ContainsKey(activityBtnItem))
        {
            dicAcBtnItem.Add(activityBtnItem, item.gameObject);
            dicAcTypeToBtnItem.Add(activityBtnItem.m_ActivityType, item.gameObject);
        }
        item.transform.Find("BtnBg").gameObject.SetActive(!(m_curSelectActBtnItem == activityBtnItem));
        item.transform.Find("BtnBgDown").gameObject.SetActive(m_curSelectActBtnItem == activityBtnItem);
    }
    /// <summary>
    /// 活动按钮item
    /// </summary>
    private class ActivityBtnItem
    {
        public EM_NewYearActType m_ActivityType;//活动类型
        public string m_ActivityName;//活动名称
        public ActivityBtnItem(EM_NewYearActType activityType, string activityName)
        {
            this.m_ActivityType = activityType;
            this.m_ActivityName = activityName;
        }
    }


    private void _CheckEunm()
    {
        //if (!listRedPointSortAct.Contains(EM_NewYearActType.RedPacketExchange) && _CheckOpenTime(EM_NewYearActType.RedPacketExchange))  //红包兑换
        //    AddActItem(EM_NewYearActType.RedPacketExchange);
        //if (!listRedPointSortAct.Contains(EM_NewYearActType.RedPacketTask) && _CheckOpenTime(EM_NewYearActType.RedPacketTask))  //红包任务
        //    AddActItem(EM_NewYearActType.RedPacketTask);

        //if (!listRedPointSortAct.Contains(EM_NewYearActType.MammonSendGifts) && _CheckOpenTime(EM_NewYearActType.MammonSendGifts))    //财神送礼
        //    AddActItem(EM_NewYearActType.MammonSendGifts);
        //if (!listRedPointSortAct.Contains(EM_NewYearActType.NewYearSelling) && _CheckOpenTime(EM_NewYearActType.NewYearSelling))  //新年贩售
        //    AddActItem(EM_NewYearActType.NewYearSelling);
        ////if (!listRedPointSortAct.Contains(EM_NewYearActType.ValentinesDay))     //情人节
        ////    AddActItem(EM_NewYearActType.ValentinesDay);

        ////if (!listRedPointSortAct.Contains(EM_NewYearActType.DogStep))     //狗狗
        ////    AddActItem(EM_NewYearActType.DogStep);
        //if (!listRedPointSortAct.Contains(EM_NewYearActType.RecruitGift) && _CheckOpenTime(EM_NewYearActType.RecruitGift))   //招募有礼
        //    AddActItem(EM_NewYearActType.RecruitGift);
        //if (!listRedPointSortAct.Contains(EM_NewYearActType.SingleRecharge) && _CheckOpenTime(EM_NewYearActType.SingleRecharge))    //单笔充值
        //    AddActItem(EM_NewYearActType.SingleRecharge);
        //if (!listRedPointSortAct.Contains(EM_NewYearActType.MutiRecharge) && _CheckOpenTime(EM_NewYearActType.MutiRecharge))    //累计充值
        //    AddActItem(EM_NewYearActType.MutiRecharge);
        //if (!listRedPointSortAct.Contains(EM_NewYearActType.TotalConst) && _CheckOpenTime(EM_NewYearActType.TotalConst))    //累计消费
        //    AddActItem(EM_NewYearActType.TotalConst);
        //if (!listRedPointSortAct.Contains(EM_NewYearActType.MammonSendGiftsNew) && _CheckOpenTime(EM_NewYearActType.MammonSendGiftsNew))    //累计消费
        //    AddActItem(EM_NewYearActType.MammonSendGiftsNew);
        //if (!listRedPointSortAct.Contains(EM_NewYearActType.NewYearSign) && _CheckOpenTime(EM_NewYearActType.NewYearSign))    //新春签到
        //    AddActItem(EM_NewYearActType.NewYearSign);
        //if (!listRedPointSortAct.Contains(EM_NewYearActType.FestivalExchange) && _CheckOpenTime(EM_NewYearActType.FestivalExchange))    //兑换
        //    AddActItem(EM_NewYearActType.FestivalExchange);

        ///////////TsuCode/////////
         if (!listRedPointSortAct.Contains(EM_NewYearActType.RedPacketExchange) && _CheckOpenTime(EM_NewYearActType.RedPacketExchange))  //红包兑换
        {
            AddActItem(EM_NewYearActType.RedPacketExchange);
        }

        if (!listRedPointSortAct.Contains(EM_NewYearActType.RedPacketTask) && _CheckOpenTime(EM_NewYearActType.RedPacketTask))  //红包任务
        {
            AddActItem(EM_NewYearActType.RedPacketTask);
        }
        if (!listRedPointSortAct.Contains(EM_NewYearActType.MammonSendGifts) && _CheckOpenTime(EM_NewYearActType.MammonSendGifts))    //财神送礼
            AddActItem(EM_NewYearActType.MammonSendGifts);
        if (!listRedPointSortAct.Contains(EM_NewYearActType.NewYearSelling) && _CheckOpenTime(EM_NewYearActType.NewYearSelling))  //新年贩售
        {
            AddActItem(EM_NewYearActType.NewYearSelling);
        }
        //if (!listRedPointSortAct.Contains(EM_NewYearActType.ValentinesDay))     //情人节
        //    AddActItem(EM_NewYearActType.ValentinesDay);

        if (!listRedPointSortAct.Contains(EM_NewYearActType.DogStep))     //狗狗
        {
			if(DogStepOpen.iParam1 == 1)
				AddActItem(EM_NewYearActType.DogStep);
        }
        if (!listRedPointSortAct.Contains(EM_NewYearActType.RecruitGift) && _CheckOpenTime(EM_NewYearActType.RecruitGift))   //招募有礼
        {
            AddActItem(EM_NewYearActType.RecruitGift);
        }
        if (!listRedPointSortAct.Contains(EM_NewYearActType.SingleRecharge) && _CheckOpenTime(EM_NewYearActType.SingleRecharge))    //单笔充值
        {
            AddActItem(EM_NewYearActType.SingleRecharge);
        }
        if (!listRedPointSortAct.Contains(EM_NewYearActType.MutiRecharge) && _CheckOpenTime(EM_NewYearActType.MutiRecharge))    //累计充值
        {
            AddActItem(EM_NewYearActType.MutiRecharge);
        }
        if (!listRedPointSortAct.Contains(EM_NewYearActType.TotalConst) && _CheckOpenTime(EM_NewYearActType.TotalConst))    //累计消费
        {
            AddActItem(EM_NewYearActType.TotalConst);
        }
        if (!listRedPointSortAct.Contains(EM_NewYearActType.MammonSendGiftsNew) && _CheckOpenTime(EM_NewYearActType.MammonSendGiftsNew))    //累计消费
        {
            AddActItem(EM_NewYearActType.MammonSendGiftsNew);
        }
        if (!listRedPointSortAct.Contains(EM_NewYearActType.NewYearSign) && _CheckOpenTime(EM_NewYearActType.NewYearSign))    //新春签到
        {
            AddActItem(EM_NewYearActType.NewYearSign);
        }
        if (!listRedPointSortAct.Contains(EM_NewYearActType.FestivalExchange) && _CheckOpenTime(EM_NewYearActType.FestivalExchange))    //兑换
        {
            AddActItem(EM_NewYearActType.FestivalExchange);
        }
        //if (!listRedPointSortAct.Contains(EM_NewYearActType.DealsEveryDay) && _CheckOpenTime(EM_NewYearActType.DealsEveryDay))    //兑换
        //{
        //    AddActItem(EM_NewYearActType.DealsEveryDay);
        //}
        //if (!listRedPointSortAct.Contains(EM_NewYearActType.ExclusionSpin) && _CheckOpenTime(EM_NewYearActType.ExclusionSpin))    //兑换
        //{
        //    AddActItem(EM_NewYearActType.ExclusionSpin);
        //}
        ///////////////////////////
    }

    /// <summary>
    /// 是否开启新年(主界面是否显示新年活动入口)
    /// </summary>
    public static bool f_IsOpenNewYearAct()
    {
        if (_CheckOpenTime(EM_NewYearActType.RedPacketExchange) || _CheckOpenTime(EM_NewYearActType.RedPacketTask) || _CheckOpenTime(EM_NewYearActType.MammonSendGifts)
            || _CheckOpenTime(EM_NewYearActType.NewYearSelling) || _CheckOpenTime(EM_NewYearActType.RecruitGift) || _CheckOpenTime(EM_NewYearActType.SingleRecharge)
            || _CheckOpenTime(EM_NewYearActType.MutiRecharge) || _CheckOpenTime(EM_NewYearActType.TotalConst) || _CheckOpenTime(EM_NewYearActType.NewYearSign))
            return true;
        return false;
    }

    private static bool _CheckOpenTime(EM_NewYearActType tEM_NewYearActType)
    {
        //GameParamDT gameParamDTOpenLevel = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.NewYearOpenLevel) as GameParamDT);
        //int userLevel = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        //if (userLevel < gameParamDTOpenLevel.iParam1)
        //{
        //    return false;
        //}
        GameParamDT gameParam;
        int StarTime;
        int EndTime;
        int NowTime = GameSocket.GetInstance().f_GetServerTime();
        switch (tEM_NewYearActType)
        {
            case EM_NewYearActType.RedPacketExchange:
                List<NBaseSCDT> listDT = glo_Main.GetInstance().m_SC_Pool.m_RedPacketExChangeSC.f_GetAll();
                for (int i = 0; i < listDT.Count; i++)
                {
                    RedPacketExchangeDT dt = listDT[i] as RedPacketExchangeDT;
                    bool isOpen = CommonTools.f_CheckTime(dt.iTimeBegin.ToString(), dt.iTimeEnd.ToString());
                    if (isOpen)
                        return true;
                }
                break;
            case EM_NewYearActType.RedPacketTask:
                List<NBaseSCDT> listDTRecruitGift = glo_Main.GetInstance().m_SC_Pool.m_RedPacketTaskSC.f_GetAll();
                for (int i = 0; i < listDTRecruitGift.Count; i++)
                {
                    RedPacketTaskDT taskDT = listDTRecruitGift[i] as RedPacketTaskDT;
                    if (taskDT.iTaskType != Data_Pool.m_RedPacketTaskPool.mRecruitGiftTaskID)
                    {
                        //bool isOpen = CommonTools.f_CheckTime(taskDT.iTimeBegin.ToString(), taskDT.iTimeEnd.ToString());
                        bool isOpen = CommonTools.f_CheckActIsOpenForOpenSeverTime_TsuFunc(taskDT.iTimeBegin, taskDT.iTimeEnd); //TsuCode
                        if (isOpen)
                            return true;
                    }
                }
                break;
            case EM_NewYearActType.NewYearSelling:
                List<NBaseSCDT> listDT2 = glo_Main.GetInstance().m_SC_Pool.m_NewYearSelling.f_GetAll();
                for (int i = 0; i < listDT2.Count; i++)
                {
                    NewYearSellingDT dt = listDT2[i] as NewYearSellingDT;
                    StarTime = ccMath.f_Data2Int(dt.iStarTime);
                    EndTime = ccMath.f_Data2Int(dt.iEndTime);

                    if (dt.iRankDownLimit <= Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level))
                    {
                        if (StarTime < NowTime && NowTime < EndTime)
                        {
                            return true;
                        }
                    }
                }
                break;
            case EM_NewYearActType.MammonSendGifts:
                gameParam = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.MammonSendGiftsOpenTime) as GameParamDT);
                return CommonTools.f_CheckTime(gameParam.iParam1.ToString(), gameParam.iParam2.ToString());
            case EM_NewYearActType.ValentinesDay:
                break;
            case EM_NewYearActType.DogStep:
                return true;
                break;
            case EM_NewYearActType.RecruitGift:
                List<NBaseSCDT> listDTRecruitGift2 = glo_Main.GetInstance().m_SC_Pool.m_RedPacketTaskSC.f_GetAll();
                for (int i = 0; i < listDTRecruitGift2.Count; i++)
                {
                    RedPacketTaskDT taskDT = listDTRecruitGift2[i] as RedPacketTaskDT;
                    if (taskDT.iTaskType == Data_Pool.m_RedPacketTaskPool.mRecruitGiftTaskID)
                    {
                        //bool isOpen = CommonTools.f_CheckTime(taskDT.iTimeBegin.ToString(), taskDT.iTimeEnd.ToString());
                        bool isOpen = CommonTools.f_CheckActIsOpenForOpenSeverTime_TsuFunc(taskDT.iTimeBegin, taskDT.iTimeEnd); //TsuCode
                        if (isOpen)
                            return true;
                    }
                }
                break;
            case EM_NewYearActType.SingleRecharge:
                List<NBaseSCDT> listDT3 = glo_Main.GetInstance().m_SC_Pool.m_NewYearSingleRechargeAwardSC.f_GetAll();
                for (int i = 0; i < listDT3.Count; i++)
                {
                    NewYearSingleRechargeAwardDT dt = listDT3[i] as NewYearSingleRechargeAwardDT;
                    //StarTime = ccMath.f_Data2Int(dt.iTimeBeg);
                    //EndTime = ccMath.f_Data2Int(dt.iTimeEnd);
                    //bool isOpen = CommonTools.f_CheckActIsOpenForOpenSeverTime(dt.iTimeBeg, dt.iTimeEnd);
                    bool isOpen = CommonTools.f_CheckActIsOpenForOpenSeverTime_TsuFunc(dt.iTimeBeg, dt.iTimeEnd);
                    if (isOpen)
                        return true;
                }
                break;
            case EM_NewYearActType.MutiRecharge:
                List<NBaseSCDT> listDT4 = glo_Main.GetInstance().m_SC_Pool.m_NewYearMultiRechargeAwardSC.f_GetAll();
                for (int i = 0; i < listDT4.Count; i++)
                {
                    NewYearMultiRechargeAwardDT dt = listDT4[i] as NewYearMultiRechargeAwardDT;
                    //bool isOpen = CommonTools.f_CheckActIsOpenForOpenSeverTime(dt.iTimeBeg, dt.iTimeEnd);
                    bool isOpen = CommonTools.f_CheckActIsOpenForOpenSeverTime_TsuFunc(dt.iTimeBeg, dt.iTimeEnd); //TsuCode
                    if (isOpen)
                        return true;
                }
                break;
            case EM_NewYearActType.TotalConst:
                NewYearSyceeConsumeDT tNewYearSyceeConsumeDT;
                for (int i = 0; i < glo_Main.GetInstance().m_SC_Pool.m_NewYearSyceeConsumeSC.f_GetAll().Count; i++)
                {
                    tNewYearSyceeConsumeDT = glo_Main.GetInstance().m_SC_Pool.m_NewYearSyceeConsumeSC.f_GetAll()[i] as NewYearSyceeConsumeDT;
                    //StarTime = ccMath.f_Data2Int(tNewYearSyceeConsumeDT.iTimeBegin);
                    //EndTime = ccMath.f_Data2Int(tNewYearSyceeConsumeDT.iTimeEnd);
                    //bool isOpen = CommonTools.f_CheckActIsOpenForOpenSeverTime(tNewYearSyceeConsumeDT.iTimeBegin, tNewYearSyceeConsumeDT.iTimeEnd);
                    bool isOpen = CommonTools.f_CheckActIsOpenForOpenSeverTime_TsuFunc(tNewYearSyceeConsumeDT.iTimeBegin, tNewYearSyceeConsumeDT.iTimeEnd); //TsuCode
                    if (isOpen)
                    {
                        return true;
                    }
                }
                break;
            case EM_NewYearActType.MammonSendGiftsNew:
                if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < 25)
                    return false;
                int MammonSendGiftId = Data_Pool.m_NewYearActivityPool.m_MammonSendGiftArr.m_DTId;
                MammonSendGiftDT tMammonSendGiftDT = glo_Main.GetInstance().m_SC_Pool.m_MammonSendGiftSC.f_GetSC(MammonSendGiftId) as MammonSendGiftDT;
                if (tMammonSendGiftDT == null)
                    return false;
                else
                {
                    return true;
                }
            case EM_NewYearActType.NewYearSign:
                GameParamDT gameParamDT2 = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.NewYearSign) as GameParamDT);
                return CommonTools.f_CheckTime(gameParamDT2.iParam1.ToString(), gameParamDT2.iParam2.ToString());
            case EM_NewYearActType.FestivalExchange:
                return Data_Pool.m_NewYearActivityPool.m_bFesticalIsOpen;
            //case EM_NewYearActType.DealsEveryDay:
            //case EM_NewYearActType.ExclusionSpin:
            //    return glo_Main.GetInstance().m_SC_Pool.m_NewYearDealsEveryDaySC.f_GetAll().Count > 0;
            default:
                break;
        }
        return false;
    }
}
