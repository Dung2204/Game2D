
//============================================
//
//    NewYearMultiRechargeAward来自NewYearMultiRechargeAward.xlsx文件自动生成脚本
//    2018/1/17 16:03:00
//    
//
//============================================
using System;
using System.Collections.Generic;



public class NewYearExclusionSpinSC : NBaseSC
{
    public NewYearExclusionSpinSC()
    {
        Create("NewYearExclusionSpinDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        NewYearExclusionSpinDT DataDT;
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
                DataDT = new NewYearExclusionSpinDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.szAward = tData[a++];
                DataDT.iHot = ccMath.atoi(tData[a++]);
                DataDT.iPos = ccMath.atoi(tData[a++]);
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
