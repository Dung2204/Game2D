
//============================================
//
//    RoleModel来自RoleModel.xlsx文件自动生成脚本
//    2017/11/6 10:25:50
//    
//
//============================================
using System;
using System.Collections.Generic;



public class RoleModelSC : NBaseSC
{
    public RoleModelSC()
    {
        Create("RoleModelDT");
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        RoleModelDT DataDT;
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
                DataDT = new RoleModelDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.szName = tData[a++];
                DataDT.iModel = ccMath.atoi(tData[a++]);
                DataDT.szMagic1 = tData[a++];
                DataDT.szMagic2 = tData[a++];
                DataDT.szMagic3 = tData[a++];
                DataDT.szMagic4 = tData[a++];
                DataDT.szReadme = tData[a++];
                DataDT.iIcon = ccMath.atoi(tData[a++]);
                DataDT.iAttackPos1 = ccMath.atoi(tData[a++]);
                DataDT.iAttackPos2 = ccMath.atoi(tData[a++]);
                DataDT.iAttackPos3 = ccMath.atoi(tData[a++]);
                DataDT.iAttackPos4 = ccMath.atoi(tData[a++]);
                DataDT.iMagicHarm1 = ccMath.atoi(tData[a++]);
                DataDT.iMagicHarm2 = ccMath.atoi(tData[a++]);
                DataDT.iMagicHarm3 = ccMath.atoi(tData[a++]);
                DataDT.iMagicHarm4 = ccMath.atoi(tData[a++]);
                DataDT.iMagicType1 = ccMath.atoi(tData[a++]);
                DataDT.iMagicType2 = ccMath.atoi(tData[a++]);
                DataDT.iMagicType3 = ccMath.atoi(tData[a++]);
                DataDT.iMagicType4 = ccMath.atoi(tData[a++]);
                DataDT.iFitMagic1 = ccMath.atoi(tData[a++]);
                DataDT.iFitMagic2 = ccMath.atoi(tData[a++]);
                DataDT.iFitMagic3 = ccMath.atoi(tData[a++]);
                DataDT.iFitMagic4 = ccMath.atoi(tData[a++]);
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
