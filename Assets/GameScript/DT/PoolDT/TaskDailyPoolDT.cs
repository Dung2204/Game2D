using ccU3DEngine;

public class TaskDailyPoolDT : BasePoolDT<long>
{
    public TaskDailyPoolDT(int templateId)
    {
        scoreArr = new int[11];
        awardArr = new int[11];
        mTemplateId = templateId;
        f_Reset();
    }

    /// <summary>
    /// 重置数据
    /// </summary>
    public void f_Reset()
    {
        mTaskState = EM_TaskState.Unfinished;
        mProgress = 0;
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
                TaskDailyDT tItem = (TaskDailyDT)glo_Main.GetInstance().m_SC_Pool.m_TaskDailySC.f_GetSC(_iTemplateId);
                if (tItem == null)
MessageBox.ASSERT("TaskDailySC Id does not exist, Id: " + _iTemplateId);
                mTemplate = tItem;
                _iData1 = mTemplate.iCondition;
                scoreArr[0] = mTemplate.iScore1;
                scoreArr[1] = mTemplate.iScore2;
                scoreArr[2] = mTemplate.iScore3;
                scoreArr[3] = mTemplate.iScore4;
                scoreArr[4] = mTemplate.iScore5;
                scoreArr[5] = mTemplate.iScore6;
                scoreArr[6] = mTemplate.iScore7;
                scoreArr[7] = mTemplate.iScore8;
                scoreArr[8] = mTemplate.iScore9;
                scoreArr[9] = mTemplate.iScore10;
                scoreArr[10] = mTemplate.iScore11;
                awardArr[0] = mTemplate.iAwardId1;
                awardArr[0] = mTemplate.iAwardId1;
                awardArr[1] = mTemplate.iAwardId2;
                awardArr[2] = mTemplate.iAwardId3;
                awardArr[3] = mTemplate.iAwardId4;
                awardArr[4] = mTemplate.iAwardId5;
                awardArr[5] = mTemplate.iAwardId6;
                awardArr[6] = mTemplate.iAwardId7;
                awardArr[7] = mTemplate.iAwardId8;
                awardArr[8] = mTemplate.iAwardId9;
                awardArr[9] = mTemplate.iAwardId10;
                awardArr[10] = mTemplate.iAwardId11;
            }
        }
    }

    public TaskDailyDT mTemplate
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

    //任务状态
    public EM_TaskState mTaskState
    {
        get;
        private set;
    }
    public void f_CheckRedPoint()
    {
        if (mTaskState != EM_TaskState.AlreadyAward)
        {
            int lv = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
            if (mProgress >= mTemplate.iConditionParam && lv >= mTemplate.iOpenLv && lv <= mTemplate.iCloseLv)
            {
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.TaskPage_Daily);
            }
        }
    }
    /// <summary>
    /// 更新次数
    /// </summary>
    /// <param name="value"></param>
    public void f_UpdateProgress(int value)
    {
        mProgress = value;
        if (mTaskState != EM_TaskState.AlreadyAward)
        {
            if (mProgress >= mTemplate.iConditionParam)
            {
                mTaskState = EM_TaskState.Finish;
                int lv = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
                if (lv >= mTemplate.iOpenLv && lv <= mTemplate.iCloseLv)
                    Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.TaskPage_Daily);
            }
            else
            {
                mTaskState = EM_TaskState.Unfinished;
            }
        }
    }

    /// <summary>
    /// 更新已领奖励状态
    /// </summary>
    public void f_UpdateAlreadyAward()
    {
        mTaskState = EM_TaskState.AlreadyAward;
    }


    private int[] scoreArr;
    public int mCurScore
    {
        get
        {
            int tLv = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
            int idx = tLv / 10;
            if (idx >= 0 && idx < scoreArr.Length)
                return scoreArr[idx];
            else
                return scoreArr[scoreArr.Length - 1];
        }
    }

    private int[] awardArr;
    public int mCurAwardId
    {
        get
        {
            int tLv = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
            int idx = tLv / 10;
            if (idx >= 0 && idx < awardArr.Length)
                return awardArr[idx];
            else
                return awardArr[scoreArr.Length - 1];
        }
    }
}
