using ccU3DEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class FestivalExchangePoolDT : BasePoolDT<long>
{
    public int mId;


    public int mNum;


    public bool IsCanGet
    {
        get
        {
            return mNum < mFestivalExchangeDT.iCount;
        }
    }

    public FestivalExchangeDT mFestivalExchangeDT
    {
        get
        {


            //FestivalExchangeDT tFestivalExchangeDT = new FestivalExchangeDT();

            //tFestivalExchangeDT.iId = 1;
            //tFestivalExchangeDT.szResAward = "3;516;1";
            //tFestivalExchangeDT.szResNeed = "2;106;480#3;516;1";
            //tFestivalExchangeDT.iBeginTime = 20180518;
            //tFestivalExchangeDT.iEndTime = 20180524;

            NBaseSCDT tFestivalExchangeDT = glo_Main.GetInstance().m_SC_Pool.m_FestivalExchangeSC.f_GetSC(mId);
            if(tFestivalExchangeDT != null)
            {
                return tFestivalExchangeDT as FestivalExchangeDT;
            }
            return null;
        }
    }
}
