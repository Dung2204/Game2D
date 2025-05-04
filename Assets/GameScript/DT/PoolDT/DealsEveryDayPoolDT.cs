using ccU3DEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DealsEveryDayPoolDT : BasePoolDT<long>
{
    public int mId;

    public int mGet;

    public bool IsCanGet
    {
        get
        {
            return mGet > 0;
        }
    }

    public NewYearDealsEveryDayDT mDealsEveryDayPoolDT
    {
        get
        {
            NBaseSCDT tDealsEveryDayPoolDT = glo_Main.GetInstance().m_SC_Pool.m_NewYearDealsEveryDaySC.f_GetSC(mId);
            if(tDealsEveryDayPoolDT != null)
            {
                return tDealsEveryDayPoolDT as NewYearDealsEveryDayDT;
            }
            return null;
        }
    }
}
