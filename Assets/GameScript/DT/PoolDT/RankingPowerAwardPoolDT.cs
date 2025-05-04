using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class RankingPowerAwardPoolDT : BasePoolDT<long>
{

    private long m_PlayerId;
    public long PlayerId
    {
        get
        {
            return m_PlayerId;
        }
    }


    private int m_Rank;
    public int Rank
    {
        get
        {
            return m_Rank;
        }
    }

    private int m_Ft;
    public int Ft
    {
        get
        {
            return m_Ft;
        }
    }

    private string m_UserName;
    public string UserName
    {
        get
        {
            return m_UserName;
        }
    }

    private BasePlayerPoolDT m_PlayerInfo;
    public BasePlayerPoolDT PlayerInfo
    {
        get
        {
            return m_PlayerInfo;
        }
    }


    public void f_UpdateInfo(int rank, int ft, string userName)
    {
        m_Rank = rank;
        m_Ft = ft;
        m_UserName = userName;
    }
}
