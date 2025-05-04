
//============================================
//
//    Buf来自Buf.xlsx文件自动生成脚本
//    2017/11/16 19:22:39
//    
//
//============================================
using System;
using System.Collections.Generic;



public class BufDT : NBaseSCDT
{

    /// <summary>
    /// Buff描述
    /// </summary>
    public string _strReadme;
    public string strReadme
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_strReadme);
        }
    }
    /// <summary>
    /// BUFF效果类型：1，属性加成类（参数x表述属性ID，参数y表示属性值，参数z表示作用对象）2：灼烧（参数x为攻击加成，参数y为增加固定值，参数z无效）3：中毒（参数x为攻击加成，参数y为增加固定值，参数z无效）4：治疗（参数x为攻击加成，参数y为增加固定值，参数z无效）5：眩晕（参数无效）6：无敌（参数无效）。7，战斗系数，可以放大缩小受到的最终伤害，万分率
    /// </summary>
    public int iType;
    /// <summary>
    /// 参数x(属性ID为：1,攻击。2,生命。3,物防。4,法防。5,攻击加成。6,生命加成。7,物防加成。8,法防加成。9,基础怒气。11,暴击率。12,抗爆率。13,命中率。14,闪避率。15,伤害加成。16,伤害减免。17，PVP增伤。18，PVP减伤。101,克起源,魏。102,克契约,蜀。103,克魂,吴。104,克黑暗,群。105,抗起源,魏。106,抗契约,蜀。107,抗魂,吴。108,抗黑暗,群）
    /// </summary>
    public int iPara;
    /// <summary>
    /// 参数y
    /// </summary>
    public int iParaY;
    /// <summary>
    /// 参数z
    /// </summary>
    public int iParaZ;
    /// <summary>
    /// 处理优先级
    /// </summary>
    public int iSort;
    /// <summary>
    /// 增益状态标记
    /// </summary>
    public int iPlusState;
    /// <summary>
    /// 不良状态标记
    /// </summary>
    public int iReduceState;
    /// <summary>
    /// 中毒和灼烧标记
    /// </summary>
    public int iPoisoningAndFire;
    /// <summary>
    /// 不可清除标记
    /// </summary>
    public int iNotClear;
}
