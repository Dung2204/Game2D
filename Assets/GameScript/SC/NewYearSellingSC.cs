
//============================================
//
//    NewYearSelling来自NewYearSelling.xlsx文件自动生成脚本
//    2018/1/18 20:40:31
//    
//
//============================================
using System;
using System.Collections.Generic;



public class NewYearSellingSC : NBaseSC
{
    public NewYearSellingSC()
    {
        Create("NewYearSellingDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        NewYearSellingDT DataDT;
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
                DataDT = new NewYearSellingDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iRankDownLimit = ccMath.atoi(tData[a++]);
                DataDT.iSellId = ccMath.atoi(tData[a++]);
                DataDT.iRescId = ccMath.atoi(tData[a++]);
                DataDT.iCount = ccMath.atoi(tData[a++]);
                DataDT.iDiscountPer = ccMath.atoi(tData[a++]);
                DataDT.iDiscountPrice = ccMath.atoi(tData[a++]);
                DataDT.iBuyCount = ccMath.atoi(tData[a++]);
                DataDT.iBuyTime = ccMath.atoi(tData[a++]);
                DataDT.iStarTime = ccMath.atoi(tData[a++]);
                DataDT.iEndTime = ccMath.atoi(tData[a++]);
                DataDT.iSign = ccMath.atoi(tData[a++]);
                SaveItem(DataDT);
            }
            catch
            {
MessageBox.DEBUG(m_strRegDTName + "Record string error, " + i);
                continue;
            }
        }
    }

}
