using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 商店Pool
/// </summary>
public class ShopStorePool : BasePool {
    #region Pool数据管理
    /// <summary>
    /// 构造
    /// </summary>
    public ShopStorePool() : base("ShopStorePoolDT")
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
        SC_ShopRandInfo scShopRandInfo = new SC_ShopRandInfo();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_ShopRandInfo, scShopRandInfo, OnShopRandInfoCallback);
    }
    /// <summary>
    /// 处理商店消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnShopRandInfoCallback(object obj)
    {
        SC_ShopRandInfo scServerShopRandInfo = (SC_ShopRandInfo)obj;
        BasePoolDT<long> saveShopStorePoolDT = f_GetForId(scServerShopRandInfo.eRandShop);
        if (saveShopStorePoolDT == null)
        {
            ShopStorePoolDT shopStorePoolDT = new ShopStorePoolDT();
            shopStorePoolDT.iId = scServerShopRandInfo.eRandShop;
            shopStorePoolDT.eShopShop = (EM_ShopType)scServerShopRandInfo.eRandShop;
            shopStorePoolDT.propFreshTimes = scServerShopRandInfo.propFreshTimes;
            shopStorePoolDT.freeTimes = scServerShopRandInfo.freeTimes;
            shopStorePoolDT.lastTime = scServerShopRandInfo.lastTime;
            shopStorePoolDT.buyMask = scServerShopRandInfo.buyMask;
            if (scServerShopRandInfo.goodsId.Length != 6)
Debug.LogError("Unusual data length！");
            for (int i = 0; i < scServerShopRandInfo.goodsId.Length; i++)
            {
                int id = scServerShopRandInfo.goodsId[i];
                shopStorePoolDT.m_shopRandItemDTArray[i] = glo_Main.GetInstance().m_SC_Pool.m_ShopRandGoodsSC.f_GetSC(id);
            }
            f_Save(shopStorePoolDT);
        }
        else
        {
            ShopStorePoolDT shopStorePoolDT = saveShopStorePoolDT as ShopStorePoolDT;
            shopStorePoolDT.iId = scServerShopRandInfo.eRandShop;
            shopStorePoolDT.eShopShop = (EM_ShopType)scServerShopRandInfo.eRandShop;
            shopStorePoolDT.propFreshTimes = scServerShopRandInfo.propFreshTimes;
            shopStorePoolDT.freeTimes = scServerShopRandInfo.freeTimes;
            shopStorePoolDT.lastTime = scServerShopRandInfo.lastTime;
            shopStorePoolDT.buyMask = scServerShopRandInfo.buyMask;
            if (scServerShopRandInfo.goodsId.Length != 6)
Debug.LogError("Unusual data length！");
            for (int i = 0; i < scServerShopRandInfo.goodsId.Length; i++)
            {
                int id = scServerShopRandInfo.goodsId[i];
                shopStorePoolDT.m_shopRandItemDTArray[i] = glo_Main.GetInstance().m_SC_Pool.m_ShopRandGoodsSC.f_GetSC(id);
            }
        }
Debug.Log("Get Market Information");
    }
    /// <summary>
    /// 添加数据
    /// </summary>
    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {

    }
    /// <summary>
    /// 更新数据
    /// </summary>
    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {

    }
    #endregion
    /// <summary>
    /// 获取该商店类型的数据
    /// </summary>
    /// <param name="shopType">商店类型</param>
    /// <returns></returns>
    private ShopRandDT GetShopRandDTByShopType(EM_ShopType shopType)
    {
        List<NBaseSCDT> listAllShopRandItem = glo_Main.GetInstance().m_SC_Pool.m_ShopRandSC.f_GetAll();
        for (int i = 0; i < listAllShopRandItem.Count; i++)
        {
            ShopRandDT item = listAllShopRandItem[i] as ShopRandDT;
            if (item.iType == (int)shopType)
            {
                return item;
            }
        }
Debug.LogError("Error, market configuration not found！");
        return null;
    }
    #region 通讯接口
    /// <summary>
    /// 请求商店信息
    /// </summary>
    /// <param name="shopType">请求的商店类型</param>
    /// <param name="tSocketCallbackDT">回调</param>
    public void f_GetShopRandInfo(EM_ShopType shopType,SocketCallbackDT tSocketCallbackDT)
    {
        ////向服务器提交数据
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_ShopRandInfo, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add((byte)shopType);//商店类型
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_ShopRandInfo, bBuf);
    }
    /// <summary>
    /// 请求商店购买
    /// </summary>
    /// <param name="shopType">购买的商店类型</param>
    /// <param name="goodIndex">物品序号：0<=goodIndex<6</param>
    /// <param name="tSocketCallbackDT">回调</param>
    public void f_ShopRandBuy(EM_ShopType shopType, int goodIndex,SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_ShopRandBuy, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add((byte)shopType);//商店类型
        tCreateSocketBuf.f_Add((byte)goodIndex);//I8u goodsIdx;	// [0, SHOPRAND_GOODSNUM)
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_ShopRandBuy, bBuf);
    }
    /// <summary>
    /// 请求商店刷新
    /// </summary>
    /// <param name="shopType">需刷新的商店类型</param>
    /// <param name="tSocketCallbackDT">回调</param>
    public void f_ShopRandRefresh(EM_ShopType shopType, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_ShopRandRefresh, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add((byte)shopType);//商店类型
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_ShopRandRefresh, bBuf);
    }
    #endregion
}
