using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class PatrolFriendInfoPoolDT : BasePoolDT<long>
{
    private BasePlayerPoolDT playerInfo;
    public BasePlayerPoolDT m_PlayerInfo
    {
        get
        {
            return playerInfo;
        }
    }

    public PatrolFriendInfoPoolDT(long friendId,int unlockNum,int patrolingNum,int riotingNum)
    {
        iId = friendId;
        if (friendId == Data_Pool.m_UserData.m_iUserId)
MessageBox.ASSERT("Your id matches the player id");
        playerInfo = (BasePlayerPoolDT)Data_Pool.m_GeneralPlayerPool.f_GetForId(friendId);
        if (playerInfo == null)
MessageBox.ASSERT("Friend data null,Id:" + friendId);
        this.unlockNum = unlockNum;
        this.patrolingNum = patrolingNum;
        this.riotingNum = riotingNum;
    }
    
    private int unlockNum;
    /// <summary>
    /// 领地解锁的数量
    /// </summary>
    public int m_iUnlockNum
    {
        get
        {
            return unlockNum;
        }
    }

    private int patrolingNum;
    /// <summary>
    /// 领地巡逻数量
    /// </summary>
    public int m_iPatrolingNum
    {
        get
        {
            return patrolingNum;
        }
    }

    private int riotingNum;
    /// <summary>
    /// 暴动中的数量
    /// </summary>
    public int m_iRiotingNum
    {
        get
        {
            return riotingNum;
        }
    }

}
