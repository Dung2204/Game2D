
//============================================
//
//    LegionTollgateBox来自LegionTollgateBox.xlsx文件自动生成脚本
//    2017/6/2 15:58:35
//    
//
//============================================
using System;
using System.Collections.Generic;



public class LegionTollgateBoxSC : NBaseSC
{
    public LegionTollgateBoxSC()
    {
        Create("LegionTollgateBoxDT");
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        LegionTollgateBoxDT DataDT;
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
                DataDT = new LegionTollgateBoxDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iAwardType1 = ccMath.atoi(tData[a++]);
                DataDT.iAwardId1 = ccMath.atoi(tData[a++]);
                DataDT.iAwardNum1 = ccMath.atoi(tData[a++]);
                DataDT.iGenCount1 = ccMath.atoi(tData[a++]);
                DataDT.iAwardType2 = ccMath.atoi(tData[a++]);
                DataDT.iAwardId2 = ccMath.atoi(tData[a++]);
                DataDT.iAwardNum2 = ccMath.atoi(tData[a++]);
                DataDT.iGenCount2 = ccMath.atoi(tData[a++]);
                DataDT.iAwardType3 = ccMath.atoi(tData[a++]);
                DataDT.iAwardId3 = ccMath.atoi(tData[a++]);
                DataDT.iAwardNum3 = ccMath.atoi(tData[a++]);
                DataDT.iGenCount3 = ccMath.atoi(tData[a++]);
                DataDT.iAwardType4 = ccMath.atoi(tData[a++]);
                DataDT.iAwardId4 = ccMath.atoi(tData[a++]);
                DataDT.iAwardNum4 = ccMath.atoi(tData[a++]);
                DataDT.iGenCount4 = ccMath.atoi(tData[a++]);
                DataDT.iAwardType5 = ccMath.atoi(tData[a++]);
                DataDT.iAwardId5 = ccMath.atoi(tData[a++]);
                DataDT.iAwardNum5 = ccMath.atoi(tData[a++]);
                DataDT.iGenCount5 = ccMath.atoi(tData[a++]);
                DataDT.iAwardType6 = ccMath.atoi(tData[a++]);
                DataDT.iAwardId6 = ccMath.atoi(tData[a++]);
                DataDT.iAwardNum6 = ccMath.atoi(tData[a++]);
                DataDT.iGenCount6 = ccMath.atoi(tData[a++]);
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
