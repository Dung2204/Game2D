using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class LegionChapterAwardPage : UIFramwork
{
    private GameObject _chapterAwardItemParent;
    private GameObject _chapterAwardItem;
    private List<NBaseSCDT> _chapterAwardList;
    private UIWrapComponent _chapterAwardWrapComponent;
    private UIWrapComponent mChapterAwardWrapComponent
    {
        get
        {
            if (_chapterAwardWrapComponent == null)
            {
                _chapterAwardList = glo_Main.GetInstance().m_SC_Pool.m_LegionChapterSC.f_GetAll(); 
                _chapterAwardWrapComponent = new UIWrapComponent(230, 1, 800, 6, _chapterAwardItemParent, _chapterAwardItem, _chapterAwardList, f_ChapterAwardItemUpdateByInfo, null);
            }
            f_SortList();
            return _chapterAwardWrapComponent;
        }
    }

    private UILabel mPassChapterLabel;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        _chapterAwardItemParent = f_GetObject("ChapterAwardParent");
        _chapterAwardItem = f_GetObject("ChapterAwardItem");
        mPassChapterLabel = f_GetObject("PassChapterLabel").GetComponent<UILabel>();
        f_RegClickEvent("CloseBtn", f_CloseBtn);
        f_RegClickEvent("CancelBtn", f_CloseBtn);
        f_RegClickEvent("MaskClose", f_CloseBtn);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        mChapterAwardWrapComponent.f_ResetView();
        LegionDungeonPoolDT poolDt = (LegionDungeonPoolDT)LegionMain.GetInstance().m_LegionDungeonPool.f_GetForId(LegionMain.GetInstance().m_LegionDungeonPool.m_iDungeonPassChap);
mPassChapterLabel.text = string.Format("[f5bf3d]Progress：[-]{0}", poolDt == null ? "not yet" : string.Format("{0} chapter {1}", poolDt.mChapterTemplate .iId, poolDt.mChapterTemplate.szName));
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
    }

    private void f_CloseBtn(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionChapterAwardPage, UIMessageDef.UI_CLOSE);
    }

    private void f_ChapterAwardItemUpdateByInfo(Transform tf, NBaseSCDT dt)
    {
        LegionChapterAwardItem tItem = tf.GetComponent<LegionChapterAwardItem>();
        tItem.f_UpdateByInfo(dt);
        f_RegClickEvent(tItem.mGetBtn, f_ChapterAwardItemClick, dt);
    }

    private void f_ChapterAwardItemClick(GameObject go, object value1, object value2)
    {
        LegionChapterDT info = (LegionChapterDT)value1;
        byte passChap = LegionMain.GetInstance().m_LegionDungeonPool.m_iDungeonPassChap;
        byte awardChap = LegionMain.GetInstance().m_LegionDungeonPool.m_iAwardChapter;
        if (info.iId <= passChap && info.iId != awardChap+1)
        {
UITool.Ui_Trip("Có phần thưởng chưa nhận");
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_ChapterAward;
        socketCallbackDt.m_ccCallbackFail = f_Callback_ChapterAward;
        LegionMain.GetInstance().m_LegionDungeonPool.f_ChapterAward((byte)info.iId, socketCallbackDt);
    }

    private void f_Callback_ChapterAward(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
UITool.Ui_Trip("Nhận thành công");
            mChapterAwardWrapComponent.f_UpdateView();
        }
        else
        {
MessageBox.ASSERT("An error occurred while receiving the reward,code:" + result);
        }
    }

    /// <summary>
    /// 排序
    /// </summary>
    private void f_SortList()
    {
        byte passChap = LegionMain.GetInstance().m_LegionDungeonPool.m_iDungeonPassChap;
        byte awardChap = LegionMain.GetInstance().m_LegionDungeonPool.m_iAwardChapter;
        _chapterAwardList.Sort(delegate (NBaseSCDT dt1, NBaseSCDT dt2)
        {

            LegionChapterDT item1 = (LegionChapterDT)dt1;
            LegionChapterDT item2 = (LegionChapterDT)dt2;
            if (item1.iId <= awardChap && item2.iId > awardChap)
            {
                return 1;
            }
            else if (item1.iId > awardChap && item2.iId <= awardChap)
            {
                return -1;
            }
            //if (item1.iId <= passChap && item2.iId <= passChap)
            //{
            //    return -1;
            //}
            //else if (item1.iId <= passChap && item2.iId > passChap)
            //{
            //    return 1;
            //}
            if (item1.iId < item2.iId)
            {
                return -1;
            }
            if (item1.iId > item2.iId)
            {
                return 1;
            }
            return 0;
        });
    }

    private void f_ProcessByMsg_ForceClose(object value)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionChapterAwardPage, UIMessageDef.UI_CLOSE);
    }
}
