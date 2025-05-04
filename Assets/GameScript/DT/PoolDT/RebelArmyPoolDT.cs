using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;
public class RebelArmyPoolDT: BasePoolDT<long>
{
    /// <summary>
    /// 模版ID
    /// </summary>
    private int _iTempleteId;

    public int _ITempleteId
    {
        get
        {
            return _iTempleteId;
        }

        set
        {
            _iTempleteId = value;
            m_RebelArmyDeploy = (RebelArmyDeployDT)glo_Main.GetInstance().m_SC_Pool.m_RebelArmyDeploySC.f_GetSC(_iTempleteId);
        }
    }
    /// <summary>
    /// 叛军等级
    /// </summary>
    public int m_RevelLv;
    /// <summary>
    /// 品质
    /// </summary>
    public byte m_Color;
    /// <summary>
    /// 逃跑时间
    /// </summary>
    public int m_EndTime;
    /// <summary>
    /// 血量亿分比（以前百分比，如4000万的血，百分之一就是40万，，会出现如果小于40万的伤害,百分比就不会变化的bug!!!）
    /// </summary>
    public int hpPercent;

    public RebelArmyDeployDT m_RebelArmyDeploy; 
}
