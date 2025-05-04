
//============================================
//
//    FashionableDress来自FashionableDress.xlsx文件自动生成脚本
//    2018/1/15 17:04:17
//    
//
//============================================
using System;
using System.Collections.Generic;



public class FashionableDressSC : NBaseSC
{
    public FashionableDressSC()
    {
        Create("FashionableDressDT");
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        FashionableDressDT DataDT;
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
                DataDT = new FashionableDressDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.szName = tData[a++];
                DataDT.iModel = ccMath.atoi(tData[a++]);
                DataDT.iIcon = ccMath.atoi(tData[a++]);
                DataDT.iImportant = ccMath.atoi(tData[a++]);
                DataDT.szMagic1 = tData[a++];
                DataDT.szMagic2 = tData[a++];
                DataDT.szMagic3 = tData[a++];
                DataDT.szMagic4 = tData[a++];
                DataDT.iPropertyId1 = ccMath.atoi(tData[a++]);
                DataDT.iPropertyNum1 = ccMath.atoi(tData[a++]);
                DataDT.iPropertyId2 = ccMath.atoi(tData[a++]);
                DataDT.iPropertyNum2 = ccMath.atoi(tData[a++]);
                DataDT.iTimeLimit = ccMath.atoi(tData[a++]);
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
