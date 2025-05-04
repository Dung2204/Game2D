
//============================================
//
//    LotteryLimitEvent.xlsx
//    
//
//============================================
using System;
using System.Collections.Generic;
using System.Linq;

public class LotteryLimitEventSC : NBaseSC
{
    public LotteryLimitEventSC()
    {
        Create("LotteryLimitEventDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        LotteryLimitEventDT DataDT;
        string[] tData;
        string[] tFoddScData = ttt[1].Split(new string[] { "|" }, System.StringSplitOptions.None);
        for (int i = 0; i < tFoddScData.Length; i++)
        {
            try
            {
                if (tFoddScData[i] == "")
                {
MessageBox.DEBUG(m_strRegDTName + "String with empty record, " + i);
                    continue;
                }
                tData = tFoddScData[i].Split(new string[] { "@," }, System.StringSplitOptions.None);
                int a = 0;
                DataDT = new LotteryLimitEventDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iEventTime = ccMath.atoi(tData[a++]);
                DataDT.szOnceCost = tData[a++];
                DataDT.szTenCost = tData[a++];
                DataDT.iPoolId = ccMath.atoi(tData[a++]);
                DataDT.iCardId = ccMath.atoi(tData[a++]);
                DataDT.iNum = ccMath.atoi(tData[a++]);
               
                SaveItem(DataDT);
            }
            catch
            {
MessageBox.DEBUG(m_strRegDTName + "String record error, " + i);
                continue;
            }
        }
    }
    public List<NBaseSCDT> f_GetSCByEventTimeId(int iId)
    {
        return f_GetAll().Where(o => o is LotteryLimitEventDT data && data.iEventTime == iId).ToList();
    }
}
