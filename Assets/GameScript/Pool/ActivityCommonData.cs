using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System;
using System.Collections.Generic;

public class ActivityCommonData : BaseProperty
{
    #region 活动铜雀台(领体力)
    private int lastInfoTimeGetPower = 0;
    private bool GetPowerIsInitForSer = false;//防止重复请求
    public bool isGetSycee;//是否得到元宝
    public bool isEatNoon = false;//中午是否吃过大餐
    public bool isEatNight = false;//晚上是否吃过大餐
    private ccCallback m_requestPowerInfoCallback;
    #endregion

    #region 活动招财符
    private int lastInfoTimeActLuckySymbol = 0;
    private bool ActLuckySymbolInitFromSer = false;//是否初始化
    public int ActLuckySymbolBuyTimes = 0;//招财符购买次数
    public int ActLuckySymbolIsGetMask = 0;//领取掩码
    #endregion

    #region 迎财神
    private int lastInfoTimeWelth = 0;
    private bool WelthInitFromSer = false;//查询只执行一次
    public int WealthDayTimes = 0;//当天迎财次数
    public int WealthTotalTimes = 0;//总的迎财次数
    public bool WealthBoxCanGet = false;//宝箱是否可以领取
    public int lastWealthTime = 0;//上一次迎财的时间
    public int WealthTotalFortune = 0;//财神总额
    #endregion

    #region 登录礼包（限时）
    private int lastInfoLoginGift = 0;
    private bool LoginGiftIsQuery = false;//查询只执行一次
    public int LoginGiftDay = 0;//限时登录天数
    public byte LoginGiftFlag = 0;//奖励领取标志 按位 giftFlg |=(1<< 天数)
    #endregion
    #region 登录礼包（限时）(开服豪礼)
    private int lastInfoLoginGiftNewServ = 0;
    private bool LoginGiftIsQueryNewServ = false;//查询只执行一次
    public int LoginGiftOpenSevDayNewServ = 0;//开服天数
    public int LoginGiftDayNewServ = 0;//限时登录天数
    public int NewServGiftGetFlag;//奖励领取标志 按位 giftFlg |=(1<< 天数)
    #endregion
    #region 月卡
    private int lastInfoMonthCard = 0;
    private bool MonthCardIsQuery = false;//查询只执行一次
    public bool m_MonthCardIsBuy25;//25元月卡是否购买
    public bool m_MonthCardIsCanGet25;//当天25元月卡是否可领取
    public bool m_MonthCardIsBuy50;//50元月卡是否购买
    public bool m_MonthCardIsCanGet50;//当天50元月卡是否可领取
    public int remainDay25;//月卡25剩余可领取天数
    public int remainDay50;//月卡50剩余可领取天数
    #endregion
    #region 首充
    public bool isFirstRecharge = false;//是否首次充值
    public bool isFirstRechargeGetAward = false;//已经首次充值,是否已经领取
    public bool isFirstRechargeInitServer = false;
    #endregion

    #region 手机绑定
    public bool isBindPhone = false;//是否已经绑定
    public bool isGetBindAward = false;//是否已经领取
    #endregion
    public ActivityCommonData():base(5)
    {
        RegSocketMessage();
    }
    /// <summary>
    /// 注册消息
    /// </summary>
    private void RegSocketMessage()
    {
        SC_GetPower scGetPower = new SC_GetPower();
        SC_LuckySymbol scLuckySymbol = new SC_LuckySymbol();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_GetPower, scGetPower, OnGetPowerCallback);
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_LuckySymbol, scLuckySymbol, OnLuckySymbolCallback);
        SC_WealthManInfo scWealthManInfo = new SC_WealthManInfo();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_WealthManInfo, scWealthManInfo, OnWealthManInfoCallback);
        SC_ActLoginGiftInfo scActLoginGiftInfo = new SC_ActLoginGiftInfo();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_ActLoginGiftInfo, scActLoginGiftInfo, OnLoginGiftInfoCallback);
        SC_ActLoginGift scActLoginGift = new SC_ActLoginGift();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_ActLoginGift, scActLoginGift, OnLoginGiftCallback);
        SC_ActLoginGiftNewServ scActLoginGiftNewServ = new SC_ActLoginGiftNewServ();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_ActLoginGiftNewServ, scActLoginGiftNewServ, OnLoginGiftCallbackNewServ);
        SC_NewServGift scNewServGift = new SC_NewServGift();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_NewServGift, scNewServGift, OnLoginGiftCallbackNewServGift); 
         SC_QueryMonthCard scQueryMonthCard = new SC_QueryMonthCard();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_QueryMonthCard, scQueryMonthCard, OnMonthCardSevCallback);
        SC_PhoneBidnInfo phoneBindInfo = new SC_PhoneBidnInfo();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_PhoneBindInfo, phoneBindInfo, OnBindPhoneCallBack);

        SC_Award phoneAward = new SC_Award();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_PhoneBindAward, phoneAward, OnGetAwardCallBack);
        //SC_FirstRechargeInfo scFirstRechargeInfo = new SC_FirstRechargeInfo();
        //GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_FirstRechargeInfo, scFirstRechargeInfo, OnFirstRechargeInfoCallback);
    }
    #region 活动迎财神服务器回调
    /// <summary>
    /// 迎财神服务器回调
    /// </summary>
    /// <param name="data"></param>
    private void OnWealthManInfoCallback(object data)
    {
        SC_WealthManInfo scWealthManInfo = (SC_WealthManInfo)data;
        this.WealthDayTimes = scWealthManInfo.todayTimeCount;
        this.WealthTotalTimes = scWealthManInfo.todayTimeCount;
        //昨天次数大于等于3而且昨天没有领取宝箱
        if (scWealthManInfo.yesterdayTimeCount >= 3 && scWealthManInfo.yesterdayBoxCount <= 0)
            this.WealthTotalTimes += scWealthManInfo.yesterdayTimeCount;

        this.WealthBoxCanGet = (WealthTotalTimes >= 6 && scWealthManInfo.todayBoxCount <= 0) ? true : false;
        this.lastWealthTime = scWealthManInfo.latestTime;
        int level = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        this.WealthTotalFortune = GetTotalFortuneByLevel(level) / 6 * this.WealthTotalTimes;

        CheckWealthManRedPoint();
    }
    private int GetTotalFortuneByLevel(int level)
    {
        WealthManDT dt = glo_Main.GetInstance().m_SC_Pool.m_WealthManSC.f_GetSC(level) as WealthManDT;
        if (dt == null)
        {
            List<NBaseSCDT> listData = glo_Main.GetInstance().m_SC_Pool.m_WealthManSC.f_GetAll();
            dt = listData[listData.Count - 1] as WealthManDT;
        }
        return dt.iTotalRewardCount;
    }
    private void CheckWealthManRedPoint()
    {
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.WealthMan);
        //1.可领取聚宝盆
        if (WealthBoxCanGet)
        {
            Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.WealthMan);
            ActivityPage.SortAct(EM_ActivityType.WealthMan);
            return;
        }
        //2.时间倒计时
        int times = Data_Pool.m_ActivityCommonData.WealthDayTimes;
        int lastWealthTime = Data_Pool.m_ActivityCommonData.lastWealthTime;
        if(times >= 0 && times < 3)//每天0-3次
        {
            int timeLeft = 15 * 60 - (GameSocket.GetInstance().f_GetServerTime() - Data_Pool.m_ActivityCommonData.lastWealthTime);
            if (timeLeft <= 0)
            {
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.WealthMan);
                ActivityPage.SortAct(EM_ActivityType.WealthMan);
            }
        }
    }
    #endregion
    #region 活动铜雀台服务器回调
    private void CheckTime()
    {
        int timeNow = GameSocket.GetInstance().f_GetServerTime();
        DateTime dateTime = ccMath.time_t2DateTime(timeNow);
        if (dateTime.Hour >= 12 && dateTime.Hour < 14)
        {
            isEatNoon = true;
        }
        else if (dateTime.Hour >= 18 && dateTime.Hour < 20)
        {
            isEatNight = true;
        }
    }
    private void CheckGetPowerRedPoint()
    {
        int timeNow = GameSocket.GetInstance().f_GetServerTime();
        DateTime dateTime = ccMath.time_t2DateTime(timeNow);
        if (!isEatNoon && dateTime.Hour >= 12 && dateTime.Hour < 14)
        {
            Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.BanQuet);
            Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.BanQuet);
            ActivityPage.SortAct(EM_ActivityType.Banquet);
        }
        else if (!isEatNight && dateTime.Hour >= 18 && dateTime.Hour < 20)
        {
            Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.BanQuet);
            Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.BanQuet);
            ActivityPage.SortAct(EM_ActivityType.Banquet);
        }
        else
        {
            Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.BanQuet);
        }
    }
    /// <summary>
    /// 领取体力消息
    /// </summary>
    /// <param name="data"></param>
    private void OnGetPowerCallback(object data)
    {
        SC_GetPower scGetPower = (SC_GetPower)data;
Debug.Log("Message received！ "+scGetPower.IsGetSycee);
        switch (scGetPower.IsGetSycee)
        {
            case 1://获得元宝 GetSycee
                CheckTime();
                isGetSycee = true;
                break;
            case 0://没有获得元宝 NotGetSycee
                CheckTime();
                isGetSycee = false;
                break;
            case 3://中午领取,晚上没有NoonAndNotNight
                isEatNoon = true;
                isEatNight = false;
                break;
            case 4://中午晚上都领取NoonAndNight
                isEatNoon = true;
                isEatNight = true;
                break;
            case 5://晚上领取,中午没有NightAndNotNoon
                isEatNoon = false;
                isEatNight = true;
                break;
            case 6://中午晚上都没领取NotNoonAndNight
                isEatNoon = false;
                isEatNight = false;
                break;
        }
        if(m_requestPowerInfoCallback != null)
            m_requestPowerInfoCallback(null);
        CheckGetPowerRedPoint();
    }
    #endregion
    #region 活动招财符服务器回调
    /// <summary>
    /// 招财符
    /// </summary>
    /// <param name="data"></param>
    private void OnLuckySymbolCallback(object data)
    {
        SC_LuckySymbol scLuckySymbol = (SC_LuckySymbol)data;
        ActLuckySymbolIsGetMask = scLuckySymbol.Status;
Debug.Log("Bitmask："+scLuckySymbol.Status+": Purchases： "+scLuckySymbol.Count);
        DateTime date = ccMath.time_t2DateTime(scLuckySymbol.LatestTime);
        ActLuckySymbolBuyTimes = scLuckySymbol.Count;
        CheckLuckySymbolRedPoint();
    }
    private void CheckLuckySymbolRedPoint()
    {
        int price = 0;
        ActLuckySymbolDT actLuckySymbolDT = (ActLuckySymbolDT)glo_Main.GetInstance().m_SC_Pool.m_ActLuckySymbolSC.f_GetSC(ActLuckySymbolBuyTimes + 1);
        if (actLuckySymbolDT == null)
        {
            List<NBaseSCDT> listData = glo_Main.GetInstance().m_SC_Pool.m_ActLuckySymbolSC.f_GetAll();
            price = ((ActLuckySymbolDT)listData[listData.Count - 1]).iBuyPrice;
        }
        price = actLuckySymbolDT.iBuyPrice;
        if (price <= 0)
        {
            Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.LuckySymbolFree);
            Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.LuckySymbolFree);
            ActivityPage.SortAct(EM_ActivityType.LuckySymbol);
        }
        else
        {
            Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.LuckySymbolFree);
        }
    }
    #endregion
    #region 登录送礼服务器回调
    private void OnLoginGiftInfoCallback(object data)
    {
        SC_ActLoginGiftInfo scActLoginGift = (SC_ActLoginGiftInfo)data;
        LoginGiftDay = scActLoginGift.totalDay;
        LoginGiftFlag = scActLoginGift.giftFlag;
        CheckLoginGiftRedPoint();
    }

    private void OnLoginGiftCallback(object data)
    {
        SC_ActLoginGift scActLoginGift = (SC_ActLoginGift)data;
        LoginGiftFlag = scActLoginGift.giftFlag;
        CheckLoginGiftRedPoint();
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
    private void CheckLoginGiftRedPoint()
    {
        bool LoginGiftIsGet = false;
        List<NBaseSCDT> listLoginGiftDT = glo_Main.GetInstance().m_SC_Pool.m_ActLoginGiftSC.f_GetAll();
        int index = 0;
        for (int i = 0; i < listLoginGiftDT.Count; i++)
        {
            BasePoolDT<long> item = new BasePoolDT<long>();
            ActLoginGiftDT actLoginGiftDT = listLoginGiftDT[i] as ActLoginGiftDT;
            if (actLoginGiftDT.itype == 1 && CommonTools.f_CheckTime(GetTimeByTimeStr(actLoginGiftDT.szStartTime), GetTimeByTimeStr(actLoginGiftDT.szEndTime)))//时间匹配.同时间段才有效
            {
                index ++;
            }
        }
        for (int i = 1; i <= LoginGiftDay && i <= index; i++)
        {
            if (!BitTool.BitTest(LoginGiftFlag, (ushort)i))
            {
                LoginGiftIsGet = true;
            }
        }
        if (LoginGiftIsGet)
        {
            Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.LoginGift);
            Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.LoginGift);
            ActivityPage.SortAct(EM_ActivityType.LoginGift);
        }
        else
        {
            Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.LoginGift);

        }
    }
    #endregion
    #region 登录送礼服务器回调(开服豪礼)
    private void OnLoginGiftCallbackNewServ(object data)
    {
        SC_ActLoginGiftNewServ scActLoginGift = (SC_ActLoginGiftNewServ)data;
        LoginGiftOpenSevDayNewServ = scActLoginGift.uCurDay;
        NewServGiftGetFlag = scActLoginGift.giftFlag;
        CheckLoginGiftRedPointNewServ();
    }

    private void OnLoginGiftCallbackNewServGift(object data)
    {
        SC_NewServGift scActLoginGift = (SC_NewServGift)data;
        LoginGiftOpenSevDayNewServ = scActLoginGift.uCurDay;
        NewServGiftGetFlag = scActLoginGift.giftFlag;
        CheckLoginGiftRedPointNewServ();
    }

    private void CheckLoginGiftRedPointNewServ()
    {
        ////TsuCode  - check red point
        //int[] arr = CommonTools.int2Arr(NewServGiftGetFlag);
        //bool checkGiftCanGet = false;
        //for (int i = 0; i < arr.Length; i++)
        //    if (arr[i] == 1) checkGiftCanGet = true;
        ////
        //List<NBaseSCDT> listLoginGiftDT = glo_Main.GetInstance().m_SC_Pool.m_ActLoginGiftSC.f_GetAll();
        //int index = 0;
        //for (int i = 0; i < listLoginGiftDT.Count; i++)
        //{
        //    BasePoolDT<long> item = new BasePoolDT<long>();
        //    ActLoginGiftDT actLoginGiftDT = listLoginGiftDT[i] as ActLoginGiftDT;
        //    if (actLoginGiftDT.itype == 2)//时间匹配.同时间段才有效
        //    {
        //        index++;
        //    }
        //}
        ////新服豪礼 1-8天
        ////if (IsOpenNewSvrGift() && !BitTool.BitTest(NewServGiftGetFlag, (ushort)LoginGiftOpenSevDayNewServ))
        //if (IsOpenNewSvrGift() && checkGiftCanGet==true) //TsuCode
        //{
        //    Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.LoginGiftNewServ);
        //    Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.LoginGiftNewServ);
        //    ActivityPage.SortAct(EM_ActivityType.LoginGiftNewServ);
        //}
        //else
        //{
        //
        //}

        //
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.LoginGiftNewServ);
        int loginGiftDay = Data_Pool.m_ActivityCommonData.LoginGiftOpenSevDayNewServ;
        int[] arr = CommonTools.int2Arr(NewServGiftGetFlag);
        List<NBaseSCDT> listLoginGiftDT = glo_Main.GetInstance().m_SC_Pool.m_ActLoginGiftSC.f_GetAll();
        List<ActLoginGiftDT> listData = new List<ActLoginGiftDT>();
        for (int i = 0; i < listLoginGiftDT.Count; i++)
        {
            ActLoginGiftDT actLoginGiftDT = listLoginGiftDT[i] as ActLoginGiftDT;
            if (actLoginGiftDT.itype == 2)
            {
                listData.Add(actLoginGiftDT);
            }
        }
        for (int i = 0; i < 7; i++)
        {
            ActLoginGiftDT actLoginGiftDT = listData[i];
            if (arr[actLoginGiftDT.iday - 1] == 1 && actLoginGiftDT.iday <= loginGiftDay)
            {
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.LoginGiftNewServ);
                ActivityPage.SortAct(EM_ActivityType.LoginGiftNewServ);
                break;
            }
        }
    }

    //判断新服好礼是否开放
    public bool IsOpenNewSvrGift()
    {
        //List<NBaseSCDT> listActLoginGiftDT = glo_Main.GetInstance().m_SC_Pool.m_ActLoginGiftSC.f_GetAll();
        //string startTime = "";
        //string endTime = "";
        //for (int i = 0; i < listActLoginGiftDT.Count; i++)
        //{
        //    ActLoginGiftDT actLoginGiftDT = listActLoginGiftDT[i] as ActLoginGiftDT;
        //    if (null == actLoginGiftDT || actLoginGiftDT.itype != 2)
        //    {
        //        continue;
        //    }
        //    if (actLoginGiftDT.iday == 1) {
        //        startTime = actLoginGiftDT.szStartTime;
        //    }
        //    if (actLoginGiftDT.iday == 7)
        //    {
        //        endTime = actLoginGiftDT.szEndTime;
        //    }
        //    if (startTime != "" && endTime != "")
        //    {
        //        break;
        //    }
        //}
        //if (startTime == "" || endTime == "") return false;
        //return CommonTools.f_CheckTime(GetTimeByTimeStr(startTime), GetTimeByTimeStr(endTime));

        //TsuCode - new
        //List<NBaseSCDT> listActLoginGiftDT = glo_Main.GetInstance().m_SC_Pool.m_ActLoginGiftSC.f_GetAll();
        //string startTime = "";
        //string endTime = "";
        //for (int i = 0; i < listActLoginGiftDT.Count; i++)
        //{
        //    ActLoginGiftDT actLoginGiftDT = listActLoginGiftDT[i] as ActLoginGiftDT;
        //    if (null == actLoginGiftDT || actLoginGiftDT.itype != 2)
        //    {
        //        continue;
        //    }
        //    if (actLoginGiftDT.iday == 1)
        //    {
        //        startTime = actLoginGiftDT.szStartTime;
        //    }
        //    if (actLoginGiftDT.iday == 7)
        //    {
        //        endTime = actLoginGiftDT.szEndTime;
        //    }
        //    if (startTime != "" && endTime != "")
        //    {
        //        break;
        //    }
        //}
        //if (startTime == "" || endTime == "") return false;
        ////return CommonTools.f_CheckTime(GetTimeByTimeStr(startTime), GetTimeByTimeStr(endTime));
        //int stTime = Int32.Parse(startTime);
        //int edTime = Int32.Parse(endTime);
        //return CommonTools.f_CheckActIsOpenForOpenSeverTime_TsuFunc(stTime, edTime);
        int[] arr = CommonTools.int2Arr(NewServGiftGetFlag);
        bool checkGiftCanGet = false;
        for (int i = 0; i < arr.Length; i++)
            if (arr[i] == 1) checkGiftCanGet = true;
        return checkGiftCanGet;
    }

    #endregion
    #region 手机绑定服务器回调
    /// <summary>
    /// 手机绑定状态回调
    /// </summary>
    /// <param name="data"></param>
    private void OnBindPhoneCallBack(object data)
    {
        SC_PhoneBidnInfo phoneBindInfo = (SC_PhoneBidnInfo)data;
        isBindPhone = phoneBindInfo.bindFlag == 1;
        isGetBindAward = phoneBindInfo.getAwardFlag == 1;
    }
    /// <summary>
    /// 获得奖励回调
    /// </summary>
    /// <param name="iData1"></param>
    /// <param name="iData2"></param>
    /// <param name="iNum"></param>
    /// <param name="aData"></param>
    private void OnGetAwardCallBack(int iData1, int iData2, int iNum, System.Collections.ArrayList aData)
    {
        List<AwardPoolDT> tAwardPoolList = new List<AwardPoolDT>();
        foreach (SC_Award item in aData)
        {
            AwardPoolDT taward = new AwardPoolDT();
            taward.f_UpdateByInfo(item.resourceType, item.resourceId, item.resourceNum);
            tAwardPoolList.Add(taward);
        }
        if (tAwardPoolList.Count > 0)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN, new object[] { tAwardPoolList });
            tAwardPoolList.Clear();
        }
    }

    #endregion
    #region 手机绑定外部接口
    /// <summary>
    /// 请求发送验证码
    /// </summary>
    /// <param name="tSocketCallbackDT"></param>
    public void f_RequestSendPhoneCode(SocketCallbackDT tSocketCallbackDT, string phone)
    {
        if (phone.Length != 11)
        {
UITool.Ui_Trip("Số điện thoại không hợp lệ");
            return;
        }
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_SendPhoneCode, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(phone, 14);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_SendPhoneCode, bBuf);
        //SDKChannelRoleInfo.CHANNEL_FLAG_MAX_LEN
    }
    /// <summary>
    /// 请求绑定手机
    /// </summary>
    /// <param name="tSocketCallbackDT"></param>
    public void f_RequestBindPhone(SocketCallbackDT tSocketCallbackDT, string phone, int code)
    {
        if (phone.Length != 11)
        {
UITool.Ui_Trip("Số điện thoại không hợp lệ");
            return;
        }
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_PhoneBind, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(phone, 14);
        tCreateSocketBuf.f_Add(code);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_PhoneBind, bBuf);
    }
    /// <summary>
    /// 请求领取绑定奖励
    /// </summary>
    /// <param name="tSocketCallbackDT"></param>
    public void f_RequestGetBindPhoneAward(SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_PhoneBindAward, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_PhoneBindAward, bBuf);
    }
    #endregion
    #region 月卡服务器回调
    private void OnMonthCardSevCallback(object data)
    {
        SC_QueryMonthCard scQueryMonthCard = (SC_QueryMonthCard)data;
        remainDay25 = scQueryMonthCard.remainDays25;
        m_MonthCardIsBuy25 = scQueryMonthCard.remainDays25 > 0 ? true : false;
        m_MonthCardIsCanGet25 = scQueryMonthCard.remainDays25 > 0 && scQueryMonthCard.awardTimes25 <= 0 ? true : false;

        remainDay50 = scQueryMonthCard.remainDays50;
        m_MonthCardIsBuy50 = scQueryMonthCard.remainDays50 > 0 ? true : false;
        m_MonthCardIsCanGet50 = scQueryMonthCard.remainDays50 > 0 && scQueryMonthCard.awardTimes50 <= 0 ? true : false;
        CheckMonthCardRedPoint();
    }
    private void CheckMonthCardRedPoint()
    {
        if ((m_MonthCardIsBuy25 && m_MonthCardIsCanGet25) || (m_MonthCardIsBuy50 && m_MonthCardIsCanGet50))
        {
            Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.MonthCardGet);
            Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.MonthCardGet);
            //ActivityPage.SortAct(EM_ActivityType.MonthCard);// move supermarket
        }
        else
            Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.MonthCardGet);
    }
    #endregion
    #region 首充服务器回调
    private void OnFirstRechargeInfoCallback(object data)
    {
        //SC_FirstRechargeInfo scFirstRechargeInfo = (SC_FirstRechargeInfo)data;
        //isFirstRecharge = scFirstRechargeInfo.m_BuyState == 1 ? true : false;
        //isFirstRechargeGetAward = scFirstRechargeInfo.m_GetState == 1 ? true : false;
        //isFirstRechargeInitServer = true;
        //ShowFirstRechargeRedDot();
    }
    public bool isSeeRechargePage = false;//看一次首充页面红点消失
    public void ShowFirstRechargeRedDot()
    {
        //Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.FirstRecharge);
        //if (isFirstRecharge)
        //{
        //    if(!isFirstRechargeGetAward)
        //        Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.FirstRecharge);
        //}
        //else
        //{
        //    if (!isSeeRechargePage)
        //        Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.FirstRecharge);
        //}
    }
    #endregion
    #region 铜雀台外部接口
    /// <summary>
    /// 吃大餐
    /// </summary>
    /// <param name="tSocketCallbackDT"></param>
    public void f_Eat(SocketCallbackDT tSocketCallbackDT, ccCallback getPowerInfoCallback)
    {
        m_requestPowerInfoCallback = getPowerInfoCallback;
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_GetPower, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_GetPower, bBuf);
    }
    public void f_GetPowerInfo(ccCallback getPowerInfoCallback)
    {
        if (!CommonTools.f_CheckSameDay(lastInfoTimeGetPower, GameSocket.GetInstance().f_GetServerTime()))
        {
            GetPowerIsInitForSer = false;
        }
        if (GetPowerIsInitForSer)
        {
            CheckGetPowerRedPoint();
            if (getPowerInfoCallback != null)
                getPowerInfoCallback(eMsgOperateResult.OR_Succeed);
            return;
        }
        GetPowerIsInitForSer = true;
        lastInfoTimeGetPower = GameSocket.GetInstance().f_GetServerTime();
        m_requestPowerInfoCallback = getPowerInfoCallback;
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_QueryGetPower, bBuf);
    }
    #endregion
    #region 迎财神外部接口
    /// <summary>
    /// 领取奖励（满6次）
    /// </summary>
    /// <param name="tSocketCallbackDT"></param>
    public void f_WealthManFortune(SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_GetWealthFortune, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_GetWealthFortune, bBuf);
    }
    public void f_WealthManGet(SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_GetWealth, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_GetWealth, bBuf);
    }
    public void f_QueryWealthManInfo(SocketCallbackDT tSocketCallbackDT)
    {
        if (!CommonTools.f_CheckSameDay(lastInfoTimeWelth, GameSocket.GetInstance().f_GetServerTime()))
        {
            WelthInitFromSer = false;
        }
        if (WelthInitFromSer)
        {
            CheckWealthManRedPoint();
            if (tSocketCallbackDT != null)
                tSocketCallbackDT.m_ccCallbackSuc(eMsgOperateResult.OR_Succeed);
            return;
        }
        WelthInitFromSer = true;
        lastInfoTimeWelth = GameSocket.GetInstance().f_GetServerTime();
        if(tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_QueryWealth, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_QueryWealth, bBuf);
    }
    #endregion
    #region 招财符外部接口
    /// <summary>
    /// 招财符信息
    /// </summary>
    /// <param name="tSocketCallbackDT"></param>
    public void f_LuckySymbolInfo(SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_LuckySymbol, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_LuckySymbol, bBuf);
    }
    public void f_LuckySymbolGet(int GetIndex, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_GetTreasureBox, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add((byte)GetIndex);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_GetTreasureBox, bBuf);
    }
    public void f_QueryLuckySymbolInfo(SocketCallbackDT tSocketCallbackDT)
    {
        if (!CommonTools.f_CheckSameDay(lastInfoTimeActLuckySymbol, GameSocket.GetInstance().f_GetServerTime()))
        {
            ActLuckySymbolInitFromSer = false;
        }
        if (ActLuckySymbolInitFromSer)
        {
            CheckLuckySymbolRedPoint();
            if (tSocketCallbackDT != null)
                tSocketCallbackDT.m_ccCallbackSuc(eMsgOperateResult.OR_Succeed);
            return;
        }
        ActLuckySymbolInitFromSer = true;
        lastInfoTimeActLuckySymbol = GameSocket.GetInstance().f_GetServerTime();
        if(tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_QueryLuckySymbol, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_QueryLuckySymbol, bBuf);
    }
    #endregion
    #region 登录送礼外部接口
    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="tSocketCallbackDT"></param>
    public void f_QueryLoginGift(SocketCallbackDT tSocketCallbackDT)
    {
        if (!CommonTools.f_CheckSameDay(lastInfoLoginGift, GameSocket.GetInstance().f_GetServerTime()))
        {
            LoginGiftIsQuery = false;
        }
        if (LoginGiftIsQuery)
        {
            CheckLoginGiftRedPoint();
            if (tSocketCallbackDT != null)
                tSocketCallbackDT.m_ccCallbackSuc(eMsgOperateResult.OR_Succeed);
            return;
        }
        LoginGiftIsQuery = true;
        lastInfoLoginGift = GameSocket.GetInstance().f_GetServerTime();
        if(tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_QueryActLoginGift, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_QueryActLoginGift, bBuf);
    }
    public void f_GetLoginGift(byte uGetDay, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_ActLoginGift, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(uGetDay);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_ActLoginGift, bBuf);
    }
    #endregion
    #region 登录送礼外部接口（开服豪礼）
    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="tSocketCallbackDT"></param>
    public void f_QueryLoginGiftNewServ(SocketCallbackDT tSocketCallbackDT)
    {
        if (!CommonTools.f_CheckSameDay(lastInfoLoginGiftNewServ, GameSocket.GetInstance().f_GetServerTime()))
        {
            LoginGiftIsQueryNewServ = false;
        }
        if (LoginGiftIsQueryNewServ)
        {
            CheckLoginGiftRedPointNewServ();
            if (tSocketCallbackDT != null)
                tSocketCallbackDT.m_ccCallbackSuc(eMsgOperateResult.OR_Succeed);
            return;
        }
        LoginGiftIsQueryNewServ = true;
        lastInfoLoginGiftNewServ = GameSocket.GetInstance().f_GetServerTime();
        if (tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_QueryActLoginGiftNewServ, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_QueryActLoginGiftNewServ, bBuf);
    }
    public void f_GetLoginGiftNewServ(SocketCallbackDT tSocketCallbackDT,int id)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_ActLoginGiftNewServ, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(id);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_ActLoginGiftNewServ, bBuf);
    }
    #endregion
    #region 月卡外部接口
    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="tSocketCallbackDT"></param>
    public void f_QueryMonthCard(SocketCallbackDT tSocketCallbackDT)
    {
        if (!CommonTools.f_CheckSameDay(lastInfoMonthCard, GameSocket.GetInstance().f_GetServerTime()))
        {
            MonthCardIsQuery = false;
        }
        if (MonthCardIsQuery)
        {
            CheckMonthCardRedPoint();
            if (tSocketCallbackDT != null)
                tSocketCallbackDT.m_ccCallbackSuc(eMsgOperateResult.OR_Succeed);
            return;
        }
        MonthCardIsQuery = true;
        lastInfoMonthCard = GameSocket.GetInstance().f_GetServerTime();
        if (tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_QueryMonthCard, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_QueryMonthCard, bBuf);
    }
    /// <summary>
    /// 领取月卡元宝奖励
    /// </summary>
    /// <param name="tSocketCallbackDT"></param>
    public void f_GetMonthCardSycee(EM_MonthCardType monthCardType, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_GetMonthCardSycee, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add((byte)monthCardType);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_GetMonthCardSycee, bBuf);
    }
    #endregion
    #region 首次充值外部接口
    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="tSocketCallbackDT"></param>
    public void f_QueryFirstRecharge(SocketCallbackDT tSocketCallbackDT)
    {
        //if (isFirstRechargeInitServer)
        //{
        //    if (tSocketCallbackDT != null)
        //    {
        //        tSocketCallbackDT.m_ccCallbackSuc(null);
        //    }
        //    ShowFirstRechargeRedDot();
        //    return;
        //}
        //if(tSocketCallbackDT != null)
        //    GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_FirstRechargeInfo, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        //CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        //byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        //GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_FirstRechargeInfo, bBuf);
    }
    /// <summary>
    /// 领取首次充值奖励
    /// </summary>
    /// <param name="tSocketCallbackDT"></param>
    public void f_GetFirstRechargeAward(SocketCallbackDT tSocketCallbackDT)
    {
        //GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_FirstRecharge, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        //CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        //byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        //GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_FirstRecharge, bBuf);
    }
    #endregion
}
public enum EM_MonthCardType
{
    Card25 = 1,//25元月卡
    Card50 = 2,//50元月卡
}
