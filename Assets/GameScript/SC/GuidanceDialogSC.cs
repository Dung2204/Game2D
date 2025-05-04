
//============================================
//
//    GuidanceDialog来自GuidanceDialog.xlsx文件自动生成脚本
//    2017/9/9 13:36:32
//    
//
//============================================
using System;
using System.Collections.Generic;



public class GuidanceDialogSC : NBaseSC
{
    public GuidanceDialogSC()
    {
        Create("GuidanceDialogDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        GuidanceDialogDT DataDT;
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
                DataDT = new GuidanceDialogDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iGroup = ccMath.atoi(tData[a++]);
                DataDT._szRoleName = tData[a++];
                DataDT.iModeId = ccMath.atoi(tData[a++]);
                DataDT.iAnchor = ccMath.atoi(tData[a++]);
                DataDT._szDialog = tData[a++];
                DataDT.szMusic = tData[a++];
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
