
//============================================
//
//    RankGift来自RankGift.xlsx文件自动生成脚本
//    2017/9/18 16:46:45
//    
//
//============================================
using System;
using System.Collections.Generic;



public class RankGiftSC : NBaseSC
{
    public RankGiftSC()
    {
        Create("RankGiftDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        RankGiftDT DataDT;
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
                DataDT = new RankGiftDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iOpenLevel = ccMath.atoi(tData[a++]);
                DataDT.iShowPos = ccMath.atoi(tData[a++]);
                DataDT.iRewardType = ccMath.atoi(tData[a++]);
                DataDT.iRewardId = ccMath.atoi(tData[a++]);
                DataDT.iRewardCount = ccMath.atoi(tData[a++]);
                DataDT.iBuyPrice = ccMath.atoi(tData[a++]);
                DataDT.iBuyTime = ccMath.atoi(tData[a++]);
                DataDT.iDiscount = ccMath.atoi(tData[a++]);
                DataDT._szDescribe = tData[a++];
                DataDT._szTitile = tData[a++];
                DataDT.szGetWayGoPage = tData[a++];
                DataDT.iIcon = ccMath.atoi(tData[a++]);
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
