using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MulSprite
{
    private Dictionary<string, Sprite> _aSprite = new Dictionary<string, Sprite>();
    private Sprite[] _aSpriteList;

    private Dictionary<string, List<Sprite>> _aDirSprite = null;

    public MulSprite(Sprite[] aSprite)
    {
        _aSpriteList = aSprite;
        for (int i = 0; i < aSprite.Length; i++)
        {
            f_Save(aSprite[i]);
        }
    }

    public void f_Save(Sprite tSprite)
    {
        _aSprite.Add(tSprite.name, tSprite);
    }

    public Sprite f_Read(string strName)
    {
        if (!_aSprite.ContainsKey(strName))
        {
MessageBox.DEBUG("f_CreateShowLogo resource not found" + strName);
            return null;
        }
        int GetInstanceID = _aSprite[strName].GetInstanceID();
        return _aSprite[strName];
    }

    public Sprite[] f_GetAll()
    {
        return _aSpriteList;
    }

    public void f_DispSpriteForClass()
    {
        if (_aDirSprite == null)
        {
            _aDirSprite = new Dictionary<string, List<Sprite>>();
            for (int i = 0; i < _aSpriteList.Length; i++)
            {
                string strName = GetRoleSpriteName(_aSpriteList[i].name);
                List<Sprite> aTmp = null;
                if (!_aDirSprite.TryGetValue(strName, out aTmp))              
                {
                    aTmp = new List<Sprite>();
                    _aDirSprite.Add(strName, aTmp);
                }
                aTmp.Add(_aSpriteList[i]);
            }  
        }
    }

    public List<Sprite> f_ReadRoleSprite(string strName)
    {
        return _aDirSprite[strName];
    }

    private string GetRoleSpriteName(string strName)
    {
        int iPos = strName.LastIndexOf("_");
        return strName.Substring(0, iPos);
    }

}

public class ResourceCatch
{
    private class ResourceCatchCallBackDT
    {
        public object m_Obj;
        public ResourceCatchDelegate m_ResourceCatchDelegate;
    }


    private class AsyncLoadDT
    {
        public ResourceDT m_ResourceDT;
        public List<ResourceCatchCallBackDT> m_ResourceCatchDelegate;
        public bool m_bCreateObj = false;

        public AsyncLoadDT(ResourceDT tResourceDT, object Obj, ResourceCatchDelegate tResourceCatchDelegate)
        {
            m_ResourceDT = tResourceDT;
            m_ResourceCatchDelegate = new List<ResourceCatchCallBackDT>();
            f_AddCallBack(Obj, tResourceCatchDelegate);
        }

        public void f_AddCallBack(object Obj, ResourceCatchDelegate tResourceCatchDelegate)
        {
            ResourceCatchCallBackDT tResourceCatchCallBackDT = new ResourceCatchCallBackDT();
            tResourceCatchCallBackDT.m_Obj = Obj;
            tResourceCatchCallBackDT.m_ResourceCatchDelegate = tResourceCatchDelegate;
            m_ResourceCatchDelegate.Add(tResourceCatchCallBackDT);
        }

        public void f_LoadComplete2CallBack()
        {
            if (m_ResourceDT.iType == 5)
            {
                Sprite[] aSprite = LoadAllAssetForSprite(m_ResourceDT.m_AbcRequest.assetBundle, m_ResourceDT);
                m_ResourceDT.m_AbcRequest.assetBundle.Unload(false);
                m_ResourceDT.m_AbcRequest = null;

                MulSprite tMulSprite = new MulSprite(aSprite);
                m_ResourceDT.m_Obj = null;
                m_ResourceDT.m_MulSprite = tMulSprite;
                for (int i = 0; i < m_ResourceCatchDelegate.Count; i++)
                {
                    if (m_ResourceCatchDelegate[i].m_ResourceCatchDelegate != null)
                    {
                        m_ResourceCatchDelegate[i].m_ResourceCatchDelegate(m_ResourceCatchDelegate[i].m_Obj, null);
                    }
                }
            }
            else
            {
                UnityEngine.Object tAssetObj = LoadAsset(m_ResourceDT.m_AbcRequest.assetBundle, m_ResourceDT);
                m_ResourceDT.m_AbcRequest.assetBundle.Unload(false);
                m_ResourceDT.m_AbcRequest = null;
                m_ResourceDT.m_Obj = tAssetObj;

                for (int i = 0; i < m_ResourceCatchDelegate.Count; i++)
                {
                    if (m_ResourceCatchDelegate[i].m_ResourceCatchDelegate != null)
                    {
                        m_ResourceCatchDelegate[i].m_ResourceCatchDelegate(m_ResourceCatchDelegate[i].m_Obj, tAssetObj);
                    }
                }
            }
            m_ResourceCatchDelegate.Clear();
            m_ResourceCatchDelegate = null;
        }

        public void f_CreateGameObj()
        {
            m_bCreateObj = true;
            string strPath = ResourceTools.f_GetLocalMainPath() + "/" + m_ResourceDT.strPath;
            byte[] bBuf;
            if (DownloadManager.f_LoadLocalFile2Byte(strPath, out bBuf))
            {
                //m_ResourceDT.m_AbcRequest = AssetBundle.LoadFromMemoryAsync(bBuf);
                m_ResourceDT.m_AbcRequest = AssetBundle.LoadFromMemoryAsync(bBuf);
            }
            else
            {
MessageBox.ASSERT("Resource Load Failed" + strPath);
            }

			if (m_ResourceDT.m_AbcRequest == null)
			{
MessageBox.ASSERT("Resource Load Failed" + strPath);
			}
        }

       
    }

    #region 加载基本资源

    private List<ResourceDT> _aData = null;
    private ccCallback _CallBack_BaseResourceCatchComplete;

    public void f_StartLoad(ccCallback tccCallback)
    {
        _CallBack_BaseResourceCatchComplete = tccCallback;
        LoadBaseResource();
    }

    private void LoadBaseResource()
    {
        _aData = glo_Main.GetInstance().m_SC_Pool.m_ResourceSC.f_GetBaseResource();
        for (int i = 0; i < _aData.Count; i++)
        {
            ResourceDT tResourceDT = (ResourceDT)_aData[i];
            LoadItemFile(tResourceDT);
        }
        ccTaskManager.GetInstance().f_Add(On_UpdateLoadFile(), true);
MessageBox.DEBUG("..Basic resource load successful..");
    }

    private void LoadItemFile(ResourceDT tResourceDT)
    {
        string strPath = ResourceTools.f_GetLocalMainPath() + "/" + tResourceDT.strPath;
        byte[] bBuf;
        if (DownloadManager.f_LoadLocalFile2Byte(strPath, out bBuf))
        {
            //tResourceDT.m_AbcRequest = AssetBundle.LoadFromMemoryAsync(bBuf);
            tResourceDT.m_AbcRequest = AssetBundle.LoadFromMemoryAsync(bBuf);
            //tResourceDT.m_Ab = AssetBundle.CreateFromMemoryImmediate(bBuf);
        }
        else
        {
MessageBox.ASSERT("Resource Load Failed" + strPath);
        }
    }

    private IEnumerator On_UpdateLoadFile()
    {
        for (int i = 0; i < _aData.Count; i++)
        {
            ResourceDT tResourceDT = (ResourceDT)_aData[i];
            while (!tResourceDT.m_AbcRequest.isDone)
                yield return 1;
        }
        for (int i = 0; i < _aData.Count; i++)
        {
            ResourceDT tResourceDT = (ResourceDT)_aData[i];
            tResourceDT.m_Ab = tResourceDT.m_AbcRequest.assetBundle;
            tResourceDT.m_AbcRequest = null;
        }
        //for (int i = 0; i < _aData.Count; i++)
        //{
        //    ResourceDT tResourceDT = (ResourceDT)_aData[i];
        //    tResourceDT.m_Obj = LoadAsset(tResourceDT.m_Ab, tResourceDT);
        //}
        _CallBack_BaseResourceCatchComplete(null);
    }
       
    #endregion 加载基本资源

    #region 异步资源加载

    private Dictionary<int, ResourceDT> _aLoadResource = new Dictionary<int, ResourceDT>();
    private Dictionary<int, AsyncLoadDT> _aAsyncLoad = new Dictionary<int, AsyncLoadDT>();
    private List<int> _aReadyClear = new List<int>();

    public void f_Update()
    {
        if (_aLoadResource.Count > 0)
        {
            foreach (KeyValuePair<int, ResourceDT> oItem in _aLoadResource)
            {
                if (oItem.Value.m_AbcRequest.isDone)
                {
                    oItem.Value.m_Ab = oItem.Value.m_AbcRequest.assetBundle;
                    oItem.Value.m_AbcRequest = null;
                    _aReadyClear.Add(oItem.Key);
                }
            }
            for (int i = 0; i < _aReadyClear.Count; i++)
            {
                _aLoadResource.Remove(_aReadyClear[i]);
            }
            _aReadyClear.Clear();

            if (_aLoadResource.Count > 0)
            {
                return;
            }
        }

         //////////////////////////////////////////////////////////////////////////
        if (_aAsyncLoad.Count > 0)
        {
            foreach (KeyValuePair<int, AsyncLoadDT> oItem in _aAsyncLoad)
            {
                if (oItem.Value.m_bCreateObj)
                {
                    if (CheckCreateGameObjectComplete(oItem.Value))
                    {
                        oItem.Value.f_LoadComplete2CallBack();
                        _aReadyClear.Add(oItem.Key);
                    }
                }
                else
                {
                    if (CheckLoadComplete(oItem.Value))
                    {
                        oItem.Value.f_CreateGameObj();
                    }
                }
            }

            for (int i = 0; i < _aReadyClear.Count; i++)
            {
                _aAsyncLoad.Remove(_aReadyClear[i]);
            }
            _aReadyClear.Clear();
        }
    }

    //public void f_LoadGameObject(ResourceDT tResourceDT, object Obj, ResourceCatchDelegate tResourceCatchDelegate)
    //{
    //    AsyncLoadDT tAsyncLoadDT = null;
    //    if (_aAsyncLoad.TryGetValue(tResourceDT.iId, out tAsyncLoadDT))
    //    {
    //        tAsyncLoadDT.f_AddCallBack(Obj, tResourceCatchDelegate);
    //    }
    //    else
    //    {
    //        tAsyncLoadDT = new AsyncLoadDT(tResourceDT, Obj, tResourceCatchDelegate);
    //        _aAsyncLoad.Add(tResourceDT.iId, tAsyncLoadDT);

    //        if (tResourceDT.m_aDenpendency != null)
    //        {
    //            for (int i = 0; i < tResourceDT.m_aDenpendency.Length; i++)
    //            {
    //                if (tResourceDT.m_aDenpendency[i] <= 0)
    //                {
    //                    continue;
    //                }
    //                RegReadyLoadReource(tResourceDT.m_aDenpendency[i]);
    //            }
    //        }
    //    }
    //}

    public bool f_LoadGameObjectImmediately(ResourceDT tResourceDT)
    {
		if(tResourceDT != null)
MessageBox.DEBUG("Request to load resource " + GloData.glo_iPackType + " " + tResourceDT.iId);
        if (GloData.glo_iPackType == 0)
        {
            return LoadGameObjectImmediatelyForLocal(tResourceDT);
        }
        return LoadGameObjectImmediatelyForStreaming(tResourceDT);
    }

    bool LoadGameObjectImmediatelyForLocal(ResourceDT tResourceDT)
    {
        if (!tResourceDT.f_CheckIsLoadComplete())
        {
            byte[] bBuf;
            string strPath = ResourceTools.f_GetLocalMainPath() + "/" + tResourceDT.strPath;
            if (DownloadManager.f_LoadLocalFile2Byte(strPath, out bBuf))
            {
                tResourceDT.m_Ab = AssetBundle.LoadFromMemory(bBuf);
                tResourceDT.m_Obj = LoadAsset(tResourceDT.m_Ab, tResourceDT);
            }
            else
            {
MessageBox.DEBUG("Local local resource load failed" + strPath);
                return false;
            }
        }
        return true;
    }

    bool LoadGameObjectImmediatelyForStreaming(ResourceDT tResourceDT)
    {
		if(tResourceDT != null)
		{	
			if (!tResourceDT.f_CheckIsLoadComplete())
			{            
				if (LoadGameObjectImmediatelyForLocal(tResourceDT))
				{
					return true;
				}
				else
				{
					byte[] bBuf;
					string strPath = ResourceTools.f_GetStreamingMainPath() + "/" + tResourceDT.strPath;
	#if UNITY_STANDALONE_WIN || UNITY_EDITOR
					if (DownloadManager.f_LoadLocalFile2Byte(strPath, out bBuf))
					{
						tResourceDT.m_Ab = AssetBundle.LoadFromMemory(bBuf);
						tResourceDT.m_Obj = LoadAsset(tResourceDT.m_Ab, tResourceDT);
					}
					else
					{
MessageBox.ASSERT("22Resource Load Failed" + strPath);
						return false;
					}
	#elif UNITY_ANDROID || UNITY_IPHONE
					tResourceDT.m_Ab = AssetBundle.LoadFromFile(strPath);
					if (tResourceDT.m_Ab == null)
					{
MessageBox.ASSERT("22Resource Load Failed" + strPath);
						return false;                 
					}
					else
					{
						tResourceDT.m_Obj = LoadAsset(tResourceDT.m_Ab, tResourceDT);
					}
	#endif
				}
			}
		}
        return true;
    }

    public void f_CallbackRecycle(object Obj)
    {
        ResourceDT tResourceDT = (ResourceDT)Obj;
        tResourceDT.m_Ab.Unload(true);
        tResourceDT.m_Ab = null;
        tResourceDT.m_Obj = null;
        tResourceDT.m_MulSprite = null;
MessageBox.DEBUG("Resource processing successful" + tResourceDT.strPath);
    }

    public MulSprite f_LoadSpritetImmediately(ResourceDT tResourceDT)
    {
        if (GloData.glo_iPackType == 0)
        {
            return LoadSpritetImmediatelyForLocal(tResourceDT);
        }
        return LoadSpritetImmediatelyForStreaming(tResourceDT);
    }

    MulSprite LoadSpritetImmediatelyForLocal(ResourceDT tResourceDT)
    {
        if (!tResourceDT.f_CheckIsLoadComplete())
        {
            byte[] bBuf;
            string strPath = ResourceTools.f_GetLocalMainPath() + "/" + tResourceDT.strPath;
            if (DownloadManager.f_LoadLocalFile2Byte(strPath, out bBuf))
            {
                tResourceDT.m_Ab = AssetBundle.LoadFromMemory(bBuf);
                Sprite[] aSprite = LoadAllAssetForSprite(tResourceDT.m_Ab, tResourceDT);
                tResourceDT.m_Ab.Unload(false);
                tResourceDT.m_Ab = null;

                MulSprite tMulSprite = new MulSprite(aSprite);
                tResourceDT.m_Obj = null;
                tResourceDT.m_MulSprite = tMulSprite;
                return tMulSprite;
            }
            else
            {
MessageBox.DEBUG("Load local resource failed" + strPath);
            }
        }
        return tResourceDT.m_MulSprite;
    }

    MulSprite LoadSpritetImmediatelyForStreaming(ResourceDT tResourceDT)
    {
        if (!tResourceDT.f_CheckIsLoadComplete())
        {            
            if (LoadSpritetImmediatelyForLocal(tResourceDT) != null)
            {
                return tResourceDT.m_MulSprite;
            }            
            else
            {
                byte[] bBuf;
                string strPath = ResourceTools.f_GetStreamingMainPath() + "/" + tResourceDT.strPath;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
                if (DownloadManager.f_LoadLocalFile2Byte(strPath, out bBuf))
                {
                    tResourceDT.m_Ab = AssetBundle.LoadFromMemory(bBuf);
                    Sprite[] aSprite = LoadAllAssetForSprite(tResourceDT.m_Ab, tResourceDT);
                    tResourceDT.m_Ab.Unload(false);
                    tResourceDT.m_Ab = null;

                    MulSprite tMulSprite = new MulSprite(aSprite);
                    tResourceDT.m_Obj = null;
                    tResourceDT.m_MulSprite = tMulSprite;
                    return tMulSprite;
                }
                else
                {
MessageBox.ASSERT("Resource Load Failed" + strPath);
                }
#elif UNITY_ANDROID || UNITY_IPHONE
                tResourceDT.m_Ab = AssetBundle.LoadFromFile(strPath);
                if (tResourceDT.m_Ab == null)
                {
MessageBox.ASSERT("Load local resource failed" + strPath);
                }
                else
                {
                    Sprite[] aSprite = LoadAllAssetForSprite(tResourceDT.m_Ab, tResourceDT);
                    tResourceDT.m_Ab.Unload(false);
                    tResourceDT.m_Ab = null;

                    MulSprite tMulSprite = new MulSprite(aSprite);
                    tResourceDT.m_Obj = null;
                    tResourceDT.m_MulSprite = tMulSprite;
                    return tMulSprite;
                }            
#endif
            }
        }
        return tResourceDT.m_MulSprite;
    }


    public bool f_CheckIsLoadComplete(ResourceDT tResourceDT)
    {
        if (tResourceDT.m_AbcRequest == null)
        {
            return false;
        }
        if (tResourceDT.m_AbcRequest.isDone)
        {
            if (tResourceDT.m_Obj != null)
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckCreateGameObjectComplete(AsyncLoadDT tAsyncLoadDT)
    {
        return tAsyncLoadDT.m_ResourceDT.m_AbcRequest.isDone;
    }

    private bool CheckLoadComplete(AsyncLoadDT tAsyncLoadDT)
    {
        ResourceDT __ResourceDT;
        for (int i = 0; i < tAsyncLoadDT.m_ResourceDT.m_aDenpendency.Length; i++)
        {
            if (tAsyncLoadDT.m_ResourceDT.m_aDenpendency[i] <= 0)
            {
                continue;
            }
            __ResourceDT = glo_Main.GetInstance().m_SC_Pool.m_ResourceSC.f_GetResourceDT(tAsyncLoadDT.m_ResourceDT.m_aDenpendency[i]);
            if (__ResourceDT.m_Ab == null)
            {
                return false;
            }
        }
        return true;
    }

    //private void RegReadyLoadReource(int iId)
    //{
    //    if (!_aLoadResource.ContainsKey(iId))
    //    {
    //        string strPath;
    //        byte[] bBuf;
    //        ResourceDT __ResourceDT = glo_Main.GetInstance().m_SC_Pool.m_ResourceSC.f_GetResourceDT(iId);
    //        if (__ResourceDT != null)
    //        {
    //            if (__ResourceDT.m_Ab == null)
    //            {
    //                strPath = ResourceTools.f_GetLocalMainPath() + "/" + __ResourceDT.strPath;
    //                if (DownloadManager.f_LoadLocalFile2Byte(strPath, out bBuf))
    //                {
    //                    //__ResourceDT.m_AbcRequest = AssetBundle.LoadFromMemoryAsync(bBuf);
    //                    __ResourceDT.m_AbcRequest = AssetBundle.LoadFromMemoryAsync(bBuf);
    //                    _aLoadResource.Add(iId, __ResourceDT);
    //                }
    //                else
    //                {
    //                    MessageBox.ASSERT("加载本地资源失败 " + strPath);
    //                }
    //            }
    //        }
    //        else
    //        {
    //            MessageBox.ASSERT("无此资源 " + iId);
    //        }
    //    }
    //}

    private IEnumerator Task_LoadGameObject(ResourceDT tResourceDT, object Obj, ResourceCatchDelegate tResourceCatchDelegate)
    {
        ResourceDT __ResourceDT = null;
        for (int i = 0; i < tResourceDT.m_aDenpendency.Length; i++)
        {
            if (tResourceDT.m_aDenpendency[i] <= 0)
            {
                continue;
            }
            __ResourceDT = glo_Main.GetInstance().m_SC_Pool.m_ResourceSC.f_GetResourceDT(tResourceDT.m_aDenpendency[i]);
            if (__ResourceDT.m_Ab == null && __ResourceDT.m_AbcRequest != null)
            {
                while (!__ResourceDT.m_AbcRequest.isDone)
                {
                    yield return 1;
                }
                __ResourceDT.m_Ab = __ResourceDT.m_AbcRequest.assetBundle;
                __ResourceDT.m_AbcRequest = null;
            }
        }
        byte[] bBuf;
        string strPath = ResourceTools.f_GetLocalMainPath() + "/" + tResourceDT.strPath;
        if (DownloadManager.f_LoadLocalFile2Byte(strPath, out bBuf))
        {
            //tResourceDT.m_AbcRequest = AssetBundle.LoadFromMemoryAsync(bBuf);
            tResourceDT.m_AbcRequest = AssetBundle.LoadFromMemoryAsync(bBuf);
            ccTaskManager.GetInstance().f_Add(Task_CreateGameObject(tResourceDT, Obj, tResourceCatchDelegate), true);
        }
        else
        {
MessageBox.ASSERT("Resource Load Failed" + strPath);
        }
    }

    private IEnumerator Task_CreateGameObject(ResourceDT tResourceDT, object Obj, ResourceCatchDelegate tResourceCatchDelegate)
    {
        while (!tResourceDT.m_AbcRequest.isDone)
        {
            yield return 1;
        }
        if (tResourceCatchDelegate != null)
        {
            Object tAssetObj = LoadAsset(tResourceDT.m_AbcRequest.assetBundle, tResourceDT);
            tResourceDT.m_AbcRequest.assetBundle.Unload(false);
            tResourceDT.m_AbcRequest = null;
            tResourceCatchDelegate(Obj, tAssetObj);
        }
    }

    #endregion 异步资源加载



    private static UnityEngine.Object LoadAsset(AssetBundle ab, ResourceDT tResourceDT)
    {
        if (ab == null || tResourceDT == null)
        {
            return null;
        }
        string ppSQL = tResourceDT.strPath.Replace(".bytes", "");
        ppSQL = ppSQL.Substring(0, ppSQL.LastIndexOf("_"));
        return ab.LoadAsset(ppSQL);
    }

    private static Sprite[] LoadAllAssetForSprite(AssetBundle ab, ResourceDT tResourceDT)
    {
        if (ab == null || tResourceDT == null)
        {
            return null;
        }
        string ppSQL = tResourceDT.strPath.Replace(".bytes", "");
        ppSQL = ppSQL.Substring(0, ppSQL.LastIndexOf("_"));
        return ab.LoadAllAssets<Sprite>();
    }
}
