using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.IO;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 游戏驱动器(全局)
/// </summary>
public class glo_Main : FLEventDispatcherMono
{
    [HideInInspector]
    public string CodeName = "368";
    /// <summary>
    /// 游戏消息pool
    /// </summary>
    public ccMessagePoolV2 m_GameMessagePool = new ccMessagePoolV2();
    /// <summary>
    /// UI消息pool
    /// </summary>
    public ccMessagePoolV2 m_UIMessagePool = new ccMessagePoolV2();
    /// <summary>
    /// 表格数据pool
    /// </summary>
    public SC_Pool m_SC_Pool;
    /// <summary>
    /// 游戏资源管理器
    /// </summary>
    public ResourceManager m_ResourceManager;

    /// <summary>
    /// 命令控制器
    /// </summary>
    public CmdController m_CmdController = new CmdController();

    /// <summary>
    /// 声音控制器
    /// </summary>
    public AudioManager m_AdudioManager;
    /// <summary>
    /// 获取游戏驱动器单例
    /// </summary>
    private static glo_Main _Instance = null;
    /// <summary>
    /// 语言控制器 (0:中文，1：繁体)
    /// </summary>
    public static int _Language => LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_LanguageSetting, 0);

    /// <summary>
    /// 获取驱动器单例
    /// </summary>
    /// <returns></returns>
    public static glo_Main GetInstance()
    {
        return _Instance;
    }
    /// <summary>
    /// 初始化屏幕设置
    /// </summary>
    void Awake()
    {
        ccU3DEngine.MessageBox.f_OpenLog();
        if (_Instance != null)
        {
            if (m_AdudioManager != null)
            {
                Destroy(m_AdudioManager.gameObject);
                m_AdudioManager = null;
                Debug.LogWarning("Audio Manager 22222 Awake");
            }
            Destroy(this.gameObject);
            return;
        }
        _Instance = this;
        InitScreenSettings();
        _Instance.m_SC_Pool = new SC_Pool();
        _Instance.m_ResourceManager = new ResourceManager();
        GloData.glo_iVer = ccMath.atoi(Application.version);
        GloData.glo_strVer = GloData.glo_strVer + GloData.glo_iVer;        
    }

	void Start()
	{
#if DEBUG_MODE
		GameStartDebugScreen debugScreen = new GameObject("startDebugScreenGO").AddComponent<GameStartDebugScreen>();
		debugScreen.f_SetCallback(delegate () {
			StartGame();
			Destroy(debugScreen.gameObject);
		});
#else
        StartGame();
#endif
	}

    /// <summary>
    /// 初始化游戏socket
    /// 加载玩家设置
    /// 初始化音乐、UI注册、游戏本地数据
    /// 显示公司LOGO页面
    /// 加载资源检测更新，开启logo调试
    /// </summary>
    void StartGame()
    {
		ResourceTools.f_UpdateURL();
		DontDestroyOnLoad(this);
        GameSocket.GetInstance();
        GameSet.f_LoadGameSet();
        UIProfabPool.f_Init();
        InitLocalData();
        InitUI();
        //#if !UNITY_EDITOR && UNITY_ANDROID && ANDROIDUPDATE
        //        StartCoroutine(StartCheckVersion());
        //#else
        //        InitResManager();
        //#endif        
        InitResManager();

        InitMessage();

        //初始化SDK
        m_SDKCmponent.f_Init(gameObject);
        f_RegSDKEvent();

        InitReYun();
        
        ScreenControl.Instance.f_InitScreenDesign(StaticValue.m_DesignScreenWidth, StaticValue.m_DesignScreenHeight);
        ccTimeEvent.GetInstance().f_RegEvent(0.5f, false, null, RunLater);
        ccTimeEvent.GetInstance().f_RegEvent(1.5f, false, null, RunLater);

        f_InitInvalidComponent();
    }

    private void InitReYun()
    {
#if REYUN
        if (!string.IsNullOrEmpty(SDKHelper.REYUN_KEY))
        {
            Tracking.f_CreateObj();

            Tracking.Instance.setPrintLog(false);
            //初始化热云
            Tracking.Instance.init(SDKHelper.REYUN_KEY, SDKHelper.CHANNEL_ID.ToString());
        }
#endif
    }

    private void RunLater(object obj)
    {
        ScreenControl.Instance.f_InitScreenDesign(StaticValue.m_DesignScreenWidth, StaticValue.m_DesignScreenHeight);
    }
    private void InitMessage()
    {
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(MessageDef.START_LOG, CallbackStartLog);        
    }

    private void CallbackStartLog(object Obj)
    {
        f_StartLog();
    }

    /// <summary>
    /// 初始化UI
    /// 注册监听资源加载完成消息
    /// </summary>
    private void InitUI()
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShowLogoPage, UIMessageDef.UI_OPEN);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_RESOURCECOMPLETE, ResLoadPage.OnSetConfigSeted);

        //ccUIManage.GetInstance().f_RegClickSleep(10 * 60, Callback_ClickSleep);
        //TestBuild();


    }

#region UI闲制处理
    private void Callback_ClickSleep(object Obj)
    {
        int iTime = (int)Obj;
        MessageBox.DEBUG("Callback_ClickSleep");
        if (StaticValue.m_curScene == EM_Scene.GameMain && !SleepTimePage.m_ShowPage && !Data_Pool.m_GuidancePool.IsEnter)//新手引导不弹
            ccUIManage.GetInstance().f_SendMsg(UINameConst.SleepTimePage, UIMessageDef.UI_OPEN);
    }
#endregion



    /// <summary>
    /// 是否使用调试log
    /// </summary>
    private bool _bUseLog = false;
    private List<string> _aLogText = new List<string>();//调试文本
    private string _strLogPath = "";//调试文本输出路径
    System.DateTime _SleepLogTime;

    /// <summary>
    /// 开启调试log
    /// </summary>
    public void f_StartLog()
    {
        if (GloData.glo_iAutoUpdateLog == 1)
        {
            _SleepLogTime = System.DateTime.Now;
            if (!_bUseLog)
            {
                _bUseLog = true;
                //_strLogPath = Application.persistentDataPath + "/FinalDevilV9.txt";
                //if (System.IO.File.Exists(_strLogPath))
                //{
                //    File.Delete(_strLogPath);
                //}
                ////在这里做一个Log的监听
                //Application.logMessageReceived += CallBack_SystemLog;
                MessageBox.f_RegLogCall(CallBackMessageBox);
            }
        }

    }
    /// <summary>
    /// 调试打印接口
    /// </summary>
    /// <param name="logString">调试字符串</param>
    /// <param name="stackTrace">栈</param>
    /// <param name="type">调试类型</param>
    public void CallBack_SystemLog(string logString, string stackTrace = null, LogType type = LogType.Log)
    {
        if (_bUseLog)
        {
            _aLogText.Add(logString);
        }

    }

    private void CallBackMessageBox(int iMsgType, string strMsg)
    {
        if (_bUseLog)
        {
            if (GloData.m_bDebugLog)
            {
                if (iMsgType == 0)
                {
                    Debug.Log(strMsg);
                }
                else
                {
                    Debug.LogError(strMsg);
                }
            }
            _aLogText.Add(strMsg);
        }
    }

    /// <summary>
    /// 保存log到txt文本
    /// </summary>
    void SaveLog()
    {
        if (!_bUseLog || _aLogText.Count <= 0)
        {
            return;
        }

        TimeSpan ttt = System.DateTime.Now - _SleepLogTime;
        if (_aLogText.Count < GloData.glo_iMaxLogSize && ttt.TotalSeconds < GloData.glo_iAutoUpdateLogTime)
        {
            return;
        }

        _SleepLogTime = System.DateTime.Now;
        string userId = SystemInfo.deviceUniqueIdentifier;
        if (Data_Pool.m_UserData != null && Data_Pool.m_UserData.m_iUserId > 0)
        {
            userId = Data_Pool.m_UserData.m_iUserId.ToString();
        }
        LogTools.f_SaveLog(userId, _aLogText);
        _aLogText.Clear();
    }

    /// <summary>
    /// 初始化本地数据
    /// </summary>
    void InitLocalData()
    {
        LocalDataManager.f_SetLocalData<bool>(LocalDataType.AutoSky, false);
        LocalDataManager.f_SetLocalData<bool>(LocalDataType.SevenStarBlessUp, false);
        LocalDataManager.f_GetLocalDataIfNotExitSetData<float>(LocalDataType.Float_MusicVolumn, 1);
        LocalDataManager.f_GetLocalDataIfNotExitSetData<float>(LocalDataType.Float_EffectVolumn, 1);
        LocalDataManager.f_GetLocalDataIfNotExitSetData<string>(LocalDataType.String_UserName, "");
        LocalDataManager.f_GetLocalDataIfNotExitSetData<string>(LocalDataType.String_Password, "");
    }
    /// <summary>
    /// 初始化游戏屏幕设置
    /// </summary>
    void InitScreenSettings()
    {
        Application.runInBackground = true;
        //Application.targetFrameRate = 30;
        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        ScreenControl.Instance.f_InitScreenDesign(StaticValue.m_DesignScreenWidth, StaticValue.m_DesignScreenHeight);
    }
    /// <summary>
    /// 初始化资源管理
    /// 开始加载资源
    /// </summary>
    private void InitResManager()
    {
        MessageBox.DEBUG("InitResManager...");

        startLoadResourceTime = Time.realtimeSinceStartup;
        LoadResource _LoadResource = new LoadResource();
        _LoadResource.f_StartLoad(Callback_LoadResSuc);
    }
    private float startLoadResourceTime = 0;
    /// <summary>
    /// 资源成功加载完回调
    /// 初始化玩家数据结构
    /// 广播资源加载完成（通知显示logo页面可以进入游戏）
    /// </summary>
    /// <param name="Obj"></param>
    void Callback_LoadResSuc(object Obj)
    {
string resourceTimeHint = "Loading resource、time taken: " + (Time.realtimeSinceStartup - startLoadResourceTime) + " seconds.";
        CallBack_SystemLog(resourceTimeHint);
        MessageBox.DEBUG(resourceTimeHint);
        Data_Pool.f_InitPool();
        LegionMain.f_ClearData();
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_RESOURCECOMPLETE);

    }
    /// <summary>
    /// 定时更新
    /// </summary>
    void FixedUpdate()
    {
        SaveLog();
        m_ResourceManager.f_Update();

        GameSocket.GetInstance().f_Update();
        ChatSocket.GetInstance().f_Update();

        m_GameMessagePool.f_Update();
        m_UIMessagePool.f_Update();

        f_UpdateInvalidScriptComponent();
    }


#if DEBUG
    string debugParams = "";
    bool isDebugTest = false;
    private void OnGUI()
    {
        if (isDebugTest || Input.GetKeyDown(KeyCode.Insert)) {
GUILayout.Label("Parameters（separated by ,）:");
            debugParams = GUILayout.TextField(debugParams);
            isDebugTest = true;
            if (GUILayout.Button("Debug")) {
                isDebugTest = false;
                string[] testParams = debugParams.Split(',');
                if (testParams[0] == "open")
                {
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.MallCardDisplayPage, UIMessageDef.UI_OPEN);
                }
                else
                {
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.MallCardDisplayPage, UIMessageDef.UI_CLOSE);
                }
            }           
        }          
    }
#endif

    /// <summary>
    /// 脚本销毁时事件处理
    /// </summary>
    private void OnDestroy()
    {

    }
    /// <summary>
    /// 程序退出时事件处理
    /// </summary>
    void OnApplicationQuit()
    {
        GameSocket.GetInstance().f_Close();
        ChatSocket.GetInstance().f_Close();
    }
    /// <summary>
    /// 登陆游戏成功，初始游戏数据准备进入游戏
    /// 成功登录游戏做一些操作
    /// </summary>
    public void f_InitGame()
    {
        SwichAccountCheck();
    }
    /// <summary>
    /// 检测是否是切换账号登录，如切换账号，则初始化相关数据
    /// </summary>
    private void SwichAccountCheck()
    {
        if (!LocalDataManager.f_HasLocalData(LocalDataType.Long_LastLoginUserID))
        {
            LocalDataManager.f_SetLocalData<long>(LocalDataType.Long_LastLoginUserID, Data_Pool.m_UserData.m_iUserId);
        }
        else
        {
            long lastLoginUserid = LocalDataManager.f_GetLocalData<long>(LocalDataType.Long_LastLoginUserID);
            if (Data_Pool.m_UserData.m_iUserId != lastLoginUserid)//切换账号登录
            {
                LocalDataManager.f_SetLocalData<long>(LocalDataType.Long_LastLoginUserID, Data_Pool.m_UserData.m_iUserId);
                StaticValue.m_isPlayMusic = true;
                StaticValue.m_isPlaySound = true;
                LocalDataManager.f_SetLocalData<float>(LocalDataType.Float_MusicVolumn, 1);
                LocalDataManager.f_SetLocalData<float>(LocalDataType.Float_EffectVolumn, 1);
                LocalDataManager.f_SetLocalData<int>(LocalDataType.Int_BattleSpeed, 1);
                LocalDataManager.f_SetLocalData<int>(LocalDataType.FirstGuidance, -99);
            }
        }
    }
    /// <summary>
    /// 开启异步操作
    /// </summary>
    /// <param name="routine"></param>
    /// <returns></returns>
    public Coroutine f_StartCoroutine(IEnumerator routine)
    {
        return StartCoroutine(routine);
    }
    /// <summary>
    /// 切换游戏场景，清理垃圾
    /// </summary>
    public void f_ChangeScene()
    {
        m_UIMessagePool.f_Clear();
        ccUIHoldPool.GetInstance().f_ClearHold();
        //ccResourceManager.GetInstance().f_ClearCatch();
        //m_ResourceManager.f_Clear();
        ccUIManage.GetInstance().f_Clear();
        Data_Pool.m_ReddotMessagePool.f_Clear();//清除红点注册信息
    }

    /// <summary>
    /// 在SDK退出前 要释放的资源放这里  游戏退出统一走SDKComponent.f_Exit();
    /// </summary>
    private void f_DisponeBySDKExit()
    {

    }

#region SDKComponent 部分

    public SDKComponent m_SDKCmponent;

    private void f_RegSDKEvent()
    {
        m_GameMessagePool.f_AddListener(MessageDef.SDK_EXIT_BY_GAMEUI, f_OnSDKExitByGameUI, this);
        m_GameMessagePool.f_AddListener(MessageDef.SDK_PAY_RESULT, f_OnSDKPayResult, this);
        m_GameMessagePool.f_AddListener(MessageDef.SDK_CHANGE_ACCOUNT, f_OnSDKChangeAccount, this);
    }

    private void f_UnregSDKEvent()
    {
        m_GameMessagePool.f_RemoveListener(MessageDef.SDK_EXIT_BY_GAMEUI, f_OnSDKExitByGameUI, this);
        m_GameMessagePool.f_RemoveListener(MessageDef.SDK_PAY_RESULT, f_OnSDKPayResult, this);
        m_GameMessagePool.f_RemoveListener(MessageDef.SDK_CHANGE_ACCOUNT, f_OnSDKChangeAccount, this);
    }


    //SDK退出部分处理
    private void f_OnSDKExitByGameUI(object sureAction)
    {
        if (sureAction != null && sureAction is Action)
        {
PopupMenuParams tParams = new PopupMenuParams("Nhắc nhở", "Xác nhận thoát?", "Đồng ý", f_OnSureSDKExit, "Hủy bỏ", f_OnCancelExit, sureAction);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParams);
        }
    }

    private void f_OnSureSDKExit(object sureAction)
    {
        if (sureAction != null && sureAction is Action)
        {
            //先处理游戏要D释放的资源
            Action tAction = (Action)sureAction;
            if (tAction != null)
                tAction();
        }
    }

    private void f_OnCancelExit(object value)
    {
        MessageBox.DEBUG("Cancel exit.");
    }


    //SDK充值部分

    private Queue<SDKPccaccyResult> payResultQueue = new Queue<SDKPccaccyResult>();

    private int[] orderIdCheckTimes = new int[] { 1, 3, 5, 15 };
    private int curOrderIdCheckIdx = 0;

    private const float PAY_MASK_MIN_TIME = 1.0f;
private const string PAY_MASK_TIP_LABEL = "Confirm command";

    private void f_OnSDKPayResult(object payResult)
    {
        SDKPccaccyResult tResult = (SDKPccaccyResult)payResult;
        if (tResult.m_Result == EM_PccaccyResult.UnsettledPay || tResult.m_Result == EM_PccaccyResult.Whitelist)
        {
            if (payResultQueue.Count == 0)
            {
                payResultQueue.Enqueue(tResult);
                f_ProcessSDKPayResult(tResult);
            }
            else
            {
                payResultQueue.Enqueue(tResult);
            }
        }
        else if (tResult.m_Result == EM_PccaccyResult.Success)
            f_ProcessSDKPayResult(tResult);
    }

    private void f_ProcessSDKPayResult(SDKPccaccyResult node)
    {
        switch (node.m_Result)
        {
            case EM_PccaccyResult.Success:
                node.m_iEventId = ccTimeEvent.GetInstance().f_RegEvent(0.1f, false, node, f_CheckOrderIdByTimeEvent);
                break;
            case EM_PccaccyResult.Cancel:
                //UITool.f_OpenOrClosePayMask(true, PAY_MASK_TIP_LABEL, PAY_MASK_MIN_TIME);
                break;
            case EM_PccaccyResult.Failed:
                //UITool.f_OpenOrClosePayMask(true, PAY_MASK_TIP_LABEL, PAY_MASK_MIN_TIME);
                break;
            case EM_PccaccyResult.UnsettledPay:
                UITool.f_OpenOrClosePccaccyMask(true, PAY_MASK_TIP_LABEL, 0f);
                curOrderIdCheckIdx = 0;
                //流水号充值
                f_CheckOrderIdByServer(node);
                break;
            case EM_PccaccyResult.Whitelist:
                UITool.f_OpenOrClosePccaccyMask(true, PAY_MASK_TIP_LABEL, 0f);
                //白名单充值
                SocketCallbackDT whitelistCallbackDt = new SocketCallbackDT();
                whitelistCallbackDt.m_ccCallbackSuc = f_OnRechargeWhitelist;
                whitelistCallbackDt.m_ccCallbackFail = f_OnRechargeWhitelist;
                Data_Pool.m_RechargePool.RechargeWhitelist(node.m_PayTemplateId, whitelistCallbackDt);
                break;
            case EM_PccaccyResult.Invalid:
                MessageBox.ASSERT("PayResult is invalid.");
                break;
            default:
                MessageBox.ASSERT("PayResult default.");
                break;
        }
    }

    //处理渠道成功 开始
    private const int TimeEventCheckPayResultMaxCount = 12;
    private SDKPccaccyResult curTimeEventPayResult = null;

    private void f_CheckOrderIdByTimeEvent(object value)
    {
        if (curTimeEventPayResult != null)
            return;
        SDKPccaccyResult tPayResult = (SDKPccaccyResult)value;
        curTimeEventPayResult = tPayResult;
        SocketCallbackDT orderIdCallbackDt = new SocketCallbackDT();
        orderIdCallbackDt.m_ccCallbackSuc = f_OnTimeEventCheckOrderId;
        orderIdCallbackDt.m_ccCallbackFail = f_OnTimeEventCheckOrderId;
        Data_Pool.m_RechargePool.RechargeSDK(tPayResult.m_OrderId, orderIdCallbackDt);
    }


    private void f_OnTimeEventCheckOrderId(object result)
    {
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
MessageBox.DEBUG(string.Format("TimeEvent Successful deposit activation，payOrderId:{0}; payTemplateId:{1}; times:{2}; eventId:{3}", curTimeEventPayResult.m_OrderId, curTimeEventPayResult.m_PayTemplateId, curTimeEventPayResult.m_iTimes, curTimeEventPayResult.m_iEventId));
            PccaccyDT tPayDt = (PccaccyDT)glo_Main.GetInstance().m_SC_Pool.m_PaySC.f_GetSC(curTimeEventPayResult.m_PayTemplateId);

#if REYUN
            if (!string.IsNullOrEmpty(SDKHelper.REYUN_KEY))
            {
                //支付方式(paymentType):无法获取统一写alipay  货币类型(currentType)：无法获取统一写CNY  价格(amount):单位：元
                Tracking.Instance.setryzf(curTimeEventPayResult.m_OrderId, "alipay", "CNY", (float)tPayDt.iPccaccyNum);
                MessageBox.DEBUG(string.Format("ReyunSDK SetPayment,m_OrderId:{0}, amount:{1}, date:{2}", curTimeEventPayResult.m_OrderId, (float)tPayDt.iPccaccyNum, DateTime.Now.ToString("HH-mm-ss")));
            }
#endif

            string tPayName = UITool.f_GetPayName(tPayDt);
TopPopupMenuParams tParam = new TopPopupMenuParams("Đang xử lý lệnh gửi tiền", string.Format("{0} Gửi tiền thành công", tPayName));
            ccUIManage.GetInstance().f_SendMsg(UINameConst.TopPopupMenuPage, UIMessageDef.UI_OPEN, tParam);
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_PAY_UPDATE_VIEW);
            //ccTimeEvent.GetInstance().f_UnRegEvent(curTimeEventPayResult.m_iEventId);
            //curTimeEventPayResult.m_iEventId = 0;
            curTimeEventPayResult = null;
        }
        else
        {
MessageBox.DEBUG(string.Format("TimeEvent Deposit activation error(Code does not exist on server))，payOrderId:{0}; payTemplateId:{1}; times:{2}; eventId:{3}", curTimeEventPayResult.m_OrderId, curTimeEventPayResult.m_PayTemplateId, curTimeEventPayResult.m_iTimes, curTimeEventPayResult.m_iEventId));
            curTimeEventPayResult.m_iTimes++;
            if (curTimeEventPayResult.m_iTimes > TimeEventCheckPayResultMaxCount)
            {
MessageBox.ASSERT(string.Format("TimeEvent Deposit has timed out，payOrderId:{0}; payTemplateId:{1}; times:{2}; eventId:{3}", curTimeEventPayResult.m_OrderId, curTimeEventPayResult. m_PayTemplateId, curTimeEventPayResult.m_iTimes, curTimeEventPayResult.m_iEventId));
                //ccTimeEvent.GetInstance().f_UnRegEvent(curTimeEventPayResult.m_iEventId);
                //curTimeEventPayResult.m_iEventId = 0;
                curTimeEventPayResult = null;
            }
            else
            {
                curTimeEventPayResult.m_iEventId = ccTimeEvent.GetInstance().f_RegEvent(5.0f, false, curTimeEventPayResult, f_CheckOrderIdByTimeEvent);
                curTimeEventPayResult = null;
            }
                
        }
    }

    //处理渠道成功 结束


    private void f_CheckOrderIdByServer(object payResult)
    {
        SDKPccaccyResult node = (SDKPccaccyResult)payResult;
        SocketCallbackDT orderIdCallbackDt = new SocketCallbackDT();
        orderIdCallbackDt.m_ccCallbackSuc = f_OnRechargeOrderId;
        orderIdCallbackDt.m_ccCallbackFail = f_OnRechargeOrderId;
        Data_Pool.m_RechargePool.RechargeSDK(node.m_OrderId, orderIdCallbackDt);
    }

    private void f_OnRechargeOrderId(object result)
    {

        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            SDKPccaccyResult tPayResult = payResultQueue.Dequeue();
#if REYUN
            if (!string.IsNullOrEmpty(SDKHelper.REYUN_KEY))
            {
                //支付方式(paymentType):无法获取统一写alipay  货币类型(currentType)：无法获取统一写CNY  价格(amount):单位：元
                PccaccyDT tPayDt = (PccaccyDT)glo_Main.GetInstance().m_SC_Pool.m_PaySC.f_GetSC(tPayResult.m_PayTemplateId);
                Tracking.Instance.setryzf(tPayResult.m_OrderId, "alipay", "CNY", (float)tPayDt.iPccaccyNum);
                MessageBox.DEBUG(string.Format("ReyunSDK SetPayment,m_OrderId:{0}, amount:{1}, date:{2}", tPayResult.m_OrderId, (float)tPayDt.iPccaccyNum, DateTime.Now.ToString("HH-mm-ss")));
            }
#endif

            f_ProcessSurePayResult((eMsgOperateResult)result, tPayResult);
            UITool.f_OpenOrClosePccaccyMask(false, string.Empty, 0);
            if (payResultQueue.Count > 0)
            {
                SDKPccaccyResult tNeedProcessResult = payResultQueue.Peek();
                f_ProcessSDKPayResult(tNeedProcessResult);
            }
        }
        else
        {
            curOrderIdCheckIdx++;
            if (curOrderIdCheckIdx < orderIdCheckTimes.Length)
            {
                SDKPccaccyResult tPayResult = payResultQueue.Peek();
                ccTimeEvent.GetInstance().f_RegEvent(orderIdCheckTimes[curOrderIdCheckIdx], false, tPayResult, f_CheckOrderIdByServer);
            }
            else
            {
                SDKPccaccyResult tPayResult = payResultQueue.Dequeue();
                UITool.f_OpenOrClosePccaccyMask(false, string.Empty, 0);
                f_ProcessSurePayResult((eMsgOperateResult)result, tPayResult);
                //UITool.Ui_Trip("充值超时");
MessageBox.ASSERT(string.Format("The deposit code does not exist on the server,orderId:{0} len:{1}", tPayResult.m_OrderId, tPayResult.m_OrderId.Length));
                if (payResultQueue.Count > 0)
                {
                    SDKPccaccyResult tNeedProcessResult = payResultQueue.Peek();
                    f_ProcessSDKPayResult(tNeedProcessResult);
                }
            }
        }
    }

    private void f_OnRechargeWhitelist(object result)
    {
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            //白名单充值成功
            UITool.Ui_Trip("" + CommonTools.f_GetTransLanguage(2276));
        }
        else
        {
            MessageBox.ASSERT("Recharge whitelist failed,code:" + (int)result);
        }
        SDKPccaccyResult tPayResult = payResultQueue.Dequeue();
        f_ProcessSurePayResult((eMsgOperateResult)result, tPayResult);
        UITool.f_OpenOrClosePccaccyMask(false, string.Empty, 0);
        if (payResultQueue.Count > 0)
        {
            SDKPccaccyResult tNeedProcessResult = payResultQueue.Peek();
            f_ProcessSDKPayResult(tNeedProcessResult);
        }
    }

    private void f_ProcessSurePayResult(eMsgOperateResult operateResult, SDKPccaccyResult payResult)
    {
        Data_Pool.m_RechargePool.f_ProcessUnsettledPay(operateResult, payResult.m_Result, payResult.m_OrderId, payResult.m_PayTemplateId);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_PAY_UPDATE_VIEW, payResult);
    }

    // SDK切换账号部分

    private void f_OnSDKChangeAccount(object value)
    {
        m_SDKCmponent.f_UpdateAccountState((EM_SDKAccountState)value);
        LoadingPage.isStartGame = true;
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.LOADING_LOGIN_SCENE_SUC, f_OnLoadingLoginSceneSuc);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.Login);
    }

    private void f_OnLoadingLoginSceneSuc(object value)
    {
        m_GameMessagePool.f_Clear();
        m_UIMessagePool.f_Clear();
        GameObject timeEvent = GameObject.Find("ccTimeEvent");
        if (timeEvent != null)
        {
            Destroy(timeEvent);
        }
        GameSocket.GetInstance().Destroy();
        ChatSocket.GetInstance().Destroy();
        Data_Pool.f_InitPool();
        StaticValue.f_ReInit();
        LegionMain.f_ClearData();
        ccUIManage.GetInstance().f_UnSetCurClickButton();
        f_RegSDKEvent();
        LoadingPage.isStartGame = false;
        if (glo_Main.GetInstance().m_SDKCmponent.AccountState == EM_SDKAccountState.LoginOut)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LoginPage, UIMessageDef.UI_OPEN, true);
        }
        else if (glo_Main.GetInstance().m_SDKCmponent.AccountState == EM_SDKAccountState.SwitchSuc)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LoginPage, UIMessageDef.UI_OPEN, false);
        }
    }

#endregion

    private void Update()
    {
        if (asyncOperation != null)
        {
            if (asyncOperation.isDone)
            {
                //加载登录界面 发送成功事件
                if (StaticValue.m_curScene == EM_Scene.Login)
                    glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.LOADING_LOGIN_SCENE_SUC);
                asyncOperation = null;
            }
        }
#if !UNITY_IOS || UNITY_EDITOR
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space)) && StaticValue.m_curScene != EM_Scene.Login)
        {
            m_SDKCmponent.f_Exit();
            return;
        }
#endif
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    m_ResourceManager.GuidanceResourceList.f_Export();
        //}
        
    }

#region 加载 临时的

    public AsyncOperation asyncOperation = null;

#endregion

#region InvalidScriptsComponent 部分

    private InvalidScriptComponent m_InvalidScriptComponent;

    private void f_InitInvalidComponent()
    {
        m_InvalidScriptComponent = new InvalidScriptComponent();
        m_InvalidScriptComponent.Init();
    }

    private void f_UpdateInvalidScriptComponent()
    {
        if (m_InvalidScriptComponent != null)
        {
            m_InvalidScriptComponent.Update();
        }
    }

#endregion
   
}
