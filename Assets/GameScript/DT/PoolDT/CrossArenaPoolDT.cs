using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class CrossArenaPoolDT : BasePoolDT<long>
{
    public CrossArenaPoolDT()
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
}
