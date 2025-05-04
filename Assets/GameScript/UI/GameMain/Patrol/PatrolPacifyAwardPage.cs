using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class PatrolPacifyAwardPage : UIFramwork
{
    private const int ShowModeId = 13071;
    private const string ShowAwardStr = "1;7;5";

    private Transform m_RoleParent;
    private GameObject m_Role;
    private UIGrid m_AwardGrid;
    private GameObject m_AwardItem;

    private ResourceCommonItemComponent _awardShowComponent;
    private ResourceCommonItemComponent m_AwardShowComponent
    {
        get
        {
            if (_awardShowComponent == null)
                _awardShowComponent = new ResourceCommonItemComponent(m_AwardGrid, m_AwardItem);
            return _awardShowComponent;
        }
    }


    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        m_RoleParent = f_GetObject("RoleParent").transform;
        m_AwardGrid = f_GetObject("AwardGrid").GetComponent<UIGrid>();
        m_AwardItem = f_GetObject("ResourceCommonItem");
        f_RegClickEvent("CloseMask", f_CloseMask);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        UITool.f_CreateRoleByModeId(ShowModeId, ref m_Role, m_RoleParent, 23);
        m_AwardShowComponent.f_Show(Data_Pool.m_AwardPool.f_GetAwardByString(ShowAwardStr));
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    private void f_CloseMask(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolPacifyAwardPage, UIMessageDef.UI_CLOSE);
    }
}
