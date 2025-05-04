using UnityEngine;
using System.Collections;



public class ResourceTools
{
    
    public static string f_GetLocalMainPath()
    {
#if UNITY_IPHONE
        //if (GloData.glo_iVer == 100012)
        //return Application.streamingAssetsPath + "/" + GloData.glo_ProName + "/";
#endif
        return Application.persistentDataPath + "/" + GloData.glo_ProName;
    }

    public static string f_GetStreamingMainPath()
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        return Application.streamingAssetsPath + "/" + GloData.glo_ProName;
#elif UNITY_IPHONE
        return Application.streamingAssetsPath + "/" + GloData.glo_ProName;
#elif UNITY_ANDROID
        return Application.dataPath + "!assets/" + GloData.glo_ProName;
#endif
    }

    public static string f_GetServerMainPath()
    {
        string ppSQL = "";
#if UNITY_WEBPLAYER
        ppSQL = "WebPlayer";
#elif UNITY_ANDROID
        ppSQL = "Android";
#elif UNITY_IPHONE
        ppSQL = "IOS";
#else
        ppSQL = "Windows";
#endif
        return GloData.glo_strCDNResource + ppSQL;
    }

	public static void f_InitURL()
	{
		GloData.glo_strCDNResource = "https://" + GloData.glo_strCDNServer + "/" + GloData.glo_ProName + "/assetbundles/";
		GloData.glo_strLoadVer = "https://" + GloData.glo_strHttpServer + "/" + GloData.glo_ProName + "/ver/LoadVer.bytes";
		GloData.glo_strLoadAllSC = "https://" + GloData.glo_strHttpServer + "/" + GloData.glo_ProName + "/multiVer/";
		GloData.glo_strSaveLog = "https://" + GloData.glo_strHttpServer + "/" + GloData.glo_ProName + "/Log/SaveLog.php";
		GloData.glo_strNotice = "https://" + GloData.glo_strHttpServer + "/" + GloData.glo_ProName + "/GameService/Notice.bytes";
		GloData.glo_strGMInfor = "https://" + GloData.glo_strHttpServer + "/" + GloData.glo_ProName + "/GameService/GMInfor.bytes";
		GloData.glo_strCheckPay = "https://" + GloData.glo_strHttpServer + "/" + GloData.glo_ProName + "/GameService/CheckPay.txt"; //TsuCode - new checkPay - txt
		GloData.glo_strNewApkPath = "https://" + GloData.glo_strHttpServer + "/" + GloData.glo_ProName;
	}

    public static void f_UpdateURL()
    {
#if W1 || W2
        string strNewProName = string.Format("{0}/{1}", GloData.glo_ProName, GloData.glo_iVer);

        GloData.glo_strLoadVer = GloData.glo_strLoadVer.Replace(GloData.glo_ProName, strNewProName);
        GloData.glo_strLoadAllSC = GloData.glo_strLoadAllSC.Replace(GloData.glo_ProName, strNewProName);

        GloData.glo_strCDNResource = GloData.glo_strCDNResource.Replace(GloData.glo_ProName, strNewProName);

        GloData.glo_strSaveLog = GloData.glo_strSaveLog.Replace(GloData.glo_ProName, strNewProName);

        GloData.glo_strNotice = GloData.glo_strNotice.Replace(GloData.glo_ProName, strNewProName);
        GloData.glo_strGMInfor = GloData.glo_strGMInfor.Replace(GloData.glo_ProName, strNewProName);
#endif
    }


    public static string f_CreateNoticeUrl()
    {
#if W1
        string strNoticeName = string.Format("Notice{0}.bytes", glo_Main.GetInstance().m_SDKCmponent.f_GetSdkChannelType());
        GloData.glo_strNotice = GloData.glo_strNotice.Replace("Notice.bytes", strNoticeName);
#endif
        return GloData.glo_strNotice;
    }
}