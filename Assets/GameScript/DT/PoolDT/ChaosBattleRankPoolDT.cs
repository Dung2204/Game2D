using ccU3DEngine;

public class ChaosBattleRankPoolDT : BasePoolDT<long>
{
    private int m_Rank;
    private int m_TitleId;
    private string m_TitleName;
    private int m_ServerId;
    private string m_ServerIdName;
    private long m_PlayerPower;
    private string m_PlayerName;
    private long m_ChaosScore; //TsuCode - ChaosScore

    public int Rank
    {
        get
        {
            return m_Rank;
        }
    }

    public long ChaosScore //TsuCode - ChaosScore
    {
        get
        {
            return m_ChaosScore;
        }
    }
    public string TitleName
    {
        get
        {
            return m_TitleName;
        }
    }

    public int ServerId
    {
        get
        {
            return m_ServerId;
        }
    }

    public string ServerIdName
    {
        get
        {
            return m_ServerIdName;
        }
    }

    public string PlayerName
    {
        get
        {
            return m_PlayerName;
        }
    }

    public long PlayerPower
    {
        get
        {
            return m_PlayerPower;
        }
    }

    public ChaosBattleRankPoolDT(int rank,int titleId,int serverId,long playerPower, string playerName)
    {
        iId = 0;
        m_Rank = rank;
        m_TitleId = titleId;
        ChaosBattleTitleDT tTitleTemplate = (ChaosBattleTitleDT)glo_Main.GetInstance().m_SC_Pool.m_ChaosBattleTitleSC.f_GetSC(titleId);
        m_TitleName = tTitleTemplate != null ? tTitleTemplate.szName : string.Empty;
        m_ServerId = serverId;
        ServerInforDT serverInfo = (ServerInforDT)glo_Main.GetInstance().m_SC_Pool.m_ServerInforSC.f_GetSC(serverId);
        m_ServerIdName = serverInfo != null ? serverInfo.szChannel : string.Empty;
        m_PlayerPower = playerPower;
        m_PlayerName = playerName != null ? playerName : string.Empty;
    }

    //TsuCode - ChaosScore -------------------
    public ChaosBattleRankPoolDT(int rank, int titleId, int serverId, long playerPower, string playerName, long ChaosScore)
    {
        iId = 0;
        m_Rank = rank;
        m_TitleId = titleId;
        ChaosBattleTitleDT tTitleTemplate = (ChaosBattleTitleDT)glo_Main.GetInstance().m_SC_Pool.m_ChaosBattleTitleSC.f_GetSC(titleId);
        m_TitleName = tTitleTemplate != null ? tTitleTemplate.szName : string.Empty;
        m_ServerId = serverId;
        ServerInforDT serverInfo = (ServerInforDT)glo_Main.GetInstance().m_SC_Pool.m_ServerInforSC.f_GetSC(serverId);
        m_ServerIdName = serverInfo != null ? serverInfo.szChannel : string.Empty;
        m_PlayerPower = playerPower;
        m_PlayerName = playerName != null ? playerName : string.Empty;
        m_ChaosScore = ChaosScore;
    }
    //--------------------------------------------------

    /// <summary>
    /// 更新改变数据（战力和头衔）
    /// </summary>
    /// <param name="power"></param>
    public void UpdateChangeData(int titleId, long power)
    {
        m_PlayerPower = power;
        m_TitleId = titleId;
        ChaosBattleTitleDT tTitleTemplate = (ChaosBattleTitleDT)glo_Main.GetInstance().m_SC_Pool.m_ChaosBattleTitleSC.f_GetSC(titleId);
        m_TitleName = tTitleTemplate != null ? tTitleTemplate.szName : string.Empty;
    }
}

