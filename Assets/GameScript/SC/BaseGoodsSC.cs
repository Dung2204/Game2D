
//============================================
//
//    BaseGoods来自BaseGoods.xlsx文件自动生成脚本
//    2017/3/7 15:45:59
//    
//
//============================================
using System;
using System.Collections.Generic;



public class BaseGoodsSC : NBaseSC
{
    public BaseGoodsSC()
    {
        Create("BaseGoodsDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        BaseGoodsDT DataDT;
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
                DataDT = new BaseGoodsDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                DataDT._szName = tData[a++];
                DataDT.iIcon = ccMath.atoi(tData[a++]);
                DataDT.iImportant = ccMath.atoi(tData[a++]);
                DataDT._szReadme = tData[a++];
                DataDT.iPileQuantity = ccMath.atoi(tData[a++]);
                DataDT.iCanUse = ccMath.atoi(tData[a++]);
                DataDT.szURL = tData[a++];
                DataDT.iUI = ccMath.atoi(tData[a++]);
                DataDT.iEffect = ccMath.atoi(tData[a++]);
                DataDT.iEffectData = ccMath.atoi(tData[a++]);
                DataDT.iPriority = ccMath.atoi(tData[a++]);
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
