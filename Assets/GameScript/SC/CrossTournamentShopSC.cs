
//============================================
//
//    ArenaRankAward来自ArenaRankAward.xlsx文件自动生成脚本
//    2017/5/4 13:24:23
//    
//
//============================================
using System;
using System.Collections.Generic;



public class CrossTournamentShopSC : NBaseSC
{
    public CrossTournamentShopSC()
    {
        Create("CrossTournamentShopDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        CrossTournamentShopDT DataDT;
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
                DataDT = new CrossTournamentShopDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
                    MessageBox.ASSERT("Id lỗi");
                }
                DataDT.iShowLv = ccMath.atoi(tData[a++]);
                DataDT.iResourceType = ccMath.atoi(tData[a++]);
                DataDT.iResourceId = ccMath.atoi(tData[a++]);
                DataDT.iResourceNum = ccMath.atoi(tData[a++]);
                DataDT.iShowIdx = ccMath.atoi(tData[a++]);
                DataDT.iBuyTimesLimit = ccMath.atoi(tData[a++]);
                DataDT.iNeedScore = ccMath.atoi(tData[a++]);
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
