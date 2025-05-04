using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Text;

public class Socket_ChatLogin : Socket_StateBase
{
    private stIp _stIp;
    private ccCallback _LoginCallbackFunc;
    private System.DateTime _dtLoginTimeOut;
    private int _iReLogin = 0;
    private ulong _iLoginToken;

    public Socket_ChatLogin(BaseSocket tBaseSocket)
        : base((int)EM_Socket.Login, tBaseSocket)
    {

    }

    public bool f_CheckIsLogined()
    {
        if (_iReLogin == 0)
        {
            return false;
        }
        return true;
    }
    
    public void f_UpdateLoginToken(SC_RelationServer tSC_RelationServer)
    {
        _stIp.m_szIp = ChangeIp(tSC_RelationServer.iHost);
        _stIp.m_iPort = tSC_RelationServer.iPort;
        _iLoginToken = tSC_RelationServer.iLoginToken;
                
        //basicNode1 tsGameStartNotfy = new basicNode1();
        //_GameSocket.f_AddListener((int)SocketCommand.GTC_GameStartNotfy, tsGameStartNotfy, On_GTC_GameStartNotfy);
    }

    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
        MessageBox.DEBUG("Chat login....");

        if (_iReLogin == 0)
        {
            _iReLogin = 1;
            CMsg_GTC_ChatLoginRelt tCMsg_GTC_LoginRelt = new CMsg_GTC_ChatLoginRelt();
            _BaseSocket.f_AddListener((int)ChatSocketCommand.RC_Login, tCMsg_GTC_LoginRelt, On_LoginSuc);
        }
        Login(null);
    }

    private void Login(object Obj)
    {
        _dtLoginTimeOut = System.DateTime.Now;
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
            CMsg_GTC_ChatLoginRelt tCMsg_GTC_LoginRelt = new CMsg_GTC_ChatLoginRelt();
            tCMsg_GTC_LoginRelt.m_result = (int) eMsgOperateResult.OR_Error_LoginTimeOut;
            On_LoginSuc(tCMsg_GTC_LoginRelt);
        }
    }

    private void SendLogin(int iReLogin = 0)
    {
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(Data_Pool.m_UserData.m_iUserId);
        tCreateSocketBuf.f_Add(_iLoginToken);
        _BaseSocket.f_SendBuf2Force((int)ChatSocketCommand.CR_Login, tCreateSocketBuf.f_GetBuf());
MessageBox.DEBUG("Send chat login request " + _iLoginToken);

        _BaseSocket.f_SetSocketStatic(EM_SocketStatic.Logining);
    }

    private void On_LoginSuc(object Obj)
    {
        CMsg_GTC_ChatLoginRelt tCMsg_GTC_LoginRelt = (CMsg_GTC_ChatLoginRelt)Obj;
        //_GameSocket.f_RemoveListener(SocketCommand.GTC_AccountEnterResult);
MessageBox.DEBUG("Login chat result： " + tCMsg_GTC_LoginRelt.m_result);
        if (_LoginCallbackFunc != null)
        {
            _LoginCallbackFunc(tCMsg_GTC_LoginRelt.m_result);
            _LoginCallbackFunc = null;            
        }
        if (tCMsg_GTC_LoginRelt.m_result == (int)eMsgOperateResult.OR_Succeed)
        {           
            _BaseSocket.f_SetSocketStatic(EM_SocketStatic.OnLine);

MessageBox.DEBUG("Chat login successful");
            _BaseSocket.f_Ping();
            f_SetComplete((int)EM_Socket.Loop);
        }     
        else
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
MessageBox.DEBUG("Chat login failed。 " + teMsgOperateResult.ToString());
            //if (_iReLogin == 1)
            //{
MessageBox.DEBUG("Login again。 ");
            ccTimeEvent.GetInstance().f_RegEvent(10f, false, null, Login);
            //}
        }

    }

    public string ChangeIp(long ipInt)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(ipInt & 0xFF).Append(".");
        sb.Append((ipInt >> 8) & 0xFF).Append(".");
        sb.Append((ipInt >> 16) & 0xFF).Append(".");
        sb.Append((ipInt >> 24) & 0xFF);
        return sb.ToString();
    }

    #region 游戏开始协议

    private void On_GTC_GameStartNotfy(object Obj)
    {
        //basicNode0 tsForceOffline = (basicNode0)Obj;
        MessageBox.DEBUG("Game Start 1111111111111。 ");
        //if (_LoginCallbackFunc != null)
        //{
        //    _LoginCallbackFunc((object)eMsgOperateResult.OR_Succeed);
        //    _LoginCallbackFunc = null;
        //}
    }

#endregion


}
