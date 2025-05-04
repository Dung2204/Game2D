
//============================================
//
//    Guidance来自Guidance.xlsx文件自动生成脚本
//    2017/9/15 19:53:34
//    
//
//============================================
using System;
using System.Collections.Generic;



public class GuidanceSC : NBaseSC
{
    public GuidanceSC()
    {
        Create("GuidanceDT");
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        GuidanceDT DataDT;
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
                DataDT = new GuidanceDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iTeam = ccMath.atoi(tData[a++]);
                DataDT.szBtnName = tData[a++];
                DataDT.szArrPos = tData[a++];
                DataDT.iArrSide = ccMath.atoi(tData[a++]);
                DataDT.szText = tData[a++];
                DataDT.szTextPos = tData[a++];
                DataDT.iParticle = ccMath.atoi(tData[a++]);
                DataDT.iSave = ccMath.atoi(tData[a++]);
                DataDT.iDelay = ccMath.atoi(tData[a++]);
                DataDT.iDialog = ccMath.atoi(tData[a++]);
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
