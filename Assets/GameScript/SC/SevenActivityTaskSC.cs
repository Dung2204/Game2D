
//============================================
//
//    SevenActivityTask来自SevenActivityTask.xlsx文件自动生成脚本
//    2017/11/30 15:41:52
//    
//
//============================================
using System;
using System.Collections.Generic;



public class SevenActivityTaskSC : NBaseSC
{
    public SevenActivityTaskSC()
    {
        Create("SevenActivityTaskDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        SevenActivityTaskDT DataDT;
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
                DataDT = new SevenActivityTaskDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iDayNum = ccMath.atoi(tData[a++]);
                DataDT._szDesc = tData[a++];
                DataDT.iPage = ccMath.atoi(tData[a++]);
                DataDT._szPageName = tData[a++];
                DataDT.itype = ccMath.atoi(tData[a++]);
                DataDT._szDonditionDesc = tData[a++];
                DataDT.iCondition1 = ccMath.atoi(tData[a++]);
                DataDT.iCondition2 = ccMath.atoi(tData[a++]);
                DataDT.iAwardType1 = ccMath.atoi(tData[a++]);
                DataDT.iAwardId1 = ccMath.atoi(tData[a++]);
                DataDT.iAwardNum1 = ccMath.atoi(tData[a++]);
                DataDT.iAwardType2 = ccMath.atoi(tData[a++]);
                DataDT.iAwardId2 = ccMath.atoi(tData[a++]);
                DataDT.iAwardNum2 = ccMath.atoi(tData[a++]);
                DataDT.iAwardType3 = ccMath.atoi(tData[a++]);
                DataDT.iAwardId3 = ccMath.atoi(tData[a++]);
                DataDT.iAwardNum3 = ccMath.atoi(tData[a++]);
                DataDT.szUIName = tData[a++];
                DataDT.iParam = ccMath.atoi(tData[a++]);
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
