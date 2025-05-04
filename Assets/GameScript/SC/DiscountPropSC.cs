
//============================================
//
//    DiscountProp来自DiscountProp.xlsx文件自动生成脚本
//    2017/6/14 14:49:56
//    
//
//============================================
using System;
using System.Collections.Generic;



public class DiscountPropSC : NBaseSC
{
    public DiscountPropSC()
    {
        Create("DiscountPropDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        DiscountPropDT DataDT;
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
                DataDT = new DiscountPropDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.szBeginTime = tData[a++];
                DataDT.szEndTime = tData[a++];
                DataDT.iRankDownLimit = ccMath.atoi(tData[a++]);
                DataDT.iRankUpLimit = ccMath.atoi(tData[a++]);
                DataDT.iRefreshTime = ccMath.atoi(tData[a++]);
                DataDT.iWeight = ccMath.atoi(tData[a++]);
                DataDT.iSellId = ccMath.atoi(tData[a++]);
                DataDT.iRescId = ccMath.atoi(tData[a++]);
                DataDT.iCount = ccMath.atoi(tData[a++]);
                DataDT.iResType = ccMath.atoi(tData[a++]);
                DataDT.iResID = ccMath.atoi(tData[a++]);
                DataDT.iPrimeNum = ccMath.atoi(tData[a++]);
                DataDT.iDiscountPer = ccMath.atoi(tData[a++]);
                DataDT.iDiscountPrice = ccMath.atoi(tData[a++]);
                DataDT.iPrivProc = ccMath.atoi(tData[a++]);
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
