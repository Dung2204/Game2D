
//============================================
//
//    GodDress来自GodDress.xlsx文件自动生成脚本
//    2018/4/11 15:30:38
//    
//
//============================================
using System;
using System.Collections.Generic;



public class GodDressSC : NBaseSC
{
    public GodDressSC()
    {
        Create("GodDressDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        GodDressDT DataDT;
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
                DataDT = new GodDressDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iBeginTime = ccMath.atoi(tData[a++]);
                DataDT.iEndTime = ccMath.atoi(tData[a++]);
                DataDT.szAward = tData[a++];
                DataDT.iBuyAward = ccMath.atoi(tData[a++]);
                DataDT.iBuyTimes = ccMath.atoi(tData[a++]);
                DataDT.iOnePrice = ccMath.atoi(tData[a++]);
                DataDT.iTenPrice = ccMath.atoi(tData[a++]);
                DataDT.iRankAwardID = ccMath.atoi(tData[a++]);
                DataDT.iBoxAwardID = ccMath.atoi(tData[a++]);
                DataDT.szTheme = tData[a++];
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
