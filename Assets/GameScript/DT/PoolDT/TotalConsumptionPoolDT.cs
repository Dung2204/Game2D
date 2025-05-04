using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;
public class TotalConsumptionPoolDT : BasePoolDT<long>
{
    public int m_iTempleteId
    {
        set
        {
            iId = value;
        }
    }

    public bool m_bIsGetAward;

    public NewYearSyceeConsumeDT m_NewYearSyceeConsume;
}

