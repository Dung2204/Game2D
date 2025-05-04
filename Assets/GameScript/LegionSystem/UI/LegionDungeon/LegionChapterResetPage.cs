using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class LegionChapterResetPage : UIFramwork 
{
    enum LegionChapterResetType
    {
        Normal = 0,
        Back = 1,
    }
    private LegionChapterResetType mCurType;

    private GameObject mResetNormalBtn;
    private GameObject mResetBackBtn;
    private GameObject mNormalSelect;
    private GameObject mBackSelect;
    private UILabel mNormalLabel;
    private UILabel mBackLabel;
    private UILabel mPassChapterLabel;

    private byte mCurChapter;
    private byte mPassChap;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mResetNormalBtn = f_GetObject("ResetNormalBtn");
        mResetBackBtn = f_GetObject("ResetBackBtn");
        mNormalSelect = f_GetObject("NormalSelect");
        mBackSelect = f_GetObject("BackSelect");
        mNormalLabel = f_GetObject("NormalLabel").GetComponent<UILabel>();
        mBackLabel = f_GetObject("BackLabel").GetComponent<UILabel>();
        mPassChapterLabel = f_GetObject("PassChapterLabel").GetComponent<UILabel>();
        f_RegClickEvent(mResetNormalBtn, f_SelectBtnClick, LegionChapterResetType.Normal);
        f_RegClickEvent(mResetBackBtn, f_SelectBtnClick, LegionChapterResetType.Back);
        f_RegClickEvent("CloseBtn", f_OnCloseBtnClick);
		f_RegClickEvent("MaskClose", f_OnCloseBtnClick);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        byte flag = LegionMain.GetInstance().m_LegionDungeonPool.m_ResetFlag;
        mCurChapter = LegionMain.GetInstance().m_LegionDungeonPool.m_iCurDungeonChapId;
        mPassChap = LegionMain.GetInstance().m_LegionDungeonPool.m_iDungeonPassChap;
        byte chapterMax = LegionMain.GetInstance().m_LegionDungeonPool.m_iChapterIdMax;
        f_UpdateByInfo((LegionChapterResetType)flag);
		mNormalLabel.text = string.Format("Reset to chapter {0}", mPassChap <= 0 || mPassChap >= chapterMax ? mCurChapter : mPassChap + 1 > LegionMain.GetInstance().m_LegionInfor.mLevelTemplate.iDungeonChapter ? mPassChap : mPassChap + 1);
        mBackLabel.text = string.Format("Reset to chapter {0}", mPassChap <= 0 ? mCurChapter : mPassChap);
        mPassChapterLabel.text = string.Format("Lastest chapter：[f5c14c]{0}[-]", mPassChap <= 0 || mPassChap >= chapterMax ? mCurChapter : mPassChap + 1);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
    }

    private void f_UpdateByInfo(LegionChapterResetType type)
    {
        mCurType = type;
        mNormalSelect.SetActive(mCurType == LegionChapterResetType.Normal);
        mBackSelect.SetActive(mCurType == LegionChapterResetType.Back);
    }

    private void f_SelectBtnClick(GameObject go, object value1, object value2)
    {
        LegionChapterResetType tType = (LegionChapterResetType)value1;
        if (tType == LegionChapterResetType.Back
            && mPassChap <= 0)
        {
UITool.Ui_Trip("Đã ở chương thấp nhất");
            return;
        }
        if (mCurType == tType)
            return;
        f_UpdateByInfo(tType);
    }
    
    private void f_OnCloseBtnClick(GameObject go, object value1, object value2)
    {
        byte flag = LegionMain.GetInstance().m_LegionDungeonPool.m_ResetFlag;
        if(flag == (byte)mCurType)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionChapterResetPage,UIMessageDef.UI_CLOSE);
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_ChapterReset;
        socketCallbackDt.m_ccCallbackFail = f_Callback_ChapterReset;
        LegionMain.GetInstance().m_LegionDungeonPool.f_ResetForServer((byte)mCurType,socketCallbackDt);
    }

    private void f_Callback_ChapterReset(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
UITool.Ui_Trip("Đặt thành công");
        }
        else
        {
UITool.Ui_Trip("Đặt thất bại");
MessageBox.ASSERT("Failed to reinstall Legion subversion，code:" + result);
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionChapterResetPage, UIMessageDef.UI_CLOSE);
    }

    private void f_ProcessByMsg_ForceClose(object value)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionChapterResetPage, UIMessageDef.UI_CLOSE);
    }

}
