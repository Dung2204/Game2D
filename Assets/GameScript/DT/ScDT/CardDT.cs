
//============================================
//
//    Card来自Card.xlsx文件自动生成脚本
//    2017/3/15 17:15:18
//    
//
//============================================
using System;
using System.Collections.Generic;



public class CardDT : NBaseSCDT
{

    /// <summary>
    /// 卡牌名称
    /// </summary>
    public string _szName;
    public string szName
    {
        get {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szName);
        }
    }
    /// <summary>
    /// 卡牌类型
    /// </summary>
    public int iCardType;
    /// <summary>
    /// 卡牌品质
    /// </summary>
    public int iImportant;
    /// <summary>
    /// 卡牌定位类型
    /// </summary>
    public int iCardFightType;
    /// <summary>
    /// 卡牌阵营
    /// </summary>
    public int iCardCamp;
    /// <summary>
    /// 初始攻击
    /// </summary>
    public int iInitAtk;
    /// <summary>
    /// 初始生命
    /// </summary>
    public int iInitHP;
    /// <summary>
    /// 初始物防
    /// </summary>
    public int iInitPhyDef;
    /// <summary>
    /// 初始法防
    /// </summary>
    public int iInitMagDef;
    /// <summary>
    /// 升级攻击
    /// </summary>
    public int iAddAtk;
    /// <summary>
    /// 升级生命
    /// </summary>
    public int iAddHP;
    /// <summary>
    /// 升级物防
    /// </summary>
    public int iAddDef;
    /// <summary>
    /// 升级法防
    /// </summary>
    public int iAddMagDef;
    /// <summary>
    /// 卡牌个性语音脚本Id
    /// </summary>
    public int iCardSoundId;
    /// <summary>
    /// 对应的模型Id
    /// </summary>
    public int iStatelId1;
    /// <summary>
    /// 对应的模型Id
    /// </summary>
    public int iStatelId2;
    /// <summary>
    /// 卡牌描述
    /// </summary>
    public string _szCardDesc;
    public string szCardDesc
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szCardDesc);
        }
    }
    /// <summary>
    /// 将魂出售价格，橙色以上不可出售
    /// </summary>
    public int iSale;
    /// <summary>
    /// 模型1技能
    /// </summary>
    public string szModelMagic1;
    /// <summary>
    /// 模型2技能
    /// </summary>
    public string szModelMagic2;
    /// <summary>
    /// 进化初始Id
    /// </summary>
    public int iEvolveId;
    /// <summary>
    /// nguyên tố ngũ hành
    /// </summary>
    public int iCardEle;


    private MagicDT[] _aMagicDT = null;
    /// <summary>
    /// 卡拥有技能缓冲变量
    /// </summary>
    public MagicDT[] m_aMagicDT
    {
        get
        {
            if (_aMagicDT == null)
            {
                _aMagicDT = new MagicDT[4];
                string tmpStr = null;
                if (szModelMagic1 != "0")
                    tmpStr = szModelMagic1;
                if (szModelMagic2 != "0")
                    tmpStr = szModelMagic2;
                string[] tmpInt = tmpStr.Split(';');
                for (int i = 0; i < tmpInt.Length; i++)
                {
                    if (tmpInt[i] != null)
                    {
                        _aMagicDT[i] = (MagicDT)glo_Main.GetInstance().m_SC_Pool.m_MagicSC.f_GetSC(int.Parse(tmpInt[i]));
                    }
                }
            }
            return _aMagicDT;
        }
    }


}
