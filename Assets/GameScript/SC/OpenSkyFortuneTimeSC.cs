
//============================================
//
//    OpenSkyFortuneTime来自OpenSkyFortuneTime.xlsx文件自动生成脚本
//    2018/3/19 11:35:21
//    
//
//============================================
using System;
using System.Collections.Generic;



public class OpenSkyFortuneTimeSC : NBaseSC
{
    public OpenSkyFortuneTimeSC()
    {
        Create("OpenSkyFortuneTimeDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string [] ttt = ppSQL.Split(new string [] { "1#QW" } , System.StringSplitOptions.None);
        OpenSkyFortuneTimeDT DataDT;
        string [] tData;
        string [] tFoddScData = ttt [1].Split(new string [] { "|" } , System.StringSplitOptions.None);
        for (int i = 0 ; i < tFoddScData.Length ; i++)
        {
            try
            {
                if (tFoddScData [i] == "")
                {
                    MessageBox.DEBUG(m_strRegDTName + "脚本存在空记�? " + i);
                    continue;
                }
                tData = tFoddScData [i].Split(new string [] { "@," } , System.StringSplitOptions.None);
                int a = 0;
                DataDT = new OpenSkyFortuneTimeDT();
                DataDT.iId = ccMath.atoi(tData [a++]);
                if (DataDT.iId <= 0)
                {
                    MessageBox.ASSERT("Id错误");
                }
                DataDT.iOpenTime = ccMath.atoi(tData [a++]);
                DataDT.iEndTime = ccMath.atoi(tData [a++]);
                SaveItem(DataDT);
            }
            catch
            {
                MessageBox.DEBUG(m_strRegDTName + "脚本记录存在错误, " + i);
                continue;
            }
        }
    }




}
