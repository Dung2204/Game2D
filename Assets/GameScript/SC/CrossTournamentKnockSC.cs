
//============================================
//
//    CrossTournamentKnock.xlsx
//    
//
//============================================
using System;
using System.Collections.Generic;



public class CrossTournamentKnockSC : NBaseSC
{
    public CrossTournamentKnockSC()
    {
        Create("CrossTournamentKnockDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        CrossTournamentKnockDT DataDT;
        string[] tData;
        string[] tFoddScData = ttt[1].Split(new string[] { "|" }, System.StringSplitOptions.None);
        for (int i = 0; i < tFoddScData.Length; i++)
        {
            try
            {
                if (tFoddScData[i] == "")
                {
                    MessageBox.DEBUG(m_strRegDTName + "Chuỗi có bản ghi trống, " + i);
                    continue;
                }
                tData = tFoddScData[i].Split(new string[] { "@," }, System.StringSplitOptions.None);
                int a = 0;
                DataDT = new CrossTournamentKnockDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
                    MessageBox.ASSERT("Id lỗi");
                }
                DataDT.iTop = ccMath.atoi(tData[a++]);
                DataDT._szName = tData[a++];
                DataDT.szAward = tData[a++];
                SaveItem(DataDT);
            }
            catch
            {
                MessageBox.DEBUG(m_strRegDTName + "Lỗi bản ghi chuỗi, " + i);
                continue;
            }
        }
    }

}
