using UnityEngine;
using System.Collections;
using ccU3DEngine;

public class Socket_Login : Socket_StateBase
{
    private stIp _stIp;
    private ccCallback _LoginCallbackFunc;
    private System.DateTime _dtLoginTimeOut;
    private int _iReLogin = 0;
    private int _iReLoginTimeId = -99;
    public Socket_Login(BaseSocket tBaseSocket)
        : base((int)EM_Socket.Login, tBaseSocket)
    {

    }

    public void f_Login(ccCallback func = null)
    {
        _LoginCallbackFunc = func;

        CMsg_GTC_LoginRelt tCMsg_GTC_LoginRelt = new CMsg_GTC_LoginRelt();
        _BaseSocket.f_AddListener((int)SocketCommand.SC_UserLogin, tCMsg_GTC_LoginRelt, On_LoginSuc);
        //basicNode1 tsGameStartNotfy = new basicNode1();
        //_GameSocket.f_AddListener((int)SocketCommand.GTC_GameStartNotfy, tsGameStartNotfy, On_GTC_GameStartNotfy);
    }

    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
MessageBox.DEBUG("Login....");

        _stIp.m_szIp = GloData.glo_strSvrIP;
        _stIp.m_iPort = GloData.glo_iSvrPort;

        Login(null);
    }

    private void Login(object Obj)
    {
        _iReLoginTimeId = -99;
        if (_BaseSocket.f_GetSocketStatic() == EM_SocketStatic.ConnectSuc)
        {
            SendLogin(_iReLogin);
        }
        else
        {
            Socket_Connect tSocket_Connect = (Socket_Connect)f_GetOtherStateBase((int)EM_Socket.Connect);
            tSocket_Connect.f_SetIpInfor(_stIp);
            f_SetComplete((int)EM_Socket.Connect, this);
        }
    }

    public override void f_Execute()
    {
        if ((System.DateTime.Now - _dtLoginTimeOut).TotalSeconds > 30)
        {
            CMsg_GTC_LoginRelt tCMsg_GTC_LoginRelt = new CMsg_GTC_LoginRelt();
            tCMsg_GTC_LoginRelt.m_result = (int) eMsgOperateResult.OR_Error_LoginTimeOut;
            On_LoginSuc(tCMsg_GTC_LoginRelt);
        }
    }
    
    private void SendLogin(int iReLogin = 0)
    {        
        _dtLoginTimeOut = System.DateTime.Now;
        if (glo_Main.GetInstance().m_SDKCmponent.IsChannel)
        {// 第三平台登陆注册
            string strMobileCode = SystemInfo.deviceUniqueIdentifier;
            CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
            tCreateSocketBuf.f_Add(iReLogin);
            tCreateSocketBuf.f_Add(Data_Pool.m_UserData.m_iServerId);
            tCreateSocketBuf.f_Add(GloData.glo_iVer);
            tCreateSocketBuf.f_Add(glo_Main.GetInstance().m_SDKCmponent.ChannelRoleInfo.ChannelUserId, SDKChannelRoleInfo.CHANNEL_USERID_MAX_LEN);
            tCreateSocketBuf.f_Add(glo_Main.GetInstance().m_SDKCmponent.ChannelRoleInfo.ChannelFlag, SDKChannelRoleInfo.CHANNEL_FLAG_MAX_LEN);            
            tCreateSocketBuf.f_Add(strMobileCode, 64);       //m_strMobileCode
            tCreateSocketBuf.f_Add(glo_Main.GetInstance().m_SDKCmponent.ChannelRoleInfo.Token, SDKChannelRoleInfo.TOKEN_MAX_LEN);
            _BaseSocket.f_SendBuf2Force((int)SocketCommand.CS_UserLoginChannel, tCreateSocketBuf.f_GetBuf());
        }
        else
        {
            CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
            tCreateSocketBuf.f_Add(iReLogin);
            tCreateSocketBuf.f_Add(Data_Pool.m_UserData.m_iServerId);
            tCreateSocketBuf.f_Add(GloData.glo_iVer);            
            tCreateSocketBuf.f_Add(0);
            tCreateSocketBuf.f_Add(0);
            tCreateSocketBuf.f_Add(StaticValue.m_LoginName, 28);
            tCreateSocketBuf.f_Add(StaticValue.m_LoginPwd, 28);
            _BaseSocket.f_SendBuf2Force((int)SocketCommand.CS_UserLogin, tCreateSocketBuf.f_GetBuf());
            //MessageBox.DEBUG("发送登陆游戏申请 " + iNum);
        }
        _BaseSocket.f_SetSocketStatic(EM_SocketStatic.Logining);
    }

    private void On_LoginSuc(object Obj)
    {
		/*
        if (_GameSocket.f_GetSocketStatic() == EM_SocketStatic.LoginEro)
        {
            return;
        }
        */
        CMsg_GTC_LoginRelt tCMsg_GTC_LoginRelt = (CMsg_GTC_LoginRelt)Obj;
        //_GameSocket.f_RemoveListener(SocketCommand.GTC_AccountEnterResult);
MessageBox.DEBUG("Response： " + tCMsg_GTC_LoginRelt.m_result);

        if (tCMsg_GTC_LoginRelt.m_result == (int)eMsgOperateResult.eOR_Account_Forbidden || 
            tCMsg_GTC_LoginRelt.m_result == (int)eMsgOperateResult.eOR_IP_Forbidden)
        {
            if (_LoginCallbackFunc != null)
            {
                _LoginCallbackFunc(tCMsg_GTC_LoginRelt.m_result);
            }
            _BaseSocket.f_Close();
            return;
        }

        if(tCMsg_GTC_LoginRelt.m_result == (int)eMsgOperateResult.OR_Error_CreateAccountTimeOut
            || tCMsg_GTC_LoginRelt.m_result == (int)eMsgOperateResult.OR_Error_LoginTimeOut)
        {
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.OnLoginQueueEvent, LoginQueueType.LoginQueueType_FakeQueue);
        }else if (_LoginCallbackFunc != null)
        {
            //超时都不回调，，弹出排队界面
            _LoginCallbackFunc(tCMsg_GTC_LoginRelt.m_result);
        }

        if (tCMsg_GTC_LoginRelt.m_result == (int)eMsgOperateResult.OR_Succeed)
        {
            _iReLogin = 1;
            _LoginCallbackFunc = null;
            _BaseSocket.f_SetSocketStatic(EM_SocketStatic.OnLine);
            _BaseSocket.UpdateServerTime(tCMsg_GTC_LoginRelt.m_iServerTime);

            Data_Pool.m_UserData.m_iUserId = tCMsg_GTC_LoginRelt.m_PlayerId;
            //Data_Pool.m_UserData.m_iServerId = tCMsg_GTC_LoginRelt.m_iServerId;
            //Data_Pool.m_UserData.m_strServerName = tCMsg_GTC_LoginRelt.m_strServerName;
MessageBox.DEBUG("Login successful, User Id:" + Data_Pool.m_UserData.m_iUserId);
            GameSet.f_SaveAccountInfor();
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.OnLoginQueueEvent, LoginQueueType.LoginQueueType_Close);
            _BaseSocket.f_Ping();
            f_SetComplete((int)EM_Socket.Loop);
        }
        else if (tCMsg_GTC_LoginRelt.m_result == (int)eMsgOperateResult.eOR_CreateAndLogin)
        {//创建账号并登陆
            _iReLogin = 1;
            _LoginCallbackFunc = null;
            _BaseSocket.f_SetSocketStatic(EM_SocketStatic.OnLine);
            _BaseSocket.UpdateServerTime(tCMsg_GTC_LoginRelt.m_iServerTime);

            Data_Pool.m_UserData.m_iUserId = tCMsg_GTC_LoginRelt.m_PlayerId;
            //Data_Pool.m_UserData.m_iServerId = tCMsg_GTC_LoginRelt.m_iServerId;
            //Data_Pool.m_UserData.m_strServerName = tCMsg_GTC_LoginRelt.m_strServerName;
MessageBox.DEBUG("Create an account and login.");
            GameSet.f_SaveAccountInfor();
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.OnLoginQueueEvent, LoginQueueType.LoginQueueType_Close);
            _BaseSocket.f_Ping();
            f_SetComplete((int)EM_Socket.SelCharacter);
        }
        else if (tCMsg_GTC_LoginRelt.m_result == (int)eMsgOperateResult.eOR_LoginInDeque)
        {
            //排队中,后端约定m_iServerId为排队人数
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.OnLoginQueueEvent, tCMsg_GTC_LoginRelt.m_iServerId);

            //避免弹出超时提示
            _dtLoginTimeOut = System.DateTime.Now;
        }
        else
        {
            if (_iReLoginTimeId <= 0)
            {
                _BaseSocket.f_Close();
                //if ((eMsgOperateResult)tCMsg_GTC_LoginRelt.m_result == eMsgOperateResult.OR_Error_AccountRepetition ||    // = 20, // 注册：账号重复
                //    (eMsgOperateResult)tCMsg_GTC_LoginRelt.m_result == eMsgOperateResult.OR_Error_NoAccount ||    // = 21 ||    //, // 登陆：账号不存在
                //    (eMsgOperateResult)tCMsg_GTC_LoginRelt.m_result == eMsgOperateResult.OR_Error_Password ||    // = 22 ||    //, // 登陆：密码错误
                //    (eMsgOperateResult)tCMsg_GTC_LoginRelt.m_result == eMsgOperateResult.OR_Error_AccountOnline)    // = 24)    //, // 登陆：账号在线)
                //{
                //    _GameSocket.f_SetSocketStatic(EM_SocketStatic.LoginEro);
                //}
                _BaseSocket.f_SetSocketStatic(EM_SocketStatic.LoginEro);

                eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)tCMsg_GTC_LoginRelt.m_result;
MessageBox.DEBUG("Login failed。 " + teMsgOperateResult.ToString());
MessageBox.DEBUG("Re-login。");
                _iReLoginTimeId = ccTimeEvent.GetInstance().f_RegEvent(10f, false, null, Login);
            }
        }

    }


#region 游戏开始协议

    private void On_GTC_GameStartNotfy(object Obj)
    {
        //basicNode0 tsForceOffline = (basicNode0)Obj;
MessageBox.DEBUG("Start game 11111111111111");
        //if (_LoginCallbackFunc != null)
        //{
        //    _LoginCallbackFunc((object)eMsgOperateResult.OR_Succeed);
        //    _LoginCallbackFunc = null;
        //}
    }

#endregion


}
