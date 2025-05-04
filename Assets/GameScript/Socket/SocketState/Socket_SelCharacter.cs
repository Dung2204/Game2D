using UnityEngine;
using System.Collections;
using ccU3DEngine;

public class Socket_SelCharacter : Socket_StateBase
{

    LoginPage2SelCharacer _LoginPage2SelCharacer = null;
    private string strRoleName = "";
    public Socket_SelCharacter(BaseSocket tBaseSocket)
        : base((int)EM_Socket.SelCharacter, tBaseSocket)
    {

    }

    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
        _LoginPage2SelCharacer = new LoginPage2SelCharacer();

        SC_ReturnRandRoleName tCS_QueryRandRoleName = new SC_ReturnRandRoleName();
        _BaseSocket.f_AddListener((int)SocketCommand.SC_ReturnRandRoleName, tCS_QueryRandRoleName, On_SC_ReturnRandRoleName);
        SC_RoleDIY tSC_RoleDIY = new SC_RoleDIY();
        _BaseSocket.f_AddListener((int)SocketCommand.SC_RoleDIY, tSC_RoleDIY, On_SC_RoleDIY);

        _LoginPage2SelCharacer.Sel_RequestNewName = Sel_RequestNewName;
        _LoginPage2SelCharacer.Sel_RequestSubmit = Sel_Submit;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CreateRolePage, UIMessageDef.UI_OPEN, _LoginPage2SelCharacer);
        
        //要创建角色
        Data_Pool.m_UserData.f_NeedCreateRole();
    }

    private void On_SC_ReturnRandRoleName(object Obj)
    {
        if (_LoginPage2SelCharacer != null && _LoginPage2SelCharacer.Login_RequestNewNameResp != null)
        {
            SC_ReturnRandRoleName tSC_ReturnRandRoleName = (SC_ReturnRandRoleName)Obj;
            _LoginPage2SelCharacer.Login_RequestNewNameResp(tSC_ReturnRandRoleName.szRoleName);
        }
    }

    private void On_SC_RoleDIY(object Obj)
    {
        SC_RoleDIY tSC_RoleDIY = (SC_RoleDIY)Obj;
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)tSC_RoleDIY.orCode;
        if (_LoginPage2SelCharacer != null && _LoginPage2SelCharacer.Login_SubmitResp != null)
        {
            _LoginPage2SelCharacer.Login_SubmitResp(teMsgOperateResult);
        }
        if (teMsgOperateResult == eMsgOperateResult.OR_Succeed)
        {//我方发起请求完成后由我们负责回收
            _LoginPage2SelCharacer = null;
            Data_Pool.m_UserData.m_szRoleName = strRoleName;
            f_SetComplete((int)EM_Socket.Loop);
        }
    }
    
    //申请一个新的名字
    void Sel_RequestNewName(object obj)
    {
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add((int)obj);
        GameSocket.GetInstance().f_SendBuf((int)SocketCommand.CS_QueryRandRoleName, tCreateSocketBuf.f_GetBuf());
    }
    
    /// <summary>
    /// 确认选角结束，提交数据向服务器确认
    /// </summary>
    /// <param name="iData1">1.男 2.女</param>
    /// <param name="szData">名字</param>
    void Sel_Submit(int iData1, int iData2, string szData)
    {
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(iData1);
        tCreateSocketBuf.f_Add(iData2);
        tCreateSocketBuf.f_Add(szData, 28);
        strRoleName = szData;
        GameSocket.GetInstance().f_SendBuf((int)SocketCommand.CS_RoleDIY, tCreateSocketBuf.f_GetBuf());
    }

    public override void f_Execute()
    {
        base.f_Execute();
        
        if (_BaseSocket.f_GetSocketStatic() == EM_SocketStatic.OnLine)
        {
            if (_BaseSocket.f_CheckHaveBuf())
            {
                _BaseSocket.f_DispSendCatchBuf();
            }
        }
        else if (_BaseSocket.f_GetSocketStatic() == EM_SocketStatic.OffLine)
        {
            f_SetComplete((int)EM_Socket.Login, -99);
        }

        base.f_Execute();
    }
}
