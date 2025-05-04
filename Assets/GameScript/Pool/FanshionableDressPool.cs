using UnityEngine;
using System.Collections;
using System;
using ccU3DEngine;

public class FanshionableDressPool : BasePool
{

    public FanshionableDressPool() : base("FanshionableDressPoolDT", true)
    {
        //g_CardPond = new BetterList<CardDT>();
    }

    #region Pool数据管理

    protected override void f_Init()
    {

        //FanshionableDressPoolDT tFanshionableDressPoolDT = new FanshionableDressPoolDT();
        //tFanshionableDressPoolDT.iId = 10000;
        //tFanshionableDressPoolDT.m_iCaridId = tServerData.equiperId;
        //tFanshionableDressPoolDT.m_iLimitTime = tServerData.limitTime;
        //tFanshionableDressPoolDT.m_iTempId = tServerData.tempId;
        //f_Save(tFanshionableDressPoolDT);

    }

    protected override void RegSocketMessage()
    {
        SC_FashionInfo tSC_FashionInfo = new SC_FashionInfo();
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_FashionInfo, tSC_FashionInfo, Callback_SocketData_Update);

    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
        SC_FashionInfo tServerData = (SC_FashionInfo)Obj;
        FanshionableDressPoolDT tFanshionableDressPoolDT = new FanshionableDressPoolDT();
        tFanshionableDressPoolDT.iId = tServerData.id;
        tFanshionableDressPoolDT.m_iCaridId = tServerData.equiperId;
        tFanshionableDressPoolDT.m_iLimitTime = tServerData.limitTime;
        tFanshionableDressPoolDT.m_iTempId = tServerData.tempId;
        f_Save(tFanshionableDressPoolDT);

        UpdateCardPoolInfor(tFanshionableDressPoolDT);
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
        SC_FashionInfo tServerData = (SC_FashionInfo)Obj;
        FanshionableDressPoolDT tFanshionableDressPoolDT = (FanshionableDressPoolDT)f_GetForId(tServerData.id);
        if (tFanshionableDressPoolDT == null)
        {
MessageBox.ASSERT("No external data，update failed");
        }
        tFanshionableDressPoolDT.m_iCaridId = tServerData.equiperId;
        tFanshionableDressPoolDT.m_iLimitTime = tServerData.limitTime;
        tFanshionableDressPoolDT.m_iTempId = tServerData.tempId;

        UpdateCardPoolInfor(tFanshionableDressPoolDT);
    }

    private void UpdateCardPoolInfor(FanshionableDressPoolDT tFanshionableDressPoolDT)
    {
        CardPoolDT tCardPoolDT = (CardPoolDT)Data_Pool.m_CardPool.f_GetForId(tFanshionableDressPoolDT.m_iCaridId);

        if (tCardPoolDT != null)
        {
            tCardPoolDT.m_FanshionableDressPoolDT = tFanshionableDressPoolDT;
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_UPDATE_USERINFOR);
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_UPDATE_MODELINFOR);//更新模型数据
        }
    }

    #endregion

    /// <summary>
    /// 穿时装
    /// </summary>
    public void f_Equip(long cardId, long iDressId, SocketCallbackDT callback)
    {
        ////向服务器提交数据
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_FashionEquip, callback.m_ccCallbackSuc, callback.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(iDressId);
        tCreateSocketBuf.f_Add(cardId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_FashionEquip, bBuf);
    }

    /// <summary>
    /// 脱时装
    /// </summary>
    public void f_UnEquip(long iDressId, SocketCallbackDT callback)
    {
        ////向服务器提交数据
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_FashionUnequip, callback.m_ccCallbackSuc, callback.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(iDressId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_FashionUnequip, bBuf);
    }

}
