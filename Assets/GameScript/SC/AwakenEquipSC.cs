
//============================================
//
//    AwakenEquip来自AwakenEquip.xlsx文件自动生成脚本
//    2017/3/30 15:02:27
//    
//
//============================================
using System;
using System.Collections.Generic;



public class AwakenEquipSC : NBaseSC
{
    public AwakenEquipSC()
    {
        Create("AwakenEquipDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        AwakenEquipDT DataDT;
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
                DataDT = new AwakenEquipDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT._szName = tData[a++];
                DataDT.iIcon = ccMath.atoi(tData[a++]);
                DataDT.iImportant = ccMath.atoi(tData[a++]);
                DataDT._szDesc = tData[a++];
                DataDT.iCount = ccMath.atoi(tData[a++]);
                DataDT.iResolveCount = ccMath.atoi(tData[a++]);
                DataDT.iList = ccMath.atoi(tData[a++]);
                DataDT.iType = ccMath.atoi(tData[a++]);
                DataDT.iSynthesisId1 = ccMath.atoi(tData[a++]);
                DataDT.iSynthesisNum1 = ccMath.atoi(tData[a++]);
                DataDT.iSynthesisId2 = ccMath.atoi(tData[a++]);
                DataDT.iSynthesisNum2 = ccMath.atoi(tData[a++]);
                DataDT.iSynthesisId3 = ccMath.atoi(tData[a++]);
                DataDT.iSynthesisNum3 = ccMath.atoi(tData[a++]);
                DataDT.iSynthesisId4 = ccMath.atoi(tData[a++]);
                DataDT.iSynthesisNum4 = ccMath.atoi(tData[a++]);
                DataDT.iMoenyNum = ccMath.atoi(tData[a++]);
                DataDT.iAddProId1 = ccMath.atoi(tData[a++]);
                DataDT.iAddPro1 = ccMath.atoi(tData[a++]);
                DataDT.iAddProId2 = ccMath.atoi(tData[a++]);
                DataDT.iAddPro2 = ccMath.atoi(tData[a++]);
                DataDT.iAddProId3 = ccMath.atoi(tData[a++]);
                DataDT.iAddPro3 = ccMath.atoi(tData[a++]);
                DataDT.iAddProId4 = ccMath.atoi(tData[a++]);
                DataDT.iAddPro4 = ccMath.atoi(tData[a++]);
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
