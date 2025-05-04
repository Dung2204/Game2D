using ccU3DEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class RankingPowerAwardPool : BasePool
{
    private List<BasePoolDT<long>> m_RankList = new List<BasePoolDT<long>>();
    private List<BasePoolDT<long>> m_RankGodEquipList = new List<BasePoolDT<long>>();
    private List<BasePoolDT<long>> m_RankTariningList = new List<BasePoolDT<long>>();
    public RankingPowerAwardPool() : base("RankingPowerAwardPoolDT")
    {

    }

    protected override void f_Init()
    {

    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
        f_Socket_UpdateData(Obj);
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
       
    }

    public void f_QueryInfo(SocketCallbackDT tSocketCallbackDT)
    {

    }
    protected override void RegSocketMessage()
    {
        SC_RankingPowerUser tRC_RankingPowerUser = new SC_RankingPowerUser();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_RankingPowerList, tRC_RankingPowerUser, f_Callback_RankingPowerList);

        SC_RankingPowerUser tRC_RankingGodEquipUser = new SC_RankingPowerUser();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_RankingGodEquipList, tRC_RankingGodEquipUser, f_Callback_RankingGodEquipList);

        SC_RankingPowerUser tRC_RankingTariningUser = new SC_RankingPowerUser();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_RankingTariningList, tRC_RankingTariningUser, f_Callback_RankingTariningList);

        SC_RankingPowerMyRank tRC_SC_RankingPowerMyRank = new SC_RankingPowerMyRank();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_RankingPowerMyRank, tRC_SC_RankingPowerMyRank, f_Callback_RankingPowerMyRank);
    }

    public void f_RankList()
    {
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();


        if (m_RankList.Count < RANK_LIST_PAGE_MAX)
        {
            int count = m_RankList.Count;
            tCreateSocketBuf.f_Add(count);
            byte[] bBuf = tCreateSocketBuf.f_GetBuf();
            GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_RankingPowerList, bBuf);
        }
        else
        {
            
        }


    }

    public void f_RankGodEquipList()
    {
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();


        if (m_RankGodEquipList.Count < RANK_LIST_PAGE_MAX)
        {
            int count = m_RankGodEquipList.Count;
            tCreateSocketBuf.f_Add(count);
            byte[] bBuf = tCreateSocketBuf.f_GetBuf();
            GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_RankingGodEquipList, bBuf);
        }
        else
        {

        }


    }

    public void f_RankTariningList()
    {
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();


        if (m_RankTariningList.Count < RANK_LIST_PAGE_MAX)
        {
            int count = m_RankTariningList.Count;
            tCreateSocketBuf.f_Add(count);
            byte[] bBuf = tCreateSocketBuf.f_GetBuf();
            GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_RankingTariningList, bBuf);
        }
        else
        {

        }


    }

    private const int RANK_LIST_PAGE_MAX = 50;
    private void f_Callback_RankingPowerList(int iData1, int iData2, int iNum, ArrayList aData)
    {
        for (int i = 0; i < aData.Count; i++)
        {
            SC_RankingPowerUser tUpdateNode = (SC_RankingPowerUser)aData[i];

            RankingPowerAwardPoolDT lvRankItem = new RankingPowerAwardPoolDT();
            lvRankItem.f_UpdateInfo(tUpdateNode.rank, tUpdateNode.ift, tUpdateNode.szRoleName);
            m_RankList.Add(lvRankItem);

        }
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_RANKING_POWER_LIST);
    }

    private void f_Callback_RankingGodEquipList(int iData1, int iData2, int iNum, ArrayList aData)
    {
        for (int i = 0; i < aData.Count; i++)
        {
            SC_RankingPowerUser tUpdateNode = (SC_RankingPowerUser)aData[i];

            RankingPowerAwardPoolDT lvRankItem = new RankingPowerAwardPoolDT();
            lvRankItem.f_UpdateInfo(tUpdateNode.rank, tUpdateNode.ift, tUpdateNode.szRoleName);
            m_RankGodEquipList.Add(lvRankItem);

        }
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_RANKING_POWER_LIST);
    }

    private void f_Callback_RankingTariningList(int iData1, int iData2, int iNum, ArrayList aData)
    {
        for (int i = 0; i < aData.Count; i++)
        {
            SC_RankingPowerUser tUpdateNode = (SC_RankingPowerUser)aData[i];

            RankingPowerAwardPoolDT lvRankItem = new RankingPowerAwardPoolDT();
            lvRankItem.f_UpdateInfo(tUpdateNode.rank, tUpdateNode.ift, tUpdateNode.szRoleName);
            m_RankTariningList.Add(lvRankItem);

        }
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_RANKING_POWER_LIST);
    }


    SC_RankingPowerMyRank tRC_SC_RankingPowerMyRank;
    private void f_Callback_RankingPowerMyRank(object aData)
    {
        tRC_SC_RankingPowerMyRank = (SC_RankingPowerMyRank)aData;
    }

    public List<BasePoolDT<long>> f_GetRankList()
    {
        return m_RankList;
    }
    public List<BasePoolDT<long>> f_GetRankGodEquipList()
    {
        return m_RankGodEquipList;
    }
    public List<BasePoolDT<long>> f_GetRankTariningList()
    {
        return m_RankTariningList;
    }

    public int f_GetMyRank()
    {
        return tRC_SC_RankingPowerMyRank.rank;
    }

    public int f_GetMyScore()
    {
        return tRC_SC_RankingPowerMyRank.score;
    }
    public void ClearListRank()
    {
        m_RankList.Clear();
        m_RankGodEquipList.Clear();
        m_RankTariningList.Clear();
    }

}
