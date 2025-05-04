using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class LabelPopupMenuPage : UIFramwork
{
    private UIInput mInput;

    private UILabel mPageTitle;
    private UILabel mLabelTitle;
    private UILabel mBtnLabel1;
    private UILabel mBtnLabel2;
    private GameObject mBtn1;
    private GameObject mBtn2;

    private LabelPopupMenuPageParam mParam;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
        mInput = f_GetObject("LabelInput").GetComponent<UIInput>();
        //mPageTitle = f_GetObject("PageTitle").GetComponent<UILabel>();
        mLabelTitle = f_GetObject("LabelTitle").GetComponent<UILabel>();
        mBtnLabel1 = f_GetObject("BtnLabel1").GetComponent<UILabel>();
        mBtnLabel2 = f_GetObject("BtnLabel2").GetComponent<UILabel>();
        mBtn1 = f_GetObject("Btn1");
        mBtn2 = f_GetObject("Btn2");
        f_RegClickEvent(mBtn1, f_BtnHandle1);
        f_RegClickEvent(mBtn2, f_BtnHandle2);
    }

    protected override void InitGUI()
    {
        base.InitGUI();
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (e == null || !(e is LabelPopupMenuPageParam))
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelPopupMenuPage, UIMessageDef.UI_CLOSE);
            return;
        }
        mParam = (LabelPopupMenuPageParam)e;
        mInput.defaultText = mParam.DefaultLabel;
        mInput.value = string.Empty;
        //mPageTitle.text = mParam.PageTitle;
        mLabelTitle.text = mParam.LabelTitel;
        mBtnLabel1.text = mParam.BtnLabel1;
        mBtnLabel2.text = mParam.BtnLabel2;
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    private void f_BtnHandle1(GameObject go,object value1,object value2)
    {
        if (mParam.BtnHandle1 != null)
        {
            string content = mInput.value.Trim();
            if (!Data_Pool.m_BlockWordPool.f_CheckValidity(ref content))
            {
UITool.Ui_Trip("Chứa ký tự không hợp lệ");
                mInput.value = content;
                return;
            }
            mParam.BtnHandle1(content);
        }
        if (mParam.AutoClose)
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelPopupMenuPage, UIMessageDef.UI_CLOSE);
    }

    private void f_BtnHandle2(GameObject go, object value1, object value2)
    {
        if (mParam.BtnHandle2 != null)
        {
            string content = mInput.value.Trim();
            if (!Data_Pool.m_BlockWordPool.f_CheckValidity(ref content))
            {
UITool.Ui_Trip("Chứa ký tự không hợp lệ");
                mInput.value = content;
                return;
            }
            mParam.BtnHandle2(content);
        }
        if (mParam.AutoClose)
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelPopupMenuPage, UIMessageDef.UI_CLOSE);
    }
}

public class LabelPopupMenuPageParam
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="autoClose">点击按钮是否自动关闭自己</param>
    /// <param name="pageTitle">页标题</param>
    /// <param name="labelTitle">内容标题</param>
    /// <param name="defaultLabel">输入框默认显示内容</param>
    /// <param name="btnLabel1">按钮1文字</param>
    /// <param name="btnHandle1">按钮1点击处理</param>
    /// <param name="btnLable2">按钮2文字</param>
    /// <param name="btnHandle2">按钮2点击处理</param>
    public LabelPopupMenuPageParam(bool autoClose,string pageTitle,string labelTitle,string defaultLabel,string btnLabel1,ccCallback btnHandle1,string btnLable2,ccCallback btnHandle2)
    {
        AutoClose = autoClose;
        PageTitle = pageTitle;
        LabelTitel = labelTitle;
        DefaultLabel = defaultLabel;
        BtnLabel1 = btnLabel1;
        BtnHandle1 = btnHandle1;
        BtnLabel2 = btnLable2;
        BtnHandle2 = btnHandle2;
    }
    
    public bool AutoClose
    {
        get;
        private set;
    }

    public string PageTitle
    {
        get;
        private set;
    }

    public string LabelTitel
    {
        get;
        private set;
    }

    public string DefaultLabel
    {
        get;
        private set;
    }

    public string BtnLabel1
    {
        get;
        private set;
    }

    public string BtnLabel2
    {
        get;
        private set;
    }

    public ccCallback BtnHandle1
    {
        get;
        private set;
    }

    public ccCallback BtnHandle2
    {
        get;
        private set;
    }
}
