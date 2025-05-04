
//============================================
//
//    DungeonChapter来自DungeonChapter.xlsx文件自动生成脚本
//    2017/11/24 14:49:52
//    
//
//============================================
using System;
using System.Collections.Generic;



public class DungeonChapterSC : NBaseSC
{
    public DungeonChapterSC()
    {
        Create("DungeonChapterDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        DungeonChapterDT DataDT;
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
                DataDT = new DungeonChapterDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iChapterType = ccMath.atoi(tData[a++]);
                DataDT._szChapterName = tData[a++];
                DataDT._szChapterDesc = tData[a++];
                DataDT.szChapterImage = tData[a++];
                DataDT.iRoleImage = ccMath.atoi(tData[a++]);
                DataDT.szTollgateId = tData[a++];
                DataDT.iBox1 = ccMath.atoi(tData[a++]);
                DataDT.iNeedStar1 = ccMath.atoi(tData[a++]);
                DataDT.iBox2 = ccMath.atoi(tData[a++]);
                DataDT.iNeedStar2 = ccMath.atoi(tData[a++]);
                DataDT.iBox3 = ccMath.atoi(tData[a++]);
                DataDT.iNeedStar3 = ccMath.atoi(tData[a++]);
                DataDT.szFirstAward = tData[a++];
                DataDT.szCheckpointMap = tData[a++];
                DataDT.iBattleSceneMap = ccMath.atoi(tData[a++]);
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
