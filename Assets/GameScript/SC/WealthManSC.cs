
//============================================
//
//    WealthMan来自WealthMan.xlsx文件自动生成脚本
//    2017/4/20 14:07:51
//    
//
//============================================
using System;
using System.Collections.Generic;



public class WealthManSC : NBaseSC
{
    public WealthManSC()
    {
        Create("WealthManDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        WealthManDT DataDT;
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
                DataDT = new WealthManDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iOneRewardType = ccMath.atoi(tData[a++]);
                DataDT.iOneRewardId = ccMath.atoi(tData[a++]);
                DataDT.iOneRewardCount = ccMath.atoi(tData[a++]);
                DataDT.iTotalRewardType = ccMath.atoi(tData[a++]);
                DataDT.iTotalRewardId = ccMath.atoi(tData[a++]);
                DataDT.iTotalRewardCount = ccMath.atoi(tData[a++]);
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
