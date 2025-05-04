using ccU3DEngine;

public class ChaosBattleShopPoolDT : BasePoolDT<long>
{
    private int m_BuyTimes;
    private ChaosBattleShopDT m_Template;

    public int BuyTimes
    {
        get
        {
            return m_BuyTimes;
        }
    }

    public ChaosBattleShopDT Template
    {
        get
        {
            return m_Template;
        }
    }

    public ChaosBattleShopPoolDT(ChaosBattleShopDT template)
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