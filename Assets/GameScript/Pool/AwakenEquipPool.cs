using System;
using System.Collections.Generic;
using System.Linq;
using ccU3DEngine;
using System.Text;

public class AwakenEquipPool : BasePool
{
    public AwakenEquipPool() : base("AwakenEquipPoolDT", true)
    {

    }
    #region Pool数据管理
    protected override void f_Init()
    {

    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
        SC_AwakenEquipInfo tServerData = (SC_AwakenEquipInfo)Obj;
        AwakenEquipPoolDT tPoolDataDT = new AwakenEquipPoolDT();
        tPoolDataDT.SetData5(tServerData.tempId);
        tPoolDataDT.iId = tServerData.id;
        tPoolDataDT.m_iTempleteId = tServerData.tempId;
        tPoolDataDT.m_num = tServerData.num;
        Data_Pool.m_AwardPool.f_AddAward(EM_ResourceType.AwakenEquip, tPoolDataDT.m_iTempleteId, tPoolDataDT.m_num);
        f_Save(tPoolDataDT);
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
        SC_AwakenEquipInfo tServerData = (SC_AwakenEquipInfo)Obj;
        AwakenEquipPoolDT tPoolDataDT = (AwakenEquipPoolDT)f_GetForId(tServerData.id);
        if (tPoolDataDT == null)
        {
            //f_Socket_AddData(tServerData,true);
MessageBox.ASSERT("No data，update failed");
            //return;
        }
        Data_Pool.m_AwardPool.f_AddAward(EM_ResourceType.AwakenEquip, tPoolDataDT.m_iTempleteId, tServerData.num - tPoolDataDT.m_num);
        tPoolDataDT.iId = tServerData.id;
        tPoolDataDT.m_iTempleteId = tServerData.tempId;
        tPoolDataDT.m_num = tServerData.num;
    }

    protected override void RegSocketMessage()
    {
        SC_AwakenEquipInfo tSC_AwakenEquip = new SC_AwakenEquipInfo();
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_AwakenEquipInfo, tSC_AwakenEquip, Callback_SocketData_Update);
        stPoolDelData tstPoolDelData = new stPoolDelData();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_AwakenEquipRemove, tstPoolDelData, Callback_Del);
    }
    protected void Callback_Del(object Obj)
    {
        stPoolDelData tServerData = (stPoolDelData)Obj;
        f_Delete(tServerData.iId);
    }
    #endregion

    /// <summary>
    /// 卡牌领悟装备
    /// </summary>
    /// <param name="cardId"></param>
    /// <param name="index"></param>
    /// <param name="tSocketCallbackDT"></param>
    public void f_Equip(long cardId, byte index, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_CardAwakenEquip, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(cardId);
        tCreateSocketBuf.f_Add(index);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CardAwakenEquip, bBuf);
    }

    public void f_Sythe(int id, int num, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_AwakenEquipSynthesis, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(id);
        tCreateSocketBuf.f_Add(num);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_AwakenEquipSynthesis, bBuf);
    }


    public void f_Sell(long id, int num, SocketCallbackDT tSocketCallbackDT) {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_AwakenEquipSell, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(id);
        tCreateSocketBuf.f_Add(num);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_AwakenEquipSell, bBuf);
    }
    /// <summary>
    /// 根据模板Id获取已拥有的数目
    /// </summary>
    /// <param name="templateId"></param>
    /// <returns></returns>
    public int f_GetHaveNumByTemplate(int templateId)
    {
        int result = 0;
        foreach (AwakenEquipPoolDT item in f_GetAll())
        {
            if (item.m_iTempleteId == templateId)
            {
                result = item.m_num;
                break;
            }
        }
        return result;
    }
}
