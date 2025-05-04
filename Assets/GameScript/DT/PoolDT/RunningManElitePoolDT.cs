using ccU3DEngine;

public class RunningManElitePoolDT : BasePoolDT<long>
{
    public RunningManElitePoolDT(RunningManEliteDT preDt,RunningManEliteDT dt)
    {
        iId = dt.iId;
        template = dt;
        isPassed = false;
        showModelDT = (RoleModelDT)glo_Main.GetInstance().m_SC_Pool.m_RoleModelSC.f_GetSC(template.iShowMonster);
        if (showModelDT == null)
MessageBox.ASSERT("Invalid model data,ID:" + template.iShowMonster);
        preTollgateName = preDt.szName;
    }

    private bool isPassed;
    /// <summary>
    /// 是否已经通关了
    /// </summary>
    public bool m_bIsPassed
    {
        get
        {
            return isPassed;
        }
    }

    private RunningManEliteDT template;
    /// <summary>
    /// 模板数据
    /// </summary>
    public RunningManEliteDT  m_Template
    {
        get
        {
            return template;
        }
    }

    private RoleModelDT showModelDT;
    public RoleModelDT m_ShowModelDT
    {
        get {
            return showModelDT;
        }
    }
    public void f_UpdatePassInfo(int passedIdx)
    {
        isPassed = iId <= passedIdx;
    }

    private string preTollgateName;
    /// <summary>
    /// 前置关卡名字
    /// </summary>
    public string m_szPreTollgateName
    {
        get
        {
            return preTollgateName;
        }
    }
}
