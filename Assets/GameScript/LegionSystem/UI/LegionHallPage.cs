using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class LegionHallPage : UIFramwork
{
    private enum HallPageType
    {
        Info = 0,  //军团信息界面
        Member,
        Dynamic,
    }
    
    private HallPageType mCurType;
    private HallInfoSubPanel mInfoSubPanel;
    private HallMemberSubPanel mMemberSubPanel;
    private HallDynamicSubPanel mDynamicSubPanel;
    
    private GameObject mInfoBtnSelect;
    private GameObject mMemberBtnSelect;
    private GameObject mDynamicBtnSelect;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mInfoSubPanel = f_GetObject("HallInfoSubPanel").GetComponent<HallInfoSubPanel>();
        mMemberSubPanel = f_GetObject("HallMemberSubPanel").GetComponent<HallMemberSubPanel>();
        mDynamicSubPanel = f_GetObject("HallDynamicSubPanel").GetComponent<HallDynamicSubPanel>();
        mInfoBtnSelect = f_GetObject("InfoBtnSelect");
        mMemberBtnSelect = f_GetObject("MemberBtnSelect");
        mDynamicBtnSelect = f_GetObject("DynamicBtnSelect");
        f_RegClickEvent("InfoBtn", f_SelectBtn, HallPageType.Info);
        f_RegClickEvent("MemberBtn", f_SelectBtn, HallPageType.Member);
        f_RegClickEvent("DynamicBtn", f_SelectBtn, HallPageType.Dynamic);
        f_RegClickEvent("CloseBtn", f_CloseBtn);
        f_RegClickEvent("MaskClose", f_CloseBtn);
        mInfoSubPanel.f_Init(this);
        mMemberSubPanel.f_Init(this);
        mMemberSubPanel.f_Init(this);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        f_UpdateByType(HallPageType.Info);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
    }

    private void f_ProcessByMsg_ForceClose(object value)
    {
        mInfoSubPanel.f_Close();
        mMemberSubPanel.f_Close();
        mDynamicSubPanel.f_Close();
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionHallPage, UIMessageDef.UI_CLOSE);
    }

    private void f_UpdateByType(HallPageType pageType)
    {
        mCurType = pageType;
        mInfoBtnSelect.SetActive(mCurType == HallPageType.Info);
        mMemberBtnSelect.SetActive(mCurType == HallPageType.Member);
        mDynamicBtnSelect.SetActive(mCurType == HallPageType.Dynamic);
        if (mCurType == HallPageType.Info)
        {
            mInfoSubPanel.f_Open();
            mMemberSubPanel.f_Close();
            mDynamicSubPanel.f_Close();
        }
        else if (mCurType == HallPageType.Member)
        {
            mInfoSubPanel.f_Close();
            mMemberSubPanel.f_Open();
            mDynamicSubPanel.f_Close();
        }
        else if (mCurType == HallPageType.Dynamic)
        {
            mInfoSubPanel.f_Close();
            mMemberSubPanel.f_Close();
            mDynamicSubPanel.f_Open();
        }
    }

    private void f_SelectBtn(GameObject go, object value1, object value2)
    {
        HallPageType tType = (HallPageType)value1;
        if (mCurType == tType)
            return;
        f_UpdateByType(tType);
    }

    private void f_CloseBtn(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionHallPage, UIMessageDef.UI_CLOSE);
    }

    #region 红点

    protected override void InitRaddot()
    {
        base.InitRaddot();
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.LegionApplicantList, mInfoSubPanel.mApplicantListBtn, ReddotCallback_Show_HallBtn, true);

        UpdateReddotUI();
    }

    protected override void UpdateReddotUI()
    {
        base.UpdateReddotUI();
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.LegionApplicantList);
    }

    protected override void On_Destory()
    {
        base.On_Destory();
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.LegionApplicantList, mInfoSubPanel.mApplicantListBtn);
    }

    private void ReddotCallback_Show_HallBtn(object Obj)
    {
        int iNum = (int)Obj;
        UITool.f_UpdateReddot(mInfoSubPanel.mApplicantListBtn, iNum, new Vector3(105, 24, 0), 401);
    }

    #endregion


}
