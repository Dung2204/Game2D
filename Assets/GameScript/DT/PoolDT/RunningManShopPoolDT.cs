using ccU3DEngine;
using System.Collections;

public class RunningManShopPoolDT : BasePoolDT<long>
{
    public RunningManShopPoolDT(RunningManShopDT template)
    {
        iId = template.iId;
        m_Template = template;
        m_iBuyTimes = 0;
    }

    public RunningManShopDT m_Template
    {
        private set;
        get;
    }

    public int m_iBuyTimes
    {
        private set;
        get;
    }

    public void f_UpdateTimes(int buyTimes)
    {
        m_iBuyTimes = buyTimes;
    }

    public void f_Reset()
    {
        if (m_Template.iRefreshType != 0)
        {
            m_iBuyTimes = 0;
        }
    }

}
