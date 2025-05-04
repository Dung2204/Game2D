using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;
using System.Collections;

public class NewYearActivityPool
{
    #region  财神送礼数据
    List<AwardPoolDT> tAwardPoolList = new List<AwardPoolDT>();
    bool _IsFirst;
    private int _iMammonSendGiftTimes;
    public bool m_bMammonSendGiftIsGet
    {
        get
        {

            if(m_MammonSendGiftArr.m_DTId == 0)
                return false;
            return m_MammonSendGiftArr.m_state == 1;


            //int _StarTime;
            //GameParamDT _Time = glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.MammonSendGiftsOpenTime) as GameParamDT;
            //_StarTime = ccMath.f_Data2Int(_Time.iParam1);
            //NBaseSCDT tMammonSendGiftDt = null;

            //tMammonSendGiftDt = glo_Main.GetInstance().m_SC_Pool.m_MammonSendGiftSC.f_GetSC((GameSocket.GetInstance().f_GetServerTime() - _StarTime) / 86400 + 1);
            //for (int i = 0; i < glo_Main.GetInstance().m_SC_Pool.m_MammonSendGiftSC.f_GetAll().Count; i++)
            //{
            //    tMammonSendGiftDt = glo_Main.GetInstance().m_SC_Pool.m_MammonSendGiftSC.f_GetAll()[i];
            //    if (GameSocket.GetInstance().f_GetServerTime() - (_StarTime + tMammonSendGiftDt.iId * 86400) > 86400)
            //        continue;
            //    else
            //        break;
            //}
            //if (tMammonSendGiftDt == null)
            //    return false;
            //return m_MammonSendGiftArr.Contains((byte)tMammonSendGiftDt.iId);
        }
    }

    public bool m_bIsShowOnePage;

    public bool m_bMammonIsOpenTime
    {
        get
        {
            GameParamDT tGameParamDT = glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.MammonSendGiftsOpenTime) as GameParamDT;
            if(tGameParamDT == null)
                return false;
            int StarTime = ccMath.f_Data2Int(tGameParamDT.iParam1);
            int EndTime = ccMath.f_Data2Int(tGameParamDT.iParam2);
            int NowTime = GameSocket.GetInstance().f_GetServerTime();

            return NowTime > StarTime && NowTime < EndTime;

        }
    }

    ccCallback _MammonSendGiftInfoCallBack;
    #endregion


    #region 萌犬闹新春

    public int m_DogNewYearIntegral
    {
        get
        {
            return m_DogNewYearResidueIntegral + m_DogNewYearStepNum * 60;
        }
    }

    private int _DogNewYearIntegral;    //总积分

    public int m_DogNewYearResidueIntegral;//剩余积分

    public int m_DogNewYearStepNum;   //步数


    #endregion


    #region 累计充值
    #endregion


    #region 财神送礼(新)

    public SC_MammonGiftInfo m_MammonSendGiftArr = new SC_MammonGiftInfo();

    #endregion


    #region 节日兑换

    public Dictionary<int, FestivalExchangePoolDT> mFestivalData { get; private set; }
    public List<FestivalExchangePoolDT> mFestivalExchangePoolDTData = new List<FestivalExchangePoolDT>();
    public Dictionary<int, DealsEveryDayPoolDT> mDealsEveryDayPoolDTData = new Dictionary<int, DealsEveryDayPoolDT>();
    public Dictionary<int, ExclusionSpinPoolDT> mExclusionSpinPoolDTData = new Dictionary<int, ExclusionSpinPoolDT>();
    public bool m_bFesticalIsOpen;

     public CMsg_SC_ExclusionroTationInfo tCMsg_SC_ExclusionroTationInfo;
    #endregion
    public NewYearActivityPool()
    {
        _IsFirst = true;
        _iMammonSendGiftTimes = 0;
        m_bIsShowOnePage = false;
        m_bFesticalIsOpen = false;
        mFestivalData = new Dictionary<int, FestivalExchangePoolDT>();
        mDealsEveryDayPoolDTData = new Dictionary<int, DealsEveryDayPoolDT>();
        mExclusionSpinPoolDTData = new Dictionary<int, ExclusionSpinPoolDT>();
        RegSocketMessage();
    }

    private void RegSocketMessage()
    {
        SC_NewYearGiftInfo tSC_NewYearGiftInfo = new SC_NewYearGiftInfo();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_NewYearGiftInfo, tSC_NewYearGiftInfo, _MammonSendGiftInfo);


        SC_NewYearStepInfo tSC_NewYearStepInfo = new SC_NewYearStepInfo();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_NewYearStepInfo, tSC_NewYearStepInfo, _DogNewYearInfo);

        SC_Award tSC_Award = new SC_Award();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_NewYearGiftRet, tSC_Award, _MammonSendGiftAward);

        SC_MammonGiftInfo tSC_MammonGiftInfo = new SC_MammonGiftInfo();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_MammonGiftInfo, tSC_MammonGiftInfo, _MammonSendGiftInfo2);

        CMsg_SC_FestivalExchange tCMsg_SC_FestivalExchange = new CMsg_SC_FestivalExchange();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_FestivalExchange, tCMsg_SC_FestivalExchange, _FestivaExchange);

        CMsg_SC_DealsEveryDayInfo tCMsg_SC_DealsEveryDayInfo = new CMsg_SC_DealsEveryDayInfo();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_DealsEveryDayInfo, tCMsg_SC_DealsEveryDayInfo, f_DealsEveryDayInfo);

        CMsg_SC_ExclusionroTationInfo tCMsg_SC_ExclusionroTationInfo = new CMsg_SC_ExclusionroTationInfo();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_ExclusionroTationInfo, tCMsg_SC_ExclusionroTationInfo, f_ExclusionroTationInfo);

        CMsg_SC_ExclusionSpinInfo tCMsg_SC_ExclusionSpinInfo = new CMsg_SC_ExclusionSpinInfo();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_ExclusionSpinInfo, tCMsg_SC_ExclusionSpinInfo, f_ExclusionSpinInfo);
    }

    private void _MammonSendGiftInfo(object obj)
    {
        SC_NewYearGiftInfo tSC_NewYearGiftInfo = (SC_NewYearGiftInfo)obj;
        _iMammonSendGiftTimes = tSC_NewYearGiftInfo.todayTimes;
        _MammonSendGiftInfoCallBack(tSC_NewYearGiftInfo);
    }

    private void _DogNewYearInfo(object obj)
    {
        SC_NewYearStepInfo tSC_NewYearStepInfo = (SC_NewYearStepInfo)obj;
        m_DogNewYearResidueIntegral = tSC_NewYearStepInfo.uScore;
        m_DogNewYearStepNum = tSC_NewYearStepInfo.uStep;
    }

    private void _ConsumptionInfo(object obj)
    {
        //_Consumption=
    }

    private void _MammonSendGiftAward(int iData1, int iData2, int iNum, System.Collections.ArrayList aData)
    {
        foreach(SC_Award item in aData)
        {
            AwardPoolDT taward = new AwardPoolDT();
            taward.f_UpdateByInfo(item.resourceType, item.resourceId, item.resourceNum);
            tAwardPoolList.Add(taward);
        }
    }


    private void _MammonSendGiftInfo2(object obj)
    {
        SC_MammonGiftInfo tSC_MammonGiftInfo = (SC_MammonGiftInfo)obj;
        m_MammonSendGiftArr = tSC_MammonGiftInfo;
        //for (int i = 0; i < aData.Count; i++)
        //{
        //    m_MammonSendGiftArr[i] = ((SC_MammonGiftInfo)aData[i]).m_DTId;
        //}

        //if (!m_bMammonSendGiftIsGet)
        //{
        //    GameParamDT gameParamDTOpenLevel = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.NewYearOpenLevel) as GameParamDT);
        //    if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < gameParamDTOpenLevel.iParam1)
        //    {
        //        return;
        //    }
        //    Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.MammonSendGifts);
        //}
        //else
        //{
        //    Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.MammonSendGifts);
        //}
    }


    private void _FestivaExchange(int iData1, int iData2, int iNum, System.Collections.ArrayList aData)
    {
        CMsg_SC_FestivalExchange tCMsg_SC_FestivalExchange;

        for(int i = 0; i < aData.Count; i++)
        {
            tCMsg_SC_FestivalExchange = (CMsg_SC_FestivalExchange)aData[i];


            if(mFestivalData.ContainsKey(tCMsg_SC_FestivalExchange.mId))
            {
                mFestivalData[tCMsg_SC_FestivalExchange.mId].mNum = tCMsg_SC_FestivalExchange.mNum;

                mFestivalExchangePoolDTData.Find((FestivalExchangePoolDT a) =>
                {
                    return a.mId == tCMsg_SC_FestivalExchange.mId;
                }).mNum = tCMsg_SC_FestivalExchange.mNum;
            }
            else
            {
                FestivalExchangePoolDT tFestivalExchangePoolDT = new FestivalExchangePoolDT();
                tFestivalExchangePoolDT.mId = tCMsg_SC_FestivalExchange.mId;
                tFestivalExchangePoolDT.mNum = tCMsg_SC_FestivalExchange.mNum;
                mFestivalData.Add(tFestivalExchangePoolDT.mId, tFestivalExchangePoolDT);
                mFestivalExchangePoolDTData.Add(tFestivalExchangePoolDT);
            }
        }
    }
    #region 外部接口

    public void f_MammonSendGiftInfo(ccCallback MammonSendGift)
    {
        _MammonSendGiftInfoCallBack = MammonSendGift;
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_NewYearGiftInfo, bBuf);
    }


    public void f_MammonSendGiftGet(SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_NewYearGiftRecv, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_NewYearGiftRecv, bBuf);
    }

    /// <summary>
    /// 闹新春初始化
    /// </summary>
    /// <param name="tSocketCallbackDT"></param>
    public void f_SendDogNewYearInfo(SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_NewYearStepInfo, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_NewYearStepInfo, bBuf);
    }

    /// <summary>
    /// 闹新春前进
    /// </summary>
    /// <param name="tSocketCallbackDT"></param>
    public void f_SendDogNewYearStep(SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_NewYearStep, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_NewYearStep, bBuf);
    }

    public void f_ConsumptionInfo(SocketCallbackDT tSocketCallbackDT)
    {
        //累计充值初始化
        //GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_NewYearStep, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        //CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        //byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        //GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_NewYearStep, bBuf);
    }


    public void f_MammonSendGiftNewInfo(SocketCallbackDT tSocketCallbackDT)
    {
        if(tSocketCallbackDT == null)
            if(!_IsFirst)
                return;

        _IsFirst = false;
        if(tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_MammonGiftInfo, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_MammonGiftInfo, bBuf);
    }

    public void f_MammonSendGiftGet2(SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_MammonGiftAward, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_MammonGiftAward, bBuf);
    }
    public void f_FestivalExchangeInfo(SocketCallbackDT tSocketCallbackDT)
    {
        if(tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_FestivalExchangeInfo, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_FestivalExchangeInfo, bBuf);
    }

    public void f_GetFestivaExchange(int uid, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_FestivalExchange, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(uid);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_FestivalExchange, bBuf);
    }
    public bool f_CheckFestivaExchangeOpenBool()
    {
        //FestivalExchangeDT tFestivalExchangeDT = new FestivalExchangeDT();

        //tFestivalExchangeDT.iId = 1;
        //tFestivalExchangeDT.szResAward = "3;516;1";
        //tFestivalExchangeDT.szResNeed = "2;106;480#3;516;1";
        //tFestivalExchangeDT.iBeginTime = 20180518;
        //tFestivalExchangeDT.iEndTime = 20180524;


        //FestivalExchangePoolDT tFestivalExchangePoolDT = new FestivalExchangePoolDT();
        //tFestivalExchangePoolDT.mId = tFestivalExchangeDT.iId;
        //tFestivalExchangePoolDT.mNum = 0;
        //mFestivalData.Add(tFestivalExchangeDT.iId, tFestivalExchangePoolDT);
        //mFestivalExchangePoolDTData.Add(tFestivalExchangePoolDT);
        //m_bFesticalIsOpen = true;


        for(int i = 0; i < glo_Main.GetInstance().m_SC_Pool.m_FestivalExchangeSC.f_GetAll().Count; i++)
        {
            FestivalExchangeDT tFestivalExchangeDT = glo_Main.GetInstance().m_SC_Pool.m_FestivalExchangeSC.f_GetAll()[i] as FestivalExchangeDT;
            if(CommonTools.f_CheckTime(tFestivalExchangeDT.iBeginTime.ToString(), tFestivalExchangeDT.iEndTime.ToString()))
            {
                FestivalExchangePoolDT tFestivalExchangePoolDT = new FestivalExchangePoolDT();
                tFestivalExchangePoolDT.mId = tFestivalExchangeDT.iId;
                tFestivalExchangePoolDT.mNum = 0;
                mFestivalExchangePoolDTData.Add(tFestivalExchangePoolDT);
                mFestivalData.Add(tFestivalExchangePoolDT.mId, tFestivalExchangePoolDT);
                m_bFesticalIsOpen = true;
            }
        }

        return m_bFesticalIsOpen;

    }
    #endregion

    public void f_InitDealsEveryDayPoolDT()
    {
        for (int i = 0; i < glo_Main.GetInstance().m_SC_Pool.m_NewYearDealsEveryDaySC.f_GetAll().Count; i++)
        {
            NewYearDealsEveryDayDT tNewYearDealsEveryDayDT = glo_Main.GetInstance().m_SC_Pool.m_NewYearDealsEveryDaySC.f_GetAll()[i] as NewYearDealsEveryDayDT;
            DealsEveryDayPoolDT tDealsEveryDayPoolDT = new DealsEveryDayPoolDT();
            tDealsEveryDayPoolDT.mId = tNewYearDealsEveryDayDT.iId;
            if (!mDealsEveryDayPoolDTData.ContainsKey(tNewYearDealsEveryDayDT.iId))
            {
                mDealsEveryDayPoolDTData.Add(tNewYearDealsEveryDayDT.iId, tDealsEveryDayPoolDT);
            }
        }

        for (int i = 0; i < glo_Main.GetInstance().m_SC_Pool.m_NewYearExclusionSpinSC.f_GetAll().Count; i++)
        {
            NewYearExclusionSpinDT tNewYearExclusionSpinDT = glo_Main.GetInstance().m_SC_Pool.m_NewYearExclusionSpinSC.f_GetAll()[i] as NewYearExclusionSpinDT;
            ExclusionSpinPoolDT tExclusionSpinPoolDT = new ExclusionSpinPoolDT();
            tExclusionSpinPoolDT.mId = tNewYearExclusionSpinDT.iId;
            if (!mExclusionSpinPoolDTData.ContainsKey(tNewYearExclusionSpinDT.iId))
            {
                mExclusionSpinPoolDTData.Add(tNewYearExclusionSpinDT.iId, tExclusionSpinPoolDT);
            }
        }

        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_DealsEveryDayInfo, bBuf);
    }

    public void f_GetDealsEveryDaye(int uid, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_DealsEveryDayBuy, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(uid);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_DealsEveryDayBuy, bBuf);
    }

    public void f_GetDealsEveryDaye7(SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_DealsEveryDayBuy7, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_DealsEveryDayBuy7, bBuf);
    }

    public void f_ExclusionSpin(SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_ExclusionSpin, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_ExclusionSpin, bBuf);
    }

    private void f_DealsEveryDayInfo(int iData1, int iData2, int iNum, System.Collections.ArrayList aData)
    {
        CMsg_SC_DealsEveryDayInfo tCMsg_SC_DealsEveryDayInfo;

        for (int i = 0; i < aData.Count; i++)
        {
            tCMsg_SC_DealsEveryDayInfo = (CMsg_SC_DealsEveryDayInfo)aData[i];


            if (mDealsEveryDayPoolDTData.ContainsKey(tCMsg_SC_DealsEveryDayInfo.mId))
            {
                mDealsEveryDayPoolDTData[tCMsg_SC_DealsEveryDayInfo.mId].mGet = tCMsg_SC_DealsEveryDayInfo.mGet;
            }
            else
            {
                DealsEveryDayPoolDT tDealsEveryDayPoolDT = new DealsEveryDayPoolDT();
                tDealsEveryDayPoolDT.mId = tCMsg_SC_DealsEveryDayInfo.mId;
                tDealsEveryDayPoolDT.mGet = tCMsg_SC_DealsEveryDayInfo.mGet;
                mDealsEveryDayPoolDTData.Add(tCMsg_SC_DealsEveryDayInfo.mId, tDealsEveryDayPoolDT);
            }
        }
        CheckDealsEveryDayRedPoint();
    }

    private void f_ExclusionroTationInfo(object obj)
    {
        tCMsg_SC_ExclusionroTationInfo = (CMsg_SC_ExclusionroTationInfo)obj;
        CheckDealsEveryDayRedPoint();
    }

    public bool CanBuyDealsEveryDay()
    {
        return tCMsg_SC_ExclusionroTationInfo.mendtime < GameSocket.GetInstance().f_GetServerTime();
    }

    public bool CanExclusionSpin()
    {
        return tCMsg_SC_ExclusionroTationInfo.mcount > 0;
    }

    public void CheckDealsEveryDayRedPoint()
    {
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.DealsEveryDay);
        bool red7 = tCMsg_SC_ExclusionroTationInfo.mendtime > GameSocket.GetInstance().f_GetServerTime();

        for (int i = 1; i <= mDealsEveryDayPoolDTData.Count; i++)
        {
            DealsEveryDayPoolDT tDealsEveryDayPoolDT = mDealsEveryDayPoolDTData[i];
            if (!tDealsEveryDayPoolDT.IsCanGet)
            {
                if(tDealsEveryDayPoolDT.mDealsEveryDayPoolDT.iCondition <= 0)
                {
                    Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.DealsEveryDay);
                    break;
                }
                else
                {
                    if (red7)
                    {
                        Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.DealsEveryDay);
                        break;
                    }
                }
            }
        }
    }

    private void f_ExclusionSpinInfo(int iData1, int iData2, int iNum, System.Collections.ArrayList aData)
    {
        CMsg_SC_ExclusionSpinInfo tCMsg_SC_ExclusionSpinInfo;

        for (int i = 0; i < aData.Count; i++)
        {
            tCMsg_SC_ExclusionSpinInfo = (CMsg_SC_ExclusionSpinInfo)aData[i];


            if (mExclusionSpinPoolDTData.ContainsKey(tCMsg_SC_ExclusionSpinInfo.mId))
            {
                mExclusionSpinPoolDTData[tCMsg_SC_ExclusionSpinInfo.mId].mGet = tCMsg_SC_ExclusionSpinInfo.mGet;
            }
            else
            {
                ExclusionSpinPoolDT tExclusionSpinPoolDT = new ExclusionSpinPoolDT();
                tExclusionSpinPoolDT.mId = tCMsg_SC_ExclusionSpinInfo.mId;
                tExclusionSpinPoolDT.mGet = tCMsg_SC_ExclusionSpinInfo.mGet;
                mExclusionSpinPoolDTData.Add(tCMsg_SC_ExclusionSpinInfo.mId, tExclusionSpinPoolDT);
            }
        }
    }

    #region  红点检测
    private void _CheckRed()
    {

    }


    #endregion
}

