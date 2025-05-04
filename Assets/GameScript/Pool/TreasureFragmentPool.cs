using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;

public class TreasureFragmentPool : BasePool
{
    public TreasureFragmentPool() : base("TreasureFragmentPoolDT", true)
    {
    }

    #region Pool管理
    protected override void f_Init()
    {
    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
       SC_TreasureFragmentInfo tServerData = (SC_TreasureFragmentInfo)Obj;
        TreasureFragmentPoolDT tPoolDataDT = new TreasureFragmentPoolDT();
        tPoolDataDT.iId = tServerData.id;
        tPoolDataDT.m_iTempleteId = tServerData.tempId;
        tPoolDataDT.m_num = tServerData.num;
        Data_Pool.m_AwardPool.f_AddAward(EM_ResourceType.TreasureFragment, tPoolDataDT.m_iTempleteId, tPoolDataDT.m_num);
        f_Save(tPoolDataDT);
        Data_Pool.m_TreasurePool.f_CheckCanFixTreasure();
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
        SC_TreasureFragmentInfo tServerData = (SC_TreasureFragmentInfo)Obj;
        TreasureFragmentPoolDT tPoolDataDT = (TreasureFragmentPoolDT)f_GetForId(tServerData.id);
        if (tPoolDataDT == null)
        {
MessageBox.ASSERT("No data，update failed");
        }
        Data_Pool.m_AwardPool.f_AddAward(EM_ResourceType.TreasureFragment, tPoolDataDT.m_iTempleteId, tServerData.num- tPoolDataDT.m_num);
        tPoolDataDT.m_iTempleteId = tServerData.tempId;
        tPoolDataDT.m_num = tServerData.num;
        Data_Pool.m_TreasurePool.f_CheckCanFixTreasure();
    }

    protected override void RegSocketMessage()
    {
        SC_TreasureFragmentInfo tSC_TreasureInit = new SC_TreasureFragmentInfo();
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_TreasureFragmentInfo, tSC_TreasureInit, Callback_SocketData_Update);
        stPoolDelData tstPoolDelData = new stPoolDelData();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_TreasureFragmentRemove, tstPoolDelData, Callback_Del);
    }
    protected void Callback_Del(object Obj)
    {
        stPoolDelData tServerData = (stPoolDelData)Obj;
        f_Delete(tServerData.iId);
        Data_Pool.m_TreasurePool.f_CheckCanFixTreasure();
    }
    #endregion
    /// <summary>
    /// 法宝碎片合成
    /// </summary>
    /// <param name="TreasureId"></param>
    /// <param name="tSocketCallbackDT"></param>
    public void f_Synthesis(int TreasureId,SocketCallbackDT tSocketCallbackDT)
    {

        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_TreasureSynthesis, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(TreasureId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_TreasureSynthesis, bBuf);
    }
    public void _Sort()
    {
        f_GetAll().Sort((BasePoolDT<long> a, BasePoolDT<long> b) =>
        {

            return ((TreasureFragmentPoolDT)a).m_TreasureFragmentsDT.iImportant > ((TreasureFragmentPoolDT)b).m_TreasureFragmentsDT.iImportant ? -1 : 1;
        });
    }
    /// <summary>
    /// 根据模板Id获取已拥有的数目
    /// </summary>
    /// <param name="templateId"></param>
    /// <returns></returns>
    public int f_GetHaveNumByTemplate(int templateId)
    {
        int result = 0;
        foreach (TreasureFragmentPoolDT item in f_GetAll())
        {
            if (item.m_iTempleteId == templateId)
            {
                result += item.m_num;
            }
        }
        return result;
    }
}
