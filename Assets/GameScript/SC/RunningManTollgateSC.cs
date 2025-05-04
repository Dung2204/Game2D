
//============================================
//
//    RunningManTollgate来自RunningManTollgate.xlsx文件自动生成脚本
//    2017/7/13 13:46:00
//    
//
//============================================
using System;
using System.Collections.Generic;



public class RunningManTollgateSC : NBaseSC
{
    public RunningManTollgateSC()
    {
        Create("RunningManTollgateDT");
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        RunningManTollgateDT DataDT;
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
                DataDT = new RunningManTollgateDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.szName = tData[a++];
                DataDT.iMonster1 = ccMath.atoi(tData[a++]);
                DataDT.iMonster2 = ccMath.atoi(tData[a++]);
                DataDT.iMonster3 = ccMath.atoi(tData[a++]);
                DataDT.iMonster4 = ccMath.atoi(tData[a++]);
                DataDT.iMonster5 = ccMath.atoi(tData[a++]);
                DataDT.iMonster6 = ccMath.atoi(tData[a++]);
                DataDT.iPassType = ccMath.atoi(tData[a++]);
                DataDT.szPassParams = tData[a++];
                DataDT.szMoneys = tData[a++];
                DataDT.szPrests = tData[a++];
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
