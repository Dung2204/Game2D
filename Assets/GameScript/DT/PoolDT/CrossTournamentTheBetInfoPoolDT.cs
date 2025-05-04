using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
public class CrossTournamentBetInfoPoolDT : BasePoolDT<long>
{
    private long _userId;
    public long m_userId
    {
        get
        {
            return _userId;
        }
    }
    
    private int _Bet;

    public int m_Bet
    {
        get
        {
            return _Bet;
        }
    }
    public CrossTournamentBetInfoPoolDT(long userId,int bet)
    {
        _userId = userId;
        _Bet = bet;
    }

    public bool IsBet(long userId)
    {
        return userId == _userId;
    }
};
public class CrossTournamentTheBetInfoPoolDT : BasePoolDT<long>

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
    public long m_UserIdA
    {
        get
        {
            return _userIdA;
        }
    }
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
    private int _iBetCountA;
    public int m_iBetCountA
    {
        get
        {
            return _iBetCountA;
        }
    }
    private int _iBetCountB;
    public int m_iBetCountB
    {
        get
        {
            return _iBetCountB;
        }
    }
    private int _iBetNumA;
    public int m_iBetNumA
    {
        get
        {
            return _iBetNumA;
        }
    }
    private int _iBetNumB;
    public int m_iBetNumB
    {
        get
        {
            return _iBetNumB;
        }
    }
    private int _iResult;
    public int m_iResult
    {
        get
        {
            return _iResult;
        }
    }
    private long _startTime;
    public long m_startTime
    {
        get
        {
            return _startTime;
        }
    }

    private long _endTime;
    public long m_endTime
    {
        get
        {
            return _endTime;
        }
    }
   
    private CrossTournamentBetInfoPoolDT _MyBet;
    public CrossTournamentBetInfoPoolDT m_MyBet
    {
        get
        {
            return _MyBet;
        }
    }
    public CrossTournamentTheBetInfoPoolDT(int id)
    {
       _Id = id;
    }
    public void f_UpdateInfo(SC_CrossTournamentTheBetInfo info)
    {
        _iThe = info.iThe;
        _userIdA = info.userIdA;
        _userIdB = info.userIdB;
        _iBetCountA = info.betCountA;
        _iBetCountB = info.betCountB;
        _iBetNumA = info.betNumA;
        _iBetNumB = info.betNumB;
        _iResult = info.result;
        _startTime = info.stratTime;
        _endTime = info.endTime;
        if(info.m_node_type != (int)eUpdateNodeType.node_update)
        {
            _MyBet = new CrossTournamentBetInfoPoolDT(info.myBet.userId, info.myBet.bet);
        }
        
    }

    public void f_MyBet(int winNo,int bet,int countA,int countB)
    {
        long userId = _userIdA;
        if (winNo == 2)
        {
            userId = _userIdB;
        }
        _MyBet = new CrossTournamentBetInfoPoolDT(userId, bet);
        _iBetCountA = countA;
        _iBetCountB = countB;
    }
}