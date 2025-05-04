
//============================================
//
//    TreasureRefineConsume来自TreasureRefineConsume.xlsx文件自动生成脚本
//    2017/3/24 19:50:11
//    
//
//============================================
using System;
using System.Collections.Generic;



public class TreasureRefineConsumeSC : NBaseSC
{
    public TreasureRefineConsumeSC()
    {
        Create("TreasureRefineConsumeDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        TreasureRefineConsumeDT DataDT;
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
                DataDT = new TreasureRefineConsumeDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iTreasureid = ccMath.atoi(tData[a++]);
                DataDT.iLv = ccMath.atoi(tData[a++]);
                DataDT.iRefinePillId = ccMath.atoi(tData[a++]);
                DataDT.iRefineNum = ccMath.atoi(tData[a++]);
                DataDT.iGoldNum = ccMath.atoi(tData[a++]);
                DataDT.iTreasureNum = ccMath.atoi(tData[a++]);
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
