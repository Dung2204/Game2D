
//============================================
//
//    PngAtlas来自PngAtlas.xlsx文件自动生成脚本
//    2017/3/16 11:17:21
//    
//
//============================================
using System;
using System.Collections.Generic;



public class PngAtlasSC : NBaseSC
{
    public PngAtlasSC()
    {
        Create("PngAtlasDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        PngAtlasDT DataDT;
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
                DataDT = new PngAtlasDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                DataDT.iStartId = ccMath.atoi(tData[a++]);
                DataDT.iEndId = ccMath.atoi(tData[a++]);
                DataDT.iAbId = ccMath.atoi(tData[a++]);
                DataDT.szFileName = tData[a++];
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
