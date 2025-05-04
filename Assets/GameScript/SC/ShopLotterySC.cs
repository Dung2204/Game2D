
//============================================
//
//    ShopLottery来自ShopLottery.xlsx文件自动生成脚本
//    2017/3/22 20:26:42
//    
//
//============================================
using System;
using System.Collections.Generic;



public class ShopLotterySC : NBaseSC
{
    public ShopLotterySC()
    {
        Create("ShopLotteryDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        ShopLotteryDT DataDT;
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
                DataDT = new ShopLotteryDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iFreeCD = ccMath.atoi(tData[a++]);
                DataDT.szOnceCost1 = tData[a++];
                DataDT.szOnceCost2 = tData[a++];
                DataDT.szTenCost1 = tData[a++];
                DataDT.szTenCost2 = tData[a++];
                DataDT.iPoolId = ccMath.atoi(tData[a++]);
                DataDT.iExtraPoolId = ccMath.atoi(tData[a++]);
                DataDT.iChoose = ccMath.atoi(tData[a++]);
                DataDT.iOpenNum = ccMath.atoi(tData[a++]);
                DataDT.szItems = tData[a++];
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
