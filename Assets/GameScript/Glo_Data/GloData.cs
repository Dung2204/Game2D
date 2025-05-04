

//#define  L1                     //内部测试服1
//#define  L2                     //内部测试服2（晓然）(在编译器里定义)
//#define  L3                    //内部测试服3
//#define  W1                    //外网测试服务器121.14.84.203
//#define LOCAL_DEV
using UnityEngine;
using System.Collections;

/// <summary>
/// 项目说明：
/// 项目名称：代号X
/// 工程开始时间                          2017-02-13
/// 
/// 內部管理后臺入口 http://192.168.0.227/DaiHaoX/
/// 全局数据定义
/// </summary>
public class GloData// : MonoBehaviour
{
    public static int glo_iVer = 100012;
    public static string glo_strVer = "Q0.";// + glo_iVer;

    /// <summary>
    /// 当前的资源包类型
    /// 0完全走外部资源加载 1整包资源
    /// czy这里用内部资源最好，省去开始下载一堆资源了
    /// </summary>
    public static int glo_iPackType = 1;


#if L1  //内部测试服1（刘洋）
    public static string glo_strSvrIP = "192.168.105.104";
#elif L2  //内部测试服2（晓然）
    public static string glo_strSvrIP = "192.168.0.208";
#elif L3
    public static string glo_strSvrIP = "192.168.105.68";
#elif W1
    public static string glo_strSvrIP = "192.168.20.6";
#elif W2

#if LOCAL_DEV
    public static string glo_strSvrIP = "127.0.0.1";
#else
    // public static string glo_strSvrIP = "123.31.11.246";
	// public static string glo_strSvrIP = "123.31.11.65";
	public static string glo_strSvrIP = "51.79.173.248";
    
#endif
    //public static string glo_strSvrIP = "149.28.154.70";

#else
    public static string glo_strSvrIP = "192.168.105.104";
#endif

    public static int glo_iSvrPort = 38001;
    /// <summary>
    /// 控制台是否输出Log
    /// </summary>
    public static bool m_bDebugLog = true;

    public static string m_apkName = "lzjs_";

    /// <summary>
    /// 全局DEBUG标识
    /// </summary>
    //public static bool m_bDebug = false;   


#if L1
    public static string glo_ProName = "DaiHaoX2";

    /// <summary>
    /// 渠道包标识，通过ServerInfor配置
    /// </summary>
    public static string m_PayChannel = "L1";

    public static string glo_strHttpServer = "192.168.105.104";
    public static string glo_strCDNServer = "192.168.105.104";
    public static string glo_strLoadVer = "http://" + glo_strHttpServer + "/" + glo_ProName + "/ver/LoadVer.bytes";
    public static string glo_strLoadAllSC = "http://" + glo_strHttpServer + "/" + glo_ProName + "/ver/";

    /// <summary>
    /// 平台登陆
    /// </summary>
    public static bool glo_bChanelLogin = false;
#elif L2
    public static string m_PayChannel = "L2";

    public static string glo_strHttpServer = "192.168.0.227";
    public static string glo_strCDNServer = "192.168.0.227";
    public static string glo_strLoadVer = "http://" + glo_strHttpServer + "/" + glo_ProName + "/ver/LoadVer.php";
    public static string glo_strLoadAllSC = "http://" + glo_strHttpServer + "/" + glo_ProName + "/ver/";

    /// <summary>
    /// 平台登陆
    /// </summary>
    public static bool glo_bChanelLogin = false;
#elif L3
    public static string m_PayChannel = "L3";

    public static string glo_strHttpServer = "192.168.0.227";
    public static string glo_strCDNServer = "192.168.0.227";
    public static string glo_strLoadVer = "http://" + glo_strHttpServer + "/" + glo_ProName + "/ver/LoadVer.php";
    public static string glo_strLoadAllSC = "http://" + glo_strHttpServer + "/" + glo_ProName + "/ver/";

    /// <summary>
    /// 平台登陆
    /// </summary>
    public static bool glo_bChanelLogin = false;
#elif W1
    public static string glo_ProName = "DaiHaoX2";
    public static string m_PayChannel = "W1";

    public static string glo_strHttpServer = "119.23.30.107";
    public static string glo_strCDNServer = "119.23.30.107";
    public static string glo_strLoadVer = "http://" + glo_strHttpServer + "/" + glo_ProName + "/ver/LoadVer.bytes";
    public static string glo_strLoadAllSC = "http://" + glo_strHttpServer + "/" + glo_ProName + "/ver/";

    /// <summary>
    /// 平台登陆
    /// </summary>
    public static bool glo_bChanelLogin = false;
#elif W2
    // public static string glo_ProName = "DaiHaoX2";
    //public static string glo_ProName = "threekingdoms";
    //public static string glo_ProName = "chess"; //local
    //public static string glo_ProName = "372"; 
    public static string m_PayChannel = "W2";


    //public static string glo_strHttpServer = "149.28.154.70";
    //public static string glo_strCDNServer = "149.28.154.70";
#if LOCAL_DEV
    public static string glo_ProName = "chess"; //local
    public static string glo_strHttpServer = "127.0.0.1:80";
    public static string glo_strCDNServer = "127.0.0.1:80";
#else
    public static string glo_ProName = "372";
    public static string glo_strHttpServer = "nextgenstudio.vn";
    public static string glo_strCDNServer = "nextgenstudio.vn";
#endif

    //public static string glo_strLoadVer = "https://" + glo_strHttpServer + "/" + glo_ProName + "/ver/LoadVer.bytes" + "?v=" + Random.Range(0, 100000);
    // public static string glo_strLoadVer = "https://" + glo_strHttpServer + "/" + glo_ProName + "/ver/LoadVer.txt" + "?v=" + Random.Range(0, 100000);
    //public static string glo_strLoadAllSC = "https://" + glo_strHttpServer + "/" + glo_ProName + "/ver/";
    //public static string glo_strLoadAllSC = "http://" + glo_strHttpServer + "/" + glo_ProName + "/multiVer/";
    public static string glo_strLoadVer = "https://" + glo_strHttpServer + "/" + glo_ProName + "/ver7hero/LoadVer.bytes" + "?v=" + Random.Range(0, 100000);
    public static string glo_strLoadAllSC = "https://" + glo_strHttpServer + "/" + glo_ProName + "/ver7hero/";

    /// <summary>
    /// 平台登陆
    /// </summary>
    public static bool glo_bChanelLogin = false;
#else

#endif
    public static string glo_strCDNResource = "https://" + glo_strCDNServer + "/" + glo_ProName + "/assetbundlesfake/";

    public static string glo_strSaveLog = "https://" + glo_strHttpServer + "/" + glo_ProName + "/Log/SaveLog.php";
    //public static string glo_strDeleteLog = "http://" + glo_strHttpServer + "/" + glo_ProName + "/Log/DeleteLog.php";

    public static string glo_strNotice = "https://" + glo_strHttpServer + "/" + glo_ProName + "/GameService/Notice.bytes" + "?v=" + Random.Range(0, 100000);
    public static string glo_strGMInfor = "https://" + glo_strHttpServer + "/" + glo_ProName + "/GameService/GMInfor.bytes";
    /// <summary>
    /// 检测支付状态
    /// czy检测是渠道是否开放了充值功能，打开充值界面会检测下，可以去掉这玩意本项目
    /// </summary>
    public static string glo_strCheckPay = "https://" + glo_strHttpServer + "/" + glo_ProName + "/GameService/CheckPay.txt" + "?v=" + Random.Range(0, 100000);

    /// <summary>
    /// 更新APK安装包
    /// </summary>
    public static string glo_strNewApkPath = "https://" + glo_strHttpServer + "/" + glo_ProName;

    public static string glo_strCheckYSDKPay = "http://api.lcatgame.com/v1/app/query_switch";

    public static int glo_iPingTime = 30;
    public static int glo_iRecvPingTime = 300;

    /// <summary>
    /// 系统管理员Id
    /// </summary>
    public static int glo_AdminUserId = 1000;

    /// <summary>
    /// 资源密码
    /// </summary>
    public static string glo_strResourcePwd = "DaiHaoX";

    /// <summary>
    /// HP高度
    /// </summary>
    public static float glo_fHpHeight = 0.52f;
    /// <summary>
    /// 新手引导   false  关闭  true  启动
    /// </summary>
    public static bool glo_StarGuidance = true;

    
    /// <summary>
    /// 最大日志存储量
    /// </summary>
    public static int glo_iMaxLogSize = 10;

    /// <summary>
    /// 0不自动上传LOG， 1自动上传LOG
    /// </summary>
    public static int glo_iAutoUpdateLog = 1;

    /// <summary>
    /// 自动上传LOG时间
    /// </summary>
    public static int glo_iAutoUpdateLogTime = 60;

    /// <summary>
    /// 是否有新的APK 0不存在，1存在
    /// </summary>
    public static int glo_iHaveNewApk = 0;

}