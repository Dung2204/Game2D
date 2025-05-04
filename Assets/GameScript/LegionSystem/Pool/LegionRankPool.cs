using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
/// <summary>
/// 军团等级排名Item
/// </summary>
public class LegionLevelRankItem : BasePoolDT<long>
{
    public LegionInfoPoolDT legionInfoPoolDT;//军团信息
    public int Rank;//排名
    public string m_szLeaderName;//军团长名称
}
public class LegionDungeRankItem : BasePoolDT<long>
{
    public LegionInfoPoolDT legionInfoPoolDT;//军团信息
    public int Rank;//排名
    public int progress;//副本进度
    public string m_szLeaderName;//军团长名称
}
/// <summary>
/// 军团排行pool
/// </summary>
public class LegionRankPool : BasePool {
    /// <summary>
    /// 等级排行列表
    /// </summary>
    public List<BasePoolDT<long>> listLevelRankLegion = new List<BasePoolDT<long>>();
    /// <summary>
    /// 副本排行列表
    /// </summary>
    public List<BasePoolDT<long>> listDungeonRankLegion = new List<BasePoolDT<long>>();

    public LegionRankPool() : base("", true)
    {

    }

    protected override void f_Init()
    {

    }

    protected override void RegSocketMessage()
    {
        RC_LegionLvRankList rcLegionLvRankList = new RC_LegionLvRankList();
        ChatSocket.GetInstance().f_RegMessage_Int2((int)LegionSocketCmd.RC_LegionLvRankList, rcLegionLvRankList, OnRCLegionLvRankListCallback);
        RC_LegionPveRankList rcLegionPveRankList = new RC_LegionPveRankList();
        ChatSocket.GetInstance().f_RegMessage_Int2((int)LegionSocketCmd.RC_LegionPveRankList, rcLegionPveRankList, OnRCLegionPveRankListCallback);
    }
    /// <summary>
    /// 等级排行回调
    /// </summary>
    private void OnRCLegionLvRankListCallback(int iData1, int iData2, int iNum, ArrayList aData)
    {
        listLevelRankLegion.Clear();
        foreach (SockBaseDT tData in aData)
        {
            RC_LegionLvRankList rcLegionLvRankList = (RC_LegionLvRankList)tData;
            LegionLevelRankItem item = new LegionLevelRankItem();
            item.iId = rcLegionLvRankList.legionId;
            item.Rank = rcLegionLvRankList.rank;
            item.m_szLeaderName = rcLegionLvRankList.m_szLeaderName;
            if (item.iId == LegionMain.GetInstance().m_LegionInfor.f_getUserLegion().iId)
                item.legionInfoPoolDT = LegionMain.GetInstance().m_LegionInfor.f_getUserLegion();
            else
                item.legionInfoPoolDT = LegionMain.GetInstance().m_LegionInfor.f_LegionSerch(rcLegionLvRankList.legionId);
            listLevelRankLegion.Add(item);
        }
    }
    /// <summary>
    /// 副本排行回调
    /// </summary>
    private void OnRCLegionPveRankListCallback(int iData1, int iData2, int iNum, ArrayList aData)
    {
        listDungeonRankLegion.Clear();
        foreach (SockBaseDT tData in aData)
        {
            RC_LegionPveRankList rcLegionPveRankList = (RC_LegionPveRankList)tData;
            LegionDungeRankItem item = new LegionDungeRankItem();
            item.iId = rcLegionPveRankList.legionId;
            item.Rank = rcLegionPveRankList.rank;
            item.progress = rcLegionPveRankList.LegionTollgate;
            item.m_szLeaderName = rcLegionPveRankList.m_szLeaderName;
            if (item.iId == LegionMain.GetInstance().m_LegionInfor.f_getUserLegion().iId)
                item.legionInfoPoolDT = LegionMain.GetInstance().m_LegionInfor.f_getUserLegion();
            else
                item.legionInfoPoolDT = LegionMain.GetInstance().m_LegionInfor.f_LegionSerch(rcLegionPveRankList.legionId);
            listDungeonRankLegion.Add(item);
        }
    }
    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {

    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {

    }
    #region 外部接口
    /// <summary>
    /// 请求信军团等级排行
    /// </summary>
    /// <param name="tSocketCallbackDT">回调</param>
    public void f_GetLvRank(SocketCallbackDT tSocketCallbackDT)
    {
        listLevelRankLegion.Clear();
        //向服务器提交数据
        ChatSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionLvRankList, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionLvRankList, bBuf);
    }
    /// <summary>
    /// 请求信军团副本排行
    /// </summary>
    /// <param name="tSocketCallbackDT">回调</param>
    public void f_GetPveRank(SocketCallbackDT tSocketCallbackDT)
    {
        listDungeonRankLegion.Clear();
        //向服务器提交数据
        ChatSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionPveRankList, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionPveRankList, bBuf);
    }
    #endregion
}