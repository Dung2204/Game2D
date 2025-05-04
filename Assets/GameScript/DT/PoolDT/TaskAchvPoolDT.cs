using ccU3DEngine;

public class TaskAchvPoolDT : BasePoolDT<long>
{
    public TaskAchvPoolDT(int iType)
    {
        iId = iType;
        mTaskState = EM_TaskState.Unfinished;
        mProgress = 0;
        mLastFinishTaskId = 0;
        mTemplateId = TaskAchvPool.f_GetInitTemplateId(iType);
    }

    private int _iTemplateId;
    public int mTemplateId
    {
        get
        {
            return _iTemplateId;
        }
        private set
        {
            if (_iTemplateId != value)
            {
                _iTemplateId = value;
                //如果传入模板Id = 0 模板数据清除
                if (_iTemplateId == 0)
                {
                    mTemplate = null;
                    return;
                }
                TaskAchievementDT tItem = (TaskAchievementDT)glo_Main.GetInstance().m_SC_Pool.m_TaskAchievementSC.f_GetSC(_iTemplateId);
                if (tItem == null)
MessageBox.ASSERT("TaskAchievementSC Id does not exist, Id: " + _iTemplateId);
                mTemplate = tItem;
            }
        }
    }

    public TaskAchievementDT mTemplate
    {
        private set;
        get;
    }

    /// <summary>
    /// 进度
    /// </summary>
    public int mProgress
    {
        get;
        private set;
    }

    /// <summary>
    /// 任务状态
    /// </summary>
    public EM_TaskState mTaskState
    {
        get;
        private set;
    }

    public int mLastFinishTaskId
    {
        get;
        private set;
    }

    #region 对外接口

    public void f_CheckRedPoint()
    {
        if (mTaskState != EM_TaskState.AlreadyAward)
        {
            int lv = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
            if (mProgress >= mTemplate.iConditionParam && lv >= mTemplate.iOpenLv)
            {
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.TaskPage_Achievement);
            }
        }
    }
    public void f_UpdateProgress(int value)
    {
        mProgress = value;
        if (mTemplate == null)
            mTaskState = EM_TaskState.AlreadyAward;
        if (mTaskState != EM_TaskState.AlreadyAward)
        {
            int lv = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
            if (mProgress >= mTemplate.iConditionParam)
            {
                mTaskState = EM_TaskState.Finish;
                if(lv >= mTemplate.iOpenLv)
                    Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.TaskPage_Achievement);
            }
            else
                mTaskState = EM_TaskState.Unfinished;
        }
    }

    /// <summary>
    /// 更新上次完成任务的Id
    /// </summary>
    /// <param name="taskId"></param>
    public void f_UpdatePassTaskId(int taskId)
    {
        mLastFinishTaskId = taskId;
        mTemplateId = Data_Pool.m_TaskAchvPool.f_GetTemplateId((int)iId, mLastFinishTaskId);
        f_UpdateProgress(mProgress);
    }

    #endregion

}
