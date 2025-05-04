using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System;
using System.Collections.Generic;
/// <summary>
/// 签到
/// </summary>
public class SignPool : BasePool {
    private int lastSignInfoTime = 0;
    ccCallback tRequestSignCallback;//请求签到信息回调
    ccCallback tSignCallbackSuc;//签到成功回调

    public int GrandSignResetTimeStamp;
    #region Pool数据管理
    public SignPool() : base("SignPoolDT", true)
    {
        
    }
    /// <summary>
    /// 初始化
    /// </summary>
    protected override void f_Init()
    {
        InitData();
    }
    /// <summary>
    /// 初始化消息
    /// </summary>
    public void InitData()
    {
        SignPoolDT signPoolDTSign = new SignPoolDT();
        signPoolDTSign.iId = (int)SignType.DaySign;
        signPoolDTSign.signSubType = 1;
        signPoolDTSign.m_ActivityType = SignType.DaySign;
        signPoolDTSign.signedCount = 0;
        f_Save(signPoolDTSign);
        SignPoolDT signPoolDTGrandSign = new SignPoolDT();
        signPoolDTGrandSign.iId = (int)SignType.GrandSign;
        signPoolDTGrandSign.signSubType = 1;
        signPoolDTGrandSign.m_ActivityType = SignType.GrandSign;
        signPoolDTGrandSign.signedCount = 0;
        f_Save(signPoolDTGrandSign);
    }
    /// <summary>
    /// 注册消息
    /// </summary>
    protected override void RegSocketMessage()
    {
        SC_UserSigned scUserSigned = new SC_UserSigned();
        SC_IsUserSigned scIsUserSigned = new SC_IsUserSigned();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_UserSigned, scUserSigned, OnUserSignedCallback);
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_IsUserSigned, scIsUserSigned, OnUserIsSignedCallback);
    }
    /// <summary>
    /// 返回是否签到消息
    /// </summary>
    /// <param name="data"></param>
    private void OnUserIsSignedCallback(object data)
    {
        MessageBox.ASSERT("TSULOG SIGN POOL OnUserIsSignedCallback");
        SC_IsUserSigned scIsUserSigned = (SC_IsUserSigned)data;
        SignPoolDT signPoolDTSign = f_GetForId((int)SignType.DaySign) as SignPoolDT;
        signPoolDTSign.signSubType = scIsUserSigned.iType;
        signPoolDTSign.signedCount = scIsUserSigned.SignedDayCount;
        signPoolDTSign.isSignToday = scIsUserSigned.Signed == 1 ? true : false;

        SignPoolDT signPoolDTGrandSign = f_GetForId((int)SignType.GrandSign) as SignPoolDT;
        signPoolDTGrandSign.signSubType = scIsUserSigned.iType;
        signPoolDTGrandSign.signedCount = scIsUserSigned.GrandSignedDayCount;
        signPoolDTGrandSign.isSignToday = scIsUserSigned.GrandSigned == 1 ? true : false;

        GrandSignResetTimeStamp = scIsUserSigned.GrandSignResetTimeStamp;
        if (tRequestSignCallback != null)
            tRequestSignCallback(data);
        lastSignInfoTime = GameSocket.GetInstance().f_GetServerTime();
        //m_RechanrgeToday = scIsUserSigned.RechargeCount;
        CheckRedPoint();
    }
    public List<SignedDT> f_GetSignData(int signIType)
    {
        MessageBox.ASSERT("TSULOG SIGN POOL f_GetSignData");
        List<SignedDT> listSignDT = new List<SignedDT>();
        List<NBaseSCDT> listSignSC = glo_Main.GetInstance().m_SC_Pool.m_SignedSC.f_GetAll();
        for (int i = 0; i < listSignSC.Count; i++)
        {
            SignedDT signedDT = listSignSC[i] as SignedDT;
            if (signedDT.iType == signIType)
            {
                listSignDT.Add(signedDT);
            }
        }
        return listSignDT;
    }
    /// <summary>
    /// 用户签到
    /// </summary>
    private void OnUserSignedCallback(object data)
    {
        MessageBox.ASSERT("TSULOG SIGN POOL OnUserSignedCallback");
        SC_UserSigned scUserSigned = (SC_UserSigned)data;
Debug.Log("Get login message");
        SignPoolDT signOriPoolDT = f_GetForId((int)scUserSigned.signedMask) as SignPoolDT;
        if (signOriPoolDT == null)
        {
            SignPoolDT signPoolDT = new SignPoolDT();
            signPoolDT.iId = (int)scUserSigned.signedMask;
            signPoolDT.signSubType = scUserSigned.iType;
            signPoolDT.m_ActivityType = (SignType)scUserSigned.signedMask;
            signPoolDT.signedCount = scUserSigned.eSignedCount;
            f_Save(signPoolDT);
        }
        else
        {
            signOriPoolDT.signSubType = scUserSigned.iType;
            signOriPoolDT.m_ActivityType = (SignType)scUserSigned.signedMask;
            signOriPoolDT.signedCount = scUserSigned.eSignedCount;
            signOriPoolDT.isSignToday = true;
        }
        tSignCallbackSuc(data);
        CheckRedPoint();
    }
    private void OnGrandSignedCallback(object data)
    {

    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
        
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {

    }
    public bool isSeeGrandSignPage = false;//是否看了豪华签到页面（看一次消失红点）
    /// <summary>
    /// 检查红点
    /// </summary>
    public void CheckRedPoint()
    {
        SignPoolDT signPoolDTSign = f_GetForId((int)SignType.DaySign) as SignPoolDT;
        SignPoolDT signPoolDTGrandSign = f_GetForId((int)SignType.GrandSign) as SignPoolDT;
        //红点(每日签到，豪华签到)
        if (!signPoolDTSign.isSignToday)
        {
            Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.UserSign);
            Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.UserSign);
            ActivityPage.SortAct(EM_ActivityType.DaySignIn);
        }
        else
        {
            Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.UserSign);
        }
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.GrandSign);
        if (!signPoolDTGrandSign.isSignToday)
        {
            if (!isSeeGrandSignPage)//是否打开过页面
            {
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.GrandSign);
                ActivityPage.SortAct(EM_ActivityType.GrandSignIn);
            }
            if (Data_Pool.m_RechargePool.f_GetAllRechageMoneyToday() >= 6)
            {
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.GrandSign);
                ActivityPage.SortAct(EM_ActivityType.GrandSignIn);
            }
        }
    }
    #endregion

    #region 外部接口
    /// <summary>
    /// 返回该活动今日是否签到
    /// </summary>
    /// <param name="signType"></param>
    /// <returns></returns>
    public bool CheckIsSignToday(SignType signType)
    {
        MessageBox.ASSERT("TSULOG SIGN POOL CheckIsSignToday");
        SignPoolDT signPoolDTSign = f_GetForId((int)signType) as SignPoolDT;
        return signPoolDTSign.isSignToday;
    }
    public bool f_RequestIsSignToday(ccCallback tCallback)
    {
        MessageBox.ASSERT("TSULOG SIGN POOL f_RequestIsSignToday");
        if (CommonTools.f_CheckSameDay(lastSignInfoTime, GameSocket.GetInstance().f_GetServerTime()))
        {
            CheckRedPoint();
            return false;
        }
        this.tRequestSignCallback = tCallback;
        ////向服务器提交数据
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_IsUserSigned, bBuf);
        return true;
    }
    /// <summary>
    /// 请求签到信息
    /// </summary>
    /// <param name="signType">签到类别 1每日签到 2.豪华签到</param>
    /// <param name="tSocketCallbackDT">回调</param>
    public void f_GetShopRandInfo(SignType signType, SocketCallbackDT tSocketCallbackDT)
    {
        if(tSocketCallbackDT != null)
            tSignCallbackSuc = tSocketCallbackDT.m_ccCallbackSuc;
        ////向服务器提交数据
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_UserSigned, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add((int)signType);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_UserSigned, bBuf);
    }
    #endregion
}
/// <summary>
/// 签到类型
/// </summary>
public enum SignType
{
    DaySign = 0,//0.每日签到
    GrandSign = 1,//1.豪华签到
}
