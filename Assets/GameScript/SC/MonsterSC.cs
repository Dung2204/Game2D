
//============================================
//
//    Monster来自Monster.xlsx文件自动生成脚本
//    2017/3/30 18:58:34
//    
//
//============================================
using System;
using System.Collections.Generic;



public class MonsterSC : NBaseSC
{
    public MonsterSC()
    {
        Create("MonsterDT");
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        MonsterDT DataDT;
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
                DataDT = new MonsterDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT._szName = tData[a++];
                DataDT.iImportant = ccMath.atoi(tData[a++]);
                DataDT.iCardCamp = ccMath.atoi(tData[a++]);
                DataDT.iCardFightType = ccMath.atoi(tData[a++]);
                DataDT.iCardEle = ccMath.atoi(tData[a++]);
                DataDT.iAnger = ccMath.atoi(tData[a++]);
                DataDT.iAtk = ccMath.atoi(tData[a++]);
                DataDT.iHp = ccMath.atoi(tData[a++]);
                DataDT.iDef = ccMath.atoi(tData[a++]);
                DataDT.iMDef = ccMath.atoi(tData[a++]);
                DataDT.iHitR = ccMath.atoi(tData[a++]);
                DataDT.iDodgeR = ccMath.atoi(tData[a++]);
                DataDT.iCritR = ccMath.atoi(tData[a++]);
                DataDT.iAntiknockR = ccMath.atoi(tData[a++]);
                DataDT.iStatelId1 = ccMath.atoi(tData[a++]);
                DataDT.szModelMagic = tData[a++];
                
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
