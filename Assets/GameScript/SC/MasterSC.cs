
//============================================
//
//    Master来自Master.xlsx文件自动生成脚本
//    2017/3/21 14:55:13
//    
//
//============================================
using System;
using System.Collections.Generic;



public class MasterSC : NBaseSC
{
    public MasterSC()
    {
        Create("MasterDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        MasterDT DataDT;
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
                DataDT = new MasterDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iType = ccMath.atoi(tData[a++]);
                DataDT.iLv = ccMath.atoi(tData[a++]);
                DataDT.iProID1 = ccMath.atoi(tData[a++]);
                DataDT.iPro1 = ccMath.atoi(tData[a++]);
                DataDT.iProId2 = ccMath.atoi(tData[a++]);
                DataDT.iPro = ccMath.atoi(tData[a++]);
                DataDT.iProId3 = ccMath.atoi(tData[a++]);
                DataDT.iPro3 = ccMath.atoi(tData[a++]);
                DataDT.iProId4 = ccMath.atoi(tData[a++]);
                DataDT.iPro4 = ccMath.atoi(tData[a++]);
                DataDT.iProId5 = ccMath.atoi(tData[a++]);
                DataDT.iPro5 = ccMath.atoi(tData[a++]);
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
