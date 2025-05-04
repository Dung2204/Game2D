

//============================================
//
//    AuraFiveElements.xlsx vòng sáng nguyên tố
//    2018/1/15 17:04:17
//    maco
//
//============================================
using System;
using System.Collections.Generic;



public class AuraFiveElementsSC : NBaseSC
{
    public AuraFiveElementsSC()
    {
        Create("AuraFiveElementsDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        AuraFiveElementsDT DataDT;
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
                DataDT = new AuraFiveElementsDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
                    MessageBox.ASSERT("Error Id");
                }
                DataDT.iType = ccMath.atoi(tData[a++]);
                DataDT.iIsAll = ccMath.atoi(tData[a++]);
                DataDT.iLevel = ccMath.atoi(tData[a++]);
                DataDT.szParam = tData[a++];
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
