using ccU3DEngine;
using System.Collections.Generic;

public class PlotDialog : ccMachineStateBase
{
    protected PlotSysManager _PlotSysManager;
    public PlotDialog(PlotSysManager plotSysManager) : base((int)EM_PlotState.EM_PlotState_Dialog)
    {
        _PlotSysManager = plotSysManager;
    }

    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
        if (null == Obj) {
MessageBox.ASSERT("PlotDialog parameter null！！！");
            f_SetComplete((int)EM_PlotState.EM_PlotState_Wait);
            return;
        }
        PlotDT plotDt = (PlotDT)Obj;
        string[] effectParams = plotDt.szEffectParams.Split('^');
        List<DungeonDialogDT> dungeonDialogDtList = new List<DungeonDialogDT>();
        for (int i = 0; i < effectParams.Length; i++)
        {
            if (effectParams[i] == "")
                continue;
            int dialogId;
            int.TryParse(effectParams[i], out dialogId);
            if (dialogId <= 0)
            {
                continue;
            }
            SC_Pool scPool = glo_Main.GetInstance().m_SC_Pool;
            if (null == scPool)
            {
                MessageBox.ASSERT("SC_Pool null");
                continue;
            }
            DungeonDialogSC dialogSc = scPool.m_DungeonDialogSC;
            if (null == dialogSc)
            {
                MessageBox.ASSERT("DungeonDialogSC null");
                continue;
            }
            DungeonDialogDT dungeonDialogDt = dialogSc.f_GetSC(dialogId) as DungeonDialogDT;
            if (null == dungeonDialogDt)
            {
                MessageBox.ASSERT("null plot，id:" + dialogId);
                continue;
            }
            dungeonDialogDtList.Add(dungeonDialogDt);
        }        
        DialogPageParams tParam = new DialogPageParams(dungeonDialogDtList, EM_DialogcCondition.ForPlotSys, (object obj) => { f_SetComplete((int)EM_PlotState.EM_PlotState_Wait); });
        ccUIManage.GetInstance().f_SendMsg(UINameConst.DialogPage, UIMessageDef.UI_OPEN, tParam);
    }

    public override void f_Execute()
    {
        base.f_Execute();
    }

    public override void f_Exit()
    {
        base.f_Exit();
    }
}
