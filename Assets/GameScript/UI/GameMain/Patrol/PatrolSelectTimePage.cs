using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class PatrolSelectTimePage : UIFramwork
{
    private GameObject m_TimeItemParent;
    private GameObject m_TimeItem;

    private int patrolType = 0; 
    private ccCallback callback_SelectBtn;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        m_TimeItemParent = f_GetObject("TimeItemParent");
        m_TimeItem = f_GetObject("TimeItem");
        f_RegClickEvent("BtnClose", f_BtnClose);
        f_RegClickEvent("BtnClose2", f_BtnClose);
        f_RegClickEvent("CloseMask", f_BtnClose);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        object[] values = (object[])e;
        patrolType = (int)values[0];
        callback_SelectBtn = (ccCallback)values[1]; 
        GridUtil.f_SetGridView<NBaseSCDT>(m_TimeItemParent, m_TimeItem, Data_Pool.m_PatrolPool.f_GetPatrolTypeByType(patrolType), f_TimeItemUpdate);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    private void f_TimeItemUpdate(GameObject go, NBaseSCDT dt)
    {
        PatrolTypeDT tInfo = (PatrolTypeDT)dt;
        PatrolSelectTimeItem tItem = go.GetComponent<PatrolSelectTimeItem>();
        tItem.f_UpdateByInfo(tInfo);
        f_RegClickEvent(tItem.m_ClickItem, f_TimeItemClick, tInfo.iTime);
    }

    private void f_TimeItemClick(GameObject go, object value1, object value2)
    {
        int tTime = (int)value1;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolSelectTimePage, UIMessageDef.UI_CLOSE);
        if (callback_SelectBtn != null)
            callback_SelectBtn(tTime);
    }

    private void f_BtnClose(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolSelectTimePage, UIMessageDef.UI_CLOSE);
    }
    
}
