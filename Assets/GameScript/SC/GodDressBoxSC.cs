
//============================================
//
//    GodDressBox来自GodDressBox.xlsx文件自动生成脚本
//    2018/4/11 15:30:48
//    
//
//============================================
using System;
using System.Collections.Generic;



public class GodDressBoxSC : NBaseSC
{
    public GodDressBoxSC()
    {
        Create("GodDressBoxDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        GodDressBoxDT DataDT;
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
                DataDT = new GodDressBoxDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iParam1 = ccMath.atoi(tData[a++]);
                DataDT.iAwardBox1 = ccMath.atoi(tData[a++]);
                DataDT.iParam2 = ccMath.atoi(tData[a++]);
                DataDT.iAwardBox2 = ccMath.atoi(tData[a++]);
                DataDT.iParam3 = ccMath.atoi(tData[a++]);
                DataDT.iAwardBox3 = ccMath.atoi(tData[a++]);
                DataDT.iParam4 = ccMath.atoi(tData[a++]);
                DataDT.iAwardBox4 = ccMath.atoi(tData[a++]);
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
