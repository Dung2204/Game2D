using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class PatrolSkillPage : UIFramwork
{
    private UIScrollView m_SkillScrollView;
    private UIGrid m_SkillGrid;
    private GameObject m_SkillItem;
    private UILabel m_TotalTime;

    PatrolSkillNode[] skillNodes;
    private PatrolSkillItem[] skillItems;

    private ccCallback callback_UpdatePatrol;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        m_SkillScrollView = f_GetObject("SkillScrollView").GetComponent<UIScrollView>();
        m_SkillGrid = f_GetObject("SkillGrid").GetComponent<UIGrid>();
        m_SkillItem = f_GetObject("SkillItem");
        m_TotalTime = f_GetObject("TotalTime").GetComponent<UILabel>();
        f_RegClickEvent("BtnClose", f_BtnClose);
        f_RegClickEvent("BtnHelp", f_BtnHelp);
        f_RegClickEvent("CloseMask", f_BtnClose);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        callback_UpdatePatrol = (ccCallback)e;
        skillNodes = Data_Pool.m_PatrolPool.m_LandSkills;
        int tTotalTime = 0;
        if (skillItems == null)
        {
            skillItems = new PatrolSkillItem[skillNodes.Length];
            for (int i = 0; i < skillItems.Length; i++)
            {
                GameObject go = NGUITools.AddChild(m_SkillGrid.gameObject, m_SkillItem);
                go.SetActive(true);
                skillItems[i] = go.GetComponent<PatrolSkillItem>();
            }
        }
        for (int i = 0; i < skillItems.Length; i++)
        {
            skillItems[i].f_UpdateByInfo(skillNodes[i]);
            f_RegClickEvent(skillItems[i].m_BtnLvUp, f_SkillItemBtnLvUp,skillNodes[i]);
            tTotalTime += skillNodes[i].m_iTotalTime;
        }
        m_SkillGrid.repositionNow = true;
        m_SkillGrid.Reposition();
        m_SkillScrollView.ResetPosition();
        m_TotalTime.text = string.Format(CommonTools.f_GetTransLanguage(901), tTotalTime);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        if (callback_UpdatePatrol != null)
            callback_UpdatePatrol(eMsgOperateResult.OR_Succeed);
    }
    
    private void f_BtnClose(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolSkillPage, UIMessageDef.UI_CLOSE);
    }

    private void f_SkillItemBtnLvUp(GameObject go, object value1, object value2)
    {
        PatrolSkillNode tNode = (PatrolSkillNode)value1;
        int timeLimit = tNode.m_LvUpTemplate.iNeedTime;
        int costSycee = tNode.m_LvUpTemplate.iCostSycee;
        int haveSycee = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee);
        if (tNode.m_bIsLock)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(902));
            return;
        }
        else if (tNode.m_iLv >= tNode.m_iLvMax)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(903));
            return;
        }
        else if (tNode.m_iTotalTime < tNode.m_LvUpTemplate.iNeedTime)
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(904), tNode.m_LvUpTemplate.iNeedTime));
            return;
        }
        else if (haveSycee < costSycee)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(905));
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_PatrolUpgrade;
        socketCallbackDt.m_ccCallbackFail = f_Callback_PatrolUpgrade;
        Data_Pool.m_PatrolPool.f_PatrolUpgrade(tNode.m_iLandId, socketCallbackDt);
    }

    private void f_Callback_PatrolUpgrade(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(906));
            for (int i = 0; i < skillItems.Length; i++)
            {
                skillItems[i].f_UpdateByInfo(skillNodes[i]);
            }
        }
        else
        {
            UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(907) + result);
        }
    }

    private void f_BtnHelp(GameObject go, object value1, object value2)
    {
        string helpMsg = CommonTools.f_GetTransLanguage(908);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonHelpPage, UIMessageDef.UI_OPEN, helpMsg);
    }

}
