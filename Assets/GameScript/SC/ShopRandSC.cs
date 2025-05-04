
//============================================
//
//    ShopRand来自ShopRand.xlsx文件自动生成脚本
//    2017/3/28 16:54:41
//    
//
//============================================
using System;
using System.Collections.Generic;



public class ShopRandSC : NBaseSC
{
    public ShopRandSC()
    {
        Create("ShopRandDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        ShopRandDT DataDT;
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
                DataDT = new ShopRandDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iType = ccMath.atoi(tData[a++]);
                DataDT.iIsSystemClock = ccMath.atoi(tData[a++]);
                DataDT.szSystemClock = tData[a++];
                DataDT.iFreeTimes = ccMath.atoi(tData[a++]);
                DataDT.iFreeCD = ccMath.atoi(tData[a++]);
                DataDT.iItem = ccMath.atoi(tData[a++]);
                DataDT.iItemNum = ccMath.atoi(tData[a++]);
                DataDT.iCoin = ccMath.atoi(tData[a++]);
                DataDT.iCoinNum = ccMath.atoi(tData[a++]);
                DataDT.iSite = ccMath.atoi(tData[a++]);
                DataDT.iMinLv = ccMath.atoi(tData[a++]);
                DataDT.iMaxLv = ccMath.atoi(tData[a++]);
                DataDT.iSetId = ccMath.atoi(tData[a++]);
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
