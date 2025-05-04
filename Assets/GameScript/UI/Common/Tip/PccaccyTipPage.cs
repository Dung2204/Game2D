using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class PccaccyTipPage : UIFramwork
{

    private UILabel m_Content;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        m_Content = f_GetObject("Content").GetComponent<UILabel>();
        f_RegClickEvent("BtnClose", f_BtnClose);
        f_RegClickEvent("BtnPay", f_BtnPay);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (e == null && !(e is string))
MessageBox.ASSERT("PayTipPage parameter error");
        m_Content.text = (string)e;
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    private void f_BtnClose(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PayTipPage, UIMessageDef.UI_CLOSE);
    }

    private void f_BtnPay(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PayTipPage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShowVip, UIMessageDef.UI_OPEN, ShowVip.EM_PageIndex.Recharge);
    }

}
