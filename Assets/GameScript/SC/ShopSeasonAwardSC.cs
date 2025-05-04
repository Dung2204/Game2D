
//============================================
//
//    ShopSeasonAward.xlsx
//    
//
//============================================
using System;
using System.Collections.Generic;



public class ShopSeasonAwardSC : NBaseSC
{
    public ShopSeasonAwardSC()
    {
        Create("ShopSeasonAwardDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        ShopSeasonAwardDT DataDT;
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
                DataDT = new ShopSeasonAwardDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
                    MessageBox.ASSERT("Error Id");
                }
                DataDT._szName = tData[a++];
                DataDT.iVip = ccMath.atoi(tData[a++]);
                DataDT.szCost = tData[a++];
                DataDT.szIcon = tData[a++];
                DataDT.iLimit = ccMath.atoi(tData[a++]);
                DataDT.iHot = ccMath.atoi(tData[a++]);
                DataDT.szAward = tData[a++];
                DataDT.iMoney = ccMath.atoi(tData[a++]);
                DataDT.iFree = ccMath.atoi(tData[a++]);
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
