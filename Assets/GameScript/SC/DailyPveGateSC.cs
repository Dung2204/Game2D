
//============================================
//
//    DailyPveGate来自DailyPveGate.xlsx文件自动生成脚本
//    2017/10/13 10:46:20
//    
//
//============================================
using System;
using System.Collections.Generic;



public class DailyPveGateSC : NBaseSC
{
    public DailyPveGateSC()
    {
        Create("DailyPveGateDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        DailyPveGateDT DataDT;
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
                DataDT = new DailyPveGateDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iType = ccMath.atoi(tData[a++]);
                DataDT._szLevelName1 = tData[a++];
                DataDT.szLevelIcon1 = tData[a++];
                DataDT.iLevelLimit1 = ccMath.atoi(tData[a++]);
                DataDT.szZhanLi1 = tData[a++];
                DataDT.szAward = tData[a++];
                DataDT.iMoster11 = ccMath.atoi(tData[a++]);
                DataDT.iMoster12 = ccMath.atoi(tData[a++]);
                DataDT.iMoster13 = ccMath.atoi(tData[a++]);
                DataDT.iMoster14 = ccMath.atoi(tData[a++]);
                DataDT.iMoster15 = ccMath.atoi(tData[a++]);
                DataDT.iMoster16 = ccMath.atoi(tData[a++]);
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
