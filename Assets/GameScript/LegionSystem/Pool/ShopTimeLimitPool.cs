using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 军团限时商店
/// </summary>
public class ShopTimeLimitPool : BasePool {
    #region Pool数据管理
    /// <summary>
    /// 构造
    /// </summary>
    public ShopTimeLimitPool() : base("ShopTimeLimitPoolDT",true)
    {

    }
    /// <summary>
    /// 初始化
    /// </summary>
    protected override void f_Init()
    {

    }
    /// <summary>
    /// 注册消息
    /// </summary>
    protected override void RegSocketMessage()
    {
        SC_LegionShopTimeLimitInfo scLegionShopTimeLimitInfo = new SC_LegionShopTimeLimitInfo();
        GameSocket.GetInstance().f_RegMessage_Int0((int)LegionSocketCmd.SC_LegionShopTimeLimit, scLegionShopTimeLimitInfo, Callback_SocketData_Update);
    }
    /// <summary>
    /// 处理商店消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnShopRandInfoCallback(object obj)
    {
        SC_LegionShopTimeLimitInfo scLegionShopTimeLimitInfo = (SC_LegionShopTimeLimitInfo)obj;
        ShopTimeLimitPoolDT poolDT = f_GetForId(scLegionShopTimeLimitInfo.id) as ShopTimeLimitPoolDT;
        if (poolDT == null)
        {
            poolDT = new ShopTimeLimitPoolDT();
            poolDT.iId = scLegionShopTimeLimitInfo.id;
            poolDT.m_myBuyTimes = scLegionShopTimeLimitInfo.pbuyTime;
            poolDT.m_abuyTimes = scLegionShopTimeLimitInfo.abuyTime;
            poolDT.m_LegionShopTimeLimitDT = glo_Main.GetInstance().m_SC_Pool.m_LegionShopTimeLimitSC.f_GetSC(scLegionShopTimeLimitInfo.id) as LegionShopTimeLimitDT;
            f_Save(poolDT);
        }
        else
        {
            poolDT.m_myBuyTimes = scLegionShopTimeLimitInfo.pbuyTime;
            poolDT.m_abuyTimes = scLegionShopTimeLimitInfo.abuyTime;
        }
    }
    /// <summary>
    /// 添加数据
    /// </summary>
    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
        OnShopRandInfoCallback(Obj);
    }
    /// <summary>
    /// 更新数据
    /// </summary>
    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
        OnShopRandInfoCallback(Obj);
    }
    #endregion
    #region 通讯接口
    /// <summary>
    /// 请求军团限时商店信息
    /// </summary>
    /// <param name="shopType">请求的商店类型</param>
    /// <param name="tSocketCallbackDT">回调</param>
    public void f_GetShopBuyTimesInfo(SocketCallbackDT tSocketCallbackDT)
    {
        f_Clear();
        ////向服务器提交数据
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionShopTimeLimitIDInfo, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionShopTimeLimitIDInfo, bBuf);
    }
    /// <summary>
    /// 请求军团限时商店购买
    /// </summary>
    /// <param name="goodID">物品序号：0<=goodID<6</param>
    /// <param name="tSocketCallbackDT">回调</param>
    public void f_ShopBuy(int goodID, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionShopTimeLimit, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(goodID);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionShopTimeLimit, bBuf);
    }
    #endregion
}
