using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class ArenaPoolDT  : BasePoolDT<long>
{
    public ArenaPoolDT()
    {

    }

    private int mRank;
    public int m_iRank
    {
        get
        {
            return mRank;
        }
    }

    private BasePlayerPoolDT mPlayerInfo;
    public BasePlayerPoolDT m_PlayerInfo
    {
        get
        {
            return mPlayerInfo;
        }
    }


    public void f_UpdateInfo(int rank,BasePlayerPoolDT playerInfo)
    {
        mRank = rank;
        mPlayerInfo = playerInfo;
    }

    private CMsg_ArenaCrossInfo mArenaCrossInfo;
    public CMsg_ArenaCrossInfo m_ArenaCrossInfo
    {
        get
        {
            return mArenaCrossInfo;
        }
    }

    public void f_UpdateArenaCrossInfo(CMsg_ArenaCrossInfo msg_ArenaCrossInfo)
    {
        mRank = msg_ArenaCrossInfo.uRank;
        mArenaCrossInfo = msg_ArenaCrossInfo;
    }
}
