
//============================================
//
//    DailyPveInfo来自DailyPveInfo.xlsx文件自动生成脚本
//    2017/11/24 15:21:01
//    
//
//============================================
using System;
using System.Collections.Generic;



public class DailyPveInfoSC : NBaseSC
{
    public DailyPveInfoSC()
    {
        Create("DailyPveInfoDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        DailyPveInfoDT DataDT;
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
                DataDT = new DailyPveInfoDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.szName = tData[a++];
                DataDT.szIcon = tData[a++];
                DataDT.iRoleIcon = ccMath.atoi(tData[a++]);
                DataDT.iOpen0 = ccMath.atoi(tData[a++]);
                DataDT.iOpen1 = ccMath.atoi(tData[a++]);
                DataDT.iOpen2 = ccMath.atoi(tData[a++]);
                DataDT.iOpen3 = ccMath.atoi(tData[a++]);
                DataDT.iOpen4 = ccMath.atoi(tData[a++]);
                DataDT.iOpen5 = ccMath.atoi(tData[a++]);
                DataDT.iOpen6 = ccMath.atoi(tData[a++]);
                DataDT.szScene = tData[a++];
                DataDT.iTiLi = ccMath.atoi(tData[a++]);
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
