
//============================================
//
//    FestivalExchange来自FestivalExchange.xlsx文件自动生成脚本
//    2018/5/21 17:42:57
//    
//
//============================================
using System;
using System.Collections.Generic;



public class FestivalExchangeSC : NBaseSC
{
    public FestivalExchangeSC()
    {
        Create("FestivalExchangeDT", true);


        
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        FestivalExchangeDT DataDT;
        string[] tData;
        string[] tFoddScData = ttt[1].Split(new string[] { "|" }, System.StringSplitOptions.None);
        for(int i = 0; i < tFoddScData.Length; i++)
        {
            try
            {
                if(tFoddScData[i] == "")
                {
MessageBox.DEBUG(m_strRegDTName + "String with empty record, " + i);
                    continue;
                }
                tData = tFoddScData[i].Split(new string[] { "@," }, System.StringSplitOptions.None);
                int a = 0;
                DataDT = new FestivalExchangeDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if(DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.ikey = ccMath.atoi(tData[a++]);
                DataDT.szResNeed = tData[a++];
                DataDT.szResAward = tData[a++];
                DataDT.iCount = ccMath.atoi(tData[a++]);
                DataDT.iRefresh = ccMath.atoi(tData[a++]);
                DataDT.iBeginTime = ccMath.atoi(tData[a++]);
                DataDT.iEndTime = ccMath.atoi(tData[a++]);
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
