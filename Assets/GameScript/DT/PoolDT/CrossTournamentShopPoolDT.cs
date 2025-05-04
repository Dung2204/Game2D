using ccU3DEngine;

public class CrossTournamentShopPoolDT : BasePoolDT<long>
{
    private int m_BuyTimes;
    private CrossTournamentShopDT m_Template;

    public int BuyTimes
    {
        get
        {
            return m_BuyTimes;
        }
    }

    public CrossTournamentShopDT Template
    {
        get
        {
            return m_Template;
        }
    }

    public CrossTournamentShopPoolDT(CrossTournamentShopDT template)
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