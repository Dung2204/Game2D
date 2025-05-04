
//============================================
//
//    Equip来自Equip.xlsx文件自动生成脚本
//    2017/3/23 19:12:03
//    
//
//============================================
using System;
using System.Collections.Generic;
using System.Linq;

public class GodEquipSC : NBaseSC
{
    public GodEquipSC()
    {
        Create("GodEquipDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        GodEquipDT DataDT;
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
                DataDT = new GodEquipDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
                    MessageBox.ASSERT("Id lỗi");
                }
                DataDT.szName = tData[a++];
                DataDT.iIcon = ccMath.atoi(tData[a++]);
                DataDT.iColour = ccMath.atoi(tData[a++]);
                DataDT.szDescribe = tData[a++];
                DataDT.iSite = ccMath.atoi(tData[a++]);
                DataDT.iGoldId = ccMath.atoi(tData[a++]);
                DataDT.iGoldNum = ccMath.atoi(tData[a++]);
                DataDT.iIntenProId = ccMath.atoi(tData[a++]);
                DataDT.iStartPro = ccMath.atoi(tData[a++]);
                DataDT.iAddPro = ccMath.atoi(tData[a++]);
                DataDT.iRefinProId1 = ccMath.atoi(tData[a++]);
                DataDT.iRefinPro1 = ccMath.atoi(tData[a++]);
                DataDT.iRefinProId2 = ccMath.atoi(tData[a++]);
                DataDT.iRefinPro2 = ccMath.atoi(tData[a++]);
                DataDT.idSkillGod = ccMath.atoi(tData[a++]);
                SaveItem(DataDT);
            }
            catch
            {
                MessageBox.DEBUG(m_strRegDTName + "Lỗi bản ghi chuỗi, " + i);
                continue;
            }
        }
    }

    public GodEquipDT f_GetSCByidSkillGod(int iId)
    {
        return f_GetAll().FirstOrDefault(o => o is GodEquipDT data && data.idSkillGod == iId) as GodEquipDT;
    }

}
