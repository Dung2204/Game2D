
//============================================
//
//    ActLuckySymbol来自ActLuckySymbol.xlsx文件自动生成脚本
//    2017/4/6 14:57:44
//    
//
//============================================
using System;
using System.Collections.Generic;



public class ActLuckySymbolSC : NBaseSC
{
    public ActLuckySymbolSC()
    {
        Create("ActLuckySymbolDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        ActLuckySymbolDT DataDT;
        string[] tData;
        string[] tFoddScData = ttt[1].Split(new string[] { "|" }, System.StringSplitOptions.None);
        for (int i = 0; i < tFoddScData.Length; i++)
        {
            try
            {
                if (tFoddScData[i] == "")
                {
MessageBox.DEBUG(m_strRegDTName + "code with empty record, " + i);
                    continue;
                }
                tData = tFoddScData[i].Split(new string[] { "@," }, System.StringSplitOptions.None);
                int a = 0;
                DataDT = new ActLuckySymbolDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iBuyPrice = ccMath.atoi(tData[a++]);
                DataDT.iRewardType1 = ccMath.atoi(tData[a++]);
                DataDT.iRewardId1 = ccMath.atoi(tData[a++]);
                DataDT.iRewardCount1 = ccMath.atoi(tData[a++]);
                DataDT.iRewardType2 = ccMath.atoi(tData[a++]);
                DataDT.iRewardId2 = ccMath.atoi(tData[a++]);
                DataDT.iRewardCount2 = ccMath.atoi(tData[a++]);
                SaveItem(DataDT);
            }
            catch
            {
MessageBox.DEBUG(m_strRegDTName + "error code, " + i);
                continue;
            }
        }
    }

}
