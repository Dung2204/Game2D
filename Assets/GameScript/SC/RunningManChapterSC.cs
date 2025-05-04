
//============================================
//
//    RunningManChapter来自RunningManChapter.xlsx文件自动生成脚本
//    2017/7/13 13:36:08
//    
//
//============================================
using System;
using System.Collections.Generic;



public class RunningManChapterSC : NBaseSC
{
    public RunningManChapterSC()
    {
        Create("RunningManChapterDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        RunningManChapterDT DataDT;
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
                DataDT = new RunningManChapterDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT._szName = tData[a++];
                DataDT.szTollgateIds = tData[a++];
                DataDT.szShowMonsters = tData[a++];
                DataDT.iBox3 = ccMath.atoi(tData[a++]);
                DataDT.iBox6 = ccMath.atoi(tData[a++]);
                DataDT.iBox9 = ccMath.atoi(tData[a++]);
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
