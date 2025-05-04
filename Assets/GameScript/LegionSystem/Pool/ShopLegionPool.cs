using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 军团商店Pool
/// </summary>
public class ShopLegionPool : BasePool
{
    #region Pool数据管理
    /// <summary>
    /// 构造
    /// </summary>
    public ShopLegionPool() : base("ShopLegionPoolDT", true)
    {
    }
    /// <summary>
    /// 初始化
    /// </summary>
    protected override void f_Init()
    {
        List<NBaseSCDT> listDT = glo_Main.GetInstance().m_SC_Pool.m_LegionShopSC.f_GetAll();
        for (int index = 0; index < listDT.Count; index++)
        {
            LegionShopDT dt = listDT[index] as LegionShopDT;
            ShopLegionPoolDT poolDT = new ShopLegionPoolDT();
            poolDT.iId = dt.iId;
            poolDT.buyTimes = 0;
            poolDT.m_LegionShopDT = dt;
            f_Save(poolDT);
        }
    }
    /// <summary>
    /// 注册消息
    /// </summary>
    protected override void RegSocketMessage()
    {
        SC_QueryLegionShop scQueryLegion = new SC_QueryLegionShop();
        GameSocket.GetInstance().f_RegMessage_Int0((int)LegionSocketCmd.SC_LegionShop, scQueryLegion, Callback_SocketData_Update);
    }
    /// <summary>
    /// 添加数据
    /// </summary>
    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
        //Debug.Log("1");
        SC_QueryLegionShop scQueryLegion = (SC_QueryLegionShop)Obj;
        ShopLegionPoolDT dt = f_GetForId(scQueryLegion.id) as ShopLegionPoolDT;
        if (dt == null)//没有找到则新增
        {
            ShopLegionPoolDT poolDT = new ShopLegionPoolDT();
            poolDT.iId = dt.iId;
            poolDT.buyTimes = 0;
            poolDT.m_LegionShopDT = glo_Main.GetInstance().m_SC_Pool.m_LegionShopSC.f_GetSC(scQueryLegion.id) as LegionShopDT;
            f_Save(poolDT);
        }
        else
        {
            dt.buyTimes = scQueryLegion.buyTimes;
        }
    }
    /// <summary>
    /// 更新数据
    /// </summary>
    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
        f_Socket_AddData(Obj, false);
    }
    #endregion
    #region 通讯接口
    /// <summary>
    /// 请求商店信息
    /// </summary>
    /// <param name="tSocketCallbackDT">回调</param>
    public void f_GetShopRandInfo(SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionShopInfo, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionShopInfo, bBuf);
    }
    /// <summary>
    /// 请求商店购买
    /// </summary>
    /// <param name="goodIndex">物品序号：0</param>
    /// <param name="tSocketCallbackDT">回调</param>
    public void f_ShopRandBuy(int goodIndex, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionShop, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf1 = new CreateSocketBuf();
        tCreateSocketBuf1.f_Add((short)goodIndex);
        byte[] bBuf = tCreateSocketBuf1.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionShop, bBuf);
    }
    #endregion
}
