
//============================================
//
//    EquipIntensifyConsume来自EquipIntensifyConsume.xlsx文件自动生成脚本
//    2017/4/6 17:54:33
//    
//
//============================================
using System;
using System.Collections.Generic;



public class GodEquipIntensifyConsumeSC : NBaseSC
{
    public GodEquipIntensifyConsumeSC()
    {
        Create("GodEquipIntensifyConsumeDT",true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        GodEquipIntensifyConsumeDT DataDT;
        string[] tData;
        string[] tFoddScData = ttt[1].Split(new string[] { "|" }, System.StringSplitOptions.None);
        for (int i = 0; i < tFoddScData.Length; i++)
        {
            try
            {
                if (tFoddScData[i] == "")
                {
                    MessageBox.DEBUG(m_strRegDTName + "Chuỗi có bản ghi trống, " + i);
                    continue;
                }
                tData = tFoddScData[i].Split(new string[] { "@," }, System.StringSplitOptions.None);
                int a = 0;
                DataDT = new GodEquipIntensifyConsumeDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
                    MessageBox.ASSERT("Id lỗi");
                }
                DataDT.iGreen = ccMath.atoi(tData[a++]);
                DataDT.iBule = ccMath.atoi(tData[a++]);
                DataDT.iViolet = ccMath.atoi(tData[a++]);
                DataDT.iOrange = ccMath.atoi(tData[a++]);
                DataDT.iRed = ccMath.atoi(tData[a++]);
                DataDT.iGold = ccMath.atoi(tData[a++]);
                SaveItem(DataDT);
            }
            catch
            {
                MessageBox.DEBUG(m_strRegDTName + "Lỗi bản ghi chuỗi, " + i);
                continue;
            }
        }
    }

}
