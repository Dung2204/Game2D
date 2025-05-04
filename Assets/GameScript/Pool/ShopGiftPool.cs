using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;

public class ShopGiftPool : BasePool
{
    private ccCallback m_giftInfoCallback = null;
    private bool isInitServerData = false;
    public ShopGiftPool() : base("ShopGiftPoolDT", true)
    {

    }


    #region Pool数据管理

    protected override void f_Init()
    {
        InitShopGift();
    }

    private void InitShopGift()
    {
        List<NBaseSCDT> listNBaseSCDT = glo_Main.GetInstance().m_SC_Pool.m_ShopGiftSC.f_GetAll();
        for (int i = 0; i < listNBaseSCDT.Count; i++)
        {
            ShopGiftDT dt = (ShopGiftDT)listNBaseSCDT[i];
            ShopGiftPoolDT poolData = new ShopGiftPoolDT();
            poolData.iId = dt.iId;
            poolData.m_buyTimes = 0;
            poolData.m_shopGiftDT = dt;
            f_Save(poolData);
        }
    }

    protected override void RegSocketMessage()
    {
        SC_ShopGiftInfo tSC_ShopGiftInfo = new SC_ShopGiftInfo();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_ShopGiftInfo, tSC_ShopGiftInfo, OnGiftInfoCallback);
    }
    private void OnGiftInfoCallback(object obj)
    {
        SC_ShopGiftInfo serverInfo = (SC_ShopGiftInfo)obj;
        List<BasePoolDT<long>> listAllData = f_GetAll();
        char[] buyMaskCharArray = serverInfo.buyMask.ToString().ToCharArray();
        //不够16，自动补0
        List<int> newMask = FillMask(buyMaskCharArray, 16 - buyMaskCharArray.Length);
        //if (buyMaskCharArray.Length != listAllData.Count)
        //    Debug.LogWarning("服务器发送的礼包数据长度和vip礼包数量不一致！");
        for (int i = 0; i < newMask.Count; i++)
        {
            if (i > listAllData.Count - 1)
            {
                break;
            }
            
            (listAllData[i] as ShopGiftPoolDT).m_buyTimes = newMask[i];
        }
        isInitServerData = true;
        if (m_giftInfoCallback != null)
        {
            m_giftInfoCallback(eMsgOperateResult.OR_Succeed);
            m_giftInfoCallback = null;
        }
        CheckRedDot();
    }
    private List<int> FillMask(char[] oriMask,int fillCount)
    {
        List<int> newMask = new List<int>();
        for (int i = 0; i < fillCount; i++)
        {
            newMask.Add(0);
        }
        for (int j = 0; j < oriMask.Length; j++)
        {
            newMask.Add(int.Parse(oriMask[j].ToString()));
        }
        return newMask;
    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {

    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {

    }
    #endregion
    //以下为外部调用接口
    /// <summary>
    /// 检测红点（有可购买的礼包）
    /// </summary>
    private void CheckRedDot()
    {
        List<BasePoolDT<long>> _packsList = Data_Pool.m_ShopGiftPool.f_GetAll();
        int vipLevel = UITool.f_GetNowVipLv();
        int canBuyNum = 0;
        int playerSycee = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee);
        for (int i = _packsList.Count - 1; i >= 0; i--)
        {
            ShopGiftPoolDT shopGiftPoolDT = _packsList[i] as ShopGiftPoolDT;
            if (shopGiftPoolDT.m_buyTimes < shopGiftPoolDT.m_shopGiftDT.iTimes && shopGiftPoolDT.m_shopGiftDT.iVipLimit <= vipLevel 
                && playerSycee > shopGiftPoolDT.m_shopGiftDT.iNewNum)
                canBuyNum++;
        }
        if (canBuyNum > 0)
            Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.ShopGiftBuy);
        else
            Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.ShopGiftBuy);
    }

    /// <summary>
    /// 请求礼包信息列表
    /// </summary>
    /// <param name="tSocketCallbackDT"></param>
    public void f_GetNewShop(ccCallback m_giftInfoSucCallback)
    {
        ////向服务器提交数据
        if (isInitServerData)
        {
            if(m_giftInfoCallback != null)
                m_giftInfoSucCallback(eMsgOperateResult.OR_Succeed);
            CheckRedDot();
            return;
        }
        m_giftInfoCallback = m_giftInfoSucCallback;
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_ShopGiftInfo, bBuf);
    }

    /// <summary>
    /// 购买礼包
    /// </summary>
    /// <param name="sellId">购买礼包id</param>
    /// <param name="tSocketCallbackDT">结果返回</param>
    public void f_Buy(int sellId, SocketCallbackDT tSocketCallbackDT)
    {
        ////向服务器提交数据
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_ShopGiftBuy, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(sellId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_ShopGiftBuy, bBuf);
    }

}