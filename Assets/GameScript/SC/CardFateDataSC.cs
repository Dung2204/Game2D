
//============================================
//
//    CardFateData来自CardFateData.xlsx文件自动生成脚本
//    2017/4/21 12:04:45
//    
//
//============================================
using System;
using System.Collections.Generic;



public class CardFateDataSC : NBaseSC
{
    public CardFateDataSC()
    {
        Create("CardFateDataDT");
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        CardFateDataDT DataDT;
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
                DataDT = new CardFateDataDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT._szName = tData[a++];
                DataDT.iGoodsType = ccMath.atoi(tData[a++]);
                DataDT.szGoodsId = tData[a++];
                DataDT.iAttrID1 = ccMath.atoi(tData[a++]);
                DataDT.iAttrValue1 = ccMath.atoi(tData[a++]);
                DataDT.iAttrID2 = ccMath.atoi(tData[a++]);
                DataDT.iAttrValue2 = ccMath.atoi(tData[a++]);
                DataDT.iAttrID3 = ccMath.atoi(tData[a++]);
                DataDT.iAttrValue3 = ccMath.atoi(tData[a++]);
                DataDT.iAttrID4 = ccMath.atoi(tData[a++]);
                DataDT.iAttrValue4 = ccMath.atoi(tData[a++]);
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
