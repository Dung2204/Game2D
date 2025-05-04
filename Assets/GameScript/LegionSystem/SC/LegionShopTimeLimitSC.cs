
//============================================
//
//    LegionShopTimeLimit来自LegionShopTimeLimit.xlsx文件自动生成脚本
//    2017/5/16 19:11:38
//    
//
//============================================
using System;
using System.Collections.Generic;



public class LegionShopTimeLimitSC : NBaseSC
{
    public LegionShopTimeLimitSC()
    {
        Create("LegionShopTimeLimitDT");
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        LegionShopTimeLimitDT DataDT;
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
                DataDT = new LegionShopTimeLimitDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iTroopsLevel = ccMath.atoi(tData[a++]);
                DataDT.iShowPos = ccMath.atoi(tData[a++]);
                DataDT.iSailResTypeID = ccMath.atoi(tData[a++]);
                DataDT.iSailResID = ccMath.atoi(tData[a++]);
                DataDT.iSailResCount = ccMath.atoi(tData[a++]);
                DataDT.iDiscountIntensity = ccMath.atoi(tData[a++]);
                DataDT.iRefreshType = ccMath.atoi(tData[a++]);
                DataDT.iRefreshTime = ccMath.atoi(tData[a++]);
                DataDT.iCanBuyTimes = ccMath.atoi(tData[a++]);
                DataDT.iBuyResTypeID = ccMath.atoi(tData[a++]);
                DataDT.iBuyResID = ccMath.atoi(tData[a++]);
                DataDT.iBuyResCount = ccMath.atoi(tData[a++]);
                DataDT.iBuyDiscount = ccMath.atoi(tData[a++]);
                DataDT.iBuyTimesOfTroops = ccMath.atoi(tData[a++]);
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
