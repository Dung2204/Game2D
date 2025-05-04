
//============================================
//
//    Card来自Card.xlsx文件自动生成脚本
//    2017/3/15 17:15:18
//    
//
//============================================
using System;
using System.Collections.Generic;



public class CardSC : NBaseSC
{
    public CardSC()
    {
        Create("CardDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        CardDT DataDT;
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
                DataDT = new CardDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                DataDT._szName = tData[a++];
                DataDT.iCardType = ccMath.atoi(tData[a++]);
                DataDT.iImportant = ccMath.atoi(tData[a++]);
                DataDT.iCardFightType = ccMath.atoi(tData[a++]);
                DataDT.iCardCamp = ccMath.atoi(tData[a++]);
                DataDT.iInitAtk = ccMath.atoi(tData[a++]);
                DataDT.iInitHP = ccMath.atoi(tData[a++]);
                DataDT.iInitPhyDef = ccMath.atoi(tData[a++]);
                DataDT.iInitMagDef = ccMath.atoi(tData[a++]);
                DataDT.iAddAtk = ccMath.atoi(tData[a++]);
                DataDT.iAddHP = ccMath.atoi(tData[a++]);
                DataDT.iAddDef = ccMath.atoi(tData[a++]);
                DataDT.iAddMagDef = ccMath.atoi(tData[a++]);
                DataDT.iCardSoundId = ccMath.atoi(tData[a++]);
                DataDT.iStatelId1 = ccMath.atoi(tData[a++]);
                DataDT.iStatelId2 = ccMath.atoi(tData[a++]);
                DataDT._szCardDesc = tData[a++];
                DataDT.iSale = ccMath.atoi(tData[a++]);
                DataDT.szModelMagic1 = tData[a++];
                DataDT.szModelMagic2 = tData[a++];
                DataDT.iEvolveId = ccMath.atoi(tData[a++]);
                DataDT.iCardEle = ccMath.atoi(tData[a++]);
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
