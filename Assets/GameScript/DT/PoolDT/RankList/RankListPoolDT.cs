using ccU3DEngine;

public class RankListPoolDT : BasePoolDT<long>
{
    private int m_Rank;
    /// <summary>
    /// 排名
    /// </summary>
    public int Rank
    {
        get
        {
            return m_Rank;
        }
    }

    private BasePlayerPoolDT m_PlayerInfo;
    /// <summary>
    /// 玩家信息
    /// </summary>
    public BasePlayerPoolDT PlayerInfo
    {
        get
        {
            return m_PlayerInfo;
        }
    }

    private int m_PraiseTimes;
    /// <summary>
    /// 被点赞次数
    /// </summary>
    public int PraiseTimes
    {
        get
        {
            return m_PraiseTimes;
        }
    }

    private int m_iCurChapterId;
    /// <summary>
    /// 当前章节
    /// </summary>
    public int CurChapterId
    {
        get
        {
            return m_iCurChapterId;
        }
    }


    /// <summary>
    /// 是否已经点赞   点赞次数 >= 点赞限制次数代表已捐赠
    /// </summary>
    public bool AlreadyPraise
    {
        get
        {
            return Data_Pool.m_RankListPool.SelfPraiseTimes >= RankListPool.SelfPraiseTimesLimit;
        }
    }
    
    public void f_UpdateInfo(int rank, BasePlayerPoolDT playerInfo)
    {
        if (null == playerInfo)
            return;
        iId = playerInfo.iId;
        m_Rank = rank;
        m_PlayerInfo = playerInfo;
    }

    public void f_UpdatePraiseInfo(int praiseTimes)
    {
        m_PraiseTimes = praiseTimes;

    }

    public void f_UpdateCurChapterId(int curChapterId)
    {
        m_iCurChapterId = curChapterId;

    }      
}
