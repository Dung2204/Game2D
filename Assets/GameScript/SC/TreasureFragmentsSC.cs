
//============================================
//
//    TreasureFragments来自TreasureFragments.xlsx文件自动生成脚本
//    2017/3/24 13:29:18
//    
//
//============================================
using System;
using System.Collections.Generic;



public class TreasureFragmentsSC : NBaseSC
{
    public TreasureFragmentsSC()
    {
        Create("TreasureFragmentsDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        TreasureFragmentsDT DataDT;
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
                DataDT = new TreasureFragmentsDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT._szName = tData[a++];
                DataDT.iIcon = ccMath.atoi(tData[a++]);
                DataDT.iImportant = ccMath.atoi(tData[a++]);
                DataDT._szDescribe = tData[a++];
                DataDT.iPileNum = ccMath.atoi(tData[a++]);
                DataDT.iType = ccMath.atoi(tData[a++]);
                DataDT.iIsUse = ccMath.atoi(tData[a++]);
                DataDT.iPath1 = ccMath.atoi(tData[a++]);
                DataDT.iPath2 = ccMath.atoi(tData[a++]);
                DataDT.iPath3 = ccMath.atoi(tData[a++]);
                DataDT.iPath4 = ccMath.atoi(tData[a++]);
                DataDT.iTreasureId = ccMath.atoi(tData[a++]);
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
