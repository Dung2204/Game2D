using ccU3DEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ExclusionSpinPoolDT : BasePoolDT<long>
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

    public NewYearExclusionSpinDT mExclusionSpinPoolDT
    {
        get
        {
            NBaseSCDT tNewYearExclusionSpinDT = glo_Main.GetInstance().m_SC_Pool.m_NewYearExclusionSpinSC.f_GetSC(mId);
            if(tNewYearExclusionSpinDT != null)
            {
                return tNewYearExclusionSpinDT as NewYearExclusionSpinDT;
            }
            return null;
        }
    }
}
