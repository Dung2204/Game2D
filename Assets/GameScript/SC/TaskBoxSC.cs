
//============================================
//
//    TaskBox来自TaskBox.xlsx文件自动生成脚本
//    2017/3/31 19:13:09
//    
//
//============================================
using System;
using System.Collections.Generic;



public class TaskBoxSC : NBaseSC
{
    public TaskBoxSC()
    {
        Create("TaskBoxDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        TaskBoxDT DataDT;
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
                DataDT = new TaskBoxDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iOpenLv = ccMath.atoi(tData[a++]);
                DataDT.iCloseLv = ccMath.atoi(tData[a++]);
                DataDT.iBox1Id = ccMath.atoi(tData[a++]);
                DataDT.iScore1 = ccMath.atoi(tData[a++]);
                DataDT.iBox2Id = ccMath.atoi(tData[a++]);
                DataDT.iScore2 = ccMath.atoi(tData[a++]);
                DataDT.iBox3Id = ccMath.atoi(tData[a++]);
                DataDT.iScore3 = ccMath.atoi(tData[a++]);
                DataDT.iBox4Id = ccMath.atoi(tData[a++]);
                DataDT.iScore4 = ccMath.atoi(tData[a++]);
                DataDT.iBox5Id = ccMath.atoi(tData[a++]);
                DataDT.iScore5 = ccMath.atoi(tData[a++]);
                DataDT.iScoreMax = ccMath.atoi(tData[a++]);
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
