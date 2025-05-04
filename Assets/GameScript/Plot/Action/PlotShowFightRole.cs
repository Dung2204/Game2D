using ccU3DEngine;

public class PlotShowFightRole : ccMachineStateBase
{
    protected PlotSysManager _PlotSysManager;
    public PlotShowFightRole(PlotSysManager plotSysManager) : base((int)EM_PlotState.EM_PlotState_ShowFightRole)
    {
        _PlotSysManager = plotSysManager;
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_SHOW_FIGHT_ROLE_END,
           (object obj) => { f_SetComplete((int)EM_PlotState.EM_PlotState_Wait); });
    }

    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_SHOW_FIGHT_ROLE, Obj);
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