using System;
using UnityEngine;
/// <summary>
/// 游戏公式相关
/// </summary>
public class GameFormula
{
    //[FFF700FF]（+50）
    /// <summary>
    ///  体力消耗转成经验  公式：主角等级*参数1*体力消耗
    /// </summary>
    /// <param name="lv">玩家等级</param>
    /// <param name="energyCost">体力消耗</param>
    /// <param name="addExp">军团技能增加的经验值</param>
    /// <returns></returns>
    public static int f_EnergyCost2Exp(int lv, int energyCost,out int addExp)
    {
        GameParamDT param = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.Energy2Exp);
        int result = lv * param.iParam1 * energyCost;
        addExp = f_Exp2AddExp(result);
        return result;
    }
    /// <summary>
    /// 军团技能计算经验值对应增加的经验
    /// </summary>
    /// <param name="exp">经验值</param>
    /// <returns></returns>
    public static int f_Exp2AddExp(int exp)
    {
        int addExp = 0;
        LegionSkillPoolDT legionSkillPoolDT = LegionMain.GetInstance().m_LegionSkillPool.f_GetForId((long)EM_LegionSkillType.SkillNine) as LegionSkillPoolDT;
        if (legionSkillPoolDT.m_LegionSkillDT != null && legionSkillPoolDT.m_LegionSkillDT.iBuffID == (int)EM_RoleProperty.ExpR)
        {
            float percent = legionSkillPoolDT.m_LegionSkillDT.iBuffCount * 1.0f / 10000;
            float lastValue = exp * percent;
            addExp = (int)(Mathf.Floor(lastValue));
        }
        else
            addExp = 0;
        return addExp;
    }
    /// <summary>
    /// 精力消耗转成经验  公式：主角等级*参数1*精力消耗
    /// </summary>
    /// <param name="lv">玩家等级</param>
    /// <param name="vigorCost">精力消耗</param>
    /// <param name="addExp">军团技能增加的经验值</param>
    /// <returns></returns>
    public static int f_VigorCost2Exp(int lv, int vigorCost, out int addExp)
    {
        GameParamDT param = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.Vigor2Exp);
        int result = lv * param.iParam1 * vigorCost;
        addExp = f_Exp2AddExp(result);
        return result;
    }

    /// <summary>
    /// 精力消耗转成银币 公式：ROUNDUP(主角等级*参数1*体力消耗/5+360,-1)
    /// </summary>
    /// <param name="lv">玩家等级</param>
    /// <param name="energyCost">体力消耗</param>
    /// <returns></returns>
    public static int f_EnergyCost2Money(int lv, int energyCost)
    {
        GameParamDT param = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.Energy2Money);
        decimal tResult = Convert.ToDecimal(lv * param.iParam1 * energyCost / 5 + 360).RoundUp(-1);
        int result = Convert.ToInt32(tResult);
        return result;
    }

    /// <summary>
    ///  精力消耗转成银币   公式：ROUNDUP(主角等级*参数1*精力消耗/2+360,-1)
    /// </summary>
    /// <param name="lv">玩家等级</param>
    /// <param name="vigorCost">精力消耗</param>
    /// <returns></returns>
    public static int f_VigorCost2Money(int lv,int vigorCost)
    {
        GameParamDT param = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.Vigor2Money);
        decimal tResult = Convert.ToDecimal(lv * param.iParam1 * vigorCost / 2 + 360).RoundUp(-1);
        int result = Convert.ToInt32(tResult);
        return result;
    }

    /// <summary>
    /// 精英挑战次数购买消费
    /// </summary>
    /// <param name="curTimes"></param>
    /// <returns></returns>
    public static int f_RMEliteBuyTimesCost(int curTimes)
    {
        int cost = (30 + curTimes * 10) * 5;
        return cost;
    }

    /// <summary>
    /// 跨服战元宝消耗  公式：第N次数 = N * value
    /// </summary>
    /// <param name="curTimes">当前已购买次数</param>
    /// <param name="buyTimes">购买次数</param>
    /// <returns></returns>
    public static int f_CrossServerBattleBuyTimesCost(int curTimes,int buyTimes)
    {
        int result = 0;
        for (int i = 1; i <= buyTimes; i++)
        {
            result += (curTimes + i) * Data_Pool.m_CrossServerBattlePool.BuyTimesSyceeParam;
        }
        return result;
    }
    //TsuCode - ChaosBattle
    public static int f_ChaosBattleBuyTimesCost(int curTimes, int buyTimes)
    {
        int result = 0;
        for (int i = 1; i <= buyTimes; i++)
        {
            result += (curTimes + i) * Data_Pool.m_ChaosBattlePool.BuyTimesSyceeParam;
        }
        return result;
    }
    //
}
