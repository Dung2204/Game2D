using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 切换账号页面
/// </summary>
public class SwichAccountPage : UIFramwork
{
    public static bool mIsEnter = false;
    private string strName;//界面输入的用户名
    private string strPwd;//界面输入的密码
    /// <summary>
    /// 页面开启
    /// </summary>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        string userName = LocalDataManager.f_GetLocalData<string>(LocalDataType.String_UserName);
        string psw = LocalDataManager.f_GetLocalData<string>(LocalDataType.String_Password);
        if (userName != "")
        {
            f_GetObject("InUserName").GetComponent<UIInput>().value = userName;
            f_GetObject("InPsw").GetComponent<UIInput>().value = psw;
        }
    }
    /// <summary>
    /// 注册消息
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnLogin", OnBtnLoginClick);
        f_RegClickEvent("BtnRegister", OnBtnRegisterClick);
        f_RegClickEvent("BtnBlack", OnBtnBlackClick);
    }
    /// <summary>
    /// 点击黑色背景关闭界面
    /// </summary>
    private void OnBtnBlackClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SwichAccountPage, UIMessageDef.UI_CLOSE);
    }
    protected override void f_Update()
    {
        base.f_Update();
        if (!mIsEnter && Input.GetKeyDown(KeyCode.Return))
        {
            OnBtnLoginClick(null, null, null);
        }
    }
    /// <summary>
    /// 点击登录界面按钮
    /// </summary>
    private void OnBtnLoginClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);

        if (LoginPage.selectServerDT == null)
        {
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Chưa có máy chủ");
            return;
        }
        if (UITool.f_GetServerState(LoginPage.selectServerDT) == EM_ServerState.Maintain)
        {
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Máy chủ đang được bảo trì");
            return;
        }

        strName = f_GetObject("InUserName").GetComponent<UIInput>().value;
        strPwd = f_GetObject("InPsw").GetComponent<UIInput>().value;
        if (strName == "")
        {
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Tài khoản không được để trống!");
            return;
        }
        if (strPwd == "")
        {
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Mật khẩu không được để trống!");
            return;
        }
        //账号
        StaticValue.m_LoginName = strName;
        //密码
        StaticValue.m_LoginPwd = strPwd;
        UITool.f_OpenOrCloseWaitTip(true);
        GameSocket.GetInstance().f_Login(LoginGame_CallBack);
        IsLoginRsp = false;
        mIsEnter = true;
        ccTimeEvent.GetInstance().f_RegEvent(8f, false, null, OnTimeOutCallback);
        //记录服务器Id
        if (!LocalDataManager.f_HasLocalData(LocalDataType.String_ServerID))
        {
            LocalDataManager.f_SetLocalData(LocalDataType.String_ServerID, Data_Pool.m_UserData.m_iServerId.ToString());
        }
        else
        {
            string tempServerId = LocalDataManager.f_GetLocalData<string>(LocalDataType.String_ServerID);
            if (tempServerId != Data_Pool.m_UserData.m_iServerId.ToString())
            {
                LocalDataManager.f_SetLocalData(LocalDataType.String_ServerID, Data_Pool.m_UserData.m_iServerId.ToString());
                LocalDataManager.f_SetLocalData(LocalDataType.String_ServerID2, tempServerId);
            }
        }
    }
    private bool IsLoginRsp = false;//登录服务器是否返回消息
    /// <summary>
    /// 登录超时检测
    /// </summary>
    private void OnTimeOutCallback(object obj)
    {
        if (!IsLoginRsp)
        {
            mIsEnter = false;
            UITool.f_OpenOrCloseWaitTip(false);
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Hết thời gian kết nối, vui lòng kiểm tra kết nối mạng");
        }
    }
    /// <summary>
    /// 登录游戏回调
    /// </summary>
    /// <param name="data">服务器返回的数据</param>
    private void LoginGame_CallBack(object Obj)
    {
        IsLoginRsp = true;
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)Obj;
        mIsEnter = false;
        if (teMsgOperateResult == eMsgOperateResult.eOR_LoginInDeque)
        {
            //登陆排队不提示信息（弹出登陆排队界面）
            return;
        }
        switch (teMsgOperateResult)
        {
            case eMsgOperateResult.OR_Succeed:
                mIsEnter = true;
                LocalDataManager.f_SetLocalData<string>(LocalDataType.String_UserName, strName);
                LocalDataManager.f_SetLocalData<string>(LocalDataType.String_Password, strPwd);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
                break;
            case eMsgOperateResult.OR_Error_NoAccount:
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Tài khoản không tồn tại!");
                break;
            case eMsgOperateResult.OR_Error_Password:
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Mật khẩu sai!");
                break;
            case eMsgOperateResult.eOR_CreateAndLogin://登录并创角//打开创角界面已经写在Socket_SelCharacter里面
                LocalDataManager.f_SetLocalData<string>(LocalDataType.String_UserName, strName);
                LocalDataManager.f_SetLocalData<string>(LocalDataType.String_Password, strPwd);
                mIsEnter = true;
                break;
            case eMsgOperateResult.OR_Error_SeverMaintain:
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Máy chủ đang được bảo trì!");
                break;
            case eMsgOperateResult.OR_Error_VersionNotMatch:
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Phiên bản không khớp, hãy tải xuống phiên bản mới nhất!");
                break;
            case eMsgOperateResult.eOR_IP_Forbidden:
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Bạn đã bị cấm!");
                break;
            case eMsgOperateResult.eOR_Account_Forbidden:
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Bạn đã bị cấm!");
                break;
            default://其他原因
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Đăng nhập không thành công:" + CommonTools.f_GetTransLanguage((int)Obj));
                break;
        }
    }
    /// <summary>
    /// 点击注册游戏按钮事件
    /// </summary>
    public void OnBtnRegisterClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Tạm thời không thể đăng ký được!");
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.RegisterPage, UIMessageDef.UI_OPEN);
    }
}
