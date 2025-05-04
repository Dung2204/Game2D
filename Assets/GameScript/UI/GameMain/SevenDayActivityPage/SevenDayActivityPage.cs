using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
using System;

public class SevenDayActivityPage : UIFramwork
{
    private UIWrapComponent _UIWraopBase_Item1;   //切页1

    private UIWrapComponent UIWraopBase_Item1
    {
        get
        {
            if (_UIWraopBase_Item1 == null)
            {
                _UIWraopBase_Item1 = new UIWrapComponent(200, 2, 750, 4, f_GetObject("ItemParent1"), f_GetObject("Item"), SevenDayPoolDTList[1], UpdateItem, null);
            }
            Sort();
            _UIWraopBase_Item1.f_SetHide(true);
            return _UIWraopBase_Item1;
        }
    }
    private void Sort()
    {
        SevenDayPoolDTList[tPageNum].Sort((BasePoolDT<long> a, BasePoolDT<long> b) =>
        {
            SevenActivityTaskPoolDT aSevenPoolDT = a as SevenActivityTaskPoolDT;
            SevenActivityTaskPoolDT bSevenPoolDT = b as SevenActivityTaskPoolDT;
            bool aIsFinsh = (a as SevenActivityTaskPoolDT).isFinsh == 1;
            bool bisFinsh = (b as SevenActivityTaskPoolDT).isFinsh == 1;

            if (aSevenPoolDT.isGain == 0 && bSevenPoolDT.isGain == 0)    //没有领取的
            {
                if (aIsFinsh && !bisFinsh)
                    return -1;
                else if (!aIsFinsh && bisFinsh)
                    return 1;
                else
                {
                    if (aSevenPoolDT.iId > bSevenPoolDT.iId)
                        return -1;
                    else if (aSevenPoolDT.iId < bSevenPoolDT.iId)
                        return 1;
                }
            }
            else if (aSevenPoolDT.isGain != 0 && bSevenPoolDT.isGain == 0)   //已领取的放在后面
            {
                return 1;
            }
            else if (aSevenPoolDT.isGain == 0 && bSevenPoolDT.isGain != 0)
            {
                return -1;
            }

            return 0;
        });
    }


    private Dictionary<int, List<BasePoolDT<long>>> SevenDayPoolDTList;  //对应的切页Item


    private int tPageNum = 1;
    private List<BasePoolDT<long>> SeleDayList;  //所选天数的列表
    private List<BasePoolDT<long>> PreviewList;   //预览用的LIst
    private GameObject ItemParent1;
    private GameObject ItemParent4;

    private UILabel ActivityEndTime;   //活动结束时间 
    private UILabel AcceptAwardTime;   //领奖结束时间

    private int NumDayNum_Page;   //当前切页点击的第几天
    private int CreateDayNum;    //创建的第几天

    private int Time_ShowTime;
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        _InitTimeUI();
        _InitList();
        f_LoadTexture();
    }
    private string strTexBgRoot = "UI/TextureRemove/NewServer/Texture_SevenDayAwardBg";
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        ////加载背景图
        //UITexture TexBg = f_GetObject("TexBg").GetComponent<UITexture>();
        //if (TexBg.mainTexture == null)
        //{
        //    Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot);
        //    TexBg.mainTexture = tTexture2D;
        //}
    }

    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("DayNumSeleBtn1", _Btn_ChangeDayNum, 1);
        f_RegClickEvent("DayNumSeleBtn2", _Btn_ChangeDayNum, 2);
        f_RegClickEvent("DayNumSeleBtn3", _Btn_ChangeDayNum, 3);
        f_RegClickEvent("DayNumSeleBtn4", _Btn_ChangeDayNum, 4);
        f_RegClickEvent("DayNumSeleBtn5", _Btn_ChangeDayNum, 5);
        f_RegClickEvent("DayNumSeleBtn6", _Btn_ChangeDayNum, 6);
        f_RegClickEvent("DayNumSeleBtn7", _Btn_ChangeDayNum, 7);

        f_RegClickEvent("Tab1", _Btn_ChangePage, 1);
        f_RegClickEvent("Tab2", _Btn_ChangePage, 2);
        f_RegClickEvent("Tab3", _Btn_ChangePage, 3);
        f_RegClickEvent("Tab4", _Btn_ChangePage, 4);

        f_RegClickEvent("MaskBg", _CloseThis);
        f_RegClickEvent("BtnClose", _CloseThis);
        f_RegClickEvent("Btn_EveryDayHot", _Btn_OpenEveryDayHot);

        f_RegClickEvent("DayDayAlpheBg", _Btn_ClosePreview);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        ccTimeEvent.GetInstance().f_UnRegEvent(Time_ShowTime);
    }
    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        ccUIHoldPool.GetInstance().f_UnHold();
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT tSocketCallback = new SocketCallbackDT();
        tSocketCallback.m_ccCallbackSuc = _UpdateUnHold;
        tSocketCallback.m_ccCallbackFail = _UpdateUnHold;
        Data_Pool.m_SevenActivityTaskPool.f_GetTaskInfo(tSocketCallback);

        //跳转界面回来要刷新界面
    }
    void _UpdateUnHold(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)obj != (int)eMsgOperateResult.OR_Succeed)
        {
            MessageBox.DEBUG(CommonTools.f_GetTransLanguage(2098));
            return;
        }

        Time_ShowTime = ccTimeEvent.GetInstance().f_RegEvent(1f, true, null, _UpdateTimeData);
        if (tPageNum != 4)
            _UpdatePageNum(tPageNum);
        else
            _UpdatePage4();
    }

    #region 按钮事件


    private void _BuyHalf(GameObject go, object obj1, object obj2)
    {
        HalfDiscountPoolDT tPoolDT = (HalfDiscountPoolDT)obj1;
        if ((tPoolDT.m_HalfDiscountDT.iCostNum * tPoolDT.m_HalfDiscountDT.iDiscount / 10) > Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee))
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(2099));
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT tBackDT = new SocketCallbackDT();
        tBackDT.m_ccCallbackFail = _BuyBack;
        tBackDT.m_ccCallbackSuc = _BuyBack;
        Data_Pool.m_AwardPool.m_GetLoginAward.Clear();
        Data_Pool.m_SevenActivityTaskPool.f_GetHalfInfo((byte)tPoolDT.m_HalfDiscountDT.iId, tBackDT);
    }

    private void _BuyBack(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)obj == (int)eMsgOperateResult.OR_Succeed)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(2100));
            _UpdatePage4();
            List<AwardPoolDT> tGoods = Data_Pool.m_AwardPool.m_GetLoginAward;
            if (tGoods.Count >= 1)
                ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN, new object[] { tGoods });
        }
        else
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(2101) + obj.ToString());
        }
    }
    private void _CloseThis(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SevenDayActivityPage, UIMessageDef.UI_CLOSE);
    }
    /// <summary>
    /// 切换切页
    /// </summary>
    private void _Btn_ChangePage(GameObject go, object obj1, object obj2)
    {
        tPageNum = (int)obj1;
        ItemParent1.SetActive(tPageNum != 4);
        ItemParent4.SetActive(tPageNum == 4);
        if (tPageNum != 4)
        {
            _UpdatePageNum(tPageNum);
        }
        else
        {
            _UpdatePage4();
        }
    }
    /// <summary>
    /// 切换天数
    /// </summary>
    private void _Btn_ChangeDayNum(GameObject go, object obj1, object obj2)
    {
        int tDayNum = (int)obj1;
        //点击的天数大于创建的天数
        if (CreateDayNum < tDayNum)
        {
            _Btn_OpenPreview(go, tDayNum, obj2);
            return;
        }
        _ChangeList(tDayNum);
        NumDayNum_Page = tDayNum;
        _ChangeDayNum(tDayNum);
    }
    /// <summary>
    /// 获取奖励
    /// </summary>
    private void _Btn_GetAward(GameObject go, object obj1, object obj2)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT tback = new SocketCallbackDT();
        tback.m_ccCallbackSuc = tttttt;
        tback.m_ccCallbackFail = tttttt;
        Data_Pool.m_SevenActivityTaskPool.f_GetSevenAward(((SevenActivityTaskDT)obj1).iId, tback);
    }
    private void tttttt(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)obj == (int)eMsgOperateResult.OR_Succeed)
        {
            UIWraopBase_Item1.f_UpdateView();
            List<AwardPoolDT> tGoods = Data_Pool.m_AwardPool.m_GetLoginAward;
            if (tGoods.Count >= 1)
                ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN, new object[] { tGoods });
        }
        else
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(2102) + obj.ToString());
        }
        UpdayeRed();

    }

    private void _BtnGoWay(GameObject go, object obj1, object obj2)
    {

        switch ((string)obj1)
        {
            case UINameConst.ShopMutiCommonPage:
                if ((int)obj2 == 6)
                {
                    if (!UITool.f_GetIsOpensystem(EM_NeedLevel.RebelArmyLevel))
                    {
                        UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(2103), UITool.f_GetSysOpenLevel(EM_NeedLevel.RebelArmyLevel)));
                        return;
                    }
                }
                if ((int)obj2 == 3)
                {
                    if (!UITool.f_GetIsOpensystem(EM_NeedLevel.ArenaLevel))
                    {
                        UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(2104), UITool.f_GetSysOpenLevel(EM_NeedLevel.ArenaLevel)));
                        return;
                    }
                }
                if ((int)obj2 == 5)
                {
                    if (!UITool.f_GetIsOpensystem(EM_NeedLevel.RunningManLvel))
                    {
                        UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(2105), UITool.f_GetSysOpenLevel(EM_NeedLevel.RunningManLvel)));
                        return;
                    }
                }
                break;

            case UINameConst.ArenaPageNew:
                if (!UITool.f_GetIsOpensystem(EM_NeedLevel.ArenaLevel))
                {
                    UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(2106), UITool.f_GetSysOpenLevel(EM_NeedLevel.ArenaLevel)));
                    return;
                }
                break;
            case UINameConst.RebelArmy:
                if (!UITool.f_GetIsOpensystem(EM_NeedLevel.RebelArmyLevel))
                {
                    UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(2107), UITool.f_GetSysOpenLevel(EM_NeedLevel.RebelArmyLevel)));
                    return;
                }
                break;
            case UINameConst.RunningManPage:
                if (!UITool.f_GetIsOpensystem(EM_NeedLevel.RunningManLvel))
                {
                    UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(2108), UITool.f_GetSysOpenLevel(EM_NeedLevel.RunningManLvel)));
                    return;
                }
                break;
            case UINameConst.GrabTreasurePage:
                if (!UITool.f_GetIsOpensystem(EM_NeedLevel.GrabTreasureLevel))
                {
                    UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(2109), UITool.f_GetSysOpenLevel(EM_NeedLevel.GrabTreasureLevel)));
                    return;
                }
                break;
            case UINameConst.LegionMenuPage:
                if (!UITool.f_GetIsOpensystem(EM_NeedLevel.LegionLevel))
                {
                    UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(16), UITool.f_GetSysOpenLevel(EM_NeedLevel.LegionLevel)));
                    return;
                }
                break;

        }

        UITool.f_GotoPage(ccUIManage.GetInstance().f_GetUIHandler(UINameConst.MainMenu), (string)obj1, (int)obj2, null, this);
        ccTimeEvent.GetInstance().f_UnRegEvent(Time_ShowTime);
        ccUIHoldPool.GetInstance().f_Hold(this);
    }

    private void _Btn_OpenEveryDayHot(GameObject go, object obj1, object obj2)
    {
        if (iTime1 >= 0)
            ccUIManage.GetInstance().f_SendMsg(UINameConst.EveryDayHotSalePage, UIMessageDef.UI_OPEN, CreateDayNum);
    }

    private void _Btn_OpenPreview(GameObject go, object obj1, object obj2)
    {
        f_GetObject("Preview").SetActive(true);
        _UpdatePreview((int)obj1);
    }
    private void _Btn_ClosePreview(GameObject go, object obj1, object obj2)
    {
        f_GetObject("Preview").SetActive(false);
    }
    #endregion

    #region UI更新
    private void _UpdatePageNum(int PageNum)
    {
        tPageNum = PageNum;
        _ChangePage(tPageNum);
        ItemParent1.SetActive(PageNum != 4);
        ItemParent4.SetActive(PageNum == 4);
        if (PageNum != 4)
        {
            UIWraopBase_Item1.f_UpdateList(SevenDayPoolDTList[tPageNum]);
            UIWraopBase_Item1.f_ResetView();
            UIWraopBase_Item1.f_UpdateView();
        }
        else
        {
            _UpdatePage4();
        }
        UpdayeRed();

    }
    private void _UpdatePage4()
    {
        HalfDiscountPoolDT tPoolDT = Data_Pool.m_SevenActivityTaskPool.HalfDiscountPool[NumDayNum_Page] as HalfDiscountPoolDT;
        _ChangePage(4);
        Transform Page4 = f_GetObject("BuyGameObject").transform;
        Transform GoodsResource = Page4.Find("ResourceCommonItem");

        UILabel NowMoneyNum = Page4.Find("NowMoneyNum/Num").GetComponent<UILabel>();
        UILabel BeforeMoneyNum = Page4.Find("BeforeMoneyNum/Num").GetComponent<UILabel>();

        GameObject Item4Buy = Page4.Find("Item4Buy").gameObject;
        Item4Buy.SetActive(tPoolDT.BuyTime < tPoolDT.m_HalfDiscountDT.iBuyNum);
        f_RegClickEvent(Item4Buy, _BuyHalf, tPoolDT);
        GoodsResource.GetComponent<ResourceCommonItem>().f_UpdateByInfo(tPoolDT.m_HalfDiscountDT.iResType, tPoolDT.m_HalfDiscountDT.iResId, tPoolDT.m_HalfDiscountDT.iResNum);
        NowMoneyNum.text = (tPoolDT.m_HalfDiscountDT.iCostNum * tPoolDT.m_HalfDiscountDT.iDiscount / 10).ToString();
        BeforeMoneyNum.text = tPoolDT.m_HalfDiscountDT.iCostNum.ToString();

    }
    private void UpdateItem(Transform tran, BasePoolDT<long> tBasePool)
    {
        tran.gameObject.SetActive(true);
        SevenActivityTaskPoolDT tSevenActivity = tBasePool as SevenActivityTaskPoolDT;
        UILabel tTitle = tran.Find("Title").GetComponent<UILabel>();
        Transform AwardItemParent = tran.Find("ItemAward");
        GameObject GetBtn = tran.Find("GetBtn").gameObject;
        GameObject GoWayBtn = tran.Find("GoWayBtn").gameObject;
        GameObject Accomplish = tran.Find("Accomplish").gameObject;
        GameObject undone = tran.Find("undone").gameObject;
        GameObject isGain = tran.Find("isGain").gameObject;
        UISprite ItemBg = tran.Find("ItemBg").GetComponent<UISprite>();


        // UITool.f_SetSpriteGray(GetBtn.GetComponent<UISprite>(), );

        if (tSevenActivity.isGain == 1)
        {
            GetBtn.transform.Find("Label").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(2110);
            f_UnRegClickEvent(GetBtn);
        }
        else
        {
            GetBtn.transform.Find("Label").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(2111);
            f_RegClickEvent(GetBtn, _Btn_GetAward, tSevenActivity.m_SevenActivityTaskDT);
        }
        int flag = 0;

        if (tSevenActivity.isFinsh == 1 && tSevenActivity.isGain == 1) flag = 3;
        else if (tSevenActivity.isFinsh == 1) flag = 2;
        else flag = 1;

        bool b = (tSevenActivity.isFinsh == 1) || (tSevenActivity.m_SevenActivityTaskDT.itype == (int)EM_eSevenDay.eSevenDay_Login);


        isGain.SetActive(flag == 3);   //既完成又已领取
        GetBtn.SetActive(flag == 2);  //1为完成  0为未领取
        GoWayBtn.SetActive(flag == 1);

        ItemBg.spriteName = flag == 3 ? "qrhd_frame_c" : "qrhd_frame_b";


        int Condition = 0;
        int ShowValue = 0;

        if (tSevenActivity.isFinsh == 0 && tSevenActivity.m_SevenActivityTaskDT.iCondition2 != 0)
        {
            Condition = tSevenActivity.m_SevenActivityTaskDT.iCondition2;
            ShowValue = tSevenActivity.m_result[1];
            tTitle.text = string.Format(tSevenActivity.m_SevenActivityTaskDT.szDonditionDesc + "({0}/{1})", UITool.f_CountToChineseStr(ShowValue), UITool.f_CountToChineseStr( Condition));
        }
        else if (tSevenActivity.isFinsh == 0 && tSevenActivity.m_SevenActivityTaskDT.iCondition2 == 0)
        {
            Condition = tSevenActivity.m_SevenActivityTaskDT.iCondition1;

            //TsuCode -fix hien thi cac goi thang
            ShowValue = tSevenActivity.m_result[0];
            if (tSevenActivity.m_SevenActivityTaskDT.itype == (int)EM_eSevenDay.eSevenDay_MonthCard)
            {
                if(tSevenActivity.iId==1102)
                    ShowValue= tSevenActivity.m_result[0];
                if (tSevenActivity.iId == 1103)
                    ShowValue = tSevenActivity.m_result[1];
            }
            //-------

            tTitle.text = string.Format(tSevenActivity.m_SevenActivityTaskDT.szDonditionDesc + "({0}/{1})", UITool.f_CountToChineseStr(ShowValue), UITool.f_CountToChineseStr(Condition));
            //if (tSevenActivity.m_SevenActivityTaskDT.itype == (int)EM_eSevenDay.eSevenDay_Rebels_Dam_Max)
            //    tTitle.text = string.Format(tSevenActivity.m_SevenActivityTaskDT.szDonditionDesc + "({0}万/{1}万)", ShowValue / 10000, Condition / 10000);
            //else
            //{
                
            //}
        }

        if (tSevenActivity.isFinsh == 1)
        {
            tTitle.text = string.Format(tSevenActivity.m_SevenActivityTaskDT.szDonditionDesc);
        }

        _CreateItemAward(AwardItemParent, AwardItemParent.GetChild(0), tSevenActivity.m_SevenActivityTaskDT.iAwardId1, tSevenActivity.m_SevenActivityTaskDT.iAwardType1, tSevenActivity.m_SevenActivityTaskDT.iAwardNum1, flag == 3);
        _CreateItemAward(AwardItemParent, AwardItemParent.GetChild(1), tSevenActivity.m_SevenActivityTaskDT.iAwardId2, tSevenActivity.m_SevenActivityTaskDT.iAwardType2, tSevenActivity.m_SevenActivityTaskDT.iAwardNum2, flag == 3);
        _CreateItemAward(AwardItemParent, AwardItemParent.GetChild(2), tSevenActivity.m_SevenActivityTaskDT.iAwardId3, tSevenActivity.m_SevenActivityTaskDT.iAwardType3, tSevenActivity.m_SevenActivityTaskDT.iAwardNum3, flag == 3);


        f_RegClickEvent(GoWayBtn, _BtnGoWay, tSevenActivity.m_SevenActivityTaskDT.szUIName, tSevenActivity.m_SevenActivityTaskDT.iParam);
    }


    int iTime1;
    int iTime2;
    DateTime tDate1;
    DateTime tDate2;
    DateTime tDate3;
    //跨天  
    //  需要请求每日登录        第8天关闭每日热卖    
    private void _UpdateTimeData(object boj)
    {
        iTime1 = (Data_Pool.m_SevenActivityTaskPool.OpenSeverTime + 604800) - GameSocket.GetInstance().f_GetServerTime();
        iTime2 = (Data_Pool.m_SevenActivityTaskPool.OpenSeverTime + 864000) - GameSocket.GetInstance().f_GetServerTime();
        f_GetObject("Btn_EveryDayHot").SetActive(iTime1 >= 0);

        if (iTime2 <= 0)
        {
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_UpdateSevenBtn);

            ccUIManage.GetInstance().f_SendMsg(UINameConst.SevenDayActivityPage, UIMessageDef.UI_CLOSE);
            return;
        }
        if (iTime1 > 0)
        {
            tDate1 = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Unspecified).AddSeconds(iTime1);
            if (tDate1.Day != 1)
                ActivityEndTime.text = string.Format(CommonTools.f_GetTransLanguage(2112), tDate1.Day - 1, tDate1.Hour, tDate1.Minute) + tDate1.Second + CommonTools.f_GetTransLanguage(2114);
            else
                ActivityEndTime.text = string.Format(CommonTools.f_GetTransLanguage(2113), tDate1.Hour, tDate1.Minute) + tDate1.Second + CommonTools.f_GetTransLanguage(2114);
            tDate3 = ccMath.time_t2DateTime(GameSocket.GetInstance().f_GetServerTime());
            if ((GameSocket.GetInstance().mNextDayTime - tDate3).TotalSeconds < 0)
            {
                _Next2Day();
            }
        }
        else
        {
            ActivityEndTime.text = string.Format(CommonTools.f_GetTransLanguage(2115));
        }
        tDate2 = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Unspecified).AddSeconds(iTime2);
        if (tDate2.Day != 1)
            AcceptAwardTime.text = string.Format(CommonTools.f_GetTransLanguage(2112), tDate2.Day - 1, tDate2.Hour, tDate2.Minute) + tDate2.Second + CommonTools.f_GetTransLanguage(2114);
        else
            AcceptAwardTime.text = string.Format(CommonTools.f_GetTransLanguage(2113), tDate2.Hour, tDate2.Minute) + tDate2.Second + CommonTools.f_GetTransLanguage(2114);
    }

    /// <summary>
    /// 跨天时间
    /// </summary>
    private void _Next2Day()
    {
        Data_Pool.m_SevenActivityTaskPool._LoadDate();
        Data_Pool.m_SevenActivityTaskPool.f_GetTaskInfo(null);
        CreateDayNum = ((GameSocket.GetInstance().f_GetServerTime() - Data_Pool.m_SevenActivityTaskPool.OpenSeverTime) / 86400) + 1;
        _InitList();
    }
    private void _CreateItemAward(Transform itemParent, Transform item, int GoodsID, int GoodsType, int GoodsNum, bool isGray)
    {
        if (GoodsID == 0)
        {
            item.gameObject.SetActive(false);
        }
        else
        {
            ResourceCommonDT tDT = new ResourceCommonDT();
            tDT.f_UpdateInfo((byte)GoodsType, GoodsID, GoodsNum);
            tDT.isGray = isGray;
            item.GetComponent<ResourceCommonItem>().f_UpdateByInfo(tDT);
        }
    }

    private void _UpdatePreview(int DayNum)
    {
        GameObject Item = f_GetObject("PreviewItem");
        GameObject Item2 = f_GetObject("PreviewItem2");
        _UpdatePreview(Item, DayNum, 1);
        _UpdatePreview(Item2, DayNum, 2);
    }

    private void _ChangePage(int Pagenum)
    {
        _UpdatePageBtnState(f_GetObject("Tab1"), Pagenum == 1);
        _UpdatePageBtnState(f_GetObject("Tab2"), Pagenum == 2);
        _UpdatePageBtnState(f_GetObject("Tab3"), Pagenum == 3);
        _UpdatePageBtnState(f_GetObject("Tab4"), Pagenum == 4);
    }
    private void _ChangeDayNum(int Daynum)
    {
        _UpdateDayNumBtn(f_GetObject("DayNumSeleBtn1"), Daynum == 1);
        _UpdateDayNumBtn(f_GetObject("DayNumSeleBtn2"), Daynum == 2);
        _UpdateDayNumBtn(f_GetObject("DayNumSeleBtn3"), Daynum == 3);
        _UpdateDayNumBtn(f_GetObject("DayNumSeleBtn4"), Daynum == 4);
        _UpdateDayNumBtn(f_GetObject("DayNumSeleBtn5"), Daynum == 5);
        _UpdateDayNumBtn(f_GetObject("DayNumSeleBtn6"), Daynum == 6);
        _UpdateDayNumBtn(f_GetObject("DayNumSeleBtn7"), Daynum == 7);

        _UpdatePageNum(f_GetObject("Tab1"), (SevenDayPoolDTList[1][0] as SevenActivityTaskPoolDT).m_SevenActivityTaskDT.szPageName);
        _UpdatePageNum(f_GetObject("Tab2"), (SevenDayPoolDTList[2][0] as SevenActivityTaskPoolDT).m_SevenActivityTaskDT.szPageName);
        _UpdatePageNum(f_GetObject("Tab3"), (SevenDayPoolDTList[3][0] as SevenActivityTaskPoolDT).m_SevenActivityTaskDT.szPageName);
        _UpdatePageNum(f_GetObject("Tab4"), CommonTools.f_GetTransLanguage(2118));
        UpdayeRed();
    }

    private void _UpdatePreview(GameObject go, int DayNum, int ItemNum)
    {
        Transform AwardList = go.transform.Find("Body/Award");
        SevenActivityTaskPoolDT SevenPoolDT;

        PreviewList = Data_Pool.m_SevenActivityTaskPool.f_GetAllForData1(DayNum);
        PreviewList = _FindListForPageNum(PreviewList, ItemNum);

        List<ResourceCommonDT> ShowAward = new List<ResourceCommonDT>();
        int Index = 0;
        int awardIndex = 0;
        int PageNum = ItemNum + 1;
        ////程序bug，如果每个类型数据第一项配了三个奖励的话会死循环，待修改
        for (int i = 0; ShowAward.Count < 4 && PageNum < 4; i++)
        {
            if (PreviewList.Count <= Index)
            {
                PreviewList = Data_Pool.m_SevenActivityTaskPool.f_GetAllForData1(DayNum);
                PreviewList = _FindListForPageNum(PreviewList, PageNum);
                Index = 0;
                PageNum++;
                continue;
            }

            ResourceCommonDT tResource = new ResourceCommonDT();
            SevenPoolDT = PreviewList[Index] as SevenActivityTaskPoolDT;
            if (SevenPoolDT.m_SevenActivityTaskDT.iAwardId1 != 0 && awardIndex == 0)
            {
                tResource.f_UpdateInfo((byte)SevenPoolDT.m_SevenActivityTaskDT.iAwardType1, SevenPoolDT.m_SevenActivityTaskDT.iAwardId1, 0);
                awardIndex++;
            }
            else if (SevenPoolDT.m_SevenActivityTaskDT.iAwardId2 != 0 && awardIndex == 1)
            {
                tResource.f_UpdateInfo((byte)SevenPoolDT.m_SevenActivityTaskDT.iAwardType2, SevenPoolDT.m_SevenActivityTaskDT.iAwardId2, 0);
                awardIndex++;
            }
            else if (SevenPoolDT.m_SevenActivityTaskDT.iAwardId3 != 0 && awardIndex == 2)
            {
                tResource.f_UpdateInfo((byte)SevenPoolDT.m_SevenActivityTaskDT.iAwardType3, SevenPoolDT.m_SevenActivityTaskDT.iAwardId3, 0);
                awardIndex++;
            }
            if ((SevenPoolDT.m_SevenActivityTaskDT.iAwardId2 == 0 && awardIndex == 1) || (SevenPoolDT.m_SevenActivityTaskDT.iAwardId3 == 0 && awardIndex == 2))
            {
                Index++;
                awardIndex = 0;
            }
            if (awardIndex == 3)
            {
                Index++;
                awardIndex = 0;
            }
            if (ShowAward.Find((ResourceCommonDT tt) =>
            {
                return tt.mResourceId == tResource.mResourceId;
            }) == null)
            {
                ShowAward.Add(tResource);
                i++;
            }
        }


        for (int i = 0; i < AwardList.childCount; i++)
        {
            AwardList.GetChild(i).gameObject.SetActive(i < ShowAward.Count);

            if (i >= ShowAward.Count)
                continue;

            AwardList.GetChild(i).GetComponent<ResourceCommonItem>().f_UpdateByInfo(ShowAward[i].mResourceType, ShowAward[i].mResourceId, 0);
        }
    }
    private void _UpdatePageBtnState(GameObject go, bool IsActive)
    {
        go.transform.Find("Down").gameObject.SetActive(!IsActive);
        go.transform.Find("Up").gameObject.SetActive(IsActive);
    }
    private void _UpdateDayNumBtn(GameObject go, bool IsActive)
    {
        go.transform.Find("NoSele").gameObject.SetActive(!IsActive);
        go.transform.Find("OnSele").gameObject.SetActive(IsActive);
		// if(IsActive)
		// {
			// if(go.transform.Find("Sprite").gameObject.GetComponent<TweenAlpha>() == null)
			// {
				// TweenAlpha t = go.transform.Find("Sprite").gameObject.AddComponent<TweenAlpha>();

				// t.from = 0.5f;
				// t.style = UITweener.Style.PingPong;
			// }
		// }
		// else
		// {
			// if(go.transform.Find("Sprite").gameObject.GetComponent<TweenAlpha>() != null)
			// {
				// go.transform.Find("Sprite").gameObject.GetComponent<TweenAlpha>().enabled = false;
			// }
		// }
    }
    private void _UpdatePageNum(GameObject go, string name)
    {
        go.transform.Find("Down/TabName").GetComponent<UILabel>().text = name;
        go.transform.Find("Up/TabName").GetComponent<UILabel>().text = name;
    }

    #endregion

    #region   红点提示

    protected override void InitRaddot()
    {
        base.InitRaddot();
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.SevenDayNumPage1, f_GetObject("DayNumSeleBtn1"), ReddotCallback_Show_DayNumSeleBtn1);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.SevenDayNumPage2, f_GetObject("DayNumSeleBtn2"), ReddotCallback_Show_DayNumSeleBtn2);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.SevenDayNumPage3, f_GetObject("DayNumSeleBtn3"), ReddotCallback_Show_DayNumSeleBtn3);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.SevenDayNumPage4, f_GetObject("DayNumSeleBtn4"), ReddotCallback_Show_DayNumSeleBtn4);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.SevenDayNumPage5, f_GetObject("DayNumSeleBtn5"), ReddotCallback_Show_DayNumSeleBtn5);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.SevenDayNumPage6, f_GetObject("DayNumSeleBtn6"), ReddotCallback_Show_DayNumSeleBtn6);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.SevenDayNumPage7, f_GetObject("DayNumSeleBtn7"), ReddotCallback_Show_DayNumSeleBtn7);

        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.SevenTabPage1, f_GetObject("Tab1"), ReddotCallback_Show_Tab1);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.SevenTabPage2, f_GetObject("Tab2"), ReddotCallback_Show_Tab2);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.SevenTabPage3, f_GetObject("Tab3"), ReddotCallback_Show_Tab3);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.SevenTabPage4, f_GetObject("Tab4"), ReddotCallback_Show_Tab4);
    }

    protected override void UpdateReddotUI()
    {
        base.UpdateReddotUI();
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.SevenDayNumPage1);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.SevenDayNumPage2);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.SevenDayNumPage3);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.SevenDayNumPage4);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.SevenDayNumPage5);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.SevenDayNumPage6);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.SevenDayNumPage7);

        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.SevenTabPage1);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.SevenTabPage2);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.SevenTabPage3);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.SevenTabPage4);
    }

    protected override void On_Destory()
    {
        base.On_Destory();
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.SevenDayNumPage1, f_GetObject("DayNumSeleBtn1"));
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.SevenDayNumPage2, f_GetObject("DayNumSeleBtn2"));
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.SevenDayNumPage3, f_GetObject("DayNumSeleBtn3"));
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.SevenDayNumPage4, f_GetObject("DayNumSeleBtn4"));
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.SevenDayNumPage5, f_GetObject("DayNumSeleBtn5"));
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.SevenDayNumPage6, f_GetObject("DayNumSeleBtn6"));
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.SevenDayNumPage7, f_GetObject("DayNumSeleBtn7"));

        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.SevenTabPage1, f_GetObject("Tab1"));
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.SevenTabPage2, f_GetObject("Tab2"));
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.SevenTabPage3, f_GetObject("Tab3"));
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.SevenTabPage4, f_GetObject("Tab4"));
    }

    private void UpdayeRed()
    {

        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.SevenTabPage1);
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.SevenTabPage2);
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.SevenTabPage3);
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.SevenTabPage4);

        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.SevenDayNumPage1);
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.SevenDayNumPage2);
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.SevenDayNumPage3);
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.SevenDayNumPage4);
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.SevenDayNumPage5);
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.SevenDayNumPage6);
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.SevenDayNumPage7);

        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.SevenDayTask);
        SevenActivityTaskPoolDT tSevenPoolDT;
        int PageNum = 0;
        int DayNum = 0;
        for (int i = 0; i < Data_Pool.m_SevenActivityTaskPool.f_GetAll().Count; i++)
        {
            tSevenPoolDT = Data_Pool.m_SevenActivityTaskPool.f_GetAll()[i] as SevenActivityTaskPoolDT;

            if (tSevenPoolDT.IDayNum > CreateDayNum)
                break;

            if (tSevenPoolDT.isFinsh == 1 && tSevenPoolDT.isGain == 0)
            {
                if (tSevenPoolDT.m_SevenActivityTaskDT.iDayNum != DayNum)
                {
                    switch (tSevenPoolDT.m_SevenActivityTaskDT.iDayNum)
                    {
                        case 1:
                            Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.SevenDayNumPage1);
                            break;
                        case 2:
                            Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.SevenDayNumPage2);
                            break;
                        case 3:
                            Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.SevenDayNumPage3);
                            break;
                        case 4:
                            Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.SevenDayNumPage4);
                            break;
                        case 5:
                            Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.SevenDayNumPage5);
                            break;
                        case 6:
                            Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.SevenDayNumPage6);
                            break;
                        case 7:
                            Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.SevenDayNumPage7);
                            break;
                    }
                    DayNum = tSevenPoolDT.m_SevenActivityTaskDT.iDayNum;
                    Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.SevenDayTask);
                }
                if (NumDayNum_Page == tSevenPoolDT.m_SevenActivityTaskDT.iDayNum)
                {
                    if (tSevenPoolDT.m_SevenActivityTaskDT.iPage != PageNum)
                    {
                        switch (tSevenPoolDT.m_SevenActivityTaskDT.iPage)
                        {
                            case 1:
                                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.SevenTabPage1);
                                break;
                            case 2:
                                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.SevenTabPage2);
                                break;
                            case 3:
                                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.SevenTabPage3);
                                break;
                            case 4:
                                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.SevenTabPage4);
                                break;
                            default:
                                break;
                        }
                        PageNum = tSevenPoolDT.m_SevenActivityTaskDT.iPage;
                    }
                }
            }

        }
    }
    private void ReddotCallback_Show_DayNumSeleBtn1(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnFragment = f_GetObject("DayNumSeleBtn1");
        UITool.f_UpdateReddot(BtnFragment, iNum, new Vector3(35, 35, 0), 74);
    }

    private void ReddotCallback_Show_DayNumSeleBtn2(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnFragment = f_GetObject("DayNumSeleBtn2");
        UITool.f_UpdateReddot(BtnFragment, iNum, new Vector3(35, 35, 0), 74);
    }

    private void ReddotCallback_Show_DayNumSeleBtn3(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnFragment = f_GetObject("DayNumSeleBtn3");
        UITool.f_UpdateReddot(BtnFragment, iNum, new Vector3(35, 35, 0), 74);
    }

    private void ReddotCallback_Show_DayNumSeleBtn4(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnFragment = f_GetObject("DayNumSeleBtn4");
        UITool.f_UpdateReddot(BtnFragment, iNum, new Vector3(35, 35, 0), 74);
    }

    private void ReddotCallback_Show_DayNumSeleBtn5(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnFragment = f_GetObject("DayNumSeleBtn5");
        UITool.f_UpdateReddot(BtnFragment, iNum, new Vector3(35, 35, 0), 74);
    }

    private void ReddotCallback_Show_DayNumSeleBtn6(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnFragment = f_GetObject("DayNumSeleBtn6");
        UITool.f_UpdateReddot(BtnFragment, iNum, new Vector3(35, 35, 0), 74);
    }

    private void ReddotCallback_Show_DayNumSeleBtn7(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnFragment = f_GetObject("DayNumSeleBtn7");
        UITool.f_UpdateReddot(BtnFragment, iNum, new Vector3(35, 35, 0), 74);
    }

    private void ReddotCallback_Show_Tab1(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnFragment = f_GetObject("Tab1");
        UITool.f_UpdateReddot(BtnFragment, iNum, new Vector3(114, 39, 0), 74);
    }
    private void ReddotCallback_Show_Tab2(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnFragment = f_GetObject("Tab2");
        UITool.f_UpdateReddot(BtnFragment, iNum, new Vector3(114, 39, 0), 74);
    }
    private void ReddotCallback_Show_Tab3(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnFragment = f_GetObject("Tab3");
        UITool.f_UpdateReddot(BtnFragment, iNum, new Vector3(114, 39, 0), 74);
    }
    private void ReddotCallback_Show_Tab4(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnFragment = f_GetObject("Tab4");
        UITool.f_UpdateReddot(BtnFragment, iNum, new Vector3(114, 39, 0), 74);
    }

    #endregion

    private void _InitTimeUI()
    {
        ActivityEndTime = f_GetObject("ActivityEndTime").GetComponent<UILabel>();
        AcceptAwardTime = f_GetObject("AcceptAwardTime").GetComponent<UILabel>();

        Time_ShowTime = ccTimeEvent.GetInstance().f_RegEvent(1f, true, null, _UpdateTimeData);
    }

    private void _InitList()
    {
        ItemParent1 = f_GetObject("ItemParent");
        ItemParent4 = f_GetObject("ItemParent4");
        ItemParent4.SetActive(false);
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT tInfo = new SocketCallbackDT();
        tInfo.m_ccCallbackSuc = _InfoSuc;
        Data_Pool.m_SevenActivityTaskPool.f_HalfInfo(tInfo);     //保留请求半价购买的
        Data_Pool.m_SevenActivityTaskPool.f_GetTaskInfo(null);
    }
    private void _InfoSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        //new DateTime(Data_Pool.m_UserData.m_CreateTime);
        CreateDayNum = ((GameSocket.GetInstance().f_GetServerTime() - Data_Pool.m_SevenActivityTaskPool.OpenSeverTime) / 86400) + 1;
        if (CreateDayNum > 7)
            CreateDayNum = 7;
        NumDayNum_Page = CreateDayNum;
        _ChangeList(CreateDayNum);
        _ChangeDayNum(CreateDayNum);
        UpdayeRed();
        //SevenActivityTaskPoolDT tSevenPoolDT;
        //for (int i = 0; i < Data_Pool.m_SevenActivityTaskPool.f_GetAllForData3(1).Count; i++)
        //{
        //    tSevenPoolDT = Data_Pool.m_SevenActivityTaskPool.f_GetAllForData3(1)[i] as SevenActivityTaskPoolDT;

        //    if (tSevenPoolDT.isGain == 0 && tSevenPoolDT.m_SevenActivityTaskDT.iDayNum <= CreateDayNum)
        //    {
        //        switch (tSevenPoolDT.m_SevenActivityTaskDT.iDayNum)
        //        {
        //            case 1:
        //                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.SevenDayNumPage1);
        //                break;
        //            case 2:
        //                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.SevenDayNumPage2);
        //                break;
        //            case 3:
        //                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.SevenDayNumPage3);
        //                break;
        //            case 4:
        //                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.SevenDayNumPage4);
        //                break;
        //            case 5:
        //                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.SevenDayNumPage5);
        //                break;
        //            case 6:
        //                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.SevenDayNumPage6);
        //                break;
        //            case 7:
        //                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.SevenDayNumPage7);
        //                break;
        //        }
        //        Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.SevenTabPage1);
        //    }
        //}
    }
    private void _ChangeList(int DayNum)
    {
        //Data1参数为天数    来获取对应天数的List
        SeleDayList = Data_Pool.m_SevenActivityTaskPool.f_GetAllForData1(DayNum);
        //添加该天数对应的切页     切页最大为3  4为半价购买  另外读表
        SevenDayPoolDTList = new Dictionary<int, List<BasePoolDT<long>>>();
        SevenDayPoolDTList.Add(1, _FindListForPageNum(1));
        SevenDayPoolDTList.Add(2, _FindListForPageNum(2));
        SevenDayPoolDTList.Add(3, _FindListForPageNum(3));
        _UpdatePageNum(1);
    }
    //private void _GoWayPage(int id)
    //{
    //    string PageName = string.Empty;
    //    int param = 0;
    //    switch ((EM_eSevenDay)id)
    //    {
    //        case EM_eSevenDay.eSevenDay_Login:
    //            break;
    //        case EM_eSevenDay.eSevenDay_Days_Recharge_X:
    //            PageName = UINameConst.ShowVip;
    //            break;
    //        case EM_eSevenDay.eSevenDay_Lv_X:
    //        case EM_eSevenDay.eSevenDay_FightPower_X:
    //        case EM_eSevenDay.eSevenDay_MainPve_X:
    //            PageName = UINameConst.DungeonChapterPage;
    //            param = (int)EM_Fight_Enum.eFight_DungeonMain;
    //            break;
    //        case EM_eSevenDay.eSevenDay_Battle_X_Card:
    //        case EM_eSevenDay.eSevenDay_Battle_X_Card_Q_X:
    //        case EM_eSevenDay.eSevenDay_Battle_X_CardWear_Q_X:
    //        case EM_eSevenDay.eSevenDay_Battle_X_WearEquip_X:
    //        case EM_eSevenDay.eSevenDay_Battle_X_WearRefine_X:
    //        case EM_eSevenDay.eSevenDay_Battle_X_MagicStren_X:
    //        case EM_eSevenDay.eSevenDay_Battle_X_MagicRefine_X:
    //            PageName = UINameConst.LineUpPage;
    //            break;
    //        case EM_eSevenDay.eSevenDay_WearEquip_Refine_X:
    //            PageName = UINameConst.EquipBagPage;
    //            break;
    //        case EM_eSevenDay.eSevenDay_FightGeneral_Draw_X:
    //        case EM_eSevenDay.eSevenDay_GodGeneral_Draw_X:
    //            PageName = UINameConst.ShopPage;
    //            param = (int)ShopPage.EM_PageIndex.RecruitContent;
    //            break;
    //        case EM_eSevenDay.eSevenDay_Arena_Rank_X:
    //            PageName = UINameConst.ArenaPage;
    //            break;
    //        case EM_eSevenDay.eSevenDay_Card_Fit_X:
    //        case EM_eSevenDay.eSevenDay_Card_DiamondFit_X:
    //            break;
    //        case EM_eSevenDay.eSevenDay_Magic_Synthesis_X:
    //        case EM_eSevenDay.eSevenDay_Magic_Synthesis_Q_X:
    //            PageName = UINameConst.GrabTreasurePage;
    //            break;
    //        case EM_eSevenDay.eSevenDay_Through_Rank_X:
    //        case EM_eSevenDay.eSevenDay_Through_Reset_X:
    //        case EM_eSevenDay.eSevenDay_Through_Star_X:
    //            PageName = UINameConst.RunningManPage;
    //            break;
    //        case EM_eSevenDay.eSevenDay_Retain24:
    //        case EM_eSevenDay.eSevenDay_Legion_Make:
    //        case EM_eSevenDay.eSevenDay_Legion_Publicity_H:
    //        case EM_eSevenDay.eSevenDay_Legion_Publicity_M:
    //        case EM_eSevenDay.eSevenDay_Legion_Publicity_L:
    //            break;
    //        case EM_eSevenDay.eSevenDay_FightGeneral_Shop_Refresh_X:
    //        case EM_eSevenDay.eSevenDay_FightGeneral_Shop_Buy_X:
    //        case EM_eSevenDay.eSevenDay_Rebels_Shop_Consume_X:
    //        case EM_eSevenDay.eSevenDay_God_Shop_Consume_X:
    //        case EM_eSevenDay.eSevenDay_Shop_Sycee_Consume_X:
    //        case EM_eSevenDay.eSevenDay_MainCard_Advanced_X:
    //        case EM_eSevenDay.eSevenDay_LifStar_X:
    //        case EM_eSevenDay.eSevenDay_LifStar_Max:
    //        case EM_eSevenDay.eSevenDay_Crusade_Buy:
    //        case EM_eSevenDay.eSevenDay_Rebels_Dam_Max:
    //        case EM_eSevenDay.eSevenDay_Rebels_Exploits_X:
    //        case EM_eSevenDay.eSevenDay_Patrol_H_X:
    //        case EM_eSevenDay.eSevenDay_Patrol_X:
    //        case EM_eSevenDay.eSevenDay_Magic_Refine_Max:
    //        case EM_eSevenDay.eSevenDay_ThroughShop_Consume_X:
    //        case EM_eSevenDay.eSevenDay_Recharge_X:
    //        case EM_eSevenDay.eSevenDay_Treasure_Grab_X:
    //        case EM_eSevenDay.eSevenDay_MainPve_Win_X:
    //        case EM_eSevenDay.eSevenDay_Arena_Win_X:
    //        case EM_eSevenDay.eSevenDay_ElitePve_Fight_X:
    //        case EM_eSevenDay.eSevenDay_Retain50:
    //        case EM_eSevenDay.eSevenDay_Retain51:
    //        case EM_eSevenDay.eSevenDay_High_CardDraw10_X:
    //        case EM_eSevenDay.eSevenDay_PluginLv_X:
    //        case EM_eSevenDay.eSevenDay_BattleCard_Advance_X:
    //        case EM_eSevenDay.eSevenDay_SendEngery_X:
    //        case EM_eSevenDay.eSevenDay_Rebels_Attack_X:
    //        case EM_eSevenDay.eSevenDay_MainPve_Star_X:
    //        case EM_eSevenDay.eSevenDay_ElitePve_Clearance_X:
    //        case EM_eSevenDay.eSevenDay_ElitePve_Star_X:
    //        case EM_eSevenDay.eSevenDay_Rebels_Beat_X:
    //        case EM_eSevenDay.eSevenDay_AllServ_Welfare_X:
    //        case EM_eSevenDay.eSevenDay_Login_Count:
    //        case EM_eSevenDay.eSevenDay_Retain63:
    //            break;
    //    }
    //    UITool.f_GotoPage(ccUIManage.GetInstance().f_GetUIHandler(UINameConst.MainMenu), PageName, param);
    //}
    private List<BasePoolDT<long>> _FindListForPageNum(int PageNum)
    {
        List<BasePoolDT<long>> tLIst = new List<BasePoolDT<long>>();
        for (int i = 0; i < SeleDayList.Count; i++)
        {
            //Data2为切页    进一步缩小位置
            if (SeleDayList[i].iData2 == PageNum)
                tLIst.Add(SeleDayList[i]);
        }
        return tLIst;
    }


    private List<BasePoolDT<long>> _FindListForPageNum(List<BasePoolDT<long>> PoolDT, int PageNum)
    {
        List<BasePoolDT<long>> tLIst = new List<BasePoolDT<long>>();
        for (int i = 0; i < PoolDT.Count; i++)
        {
            //Data2为切页    进一步缩小位置
            if (PoolDT[i].iData2 == PageNum)
                tLIst.Add(PoolDT[i]);
        }
        return tLIst;
    }
























    //private bool _IsAccomplish(int Id, int Condition1, int Condition2)
    //{
    //    TeamPoolDT tTeamPoolDT;
    //    int NeedCondition1 = 0;
    //    int NeedCondition2 = 0;
    //    switch ((EM_eSevenDay)Id)
    //    {
    //        case EM_eSevenDay.eSevenDay_Login:
    //            return true;
    //        case EM_eSevenDay.eSevenDay_Recharge:
    //            break;
    //        case EM_eSevenDay.eSevenDay_Lv:
    //            return Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) >= Condition1;
    //        case EM_eSevenDay.eSevenDay_Power:
    //            return Data_Pool.m_TeamPool.f_GetTotalBattlePower() >= Condition1;
    //        case EM_eSevenDay.eSevenDay_MainPve:
    //            for (int i = 0; i < Data_Pool.m_DungeonPool.f_GetAll().Count; i++)
    //            {
    //                if (!Data_Pool.m_DungeonPool.f_CheckChapterLockState(Data_Pool.m_DungeonPool.f_GetAll()[i] as DungeonPoolDT))
    //                {
    //                    NeedCondition1 = i;
    //                }
    //            }
    //            return NeedCondition1 >= Condition1;
    //        case EM_eSevenDay.eSevenDay_BattleNum:
    //            return Data_Pool.m_TeamPool.f_GetAll().Count >= Condition1;
    //        case EM_eSevenDay.eSevenDay_BattleQuality:
    //            for (int i = 0; i < Data_Pool.m_TeamPool.f_GetAll().Count; i++)
    //            {
    //                tTeamPoolDT = Data_Pool.m_TeamPool.f_GetAll()[0] as TeamPoolDT;
    //                if (tTeamPoolDT.m_CardPoolDT.m_CardDT.iImportant >= Condition2)
    //                {
    //                    NeedCondition1++;
    //                }
    //            }
    //            return NeedCondition1 >= Condition1;
    //        case EM_eSevenDay.eSevenDay_AllEquipQuality:
    //            if (Data_Pool.m_TeamPool.f_GetAll().Count >= Condition1)
    //            {
    //                for (int i = 0; i < Data_Pool.m_TeamPool.f_GetAll().Count; i++)
    //                {
    //                    tTeamPoolDT = Data_Pool.m_TeamPool.f_GetAll()[0] as TeamPoolDT;
    //                    if (Array.IndexOf(tTeamPoolDT.m_aEquipPoolDT, null) > 0)
    //                    {
    //                        return false;
    //                    }
    //                    else
    //                    {
    //                        for (int j = 0; j < tTeamPoolDT.m_aEquipPoolDT.Length; j++)
    //                        {
    //                            if (tTeamPoolDT.m_aEquipPoolDT[j].m_EquipDT.iColour >= Condition2)
    //                            {
    //                                NeedCondition1++;
    //                            }
    //                            else
    //                            {
    //                                return false;
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //            else
    //                return false;
    //            return NeedCondition1>=
    //        case EM_eSevenDay.eSevenDay_AllCardEquipStren:

    //        case EM_eSevenDay.eSevenDay_AllCardEquipOptim:

    //        case EM_eSevenDay.eSevenDay_AllDeviceStren:

    //        case EM_eSevenDay.eSevenDay_AllDeviceOptim:

    //        case EM_eSevenDay.eSevenDay_AllEquipRefine:

    //        case EM_eSevenDay.eSevenDay_MoneyDraw:

    //        case EM_eSevenDay.eSevenDay_HighDraw:

    //        case EM_eSevenDay.eSevenDay_ArenaRank:

    //        case EM_eSevenDay.eSevenDay_CardFit:

    //        case EM_eSevenDay.eSevenDay_CardFitDiamond:

    //        case EM_eSevenDay.eSevenDay_DeviceSynthesis:

    //        case EM_eSevenDay.eSevenDay_DeviceQuaSynth:

    //        case EM_eSevenDay.eSevenDay_Through:

    //        case EM_eSevenDay.eSevenDay_ThroughReset:

    //        case EM_eSevenDay.eSevenDay_ThroughStar:

    //        case EM_eSevenDay.eSevenDay_ThroughFight:

    //        case EM_eSevenDay.eSevenDay_CreateOrJoinLegion:

    //        case EM_eSevenDay.eSevenDay_LegionHighPublicity:

    //        case EM_eSevenDay.eSevenDay_LegionMidPublicity:

    //        case EM_eSevenDay.eSevenDay_LegionLowPublicity:

    //        case EM_eSevenDay.eSevenDay_DeviceShopRefresh:

    //        case EM_eSevenDay.eSevenDay_DeviceShopBuy:

    //        case EM_eSevenDay.eSevenDay_ArenaShopConsume:

    //        case EM_eSevenDay.eSevenDay_VirusShopConsume:

    //        case EM_eSevenDay.eSevenDay_DeviceShopConsume:

    //        case EM_eSevenDay.eSevenDay_ShopSyceeConsume:

    //        case EM_eSevenDay.eSevenDay_MainCardAdvanced:

    //        case EM_eSevenDay.eSevenDay_AllCardFriendShip:

    //        case EM_eSevenDay.eSevenDay_MaxFriendShip:

    //        case EM_eSevenDay.eSevenDay_ShoppingItem:

    //        case EM_eSevenDay.eSevenDay_MaxVirusDam:

    //        case EM_eSevenDay.eSevenDay_VirusScoreSum:

    //        case EM_eSevenDay.eSevenDay_AreaHighGuard:

    //        case EM_eSevenDay.eSevenDay_AreaGuard:

    //        case EM_eSevenDay.eSevenDay_DeviceMaxOptim:

    //        case EM_eSevenDay.eSevenDay_DeviceFrontShopConsume:

    //        default:
    //            break;
    //    }
    //    return false;
    //}
}
