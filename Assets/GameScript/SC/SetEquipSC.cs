
//============================================
//
//    SetEquip来自SetEquip.xlsx文件自动生成脚本
//    2017/6/27 15:00:11
//    
//
//============================================
using System;
using System.Collections.Generic;



public class SetEquipSC : NBaseSC
{
    public SetEquipSC()
    {
        Create("SetEquipDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }
    private Dictionary<int, SetEquipDT> m_SetEquipSC = new Dictionary<int, SetEquipDT>();

    public Dictionary<int, SetEquipDT> M_SetEquipSC
    {
        get
        {
            return m_SetEquipSC;
        }
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        SetEquipDT DataDT;
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
                DataDT = new SetEquipDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT._szName = tData[a++];
                DataDT.iEquipId1 = ccMath.atoi(tData[a++]);
                DataDT.iEquipId2 = ccMath.atoi(tData[a++]);
                DataDT.iEquipId3 = ccMath.atoi(tData[a++]);
                DataDT.iEquipId4 = ccMath.atoi(tData[a++]);
                DataDT.iTwoEquipProId = ccMath.atoi(tData[a++]);
                DataDT.iTwoPro = ccMath.atoi(tData[a++]);
                DataDT.iThreeEquipProId = ccMath.atoi(tData[a++]);
                DataDT.iThreePro = ccMath.atoi(tData[a++]);
                DataDT.iFourEquipProId1 = ccMath.atoi(tData[a++]);
                DataDT.iFourPro1 = ccMath.atoi(tData[a++]);
                DataDT.iFourEquipProId2 = ccMath.atoi(tData[a++]);
                DataDT.iFourPro2 = ccMath.atoi(tData[a++]);
                SaveItem(DataDT);
                _InitSetEquipSC(DataDT);
            }
            catch
            {
MessageBox.DEBUG(m_strRegDTName + "String record error, " + i);
                continue;
            }
        }
    }
    private void _InitSetEquipSC(SetEquipDT tttt)
    {
        m_SetEquipSC.Add(tttt.iEquipId1, tttt);
        m_SetEquipSC.Add(tttt.iEquipId2, tttt);
        m_SetEquipSC.Add(tttt.iEquipId3, tttt);
        m_SetEquipSC.Add(tttt.iEquipId4, tttt);
    }
}
