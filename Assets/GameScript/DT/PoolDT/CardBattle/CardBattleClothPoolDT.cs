using ccU3DEngine;

/// <summary>
/// 斗将中的卡牌布阵PoolDT
/// </summary>
public class CardBattleClothPoolDT : BasePoolDT<long>
{
    public CardBattleClothPoolDT(int idx, int cardTemplateId)
    {
        iId = cardTemplateId;
        Idx = idx;
        f_UpdateCard(cardTemplateId);
    }

    public void f_UpdateCard(int cardTemplateId)
    {
        iId = cardTemplateId;
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
