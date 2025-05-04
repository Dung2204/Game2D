
//============================================
//
//    Treasure来自Treasure.xlsx文件自动生成脚本
//    2017/3/24 13:29:09
//    
//
//============================================
using System;
using System.Collections.Generic;



public class TreasureSC : NBaseSC
{
    public TreasureSC()
    {
        Create("TreasureDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        TreasureDT DataDT;
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
                DataDT = new TreasureDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT._szName = tData[a++];
                DataDT.iIcon = ccMath.atoi(tData[a++]);
                DataDT.iImportant = ccMath.atoi(tData[a++]);
                DataDT._szDescribe = tData[a++];
                DataDT.iSite = ccMath.atoi(tData[a++]);
                DataDT.iIntenProId1 = ccMath.atoi(tData[a++]);
                DataDT.iStartPro1 = ccMath.atoi(tData[a++]);
                DataDT.iAddPro1 = ccMath.atoi(tData[a++]);
                DataDT.iIntenProId2 = ccMath.atoi(tData[a++]);
                DataDT.iStartPro2 = ccMath.atoi(tData[a++]);
                DataDT.iAddPro2 = ccMath.atoi(tData[a++]);
                DataDT.iRefinProId1 = ccMath.atoi(tData[a++]);
                DataDT.iRefinPro1 = ccMath.atoi(tData[a++]);
                DataDT.iRefinProId2 = ccMath.atoi(tData[a++]);
                DataDT.iRefinPro2 = ccMath.atoi(tData[a++]);
                DataDT.iExp = ccMath.atoi(tData[a++]);
                DataDT.iList = ccMath.atoi(tData[a++]);
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
