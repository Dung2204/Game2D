
//============================================
//
//    LegionSkill来自LegionSkill.xlsx文件自动生成脚本
//    2017/5/24 14:37:24
//    
//
//============================================
using System;
using System.Collections.Generic;



public class LegionSkillSC : NBaseSC
{
    public LegionSkillSC()
    {
        Create("LegionSkillDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        LegionSkillDT DataDT;
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
                DataDT = new LegionSkillDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iType = ccMath.atoi(tData[a++]);
                DataDT.iLevel = ccMath.atoi(tData[a++]);
                DataDT.iIcon = ccMath.atoi(tData[a++]);
                DataDT.szDesc = tData[a++];
                DataDT.iBuffID = ccMath.atoi(tData[a++]);
                DataDT.iBuffCount = ccMath.atoi(tData[a++]);
                DataDT.iLevelUpCost = ccMath.atoi(tData[a++]);
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
