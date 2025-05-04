
//============================================
//
//    DescendFortune来自DescendFortune.xlsx文件自动生成脚本
//    2017/10/10 9:53:52
//    
//
//============================================
using System;
using System.Collections.Generic;



public class DescendFortuneSC : NBaseSC
{
    public DescendFortuneSC()
    {
        Create("DescendFortuneDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        DescendFortuneDT DataDT;
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
                DataDT = new DescendFortuneDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iVip = ccMath.atoi(tData[a++]);
                DataDT.iParam = ccMath.atoi(tData[a++]);
                DataDT.szTimes = tData[a++];
                DataDT.szWeight = tData[a++];
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
