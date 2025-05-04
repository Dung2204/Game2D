using UnityEngine;
using System.Collections;
using System;

public class SubPanelBase : MonoBehaviour, IzSubPanel
{
    protected UIFramwork mParentUI;

    public GameObject mPanel;

    public virtual void f_Init(UIFramwork parentUI)
    {
        mParentUI = parentUI;
        mPanel.SetActive(false);
    }

    public virtual void f_Open()
    {
        if (!mPanel.activeSelf)
        {
            mPanel.SetActive(true);
            f_RegEvent();
        }
    }

    public virtual void f_Close()
    {
        if (mPanel.activeSelf)
        {
            mPanel.SetActive(false);
            f_UnregEvent();
        }
    }

    public virtual void f_RegEvent()
    {

    }

    public virtual void f_UnregEvent()
    {
        
    }

    protected void f_RegClickEvent(GameObject Obj, ccUIEventListener.VoidDelegateV2 aCallBackFuc, object oSaveData1 = null, object oSaveData2 = null)
    {
        mParentUI.f_RegClickEvent(Obj, aCallBackFuc, oSaveData1, oSaveData2);
        
    }

    /// <summary>
    /// 按住
    /// </summary>
    /// <param name="go"></param>
    /// <param name="callbackFuc">回调函数</param>
    /// <param name="value2">要传递的自定义数据</param>
    protected void f_RegPressEvent(GameObject go, ccUIEventListener.VoidDelegateV2 callbackFuc, object value2)
    {
        mParentUI.f_RegPressEvent(go, callbackFuc, value2);
    }
}
