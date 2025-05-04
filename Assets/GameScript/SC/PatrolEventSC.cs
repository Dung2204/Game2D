
//============================================
//
//    PatrolEvent来自PatrolEvent.xlsx文件自动生成脚本
//    2017/9/6 10:37:54
//    
//
//============================================
using System;
using System.Collections.Generic;



public class PatrolEventSC : NBaseSC
{
    public PatrolEventSC()
    {
        Create("PatrolEventDT");
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        PatrolEventDT DataDT;
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
                DataDT = new PatrolEventDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT._szDes = tData[a++];
                DataDT.iAwardId = ccMath.atoi(tData[a++]);
                DataDT.iDoubleOdds = ccMath.atoi(tData[a++]);
                DataDT.iLand1 = ccMath.atoi(tData[a++]);
                DataDT.iLand2 = ccMath.atoi(tData[a++]);
                DataDT.iLand3 = ccMath.atoi(tData[a++]);
                DataDT.iLand4 = ccMath.atoi(tData[a++]);
                DataDT.iLand5 = ccMath.atoi(tData[a++]);
                DataDT.iLand6 = ccMath.atoi(tData[a++]);
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
