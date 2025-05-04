using UnityEngine;
using System.Collections;
using ccU3DEngine;

public class BuildMenu : ccUIBase
{
    private float _fDefaultSize = 10;
    Vector3 _v3InitPos;

    /// <summary>
    /// 打开建筑菜单的类型 0等待建筑菜单 1管理建筑菜单
    /// </summary>
    private int _iMenuType = 0;
    private int _iStep = 0;

    
#region 按钮消息

    /// <summary>
    /// 定义自己需要处理的消息
    /// UI消息定义放在UIMessageDef中
    /// </summary>
    protected override void f_InitMessage()
    {        
        f_RegClickEvent("BtnMove", UI_BtnMove);
        f_RegClickEvent("BtnLook", UI_BtnLook);
        f_RegClickEvent("BtnDel", UI_BtnDel);
        f_RegClickEvent("BtnBack", UI_BtnBack);
        
        f_RegClickEvent("BtnConfirm2", UI_BtnConfirm2);
        f_RegClickEvent("BtnTurn2", UI_BtnTurn2);
        f_RegClickEvent("BtnBack2", UI_BtnBack2);

        f_RegClickEvent("BtnConfirm4", UI_BtnConfirm4);
        f_RegClickEvent("BtnBack4", UI_BtnBack4);
        

        base.f_InitMessage();
    }

    
    private void UI_BtnBack4(GameObject go, object obj1, object obj2)
    {
        UI_OPEN(1);
    }
    
    #endregion

    /// <summary>
    /// 当前页被打开UI消息
    /// </summary>
    /// <param name="e"></param>
    protected override void UI_OPEN(object e)
    {
        _iMenuType = (int)e;
        ChangeStep(0);
                
        base.UI_OPEN(e);
    }

    private void ChangeStep(int iStep)
    {
        _iStep = iStep;
        InitGUI();
        UpdateGUI();
    }
  

    private void on_Update_Pos_Ok(object Obj)
    {
        Vector3 Pos = (Vector3)Obj;
        //计算摄像机焦距对应的建筑位置的变化

        //transform.position = Pos;
        f_GetObject("BtnConfirm2").SetActive(true);
    }

    private void on_Update_Pos_Ero(object Obj)
    {
        //transform.position = (Vector3)Obj;
        f_GetObject("BtnConfirm2").SetActive(false);
    }
    
    /// <summary>
    /// 初始UI, 手工调用
    /// </summary>
    protected override void InitGUI()
    {
        
    }

    /// <summary>
    /// 刷新显示操作，手工调用
    /// </summary>
    protected override void UpdateGUI()
    {
        //点击已建造好的建筑
        if (_iMenuType == 1)
        { //1.點擊，應該是移動、內容、刪除
            if (_iStep == 0)
            {
                f_GetObject("Step1").SetActive(true);
                f_GetObject("Step2").SetActive(false);
                f_GetObject("Step4").SetActive(false);
            }
            else if (_iStep == 1)
            {//2.按了移動後，就是確定、旋轉           
                f_GetObject("Step1").SetActive(false);
                f_GetObject("Step2").SetActive(true);
                f_GetObject("Step4").SetActive(false);
            }
            else if (_iStep == 2)
            {//3.按內容，顯示店舖界面      
                f_GetObject("Step1").SetActive(false);
                f_GetObject("Step2").SetActive(false);
                f_GetObject("Step4").SetActive(false);
            }
            else if (_iStep == 3)
            {
                //4.按刪除，顯示確定、取消
                f_GetObject("Step1").SetActive(false);
                f_GetObject("Step2").SetActive(false);
                f_GetObject("Step4").SetActive(true);
            }

        }
        //新建建筑时
        else if (_iMenuType == 2)
        {
            if (_iStep == 0)
            {
                //按了移動後，就是確定、旋轉           
                f_GetObject("Step1").SetActive(false);
                f_GetObject("Step2").SetActive(true);
                f_GetObject("Step4").SetActive(false);
            }
        }
        else if (_iMenuType == 3)
        {


        }
        else if (_iMenuType == 4)
        {

        }
        else
        {
MessageBox.ASSERT("Error BuildMenu, type " + _iMenuType);
        }
    }
    
    
    private void on_OpenStep1(object Obj)
    {//UI_BUILDMENU_OPENSTEP1
       
    }

    private void UI_BtnBack(GameObject go, object obj1, object obj2)
    {
        
        UI_CLOSE(null);             
    }

    private void UI_BtnLook(GameObject go, object obj1, object obj2)
    {//未处理
        //ChangeStep(2);
        ccUIHoldPool.GetInstance().f_Hold(this);

    }

    private void UI_BtnDel(GameObject go, object obj1, object obj2)
    {
        ChangeStep(3);
    }

    private void UI_BtnMove(GameObject go, object obj1, object obj2)
    {
        ChangeStep(1);
    }

    /// <summary>
    /// 确认删除建筑
    /// </summary>
    /// <param name="go"></param>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    private void UI_BtnConfirm4(GameObject go, object obj1, object obj2)
    {
        UI_CLOSE(null);
    }
    
    

    private void on_OpenStep2_ReadyBuild(object Obj)
    {//
       
    }

    private void UI_BtnBack2(GameObject go, object obj1, object obj2)
    {
        if (_iMenuType == 1)
        {
            ChangeStep(0);
        }
        else if (_iMenuType == 2)
        {//取消建造             
            UI_CLOSE(null);
            ccUIHoldPool.GetInstance().f_UnHold();
        }
    }

    private void UI_BtnTurn2(GameObject go, object obj1, object obj2)
    {
        
    }

    private void UI_BtnConfirm2(GameObject go, object obj1, object obj2)
    {
      
    }
    


    float orthographicSize;
    Vector3 _v3CameraPos;
    ///// <summary>
    ///// 使用此Update, 可以控制使用普通的Update还是FixedUpdate
    ///// </summary>
    protected override void f_Update()
    {
        

    }

    /// <summary>
    /// 当前面被关闭UI操作
    /// </summary>
    /// <param name="e"></param>
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }



    

}
