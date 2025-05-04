using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System;
using System.Collections.Generic;
/// <summary>
/// 十万元宝
/// </summary>
public class TenSyceePool : BasePool
{
    bool mIsInitServer = false;
    int mlastInitTime = 0;
    #region Pool数据管理
    public TenSyceePool()
        : base("TenSyceePoolDT" , true)
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
        for (int i = 0 ; i < glo_Main.GetInstance().m_SC_Pool.m_SyceeAwardSC.f_GetAll().Count ; i++)
        {
            TenSyceePoolDT poolDT = new TenSyceePoolDT();
            poolDT.mSyceeAwardDT = glo_Main.GetInstance().m_SC_Pool.m_SyceeAwardSC.f_GetAll() [i] as SyceeAwardDT;
            poolDT.iId = poolDT.mSyceeAwardDT.iId;
            poolDT.mIsGet = false;
            f_Save(poolDT);
        }
    }
    /// <summary>
    /// 注册消息
    /// </summary>
    protected override void RegSocketMessage()
    {
        SC_SyceeAwardInfo scSyceeAwardInfo = new SC_SyceeAwardInfo();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_SyceeAwardInfo , scSyceeAwardInfo , OnSyceeAwardInfoCallback);
    }
    /// <summary>
    /// 回调
    /// </summary>
    private void OnSyceeAwardInfoCallback(object aData)
    {
        //foreach (SockBaseDT tData in aData)
        //{
        //    SC_SyceeAwardInfo scSyceeAwardInfo = (SC_SyceeAwardInfo)tData;


        //TenSyceePoolDT tenSyceePoolDT = f_GetForId((int)scSyceeAwardInfo.iId) as TenSyceePoolDT;
        //if (tenSyceePoolDT != null)
        //{
        //    tenSyceePoolDT.mIsGet = true;
        // }
        // else
        // {
        //     MessageBox.ASSERT("十万元宝服务器发送的id不存在");
        // }
        //}
        SC_SyceeAwardInfo scSyceeAwardInfo = (SC_SyceeAwardInfo)aData;
        List<BasePoolDT<long>> t = f_GetAll();
        for (int i = 0; i < t.Count; i++)
        {
            TenSyceePoolDT tTenSyceePoolDT = t[i] as TenSyceePoolDT;
            if ((scSyceeAwardInfo.awardFlag >> (int)tTenSyceePoolDT.iId-1) > 0) {
                tTenSyceePoolDT.mIsGet = true;
            }
        }
        
        mlastInitTime = GameSocket.GetInstance().f_GetServerTime();
        mIsInitServer = true;
        CheckRedPoint();
    }

    protected override void f_Socket_AddData(SockBaseDT Obj , bool bNew)
    {

    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {

    }
    /// <summary>
    /// 检查红点
    /// </summary>
    public void CheckRedPoint()
    {
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.TenSycee);

        if (!f_CheckOpen())
            return;

        //int createRoleTime = Data_Pool.m_UserData.m_CreateTime;
        List<BasePoolDT<long>> listPoolDT = f_GetAll();
        //int serverTime = GameSocket.GetInstance().f_GetServerTime();

        //DateTime dataTime1 = ccMath.time_t2DateTime(createRoleTime);
        //long dataTimeStart = ccMath.DateTime2time_t(new DateTime(dataTime1.Year , dataTime1.Month , dataTime1.Day));


        for (int i = 0 ; i < listPoolDT.Count ; i++)
        {
            TenSyceePoolDT poolDT = listPoolDT [i] as TenSyceePoolDT;
            //int dayCount = (int)(serverTime - dataTimeStart) / (24 * 60 * 60) + 1;
            if (!poolDT.mIsGet && Data_Pool.m_UserData.m_LoginDays >= poolDT.mSyceeAwardDT.iCondition)
            {
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.TenSycee);
                ActivityPage.SortAct(EM_ActivityType.TenSycee);
            }
        }
    }
    /// <summary>
    /// 检测活动有没有开启
    /// </summary>
    /// <returns></returns>
    public bool f_CheckOpen()
    {
        if (mIsInitServer)
        {
            List<BasePoolDT<long>> listPoolDT = f_GetAll();
            for (int i = 0 ; i < listPoolDT.Count ; i++)
            {
                TenSyceePoolDT poolDT = listPoolDT [i] as TenSyceePoolDT;
                if (!poolDT.mIsGet)
                    return true;
            }
            return false;
        }
        else
            return true;
    }
    #endregion

    #region 外部接口
    /// <summary>
    /// 请求十万元宝领取信息
    /// </summary>
    /// <param name="tSocketCallbackDT">信息回调</param>
    public void f_RequestSyceeInfo(SocketCallbackDT tSocketCallbackDT)
    {
        if (!CommonTools.f_CheckSameDay(GameSocket.GetInstance().f_GetServerTime() , mlastInitTime))
        {
            mIsInitServer = false;
        }
        if (mIsInitServer)
        {
            CheckRedPoint();
            if (tSocketCallbackDT != null)
                tSocketCallbackDT.m_ccCallbackSuc(null);
            return;
        }
        if (tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_SyceeAwardInfo , tSocketCallbackDT.m_ccCallbackSuc , tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte [] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_SyceeAwardInfo , bBuf);
    }
    /// <summary>
    /// 请求领取十万元宝
    /// </summary>
    /// <param name="tSocketCallbackDT">回调</param>
    public void f_GetSyceeAward(short id , SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_SyceeAward , tSocketCallbackDT.m_ccCallbackSuc , tSocketCallbackDT.m_ccCallbackFail);
        ////向服务器提交数据
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(id);
        byte [] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_SyceeAward , bBuf);
    }


    public void f_SortPoolData()
    {
        f_GetAll().Sort((BasePoolDT<long> a , BasePoolDT<long> b) =>
        {
            TenSyceePoolDT poolDTI = a as TenSyceePoolDT;
            TenSyceePoolDT poolDTJ = b as TenSyceePoolDT;
            if (poolDTI.mIsGet && !poolDTJ.mIsGet)
                return 1;
            else if (!poolDTI.mIsGet && poolDTJ.mIsGet)
                return -1;
            else
            {
                if (poolDTI.iId > poolDTJ.iId)
                    return 1;
                else if (poolDTI.iId < poolDTJ.iId)
                    return -1;
                return 0;
            }

        });

    }
    #endregion
}
