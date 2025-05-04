
//============================================
//
//    PatrolLandSkill来自PatrolLandSkill.xlsx文件自动生成脚本
//    2017/9/6 10:38:14
//    
//
//============================================
using System;
using System.Collections.Generic;



public class PatrolLandSkillSC : NBaseSC
{
    public PatrolLandSkillSC()
    {
        Create("PatrolLandSkillDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        PatrolLandSkillDT DataDT;
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
                DataDT = new PatrolLandSkillDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iLandId = ccMath.atoi(tData[a++]);
                DataDT.iLv = ccMath.atoi(tData[a++]);
                DataDT._szDesc = tData[a++];
                DataDT.iOdds = ccMath.atoi(tData[a++]);
                DataDT.iMulti = ccMath.atoi(tData[a++]);
                DataDT.iNeedTime = ccMath.atoi(tData[a++]);
                DataDT.iCostSycee = ccMath.atoi(tData[a++]);
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
