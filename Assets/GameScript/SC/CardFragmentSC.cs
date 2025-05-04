
//============================================
//
//    CardFragment来自CardFragment.xlsx文件自动生成脚本
//    2017/3/7 15:46:15
//    
//
//============================================
using System;
using System.Collections.Generic;



public class CardFragmentSC : NBaseSC
{
    public CardFragmentSC()
    {
        Create("CardFragmentDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        CardFragmentDT DataDT;
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
                DataDT = new CardFragmentDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                DataDT._szName = tData[a++];
                DataDT.iIcon = ccMath.atoi(tData[a++]);
                DataDT.iImportant = ccMath.atoi(tData[a++]);
                DataDT._szReadme = tData[a++];
                DataDT.iNeedNum = ccMath.atoi(tData[a++]);
                DataDT.iNewCardId = ccMath.atoi(tData[a++]);
                DataDT.szStage = tData[a++];
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
