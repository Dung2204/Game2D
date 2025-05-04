
//============================================
//
//    HalfDiscount来自HalfDiscount.xlsx文件自动生成脚本
//    2017/11/23 19:01:05
//    
//
//============================================
using System;
using System.Collections.Generic;



public class HalfDiscountSC : NBaseSC
{
    public HalfDiscountSC()
    {
        Create("HalfDiscountDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        HalfDiscountDT DataDT;
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
                DataDT = new HalfDiscountDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT._szDesc = tData[a++];
                DataDT.iResType = ccMath.atoi(tData[a++]);
                DataDT.iResId = ccMath.atoi(tData[a++]);
                DataDT.iResNum = ccMath.atoi(tData[a++]);
                DataDT.iBuyNum = ccMath.atoi(tData[a++]);
                DataDT.iCostNum = ccMath.atoi(tData[a++]);
                DataDT.iDiscount = ccMath.atoi(tData[a++]);
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
