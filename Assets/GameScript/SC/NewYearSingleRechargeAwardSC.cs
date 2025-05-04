
//============================================
//
//    NewYearSingleRechargeAward来自NewYearSingleRechargeAward.xlsx文件自动生成脚本
//    2018/1/17 16:03:21
//    
//
//============================================
using System;
using System.Collections.Generic;



public class NewYearSingleRechargeAwardSC : NBaseSC
{
    public NewYearSingleRechargeAwardSC()
    {
        Create("NewYearSingleRechargeAwardDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        NewYearSingleRechargeAwardDT DataDT;
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
                DataDT = new NewYearSingleRechargeAwardDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iCondition = ccMath.atoi(tData[a++]);
                DataDT.szAward = tData[a++];
                DataDT.iTimeBeg = ccMath.atoi(tData[a++]);
                DataDT.iTimeEnd = ccMath.atoi(tData[a++]);
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
