using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 保存玩家所在军团的基础信息
/// </summary>
public class LegionInfor
{

    #region 军团初始化信息

    private long _iLegionId;
    private int _iIOTime;
    private int _SacrificeType = 0;    //祭天类型
    private int _SacrificeAward;    //奖励领取进度
    public int m_iSacrificeType
    {
        get
        {
            return _SacrificeType;
        }
    }

    public int mSacrificeAward
    {
        get
        {
            return _SacrificeAward;
        }
    }

    /// <summary>
    /// 军团Id
    /// </summary>
    public long m_iLegionId
    {
        get
        {
            return _iLegionId;
        }
    }

    private int _iLegionPos = 0;
    /// <summary>
    /// 自己的军团职位   EM_LegionPostionEnum
    /// </summary>
    public int m_iLegionPos
    {
        get
        {
            return _iLegionPos;
        }
    }

    /// <summary>
    /// 加入退出时间时间戳
    /// </summary>
    public int m_iIOTime
    {
        get
        {
            return _iIOTime;
        }
    }


    #endregion

    /// <summary>
    /// 自己相关军团信息
    /// </summary>
    private LegionInfoPoolDT _slefLegionInfo = new LegionInfoPoolDT();

    /// <summary>
    /// 军团信息列表
    /// </summary>
    private List<BasePoolDT<long>> _legionList = new List<BasePoolDT<long>>();


    private LegionLevelDT _levelTemplate;

    public LegionLevelDT mLevelTemplate
    {
        get
        {
            return _levelTemplate;
        }
    }

    public LegionInfor()
    {
        f_Init();
        RegSocketMessage();
    }

    private void f_Init()
    {
        _iLegionId = 0;
        _iIOTime = 0;
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.LEGION_DISSOLVE_OR_QUIT, f_ProcessLegionDissolveOrQuit);
    }

    void RegSocketMessage()
    {
        CMsg_GTC_UserLegionInit tstUserLegionInit = new CMsg_GTC_UserLegionInit();
        GameSocket.GetInstance().f_RegMessage((int)LegionSocketCmd.SC_UserLegionInit, tstUserLegionInit, f_GTC_UserLegionInitHandle);

        CMsg_RTC_LegionInfo tstRCLegionInfo = new CMsg_RTC_LegionInfo();
        ChatSocket.GetInstance().f_AddListener_EndString((int)LegionSocketCmd.RC_LegionInfo, tstRCLegionInfo, f_RTC_LegionInfoHandle);

        CMsg_LongNode tstLegionApplyList = new CMsg_LongNode();
        GameSocket.GetInstance().f_RegMessage_Int3((int)LegionSocketCmd.SC_LegionApplyList, tstLegionApplyList, f_GTC_LegionApplyListHandle);

        SC_LegionInit tLegionSacrifice = new SC_LegionInit();
        GameSocket.GetInstance().f_RegMessage((int)LegionSocketCmd.SC_LegionInit, tLegionSacrifice, SaveLegionSacrifice);

        CMsg_RTC_LegionInfo tstSearchInfo = new CMsg_RTC_LegionInfo();
        ChatSocket.GetInstance().f_AddListener_EndString((int)LegionSocketCmd.RC_LegionSearchByName, tstSearchInfo, f_RTC_SearchLegionHandle);
    }

    /// <summary>
    /// 军团解散或者退出处理
    /// </summary>
    /// <param name="value"></param>
    private void f_ProcessLegionDissolveOrQuit(object value)
    {
        //初始化重置
        _applyListInit = false;
        _iLegionInfoListTime = 0;

        if (LegionMain.GetInstance().m_LegionInfor.m_iLegionId == 0)
            LegionMain.GetInstance().m_LegionInfor.f_ExecuteAfterApplyList(f_ExecuteAfterApplyList);
    }

    private void f_ExecuteAfterApplyList(object result)
    {
        if ((int)result != (int)eMsgOperateResult.OR_Succeed)
MessageBox.ASSERT("Initialization of application list failed,code:" + result);
        else
MessageBox.DEBUG("Initialization of application list successfully");
    }

    #region 协议处理函数

    private void f_GTC_UserLegionInitHandle(object value)
    {
        CMsg_GTC_UserLegionInit tServerInfo = (CMsg_GTC_UserLegionInit)value;
        if (_iLegionId != 0 && tServerInfo.legionId == 0)
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.LEGION_DISSOLVE_OR_QUIT);
        _iLegionId = tServerInfo.legionId;
        _iIOTime = tServerInfo.ioTime;
        //更新自己的职位信息 start
        _iLegionPos = tServerInfo.pos;
        LegionMain.GetInstance().m_LegionPlayerPool.f_UpdateSelfPost(tServerInfo.pos);
        //end
        _SacrificeType = tServerInfo.SacrificeType;
        _SacrificeAward = tServerInfo.SacrificeAward;
        LegionMain.GetInstance().m_LegionDungeonPool.f_UpdateChapterAward(tServerInfo.uDungeonAwardChaper);
        //军团信息初始化 走LvUpRole类型 通知SDK
        //TsuCommnt //glo_Main.GetInstance().m_SDKCmponent.f_SetRoleInfo(EM_SetRoleInfoType.LvUpRole, Data_Pool.m_UserData.m_iServerId, Data_Pool.m_UserData.m_strServerName, Data_Pool.m_UserData.m_szRoleName, Data_Pool.m_UserData.m_iUserId,
        //Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee), UITool.f_GetVipLv(Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Vip)),Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level), LegionMain.GetInstance().m_LegionInfor.f_getUserLegion().LegionName,
        //Data_Pool.m_UserData.m_CreateTime.ToString(), Data_Pool.m_UserData.m_szSexDesc, Data_Pool.m_TeamPool.f_GetTotalBattlePower(), LegionMain.GetInstance().m_LegionInfor.m_iLegionId, Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_MainCard), Data_Pool.m_CardPool.f_GetRoleJob(), LegionMain.GetInstance().m_LegionPlayerPool.m_iSelfPos, LegionTool.f_GetPosDesc(LegionMain.GetInstance().m_LegionPlayerPool.m_iSelfPos),
        //"Không");
    }

    private void f_RTC_LegionInfoHandle(object value, string ppSQL)
    {
        CMsg_RTC_LegionInfo tServerData = (CMsg_RTC_LegionInfo)value;
        string[] aTextData = ccMath.f_String2ArrayString(ppSQL, "#");
        for (int i = 0; i < aTextData.Length; i++)
        {
            aTextData[i] = aTextData[i].Replace("\0", "");
        }
        //自己
        if (tServerData.id == _iLegionId)
        {
            int curLv = _slefLegionInfo.f_GetProperty((int)EM_LegionProperty.Lv);
            int serverLv = tServerData.lv;
            if (curLv != serverLv)
            {
                _levelTemplate = (LegionLevelDT)glo_Main.GetInstance().m_SC_Pool.m_LegionLevelSC.f_GetSC(serverLv);
                if (_levelTemplate == null)
MessageBox.ASSERT("Legion Lv data does not exist，Lv " + serverLv);
            }
            _slefLegionInfo.iId = tServerData.id;
            _slefLegionInfo.f_UpdateProperty((int)EM_LegionProperty.MasterUserId, tServerData.chiefId);
            //int
            _slefLegionInfo.f_UpdateProperty((int)EM_LegionProperty.FoundTime, tServerData.foundTime);
            _slefLegionInfo.f_UpdateProperty((int)EM_LegionProperty.Icon, tServerData.iconId);
            _slefLegionInfo.f_UpdateProperty((int)EM_LegionProperty.Frame, tServerData.frameId);
            _slefLegionInfo.f_UpdateProperty((int)EM_LegionProperty.Lv, tServerData.lv);
            _slefLegionInfo.f_UpdateProperty((int)EM_LegionProperty.Exp, tServerData.exp);
            _slefLegionInfo.f_UpdateProperty((int)EM_LegionProperty.MemberNum, tServerData.memNum);
            //字符串
            _slefLegionInfo.f_UpdateProperty((int)EM_LegionProperty.Name, aTextData[0]);
            _slefLegionInfo.f_UpdateProperty((int)EM_LegionProperty.Manifesto, aTextData[1]);
            _slefLegionInfo.f_UpdateProperty((int)EM_LegionProperty.Notice, aTextData[2]);
            //通知军团信息更新
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_SELF_LEGION_INFO_UPDATE);
        }
        //军团信息列表
        else
        {
            LegionInfoPoolDT poolDt = new LegionInfoPoolDT();
            //long
            poolDt.iId = tServerData.id;
            poolDt.f_UpdateProperty((int)EM_LegionProperty.MasterUserId, tServerData.chiefId);
            //int
            poolDt.f_UpdateProperty((int)EM_LegionProperty.FoundTime, tServerData.foundTime);
            poolDt.f_UpdateProperty((int)EM_LegionProperty.Icon, tServerData.iconId);
            poolDt.f_UpdateProperty((int)EM_LegionProperty.Frame, tServerData.frameId);
            poolDt.f_UpdateProperty((int)EM_LegionProperty.Lv, tServerData.lv);
            poolDt.f_UpdateProperty((int)EM_LegionProperty.Exp, tServerData.exp);
            poolDt.f_UpdateProperty((int)EM_LegionProperty.MemberNum, tServerData.memNum);
            //字符串
            poolDt.f_UpdateProperty((int)EM_LegionProperty.Name, aTextData[0]);
            poolDt.f_UpdateProperty((int)EM_LegionProperty.Manifesto, aTextData[1]);
            poolDt.f_UpdateProperty((int)EM_LegionProperty.Notice, aTextData[2]);
            _legionList.Add(poolDt);
        }
    }
    
    private void f_RTC_SearchLegionHandle(object value, string ppSQL)
    {
        CMsg_RTC_LegionInfo tServerData = (CMsg_RTC_LegionInfo)value;
        string[] aTextData = ccMath.f_String2ArrayString(ppSQL, "#");
        for (int i = 0; i < aTextData.Length; i++)
        {
            aTextData[i] = aTextData[i].Replace("\0", "");
        }
        if (_searchLegionInfo == null)
        {
            LegionInfoPoolDT poolDt = new LegionInfoPoolDT();
            _searchLegionInfo = poolDt; 
        }
        LegionInfoPoolDT tInfo = (LegionInfoPoolDT)_searchLegionInfo;
        //long
        tInfo.iId = tServerData.id;
        tInfo.f_UpdateProperty((int)EM_LegionProperty.MasterUserId, tServerData.chiefId);
        //int
        tInfo.f_UpdateProperty((int)EM_LegionProperty.FoundTime, tServerData.foundTime);
        tInfo.f_UpdateProperty((int)EM_LegionProperty.Icon, tServerData.iconId);
        tInfo.f_UpdateProperty((int)EM_LegionProperty.Frame, tServerData.frameId);
        tInfo.f_UpdateProperty((int)EM_LegionProperty.Lv, tServerData.lv);
        tInfo.f_UpdateProperty((int)EM_LegionProperty.Exp, tServerData.exp);
        tInfo.f_UpdateProperty((int)EM_LegionProperty.MemberNum, tServerData.memNum);
        //字符串
        tInfo.f_UpdateProperty((int)EM_LegionProperty.Name, aTextData[0]);
        tInfo.f_UpdateProperty((int)EM_LegionProperty.Manifesto, aTextData[1]);
        tInfo.f_UpdateProperty((int)EM_LegionProperty.Notice, aTextData[2]);
    }

    private void f_GTC_LegionApplyListHandle(int value1, int value2, int value3, int num, ArrayList aData)
    {
        foreach (SockBaseDT tData in aData)
        {
            CMsg_LongNode tServerData = (CMsg_LongNode)tData;
            if (value3 == (int)eUpdateNodeType.node_add)
            {
                f_AddSelfLegionApplyItem(tServerData.value1);
            }
            else if (value3 == (int)eUpdateNodeType.node_default)
            {
                f_AddSelfLegionApplyItem(tServerData.value1);
            }
            else if (value3 == (int)eUpdateNodeType.node_update)
            {
MessageBox.ASSERT("No single list update (no corps)");
            }
            else if (value3 == (int)eUpdateNodeType.node_delete)
            {
                f_DeleteSelfLegionApplyItem(tServerData.value1);
            }
            else
            {
MessageBox.ASSERT("There is incorrect information in the node of the single list");
            }
        }
    }



    #endregion



    #region 发送协议

    /// <summary>
    /// 玩家军团初始化
    /// </summary>
    /// <param name="socketCallbackDt"></param>
    public void f_UserLegionInit(SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_UserLegionInit, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_UserLegionInit, bBuf);
    }

    /// <summary>
    /// 自己的申请军团列表 在申请军团信息前申请
    /// </summary>
    /// <param name="socketCallbackDt"></param>
    public void f_LegionApplyList(SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionApplyList, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionApplyList, bBuf);
    }

    /// <summary>
    /// 军团信息
    /// </summary>
    /// <param name="page">0:自己的军团 >0 按页请求军团</param>
    /// <param name="socketCallbackDt"></param>
    public void f_LegionInfo(int page, SocketCallbackDT socketCallbackDt)
    {
        ChatSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CR_LegionInfo, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(page);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        ChatSocket.GetInstance().f_SendBuf(LegionSocketCmd.CR_LegionInfo, bBuf);
    }

    /// <summary>
    /// 创建军团
    /// </summary>
    /// <param name="socketCallbackDt"></param>
    public void f_LegionFound(byte iconId, byte frameId, string szName, SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionFound, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(iconId);
        tCreateSocketBuf.f_Add(frameId);
        tCreateSocketBuf.f_Add(szName, LegionConst.LEGION_NAME_BYTE_NUM);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionFound, bBuf);
    }

    /// <summary>
    /// 申请军团
    /// </summary>
    /// <param name="legionId"></param>
    /// <param name="socketCallbackDt"></param>
    public void f_LegionApply(long legionId, SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionApply, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(legionId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionApply, bBuf);
    }

    /// <summary>
    /// 取消申请军团
    /// </summary>
    /// <param name="legionId"></param>
    /// <param name="socketCallbackDt"></param>
    public void f_LegionDisapply(long legionId, SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionDisapply, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(legionId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionDisapply, bBuf);
    }

    /// <summary>
    /// 根据名字查询军团
    /// </summary>
    /// <param name="legionName"></param>
    /// <param name="socketCallbackDt"></param>
    public void f_LegionSearch(string legionName, SocketCallbackDT socketCallbackDt)
    {
        ChatSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CR_LegionSearchByName, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(legionName,LegionConst.LEGION_NAME_BYTE_NUM);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        ChatSocket.GetInstance().f_SendBuf(LegionSocketCmd.CR_LegionSearchByName, bBuf);
    }
    /// <summary>
    /// 通过军团id获取军团（未找到则返回null）
    /// </summary>
    /// <param name="id">军团id</param>
    /// <returns></returns>
    public LegionInfoPoolDT f_LegionSerch(long id)
    {
        for (int i = 0; i < _legionList.Count; i++)
        {
            LegionInfoPoolDT poolDt = _legionList[i] as LegionInfoPoolDT;
            if (poolDt.iId == id)
            {
                return poolDt;
            }
        }
        return null;
    }
    /// <summary>
    /// 修改公告
    /// </summary>
    /// <param name="legionNotice"></param>
    /// <param name="socketCallbackDt"></param>
    public void f_LegionNotice(string legionNotice, SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_Notice, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        //预留多2个字节给服务器
        tCreateSocketBuf.f_Add(legionNotice,LegionConst.LEGION_NOTICE_BYTE_LIMIT + 2);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_Notice, bBuf);
        _slefLegionInfo.f_UpdateProperty((int)EM_LegionProperty.Notice, legionNotice);
    }

    /// <summary>
    /// 修改宣言
    /// </summary>
    /// <param name="legionManifesto"></param>
    /// <param name="socketCallbackDt"></param>
    public void f_LegionManifesto(string legionManifesto, SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_Manifesto, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(legionManifesto, LegionConst.LEGION_MANIFESTO_BYTE_LIMIT + 2);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_Manifesto, bBuf);
        _slefLegionInfo.f_UpdateProperty((int)EM_LegionProperty.Manifesto, legionManifesto);
    }

    #endregion


    #region 对外接口

    /// <summary>
    /// 获取玩家军团信息
    /// </summary>
    /// <returns></returns>
    public LegionInfoPoolDT f_getUserLegion()
    {
        return _slefLegionInfo;
    }
    public void f_LegionUpLv(SocketCallbackDT socket)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionUpLevel, socket.m_ccCallbackSuc, socket.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(1);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionUpLevel, bBuf);
    }
    /// <summary>
    /// 获取军团列表
    /// </summary>
    /// <returns></returns>
    public List<BasePoolDT<long>> f_GetLegionList()
    {
        return _legionList;
    }

    /// <summary>
    /// 玩家军团执行在初始化后
    /// </summary>
    /// <param name="callbackHandle"></param>
    public void f_ExecuteAfterLegionInit(ccCallback callbackHandle)
    {
        if (_bUserLegionInit)
        {
            if (callbackHandle != null)
                callbackHandle(eMsgOperateResult.OR_Succeed);
        }
        else
        {
            _callback_UserLegionInit = callbackHandle;
            SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
            socketCallbackDt.m_ccCallbackSuc = f_Callback_UserLegionInit;
            socketCallbackDt.m_ccCallbackFail = f_Callback_UserLegionInit;
            f_UserLegionInit(socketCallbackDt);
        }
    }

    /// <summary>
    /// 在玩家军团申请列表后执行
    /// </summary>
    /// <param name="callbackHandle"></param>
    public void f_ExecuteAfterApplyList(ccCallback callbackHandle)
    {
        if (_applyListInit)
        {
            if (callbackHandle != null)
                callbackHandle(eMsgOperateResult.OR_Succeed);
        }
        else
        {
            _slefLegionApplyList.Clear();
            _callback_ApplyListInit = callbackHandle;
            SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
            socketCallbackDt.m_ccCallbackSuc = f_Callback_ApplyListInit;
            socketCallbackDt.m_ccCallbackFail = f_Callback_ApplyListInit;
            f_LegionApplyList(socketCallbackDt);
        }
    }

    /// <summary>
    /// 检查是否在申请中
    /// </summary>
    /// <param name="legionId"></param>
    /// <returns></returns>
    public bool f_CheckIsApplying(long legionId)
    {
        return _slefLegionApplyList.Contains(legionId);
    }

    /// <summary>
    /// 请求军团信息后执行
    /// </summary>
    /// <param name="self">true:申请自己的军团信息，false:还是军团列表</param>
    /// <param name="first">只对军团列表生效 true:请求第一页 false:滑动到底部再申请</param>
    /// <param name="callbackHandle"></param>
    /// <param name="force">强行刷新 true:重新请求 false:会根据时间来请求</param>
    public void f_ExecuteAfterLegionInfo(bool self, bool first, ccCallback callbackHandle, bool force = false)
    {
        _callback_LegionInfo = callbackHandle;
        if (self)
        {
            //暂时每次重新请求自己的军团信息
            //if (_bSelfLegionInfoInit)
            //{
            //    if (callbackHandle != null)
            //        callbackHandle(eMsgOperateResult.OR_Succeed);
            //    return;
            //}
            SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
            socketCallbackDt.m_ccCallbackSuc = f_Callback_LegionInfo;
            socketCallbackDt.m_ccCallbackFail = f_Callback_LegionInfo;
            f_LegionInfo(0, socketCallbackDt);
            return;
        }
        if (first)
        {
            int tNow = GameSocket.GetInstance().f_GetServerTime();
            if (tNow - _iLegionInfoListTime > LegionConst.LEGION_LIST_TIME_DIS)
            {
                _iLegionInfoListTime = tNow;
                _iCurLegionInfoIdx = 1;
                _legionList.Clear();
                SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
                socketCallbackDt.m_ccCallbackSuc = f_Callback_LegionInfo;
                socketCallbackDt.m_ccCallbackFail = f_Callback_LegionInfo;
                f_LegionInfo(_iCurLegionInfoIdx, socketCallbackDt);
            }
            else
            {
                if (callbackHandle != null)
                {
                    f_SortLegionList();
                    callbackHandle(eMsgOperateResult.OR_Succeed);
                }

            }
        }
        else
        {
            if (_legionList.Count == (_iCurLegionInfoIdx - 1) * LegionConst.LEGION_LIST_NUM_PRE_PAGE)
            {
                SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
                socketCallbackDt.m_ccCallbackSuc = f_Callback_LegionInfo;
                socketCallbackDt.m_ccCallbackFail = f_Callback_LegionInfo;
                f_LegionInfo(_iCurLegionInfoIdx, socketCallbackDt);
            }
            else
            {
                if (callbackHandle != null)
                    callbackHandle(eMsgOperateResult.OR_Succeed);
            }
        }
    }

    #endregion

    //工会申请列表排序
    private void f_SortLegionList()
    {
        _legionList.Sort(delegate (BasePoolDT<long> value1, BasePoolDT<long> value2)
        {
            LegionInfoPoolDT item1 = (LegionInfoPoolDT)value1;
            LegionInfoPoolDT item2 = (LegionInfoPoolDT)value2;
            bool inAppling1 = f_CheckIsApplying(item1.iId);
            bool inAppling2 = f_CheckIsApplying(item2.iId);
            int lv1 = item1.f_GetProperty((int)EM_LegionProperty.Lv);
            int lv2 = item2.f_GetProperty((int)EM_LegionProperty.Lv);
            int foundTime1 = item1.f_GetProperty((int)EM_LegionProperty.FoundTime);
            int foundTime2 = item2.f_GetProperty((int)EM_LegionProperty.FoundTime);
            if (inAppling1 && !inAppling2)
                return -1;
            else if (!inAppling1 && inAppling2)
                return 1;

            if (lv1 > lv2)
                return -1;
            else if (lv1 < lv2)
                return 1;

            if (foundTime1 > foundTime2)
                return 1;
            else if (foundTime1 < foundTime2)
                return -1;

            return item1.iId.CompareTo(item2.iId);
        }
        );
    }


    //UserLegionInit
    private bool _bUserLegionInit = false;
    private ccCallback _callback_UserLegionInit;

    private void f_Callback_UserLegionInit(object value)
    {
        if (_callback_UserLegionInit != null)
        {
            if ((int)value == (int)eMsgOperateResult.OR_Succeed)
                _bUserLegionInit = true;
            _callback_UserLegionInit(value);
        }
    }

    //军团列表相关
    private ccCallback _callback_LegionInfo;

    // 自己的军团数据是否已经初始化   每次重新请求自己军团信息暂时不需要
    //private bool _bSelfLegionInfoInit = false;
    //请求军团列表时间戳
    private int _iLegionInfoListTime = 0;
    //当前军团信息Idx
    private int _iCurLegionInfoIdx = 1;

    private void f_Callback_LegionInfo(object value)
    {
        _iCurLegionInfoIdx++;
        if (_callback_LegionInfo != null)
        {
            if ((int)value == (int)eMsgOperateResult.OR_Succeed)
            {
                //代表已经申请过自己的工会信息  每次重新请求自己工会信息
                //if(_slefLegionInfo.iId != 0)
                //_bSelfLegionInfoInit = true;
            }

            _callback_LegionInfo(value);
        }
    }


    #region 自己申请军团列表

    private bool _applyListInit = false;
    private ccCallback _callback_ApplyListInit;
    private List<long> _slefLegionApplyList = new List<long>();

    private void f_Callback_ApplyListInit(object result)
    {
        if (_callback_ApplyListInit != null)
        {
            if ((int)result == (int)eMsgOperateResult.OR_Succeed)
                _applyListInit = true;
            _callback_ApplyListInit(result);
        }
    }

    private void f_AddSelfLegionApplyItem(long legionId)
    {
        _slefLegionApplyList.Add(legionId);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_LEGION_SELf_LEGION_APPLY_LIST_UPDATE);
    }

    private void f_DeleteSelfLegionApplyItem(long legionId)
    {
        _slefLegionApplyList.Remove(legionId);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_LEGION_SELf_LEGION_APPLY_LIST_UPDATE);
    }

    /// <summary>
    /// 自己已申请的军团列表数量
    /// </summary>
    public int m_iSelfApplyLegionNum
    {
        get
        {
            return _slefLegionApplyList.Count;
        }
    }

    #endregion

    #region 查找军团

    private BasePoolDT<long> _searchLegionInfo;
    public BasePoolDT<long> m_SearchLegionInfo
    {
        get
        {
            return _searchLegionInfo;
        }
    }



    #endregion

    #region 军团祭天相关

    private SC_LegionInit _LegionSacrifice = new SC_LegionInit();

    public SC_LegionInit m_LegionSacrifice
    {
        get
        {
            return _LegionSacrifice;
        }
    }

    ccCallback SacrificeShowUI;
    private void SaveLegionSacrifice(object obj)
    {
        SC_LegionInit tserverData = (SC_LegionInit)obj;
        int curLv = _slefLegionInfo.f_GetProperty((int)EM_LegionProperty.Lv);
        int serverLv = tserverData.level;
        if (curLv != serverLv)
        {
            _levelTemplate = (LegionLevelDT)glo_Main.GetInstance().m_SC_Pool.m_LegionLevelSC.f_GetSC(serverLv);
            if (_levelTemplate == null)
MessageBox.ASSERT("Level corps does not exist，Lv " + serverLv);
        }
        _slefLegionInfo.f_UpdateProperty((int)EM_LegionProperty.Exp, tserverData.exp);
        _slefLegionInfo.f_UpdateProperty((int)EM_LegionProperty.Lv, tserverData.level);
        //更新军团经验 通知更新
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_SELF_LEGION_INFO_UPDATE);
        _LegionSacrifice.uSacrifice = tserverData.uSacrifice;
        if (SacrificeShowUI != null)
        {
            SacrificeShowUI(tserverData);
            SacrificeShowUI = null;
        } 
    }
    /// <summary>
    /// 祭天
    /// </summary>
    public void f_SendSacrifice(byte id, SocketCallbackDT tSocketCallBack)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionSacrifice, tSocketCallBack.m_ccCallbackSuc, tSocketCallBack.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(id);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionSacrifice, bBuf);
    }

    public void f_SendSacrificeInfo(ccCallback back)
    {
        SacrificeShowUI = back;
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(1);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionInit, bBuf);
    }

    public void f_SendGetBox(byte id, SocketCallbackDT tSocketCallBack)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionSacrificeAward, tSocketCallBack.m_ccCallbackSuc, tSocketCallBack.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(id);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionSacrificeAward, bBuf);
    }
    #endregion
}
