
//============================================
//
//    CrossServerBattleShop来自CrossServerBattleShop.xlsx文件自动生成脚本
//    2018/3/29 13:34:47
//    
//
//============================================
using System;
using System.Collections.Generic;



public class CrossServerBattleShopSC : NBaseSC
{
    public CrossServerBattleShopSC()
    {
        Create("CrossServerBattleShopDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        CrossServerBattleShopDT DataDT;
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
                DataDT = new CrossServerBattleShopDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
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
MessageBox.DEBUG(m_strRegDTName + "String record error, " + i);
                continue;
            }
        }
    }

}
