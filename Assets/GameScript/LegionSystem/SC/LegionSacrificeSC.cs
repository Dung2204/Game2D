
//============================================
//
//    LegionSacrifice来自LegionSacrifice.xlsx文件自动生成脚本
//    2017/5/23 12:00:28
//    
//
//============================================
using System;
using System.Collections.Generic;



public class LegionSacrificeSC : NBaseSC
{
    public LegionSacrificeSC()
    {
        Create("LegionSacrificeDT");
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        LegionSacrificeDT DataDT;
        string[] tData;
        string[] tFoddScData = ttt[1].Split(new string[] { "|" }, System.StringSplitOptions.None);
        for (int i = 0; i < tFoddScData.Length; i++)
        {
            try
            {
                if (tFoddScData[i] == "")
                {
MessageBox.DEBUG(m_strRegDTName + "String with error record, " + i);
                    continue;
                }
                tData = tFoddScData[i].Split(new string[] { "@," }, System.StringSplitOptions.None);
                int a = 0;
                DataDT = new LegionSacrificeDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.szName = tData[a++];
                DataDT.iSacrificeType = ccMath.atoi(tData[a++]);
                DataDT.iCostType = ccMath.atoi(tData[a++]);
                DataDT.iCostCount = ccMath.atoi(tData[a++]);
                DataDT.iSacrificeNum = ccMath.atoi(tData[a++]);
                DataDT.iSacrificeContributeNum = ccMath.atoi(tData[a++]);
                DataDT.iSacrificeExpNum = ccMath.atoi(tData[a++]);
                DataDT.iSacrificeCritNum = ccMath.atoi(tData[a++]);
                DataDT.iSacrificeCritOdds = ccMath.atoi(tData[a++]);
                DataDT.iNpc = ccMath.atoi(tData[a++]);
                DataDT.szNotText = tData[a++];
                DataDT._szAlreadyText = tData[a++];
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
