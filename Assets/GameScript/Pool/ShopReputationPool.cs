using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 商店Pool
/// </summary>
public class ShopReputationPool : BasePool
{
    #region Pool数据管理
    /// <summary>
    /// 构造
    /// </summary>
    public ShopReputationPool() : base("ShopReputationPoolDT",true)
    {

    }
    /// <summary>
    /// 初始化
    /// </summary>
    protected override void f_Init()
    {
        List<NBaseSCDT> listDT = glo_Main.GetInstance().m_SC_Pool.m_ReputationShopSC.f_GetAll();
        for (int index = 0; index < listDT.Count; index++)
        {
            ReputationShopDT dt = listDT[index] as ReputationShopDT;
            ShopReputationPoolDT poolDT = new ShopReputationPoolDT();
            poolDT.iId = dt.iId;
            poolDT.buyTimes = 0;
            poolDT.m_ReputationShopDT = dt;
            f_Save(poolDT);
        }
    }
    /// <summary>
    /// 注册消息
    /// </summary>
    protected override void RegSocketMessage()
    {
        SC_QueryReputation scQueryReputation = new SC_QueryReputation();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_QueryReputation, scQueryReputation, Callback_SocketData_Update);
    }
    /// <summary>
    /// 添加数据
    /// </summary>
    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
        Debug.Log("1");
        SC_QueryReputation scQueryReputation = (SC_QueryReputation)Obj;
        ShopReputationPoolDT dt = f_GetForId(scQueryReputation.id) as ShopReputationPoolDT;
        if (dt == null)//没有找到则新增
        {
            ShopReputationPoolDT poolDT = new ShopReputationPoolDT();
            poolDT.buyTimes = 0;
            ReputationShopDT reputationShopDT = glo_Main.GetInstance().m_SC_Pool.m_ReputationShopSC.f_GetSC(scQueryReputation.id) as ReputationShopDT;
            if (null == reputationShopDT)
            {
                MessageBox.ASSERT("ShopReputationPool:f_Socket_AddData,ReputationShop null,id：" + scQueryReputation.id);
                return;
            }
            poolDT.m_ReputationShopDT = reputationShopDT;
            poolDT.iId = reputationShopDT.iId;
            f_Save(poolDT);
        }
        else
        {
            dt.buyTimes = scQueryReputation.buyTimes;
        }
    }
    /// <summary>
    /// 更新数据
    /// </summary>
    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
        Debug.Log("2");
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
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_QueryReputation, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_QueryReputation, bBuf);
    }
    /// <summary>
    /// 请求商店购买
    /// </summary>
    /// <param name="goodIndex">物品序号：0</param>
    /// <param name="tSocketCallbackDT">回调</param>
    public void f_ShopRandBuy(int goodIndex, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_Reputation, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf1 = new CreateSocketBuf();
        tCreateSocketBuf1.f_Add((short)goodIndex);//I8u goodsIdx;	// [0, SHOPRAND_GOODSNUM)
        byte[] bBuf = tCreateSocketBuf1.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_Reputation, bBuf);
    }
    #endregion
}
