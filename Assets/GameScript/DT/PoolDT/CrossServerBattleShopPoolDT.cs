using ccU3DEngine;

public class CrossServerBattleShopPoolDT : BasePoolDT<long>
{
    private int m_BuyTimes;
    private CrossServerBattleShopDT m_Template;

    public int BuyTimes
    {
        get
        {
            return m_BuyTimes;
        }
    }

    public CrossServerBattleShopDT Template
    {
        get
        {
            return m_Template;
        }
    }

    public CrossServerBattleShopPoolDT(CrossServerBattleShopDT template)
    {
        iId = template.iId;
        m_BuyTimes = 0;
        m_Template = template;
        _iData1 = template.iShowIdx;
    }

    public void f_UpdateInfo(int buyTimes)
    {
        m_BuyTimes = buyTimes;
    }

    public void f_Reset()
    {
        m_BuyTimes = 0;
    }
}