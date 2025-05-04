using ccU3DEngine;

public class RunningManRankPoolDT : BasePoolDT<long>
{
    public RunningManRankPoolDT(int rank,long userId,int starNum)
    {
        m_iRank = rank;
        m_iPlayerId = userId;
        playerInfo = (BasePlayerPoolDT)Data_Pool.m_GeneralPlayerPool.f_GetForId(userId);
        if (userId == Data_Pool.m_UserData.m_iUserId)
            LegionTool.f_SelfPlayerInfoDispose(ref playerInfo);
        if (m_PlayerInfo == null)
MessageBox.ASSERT("Player data does not exist, UserId");
        m_iStarNum = starNum;
    }

    public void UpdateRankAndStar(int rank,  int starNum)
    {
        m_iRank = rank;
        m_iStarNum = starNum;
    }

    public int m_iRank
    {
        private set;
        get;
    }

    public long m_iPlayerId
    {
        private set;
        get;
    }

    private BasePlayerPoolDT playerInfo;
    public BasePlayerPoolDT m_PlayerInfo
    {
        get
        {
            return playerInfo;
        }
    }

    public int m_iStarNum
    {
        private set;
        get;
    } 
}
