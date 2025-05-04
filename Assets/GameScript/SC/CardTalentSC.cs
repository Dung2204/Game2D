
//============================================
//
//    CardTalent来自CardTalent.xlsx文件自动生成脚本
//    2017/5/22 11:14:49
//    
//
//============================================
using System;
using System.Collections.Generic;



public class CardTalentSC : NBaseSC
{
    public CardTalentSC()
    {
        Create("CardTalentDT");
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        CardTalentDT DataDT;
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
                DataDT = new CardTalentDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT._szName = tData[a++];
                DataDT.iTarget = ccMath.atoi(tData[a++]);
                DataDT.iPropertyId1 = ccMath.atoi(tData[a++]);
                DataDT.iPropertyNum1 = ccMath.atoi(tData[a++]);
                DataDT.iPropertyId2 = ccMath.atoi(tData[a++]);
                DataDT.iPropertyNum2 = ccMath.atoi(tData[a++]);
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
