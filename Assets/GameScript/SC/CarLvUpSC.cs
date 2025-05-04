
//============================================
//
//    CarLvUp来自CarLvUp.xlsx文件自动生成脚本
//    2017/3/7 15:46:22
//    
//
//============================================
using System;
using System.Collections.Generic;



public class CarLvUpSC : NBaseSC
{
    public CarLvUpSC()
    {
        Create("CarLvUpDT");
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        CarLvUpDT DataDT;
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
                DataDT = new CarLvUpDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                DataDT.iCardType = ccMath.atoi(tData[a++]);
                DataDT.iAtk = ccMath.atoi(tData[a++]);
                DataDT.iAnergy = ccMath.atoi(tData[a++]);
                DataDT.iWhiteCard = ccMath.atoi(tData[a++]);
                DataDT.iGreenCard = ccMath.atoi(tData[a++]);
                DataDT.iBlueCard = ccMath.atoi(tData[a++]);
                DataDT.iPurpleCard = ccMath.atoi(tData[a++]);
                DataDT.iOragenCard = ccMath.atoi(tData[a++]);
                DataDT.iRedCard = ccMath.atoi(tData[a++]);
                DataDT.iGoldCard = ccMath.atoi(tData[a++]); //TsuCode - Tuong kim
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
