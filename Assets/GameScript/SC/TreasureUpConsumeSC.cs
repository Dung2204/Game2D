
//============================================
//
//    TreasureUpConsume来自TreasureUpConsume.xlsx文件自动生成脚本
//    2017/3/24 13:29:38
//    
//
//============================================
using System;
using System.Collections.Generic;



public class TreasureUpConsumeSC : NBaseSC
{
    public TreasureUpConsumeSC()
    {
        Create("TreasureUpConsumeDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        TreasureUpConsumeDT DataDT;
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
                DataDT = new TreasureUpConsumeDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iImportant = ccMath.atoi(tData[a++]);
                DataDT.iLV = ccMath.atoi(tData[a++]);
                DataDT.iIntensifyExp = ccMath.atoi(tData[a++]);
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
