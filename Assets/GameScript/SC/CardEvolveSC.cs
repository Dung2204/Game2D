
//============================================
//
//    CardEvolve来自CardEvolve.xlsx文件自动生成脚本
//    2017/5/22 11:11:18
//    
//
//============================================
using System;
using System.Collections.Generic;



public class CardEvolveSC : NBaseSC
{
    public CardEvolveSC()
    {
        Create("CardEvolveDT");
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        CardEvolveDT DataDT;
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
                DataDT = new CardEvolveDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iEvoLv = ccMath.atoi(tData[a++]);
                DataDT.iNeedLv = ccMath.atoi(tData[a++]);
                DataDT.iNextLvId = ccMath.atoi(tData[a++]);
                DataDT.iMoney = ccMath.atoi(tData[a++]);
                DataDT.iEvolvePill = ccMath.atoi(tData[a++]);
                DataDT.iNeedCardNum = ccMath.atoi(tData[a++]);
                DataDT._szTalentName = tData[a++];
                DataDT._szTalentDescribe = tData[a++];
                DataDT.iTalentId = ccMath.atoi(tData[a++]);
                DataDT._szRemark = tData[a++];
                DataDT.iTypeId = ccMath.atoi(tData[a++]);
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
