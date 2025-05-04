using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 注册界面
/// </summary>
public class RegisterPage : UIFramwork {
    public UIInput InputUserName;//用户名输入框
    public UIInput InputPsw;//密码输入框
    public UIInput InputPswConfirm;//确认密码输入框
    public UILabel LabelHint;//提示标签
    public GameObject BtnBlack;//界面黑色背景
    public GameObject BtnRegister;//注册按钮
    /// <summary>
    /// 页面开启
    /// </summary>
    /// <param name="e"></param>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        ClearHintData(null);
        InputUserName.value = "";
        InputPsw.value = "";
        InputPswConfirm.value = "";
    }
    /// <summary>
    /// 注册UI事件
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent(BtnBlack, OnBlackClick);
        f_RegClickEvent(BtnRegister, OnBtnRegisterClick);
    }
    /// <summary>
    /// 点击背景黑色区域
    /// </summary>
    public void OnBlackClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RegisterPage, UIMessageDef.UI_CLOSE);
    }
    /// <summary>
    /// 点击注册界面
    /// </summary>
    public void OnBtnRegisterClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        if (InputUserName.value == "")
        {
LabelHint.text = "Account cannot be empty！";
            ccTimeEvent.GetInstance().f_RegEvent(0.8f, false, null, ClearHintData);
            return;
        }
        if (InputPsw.value == "" || InputPswConfirm.value == "")
        {
LabelHint.text = "Password cannot be blank！";
            ccTimeEvent.GetInstance().f_RegEvent(0.8f, false, null, ClearHintData);
            return;
        }
        if (InputPsw.value != InputPswConfirm.value)
        {
LabelHint.text = "Confirmed wrong password！";
            ccTimeEvent.GetInstance().f_RegEvent(0.8f, false, null, ClearHintData);
            return;
        }
        GameSocket.GetInstance().f_CreateAccount(InputUserName.value, InputPsw.value,OnRegisterCallback);
    }
    /// <summary>
    /// 注册事件返回
    /// </summary>
    private void OnRegisterCallback(object dataRe)
    {
        if (dataRe != null)
        {
            eMsgOperateResult result = (eMsgOperateResult)dataRe;
            if (result == eMsgOperateResult.OR_Succeed)
            {
                LocalDataManager.f_SetLocalData<string>(LocalDataType.String_UserName, InputUserName.value);
                LocalDataManager.f_SetLocalData<string>(LocalDataType.String_Password, InputPsw.value);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.SwichAccountPage, UIMessageDef.UI_CLOSE);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.RegisterPage, UIMessageDef.UI_CLOSE);
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Đăng ký thành công");
                GameSocket.GetInstance().f_SetSocketStatic(EM_SocketStatic.OnLine);
            }
            else if (result == eMsgOperateResult.OR_Error_AccountRepetition)
            {
Debug.Log("This user name is already registered");
LabelHint.text = "Tên người dùng này đã được đăng ký.";
                ccTimeEvent.GetInstance().f_RegEvent(0.8f, false, null, ClearHintData);
            }
            else
            {
Debug.Log("Registration failed ， details：" + result);
            }
        }
    }
    /// <summary>
    /// 清空提示框内容
    /// </summary>
    /// <param name="data"></param>
    public void ClearHintData(object data)
    {
        LabelHint.text = "";
    }
}
