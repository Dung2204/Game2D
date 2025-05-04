using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 限时折扣pool
/// </summary>
public class TimeDiscountPool : BasePool
{
    public int m_proc;//进度
    #region 充值优惠
    public DiscountRechargeDT m_DiscountRechargeDT = null;//额度
    public int m_StartTime = 0;//优惠开始时间
    public bool m_GetState = false;//领取状态（是否可以领取）
    #endregion
    #region 全民福利
    public int m_BuyCount = 0;//全服购买人数
    public List<BasePoolDT<long>> m_listOpenServ = new List<BasePoolDT<long>>();//全服购买人数
    #endregion
    public TimeDiscountPool() : base("",true)
    {

    }
    /// <summary>
    /// 初始化
    /// </summary>
    protected override void f_Init()
    {
        m_listOpenServ.Clear();
        List<NBaseSCDT> listData = glo_Main.GetInstance().m_SC_Pool.m_DiscountAllServSC.f_GetAll();
        for (int i = 0; i < listData.Count; i++)
        {
            DiscountAllServDT dt = listData[i] as DiscountAllServDT;
            TimeDiscountOpenServPoolDT poolDT = new TimeDiscountOpenServPoolDT();
            poolDT.iId = dt.iId;
            poolDT.m_buyTimes = 0;
            poolDT.m_DiscountAllServDT = dt;
            m_listOpenServ.Add(poolDT);
        }
    }
    /// <summary>
    /// 注册消息事件
    /// </summary>
    protected override void RegSocketMessage()
    {
        SC_DiscountPropInfo scDiscountPropInfo = new SC_DiscountPropInfo();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_DiscountPropInfo, scDiscountPropInfo, OnInfoCallback);
        SC_DiscountProp scDiscountProp = new SC_DiscountProp();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_DiscountProp, scDiscountProp, OnBuyCallback);
        SC_DiscountRechargeInfo scDiscountRechargeInfo = new SC_DiscountRechargeInfo();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_DiscountRechargeInfo, scDiscountRechargeInfo, OnRechargeInfo);
        SC_DiscountAllServInfo scDiscountAllServInfo = new SC_DiscountAllServInfo();
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_DiscountAllServInfo, scDiscountAllServInfo, OnOpenServ);
        SC_DiscountAllServ scDiscountAllServ = new SC_DiscountAllServ();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_DiscountAllServ, scDiscountAllServ, OnOpenServGet);
    }
    /// <summary>
    /// 全服购买人数
    /// </summary>
    private void OnOpenServ(int iData1, int iData2, int iNum, ArrayList aData)
    {
        m_BuyCount = iData1;
        foreach (SockBaseDT tData in aData)
        {
            SC_DiscountAllServInfo scDiscountAllServInfo = (SC_DiscountAllServInfo)tData;
            for (int i=0; i < m_listOpenServ.Count; i++)
            {
                TimeDiscountOpenServPoolDT poolDT = m_listOpenServ[i] as TimeDiscountOpenServPoolDT;
                if (scDiscountAllServInfo.id == poolDT.iId)
                {
                    poolDT.m_buyTimes = scDiscountAllServInfo.num;
                }
            }
        }
    }
    /// <summary>
    /// 全服购买领取
    /// </summary>
    /// <param name="obj"></param>
    private void OnOpenServGet(object obj)
    {
        SC_DiscountAllServ scDiscountAllServ = (SC_DiscountAllServ)obj;
        for (int i = 0; i < m_listOpenServ.Count; i++)
        {
            TimeDiscountOpenServPoolDT poolDT = m_listOpenServ[i] as TimeDiscountOpenServPoolDT;
            if (scDiscountAllServ.id == poolDT.iId)
            {
                poolDT.m_buyTimes++;
                break;
            }
        }
    }
    /// <summary>
    /// 充值优惠信息
    /// </summary>
    /// <param name="obj"></param>
    private void OnRechargeInfo(object obj)
    {
        SC_DiscountRechargeInfo scDiscountRechargeInfo = (SC_DiscountRechargeInfo)obj;
        m_DiscountRechargeDT = glo_Main.GetInstance().m_SC_Pool.m_DiscountRechargeSC.f_GetSC(scDiscountRechargeInfo.id) as DiscountRechargeDT;
        m_StartTime = scDiscountRechargeInfo.timeBeg;
        m_GetState = scDiscountRechargeInfo.state == 0 ? true : false;
    }
    /// <summary>
    /// 购买回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnBuyCallback(object obj)
    {
        SC_DiscountProp scDiscountProp = (SC_DiscountProp)obj;
        TimeDiscountPoolDT dt = Data_Pool.m_TimeDiscountPool.f_GetForId(scDiscountProp.id) as TimeDiscountPoolDT;
        dt.m_BuyTimes++;
        m_proc = scDiscountProp.iProc;
    }
    /// <summary>
    /// 折扣信息查询回调
    /// </summary>
    public void OnInfoCallback(object obj)
    {
        f_Clear();
        SC_DiscountPropInfo scDiscountPropInfo = (SC_DiscountPropInfo)obj;
        SC_DiscountPropInfoNode[] info = scDiscountPropInfo.info;
        m_proc = scDiscountPropInfo.iProc;
        for (int i = 0; i < info.Length; i++)
        {
            TimeDiscountPoolDT poolDT = new TimeDiscountPoolDT();
            poolDT.iId = info[i].id;
            poolDT.m_BuyTimes = info[i].buyTimes;
            poolDT.m_DiscountPropDT = glo_Main.GetInstance().m_SC_Pool.m_DiscountPropSC.f_GetSC(info[i].id) as DiscountPropDT;
            f_Save(poolDT);
        }
    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {

    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {

    }
    #region 限时折扣外部接口
    /// <summary>
    /// 查询
    /// </summary>
    public void f_RequestInfo(SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_DiscountPropInfo, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_DiscountPropInfo, bBuf);
    }
    /// <summary>
    /// 购买
    /// </summary>
    public void f_Buy(int giftId, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_DiscountProp, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(giftId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_DiscountProp, bBuf);
    }
    #endregion
    #region 充值优惠外部接口
    /// <summary>
    /// 查询信息
    /// </summary>
    public void f_ChargeInfo(SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_DiscountRechargeInfo, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_DiscountRechargeInfo, bBuf);
    }
    /// <summary>
    /// 领取
    /// </summary>
    /// <param name="tSocketCallbackDT"></param>
    public void f_ChargeGet(SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_DsicountRecharge, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_DsicountRecharge, bBuf);
    }
    #endregion
    #region 全民福利外部接口
    /// <summary>
    /// 查询信息
    /// </summary>
    public void f_OpenServInfo(SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_DiscountAllServInfo, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_DiscountAllServInfo, bBuf);
    }
    /// <summary>
    /// 领取
    /// </summary>
    public void f_OpenServGet(int id,SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_DiscountAllServ, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(id);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_DiscountAllServ, bBuf);
    }
    #endregion

    #region 检测限时折扣时间是否开启
    /// <summary>
    /// 检测限时折扣时间是否开启
    /// </summary>
    public bool f_CheckOpenTimeLimitActTime()
    {
        List<NBaseSCDT> listData = glo_Main.GetInstance().m_SC_Pool.m_DiscountPropSC.f_GetAll();
        for (int i = 0; i < listData.Count; i++)
        {
            DiscountPropDT data = listData[i] as DiscountPropDT;
            int Level = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
            if (Level >= data.iRankDownLimit && Level <= data.iRankUpLimit)
            {
                if (CommonTools.f_CheckTime(data.szBeginTime, data.szEndTime))
                {
                    return true;
                }
            }
        }
        return false;
    }
    #endregion
}
