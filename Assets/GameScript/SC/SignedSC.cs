
//============================================
//
//    Signed来自Signed.xlsx文件自动生成脚本
//    2017/3/30 14:15:17
//    
//
//============================================
using System;
using System.Collections.Generic;



public class SignedSC : NBaseSC
{
    public SignedSC()
    {
        Create("SignedDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        SignedDT DataDT;
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
                DataDT = new SignedDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iType = ccMath.atoi(tData[a++]);
                DataDT.iDayCount = ccMath.atoi(tData[a++]);
                DataDT.iAwardType = ccMath.atoi(tData[a++]);
                DataDT.iAwardID = ccMath.atoi(tData[a++]);
                DataDT.iAwardNum = ccMath.atoi(tData[a++]);
                DataDT.iVipRange = ccMath.atoi(tData[a++]);
                DataDT.iGrandAwardType1 = ccMath.atoi(tData[a++]);
                DataDT.iGrandAwardID1 = ccMath.atoi(tData[a++]);
                DataDT.iGrandAwardNum1 = ccMath.atoi(tData[a++]);
                DataDT.iGrandAwardType2 = ccMath.atoi(tData[a++]);
                DataDT.iGrandAwardID2 = ccMath.atoi(tData[a++]);
                DataDT.iGrandAwardNum2 = ccMath.atoi(tData[a++]);
                DataDT.iGrandAwardType3 = ccMath.atoi(tData[a++]);
                DataDT.iGrandAwardID3 = ccMath.atoi(tData[a++]);
                DataDT.iGrandAwardNum3 = ccMath.atoi(tData[a++]);
                DataDT.iGrandAwardType4 = ccMath.atoi(tData[a++]);
                DataDT.iGrandAwardID4 = ccMath.atoi(tData[a++]);
                DataDT.iGrandAwardNum4 = ccMath.atoi(tData[a++]);
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
