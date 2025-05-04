using ccU3DEngine;
using UnityEngine;
using System.Collections;
using System;

public class HallInfoSubPanel : SubPanelBase
{
    public GameObject mApplicantListBtn;
    public GameObject mDissolveBtn;

    public GameObject mNoticeBtn;
    public GameObject mManifestoBtn;

    public UILabel mNoticeLabel;
    public UILabel mManifestoLabel;

    public UI2DSprite mLegionIcon;
    public UILabel mLevelLabel;
    public UILabel mChiefLabel;
    public UILabel mMemberLabel;
    public UILabel mInfoLegionName;
    public UILabel mExpLabel;
    public UISlider mExpSlider;

    public override void f_Init(UIFramwork parentUI)
    {
        base.f_Init(parentUI);
        f_RegClickEvent(mApplicantListBtn, f_ApplicantListBtn);
        f_RegClickEvent(mDissolveBtn, f_DissolveBtn);
        f_RegClickEvent(mNoticeBtn, f_NoticeBtn);
        f_RegClickEvent(mManifestoBtn, f_ManifestoBtn);

    }

    public override void f_Open()
    {
        base.f_Open();
        LegionInfoPoolDT selfInfo = LegionMain.GetInstance().m_LegionInfor.f_getUserLegion();
        mNoticeLabel.text = string.IsNullOrEmpty(selfInfo.Notice) ? LegionConst.LEGION_NOTICE_DEFAULT_VALUE : selfInfo.Notice;
        mManifestoLabel.text = string.IsNullOrEmpty(selfInfo.Manifesto) ? LegionConst.LEGION_MANIFESTO_DEFAULT_VALUE : selfInfo.Manifesto;
        bool noticeActive = LegionTool.f_IsEnoughPermission(EM_LegionOperateType.SetNotice, false);
        bool manifestoActive = LegionTool.f_IsEnoughPermission(EM_LegionOperateType.SetManifesto, false);
        mNoticeBtn.SetActive(noticeActive);
        mManifestoBtn.SetActive(manifestoActive);
        f_UpdateInfo(selfInfo);
    }

    public void f_UpdateInfo(LegionInfoPoolDT info)
    {
        int lv = info.f_GetProperty((int)EM_LegionProperty.Lv);
        int memMax = ((LegionLevelDT)glo_Main.GetInstance().m_SC_Pool.m_LegionLevelSC.f_GetSC(lv)).iCountMax;
        int memNum = LegionMain.GetInstance().m_LegionPlayerPool.m_iMemberNum;
        int ExpMax = LegionTool.f_GetLvUpExpValue(info.f_GetProperty((int)EM_LegionProperty.Lv));
        int ExpCur = info.f_GetProperty((int)EM_LegionProperty.Exp);
        mLegionIcon.sprite2D = UITool.f_GetIconSprite(info.f_GetProperty((int)EM_LegionProperty.Icon));
        mLevelLabel.text = string.Format(CommonTools.f_GetTransLanguage(525), lv);
        BasePlayerPoolDT playerInfo = (BasePlayerPoolDT)Data_Pool.m_GeneralPlayerPool.f_GetForId(info.MasterUserId);
        mChiefLabel.text = string.Format(CommonTools.f_GetTransLanguage(526), playerInfo != null ? playerInfo.m_szName : string.Empty);
        mMemberLabel.text = string.Format(CommonTools.f_GetTransLanguage(527), memNum, memMax);
        mInfoLegionName.text = info.LegionName;
        mExpSlider.value = ExpMax == 0 ? 1.0f : ExpCur / (float)ExpMax;
        mExpLabel.text = string.Format("{0}/{1}", ExpCur, ExpMax);
    }

    #region 宣言和公告


    private void f_NoticeBtn(GameObject go, object value1, object value2)
    {
        LabelPopupMenuPageParam param = new LabelPopupMenuPageParam(false, CommonTools.f_GetTransLanguage(528), CommonTools.f_GetTransLanguage(529), CommonTools.f_GetTransLanguage(530), CommonTools.f_GetTransLanguage(531), f_SureNoticeHandle, CommonTools.f_GetTransLanguage(532), f_CancelNoticeHandle);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelPopupMenuPage, UIMessageDef.UI_OPEN, param);
    }

    private void f_CancelNoticeHandle(object value)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelPopupMenuPage, UIMessageDef.UI_CLOSE);
    }

    private void f_SureNoticeHandle(object value)
    {
        string content = (string)value;
        int byteLen = ccMath.f_GetStringBytesLength(content);
        //int strLen = content.Length;
        if (byteLen <= 0)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(533));
            return;
        }
        else if (byteLen > LegionConst.LEGION_NOTICE_BYTE_LIMIT)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(534));
            return;
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelPopupMenuPage, UIMessageDef.UI_CLOSE);
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_LegionNotice;
        socketCallbackDt.m_ccCallbackFail = f_Callback_LegionNotice;
        LegionMain.GetInstance().m_LegionInfor.f_LegionNotice(content, socketCallbackDt);
    }

    private void f_Callback_LegionNotice(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            mNoticeLabel.text = LegionMain.GetInstance().m_LegionInfor.f_getUserLegion().Notice;
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(535));
        }
        else
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(536));
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(537) + result);
        }
    }

    private void f_ManifestoBtn(GameObject go, object value1, object value2)
    {
        LabelPopupMenuPageParam param = new LabelPopupMenuPageParam(false, CommonTools.f_GetTransLanguage(528), CommonTools.f_GetTransLanguage(538), CommonTools.f_GetTransLanguage(539), CommonTools.f_GetTransLanguage(531), f_SureManifestoHandle, CommonTools.f_GetTransLanguage(532), f_CancelManifestoHandle);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelPopupMenuPage, UIMessageDef.UI_OPEN, param);
    }

    private void f_CancelManifestoHandle(object value)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelPopupMenuPage, UIMessageDef.UI_CLOSE);
    }

    private void f_SureManifestoHandle(object value)
    {
        string content = (string)value;
        int byteLen = ccMath.f_GetStringBytesLength(content);
        //int strLen = content.Length;
        if (byteLen <= 0)
        {
            //不能输入为空
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(533));
            return;
        }
        else if (byteLen > LegionConst.LEGION_MANIFESTO_BYTE_LIMIT)
        {
            //内容过长
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(534));
            return;
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelPopupMenuPage, UIMessageDef.UI_CLOSE);
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_LegionManifesto;
        socketCallbackDt.m_ccCallbackFail = f_Callback_LegionManifesto;
        LegionMain.GetInstance().m_LegionInfor.f_LegionManifesto(content, socketCallbackDt);
    }

    private void f_Callback_LegionManifesto(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            mManifestoLabel.text = LegionMain.GetInstance().m_LegionInfor.f_getUserLegion().Manifesto;
            //修改军团宣言成功
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(540));
        }
        else
        {
            //军团宣言过长
            //修改军团宣言失败
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(541));
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(542) + result);
        }
    }

    #endregion

    private void f_ApplicantListBtn(GameObject go, object value1, object value2)
    {
        if (!LegionTool.f_IsEnoughPermission(EM_LegionOperateType.ApplicantList))
        {
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        LegionMain.GetInstance().m_LegionPlayerPool.f_ExecuteAfterApplicantList(f_Callback_ApplicantList);
    }

    private void f_Callback_ApplicantList(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionHallPage, UIMessageDef.UI_CLOSE);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionApplicantPage, UIMessageDef.UI_OPEN);
        }
    }

    private void f_DissolveBtn(GameObject go, object value1, object value2)
    {
        if (!LegionTool.f_IsEnoughPermission(EM_LegionOperateType.DissolveLegion))
        {
            return;
        }
        else if (LegionMain.GetInstance().m_LegionPlayerPool.m_iMemberNum > LegionConst.LEGION_DISSOLVE_MEMBER_LIMIT)
        {
            //军团人数大于{0}人时不可解散军团
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(543), LegionConst.LEGION_DISSOLVE_MEMBER_LIMIT));
            return;
        }
        //确认解散军团?
        string tContent = string.Format(CommonTools.f_GetTransLanguage(544));
        //  系统提示  确定 取消
        PopupMenuParams tParam = new PopupMenuParams(CommonTools.f_GetTransLanguage(528), tContent, CommonTools.f_GetTransLanguage(531), f_SureDissolve, CommonTools.f_GetTransLanguage(532));
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParam);
    }

    private void f_SureDissolve(object value)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_Dissolve;
        socketCallbackDt.m_ccCallbackFail = f_Callback_Dissolve;
        LegionMain.GetInstance().m_LegionPlayerPool.f_LegionDissolve(socketCallbackDt);

    }

    private void f_Callback_Dissolve(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
            //解散成功
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(545));
        else if ((int)result == (int)eMsgOperateResult.eOR_LegionInBattle)
            //军团战期间不允许解散
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(546));
        else
            //军团解散错误 
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(547) + result);

    }
}
