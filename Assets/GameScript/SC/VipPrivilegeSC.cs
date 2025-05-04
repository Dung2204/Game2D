
//============================================
//
//    VipPrivilege来自VipPrivilege.xlsx文件自动生成脚本
//    2017/4/1 9:44:39
//    
//
//============================================
using System;
using System.Collections.Generic;



public class VipPrivilegeSC : NBaseSC
{
    public VipPrivilegeSC()
    {
        Create("VipPrivilegeDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        VipPrivilegeDT DataDT;
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
                DataDT = new VipPrivilegeDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT._szDesc = tData[a++];
                DataDT.iSite = ccMath.atoi(tData[a++]);
                DataDT.iLv0 = ccMath.atoi(tData[a++]);
                DataDT.iLv1 = ccMath.atoi(tData[a++]);
                DataDT.iLv2 = ccMath.atoi(tData[a++]);
                DataDT.iLv3 = ccMath.atoi(tData[a++]);
                DataDT.iLv4 = ccMath.atoi(tData[a++]);
                DataDT.iLv5 = ccMath.atoi(tData[a++]);
                DataDT.iLv6 = ccMath.atoi(tData[a++]);
                DataDT.iLv7 = ccMath.atoi(tData[a++]);
                DataDT.iLv8 = ccMath.atoi(tData[a++]);
                DataDT.iLv9 = ccMath.atoi(tData[a++]);
                DataDT.iLv10 = ccMath.atoi(tData[a++]);
                DataDT.iLv11 = ccMath.atoi(tData[a++]);
                DataDT.iLv12 = ccMath.atoi(tData[a++]);
                DataDT.iLv13 = ccMath.atoi(tData[a++]);
                DataDT.iLv14 = ccMath.atoi(tData[a++]);
                DataDT.iLv15 = ccMath.atoi(tData[a++]);
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
