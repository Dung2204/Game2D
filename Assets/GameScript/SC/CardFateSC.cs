
//============================================
//
//    CardFate来自CardFate.xlsx文件自动生成脚本
//    2017/4/21 12:04:38
//    
//
//============================================
using System;
using System.Collections.Generic;



public class CardFateSC : NBaseSC
{
    public CardFateSC()
    {
        Create("CardFateDT");
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        CardFateDT DataDT;
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
                DataDT = new CardFateDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT._szName = tData[a++];
                DataDT.iCardQualityLower = ccMath.atoi(tData[a++]);
                DataDT.iCardQualityUpper = ccMath.atoi(tData[a++]);
                //for (int j = a; j < tData.Length; j++)
                //{
                //    if (tData[j] == "")
                //    {
                //        DataDT.szFateId.Remove(DataDT.szFateId.Length - 1);
                //        break;
                //    }
                //    DataDT.szFateId += tData[j] + ";";
                //}
                DataDT.szFateId = tData[a++];
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
