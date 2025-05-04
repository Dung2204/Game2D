using UnityEngine;
using System.Collections;
using ccU3DEngine;

public class Socket_Regedit : Socket_StateBase
{
    private System.DateTime _dtLoginTimeOut;    
    private static ccCallback CallBack_AccountCreate;
    private string _strRegName;
    private string _strRegPwd;

    public Socket_Regedit(BaseSocket tBaseSocket)
        : base((int)EM_Socket.Regedit, tBaseSocket)
    {
       
    }
       
    public void f_CreateAccount(string strName, string strPwd, ccCallback handler)
    {
        CallBack_AccountCreate = handler;

        CMsg_AccountCreateRelt tCMsg_AccountCreateRelt = new CMsg_AccountCreateRelt();
        _BaseSocket.f_AddListener((int)SocketCommand.SC_UserCreate, tCMsg_AccountCreateRelt, On_AccountCreate);

        _strRegName = strName;
        _strRegPwd = strPwd;
		_dtLoginTimeOut = System.DateTime.Now;
    }
        
    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
MessageBox.DEBUG("Creating an account....");

        if (_BaseSocket.f_GetSocketStatic() == EM_SocketStatic.ConnectSuc)
        {
            DoLogin();
        }
        else
        {
            f_SetComplete((int)EM_Socket.Connect, this);
        }
        
    }

    public override void f_Execute()
    {
        if ((System.DateTime.Now - _dtLoginTimeOut).TotalSeconds > 15)
        {
			CMsg_AccountCreateRelt result = new CMsg_AccountCreateRelt();
			result.m_result = (int)eMsgOperateResult.OR_Error_CreateAccountTimeOut;
			On_AccountCreate(result);
        }
    }

    private void DoLogin()
    {
        //InitMessage();
        //PlayerTools.f_SetFirstReg();
        
        //StaticValue.m_LoginName = strName;
        //StaticValue.m_LoginPwd = strPwd;         //"73b7dfbf835580e77ec4ec1e757ec2a1fb1f5d08";//SystemInfo.deviceUniqueIdentifier;// 返回机器码

        //CMsg_CTG_AccountCreate tCMsg_CTG_AccountCreate = new CMsg_CTG_AccountCreate();
        //tCMsg_CTG_AccountCreate.m_strAccount = _strRegName;
        //tCMsg_CTG_AccountCreate.m_strPassword = _strRegPwd;

        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(GloData.glo_iVer);  //客户端版本号 2016-7-8
        tCreateSocketBuf.f_Add(_strRegName, 28);
        tCreateSocketBuf.f_Add(_strRegPwd, 28);

        _BaseSocket.f_SendBuf2Force((int)SocketCommand.CS_UserCreate, tCreateSocketBuf.f_GetBuf());
    }

    private void On_AccountCreate(object Obj)
    {
        if (_BaseSocket.f_GetSocketStatic() == EM_SocketStatic.LoginEro)
        {
            return;
        }
        CMsg_AccountCreateRelt tCMsg_AccountCreateRelt = (CMsg_AccountCreateRelt)Obj;
        if (CallBack_AccountCreate != null)
        {
            CallBack_AccountCreate(tCMsg_AccountCreateRelt.m_result);
            CallBack_AccountCreate = null;
        }
    }
}
