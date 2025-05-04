using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ccU3DEngine;
using ccU3DEngine.ccEngine.ThingManager;
using Spine.Unity;
using System.Linq;

/// <summary>
/// 异步加载资源回调
/// </summary>
/// <param name="Obj">申请资源对象</param>
/// <param name="asset">异步加载成功返回后的资源，根据需要通过GameObject.Instantiate创建</param>
public delegate void ResourceCatchDelegate(object Obj, UnityEngine.Object asset);

public class ResourceManager
{
  
    private ResourceCatch _ResourceCatch = new ResourceCatch();   
    public LoadResourceList GuidanceResourceList = new LoadResourceList();

    private List<ResourceDT> _modleDT_List = new List<ResourceDT>();
    //private ccThingManager _ccThingManager = new ccThingManager();

    public ResourceManager()
    {
    }


    public void f_Update()
    {
        _ResourceCatch.f_Update();        
        //_ccThingManager.f_Update();
    }
   
    ///// <summary>
    ///// 进入游戏进行第二次加载
    ///// </summary>
    ///// <param name="tccCallback">资源加载完回调</param>
    //public void f_StartUpdateServerFile(bool bOpenLoadPage, ccCallback tccCallback)
    //{
    //    _ResourceLoadStep.SetFinishCallback(tccCallback);
    //    _ResourceLoadStep.StartUpdateServerFile(bOpenLoadPage);
    //}


    /// <summary>
    /// 清除缓存资源
    /// </summary>
    public void f_Clear()
    {
        glo_Main.GetInstance().m_SC_Pool.m_ResourceSC.f_Clear();
    }


#region 创建资源

    /// <summary>
    /// 创建角色基本控制模型
    /// </summary>
    /// <returns></returns>
    public RoleControl f_CreateRoleModel()
    {
        UnityEngine.Object tModel = Resources.Load("GamePrefab/Role");
        GameObject oRole = GameObject.Instantiate(tModel) as GameObject;
        RoleControl tRoleControl = oRole.GetComponent<RoleControl>();
        return tRoleControl;
    }



    private Transform _HPPanel = null;
    public HpMpPanel f_CreateHP()
    {
        if (_HPPanel == null)
        {
            _HPPanel = GameObject.Find("HPPanel").transform;
        }
        UnityEngine.Object tModel = Resources.Load("UI/UIPrefab/BattleScene/HP");
        GameObject oRole = GameObject.Instantiate(tModel) as GameObject;
        oRole.transform.parent = _HPPanel;
        oRole.transform.localRotation = Quaternion.identity;
        return oRole.GetComponent<HpMpPanel>();
    }
    /// <summary>
    /// 创建特效,回收通过f_Destory
    /// </summary>
    /// <param name="iResId"></param>
    public GameObject f_CreateMagic(int iResId)
    {
        string strFileName = "SD" + iResId + "_bytes";
        MessageBox.DEBUG("f_CreateMagic " + strFileName);
        ResourceDT tResourceDT = (ResourceDT)glo_Main.GetInstance().m_SC_Pool.m_ResourceSC.f_GetResourceDT(strFileName);
        if (tResourceDT == null)
        {
MessageBox.ASSERT("Resource not found。 " + strFileName);
            return null;

        }
        if (!ccResourceManager.GetInstance().f_CheckIsHave(strFileName))
        {
            if (_ResourceCatch.f_LoadGameObjectImmediately(tResourceDT))
            {
                if (tResourceDT.m_Obj == null)
                {
MessageBox.ASSERT("Resource not found。 " + strFileName);
                    return null;
                }
                ccResourceManager.GetInstance().f_RegResource(strFileName, tResourceDT.m_Obj, _ResourceCatch.f_CallbackRecycle, tResourceDT);
            }
        }
        GameObject oModel = ccResourceManager.GetInstance().f_Instantiate(strFileName);
        ResetShader(oModel);
        ResetSD(oModel);
        GuidanceResourceList.f_Add(tResourceDT);
        MessageBox.DEBUG("f_CreateMagic End");
        return oModel;
    }

    /// <summary>
    /// 创建角色,回收通过f_Destory
    /// </summary>
    /// <param name="iResId"></param>
    public GameObject f_CreateRole(int iResId, bool needToShowRed, bool needShowShadow = true, int notRoleID = 99999)
    {
        string strFileName = needToShowRed ? "SD" + iResId + "_1_bytes" : "SD" + iResId + "_bytes";
        MessageBox.DEBUG("f_CreateRole " + strFileName);

        ResourceDT tResourceDT = (ResourceDT)glo_Main.GetInstance().m_SC_Pool.m_ResourceSC.f_GetResourceDT(strFileName);
        if (tResourceDT == null)
        {
            strFileName = "SD" + iResId + "_bytes";
            tResourceDT = (ResourceDT)glo_Main.GetInstance().m_SC_Pool.m_ResourceSC.f_GetResourceDT(strFileName);
            if (tResourceDT == null)
            {
MessageBox.DEBUG("Resource not found。" + strFileName + " id 14011");
                tResourceDT = (ResourceDT)glo_Main.GetInstance().m_SC_Pool.m_ResourceSC.f_GetResourceDT("SD14011_bytes");
                strFileName = "SD14011_bytes";
            }
        }
        if (!ccResourceManager.GetInstance().f_CheckIsHave(strFileName))
        {
            if (_ResourceCatch.f_LoadGameObjectImmediately(tResourceDT))
            {
                if (tResourceDT.m_Obj == null)
                {
MessageBox.ASSERT("Resource not found。" + strFileName);
                    return null;
                }
                ccResourceManager.GetInstance().f_RegResource(strFileName, tResourceDT.m_Obj, _ResourceCatch.f_CallbackRecycle, tResourceDT);
            }
        }
        GameObject oModel = ccResourceManager.GetInstance().f_Instantiate(strFileName);
        Transform effect = oModel.transform.Find("magic");
        if (effect)
        {
            effect.gameObject.SetActive(false);
        }
        ResetShader(oModel);
        ResetSD(oModel);
        Transform shadow = oModel.transform.Find("Shadow");
		//My Code
		string temp = iResId + "";
		temp = temp.Remove(temp.Length - 1);
		int cardID = int.Parse(temp);
		CardDT CardDT = (CardDT)glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(cardID);
		int rarity = 1;
		if(CardDT == null)
		{
			rarity = 7;
		}
		else
		{
			// MessageBox.ASSERT("Rarity:" + CardDT.iImportant);
			rarity = CardDT.iImportant;
		}
		//
        if (needShowShadow && shadow == null)
        {
            UITool.f_ShowBone(oModel.transform.GetComponent<SkeletonAnimation>(), 0, rarity);
//            GameObject go = f_CreateShadow();
//            go.transform.parent = oModel.transform;
//            go.transform.localPosition = Vector3.zero;
//            go.transform.localScale = Vector3.one * GameParamConst.shadowScalePerc;
//            go.transform.name = GameParamConst.prefabShadowName;
        }
        GuidanceResourceList.f_Add(tResourceDT);
        MessageBox.DEBUG("f_CreateRole End");
        return oModel;
    }

    /// <summary>
    /// 预加载资源，不实例化
    /// </summary>
    /// <param name="iResId"></param>
    /// <returns></returns>
    public bool f_PreloadRoleRes(int iResId)
    {
        string strFileName = "SD" + iResId + "_bytes";
        ResourceDT tResourceDT = (ResourceDT)glo_Main.GetInstance().m_SC_Pool.m_ResourceSC.f_GetResourceDT(strFileName);
        if (tResourceDT == null)
        {
MessageBox.DEBUG("Resource not found。" + strFileName + " id 14011");
            return false;
        }
        if (!ccResourceManager.GetInstance().f_CheckIsHave(strFileName))
        {
            if (_ResourceCatch.f_LoadGameObjectImmediately(tResourceDT))
            {
                ccResourceManager.GetInstance().f_RegResource(strFileName, tResourceDT.m_Obj, _ResourceCatch.f_CallbackRecycle, tResourceDT);
            }
        }
        GuidanceResourceList.f_Add(tResourceDT);
        return true;
    }

    void ResetSD(GameObject oModel)
    {
        SkeletonRenderer tSkeletonRenderer = (SkeletonRenderer)oModel.transform.GetComponent<SkeletonRenderer>();
        if (null == tSkeletonRenderer)
        {
Debug.LogError("SkeletonRenderer null，modelname：" + oModel.name);
            return;
        }
        tSkeletonRenderer.ClearState();
        SkeletonAnimation tSkeletonAnimation = oModel.transform.GetComponent<SkeletonAnimation>();
        tSkeletonAnimation.timeScale = 0.5f;
    }

    public void ResetShader(UnityEngine.Object obj, string shaderName = "Spine/Skeleton")
    {
        List<Material> listMat = new List<Material>();
        listMat.Clear();

        if (obj is Material)
        {
            Material m = obj as Material;
            listMat.Add(m);
        }
        else if (obj is GameObject)
        {
            GameObject go = obj as GameObject;
            Renderer[] rends = go.GetComponentsInChildren<Renderer>();
            if (null != rends)
            {
                foreach (Renderer item in rends)
                {
                    Material[] materialsArr = item.sharedMaterials;
                    foreach (Material m in materialsArr)
                        listMat.Add(m);
                }
            }

        }

        for (int i = 0; i < listMat.Count; i++)
        {
            Material m = listMat[i];
            if (null == m)
                continue;
            //var shaderName = "Spine/Skeleton";
            var newShader = Shader.Find(shaderName);
            if (newShader != null)
                m.shader = newShader;
        }
    }

    /// <summary>
    /// 创建的GameObject通过此方法进行回收
    /// </summary>
    /// <param name="Obj"></param>
    public void f_DestorySD(GameObject Obj)
    {
        ccResourceManager.GetInstance().f_UnInstantiate(Obj);
    }


    /// <summary>
    /// 读取Icon
    /// </summary>
    /// <param name="tPngAtlasDT">图集DT</param>
    /// <param name="iIconId">Icon的资源Id</param>
    /// <returns></returns>
    public Sprite f_CreateIcon(PngAtlasDT tPngAtlasDT, int iIconId)
    {
        string strFileName = tPngAtlasDT.szFileName + "_png.bytes";
        ResourceDT tResourceDT = (ResourceDT)glo_Main.GetInstance().m_SC_Pool.m_ResourceSC.f_GetResourceDT(strFileName);
        if (tResourceDT == null)
        {
MessageBox.DEBUG("Cannot find icon。" + strFileName + " id 999999");
            iIconId = 999999;
            strFileName = "10000atlas_png.bytes";
            tResourceDT = (ResourceDT)glo_Main.GetInstance().m_SC_Pool.m_ResourceSC.f_GetResourceDT(strFileName);
            if (tResourceDT == null)
            {
MessageBox.ASSERT("default icon resource not found 999999");
				return UITool.f_GetSkillIcon("Temp_Icon");
            }
        }
        tResourceDT.iType = 5;
        MulSprite tMulSprite = _ResourceCatch.f_LoadSpritetImmediately(tResourceDT);
        if (tMulSprite != null)
        {
            Sprite tSprite = tMulSprite.f_Read(iIconId.ToString());
            if (tSprite != null)
            {
                return tSprite;
            }
        }
        iIconId = 999999;
        tPngAtlasDT = (PngAtlasDT)glo_Main.GetInstance().m_SC_Pool.m_PngAtlasSC.f_GetSC(6);
        GuidanceResourceList.f_Add(tResourceDT);
        return f_CreateIcon(tPngAtlasDT, iIconId);
    }

    /// <summary>
    /// 创建单张Sprite大图
    /// </summary>
    /// <param name="szName"></param>
    /// <returns></returns>
    public Sprite f_CreateSprite(string szName)
    {
        string strFileName = (szName + "_png.bytes").ToLower();
        ResourceDT tResourceDT = (ResourceDT)glo_Main.GetInstance().m_SC_Pool.m_ResourceSC.f_GetResourceDT(strFileName);
        if (tResourceDT == null)
        {
MessageBox.ASSERT("Resource not found。" + strFileName);
        }
        else
        {
            tResourceDT.iType = 5;
            MulSprite tMulSprite = _ResourceCatch.f_LoadSpritetImmediately(tResourceDT);
            if (tMulSprite != null)
            {
                GuidanceResourceList.f_Add(tResourceDT);
                return tMulSprite.f_Read(szName);
            }
        }
        return null;
    }

    /// <summary>
    /// 加载Resource里的Texture2D资源
    /// </summary>
    /// <param name="strFile"></param>
    /// <returns></returns>
    public Texture2D f_CreateResourceTexture2D(string strFile)//,object data)
    {
        Texture2D tTexture2D = Resources.Load(strFile) as Texture2D;
        if (tTexture2D == null)
        {
            string[] strArrar = strFile.Split('/');
            string TextureFileName = strArrar[strArrar.Length - 1];
            strArrar[strArrar.Length - 1] = glo_Main.GetInstance().CodeName + "_" + TextureFileName;
            // MessageBox.ASSERT("Không tìm thấy tài nguyên" + strFile);
            tTexture2D = Resources.Load(string.Join("/", strArrar)) as Texture2D;
            // return null;
        }
        if (tTexture2D == null)
        {
            string[] strArrar = strFile.Split('/');
            string TextureFileName = strArrar[strArrar.Length - 1];
            string strFileName = string.Format("{0}_png.bytes", TextureFileName.ToLower());
            ResourceDT tResourceDT = (ResourceDT)glo_Main.GetInstance().m_SC_Pool.m_ResourceSC.f_GetResourceDT(strFileName);

            if(tResourceDT == null)
            {

            }
            else
            {
                if (tResourceDT.m_Ab == null)
                {
                    _ResourceCatch.f_LoadGameObjectImmediately(tResourceDT);
                }
                tTexture2D = tResourceDT.m_Obj as Texture2D;
                if (tTexture2D == null)
                {
MessageBox.ASSERT("Texture not found" + TextureFileName);
                }
                GuidanceResourceList.f_Add(tResourceDT);
            }

        }
        return tTexture2D;
    }

    public GameObject f_CreateShadow()
    {
        GameObject tShadow = Resources.Load("UI/UIPrefab/Shadow") as GameObject;
        GameObject tGameObject = GameObject.Instantiate(tShadow) as GameObject;
        if (tGameObject == null)
        {
MessageBox.ASSERT("Resource does not exist: Shadow ");
        }
        return tGameObject;
    }

    public GameObject f_CreateReddot()
    {
        GameObject tModel = Resources.Load("UI/UIPrefab/Reddot") as GameObject;
        GameObject tGameObject = GameObject.Instantiate(tModel) as GameObject;
        if (tGameObject == null)
        {
MessageBox.ASSERT("Resource does not exist: Reddot ");
        }
        return tGameObject;
    }

    public GameObject f_CreateEventTimeSystem(string szSystemId)
    {
        GameObject tGameObject = null ;
        GameObject tModel = null ;
        tModel = Resources.Load("UI/UIPrefab/GameMain/EventTime/"+ szSystemId) as GameObject;
        tGameObject = GameObject.Instantiate(tModel) as GameObject;
        if (tGameObject == null)
        {
MessageBox.ASSERT("Resource does not exist: EventTimeIcon ");
        }

        return tGameObject;
    }

    public GameObject f_CreateEventOnlineVipSys()
    {        
        GameObject tModel = Resources.Load("UI/UIPrefab/GameMain/EventTime/EventOnlineVipIcon") as GameObject;
        GameObject tGameObject = GameObject.Instantiate(tModel) as GameObject;
        if (tGameObject == null)
        {
MessageBox.ASSERT("Resource does not exist: EventTimeIcon ");
        }
        return tGameObject;
    }
    /// <summary>
    /// 获取音效
    /// </summary>
    /// <param name="ButtleOrBg">按钮 或者背景音乐 0是按钮  1为特效 其他为背景音乐</param>
    /// <param name="MusicType">音乐类型</param>
    /// <returns></returns>
    public AudioClip f_GetAudioClipByDialog(string MusicName)
    {
        AudioClip tAudioClip = Resources.Load<AudioClip>("Audio/Dialog/" + MusicName) as AudioClip;
MessageBox.DEBUG("No sound found： "+ MusicName);
        if (tAudioClip == null)
        {
            string strFileName = string.Format("{0}_mp3.bytes", MusicName.ToLower());
            ResourceDT tResourceDT = (ResourceDT)glo_Main.GetInstance().m_SC_Pool.m_ResourceSC.f_GetResourceDT(strFileName);
            //if (!ccResourceManager.GetInstance().f_CheckIsHave(strFileName))
            //{
            //    if (_ResourceCatch.f_LoadGameObjectImmediately(tResourceDT))
            //    {
            //        ccResourceManager.GetInstance().f_RegResource(strFileName, tResourceDT.m_Obj, _ResourceCatch.f_CallbackRecycle, tResourceDT);
            //    }
            //}
            if (tResourceDT == null)
            {
                strFileName = string.Format("{0}_wav.bytes", MusicName.ToLower());
                tResourceDT = (ResourceDT)glo_Main.GetInstance().m_SC_Pool.m_ResourceSC.f_GetResourceDT(strFileName);
            }
            if (tResourceDT != null && tResourceDT.m_Ab == null)
            {
                _ResourceCatch.f_LoadGameObjectImmediately(tResourceDT);
                tAudioClip = tResourceDT.m_Ab.LoadAsset<AudioClip>(MusicName);
            }
            if (tAudioClip == null)
            {
                MessageBox.ASSERT("No background music found: " + MusicName);
            }
        }
        return tAudioClip;
    }

    /// <summary>
    /// 获取音效
    /// </summary>
    /// <param name="ButtleOrBg">按钮 或者背景音乐 0是按钮  1为特效 其他为背景音乐</param>
    /// <param name="MusicType">音乐类型</param>
    /// <returns></returns>
    public AudioClip f_GetAudioClip(string MusicName)
    {
        AudioClip tAudioClip = Resources.Load<AudioClip>("Audio/" + MusicName) as AudioClip;
        if (tAudioClip == null)
        {
            string strFileName = string.Format("{0}_wav.bytes", MusicName.ToLower());
            ResourceDT tResourceDT = (ResourceDT)glo_Main.GetInstance().m_SC_Pool.m_ResourceSC.f_GetResourceDT(strFileName);
            //if (!ccResourceManager.GetInstance().f_CheckIsHave(strFileName))
            //{
            //    if (_ResourceCatch.f_LoadGameObjectImmediately(tResourceDT))
            //    {
            //        ccResourceManager.GetInstance().f_RegResource(strFileName, tResourceDT.m_Obj, _ResourceCatch.f_CallbackRecycle, tResourceDT);
            //    }
            //}
            if (tResourceDT != null && tResourceDT.m_Ab == null)
            {
                _ResourceCatch.f_LoadGameObjectImmediately(tResourceDT);
            }
            if (tResourceDT != null && tResourceDT.m_Ab != null)
            {
                tAudioClip = tResourceDT.m_Ab.LoadAsset<AudioClip>(MusicName);
            }

            if (tAudioClip == null)
            {
MessageBox.ASSERT("No background music found " + MusicName);
            }
        }
        return tAudioClip;
    }

    /// <summary>
    /// 获取音效
    /// </summary>
    /// <param name="ButtleOrBg">按钮 或者背景音乐 0是按钮  1为特效 其他为背景音乐</param>
    /// <param name="MusicType">音乐类型</param>
    /// <returns></returns>
    public AudioClip f_GetAudioClip(int ButtleOrBg, int MusicType)
    {
        string MusicName = string.Empty;

        if (ButtleOrBg == 0)
        {
            MusicName = _GetButtleAudio(MusicType);
        }
        else if (ButtleOrBg == 1)
        {
            MusicName = _GetEffectAudio(MusicType);
        }
        else
        {
            MusicName = _GetBgAudio(MusicType);
        }
        AudioClip tAudioClip = Resources.Load<AudioClip>("Audio/" + MusicName) as AudioClip;
        if (tAudioClip == null)
        {
            string strFileName = string.Format("{0}_wav.bytes", MusicName.ToLower());
            ResourceDT tResourceDT = (ResourceDT)glo_Main.GetInstance().m_SC_Pool.m_ResourceSC.f_GetResourceDT(strFileName);
            //if (!ccResourceManager.GetInstance().f_CheckIsHave(strFileName))
            //{
            //    if (_ResourceCatch.f_LoadGameObjectImmediately(tResourceDT))
            //    {
            //        ccResourceManager.GetInstance().f_RegResource(strFileName, tResourceDT.m_Obj, _ResourceCatch.f_CallbackRecycle, tResourceDT);
            //    }
            //}
            if (tResourceDT.m_Ab == null)
            {
                _ResourceCatch.f_LoadGameObjectImmediately(tResourceDT);
            }
            tAudioClip = tResourceDT.m_Ab.LoadAsset<AudioClip>(MusicName);
            if (tAudioClip == null)
            {
MessageBox.ASSERT("No background music found " + MusicName);
            }
        }
        return tAudioClip;
    }

    public AudioClip f_GetAudioMagic(string strName)
    {
        string strSDName = "SD" + strName.Substring(0, strName.Length - 1) + "_bytes";
        ResourceDT tResourceDT = (ResourceDT)glo_Main.GetInstance().m_SC_Pool.m_ResourceSC.f_GetResourceDT(strSDName);
        if (!ccResourceManager.GetInstance().f_CheckIsHave(strSDName))
        {
            if (_ResourceCatch.f_LoadGameObjectImmediately(tResourceDT))
            {
				if(tResourceDT != null)
				{
					ccResourceManager.GetInstance().f_RegResource(strSDName, tResourceDT.m_Obj, _ResourceCatch.f_CallbackRecycle, tResourceDT);
				}
            }
        }
		AudioClip tAudioClip = null;
		if(tResourceDT != null)
		{
			tAudioClip = tResourceDT.m_Ab.LoadAsset<AudioClip>(strName);
		}
        if (tAudioClip == null)
        {
			MessageBox.ASSERT("No sound found " + strName);
        }
        return tAudioClip;
    }

    /// <summary>
    /// 创建角色,回收通过f_Destory
    /// </summary>
    /// <param name="iResId"></param>
    public GameObject f_Create3DModel(int iResId, bool needToShowRed)
    {
        string strFileName = needToShowRed ? iResId + "_1_prefab.bytes" : iResId + "_prefab.bytes";
        MessageBox.DEBUG("f_CreateRole " + strFileName);

        ResourceDT tResourceDT = (ResourceDT)glo_Main.GetInstance().m_SC_Pool.m_ResourceSC.f_GetResourceDT(strFileName);
        if (tResourceDT == null)
        {
            strFileName = iResId + "_prefab.bytes";
            tResourceDT = (ResourceDT)glo_Main.GetInstance().m_SC_Pool.m_ResourceSC.f_GetResourceDT(strFileName);
            if (tResourceDT == null)
            {
MessageBox.DEBUG("Resource not found。" + strFileName + " 显示 2004001");
                tResourceDT = (ResourceDT)glo_Main.GetInstance().m_SC_Pool.m_ResourceSC.f_GetResourceDT("2004001_prefab.bytes");
                strFileName = "2004001_prefab.bytes";
            }
        }
        if (!ccResourceManager.GetInstance().f_CheckIsHave(strFileName))
        {
            if(tResourceDT.m_Ab == null)
                _modleDT_List.Add(tResourceDT);

            if (_ResourceCatch.f_LoadGameObjectImmediately(tResourceDT))
            {
                if (tResourceDT.m_Obj == null)
                {
MessageBox.ASSERT("Resource not found。" + strFileName);
                    return null;
                }
            }
        }
        GameObject oModel = GameObject.Instantiate(tResourceDT.m_Obj) as GameObject;
        ResetShader(oModel, "Mobile/Diffuse");
        MessageBox.DEBUG("f_Create3DModel End");
        return oModel;
    }

    public void Clear3DModleDT()
    {
        for (int i = 0; i < _modleDT_List.Count; i++)
            _ResourceCatch.f_CallbackRecycle(_modleDT_List[i]);

        _modleDT_List = new List<ResourceDT>();
    }

    private string _GetEffectAudio(int MusicType)
    {
        switch ((AudioEffectType)MusicType)
        {

            case AudioEffectType.TreasureRfine:
                return "TreasureRfine";
            case AudioEffectType.TreasureInten:
                return "TreasureInten";
            case AudioEffectType.Recycle_01:
                return "Recycle_01";
            case AudioEffectType.Recycle_02:
                return "Recycle_02";
            case AudioEffectType.FitMagic:
                return "FitMagic";
            case AudioEffectType.CardRefine:
                return "CardRefine";
            case AudioEffectType.CardUpLevel:
                return "CardUpLevel";
            case AudioEffectType.TenRaffle:
                return "TenRaffle";
            case AudioEffectType.Gain_Whith:
                return "Gain_Whith";
            case AudioEffectType.Gain_Orange:
                return "Gain_Orange";
            case AudioEffectType.Gain_Red:
                return "Gain_Red";
            case AudioEffectType.Gain_Purple:
                return "Gain_Purple";
            case AudioEffectType.Victory1:
                return "Victory1";
            case AudioEffectType.Victory2:
                return "Victory2";
            case AudioEffectType.Victory3:
                return "Victory3";
            case AudioEffectType.BattleFormLight:
                return "BattleFormLight";
            case AudioEffectType.EquipRefine:
                return "EquipRefine";
            case AudioEffectType.EquipInten:
                return "EquipInten";
            case AudioEffectType.EquipStar:
                return "EquipStar";
			case AudioEffectType.Intro_Boy:
                return "Intro_Boy";
			case AudioEffectType.Intro_Girl:
                return "Intro_Girl";
			case AudioEffectType.TenRaffle2:
                return "TenRaffle2";
            case AudioEffectType.Crit:
                return "Crit";
            case AudioEffectType.miss:
                return "miss";
            default:
                return "";
        }
    }

    private string _GetBgAudio(int MusicType)
    {
        switch ((AudioMusicType)MusicType)
        {
            case AudioMusicType.LoginBg:
                return "LoginBgMusic";
            case AudioMusicType.MainBg:
                return "MainMusic";
            case AudioMusicType.BattleMusic:
                int tRandom = UnityEngine.Random.Range(1, 7);
				MessageBox.ASSERT("Num: " + tRandom);
                return "BattleMusic" + tRandom;
            case AudioMusicType.BattleVictory:
                return "BattleVictory";
            case AudioMusicType.BattleFail:
                return "BattleFail";
            case AudioMusicType.challenge:
                int tRandom2 = UnityEngine.Random.Range(1, 7);
                return "challenge" + tRandom2;
            case AudioMusicType.Loading:
                return "Loading";
			case AudioMusicType.ChargeBg:
				return "ChargeShop";
			case AudioMusicType.Dungeon:
				return "UI_Dungeon";
			case AudioMusicType.UI_Challenge:
				return "UI_Challenge";
			case AudioMusicType.Arena:
				return "UI_Arena";
			case AudioMusicType.Legion:
				return "UI_Legion";
			case AudioMusicType.UI_Rank:
				return "UI_Rank";
            default:
                return "";
        }


    }
    private string _GetButtleAudio(int MusicType)
    {
        switch ((AudioButtle)MusicType)
        {
            case AudioButtle.ButtonNormal:
                return "ButtonNormal";
            default:
                return "";
        }
    }


    #endregion

    public CardCashSkill f_CreateCardCashSkill()
    {
        UnityEngine.Object tModel = Resources.Load("UI/UIPrefab/BattleScene/CardCashSkill");
        GameObject oRole = GameObject.Instantiate(tModel) as GameObject;
        return oRole.GetComponent<CardCashSkill>();
    }

    public GodEquipSkillEff f_CreateGodEquipSkillEff()
    {
        if (_HPPanel == null)
        {
            _HPPanel = GameObject.Find("HPPanel").transform;
        }
        UnityEngine.Object tModel = Resources.Load("UI/UIPrefab/BattleScene/GodEquipSkillEff");
        GameObject oRole = GameObject.Instantiate(tModel) as GameObject;
        oRole.transform.parent = _HPPanel;
        oRole.transform.localRotation = Quaternion.identity;
        return oRole.GetComponent<GodEquipSkillEff>();
    }
    /// <summary>
    /// nguyên tố trùng kích
    /// </summary>
    /// <returns></returns>
    public FightElementItem f_CreateFightElementItem()
    {
        UnityEngine.Object tModel = Resources.Load("UI/UIPrefab/BattleScene/FightElementItem");
        GameObject oRole = GameObject.Instantiate(tModel) as GameObject;
        return oRole.GetComponent<FightElementItem>();
    }

}
