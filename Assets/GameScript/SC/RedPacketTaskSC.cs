
//============================================
//
//    RedPacketTask来自RedPacketTask.xlsx文件自动生成脚本
//    2018/3/9 15:25:18
//    
//
//============================================
using System;
using System.Collections.Generic;



public class RedPacketTaskSC : NBaseSC
{
    public RedPacketTaskSC()
    {
        Create("RedPacketTaskDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        RedPacketTaskDT DataDT;
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
                DataDT = new RedPacketTaskDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT._szDesc = tData[a++];
                DataDT.iTaskType = ccMath.atoi(tData[a++]);
                DataDT.iConditonParam = ccMath.atoi(tData[a++]);
                DataDT.szAward1 = tData[a++];
                DataDT.szAward2 = tData[a++];
                DataDT.szAward3 = tData[a++];
                DataDT.szUIName = tData[a++];
                DataDT.iUIParam = ccMath.atoi(tData[a++]);
                DataDT.iTimeBegin = ccMath.atoi(tData[a++]);
                DataDT.iTimeEnd = ccMath.atoi(tData[a++]);
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
