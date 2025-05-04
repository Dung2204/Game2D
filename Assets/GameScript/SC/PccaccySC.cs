
//============================================
//
//    Pay来自Pay.xlsx文件自动生成脚本
//    2017/4/1 15:35:11
//    
//
//============================================
using System;
using System.Collections.Generic;



public class PccaccySC : NBaseSC
{
    public PccaccySC()
    {
        Create("PccaccyDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        PccaccyDT DataDT;
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
                DataDT = new PccaccyDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iPccaccyNum = ccMath.atoi(tData[a++]);
                DataDT._szPccaccyDesc = tData[a++];
                DataDT.iFirstPccaccyNum = ccMath.atoi(tData[a++]);
                DataDT.iPresentPccaccyNum = ccMath.atoi(tData[a++]);
                //DataDT.iRate= ccMath.atoi(tData[a++]);
                //TsuCode
                DataDT.iRate = ccMath.atof(tData[a++]);
                //
                DataDT.iPayCount= ccMath.atoi(tData[a++]);
                DataDT.szProductID_web = tData[a++];
                DataDT.szProductID_ios = tData[a++];
                DataDT.szProductID_android = tData[a++];
                DataDT.szPayShow = tData[a++];

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
