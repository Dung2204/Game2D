
//============================================
//
//    GodDressRankAward来自GodDressRankAward.xlsx文件自动生成脚本
//    2018/4/11 15:37:11
//    
//
//============================================
using System;
using System.Collections.Generic;



public class GodDressRankAwardSC : NBaseSC
{
    public GodDressRankAwardSC()
    {
        Create("GodDressRankAwardDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        GodDressRankAwardDT DataDT;
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
                DataDT = new GodDressRankAwardDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iRankBeg1 = ccMath.atoi(tData[a++]);
                DataDT.iRankEnd1 = ccMath.atoi(tData[a++]);
                DataDT.szAward1 = tData[a++];
                DataDT.iRankBeg2 = ccMath.atoi(tData[a++]);
                DataDT.iRankEnd2 = ccMath.atoi(tData[a++]);
                DataDT.szAward2 = tData[a++];
                DataDT.iRankBeg3 = ccMath.atoi(tData[a++]);
                DataDT.iRankEnd3 = ccMath.atoi(tData[a++]);
                DataDT.szAward3 = tData[a++];
                DataDT.iRankBeg4 = ccMath.atoi(tData[a++]);
                DataDT.iRankEnd4 = ccMath.atoi(tData[a++]);
                DataDT.szAward4 = tData[a++];
                DataDT.iRankBeg5 = ccMath.atoi(tData[a++]);
                DataDT.iRankEnd5 = ccMath.atoi(tData[a++]);
                DataDT.szAward5 = tData[a++];
                DataDT.iRankBeg6 = ccMath.atoi(tData[a++]);
                DataDT.iRankEnd6 = ccMath.atoi(tData[a++]);
                DataDT.szAward6 = tData[a++];
                DataDT.iRankBeg7 = ccMath.atoi(tData[a++]);
                DataDT.iRankEnd7 = ccMath.atoi(tData[a++]);
                DataDT.szAward7 = tData[a++];
                DataDT.iRankBeg8 = ccMath.atoi(tData[a++]);
                DataDT.iRankEnd8 = ccMath.atoi(tData[a++]);
                DataDT.szAward8 = tData[a++];
                DataDT.iRankBeg9 = ccMath.atoi(tData[a++]);
                DataDT.iRankEnd9 = ccMath.atoi(tData[a++]);
                DataDT.szAward9 = tData[a++];
                DataDT.iRankBeg10 = ccMath.atoi(tData[a++]);
                DataDT.iRankEnd10 = ccMath.atoi(tData[a++]);
                DataDT.szAward10 = tData[a++];
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
