
//============================================
//
//    WeekFund来自WeekFund.xlsx文件自动生成脚本
//    2018/4/20 15:32:03
//    
//
//============================================
using System;
using System.Collections.Generic;



public class WeekFundSC : NBaseSC
{
    public WeekFundSC()
    {
        Create("WeekFundDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        WeekFundDT DataDT;
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
                DataDT = new WeekFundDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.itype = ccMath.atoi(tData[a++]);
                DataDT.iActivityBegin = ccMath.atoi(tData[a++]);
                DataDT.iActivityEnd = ccMath.atoi(tData[a++]);
                DataDT.iAwardEnd = ccMath.atoi(tData[a++]);
                DataDT.iRechargeNum = ccMath.atoi(tData[a++]);
                DataDT.szAward1 = tData[a++];
                DataDT.szAward2 = tData[a++];
                DataDT.szAward3 = tData[a++];
                DataDT.szAward4 = tData[a++];
                DataDT.szAward5 = tData[a++];
                DataDT.szAward6 = tData[a++];
                DataDT.szAward7 = tData[a++];
                DataDT.szNote = tData[a++];
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
