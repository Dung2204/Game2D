using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

public class LogTools
{


    public static void f_SaveLog(string strUserId, List<string> aLog)
    {
        StringBuilder strLog = new StringBuilder();
        for (int i = 0; i < aLog.Count; i++)
        {
            strLog.Append(DateTime.Now.ToString()+" "+aLog[i]);
            strLog.Append("\n");
        }
        
        WWWForm form = new WWWForm();
        form.AddField("iUserId", strUserId);
        form.AddField("strLog", strLog.ToString());
        System.IO.File.AppendAllText(Application.persistentDataPath + "/log.txt", strLog.ToString());
        //WWW w = new WWW(GloData.glo_strSaveLog, form); //TsuComment
    }

    //public static void f_DeleteLog(long iUserId)
    //{
    //    WWWForm form = new WWWForm();
    //    form.AddField("iUserId", iUserId.ToString());
    //    WWW w = new WWW(GloData.glo_strDeleteLog, form);
    //}



}