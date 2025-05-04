
//============================================
//
//    RunningManElite来自RunningManElite.xlsx文件自动生成脚本
//    2017/7/19 10:18:01
//    
//
//============================================
using System;
using System.Collections.Generic;



public class RunningManEliteSC : NBaseSC
{
    public RunningManEliteSC()
    {
        Create("RunningManEliteDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        RunningManEliteDT DataDT;
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
                DataDT = new RunningManEliteDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.szName = tData[a++];
                DataDT.iAwardFirst = ccMath.atoi(tData[a++]);
                DataDT.szAward = tData[a++];
                DataDT.iShowMonster = ccMath.atoi(tData[a++]);
                DataDT.iMonster1 = ccMath.atoi(tData[a++]);
                DataDT.iMonster2 = ccMath.atoi(tData[a++]);
                DataDT.iMonster3 = ccMath.atoi(tData[a++]);
                DataDT.iMonster4 = ccMath.atoi(tData[a++]);
                DataDT.iMonster5 = ccMath.atoi(tData[a++]);
                DataDT.iMonster6 = ccMath.atoi(tData[a++]);
                DataDT.iSceneId = ccMath.atoi(tData[a++]);
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
