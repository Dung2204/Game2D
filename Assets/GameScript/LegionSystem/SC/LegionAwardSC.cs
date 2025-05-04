
//============================================
//
//    LegionAward来自LegionAward.xlsx文件自动生成脚本
//    2017/5/23 12:00:18
//    
//
//============================================
using System;
using System.Collections.Generic;



public class LegionAwardSC : NBaseSC
{
    public LegionAwardSC()
    {
        Create("LegionAwardDT");
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        LegionAwardDT DataDT;
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
                DataDT = new LegionAwardDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iNeedNum1 = ccMath.atoi(tData[a++]);
                DataDT.szName1 = tData[a++];
                DataDT.iGoodsType1 = ccMath.atoi(tData[a++]);
                DataDT.iGoodsId1 = ccMath.atoi(tData[a++]);
                DataDT.iGoodsNum1 = ccMath.atoi(tData[a++]);
                DataDT.szPopText1 = tData[a++];
                DataDT.iNeedNum2 = ccMath.atoi(tData[a++]);
                DataDT.szName2 = tData[a++];
                DataDT.iGoodsType2 = ccMath.atoi(tData[a++]);
                DataDT.iGoodsId2 = ccMath.atoi(tData[a++]);
                DataDT.iGoodsNum2 = ccMath.atoi(tData[a++]);
                DataDT.szPopText2 = tData[a++];
                DataDT.iNeedNum3 = ccMath.atoi(tData[a++]);
                DataDT.szName3 = tData[a++];
                DataDT.iGoodsType3 = ccMath.atoi(tData[a++]);
                DataDT.iGoodsId3 = ccMath.atoi(tData[a++]);
                DataDT.iGoodsNum3 = ccMath.atoi(tData[a++]);
                DataDT.szPopText3 = tData[a++];
                DataDT.iNeedNum4 = ccMath.atoi(tData[a++]);
                DataDT.szName4 = tData[a++];
                DataDT.iGoodsType4 = ccMath.atoi(tData[a++]);
                DataDT.iGoodsId4 = ccMath.atoi(tData[a++]);
                DataDT.iGoodsNum4 = ccMath.atoi(tData[a++]);
                DataDT.szPopText4 = tData[a++];
                DataDT.iMax = ccMath.atoi(tData[a++]);
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
