
//============================================
//
//    TaskDaily来自TaskDaily.xlsx文件自动生成脚本
//    2017/11/23 19:04:43
//    
//
//============================================
using System;
using System.Collections.Generic;



public class TaskDailySC : NBaseSC
{
    public TaskDailySC()
    {
        Create("TaskDailyDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        TaskDailyDT DataDT;
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
                DataDT = new TaskDailyDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iTaskType = ccMath.atoi(tData[a++]);
                DataDT.iOpenLv = ccMath.atoi(tData[a++]);
                DataDT.iCloseLv = ccMath.atoi(tData[a++]);
                DataDT.iIconId = ccMath.atoi(tData[a++]);
                DataDT._szDesc = tData[a++];
                DataDT.iCondition = ccMath.atoi(tData[a++]);
                DataDT.iConditionParam = ccMath.atoi(tData[a++]);
                DataDT.iGotoId = ccMath.atoi(tData[a++]);
                DataDT.iAwardMoney = ccMath.atoi(tData[a++]);
                DataDT.iAwardId1 = ccMath.atoi(tData[a++]);
                DataDT.iAwardId2 = ccMath.atoi(tData[a++]);
                DataDT.iAwardId3 = ccMath.atoi(tData[a++]);
                DataDT.iAwardId4 = ccMath.atoi(tData[a++]);
                DataDT.iAwardId5 = ccMath.atoi(tData[a++]);
                DataDT.iAwardId6 = ccMath.atoi(tData[a++]);
                DataDT.iAwardId7 = ccMath.atoi(tData[a++]);
                DataDT.iAwardId8 = ccMath.atoi(tData[a++]);
                DataDT.iAwardId9 = ccMath.atoi(tData[a++]);
                DataDT.iAwardId10 = ccMath.atoi(tData[a++]);
                DataDT.iAwardId11 = ccMath.atoi(tData[a++]);
                DataDT.iScore1 = ccMath.atoi(tData[a++]);
                DataDT.iScore2 = ccMath.atoi(tData[a++]);
                DataDT.iScore3 = ccMath.atoi(tData[a++]);
                DataDT.iScore4 = ccMath.atoi(tData[a++]);
                DataDT.iScore5 = ccMath.atoi(tData[a++]);
                DataDT.iScore6 = ccMath.atoi(tData[a++]);
                DataDT.iScore7 = ccMath.atoi(tData[a++]);
                DataDT.iScore8 = ccMath.atoi(tData[a++]);
                DataDT.iScore9 = ccMath.atoi(tData[a++]);
                DataDT.iScore10 = ccMath.atoi(tData[a++]);
                DataDT.iScore11 = ccMath.atoi(tData[a++]);
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
