
//============================================
//
//    GetWay来自GetWay.xlsx文件自动生成脚本
//    2017/8/24 10:53:07
//    
//
//============================================
using System;
using System.Collections.Generic;



public class GetWaySC : NBaseSC
{
    public GetWaySC()
    {
        Create("GetWayDT");
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        GetWayDT DataDT;
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
                DataDT = new GetWayDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iResType = ccMath.atoi(tData[a++]);
                DataDT._szResName = tData[a++];
                DataDT.szGetWayGoPage1 = tData[a++];
                DataDT.iParam1 = ccMath.atoi(tData[a++]);
                DataDT._szGetWayName1 = tData[a++];
                DataDT._szGetWayDesc1 = tData[a++];
                DataDT.szGetWayGoPage2 = tData[a++];
                DataDT.iParam2 = ccMath.atoi(tData[a++]);
                DataDT.szGetWayName2 = tData[a++];
                DataDT._szGetWayDesc2 = tData[a++];
                DataDT.szGetWayGoPage3 = tData[a++];
                DataDT.iParam3 = ccMath.atoi(tData[a++]);
                DataDT.szGetWayName3 = tData[a++];
                DataDT.szGetWayDesc3 = tData[a++];
                DataDT.szGetWayGoPage4 = tData[a++];
                DataDT.iParam4 = ccMath.atoi(tData[a++]);
                DataDT.szGetWayName4 = tData[a++];
                DataDT.szGetWayDesc4 = tData[a++];
                DataDT.szGetWayGoPage5 = tData[a++];
                DataDT.iParam5 = ccMath.atoi(tData[a++]);
                DataDT.szGetWayName5 = tData[a++];
                DataDT.szGetWayDesc5 = tData[a++];
                DataDT.szRemarks = tData[a++];
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
