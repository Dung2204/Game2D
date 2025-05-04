using UnityEngine;
using System.Collections;

public class RoleControlDT
{
    public long m_iId;
    public RoleModelDT m_RoleModelDT;

    private long iHp;
    public long m_iHp
    {
        get
        {
            return iHp;
        }
        set
        {
            if (value == iHp)
            {
                return;
            }
            iHp = value;

            //通知剧情系统指定回合某个阵营某个站位武将血量达到一定值
            bool bIsHavePlot = Data_Pool.m_DungeonPool.f_JudgeIsHavePlot();
            if (!bIsHavePlot)
                return;
            PlotCheckParam plotCheckParam = new PlotCheckParam();
            plotCheckParam.triggerType = EM_PlotTriggerType.FightRoleHp;
            plotCheckParam.triggerParams = new int[3];
            plotCheckParam.triggerParams[0] = (int)m_EM_Factions;
            plotCheckParam.triggerParams[1] = (int)m_EM_FormationPos;
            plotCheckParam.triggerParams[2] = (int)(iHp * 100 / m_iMaxHp);
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_PLOT_CHECK, plotCheckParam);
        }
    }
    public long m_iMaxHp;

    public int[] m_aModelMagic;

    //public EM_FormationPos m_EM_FormationPos;
    public EM_CloseArrayPos m_EM_FormationPos;
    public EM_Factions m_EM_Factions;

    public FashionableDressDT m_FashionableDressDT;
    public NBaseSCDT m_CardDT;
}