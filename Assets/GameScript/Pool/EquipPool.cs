using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;

public class EquipPool : BasePool
{
    public EquipPool() : base("EquipPoolDT", true)
    {
    }

    #region Pool数据管理 
    protected override void f_Init()
    {
    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
        SC_EquipInfo tServerData = (SC_EquipInfo)Obj;
        EquipPoolDT tPoolDataDT = new EquipPoolDT();

        tPoolDataDT.iId = tServerData.id;
        tPoolDataDT.m_iTempleteId = tServerData.tempId;
        tPoolDataDT.m_iexpRefine = tServerData.expRefine;
        tPoolDataDT.m_lvIntensify = tServerData.lvIntensify;
        tPoolDataDT.m_lvRefine = tServerData.lvRefine;
        tPoolDataDT.m_sstars = tServerData.stars;
        tPoolDataDT.m_slucky = tServerData.lucky;
        tPoolDataDT.m_sexpStars = tServerData.expStars;
        Data_Pool.m_AwardPool.f_AddAward(EM_ResourceType.Equip, tPoolDataDT.m_iTempleteId, 1);
        f_Save(tPoolDataDT);
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {

        SC_EquipInfo tServerData = (SC_EquipInfo)Obj;
        EquipPoolDT tPoolDataDT = (EquipPoolDT)f_GetForId(tServerData.id);
        if (tPoolDataDT == null)
        {
MessageBox.ASSERT("BaseGoods information does not exist，update failed");
        }
        //tPoolDataDT.m_iTempleteId = tServerData.tempId;
        tPoolDataDT.m_iTempleteId = tServerData.tempId;
        tPoolDataDT.m_iexpRefine = tServerData.expRefine;
        tPoolDataDT.m_lvIntensify = tServerData.lvIntensify;
        tPoolDataDT.m_lvRefine = tServerData.lvRefine;
        tPoolDataDT.m_sstars = tServerData.stars;
        tPoolDataDT.m_sexpStars = tServerData.expStars;
        tPoolDataDT.m_slucky = tServerData.lucky;
    }

    protected override void RegSocketMessage()
    {
        SC_EquipInfo tSC_ItemInit = new SC_EquipInfo();
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_EquipInfo, tSC_ItemInit, Callback_SocketData_Update);
        stPoolDelData tstPoolDelData = new stPoolDelData();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_EquipRemove, tstPoolDelData, Callback_Del);
        SC_EquipIntensify tSC_EquipIntensify = new SC_EquipIntensify();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_EquipIntensify, tSC_EquipIntensify, InterSuc);
        SC_EquipCostHistory tCS_EquipCostHistory = new SC_EquipCostHistory();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_EquipCostHistory, tCS_EquipCostHistory, tEquipCostHistory);

    }
    protected void Callback_Del(object Obj)
    {
        stPoolDelData tServerData = (stPoolDelData)Obj;
        f_Delete(tServerData.iId);
    }

    void InterSuc(object obj)
    {
        SC_EquipIntensify tSC_EquipIntensify = (SC_EquipIntensify)obj;
        m_EquipIntensify.critTimes = tSC_EquipIntensify.critTimes;
        m_EquipIntensify.realTimes = tSC_EquipIntensify.realTimes;
        if (ShowTirt != null)
            ShowTirt(obj);
    }

    void tEquipCostHistory(object obj)
    {
        if (EquipCostHistory != null)
            EquipCostHistory(obj);
    }
    #endregion


    public SC_EquipIntensify m_EquipIntensify = new SC_EquipIntensify();
    /////////////////外部调用接口///////////////////////////////

    public ccCallback ShowTirt;
    public ccCallback EquipCostHistory;
    public int m_OneKeyRefineLv=80;
    /// <summary>
    /// 一键强化
    /// </summary>
    /// <param name="id">唯一ID</param>
    /// <param name="dstLv">目标等级</param>
    public void f_OneInten(long id, short dstLv, SocketCallbackDT tSocketCallbackDT, ccCallback back)
    {
        ShowTirt = back;
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_EquipIntensifyOneKey, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(id);
        tCreateSocketBuf.f_Add(dstLv);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_EquipIntensifyOneKey, bBuf);
MessageBox.DEBUG("Send" + (int)SocketCommand.CS_EquipIntensifyOneKey);
    }
    /// <summary>
    /// 装备升级
    /// </summary>
    public void f_LevelUp(long EquipId, short bTime, ccCallback back)
    {
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(EquipId);
        tCreateSocketBuf.f_Add(bTime);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_EquipIntensifyOneKey, bBuf);
        ShowTirt = back;
    }
    /// <summary>
    /// 精炼
    /// </summary>
    public void f_Refine(long EquipId, int tempId, int num, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_EquipRefine, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(EquipId);
        tCreateSocketBuf.f_Add(tempId);
        tCreateSocketBuf.f_Add(num);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_EquipRefine, bBuf);
    }
    /// <summary>
    /// 装备升星
    /// </summary>
    /// <param name="EquipId"></param>
    /// <param name="upType"></param>
    /// <param name="tSocketCallbackDT"></param>
    public void f_EquipUpStar(long EquipId, EM_EquipUpStarType upType, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_EquipStarsup, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(EquipId);
        tCreateSocketBuf.f_Add((byte)upType);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_EquipStarsup, bBuf);
    }

    /// <summary>
    /// 根据模板Id获取已拥有的数目
    /// </summary>
    /// <param name="templateId"></param>
    /// <returns></returns>
    public int f_GetHaveNumByTemplate(int templateId)
    {
        int result = 0;
        foreach (EquipPoolDT item in f_GetAll())
        {
            if (item.m_iTempleteId == templateId)
            {
                result++;
            }
        }
        return result;
    }
    ///// <summary>
    ///// 检查装备回收红点
    ///// </summary>
    ///// <returns></returns>
    //public void f_CheckEquipRecycleRedPoint()
    //{
    //    Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.EquipRecycle);
    //    List<BasePoolDT<long>> listData = f_GetAll();
    //    for (int i = 0; i < listData.Count; i++)
    //    {
    //        EquipPoolDT poolDT = listData[i] as EquipPoolDT;
    //        if ( (poolDT.m_EquipDT.iColour == (int)EM_Important.Blue || poolDT.m_EquipDT.iColour == (int)EM_Important.Green)
    //            && !Data_Pool.m_TeamPool.f_CheckInEquipByKeyId(poolDT.iId))
    //        {
    //            Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.EquipRecycle);
    //            return;
    //        }
    //    }
    //}
    /// <summary>
    /// 装备重生
    /// </summary>
    public void f_EquipRebirth(long dstId, SocketCallbackDT back)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_Rebirth, back.m_ccCallbackSuc, back.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add((byte)EM_ResourceType.Equip);
        tCreateSocketBuf.f_Add(dstId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_Rebirth, bBuf);
    }
    /// <summary>
    /// 装备分解
    /// </summary>
    public void f_EquipRecycle(BasePoolDT<long> id1, BasePoolDT<long> id2, BasePoolDT<long> id3, BasePoolDT<long> id4,
        BasePoolDT<long> id5, SocketCallbackDT back)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_Recycle, back.m_ccCallbackSuc, back.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add((byte)EM_ResourceType.Equip);
        long[] tid = new long[5];
        BasePoolDT<long>[] tArr = { id1, id2, id3, id4, id5 };
        for (int i = 0; i < tArr.Length; i++)
        {
            if (tArr[i] != null)
            {
                tCreateSocketBuf.f_Add(tArr[i].iId);
                tid[i] = tArr[i].iId;
            }
            else
            {
                tCreateSocketBuf.f_Add(0L);
                tid[i] = 0;
            }
        }
        MessageBox.ASSERT("Thu hồi trang bị id1   " + tid[0] + "   Id2   " + tid[1] + "   id3   " + tid[2] + "   id4   " + tid[3] + "   id5   " + tid[4]);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_Recycle, bBuf);
    }
    /// <summary>
    /// 请求装备历史升星
    /// </summary>
    /// <param name="id"></param>
    /// <param name="back"></param>
    public void f_GetEquipCostHistory(long id, ccCallback History, SocketCallbackDT back)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_EquipCostHistory, back.m_ccCallbackSuc, back.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(id);
        EquipCostHistory = History;
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_EquipCostHistory, bBuf);
    }


    public void f_EquipOneKeyRefine(long id,int num,int[] RefineItemId, int[] RefineItemNum, SocketCallbackDT tSocketCallback) {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_EquipRefineOnekey, tSocketCallback.m_ccCallbackSuc, tSocketCallback.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(id);
        tCreateSocketBuf.f_Add(num);
        for (int i = 0; i < RefineItemId.Length; i++)
        {
            tCreateSocketBuf.f_Add(RefineItemId[i]);
            tCreateSocketBuf.f_Add(RefineItemNum[i]);
        }
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_EquipRefineOnekey, bBuf);

    }

    public SetEquipDT f_GetSetEquipDT(int id)
    {
        if (glo_Main.GetInstance().m_SC_Pool.m_SetEquipSC.M_SetEquipSC.ContainsKey(id))
            return glo_Main.GetInstance().m_SC_Pool.m_SetEquipSC.M_SetEquipSC[id];
        else
            return null;
    }

    /// <summary>
    /// 排序 已装备>颜色>精炼>等级
    /// </summary>
    public void f_SortList()
    {
        f_GetAll().Sort((BasePoolDT<long> tmp1, BasePoolDT<long> tmp2) =>
        {
            //    EquipPoolDT tmpEquip1 = (EquipPoolDT)tmp1;
            //    EquipPoolDT tmpEquip2 = (EquipPoolDT)tmp2;

            //return tmpEquip1.m_EquipDT.iColour > tmpEquip2.m_EquipDT.iColour ? -1 : 1;
            if (UITool.f_GetHowEquip(tmp1.iId) != "" && UITool.f_GetHowEquip(tmp2.iId) == "")
                return -1;
            else if (UITool.f_GetHowEquip(tmp2.iId) != "" && UITool.f_GetHowEquip(tmp1.iId) == "")
                return 1;
            else
            {
                if (tmp1.iData1 > tmp2.iData1)
                    return -1;
                else if (tmp1.iData1 < tmp2.iData1)
                    return 1;
                else
                {
                    if (tmp1.iData2 > tmp2.iData2)
                        return -1;
                    else if (tmp1.iData2 < tmp2.iData2)
                        return 1;
                    else
                    {
                        if (tmp1.iData3 > tmp2.iData3)
                            return -1;
                        else if (tmp1.iData3 <= tmp2.iData3)
                            return 1;
                        else
                        {
                            if (tmp1.iData4 > tmp2.iData4)
                                return -1;
                            else if (tmp1.iData4 < tmp2.iData4)
                                return 1;
                        }
                    }
                }
                //if (tmpEquip1.m_EquipDT.iColour > tmpEquip2.m_EquipDT.iColour)
                //    return -1;
                //else if (tmpEquip1.m_EquipDT.iColour < tmpEquip2.m_EquipDT.iColour)
                //    return 1;
                //else
                //{
                //    if (tmpEquip1.m_lvRefine > tmpEquip2.m_lvRefine)
                //        return -1;
                //    else if (tmpEquip1.m_lvRefine < tmpEquip2.m_lvRefine)
                //        return 1;
                //    else
                //    {
                //        if (tmpEquip1.m_lvIntensify > tmpEquip2.m_lvIntensify)
                //            return -1;
                //        else if (tmpEquip1.m_lvIntensify <= tmpEquip2.m_lvIntensify)
                //            return 1;
                //        else
                //        {
                //            if (tmpEquip1.m_EquipDT.iId > tmpEquip2.m_EquipDT.iId)
                //                return -1;
                //            else if (tmpEquip1.m_EquipDT.iId < tmpEquip2.m_EquipDT.iId)
                //                return 1;
                //        }
                //    }
                //}
            }
            return 0;
        }
        );
    }

    public void f_SortList(List<BasePoolDT<long>> obj)
    {
        obj.Sort((BasePoolDT<long> tmp1, BasePoolDT<long> tmp2) =>
        {
            EquipPoolDT tmpEquip1 = (EquipPoolDT)tmp1;
            EquipPoolDT tmpEquip2 = (EquipPoolDT)tmp2;

            //return tmpEquip1.m_EquipDT.iColour > tmpEquip2.m_EquipDT.iColour ? -1 : 1;
            if (UITool.f_GetHowEquip(tmpEquip1.iId) != "" && UITool.f_GetHowEquip(tmpEquip2.iId) == "")
                return -1;
            else if (UITool.f_GetHowEquip(tmpEquip2.iId) != "" && UITool.f_GetHowEquip(tmpEquip1.iId) == "")
                return 1;
            else
            {
                if (tmpEquip1.m_EquipDT.iColour > tmpEquip2.m_EquipDT.iColour)
                    return -1;
                else if (tmpEquip1.m_EquipDT.iColour < tmpEquip2.m_EquipDT.iColour)
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
                        else if (tmpEquip1.m_lvIntensify <= tmpEquip2.m_lvIntensify)
                            return 1;
                        else
                        {
                            if (tmpEquip1.m_EquipDT.iId > tmpEquip2.m_EquipDT.iId)
                                return -1;
                            else if (tmpEquip1.m_EquipDT.iId < tmpEquip2.m_EquipDT.iId)
                                return 1;
                        }
                    }
                }
            }
            return 0;
        }
        );
    }
}

