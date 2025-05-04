
//============================================
//
//    CrossServerBattleZone来自CrossServerBattleZone.xlsx文件自动生成脚本
//    2018/3/22 19:30:02
//    
//
//============================================
using System;
using System.Collections.Generic;



public class ChaosBattleZoneSC : NBaseSC
{
    public ChaosBattleZoneSC()
    {
        Create("ChaosBattleZoneDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        ChaosBattleZoneDT DataDT;
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
                DataDT = new ChaosBattleZoneDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT._szName = tData[a++];
                DataDT.iBeginPower = ccMath.atoi(tData[a++]);
                DataDT.iEndPower = ccMath.atoi(tData[a++]);
                DataDT.iWinAward = ccMath.atoi(tData[a++]);
                DataDT.iLoseAward = ccMath.atoi(tData[a++]);
                DataDT.iWinSteakAward = ccMath.atoi(tData[a++]);
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
