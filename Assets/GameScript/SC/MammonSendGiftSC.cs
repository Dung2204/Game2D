
//============================================
//
//    MammonSendGift来自MammonSendGift.xlsx文件自动生成脚本
//    2018/4/16 14:16:17
//    
//
//============================================
using System;
using System.Collections.Generic;



public class MammonSendGiftSC : NBaseSC
{
    public MammonSendGiftSC()
    {
        Create("MammonSendGiftDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        MammonSendGiftDT DataDT;
        string[] tData;
        string[] tFoddScData = ttt[1].Split(new string[] { "|" }, System.StringSplitOptions.None);
        for (int i = 0; i < tFoddScData.Length; i++)
        {
            try
            {
                if (tFoddScData[i] == "")
                {
MessageBox.DEBUG(m_strRegDTName + "String with empty record ," + i);
                    continue;
                }
                tData = tFoddScData[i].Split(new string[] { "@," }, System.StringSplitOptions.None);
                int a = 0;
                DataDT = new MammonSendGiftDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.szAwardId = tData[a++];
                DataDT.iActivityType = ccMath.atoi(tData[a++]);
                DataDT.iStarTime = ccMath.atoi(tData[a++]);
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
