
//============================================
//
//    EquipUpStar来自EquipUpStar.xlsx文件自动生成脚本
//    2017/3/22 19:57:04
//    
//
//============================================
using System;
using System.Collections.Generic;



public class GodEquipUpStarSC : NBaseSC
{
    public GodEquipUpStarSC()
    {
        Create("GodEquipUpStarDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        GodEquipUpStarDT DataDT;
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
                DataDT = new GodEquipUpStarDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
                    MessageBox.ASSERT("Id lỗi");
                }
                DataDT.iEquipId = ccMath.atoi(tData[a++]);
                DataDT.szEquipName = tData[a++];
                DataDT.iUpLv = ccMath.atoi(tData[a++]);
                DataDT.iUpExp = ccMath.atoi(tData[a++]);
                DataDT.iBasicExp = ccMath.atoi(tData[a++]);
                DataDT.iCriticalHitsOdds = ccMath.atoi(tData[a++]);
                DataDT.iCriticalHitsResult = ccMath.atoi(tData[a++]);
                DataDT.iInitial = ccMath.atoi(tData[a++]);
                DataDT.iSilverNum1 = ccMath.atoi(tData[a++]);
                DataDT.iGoldNum2 = ccMath.atoi(tData[a++]);
                DataDT.iDebrisId = ccMath.atoi(tData[a++]);
                DataDT.iDebrisNum = ccMath.atoi(tData[a++]);
                DataDT.iProId = ccMath.atoi(tData[a++]);
                DataDT.iAddPro = ccMath.atoi(tData[a++]);
                DataDT.iAddNum = ccMath.atoi(tData[a++]);
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
