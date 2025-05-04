
//============================================
//
//    TranslateLanguage来自TranslateLanguage.xlsx文件自动生成脚本
//    2018/7/18 15:55:07
//    
//
//============================================
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TranslateConfigSC : NBaseSC
{
    public TranslateConfigSC()
    {
        Create("TranslateConfigDT", true);
    }

    public override void f_LoadSCForData(string strData)
    {
        DispSaveData(strData);
    }

    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);
        TranslateConfigDT DataDT;
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
                DataDT = new TranslateConfigDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (DataDT.iId <= 0)
                {
MessageBox.ASSERT("Error Id");
                }
                DataDT.szKey = tData[a++];
                DataDT.szVietnamese = ReplaceText(tData[a++]);
                DataDT.szEnglish = ReplaceText(tData[a++]); 
                DataDT.szThaiLand = ReplaceText(tData[a++]);
                SaveItem(DataDT);
            }
            catch
            {
MessageBox.DEBUG(m_strRegDTName + "String record error, " + i);
                continue;
            }
        }
    }
    private string ReplaceText(string str)
    {
        if(!str.Contains("/n"))
        {
            return str;
        }
        str = str.Replace("/n","\n");
        return str;
    }

    public TranslateConfigDT GetConfigByKey(string key) {
        return f_GetAll().FirstOrDefault(o => o is TranslateConfigDT dt && dt.szKey == key) as TranslateConfigDT;
    }

    public string GetTranslateTextByKey(string key)
    {
        TranslateConfigDT dt = GetConfigByKey(key);
        if (dt == null)
        {
            return string.Empty;
        }
        if (glo_Main._Language == 0)
        {
            return dt.szVietnamese;
        }
        else if (glo_Main._Language == 1)
        {
            return dt.szEnglish;
        }
        else if (glo_Main._Language == 2)
        {
            return dt.szThaiLand;
        }
        return string.Empty;
    }
}
