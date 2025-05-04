
//============================================
//
//    SevenActivityTask来自SevenActivityTask.xlsx文件自动生成脚本
//    2017/11/30 15:41:52
//    
//
//============================================
using System;
using System.Collections.Generic;
using System.Linq;

public class VoteAppSC : NBaseSC
{
    public VoteAppSC()
    {
        Create("VoteAppDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);

    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        VoteAppDT DataDT;
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
                DataDT = new VoteAppDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iEventTime = ccMath.atoi(tData[a++]);


                DataDT.szAward = tData[a++];
                DataDT.szAndroid = tData[a++];
                DataDT.szIos = tData[a++];

                SaveItem(DataDT);
            }
            catch
            {
MessageBox.DEBUG(m_strRegDTName + "String record error, " + i);
                continue;
            }
        }
    }

    public List<NBaseSCDT> f_GetSCByEventTimeId(int iId)
    {
        return f_GetAll().Where(o => o is VoteAppDT data && data.iEventTime == iId).ToList();
    }

}
