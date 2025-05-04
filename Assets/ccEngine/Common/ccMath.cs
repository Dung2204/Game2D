using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
/// <summary>
/// 数学计算工具类
/// </summary>
public class ccMath
{
    //public static int f_CreateNewId()//int iNpcIndexScId, int iBuildKeyId)
    //{
    //    string ppSQL = "";
    //    int iKeyId = 0;        

    //    int index = Random.Range(0, 1000);
    //    ppSQL = System.DateTime.Now.ToString("ddhhmm") + index;
    //    iKeyId = ccMath.atoi(ppSQL);

    //    return iKeyId;

    //}
    /// <summary>
    /// 结构体（天、小时、分钟）
    /// </summary>
    public struct ccSecToTime
    {
        public string Day;
        public string Hour;
        public string Minute;
        public ccSecToTime(string _Day, string _Hour, string _Minute)
        {
            Day = _Day;
            Hour = _Hour;
            Minute = _Minute;
        }
    }
    /// <summary>
    /// key序号
    /// </summary>
    private static int iKeyIndex = 100000;
    /// <summary>
    /// 创建并获取新key id
    /// </summary>
    /// <returns></returns>
    public static int f_CreateKeyId()
    {
        return ++iKeyIndex;

        //string ppSQL = "";
        //int iKeyId = 0;

        //int index = iKeyIndex;// Random.Range(0, 1000);
        //ppSQL = System.DateTime.Now.ToString("hhmmss") + index;
        //iKeyId = ccMath.atoi(ppSQL);

        //return iKeyId;
    }

    /// <summary>
    /// 字符串转换成整形
    /// </summary>
    /// <param name="ppSQL">待转换的字符串</param>
    /// <returns></returns>
    public static int atoi(string ppSQL)
    {
        if (ppSQL.Length == 0)
        {
            return 0;
        }
        if (ppSQL == "")
        {
            return 0;
        }
        int fTTT = 0;
        try
        {
            fTTT = System.Convert.ToInt32(ppSQL);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Lỗi chuyển đổi dữ liệu, tên dữ liệu：" + ppSQL + " " + e.ToString());
            return 0;
        }
        return fTTT;
    }
    /// <summary>
    /// 字符串转成浮点型
    /// </summary>
    /// <param name="ppSQL">待转换的字符串</param>
    /// <returns></returns>
    public static float atof(string ppSQL)
    {
        if (ppSQL.Length == 0)
        {
            return 0;
        }
        if (ppSQL == "")
        {
            return 0;
        }
        float fTTT = 0;
        try
        {
            fTTT = float.Parse(ppSQL);
        }
        catch
        {
            Debug.LogError("Lỗi chuyển đổi dữ liệu, tên dữ liệu：" + ppSQL);
            return 0;
        }
        if (fTTT < 0)
        {
            return fTTT;
        }
        return fTTT;
    }

    /// <summary>
    /// 字符串转成Long
    /// </summary>
    /// <param name="ppSQL">待转换的字符串</param>
    /// <returns></returns>
    public static long atol(string ppSQL)
    {
        if (ppSQL.Length == 0)
        {
            return 0;
        }
        if (ppSQL == "")
        {
            return 0;
        }
        long fTTT = 0;
        try
        {
            fTTT = long.Parse(ppSQL);
        }
        catch
        {
            Debug.LogError("Lỗi chuyển đổi dữ liệu, tên dữ liệu：" + ppSQL);
            return 0;
        }
        if (fTTT < 0)
        {
            return fTTT;
        }
        return fTTT;
    }

    /// <summary>
    /// 字符串分割，并把分割后的字符串数组转换成整形数组
    /// </summary>
    /// <param name="ppSQL">需分割的字符串</param>
    /// <param name="strSign">用于分割的字符串</param>
    /// <returns></returns>
    public static int[] f_String2ArrayInt(string ppSQL, string strSign)
    {
        string[] aData = ppSQL.Split(new string[] { strSign }, System.StringSplitOptions.None);
        int[] aRetData = new int[aData.Length];
        for (int aaa = 0; aaa < aData.Length; aaa++)
        {
            aRetData[aaa] = ccMath.atoi(aData[aaa]);
        }

        return aRetData;
    }

    /// <summary>
    /// 字符串分割，并把分割后的字符串数组转换成浮点型数组
    /// </summary>
    /// <param name="ppSQL">需分割的字符串</param>
    /// <param name="strSign">用于分割的字符串</param>
    /// <returns></returns>
    public static float[] f_String2ArrayFloat(string ppSQL, string strSign)
    {
        string[] aData = ppSQL.Split(new string[] { strSign }, System.StringSplitOptions.None);
        float[] aRetData = new float[aData.Length];
        for (int aaa = 0; aaa < aData.Length; aaa++)
        {
            aRetData[aaa] = ccMath.atof(aData[aaa]);
        }

        return aRetData;
    }

    /// <summary>
    /// 字符串分割成字符数组
    /// </summary>
    /// <param name="ppSQL">需分割的字符串</param>
    /// <param name="strSign">用于分割的字符串</param>
    /// <returns></returns>
    public static string[] f_String2ArrayString(string ppSQL, string strSign)
    {
        return ppSQL.Split(new string[] { strSign }, System.StringSplitOptions.None);
    }


    /// <summary>
    /// 克隆对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="serializableObject"></param>
    /// <returns></returns>
    public static T CloneObject<T>(T serializableObject)
    {
        object objCopy = null;

        MemoryStream stream = new MemoryStream();
        BinaryFormatter binFormatter = new BinaryFormatter();
        binFormatter.Serialize(stream, serializableObject);
        stream.Position = 0;
        objCopy = (T)binFormatter.Deserialize(stream);
        stream.Close();
        return (T)objCopy;

    }

    public static T CloneArray<T>(T RealObject)
    {
        using (Stream objectStream = new MemoryStream())
        {
            System.Runtime.Serialization.IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(objectStream, RealObject);
            objectStream.Seek(0, SeekOrigin.Begin);
            return (T)formatter.Deserialize(objectStream);
        }
    }


    /// <summary>
    /// 获得0-1的随机命中
    /// </summary>
    /// <param name="fRand">fRand应在0到1之间，当随机数小于fRand时返回真，否则返回假</param>
    /// <returns></returns>
    public static bool f_CheckRandIsOK_0_1(float fRand)
    {
        if (fRand >= 1)
        {
            return true;
        }

        float fRnd = (float)(Random.Range(0f, 1f));

        if (fRnd < fRand)
        {
            return true;
        }
        return false;
    }


    /// <summary>
    /// 获得0-100的随机命中
    /// </summary>
    /// <param name="iRand">iRand应在0到100之间，当随机数小于iRand时返回真，否则返回假</param>
    /// <returns></returns>
    public static bool f_CheckRandIsOK_0_100(int iRand)
    {
        if (iRand >= 100)
        {
            return true;
        }
        int iRnd = (Random.Range(0, 100));

        if (iRnd < iRand)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 获得点Pos周围fRandRange范围内的随机坐标
    /// </summary>
    /// <param name="Pos">中心点</param>
    /// <param name="fRandRange">以Pos为中心点的正方形宽</param>
    /// <param name="bNeedCheck">是否检测随机</param>
    /// <returns></returns>
    public static Vector3 f_GetRandIsPosRang(Vector3 Pos, float fRandRange, bool bNeedCheck = true)
    {
        float fRndX = (float)(Random.Range(-1 * fRandRange, fRandRange));
        float fRndY = (float)(Random.Range(-1 * fRandRange, fRandRange));
        if (bNeedCheck)
        {
            if (f_CheckRandIsOK_0_1(0.5f))
            {
                Pos.x = Pos.x + fRndX;
                Pos.z = Pos.z + fRndY;
            }
            else
            {
                Pos.z = Pos.z + fRndX;
                Pos.x = Pos.x + fRndY;
            }
        }
        else
        {
            Pos.x = Pos.x + fRndX;
            Pos.z = Pos.z + fRndY;
        }

        return Pos;
    }
    /// <summary>
    /// 获取指定范围的随机整形值，包含头不含尾
    /// </summary>
    /// <param name="iStart">随机数开始值</param>
    /// <param name="iEnd">随机数结尾值（不包含）</param>
    /// <returns></returns>
    public static int f_GetRand(int iStart, int iEnd)
    {
        return Random.Range(iStart, iEnd);
    }
    /// <summary>
    /// 获取指定范围的随机浮点数值，包含头不含尾
    /// </summary>
    /// <param name="iStart">随机数开始值</param>
    /// <param name="iEnd">随机数结尾值（不包含）</param>
    /// <returns></returns>
    public static float f_GetRand(float iStart, float iEnd)
    {
        return (float)(Random.Range(iStart, iEnd));
    }

    /// <summary>
    /// 计较两条线段长度大小
    /// 线段1大于线段2则返回真，否则返回假
    /// </summary>
    /// <param name="oP1">线段1坐标头</param>
    /// <param name="oP2">线段1坐标尾</param>
    /// <param name="nP1">线段2坐标头</param>
    /// <param name="nP2">线段2坐标尾</param>
    /// <returns></returns>
    public static int isEnlarge(Vector2 oP1, Vector2 oP2, Vector2 nP1, Vector2 nP2)
    {
        float leng1 = Mathf.Sqrt((oP1.x - oP2.x) * (oP1.x - oP2.x) + (oP1.y - oP2.y) * (oP1.y - oP2.y));
        float leng2 = Mathf.Sqrt((nP1.x - nP2.x) * (nP1.x - nP2.x) + (nP1.y - nP2.y) * (nP1.y - nP2.y));
        if (leng1 > leng2)
        {
            return 1;
        }
        else if (leng1 == leng2)
        {
            return 0;
        }
        else
        {
            return -1;
        }
    }

    /// <summary>
    /// 判断点是否在椭圆内
    /// </summary>
    /// <param name="fTestX">待检测点x坐标</param>
    /// <param name="fTestY">待检测点y坐标</param>
    /// <param name="fX">椭圆中心点x坐标</param>
    /// <param name="fY">椭圆中心点y坐标</param>
    /// <param name="fR">椭圆的最大半径(实际就是将圆沿y轴压扁变为椭圆的源圆的半径 R = 119)</param>
    /// <param name="fSy">将圆沿y轴压扁变为椭圆时候的比例(长119 高68 则比例为 68/119 = 0.75)</param>
    /// <returns></returns>
    public static bool PointIsInEllipse(float fTestX, float fTestY, float fX, float fY, float fR, float fSy)
    {
        Vector2 TestPos = new Vector2(fTestX, fTestY);
        Vector2 Pos = new Vector2(fX, fY);
        Vector2 tPos = TestPos - Pos;
        tPos.y /= fSy;
        if (tPos.magnitude < fR)
        {
            return true;
        }
        return false;
    }


    /// <summary>
    /// 检测Pos2在Pos1的左还是右  -1左 0中 1右
    /// </summary>
    /// <param name="Pos1">坐标参照点</param>
    /// <param name="Pos2">坐标2</param>
    /// <returns></returns>
    public static int f_CheckPointIsLeftRight(Vector3 Pos1, Vector3 Pos2)
    {
        if (Pos1.x < Pos2.x)
        {
            return 1;
        }
        else if (Pos1.x == Pos2.x)
        {
            return 0;
        }
        else
            return -1;
    }

    /// <summary>
    /// 检测Pos2在Pos1的方向 -1错误 0右上 1右下 2左下 3左上 4右 5左 6上 7下
    /// </summary>
    /// <param name="Pos1">坐标参照点</param>
    /// <param name="Pos2">坐标2</param>
    /// <returns></returns>
    public static int f_CheckPointIsWay(Vector3 Pos1, Vector3 Pos2)
    {
        int i = 0;
        if (Pos1.x < Pos2.x)
        {
            i = 10;
        }
        else if (Pos1.x > Pos2.x)
        {
            i = 20;
        }
        if (Pos1.y < Pos2.y)
        {
            i = i + 1;
        }
        else if (Pos1.y > Pos2.y)
        {
            i = i + 2;
        }
        //0右上 1右下 2左下 3左上 
        if (i == 11)
        {
            return 1;
        }
        else if (i == 12)
        {
            return 0;
        }
        else if (i == 21)
        {
            return 2;
        }
        else if (i == 22)
        {
            return 3;
        }

        else if (i == 10)
        {
            return 4;
        }
        else if (i == 20)
        {
            return 5;
        }

        else if (i == 1)
        {
            return 7;
        }
        else if (i == 2)
        {
            return 6;
        }

        return -1;
    }


    /// <summary>
    /// 相对X轴夹角
    /// </summary>
    /// <param name="Pos">点</param>
    /// <returns></returns>
    public static float f_Angle2X(Vector3 Pos)
    {
        // 计算两者之间角度
        return Mathf.Atan2(Pos.x, Pos.z) * Mathf.Rad2Deg;
    }

    /// <summary>
    ///  计算夹角的角度 0~360  
    /// </summary>
    /// <param name="from_">点1</param>
    /// <param name="to_">点2</param>
    /// <returns></returns>
    public static float f_Angle_360(Vector3 from_, Vector3 to_)
    {
        Vector3 v3 = Vector3.Cross(from_, to_);
        if (v3.z > 0)
            return Vector3.Angle(from_, to_);
        else
            return 360 - Vector3.Angle(from_, to_);
    }
    /// <summary>
    /// 获取文件名(待修改)
    /// </summary>
    /// <param name="strPath"></param>
    /// <param name="strSign"></param>
    /// <returns></returns>
    public static string f_GetFileName(string strPath, string strSign = "/")
    {
        string ppSQL = "";

        return ppSQL;
    }
    /// <summary>
    /// 将长整型时间转换成System.DateTime格式
    /// </summary>
    /// <param name="iTime">长整型时间</param>
    /// <returns></returns>
    public static System.DateTime time_t2DateTime(long iTime)
    {
        System.DateTime dt = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Unspecified).AddSeconds(iTime);
        //dt = dt.AddHours(8);
        //TsuCode
        dt = dt.AddHours(7);
        //
        return dt;
    }

    /// <summary>
    /// 根据时间戳得到流逝的天数 1970.01.01
    /// </summary>
    /// <param name="time">流逝时间 秒数</param>
    /// <returns></returns>
    public static int f_GetTotalDaysByTime(long time)
    {
        //return ((int)time + 28800) / 86400;
        //TsuCode
        return ((int)time + 25200) / 86400;
        //
    }

    /// <summary>
    /// 根据时间戳得到DayOfWeek 1970.01.01
    /// </summary>
    /// <param name="time">流逝时间 秒数</param>
    /// <returns></returns>
    public static System.DayOfWeek f_GetDayOfWeekByTime(long time)
    {
        int day = f_GetTotalDaysByTime(time);
        int result = (day + (int)System.DayOfWeek.Thursday) % ((int)System.DayOfWeek.Saturday + 1);
        return (System.DayOfWeek)result;
    }

    /// <summary>
    /// 根据时间戳得到当天流逝的秒数
    /// </summary>
    /// <param name="time">流逝时间 秒数</param>
    /// <returns></returns>
    public static int f_GetSecondOfDayByTime(long time)
    {
        //return ((int)time + 28800) % 86400;
        //Tsucode
        return ((int)time + 25200) % 86400;
        //
    }

    /// <summary>
    /// 将System.DateTime转换成长整型时间格式
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static long DateTime2time_t(System.DateTime dateTime)
    {
        long time_t;
        System.DateTime dt1 = new System.DateTime(1970, 1, 1, 0, 0, 0);
        System.TimeSpan ts = dateTime - dt1;
        //time_t = ts.Ticks / 10000000 - 28800;
        //Tsucode
        time_t = ts.Ticks / 10000000 - 25200;
        //
        return time_t;
    }
    /// <summary>
    /// 将指定参数日期转换成长整型时间格式
    /// </summary>
    /// <param name="year">年</param>
    /// <param name="month">月</param>
    /// <param name="day">日</param>
    /// <param name="hour">小时</param>
    /// <param name="minute">分钟</param>
    /// <param name="second">秒</param>
    /// <returns></returns>
    public static long f_Time_String2Int(int year, int month, int day, int hour, int minute, int second)
    {
        long time_t;
        System.DateTime dt0 = new System.DateTime(1970, 1, 1, 0, 0, 0);
        System.DateTime dt1 = new System.DateTime(year, month, day, hour, minute, second);
        System.TimeSpan ts = dt1 - dt0;
        //time_t = ts.Ticks / 10000000 - 28800;
        //Tsucode
        time_t = ts.Ticks / 10000000 - 25200;
        //
        return time_t;
    }


    /// <summary>
    /// int秒转换成"1:10:19"
    /// </summary>
    /// <returns></returns>
    /// <param name="iSecTime">秒数</param>
    /// <param name="bAddZero">true自动补零 false不进行自动补零</param>
    /// <returns></returns>
    public static string f_Time_Int2String(int iSecTime, bool bAddZero = true)
    {
        System.TimeSpan ts;
        if (iSecTime < 0)
        {
            return "ERO";
        }
        else
        {
            ts = new System.TimeSpan(0, 0, iSecTime);
        }
        //string strPlayerTime = (int)ts.TotalHours + "小时" + ts.Minutes + "分钟" + ts.Seconds + "秒";
        string ppSQL = "";
        string strHours = ts.Hours.ToString();

        if ((ts.Hours < 10) && bAddZero == true)
        {
            ppSQL = "0" + ts.Hours;
        }
        else
        {
            ppSQL = ts.Hours.ToString();
        }

        string strMin = ts.Minutes.ToString();
        if ((ts.Minutes < 10) && bAddZero == true)
        {
            strMin = "0" + ts.Minutes.ToString();
            if (ppSQL != "")
            {
                ppSQL = ppSQL + ":" + strMin;
            }
            else
            {
                ppSQL = strMin;
            }
        }
        else
        {
            if (ppSQL != "")
            {
                ppSQL = ppSQL + ":" + strMin;
            }
            else
            {
                ppSQL = strMin;
            }
        }


        string strSec = ts.Seconds.ToString();
        if ((ts.Seconds < 10) && bAddZero == true)
        {
            strSec = "0" + ts.Seconds.ToString();
            if (ppSQL != "")
            {
                ppSQL = ppSQL + ":" + strSec;
            }
            else
            {
                ppSQL = strSec;
            }
        }
        else
        {
            if (ppSQL != "")
            {
                ppSQL = ppSQL + ":" + ts.Seconds.ToString();
            }
            else
            {
                ppSQL = strSec;
            }
        }

        return ppSQL;
    }

    /// <summary>
    /// 秒转换时间字符串，以最大的为主，其余后面单位舍去
    /// </summary>
    /// <param name="iSecTime">秒</param>
    /// <returns></returns>
    public static string f_Time_2String(int iSecTime)
    {
        System.TimeSpan ts = new System.TimeSpan(0, 0, iSecTime);

        string _time = "";
        if (ts.Days > 0)
        {
            _time = ts.Days + " ngày";
        }
        else
        {
            if (ts.Hours > 0)
            {
                _time = ts.Hours + " giờ";
            }
            else
            {
                if (ts.Minutes > 0)
                {
                    _time = ts.Minutes + " phút";
                }
                else
                {
                    _time = "1 phút";
                }
            }
        }
        return _time;
    }

    /// <summary>
    /// 策划配置的时间来转换    20180216
    /// </summary>
    /// <param name="Time"></param>
    /// <returns></returns>
    public static int f_Data2Int(int Time)
    {
        if (Time == 0)
            return 0;
        if (Time.ToString().Length != 8)
        {
            MessageBox.ASSERT("Lỗi thời gian " + Time);
        }
        string szTime = Time.ToString();

        int Year = int.Parse(szTime.Substring(0, 4));

        int Mon = int.Parse(szTime.Substring(4, 2));

        int Day = int.Parse(szTime.Substring(6, 2));

        return (int)ccMath.f_Time_String2Int(Year, Mon, Day, 0, 0, 0);
    }

    /*
    public static Vector3 f_WorldPos2NguiPos ( Vector3 WorldPos )
    {
        Vector3 pos = Camera.main.WorldToScreenPoint( WorldPos );
        Vector3 NguiPos = UICamera.currentCamera.ScreenToWorldPoint( pos );
        NguiPos.z = 0;

        return NguiPos;
    }
*/

    /// <summary>
    /// 
    /// </summary>
    /// <param name="iPos"></param>
    /// <returns></returns>
    public static int f_GetToInt32(int iPos)
    {
        int a = 0;
        string ppSQL = "";      //"0000000000";
        for (int i = 1; i <= 10 - iPos; i++)
        {
            ppSQL = ppSQL + "0";
        }
        ppSQL = ppSQL + "1";
        int iLen = ppSQL.Length;
        for (int i = 0; i < 10 - iLen; i++)
        {
            ppSQL = ppSQL + "0";
        }
        return System.Convert.ToInt32(ppSQL, 2);
    }


    /// <summary>
    /// 拆分Class获得相应的子Class对应的职业Id
    /// </summary>
    /// <param name="iData"></param>
    /// <returns></returns>
    public static ArrayList f_GetTargetClasses(int iData)
    {
        string ppSQL = System.Convert.ToString(iData, 2);
        ArrayList aList = new ArrayList();
        int a = 0;
        for (int i = 1; i <= ppSQL.Length; i++)
        {
            string strChr = ppSQL.Substring(ppSQL.Length - i, 1);
            if (strChr == "1")
            {
                a = f_GetToInt32(i);
                aList.Add(a);
            }
        }
        return aList;
    }



    /// <summary>
    /// 检测字串里是否包含指定的字符
    /// </summary>
    /// <param name="strScrString"></param>
    /// <param name="strSign"></param>
    /// <returns></returns>
    public static bool f_CheckStringHaveSign(string strScrString, string strSign)
    {
        if (-1 == strScrString.IndexOf(strSign))
        {
            return false;
        }
        return true;
    }


    /// <summary>
    /// 字串換行, 使字串 "AAAAAA\r\nBBBBBB" 變成 "AAAAA" + '\n' + "BBBBB"
    /// </summary>
    /// <param name="oris"></param>
    /// <returns></returns>
    public static string f_String_rn2n(string oris)
    {
        //string outs = oris;
        //string fronts, backs;
        //if (oris != "")
        //{
        //    int pos = oris.IndexOf("\\r\\n");
        //    if (pos != -1)
        //    {	// 字串內含有 "\r\n"
        //        fronts = oris.Substring(0, pos);
        //        backs = oris.Substring(pos + ROW_TEXT.Length, oris.Length - pos - ROW_TEXT.Length);
        //        outs = fronts + '\n' + StringRowDetect(backs);
        //    }
        //}
        return oris.Replace("\\r\\n", "\n");
    }


    public static Vector3 f_WorldPos2NguiPos(Vector3 WorldPos)
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(WorldPos);
        Vector3 NguiPos = UICamera.currentCamera.ScreenToWorldPoint(pos);
        NguiPos.z = 0;

        return NguiPos;
    }

    /// <summary>
    /// 获得当前NGUI摄像机的实际大小
    /// 等效下面的计算方法,注意aspectRatio对全局的影响
    ///  _fCameraWidth = 1920 * (float)tUIWidget.aspectRatio / 2000;
    ///  _fCameraHeight = 1080 * (float)tUIWidget.aspectRatio / 2000;        
    /// </summary>
    /// <param name="oUIRoot"></param>
    /// <returns></returns>
    public Vector3[] f_GetNGUICameraSize(GameObject oUIRoot)
    {
        UIPanel tUIPanel = oUIRoot.GetComponent<UIPanel>();
        Vector3[] corners = tUIPanel.worldCorners;

        return corners;
    }

    /// <summary>
	/// 获得字符串的字节长度 
	/// </summary>
	public static int f_GetStringBytesLength(string str)
    {
        return System.Text.Encoding.UTF8.GetByteCount(str);
    }

    /// <summary>
    /// 数字转中文
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static string NumberToChinese(int number)
    {
        return NumberToChinese(number.ToString());
    }

    public static string NumberToChinese(string str)
    {
		string res = str;
        //string res = string.Empty;
        // string schar = str.Substring(0, 1);
        // switch (schar)
        // {
            // case "1":
                // res = "1";
                // break;
            // case "2":
                // res = "2";
                // break;
            // case "3":
                // res = "3";
                // break;
            // case "4":
                // res = "4";
                // break;
            // case "5":
                // res = "5";
                // break;
            // case "6":
                // res = "6";
                // break;
            // case "7":
                // res = "7";
                // break;
            // case "8":
                // res = "8";
                // break;
            // case "9":
                // res = "9";
                // break;
            // default:
                // res = "0";
                // break;
        // }
        // if (str.Length > 1)
        // {
            // switch (str.Length)
            // {
                // case 2:
                // case 6:
                    // res += "10";
                    // break;
                // case 3:
                // case 7:
                    // res += "100";
                    // break;
                // case 4:
                    // res += "1000";
                    // break;
                // case 5:
                    // res += "10000";
                    // break;
                // default:
                    // res += "";
                    // break;
            // }
            // res += NumberToChinese(int.Parse(str.Substring(1, str.Length - 1)));
        // }
        return res;
    }

    public static string f_CheckStringIsEmptyOrNone(string str, string defaultStr)
    {
        if (str == null || str == "")
        {
            return defaultStr;
        }
        return str;
    }
}