using UnityEngine;
using System.Collections;

public class DownLoadNewApk : UIFramwork
{

    protected override void f_InitMessage()
    {
        base.f_InitMessage();

        f_RegClickEvent("BtnOpenUrl", OnBtnOpenUrl);
    }


    #region 更新APK

        /// <summary>
        /// 选择服务器名字消息
        /// </summary>
        /// <param name="msg">服务器名字</param>
    private void OnDownLoadApk(object obj)
    {
        string strURL = (string)obj;

        f_GetObject("DownLoadNewApk").SetActive(true);

    }

    private void OnBtnOpenUrl(GameObject go, object obj1, object obj2)
    {
        string strUrl = "";

#if UNITY_IPHONE
        //有新版本需要更新   
        strUrl = string.Format("{0}DownLoadNewApk.php?Chanel={1}&Ver={2}&Pack={3}", GloData.glo_strNewApkPath, glo_Main.GetInstance().m_SDKCmponent.f_GetSdkChannelType(), GloData.glo_iVer, GloData.glo_iPackType);
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR || UNITY_ANDROID
        //有新版本需要更新   
        if (glo_Main.GetInstance().m_SDKCmponent.IsChannel)
        {// 第三平台登陆注册
            if (GloData.glo_iPackType == 0)
            {
                //strUrl = GloData.glo_strNewApkPath + "/" + GloData.glo_iVer + "/APKKK/" + GloData.m_apkName + glo_Main.GetInstance().m_SDKCmponent.f_GetSdkChannelType() + ".apk";
                strUrl = GloData.glo_strNewApkPath + "/" + GloData.glo_iVer + "/APKKK/" + GloData.m_apkName + glo_Main.GetInstance().m_SDKCmponent.f_GetSdkChannelType() + ".html";
            }
            else
            {
                //strUrl = GloData.glo_strNewApkPath + "/" + GloData.glo_iVer + "/APKKK/" + GloData.m_apkName + glo_Main.GetInstance().m_SDKCmponent.f_GetSdkChannelType() + "full.apk";
                strUrl = GloData.glo_strNewApkPath + "/" + GloData.glo_iVer + "/APKKK/" + GloData.m_apkName + glo_Main.GetInstance().m_SDKCmponent.f_GetSdkChannelType() + "full.html";
            }
        }
        else
        {//NoChannelSDK
            if (GloData.glo_iPackType == 0)
            {
                //strUrl = GloData.glo_strNewApkPath + "/" + GloData.glo_iVer + "/APKKK/"+ GloData.m_apkName +"run.apk";
                strUrl = GloData.glo_strNewApkPath + "/" + GloData.glo_iVer + "/APKKK/" + GloData.m_apkName + "run.html";
            }
            else
            {
                //strUrl = GloData.glo_strNewApkPath + "/" + GloData.glo_iVer + "/APKKK/" + GloData.m_apkName + "runfull.apk";
                strUrl = GloData.glo_strNewApkPath + "/" + GloData.glo_iVer + "/APKKK/" + GloData.m_apkName + "runfull.html";
            }
        }
#endif
MessageBox.DEBUG("Update new version" + strUrl);
        Application.OpenURL(strUrl);
    }

    #endregion


}
