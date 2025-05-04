
//============================================
//
//    FirstRecharge来自FirstRecharge.xlsx文件自动生成脚本
//    2018/2/28 19:16:17
//    
//
//============================================
using System;
using System.Collections.Generic;



public class FirstRechargeSC : NBaseSC
{
    public FirstRechargeSC()
    {
        Create("FirstRechargeDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        FirstRechargeDT DataDT;
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
                DataDT = new FirstRechargeDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iCondition = ccMath.atoi(tData[a++]);
                DataDT.szAward = tData[a++];
                //TsuCode-  FirstRechargeNew - NapDau
                DataDT.szAward1 = tData[a++];
                DataDT.szAward2 = tData[a++];
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
