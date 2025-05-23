﻿using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Text;

/// <summary>
/// socket连接状态
/// </summary>
public class Socket_Connect : Socket_StateBase
{
    private stIp _stIp;
    private Socket_StateBase _Socket_StateBaseForNext = null;
    private bool _bConnecting = false;
    //private ccCallback _ConnectCallback;

    private int _iRetryTimeId = 0;

    public Socket_Connect(BaseSocket tBaseSocket)
        : base((int)EM_Socket.Connect, tBaseSocket)
    {
        
    }

    public void f_SetIpInfor(stIp tstIp)
    {
        _stIp = tstIp;
    }

    /// <summary>
    /// 状态进入
    /// </summary>
    /// <param name="Obj"></param>
    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
Debug.LogError("Initiating connection....");
        if (Obj == null)
        {
Debug.LogError("Unknown status");
            return;
        }
        _Socket_StateBaseForNext = (Socket_StateBase)Obj;
        Connect();
    }

    /// <summary>
    /// 连接服务器
    /// </summary>
    private void Connect()
    {
        if (_bConnecting)
        {
            return;
        }
        _bConnecting = true;
        InitSocket();
    }

    /// <summary>
    /// 初始化socket
    /// </summary>
    /// <param name="iTime"></param>
    /// <returns></returns>
    private bool InitSocket(int iTime = 2000)
    {
        _BaseSocket.f_Close();
        _BaseSocket.f_SetSocketStatic(EM_SocketStatic.Connecting);

Debug.LogError("Initiating connection...." + _stIp.m_szIp + ":" + _stIp.m_iPort);

        //UpdateServerInfor();

        //ResetTimeOut();
        if (!_BaseSocket.Connect(_stIp.m_szIp, _stIp.m_iPort, _BaseSocket.f_Router, false, ConnectCallBack, iTime))
        {
            _BaseSocket.f_Close();
            //MessageBox.ASSERT("连接游戏失败");
            ConnectCallBack(false);
            return false;
        }

        return true;
    }
    /// <summary>
    /// socket连接回调
    /// </summary>
    /// <param name="oData"></param>
    private void UpdateServerInfor()
    {
        GloData.glo_strSvrIP = "192.168.0.128";
        GloData.glo_iSvrPort = 58500;

        //string strMyChanel = GloData.m_PayChannel.ToUpper();
        //ServerInforDT tServerInforDT = glo_Main.GetInstance().m_SC_Pool.m_ServerInforSC.f_Get(strMyChanel);
        //if (tServerInforDT == null)
        //{
        //    GloData.glo_strVer = "EE" + GloData.glo_strVer;
        //}
        //else
        //{
        //    GloData.glo_strHttpServerIP = tServerInforDT.strResIP;
        //    GloData.glo_strSvrIP = tServerInforDT.strIP;
        //    GloData.glo_iSvrPort = tServerInforDT.iPort;
        //    if (strMyChanel.Length > 4)
        //    {
        //        strMyChanel = strMyChanel.Substring(0, 4);
        //    }
        //    GloData.glo_strVer = strMyChanel + "" + tServerInforDT.Id + "." + GloData.glo_strVer;

        //}
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(MessageDef.LOGINEROINFOR);
    }

    private void ConnectCallBack(object oData)
    {
        _bConnecting = false;
        if (_BaseSocket.f_GetSocketStatic () == EM_SocketStatic.ConnectSuc)
        {
Debug.LogError("Connection successful，invalid timeout status");
            return;
        }
        bool bRet = (bool)oData;
        //_ConnectCallback(bRet);
        if (bRet)
        {
Debug.LogError("Connection successful");
            _BaseSocket.f_SetSocketStatic(EM_SocketStatic.ConnectSuc);
            f_SetComplete((int)_Socket_StateBaseForNext.iId);
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.RETRYCONNECTSUC);
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.OnLoginQueueEvent, LoginQueueType.LoginQueueType_Close);
        }
        else
        {
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.OnLoginQueueEvent, LoginQueueType.LoginQueueType_FakeQueue);
            ccTimeEvent.GetInstance().f_RegEvent(6, false, null, Callback_RetryConnect);
        }
    }
    
    //private System.DateTime _dtLoginTimeOut;
    //public override void f_Execute()
    //{
    //    if (_bRetryConnect)
    //    {
    //        if ((System.DateTime.Now - _dtLoginTimeOut).TotalSeconds > 6)
    //        {
    //            MessageBox.DEBUG("重连...."  + _stIp.m_szIp + ":" + _stIp.m_iPort);
    //            ResetTimeOut();

    //            ccTimeEvent.GetInstance().f_RegEvent(0.1f, false, null, Callback_RetryConnect);

    //            //连接重试连接失败，弹出菊花UI
    //            //_bRetryConnect = false;
    //            //f_SetComplete((int)_Socket_StateBaseForNext.iId, (int)eMsgOperateResult.OR_Error_ConnectTimeOut);
    //            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.GAMEMESSAGEBOX, (int)eMsgOperateResult.OR_Error_ConnectTimeOut);
    //            glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.RETRYCONNECT, Callback_RetryConnect, null);
    //            return;

    //        }
    //    }
    //}

    //void ResetTimeOut()
    //{
    //    _dtLoginTimeOut = System.DateTime.Now;
    //}

    /// <summary>
    /// 重连
    /// </summary>
    /// <param name="Obj"></param>
	void CheckNet()
	{
		switch (Application.internetReachability)
		{
			case NetworkReachability.NotReachable:
TopPopupMenuParams param = new TopPopupMenuParams("Thông báo", "Kết nối mạng không ổn định, vui lòng kiểm tra lại.", "Xác nhận", TTTT);
				ccUIManage.GetInstance().f_SendMsg(UINameConst.TopPopupMenuPage, UIMessageDef.UI_OPEN, param);
				// ccUIManage.GetInstance().f_SendMsg(UINameConst.NetworkErrorPage, UIMessageDef.UI_OPEN);
				break;
		}
	}
	
	private void TTTT(object Obj)
    {

    }
	
    private void Callback_RetryConnect(object Obj)
    {
        if (_BaseSocket.f_GetSocketStatic() == EM_SocketStatic.ConnectSuc || _bConnecting == true)
        {
            UITool.f_OpenOrCloseWaitTip(false);
MessageBox.DEBUG("Connection successful, disconnecting state again");
            return;
        }
        //连接重试连接失败，弹出菊花UI
        //_bRetryConnect = false;
        //f_SetComplete((int)_Socket_StateBaseForNext.iId, (int)eMsgOperateResult.OR_Error_ConnectTimeOut);
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.GAMEMESSAGEBOX, (int)eMsgOperateResult.OR_Error_ConnectTimeOut);
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.RETRYCONNECT, Callback_RetryConnect, null);
		CheckNet();
        Connect();
    }
     


}
