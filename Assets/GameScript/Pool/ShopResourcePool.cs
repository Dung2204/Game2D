using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;

public class ShopResourcePool : BasePool
{
    private bool isInitServer = false;
    private int lastTimeInfo = 0;
    public ShopResourcePool():base("ShopResourcePoolDT", true)
    {

    }


    #region Pool数据管理

    protected override void f_Init()
    {
        InitShop();
    }

    /// <summary>
    /// 初始商店的默认购买列表
    /// </summary>
    private void InitShop()
    {
        List<NBaseSCDT> listNBaseSCDT = glo_Main.GetInstance().m_SC_Pool.m_ShopResourceSC.f_GetAll();
        for (int i = 0; i < listNBaseSCDT.Count; i++)
        {
            ShopResourceDT dt = (ShopResourceDT)listNBaseSCDT[i];
            ShopResourcePoolDT poolData = new ShopResourcePoolDT(dt);
            f_Save(poolData);
        }
    }

    protected override void RegSocketMessage()
    {
        SC_ShopResourceInfo tSC_ShopResourceInfo = new SC_ShopResourceInfo();
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_ShopResourceInfo, tSC_ShopResourceInfo, Callback_SocketData_Update);
        //stPoolDelData tstPoolDelData = new stPoolDelData();
        //GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_CardRemove, tstPoolDelData, Callback_Del);
    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
        SC_ShopResourceInfo tServerData = (SC_ShopResourceInfo)Obj;
        ShopResourcePoolDT tPoolDataDT = new ShopResourcePoolDT();
        tPoolDataDT.iId = tServerData.id;
        tPoolDataDT.m_iTempleteId = tServerData.id;
        tPoolDataDT.m_iBuyTimes = tServerData.buyTimes;
        f_Save(tPoolDataDT);
        lastTimeInfo = GameSocket.GetInstance().f_GetServerTime();
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
        SC_ShopResourceInfo tServerData = (SC_ShopResourceInfo)Obj;
        ShopResourcePoolDT tPoolDataDT = (ShopResourcePoolDT)f_GetForId(tServerData.id);
        if (tPoolDataDT == null)
        {
MessageBox.ASSERT("BaseGoods information does not exist，update failed");
        }
        tPoolDataDT.m_iBuyTimes = tServerData.buyTimes;
        lastTimeInfo = GameSocket.GetInstance().f_GetServerTime();
    }

    protected void Callback_Del(object Obj)
    {
        //stPoolDelData tServerData = (stPoolDelData)Obj;
        //f_Delete(tServerData.iId);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //以下为外部调用接口


    /// <summary>
    /// 请求道具商店列表
    /// </summary>
    /// <param name="tSocketCallbackDT"></param>
    public void f_GetNewShop(SocketCallbackDT tSocketCallbackDT)
    {
        if (!CommonTools.f_CheckSameDay(lastTimeInfo, GameSocket.GetInstance().f_GetServerTime()))
        {
            isInitServer = false;
        }
        if (isInitServer)
        {
            tSocketCallbackDT.m_ccCallbackSuc(eMsgOperateResult.OR_Succeed);
            return;
        }
        isInitServer = true;
        ////向服务器提交数据
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_ShopResourceInfo, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_ShopResourceInfo, bBuf);
    }

    /// <summary>
    /// 购买道具
    /// </summary>
    /// <param name="tShopResourcePoolDT">准备购买物品的数据结构</param>
    /// <param name="iBuyNum">购买数量</param>
    /// <param name="tSocketCallbackDT">结果返回</param>
    public void f_Buy(int iId, int iBuyNum, SocketCallbackDT tSocketCallbackDT)
    {       
        ////向服务器提交数据
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_ShopResourceBuy, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(iId);
        tCreateSocketBuf.f_Add(iBuyNum);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_ShopResourceBuy, bBuf);
    }

    /// <summary>
    /// 根据资源类型和资源Id 获得 商城数据
    /// </summary>
    /// <param name="type">资源类型</param>
    /// <param name="tempId">资源Id</param>
    /// <returns></returns>
    public BasePoolDT<long> f_GetShopPoolDtByTypeAndId(int type, int tempId)
    {
        BasePoolDT<long> result = f_GetAll().Find(delegate (BasePoolDT<long> item)
        {
            ShopResourcePoolDT poolDt = (ShopResourcePoolDT)item;
            return poolDt.m_ShopResourceDT.iType == type && poolDt.m_ShopResourceDT.iTempId == tempId;
        }
        );
        if (result == null)
MessageBox.ASSERT(string.Format("Getting Shop data failed,type:{0} tempId:{1}", type, tempId));
        return result;
    }
}
