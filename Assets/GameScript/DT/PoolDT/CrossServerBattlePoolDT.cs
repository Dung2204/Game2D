using ccU3DEngine;

/// <summary>
/// 跨服战对手信息
/// </summary>
public class CrossServerBattlePoolDT : BasePoolDT<long>
{
    private int m_ServerId;
    private string m_ServerName;

    public string ServerName
    {
        get
        {
            return m_ServerName;
        }
    }

    private bool m_IsNull = true;
    /// <summary>
    /// 是否重置，置空
    /// </summary>
    public bool IsNull
    {
        get
        {
            return m_IsNull;
        }
    }

    private int m_CardId;
    /// <summary>
    /// 卡牌Id
    /// </summary>
    public int CardId
    {
        get
        {
            if(m_CardId == 0)
            {
                return 1000;
            }
            return m_CardId;
        }
    }

    public int Sex
    {
        get
        {
            return m_CardId;
        }
    }



    private int m_FrameId;
    /// <summary>
    /// 边框Id
    /// </summary>
    public int FrameId
    {
        get
        {
            return m_FrameId;
        }
    }

    private string m_PlayerName;
    /// <summary>
    /// 角色名字
    /// </summary>
    public string PlayerName
    {
        get
        {
            return m_PlayerName == null ? string.Empty : m_PlayerName;
        }
    }

    private int m_Power;
    /// <summary>
    /// 战斗力
    /// </summary>
    public int Power
    {
        get
        {
            return m_Power;
        }
    }

    private int m_level;

    public int Level
    {
        get
        {
            return m_level;
        }
    }

    public void f_UpdateInfo(long playerId, int serverId, int cardId, int frameId, int power, string playerName, int level)
    {
        m_IsNull = false;
        iId = playerId;
        m_CardId = cardId;
        m_FrameId = frameId;
        m_Power = power;
        m_PlayerName = playerName;
        m_ServerId = serverId;
        m_level = level;
        ServerInforDT serverInfor = (ServerInforDT)glo_Main.GetInstance().m_SC_Pool.m_ServerInforSC.f_GetSC(serverId);
        if(serverInfor == null)
MessageBox.ASSERT("The inter-server arena sent data does not exist,serverId:" + serverId);
        m_ServerName = serverInfor == null ? string.Empty : serverInfor.szName;
    }

    public void f_ResetInfo()
    {
        m_IsNull = true;
    }
}
