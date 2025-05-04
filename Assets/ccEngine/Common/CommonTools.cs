using ccU3DEngine;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Globalization;
/// <summary>
/// 常用工具
/// </summary>
public class CommonTools
{
    /// <summary>
    /// 执行回调
    /// </summary>
    /// <param name="tccCallback">回调方法</param>
    /// <param name="Obj">回调参数</param>
    public static void f_DoCallBack(ccCallback tccCallback, object Obj)
    {
        if (tccCallback != null)
        {
            tccCallback(Obj);
        }
    }

    /// <summary>
    /// 以万为单位获取百分比乘值(向下取整)
    /// </summary>
    /// <param name=""></param>
    public static string f_GetPercentValueTenThousandStr(int value, int tenSoundPercent)
    {
        float percent = tenSoundPercent * 1.0f / 10000;
        float lastValue = value * (1 + percent);
        return Mathf.Floor(lastValue).ToString("0");
    }

    public static int f_GetPercentValueTenThousandInt32(int value, int tenSoundPercent)
    {
        float percent = tenSoundPercent * 1.0f / 10000;
        float lastValue = value * (1 + percent);
        return Convert.ToInt32(Mathf.Floor(lastValue));
    }

    /// <summary>
    /// 以万为单位获取百分比
    /// </summary>
    /// <param name=""></param>
    public static string f_GetPercentValueTenThousandStr(int value)
    {
        return (value * 1.0f / 10000).ToString("0.0%");
    }
    public static long f_GetPercentValueTenThousandInt64(long value, int tenSoundPercent)
    {
        float percent = tenSoundPercent * 1.0f / 10000;
        float lastValue = value * (1 + percent);
        return Convert.ToInt64(Mathf.Floor(lastValue));
    }
    /// <summary>
    /// 以万为单位获取百分比字符串，保留小数点后一位
    /// </summary>
    /// <param name=""></param>
    public static string f_GetPercentStrTenThousandStr(int tenSoundPercent)
    {
        float percent = tenSoundPercent * 1.0f / 10000;
        return percent.ToString("0.0%", CultureInfo.InvariantCulture);
    }
    /// <summary>
    /// 反序列化
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="path">路径</param>
    /// <param name="fileName">文件名字</param>
    /// <returns></returns>
    public static List<T> f_DeserializeTemplate<T>(string path, string fileName)
    {
        if (!Directory.Exists(path))
        {
            Debug.LogError("Path Not Exist " + path);
            return null;
        }
        string fullPath = Path.Combine(path, fileName);
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        FileStream fs = new FileStream(fullPath, FileMode.Open);
        BinaryFormatter bf = new BinaryFormatter();
        stopwatch.Start();
        List<T> list = bf.Deserialize(fs) as List<T>;
        stopwatch.Stop();
        fs.Close();
        Debug.Log("DeSerialize:" + stopwatch.ElapsedMilliseconds);
        return list;
    }

    /// <summary>
    /// 序列化
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="path">路径</param>
    /// <param name="fileName">文件名字</param>
    /// <param name="sourceData">源数据</param>
    public static void f_SerializeTemplate<T>(string path, string fileName, List<T> sourceData)
    {
        if (!Directory.Exists(path))
        {
            Debug.LogError("Path Not Exist " + path);
            return;
        }
        string fullPath = Path.Combine(path, fileName);
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        FileStream fs = new FileStream(fullPath, FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        stopwatch.Start();
        bf.Serialize(fs, sourceData);
        stopwatch.Stop();
        fs.Close();
        Debug.Log("Serialize:" + stopwatch.ElapsedMilliseconds);
    }

    /// <summary>
    /// 通过传入的秒数返回时间字符串（时分秒）
    /// </summary>
    /// <param name="second">传入的秒数</param>
    /// <returns></returns>
    public static string f_GetStringBySecond(int second)
    {
        string timeStr = "";
        int hour = second / 3600;
        int minute = (second - hour * 60 * 60) / 60;
        int sec = (second - hour * 60 * 60) % 60;
        timeStr += hour < 10 ? ("0" + hour.ToString()) : hour.ToString();
        timeStr += ":";
        timeStr += minute < 10 ? ("0" + minute.ToString()) : minute.ToString();
        timeStr += ":";
        timeStr += sec < 10 ? ("0" + sec.ToString()) : sec.ToString();
        return timeStr;
    }
    /// <summary>
    /// 浅复制数据到新list（仅可对新list移除和增加，否则会改变源数据）
    /// </summary>
    /// <param name="listOri"></param>
    /// <returns></returns>
    public static List<BasePoolDT<long>> f_CopyPoolDTArrayToNewList(List<BasePoolDT<long>> listOri)
    {
        List<BasePoolDT<long>> listDes = new List<BasePoolDT<long>>();
        for (int i = 0; i < listOri.Count; i++)
        {
            listDes.Add(listOri[i]);
        }
        return listDes;
    }
    /// <summary>
    /// 深复制数据到新list
    /// </summary>
    /// <param name="listOri"></param>
    /// <returns></returns>
    public static List<BasePoolDT<long>> f_CopyPoolDTArray(List<BasePoolDT<long>> listOri)
    {
        List<BasePoolDT<long>> listDes = new List<BasePoolDT<long>>();
        for (int i = 0; i < listOri.Count; i++)
        {
            listDes.Add(listOri[i].Clone());
        }
        return listDes;
    }
    /// <summary>
    /// 复制数据到新list（仅可对新list移除和增加，否则会改变源数据）
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static List<NBaseSCDT> f_CopySCDTArray(List<NBaseSCDT> source)
    {
        List<NBaseSCDT> target = new List<NBaseSCDT>();
        for (int i = 0; i < source.Count; i++)
        {
            target.Add(source[i]);
        }
        return target;
    }
    /// <summary>
    /// 获取时间
    /// </summary>
    /// <param name="strStartTime">20181224</param>
    /// <returns></returns>
    public static DateTime f_GetDateTimeByTimeStr(string strStartTime)
    {
        if (strStartTime.Length < 8)
        {
            MessageBox.ASSERT("Sai định dạng： " + strStartTime);
        }
        else if (strStartTime.Length > 8)
        {
            MessageBox.DEBUG("Thời gian quá dài： " + strStartTime);
        }
        int year1 = int.Parse(strStartTime.Substring(0, 4));//20181224
        int month1 = int.Parse(strStartTime.Substring(4, 2));
        int day1 = int.Parse(strStartTime.Substring(6, 2));
        return new DateTime(year1, month1, day1);
    }

    /// <summary>
    /// 获取时间
    /// </summary>
    /// <param name="strStartTime">201812240815</param>
    /// <returns></returns>
    public static DateTime f_GetDateTimeByTimeStr2(string strStartTime)
    {
        if (strStartTime.Length < 12)
        {
            MessageBox.ASSERT("Sai định dạng： " + strStartTime);
        }
        else if (strStartTime.Length > 12)
        {
            MessageBox.DEBUG("Thời gian quá dài： " + strStartTime);
        }
        int year1 = int.Parse(strStartTime.Substring(0, 4));//20181224
        int month1 = int.Parse(strStartTime.Substring(4, 2));
        int day1 = int.Parse(strStartTime.Substring(6, 2));
        int hour1 = int.Parse(strStartTime.Substring(8, 2));
        int minute1 = int.Parse(strStartTime.Substring(10, 2));
        return new DateTime(year1, month1, day1, hour1, minute1, 0);
    }
    /// <summary>
    /// 检查服务器是否开启
    /// </summary>
    /// <param name="openTime">精确到分：201812240815</param>
    /// <returns></returns>
    public static bool f_CheckServerTimeOpen(string openTime)
    {
        DateTime datetime1970 = new DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        TimeSpan timeSpan = DateTime.UtcNow - datetime1970;
        DateTime dataOpenTime = f_GetDateTimeByTimeStr2(openTime);
        long dateOT = ccMath.DateTime2time_t(dataOpenTime);
        if (timeSpan.TotalSeconds > dateOT)
            return true;
        else
            return false;
    }
    /// <summary>
    /// 检查活动时间是否处于时间段内
    /// </summary>
    /// <param name="strStartTime">格式：20181224</param>
    /// <param name="strEndTime">格式：20181224</param>
    /// <returns></returns>
    //public static bool f_CheckTime(string strStartTime, String strEndTime) //TsuComment
    public static bool f_CheckTime(string strStartTime, String strEndTime) //Tsucode
    {
        DateTime startTime = f_GetDateTimeByTimeStr(strStartTime);
        DateTime endTime = f_GetDateTimeByTimeStr(strEndTime);
        return f_CheckTime(startTime, endTime);
    }
    /// <summary>
    /// 检查活动时间是否处于时间段内
    /// </summary>
    /// <param name="startTime"></param>
    /// <param name="endTime"></param>
    /// <returns></returns>
    public static bool f_CheckTime(DateTime startTime, DateTime endTime)
    {
        int timeNow = GameSocket.GetInstance().f_GetServerTime();
        long timeStart = ccMath.DateTime2time_t(startTime);
        long timeEnd = ccMath.DateTime2time_t(endTime);
        if (timeStart < timeNow && timeNow < timeEnd)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 通过奖励字符串获取资源
    /// </summary>
    /// <param name="resourceStr">格式："1;7;100"</param>
    public static ResourceCommonDT f_GetCommonResourceByResourceStr(string resourceStr)
    {
        string[] szArr = resourceStr.Split(';');
        ResourceCommonDT resourceCommonDT = new ResourceCommonDT();
        resourceCommonDT.f_UpdateInfo(Convert.ToByte(szArr[0]), Convert.ToInt32(szArr[1]), Convert.ToInt32(szArr[2]));
        return resourceCommonDT;
    }
    /// <summary>
    /// 通过奖励字符串获取资源
    /// </summary>
    /// <param name="resourceStr">格式："1;7;100"</param>
    public static AwardPoolDT f_GetAwardPoolDTByResourceStr(string resourceStr)
    {
        string[] szArr = resourceStr.Split(';');
        AwardPoolDT awardPoolDT = new AwardPoolDT();
        awardPoolDT.f_UpdateByInfo(Convert.ToByte(szArr[0]), Convert.ToInt32(szArr[1]), Convert.ToInt32(szArr[2]));
        return awardPoolDT;
    }
    /// <summary>
    /// 检查字符串字节数
    /// </summary>
    /// <param name="Name">字符串名称</param>
    /// <param name="isNeedCheckSpecialNum">是否需要检测特殊符号（如果需要检测，则会在有特殊符号的时候返回-1</param>
    /// <returns></returns>
    public static int f_CheckLength(string Name, bool isNeedCheckSpecialNum = true)
    {
        int size = 0;
        for (int i = 0; i < Name.Length; i++)
        {
            int ach = (int)Name[i];
            if (ach > 127)//汉字
            {
                size += 2;
            }
            else if (//英文或数字
                (ach >= 'a' && ach <= 'z') ||
                (ach >= 'A' && ach <= 'Z') ||
                (ach >= '0' && ach <= '9'))
            {
                size += 1;
            }
            else if (isNeedCheckSpecialNum)//不可使用特殊符号
            {
                return -1;
            }
            else
            {
                size += 1;
            }
        }
        return size;
    }

    /// <summary>
    /// 将2个Int转成Long
    /// </summary>
    /// <param name="value1">低位</param>
    /// <param name="value2">高位</param>
    /// <returns></returns>
    public static long f_TwoInt2Long(uint value1, uint value2)
    {
        long result = value2;
        result = result << 32;
        result += value1;
        return result;
    }
    /// <summary>
    /// 检测是否是同一天,跨天重置（延时15秒）
    /// </summary>
    /// <param name="time1">时间1</param>
    /// <param name="time2">时间2</param>
    /// <returns></returns>
    public static bool f_CheckSameDay(int time1, int time2)
    {
        time1 -= 15;
        time2 -= 15;
        DateTime dataTime1 = ccMath.time_t2DateTime(time1);
        DateTime dataTime2 = ccMath.time_t2DateTime(time2);
        if (dataTime1.Year == dataTime2.Year &&
            dataTime1.Month == dataTime2.Month &&
            dataTime1.Day == dataTime2.Day)
        {
            //Debug.LogError("跨天重置");
            return true;
        }
        return false;
    }

    public static void _CharArrToString(char[] chararr, out string tString)
    {
        tString = string.Empty;
        for (int i = 0; i < chararr.Length; i++)
        {
            tString += chararr[i].ToString();
        }
    }

    public static List<ResourceCommonDT> f_GetListCommonDT(string strWasterItem)
    {
        List<ResourceCommonDT> listCommonDT = new List<ResourceCommonDT>();
        string[] listItem = strWasterItem.Split('#');
        for (int i = 0; i < listItem.Length; i++)
        {
            string StrCommonItem = listItem[i];
            if (StrCommonItem.Contains(";"))
            {
                ResourceCommonDT commonDT = new ResourceCommonDT();
                int resourceType = int.Parse(StrCommonItem.Split(';')[0]);
                int resourceId = int.Parse(StrCommonItem.Split(';')[1]);
                int resourceCount = int.Parse(StrCommonItem.Split(';')[2]);
                commonDT.f_UpdateInfo((byte)resourceType, resourceId, resourceCount);
                listCommonDT.Add(commonDT);
            }
        }
        return listCommonDT;
    }

    public static List<AwardPoolDT> f_GetListAwardPoolDT(string strWasterItem)
    {
        List<AwardPoolDT> listCommonDT = new List<AwardPoolDT>();
        string[] listItem = strWasterItem.Split('#');
        for (int i = 0; i < listItem.Length; i++)
        {
            string StrCommonItem = listItem[i];
            if (StrCommonItem.Contains(";"))
            {
                AwardPoolDT commonDT = new AwardPoolDT();
                int resourceType = int.Parse(StrCommonItem.Split(';')[0]);
                int resourceId = int.Parse(StrCommonItem.Split(';')[1]);
                int resourceCount = int.Parse(StrCommonItem.Split(';')[2]);
                commonDT.f_UpdateByInfo((byte)resourceType, resourceId, resourceCount);
                listCommonDT.Add(commonDT);
            }
        }
        return listCommonDT;
    }
    public static string f_GetAddProperty(EM_RoleProperty roleProperty, int value)
    {
        string strAddPro = value.ToString();
        //除了攻击、防御、法防和怒气，其余全是与万单位
        if (roleProperty != (EM_RoleProperty.Atk) && roleProperty != (EM_RoleProperty.Def) && roleProperty != (EM_RoleProperty.Hp)
            && roleProperty != (EM_RoleProperty.MDef) && roleProperty != (EM_RoleProperty.Anger))
            strAddPro = CommonTools.f_GetPercentValueTenThousandStr(value);
        return strAddPro;
    }
    /// <summary>
    /// 将字符串插入换行
    /// </summary>
    /// <param name="str">要处理的字符串</param>
    /// <returns></returns>
    public static string f_GetVerticleStr(string str)
    {
        char[] strArray = str.ToCharArray();
        string strHandle = "";
        for (int j = 0; j < strArray.Length; j++)
        {
            strHandle += strArray[j];
            if (j != strArray.Length - 1)
                strHandle += "\n";
        }
        return strHandle;
    }

    public static string GetBroadCastMess(string name,int opt,int cardTempId) {
        if (opt == (int)SocketCommand.CS_TurntableLottery)
        {
            return string.Format(CommonTools.f_GetTransLanguage(2231), name, cardTempId);
        }
        else
        {
            ResourceCommonDT commonDT = new ResourceCommonDT();
            commonDT.f_UpdateInfo((byte)EM_ResourceType.Card, cardTempId, 1);
            string cardName = commonDT.mName;
            UITool.f_GetImporentColorName(commonDT.mImportant, ref cardName);
            return string.Format(CommonTools.f_GetTransLanguage(9), name, UITool.GetGetWayBySocketId(opt), UITool.f_GetImportantColorName((EM_Important)commonDT.mImportant), cardName);
        }
    }


    /// <summary>
    /// 获取翻译表文本
    /// </summary>
    /// <returns></returns>
    public static string f_GetTransLanguage(int textId)
    {
        ///MessageBox.DEBUG("获取翻译表Id为："+textId);
        string language = "";
        TranslateLanguageDT _TranslateDt = (TranslateLanguageDT)glo_Main.GetInstance().m_SC_Pool.m_TranslateLanguageSC.f_GetSC(textId); //TsuComment
        //TsuCode
        //TranslateTsuDT _TranslateDt = (TranslateTsuDT)glo_Main.GetInstance().m_SC_Pool.m_TranslateTsuSC.f_GetSC(textId);
        //

        if (_TranslateDt == null)
        {
            return " Undefinition";
        }
        //获取文本字符串
        if (glo_Main._Language == 0)//默认中文
        {
            language = _TranslateDt.szVietnamese;
        }
        else if (glo_Main._Language == 1)//繁体
        {
            language = _TranslateDt.szEnglish;
        }
        else if (glo_Main._Language == 2)//繁体
        {
            language = _TranslateDt.szThailand;
        }
        return language;
    }

    /// <summary>
    /// 根据开服时间
    /// </summary>
    /// <returns></returns>
    public static bool f_CheckActIsOpenForOpenSeverTime(int StarDay, int EndDay)
    {
        //int PassTime = GameSocket.GetInstance().f_GetServerTime() - Data_Pool.m_SevenActivityTaskPool.OpenSeverTime;
        //int PassDay = (PassTime / 86400) + 1;
        //return PassDay >= StarDay && PassDay < EndDay;

        ////TsuCode
            int nowTimeServer = GameSocket.GetInstance().f_GetServerTime(); //ServerTime now(second)
        string start = StarDay.ToString(); //TsuCode
        string end = EndDay.ToString();

        DateTime startTime = new DateTime(int.Parse(start.Substring(0, 4)), int.Parse(start.Substring(4, 2)), int.Parse(start.Substring(6, 2))); //TsuCode
        long sTime = ccMath.DateTime2time_t(startTime);

        DateTime endTime = new DateTime(int.Parse(end.Substring(0, 4)), int.Parse(end.Substring(4, 2)), int.Parse(end.Substring(6, 2)));
        long eTime = ccMath.DateTime2time_t(endTime);
        if (nowTimeServer >= sTime && nowTimeServer <= eTime) return true;

        return false;
        //////////////
    }

    public static int f_GetActEndTimeForOpenSeverTime(int EndDay)
    {
        //TsuCode
        string end = EndDay.ToString();
        int sYear = int.Parse(end.Substring(0, 4));
        int sMonth = int.Parse(end.Substring(4, 2));
        int sDay = int.Parse(end.Substring(6, 2));
        DateTime endTime = new DateTime(sYear, sMonth, sDay); //TsuCode
        int sTime = (int)ccMath.DateTime2time_t(endTime);
        
        return sTime;
        //

        //return Data_Pool.m_SevenActivityTaskPool.OpenSeverTime + (EndDay - 1) * 86400;

    }

    public static int f_GetActStarTimeForOpenSeverTime(int StarDay) {
        //TsuCode
        string start = StarDay.ToString();
        int sYear = int.Parse(start.Substring(0, 4));
        int sMonth = int.Parse(start.Substring(4, 2));
        int sDay = int.Parse(start.Substring(6, 2));
        DateTime startTime = new DateTime(sYear, sMonth, sDay); //TsuCode
        int sTime = (int)ccMath.DateTime2time_t(startTime);
        return sTime;
        //
        //return Data_Pool.m_SevenActivityTaskPool.OpenSeverTime + (StarDay - 1) * 86400;

    }

    #region TsuCode - Func

    /// <summary>
    /// Check format day, YYYYMMDD =false
    /// <param name="day"></param>
    /// </summary>
    /// <returns> YYYYMMDD =false </returns>
    public bool checkDayFormat(int day)
    {
        GameParamDT param = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(94);
        int limitDay = param.iParam1;
        if (day >= 0 && day<= limitDay) return true;
        return false; //YYYYMMDD
    }

    /// <summary>
    /// Check OpenServerDay > limitday (gameParam id 94) = true
    /// </summary>
    /// <returns></returns>
    public bool checkOpenServerDay()
    {
        int openServerTime = Data_Pool.m_SevenActivityTaskPool.OpenSeverTime;
        int nowTime = GameSocket.GetInstance().f_GetServerTime();
        int openServerDay = ccMath.f_GetTotalDaysByTime(nowTime) - ccMath.f_GetTotalDaysByTime(openServerTime) + 1;
        GameParamDT param = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(94);
        int limitDay = param.iParam1;
        if (openServerDay > limitDay) return true;
        return false;
    }

    public static int f_GetActEndTimeForOpenSeverTime_Tsu(int EndDay)
    {
        GameParamDT param = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(94);
        int limitDay = param.iParam1;
        if (EndDay>=0 && EndDay <= limitDay)
        {
            int openServerTime = Data_Pool.m_SevenActivityTaskPool.OpenSeverTime - 86400;
            return openServerTime + (EndDay * 86400);
        }
        if(EndDay >= 20200101) { 
            //TsuCode
            string end = EndDay.ToString();
            int sYear = int.Parse(end.Substring(0, 4));
            int sMonth = int.Parse(end.Substring(4, 2));
            int sDay = int.Parse(end.Substring(6, 2));
            DateTime endTime = new DateTime(sYear, sMonth, sDay); //TsuCode
            int sTime = (int)ccMath.DateTime2time_t(endTime);
            return sTime;
        }
        //
        return 0;
    }
    //return Data_Pool.m_SevenActivityTaskPool.OpenSeverTime + (EndDay - 1) * 86400;


    public static int f_GetActStarTimeForOpenSeverTime_Tsu(int StarDay)
    {
        //TsuCode
        GameParamDT param = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(94);
        int limitDay = param.iParam1;
        if (StarDay >= 0 && StarDay <= limitDay)
        {
            int openServerTime = Data_Pool.m_SevenActivityTaskPool.OpenSeverTime - 86400;
            return openServerTime + (StarDay * 86400);
        }
        else if (StarDay >= 20200101)
        {
            string start = StarDay.ToString();
            int sYear = int.Parse(start.Substring(0, 4));
            int sMonth = int.Parse(start.Substring(4, 2));
            int sDay = int.Parse(start.Substring(6, 2));
            DateTime startTime = new DateTime(sYear, sMonth, sDay); //TsuCode
            int sTime = (int)ccMath.DateTime2time_t(startTime);

            return sTime;
        }
        return 0;
        //
        //return Data_Pool.m_SevenActivityTaskPool.OpenSeverTime + (StarDay - 1) * 86400;

    }
    public static bool f_CheckActIsOpenForOpenSeverTime_TsuFunc(int StarDay, int EndDay)
    {
       
        // startDay, endDay - openServerTime
        int openServerTime = Data_Pool.m_SevenActivityTaskPool.OpenSeverTime;
        int nowTimeServer = GameSocket.GetInstance().f_GetServerTime(); //ServerTime now(second)
        int openServerDay = ccMath.f_GetTotalDaysByTime(nowTimeServer) - ccMath.f_GetTotalDaysByTime(openServerTime) + 1 ; 
        GameParamDT param = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(94);
        int limitDay = param.iParam1;

        if (openServerDay<=limitDay)
            if ((StarDay >= 0 && StarDay <= limitDay) && (EndDay >= StarDay && EndDay <= limitDay))
            {
                if (openServerDay>=StarDay && openServerDay < EndDay)
                    return true;
            }
        if(openServerDay>=limitDay)
        if(StarDay >= 20200101 && EndDay >= 20200101)
        {
               
            string start = StarDay.ToString(); //TsuCode
            string end = EndDay.ToString();
            int sYear = int.Parse(start.Substring(0, 4));
            int sMonth = int.Parse(start.Substring(4, 2));
            int sDay = int.Parse(start.Substring(6, 2));
            DateTime startTime = new DateTime(sYear, sMonth, sDay); //TsuCode
            //DateTime startTime = new DateTime(int.Parse(start.Substring(0, 4)), int.Parse(start.Substring(4, 2)), int.Parse(start.Substring(6, 2))); //TsuCode
            long sTime = ccMath.DateTime2time_t(startTime);

            DateTime endTime = new DateTime(int.Parse(end.Substring(0, 4)), int.Parse(end.Substring(4, 2)), int.Parse(end.Substring(6, 2)));
            long eTime = ccMath.DateTime2time_t(endTime);
            if (nowTimeServer >= sTime && nowTimeServer <= eTime) return true;
        }
        ///////////
        return false;
    }
    
    public static int getYear(int day)
    {
        int openServerTime = Data_Pool.m_SevenActivityTaskPool.OpenSeverTime;
        int nowTime = GameSocket.GetInstance().f_GetServerTime();
        int openServerDay = ccMath.f_GetTotalDaysByTime(nowTime) - ccMath.f_GetTotalDaysByTime(openServerTime) + 1;
        GameParamDT param = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(94);
        int limitDay = param.iParam1;

        if (openServerDay <= limitDay)
            if ((day >= 0 && day <= limitDay))
            {
                int time = openServerTime + (day * 86400) - 86400;
                int year = ccMath.time_t2DateTime(time).Year;
                return year;
            }
        if (openServerDay >= limitDay)
            if (day >= 20200101 && day >= 20200101)
            {
                string strDay = day.ToString(); //TsuCode
                int sYear = int.Parse(strDay.Substring(0, 4));
                //int sMonth = int.Parse(start.Substring(4, 2));
                //int sDay = int.Parse(start.Substring(6, 2));
                return sYear;
            }
        return 0;
    }
    public static int getMonth(int day)
    {
        int openServerTime = Data_Pool.m_SevenActivityTaskPool.OpenSeverTime;
        int nowTime = GameSocket.GetInstance().f_GetServerTime();
        int openServerDay = ccMath.f_GetTotalDaysByTime(nowTime) - ccMath.f_GetTotalDaysByTime(openServerTime) + 1;
        GameParamDT param = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(94);
        int limitDay = param.iParam1;

        if (openServerDay <= limitDay)
            if ((day >= 0 && day <= limitDay))
            {
                int time = openServerTime + (day * 86400) - 86400;
                int month = ccMath.time_t2DateTime(time).Month;
                MessageBox.ASSERT("YEAR: " + month);
                return month;
            }
        if (openServerDay >= limitDay)
            if (day >= 20200101 && day >= 20200101)
            {
                string strDay = day.ToString(); //TsuCode
                int sMonth = int.Parse(strDay.Substring(4, 2));
                //int sDay = int.Parse(start.Substring(6, 2));
                return sMonth;
               
            }
        return 0;

    }
    public static int getDay(int day)
    {
        int openServerTime = Data_Pool.m_SevenActivityTaskPool.OpenSeverTime;
        int nowTime = GameSocket.GetInstance().f_GetServerTime();
        int openServerDay = ccMath.f_GetTotalDaysByTime(nowTime) - ccMath.f_GetTotalDaysByTime(openServerTime) + 1;
        GameParamDT param = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(94);
        int limitDay = param.iParam1;

        if (openServerDay <= limitDay)
            if ((day >= 0 && day <= limitDay))
            {
                int time = openServerTime + (day * 86400) - 86400;
                int d = ccMath.time_t2DateTime(time).Day;
                return d;
            }
        if (openServerDay >= limitDay)
            if (day >= 20200101 && day >= 20200101)
            {
                string strDay = day.ToString(); //TsuCode
                int sDay = int.Parse(strDay.Substring(6, 2));
                return sDay;
            }
        return 0;
    }
    #endregion TsuCode - Func
    public static int[] int2Arr(int n) //TsuCode
    {
        if (n == 0) return new int[1] { 0 };

        var digits = new List<int>();

        for (; n != 0; n /= 10)
            digits.Add(n % 10);

        var arr = digits.ToArray();
        Array.Reverse(arr);
        return arr;
    }
    public static int f_GetDayCreatAcc()
    {
        int day = 1;
        int second_a = Data_Pool.m_UserData.m_CreateTime - Data_Pool.m_SevenActivityTaskPool.OpenSeverTime;// số s server tới ngày tạo acc
        int second_b = second_a % 86400;// số s dư so với 1 ngày
        day = ((GameSocket.GetInstance().f_GetServerTime() - Data_Pool.m_UserData.m_CreateTime - second_b) / 86400) + 1;
        return day;
    }

    public static int f_GetZeroTimestamp(long time)
    {
        DateTime data = ccMath.time_t2DateTime(time);
        DateTime startTime = new DateTime(data.Year, data.Month, data.Day); //TsuCode
        return (int)ccMath.DateTime2time_t(startTime);
    }

    public static bool f_CheckOpenServerDay(int day)
    {
        int openServerTime = Data_Pool.m_SevenActivityTaskPool.OpenSeverTime;
        int nowTime = GameSocket.GetInstance().f_GetServerTime();
        int openServerDay = ccMath.f_GetTotalDaysByTime(nowTime) - ccMath.f_GetTotalDaysByTime(openServerTime) + 1;
        int limitDay = day;
        if (openServerDay >= limitDay) return true;
        return false;
    }
}