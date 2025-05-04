using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;

public class CardPoolDT : BasePoolDT<long>
{
    public CardPoolDT()
    {
        m_RolePropertyPool = new RolePropertyPool(this);
        m_ArtifactPoolDT = new CardArtifactPoolDT();
    }


    public int m_iLv;              // 等级
    /// <summary>
    /// 当前小人经验
    /// </summary>
    public int m_iExp;              // Exp

    private int _iEvolveId;
    /// <summary>
    /// 卡片当前进化Id
    /// </summary>
    public int m_iEvolveId
    {
        get
        {
            return _iEvolveId;
        }
        set
        {
            if (_iEvolveId != value)
            {
                _iEvolveId = value;
                _CardEvolveDT.Clear();
                for (int i = _iTempleteId * 100 + 1; i <= _iEvolveId; i++)
                {
                    _CardEvolveDT.Add((CardEvolveDT)glo_Main.GetInstance().m_SC_Pool.m_CardEvolveSC.f_GetSC(i));
                }
            }
            if (_iEvolveId == 0)
            {
                CardEvolveDT tCardEvolveDT = new CardEvolveDT();
                tCardEvolveDT.iEvoLv = 0;
                tCardEvolveDT.iNextLvId = m_CardDT.iEvolveId;
                tCardEvolveDT.iEvolvePill = 0;
                _CardEvolveDT.Add(tCardEvolveDT);
            }
        }
    }

    /// <summary>
    /// 卡牌进阶DT
    /// </summary>
    public CardEvolveDT m_CardEvolveDT
    {

        get
        {
            if (_CardEvolveDT.Count == 0)
                return null;
            return _CardEvolveDT[_CardEvolveDT.Count - 1];
        }

    }

    /// <summary>
    /// 进化等级
    /// </summary>
    public int m_iEvolveLv
    {
        get
        {
            if (m_CardEvolveDT != null)
                return m_CardEvolveDT.iEvoLv;
            else
                return 0;
        }
    }


    public int m_iLvAwaken;         // 领悟等级
    /// <summary>
    /// 领悟掩码 十进制掩码 ABCD(1000+100+10+1)
    /// </summary>
    public int m_iFlagAwaken;       // 领悟掩码 DCBA


    public short uSkyDestinyLv; // 天命等级
    public int uSkyDestinyExp;// 天命经验


    /// <summary>
    /// 角色卡牌模版
    /// </summary>
    private int _iTempleteId;
    public int m_iTempleteId
    {
        get
        {
            return _iTempleteId;
        }

        set
        {
            if (_iTempleteId != value)
            {
                _iTempleteId = value;
                m_CardDT = (CardDT)glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(_iTempleteId);
                _iData1 = _iTempleteId;
            }
        }
    }

    public CardDT m_CardDT;
    // icon
    public int Icon
    {
        get
        {
            int iModelId = m_CardDT.iStatelId1;
            if (iModelId == 0)
            {
                iModelId = m_CardDT.iStatelId2;
            }
            RoleModelDT roleModle = (RoleModelDT)glo_Main.GetInstance().m_SC_Pool.m_RoleModelSC.f_GetSC(iModelId);
            return roleModle.iIcon;
        }
    }

    /// <summary>
    /// 卡牌缘分信息
    /// </summary>
    public CardFatePoolDT m_CardFatePoolDT;

    public RolePropertyPool m_RolePropertyPool;

    /// <summary>
    /// 卡牌的全部进阶属性
    /// </summary>
    public List<CardEvolveDT> _CardEvolveDT = new List<CardEvolveDT>();
    
    /// <summary>
    /// 对应当前装备的时装
    /// </summary>
    public FanshionableDressPoolDT m_FanshionableDressPoolDT;

    /// <summary>
    /// 卡牌神器结构体
    /// </summary>
    public CardArtifactPoolDT m_ArtifactPoolDT;

    public int m_TacticalId;

    public byte m_IsPayMonthCard {
        get {
            bool monthCard = Data_Pool.m_ActivityCommonData.m_MonthCardIsBuy25;
            bool PerpetualCard= Data_Pool.m_ActivityCommonData.m_MonthCardIsBuy50;
            if (monthCard && PerpetualCard)
                return 3;
            else if (monthCard)
                return 1;
            else if (PerpetualCard)
                return 2;
            else
                return 0; 
        }
    }  //   0没有购买 1 月卡  2 终身卡 3 都购买
}
