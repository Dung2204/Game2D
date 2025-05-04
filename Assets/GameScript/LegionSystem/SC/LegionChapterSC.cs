
//============================================
//
//    LegionChapter来自LegionChapter.xlsx文件自动生成脚本
//    2018/1/13 21:29:56
//    
//
//============================================
using System;
using System.Collections.Generic;



public class LegionChapterSC : NBaseSC
{
    public LegionChapterSC()
    {
        Create("LegionChapterDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        LegionChapterDT DataDT;
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
                DataDT = new LegionChapterDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT._szName = tData[a++];
                DataDT.iTollgateId1 = ccMath.atoi(tData[a++]);
                DataDT.iTollgateId2 = ccMath.atoi(tData[a++]);
                DataDT.iTollgateId3 = ccMath.atoi(tData[a++]);
                DataDT.iTollgateId4 = ccMath.atoi(tData[a++]);
                DataDT.iImage = ccMath.atoi(tData[a++]);
                DataDT.szAward = tData[a++];
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
