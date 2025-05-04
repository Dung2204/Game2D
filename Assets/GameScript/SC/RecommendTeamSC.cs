
//============================================
//
//    RecommendTeam来自RecommendTeam.xlsx文件自动生成脚本
//    2018/3/1 16:33:02
//    
//
//============================================
using System;
using System.Collections.Generic;



public class RecommendTeamSC : NBaseSC
{
    public RecommendTeamSC()
    {
        Create("RecommendTeamDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        RecommendTeamDT DataDT;
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
                DataDT = new RecommendTeamDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iCardCamp = ccMath.atoi(tData[a++]);
                DataDT._szName = tData[a++];
                DataDT.iPos1 = ccMath.atoi(tData[a++]);
                DataDT.iPos2 = ccMath.atoi(tData[a++]);
                DataDT.iPos3 = ccMath.atoi(tData[a++]);
                DataDT.iPos4 = ccMath.atoi(tData[a++]);
                DataDT.iPos5 = ccMath.atoi(tData[a++]);
                DataDT.iPos6 = ccMath.atoi(tData[a++]);
                DataDT._szDesc = tData[a++];
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
