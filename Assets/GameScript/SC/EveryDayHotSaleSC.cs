
//============================================
//
//    EveryDayHotSale来自EveryDayHotSale.xlsx文件自动生成脚本
//    2017/11/20 20:07:20
//    
//
//============================================
using System;
using System.Collections.Generic;



public class EveryDayHotSaleSC : NBaseSC
{
    public EveryDayHotSaleSC()
    {
        Create("EveryDayHotSaleDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        EveryDayHotSaleDT DataDT;
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
                DataDT = new EveryDayHotSaleDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iDayNum = ccMath.atoi(tData[a++]);
                DataDT.iConsumeType = ccMath.atoi(tData[a++]);
                DataDT.iConsumeNum = ccMath.atoi(tData[a++]);
                DataDT.iGoodsType = ccMath.atoi(tData[a++]);
                DataDT.iGoodsId = ccMath.atoi(tData[a++]);
                DataDT.iGoodsNum = ccMath.atoi(tData[a++]);
                DataDT.iBuyMax = ccMath.atoi(tData[a++]);
                DataDT.iDiscount = ccMath.atoi(tData[a++]);
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
