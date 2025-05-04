using ccU3DEngine;
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class TaskAchvPool : BasePool
{
    private List<BasePoolDT<long>> curTaskPoolList;
    public List<BasePoolDT<long>> CurTaskPoolList
    {
        get
        {
            return curTaskPoolList;
        }
    }

    public TaskAchvPool() : base("TaskAchvPoolDT", false)
    {

    }

    protected override void f_Init()
    {
        curTaskPoolList = new List<BasePoolDT<long>>();
        Array enumArr = Enum.GetValues(typeof(EM_AchievementTaskCondition));
        for (int i = 0; i < enumArr.Length; i++)
        {
            int tType = (int)enumArr.GetValue(i);
            TaskAchvPoolDT tPooldt = new TaskAchvPoolDT(tType);
            f_Save(tPooldt);
        }
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.TaskAchvUpdateProgress, f_UpdateProgressByLocal);
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.PlayerLvUpdate, f_UpdateDataByLv);
    }

    private void f_UpdateDataByLv(object value)
    {
        int lv = (int)value;
        curTaskPoolList.Clear();
        TaskAchvPoolDT tCurNode;
        List<BasePoolDT<long>> tSourceTaskPollList = f_GetAll();
        for (int i = 0; i < tSourceTaskPollList.Count; i++)
        {
            tCurNode = (TaskAchvPoolDT)tSourceTaskPollList[i];
            if (tCurNode.mTemplate != null && lv >= tCurNode.mTemplate.iOpenLv)
                curTaskPoolList.Add(tSourceTaskPollList[i]);
        }
        f_SortList();
        f_RemoveUnUseCurTaskPoolList();
    }


    protected override void RegSocketMessage()
    {
        SC_TaskAchievementInfo tSC_TaskAchievementInfo = new SC_TaskAchievementInfo();
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_TaskAchievement, tSC_TaskAchievementInfo, Callback_SocketData_Update);
    }

    public void f_CheckRedPoint()
    {
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.TaskPage_Achievement);
        List<BasePoolDT<long>> tmp = f_GetAll();
        for (int i = 0; i < tmp.Count; i++)
        {
            TaskAchvPoolDT item = (TaskAchvPoolDT)tmp[i];
            item.f_CheckRedPoint();
        }
    }
    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
MessageBox.ASSERT("Data has been updated");
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
        SC_TaskAchievementInfo tServerData = (SC_TaskAchievementInfo)Obj;
        TaskAchvPoolDT tItem = (TaskAchvPoolDT)f_GetForId(tServerData.eType);
        if (tItem != null)
        {
            tItem.f_UpdatePassTaskId(tServerData.lastId);
            f_RemoveUnUseCurTaskPoolList();
            if (f_CheckUpdateByServer(tServerData.eType))
            {
                tItem.f_UpdateProgress(tServerData.uValue);
            }
            f_SortList();
        }
        else
MessageBox.ASSERT("Client not processing , TypeId:" + tServerData.eType);
    }

    public void f_TaskAchievementInit(SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_TaskAchievement, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_TaskAchievement, bBuf);
    }

    public void f_GetAchievementAward(byte type,SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_TaskAchievementAward, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(type);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_TaskAchievementAward, bBuf);
    }

    /// <summary>
    /// 移除全部完成的任务数据
    /// </summary>
    private void f_RemoveUnUseCurTaskPoolList()
    {
        int removeNum = curTaskPoolList.RemoveAll(delegate (BasePoolDT<long> item)
        {
            TaskAchvPoolDT tItem = (TaskAchvPoolDT)item;
            return tItem.mTemplate == null;
        });
MessageBox.DEBUG("Deleted task: " + removeNum);
        f_SortList();

        MessageBox.DEBUG("f_RemoveUnUseCurTaskPoolList End");
    }

    #region 进度更新相关

    private int[] _serverUpdateTypeId = new int[] { 8, 9, 10 };

    //true:根据服务器数据更新，false:客户端自己更新
    private bool f_CheckUpdateByServer(int iType)
    {
        for (int i = 0; i < _serverUpdateTypeId.Length; i++)
        {
            if (_serverUpdateTypeId[i] == iType)
                return true;
        }
        return false;
    }

    /// <summary>
    /// 本地更新成就任务进度
    /// </summary>
    /// <param name="value">int[] 0:类型 1:进度值 </param>
    private void f_UpdateProgressByLocal(object value)
    {
        int[] result = (int[])value;
        int tType = result[0];
        int tValue = result[1];
        TaskAchvPoolDT tItem = (TaskAchvPoolDT)f_GetForId(tType);
        if (tItem != null)
        {
            if (!f_CheckUpdateByServer(tType))
            {
                tItem.f_UpdateProgress(tValue);
                f_SortList();
            }
            else
MessageBox.ASSERT("There is a problem with the client-side update progress，TypeId：" + tType);
        }
        else
MessageBox.ASSERT("Client not processing, TypeId:" + tType);
    }

    #endregion

    #region 成就任务SC相关

    private Dictionary<int, List<TaskAchievementDT>> mTaskAchvSC_Dic = new Dictionary<int, List<TaskAchievementDT>>();

    public int f_GetTemplateId(int iType,int lastFinishTaskId)
    {
        if (!mTaskAchvSC_Dic.ContainsKey(iType))
        {
            List<TaskAchievementDT> tList = new List<TaskAchievementDT>();
            List<NBaseSCDT> tSourceList = glo_Main.GetInstance().m_SC_Pool.m_TaskAchievementSC.f_GetAll().FindAll(delegate (NBaseSCDT item) { TaskAchievementDT tItem = (TaskAchievementDT)item;
                return tItem.iCondition == iType;
            });
            for (int i = 0; i < tSourceList.Count; i++)
            {
                tList.Add((TaskAchievementDT)tSourceList[i]);
            }
            mTaskAchvSC_Dic.Add(iType,tList);
        }
        TaskAchievementDT tCurItem = mTaskAchvSC_Dic[iType].Find(delegate (TaskAchievementDT item) { return item.iId > lastFinishTaskId; });
        if (tCurItem != null)
            return tCurItem.iId;
        else
            return 0;
    }

    public static int f_GetInitTemplateId(int iType)
    {
        TaskAchievementDT tResult = (TaskAchievementDT)glo_Main.GetInstance().m_SC_Pool.m_TaskAchievementSC.f_GetAll().Find(delegate (NBaseSCDT item)
        {
            TaskAchievementDT tItem = (TaskAchievementDT)item;
            return tItem.iCondition == iType;
        });
        if (tResult != null)
            return tResult.iId;
        else
            return 0;
    }

    #endregion

    #region 初始化相关
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
            tCallbackDT.m_ccCallbackSuc = f_TaskAchvInitSuc;
            tCallbackDT.m_ccCallbackFail = f_TaskAchvInitFail;
            f_TaskAchievementInit(tCallbackDT);
        }
        else
        {
            if (func != null)
                func(0);
        }
    }

    public void f_TaskAchvInitSuc(object result)
    {
        if (_tmpFunc != null)
            _tmpFunc(result);
        mInitFlag = true;
    }

    public void f_TaskAchvInitFail(object result)
    {
        if (_tmpFunc != null)
            _tmpFunc(result);
        MessageBox.ASSERT("TaskAchievement Init Fail");
    }

    #endregion

    /// <summary>
    /// 任务数据排序  已完成>未完成>已领取  且Id小的在前面
    /// </summary>
    private void f_SortList()
    {
        curTaskPoolList.Sort(delegate (BasePoolDT<long> node1, BasePoolDT<long> node2)
        {
            TaskAchvPoolDT item1 = (TaskAchvPoolDT)node1;
            TaskAchvPoolDT item2 = (TaskAchvPoolDT)node2;
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
