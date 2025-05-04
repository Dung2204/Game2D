
//============================================
//
//    RunningManShop来自RunningManShop.xlsx文件自动生成脚本
//    2017/7/31 16:31:08
//    
//
//============================================
using System;
using System.Collections.Generic;



public class RunningManShopSC : NBaseSC
{
    public RunningManShopSC()
    {
        Create("RunningManShopDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        RunningManShopDT DataDT;
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
                DataDT = new RunningManShopDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iStoreOpenLevel = ccMath.atoi(tData[a++]);
                DataDT.iStoreTabNo = ccMath.atoi(tData[a++]);
                DataDT.iSailResTypeID = ccMath.atoi(tData[a++]);
                DataDT.iSailResID = ccMath.atoi(tData[a++]);
                DataDT.iSailResCount = ccMath.atoi(tData[a++]);
                DataDT.iOpenType = ccMath.atoi(tData[a++]);
                DataDT.iOpenValue = ccMath.atoi(tData[a++]);
                DataDT.iRefreshType = ccMath.atoi(tData[a++]);
                DataDT.iCanBuyTimes = ccMath.atoi(tData[a++]);
                DataDT.iDiscount = ccMath.atoi(tData[a++]);
                DataDT.iTokenResTableID1 = ccMath.atoi(tData[a++]);
                DataDT.iTokenResID1 = ccMath.atoi(tData[a++]);
                DataDT.iTokenResCount1 = ccMath.atoi(tData[a++]);
                DataDT.iTokenResTableID2 = ccMath.atoi(tData[a++]);
                DataDT.iTokenResID2 = ccMath.atoi(tData[a++]);
                DataDT.iTokenResCount2 = ccMath.atoi(tData[a++]);
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
