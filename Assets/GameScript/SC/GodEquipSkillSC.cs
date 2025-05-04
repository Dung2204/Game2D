
//============================================
//
//    Equip来自Equip.xlsx文件自动生成脚本
//    2017/3/23 19:12:03
//    
//
//============================================
using System;
using System.Collections.Generic;



public class GodEquipSkillSC : NBaseSC
{
    public GodEquipSkillSC()
    {
        Create("GodEquipSkillDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        GodEquipSkillDT DataDT;
        string[] tData;
        string[] tFoddScData = ttt[1].Split(new string[] { "|" }, System.StringSplitOptions.None);
        for (int i = 0; i < tFoddScData.Length; i++)
        {
            try
            {
                if (tFoddScData[i] == "")
                {
                    MessageBox.DEBUG(m_strRegDTName + "脚本存在空记录, " + i);
                    continue;
                }
                tData = tFoddScData[i].Split(new string[] { "@," }, System.StringSplitOptions.None);
                int a = 0;
                DataDT = new GodEquipSkillDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
                    MessageBox.ASSERT("Id错误");
                }
                DataDT.iBefore = ccMath.atoi(tData[a++]);
                DataDT.iActive = ccMath.atoi(tData[a++]);
                DataDT.iType = ccMath.atoi(tData[a++]);
                DataDT.iExtRand1 = ccMath.atoi(tData[a++]);
                DataDT.iCase = ccMath.atoi(tData[a++]);
                DataDT.iParam = ccMath.atoi(tData[a++]);
                DataDT.iMagicId = ccMath.atoi(tData[a++]);
                SaveItem(DataDT);
            }
            catch
            {
                MessageBox.DEBUG(m_strRegDTName + "脚本记录存在错误, " + i);
                continue;
            }
        }
    }

}
