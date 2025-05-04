using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;

public class GodEquipFragmentPool : BasePool
{
    public GodEquipFragmentPool() : base("GodEquipFragmentPoolDT", true)
    {

    }

    #region Pool数据管理
    protected override void f_Init()
    {
    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
        SC_GodEquipFragmentInfo tServerData = (SC_GodEquipFragmentInfo)Obj;
        GodEquipFragmentPoolDT tPoolDataDT = new GodEquipFragmentPoolDT();
        tPoolDataDT.iId = tServerData.id;
        tPoolDataDT.m_iNum = tServerData.num;
        tPoolDataDT.m_iTempleteId = tServerData.tempId;
        Data_Pool.m_AwardPool.f_AddAward(EM_ResourceType.GodEquipFragment, tPoolDataDT.m_iTempleteId, tPoolDataDT.m_iNum);
        if (tPoolDataDT.m_iNum >= tPoolDataDT.m_EquipFragmentsDT.iBondNum)
        {
            Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.GodEquipBag_Fragment);
        }
        f_Save(tPoolDataDT);
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
        SC_GodEquipFragmentInfo tServerData = (SC_GodEquipFragmentInfo)Obj;
        GodEquipFragmentPoolDT tPoolDataDT = (GodEquipFragmentPoolDT)f_GetForId(tServerData.id);
        if (tPoolDataDT == null)
        {
            MessageBox.ASSERT("Không tồn tại thông tin BaseGoods，cập nhập thất bại");
        }

        Data_Pool.m_AwardPool.f_AddAward(EM_ResourceType.GodEquipFragment, tPoolDataDT.m_iTempleteId, tServerData.num - tPoolDataDT.m_iNum);
        if (tPoolDataDT.m_iNum >= tPoolDataDT.m_EquipFragmentsDT.iBondNum && tServerData.num < tPoolDataDT.m_EquipFragmentsDT.iBondNum)
        {
            Data_Pool.m_ReddotMessagePool.f_MsgSubtract(EM_ReddotMsgType.GodEquipBag_Fragment);
        }
        else if (tPoolDataDT.m_iNum < tPoolDataDT.m_EquipFragmentsDT.iBondNum && tServerData.num >= tPoolDataDT.m_EquipFragmentsDT.iBondNum)
        {
            Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.GodEquipBag_Fragment);
        }
        tPoolDataDT.m_iNum = tServerData.num;
    }

    protected override void RegSocketMessage()
    {
        SC_GodEquipFragmentInfo tSC_CardInit = new SC_GodEquipFragmentInfo();
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_GodEquipFragmentInfo, tSC_CardInit, Callback_SocketData_Update);
        stPoolDelData tstPoolDelData = new stPoolDelData();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_GodEquipFragmentRemove, tstPoolDelData, Callback_Del);
    }
    protected void Callback_Del(object Obj)
    {
        stPoolDelData tServerData = (stPoolDelData)Obj;
        GodEquipFragmentPoolDT tPoolDataDT = (GodEquipFragmentPoolDT)f_GetForId(tServerData.iId);
        if (tPoolDataDT == null)
        {
            MessageBox.ASSERT("Không tồn tại thông tin BaseGoods，cập nhập thất bại");
        }
        if (tPoolDataDT.m_iNum >= tPoolDataDT.m_EquipFragmentsDT.iBondNum)
        {
            Data_Pool.m_ReddotMessagePool.f_MsgSubtract(EM_ReddotMsgType.GodEquipBag_Fragment);
        }
        f_Delete(tServerData.iId);
    }

    #endregion

    /////////////外部调用接口////////////////////
    /// <summary>
    /// 检查红点提示
    /// </summary>
    public void f_ReCheckRedPoint()
    {
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.GodEquipBag_Fragment);
        List<BasePoolDT<long>> listData = f_GetAll();
        for (int i = 0; i < listData.Count; i++)
        {
            GodEquipFragmentPoolDT tPoolDataDT = listData[i] as GodEquipFragmentPoolDT;
            if (tPoolDataDT.m_iNum >= tPoolDataDT.m_EquipFragmentsDT.iBondNum)
            {
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.GodEquipBag_Fragment);
            }
        }
    }
    /// <summary>
    /// 装备碎片合成
    /// </summary>
    /// <param name="iEquipFragmentID"></param>
    /// <param name="tSocketCallbackDT"></param>
    public void f_Synthesis(int iEquipFragmentID, int num, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_GodEquipSynthesis, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(iEquipFragmentID);
        tCreateSocketBuf.f_Add(num);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_GodEquipSynthesis, bBuf);
    }

    /// <summary>
    /// 碎片卖出
    /// </summary>
    /// <param name="iEquipId"></param>
    /// <param name="tSocketCallbackDT"></param>
    public void f_Sell(long iEquipId, int num, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_EquipFragmentSell, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(iEquipId);
        tCreateSocketBuf.f_Add(num);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_EquipFragmentSell, bBuf);
    }

    /// <summary>
    /// 根据模板Id获取已拥有的数目
    /// </summary>
    /// <param name="templateId"></param>
    /// <returns></returns>
    public int f_GetHaveNumByTemplate(int templateId)
    {
        int result = 0;
        foreach (GodEquipFragmentPoolDT item in f_GetAll())
        {
            if (item.m_iTempleteId == templateId)
            {
                result += item.m_iNum;
            }
        }
        return result;
    }

    public void _Sort()
    {
        f_GetAll().Sort((BasePoolDT<long> a, BasePoolDT<long> b) =>
        {
            GodEquipFragmentPoolDT tEquipFragment1 = a as GodEquipFragmentPoolDT;
            GodEquipFragmentPoolDT tEquipFragment2 = b as GodEquipFragmentPoolDT;
            bool IsGain1 = tEquipFragment1.m_iNum >= tEquipFragment1.m_EquipFragmentsDT.iBondNum;
            bool IsGain2 = tEquipFragment2.m_iNum >= tEquipFragment2.m_EquipFragmentsDT.iBondNum;
            if (IsGain1 && !IsGain2)
            {
                return -1;
            }
            else if (!IsGain1 && IsGain2)
            {
                return 1;
            }
            else if (IsGain1 && IsGain2)
            {
                if (tEquipFragment1.m_EquipFragmentsDT.iList < tEquipFragment2.m_EquipFragmentsDT.iList)
                    return -1;
                else
                    return 1;
            }
            else
                return tEquipFragment1.m_EquipFragmentsDT.iList >= tEquipFragment2.m_EquipFragmentsDT.iList ? 1 : -1;
        });
    }
}
