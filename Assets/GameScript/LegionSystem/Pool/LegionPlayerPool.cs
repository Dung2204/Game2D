using ccU3DEngine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 军团成员Pool
/// </summary>
public class LegionPlayerPool : BasePool
{
    private List<BasePoolDT<long>> _applicantList = new List<BasePoolDT<long>>();

    /// <summary>
    /// 成员数目（实时的，成员列表的数目）
    /// </summary>
    public int m_iMemberNum
    {
        get
        {
            return f_GetAll().Count;
        }
    }

    public int _iDeputyNum;
    /// <summary>
    /// 军团副团长数目
    /// </summary>
    public int m_iDeputyNum
    {
        get
        {
            return _iDeputyNum;
        }
    }

    private int _iSelfPos;

    public void f_UpdateSelfPost(int selfPos)
    {
        //如果玩家自己职位变了，那么就重置一下申请列表初始化状态
        if (_iSelfPos != selfPos)
        {
            if (!LegionTool.f_IsEnoughPermission(EM_LegionOperateType.ApplicantList, false)
                && LegionTool.f_IsEnoughPermission(EM_LegionOperateType.ApplicantList, false, selfPos))
            {
                _iSelfPos = selfPos;
                _bApplicantListInit = false;
                f_ExecuteAfterApplicantList(null);
                return;
            }
            _iSelfPos = selfPos; 
            if (!LegionTool.f_IsEnoughPermission(EM_LegionOperateType.ApplicantList, false))
            {
                Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.LegionApplicantList);
            }
        }
    }

    /// <summary>
    /// 玩家自己的职位
    /// </summary>
    public int m_iSelfPos
    {
        get
        {
            return _iSelfPos;
        }
    }

    public LegionPlayerPool() : base("LegionPlayerPoolDT", true)
    {
        _iSelfPos = 0;
    }

    /// <summary>
    /// 军团 今日贡献大于0的成员数量
    /// </summary>
    public int m_iTodayContriMemberNum
    {
        get
        {
            int tResult = 0;
            List<BasePoolDT<long>> tMemberList = f_GetAll();
            for (int i = 0; i < tMemberList.Count; i++)
            {
                LegionPlayerPoolDT tItem = (LegionPlayerPoolDT)tMemberList[i];
                if (tItem.PlayerInfo.m_iTodayContri > 0)
                    tResult++;
            }
            return tResult;
        } 
    }

    #region Pool数据管理 

    protected override void f_Init()
    {
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.LEGION_DISSOLVE_OR_QUIT, f_ProcessLegionDissolveOrQuit);
    }


    /// <summary>
    /// 是否是退出操作
    /// </summary>
    private bool _isQuitOperate = false;
    /// <summary>
    /// 军团解散或者退出处理
    /// </summary>
    /// <param name="value"></param>
    private void f_ProcessLegionDissolveOrQuit(object value)
    {
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_LEGION_WARN_SHOW, _isQuitOperate ? EM_LegionOutType.Quit : EM_LegionOutType.Dissolve);
        _isQuitOperate = false;
        _bMemInit = false;
        _bApplicantListInit = false;
    }

    protected override void RegSocketMessage()
    {
        CMsg_LongNode tstMemInitNode = new CMsg_LongNode();
        ChatSocket.GetInstance().f_RegMessage_Int3((int)LegionSocketCmd.RC_LegionMemInit, tstMemInitNode, f_RTC_LegionMemInitHandle);

        CMsg_RTC_UserLegion tstUserLegion = new CMsg_RTC_UserLegion();
        ChatSocket.GetInstance().f_RegMessage((int)LegionSocketCmd.RC_UserLegion, tstUserLegion,f_RTC_UserLegionHandle);

        CMsg_LongNode tstApplicantList = new CMsg_LongNode();
        ChatSocket.GetInstance().f_RegMessage_Int3((int)LegionSocketCmd.RC_LegionApplicantList, tstApplicantList, f_RTC_ApplicantListHandle);
        
    }


    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {

    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {

    }

    #endregion

    #region 协议处理函数

    private void f_RTC_LegionMemInitHandle(int value1, int value2, int value3, int num, ArrayList aData)
    {
        if (!_bMemInit)
        {
            //暂时报错处理，后面取消
MessageBox.ASSERT("Member data is not initialized without processing");
            return;
        }
        foreach (SockBaseDT tData in aData)
        {
            if (value3 == (int)eUpdateNodeType.node_add)
            {
                f_RTC_LegionMenAddData(tData, true);
            }
            else if (value3 == (int)eUpdateNodeType.node_default)
            {
                f_RTC_LegionMenAddData(tData, false);
            }
            else if (value3 == (int)eUpdateNodeType.node_update)
            {
                f_RTC_LegionMenUpdateData(tData);
            }
            else if (value3 == (int)eUpdateNodeType.node_delete)
            {
                f_RTC_LegionMenDeleteData(tData);
            }
            else
            {
MessageBox.ASSERT("Member data has wrong node type");
            }
        }
        f_SortMemberList();
    }

    private void f_RTC_LegionMenAddData(object value, bool isNew)
    {
        CMsg_LongNode tServerData = (CMsg_LongNode)value;
        LegionPlayerPoolDT poolDt = new LegionPlayerPoolDT();
        poolDt.iId = tServerData.value1;
        BasePlayerPoolDT playerInfo = (BasePlayerPoolDT)Data_Pool.m_GeneralPlayerPool.f_GetForId(tServerData.value1);
        if (playerInfo == null)
        {
MessageBox.ASSERT("Added member data does not exist");
        }
        poolDt.f_UpdatePlayerInfo(playerInfo);
        f_Save(poolDt);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_LEGION_MEMBER_DATA_UPDATE);
    }

    private void f_RTC_LegionMenUpdateData(object value)
    {
        CMsg_LongNode tServerData = (CMsg_LongNode)value;
        BasePlayerPoolDT playerInfo = (BasePlayerPoolDT)Data_Pool.m_GeneralPlayerPool.f_GetForId(tServerData.value1);
        if (playerInfo == null)
        {
MessageBox.ASSERT("Updated member data does not exist");
        }
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_LEGION_MEMBER_DATA_UPDATE);
    }

    private void f_RTC_LegionMenDeleteData(object value)
    {
        CMsg_LongNode tServerData = (CMsg_LongNode)value;
        BasePlayerPoolDT playerInfo = (BasePlayerPoolDT)Data_Pool.m_GeneralPlayerPool.f_GetForId(tServerData.value1);
        if (playerInfo == null)
        {
MessageBox.ASSERT("Deleted member data does not exist");
        }
        f_Delete(tServerData.value1);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_LEGION_MEMBER_DATA_UPDATE);
    }

    private void f_RTC_UserLegionHandle(object value)
    {
        CMsg_RTC_UserLegion tServerData = (CMsg_RTC_UserLegion)value;
        BasePlayerPoolDT info = (BasePlayerPoolDT)Data_Pool.m_GeneralPlayerPool.f_GetForId(tServerData.userId);
        if (tServerData.userId == Data_Pool.m_UserData.m_iUserId)
        {
            LegionTool.f_SelfPlayerInfoDispose(ref info); 
        }
        if (info != null)
        {
            if (info.m_iLegionPostion == (int)EM_LegionPostionEnum.eLegion_Deputy
                && tServerData.uPos != (int)EM_LegionPostionEnum.eLegion_Deputy)
                _iDeputyNum--;
            else if(info.m_iLegionPostion != (int)EM_LegionPostionEnum.eLegion_Deputy
                && tServerData.uPos == (int)EM_LegionPostionEnum.eLegion_Deputy)
                _iDeputyNum++;
            info.f_SaveLegion(tServerData.uPos, tServerData.todayContri, tServerData.totalContri);
        }
        else
        {
MessageBox.ASSERT("Player data does not exist during processing, Id:" + tServerData.userId);
        }
        f_SortMemberList();
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_LEGION_MEMBER_DATA_UPDATE);

    }

    private void f_RTC_ApplicantListHandle(int value1, int value2,int value3, int num, ArrayList aData)
    {
        foreach (SockBaseDT tData in aData)
        {
            if (value3 == (int)eUpdateNodeType.node_add)
            {
                f_GTC_ApplicantListAddData(tData, true);
            }
            else if (value3 == (int)eUpdateNodeType.node_default)
            {
                f_GTC_ApplicantListAddData(tData, false);
            }
            else if (value3 == (int)eUpdateNodeType.node_update)
            {
                f_GTC_ApplicantListUpdateData(tData);
            }
            else if (value3 == (int)eUpdateNodeType.node_delete)
            {
                f_GTC_ApplicantListDeleteData(tData);
            }
            else
            {
MessageBox.ASSERT("Member information has wrong node type");
            }
        }
    }

    private void f_GTC_ApplicantListAddData(object value, bool isNew)
    {
        CMsg_LongNode tServerData = (CMsg_LongNode)value;
        LegionPlayerPoolDT poolDt = new LegionPlayerPoolDT();
        poolDt.iId = tServerData.value1;
        BasePlayerPoolDT playerInfo = (BasePlayerPoolDT)Data_Pool.m_GeneralPlayerPool.f_GetForId(tServerData.value1);
        if (playerInfo == null)
        {
MessageBox.ASSERT("Added player data does not exist");
        }
        poolDt.f_UpdatePlayerInfo(playerInfo);
        _applicantList.Add(poolDt);
        Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.LegionApplicantList);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_LEGION_APPLICANT_DATA_UPDATE);
    }

    private void f_GTC_ApplicantListUpdateData(object value)
    {
        CMsg_LongNode tServerData = (CMsg_LongNode)value;
        BasePlayerPoolDT playerInfo = (BasePlayerPoolDT)Data_Pool.m_GeneralPlayerPool.f_GetForId(tServerData.value1);
        if (playerInfo == null)
        {
MessageBox.ASSERT("Updated player data does not exist");
        }
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_LEGION_APPLICANT_DATA_UPDATE);
    }

    private void f_GTC_ApplicantListDeleteData(object value)
    {
        CMsg_LongNode tServerData = (CMsg_LongNode)value;
        BasePlayerPoolDT playerInfo = (BasePlayerPoolDT)Data_Pool.m_GeneralPlayerPool.f_GetForId(tServerData.value1);
        if (playerInfo == null)
        {
MessageBox.ASSERT("Deleted player data does not exist");
        }
        int removeNum = _applicantList.RemoveAll(delegate (BasePoolDT<long> item)
        {
            return item.iId == tServerData.value1;
        });
        Data_Pool.m_ReddotMessagePool.f_MsgSubtract(EM_ReddotMsgType.LegionApplicantList);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_LEGION_APPLICANT_DATA_UPDATE);
    }

    #endregion

    #region 发送协议

    /// <summary>
    /// 军团成员列表
    /// </summary>
    /// <param name="socketCallbackDt"></param>
    public void f_LegionMemInit(SocketCallbackDT socketCallbackDt)
    {
        f_Clear();
        ChatSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CR_LegionMemInit, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        ChatSocket.GetInstance().f_SendBuf(LegionSocketCmd.CR_LegionMemInit, bBuf);
    }

    /// <summary>
    /// 初始化申请列表  (游戏服请求，关系服返回)
    /// </summary>
    /// <param name="socketCallbackDt"></param>
    public void f_LegionApplicantList(SocketCallbackDT socketCallbackDt)
    {
        ChatSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionApplicantList, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionApplicantList, bBuf);
    }

    /// <summary>
    /// 响应玩家申请
    /// </summary>
    /// <param name="userId">玩家Id</param>
    /// <param name="isAccept">0: 拒绝 非0：接受</param>
    /// <param name="socketCallbackDt"></param>
    public void f_LegionRespond(long userId, byte isAccept, SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionRespond, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(userId);
        tCreateSocketBuf.f_Add(isAccept);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionRespond, bBuf);
    }

    /// <summary>
    /// 解散工会
    /// </summary>
    public void f_LegionDissolve(SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionDissolve, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionDissolve, bBuf);
    }

    /// <summary>
    /// 踢人
    /// </summary>
    public void f_LegionKickout(long userId,SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionKickout, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(userId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionKickout, bBuf);
    }

    /// <summary>
    /// 退出工会
    /// </summary>
    public void f_LegionQuit(SocketCallbackDT socketCallbackDt)
    {
        _isQuitOperate = true;
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionQuit, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionQuit, bBuf);
    }

    /// <summary>
    /// 任命副团长
    /// </summary>
    public void f_LegionAppoint(long userId, SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionAppoint, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(userId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionAppoint, bBuf);
    }

    /// <summary>
    /// 罢免副团长
    /// </summary>
    public void f_LegionDimiss(long userId,SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionDismiss, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(userId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionDismiss, bBuf);
    }

    /// <summary>
    /// 禅让军团长
    /// </summary>
    public void f_LegionHandover(long userId,SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionHandover, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(userId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionHandover, bBuf);
    }

    /// <summary>
    /// 弹劾军团长
    /// </summary>
    public void f_LegionImpeach(SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)LegionSocketCmd.CS_LegionImpeach, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(LegionSocketCmd.CS_LegionImpeach, bBuf);
    }
    #endregion

    #region 对外接口

    public void f_ExecuteAfterLegionMemInit(ccCallback callbackHandle)
    {
        if (_bMemInit)
        {
            if (callbackHandle != null)
            {
                callbackHandle(eMsgOperateResult.OR_Succeed);
            }
        }
        else
        {
            _bMemInit = true;
            _iDeputyNum = 0;
            _callback_MemInit = callbackHandle;
            SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
            socketCallbackDt.m_ccCallbackSuc = f_Callback_MemInit;
            socketCallbackDt.m_ccCallbackFail = f_Callback_MemInit;
            f_LegionMemInit(socketCallbackDt);
        }
    }

    public void f_ExecuteAfterApplicantList(ccCallback callbackHandle)
    {
        if (_bApplicantListInit)
        {
            if (callbackHandle != null)
                callbackHandle(eMsgOperateResult.OR_Succeed);
        }
        else
        {
            _applicantList.Clear();
            Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.LegionApplicantList);
            _callback_ApplicantList = callbackHandle;
            SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
            socketCallbackDt.m_ccCallbackSuc = f_Callback_ApplicantList;
            socketCallbackDt.m_ccCallbackFail = f_Callback_ApplicantList;
            f_LegionApplicantList(socketCallbackDt);
        }
    }

    public List<BasePoolDT<long>> f_GetApplicantList()
    {
        return _applicantList;
    }

    /// <summary>
    /// 检查操作权限
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public EM_LegionPermission f_CheckOperateTypePermission(EM_LegionOperateType type,int legionPos = 0)
    {
        if (legionPos == 0)
            legionPos = _iSelfPos;
        if (type == EM_LegionOperateType.AcceptApplicant
            || type == EM_LegionOperateType.DisacceptApplicant
            || type == EM_LegionOperateType.ApplicantList
            || type == EM_LegionOperateType.SetNotice
            || type == EM_LegionOperateType.SetManifesto
            || type == EM_LegionOperateType.OpenOrUpSkill
            || type == EM_LegionOperateType.LegionBattleSignUp
            )
        {
            if (legionPos > (int)EM_LegionPostionEnum.eLegion_Deputy)
                return EM_LegionPermission.Deputy;
        }
        else if (type == EM_LegionOperateType.DissolveLegion 
                || type == EM_LegionOperateType.SetResetDungeonChapter
                || type == EM_LegionOperateType.LevelUpLegion
                )
        {
            if (legionPos > (int)EM_LegionPostionEnum.eLegion_Chief)
                return EM_LegionPermission.Chief;
        }
        return EM_LegionPermission.Enough;
    }
    
    #endregion

    private bool _bMemInit = false;
    private ccCallback _callback_MemInit;
    private void f_Callback_MemInit(object result)
    {
        if ((int)result != (int)eMsgOperateResult.OR_Succeed)
            _bMemInit = false;
        if (_callback_MemInit != null)
            _callback_MemInit(result);
    }

    private void f_SortMemberList()
    {
        f_GetAll().Sort(delegate (BasePoolDT<long> value1, BasePoolDT<long> value2)
        {
            LegionPlayerPoolDT item1 = (LegionPlayerPoolDT)value1;
            LegionPlayerPoolDT item2 = (LegionPlayerPoolDT)value2;
            bool isSelf1 = item1.iId == Data_Pool.m_UserData.m_iUserId;
            bool isSelf2 = item2.iId == Data_Pool.m_UserData.m_iUserId;
            if (isSelf1 && !isSelf2)
            {
                return -1;
            }
            else if (!isSelf1 && isSelf2)
            {
                return 1;
            }
            if (item1.PlayerInfo.m_iLegionPostion > item2.PlayerInfo.m_iLegionPostion)
            {
                return 1;
            }
            else if (item1.PlayerInfo.m_iLegionPostion < item2.PlayerInfo.m_iLegionPostion)
            {
                return -1;
            }
            if (item1.PlayerInfo.m_iBattlePower > item2.PlayerInfo.m_iBattlePower)
            {
                return -1;
            }
            else if (item1.PlayerInfo.m_iBattlePower < item2.PlayerInfo.m_iBattlePower)
            {
                return 1;
            }
            return 0;
        }
        );
    }

    private bool _bApplicantListInit = false;
    private ccCallback _callback_ApplicantList;

    private void f_Callback_ApplicantList(object result)
    {
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
            _bApplicantListInit = true;
        if (_callback_ApplicantList != null)
            _callback_ApplicantList(result);
    }
}
