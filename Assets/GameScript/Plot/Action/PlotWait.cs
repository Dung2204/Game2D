using ccU3DEngine;

public class PlotWait : ccMachineStateBase
{
    protected PlotSysManager _PlotSysManager;
    public PlotWait(PlotSysManager plotSysManager) : base((int)EM_PlotState.EM_PlotState_Wait)
    {
        _PlotSysManager = plotSysManager;
    }

    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
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
