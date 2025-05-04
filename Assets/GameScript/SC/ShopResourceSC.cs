
//============================================
//
//    ShopResource来自ShopResource.xlsx文件自动生成脚本
//    2017/3/15 18:22:20
//    
//
//============================================
using System;
using System.Collections.Generic;



public class ShopResourceSC : NBaseSC
{
    public ShopResourceSC()
    {
        Create("ShopResourceDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        ShopResourceDT DataDT;
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
                DataDT = new ShopResourceDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                DataDT.iType = ccMath.atoi(tData[a++]);
                DataDT.iTempId = ccMath.atoi(tData[a++]);
                DataDT._szName = tData[a++];
                DataDT.iNum = ccMath.atoi(tData[a++]);
                DataDT.iShowLv = ccMath.atoi(tData[a++]);
                DataDT.iShowSite = ccMath.atoi(tData[a++]);
                DataDT.iRefrsh = ccMath.atoi(tData[a++]);
                DataDT.iRefrshTime = ccMath.atoi(tData[a++]);
                DataDT.szDIscount = tData[a++];
                DataDT.szNewNum = tData[a++];
                DataDT.szVipLimitTimes = tData[a++];
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
