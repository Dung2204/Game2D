
//============================================
//
//    RedPacketExchange来自RedPacketExchange.xlsx文件自动生成脚本
//    2018/1/16 13:49:00
//    
//
//============================================
using System;
using System.Collections.Generic;



public class RedPacketExchangeSC : NBaseSC
{
    public RedPacketExchangeSC()
    {
        Create("RedPacketExchangeDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        RedPacketExchangeDT DataDT;
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
                DataDT = new RedPacketExchangeDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iExchangeTimes = ccMath.atoi(tData[a++]);
                DataDT.szConsumeRes = tData[a++];
                DataDT.iDstResType = ccMath.atoi(tData[a++]);
                DataDT.iDstResId = ccMath.atoi(tData[a++]);
                DataDT.iDstResCount = ccMath.atoi(tData[a++]);
                DataDT.iTimeBegin = ccMath.atoi(tData[a++]);
                DataDT.iTimeEnd = ccMath.atoi(tData[a++]);
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
