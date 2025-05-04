
//============================================
//
//    Magic来自Magic.xlsx文件自动生成脚本
//    2017/3/28 14:53:07
//    
//
//============================================
using System;
using System.Collections.Generic;



public class MagicSC : NBaseSC
{
    public MagicSC()
    {
        Create("MagicDT");
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        MagicDT DataDT;
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
                DataDT = new MagicDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT._szName = tData[a++];
                DataDT.iLv = ccMath.atoi(tData[a++]);
                DataDT._szReadme = tData[a++];
                DataDT.iClass = ccMath.atoi(tData[a++]);
                DataDT.iMp = ccMath.atoi(tData[a++]);
                DataDT.iGroupHero1 = ccMath.atoi(tData[a++]);
                DataDT.iGroupHero2 = ccMath.atoi(tData[a++]);
                DataDT.iGroupHero3 = ccMath.atoi(tData[a++]);
                DataDT.iUseLv = ccMath.atoi(tData[a++]);
                DataDT.iHit = ccMath.atoi(tData[a++]);
                DataDT.iCrit = ccMath.atoi(tData[a++]);
                DataDT.iTarget = ccMath.atoi(tData[a++]);
                DataDT.iTargetPos = ccMath.atoi(tData[a++]);
                DataDT.iTartgetNum = ccMath.atoi(tData[a++]);
                DataDT.iType = ccMath.atoi(tData[a++]);
                DataDT.iFp = ccMath.atoi(tData[a++]);
                DataDT.iFate = ccMath.atoi(tData[a++]);
                DataDT.iGod = ccMath.atoi(tData[a++]);
                DataDT.iHp = ccMath.atoi(tData[a++]);
                DataDT.szSpellDamage = tData[a++];
                DataDT.iExtRand1 = ccMath.atoi(tData[a++]);
                DataDT.iExtTarget1 = ccMath.atoi(tData[a++]);
                DataDT.iExtTargetPos1 = ccMath.atoi(tData[a++]);
                DataDT.iExtTartgetNum1 = ccMath.atoi(tData[a++]);
                DataDT.iExtType1 = ccMath.atoi(tData[a++]);
                DataDT.iExtData11 = ccMath.atoi(tData[a++]);
                DataDT.iExtData12 = ccMath.atoi(tData[a++]);
                DataDT.iExtRand2 = ccMath.atoi(tData[a++]);
                DataDT.iExtTarget2 = ccMath.atoi(tData[a++]);
                DataDT.iExtTargetPos2 = ccMath.atoi(tData[a++]);
                DataDT.iExtTargetNum2 = ccMath.atoi(tData[a++]);
                DataDT.iExtType2 = ccMath.atoi(tData[a++]);
                DataDT.iExtData21 = ccMath.atoi(tData[a++]);
                DataDT.iExtData22 = ccMath.atoi(tData[a++]);
                DataDT.iBufId1 = ccMath.atoi(tData[a++]);
                DataDT.iBufRand1 = ccMath.atoi(tData[a++]);
                DataDT.iBufTarget1 = ccMath.atoi(tData[a++]);
                DataDT.iBufTargetPos1 = ccMath.atoi(tData[a++]);
                DataDT.iBufTargetNum1 = ccMath.atoi(tData[a++]);
                DataDT.iBufLive1 = ccMath.atoi(tData[a++]);
                DataDT.iBufId2 = ccMath.atoi(tData[a++]);
                DataDT.iBufRand2 = ccMath.atoi(tData[a++]);
                DataDT.iBufTarget2 = ccMath.atoi(tData[a++]);
                DataDT.iBufTargetPos2 = ccMath.atoi(tData[a++]);
                DataDT.iBufTargetNum2 = ccMath.atoi(tData[a++]);
                DataDT.iBufLive2 = ccMath.atoi(tData[a++]);
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
