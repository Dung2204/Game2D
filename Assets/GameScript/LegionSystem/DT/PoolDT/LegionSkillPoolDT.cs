using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 军团技能PoolDT
/// </summary>
public class LegionSkillPoolDT : BasePoolDT<long>
{
    private int _skillLevel;
    /// <summary>
    /// 玩家当前的学习等级
    /// </summary>
    public int m_SkillLevel
    {
        set
        {
            _skillLevel = value;
            m_LegionSkillDT = GetLegionSkillDT((int)iId, _skillLevel);
        }
        get { return _skillLevel; }
    }
    /// <summary>
    /// 技能上限等级
    /// </summary>
    public int m_SkillLevelMax;
    /// <summary>
    /// 军团技能DT(等级为0的DT则为null)
    /// </summary>
    public LegionSkillDT m_LegionSkillDT;
    /// <summary>
    /// 通过技能类型和技能等级获取DT
    /// </summary>
    /// <param name="skillType">技能类型</param>
    /// <param name="skillLevel">技能等级</param>
    /// <returns></returns>
    public LegionSkillDT GetLegionSkillDT(int skillType, int skillLevel)
    {
        List<NBaseSCDT> listDT = glo_Main.GetInstance().m_SC_Pool.m_LegionSkillSC.f_GetAll();
        for (int i = 0; i < listDT.Count; i++)
        {
            LegionSkillDT dt = listDT[i] as LegionSkillDT;
            if (dt.iType == skillType && dt.iLevel == skillLevel)
                return dt;
        }
        return null;
    }
}
