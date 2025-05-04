
//============================================
//
//    RebelArmy来自RebelArmy.xlsx文件自动生成脚本
//    2017/5/11 11:24:19
//    
//
//============================================
using System;
using System.Collections.Generic;



public class RebelArmySC : NBaseSC
{
    public RebelArmySC()
    {
        Create("RebelArmyDT");
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        RebelArmyDT DataDT;
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
                DataDT = new RebelArmyDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT._szName = tData[a++];
                DataDT.iType = ccMath.atoi(tData[a++]);
                DataDT.iImporent = ccMath.atoi(tData[a++]);
                DataDT.iLv = ccMath.atoi(tData[a++]);
                DataDT.iAtk = ccMath.atoi(tData[a++]);
                DataDT.lHp = ccMath.atol(tData[a++]);
                DataDT.iPhyDef = ccMath.atoi(tData[a++]);
                DataDT.iMagDef = ccMath.atoi(tData[a++]);
                DataDT.iHitR = ccMath.atoi(tData[a++]);
                DataDT.iDodgeR = ccMath.atoi(tData[a++]);
                DataDT.iCritR = ccMath.atoi(tData[a++]);
                DataDT.iAntiknockR = ccMath.atoi(tData[a++]);
                DataDT._szReadme = tData[a++];
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
