using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 副本重置关卡次数提示框
/// </summary>
public class ResetLevelWindowPage : UIFramwork
{
    ResetLevelWindowParam param = null;
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (!(e is ResetLevelWindowParam))
        {
MessageBox.ASSERT("Parameter error");
            return;
        }
        param = (ResetLevelWindowParam)e;
f_GetObject("LabelHint").GetComponent<UILabel>().text = "Làm mới hôm nay: " + param.m_LeftFreshTimes + " lần";
f_GetObject("LabelPrice").GetComponent<UILabel>().text = param.m_WasteSyceeCount + " làm mới？";
    }
    /// <summary>
    /// 注册消息
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnConfirm", OnConfirmClick);
        f_RegClickEvent("BtnCancel", OnCancelClick);
    }
    #region 按钮事件
    /// <summary>
    /// 点击确认按钮事件
    /// </summary>
    private void OnConfirmClick(GameObject go, object obj1, object obj2)
    {
        if (param != null && param.m_OnConfirmCallback != null)
        {
            param.m_OnConfirmCallback(param.obj);
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResetLevelWindowPage, UIMessageDef.UI_CLOSE);
    }
    /// <summary>
    /// 点击取消按钮事件
    /// </summary>
    private void OnCancelClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResetLevelWindowPage, UIMessageDef.UI_CLOSE);
    }
    #endregion
}
/// <summary>
/// 参数
/// </summary>
public class ResetLevelWindowParam
{
    /// <summary>
    /// 确认按钮回调
    /// </summary>
    public ccCallback m_OnConfirmCallback = null;
    /// <summary>
    /// 回调参数
    /// </summary>
    public object obj = null;
    /// <summary>
    /// 消耗元宝数
    /// </summary>
    public int m_WasteSyceeCount;
    /// <summary>
    /// 还可重置次数
    /// </summary>
    public int m_LeftFreshTimes;
}
