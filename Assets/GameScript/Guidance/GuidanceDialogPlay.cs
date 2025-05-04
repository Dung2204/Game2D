using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;
using UnityEngine;

class GuidanceDialogPlay : ccMachineStateBase
{
    GuidanceDialogDT tDialog;
    List<DungeonDialogDT> tList;
    DialogPageParams tDialogPageParms;
    private int DTid;   //id
    private int Group;  //组
    public GuidanceDialogPlay() : base((int)EM_Guidance.GuidanceDialogPlay)
    {
    }

    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
        if (Obj != null)
        {
            tDialog = (GuidanceDialogDT)Obj;
            DTid = tDialog.iId;
            Group = tDialog.iGroup;
            tList = new List<DungeonDialogDT>();
        }
        for (int i = DTid; i < glo_Main.GetInstance().m_SC_Pool.m_GuidanceDialogSC.f_GetAll().Count+1; i++)
        {
            GuidanceDialogDT tGuidanceDt = glo_Main.GetInstance().m_SC_Pool.m_GuidanceDialogSC.f_GetSC(i) as GuidanceDialogDT;
            if (tGuidanceDt == null || Group != tGuidanceDt.iGroup)
                break;
            DungeonDialogDT tDiaogDt = new DungeonDialogDT();
            tDiaogDt.iAnchor = tGuidanceDt.iAnchor;
            tDiaogDt.iCondition = 1;
            tDiaogDt.iId = tGuidanceDt.iId;
            tDiaogDt.iModeId = tGuidanceDt.iModeId;
            tDiaogDt.iTollgateId = 0;
            tDiaogDt._szDialog = tGuidanceDt._szDialog;
            tDiaogDt._szRoleName = tGuidanceDt.szRoleName;
            tDiaogDt.szMusic = tGuidanceDt.szMusic;
            tList.Add(tDiaogDt);
        }
        tDialogPageParms = new DialogPageParams(tList, EM_DialogcCondition.BattleStart, f_ChangeDialogEnd);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.DialogPage, UIMessageDef.UI_OPEN, tDialogPageParms);
    }
    void f_ChangeDialogEnd(object obj)
    {
        f_SetComplete((int)EM_Guidance.GuidanceDialogEnd);
    }
}

