using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;

public class BuildPage : UIFramwork
{

    private int m_iPageIndex = 1;    
    private int _iStep = 0;


    // Use this for initialization
    void Start()
    {

        //InitGUI();
    }


#region 按钮消息

    /// <summary>
    /// 定义自己需要处理的消息
    /// UI消息定义放在UIMessageDef中
    /// </summary>
    protected override void f_InitMessage()
    {
        //glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_GameMainUpdatePromptMessage, OnUpdatePromptInfor, this);

        //f_RegClickEvent("BtnPermit", UI_BtnSetUp);
        f_RegClickEvent("ExitBtn", UI_ExitBtn);
        f_RegClickEvent("ShopLogoExitBtn", UI_ExitBtn);

        f_RegClickEvent("Menu0", UI_Menu1);
        f_RegClickEvent("Menu1", UI_Menu2);
        f_RegClickEvent("Menu2", UI_Menu3);
        f_RegClickEvent("Menu3", UI_Menu4);

        base.f_InitMessage();
    }

    private void UI_Menu1(GameObject go, object obj1, object obj2)
    {
        m_iPageIndex = 1;
        ChangePage();
    }

    private void UI_Menu2(GameObject go, object obj1, object obj2)
    {
        m_iPageIndex = 2;
        ChangePage();
    }

    private void UI_Menu3(GameObject go, object obj1, object obj2)
    {
        m_iPageIndex = 3;
        ChangePage();
    }

    private void UI_Menu4(GameObject go, object obj1, object obj2)
    {
        m_iPageIndex = 4;
        ChangePage();
    }


    private bool _bRightMenuPop = false;
    private void UI_ExitBtn(GameObject go, object obj1, object obj2)
    {
        if (_iStep == 0)
        {
            //通知HoldPool弹出当前页
            ccUIHoldPool.GetInstance().f_UnHold();
            //关闭自身
            UI_CLOSE(null);
        }
        else if (_iStep == 1)
        {

        }



    }

    private void ChangePage()
    {
        InitGUI();
    }

#endregion

    /// <summary>
    /// 当前页被打开UI消息
    /// </summary>
    /// <param name="e"></param>
    protected override void UI_OPEN(object e)
    {
        UI_Menu1(null, null, null);

        base.UI_OPEN(e);
    }


    /// <summary>
    /// 初始UI, 手工调用
    /// </summary>
    protected override void InitGUI()
    {
        if (m_iPageIndex == 1)
        {//食店
            Page1();
        }
        else if (m_iPageIndex == 2)
        {//特殊
            Page2();
        }
        else if (m_iPageIndex == 3)
        {//环境
            Page3();
        }
        else if (m_iPageIndex == 4)
        {//投资
            Page4();
        }

        //UpdateGUI();
    }
    
    private void Page1()
    {
        GameObject Page1 = f_GetObject("Page0");
        GameObject Page2 = f_GetObject("Page1");
        GameObject Page3 = f_GetObject("Page2");
        GameObject Page4 = f_GetObject("Page3");
        Page1.SetActive(true);
        Page2.SetActive(false);
        Page3.SetActive(false);
        Page4.SetActive(false);

        CreateShopLogo();
    }

    private void CreateShopLogo()
    {
        //创建商店列表
        GameObject ShopLogoScrollView = f_GetObject("ShopLogoScrollView");
        
    }
    


    private void Page2()
    {
		GameObject Page1 = f_GetObject("Page0");
		GameObject Page2 = f_GetObject("Page1");
		GameObject Page3 = f_GetObject("Page2");
		GameObject Page4 = f_GetObject("Page3");
        Page1.SetActive(false);
        Page2.SetActive(true);
        Page3.SetActive(false);
        Page4.SetActive(false);

        CreateShopLogo();
    }

    private void Page3()
    {
		GameObject Page1 = f_GetObject("Page0");
		GameObject Page2 = f_GetObject("Page1");
		GameObject Page3 = f_GetObject("Page2");
		GameObject Page4 = f_GetObject("Page3");
        Page1.SetActive(false);
        Page2.SetActive(false);
        Page3.SetActive(true);
        Page4.SetActive(false);

        CreateShopLogo();
    }

    private void Page4()
    {
        GameObject Page1 = f_GetObject("Page0");
        GameObject Page2 = f_GetObject("Page1");
        GameObject Page3 = f_GetObject("Page2");
        GameObject Page4 = f_GetObject("Page3");
        Page1.SetActive(false);
        Page2.SetActive(false);
        Page3.SetActive(false);
        Page4.SetActive(true);

        CreateShopLogo();
    }





    /// <summary>
    /// 刷新显示操作，手工调用
    /// </summary>
    protected override void UpdateGUI()
    {

    }

    ///// <summary>
    ///// 使用此Update, 可以控制使用普通的Update还是FixedUpdate
    ///// </summary>
    //protected override void f_Update()
    //{


    //}

    /// <summary>
    /// 当前面被关闭UI操作
    /// </summary>
    /// <param name="e"></param>
    protected override void UI_CLOSE(object e)
    {
        
        base.UI_CLOSE(e);
    }


    


}
