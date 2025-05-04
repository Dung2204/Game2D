using ccU3DEngine;
using UnityEngine;

public class PlotArtAni : ccMachineStateBase
{
    protected PlotSysManager _PlotSysManager;
    public PlotArtAni(PlotSysManager plotSysManager) : base((int)EM_PlotState.EM_PlotState_ArtAni)
    {
        _PlotSysManager = plotSysManager;
    }

    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
        Debug.LogError("PlotArtAni");
        f_SetComplete((int)EM_PlotState.EM_PlotState_Wait);
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