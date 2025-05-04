using ccU3DEngine;

public class CrossArenaShopPoolDT : BasePoolDT<long>
{
    private int m_BuyTimes;
    private CrossArenaShopDT m_Template;

    public int BuyTimes
    {
        get
        {
            return m_BuyTimes;
        }
    }

    public CrossArenaShopDT Template
    {
        get
        {
            return m_Template;
        }
    }

    public CrossArenaShopPoolDT(CrossArenaShopDT template)
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