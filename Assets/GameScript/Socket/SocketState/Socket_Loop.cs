using System;
using UnityEngine;
using System.Collections;
using System.Timers;
using ccU3DEngine;

public class Socket_Loop : Socket_StateBase
{
    private int _iPingId = -99;
    private System.DateTime m_dtPingTime;
    private bool _bTestTime = false;

    private System.Timers.Timer timer = null;//new System.Timers.Timer(GloData.glo_iPingTime * 1000);

    public Socket_Loop(BaseSocket tBaseSocket)
        : base((int)EM_Socket.Loop, tBaseSocket)
    {
        timer = new System.Timers.Timer(GloData.glo_iPingTime * 1000);
        timer.Elapsed += new System.Timers.ElapsedEventHandler(Callback_Ping1);
        timer.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
        //timer.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
Debug.LogError("=====================================Initialize Timer"+_BaseSocket .m_strTTTT);
    }

    //到达时间的时候执行事件；
    public void Callback_Ping1(object source, System.Timers.ElapsedEventArgs e)
    {
        if (_BaseSocket.f_GetSocketStatic() == EM_SocketStatic.OnLine)
        {
            //int iTime = (int)(System.DateTime.Now - _BaseSocket.m_dtSocketTimeout).TotalSeconds;
            //if (iTime > GloData.glo_iPingTime)
            //{
            _iRetryTimes = 0;
            _bTestTime = true;
            m_dtPingTime = System.DateTime.Now;
            _BaseSocket.f_Ping();
            //Debug.LogError("=====================================重复ping"+ "    " +_BaseSocket.m_strTTTT);
            //}
        }
        else if(_BaseSocket.f_GetSocketStatic() == EM_SocketStatic.OffLine)
        {
            timer.Stop();
        }
    }

    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
        f_UpdateTestTime();
        if (_iPingId == -99)
        {
Debug.LogError("======================= Timer Start：" + _BaseSocket.m_strTTTT);
            timer.Start();
            _iPingId = 110000;
            //_iPingId = ccTimeEvent.GetInstance().f_RegEvent(GloData.glo_iPingTime, true, null, Callback_Ping);
        }
    }

    public override void f_Execute()
    {
        base.f_Execute();

        if (_BaseSocket.f_GetSocketStatic() == EM_SocketStatic.OnLine)
        {
            if (TestSocketOnline())// && _BaseSocket.f_CheckHaveBuf())
            {
                if (!_bTestTime)
                {
                    _BaseSocket.f_DispSendCatchBuf();
                }
            }
            else
            {
                timer.Stop();
Debug.LogError("======================= Timer End：" + _BaseSocket.m_strTTTT);
                //ccTimeEvent.GetInstance().f_UnRegEvent(_iPingId);
                _iPingId = -99;
                _BaseSocket.f_Close();
            }
        }
        else if (_BaseSocket.f_GetSocketStatic() == EM_SocketStatic.OffLine)
        {
            f_SetComplete((int)EM_Socket.Login, -99);
        }
        else
        {
MessageBox.ASSERT("Socket error status " + _BaseSocket.f_GetSocketStatic().ToString());
        }
    }

    private bool TestSocketOnline()
    {
        if (_BaseSocket.f_TestSocket())
        {
            //if (!glo_Main.GetInstance().m_StarSDK.f_CheckIsPaying())
            //{
            if (PingTimeOut())
            {
                return false;
            }
            //}
        }
        else
        {
            return false;
        }
        return true;
    }

    public void f_UpdateTestTime()
    {
        _iRetryTimes = 0;
        _bTestTime = false;
        m_dtPingTime = System.DateTime.Now;
    }

    private void Callback_Ping(object Obj)
    {
        if (_BaseSocket.f_GetSocketStatic() == EM_SocketStatic.OnLine)
        {
            //int iTime = (int)(System.DateTime.Now - _BaseSocket.m_dtSocketTimeout).TotalSeconds;
            //if (iTime > GloData.glo_iPingTime)
            //{
            _iRetryTimes = 0;
            _bTestTime = true;
            m_dtPingTime = System.DateTime.Now;
            _BaseSocket.f_Ping();
            //}
        }
    }

    private int _iRetryTimes = 0;
    private bool PingTimeOut()
    {
        if (_bTestTime)
        {
            int iTime = (int)(System.DateTime.Now - m_dtPingTime).TotalSeconds;
            if (iTime > 5)
            {
                if (_iRetryTimes > 2)
                {
                    f_UpdateTestTime();
MessageBox.DEBUG("PingTimeOut connection timed out。");
Debug.LogError("PingTimeOut connection timed out。");
                    //_BaseSocket.f_Close();
                    return true;
                }

                m_dtPingTime = System.DateTime.Now;
                _BaseSocket.f_Ping();
                _iRetryTimes++;
                Debug.LogError("========================="+_iRetryTimes);
            }
        }
        return false;
        //int iTime = (int)(System.DateTime.Now - _BaseSocket.m_dtSocketTimeout).TotalSeconds;
        //if (iTime > GloData.glo_iRecvPingTime)
        //{
        //    MessageBox.DEBUG("BufTimeOut 网络超时。");
        //    //_BaseSocket.f_Close();
        //    return true;
        //}
        //return false;
    }

}
