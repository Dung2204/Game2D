
//============================================
//
//    ShopGift来自ShopGift.xlsx文件自动生成脚本
//    2017/3/23 14:07:25
//    
//
//============================================
using System;
using System.Collections.Generic;



public class ShopGiftSC : NBaseSC
{
    public ShopGiftSC()
    {
        Create("ShopGiftDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        ShopGiftDT DataDT;
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
                DataDT = new ShopGiftDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iType = ccMath.atoi(tData[a++]);
                DataDT.iTempId = ccMath.atoi(tData[a++]);
                DataDT.iNum = ccMath.atoi(tData[a++]);
                DataDT.iShowSite = ccMath.atoi(tData[a++]);
                DataDT.iTimes = ccMath.atoi(tData[a++]);
                DataDT.iVipLimit = ccMath.atoi(tData[a++]);
                DataDT.iOldNum = ccMath.atoi(tData[a++]);
                DataDT.iNewNum = ccMath.atoi(tData[a++]);
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
