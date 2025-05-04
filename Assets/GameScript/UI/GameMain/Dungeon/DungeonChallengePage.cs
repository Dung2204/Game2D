using ccU3DEngine;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DungeonChallengePage : UIFramwork
{
    //挑战页星星
    private UILabel mChallengeTitle;
    private UISprite[] mChallengePageStar;
    private UILabel mDesc;
    private UILabel mFightValue;
    private UILabel mCostInfo;
    private UILabel mCountInfo;
    private UILabel mExpGet;
    private UILabel mCoinGet;
    private UILabel mLegendLabel;
    private GameObject mChallengeBtn;
    private GameObject mSweepBtn;
    private UILabel mSweepBtnLabel;
    private GameObject mResetBtn;
    private GameObject mClothArrayBtn;

    //重置关卡需要消耗的元宝数，根据次数
    private static int[] ResetNeedSycee = new int[4] { 20, 50, 100, 150 };
    private string strTexBgRoot = "UI/TextureRemove/Dungeon/ptfb_frame_h";

    //是否正在请求挑战（防止重复点击，连点）
    private bool isRequestingDungeon = false;

    //模型与对话
    private UILabel mModeDialog;
    private Transform mModeParent;
    private GameObject _role;

    //关卡掉落显示
    private UIGrid _awardGrid;
    private GameObject _awardItem;
    private UIScrollView _awardScrollView;
    private ResourceCommonItemComponent _awardShowComponent;
    private ResourceCommonItemComponent mAwardShowComponent
    {
        get
        {
            if (_awardShowComponent == null)
                _awardShowComponent = new ResourceCommonItemComponent(_awardGrid, _awardItem);
            return _awardShowComponent;
        }
    }

    //当前关卡数据
    private DungeonTollgatePoolDT mCurTollgateData;
    private int mSweepCount;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mDesc = f_GetObject("Desc").GetComponent<UILabel>();
        mFightValue = f_GetObject("FightValue").GetComponent<UILabel>();
        mCostInfo = f_GetObject("CostInfo").GetComponent<UILabel>();
        mCountInfo = f_GetObject("CountInfo").GetComponent<UILabel>();
        mExpGet = f_GetObject("ExpGet").GetComponent<UILabel>();
        mCoinGet = f_GetObject("CoinGet").GetComponent<UILabel>();
        mLegendLabel = f_GetObject("LegendLabel").GetComponent<UILabel>();
        mChallengeBtn = f_GetObject("ChallengeBtn");
        mSweepBtn = f_GetObject("SweepBtn");
        mSweepBtnLabel = f_GetObject("SweepBtnLabel").GetComponent<UILabel>();
        mResetBtn = f_GetObject("ResetBtn");

        mModeDialog = f_GetObject("ModeDialog").GetComponent<UILabel>();
        mModeParent = f_GetObject("ModeParent").transform;
        _awardItem = f_GetObject("ResourceCommonItem");
        _awardGrid = f_GetObject("AwardGrid").GetComponent<UIGrid>();
        _awardGrid.onReposition = f_AwardGridReposition;
        _awardScrollView = f_GetObject("AwardScrolView").GetComponent<UIScrollView>();
        mClothArrayBtn = f_GetObject("ClothArrayBtn");

        int starNum = 3;
        mChallengePageStar = new UISprite[starNum];
        for (int i = 0; i < starNum; i++)
        {
            mChallengePageStar[i] = f_GetObject(string.Format("ChallengeStar{0}", i)).GetComponent<UISprite>();
        }
        mChallengeTitle = f_GetObject("ChallengeTitle").GetComponent<UILabel>();

        //点击事件
        f_RegClickEvent(mChallengeBtn, f_ChallengeBtnClick);
        f_RegClickEvent(mSweepBtn, f_SweepBtnClick);
        f_RegClickEvent(mResetBtn, f_ResetBtnClick);
        f_RegClickEvent(mClothArrayBtn, f_ClothArrayBtn);
        f_RegClickEvent("ChallengMask", OnCloseChallengePage);   
    }

    /// <summary>
    /// 打开界面
    /// </summary>
    /// <param name="e">EquipSythesis类似</param>
    protected override void UI_OPEN(object e)
    {
        if (null == e) {
MessageBox.ASSERT("Empty parameter！");
            return;
        }
        base.UI_OPEN(e);
        DungeonTollgatePoolDT dt = (DungeonTollgatePoolDT)e;
        f_UpdateTollgateInfo(dt);
        f_LoadTexture();
		//My Code
		float windowAspect = (float)Screen.width /  (float) Screen.height ;
		MessageBox.ASSERT("" + windowAspect);
		if(windowAspect <= 1.55)
		{
			f_GetObject("Panel").transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
		}
		//
        Data_Pool.m_GuidancePool.IsOpenChanllenge = true;
        //UI事件
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_THENEXTDAY_UIPROCESS, f_NextDayProcess, this);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_UPDATE_USERINFOR, f_UpdateByUserInfChange, this);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_THENEXTDAY_UIPROCESS, f_NextDayProcess, this);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_UPDATE_USERINFOR, f_UpdateByUserInfChange, this);
    }

    /// <summary>
    /// 根据关卡数据刷新界面
    /// </summary>
    private void f_UpdateTollgateInfo(DungeonTollgatePoolDT dt)
    {
        //设置基本信息
        mCurTollgateData = dt;
        mDesc.text = dt.mTollgateTemplate.szTollgateDesc;
        mChallengeTitle.text = dt.mTollgateTemplate.szTollgateName;
        mFightValue.text = string.Format(CommonTools.f_GetTransLanguage(1059), dt.mTollgateTemplate.iFightValue, Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_CardPower));
        bool isDungeon = dt.mChapterType == (int)EM_Fight_Enum.eFight_DungeonMain || dt.mChapterType == (int)EM_Fight_Enum.eFight_DungeonElite;
        bool check = isDungeon && dt.mStarNum == 0;
        mCostInfo.text = check ? CommonTools.f_GetTransLanguage(2337) : string.Format("{0}", dt.mTollgateTemplate.iEnergyCost);
        f_GetObject("CostIcon").SetActive(!check);
        mCountInfo.text = string.Format("{0}/{1}", dt.mTollgateTemplate.iCountLimit - dt.mTimes, dt.mTollgateTemplate.iCountLimit);
        mLegendLabel.text = dt.mChapterType == (int)EM_Fight_Enum.eFight_Legend ? string.Format(CommonTools.f_GetTransLanguage(1061), Data_Pool.m_DungeonPool.mDungeonLegendLeftTimes, Data_Pool.m_DungeonPool.mDungeonLegendLimit) : string.Empty;
        mSweepCount = Data_Pool.m_DungeonPool.f_GetSweepCount(dt);
        mSweepBtnLabel.text = string.Format(CommonTools.f_GetTransLanguage(1063), mSweepCount);
        bool isUserTimes = dt.mTimes >= dt.mTollgateTemplate.iCountLimit;

        //名将隐藏重置、扫荡及星星按钮
        bool isLegend = dt.mChapterType == (int)EM_Fight_Enum.eFight_Legend;
        mResetBtn.SetActive(isUserTimes && !isLegend);
        mSweepBtn.SetActive(!isLegend && dt.mStarNum >= GameParamConst.StarNumPreTollgate && !isUserTimes);
        for (int i = 0; i < mChallengePageStar.Length; i++) {
            mChallengePageStar[i].gameObject.SetActive(!isLegend);
        }
        if (!isLegend) UITool.f_UpdateStarNum(mChallengePageStar, dt.mStarNum);

        //通关经验=主角等级*参数1*体力消耗
        GameParamDT gameParam = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(3);
        mExpGet.text = (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) * gameParam.iParam1 * dt.mTollgateTemplate.iEnergyCost).ToString();

        //钱=主角等级*参数1*体力消耗/5+360（向10取整）
        gameParam = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(5);
        int coinGet = (int)Mathf.Ceil(Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) * gameParam.iParam1 * dt.mTollgateTemplate.iEnergyCost / 5 + 360);
        coinGet = coinGet % 10 > 0 ? (coinGet / 10) * 10 + 10 : coinGet;
        mCoinGet.text = coinGet.ToString();

        //展示掉落
        List<AwardPoolDT> awardList = Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(dt.mTollgateTemplate.iPond);
        if (dt.mChapterType == (int)EM_Fight_Enum.eFight_DungeonMain)
        {
            //主线副本在情人节和红包活动中要额外添加展示活动道具
            GameParamDT tGameParamDT = glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.ValentinesDayOpenTime) as GameParamDT;
            if (null != tGameParamDT && CommonTools.f_CheckTime(tGameParamDT.iParam1.ToString(), tGameParamDT.iParam2.ToString()))
            {
                //情人节活动增加掉落玫瑰展示
                AwardPoolDT valentines = new AwardPoolDT();
                valentines.f_UpdateByInfo((byte)2, 370, 1);
                awardList.Add(valentines);
            }
            List<NBaseSCDT> listDTRecruitGift = glo_Main.GetInstance().m_SC_Pool.m_RedPacketTaskSC.f_GetAll();
            for (int i = 0; i < listDTRecruitGift.Count; i++)
            {
                //若开启红包活动则添加红包掉落道具
                RedPacketTaskDT taskDT = listDTRecruitGift[i] as RedPacketTaskDT;
                if (taskDT.iTaskType == Data_Pool.m_RedPacketTaskPool.mRecruitGiftTaskID)
                {
                    //过滤掉招募有礼类型
                    continue;
                }

                //TsuCode - rot li xi theo Gameparam id 90 (khac 0 la roi)
                int dropCheck = UITool.f_GetGameParamDT(95).iParam1;
                if (dropCheck==0)
                {
                    continue;
                }

                //
                //TsuComment - rot li xi theo RedPacket
                //bool isOpen = CommonTools.f_CheckTime(taskDT.iTimeBegin.ToString(), taskDT.iTimeEnd.ToString());
                //if (!isOpen)
                //{
                //    continue;
                //}
                //
                for (int redPackets = 361; redPackets <= 365; redPackets++)
                {
                    //紅包，“新”、“春”、“闖”、“三”、“囯”對應 361-365
                    AwardPoolDT redPacketProp = new AwardPoolDT();
                    redPacketProp.f_UpdateByInfo((byte)2, redPackets, 1);
                    awardList.Add(redPacketProp);
                }
                break;
            }
        }        
        mAwardShowComponent.f_Show(awardList, EM_CommonItemShowType.Icon, EM_CommonItemClickType.AllTip);        
        f_GetObject("BtnContentItemGrid").GetComponent<UIGrid>().Reposition();

        //更新模型
        int modeId = mCurTollgateData.mTollgateTemplate.iModeId;
        mModeDialog.text = mCurTollgateData.mTollgateTemplate.szModeDialog;
        RoleModelDT tRoleModelDt = (RoleModelDT)glo_Main.GetInstance().m_SC_Pool.m_RoleModelSC.f_GetSC(modeId);
        UITool.f_CreateRoleByModeId(modeId, ref _role, mModeParent, 24);
		int tRandom = UnityEngine.Random.Range(1, 3);
		try
		{
			if (modeId != null)
			{
				string id = "" + modeId;
				id = id.Remove(id.Length - 1);
				//MessageBox.ASSERT("Id: " + id);
				glo_Main.GetInstance().m_AdudioManager.f_PlayAudioVoice(id + "" + tRandom);
			}
		}
		catch (Exception e)
		{
			MessageBox.ASSERT(e.Message);
		}
    }
  
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        //加载背景图
        UITexture TexBg = f_GetObject("BG_DescBg").GetComponent<UITexture>();
        if (TexBg.mainTexture == null)
        {
            Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot);
            TexBg.mainTexture = tTexture2D;
        }
    }

    private void f_AwardGridReposition()
    {
        //在ResourceCommonItemComponent工具中，物品小于4然后就改为UIWidget.Pivot.Center了，所以这里强制改回左边
        _awardScrollView.contentPivot = UIWidget.Pivot.Left; 
        _awardScrollView.ResetPosition();
    }

    /// <summary>
    /// 挑战
    /// </summary>
    /// <param name="go"></param>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    private void f_ChallengeBtnClick(GameObject go, object value1, object value2)
    {
        if (isRequestingDungeon)
        {
            return;
        }
        if (mCurTollgateData.mTimes >= mCurTollgateData.mTollgateTemplate.iCountLimit)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1064));
            return;
        }
        else if (mCurTollgateData.mChapterType == (int)EM_Fight_Enum.eFight_DungeonMain || mCurTollgateData.mChapterType == (int)EM_Fight_Enum.eFight_DungeonElite)
        {
            if (mCurTollgateData.mStarNum != 0 && !UITool.f_IsEnoughMoney(EM_MoneyType.eUserAttr_Energy, mCurTollgateData.mTollgateTemplate.iEnergyCost, true, true, this))
            {
                return;
            }
        }
        else if (!UITool.f_IsEnoughMoney(EM_MoneyType.eUserAttr_Energy, mCurTollgateData.mTollgateTemplate.iEnergyCost, true, true, this))
        {
            return;
        }

        //传说副本 而且不是神魔关卡检查次数
        else if (mCurTollgateData.mChapterType == (int)EM_Fight_Enum.eFight_Legend && mCurTollgateData.mIndex < GameParamConst.LegendDungeonRestIdx && Data_Pool.m_DungeonPool.mDungeonLegendLeftTimes <= 0)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1065));
            return;
        }
        Data_Pool.m_DungeonPool.m_DungeonStartExp = Data_Pool.m_UserData.mExpPercent;

        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT tCallBackDT = new SocketCallbackDT();
        tCallBackDT.m_ccCallbackSuc = CallBack_DungeonChallenge_Suc;
        tCallBackDT.m_ccCallbackFail = CallBack_DungeonChallenge_Fail;

        //保存上次的挑战关卡id，用以展示模型跳出效果
        bool tIsFightTollgate = Data_Pool.m_DungeonPool.f_CheckIsFightTollgate(mCurTollgateData.mChapterId, mCurTollgateData.mTollgateId);
        if(tIsFightTollgate)
        PlayerPrefs.SetInt(StaticValue.LastChallengeDungeonIdKey + mCurTollgateData.mChapterType, mCurTollgateData.mTollgateId);

        //发送挑战协议
        Data_Pool.m_GuidancePool.DungeonID = mCurTollgateData.mTollgateId;
        isRequestingDungeon = true;
        Data_Pool.m_DungeonPool.f_DungeonChallenge(mCurTollgateData, tCallBackDT);
    }
   
    /// <summary>
    /// 扫荡
    /// </summary>
    /// <param name="go"></param>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    private void f_SweepBtnClick(GameObject go, object value1, object value2)
    {
        if (isRequestingDungeon)
        {
            return;
        }
        if (!UITool.f_GetIsOpensystem(EM_NeedLevel.SweepLevel))
        {
            int tNeedLv = UITool.f_GetSysOpenLevel(EM_NeedLevel.SweepLevel);
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1067), tNeedLv));
            return;
        }
        else if (mSweepCount <= 0)
        {
            if (mCurTollgateData.mTimes >= mCurTollgateData.mTollgateTemplate.iCountLimit)
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1069));
                //return;
            }
            //else if (!UITool.f_IsEnoughMoney(EM_MoneyType.eUserAttr_Energy, mCurTollgateData.mTollgateTemplate.iEnergyCost, true, true, this))
            //{
            //    return;
            //}
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT tCallBackDT = new SocketCallbackDT();
        tCallBackDT.m_ccCallbackSuc = CallBack_DungeonSweep_Suc;
        tCallBackDT.m_ccCallbackFail = CallBack_DungeonSweep_Fail;
        isRequestingDungeon = true;
        //发送扫荡协议
        Data_Pool.m_DungeonPool.f_DungeonSweep(mCurTollgateData.mTollgateId, (byte)mSweepCount, tCallBackDT);
    }
    
    /// <summary>
    /// 点击重置次数
    /// </summary>
    private void f_ResetBtnClick(GameObject go, object value1, object value2)
    {
        int maxTimes = 4;
        int vipLevel = UITool.f_GetNowVipLv();
        int userResetTimes = UITool.GetTimesByVip((int)EM_VipPrivilege.eVip_TollgateReset, vipLevel);//计算当前vip可重置的最大次数
        if (mCurTollgateData.mResetTimes >= userResetTimes)
        {
            if (userResetTimes < maxTimes)
            {
                int nextResetTimes = UITool.GetTimesByVip((int)EM_VipPrivilege.eVip_TollgateReset, vipLevel + 1);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, string.Format(CommonTools.f_GetTransLanguage(1070), vipLevel + 1, nextResetTimes));
            }
            else
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1071));
            }
            return;
        }
        else
        {
            int needSyceeReset = mCurTollgateData.mResetTimes >= ResetNeedSycee.Length ? ResetNeedSycee[ResetNeedSycee.Length - 1] : ResetNeedSycee[mCurTollgateData.mResetTimes];
            ResetLevelWindowParam param = new ResetLevelWindowParam();
            param.m_OnConfirmCallback = OnConfirmResetClick;
            param.m_WasteSyceeCount = needSyceeReset;
            param.m_LeftFreshTimes = userResetTimes - mCurTollgateData.mResetTimes;
            param.obj = needSyceeReset;
            ccUIManage.GetInstance().f_SendMsg(UINameConst.ResetLevelWindowPage, UIMessageDef.UI_OPEN, param);
        }
    }

    /// <summary>
    /// 确认重置关卡次数回调
    /// </summary>
    private void OnConfirmResetClick(object obj)
    {
        int needSyceeReset = (int)obj;
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee) < needSyceeReset)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1072));
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT tCallBackDT = new SocketCallbackDT();
        tCallBackDT.m_ccCallbackSuc = CallBack_DungeonReset_Suc;
        tCallBackDT.m_ccCallbackFail = CallBack_DungeonReset_Fail;
        //发送重置协议
        Data_Pool.m_DungeonPool.f_DungeonReset(mCurTollgateData.mTollgateId, tCallBackDT);
    }

    /// <summary>
    /// 关闭挑战页
    /// </summary>
    /// <param name="go"></param>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    private void OnCloseChallengePage(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.DungeonChallengePage, UIMessageDef.UI_CLOSE);
    }

    /// <summary>
    /// 打开布阵界面
    /// </summary>
    /// <param name="go"></param>
    private void f_ClothArrayBtn(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ClothArrayPage, UIMessageDef.UI_OPEN);
    }

    /// <summary>
    /// 挑战成功
    /// </summary>
    /// <param name="result"></param>
    private void CallBack_DungeonChallenge_Suc(object result)
    {
        isRequestingDungeon = false;
        UITool.f_OpenOrCloseWaitTip(false);
        MessageBox.DEBUG("DungeonChallenge Suc! code:" + result);
        //展示加载界面 并加载战斗场景
        ccUIManage.GetInstance().f_SendMsg(UINameConst.DungeonTollgatePageNew, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.BattleMain);
    }

    /// <summary>
    /// 挑战失败
    /// </summary>
    /// <param name="result"></param>
    private void CallBack_DungeonChallenge_Fail(object result)
    {
        isRequestingDungeon = false;
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.UI_ShowFailContent("DungeonChallenge Error! code:" + result);
    }

    /// <summary>
    /// 扫荡成功
    /// </summary>
    /// <param name="result"></param>
    private void CallBack_DungeonSweep_Suc(object result)
    {
        isRequestingDungeon = false;
        UITool.f_OpenOrCloseWaitTip(false);
        MessageBox.DEBUG("DungeonSweep Suc! code:" + result);

        //刷新界面
        f_UpdateTollgateInfo(mCurTollgateData);

        //打开奖励显示界面
        ccUIManage.GetInstance().f_SendMsg(UINameConst.DungeonSweepPage, UIMessageDef.UI_OPEN, Data_Pool.m_DungeonPool.f_GetSweepResult());
    }

    /// <summary>
    /// 扫荡失败
    /// </summary>
    /// <param name="result"></param>
    private void CallBack_DungeonSweep_Fail(object result)
    {
        isRequestingDungeon = false;
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.UI_ShowFailContent("DungeonSweep Error! code:" + result);
    }

    /// <summary>
    /// 重置成功
    /// </summary>
    /// <param name="result"></param>
    private void CallBack_DungeonReset_Suc(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);

        //刷新界面
        f_UpdateTollgateInfo(mCurTollgateData);
    }

    /// <summary>
    /// 重置失败
    /// </summary>
    /// <param name="result"></param>
    private void CallBack_DungeonReset_Fail(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.UI_ShowFailContent("DungeonReset Error! code:" + result);
    }

    /// <summary>
    /// UI界面跨天处理
    /// </summary>
    /// <param name="value"></param>
    private void f_NextDayProcess(object value)
    {
        if ((EM_NextDaySource)value != EM_NextDaySource.DungeonPool)
            return;
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1073));
        if (mCurTollgateData != null) f_UpdateTollgateInfo(mCurTollgateData);

    }

    /// <summary>
    /// 玩家信息改变刷新一下界面
    /// </summary>
    private void f_UpdateByUserInfChange(object value)
    {
        if (mCurTollgateData != null) f_UpdateTollgateInfo(mCurTollgateData);
    }
}
