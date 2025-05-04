using ccU3DEngine;
using System;

/// <summary>
/// 跨天处理类
/// </summary>
public class Day2Day
{
    private int _iBroadcaseId = 0;
    private bool _bIsSend = false;
    private DateTime _RegDateTime;

    private DateTime _nextDayTime;
    public DateTime mNextDayTime
    {
        get
        {
            return _nextDayTime;
        }
    }


    private int _iStartCheckEventId;

    public void f_Start()
    {
        _iStartCheckEventId = ccTimeEvent.GetInstance().f_RegEvent(4, true, null, CallBack_StartCheck);
    }

    private void CallBack_StartCheck(object Obj)
    {
        if (StaticValue.m_iNewServerTime != -99)
        {
            ccTimeEvent.GetInstance().f_UnRegEvent(_iStartCheckEventId);
            RegNewDayEvent();            
        }
    }

    private void RegNewDayEvent()
    {
        _RegDateTime = ccMath.time_t2DateTime(GameSocket.GetInstance().f_GetServerTime());
        int iSec = (23 - _RegDateTime.Hour) * 3600 + (59 - _RegDateTime.Minute) * 60 + (59 - _RegDateTime.Second) + 5;
        _nextDayTime = _RegDateTime.AddSeconds(iSec);
        ccTimeEvent.GetInstance().f_RegEvent(iSec, false, null, CallBack_TimeOut);
    }
    
    private void CallBack_TimeOut(object Obj)
    {
        RegNewDayEvent();
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.TheNextDay);
    }

}