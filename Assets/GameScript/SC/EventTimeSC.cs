
//============================================
//
//    SevenActivityTask来自SevenActivityTask.xlsx文件自动生成脚本
//    2017/11/30 15:41:52
//    
//
//============================================
using System;
using System.Collections.Generic;



public class EventTimeSC : NBaseSC
{
    public EventTimeSC()
    {
        Create("EventTimeDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        EventTimeDT DataDT;
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
                DataDT = new EventTimeDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.szSystemId = tData[a++];
                DataDT.szIcon = tData[a++];
                DataDT.iType = ccMath.atoi(tData[a++]);
                DataDT.iOpenTime = ccMath.atoi(tData[a++]);
                DataDT.iEndTime = ccMath.atoi(tData[a++]);
                DataDT.iCloseTime = ccMath.atoi(tData[a++]);
                DataDT.iPosition = ccMath.atoi(tData[a++]);
                DataDT.iLevel = ccMath.atoi(tData[a++]);
                DataDT.szNameConst = tData[a++];
                DataDT.iHold = ccMath.atoi(tData[a++]);

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
