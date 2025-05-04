using System;
using System.Collections.Generic;
using ccU3DEngine;
using UnityEngine;

class GuidanceStar : ccMachineStateBase
{

    private bool IsChangeUpLevelID;
    private int ChangeSaveID;

    private GuidanceManage _GuidanceMnaage;

    private GuidanceDT _GuidanceDt;
    int _iDialogID;
    public GuidanceStar(GuidanceManage _GuidanceManage) : base((int)EM_Guidance.GuidanceRead)
    {
        this._GuidanceMnaage = _GuidanceManage;
        _iDialogID = 0;
        ChangeSaveID = 0;
        IsChangeUpLevelID = false;
    }

    private void Test(object Obj)
    {
PopupMenuParams tParam = new PopupMenuParams("Xử lý", string.Format("Xử lý thành công 1"), "Xác nhận", null, "", TTTT, 1);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParam);

TopPopupMenuParams param = new TopPopupMenuParams("Chú ý", "Tài khoản này đang được đăng nhập ở nơi khác", "Thoát", TTTT,10);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopPopupMenuPage, UIMessageDef.UI_OPEN, param);
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.GameNoticePage, UIMessageDef.UI_OPEN, Data_Pool.m_GameNoticePool.f_GetAll()[0]);
    }

    private void TTTT(object Obj)
    {

    }

    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
        //_GuidanceMnaage.Btn_Esc.SetActive(!StaticValue.mIsNeedShowLevelPage);
        if (Data_Pool.m_GuidancePool.IGuidanceID == 1 && ccUIManage.GetInstance().f_CheckUIIsOpen(UINameConst.DungeonTollgatePageNew))
        {
            List<NBaseSCDT> tList = glo_Main.GetInstance().m_SC_Pool.m_GuidanceTeamSC.f_GetAll();
            GuidanceTeamDT tGuidanceTeamDT;
            for (int i = 0; i < tList.Count; i++)
            {
                tGuidanceTeamDT = tList[i] as GuidanceTeamDT;
                if (tGuidanceTeamDT != null && (tGuidanceTeamDT.iTrigger == (int) EM_GuidanceType.FirstLogin))
                {
                    Data_Pool.m_GuidancePool.IGuidanceID = tGuidanceTeamDT.iGuidanceId;
                    break;
                }
            }
        }

        _GuidanceMnaage.Btn_Esc.SetActive(false);
        //GuidanceDT tGuidanceDT= Data_Pool.m_GuidancePool.m_GuidanceDT;
        //if(tGuidanceDT == null)
        //{
        //    //检测是否可以播放
        //    MessageBox.DEBUG("新手引导结束,按钮设置为null");
        //    f_SetComplete((int)EM_Guidance.GuidanceEnd);
        //    return;
        //}
        //_GuidanceMnaage.Btn_Esc.SetActive(false);
        Data_Pool.m_GuidancePool.f_SetCurClickButton("tetetetettetetet", null);

        if (StaticValue.mIsNeedShowLevelPage && Data_Pool.m_GuidancePool.m_GuidanceType != EM_GuidanceType.UpLevel)
        {

            ChangeSaveID = Data_Pool.m_GuidancePool.IGuidanceID;
            Data_Pool.m_GuidancePool.IGuidanceID = 100;
            IsChangeUpLevelID = true;

MessageBox.DEBUG("Current Beginner Guide Type" + Data_Pool.m_GuidancePool.m_GuidanceType.ToString() + "ID for" + Data_Pool.m_GuidancePool.IGuidanceID);
        }
        else if (Data_Pool.m_GuidancePool.IGuidanceID == 2005)
        {
            if (!Data_Pool.m_ArenaPool.mBreakRankInfo.m_bShowInfo)
            {
                Data_Pool.m_GuidancePool.IGuidanceID++;
            }
        }
        else if (IsChangeUpLevelID)
        {
            Data_Pool.m_GuidancePool.IGuidanceID = ChangeSaveID;
            IsChangeUpLevelID = false;
        }



        Data_Pool.m_GuidancePool.m_GuidanceArr.SetActive(false);


        _GuidanceDt = Data_Pool.m_GuidancePool.m_GuidanceDT;
        if (_GuidanceDt == null)
        {
            //检测是否可以播放
MessageBox.DEBUG("The beginner's tutorial is over, the button has been set to null");
            f_SetComplete((int)EM_Guidance.GuidanceEnd);
            return;
        }
        //播放剧情
        if (_GuidanceDt.iDialog != 0 && _iDialogID != _GuidanceDt.iDialog)
        {
            if (_GuidanceDt.iDelay != 0)
                ccTimeEvent.GetInstance().f_RegEvent(_GuidanceDt.iDelay, false, _GuidanceDt.iDialog, _ChangeDialog);
            else
                _ChangeDialog(_GuidanceDt.iDialog);
            _iDialogID = _GuidanceDt.iDialog;
            return;
        }
        _ChangePlayState(_GuidanceDt);

        //ccTimeEvent.GetInstance().f_RegEvent(5, false, null, Test);
    }

    public bool IsPreviousSave(int id)
    {
        GuidanceDT tGuidanceDT = glo_Main.GetInstance().m_SC_Pool.m_GuidanceSC.f_GetSC(id) as GuidanceDT;
        if (tGuidanceDT == null)
            return false;
        if (tGuidanceDT.iSave == 0)
            return false;
MessageBox.DEBUG("Save Frame in" + tGuidanceDT.iSave);
        return true;
    }
    public void _ChangePlayState(object obj)
    {
        f_SetComplete((int)EM_Guidance.GuidancePlay, obj);
    }
    private void _ChangeDialog(object obj)
    {
        f_SetComplete((int)EM_Guidance.GuidanceDialogRead, obj);
    }
}

