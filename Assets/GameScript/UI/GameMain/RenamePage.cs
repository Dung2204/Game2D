using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 改名页面
/// </summary>
public class RenamePage : UIFramwork {

    private SocketCallbackDT RenameCallback = new SocketCallbackDT();//改名回调
    /// <summary>
    /// 页面开启
    /// </summary>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        f_GetObject("InputRoleName").GetComponent<UIInput>().value = Data_Pool.m_UserData.m_szRoleName;
    }
    /// <summary>
    /// 初始化
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BlackBG", OnBtnBlackClick);
        f_RegClickEvent("BtnConfirm", OnBtnConfirmClick);
        f_RegClickEvent("BtnDice", OnBtnDiceClick);

        RenameCallback.m_ccCallbackSuc = OnRenameSucCallback;
        RenameCallback.m_ccCallbackFail = OnRenameFailCallback;
    }
    #region 按钮事件
    /// <summary>
    /// 点击黑色背景关闭页面
    /// </summary>
    private void OnBtnBlackClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RenamePage, UIMessageDef.UI_CLOSE);
    }
    /// <summary>
    /// 选中的角色性别
    /// </summary>
    private enum EM_SelectRoleType
    {
        Boy = 0,
        Girl = 1,
    }
    /// <summary>
    /// 点击随机名字
    /// </summary>
    private void OnBtnDiceClick(GameObject go, object obj1, object obj2)
    {
        //获取卡牌主卡id，偶数为男，奇数角色性别为女
        int mainCardId = Data_Pool.m_TeamPool.f_GetFormationPosCardPoolDT(EM_FormationPos.eFormationPos_Main).m_CardDT.iId;
        int sex = (int)EM_SelectRoleType.Boy;
        if (mainCardId % 2 != 0)
            sex = (int)EM_SelectRoleType.Girl;
        Data_Pool.m_UserData.f_RequestNewName(sex, OnGetRandomNameCallback);
    }
    /// <summary>
    /// 请求名字回调
    /// </summary>
    /// <param name="obj">返回随机名字</param>
    private void OnGetRandomNameCallback(object obj)
    {
        string getName = (string)obj;
        f_GetObject("InputRoleName").GetComponent<UIInput>().value = getName;
    }
    /// <summary>
    /// 确认修改名字
    /// </summary>
    private void OnBtnConfirmClick(GameObject go, object obj1, object obj2)
    {
        string name = f_GetObject("InputRoleName").GetComponent<UIInput>().value;
        if (name.Equals(Data_Pool.m_UserData.m_szRoleName))//没有改名字
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.RenamePage, UIMessageDef.UI_CLOSE);
            return;
        }
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee) < 500)//扣500元宝
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(2035));
            return;
        }
        if (name == "")
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(2036));
            return;
        }
        if (!Data_Pool.m_BlockWordPool.f_CheckValidity(ref name))
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(2037));
            f_GetObject("InputRoleName").GetComponent<UIInput>().value = name;
            return;
        }
        if (CommonTools.f_CheckLength(name) < 0)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(2038));
            return;
        }
        int size = ccMath.f_GetStringBytesLength(name);

        if (size > GameParamConst.RoleNameByteMaxNum)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(2039));
            return;
        }
        else if (size < GameParamConst.RoleNameByteMinNum)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(2040));
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_UserData.f_ChangeName(name, RenameCallback);
    }
    #endregion
    /// <summary>
    /// 成功回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnRenameSucCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(2041));
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RenamePage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.UserInfoPage, UIMessageDef.UI_OPEN);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_UPDATE_USERINFOR);//通知更新资料
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_UPDATE_MODELINFOR);//通知更新资料
    }
    private void OnRenameFailCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        switch (teMsgOperateResult)
        {
            case eMsgOperateResult.eOR_DuplicateRoleName:
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(2042));
                break;
            default:
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(2043) + CommonTools.f_GetTransLanguage((int)obj));
                break;
        }
    }
}
