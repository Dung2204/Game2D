
//============================================
//
//    BattleFormations来自BattleFormations.xlsx文件自动生成脚本
//    2017/4/17 19:48:33
//    
//
//============================================
using System;
using System.Collections.Generic;



public class BattleFormationsSC : NBaseSC
{
    public BattleFormationsSC()
    {
        Create("BattleFormationsDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        BattleFormationsDT DataDT;
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
                DataDT = new BattleFormationsDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iType = ccMath.atoi(tData[a++]);
                DataDT._szTypeName = tData[a++];
                DataDT.iPosition = ccMath.atoi(tData[a++]);
                DataDT._szDescribe = tData[a++];
                DataDT.iActivePorpID = ccMath.atoi(tData[a++]);
                DataDT.iActivePorpCount = ccMath.atoi(tData[a++]);
                DataDT.iAttrID = ccMath.atoi(tData[a++]);
                DataDT.iAttrValue = ccMath.atoi(tData[a++]);
                DataDT.iPropID = ccMath.atoi(tData[a++]);
                DataDT.iPropCount = ccMath.atoi(tData[a++]);
                DataDT.iRoleQuality = ccMath.atoi(tData[a++]);
                DataDT.iDropID = ccMath.atoi(tData[a++]);
                DataDT.szTypeIcon = tData[a++];
                DataDT.szIconID = tData[a++];
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
