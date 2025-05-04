
//============================================
//
//    TurntableLottery来自TurntableLottery.xlsx文件自动生成脚本
//    2018/5/22 17:16:38
//    
//
//============================================
using System;
using System.Collections.Generic;



public class TurntableLotterySC : NBaseSC
{
    public TurntableLotterySC()
    {
        Create("TurntableLotteryDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        TurntableLotteryDT DataDT;
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
                DataDT = new TurntableLotteryDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iAwardType = ccMath.atoi(tData[a++]);
                DataDT.iAwardId = ccMath.atoi(tData[a++]);
                DataDT.iAwardNum = ccMath.atoi(tData[a++]);
                DataDT.iOpenTimes = ccMath.atoi(tData[a++]);
                DataDT.ibClear = ccMath.atoi(tData[a++]);
                DataDT.iWeight = ccMath.atoi(tData[a++]);
                DataDT.ibBoardcast = ccMath.atoi(tData[a++]);
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
