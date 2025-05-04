
//============================================
//    MacoCode
//    ElementalSeason.xlsx  mùa nguyên tố
//    
//
//============================================
using System;
using System.Collections.Generic;



public class ElementalSeasonSC : NBaseSC
{
    public ElementalSeasonSC()
    {
        Create("ElementalSeasonDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        ElementalSeasonDT DataDT;
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
                DataDT = new ElementalSeasonDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                DataDT.iTimeDefault = ccMath.atoi(tData[a++]);
                DataDT.iDay = ccMath.atoi(tData[a++]);
                DataDT.iAddHp = ccMath.atoi(tData[a++]);
                DataDT.iAddAtk = ccMath.atoi(tData[a++]);
                DataDT.iAddDef = ccMath.atoi(tData[a++]);
                DataDT.iAddMDef = ccMath.atoi(tData[a++]);
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
