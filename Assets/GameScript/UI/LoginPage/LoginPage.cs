// #define QUICK_SDK
//#define NEXTGEN_SDK

using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
using System;
using Spine.Unity;
/// <summary>
/// 登录界面
/// </summary>
public class LoginPage : UIFramwork
{
    private GameObject m_EnterGameParent;
    private GameObject m_SwitchAccountParent;
    private GameObject m_BtnSwitchAccount;
    private GameObject m_BtnQuickSwitchAccount;
    public static ServerInforDT selectServerDT = null;//选中的服务器，默认为空
    private UILabel mlabelServerState;
    private string[] m_ServerStateName;
    private UISprite mspriteServerState;
    private string[] m_ServerStateIcon;
	//My Code
	GameParamDT AssetOpen;
	GameObject TexBgKD;
	//

    private string texPath = "UI/TextureRemove/Login/Tex_LoginBg";
	private string texPath2 = "Tex_LoginBg";

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        m_EnterGameParent = f_GetObject("EnterGameParent");
        m_SwitchAccountParent = f_GetObject("SwitchAccountParent");
        m_BtnSwitchAccount = f_GetObject("BtnSwitchAccount");
        m_BtnQuickSwitchAccount = f_GetObject("BtnQuickSwitchAccount");
        mlabelServerState = f_GetObject("LabelState").GetComponent<UILabel>();
        m_ServerStateName = new string[(int)EM_ServerState.Max];
        m_ServerStateName[(int)EM_ServerState.New]        = CommonTools.f_GetTransLanguage(2209);
        m_ServerStateName[(int)EM_ServerState.Unhindered] = CommonTools.f_GetTransLanguage(2210);
        m_ServerStateName[(int)EM_ServerState.Hot]        = CommonTools.f_GetTransLanguage(2211);
        m_ServerStateName[(int)EM_ServerState.Maintain]   = CommonTools.f_GetTransLanguage(2212);

        mspriteServerState = f_GetObject("SpriteState").GetComponent<UISprite>();
        m_ServerStateIcon = new string[(int)EM_ServerState.Max];
        m_ServerStateIcon[(int)EM_ServerState.New]        = "Border_jrdk1";
        m_ServerStateIcon[(int)EM_ServerState.Unhindered] = "Border_jrdk1";
        m_ServerStateIcon[(int)EM_ServerState.Hot]        = "Border_jrdk2";
        m_ServerStateIcon[(int)EM_ServerState.Maintain]   = "Border_jrdk3";
        f_RegClickEvent(m_BtnQuickSwitchAccount, f_BtnQuickSwitchAccount);
		//My Code
		AssetOpen = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(93) as GameParamDT);
		//
    }

    /// <summary>
    /// 页面打开
    /// </summary>
    protected override void UI_OPEN(object e)
    {
        _NeedCloseSound = false;
        base.UI_OPEN(e);
        selectServerDT = null;
        bool isNeedCallLogin = true;
        if(e is bool)
            isNeedCallLogin = (bool)e;
        bool isHasAccount = LocalDataManager.f_GetLocalData<string>(LocalDataType.String_UserName) != "";
        if (!isHasAccount && !glo_Main.GetInstance().m_SDKCmponent.IsChannel) //没有账号则弹出切换账号界面
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.SwichAccountPage, UIMessageDef.UI_OPEN);
        }
        //glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.LoginBg);
        InitUI();
        if(LocalDataManager.f_HasLocalData(LocalDataType.String_ServerID))//旧号，如果本地上一次登录服务器
        {
            string serverID = LocalDataManager.f_GetLocalData<string>(LocalDataType.String_ServerID);
            ServerInforDT infoDT = f_TryGetServerNameByHistoryIP(serverID, glo_Main.GetInstance().m_SDKCmponent.f_GetSdkChannelType());
            SelectServerPage.mNearSelectServer1 = infoDT;
            if(LocalDataManager.f_HasLocalData(LocalDataType.String_ServerID2))
            {
                string serverID2 = LocalDataManager.f_GetLocalData<string>(LocalDataType.String_ServerID2);
                ServerInforDT infoDT2 = (ServerInforDT)glo_Main.GetInstance().m_SC_Pool.m_ServerInforSC.f_GetSC(int.Parse(serverID2));
                SelectServerPage.mNearSelectServer2 = infoDT2;
            }
            else
            {
                SelectServerPage.mNearSelectServer2 = null;
            }
            if(infoDT == null)
                infoDT = f_GetLastOpenServerIP(glo_Main.GetInstance().m_SDKCmponent.f_GetSdkChannelType());
            selectServerDT = infoDT;
            OnSeleceServerNameMsg(infoDT);
        }
        else
        {
            ServerInforDT infoDT = f_GetLastOpenServerIP(glo_Main.GetInstance().m_SDKCmponent.f_GetSdkChannelType());
            selectServerDT = infoDT;
            OnSeleceServerNameMsg(infoDT);
            SelectServerPage.mNearSelectServer1 = null;
            SelectServerPage.mNearSelectServer2 = null;
        }

        //预加载主角模型
        CardDT ManCardDT = glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(GameParamConst.ManCardId) as CardDT;
        glo_Main.GetInstance().m_ResourceManager.f_PreloadRoleRes(((RoleModelDT)glo_Main.GetInstance().m_SC_Pool.m_RoleModelSC.f_GetSC(ManCardDT.iStatelId1)).iModel);
        CardDT WomanCardDT = glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(GameParamConst.WomanCardId) as CardDT;
        glo_Main.GetInstance().m_ResourceManager.f_PreloadRoleRes(((RoleModelDT)glo_Main.GetInstance().m_SC_Pool.m_RoleModelSC.f_GetSC(WomanCardDT.iStatelId1)).iModel);

        if(isNeedCallLogin)
        {
            f_HideUIBeforeSDKLoginSuccess();
            glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.SDK_LOGIN_RESULT, f_OnSDKLoginResult, this);
#if Y_SDK
            glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.SDK_YSDK_SHOWLOGINVIEW, f_OnYSDKQuickLoginResult, this);
            glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.SDK_YSDK_SHOWSHOWNICKNAME, f_OnNickNameFresh, this);
#endif
            glo_Main.GetInstance().m_SDKCmponent.f_Login();
        }
        else
        {
            f_ShowUIAfterSDKLoginSuccess();
        }
        f_LoadTexture();
        //glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.BattleMusic);

        HandleSelectLanguage();

    }
    private List<string> _languageList = new List<string>();
    private UIPopupList _languagePopupList;
    private UILabel _languageSelected;

    private void HandleSelectLanguage()
    {
        _languageList.Clear();
        _languageList.Add("Vietnamese");
        _languageList.Add("English");
        _languageList.Add("ThaiLand");

        _languagePopupList = f_GetObject("languagePopUp").GetComponent<UIPopupList>();
        _languagePopupList.Clear();

        //_languageSelected = f_GetObject("selectedPopUp").GetComponent<UILabel>();
        foreach (var item in _languageList) { _languagePopupList.AddItem(item); }
        int languageSet = LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_LanguageSetting, 1);
        //_languageSelected.text = 
         _languagePopupList.value = _languageList[languageSet];
        EventDelegate.Add(_languagePopupList.onChange, () => {
            UITool.OnLanguageSelectionChaged(_languageList, _languageSelected);
        });
    }

    private string strTex_YsdkBgRoot = "UI/TextureRemove/Login/Tex_YsdkBg";
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
		//My Code
		UITexture Tex = f_GetObject("bg").GetComponent<UITexture>();
		TexBgKD = GameObject.Find("logo2KD");
		if(TexBgKD != null)
		{
			if(AssetOpen.iParam2 == 0)
			{
				TexBgKD.SetActive(false);
			}
		}
		Texture2D tTexture2D = null;
		if(AssetOpen.iParam1 == 1)
		{
			// tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("Tex_LogoLoginBg");
		}
        Tex.mainTexture = tTexture2D;
		if (Tex.mainTexture == null)
        {
            tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(texPath);
			Tex.mainTexture = tTexture2D;
        }
		//
#if Y_SDK
        //加载背景图
        UITexture Tex_YsdkBg = f_GetObject("Tex_YsdkBg").GetComponent<UITexture>();
        if (Tex_YsdkBg.mainTexture == null)
        {
            Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTex_YsdkBgRoot);
            Tex_YsdkBg.mainTexture = tTexture2D;
        }
#endif
    }
	
	private void InitRecruitModel(GameObject parent, int cardId, bool dir, Vector3 postion, int index)
    {
		UITool.f_GetStatelObject(cardId, parent.transform, dir ? Vector3.zero : new Vector3(0,180,0), postion, 4, "Model", 1);
    }

    /// <summary>
    /// 获取表中最后一个已开启的ip
    /// </summary>
    /// <returns></returns>
    private ServerInforDT f_GetLastOpenServerIP(string szCurChanel)
    {
        List<NBaseSCDT> listServerData = glo_Main.GetInstance().m_SC_Pool.m_ServerInforSC.f_GetAll();
        int openIndex = -1;
        for(int i = 0; i < listServerData.Count; i++)
        {
            ServerInforDT dt = listServerData[i] as ServerInforDT;
            List<string> canNotLoginChanel = new List<string>(dt.szLockChanel.Split(';'));
            if(canNotLoginChanel.Contains(szCurChanel))
                continue;
            if(dt.szAutoOpenTime != null && dt.szAutoOpenTime != "" && !CommonTools.f_CheckServerTimeOpen(dt.szAutoOpenTime))
                continue;
            if(dt.iServState != (int)EM_ServerState.UnOpen && dt.iServState != (int)EM_ServerState.Preheat)
                openIndex = i;
        }
        if(openIndex == -1)//没有任何服务器
        {
            return null;
        }
        ServerInforDT lastServerInforDT = listServerData[openIndex] as ServerInforDT;
        GloData.glo_strSvrIP = lastServerInforDT.szIP;
        GloData.glo_iSvrPort = lastServerInforDT.iPort;
        Data_Pool.m_UserData.f_SetServerInfo(lastServerInforDT.iServerId, lastServerInforDT.szName);
        return lastServerInforDT;
    }
    /// <summary>
    /// 1.历史登录ip获取服务器名字
    /// 2.如果在表中没有找到历史登录ip或该ip未开启，则返回null
    /// </summary>
    /// <param name="serverID">历史登录服务器id</param>
    /// <param name="szCurChanel">当前渠道标识</param>
    /// <returns></returns>
    private ServerInforDT f_TryGetServerNameByHistoryIP(string serverID, string szCurChanel)
    {
        List<NBaseSCDT> listServerData = glo_Main.GetInstance().m_SC_Pool.m_ServerInforSC.f_GetAll();
        for(int i = 0; i < listServerData.Count; i++)
        {
            ServerInforDT dt = listServerData[i] as ServerInforDT;
            List<string> canNotLoginChanel = new List<string>(dt.szLockChanel.Split(';'));
            if(canNotLoginChanel.Contains(szCurChanel))
                continue;
            if(dt.szAutoOpenTime != null && dt.szAutoOpenTime != "" && !CommonTools.f_CheckServerTimeOpen(dt.szAutoOpenTime))
                continue;
            if(dt.iServerId.ToString() == serverID && dt.iServState != (int)EM_ServerState.UnOpen && dt.iServState != (int)EM_ServerState.Preheat)
            {
                GloData.glo_strSvrIP = dt.szIP;
                GloData.glo_iSvrPort = dt.iPort;
                Data_Pool.m_UserData.f_SetServerInfo(dt.iServerId, dt.szName);
                return dt;
            }
        }
        return null;
    }
    /// <summary>
    /// 初始化UI(加载背景图和版本号信息)
    /// </summary>
    private void InitUI()
    {
        f_GetObject("labelVersion").GetComponent<UILabel>().text = GloData.glo_strVer;

        GameObject Ani = null;
        // UITool.f_CreateMagicById(20044, ref Ani, f_GetObject("Ani").transform, 4, null);
        // if(Ani != null)
        // {
            // SkeletonAnimation tSkeletonAnimation = Ani.GetComponent<SkeletonAnimation>();
            // if (null == tSkeletonAnimation) {
				// Debug.LogError("SkeletonAnimation null，modelname： " + Ani.name);
                // return;
            // }
            // tSkeletonAnimation.state.SetAnimation(0, "Stand", true);
        // }
		InitRecruitModel(f_GetObject("Ani"), 1401, true, new Vector3(0, -3, 0), 0);
    }
	
    #region 注册按钮点击事件
    /// <summary>
    /// 注册界面按钮点击事件
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnLogin", f_LoginGame);
        f_RegClickEvent("BtnSwitchAccount", f_SwichAccount);
        f_RegClickEvent("BtnSelectServer", f_OnSelectServerClick);
#if Y_SDK
        f_RegClickEvent("BtnYsdkSwitch", f_BtnYsdkSwitchClick);
        f_RegClickEvent("BtnYsdkQQLogin", f_BtnYsdkQQLoginClick);
        f_RegClickEvent("BtnYsdkWXLogin", f_BtnYsdkWXLoginClick);
#endif

        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_DOWNLOADRESERO, OnLoadingEroMessage, this);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_LOGINSERVERNAME, OnSeleceServerNameMsg, this);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_STARTGAME, LoadingPage.OnEnterGameHandle, this);
    }
    #endregion



    /// <summary>
    /// 选择服务器名字消息
    /// </summary>
    /// <param name="msg">服务器名字</param>
    private void OnSeleceServerNameMsg(object obj)
    {
        if(obj == null)
        {
            mlabelServerState.text = "";
            f_GetObject("LabelServerName").GetComponent<UILabel>().text = "";
            return;
        }
        ServerInforDT dt = (ServerInforDT)obj;
        selectServerDT = dt;             
        if(dt.szName != null)
            f_GetObject("LabelServerName").GetComponent<UILabel>().text = dt.szName;
        EM_ServerState serverState = UITool.f_GetServerState(dt);
        if (serverState >= EM_ServerState.Max) return;
        mlabelServerState.text = m_ServerStateName[(int)serverState];
        mspriteServerState.spriteName = m_ServerStateIcon[(int)serverState];
    }
    /// <summary>
    /// 加载错误函数回调
    /// </summary>
    /// <param name="Obj"></param>
    private void OnLoadingEroMessage(object Obj)
    {
        string ppSQL = (string)Obj;
    }
    #region 按钮事件及其回调处理
    /// <summary>
    /// 登陆游戏按钮事件
    /// </summary>
    public void f_LoginGame(GameObject go, object obj1, object obj2)
    {
        //#if !QUICK_SDK && !LCAT_SDK && !Y_SDK && !IOSGen_SDK
        if (!glo_Main.GetInstance().m_SDKCmponent.IsChannel)
        {
            string userName = LocalDataManager.f_GetLocalData<string>(LocalDataType.String_UserName);
            string psw = LocalDataManager.f_GetLocalData<string>(LocalDataType.String_Password);
            if (userName == "")
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.SwichAccountPage, UIMessageDef.UI_OPEN);
                return;
            }
        }
//#endif

        if (selectServerDT == null)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1672));
            return;
        }
        if(UITool.f_GetServerState(selectServerDT) == EM_ServerState.Maintain)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1673));
            return;
        }
        //检测渠道登录状况
        if(glo_Main.GetInstance().m_SDKCmponent.IsChannel && string.IsNullOrEmpty(glo_Main.GetInstance().m_SDKCmponent.ChannelRoleInfo.ChannelUserId))
        {
            f_HideUIBeforeSDKLoginSuccess();
            glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.SDK_LOGIN_RESULT, f_OnSDKLoginResult, this);
#if Y_SDK
            glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.SDK_YSDK_SHOWLOGINVIEW, f_OnYSDKQuickLoginResult, this);
            glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.SDK_YSDK_SHOWSHOWNICKNAME, f_OnNickNameFresh, this);
#endif
            glo_Main.GetInstance().m_SDKCmponent.f_Login();
            return;
        }

        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        StaticValue.m_LoginName = LocalDataManager.f_GetLocalData<string>(LocalDataType.String_UserName);
        StaticValue.m_LoginPwd = LocalDataManager.f_GetLocalData<string>(LocalDataType.String_Password);


        GameSocket.GetInstance().f_Login(Callback_Login);
        UITool.f_OpenOrCloseWaitTip(true);
        IsLoginRsp = false;
        ccTimeEvent.GetInstance().f_RegEvent(8f, false, null, OnTimeOutCallback);//超时显示提示消息
        //记录服务器Id
        if(!LocalDataManager.f_HasLocalData(LocalDataType.String_ServerID))
        {
            LocalDataManager.f_SetLocalData(LocalDataType.String_ServerID, Data_Pool.m_UserData.m_iServerId.ToString());
        }
        else
        {
            string tempServerId = LocalDataManager.f_GetLocalData<string>(LocalDataType.String_ServerID);
            if(tempServerId != Data_Pool.m_UserData.m_iServerId.ToString())
            {
                LocalDataManager.f_SetLocalData(LocalDataType.String_ServerID, Data_Pool.m_UserData.m_iServerId.ToString());
                LocalDataManager.f_SetLocalData(LocalDataType.String_ServerID2, tempServerId);
            }
        }
    }
    private bool IsLoginRsp = false;//登录是否返回消息
    /// <summary>
    /// 超时回调检测
    /// </summary>
    private void OnTimeOutCallback(object obj)
    {
        if(!IsLoginRsp)
        {
            UITool.f_OpenOrCloseWaitTip(false);

            //超时用假排队替换
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.OnLoginQueueEvent, LoginQueueType.LoginQueueType_FakeQueue);
            //ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1674));
        }
    }
    /// <summary>
    /// 登录返回
    /// </summary>
    /// <param name="Obj"></param>
    private void Callback_Login(object Obj)
    {
        IsLoginRsp = true;
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)Obj;
#if Y_SDK
        if (teMsgOperateResult == eMsgOperateResult.OR_Succeed || teMsgOperateResult == eMsgOperateResult.eOR_CreateAndLogin)
        {
            glo_Main.GetInstance().m_GameMessagePool.f_RemoveListener(MessageDef.SDK_YSDK_SHOWLOGINVIEW, f_OnYSDKQuickLoginResult, this);
            glo_Main.GetInstance().m_GameMessagePool.f_RemoveListener(MessageDef.SDK_YSDK_SHOWSHOWNICKNAME, f_OnNickNameFresh, this);
        }
#endif

        if (teMsgOperateResult == eMsgOperateResult.OR_Error_CreateAccountTimeOut
            || teMsgOperateResult == eMsgOperateResult.OR_Error_LoginTimeOut)
        {
            //超时已经弹出了登陆排队页面,不再弹提示
            return;
        }

        switch (teMsgOperateResult)
        {
            case eMsgOperateResult.OR_Succeed:
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
                break;
            case eMsgOperateResult.OR_Error_NoAccount:
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1675));
                break;
            case eMsgOperateResult.OR_Error_Password:
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1676));
                break;
            case eMsgOperateResult.eOR_CreateAndLogin:
                //打开创角界面已经写在Socket_SelCharacter里面
                break;
            case eMsgOperateResult.OR_Error_SeverMaintain:
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1677));
                break;
            case eMsgOperateResult.OR_Error_VersionNotMatch:
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1678));
                break;
            case eMsgOperateResult.eOR_IP_Forbidden:
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1679));
                break;
            case eMsgOperateResult.eOR_Account_Forbidden:
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1680));
                break;
            default:
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1681) + CommonTools.f_GetTransLanguage((int)Obj));
                break;
        }
    }
    /// <summary>
    /// 点击切换账号按钮事件
    /// </summary>
    private void f_SwichAccount(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SwichAccountPage, UIMessageDef.UI_OPEN);
    }
    /// <summary>
    /// 点击选择服务器按钮
    /// </summary>
    private void f_OnSelectServerClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SelectServerPage, UIMessageDef.UI_OPEN);
    }
    #endregion

    #region SDK相关登录部分

    private void f_OnSDKLoginResult(object isFailed)
    {
        int tIsFailed = (int)isFailed;
        if(tIsFailed == 0) //成功
        {
            f_ShowUIAfterSDKLoginSuccess();
            glo_Main.GetInstance().m_GameMessagePool.f_RemoveListener(MessageDef.SDK_LOGIN_RESULT, f_OnSDKLoginResult, this);
        }
    }
    private void f_HideUIBeforeSDKLoginSuccess()
    {
        m_EnterGameParent.SetActive(false);
        m_SwitchAccountParent.SetActive(false);
    }

    private void f_ShowUIAfterSDKLoginSuccess()
    {
        m_EnterGameParent.SetActive(true);
        m_SwitchAccountParent.SetActive(!glo_Main.GetInstance().m_SDKCmponent.IsChannel);
#if NEXTGEN_SDK
         m_SwitchAccountParent.SetActive(true);
         m_BtnSwitchAccount.SetActive(false);       //nosdk
         m_BtnQuickSwitchAccount.SetActive(true);   //sdk
#else
         m_BtnQuickSwitchAccount.SetActive(false);
         m_BtnSwitchAccount.SetActive(true);
#endif
    }

    #region 应用宝(YSDK相关)
#if Y_SDK
    /// <summary>
    /// 应用登录分支回调
    /// </summary>
    private void f_OnYSDKQuickLoginResult(object data)
    {
        string strMsg = (string)data;
        string[] tParam = ccMath.f_String2ArrayString(strMsg, ",");
        int isFail = int.Parse(tParam[0]);
        string userId = tParam[1];
        string token = tParam[2];
        string nick_name = strMsg.Substring(tParam[0].Length + tParam[1].Length + tParam[2].Length + 3);
        f_GetObject("LabelYsdkNickName").GetComponent<UILabel>().text = "";
        f_GetObject("YsdkLoginSucWidow").SetActive(isFail == 0);
        f_GetObject("YsdkLoginWidow").SetActive(isFail != 0);
    }
    /// <summary>
    /// 应用宝更新名字
    /// </summary>
    /// <param name="nickName"></param>
    private void f_OnNickNameFresh(object nickName)
    {
f_GetObject("LabelYsdkNickName").GetComponent<UILabel>().text = "Welcome，" + (string)nickName;
    }
    /// <summary>
    /// ysdk注销按钮事件
    /// </summary>
    private void f_BtnYsdkSwitchClick(GameObject go, object obj1, object obj2)
    {
        YSDKHelper.f_SwitchAccount();
        f_GetObject("YsdkLoginSucWidow").SetActive(false);
        f_GetObject("YsdkLoginWidow").SetActive(true);
    }
    /// <summary>
    /// ysdk QQ按钮事件
    /// </summary>
    private void f_BtnYsdkQQLoginClick(GameObject go, object obj1, object obj2)
    {
        YSDKHelper.f_LoginQQ();
    }
    /// <summary>
    /// ysdk 微信按钮事件
    /// </summary>
    private void f_BtnYsdkWXLoginClick(GameObject go, object obj1, object obj2)
    {
        YSDKHelper.f_LoginWX();
    }
#endif
    #endregion
    #endregion

    #region QuickSdk 切换账号相关

    private void f_BtnQuickSwitchAccount(GameObject go, object value1, object value2)
    {
        glo_Main.GetInstance().m_SDKCmponent.f_SwitchAccount(false);
    }

    #endregion

}
