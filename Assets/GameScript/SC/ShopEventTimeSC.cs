
//============================================
//
//    ShopEventTime.xlsx
//    
//
//============================================
using System;
using System.Collections.Generic;



public class ShopEventTimeSC : NBaseSC
{
    public ShopEventTimeSC()
    {
        Create("ShopEventTimeDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        ShopEventTimeDT DataDT;
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
                DataDT = new ShopEventTimeDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
                    MessageBox.ASSERT("Error Id");
                }
                DataDT._szName = tData[a++];
                DataDT.szBanner = tData[a++];
                DataDT.iType = ccMath.atoi(tData[a++]);
                DataDT.iOpenTime = ccMath.atoi(tData[a++]);
                DataDT.iEndTime = ccMath.atoi(tData[a++]);
                DataDT.szNameConst = tData[a++];
                DataDT.szItems = tData[a++];
                DataDT.iResetType = ccMath.atoi(tData[a++]);
                DataDT.szPoster = tData[a++];

                SaveItem(DataDT);
            }
            catch
            {
                MessageBox.DEBUG(m_strRegDTName + "String record error, " + i);
                continue;
            }
        }
    }

}
