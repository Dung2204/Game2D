using ccU3DEngine;

public class ChaosHistoryPoolDT : BasePoolDT<long>
{
    private long m_UserId;
    private int m_ServerId;
    private string m_ServerName;
    private string m_UserName;
    private long m_EnemyId;
    private int m_ServerEnemyId;
    private string m_ServerEnemyName;
    private string m_EnemyName;
    private int m_BattleRes;
    private int m_RecordTime;
    private int m_Note;

    public long UserId
    {
        get
        {
            return m_UserId;
        }
    }

    public int ServerId 
    {
        get
        {
            return m_ServerId;
        }
    }
    public string UserName
    {
        get
        {
            return m_UserName;
        }
    }

    public long EnemyId
    {
        get
        {
            return m_EnemyId;
        }
    }

    public int ServerEnemyId
    {
        get
        {
            return m_ServerEnemyId;
        }
    }

    public string EnemyName
    {
        get
        {
            return m_EnemyName;
        }
    }

    public int BattleRes
    {
        get
        {
            return m_BattleRes;
        }
    }

    public int RecordTime
    {
        get
        {
            return m_RecordTime;
        }
    }

    public int Note
    {
        get
        {
            return m_Note;
        }
    }

    public string ServerName
    {
        get
        {
            return m_ServerName;
        }
    }

    public string ServerEnemyName
    {
        get
        {
            return m_ServerEnemyName;
        }
    }
    public ChaosHistoryPoolDT(long userId, int serverId,string userName,long enemyId,int serverEnemyId, string enemyName, int battleRes, int recordTime, int note)
    {
        iId = 0;
        m_UserId = userId;
        m_ServerId  = serverId;
        m_UserName = userName != null ? userName : string.Empty;

        m_EnemyId = enemyId;
        m_ServerEnemyId = serverEnemyId;
        m_EnemyName = enemyName != null ? enemyName : string.Empty;

        m_BattleRes = battleRes;
        m_RecordTime = recordTime;
        m_Note = note;

        ServerInforDT serverInfo = (ServerInforDT)glo_Main.GetInstance().m_SC_Pool.m_ServerInforSC.f_GetSC(serverId);
        m_ServerName = serverInfo != null ? serverInfo.szName : string.Empty;

        ServerInforDT serverInfoEnemy = (ServerInforDT)glo_Main.GetInstance().m_SC_Pool.m_ServerInforSC.f_GetSC(serverEnemyId);
        m_ServerEnemyName = serverInfoEnemy != null ? serverInfoEnemy.szName : string.Empty;

    }

    public ChaosHistoryPoolDT(long userId, int serverId, string userName, long enemyId, int serverEnemyId,int battleRes, int recordTime, int note)
    {
        iId = 0;
        m_UserId = userId;
        m_ServerId = serverId;
        m_UserName = userName != null ? userName : string.Empty;

        m_EnemyId = enemyId;
        m_ServerEnemyId = serverEnemyId;
        //m_EnemyName = enemyName != null ? enemyName : string.Empty;

        m_BattleRes = battleRes;
        m_RecordTime = recordTime;
        m_Note = note;

        ServerInforDT serverInfo = (ServerInforDT)glo_Main.GetInstance().m_SC_Pool.m_ServerInforSC.f_GetSC(serverId);
        m_ServerName = serverInfo != null ? serverInfo.szName : string.Empty;

        ServerInforDT serverInfoEnemy = (ServerInforDT)glo_Main.GetInstance().m_SC_Pool.m_ServerInforSC.f_GetSC(serverEnemyId);
        m_ServerEnemyName = serverInfoEnemy != null ? serverInfoEnemy.szName : string.Empty;

    }

    /// <summary>
    /// 更新改变数据（战力和头衔）
    /// </summary>
    /// <param name="power"></param>
    //public void UpdateChangeData(int titleId, long power)
    //{
    //    m_PlayerPower = power;
    //    m_TitleId = titleId;
    //    ChaosBattleTitleDT tTitleTemplate = (ChaosBattleTitleDT)glo_Main.GetInstance().m_SC_Pool.m_ChaosBattleTitleSC.f_GetSC(titleId);
    //    m_TitleName = tTitleTemplate != null ? tTitleTemplate.szName : string.Empty;
    //}
}

