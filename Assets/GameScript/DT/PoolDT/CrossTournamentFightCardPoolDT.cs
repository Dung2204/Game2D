using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;

public class CrossTournamentFightCardPoolDT : BasePoolDT<long>

{

    private int _iCardId;
    /// <summary>
    /// 出占卡牌
    /// </summary>    
    public int m_iCardId
    {
        get
        {
            return _iCardId;
        }
    }

    /// <summary>
    /// 角色卡牌模版
    /// </summary>
    //public CardPoolDT m_CardPoolDT
    //{
    //    get
    //    {
    //        return _CardPoolDT;
    //    }
    //}

    public CardDT m_CardDT
    {
        get
        {
            return (CardDT)glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(_iCardId);
        }
    }
    private int _iLevel;
    public int m_iLevel
    {
        get
        {
            return _iLevel;
        }
    }
    public CrossTournamentFightCardPoolDT()
    {
    }
    public void f_UpdateInfo(TournamentFighterCard info)
    {
        _iCardId = info.tempId;
        _iLevel = info.uLv;
    }
}