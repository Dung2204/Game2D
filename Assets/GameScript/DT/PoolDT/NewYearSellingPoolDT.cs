using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;

class NewYearSellingPoolDT : BasePoolDT<long>
{
    private int _iTempleteId;
    private int _BuyTime;
    public bool m_bIsShowBtn
    {
        get
        {
            if (m_NewYearSelling == null)
                return false;
            int StarTime = 0;
            int EndTime = 0;
            int NowTime = GameSocket.GetInstance().f_GetServerTime();

            StarTime = ccMath.f_Data2Int(m_NewYearSelling.iStarTime);
            EndTime = ccMath.f_Data2Int(m_NewYearSelling.iEndTime);

            if (m_NewYearSelling.iRankDownLimit <= Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level))
            {
                if (NowTime >= StarTime && NowTime < EndTime)
                    return _BuyTime < m_NewYearSelling.iBuyTime;
            }
            return false;  
        }
    }
    public int m_BuyTime
    {
        get { return _BuyTime; }
        set
        {
            _BuyTime = value;
        }
    }
    public int m_iTempleteId
    {
        get { return _iTempleteId; }

        set
        {
            _iTempleteId = value;
            m_NewYearSelling = glo_Main.GetInstance().m_SC_Pool.m_NewYearSelling.f_GetSC(value) as NewYearSellingDT;
        }
    }

    public NewYearSellingDT m_NewYearSelling;
}

