using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class PccaccyMaskPage : UIFramwork
{
    private UILabel m_ShowTip;
    
    private float m_ShowTime;
    private float m_CurTime;
    private bool m_IsClose;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        m_ShowTip = f_GetObject("ShowTip").GetComponent<UILabel>();
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (e == null || !(e is PccaccyMaskParams))
            MessageBox.ASSERT("PayMaskPage param error");
        PccaccyMaskParams tParam = (PccaccyMaskParams)e; 
        m_ShowTip.text = tParam.m_ShowTip;
        m_ShowTime = tParam.m_ShowTime;
        m_CurTime = 0;
        m_IsClose = false;
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    protected override void f_Update()
    {
        base.f_Update();
        if (m_ShowTime == 0)
            return;
        else if (m_IsClose)
            return;
        m_CurTime += Time.unscaledDeltaTime;
        if (m_CurTime > m_ShowTime)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.PayMaskPage, UIMessageDef.UI_CLOSE);
            m_IsClose = true;
        }
    }
}

public class PccaccyMaskParams
{
    private string showTip;
    private float showTime;

    public PccaccyMaskParams(string showTip,float showTime)
    {
        this.showTip = showTip;
        this.showTime = showTime;
    }

    public string m_ShowTip
    {
        get
        {
            return showTip;
        }
    }

    public float m_ShowTime
    {
        get
        {
            return showTime;
        }
    }

}
