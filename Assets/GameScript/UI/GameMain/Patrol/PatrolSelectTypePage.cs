using ccU3DEngine;
using UnityEngine;
using System;

public class PatrolSelectTypePage : UIFramwork
{
    private PatrolSelectTypeItem[] m_SelectItems;
    
    Array typeArr;

    private ccCallback m_SureCallback;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        typeArr = Enum.GetValues(typeof(EM_PatrolType));
        m_SelectItems = new PatrolSelectTypeItem[typeArr.Length];
        for (int i = 0; i < m_SelectItems.Length; i++)
        {
            m_SelectItems[i] = f_GetObject("SelectItem" + i).GetComponent<PatrolSelectTypeItem>();
            f_RegClickEvent(m_SelectItems[i].m_Bg, f_ItemClick, typeArr.GetValue(i));
        }
        f_RegClickEvent("BtnClose", f_BtnClose);
        f_RegClickEvent("BtnClose2", f_BtnClose);
        f_RegClickEvent("CloseMask", f_BtnClose);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        m_SureCallback = (ccCallback)e;
        for (int i = 0; i < m_SelectItems.Length; i++)
        {
            int idx = (int)typeArr.GetValue(i);
            m_SelectItems[i].f_UpdateByInfo((PatrolTypeDT)Data_Pool.m_PatrolPool.f_GetPatrolTypeByType(idx)[0]);
        }
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    private void f_ItemClick(GameObject go, object value1, object value2)
    {
        int tmpType = (int)value1;
        int vipLv = UITool.f_GetNowVipLv();
        int needVipLv = ((PatrolTypeDT)Data_Pool.m_PatrolPool.f_GetPatrolTypeByType(tmpType)[0]).iNeedVip;
        if (vipLv < needVipLv)
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(936), needVipLv));
            return;
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolSelectTypePage, UIMessageDef.UI_CLOSE);
        if (m_SureCallback != null)
            m_SureCallback(tmpType);
    }

    private void f_BtnClose(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolSelectTypePage, UIMessageDef.UI_CLOSE);
    }
}
