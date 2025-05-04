
//============================================
//
//    ActLoginGift来自ActLoginGift.xlsx文件自动生成脚本
//    2017/11/14 15:15:17
//    
//
//============================================
using System;
using System.Collections.Generic;



public class ActLoginGiftSC : NBaseSC
{
    public ActLoginGiftSC()
    {
        Create("ActLoginGiftDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        ActLoginGiftDT DataDT;
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
                DataDT = new ActLoginGiftDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.itype = ccMath.atoi(tData[a++]);
                DataDT.iday = ccMath.atoi(tData[a++]);
                DataDT._szName = tData[a++];
                DataDT.iCondition = ccMath.atoi(tData[a++]); //TsuCode - điều kiện
                DataDT.szStartTime = tData[a++];
                DataDT.szEndTime = tData[a++];
                DataDT.iType1 = ccMath.atoi(tData[a++]);
                DataDT.iID1 = ccMath.atoi(tData[a++]);
                DataDT.iCount1 = ccMath.atoi(tData[a++]);
                DataDT.iType2 = ccMath.atoi(tData[a++]);
                DataDT.iID2 = ccMath.atoi(tData[a++]);
                DataDT.iCount2 = ccMath.atoi(tData[a++]);
                DataDT.iType3 = ccMath.atoi(tData[a++]);
                DataDT.iID3 = ccMath.atoi(tData[a++]);
                DataDT.iCount3 = ccMath.atoi(tData[a++]);
                DataDT.iType4 = ccMath.atoi(tData[a++]);
                DataDT.iID4 = ccMath.atoi(tData[a++]);
                DataDT.iCount4 = ccMath.atoi(tData[a++]);
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
