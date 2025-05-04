using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 获得阵图碎片显示
/// </summary>
public class BattleFormFragmentPage : UIFramwork
{
    BattleFormFragmentParam param;
    /// <summary>
    /// 页面开启
    /// </summary>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        param = (BattleFormFragmentParam)e;
    } 
    /// <summary>
    /// 页面关闭
    /// </summary>
    /// <param name="e"></param>
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }
    /// <summary>
    /// 注册消息事件
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("MaskClose", f_MaskClose);
        ResourceCommonDT resourceCommonDT = new ResourceCommonDT();
        resourceCommonDT.f_UpdateInfo(2, (int)EM_MoneyType.eBattleFormFragment, 1);
        f_GetObject("Icon").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSprite(resourceCommonDT.mIcon);
        f_GetObject("IconBorder").GetComponent<UISprite>().spriteName = UITool.f_GetImporentCase(resourceCommonDT.mImportant);
    }

    private void f_MaskClose(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.BattleFormFragmentPage, UIMessageDef.UI_CLOSE);
        param.mCallback(param.data);
    }
}
public class BattleFormFragmentParam
{
    public ccCallback mCallback;
    public object data;
}
