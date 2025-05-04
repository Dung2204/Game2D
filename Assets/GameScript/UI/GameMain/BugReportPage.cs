using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 问题反馈页面
/// </summary>
public class BugReportPage : UIFramwork {
    /// <summary>
    /// 页面开启
    /// </summary>
    /// <param name="e"></param>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        f_GetObject("InputMessage").GetComponent<UIInput>().value = "";
    }
    /// <summary>
    /// 初始化游戏
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BlackBG", OnBlackBGClick);
        f_RegClickEvent("BtnConfirm", OnBtnConfirmClick);
    }
    #region 按钮事件
    /// <summary>
    /// 点击黑色背景关闭页面
    /// </summary>
    private void OnBlackBGClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.BugReportPage, UIMessageDef.UI_CLOSE);
    }
    /// <summary>
    /// 点击确定
    /// </summary>
    private void OnBtnConfirmClick(GameObject go, object obj1, object obj2)
    {
        string message = f_GetObject("InputMessage").GetComponent<UIInput>().value;
        if (message == "")
        {
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Nội dung không được để trống!");
            return;
        }
        if (!Data_Pool.m_BlockWordPool.f_CheckValidity(ref message))
        {
UITool.Ui_Trip("Chứa ký tự không hợp lệ");
            f_GetObject("InputMessage").GetComponent<UIInput>().value = message;
            return;
        }
        Data_Pool.m_UserData.f_SendReport(message);
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Đã gửi phản hồi");
        ccUIManage.GetInstance().f_SendMsg(UINameConst.BugReportPage, UIMessageDef.UI_CLOSE);
    }
    #endregion
}
