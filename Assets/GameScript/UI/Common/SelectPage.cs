using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 选择通用界面
/// </summary>
public class SelectPage : UIFramwork {
    /// <summary>
    /// 传递参数
    /// </summary>
    private SelectPageParam selectPageParam;
    /// <summary>
    /// 页面开启
    /// </summary>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        selectPageParam = (SelectPageParam)e;
        f_GetObject("LabelContent").GetComponent<UILabel>().text = selectPageParam.strContent;
        f_GetObject("BtnLeft").GetComponentInChildren<UILabel>().text = selectPageParam.btnLeftTitle;
        f_GetObject("BtnRight").GetComponentInChildren<UILabel>().text = selectPageParam.btnRightTitle;
    }
    /// <summary>
    /// 注册消息
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnBlack", OnBlackClick);
        f_RegClickEvent("BtnLeft", OnBtnLeftClick);
        f_RegClickEvent("BtnRight", OnBtnRightClick);
    }
    #region 按钮消息
    /// <summary>
    /// 点击黑色背景，关闭页面
    /// </summary>
    private void OnBlackClick(GameObject go,object obj1,object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SelectPage, UIMessageDef.UI_CLOSE);
    }
    /// <summary>
    /// 点击左按钮回调
    /// </summary>
    private void OnBtnLeftClick(GameObject go, object obj1, object obj2)
    {
        if (selectPageParam.OnBtnLeftCallback != null)
        {
            selectPageParam.OnBtnLeftCallback(selectPageParam.LeftCallbakcParam);
        }
    }
    /// <summary>
    /// 点击右按钮回调
    /// </summary>
    private void OnBtnRightClick(GameObject go, object obj1, object obj2)
    {
        if (selectPageParam.onBtnRightCallback != null)
        {
            selectPageParam.onBtnRightCallback(selectPageParam.RightCallbakcParam);
        }
    }
    #endregion
}
/// <summary>
/// 传递参数
/// </summary>
public class SelectPageParam
{
    public string strContent="";//显示内容
    public string btnLeftTitle="";//左按钮标题
    public string btnRightTitle="";//右按钮标题
    public ccCallback OnBtnLeftCallback = null;//左按钮事件回调
    public object LeftCallbakcParam = null;//回调参数
    public ccCallback onBtnRightCallback = null;//有按钮事件回调
    public object RightCallbakcParam = null;//回调参数
}