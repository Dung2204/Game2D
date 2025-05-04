using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
using System;
using System.IO;


public enum EM_DownType
{
    TEXT = 1,
    GameObject = 2,
    ObjectList = 3,
    BIN = 4,
}


//public class DownLoadResult
//{
//    public ResourceDT m_ResourceDT;
//    public string m_strText;
//    public byte[] m_aData;
//    public UnityEngine.Object[] m_aObj;
//    public GameObject m_Obj;

//    public DownLoadResult(ResourceDT tResourceDT, string strText, GameObject tObj, UnityEngine.Object[] aObj, byte[] aData)
//    {
//        m_ResourceDT = tResourceDT;
//        m_strText = strText;
//        m_Obj = tObj;
//        m_aObj = aObj;
//        m_aData = aData;
//    }

//}

public class DownloadManager
{
    private ccCallback _Callback_DownLoadComplete = null;
    //private List<WWW> _aWWWList = new List<WWW>();
    private List<DownloadItem> m_aWaitDownLoad = new List<DownloadItem>();
    private List<DownloadItem> m_aDownLoading = new List<DownloadItem>();
    private List<DownloadItem> tmpList = new List<DownloadItem>();

    private List<DownloadItem> _aFailList = new List<DownloadItem>();
    private List<DownloadItem> _aComplete = new List<DownloadItem>();
   
    public int totalLoaded = 0;
#if UNITY_WEBPLAYER
    public int maxLoading = 5;
#elif UNITY_ANDROID
    public int maxLoading = 5;
#else
    public int maxLoading = 1;
#endif

    //public List<AssetBundle> m_aAssetBundle = new List<AssetBundle>();

    class DownloadItem 
    {        
        private int _iTimeOut = 200;
        public WWW www = null;
        public string m_strUrl;
        public string m_strLocalFile;
        public ccCallback m_ccCallback;
        private object _CallbackData;

        public DownloadItem(string strUrl, string strLocalFile, ccCallback tccCallback, object tCallbackData)
        {
            m_strLocalFile = strLocalFile;
            m_strUrl = strUrl;
            m_ccCallback = tccCallback;
            _CallbackData = tCallbackData;
        }

        /// <summary>
        /// 0未开始 1正在进行中 2加载成功 3失败
        /// </summary>
        private int m_iStatic = 0;
        public int retryTimes = 3;
        public string error;
        private float _time_start;

        /// <summary>
        /// 0未开始 1正在进行中 2加载成功 3失败
        /// </summary>
        public int f_GetStatic()
        {
            return m_iStatic;
        }

        public void Dispose()
		{
            //this.Dispose(true);
            //GC.SuppressFinalize(this);	
            this.www.Dispose();
            this.www = null;
            //_ab.Unload(false);

            m_ccCallback = null;
        }

        //protected virtual void Dispose(bool disposing)
        //{
        //    if (this.www != null)
        //    {
        //        if (disposing)
        //        {
        //        }
        //        //AssetBundleDestroyer.Add(this.ab);
        //        this.www.Dispose();
        //        this.www = null;

        //        m_ccCallback = null;
        //        m_oInputData = null;
        //    }
        //}
        private bool _bFail;
        public void f_Update()
        {
            if (this.www != null)
            {
                if (this.www.isDone)
                {
                    string ppSQL = www.error + " " + www.url;
                    if (!string.IsNullOrEmpty(this.www.error))
                    {
                        if (this.www.error.Contains("rewind wasn't possible"))
                        {
                            Debug.Log("http resonse error, change POST to GET. error:" + this.www.error);
                            //ServerRejectPostRequest = true;
                        }                        
                        if (!this.Retry())
                        {
                            this.error = ppSQL;
                            m_iStatic = 3;
                        }
                    }
                    else
                    {
                        m_iStatic = 2;
                    }
                }
                else
                {
                    _bFail = false;
                    if (this.www.progress != 0f)
                    {
						float iTTT = Time.time - this._time_start;
						if (iTTT < _iTimeOut)
						{
                            _bFail = false;
                        }
                        else
                        {
                            _bFail = true;
                        }
                        //m_iStatic = 1;
                    }

                    if (_bFail)
                    {
                        if ((Time.time - this._time_start) > _iTimeOut)
                        {
                            MessageBox.DEBUG("Timeout... " + www.error + " " + www.url);
                        }
                        string ppSQL = www.error + " " + www.url;
                        if (!this.Retry())
                        {
                            this.error = ppSQL;
                            m_iStatic = 3;
                        }
                        this.error = string.Format("Timeout, start:{0}, now:{1}, pass:{2}", this._time_start, Time.time, Time.time - this._time_start);
                    }
                }
            }
        }

        public void f_Start()
        {
            www = new WWW(m_strUrl);
            this._time_start = Time.time;
            m_iStatic = 1;
        }

        private bool Retry()
        {
            Debug.Log(string.Format("DownloadItem.Retry, retryTimes:{0}, url:{1}", this.retryTimes, this.m_strUrl));
			CheckNet();
            if (this.www != null)
            {
                this.www.Dispose();
                this.www = null;
            }
            if (this.retryTimes <= 0)
            {
                return false;
            }
            this.retryTimes--;
            if (this.retryTimes <= 5)
            {
                this.f_Start();
            }
            return true;
        }
		
		void CheckNet()
		{
			switch (Application.internetReachability)
			{
				case NetworkReachability.NotReachable:
					// ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Không thể kết nối đến server, hãy kiểm tra lại đường truyền！");
					glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.OnLoginQueueEvent, LoginQueueType.LoginQueueType_FakeQueue);
					break;
			}
		}

        public void f_DoCallBack()
        {
            if (m_iStatic > 1)
            {
                if (m_iStatic == 2)
                {                  
                    File.WriteAllBytes(m_strLocalFile, www.bytes);

                    if (m_ccCallback != null)
                    {
                        if (_CallbackData == null)
                        {
                            m_ccCallback(eMsgOperateResult.OR_Succeed);
                        }
                        else
                        { 
                            m_ccCallback(_CallbackData);
                        }
                    }
                }      
            }
        }

    }

    public void f_RegCompleteCallBack(ccCallback tccCallback)
    {
        _Callback_DownLoadComplete = tccCallback;
    }

    //private bool _bDoing = false;
//    public void f_RegDownload(ResourceDT tResourceDT, ccCallback tccCallback)
//    {
//        string ppSQL = "";

//#if UNITY_WEBPLAYER
//        ppSQL = "http://" + GloData.glo_strHttpServerIP + "//" + GloData.glo_ProName + "//Run//StreamingAssets//" + strFile;
//#elif UNITY_ANDROID
//    #if UNITY_EDITOR
//        if (File.Exists(Application.streamingAssetsPath + "/" + tResourceDT.strPath + ".android"))
//        {
//            ppSQL = "file://" + Application.streamingAssetsPath + "\\" + tResourceDT.strPath + ".android";
//        }
//        else
//        {
//            ppSQL = GloData.glo_strHttpServerIP + "//" + GloData.glo_ProName + "//Run//StreamingAssets//" + tResourceDT.strPath + ".android";
//        }
//#else
//                    ppSQL = Application.streamingAssetsPath + "//" + strFile + ".android";
//#endif
//#elif UNITY_IPHONE
//        if (File.Exists(Application.streamingAssetsPath + "/" + tResourceDT.strPath + ".iphone"))
//        {
//            ppSQL = "file://" + Application.streamingAssetsPath + "\\" + tResourceDT.strPath + ".iphone";
//        }
//        else
//        {
//            ppSQL = GloData.glo_strHttpServerIP + "//" + GloData.glo_ProName + "//Run//StreamingAssets//" + tResourceDT.strPath + ".iphone";
//        }
//#else
//        if (File.Exists(Application.streamingAssetsPath + "/" + tResourceDT.strPath + ".w"))
//        {
//            ppSQL = "file://" + Application.streamingAssetsPath + "\\" + tResourceDT.strPath + ".w";
//        }
//        else
//        {
//            ppSQL = GloData.glo_strHttpServerIP + "//" + GloData.glo_ProName + "//Run//StreamingAssets//" + tResourceDT.strPath + ".w";
//        }

//#endif
//        DownloadItem tDownloadItem = new DownloadItem(tResourceDT, ppSQL, tccCallback);
//        m_aWaitDownLoad.Add(tDownloadItem);
//        _bDoing = true;
//    }

    public void f_RegDownload(string strUrl, string strLocalFile, ccCallback tccCallback, object tCallbackData)
    {
        DownloadItem tDownloadItem = new DownloadItem(strUrl, strLocalFile, tccCallback, tCallbackData);
        m_aWaitDownLoad.Add(tDownloadItem);
        //_bDoing = true;
    }


    public void f_RetryFail()
    {
        if (_aFailList.Count > 0)
        {
            MessageBox.DEBUG("f_RetryFail "  + _aFailList.Count);
            foreach (DownloadItem item in _aFailList)
            {
                m_aWaitDownLoad.Add(item);
            }
            _aFailList.Clear();
            //glo_Main.GetInstance().m_ResourceManager.f_SetNeedDownLoadNum(m_aWaitDownLoad.Count);
        }
        //_bDoing = true;
    }

    public void f_Update()
    {
        if (m_aWaitDownLoad.Count == 0 && m_aDownLoading.Count == 0)
        {
            return;
        }
        if ( m_aDownLoading.Count < maxLoading)
        {            
            foreach (DownloadItem item in m_aWaitDownLoad)
            {                
                if ((tmpList.Count + m_aDownLoading.Count) >= maxLoading)
                {
                    break;
                }
                tmpList.Add(item);
            }
            if (tmpList.Count > 0)
            {
                foreach (DownloadItem item2 in tmpList)
                {
                    m_aWaitDownLoad.Remove(item2);
                    item2.f_Start();
                    totalLoaded++;
                    m_aDownLoading.Add(item2);
                }
                tmpList.Clear();
            }
        }       
        if (m_aDownLoading.Count > 0)
        {
            for (int i = 0; i < m_aDownLoading.Count; i++)
            {
                DownloadItem item = m_aDownLoading[i];
                item.f_Update();
                int iS = item.f_GetStatic();
                if (iS == 2)
                {
MessageBox.DEBUG("Load successful" + item.m_strUrl + " " + item.m_strLocalFile);
                    //item.f_DoCallBack();
                    tmpList.Add(item);
                    _aComplete.Add(item);
                    //item.Dispose();
                }
                else if (iS == 3)
                {
MessageBox.DEBUG("Load failed" + item.m_strUrl + " [" + item.error + "]");
                    //item.f_DoCallBack();
                    tmpList.Add(item);
                    _aFailList.Add(item);
                    //_aComplete.Add(item);
                }                
            }
            if (tmpList.Count > 0)
            {
                foreach (DownloadItem item in tmpList)
                {
                    m_aDownLoading.Remove(item);
                    int iS = item.f_GetStatic();
                    if (iS == 2)
                    {
                        item.f_DoCallBack();
                        item.Dispose();
                    }
                }
                tmpList.Clear();
            }
        }

        if (m_aWaitDownLoad.Count == 0 && m_aDownLoading.Count == 0)
        {            
            if (_aFailList.Count > 0)
            {
glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_DOWNLOADRESERO, "Resource load failed, try again.(" + _aFailList.Count + ")");
                f_RetryFail();
            }
            else
            {
                //_bDoing = false;
                DoDownLoadCompleteing();               
            }            
        }
        
    }


    private void DoDownLoadCompleteing()
    {
        //DownloadItem item = null;
        //for (int i = 0;  i < _aComplete.Count; i++)
        //{
        //    //_aComplete[i].f_DoCallBack();
        //    _aComplete[i].Dispose();
        //}
        _aComplete.Clear();
        _aFailList.Clear();
        m_aWaitDownLoad.Clear();
        tmpList.Clear();
        if (_Callback_DownLoadComplete != null)
        {
            _Callback_DownLoadComplete(eMsgOperateResult.OR_Succeed);
            //_Callback_DownLoadComplete = null;
        }
    }


#region 外部静态方法

    /// <summary>
    /// 同步加载本地文件
    /// </summary>
    /// <param name="strLocalFile"></param>
    /// <param name="bBuf"></param>
    /// <returns></returns>
    public static bool f_LoadLocalFile2Byte(string strLocalFile, out byte[] bBuf)
    {        
        bBuf = null;
        try
        {
            //MessageBox.DEBUG("f_LoadLocalFile2Byte " + strLocalFile);
            if (File.Exists(strLocalFile))
            {
                Stream readStream = new FileStream(strLocalFile, FileMode.Open);
                bBuf = new byte[readStream.Length];
                readStream.Read(bBuf, 0, (int)readStream.Length);
                readStream.Close();
                FileMd5Tools.f_DecodeAndCaculateFileMD5(GloData.glo_strResourcePwd, ref bBuf);
            }
            else
            {
                return false;
            }
        }
        catch (System.Exception ex)
        {
            //打开文件流失败
            MessageBox.DEBUG("f_LoadLocalFile2Byte " + ex.ToString());
            return false;
        }
        return true;
    }


    public static bool f_LoadLocalFile2Text(string strLocalFile, out string ppSQL)
    {
        byte[] bBuf;
        ppSQL = "";
        if (f_LoadLocalFile2Byte(strLocalFile, out bBuf))
        {
            ppSQL = System.Text.Encoding.Default.GetString(bBuf);
        }
        else
        {
            return false;
        }
        return true;
    }



#endregion



}

