using ccU3DEngine;
using System.Collections.Generic;
using UnityEngine;

//实现思路：
//1、战斗前，如果要触发剧情系统，则传入关卡id，，剧情系统将该关卡所有剧情存起来
//2、外界将所有可能触发的剧情参数通过消息派发，剧情管理器接收到消息参数，检测所有未进行过的剧情看是否触发效果类型，将所有触发的效果存进剧情队列中，并停止战斗
//3、如果有触发剧情，则一直检测剧情状态，如果是等待状态，则进行下一个剧情，，否则更新该剧情状态
//4、如果剧情结束，则恢复战斗

public class PlotSysManager : UIFramwork
{
    protected ccMachineManager mPlotMachineManger = null;               //剧情状态机
    private List<PlotDT>       mUnGoingPlotDtList;                      //未进行的关卡剧情数据
    private Queue<PlotDT>      mGoingOnPlotQueue;                       //进行中的剧情队列
    private bool               mIsGoingPlot = false;                    //是否在剧情中
    private bool               mIsPlotBeforFight = false;               //是否战斗前剧情  
    private PlotCheckParam     mPlotCheckParam;                         //当前检测中的剧情参数
    private bool[]             mIsHaveTypePlot;                         //是否有该触发类型的剧情（用以优化判断）

    //UI元素
    public  UILabel    mlabelTypingTxt;  //打字效果label
    private GameObject mobjSkipBtn;      //跳过按钮
    private string strTexBgRoot = "UI/TextureRemove/Shop/Tex_ShopBg";

    protected override void f_CustomAwake()
    {
        InitGUI();
        base.f_CustomAwake();
    }

    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_INIT_CHECKPOINT_PLOT, OnInitCheckpointPlot);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_PLOT_CHECK, OnPlotCheck);
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mIsGoingPlot = false;
        mIsPlotBeforFight = false;
        mUnGoingPlotDtList = new List<PlotDT>();
        mGoingOnPlotQueue = new Queue<PlotDT>();
        mPlotMachineManger = new ccMachineManager(new PlotWait(this));
        mPlotMachineManger.f_RegState(new PlotTextTyping(this));
        mPlotMachineManger.f_RegState(new PlotDialog(this));
        mPlotMachineManger.f_RegState(new PlotChangeFightRole(this));
        mPlotMachineManger.f_RegState(new PlotShowFightRole(this));
        mPlotMachineManger.f_RegState(new PlotArtAni(this));
        mPlotMachineManger.f_RegState(new PlotChangeFightRoleSkill(this));
        mPlotMachineManger.f_ChangeState((int)EM_PlotState.EM_PlotState_Wait);

        mobjSkipBtn = f_GetObject("SkipBtn");
        UIEventListener.Get(mobjSkipBtn).onClick = f_OnSkipPlot;
        mlabelTypingTxt.transform.parent.gameObject.SetActive(false);
        m_Panel.gameObject.SetActive(false);
        mIsHaveTypePlot = new bool[(int)EM_PlotTriggerType.Max];

        //加载背景图
        UITexture Tex_ShopBg = f_GetObject("Texture_BG").GetComponent<UITexture>();
        if (Tex_ShopBg.mainTexture == null)
        {
            Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot);
            Tex_ShopBg.mainTexture = tTexture2D;
        }
    }

    // Update is called once per frame
    void Update() {
        if (!mIsGoingPlot)
            return;

        if (null == mPlotMachineManger)
        {
            MessageBox.ASSERT("mPlotMachineManger Null!!!?");
            return;
        }

        //获取当前剧情状态
        ccMachineStateBase tccMachineStateBase = mPlotMachineManger.f_GetCurMachineState();
        if (null == tccMachineStateBase)
        {
MessageBox.ASSERT("The plot does not exist！！！？？");
            EndPlot();
            return;
        }

        if ((EM_PlotState)tccMachineStateBase.iId == EM_PlotState.EM_PlotState_Wait)
        {
            //开始下一个剧情
            if (mGoingOnPlotQueue.Count > 0)
            {
                PlotDT plotDt = mGoingOnPlotQueue.Dequeue();
                mobjSkipBtn.SetActive(plotDt.iTriggerEffect <= (int)EM_PlotState.EM_PlotState_Dialog);
                mPlotMachineManger.f_ChangeState(plotDt.iTriggerEffect, plotDt);
            }
            else
            {
                //剧情结束
                EndPlot();               
            }
        }
        mPlotMachineManger.f_Update();
    }

    void OnDestroy()
    {
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_INIT_CHECKPOINT_PLOT);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_PLOT_CHECK);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_CHANGE_FIGHT_ROLE_END);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_SHOW_FIGHT_ROLE_END);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_SET_FIGHT_ROLE_SKILL_END);
    }

    //除了打字效果，其他跳过剧情，直接跳过剧情战斗
    private void f_OnSkipPlot(GameObject obj)
    {
        //跳过当前播着的剧情
        ccMachineStateBase tccMachineStateBase = mPlotMachineManger.f_GetCurMachineState();
        if (null == tccMachineStateBase)
            return;
        bool isTypingEffect = (EM_PlotState)tccMachineStateBase.iId == EM_PlotState.EM_PlotState_TextTyping;
        tccMachineStateBase.f_SetComplete((int)EM_PlotState.EM_PlotState_Wait);

        //打字效果不处理
        if (isTypingEffect)
            return;

        //如果播着剧情，则把剧情关掉
        if (ccUIManage.GetInstance().f_CheckUIIsOpen(UINameConst.DialogPage))
            ccUIManage.GetInstance().f_SendMsg(UINameConst.DialogPage, UIMessageDef.UI_CLOSE);

        //跳过剧情和战斗
        m_Panel.gameObject.SetActive(false);
        mIsGoingPlot = false;
        mIsPlotBeforFight = false;
        mUnGoingPlotDtList.Clear();
        mGoingOnPlotQueue.Clear();
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_SHOWLASTTURN);
    }

    //初始化关卡剧情
    private void OnInitCheckpointPlot(object obj) {
        int checkpointId = StaticValue.m_CurBattleConfig.m_iTollgateId;
        mUnGoingPlotDtList.Clear();
        mGoingOnPlotQueue.Clear();
        mIsHaveTypePlot = new bool[(int)EM_PlotTriggerType.Max];

        SC_Pool scPool = glo_Main.GetInstance().m_SC_Pool;
        if (null == scPool)
        {
MessageBox.ASSERT("OnInitCheckpointPlot，Object group does not exist！！");
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_DIALOG_BATTLESTART);
            return;
        }
        PlotSC plotSC = scPool.m_PlotSC;
        if (null == plotSC)
        {
MessageBox.ASSERT("OnInitCheckpointPlot，Plot group does not exist！！");
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_DIALOG_BATTLESTART);
            return;
        }

        //根据关卡id缓存所有剧情数据
        List<NBaseSCDT> allPlots = plotSC.f_GetAll();
        for (int i = 0; i < allPlots.Count; i++)
        {
            PlotDT plotDt = (PlotDT)allPlots[i];
            if (plotDt.iCheckpointId != checkpointId)
            {
                continue;
            }

            //如果是战斗前的剧情直接加入队列中
            if (plotDt.iTriggerType == (int)EM_PlotTriggerType.PreplotEnd && plotDt.szTriggerParams == "0")
            {
                mGoingOnPlotQueue.Enqueue(plotDt);
                continue;
            }

            if (plotDt.iTriggerType >= (int)EM_PlotTriggerType.Max)
            {
MessageBox.ASSERT("Trigger does not exist！：" + plotDt.iTriggerType);
            }
            else
            {
                mIsHaveTypePlot[plotDt.iTriggerType] = true;
            }
            mUnGoingPlotDtList.Add(plotDt);
        }
        if (mUnGoingPlotDtList.Count <= 0 && mGoingOnPlotQueue.Count <= 0)
        {
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_DIALOG_BATTLESTART);
            return;
        }

        //开始剧情
        mIsPlotBeforFight = true;
        StartPlot();
    }

    //剧情检测,看是否触发剧情
    private void OnPlotCheck(object obj) {
        if (null == obj || !(obj is PlotCheckParam)) return;
        PlotCheckParam plotCheckParam = (PlotCheckParam)obj;

        if (plotCheckParam.triggerType >= EM_PlotTriggerType.Max || !mIsHaveTypePlot[(int)plotCheckParam.triggerType])
        {
            if (null != plotCheckParam.callback) plotCheckParam.callback(plotCheckParam.callbackData);
            return;
        }
       
        //检测战斗中触发的剧情       
        int triggerPlotId = 0;
        PlotDT plotDt = null;
        for (int i = 0; i < mUnGoingPlotDtList.Count; i++)
        {
            plotDt = mUnGoingPlotDtList[i];
            if (plotDt.iTriggerType != (int)plotCheckParam.triggerType)
            {
                continue;
            }

            //如果是战斗结束，直接满足条件
            if (plotCheckParam.triggerType == EM_PlotTriggerType.FightWin)
            {
                triggerPlotId = plotDt.iId;
                break;
            }

            //检测参数是否合法
            int[] nTriggerParmas = SplitTriggerParams(plotDt.szTriggerParams);
            if (nTriggerParmas.Length != plotCheckParam.triggerParams.Length)
            {
MessageBox.ASSERT("Trigger parameters and judgment parameters are inconsistent！！！Number of triggers: " + nTriggerParmas.Length +
                    ", Số lượng phán đoán:" + plotCheckParam.triggerParams.Length + ",Cốt truyện id：" + plotDt.iId);
                break;
            }

            //通过类型判断是否满足触发条件
            switch (plotCheckParam.triggerType)
            {
                case EM_PlotTriggerType.Round://检测回合数
                case EM_PlotTriggerType.FightRoleAction://指定回合某个阵营某个站位武将行动前或行动后
                case EM_PlotTriggerType.FightRoleSkill://某个阵营某个站位武将技能触发                   
                    triggerPlotId = plotDt.iId;
                    for (int j = 0; j < nTriggerParmas.Length; j++)
                    {
                        //所有条件相等
                        if (nTriggerParmas[j] != plotCheckParam.triggerParams[j])
                        {
                            triggerPlotId = 0;
                            break;
                        }
                    }
                    break;
                case EM_PlotTriggerType.FightRoleHp://指定某个阵营某个站位武将血量达到一定值
                    triggerPlotId = plotDt.iId;
                    for (int j = 0; j < nTriggerParmas.Length; j++)
                    {
                        //所有条件小于等于,如果大于则不满足
                        if (plotCheckParam.triggerParams[j] > nTriggerParmas[j])
                        {
                            triggerPlotId = 0;
                            break;
                        }
                    }
                    break;
                case EM_PlotTriggerType.FightRoleAnger://指定某个阵营某个站位武将怒气达到一定值
                    triggerPlotId = plotDt.iId;
                    for (int j = 0; j < nTriggerParmas.Length; j++)
                    {
                        //所有条件大于等于，如果小于则不满足
                        if (plotCheckParam.triggerParams[j] < nTriggerParmas[j])
                        {
                            triggerPlotId = 0;
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }

            //检测到符合条件剧情，退出循环
            if (triggerPlotId > 0) {                
                break;
            }
        }

        if (triggerPlotId <= 0)
        {
            //没有触发剧情
            if (null != plotCheckParam.callback) {
                plotCheckParam.callback(plotCheckParam.callbackData);                
            }
            return;
        }

        //判断上个剧情没结束的处理
        if (mIsGoingPlot) {
MessageBox.ASSERT("Previous plot not completed, plot type：" + mPlotCheckParam.triggerType + ", forced stop，open new，type：" + plotCheckParam.triggerType);
            mGoingOnPlotQueue.Clear();
            mIsGoingPlot = false;
        }
        mPlotCheckParam = plotCheckParam;

        //将符合条件的剧情加入队列中
        mGoingOnPlotQueue.Enqueue(plotDt);

        //再来检测战斗剧情后面连续的剧情
        List<PlotDT> unGoingPlotDtList = new List<PlotDT>();
        for (int i = 0; i < mUnGoingPlotDtList.Count; i++)
        {
            PlotDT _plotDt = mUnGoingPlotDtList[i];
            if (_plotDt.iId == triggerPlotId)
                continue;

            //过滤掉不是前置关联的剧情
            if (_plotDt.iTriggerType != (int)EM_PlotTriggerType.PreplotEnd)
            {
                unGoingPlotDtList.Add(_plotDt);
                continue;
            }
            if (_plotDt.szTriggerParams == "0")
            {
                //约定如果前置触发id为0则表示是战斗前的剧情
                continue;
            }

            //判断是否为触发的前置剧情
            int preTriggerId;
            int.TryParse(_plotDt.szTriggerParams, out preTriggerId);
            if (preTriggerId == triggerPlotId)
            {
                mGoingOnPlotQueue.Enqueue(_plotDt);
                triggerPlotId = _plotDt.iId;
            }
            else
            {
                unGoingPlotDtList.Add(_plotDt);
            }
        }
        mUnGoingPlotDtList = unGoingPlotDtList;

        //开始剧情
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.FIGHTPAUSE);
        StartPlot();
    }

    //开始剧情
    private void StartPlot()
    {
        mPlotMachineManger.f_ChangeState((int)EM_PlotState.EM_PlotState_Wait);
        mIsGoingPlot = true;
        m_Panel.gameObject.SetActive(true);
    }

    //剧情结束
    private void EndPlot()
    {
        mIsGoingPlot = false;
        if (mIsPlotBeforFight)
        {
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_DIALOG_BATTLESTART);
        }
        else
        {
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.FIGHTRESUME);
        }
        mIsPlotBeforFight = false;
        m_Panel.gameObject.SetActive(false);
        if (null == mPlotCheckParam) {
            //战斗前剧情主动触发，空位正常情况
            //MessageBox.ASSERT("剧情结束，但是剧情参数为空！！！？");
            return;
        }
        if (null == mPlotCheckParam.callback) return;
        mPlotCheckParam.callback(mPlotCheckParam.callbackData);
        mPlotCheckParam = null;       
    }

    //拆分触发参数
    private int[] SplitTriggerParams(string szTriggerParams)
    {
        string[] szParams = szTriggerParams.Split(';');
        List<int> ParamList = new List<int>();
        try
        {
            for (int i = 0; i < szParams.Length; i++)
            {
                if (szParams[i] == "")
                    continue;
                int param = int.Parse(szParams[i]);
                ParamList.Add(param);
            }
        }
        catch (System.Exception e)
        {
MessageBox.ASSERT("Unusual trigger parameter configuration ：" + szTriggerParams + ",err:" + e.Message);
        }
        return ParamList.ToArray();
    }
}

/// <summary>
/// 剧情检测参数
/// </summary>
public class PlotCheckParam
{
    public EM_PlotTriggerType triggerType;
    public int[] triggerParams;
    public ccCallback callback;
    public object callbackData;
}
