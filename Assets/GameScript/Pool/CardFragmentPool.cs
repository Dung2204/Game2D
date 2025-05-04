using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;

/// <summary>
/// 卡牌碎片Pool
/// </summary>
public class CardFragmentPool : BasePool
{
    public CardFragmentPool() : base("CardFragmentPoolDT", true)
    {

    }


    #region Pool数据管理

    protected override void f_Init()
    {
    }

    protected override void RegSocketMessage()
    {
        SC_CardFragmentInfo tSC_CardFragmentInfo = new SC_CardFragmentInfo();
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_CardFragmentInfo, tSC_CardFragmentInfo, Callback_SocketData_Update);
        stPoolDelData tstPoolDelData = new stPoolDelData();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_CardFragmentRemove, tstPoolDelData, Callback_Del);
    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
        SC_CardFragmentInfo tServerData = (SC_CardFragmentInfo)Obj;
        CardFragmentPoolDT tPoolDataDT = new CardFragmentPoolDT();

        tPoolDataDT.iId = tServerData.id;
        tPoolDataDT.m_iTempleteId = tServerData.tempId;
        tPoolDataDT.m_iNum = tServerData.iNum;
        Data_Pool.m_AwardPool.f_AddAward(EM_ResourceType.CardFragment, tPoolDataDT.m_iTempleteId, tPoolDataDT.m_iNum);

        if (tPoolDataDT.m_CardFragmentDT != null && tPoolDataDT.m_iNum >= tPoolDataDT.m_CardFragmentDT.iNeedNum)
        {
            Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.CardBag_Fragment);
        }

        f_Save(tPoolDataDT);
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
        SC_CardFragmentInfo tServerData = (SC_CardFragmentInfo)Obj;
        CardFragmentPoolDT tPoolDataDT = (CardFragmentPoolDT)f_GetForId(tServerData.id);
        if (tPoolDataDT == null)
        {
MessageBox.ASSERT("BaseGoods information does not exist，update failed");
        }
        //tPoolDataDT.m_iTempleteId = tServerData.tempId;
        Data_Pool.m_AwardPool.f_AddAward(EM_ResourceType.CardFragment, tPoolDataDT.m_iTempleteId, tServerData.iNum - tPoolDataDT.m_iNum);
        if (tPoolDataDT.m_iNum >= tPoolDataDT.m_CardFragmentDT.iNeedNum && tServerData.iNum < tPoolDataDT.m_CardFragmentDT.iNeedNum)
        {
            Data_Pool.m_ReddotMessagePool.f_MsgSubtract(EM_ReddotMsgType.CardBag_Fragment);
        }
        else if (tPoolDataDT.m_iNum < tPoolDataDT.m_CardFragmentDT.iNeedNum && tServerData.iNum >= tPoolDataDT.m_CardFragmentDT.iNeedNum)
        {
            Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.CardBag_Fragment);
        }

        tPoolDataDT.m_iNum = tServerData.iNum;
    }

    protected void Callback_Del(object Obj)
    {
        stPoolDelData tServerData = (stPoolDelData)Obj;
        CardFragmentPoolDT tPoolDataDT = (CardFragmentPoolDT)f_GetForId(tServerData.iId);
        if (tPoolDataDT.m_iNum >= tPoolDataDT.m_CardFragmentDT.iNeedNum)
        {
            Data_Pool.m_ReddotMessagePool.f_MsgSubtract(EM_ReddotMsgType.CardBag_Fragment);
        }
        f_Delete(tServerData.iId);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //以下为外部调用接口
    /// <summary>
    /// 检查红点提示
    /// </summary>
    public void f_ReCheckRedPoint()
    {
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.CardBag_Fragment);
        List<BasePoolDT<long>> listData = f_GetAll();
        for (int i = 0; i < listData.Count; i++)
        {
            CardFragmentPoolDT tPoolDataDT = listData[i] as CardFragmentPoolDT;
            if (tPoolDataDT.m_iNum >= tPoolDataDT.m_CardFragmentDT.iNeedNum)
            {
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.CardBag_Fragment);
            }
        }
    }
    /// <summary>
    /// 卡牌合成接口
    /// </summary>
    /// <param name="Id">模板Id</param>
    /// <param name="tSocketCallbackDT"></param>
    public void f_Synthesis(int iCardFragmentId, int num, SocketCallbackDT tSocketCallbackDT)
    {

        //I64u cardId;
        //CommItem item[4];

        ////向服务器提交数据
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_CardSynthesis, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(iCardFragmentId);
        tCreateSocketBuf.f_Add(num);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CardSynthesis, bBuf);

    }

    /// <summary>
    /// 卖出接口
    /// </summary>
    /// <param name="Id">指定Id</param>
    /// <param name="tSocketCallbackDT"></param>
    public void f_Sell(CardPoolDT tCardPoolDT, SocketCallbackDT tSocketCallbackDT)
    {

        ////向服务器提交数据
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_SellCard, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(tCardPoolDT.iId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_SellCard, bBuf);

    }

    /// <summary>
    /// 根据模板Id获取已拥有的数目
    /// </summary>
    /// <param name="templateId"></param>
    /// <returns></returns>
    public int f_GetHaveNumByTemplate(int templateId)
    {
        int result = 0;
        foreach (CardFragmentPoolDT item in f_GetAll())
        {
            if (item.m_iTempleteId == templateId)
            {
                result += item.m_iNum;
            }
        }
        return result;
    }


    public void f_SortList()
    {
        f_GetAll().Sort((BasePoolDT<long> a, BasePoolDT<long> b) =>
        {
            CardFragmentPoolDT tCardFragment1 = a as CardFragmentPoolDT;
            CardFragmentPoolDT tCardFragment2 = b as CardFragmentPoolDT;
            bool IsGain1 = tCardFragment1.m_iNum >= tCardFragment1.m_CardFragmentDT.iNeedNum;
            bool IsGain2 = tCardFragment2.m_iNum >= tCardFragment2.m_CardFragmentDT.iNeedNum;
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
                if (tCardFragment1.m_CardFragmentDT.iImportant > tCardFragment2.m_CardFragmentDT.iImportant)
                    return -1;
                else
                    return 1;
            }
            else
            {
                if (tCardFragment1.m_CardFragmentDT.iImportant > tCardFragment2.m_CardFragmentDT.iImportant)
                    return -1;
                else
                    return 1;
            }
        });
    }

}
