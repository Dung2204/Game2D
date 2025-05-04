
//============================================
//
//    ShopRandGoods来自ShopRandGoods.xlsx文件自动生成脚本
//    2017/3/28 16:54:56
//    
//
//============================================
using System;
using System.Collections.Generic;



public class ShopRandGoodsSC : NBaseSC
{
    public ShopRandGoodsSC()
    {
        Create("ShopRandGoodsDT");
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        ShopRandGoodsDT DataDT;
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
                DataDT = new ShopRandGoodsDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iSetId = ccMath.atoi(tData[a++]);
                DataDT.iGoodsType = ccMath.atoi(tData[a++]);
                DataDT.iGoodsId = ccMath.atoi(tData[a++]);
                DataDT.iGoodsNum = ccMath.atoi(tData[a++]);
                DataDT.iWeight = ccMath.atoi(tData[a++]);
                DataDT.iCostType = ccMath.atoi(tData[a++]);
                DataDT.iCostId = ccMath.atoi(tData[a++]);
                DataDT.iCostNum = ccMath.atoi(tData[a++]);
                DataDT.iIsRecommend = ccMath.atoi(tData[a++]);
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
