using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using UnityEditor;

public class GameSocket : BaseSocket
{
    private Socket_Loop _Socket_Loop = null;
    private static GameSocket _Instance = null;
    private System.DateTime m_fLastTime;
    /// <summary>
    /// 跨天处理类
    /// </summary>
    private Day2Day _Day2Day = new Day2Day();
    public static GameSocket GetInstance()
    {
        if (null == _Instance)
        {
            _Instance = new GameSocket();
        }

        return _Instance;
    }

    public GameSocket()
    {
        m_strTTTT = "GameSocket";
    }

    protected override void InitMachine()
    {
        base.InitMachine();       
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.OnLoginQueueEvent, OpenLoginQueue);
        Socket_Wait tSocket_Wait = new Socket_Wait(this);
        _Socket_Loop = new Socket_Loop(this);
        _SocketMachineManger = new ccMachineManager(_Socket_Loop);
        _SocketMachineManger.f_RegState(new Socket_Regedit(this));
        _SocketMachineManger.f_RegState(new Socket_Connect(this));
        _SocketMachineManger.f_RegState(new Socket_Login(this));
        _SocketMachineManger.f_RegState(new Socket_Wait(this));
        _SocketMachineManger.f_RegState(new Socket_SelCharacter(this));
        _SocketMachineManger.f_ChangeState(tSocket_Wait);

    }

    /// <summary>
    /// 登陆排队，-1关闭排队界面，0 打开假排队 其他打开服务器排队
    /// </summary>
    /// <param name="obj"></param>
    private void OpenLoginQueue(object obj)
    {
        if (StaticValue.m_curScene != EM_Scene.Login)
            return;
        int code = (int)obj;
        if (code == (int)LoginQueueType.LoginQueueType_Close)
        {
            if(ccUIManage.GetInstance().f_CheckUIIsOpen(UINameConst.LoginQueuePage))
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LoginQueuePage, UIMessageDef.UI_CLOSE);
        }
        else if (code == (int)LoginQueueType.LoginQueueType_FakeQueue)
        {
            if (StaticValue.m_IsCancelQueue) return;
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LoginQueuePage, UIMessageDef.UI_OPEN);
        }
        else
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LoginQueuePage, UIMessageDef.UI_OPEN,code);
        }
    }

    public override void f_Close()
    {
        base.f_Close();

    }


    #region 创建帐号

    public void f_CreateAccount(string strName, string strPwd, ccCallback handler, UnityEngine.Object pParent = null)
    {
        Socket_Regedit tSocket_Regedit = (Socket_Regedit)_SocketMachineManger.f_GetStaticBase((int)EM_Socket.Regedit);
        tSocket_Regedit.f_CreateAccount(strName, strPwd, handler);
        _SocketMachineManger.f_ChangeState(tSocket_Regedit);
    }

    #endregion


    #region 登陆相关

    public void f_Login(ccCallback func = null)
    {
        Socket_Login tSocket_Login = (Socket_Login)_SocketMachineManger.f_GetStaticBase((int)EM_Socket.Login);
        tSocket_Login.f_Login(func);
        _SocketMachineManger.f_ChangeState(tSocket_Login, func);
    }

    #endregion

    #region 内部消息处理

    public override void Destroy()
    {
        base.Destroy();
        GameSocket._Instance = null;
    }

    public override void f_Update()
    {
        base.f_Update();

    }

    #endregion

    #region 外部消息处理

    protected override void InitMessage()
    {
        base.InitMessage();

        //m_GMSocketMessagePool.f_AddListener(SocketCommand.GM_SCK_UNBUILD.ToString(), On_GM_SCK_UNBUILD, null);
        stGameCommandReturn tGameCommandRet = new stGameCommandReturn();
        f_AddListener((int)SocketCommand.CONTROL_CTG_OperateResult, tGameCommandRet, On_CMsg_GameCommandReturn);

        CMsg_GTC_AccountLoginRelt tCMsg_GTC_AccountLoginRelt = new CMsg_GTC_AccountLoginRelt();
        f_AddListener((int)SocketCommand.SC_UserAttrInit, tCMsg_GTC_AccountLoginRelt, On_RoleData);

        SC_UserAttr tSC_UserAttr = new SC_UserAttr();
        f_AddListener((int)SocketCommand.SC_UserAttr, tSC_UserAttr, On_Data_CTG_ChangeRoleData, 1);


        basicNode1 tbasicNode1 = new basicNode1();
        f_AddListener((int)SocketCommand.SC_RoleEnterGame, tbasicNode1, On_StartGame);

        ChatOffline ChatOffline = new ChatOffline();
        f_AddListener((int)SocketCommand.SC_ChatOffLine, ChatOffline, On_ChatOffline);
        

        //////////////////////////////////////////////////////////////////////////
        //DATA

        basicNode1 tPing = new basicNode1();
        f_AddListener((int)SocketCommand.PING_Reps, tPing, On_Ping);

        SC_RelationServer tSC_RelationServer = new SC_RelationServer();
        f_AddListener((int)SocketCommand.SC_RelationServer, tSC_RelationServer, On_ChatLogin);

        basicNode1 tSC_KickoutReason = new basicNode1();
        f_AddListener((int)SocketCommand.SC_Kickout, tSC_KickoutReason, f_KickoutByServer);
    }

    #endregion

    void On_ChatOffline(object value)
    {
        Debug.LogError("===============chat   offline======================");
        ChatSocket.GetInstance().f_Close();
        //ChatSocket.GetInstance().f_SetSocketStatic(EM_SocketStatic.OffLine);
        //ChatSocket.GetInstance()._Socket_Loop.f_SetComplete((int)EM_Socket.Login, -99);
    }

    #region 被踢下线相关

    private void f_KickoutByServer(object value)
    {
        basicNode1 node1 = (basicNode1)value;
        eMsgOperateResult result = (eMsgOperateResult)node1.value1;
        if (result == eMsgOperateResult.OR_Error_ElseWhereLogin)
        {
TopPopupMenuParams param = new TopPopupMenuParams("Thông báo", "Tài khoản này đang được đăng nhập ở nơi khác", "Thoát", f_QuitGameSureHandle);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.TopPopupMenuPage, UIMessageDef.UI_OPEN, param);
        }
        else if (result == eMsgOperateResult.OR_Error_SeverMaintain)
        {
TopPopupMenuParams param = new TopPopupMenuParams("Thông báo", "Máy chủ đang được bảo trì", "Thoát", f_QuitGameSureHandle,10);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.TopPopupMenuPage, UIMessageDef.UI_OPEN, param);
        }
        else if (result == eMsgOperateResult.eOR_Account_Forbidden)
        {
TopPopupMenuParams param = new TopPopupMenuParams("Thông báo", "Tài khoản của bạn đã bị khóa, hãy liên hệ với bộ phận hỗ trợ để biết thêm chi tiết", "Thoát", f_QuitGameSureHandle,5);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.TopPopupMenuPage, UIMessageDef.UI_OPEN, param);
        }
        else
        {
TopPopupMenuParams param = new TopPopupMenuParams("Thông báo", "Mất kết nối với máy chủ", "Thoát", f_QuitGameSureHandle);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.TopPopupMenuPage, UIMessageDef.UI_OPEN, param);
        }
    }

    private void f_QuitGameSureHandle(object value)
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        //退出游戏
    }

#endregion

    protected void On_Ping(object Obj)
    {
        base.On_Ping(Obj);
//        if ((DateTime.Now - ChatSocket.GetInstance()._LastUpdateTime).TotalSeconds > GloData.glo_iPingTime)
//        {
//            
//        }
    }

#region 玩家相关

    private void On_RoleData(object Obj)
    {
        if (Obj == null)
        {
            return;
        }
        CMsg_GTC_AccountLoginRelt tCMsg_GTC_AccountLoginRelt = (CMsg_GTC_AccountLoginRelt)Obj;
        UserDataTools.f_ServerData2GameData(tCMsg_GTC_AccountLoginRelt);
    }


    /// <summary>
    /// 修改玩家信息操作
    /// </summary>
    /// <param name="Obj"></param>
    private void On_Data_CTG_ChangeRoleData(object Obj)
    {
        if (Obj == null)
        {
            return;
        }
        SC_UserAttr tSC_UserAttr = (SC_UserAttr)Obj;
        UserDataTools.f_ChangePlayerData((EM_UserAttr)tSC_UserAttr.attrEnum, tSC_UserAttr.iValue);
    }


#endregion


#region
    /// <summary>
    /// 接受服务器进入游戏指令
    /// </summary>
    /// <param name="Obj"></param>
    private void On_StartGame(object Obj)
    {
        _Day2Day.f_Start();
        glo_Main.GetInstance().f_InitGame();

        //LogTools.f_DeleteLog(Data_Pool.m_UserData.m_iUserId);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_STARTGAME);

        if (!Data_Pool.m_UserData.m_isHaveRole)
            glo_Main.GetInstance().m_SDKCmponent.f_SetRoleInfo(EM_SetRoleInfoType.CreateRole, Data_Pool.m_UserData.m_iServerId, Data_Pool.m_UserData.m_strServerName, Data_Pool.m_UserData.m_szRoleName, Data_Pool.m_UserData.m_iUserId,
                Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee), UITool.f_GetNowVipLv(), Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level), LegionMain.GetInstance().m_LegionInfor.f_getUserLegion().LegionName,
                Data_Pool.m_UserData.m_CreateTime.ToString(), Data_Pool.m_UserData.m_szSexDesc, Data_Pool.m_TeamPool.f_GetTotalBattlePower(), LegionMain.GetInstance().m_LegionInfor.m_iLegionId, Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_MainCard), Data_Pool.m_CardPool.f_GetRoleJob(), LegionMain.GetInstance().m_LegionPlayerPool.m_iSelfPos, LegionTool.f_GetPosDesc(LegionMain.GetInstance().m_LegionPlayerPool.m_iSelfPos),
                "Không");
        glo_Main.GetInstance().m_SDKCmponent.f_SetRoleInfo(EM_SetRoleInfoType.EnterGame, Data_Pool.m_UserData.m_iServerId, Data_Pool.m_UserData.m_strServerName, Data_Pool.m_UserData.m_szRoleName, Data_Pool.m_UserData.m_iUserId,
                Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee), UITool.f_GetNowVipLv(), Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level), LegionMain.GetInstance().m_LegionInfor.f_getUserLegion().LegionName,
                Data_Pool.m_UserData.m_CreateTime.ToString(), Data_Pool.m_UserData.m_szSexDesc, Data_Pool.m_TeamPool.f_GetTotalBattlePower(), LegionMain.GetInstance().m_LegionInfor.m_iLegionId, Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_MainCard), Data_Pool.m_CardPool.f_GetRoleJob(), LegionMain.GetInstance().m_LegionPlayerPool.m_iSelfPos, LegionTool.f_GetPosDesc(LegionMain.GetInstance().m_LegionPlayerPool.m_iSelfPos),
                "Không");

#if REYUN
            if (!string.IsNullOrEmpty(SDKHelper.REYUN_KEY))
        {
            //热云注册
            if (!Data_Pool.m_UserData.m_isHaveRole)
            {
                Tracking.Instance.register(Data_Pool.m_UserData.m_iUserId.ToString());
                MessageBox.DEBUG(string.Format("ReyunSDK Register,accountId:{0},date:{1}", Data_Pool.m_UserData.m_iUserId, DateTime.Now.ToString("HH-mm-ss")));
            }
            //热云登录
            Tracking.Instance.login(Data_Pool.m_UserData.m_iUserId.ToString());
            MessageBox.DEBUG(string.Format("ReyunSDK Login,accountId:{0},date:{1}", Data_Pool.m_UserData.m_iUserId, DateTime.Now.ToString("HH-mm-ss")));
        }

#endif
    }

#endregion

#region 时间

    public override void f_Ping()
    {
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        int iNum = f_SendBuf2Force((int)SocketCommand.PING, bBuf);

        MessageBox.DEBUG("Game Ping");
    }

    /// <summary>
    /// 跨天处理的时间
    /// </summary>
    public DateTime mNextDayTime
    {
        get
        {
            return _Day2Day.mNextDayTime;
        }
    }

#endregion


#region 聊天相关
    protected void On_ChatLogin(object Obj)
    {
        if (Obj == null)
        {
            return;
        }
        SC_RelationServer tSC_RelationServer = (SC_RelationServer)Obj;
        ChatSocket.GetInstance().f_Login(tSC_RelationServer);
    }

#endregion

}//END Class
