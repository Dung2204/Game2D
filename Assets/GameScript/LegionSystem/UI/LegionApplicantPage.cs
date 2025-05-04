using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class LegionApplicantPage : UIFramwork
{
    private GameObject mCloseBtn;

    private GameObject mNullTip;
    private UILabel mMemberNum;

    private GameObject mLegionApplicantParent;
    private GameObject mLegionApplicantItem;
    private List<BasePoolDT<long>> _applicantList;
    private UIWrapComponent _legionApplicantWrapComponent;
    private UIWrapComponent mLegionApplicantWrapComponent
    {
        get
        {
            if (_legionApplicantWrapComponent == null)
            {
                _applicantList = LegionMain.GetInstance().m_LegionPlayerPool.f_GetApplicantList();
                _legionApplicantWrapComponent = new UIWrapComponent(210, 1, 800, 6, mLegionApplicantParent, mLegionApplicantItem, _applicantList, f_LegionApplicantItemUpdateByInfo, null);
            }
            return _legionApplicantWrapComponent;
        }
    }

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
        mCloseBtn = f_GetObject("CloseBtn");
        mNullTip = f_GetObject("NullTip");
        mMemberNum = f_GetObject("MemberNum").GetComponent<UILabel>();
        mLegionApplicantParent = f_GetObject("LegionApplicantParent");
        mLegionApplicantItem = f_GetObject("LegionApplicantItem");
        f_RegClickEvent(mCloseBtn, f_CLoseBtn);
        f_RegClickEvent("MaskClose", f_CLoseBtn);
    }

    protected override void InitGUI()
    {
        base.InitGUI();
    }

    private void f_UpdateByMsg_ApplicantData(object value)
    {
        f_UpdateByInfo();
        mLegionApplicantWrapComponent.f_UpdateView(); 
    }

    private void f_ProcessByMsg_ForceClose(object value)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionApplicantPage,UIMessageDef.UI_CLOSE);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_LEGION_APPLICANT_DATA_UPDATE, f_UpdateByMsg_ApplicantData,this);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
        mLegionApplicantWrapComponent.f_ResetView();
        f_UpdateByInfo();
    }

    private void f_UpdateByInfo()
    {
mMemberNum.text = string.Format("[FBBD46]Member：[-]{0}/{1}", LegionMain.GetInstance().m_LegionPlayerPool.m_iMemberNum, LegionMain.GetInstance().m_LegionInfor.mLevelTemplate.iCountMax);
        mNullTip.SetActive(_applicantList.Count == 0);
    }


    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_LEGION_APPLICANT_DATA_UPDATE, f_UpdateByMsg_ApplicantData,this);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
    }

    private void f_CLoseBtn(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionApplicantPage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionHallPage, UIMessageDef.UI_OPEN);
    }

    private void f_LegionApplicantItemUpdateByInfo(Transform tf, BasePoolDT<long> dt)
    {
        LegionApplicantItem item = tf.GetComponent<LegionApplicantItem>();
        item.f_UpdateByInfo(dt);
        f_RegClickEvent(item.mAcceptBtn, f_ApplicantAcceptBtn, dt);
        f_RegClickEvent(item.mDisacceptBtn, f_ApplicantDisacceptBtn, dt);
    }

    private void f_ApplicantAcceptBtn(GameObject go, object value1, object value2)
    {
        if (!LegionTool.f_IsEnoughPermission(EM_LegionOperateType.AcceptApplicant))
        {
            f_CLoseBtn(null,null,null);
            return;
        }
        LegionPlayerPoolDT poolDt = (LegionPlayerPoolDT)value1;
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_LegionAccept;
        socketCallbackDt.m_ccCallbackFail = f_Callback_LegionAccept;
        LegionMain.GetInstance().m_LegionPlayerPool.f_LegionRespond(poolDt.iId,1,socketCallbackDt);
    }

    private void f_ApplicantDisacceptBtn(GameObject go, object value1, object value2)
    {
        if (!LegionTool.f_IsEnoughPermission(EM_LegionOperateType.DisacceptApplicant))
        {
            f_CLoseBtn(null, null, null);
            return;
        }
        LegionPlayerPoolDT poolDt = (LegionPlayerPoolDT)value1;
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_LegionDisaccept;
        socketCallbackDt.m_ccCallbackFail = f_Callback_LegionDisaccept;
        LegionMain.GetInstance().m_LegionPlayerPool.f_LegionRespond(poolDt.iId, 0, socketCallbackDt);
    }

    private void f_Callback_LegionAccept(object result)
    {
		MessageBox.ASSERT("code:" + result);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
UITool.Ui_Trip("Gia nhập thành công");
        }
        else if ((int)result == (int)eMsgOperateResult.eOR_LegionAlreadyIn)
        {
UITool.Ui_Trip("Đã thuộc quân đoàn khác");
        }
        else if ((int)result == (int)eMsgOperateResult.eOR_LegionApplyOvertime)
        {
UITool.Ui_Trip("Đã hết hạn");
        }
		else if ((int)result == 61019 || (int)result == 50001)
        {
UITool.Ui_Trip("Đã đạt giới hạn số lượng thành viên");
        }
        else
        {
MessageBox.ASSERT("Single browsing error，code:" + result);
        }
        //通过消息更新
        //mLegionApplicantWrapComponent.f_UpdateView();
        //f_UpdateByInfo();
    }

    private void f_Callback_LegionDisaccept(object result)
    {
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
UITool.Ui_Trip("Đã từ chối");
        }
        else if ((int)result == (int)eMsgOperateResult.eOR_LegionAlreadyIn)
        {
UITool.Ui_Trip("Đã thuộc quân đoàn khác");
        }
        else if ((int)result == (int)eMsgOperateResult.eOR_LegionApplyOvertime)
        {
UITool.Ui_Trip("Đã hết hạn");
        }
		else if ((int)result == 61019 || (int)result == 50001)
        {
UITool.Ui_Trip("Đã đạt giới hạn số lượng thành viên");
        }
        else
        {
MessageBox.ASSERT("Single rejection error，code:" + result);
        }
        //通过消息更新
        //mLegionApplicantWrapComponent.f_UpdateView();
        //f_UpdateByInfo();
    }

}
