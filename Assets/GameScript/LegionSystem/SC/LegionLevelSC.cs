
//============================================
//
//    LegionLevel来自LegionLevel.xlsx文件自动生成脚本
//    2017/6/1 14:18:44
//    
//
//============================================
using System;
using System.Collections.Generic;



public class LegionLevelSC : NBaseSC
{
    public LegionLevelSC()
    {
        Create("LegionLevelDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        LegionLevelDT DataDT;
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
                DataDT = new LegionLevelDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iExp = ccMath.atoi(tData[a++]);
                DataDT.iCountMax = ccMath.atoi(tData[a++]);
                DataDT.iDungeonChapter = ccMath.atoi(tData[a++]);
                DataDT.iSkillId1 = ccMath.atoi(tData[a++]);
                DataDT.iSkillLimit1 = ccMath.atoi(tData[a++]);
                DataDT.iSkillCost1 = ccMath.atoi(tData[a++]);
                DataDT.iSkillId2 = ccMath.atoi(tData[a++]);
                DataDT.iSkillLimit2 = ccMath.atoi(tData[a++]);
                DataDT.iSkillCost2 = ccMath.atoi(tData[a++]);
                DataDT.iSkillId3 = ccMath.atoi(tData[a++]);
                DataDT.iSkillLimit3 = ccMath.atoi(tData[a++]);
                DataDT.iSkillCost3 = ccMath.atoi(tData[a++]);
                DataDT.iSkillId4 = ccMath.atoi(tData[a++]);
                DataDT.iSkillLimit4 = ccMath.atoi(tData[a++]);
                DataDT.iSkillCost4 = ccMath.atoi(tData[a++]);
                DataDT.iSkillId5 = ccMath.atoi(tData[a++]);
                DataDT.iSkillLimit5 = ccMath.atoi(tData[a++]);
                DataDT.iSkillCost5 = ccMath.atoi(tData[a++]);
                DataDT.iSkillId6 = ccMath.atoi(tData[a++]);
                DataDT.iSkillLimit6 = ccMath.atoi(tData[a++]);
                DataDT.iSkillCost6 = ccMath.atoi(tData[a++]);
                DataDT.iSkillId7 = ccMath.atoi(tData[a++]);
                DataDT.iSkillLimit7 = ccMath.atoi(tData[a++]);
                DataDT.iSkillCost7 = ccMath.atoi(tData[a++]);
                DataDT.iSkillId8 = ccMath.atoi(tData[a++]);
                DataDT.iSkillLimit8 = ccMath.atoi(tData[a++]);
                DataDT.iSkillCost8 = ccMath.atoi(tData[a++]);
                DataDT.iSkillId9 = ccMath.atoi(tData[a++]);
                DataDT.iSkillLimit9 = ccMath.atoi(tData[a++]);
                DataDT.iSkillCost9 = ccMath.atoi(tData[a++]);
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
