
//============================================
//
//    DiscountAllServ来自DiscountAllServ.xlsx文件自动生成脚本
//    2017/6/20 10:15:26
//    
//
//============================================
using System;
using System.Collections.Generic;



public class DiscountAllServSC : NBaseSC
{
    public DiscountAllServSC()
    {
        Create("DiscountAllServDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        DiscountAllServDT DataDT;
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
                DataDT = new DiscountAllServDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iType = ccMath.atoi(tData[a++]);
                DataDT.iResID = ccMath.atoi(tData[a++]);
                DataDT.iCount = ccMath.atoi(tData[a++]);
                DataDT.iAllowGetNum = ccMath.atoi(tData[a++]);
                DataDT.iGetLimit = ccMath.atoi(tData[a++]);
                DataDT.szDescript = tData[a++];
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
