using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;

public class TreasurePool : BasePool
{
    public TreasurePool() : base("TreasurePoolDT", true)
    {
    }

    #region Pool管理
    protected override void f_Init()
    {
    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
        SC_TreasureInfo tServerData = (SC_TreasureInfo)Obj;
        TreasurePoolDT tPoolDataDT = new TreasurePoolDT();

        tPoolDataDT.iId = tServerData.id;
        tPoolDataDT.m_ExpIntensify = tServerData.expIntensify;
        tPoolDataDT.m_Num = tServerData.Num;
        tPoolDataDT.m_iTempNum = tServerData.Num;
        tPoolDataDT.m_lvIntensify = tServerData.lvIntensify;
        tPoolDataDT.m_lvRefine = tServerData.lvRefine;
        tPoolDataDT.m_iTempleteId = tServerData.tempId;
        Data_Pool.m_AwardPool.f_AddAward(EM_ResourceType.Treasure, tPoolDataDT.m_iTempleteId, tPoolDataDT.m_Num);
        f_Save(tPoolDataDT);
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
        SC_TreasureInfo tServerData = (SC_TreasureInfo)Obj;
        TreasurePoolDT tPoolDataDT = (TreasurePoolDT)f_GetForId(tServerData.id);
        if (tPoolDataDT == null)
        {
MessageBox.ASSERT("No data, update failed");
        }
        Data_Pool.m_AwardPool.f_AddAward(EM_ResourceType.Treasure, tPoolDataDT.m_iTempleteId, tServerData.Num - tPoolDataDT.m_Num);
        tPoolDataDT.m_ExpIntensify = tServerData.expIntensify;
        tPoolDataDT.m_lvIntensify = tServerData.lvIntensify;
        tPoolDataDT.m_Num = tServerData.Num;
        tPoolDataDT.m_lvRefine = tServerData.lvRefine;
        tPoolDataDT.m_iTempleteId = tServerData.tempId;
    }

    protected override void RegSocketMessage()
    {
        SC_TreasureInfo tSC_TreasureInit = new SC_TreasureInfo();
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_TreasureInfo, tSC_TreasureInit, Callback_SocketData_Update);
        stPoolDelData tstPoolDelData = new stPoolDelData();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_TreasureRemove, tstPoolDelData, Callback_Del);
    }
    protected void Callback_Del(object Obj)
    {
        stPoolDelData tServerData = (stPoolDelData)Obj;
        f_Delete(tServerData.iId);
    }
    #endregion

    /// <summary>
    /// 法宝强化
    /// </summary>
    /// <param name="TreansureId"></param>
    /// <param name="srcId"></param>
    /// <param name="tSocketCallbackDT"></param>
    public void f_Intensify(long TreansureId, long srcId1,
        long srcId2, long srcId3, long srcId4, long srcId5, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_TreasureIntensify, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(TreansureId);
        tCreateSocketBuf.f_Add(srcId1);
        tCreateSocketBuf.f_Add(srcId2);
        tCreateSocketBuf.f_Add(srcId3);
        tCreateSocketBuf.f_Add(srcId4);
        tCreateSocketBuf.f_Add(srcId5);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_TreasureIntensify, bBuf);
    }
    /// <summary>
    /// 法宝精炼
    /// </summary>
    /// <param name="TreansureId"></param>
    /// <param name="tSocketCallbackDT"></param>
    public void f_Refine(long TreansureId, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_TreasureRefine, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(TreansureId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_TreasureRefine, bBuf);
    }

    public void f_SortList()
    {
        f_GetAll().Sort((BasePoolDT<long> tmp1, BasePoolDT<long> tmp2) =>
        {
            TreasurePoolDT tmpEquip1 = (TreasurePoolDT)tmp1;
            TreasurePoolDT tmpEquip2 = (TreasurePoolDT)tmp2;

            //return tmpEquip1.m_EquipDT.iColour > tmpEquip2.m_EquipDT.iColour ? -1 : 1;
            if (UITool.f_GetHowEquip(tmpEquip1.iId) != "" && UITool.f_GetHowEquip(tmpEquip2.iId) == "")
                return -1;
            else if (UITool.f_GetHowEquip(tmpEquip2.iId) != "" && UITool.f_GetHowEquip(tmpEquip1.iId) == "")
                return 1;
            else
            {
                if (tmpEquip1.m_lvRefine > tmpEquip2.m_lvRefine)
                    return -1;
                else if (tmpEquip1.m_lvRefine < tmpEquip2.m_lvRefine)
                    return 1;
                else
                {
                    if (tmpEquip1.m_lvIntensify > tmpEquip2.m_lvIntensify)
                        return -1;
                    else if (tmpEquip1.m_lvIntensify < tmpEquip2.m_lvIntensify)
                        return 1;
                    else
                    {
                        if (tmpEquip1.m_TreasureDT.iImportant > tmpEquip2.m_TreasureDT.iImportant)
                            return -1;
                        else if (tmpEquip1.m_TreasureDT.iImportant < tmpEquip2.m_TreasureDT.iImportant)
                            return 1;
                        else
                        {
                            if (tmpEquip1.m_TreasureDT.iId > tmpEquip2.m_TreasureDT.iId)
                                return -1;
                            else if (tmpEquip1.m_TreasureDT.iId < tmpEquip2.m_TreasureDT.iId)
                                return 1;
                        }
                    }

                }
            }
            return 0;
        }
        );
    }
    public void f_SortList(List<BasePoolDT<long>> obj)
    {
        f_GetAll().Sort((BasePoolDT<long> tmp1, BasePoolDT<long> tmp2) =>
        {
            TreasurePoolDT tmpEquip1 = (TreasurePoolDT)tmp1;
            TreasurePoolDT tmpEquip2 = (TreasurePoolDT)tmp2;

            //return tmpEquip1.m_EquipDT.iColour > tmpEquip2.m_EquipDT.iColour ? -1 : 1;
            if (UITool.f_GetHowEquip(tmpEquip1.iId) != "" && UITool.f_GetHowEquip(tmpEquip2.iId) == "")
                return -1;
            else if (UITool.f_GetHowEquip(tmpEquip2.iId) != "" && UITool.f_GetHowEquip(tmpEquip1.iId) == "")
                return 1;
            else
            {
                if (tmpEquip1.m_TreasureDT.iImportant > tmpEquip2.m_TreasureDT.iImportant)
                    return -1;
                else if (tmpEquip1.m_TreasureDT.iImportant < tmpEquip2.m_TreasureDT.iImportant)
                    return 1;
                else
                {
                    if (tmpEquip1.m_lvRefine > tmpEquip2.m_lvRefine)
                        return -1;
                    else if (tmpEquip1.m_lvRefine < tmpEquip2.m_lvRefine)
                        return 1;
                    else
                    {
                        if (tmpEquip1.m_lvIntensify > tmpEquip2.m_lvIntensify)
                            return -1;
                        else if (tmpEquip1.m_lvIntensify < tmpEquip2.m_lvIntensify)
                            return 1;
                        else
                        {
                            if (tmpEquip1.m_TreasureDT.iId > tmpEquip2.m_TreasureDT.iId)
                                return -1;
                            else if (tmpEquip1.m_TreasureDT.iId < tmpEquip2.m_TreasureDT.iId)
                                return 1;
                        }
                    }
                }
            }
            return 0;
        }
        );
    }

    /// <summary>
    /// 法宝重生
    /// </summary>
    /// <param name="id"></param>
    /// <param name="tSocketCallbackDT"></param>
    public void f_TreasureRebirth(long id, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_Rebirth, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add((byte)EM_ResourceType.Treasure);
        tCreateSocketBuf.f_Add(id);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_Rebirth, bBuf);
    }
    /// <summary>
    /// 根据模板Id获取已拥有的数目
    /// </summary>
    /// <param name="templateId"></param>
    /// <returns></returns>
    public int f_GetHaveNumByTemplate(int templateId)
    {
        int result = 0;
        foreach (TreasurePoolDT item in f_GetAll())
        {
            if (item.m_iTempleteId == templateId)
            {
                result += item.m_Num;
            }
        }
        return result;
    }

    /// <summary>
    /// 检查可以合成的法宝数量
    /// </summary>
    /// <param name="treasureId">法宝id</param>
    /// <returns></returns>
    public int f_CheckTreasureCanMixMaxCount(int treasureId)
    {
        int num = -1;
        List<NBaseSCDT> listSCData = glo_Main.GetInstance().m_SC_Pool.m_TreasureFragmentsSC.f_GetAll();
        for (int i = 0; i < listSCData.Count; i++)
        {
            TreasureFragmentsDT dt = listSCData[i] as TreasureFragmentsDT;
            if (dt.iTreasureId == treasureId)
            {
                int tempNum = Data_Pool.m_TreasureFragmentPool.f_GetHaveNumByTemplate(dt.iId);
                if (tempNum < num || num == -1)
                {
                    num = tempNum;
                }
            }
        }
        num = num < 0 ? 0 : num;
        return num;
    }
    /// <summary>
    /// 检测该法宝的法宝碎片
    /// </summary>
    /// <param name="treasureId">法宝id</param>
    /// <returns></returns>
    public bool f_CheckHasTreasureFragment(int treasureId)
    {
        if (treasureId == 5004 || treasureId == 5005 || treasureId == 5006 || treasureId == 5007)
            return true;
        List<BasePoolDT<long>> listData = Data_Pool.m_TreasureFragmentPool.f_GetAll();
        for (int i = 0; i < listData.Count; i++)
        {
            TreasureFragmentPoolDT poolDT = listData[i] as TreasureFragmentPoolDT;
            int fixID = poolDT.m_TreasureFragmentsDT.iTreasureId;
            if (fixID == treasureId)
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 检查是否有可合成的法宝
    /// </summary>
    /// <returns></returns>
    public bool f_CheckCanFixTreasure()
    {
        List<TreasureDT> listNeedFixTreasure = new List<TreasureDT>();//需要合成的宝物列表
        //1.加入必须使用蓝色法宝列表
        listNeedFixTreasure.Add(glo_Main.GetInstance().m_SC_Pool.m_TreasureSC.f_GetSC(5004) as TreasureDT);
        listNeedFixTreasure.Add(glo_Main.GetInstance().m_SC_Pool.m_TreasureSC.f_GetSC(5005) as TreasureDT);
        listNeedFixTreasure.Add(glo_Main.GetInstance().m_SC_Pool.m_TreasureSC.f_GetSC(5006) as TreasureDT);
        listNeedFixTreasure.Add(glo_Main.GetInstance().m_SC_Pool.m_TreasureSC.f_GetSC(5007) as TreasureDT);
        //2.检查玩家拥有的法宝碎片
        List<BasePoolDT<long>> listData = Data_Pool.m_TreasureFragmentPool.f_GetAll();
        for (int i = 0; i < listData.Count; i++)
        {
            TreasureFragmentPoolDT poolDT = listData[i] as TreasureFragmentPoolDT;
            int fixID = poolDT.m_TreasureFragmentsDT.iTreasureId;
            TreasureDT dt = glo_Main.GetInstance().m_SC_Pool.m_TreasureSC.f_GetSC(fixID) as TreasureDT;
            if (!listNeedFixTreasure.Contains(dt))
                listNeedFixTreasure.Add(dt);
        }
        for (int i = 0; i < listNeedFixTreasure.Count; i++)
        {
            if (f_CheckTreasureCanMixMaxCount(listNeedFixTreasure[i].iId) > 0)
            {
                if (UITool.f_GetIsOpensystem(EM_NeedLevel.GrabTreasureLevel))
                {
                    Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.TreasureCanFix);
                    Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.TreasureCanFix);
                }
                return true;
            }
        }
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.TreasureCanFix);
        return false;
    }

    /// <summary>
    /// 检测法宝有无狗粮升级
    /// </summary>
    public bool f_CheckTreasureUpLevel(TreasurePoolDT treasure)
    {
        bool canUp = false;
        List<BasePoolDT<long>> List = f_GetAll();
        TreasurePoolDT tTteasureDT;
        TreasureUpConsumeDT Consume = glo_Main.GetInstance().m_SC_Pool.m_TreasureUpConsumeSC.f_GetSC(treasure.m_TreasureDT.iImportant * 1000 + treasure.m_lvIntensify ) as TreasureUpConsumeDT;
        if (Consume == null)
        {
            return canUp;
        }
        int NeedExp = Consume.iIntensifyExp - treasure.m_ExpIntensify;

        bool Money = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Money)>= NeedExp;
        bool Treasure = false;

        for (int i = 0; i < List.Count; i++)
        {
            tTteasureDT = List[i] as TreasurePoolDT;
            if (tTteasureDT.iId != treasure.iId)
            {
                if ((tTteasureDT.m_iTempleteId == 5004 ||
                    tTteasureDT.m_iTempleteId == 5005 ||
                    tTteasureDT.m_iTempleteId == 5006 ||
                    tTteasureDT.m_iTempleteId == 5004 || tTteasureDT.m_TreasureDT.iImportant < (int)EM_Important.Purple)
                    &&f_CheckTreasureCanConsume(tTteasureDT))
                {
                    NeedExp -= tTteasureDT.m_TreasureDT.iExp * tTteasureDT.m_Num;
                }

                if (NeedExp <= 0)
                {
                    Treasure=true;
                    break;
                }
            }
        }

        canUp = Money && Treasure;

        return canUp;
    }
    /// <summary>
    /// 检测法宝可否精练
    /// </summary>
    /// <returns></returns>
    public bool f_CheckTreasureRefin(TreasurePoolDT treasure)
    {
        bool canRefine = false;
        TreasureRefineConsumeDT RefineConsumeDT = glo_Main.GetInstance().m_SC_Pool.m_TreasureRefineConsumeSC.f_GetSC(treasure.m_iTempleteId * 100 + treasure.m_lvRefine + 1) as TreasureRefineConsumeDT;
        if (RefineConsumeDT == null)
        {
            return false;
        }

        bool Goods = Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(RefineConsumeDT.iRefinePillId) >= RefineConsumeDT.iRefineNum;
        bool Money = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Money) >= RefineConsumeDT.iGoldNum;
        int NeedTreasureNum = RefineConsumeDT.iTreasureNum;
        bool Treasure = NeedTreasureNum<=0;
        if (NeedTreasureNum > 0)
        {
            List<BasePoolDT<long>> List = f_GetAll();
            TreasurePoolDT tTteasureDT;
            for (int i = 0; i < List.Count; i++)
            {
                tTteasureDT = List[i] as TreasurePoolDT;
                if (treasure.iId != tTteasureDT.iId && treasure.m_iTempleteId == tTteasureDT.m_iTempleteId && !Data_Pool.m_TeamPool.f_CheckTeamTreasure(tTteasureDT.iId))
                {
                    if (f_CheckTreasureCanConsume(tTteasureDT) )
                    {
                        NeedTreasureNum -= tTteasureDT.m_Num;
                    }
                    if (NeedTreasureNum <= 0)
                    {
                        Treasure = true;
                        break;
                    }
                }
            }
        }

        canRefine = Goods && Money && Treasure;

        return canRefine;
    }
    /// <summary>
    /// 检查法宝是否为纯净法宝
    /// </summary>
    /// <returns></returns>
    public bool f_CheckTreasureCanConsume(TreasurePoolDT treasure)
    {
        if (treasure.m_ExpIntensify == 0 && treasure.m_lvIntensify == 1 && treasure.m_lvRefine == 0)
        {
            return true;
        }

        return false;
    }
}
