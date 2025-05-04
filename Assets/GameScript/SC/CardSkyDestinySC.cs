
//============================================
//
//    CardSkyDestiny来自CardSkyDestiny.xlsx文件自动生成脚本
//    2017/8/24 10:46:26
//    
//
//============================================
using System;
using System.Collections.Generic;



public class CardSkyDestinySC : NBaseSC
{
    public CardSkyDestinySC()
    {
        Create("CardSkyDestinyDT");
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        CardSkyDestinyDT DataDT;
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
                DataDT = new CardSkyDestinyDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iNeedExp = ccMath.atoi(tData[a++]);
                DataDT.iGoodsNum = ccMath.atoi(tData[a++]);
                DataDT.iOnceUp = ccMath.atoi(tData[a++]);
                DataDT.iOnceDown = ccMath.atoi(tData[a++]);
                DataDT.iNowExp1 = ccMath.atoi(tData[a++]);
                DataDT.iProbability1 = ccMath.atoi(tData[a++]);
                DataDT.szProbabilityDesc1 = tData[a++];
                DataDT.iNowExp2 = ccMath.atoi(tData[a++]);
                DataDT.iProbability2 = ccMath.atoi(tData[a++]);
                DataDT.szProbabilityDesc2 = tData[a++];
                DataDT.iNowExp3 = ccMath.atoi(tData[a++]);
                DataDT.iProbability3 = ccMath.atoi(tData[a++]);
                DataDT.szProbabilityDesc3 = tData[a++];
                DataDT.iNowExp4 = ccMath.atoi(tData[a++]);
                DataDT.iProbability4 = ccMath.atoi(tData[a++]);
                DataDT.szProbabilityDesc4 = tData[a++];
                DataDT.iNowExp5 = ccMath.atoi(tData[a++]);
                DataDT.iProbability5 = ccMath.atoi(tData[a++]);
                DataDT.szProbabilityDesc5 = tData[a++];
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
