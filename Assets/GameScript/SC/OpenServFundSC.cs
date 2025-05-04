
//============================================
//
//    OpenServFund来自OpenServFund.xlsx文件自动生成脚本
//    2017/4/26 14:42:29
//    
//
//============================================
using System;
using System.Collections.Generic;



public class OpenServFundSC : NBaseSC
{
    public OpenServFundSC()
    {
        Create("OpenServFundDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        OpenServFundDT DataDT;
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
                DataDT = new OpenServFundDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iActType = ccMath.atoi(tData[a++]);
                DataDT._szActName = tData[a++];
                DataDT._szActContext = tData[a++];
                DataDT.iCondiction = ccMath.atoi(tData[a++]);
                DataDT.iGiftTabID = ccMath.atoi(tData[a++]);
                DataDT.iGiftID = ccMath.atoi(tData[a++]);
                DataDT.iGiftCount = ccMath.atoi(tData[a++]);
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
