
//============================================
//
//    LegionTollgate来自LegionTollgate.xlsx文件自动生成脚本
//    2018/1/18 14:31:44
//    
//
//============================================
using System;
using System.Collections.Generic;



public class LegionTollgateSC : NBaseSC
{
    public LegionTollgateSC()
    {
        Create("LegionTollgateDT");
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        LegionTollgateDT DataDT;
        string[] tData;
        string[] tFoddScData = ttt[1].Split(new string[] { "|" }, System.StringSplitOptions.None);
        for (int i = 0; i < tFoddScData.Length; i++)
        {
            try
            {
                if (tFoddScData[i] == "")
                {
MessageBox.DEBUG(m_strRegDTName + "Command with empty record, " + i);
                    continue;
                }
                tData = tFoddScData[i].Split(new string[] { "@," }, System.StringSplitOptions.None);
                int a = 0;
                DataDT = new LegionTollgateDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT._szName = tData[a++];
                DataDT.iBoxImage = ccMath.atoi(tData[a++]);
                DataDT.iMonster1 = ccMath.atoi(tData[a++]);
                DataDT.iMonster2 = ccMath.atoi(tData[a++]);
                DataDT.iMonster3 = ccMath.atoi(tData[a++]);
                DataDT.iMonster4 = ccMath.atoi(tData[a++]);
                DataDT.iMonster5 = ccMath.atoi(tData[a++]);
                DataDT.iMonster6 = ccMath.atoi(tData[a++]);
                DataDT.iScene = ccMath.atoi(tData[a++]);
                DataDT.iBuff = ccMath.atoi(tData[a++]);
                DataDT.iContri = ccMath.atoi(tData[a++]);
                DataDT.iKillContri = ccMath.atoi(tData[a++]);
                DataDT.iFiniExp = ccMath.atoi(tData[a++]);
                SaveItem(DataDT);
            }
            catch
            {
MessageBox.DEBUG(m_strRegDTName + "There was an error in command record, " + i);
                continue;
            }
        }
    }

}
