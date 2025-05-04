
//============================================
//
//   AFKAward.xlsx
//    
//
//============================================
using System;
using System.Collections.Generic;
using System.Linq;

public class AFKConfigSC : NBaseSC
{
    public AFKConfigSC()
    {
        Create("AFKConfigDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        AFKConfigDT DataDT;
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
                DataDT = new AFKConfigDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.iLv = ccMath.atoi(tData[a++]);
                DataDT.iExp = ccMath.atoi(tData[a++]);
                DataDT.iMoney = ccMath.atoi(tData[a++]);
                SaveItem(DataDT);
            }
            catch
            {
MessageBox.DEBUG(m_strRegDTName + "String record error, " + i);
                continue;
            }
        }
    }

    public AFKConfigDT f_GetSCByLevel(int Level)
    {
        //AFKConfigDT aFKConfigDT = null;
        List<NBaseSCDT> revList = new List<NBaseSCDT>(f_GetAll());
        revList.Reverse();
        return revList.FirstOrDefault(o => o is AFKConfigDT afk && afk.iLv <= Level) as AFKConfigDT;
        //for (int i = 0; i < f_GetAll().Count; i++)
        //{
        //    AFKConfigDT temp = (AFKConfigDT)f_GetAll()[i];
        //    if (Level > temp.iLv)
        //        aFKConfigDT = temp;
        //}

        //return aFKConfigDT;
    }

}
