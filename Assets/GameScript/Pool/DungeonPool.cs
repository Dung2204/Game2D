using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Core;
using UnityEngine;

public class DungeonPool : BasePool
{
    private bool mIsHavePlot = false;//关卡是否有剧情

    public DungeonPool() : base("DungeonPoolDT", false)
    {

    }

    protected override void f_Init()
    {
        List<NBaseSCDT> tValues = glo_Main.GetInstance().m_SC_Pool.m_DungeonChapterSC.f_GetAll();
        //为章节添加索引 从0开始
        mChapterMaxIdx.Clear();
        mChapeterNames.Clear();
        for (int i = 0; i < tValues.Count; i++)
        {
            DungeonChapterDT node = (DungeonChapterDT)tValues[i];
            if (mChapterMaxIdx.ContainsKey(node.iChapterType))
            {
                mChapterMaxIdx[node.iChapterType]++;
                mChapeterNames[node.iChapterType].Add(node.szChapterName);
            }
            else
            {
                mChapterMaxIdx.Add(node.iChapterType, 0);
                BetterList<string> tTypeChapterNameList = new BetterList<string>();
                tTypeChapterNameList.Add(node.szChapterName);
                mChapeterNames.Add(node.iChapterType, tTypeChapterNameList);
            }
            DungeonPoolDT dt = new DungeonPoolDT(mChapterMaxIdx[node.iChapterType]);
            dt.iId = tValues[i].iId;
            dt.m_iChapterTemplateId = tValues[i].iId;
            f_CheckChapterPassIdx(dt);
            f_Save(dt);
        }

        mDungeonLegendTimes = 0;
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.TheNextDay, f_UpdateDataByNextDay);
    }

    /// <summary>
    /// 副本,跨天处理
    /// </summary>
    public void f_UpdateDataByNextDay(object value)
    {
        List<BasePoolDT<long>> tList = f_GetAll();
        DungeonPoolDT tItem;
        for (int i = 0; i < tList.Count; i++)
        {
            tItem = (DungeonPoolDT)tList[i];
            tItem.f_ResetTollgateData();
        }
        mDungeonLegendTimes = 0;
        //跨天UI事件  通知关心此消息的UI更新
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_THENEXTDAY_UIPROCESS, EM_NextDaySource.DungeonPool);
    }

    public float m_DungeonStartExp;

    //Idx最大值
    private Dictionary<int, int> mChapterMaxIdx = new Dictionary<int, int>();

    //每个类型 对应章节Idx的章节名字
    private Dictionary<int, BetterList<string>> mChapeterNames = new Dictionary<int, BetterList<string>>();

    /// <summary>
    /// 获得对应副本类型，对应副本Idx的章节名字
    /// </summary>
    /// <param name="dungeonType">副本类型</param>
    /// <param name="chapterIdx">副本章节Idx</param>
    /// <returns></returns>
    public string f_GetChapterName(int dungeonType, int chapterIdx)
    {
        if (mChapeterNames.ContainsKey(dungeonType))
        {
            if (chapterIdx >= 0 && chapterIdx < mChapeterNames[dungeonType].size)
                return mChapeterNames[dungeonType][chapterIdx];
            else
                return string.Empty;
        }
        else
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// 转换副本选项卡数据（将老的count条选项卡数据合并为一个选项卡数据）
    /// </summary>
    /// <param name="allDungeonPoolDTs">章节数据数组</param>
    /// <param name="count">新的一条选项卡需要几条老的选项卡数量（万恶的名将副本每页不等长，所以后面改为数组）</param>
    /// <returns></returns>
    public List<BasePoolDT<long>> f_AllDungeonPoolDTs2DungeonPoolDTOfPage(List<BasePoolDT<long>> allDungeonPoolDTs, int[] count)
    {
        DungeonPoolDTOfPage pageData = null;
        List<BasePoolDT<long>> pageDataList = new List<BasePoolDT<long>>();
        for (int i = 0; i < allDungeonPoolDTs.Count;)
        {

            bool isEnd = false;
            for (int j = 0; j < count.Length; j++)
            {
                int pageCount = count[j];
                pageData = new DungeonPoolDTOfPage();
                pageData.DungeonPoolDTList = new BasePoolDT<long>[pageCount];
                pageDataList.Add(pageData);
                for (int k = 0; k < pageCount; k++)
                {
                    if (i >= allDungeonPoolDTs.Count)
                    {
                        isEnd = true;
                        break;
                    }
                    DungeonPoolDT dungeonPoolDT = (DungeonPoolDT)allDungeonPoolDTs[i];
                    i++;
                    if (null == dungeonPoolDT) continue;
                    int chapterPassIdx = mChapterPassIdx[dungeonPoolDT.m_ChapterTemplate.iChapterType] + 2;
                    if (dungeonPoolDT.mIndex > chapterPassIdx)
                    {
                        isEnd = true;
                        break;
                    }
                    pageData.DungeonPoolDTList[k] = dungeonPoolDT;
                }
                if (isEnd) break;
            }
            if (isEnd) break;
        }
        return pageDataList;
    }

    /// <summary>
    /// 记录每种类型单前通关章节的Index = chapterIdx +1
    /// </summary>
    private Dictionary<int, int> mChapterPassIdx = new Dictionary<int, int>();
    /// <summary>
    /// 检查每种类型通关章节Index
    /// </summary>
    private void f_CheckChapterPassIdx(DungeonPoolDT dt)
    {
        if (mChapterPassIdx.ContainsKey(dt.m_ChapterTemplate.iChapterType))
        {
            if (dt.mTollgatePassNum >= dt.mTollgateMaxNum)
            {
                if (dt.mIndex >= mChapterPassIdx[dt.m_ChapterTemplate.iChapterType])
                {
                    mChapterPassIdx[dt.m_ChapterTemplate.iChapterType] = dt.mIndex + 1;
                    //精英关卡通过章节
                    if (dt.m_ChapterTemplate.iChapterType == (int)EM_Fight_Enum.eFight_DungeonElite)
                    {
                        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.TaskAchvUpdateProgress,
                            new int[] { (int)EM_AchievementTaskCondition.eAchv_EliteChapters, dt.mIndex + 1 });
                    }
                }

            }
            else if (dt.mTollgatePassNum > 0)
            {
                if (dt.m_ChapterTemplate.iChapterType == (int)EM_Fight_Enum.eFight_Legend)
                {
                    //这个特殊处理。。。  名将副本大魔王关卡 直接开启下一关。。。
                    if (dt.mIndex >= mChapterPassIdx[dt.m_ChapterTemplate.iChapterType] && dt.mTollgatePassNum >= GameParamConst.LegendDungeonRestIdx)
                    {
                        mChapterPassIdx[dt.m_ChapterTemplate.iChapterType] = dt.mIndex + 1;
                    }
                    return;
                }
                if (dt.mIndex >= mChapterPassIdx[dt.m_ChapterTemplate.iChapterType])
                {
                    mChapterPassIdx[dt.m_ChapterTemplate.iChapterType] = dt.mIndex;
                    //精英关卡通过章节
                    if (dt.m_ChapterTemplate.iChapterType == (int)EM_Fight_Enum.eFight_DungeonElite)
                    {
                        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.TaskAchvUpdateProgress,
                            new int[] { (int)EM_AchievementTaskCondition.eAchv_EliteChapters, dt.mIndex });
                    }
                }
            }
        }
        else
        {
            mChapterPassIdx.Add(dt.m_ChapterTemplate.iChapterType, dt.mIndex);
        }

    }

    public int f_GetChapterPassIdx(EM_Fight_Enum eM_Fight_Enum)
    {
        return mChapterPassIdx[(int)eM_Fight_Enum];
    }
    /// <summary>
    /// 检查副本是否开启
    /// </summary>
    public bool f_CheckChapterLockState(DungeonPoolDT dt, bool showTip = false)
    {
        if (dt.m_ChapterTemplate.iChapterType == (int)EM_Fight_Enum.eFight_DungeonMain)
        {
            if (dt.mIndex > mChapterPassIdx[dt.m_ChapterTemplate.iChapterType])
            {
                if (!showTip)
                    return true;
UITool.Ui_Trip(string.Format("Chưa qua chương {0}： {1}", dt.mIndex, f_GetChapterName(dt.m_ChapterTemplate.iChapterType, dt.mIndex - 1)));
                return true;
            }
            else
            {
                return false;
            }
        }
        //精英副本和名将副本处理相同
        else if (dt.m_ChapterTemplate.iChapterType == (int)EM_Fight_Enum.eFight_DungeonElite || dt.m_ChapterTemplate.iChapterType == (int)EM_Fight_Enum.eFight_Legend)
        {
            if (dt.mIndex >= mChapterPassIdx[(int)EM_Fight_Enum.eFight_DungeonMain])
            {
                if (!showTip)
                    return true;
UITool.Ui_Trip(string.Format("Chưa qua chương {0} ： {1}", dt.mIndex + 1, f_GetChapterName((int)EM_Fight_Enum.eFight_DungeonMain, dt.mIndex)));
                return true;
            }
            else if (dt.mIndex > mChapterPassIdx[dt.m_ChapterTemplate.iChapterType])
            {
                if (!showTip)
                    return true;
UITool.Ui_Trip(string.Format("Chưa qua chương {0}： {1}", dt.mIndex, f_GetChapterName(dt.m_ChapterTemplate.iChapterType, dt.mIndex - 1)));
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
MessageBox.ASSERT("EM_DungeonType is not processed");
            return true;
        }
    }

    /// <summary>
    /// 检测副本 当前战斗副本
    /// </summary>
    public bool f_CheckIsFightChapter(DungeonPoolDT dt)
    {
        if (dt.m_ChapterTemplate.iChapterType == (int)EM_Fight_Enum.eFight_DungeonMain)
        {
            return dt.mIndex == mChapterPassIdx[dt.m_ChapterTemplate.iChapterType];
        }
        else if (dt.m_ChapterTemplate.iChapterType == (int)EM_Fight_Enum.eFight_DungeonElite || dt.m_ChapterTemplate.iChapterType == (int)EM_Fight_Enum.eFight_Legend)
        {
            return dt.mIndex == mChapterPassIdx[dt.m_ChapterTemplate.iChapterType] && dt.mIndex < mChapterPassIdx[(int)EM_Fight_Enum.eFight_DungeonMain];
        }
        else
        {
MessageBox.ASSERT("EM_DungeonType is not processed");
            return false;
        }
    }

    /// <summary>
    /// 获取目前通关之后没有满星的关卡
    /// </summary>
    /// <param name="chapterType">副本类型</param>
    /// <returns></returns>
    public List<DungeonTollgatePoolDT> f_GetAllNoMaxStar(int chapterType)
    {
        List<DungeonTollgatePoolDT> backDT = new List<DungeonTollgatePoolDT>();
        List<BasePoolDT<long>> tList = f_GetAllForData1(chapterType);
        for (int i = 0; i < tList.Count; i++)
        {
            DungeonPoolDT dt = tList[i] as DungeonPoolDT;
            for (int j = 0; j < dt.mTollgatePassNum; j++)
            {
                if (dt.mTollgateList[j].mStarNum < 3)
                {
                    backDT.Add(dt.mTollgateList[j]);
                }
            }
        }
        return backDT;
    }


    public int f_GetFightChapterIdx(int chapterType, int chapterIdx = 0)
    {
        int tResult = 0;
        List<BasePoolDT<long>> tList = f_GetAllForData1(chapterType);
        int tPassIdx = mChapterPassIdx[chapterType];
        if (chapterIdx != 0)
            tPassIdx = chapterIdx;
        int tChapMaxIdx = mChapterMaxIdx[chapterType];
        if (chapterType == (int)EM_Fight_Enum.eFight_DungeonMain)
        {
            tResult = System.Math.Min(tPassIdx, tChapMaxIdx);
        }
        else
        {
            int tMainLinePassIdx = System.Math.Max(0, mChapterPassIdx[(int)EM_Fight_Enum.eFight_DungeonMain] - 1);
            tResult = System.Math.Min(tMainLinePassIdx, tPassIdx);
            tResult = System.Math.Min(tResult, tChapMaxIdx);
        }
        return tResult;
    }

    /// <summary>
    /// 检测关卡 是否是单前战斗关卡
    /// </summary>
    public bool f_CheckIsFightTollgate(int chapterId, int tollgateId)
    {
        DungeonPoolDT chapterDT = (DungeonPoolDT)f_GetForId(chapterId);
        if (chapterDT == null)
        {
            return false;
        }
        DungeonTollgatePoolDT tollgateDT = chapterDT.f_GetTollgateData(tollgateId);
        if (tollgateDT == null)
            return false;
        bool tChapterFight = f_CheckIsFightChapter(chapterDT);
        if (tChapterFight && tollgateDT.mIndex == chapterDT.mTollgatePassNum)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 根据章节ID 获取 副本类型
    /// </summary>
    /// <param name="chapterId"></param>
    /// <returns></returns>
    public int f_GetDungeonTypeByChapterId(int chapterId)
    {
        DungeonPoolDT chapterDT = (DungeonPoolDT)f_GetForId(chapterId);
        return chapterDT.m_ChapterTemplate.iChapterType;
    }
    public DungeonTollgatePoolDT f_GetDungeonTollgateForID(int TollgateID)
    {
        DungeonTollgateDT tTollgate = glo_Main.GetInstance().m_SC_Pool.m_DungeonTollgateSC.f_GetSC(TollgateID) as DungeonTollgateDT;

        DungeonPoolDT dungeonDT = f_GetForId(tTollgate.iDungeonChapter) as DungeonPoolDT;

        DungeonTollgatePoolDT dungeonTollgateDT = dungeonDT.f_GetTollgateData(TollgateID);
        return dungeonTollgateDT;
    }

    /// <summary>
    /// 获取副本能扫荡次数
    /// </summary>
    public int f_GetSweepCount(DungeonTollgatePoolDT dt)
    {
        int tEnergyValue = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Energy);
        int tNum = tEnergyValue / dt.mTollgateTemplate.iEnergyCost;
        tNum = System.Math.Min(tNum, dt.mTollgateTemplate.iCountLimit - dt.mTimes);
        return System.Math.Min(tNum, GameParamConst.TollgateSweepMaxNum);
    }

    private int _sweepVipLvLimit = -99;
    public int mSweepVipLvLimit
    {
        get
        {
            if (_sweepVipLvLimit == -99)
            {
                _sweepVipLvLimit = Data_Pool.m_RechargePool.f_SweepVipLvLimit();
            }
            return _sweepVipLvLimit;
        }
    }

    /// <summary>
    /// 获取某种类型副本的总星数
    /// </summary>
    /// <param name="eType"></param>
    /// <returns></returns>
    public int f_GetStarNumByType(EM_Fight_Enum eType)
    {
        int result = 0;
        List<BasePoolDT<long>> tList = f_GetAllForData1((int)eType);
        for (int i = 0; i < tList.Count; i++)
        {
            DungeonPoolDT tItem = (DungeonPoolDT)tList[i];
            result += tItem.mStarNum;
        }
        return result;
    }

    /// <summary>
    /// 判断关卡是否有剧情
    /// </summary>
    /// <returns></returns>
    public bool f_JudgeIsHavePlot()
    {
        EM_Fight_Enum fightType = StaticValue.m_CurBattleConfig.m_eBattleType;
        bool isDup = fightType == EM_Fight_Enum.eFight_DungeonMain || fightType == EM_Fight_Enum.eFight_DungeonElite || fightType == EM_Fight_Enum.eFight_Legend;
        return mIsHavePlot && isDup && GloData.glo_StarGuidance;
    }

    /// <summary>
    /// 更新是否有剧情
    /// </summary>
    public void f_UpdateIsHavePlot()
    {
        EM_Fight_Enum fightType = StaticValue.m_CurBattleConfig.m_eBattleType;
        if (fightType != EM_Fight_Enum.eFight_DungeonMain
            && fightType != EM_Fight_Enum.eFight_DungeonElite
            && fightType != EM_Fight_Enum.eFight_Legend
            )
        {
            return;
        }
        mIsHavePlot = false;

        //先判断是否为当前最新调整关卡，如果不是则没有剧情
        int tollgateId = StaticValue.m_CurBattleConfig.m_iTollgateId;
        bool tIsFightTollgate = Data_Pool.m_DungeonPool.f_CheckIsFightTollgate(StaticValue.m_CurBattleConfig.m_iChapterId, tollgateId);
        if (!tIsFightTollgate && tollgateId != GameParamConst.PLOT_TOLLGATEID)
        {
            return;
        }

        //如果打过则没有剧情        
        int tId = PlayerPrefs.GetInt(string.Format(GameParamConst.DialogTollgateIdx, Data_Pool.m_UserData.m_iUserId, (int)fightType), -1);
        if (tId == tollgateId)
            return;

        //从剧情表中判断是否有剧情
        List<NBaseSCDT> plotList = glo_Main.GetInstance().m_SC_Pool.m_PlotSC.f_GetAll();
        for (int i = 0; i < plotList.Count; i++)
        {
            PlotDT plotDt = plotList[i] as PlotDT;
            if (null == plotDt)
                continue;
            if (plotDt.iCheckpointId == tollgateId)
            {
                mIsHavePlot = true;
                break;
            }
        }
    }

    /// <summary>
    /// 获取战斗需隐藏的角色数据（战斗剧情控制显示角色）
    /// 隐藏角色数据，位处理，我方1-6，敌方7-12，，如果哪位要隐藏则该位置1
    /// </summary>
    /// <returns></returns>
    public int f_GetHideFightRoleData()
    {
        int hideFightRoleData = 0;

        //先判断是否要播放剧情       
        if (!f_JudgeIsHavePlot())
        {
            return hideFightRoleData;
        }

        //从剧情表中获取隐藏角色数据
        int tollgateId = StaticValue.m_CurBattleConfig.m_iTollgateId;
        List<NBaseSCDT> plotList = glo_Main.GetInstance().m_SC_Pool.m_PlotSC.f_GetAll();
        for (int i = 0; i < plotList.Count; i++)
        {
            //遍历该关卡所有要隐藏角色的剧情数据
            PlotDT plotDt = plotList[i] as PlotDT;
            if (null == plotDt)
                continue;
            if (plotDt.iCheckpointId != tollgateId)
                continue;
            if (plotDt.iTriggerEffect != (int)EM_PlotState.EM_PlotState_ShowFightRole)
                continue;

            //分隔多个待显示角色数据
            string[] szShowFightRoleParams = plotDt.szEffectParams.Split('^');
            for (int j = 0; j < szShowFightRoleParams.Length; j++)
            {
                string szShowFightRoleParam = szShowFightRoleParams[j];
                if (szShowFightRoleParam == "")
                    continue;

                //分隔一个待显示角色数据
                string[] szShowFightRole = szShowFightRoleParam.Split(';');
                if (szShowFightRole.Length < 2)
                {
MessageBox.ASSERT("Plot is not normal，plot id：" + plotDt.iId);
                    continue;
                }

                try
                {
                    int camp = int.Parse(szShowFightRole[0]);
                    int standIndex = int.Parse(szShowFightRole[1]);
                    int fightIndex = camp * 6 + standIndex + 1;
                    hideFightRoleData = BitTool.BitSet(hideFightRoleData, (ushort)fightIndex);
                }
                catch (System.Exception e)
                {
MessageBox.ASSERT("Plot is not normal，plot id：：" + plotDt.iId + ",error：" + e.Message);
                    break;
                }
            }
        }
        return hideFightRoleData;
    }

    protected override void RegSocketMessage()
    {
        //添加章节数据
        SC_DungeonChapterInfo tSC_DungeonChapterInfo = new SC_DungeonChapterInfo();
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_DungeonChapter, tSC_DungeonChapterInfo, Callback_SocketData_Update);

        //添加关卡数据 
        SC_DungeonTollgateInfo tSC_DungeonTollgateInfo = new SC_DungeonTollgateInfo();
        GameSocket.GetInstance().f_RegMessage_Int2((int)SocketCommand.SC_DungeonTollgate, tSC_DungeonTollgateInfo, Callback_SocketData_TollgateUpdate);

        //结算数据
        SC_DungeonFinishInfo tSC_DungeonFinishInfo = new SC_DungeonFinishInfo();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_DungeonFinish, tSC_DungeonFinishInfo, Callback_SocketData_DungeonFinish);

        //名将副本相关
        basicNode1 tSC_DungeonLegend = new basicNode1();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_DungeonLegend, tSC_DungeonLegend, f_Callback_SocketData_DungeonLegend);


        //一键领取章节奖励
        SC_DungeonAwardChanged tSC_DungeonAwardChanged = new SC_DungeonAwardChanged();
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_DungeonAwardChanged, tSC_DungeonAwardChanged, Callback_SocketData_DungeonAwardChanged);

    }

    #region MyRegion

    /// <summary>
    /// 主线通关章节
    /// </summary>
    public int m_DungeonMainMax;
    /// <summary>
    /// 精英通关章节
    /// </summary>
    public int m_DungeonEliteMax;
    /// <summary>
    /// 已通关的主线ID
    /// </summary>
    public int m_YetDungeonMainId;
    /// <summary>
    /// 已通关的精英ID
    /// </summary>
    public int m_YetDungeonEliteId;
    /// <summary>
    /// 名将副本挑战次数
    /// </summary>
    public int mDungeonLegendTimes
    {
        get;
        private set;
    }

    /// <summary>
    /// 名将副本挑战限制次数
    /// </summary>
    public int mDungeonLegendLimit
    {
        get
        {
            return Data_Pool.m_RechargePool.f_GetCurLvVipPriValue(EM_VipPrivilege.eVip_FamousGeneral);
        }
    }

    /// <summary>
    /// 名将副本剩余次数
    /// </summary>
    public int mDungeonLegendLeftTimes
    {
        get
        {
            return mDungeonLegendLimit - mDungeonLegendTimes;
        }
    }

    /// <summary>
    /// 处理名将副本次数
    /// </summary>
    /// <param name="value"></param>
    private void f_Callback_SocketData_DungeonLegend(object value)
    {
        basicNode1 tServerData = (basicNode1)value;
        mDungeonLegendTimes = tServerData.value1;
        f_CheckLegionRedDot();
    }
    public void f_CheckLegionRedDot()
    {
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.LegendHavaTimes);
        int Lv = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        bool isOpen = Lv >= UITool.f_GetSysOpenLevel(EM_NeedLevel.LegendLevel);
        if (!isOpen)
        {
            return;
        }
        List<BasePoolDT<long>> listTemp = Data_Pool.m_DungeonPool.f_GetAllForData1((int)EM_Fight_Enum.eFight_Legend);
        for (int i = 0; i < listTemp.Count; i++)
        {
            DungeonPoolDT poolDT = listTemp[i] as DungeonPoolDT;
            bool bLock = Data_Pool.m_DungeonPool.f_CheckChapterLockState(poolDT);
            if (!bLock)//已经开放
            {
                bool isCanGetBox = Data_Pool.m_DungeonPool.f_CheckHasBoxCanGet(poolDT);
                if (isCanGetBox || mDungeonLegendTimes < mDungeonLegendLimit)//有挑战次数或者有宝箱
                {
                    Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.LegendHavaTimes);
                    break;
                }
            }
        }

    }
    #endregion

    #region 结算相关

    /// <summary>
    /// 是否已结算
    /// </summary>
    /// <returns></returns>
    public bool f_DungeonIsFinish()
    {
        return _bDungeonFinish;
    }
    private bool _bDungeonFinish = false;

    /// <summary>
    /// 结算相关信息
    /// </summary>
    public DungeonFinishInfo mDungeonFinishInfo;

    protected void Callback_SocketData_DungeonFinish(object result)
    {
        SC_DungeonFinishInfo node = (SC_DungeonFinishInfo)result;
        if (mDungeonFinishInfo == null)
            mDungeonFinishInfo = new DungeonFinishInfo();
        mDungeonFinishInfo.f_UpdateInfo(StaticValue.m_CurBattleConfig.m_iTollgateId, node.starNum, node.isFirstWin);
        Data_Pool.m_GuidancePool.m_OtherSave = true;
        _bDungeonFinish = true;
        f_Battle2MenuProcessParamByRet(node.starNum, node.isFirstWin);
        //更新对话数据
        if (node.isFirstWin > 0)
            Data_Pool.m_DialogPool.f_UpdateDialogDataByFirstWin((int)StaticValue.m_CurBattleConfig.m_eBattleType, StaticValue.m_CurBattleConfig.m_iTollgateId);
    }

    /// <summary>
    /// 根据战斗结果更新处理参数
    /// </summary>
    /// <param name="starNum"></param>
    /// <param name="isFirstWin"></param>
    private void f_Battle2MenuProcessParamByRet(int starNum, byte isFirstWin)
    {
        EM_Fight_Enum fightType = StaticValue.m_CurBattleConfig.m_eBattleType;
        int chapterId = StaticValue.m_CurBattleConfig.m_iChapterId;
        int tollgateId = StaticValue.m_CurBattleConfig.m_iTollgateId;
        if (isFirstWin > 0 && starNum > 0)
        {
            DungeonPoolDT chapterPoolDt = (DungeonPoolDT)f_GetForId(chapterId);
            DungeonTollgatePoolDT tollgatePoolDt = chapterPoolDt.f_GetTollgateData(tollgateId);
            if (tollgatePoolDt.mIndex < chapterPoolDt.mTollgateMaxNum - 1)
            {
                tollgateId = chapterPoolDt.mTollgateList[tollgatePoolDt.mIndex + 1].mTollgateId;
            }
            else
            {
                DungeonPoolDT nextChapterPoolDt = (DungeonPoolDT)f_GetForId(chapterId + 1);
                if (nextChapterPoolDt != null && !f_CheckChapterLockState(nextChapterPoolDt))
                {
                    chapterId++;
                    tollgateId = nextChapterPoolDt.mTollgateList[0].mTollgateId;
                }
            }
        }
        StaticValue.m_Battle2MenuProcessParam.f_UpdateParam(EM_Battle2MenuProcess.Dungeon, fightType, chapterId, tollgateId);
    }

    /// <summary>
    /// 叛军结算信息
    /// </summary>
    public string UIName;
    /// <summary>
    /// 叛军结算
    /// </summary>
    /// <param name="obj"></param>

    #endregion


    #region 关卡数据相关处理

    protected void Callback_SocketData_TollgateUpdate(int iData1, int iData2, int iNum, ArrayList aData)
    {
        foreach (SockBaseDT tData in aData)
        {
            if (iData1 == (int)eUpdateNodeType.node_add)
            {
                f_Socket_TollgateAddData(tData, iData2, true);
            }
            else if (iData1 == (int)eUpdateNodeType.node_update)
            {
                f_Socket_TollgateUpdateData(tData, iData2);
            }
            else if (iData1 == (int)eUpdateNodeType.node_default)
            {
                f_Socket_TollgateAddData(tData, iData2, false);
            }
        }
        DungeonPoolDT dt = (DungeonPoolDT)f_GetForId(iData2);
        if (dt == null)
        {
MessageBox.ASSERT(string.Format("DungeonPool initiates chapter update，ChapterId:{0}", iData2));
            return;
        }
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_DUNGEON_CHAPTER_UPDATE);
    }

    private void f_Socket_TollgateAddData(SockBaseDT node, int chapterId, bool bNew)
    {
        MessageBox.ASSERT("DungeonPool Tollgate NodeType.Add or default is Disable");
    }

    private void f_Socket_TollgateUpdateData(SockBaseDT node, int chapterId)
    {
        SC_DungeonTollgateInfo tServerData = (SC_DungeonTollgateInfo)node;
        DungeonPoolDT dt = (DungeonPoolDT)f_GetForId(chapterId);
        if (dt == null)
        {
MessageBox.ASSERT(string.Format("DungeonPool update failed，ChapterId:{0} TollgateId:{1}", chapterId, tServerData.uTollgateId));
            return;
        }
        //更新操作
        dt.f_UpdateTollgateInfo(tServerData.uTollgateId, tServerData.uStars, tServerData.boxTimes, tServerData.times, tServerData.resetTimes);
    }

    #endregion


    #region 章节数据相关处理

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
        MessageBox.ASSERT("DungeonPool Chapter NodeType.Add or default is Disable");
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
        SC_DungeonChapterInfo tServerData = (SC_DungeonChapterInfo)Obj;
        DungeonPoolDT dt = (DungeonPoolDT)f_GetForId(tServerData.uChapterId);
        if (dt == null)
        {
MessageBox.ASSERT(string.Format("DungeonPool Chapter Update Failed，ChapterId:{0}", tServerData.uChapterId));
            return;
        }
        dt.f_UpdateInfo(tServerData.uTollgates, tServerData.uToBoxGetNum, tServerData.uStars, tServerData.uBoxFlag);

        f_CheckChapterPassIdx(dt);
        if (dt.m_ChapterTemplate.iChapterType == (int)EM_Fight_Enum.eFight_DungeonMain)
        {
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.TaskAchvUpdateProgress, new int[] { (int)EM_AchievementTaskCondition.eAchv_MainTollgateStars, f_GetStarNumByType(EM_Fight_Enum.eFight_DungeonMain) });
            m_DungeonMainMax = dt.mIndex;

        }
        else if (dt.m_ChapterTemplate.iChapterType == (int)EM_Fight_Enum.eFight_DungeonElite)
        {
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.TaskAchvUpdateProgress, new int[] { (int)EM_AchievementTaskCondition.eAchv_EliteTollgateStars, f_GetStarNumByType(EM_Fight_Enum.eFight_DungeonElite) });
            m_DungeonEliteMax = dt.mIndex;
        }
    }

    private List<AwardPoolDT> _awardList1 = new List<AwardPoolDT>();

    //一键领取关卡和星数奖励回调
    protected void Callback_SocketData_DungeonAwardChanged(int iData1, int iData2, int iNum, ArrayList aData)
    {
        List<int> rewardId = new List<int>();
        //合并所有奖励并展示
        for (int i = 0; i < aData.Count; i++)
        {
            SC_DungeonAwardChanged item = (SC_DungeonAwardChanged)aData[i];

            //获取领取的关卡奖励
            DungeonPoolDT chapterDT = (DungeonPoolDT)f_GetForId(item.uChapterId);
            int[] toPos = BitTool.GetIndexOfBit1(item.uTollgateBoxTimes);
            int[] chapterBoxPos = BitTool.GetIndexOfBit1(item.uBoxFlag);
            chapterDT.f_UpdateBoxInfo(toPos.Length, chapterBoxPos);
            for (int j = 0; j < chapterBoxPos.Length; j++)
            {
                if (chapterBoxPos[j] == 1) rewardId.Add(chapterDT.m_ChapterTemplate.iBox1);
                else if (chapterBoxPos[j] == 2) rewardId.Add(chapterDT.m_ChapterTemplate.iBox2);
                else if (chapterBoxPos[j] == 3) rewardId.Add(chapterDT.m_ChapterTemplate.iBox3);
            }

            for (int j = 0; j < toPos.Length; j++)
            {
                chapterDT.mTollgateList[toPos[j] - 1].f_UpdateBoxInfo(1);
                int boxId = chapterDT.mTollgateList[toPos[j] - 1].mTollgateTemplate.iBoxId;
                if (boxId > 0)
                {
                    rewardId.Add(boxId);
                }
            }
        }

        _awardList1 = Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardIdArray(rewardId);

        //修改奖励内存数据

        //刷新界面
        //glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_DUNGEON_CHAPTER_ONEKEYGETREWARD_SUC);
    }

    #endregion


    #region 发送协议接口

    /// <summary>
    /// 请求副本章节数据（暂不用）
    /// </summary>
    public void f_DungeonChapter(SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_DungeonChapter, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_DungeonChapter, bBuf);
    }

    /// <summary>
    /// 请求某个章节的关卡数据
    /// </summary>
    /// <param name="chapterId">章节Id</param>
    public void f_DungeonTollgate(int chapterId, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_DungeonTollgate, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(chapterId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_DungeonTollgate, bBuf);
    }

    /// <summary>
    /// 请求挑战某个关卡
    /// </summary>
    /// <param name="tollgateId">关卡Id</param>
    public void f_DungeonChallenge(DungeonTollgatePoolDT tDungeonTollgatePoolDT, SocketCallbackDT tSocketCallbackDT)
    {
        //更新战斗数据
        DungeonPoolDT dungeonPoolDt = f_GetForId(tDungeonTollgatePoolDT.mChapterId) as DungeonPoolDT;
        StaticValue.m_CurBattleConfig.f_UpdateInfo((EM_Fight_Enum)tDungeonTollgatePoolDT.mChapterType, tDungeonTollgatePoolDT.mChapterId,
            tDungeonTollgatePoolDT.mTollgateId, dungeonPoolDt.m_ChapterTemplate.iBattleSceneMap);

        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_DungeonChallenge, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(tDungeonTollgatePoolDT.mTollgateId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_DungeonChallenge, bBuf);
        _bDungeonFinish = false;
    }

    /// <summary>
    /// 请求扫荡副本
    /// </summary>
    /// <param name="tollgateId">副本Id</param>
    /// <param name="times">扫荡次数</param>
    public void f_DungeonSweep(int tollgateId, byte times, SocketCallbackDT tSocketCallbackDT)
    {
        f_InitSweepData(tollgateId, times);
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_DungeonSweep, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(tollgateId);
        tCreateSocketBuf.f_Add(times);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_DungeonSweep, bBuf);
    }
    /// <summary>
    /// 主线宝鉴一键扫荡
    /// </summary>
    /// <param name="chapterType"></param>
    /// <param name="times"></param>
    /// <param name="cardId"></param>
    /// <param name="autoUseItem"></param>
    /// <param name="tSocketCallbackDT"></param>
    public void f_MainLineOneKeySweep(int tollgateId, int chapterType, int times, int cardId, int autoUseItem, SocketCallbackDT tSocketCallbackDT)
    {
        f_InitSweepData(tollgateId, times);
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_DungeonSweepAll, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add((byte)chapterType);
        tCreateSocketBuf.f_Add((byte)times);
        tCreateSocketBuf.f_Add(cardId);
        tCreateSocketBuf.f_Add((byte)autoUseItem);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_DungeonSweepAll, bBuf);
    }
    /// <summary>
    /// 请求重置关卡次数
    /// </summary>
    /// <param name="tollgateId">副本Id</param>
    public void f_DungeonReset(int tollgateId, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_DungeonReset, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(tollgateId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_DungeonReset, bBuf);
    }

    /// <summary>
    /// 请求副本关卡宝箱
    /// </summary>
    /// <param name="tollgateId">关卡Id</param>
    public void f_DungeonTollgateBox(int tollgateId, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_DungeonTollgateBox, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(tollgateId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_DungeonTollgateBox, bBuf);
    }

    /// <summary>
    /// 请求副本章节宝箱
    /// </summary>
    /// <param name="chapterId">章节Id</param>
    /// <param name="boxIdx">宝箱索引 [1,2,3]</param>
    public void f_DungeonChapterBox(int chapterId, byte boxIdx, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_DungeonChapterBox, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(chapterId);
        tCreateSocketBuf.f_Add(boxIdx);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_DungeonChapterBox, bBuf);
    }

    private bool bInitDungeonLegend = false;
    /// <summary>
    /// 请求名将副本的数据
    /// </summary>
    public void f_DungeonLegend()
    {
        if (bInitDungeonLegend)
            return;
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_DungeonLegend, bBuf);
        bInitDungeonLegend = true;
    }


    /// <summary>
    /// 一键领取关卡和星数奖励
    /// </summary>
    /// <param name="dungeonType">副本类型</param>
    public void f_GetCheckpointAndStarReward(ushort dungeonType, SocketCallbackDT tSocketCallbackDT)
    {
        _awardList1.Clear();
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_DungeonBoxOneKeyGet, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(dungeonType);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_DungeonBoxOneKeyGet, bBuf);
    }
    #endregion

    /// <summary>
    /// 根据类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public DungeonTollgatePoolDT f_GetTollgatePoolDTByType(EM_Fight_Enum type)
    {
        DungeonPoolDT pooldt = (DungeonPoolDT)f_GetForData1((int)type);
        if (pooldt == null)
        {
MessageBox.ASSERT("Subject data does not exist type:" + type.ToString());
        }
        return pooldt.mTollgateList[0];
    }

    private ccCallback mCallback_AfterInitDungeon;
    private DungeonPoolDT mExecutDungeonPoolDt;
    public void f_ExecuteAfterInitDungeon(DungeonPoolDT poolDt, ccCallback initCallback)
    {
        if (poolDt.mInitByServer)
        {
            initCallback(poolDt);
        }
        else
        {
            mCallback_AfterInitDungeon = initCallback;
            mExecutDungeonPoolDt = poolDt;
            SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
            socketCallbackDt.m_ccCallbackSuc = f_Callback_DungeonTollgate_Suc;
            socketCallbackDt.m_ccCallbackFail = f_Callback_DungeonTollgate_Fail;
            //发送请求某个章节的关卡数据
            f_DungeonTollgate(poolDt.m_iChapterTemplateId, socketCallbackDt);
        }
    }

    private void f_Callback_DungeonTollgate_Suc(object result)
    {
        if (mCallback_AfterInitDungeon != null && mExecutDungeonPoolDt != null)
        {
            mExecutDungeonPoolDt.f_UpdateInitByServerState(true);
            mCallback_AfterInitDungeon(mExecutDungeonPoolDt);
        }
    }

    private void f_Callback_DungeonTollgate_Fail(object result)
    {
        if (mCallback_AfterInitDungeon != null)
        {
            mCallback_AfterInitDungeon(result);
        }
    }
    /// <summary>
    /// 判断当前是否已经通关  (点击挑战数据就已经夏利)
    /// </summary>
    /// <returns></returns>
    public bool f_IsYetDungenon(int ChapterId, int tollgateID)
    {
        bool Yet = false;
        DungeonPoolDT chapterDT = (DungeonPoolDT)f_GetForId(ChapterId);
        if (chapterDT == null)
            return false;
        DungeonTollgatePoolDT tollgateDT = chapterDT.f_GetTollgateData(tollgateID);
        if (tollgateDT == null)
            return false;

        if (tollgateDT.mStarNum >= 1)
            return true;
        return Yet;
    }

    #region 扫荡奖励相关
    //扫荡关卡Idx
    private int sweepIdx = 0;
    //扫荡关卡Id
    private int sweepTollgateId = 0;
    private int sweepTimes = 0;
    private DungeonTollgateDT sweepTollgateDT = null;

    private List<DungeonSweepResult> sweepResultList = new List<DungeonSweepResult>();

    /// <summary>
    /// 扫荡前初始化数据
    /// </summary>
    /// <param name="tollgateId"></param>
    /// <param name="times"></param>
    private void f_InitSweepData(int tollgateId, int times)
    {
        sweepTollgateId = tollgateId;
        sweepTimes = times;
        sweepIdx = 0;
        sweepResultList.Clear();
        //等级相关信息
        int lv = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        int exp = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Exp);
        StaticValue.m_sLvInfo.f_UpdateInfo(lv, exp);
    }

    public void f_AddSweepResult(List<AwardPoolDT> awardList)
    {
        if (sweepTollgateDT == null || sweepTollgateDT.iId != sweepTollgateId)
            sweepTollgateDT = (DungeonTollgateDT)glo_Main.GetInstance().m_SC_Pool.m_DungeonTollgateSC.f_GetSC(sweepTollgateId);
        if (sweepIdx >= sweepTimes)
        {
MessageBox.ASSERT("The number of scans and the number of rewards are not the same");
        }
        else
        {
            int tEnergyCost = sweepTollgateDT.iEnergyCost;
            int tLv = StaticValue.m_sLvInfo.m_iAddLv;
            int addExp;
            int tExp = GameFormula.f_EnergyCost2Exp(tLv, tEnergyCost, out addExp);
            int tMoney = GameFormula.f_EnergyCost2Money(tLv, tEnergyCost);
            //算出经验后添加经验
            StaticValue.m_sLvInfo.f_AddExp(tExp + addExp);
            DungeonSweepResult tResult = new DungeonSweepResult(tExp, addExp, tMoney, awardList);
            sweepResultList.Add(tResult);
        }
        sweepIdx++;
    }

    public List<DungeonSweepResult> f_GetSweepResult()
    {
        return sweepResultList;
    }

    public List<AwardPoolDT> f_GetOneKeyGetRewardInfo()
    {
        return _awardList1;
    }

    #endregion
    #region 红点处理
    /// <summary>
    /// 重设副本领取关卡宝箱数
    /// </summary>
    /// <param name="mTollgateId"></param>
    public void f_ResetDungeonGetBoxCountData(int mTollgateId)
    {
        List<BasePoolDT<long>> listData = f_GetAll();
        for (int i = 0; i < listData.Count; i++)
        {
            DungeonPoolDT poolDT = listData[i] as DungeonPoolDT;
            int getCount = 0;
            bool isNeedUpdateDungeonPoolDT = false;
            for (int j = 0; j < poolDT.mTollgateList.Count; j++)
            {
                DungeonTollgatePoolDT tollgateDT = poolDT.mTollgateList[j];
                if (tollgateDT.mTollgateId == mTollgateId)
                    isNeedUpdateDungeonPoolDT = true;
                if (j < poolDT.mTollgatePassNum)
                {
                    if (tollgateDT.mTollgateTemplate.iBoxId > 0 && tollgateDT.f_GetBoxState() == EM_BoxGetState.AlreadyGet)
                    {
                        getCount++;
                    }
                }
                else
                {
                    break;
                }
            }
            if (isNeedUpdateDungeonPoolDT)
            {
                poolDT.mToBoxGetNum = getCount;
                break;
            }
        }
    }
    /// <summary>
    /// 检测主线副本一键领取红点
    /// </summary>
    /// <returns></returns>
    public bool f_CheckMainDungeonTollgateBoxGetRedPoint(EM_Fight_Enum fightEnum)
    {
        bool isTureRedPoint = false;
        List<BasePoolDT<long>> listData = Data_Pool.m_DungeonPool.f_GetAllForData1((int)fightEnum);
        for (int i = 0; i < listData.Count; i++)
        {
            DungeonPoolDT poolDT = listData[i] as DungeonPoolDT;
            if (poolDT.mTollgatePassNum == 0)
            {
                break;
            }
            if (f_CheckHasBoxCanGet(poolDT))
            {
                isTureRedPoint = true;
                break;
            }
            for (int j = 0; j < poolDT.mTollgateList.Count; j++)
            {
                if (poolDT.mTollgateList[j].mTollgateTemplate.iBoxId > 0 && poolDT.mTollgateList[j].f_GetBoxState() == EM_BoxGetState.CanGet)
                {
                    isTureRedPoint = true;
                    break;
                }

            }
            if (isTureRedPoint) break;
        }

        return isTureRedPoint;
    }

    /// <summary>
    /// 检测主线副本红点
    /// </summary>
    /// <returns></returns>
    public bool f_CheckMainDungeonBoxGetRedPoint()
    {
        List<BasePoolDT<long>> listData = Data_Pool.m_DungeonPool.f_GetAllForData1((int)EM_Fight_Enum.eFight_DungeonMain);
        return f_checkRedPoint(listData);
    }
    /// <summary>
    /// 检测精英副本红点
    /// </summary>
    /// <returns></returns>
    public bool f_CheckEliteDungeonBoxGetRedPoint()
    {
        List<BasePoolDT<long>> listData = Data_Pool.m_DungeonPool.f_GetAllForData1((int)EM_Fight_Enum.eFight_DungeonElite);
        return f_checkRedPoint(listData);
    }
    private bool f_checkRedPoint(List<BasePoolDT<long>> listData)
    {
        bool isTureRedPoint = false;
        for (int i = 0; i < listData.Count; i++)
        {
            DungeonPoolDT poolDT = listData[i] as DungeonPoolDT;
            if (f_CheckChapterLockState(poolDT))
            {
                return isTureRedPoint;
            }
            else
            {
                if (f_CheckHasBoxCanGet(poolDT))
                {
                    isTureRedPoint = true;
                    return isTureRedPoint;
                }
            }
        }
        return isTureRedPoint;
    }
    /// <summary>
    /// 检查是否有宝箱可以领取
    /// </summary>
    public bool f_CheckHasBoxCanGet(DungeonPoolDT dungeonPoolDT)
    {
        bool HasBox = false;
        int BoxCount = 0;
        for (int i = 0; i < dungeonPoolDT.mTollgateList.Count; i++)
        {
            if (i < dungeonPoolDT.mTollgatePassNum)
            {
                if (dungeonPoolDT.mTollgateList[i].mTollgateTemplate.iBoxId > 0)
                    BoxCount++;
            }
            else
                break;
        }
        if (dungeonPoolDT.mToBoxGetNum < BoxCount)
            HasBox = true;
        bool isCanGetBox1 = !dungeonPoolDT.mBox1Get && dungeonPoolDT.m_ChapterTemplate.iNeedStar1 > 0 && (dungeonPoolDT.mStarNum >= dungeonPoolDT.m_ChapterTemplate.iNeedStar1);
        bool isCanGetBox2 = !dungeonPoolDT.mBox2Get && dungeonPoolDT.m_ChapterTemplate.iNeedStar2 > 0 && (dungeonPoolDT.mStarNum >= dungeonPoolDT.m_ChapterTemplate.iNeedStar2);
        bool isCanGetBox3 = !dungeonPoolDT.mBox3Get && dungeonPoolDT.m_ChapterTemplate.iNeedStar3 > 0 && (dungeonPoolDT.mStarNum >= dungeonPoolDT.m_ChapterTemplate.iNeedStar3);
        HasBox = HasBox || isCanGetBox1 || isCanGetBox2 || isCanGetBox3;
        return HasBox;
    }
    #endregion
    #region 七日活动用
    public int f_GetMaxStraNum(int tType)
    {
        int starNum = 0;
        DungeonPoolDT tDungeonPoolDT;
        for (int i = 0; i < f_GetAllForData1(tType).Count; i++)
        {
            tDungeonPoolDT = f_GetAllForData1(tType)[0] as DungeonPoolDT;
            if (tDungeonPoolDT != null)
            {
                starNum += tDungeonPoolDT.mStarNum;
            }
        }
        return starNum;
    }
    #endregion
}

/// <summary>
/// 副本结算信息
/// </summary>
public class DungeonFinishInfo
{
    public DungeonFinishInfo()
    {
        StarNum = 0;
        IsFirstWin = false;
        NeedShowFirstWin = false;
        mAwardSource = EM_AwardSource.eAward_Dungeon;
    }

    public int StarNum
    {
        get;
        private set;
    }

    public bool IsFirstWin
    {
        get;
        private set;
    }

    public EM_AwardSource mAwardSource
    {
        private set;
        get;
    }

    public bool NeedShowFirstWin
    {
        get;
        private set;
    }

    /// <summary>
    /// 奖励的元宝数量
    /// </summary>
    public int AwardSyceeNum
    {
        get;
        private set;
    }

    public int EnergyCost
    {
        get;
        private set;
    }

    public void f_UpdateInfo(int tollgateId, int starNum, byte isFirstWin)
    {
        StarNum = starNum;
        IsFirstWin = isFirstWin > 0 ? true : false;
        DungeonTollgateDT tItem = (DungeonTollgateDT)glo_Main.GetInstance().m_SC_Pool.m_DungeonTollgateSC.f_GetSC(tollgateId);
        if (IsFirstWin && tItem.iFirstWinSycee > 0)
        {
            NeedShowFirstWin = true;
            AwardSyceeNum = tItem.iFirstWinSycee;
        }
        EnergyCost = tItem.iEnergyCost;
    }

    public void f_GetInfo(ref int awardSycee)
    {
        awardSycee = AwardSyceeNum;
        NeedShowFirstWin = false;
    }



}
