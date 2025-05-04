using UnityEngine;
using System.Collections;
using System;
using ccU3DEngine;
using System.Collections.Generic;

/// <summary>
/// 基础物品Pool
/// </summary>
public class BaseGoodsPool : BasePool
{
    public BaseGoodsPool() : base("BaseGoodsPoolDT", true)
    {

    }


    #region Pool数据管理

    protected override void f_Init()
    {
    }

    protected override void RegSocketMessage()
    {
        SC_ItemInit tSC_ItemInit = new SC_ItemInit();
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_ItemInfo, tSC_ItemInit, Callback_SocketData_Update);
        stPoolDelData tstPoolDelData = new stPoolDelData();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_ItemRemove, tstPoolDelData, Callback_Del);
    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
        SC_ItemInit tServerData = (SC_ItemInit)Obj;
        BaseGoodsPoolDT tPoolDataDT = new BaseGoodsPoolDT();
        tPoolDataDT.SetIData5(tServerData.tempId);
        tPoolDataDT.iId = tServerData.id;
        tPoolDataDT.m_iTempleteId = tServerData.tempId;
        tPoolDataDT.m_iNum = tServerData.iNum;
        Data_Pool.m_AwardPool.f_AddAward(EM_ResourceType.Good, tPoolDataDT.m_iTempleteId, tPoolDataDT.m_iNum);
        f_Save(tPoolDataDT);
        CheckUserInfoUpdate(tServerData.tempId);
    }
    private void CheckUserInfoUpdate(int tempId)
    {
        if(tempId == (int)EM_MoneyType.eCardRefineStone || tempId == (int)EM_MoneyType.eCardAwakenStone || tempId == (int)EM_MoneyType.eCardSkyPill
                || tempId == (int)EM_MoneyType.eBattleFormFragment || tempId == (int)EM_MoneyType.eRedEquipElite || tempId == (int)EM_MoneyType.eRedBattleToken
                || tempId == (int)EM_MoneyType.eFreshToken || tempId == (int)EM_MoneyType.eTreasureRefinePill || tempId == (int)EM_MoneyType.eNorAd
                || tempId == (int)EM_MoneyType.eGenAd)
        {
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_UPDATE_USERINFOR);
        }

    }
    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
        SC_ItemInit tServerData = (SC_ItemInit)Obj;
        BaseGoodsPoolDT tPoolDataDT = (BaseGoodsPoolDT)f_GetForId(tServerData.id);
        if(tPoolDataDT == null)
        {
MessageBox.ASSERT("BaseGoods information does not exist，update failed");
        }
        Data_Pool.m_AwardPool.f_AddAward(EM_ResourceType.Good, tPoolDataDT.m_iTempleteId, tServerData.iNum - tPoolDataDT.m_iNum);
        tPoolDataDT.m_iNum = tServerData.iNum;
        CheckUserInfoUpdate(tServerData.tempId);
    }

    protected void Callback_Del(object Obj)
    {
        stPoolDelData tServerData = (stPoolDelData)Obj;
        BaseGoodsPoolDT tPoolDataDT = (BaseGoodsPoolDT)f_GetForId(tServerData.iId);
        //检查阵图红点
        BattleFormationsDT battleFormationsDT = (glo_Main.GetInstance().m_SC_Pool.m_BattleFormationsSC.f_GetSC(Data_Pool.m_BattleFormPool.iDestinyProgress + 1) as BattleFormationsDT);
        if(battleFormationsDT == null || tPoolDataDT.m_BaseGoodsDT.iId == battleFormationsDT.iActivePorpID)
        {
            Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.BattleForm_CanAct);
            Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.BattleForm_CanAct);
        }
        f_Delete(tServerData.iId);
        CheckUserInfoUpdate(tPoolDataDT.m_BaseGoodsDT.iId);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //以下为外部调用接口

    /// <summary>
    /// 卖出接口
    /// </summary>
    /// <param name="Id">指定Id</param>
    /// <param name="tSocketCallbackDT"></param>
    public void f_Sell(BaseGoodsPoolDT tBaseGoodsPoolDT, int iNum, SocketCallbackDT tSocketCallbackDT)
    {

        ////向服务器提交数据
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_SellItem, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(tBaseGoodsPoolDT.iId);
        tCreateSocketBuf.f_Add(iNum);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_SellItem, bBuf);
    }

    /// <summary>
    /// 使用接口
    /// </summary>
    /// <param name="Id">指定Id</param>
    /// <param name="tSocketCallbackDT"></param>
    public void f_Use(long id, int iNum, int param, SocketCallbackDT tSocketCallbackDT)
    {
        ////向服务器提交数据
        Data_Pool.m_AwardPool.m_GetLoginAward.Clear();
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_UseItem, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(id);
        tCreateSocketBuf.f_Add(iNum);
        tCreateSocketBuf.f_Add(param);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_UseItem, bBuf);
    }

    /// <summary>
    /// 根据模板Id获取已拥有的数目
    /// </summary>
    /// <param name="templateId"></param>
    /// <returns></returns>
    public int f_GetHaveNumByTemplate(int templateId)
    {
        if(f_GetForData5(templateId) == null)
            return 0;
        else
        {
            List<BasePoolDT<long>> tList = f_GetAllForData5(templateId);
            int m_Num = 0;
            for(int i = 0; i < tList.Count; i++)
            {
                m_Num += (tList[i] as BaseGoodsPoolDT).m_iNum;
            }
            return m_Num;
        }
    }
}
