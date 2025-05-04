
//============================================
//
//    BattleFeatShop来自BattleFeatShop.xlsx文件自动生成脚本
//    2017/9/27 15:11:29
//    
//
//============================================
using System;
using System.Collections.Generic;



public class BattleFeatShopSC : NBaseSC
{
    public BattleFeatShopSC()
    {
        Create("BattleFeatShopDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        BattleFeatShopDT DataDT;
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
                DataDT = new BattleFeatShopDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iStoreOpenLevel = ccMath.atoi(tData[a++]);
                DataDT.iSailResTypeID = ccMath.atoi(tData[a++]);
                DataDT.iSailResID = ccMath.atoi(tData[a++]);
                DataDT.iSailResCount = ccMath.atoi(tData[a++]);
                DataDT.iOpenValue = ccMath.atoi(tData[a++]);
                DataDT.iShowPos = ccMath.atoi(tData[a++]);
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
