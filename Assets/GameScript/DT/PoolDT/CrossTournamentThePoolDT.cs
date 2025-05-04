using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;

public class CrossTournamentThePoolDT : BasePoolDT<long>

{
    private int _Id;

    public int m_Id
    {
        get
        {
            return _Id;
        }
    }
    private int _iThe;
    public int m_iThe
    {
        get
        {
            return _iThe;
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
    
    private int _iWinA;
    public int m_iWinA
    {
        get
        {
            return _iWinA;
        }
    }

    private int _iWinB;
    public int m_iWinB
    {
        get
        {
            return _iWinB;
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

    private int _RecordNo;
    public int m_RecordNo
    {
        get
        {
            return _RecordNo;
        }
    }
    private long _uTime;

    public long m_uTime
    {
        get
        {
            return _uTime;
        }
    }
    private long _totalDamageA;

    public long m_totalDamageA
    {
        get
        {
            return _totalDamageA;
        }
    }
    private long _totalDamageB;

    public long m_totalDamageB
    {
        get
        {
            return _totalDamageB;
        }
    }
    public CrossTournamentThePoolDT(int id)
    {
        _Id = id;
    }
    public void f_UpdateInfo(CMsg_CrossTournamentTheInfo info)
    {
        _iThe = info.iThe;
        _userIdA = info.userIdA;
        _userIdB = info.userIdB;
        _iWinA = info.winA;
        _iWinB = info.winB;
        _Result = info.result;
        _RecordNo = info.recordNo;
        _uTime = info.uTime;
        _totalDamageA = info.damageA;
        _totalDamageB = info.damageB;
    }
    public void f_InitInfo()
    {
        if(_Id >=1 && _Id <= 32)
        {
            _iThe = 64;
        }else if (_Id >= 33 && _Id <= 48)
        {
            _iThe = 32;
        }
        else if (_Id >= 49 && _Id <= 56)
        {
            _iThe = 16;
        }
        else if (_Id >= 57 && _Id <= 60)
        {
            _iThe = 8;
        }
        else if (_Id >= 61 && _Id <= 62)
        {
            _iThe = 4;
        }
        else if (_Id == 63)
        {
            _iThe = 3;
        }
        else if (_Id == 64)
        {
            _iThe = 2;
        }
        _userIdA = 0;
        _userIdB = 0;
        _iWinA = 0;
        _iWinB = 0;
        _Result = 0;
        _RecordNo = 0;
        _uTime = 0;
    }
}