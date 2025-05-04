using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 显示在中间的提示，带背景的
/// </summary>
public class LabelCenterTipPage : UIFramwork {
    string tipStr = "";
    bool isShowing = false;//是否正在展示文字
    int eventId;
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (isShowing)
            return;
        isShowing = true;
        tipStr = (string)e;
        ShowAnim();
        eventId = ccTimeEvent.GetInstance().f_RegEvent(0.8f, false, null, DestorySelft);
    }
    /// <summary>
    /// 展现动画
    /// </summary>
    private void ShowAnim()
    {
        f_GetObject("HintText").GetComponent<UILabel>().text = tipStr;
        TweenScale tweenScale = f_GetObject("Root").GetComponent<TweenScale>();
        tweenScale.ResetToBeginning();
        tweenScale.PlayForward();
    }
    /// <summary>
    /// 自动销毁
    /// </summary>
    private void DestorySelft(object data)
    {
        isShowing = false;
        ccTimeEvent.GetInstance().f_UnRegEvent(eventId);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelCenterTipPage, UIMessageDef.UI_CLOSE);
    }
}
