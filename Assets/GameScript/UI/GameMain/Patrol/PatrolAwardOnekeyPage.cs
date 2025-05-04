using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class PatrolAwardOnekeyPage : UIFramwork
{
    private GameObject m_AwardItem;

    private GameObject m_OnlyRoot;
    private UIScrollView m_OnlyNormalAwardScrollView;
    private UIGrid m_OnlyNormalAwardGrid;
    private ResourceCommonItemComponent _onlyNormalAwardShow;
    private ResourceCommonItemComponent m_OnlyNormalAwardShow
    {
        get
        {
            if (_onlyNormalAwardShow == null)
                _onlyNormalAwardShow = new ResourceCommonItemComponent(m_OnlyNormalAwardGrid, m_AwardItem);
            return _onlyNormalAwardShow;
        }
    }

    private GameObject m_NormalSkillRoot; 
    private UIScrollView m_NormalAwardScrollView;
    private UIGrid m_NormalAwardGrid;
    private ResourceCommonItemComponent _normalAwardShow;
    private ResourceCommonItemComponent m_NormalAwardShow
    {
        get
        {
            if (_normalAwardShow == null)
                _normalAwardShow = new ResourceCommonItemComponent(m_NormalAwardGrid, m_AwardItem);
            return _normalAwardShow;
        }
    }

    private UIScrollView m_SkillAwardScrollView;
    private UIGrid m_SkillAwardGrid;
    private ResourceCommonItemComponent _skillAwardShow;
    private ResourceCommonItemComponent m_SkillAwardShow
    {
        get
        {
            if (_skillAwardShow == null)
                _skillAwardShow = new ResourceCommonItemComponent(m_SkillAwardGrid, m_AwardItem);
            return _skillAwardShow;
        }
    }

    //可领奖列表
    List<PatrolLandNode> getAwardList;
    
    //总奖励
    List<AwardPoolDT> totalAwardList = new List<AwardPoolDT>();
    //事件奖励（不包含 技能翻倍的)
    List<AwardPoolDT> normalAwardlList = new List<AwardPoolDT>();
    //技能翻倍奖励
    List<AwardPoolDT> skillAwardList = new List<AwardPoolDT>();

    private ccCallback m_Callback_NeedUpdate;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        m_AwardItem = f_GetObject("PatrolLandItem");

        m_OnlyRoot = f_GetObject("OnlyRoot");
        m_OnlyNormalAwardScrollView = f_GetObject("OnlyNormalAwardScrollView").GetComponent<UIScrollView>();
        m_OnlyNormalAwardGrid = f_GetObject("OnlyNormalAwardGrid").GetComponent<UIGrid>();
        
        m_NormalSkillRoot = f_GetObject("NormalSkillRoot");
        m_NormalAwardScrollView = f_GetObject("NormalAwardScrollView").GetComponent<UIScrollView>();
        m_NormalAwardGrid = f_GetObject("NormalAwardGrid").GetComponent<UIGrid>();
        m_SkillAwardScrollView = f_GetObject("SkillAwardScrollView").GetComponent<UIScrollView>();
        m_SkillAwardGrid = f_GetObject("SkillAwardGrid").GetComponent<UIGrid>();
        
        f_RegClickEvent("BtnClose", f_BtnClose); 
        f_RegClickEvent("CloseMask", f_BtnClose);
        f_RegClickEvent("BtnGetAward", f_BtnGetAward);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        object[] values = (object[])e;
        totalAwardList.Clear();
        normalAwardlList.Clear();
        skillAwardList.Clear();
        getAwardList = (List<PatrolLandNode>)values[0];
        m_Callback_NeedUpdate = (ccCallback)values[1];
        for (int i = 0; i < getAwardList.Count; i++)
        {
            List<PatrolEventNode> tEventList = getAwardList[i].m_EventList;
            for (int j = 0; j < tEventList.Count; j++)
            {
                if (tEventList[j].m_AwardDt == null)
                    continue;
                ResourceCommonDT tAwardDt = tEventList[j].m_AwardDt;
                f_AddAwardList(totalAwardList, tAwardDt.mResourceType, tAwardDt.mResourceId, tAwardDt.mResourceNum);
                if (tEventList[j].m_bIsSkillDouble)
                {
                    int tSkillAwardNum = tAwardDt.mResourceNum / tEventList[j].m_iAwardMultiple;
                    int tNormalAwardNum = tAwardDt.mResourceNum - tSkillAwardNum;
                    f_AddAwardList(skillAwardList, tAwardDt.mResourceType, tAwardDt.mResourceId, tSkillAwardNum);
                    f_AddAwardList(normalAwardlList, tAwardDt.mResourceType, tAwardDt.mResourceId, tNormalAwardNum);
                }
                else
                {
                    f_AddAwardList(normalAwardlList, tAwardDt.mResourceType, tAwardDt.mResourceId, tAwardDt.mResourceNum);
                }
            }
        }
        m_OnlyRoot.SetActive(skillAwardList.Count <= 0);
        m_NormalSkillRoot.SetActive(skillAwardList.Count > 0);
        if (skillAwardList.Count <= 0)
        {
            m_OnlyNormalAwardShow.f_Show(normalAwardlList);
            m_OnlyNormalAwardScrollView.ResetPosition();
        }
        else
        {
            m_NormalAwardShow.f_Show(normalAwardlList);
            m_NormalAwardScrollView.ResetPosition();
            m_SkillAwardShow.f_Show(skillAwardList);
            m_SkillAwardScrollView.ResetPosition();
        }
        
         
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    private void f_AddAwardList(List<AwardPoolDT> addList, int awardType, int awardId, int awardNum)
    {
        for (int i = 0; i < addList.Count; i++)
        {
            if (addList[i].mTemplate.mResourceType == awardType && addList[i].mTemplate.mResourceId == awardId)
            {
                addList[i].mTemplate.f_AddNum(awardNum);
                return;
            }
        }
        AwardPoolDT tAwardDt = new AwardPoolDT();
        tAwardDt.f_UpdateByInfo((byte)awardType, awardId, awardNum);
        addList.Add(tAwardDt);
    }

    private void f_BtnClose(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolAwardOnekeyPage, UIMessageDef.UI_CLOSE);
    }

    private int awardIdx = 0;
    private void f_BtnGetAward(GameObject go, object value1, object value2)
    {
        awardIdx = 0;
        if (getAwardList.Count > 0)
        {
            UITool.f_OpenOrCloseWaitTip(true);
            f_SendGetAwardProtocol(getAwardList[awardIdx]);
        }
        else
        {
UITool.Ui_Trip("Không có phần thưởng để nhận");
            ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolAwardOnekeyPage, UIMessageDef.UI_CLOSE);
        } 
    }

    private void f_SendGetAwardProtocol(PatrolLandNode node)
    {
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_PatrolAward;
        socketCallbackDt.m_ccCallbackFail = f_Callback_PatrolAward;
        Data_Pool.m_PatrolPool.f_PatrolAward(node.m_iTemplateId, socketCallbackDt);
    }
    private void f_DelaySendGetAwardProtocol(object value)
    {
        PatrolLandNode tNode = (PatrolLandNode)value;
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_PatrolAward;
        socketCallbackDt.m_ccCallbackFail = f_Callback_PatrolAward;
        Data_Pool.m_PatrolPool.f_PatrolAward(tNode.m_iTemplateId, socketCallbackDt);
    }

    private void f_Callback_PatrolAward(object result)
    {
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            awardIdx++;
            if (awardIdx < getAwardList.Count)
            {
                ccTimeEvent.GetInstance().f_RegEvent(0.01f,false, getAwardList[awardIdx],f_DelaySendGetAwardProtocol);
            }
            else
            {
                UITool.f_OpenOrCloseWaitTip(false);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolAwardOnekeyPage, UIMessageDef.UI_CLOSE);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN, new object[] { totalAwardList });
                if (m_Callback_NeedUpdate != null)
                    m_Callback_NeedUpdate(eMsgOperateResult.OR_Succeed);
            }
        }
        else
        {
            UITool.f_OpenOrCloseWaitTip(false);
UITool.UI_ShowFailContent("Error receiving reward successfully,code:" + result);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolAwardOnekeyPage, UIMessageDef.UI_CLOSE);
            if (m_Callback_NeedUpdate != null)
                m_Callback_NeedUpdate(eMsgOperateResult.OR_Succeed);
        }
    }
}
