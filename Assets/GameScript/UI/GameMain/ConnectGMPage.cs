using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 联系GM页面（CP联系方式，如电话、QQ、邮箱）
/// </summary>
public class ConnectGMPage : UIFramwork {

    protected override void f_InitMessage()
    {
        base.f_InitMessage(); 
        f_RegClickEvent("BlackBG", OnBtnBlackClick);
    }
    /// <summary>
    /// 页面开启
    /// </summary>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        f_GetObject("LabelContent").GetComponent<UILabel>().text = (string)e;
    }
    #region 按钮事件
    /// <summary>
    /// 点击黑色背景
    /// </summary>
    private void OnBtnBlackClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ConnectGMPage, UIMessageDef.UI_CLOSE);
    }
    #endregion
}
