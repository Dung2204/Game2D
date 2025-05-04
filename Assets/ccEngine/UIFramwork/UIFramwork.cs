using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;
using UnityEngine.UI;
using UnityEngine.Events;
/// <summary>
/// UI组件
/// 所有UI的基类
/// </summary>
public class UIFramwork : ccUIBase
{
    /*
禁止使用，如果需要使用重载f_CustomAwake
void Awake()
{

}
*/

    /*
    protected override void  f_CustomAwake()
    {

    }
    */

    /// <summary>
    /// 定义自己需要处理的消息
    /// UI消息定义放在UIMessageDef中
    /// UI初始时自动调用
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
    }
        
    /// <summary>
    /// UI初始化工作，手工调用
    /// </summary>
    protected override void InitGUI()
    {

    }

    protected bool _NeedCloseSound = true;
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        if (_NeedCloseSound)
        {
            glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(Enum.GetName(typeof(AudioEffectType), AudioEffectType.UI_Close).ToString());
        }
    }

    public void f_RegClickEvent(string strObj, ccUIEventListener.VoidDelegateV2 aCallBackFuc, object oSaveData1 = null, object oSaveData2 = null, bool bNeedClickSound = true)
    {
        GameObject Btn = f_GetObject(strObj);
        if (Btn != null)
            f_RegClickEvent(Btn, aCallBackFuc, oSaveData1, oSaveData2, bNeedClickSound);
        else
            MessageBox.DEBUG("按钮GameObject为null："+strObj);
    }

    public void f_RegClickEvent(GameObject obj, ccUIEventListener.VoidDelegateV2 aCallBackFuc, object oSaveData1 = null, object oSaveData2 = null, bool bNeedClickSound = true)
    {
        if (bNeedClickSound)
        {
            ccUIEventListener.VoidDelegateV2 sound = new ccUIEventListener.VoidDelegateV2(delegate (GameObject o, object v1, object v2)
                {
                    glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);

                });
           ccUIEventListener.VoidDelegateV2 aCallBack = ccUIEventListener.VoidDelegateV2.Combine(aCallBackFuc, sound) as ccUIEventListener.VoidDelegateV2;
           base.f_RegClickEvent(obj, aCallBack, oSaveData1, oSaveData2);
        }
        else
        {
            base.f_RegClickEvent(obj, aCallBackFuc, oSaveData1, oSaveData2);
        }
    }
    public delegate void test1();
    void test()
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);

    }
    /// <summary>
    /// UI更新工作，手工调用
    /// </summary>
    protected override void UpdateGUI()
    {
        //throw new NotImplementedException();
    }

    // Use this for initialization
    //void Start()
    //

    //}

    // 不建议使用
    //void Update()
    //{

    //}

    public void f_RegClickEvent(string strName, UnityAction tccCallback)
    {
        GameObject tObj = f_GetObject(strName);
        if (tObj == null)
        {
            MessageBox.ASSERT("没有找到对应的物件，注册失败 " + strName);
        }
        Button tButton = tObj.GetComponent<Button>();
        tButton.onClick.AddListener(tccCallback);
    }

    #region 红点提示，自动调用
    ///// <summary>
    ///// 初始化红点
    ///// </summary>
    //protected virtual void InitRaddot()
    //{

    //}

    ///// <summary>
    ///// 刷新红点提示UI 
    ///// UI_OPEN和UI_UNHOLD时会自动调用刷新方法
    ///// </summary>
    //protected virtual void UpdateReddotUI()
    //{

    //}

    #endregion

    protected void AddGOReference(string path)
    {
        Transform go = transform.Find(path);
        if (go == null)
        {
            Debug.LogError(path + " not found !!! ");
            return;
        }
        if (m_aObjList.Contains(go.gameObject))
        {
            Debug.LogError("Duplicate GO Reference !!!");
        }
        else
        {
            m_aObjList.Add(go.gameObject);
        }
    }
    protected void AddGOReference(GameObject go)
    {
        if (go == null)
        {
            Debug.LogError(" not found !!! ");
            return;
        }
        if (m_aObjList.Contains(go))
        {
            Debug.LogError("Duplicate GO Reference !!!");
        }
        else
        {
            m_aObjList.Add(go);
        }
    }
}
