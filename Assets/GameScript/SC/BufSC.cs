
//============================================
//
//    Buf来自Buf.xlsx文件自动生成脚本
//    2017/11/16 19:22:39
//    
//
//============================================
using System;
using System.Collections.Generic;



public class BufSC : NBaseSC
{
    public BufSC()
    {
        Create("BufDT");
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        BufDT DataDT;
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
                DataDT = new BufDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT._strReadme = tData[a++];
                DataDT.iType = ccMath.atoi(tData[a++]);
                DataDT.iPara = ccMath.atoi(tData[a++]);
                DataDT.iParaY = ccMath.atoi(tData[a++]);
                DataDT.iParaZ = ccMath.atoi(tData[a++]);
                DataDT.iSort = ccMath.atoi(tData[a++]);
                DataDT.iPlusState = ccMath.atoi(tData[a++]);
                DataDT.iReduceState = ccMath.atoi(tData[a++]);
                DataDT.iPoisoningAndFire = ccMath.atoi(tData[a++]);
                DataDT.iNotClear = ccMath.atoi(tData[a++]);
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
