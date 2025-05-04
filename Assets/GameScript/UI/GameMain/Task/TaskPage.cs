using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class TaskPage : UIFramwork
{
    private GameObject _backBtn;

    private GameObject _dailyTaskPage;
    private GameObject _dailyTaskItemParent;
    private GameObject _dailyTaskItem;
    private List<BasePoolDT<long>> _dailyTaskList;
    private UIWrapComponent _dailyTaskWrapComponent;
    private UIWrapComponent mDailyTaskWrapComponent
    {
        get
        {
            if (_dailyTaskWrapComponent == null)
            {
                _dailyTaskList = Data_Pool.m_TaskDailyPool.CurLvTaskList;
                _dailyTaskWrapComponent = new UIWrapComponent(200, 1, 800, 10, _dailyTaskItemParent, _dailyTaskItem, _dailyTaskList, DailyTaskItemUpdateByInfo, null);
            }
            return _dailyTaskWrapComponent;
        }
    }
    private UILabel _scoreLabel;
    private UISlider _bxoSlider;
    private GameObject _taskBoxItemParent;
    private GameObject _taskBoxItem;
    private BoxCommonItem[] _taskBoxItems;
    private GameObject _dailyBtn;
    private GameObject _dailyBtnSelect;
    //成就任务相关
    private GameObject _achievementTaskPage;
    private GameObject _achievementTaskItemParent;
    private GameObject _achievementTaskItem;
    private List<BasePoolDT<long>> _achievementTaskList;
    private UIWrapComponent _achievementTaskWrapComponent;
    private UIWrapComponent mAchievementTaskWrapComponent
    {
        get
        {
            if (_achievementTaskWrapComponent == null)
            {
                _achievementTaskList = Data_Pool.m_TaskAchvPool.CurTaskPoolList;
                _achievementTaskWrapComponent = new UIWrapComponent(200, 1, 800, 10, _achievementTaskItemParent, _achievementTaskItem, _achievementTaskList, AchievementTaskItemUpdateByInfo, null);
            }
            return _achievementTaskWrapComponent;
        }
    }
    private GameObject _achievementBtn;
    private GameObject _achievementBtnSelect;

    private Transform _roleParent;
    private GameObject _role;
    private const int ShowRoleId = 12051;

    /// <summary>
    /// 单前任务类型
    /// </summary>
    private EM_TaskType mCurTaskType;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        _backBtn = f_GetObject("BackBtn");
        _dailyTaskPage = f_GetObject("DailyTaskPage");
        _dailyTaskItemParent = f_GetObject("DailyTaskItemParent");
        _dailyTaskItem = f_GetObject("DailyTaskItem");
        _scoreLabel = f_GetObject("ScoreLabel").GetComponent<UILabel>();
        _bxoSlider = f_GetObject("BoxSlider").GetComponent<UISlider>();
        _taskBoxItemParent = f_GetObject("BoxItemParent");
        _taskBoxItem = f_GetObject("BoxCommonItem");
        _taskBoxItems = new BoxCommonItem[GameParamConst.TaskBoxMaxNum];
        for (int i = 0; i < _taskBoxItems.Length; i++)
        {
            _taskBoxItems[i] = BoxCommonItem.f_Create(_taskBoxItemParent, _taskBoxItem);
            _taskBoxItems[i].f_UpdateClickHandle(f_TaskBoxClickHandle, i);
        }
        _achievementTaskPage = f_GetObject("AchievementTaskPage");
        _achievementTaskItemParent = f_GetObject("AchievementTaskItemParent");
        _achievementTaskItem = f_GetObject("AchievementTaskItem");
        _dailyBtn = f_GetObject("DailyBtn");
        _dailyBtnSelect = f_GetObject("DailyBtnSelect");
        _achievementBtn = f_GetObject("AchievementBtn");
        _achievementBtnSelect = f_GetObject("AchievementBtnSelect");
        _roleParent = f_GetObject("RoleParent").transform;
        f_RegClickEvent(_backBtn, f_BackBtnClick);
        f_RegClickEvent(_dailyBtn, f_DailyTaskBtnClick);
        f_RegClickEvent(_achievementBtn, f_AchevementBtnClick);
        //UITool.f_CreateRoleByModeId(ShowRoleId, ref _role, _roleParent, 1);
    }

    protected override void InitRaddot()
    {
        base.InitRaddot();

        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.TaskPage_Daily, f_GetObject("DailyBtn"), ReddotCallback_Show_BtnTask);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.TaskPage_Achievement, f_GetObject("AchievementBtn"), ReddotCallback_Show_AchievementBtn);

        UpdateReddotUI();


    }
    protected override void On_Destory()
    {
        base.On_Destory();
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.TaskPage_Daily, f_GetObject("DailyBtn"));
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.TaskPage_Achievement, f_GetObject("AchievementBtn"));

    }
    protected override void UpdateReddotUI()
    {
        base.UpdateReddotUI();

        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.TaskPage_Daily);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.TaskPage_Achievement);
    }

    private void ReddotCallback_Show_BtnTask(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnTask = f_GetObject("DailyBtn");
        UITool.f_UpdateReddot(BtnTask, iNum, new Vector3(95, 30, 0), 401);
    }
    private void ReddotCallback_Show_AchievementBtn(object Obj)
    {
        int iNum = (int)Obj;
        GameObject AchievementBtn = f_GetObject("AchievementBtn");
        UITool.f_UpdateReddot(AchievementBtn, iNum, new Vector3(95, 30, 0), 401);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        //触发刷新下战力数值
        Data_Pool.m_TeamPool.f_GetTotalBattlePower();
        List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
        listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Money);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Energy);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, listMoneyType);
        mCurTaskType = EM_TaskType.Daily;
        f_UpdateTaskByType(mCurTaskType);
		
		//My Code
		float windowAspect = (float)Screen.width /  (float) Screen.height ;
		MessageBox.ASSERT("" + windowAspect);
		if(windowAspect <= 1.55)
		{
			f_GetObject("Anchor-Center").transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
		}
		//
		
        //UI事件
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_THENEXTDAY_UIPROCESS, f_NextDayProcess, this);

    }

    /// <summary>
    /// 跨天处理
    /// </summary>
    private void f_NextDayProcess(object value)
    {
        if ((EM_NextDaySource)value != EM_NextDaySource.TaskDailyPool)
            return;
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1266));
        if (mCurTaskType == EM_TaskType.Daily)
        {
            Data_Pool.m_TaskDailyPool.f_ExecuteAfterInit(f_ExecuteAfterTaskDailyInit);
        }
    }

    private void f_ExecuteAfterTaskDailyInit(object resutlt)
    {
        mDailyTaskWrapComponent.f_ResetView();
        f_UpdateTaskBox();
        f_UpdateBoxSlider();
    }

    private void f_ExecuteAfterAchevementDailyInit(object resutlt)
    {
        mAchievementTaskWrapComponent.f_ResetView();
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
        //UI事件
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_THENEXTDAY_UIPROCESS, f_NextDayProcess, this);
    }

    protected override void UI_HOLD(object e)
    {
        base.UI_HOLD(e);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
    }

    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        //触发刷新下战力数值
        Data_Pool.m_TeamPool.f_GetTotalBattlePower();
        List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
        listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Money);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Energy);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, listMoneyType);
        f_UpdateTaskByType(mCurTaskType);
    }

    private void f_UpdateTaskByType(EM_TaskType type)
    {
        _dailyTaskPage.SetActive(type == EM_TaskType.Daily);
        _dailyBtnSelect.SetActive(type == EM_TaskType.Daily);
        _achievementTaskPage.SetActive(type == EM_TaskType.Achievement);
        _achievementBtnSelect.SetActive(type == EM_TaskType.Achievement);
        if (type == EM_TaskType.Daily)
        {
            Data_Pool.m_TaskDailyPool.f_ExecuteAfterInit(f_ExecuteAfterTaskDailyInit);
        }
        else if (type == EM_TaskType.Achievement)
        {
            Data_Pool.m_TaskDailyPool.f_ExecuteAfterInit(f_ExecuteAfterAchevementDailyInit);
        }
    }

    private void f_DailyTaskBtnClick(GameObject go, object value1, object value2)
    {
        if (mCurTaskType == EM_TaskType.Daily)
            return;
        mCurTaskType = EM_TaskType.Daily;
        f_UpdateTaskByType(mCurTaskType);
    }

    private void f_AchevementBtnClick(GameObject go, object value1, object value2)
    {
        if (mCurTaskType == EM_TaskType.Achievement)
            return;
        mCurTaskType = EM_TaskType.Achievement;
        f_UpdateTaskByType(mCurTaskType);
    }

    /// <summary>
    /// 更新宝箱状态
    /// </summary>
    private void f_UpdateTaskBox()
    {
        for (int i = 0; i < _taskBoxItems.Length; i++)
        {
            _taskBoxItems[i].f_UpdateInfo(EM_BoxType.Task, Data_Pool.m_TaskDailyPool.f_GetBoxStateByIdx(i), Data_Pool.m_TaskDailyPool.f_GetBoxExtraInfo(i),false);
        }
    }

    /// <summary>
    /// 更新积分
    /// </summary>
    private void f_UpdateBoxSlider()
    {
        _scoreLabel.text = Data_Pool.m_TaskDailyPool.mScore.ToString();
        _bxoSlider.value = Data_Pool.m_TaskDailyPool.mScore / (float)Data_Pool.m_TaskDailyPool.mScoreMax;
    }

    //日常任务Item更新
    private void DailyTaskItemUpdateByInfo(Transform item, BasePoolDT<long> dt)
    {
        TaskDailyPoolDT node = (TaskDailyPoolDT)dt;
        TaskItem tUpdateItem = item.GetComponent<TaskItem>();
        tUpdateItem.f_UpdateByInfo(node);
        f_RegClickEvent(tUpdateItem.mGetAwardBtn, f_DailyTaskAwardClick, node);
        f_RegClickEvent(tUpdateItem.mGotoBtn, f_GotoClick, node);
    }

    /// <summary>
    /// 前往按钮 点击
    /// </summary>
    private void f_GotoClick(GameObject go, object value1, object value2)
    {
        TaskDailyPoolDT node = (TaskDailyPoolDT)value1;
        MessageBox.DEBUG(string.Format(CommonTools.f_GetTransLanguage(1267), node.mTemplateId, node.mTemplate.iGotoId));
        UITool.f_DailyGoto(this,(EM_DailyTaskCondition)node.mTemplate.iCondition);
    }

    private TaskDailyPoolDT mCurTaskPoolDT;
    /// <summary>
    /// 领奖按钮点击
    /// </summary>
    /// <param name="go"></param>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    private void f_DailyTaskAwardClick(GameObject go, object value1, object value2)
    {
        mCurTaskPoolDT = (TaskDailyPoolDT)value1;
        SocketCallbackDT callbackDT = new SocketCallbackDT();
        callbackDT.m_ccCallbackSuc = f_GetDailyTaskAwardSuc;
        callbackDT.m_ccCallbackFail = f_GetDailyTaskAwardFail;
        Data_Pool.m_TaskDailyPool.f_TaskDailyAwarded(mCurTaskPoolDT.mTemplateId, callbackDT);
        UITool.f_OpenOrCloseWaitTip(true);
    }

    /// <summary>
    /// 任务领奖成功回调
    /// </summary>
    /// <param name="value"></param>
    private void f_GetDailyTaskAwardSuc(object value)
    {
        mDailyTaskWrapComponent.f_UpdateView();
        f_UpdateTaskBox();
        f_UpdateBoxSlider();
        //展示获得奖励
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId((int)EM_ResourceType.Money,(int)EM_UserAttr.eUserAttr_TaskScore,
            mCurTaskPoolDT.mCurScore,mCurTaskPoolDT.mCurAwardId), this });
        UITool.f_OpenOrCloseWaitTip(false);
        Data_Pool.m_TaskDailyPool.f_CheckRedPoint();//不需要联网（日常任务）
    }

    /// <summary>
    /// 任务领奖失败回调
    /// </summary>
    /// <param name="value"></param>
    private void f_GetDailyTaskAwardFail(object value)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(1268) + value);
        MessageBox.ASSERT("DailyTaskAwardFail. Fail Code:" + value);
    }

    private int mCurBoxIdx = 0;
    /// <summary>
    /// 宝箱点击
    /// </summary>
    private void f_TaskBoxClickHandle(object value)
    {
        mCurBoxIdx = (int)value;
        TaskBoxInfo tBoxInfo = Data_Pool.m_TaskDailyPool.f_GetBoxInfo(mCurBoxIdx);
        BoxGetSubPageParam tParam = new BoxGetSubPageParam(tBoxInfo.mBoxId, tBoxInfo.mIdx, tBoxInfo.mBoxScore.ToString(), EM_BoxType.Task, tBoxInfo.mBoxState, f_TaskBoxGetHandle, this);
        //打开宝箱显示界面
        ccUIManage.GetInstance().f_SendMsg(UINameConst.BoxGetSubPage, UIMessageDef.UI_OPEN, tParam);
    }

    private void f_TaskBoxGetHandle(object value)
    {
        int idx = (int)value;
        EM_BoxGetState tBoxState = Data_Pool.m_TaskDailyPool.f_GetBoxStateByIdx(idx);
        if (tBoxState == EM_BoxGetState.CanGet)
        {
            SocketCallbackDT callbackDT = new SocketCallbackDT();
            callbackDT.m_ccCallbackSuc = f_GetTaskBoxAwardSuc;
            callbackDT.m_ccCallbackFail = f_GetTaskBoxAwardFail;
            Data_Pool.m_TaskDailyPool.f_TaskBoxAwarded((int)Data_Pool.m_TaskDailyPool.mCurTaskBoxDT.iId, (byte)(idx + 1), callbackDT);
            UITool.f_OpenOrCloseWaitTip(true);
        }
    }


    private void f_GetTaskBoxAwardSuc(object value)
    {
        TaskBoxInfo tBoxInfo = Data_Pool.m_TaskDailyPool.f_GetBoxInfo(mCurBoxIdx);
        //关闭宝箱领取界面
        ccUIManage.GetInstance().f_SendMsg(UINameConst.BoxGetSubPage, UIMessageDef.UI_CLOSE);
        //展示获得奖励
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN, new object[] { Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(tBoxInfo.mBoxId), this });
        f_UpdateTaskBox();
        UITool.f_OpenOrCloseWaitTip(false);
        Data_Pool.m_TaskDailyPool.f_CheckRedPoint();//不需要联网（日常任务）
    }

    private void f_GetTaskBoxAwardFail(object value)
    {
        UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(1269) + value);
        MessageBox.ASSERT("GetTaskBoxAwardFail. Fail Code:" + value);
        //关闭宝箱领取界面
        ccUIManage.GetInstance().f_SendMsg(UINameConst.BoxGetSubPage, UIMessageDef.UI_CLOSE);
        f_UpdateTaskBox();
        UITool.f_OpenOrCloseWaitTip(false);
    }

    private void f_BackBtnClick(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TaskPage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
    }


    //成就任务Item更新
    private void AchievementTaskItemUpdateByInfo(Transform item, BasePoolDT<long> dt)
    {
        TaskAchvPoolDT node = (TaskAchvPoolDT)dt;
        TaskItem tUpdateItem = item.GetComponent<TaskItem>();
        tUpdateItem.f_UpdateByInfo(node);
        f_RegClickEvent(tUpdateItem.mGetAwardBtn, f_AchievementTaskAwardClick, node);
        f_RegClickEvent(tUpdateItem.mGotoBtn, f_AchvGotoClick, node);
    }

    /// <summary>
    /// 成就任务  前往按钮 点击
    /// </summary>
    private void f_AchvGotoClick(GameObject go, object value1, object value2)
    {
        TaskAchvPoolDT node = (TaskAchvPoolDT)value1;
        UITool.f_AchievementGoto(this,(EM_AchievementTaskCondition)node.mTemplate.iCondition);
    }

    private int mCurAchvAwardId;
    /// <summary>
    /// 领奖按钮点击
    /// </summary>
    /// <param name="go"></param>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    private void f_AchievementTaskAwardClick(GameObject go, object value1, object value2)
    {
        TaskAchvPoolDT node = (TaskAchvPoolDT)value1;
        mCurAchvAwardId = node.mTemplate.iAwardId;
        SocketCallbackDT callbackDT = new SocketCallbackDT();
        callbackDT.m_ccCallbackSuc = f_GetAchievementTaskAwardSuc;
        callbackDT.m_ccCallbackFail = f_GetAchievementTaskAwardFail;
        Data_Pool.m_TaskAchvPool.f_GetAchievementAward((byte)node.mTemplate.iCondition, callbackDT);
        UITool.f_OpenOrCloseWaitTip(true);
    }

    /// <summary>
    /// 成就任务领奖成功回调
    /// </summary>
    /// <param name="value"></param>
    private void f_GetAchievementTaskAwardSuc(object value)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        mAchievementTaskWrapComponent.f_UpdateView();
        //展示获得奖励
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN, new object[] { Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(mCurAchvAwardId), this });

        Data_Pool.m_TaskAchvPool.f_CheckRedPoint();//不需要联网（日常成就）
    }

    /// <summary>
    /// 成就任务领奖失败回调
    /// </summary>
    /// <param name="value"></param>
    private void f_GetAchievementTaskAwardFail(object value)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(1270) + value);
        MessageBox.ASSERT("AchievementTaskAwardFail. Fail Code:" + value);
    }

    


    

}
