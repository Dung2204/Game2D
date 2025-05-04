using ccU3DEngine;

/// <summary>
/// 斗将的卡牌队伍Pool
/// </summary>
public class CardBattleTeamPoolDT : BasePoolDT<long>
{
    public CardBattleTeamPoolDT(int idx, int cardTemplateId)
    {
        iId = cardTemplateId;
        Idx = idx;
        CardTemplateId = cardTemplateId;
        CardTemplate = (CardDT)glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(cardTemplateId);
    }

    public int Idx
    {
        get;
        private set;
    }

    public int CardTemplateId
    {
        get;
        private set;
    }

    public CardDT CardTemplate
    {
        get;
        private set;
    }
}
