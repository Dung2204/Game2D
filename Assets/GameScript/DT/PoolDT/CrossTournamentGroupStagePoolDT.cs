using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;

public class CrossTournamentGroupStagePoolDT : BasePoolDT<long>

{
    private int _Id;

    public int m_Id
    {
        get
        {
            return _Id;
        }
    }
    private long _userIdA;
  
    public CrossUserTournamentPoolDT m_userA
    {
        get
        {
            return Data_Pool.m_CrossTournamentPool.GetUser(_userIdA);
        }
    }

    private long _userIdB;

    public CrossUserTournamentPoolDT m_userB
    {
        get
        {
            return Data_Pool.m_CrossTournamentPool.GetUser(_userIdB);
        }
    }
    private int _iWday;
    public int m_iWday
    {
        get
        {
            return _iWday;
        }
    }
    private int _iTime;
    public int m_iTime
    {
        get
        {
            return _iTime;
        }
    }

    private int _iIndex;
    public int m_iIndex
    {
        get
        {
            return _iIndex;
        }
    }

    private int _Result;
    public int m_Result
    {
        get
        {
            return _Result;
        }
    }
   
    public CrossTournamentGroupStagePoolDT(int id)
    {
        _Id = id;
    }
    public void f_UpdateInfo(CMsg_CrossTournamentGroupStageInfo info)
    {
        _userIdA = info.userIdA;
        _userIdB = info.userIdB;
        _iWday = info.wday;
        _iTime = info.time;
        _iIndex = info.index;
        _Result = info.result;
    }

}