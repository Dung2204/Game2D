
//============================================
//
//    EquipFragments来自EquipFragments.xlsx文件自动生成脚本
//    2017/3/23 14:03:57
//    
//
//============================================
using System;
using System.Collections.Generic;



public class GodEquipFragmentsSC : NBaseSC
{
    public GodEquipFragmentsSC()
    {
        Create("GodEquipFragmentsDT");
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        GodEquipFragmentsDT DataDT;
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
                DataDT = new GodEquipFragmentsDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
                    MessageBox.ASSERT("Id lỗi");
                }
                DataDT.szName = tData[a++];
                DataDT.iIcon = ccMath.atoi(tData[a++]);
                DataDT.iColour = ccMath.atoi(tData[a++]);
                DataDT.szDescribe = tData[a++];
                DataDT.iPileNum = ccMath.atoi(tData[a++]);
                DataDT.iList = ccMath.atoi(tData[a++]);
                DataDT.iBondNum = ccMath.atoi(tData[a++]);
                DataDT.iGoldType = ccMath.atoi(tData[a++]);
                DataDT.iGoldNum = ccMath.atoi(tData[a++]);
                DataDT.iDstEquipId = ccMath.atoi(tData[a++]);
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
