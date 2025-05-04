using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TaskBoxInfo
{
    
    public TaskBoxInfo(int idx)
    {
        mIdx = idx;
    }

    public void f_UpdateInfo(int boxId, int boxScore)
    {
        mBoxId = boxId;
        if (mBoxId == 0)
            mBoxState = EM_BoxGetState.Invalid;
        mBoxScore = boxScore;
    }

    public void f_UpdateInfo(EM_BoxGetState boxState)
    {
        mBoxState = boxState;
    }

    public EM_BoxGetState mBoxState
    {
        get;
        private set;
    }
    public int mBoxScore
    {
        get;
        private set;
    }

    public int mBoxId
    {
        get;
        private set;
    }

    public int mIdx
    {
        get;
        private set;
    }
}

public class TaskDailyPool : BasePool
{
    /// <summary>
    /// 分数
    /// </summary>
    public int mScore
    {
        get;
        private set;
    }

    public int mScoreMax
    {
        get
        {
            if (mCurTaskBoxDT != null)
                return mCurTaskBoxDT.iScoreMax;
            else
                return 1;
        }
    }

    /// <summary>
    /// 宝箱掩码
    /// </summary>
    public int mBoxFlag
    {
        get;
        private set;
    }

    /// <summary>
    /// 单前等级任务宝箱数据
    /// </summary>
    public TaskBoxDT mCurTaskBoxDT
    {
        get;
        private set;
    }
    private TaskBoxInfo[] mBoxInfo;

    /// <summary>
    /// 玩家单前等级单前等级的任务PoolList
    /// </summary>
    public List<BasePoolDT<long>> CurLvTaskList
    {
        get
        {
            return curLvTaskList;
        }
    }

    private List<BasePoolDT<long>> curLvTaskList = new List<BasePoolDT<long>>();


    public TaskDailyPool() : base("TaskDailyPoolDT", false)
    {

    }

    protected override void f_Init()
    {
        List<NBaseSCDT> tmp = glo_Main.GetInstance().m_SC_Pool.m_TaskDailySC.f_GetAll();
        TaskDailyDT tmpItem;
        TaskDailyPoolDT tPoolItem;
        for (int i = 0; i < tmp.Count; i++)
        {
            tmpItem = (TaskDailyDT)tmp[i];
            if (tmpItem.iTaskType == 1)//日常任务
            {
                tPoolItem = new TaskDailyPoolDT(tmpItem.iId);
                tPoolItem.iId = tmpItem.iId;
                f_Save(tPoolItem);
            }
            else//主线任务
            {

            }
        }
        mScore = 0;
        mBoxFlag = 0;
        mInitFlag = false;
        mBoxInfo = new TaskBoxInfo[GameParamConst.TaskBoxMaxNum];
        for (int i = 0; i < mBoxInfo.Length; i++)
        {
            mBoxInfo[i] = new TaskBoxInfo(i);
            mBoxInfo[i].f_UpdateInfo(0, 0);
        }
        f_UpdateCurTaskBoxDT(1);
        f_UpdateCurLvTaskPoolList(1);
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.PlayerLvUpdate, f_UpdateDataByLv);
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.TheNextDay, f_UpdateDataByNextDay);
    } 

    /// <summary>
    /// 重置任务数据
    /// </summary>
    public void f_Reset()
    {
        foreach (TaskDailyPoolDT item in f_GetAll())
        {
            item.f_Reset();
        }
        mScore = 0;
        mBoxFlag = 0;
        f_UpdateBoxInfo(mBoxFlag);
    }

    protected override void RegSocketMessage()
    {
        //更新任务进度数据
        SC_TaskDailyData tSC_TaskDailyData = new SC_TaskDailyData();
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_TaskDaily,tSC_TaskDailyData,Callback_SocketData_TaskDailyUpdate);

        //更新任务领奖数据
        basicNode1 tBasicNode1 = new basicNode1();
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_TaskDailyAwarded, tBasicNode1, Callback_SocketData_TaskDailyAwardUpdate);

        //宝箱积分相关
        SC_TaskBox tSC_TaskBox = new SC_TaskBox();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_TaskBox, tSC_TaskBox, Callback_SocketData_TaskBoxUpdate);

        //主线任务相关（仅显示在主界面）
        SC_TaskMainInit scTaskMainInit = new SC_TaskMainInit();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_TaskMainInit, scTaskMainInit, Callback_TaskMainInit);
    }

    public int mMainTaskId;       // 当前任务Id
    public int mMainTaskValue;        // 进度值
    public int mMainTaskAwardTimes;	// 奖励领取次数
    /// <summary>
    /// 主线任务回调
    /// </summary>
    public void Callback_TaskMainInit(object data)
    {
        SC_TaskMainInit scTaskMainInit = (SC_TaskMainInit)data;
        mMainTaskId = scTaskMainInit.uTaskId;
        mMainTaskValue = scTaskMainInit.uValue;
        mMainTaskAwardTimes = scTaskMainInit.uAwardTimes;
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_TASKMAINUPDATE);
    }
    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
        throw new NotImplementedException();
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
        throw new NotImplementedException();
    }


    private void Callback_SocketData_TaskDailyUpdate(int iData1, int iData2, int iNum, ArrayList aData)
    {
        foreach (SockBaseDT tData in aData)
        {
            if (iData1 == (int)eUpdateNodeType.node_add)
            {
MessageBox.ASSERT("Data has been updated");
            }
            else if (iData1 == (int)eUpdateNodeType.node_update)
            {
                f_Socket_TaskDailyUpdateData(tData);
            }
            else if (iData1 == (int)eUpdateNodeType.node_default)
            {
MessageBox.ASSERT("Data has been updated");
            }
        }
    }

    private void f_Socket_TaskDailyUpdateData(SockBaseDT node)
    {
        SC_TaskDailyData tServerData = (SC_TaskDailyData)node;
        List<BasePoolDT<long>> tmp = f_GetAllForData1((int)tServerData.eType);
        for (int i = 0; i < tmp.Count; i++)
        {
            TaskDailyPoolDT item = (TaskDailyPoolDT)tmp[i];
            item.f_UpdateProgress(tServerData.uValue);
        }
        f_SortList();
    }
    public void f_CheckRedPoint()
    {
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.TaskPage_Daily);
        List<BasePoolDT<long>> tmp = f_GetAll();
        for (int i = 0; i < tmp.Count; i++)
        {
            TaskDailyPoolDT item = (TaskDailyPoolDT)tmp[i];
            item.f_CheckRedPoint();
        }
        for (int i = 0; i < mBoxInfo.Length; i++)
        {
            if(mBoxInfo[i].mBoxState == EM_BoxGetState.CanGet)
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.TaskPage_Daily);
        }
    }
    private void Callback_SocketData_TaskDailyAwardUpdate(int iData1, int iData2, int iNum, ArrayList aData)
    {
        foreach (SockBaseDT tData in aData)
        {
            if (iData1 == (int)eUpdateNodeType.node_add)
            {
MessageBox.ASSERT("Data has been updated");
            }
            else if (iData1 == (int)eUpdateNodeType.node_update)
            {
                f_Socket_TaskDailyAwardUpdateData(tData);
            }
            else if (iData1 == (int)eUpdateNodeType.node_default)
            {
MessageBox.ASSERT("Data has been updated");
            }
        }
    }

    //tServerData.value1  已完成的任务ID
    private void f_Socket_TaskDailyAwardUpdateData(SockBaseDT node)
    {
        basicNode1 tServerData = (basicNode1)node;
        TaskDailyPoolDT item = (TaskDailyPoolDT)f_GetForId(tServerData.value1);
        if (item == null)
        {
MessageBox.ASSERT("TaskDaily Id sent by server does not exist, Id:" + tServerData.value1);
        }
        else
        {
            item.f_UpdateAlreadyAward();
            f_SortList();
        }
    }

   
    /// <summary>
    /// 更新宝箱信息
    /// </summary>
    /// <param name="boxFlag"></param>
    private void f_UpdateBoxInfo(int boxFlag)
    {
        //处理掩码
        //int tBox1 = (boxFlag / 10000) % 10;
        //int tBox2 = (boxFlag / 1000) % 10;
        //int tBox3 = (boxFlag / 100) % 10;
        //int tBox4 = (boxFlag / 10) % 10;
        //int tBox5 = boxFlag % 10;
        int[] tBoxFlag = new int[GameParamConst.TaskBoxMaxNum];
        for (int i = 0; i < GameParamConst.TaskBoxMaxNum; i++)
        {
            tBoxFlag[i] = (boxFlag / (int)Math.Pow(10, GameParamConst.TaskBoxMaxNum - 1 - i)) % 10;
            if (mBoxInfo[i].mBoxId != 0)
            {
                if (mScore >= mBoxInfo[i].mBoxScore)
                {
                    mBoxInfo[i].f_UpdateInfo(tBoxFlag[i] != 0 ? EM_BoxGetState.AlreadyGet : EM_BoxGetState.CanGet);
                    if(mBoxInfo[i].mBoxState == EM_BoxGetState.CanGet)
                        Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.TaskPage_Daily);
                }
                else
                    mBoxInfo[i].f_UpdateInfo(EM_BoxGetState.Lock);
            }
            else
                mBoxInfo[i].f_UpdateInfo(EM_BoxGetState.Invalid);
        }
    }

    private void Callback_SocketData_TaskBoxUpdate(object result)
    {
        SC_TaskBox node = (SC_TaskBox)result;
        mScore = node.uScore;
        mBoxFlag = node.uFlag;
        f_UpdateBoxInfo(mBoxFlag);
    }

    private void f_UpdateDataByLv(object value)
    {
        int lv = (int)value;
        f_UpdateCurTaskBoxDT(lv);
        f_UpdateCurLvTaskPoolList(lv);
    }

    private void f_UpdateDataByNextDay(object value)
    {
        f_Reset();
        //跨天UI事件  通知关心此消息的UI更新
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_THENEXTDAY_UIPROCESS,EM_NextDaySource.TaskDailyPool);
    }

    /// <summary>
    /// 更新任务宝箱配置数据
    /// </summary>
    /// <param name="lv">玩家等级</param>
    private void f_UpdateCurTaskBoxDT(int lv)
    {
        List<NBaseSCDT> tmp = glo_Main.GetInstance().m_SC_Pool.m_TaskBoxSC.f_GetAll();
        TaskBoxDT tmpItem;
        for (int i = 0; i < tmp.Count; i++)
        {
            tmpItem = (TaskBoxDT)tmp[i];
            if (lv >= tmpItem.iOpenLv && lv < tmpItem.iCloseLv)
            {
                mCurTaskBoxDT = tmpItem;
                mBoxInfo[0].f_UpdateInfo(mCurTaskBoxDT.iBox1Id,mCurTaskBoxDT.iScore1);
                mBoxInfo[1].f_UpdateInfo(mCurTaskBoxDT.iBox2Id, mCurTaskBoxDT.iScore2);
                mBoxInfo[2].f_UpdateInfo(mCurTaskBoxDT.iBox3Id, mCurTaskBoxDT.iScore3);
                mBoxInfo[3].f_UpdateInfo(mCurTaskBoxDT.iBox4Id, mCurTaskBoxDT.iScore4);
                mBoxInfo[4].f_UpdateInfo(mCurTaskBoxDT.iBox5Id, mCurTaskBoxDT.iScore5);
                f_UpdateBoxInfo(mBoxFlag);
            }
        }
    }

    /// <summary>
    /// 更新单前等级的
    /// </summary>
    /// <param name="lv"></param>
    private void f_UpdateCurLvTaskPoolList(int lv)
    {
        curLvTaskList.Clear();
        List<BasePoolDT<long>> tAllTaskList = f_GetAll();
        for (int i = 0; i < tAllTaskList.Count; i++)
        {
            TaskDailyPoolDT tItem = (TaskDailyPoolDT)tAllTaskList[i];
            if (lv >= tItem.mTemplate.iOpenLv && lv <= tItem.mTemplate.iCloseLv)
                curLvTaskList.Add(tAllTaskList[i]);
        }
        f_SortList();
    }

    #region 发送协议

    /// <summary>
    /// 请求日常任务初始化
    /// </summary>
    public void f_TaskDailyInit(SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_TaskDaily, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_TaskDaily, bBuf);
    }

    /// <summary>
    /// 请求任务奖励
    /// </summary>
    /// <param name="taskId">任务Id</param>
    public void f_TaskDailyAwarded(int taskId,SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_TaskDailyAward, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(taskId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_TaskDailyAward, bBuf);
    }

    /// <summary>
    /// 请求宝箱奖励
    /// </summary>
    /// <param name="iId">任务宝箱表对应配置的Id</param>
    /// <param name="boxIdx">宝箱索引 1-5</param>
    public void f_TaskBoxAwarded(int iId, byte boxIdx,SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_TaskBox, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(iId);
        tCreateSocketBuf.f_Add(boxIdx);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_TaskBox, bBuf);
    }
    public void f_TaskMainInit(int taskId, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_TaskMainInit, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(taskId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_TaskMainInit, bBuf);
    }
    public void f_TaskTaskMainAward(SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_TaskMainAward, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_TaskMainAward, bBuf);
    }
    #endregion

    public EM_BoxGetState f_GetBoxStateByIdx(int idx)
    {
        if (idx < 0 || idx >=  mBoxInfo.Length)
        {
            return EM_BoxGetState.Invalid;
        }
        else
        {
            return mBoxInfo[idx].mBoxState;
        }
    }

    public string f_GetBoxExtraInfo(int idx)
    {
        if (idx < 0 || idx >= mBoxInfo.Length)
            return string.Empty;
        else
return string.Format("[f3c240]{0} points", mBoxInfo[idx].mBoxScore);
    }

    public TaskBoxInfo f_GetBoxInfo(int idx)
    {
        if (idx < 0 || idx >= mBoxInfo.Length)
            return null;
        else
            return mBoxInfo[idx];
    }

    private bool mInitFlag = false;
    private ccCallback _tmpFunc;
    /// <summary>
    /// 初始化日常任务数据 用此方法
    /// </summary>
    public void f_ExecuteAfterInit(ccCallback func)
    {
        if (!mInitFlag)
        {
            _tmpFunc = func;
            SocketCallbackDT tCallbackDT = new SocketCallbackDT();
            tCallbackDT.m_ccCallbackSuc = f_TaskDailyInitSuc;
            tCallbackDT.m_ccCallbackFail = f_TaskDailyInitFail;
            f_TaskDailyInit(tCallbackDT);
        }
        else
        {
            if (func != null)
                func(0);
        }
    }

    public void f_TaskDailyInitSuc(object result)
    {
        if (_tmpFunc != null)
            _tmpFunc(result);
        mInitFlag = true;
    }

    public void f_TaskDailyInitFail(object result)
    {
        if (_tmpFunc != null)
            _tmpFunc(result);
        MessageBox.ASSERT("TaskDaily Init Fail");
    }

    /// <summary>
    /// 任务数据排序  已完成>未完成>已领取  且Id小的在前面
    /// </summary>
    private void f_SortList()
    {
        curLvTaskList.Sort(delegate (BasePoolDT<long> node1, BasePoolDT<long> node2)
        {
            TaskDailyPoolDT item1 = (TaskDailyPoolDT)node1;
            TaskDailyPoolDT item2 = (TaskDailyPoolDT)node2;
            if ((int)item1.mTaskState > (int)item2.mTaskState)
                return 1;
            else if ((int)item1.mTaskState < (int)item2.mTaskState)
                return -1;
            else
            {
                if (item1.iId > item2.iId)
                    return 1;
                else if (item1.iId < item2.iId)
                    return -1;
            }
            return 0;
        });
    }
}
