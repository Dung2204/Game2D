
//============================================
//
//    PatrolLand来自PatrolLand.xlsx文件自动生成脚本
//    2017/9/7 19:31:56
//    
//
//============================================
using System;
using System.Collections.Generic;



public class PatrolLandSC : NBaseSC
{
    public PatrolLandSC()
    {
        Create("PatrolLandDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        PatrolLandDT DataDT;
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
                DataDT = new PatrolLandDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT._szName = tData[a++];
                DataDT.iUnlock = ccMath.atoi(tData[a++]);
                DataDT._szDesc = tData[a++];
                DataDT.iModelId = ccMath.atoi(tData[a++]);
                DataDT.szModelDialog = tData[a++];
                DataDT.szPassAward = tData[a++];
                DataDT.szPatrolAwardShow = tData[a++];
                DataDT.iMonster1 = ccMath.atoi(tData[a++]);
                DataDT.iMonster2 = ccMath.atoi(tData[a++]);
                DataDT.iMonster3 = ccMath.atoi(tData[a++]);
                DataDT.iMonster4 = ccMath.atoi(tData[a++]);
                DataDT.iMonster5 = ccMath.atoi(tData[a++]);
                DataDT.iMonster6 = ccMath.atoi(tData[a++]);
                DataDT.iPower = ccMath.atoi(tData[a++]);
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
