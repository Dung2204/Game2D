using ccU3DEngine;
using UnityEngine;
using System;
using System.Collections.Generic;

public class ChaosBattlePage : UIFramwork
{
    //匹配遮罩
    private GameObject mMatchMask;
    //Texture
    private UITexture mTex_ChaosBattleBg;
    private UITexture mTex_EnemyRoleBg;

    //Left
    private UILabel mScore;
    private UILabel mJobTitle;
    private UIGrid mStarGrid;
    private GameObject mStar;

    private UISprite[] mStars;

    //Right
    private UILabel mLeftBattleTimes;
    private UILabel mBattleZone;
    private UILabel mWinAward;
    private UILabel mLoseAward;
    private UILabel mWinStreakAward;
    private UILabel mCurWinStreak;
    private UILabel mJobTitleAward;

    //Self
    private Transform mSelfRoleParent;
    private UILabel mSelfServerInfo;
    private UILabel mSelfPlayerName;
    private UILabel mSelfPlayerPower;
    private UILabel mSelfLevel;
    private UI2DSprite mSelfIcon;

    private GameObject mSelfRole;

    //Enemy
    private GameObject mMatchEnemy;
    private GameObject mEndMatchEnemy;
    private Transform mEnemyRoleParent;
    private UILabel mEnemyServerInfo;
    private UILabel mEnemyPlayerName;
    private UILabel mEnemyPlayerPower;
    private UILabel mEnemyLevel;
    private UI2DSprite mEnemyIcon;

    private GameObject mEnemyRole;

    private UILabel mMatchTip;
    private GameObject mBtnMatch;
    private GameObject mBtnMatchGray;
    private UILabel mShowEnemyTime;


    private ChaosBattlePool m_CurDataPool;
    //TsuCode - newRankList
    private GameObject _rankIntegralItemParent;//排行积分父节点
    private GameObject _rankIntegralItem;//排行积分Item
    private GameObject _rankAwardParent;//排行奖励父节点
    private GameObject _rankAwardInfoItem;//排行item

    private UILabel _myRank;//我的排名
    private UILabel _myIntegral;//我的积分
    private GameObject _noManRankSign;
    bool m_NeedUpdate = false;

    private UIWrapComponent _RankAwardWrapComponent;
    private GodDressPoolDT m_GodDressPoolDT;

    //TsuCode - Record
    private GameObject _recordParent;//
    private GameObject _recordItem;
    //-------------------
    private UIWrapComponent mRankAwardWrapComponent
    {
        get
        {
            if (_RankAwardWrapComponent == null)
            {
                //只显示前一百名
                List<BasePoolDT<long>> listRankData = m_GodDressPoolDT.m_GodRankData.GetRange(0, 7);
                _RankAwardWrapComponent = new UIWrapComponent(100, 1, 600, 8, _rankAwardParent, _rankAwardInfoItem, listRankData, RankIntegralItemUpdateByInfo, null);
            }
            return _RankAwardWrapComponent;
        }
    }

    private UIScrollView mScrollView;
    private GameObject mRankItemParent;

    //------------------------
    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        HELP_CONTENT = CommonTools.f_GetTransLanguage(1017);
        MatchingTips = new string[] { CommonTools.f_GetTransLanguage(1022), CommonTools.f_GetTransLanguage(1023), CommonTools.f_GetTransLanguage(1024) };
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mMatchMask = f_GetObject("MatchMask");
        mTex_ChaosBattleBg = f_GetObject("Tex_CrossServerBattleBg").GetComponent<UITexture>();
        //mTex_EnemyRoleBg = f_GetObject("Tex_EnemyRoleBg").GetComponent<UITexture>();
        //Left
        mScore = f_GetObject("Score").GetComponent<UILabel>();
        mJobTitle = f_GetObject("JobTitle").GetComponent<UILabel>();
        mStarGrid = f_GetObject("StarGrid").GetComponent<UIGrid>();
        mStar = f_GetObject("Star");
        //Right
        mLeftBattleTimes = f_GetObject("LeftBattleTimes").GetComponent<UILabel>();
        mBattleZone = f_GetObject("BattleZone").GetComponent<UILabel>();
        mWinAward = f_GetObject("WinAward").GetComponent<UILabel>();
        mLoseAward = f_GetObject("LoseAward").GetComponent<UILabel>();
        mWinStreakAward = f_GetObject("WinStreakAward").GetComponent<UILabel>();
        mCurWinStreak = f_GetObject("CurWinStreak").GetComponent<UILabel>();
        mJobTitleAward = f_GetObject("JobTitleAward").GetComponent<UILabel>();
        //self
        mSelfRoleParent = f_GetObject("SelfRoleParent").transform;
        mSelfServerInfo = f_GetObject("SelfServerInfo").GetComponent<UILabel>();
        mSelfPlayerName = f_GetObject("SelfPlayerName").GetComponent<UILabel>();
        mSelfPlayerPower = f_GetObject("SelfPlayerPower").GetComponent<UILabel>();
        mSelfLevel = f_GetObject("SelfLevel").GetComponent<UILabel>();
        mSelfIcon = f_GetObject("SelfIcon").GetComponent<UI2DSprite>();
        //Enemy
        mMatchEnemy = f_GetObject("MatchEnemy");
        mEndMatchEnemy = f_GetObject("EndMatchEnemy");
        mEnemyRoleParent = f_GetObject("EnemyRoleParent").transform;
        mEnemyServerInfo = f_GetObject("EnemyServerInfo").GetComponent<UILabel>();
        mEnemyPlayerName = f_GetObject("EnemyPlayerName").GetComponent<UILabel>();
        mEnemyPlayerPower = f_GetObject("EnemyPlayerPower").GetComponent<UILabel>();
        mEnemyIcon = f_GetObject("EnemyIcon").GetComponent<UI2DSprite>();
        mEnemyLevel = f_GetObject("EnemyLevel").GetComponent<UILabel>();

        mMatchTip = f_GetObject("MatchTip").GetComponent<UILabel>();
        mMagicParent = f_GetObject("MagicParent").transform;
        mBtnMatch = f_GetObject("BtnMatch");
        mBtnMatchGray = f_GetObject("BtnMatchGray");
        mShowEnemyTime = f_GetObject("ShowEnemyTime").GetComponent<UILabel>();

        f_RegClickEvent("BtnReturn", f_OnReturnBtnClick);
        f_RegClickEvent("BtnMatch", f_OnMatchBtnClick);
        f_RegClickEvent("BtnBuyTimes", f_OnBuyTimesBtnClick);
        f_RegClickEvent("BtnShop", f_OnShopBtnClick);
        f_RegClickEvent("BtnRank", f_OnRankBtnClick);
        f_RegClickEvent("BtnHelp", f_OnHelpBtnClick);

        //TsuCode - newRankList
        _rankIntegralItemParent = f_GetObject("RankIntegralGrid");
        _rankIntegralItem = f_GetObject("RankIntegralItem");
        _rankAwardParent = f_GetObject("GridContentParent");
        _rankAwardInfoItem = f_GetObject("ContentItem");
        _noManRankSign = f_GetObject("ActIntegralRankSign");

        _myRank = f_GetObject("RankText").GetComponent<UILabel>();
        _myIntegral = f_GetObject("IntegralText").GetComponent<UILabel>();
        //f_RegClickEvent("RankAward", f_OpenRankAwardPage);
        f_RegClickEvent("RankAward", f_OpenRankAwardPage_ChaosRank);
        f_RegClickEvent("TaskAward", f_OpenRankAwardPage_ChaosTask);

        mScrollView = f_GetObject("ScrollView").GetComponent<UIScrollView>();
        mRankItemParent = f_GetObject("RankItemParent");
        mScrollView.onDragFinished = f_MomnetEnds;
        //TsuCode - Record
        _recordParent = f_GetObject("RecordParent");
        _recordItem = f_GetObject("RecordItem");
        //

        //初始化星数
        mStar.SetActive(false);
        mStars = new UISprite[Data_Pool.m_ChaosBattlePool.TitleStarMax];
        for(int i = 0; i < Data_Pool.m_ChaosBattlePool.TitleStarMax; i++)
        {
            mStars[i] = NGUITools.AddChild(mStarGrid.gameObject, mStar).GetComponent<UISprite>();
            mStars[i].gameObject.SetActive(true);
        }
        mStarGrid.repositionNow = true;
        mStarGrid.Reposition();
    }

    protected override void UI_OPEN(object e)
    {
        
        base.UI_OPEN(e);
        //断线重连如果切去其他界面，这时候不处理，服务器消息返回再打开界面就会和其他界面重叠
        if (!UITool.f_IsCanOpenChallengePage(UINameConst.ChaosBattlePage))
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.ChaosBattlePage, UIMessageDef.UI_CLOSE);
            return;
        }

        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < UITool.f_GetSysOpenLevel(EM_NeedLevel.ChaosBattle))
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1011), UITool.f_GetSysOpenLevel(EM_NeedLevel.ChaosBattle)));
            ccUIManage.GetInstance().f_SendMsg(UINameConst.ChaosBattlePage, UIMessageDef.UI_CLOSE);
            ccUIHoldPool.GetInstance().f_UnHold();
            return;
        }
        f_SetUIInfo();
        //TsuCode - newRankList
        m_GodDressPoolDT = Data_Pool.m_GodDressPool.f_GetCurPoolDt();
        //f_SetRankIntegralInfo();
        Data_Pool.m_ChaosBattlePool.f_ExecuteAfterRankList((byte)1, false, ref m_NeedUpdate, f_Callback_RankList);
        // Data_Pool.m_ChaosBattlePool.f_ExecuteAfterRankList_Score(f_Callback_RankList);
      

        //Record
        SocketCallbackDT socket = new SocketCallbackDT();
        socket.m_ccCallbackSuc = f_Callback_ChaosHistory;
        socket.m_ccCallbackFail = f_Callback_ChaosHistory;
        Data_Pool.m_ChaosBattlePool.f_ChaosHistory(socket);
   
    }

    private void f_SetUIInfo()
    {
        m_CurDataPool = Data_Pool.m_ChaosBattlePool;
        f_LoadTexture();
        f_UpdateLeftInfo();
        f_UpdateRightInfo();
        f_UpdateSelfRole();
        //mMatchEnemy.SetActive(true);
        mEndMatchEnemy.SetActive(false);
        mMatchTip.text = "";
        mBtnMatch.SetActive(true);
        mBtnMatchGray.SetActive(false);
        mShowEnemyTime.text = string.Empty;
        mMatchMask.SetActive(false);
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_ChaosBattleInit;
        socketCallbackDt.m_ccCallbackFail = f_Callback_ChaosBattleInit;
        m_CurDataPool.f_ChaosBattleInit(socketCallbackDt);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_ChaosBattle_TimesUpdate, f_Callback_TimesUpdate, this);
        f_OpenOrCloseMoneyTopPage(true);
    }

    private void f_Callback_ChaosBattleInit(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if((int)result != (int)eMsgOperateResult.OR_Succeed)
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(1012));
        f_UpdateLeftInfo();
        f_UpdateRightInfo();
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        if(mSelfRole != null)
        {
            UITool.f_DestoryStatelObject(mSelfRole);
            mSelfRole = null;
        }
        if(mEnemyRole != null)
        {
            UITool.f_DestoryStatelObject(mEnemyRole);
            mEnemyRole = null;
        }
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_ChaosBattle_TimesUpdate, f_Callback_TimesUpdate, this);
        f_OpenOrCloseMoneyTopPage(false);
        //Record
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_TurntablePage_UpdateRecord, f_EeventCallback_UpdateList, this);
        //
    }

    protected override void UI_HOLD(object e)
    {
        base.UI_HOLD(e);
        f_OpenOrCloseMoneyTopPage(false);
    }

    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        f_SetUIInfo();
    }

    private string strTexChaosBattleBgPatch = "UI/TextureRemove/Challenge/Tex_CrossServerBattleBg";
    private string strTexEnemyRoleBgPath = "UI/TextureRemove/Challenge/Tex_CrossServerBattleRoleBg";
    private void f_LoadTexture()
    {
		if(ScreenControl.Instance.mFunctionRatio <= 0.85f)
		{
			// f_GetObject("TopPanel").transform.localPosition = new Vector3(0f, -180f, 0f);
		}
        if(mTex_ChaosBattleBg.mainTexture == null)
        {
            mTex_ChaosBattleBg.mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexChaosBattleBgPatch);
        }
        //if(mTex_EnemyRoleBg.mainTexture == null)
        //{
        //    mTex_EnemyRoleBg.mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexEnemyRoleBgPath);
        //}
    }

    private void f_UpdateLeftInfo()
    {
        mScore.text = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_ChaosScore).ToString();
        mJobTitle.text = m_CurDataPool.TitleTemplate == null ? string.Empty : m_CurDataPool.TitleTemplate.szName;
        UITool.f_UpdateStarNum(mStars, m_CurDataPool.TitleStar, "Icon_RMStar_4", "", m_CurDataPool.TitleTemplate == null ? m_CurDataPool.TitleStarMax : m_CurDataPool.TitleTemplate.iStarNum);
    }

    private void f_UpdateRightInfo()
    {
        mLeftBattleTimes.text = string.Format(CommonTools.f_GetTransLanguage(1013), m_CurDataPool.BattleLeftTimes);
        mBattleZone.text = m_CurDataPool.ZoneTemplate == null ? string.Empty : string.Format("{0}{1}-{2}",
            // m_CurDataPool.ZoneTemplate.szName, UITool.f_CountToChineseStr(m_CurDataPool.ZoneTemplate.iBeginPower), m_CurDataPool.ZoneTemplate.iEndPower == 0 ? "Max" : UITool.f_CountToChineseStr(m_CurDataPool.ZoneTemplate.iEndPower));
			m_CurDataPool.ZoneTemplate.szName, (m_CurDataPool.ZoneTemplate.iBeginPower + ""), m_CurDataPool.ZoneTemplate.iEndPower == 0 ? "Max" : (m_CurDataPool.ZoneTemplate.iEndPower + ""));
        mWinAward.text = m_CurDataPool.ZoneTemplate == null ? string.Empty : m_CurDataPool.ZoneTemplate.iWinAward.ToString();//string.Format(CommonTools.f_GetTransLanguage(1014), m_CurDataPool.ZoneTemplate.iWinAward);
        mLoseAward.text = m_CurDataPool.ZoneTemplate == null ? string.Empty : m_CurDataPool.ZoneTemplate.iLoseAward.ToString(); //string.Format(CommonTools.f_GetTransLanguage(1014), m_CurDataPool.ZoneTemplate.iLoseAward);
        string winStreakAward = "";
        if (m_CurDataPool.ZoneTemplate != null)
        {
            //连胜奖励要显示下一场的
            int integralArard = m_CurDataPool.Winstreak >= 10 ? m_CurDataPool.ZoneTemplate.iWinSteakAward * 10 : m_CurDataPool.ZoneTemplate.iWinSteakAward * m_CurDataPool.Winstreak;
            winStreakAward = integralArard.ToString();//string.Format(CommonTools.f_GetTransLanguage(1014), integralArard);
        }
        mWinStreakAward.text = winStreakAward;

        //连胜是胜利次数减一，，两场胜算连胜一场
        int continueWin = m_CurDataPool.Winstreak > 1 ? m_CurDataPool.Winstreak - 1 : 0;
        mCurWinStreak.text = string.Format("{0}", continueWin);

        //根据战区和头衔获取头衔积分奖励
        List<NBaseSCDT> titleAwardList = glo_Main.GetInstance().m_SC_Pool.m_ChaosBattleAwardSC.f_GetAll();
        if (null == titleAwardList || titleAwardList.Count <= 0)
        {
MessageBox.ASSERT("ChaosBattleAward data table is empty or data is empty！");
            return;
        }
        ChaosBattleAwardDT battleAward = titleAwardList.Find((NBaseSCDT item) => {
            ChaosBattleAwardDT dataDT = item as ChaosBattleAwardDT;
            if (null == item) return false;
            return dataDT.iZoneId == m_CurDataPool.ZoneId && dataDT.iTitleId == m_CurDataPool.TitleId;
        }) as ChaosBattleAwardDT;
        if (null == battleAward)
        {
MessageBox.ASSERT(string.Format("The corresponding reward could not be found，title id ：{0},area id：{1}", m_CurDataPool.TitleId, m_CurDataPool.ZoneId));
            return;
        }
        mJobTitleAward.text = battleAward == null ? string.Empty : battleAward.iAward.ToString(); //string.Format(CommonTools.f_GetTransLanguage(1014), battleAward.iAward);
    }

    private void f_UpdateSelfRole()
    {
        //if(Data_Pool.m_CardPool.mRolePoolDt.m_FanshionableDressPoolDT == null)
        //    UITool.f_CreateRoleByCardId(Data_Pool.m_CardPool.mRolePoolDt.m_CardDT.iId, ref mSelfRole, mSelfRoleParent, 1);
        //if(Data_Pool.m_CardPool.mRolePoolDt.m_FanshionableDressPoolDT != null)
        //    UITool.f_CreateRoleByCardId(Data_Pool.m_CardPool.mRolePoolDt.m_FanshionableDressPoolDT.m_iTempId, ref mSelfRole, mSelfRoleParent, 1);
        mSelfServerInfo.text = Data_Pool.m_UserData.m_strServerName;
        mSelfPlayerName.text = UITool.f_GetImporentForName(Data_Pool.m_CardPool.mRolePoolDt.m_CardDT.iImportant, Data_Pool.m_UserData.m_szRoleName);
        mSelfPlayerPower.text = Data_Pool.m_TeamPool.f_GetTotalBattlePower() + "";
        mSelfLevel.text = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level).ToString();
        mSelfIcon.sprite2D = UITool.f_GetIconSpriteBySexId(Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_MainCard) % 2);
    }

    private void f_UpdateEnemyRole(bool matching, ChaosBattlePoolDT enemyInfo)
    {
        //mMatchEnemy.SetActive(matching || enemyInfo.IsNull);
        mEndMatchEnemy.SetActive(!matching && !enemyInfo.IsNull);
        if(!matching && !enemyInfo.IsNull)
        {
            //UITool.f_CreateRoleByCardId(enemyInfo.CardId, ref mEnemyRole, mEnemyRoleParent, 1);
            mEnemyServerInfo.text = enemyInfo.ServerName;
            mEnemyPlayerName.text = UITool.f_GetImporentForName(enemyInfo.FrameId, enemyInfo.PlayerName);
            mEnemyPlayerPower.text = enemyInfo.Power + "";
            if(enemyInfo.Sex < 100)
			{
				mEnemyIcon.sprite2D = UITool.f_GetIconSpriteBySexId(enemyInfo.Sex);
			}
			else
			{
				mEnemyIcon.sprite2D = UITool.f_GetIconSpriteByCardId(enemyInfo.Sex);
			}
            mEnemyLevel.text = enemyInfo.Level + "";
        }
    }

    private void f_OpenOrCloseMoneyTopPage(bool isOpen)
    {
        if(isOpen)
        {
            List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
            listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
            listMoneyType.Add(EM_MoneyType.eUserAttr_Money);
            listMoneyType.Add(EM_MoneyType.eUserAttr_ChaosScore);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, listMoneyType);
        }
        else
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
        }
    }

    #region 按钮响应

    private void f_OnReturnBtnClick(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ChaosBattlePage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
    }

    private void f_OnMatchBtnClick(GameObject go, object value1, object value2)
    {
        //TsuCode --
        GameParamDT param = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(100);
        int nowTimeServer = GameSocket.GetInstance().f_GetServerTime(); //ServerTime now(second)
        DateTime time = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Unspecified).AddSeconds(nowTimeServer + 7 * 60 * 60);
        if (param != null)
            if (time.Hour < param.iParam1 || time.Hour > param.iParam2)
            {
UITool.Ui_Trip("Chưa mở");
                return;
            }

        //--------------------
        if (m_IsServerMatching || m_CurShowTime <= ShowMatchTime + ShowEnemyTime)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1015));
            return;
        }
        else if (Data_Pool.m_ChaosBattlePool.BattleLeftTimes <= 0)   //TsuComment - Bo phan kiem tra so lan chien dau
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1016));
            return;
        }
        mBtnMatch.SetActive(false);
        mBtnMatchGray.SetActive(true);
        f_BeginShowMatchAni();
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_MatchBattle;
        socketCallbackDt.m_ccCallbackFail = f_Callback_MatchBattle;
        Data_Pool.m_ChaosBattlePool.f_MatchBattle(socketCallbackDt);
    }

    private void f_OnBuyTimesBtnClick(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionDungeonBuyPage, UIMessageDef.UI_OPEN, LegionDungeonBuyPage.EM_BuyType.ChaosBattle);
    }

    private void f_OnShopBtnClick(GameObject go, object value1, object value2)
    {
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopMutiCommonPage, UIMessageDef.UI_OPEN, EM_ShopMutiType.ChaosBattle);
    }

    private void f_OnRankBtnClick(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ChaosBattleRankPage, UIMessageDef.UI_OPEN);
    }


    private string HELP_CONTENT = string.Empty;
    private void f_OnHelpBtnClick(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonHelpPage, UIMessageDef.UI_OPEN, 13);
    }

    #endregion

    private void f_Callback_TimesUpdate(object value)
    {
        mLeftBattleTimes.text = string.Format(CommonTools.f_GetTransLanguage(1018), m_CurDataPool.BattleLeftTimes);
    }

    private void f_Callback_MatchBattle(object result)
    {
        m_IsServerMatching = false;
        if((int)result == (int)eMsgOperateResult.OR_Succeed)
        {

        }
        else
        {
            if((int)result == (int)eMsgOperateResult.eOR_ChaosBattleTims)
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1019));
            else
                //UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1020), (int)result));
UITool.Ui_Trip("Không thể tham gia");
            f_EndShowMatchAni();
        }

    }

    /// <summary>
    /// 展示匹配时间
    /// </summary>
    public const float ShowMatchTime = 1.0f;

    /// <summary>
    /// 展示敌人时间
    /// </summary>
    public const float ShowEnemyTime = 3.0f;

    /// <summary>
    /// 更新时间间隔
    /// </summary>
    public const float UpdateTimeDis = 0.5f;

    /// <summary>
    /// 服务器是否正在匹配中
    /// </summary>
    private bool m_IsServerMatching = false;
    /// <summary>
    /// 当前展示时间
    /// </summary>
    private float m_CurShowTime = 99999f;

    /// <summary>
    /// 时间Id
    /// </summary>
    private int m_TimeEventId = 0;

    /// <summary>
    /// 开始展示匹配动画
    /// </summary>
    private void f_BeginShowMatchAni()
    {
        mMatchMask.SetActive(true);
        m_IsServerMatching = true;
        m_CurShowTime = 0.0f;
        m_TimeEventId = ccTimeEvent.GetInstance().f_RegEvent(UpdateTimeDis, true, null, f_MatchAniUpdate);
    }

    private void f_EndShowMatchAni()
    {
        f_UpdateEnemyRole(true, m_CurDataPool.EnemyInfo);
        mMatchTip.text = "";
        mShowEnemyTime.text = string.Empty;
        mMatchMask.SetActive(false);
        if(m_TimeEventId != 0)
        {
            ccTimeEvent.GetInstance().f_UnRegEvent(m_TimeEventId);
            m_TimeEventId = 0;
        }
        m_CurShowTime = 99999f;
        mBtnMatch.SetActive(true);
        mBtnMatchGray.SetActive(false);
        m_CreateMagicAni = false;
        if(mMagicGo != null)
        {
            //UITool.f_DestoryStatelObject(mMagicGo);
            Destroy(mMagicGo);
            mMagicGo = null;
        }
    }

    private void f_MatchAniUpdate(object result)
    {
        if(m_CurShowTime < ShowMatchTime)
        {
            m_CurShowTime += UpdateTimeDis;
            mMatchTip.text = f_GetMatchingTip();
            return;
        }
        else if(m_IsServerMatching)
        {
            mMatchTip.text = f_GetMatchingTip();
            return;
        }
        else if(!m_CreateMagicAni)
        {
            MessageBox.ASSERT("TsuLog ChaosENEMY: " + m_CurDataPool.EnemyInfo.PlayerName);
            //回调成功但是没有对手数据结束
            if(m_CurDataPool.EnemyInfo.IsNull)
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1021));
                f_EndShowMatchAni();
                return;
            }
            if(!mEndMatchEnemy.activeSelf)
                f_UpdateEnemyRole(false, m_CurDataPool.EnemyInfo);
            //spine 动画
            //UITool.f_CreateMagicById(20011, ref mMagicGo, mMagicParent, 2, f_EndMagicAni);

            //粒子动画
            if (mMagicGo != null)
            {
                //UITool.f_DestoryStatelObject(mMagicGo);
                Destroy(mMagicGo);
                mMagicGo = null;
            }
            mMagicGo = UITool.f_CreateEffect_Old("effect_vs_pipei_01", mMagicParent, Vector3.zero, 1.0f, 0f, "UI/UIEffect/TempVSEffect/Prefabs/");
            //重置下动画
            mMagicGo.SetActive(false);
            ccTimeEvent.GetInstance().f_RegEvent(0.2f, false, null, (object value) =>
            {
                if (mMagicGo != null)
                    mMagicGo.SetActive(true);
            });
            //3.5f后结束动画
            ccTimeEvent.GetInstance().f_RegEvent(3.5f, false, null,f_EndMagicAni);
            m_CreateMagicAni = true;
        }
        else if(mMagicGo == null && m_CurShowTime <= ShowMatchTime + ShowEnemyTime)
        {
            //回调成功但是没有对手数据结束
            if(m_CurDataPool.EnemyInfo.IsNull)
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1021));
                f_EndShowMatchAni();
                return;
            }
            if(!mEndMatchEnemy.activeSelf)
                f_UpdateEnemyRole(false, m_CurDataPool.EnemyInfo);
            mShowEnemyTime.text = string.Format("{0}", (int)(ShowMatchTime + ShowEnemyTime - m_CurShowTime));
            m_CurShowTime += UpdateTimeDis;
        }
        else if(mMagicGo == null)
        {
            f_EndShowMatchAni();
            f_BeginBattle();
        }
    }

    private void f_BeginBattle()
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ChaosBattlePage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.BattleMain);
    }

    private string[] MatchingTips = null;
    private int m_MatchingTipIdx = 0;
    private string f_GetMatchingTip()
    {
        if(m_MatchingTipIdx >= MatchingTips.Length)
        {
            m_MatchingTipIdx = 0;
        }
        return MatchingTips[m_MatchingTipIdx++];
    }


    private bool m_CreateMagicAni = false;

    private Transform mMagicParent;
    private GameObject mMagicGo = null;
    private void f_EndMagicAni(Spine.AnimationState state, int trackIndex)
    {
        state.End -= f_EndMagicAni;
        f_EndShowMatchAni();
        f_BeginBattle();
    }

    private void f_EndMagicAni(object value)
    {
        f_EndShowMatchAni();
        f_BeginBattle();
    }
    //------------------------------------------------------------------------------------------------//
    #region TsuCode - newRankList

    //Mot so func clone tu GodDress -> show Rank
    private void OnRankIntegralItemUpdate(GameObject item, RankUserInfo data)
    {
        UILabel name = item.transform.Find("Name").GetComponent<UILabel>();
        UILabel point = item.transform.Find("Point").GetComponent<UILabel>();
        UILabel roleName = item.transform.Find("NameRole").GetComponent<UILabel>();
        name.text = data.idx.ToString();
        roleName.text = data.szRoleName;
        point.text = data.uScore.ToString();
    }
 

    private void RankIntegralItemUpdateByInfo(Transform item, BasePoolDT<long> dt)
    {
        GodRankData data = (GodRankData)dt;
        UILabel rankText = item.Find("Index").GetComponent<UILabel>();
        GameObject awardParent = item.Find("ScrollView/Grid").gameObject;
        GameObject awardItem = item.Find("ResourceCommonItem").gameObject;
        //名次
        if (data.m_BeginRank == data.m_End_Rank)
        {
            rankText.text = string.Format(CommonTools.f_GetTransLanguage(1506), data.m_BeginRank.ToString());
        }
        else
        {
            rankText.text = string.Format(CommonTools.f_GetTransLanguage(1507), data.m_BeginRank.ToString(), data.m_End_Rank > 101 ? "∞" : data.m_End_Rank.ToString());
        }
        //设置奖励
        GridUtil.f_SetGridView<ResourceCommonDT>(awardParent, awardItem, data.m_ResourceCommonDTList, CommonAwarUpdate);
    }

  
    private void f_OpenRankAwardPage(GameObject go, object value1, object value2)
    {
        CommonShowAwardParam param = new CommonShowAwardParam();
        param.m_Item = f_GetObject("Item");

        param.m_PoolDTLise = m_GodDressPoolDT.m_GodRankData.GetRange(0, 6);
        param.m_PoolDTUpdate = RankIntegralItemUpdateByInfo;

        ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonShowAward, UIMessageDef.UI_OPEN, param);
    }

    private void CommonAwarUpdate(GameObject item, ResourceCommonDT data)
    {
        item.GetComponent<ResourceCommonItem>().f_UpdateByInfo(data);
    }


    //Record--------------------------------------------------------------------
    private void f_Callback_ChaosHistory(object result)
    {
        if ((int)result != (int)eMsgOperateResult.OR_Succeed)
        {
            MessageBox.ASSERT("CHECK CHAOS History Fail ");

        }
        else
        {
            MessageBox.ASSERT("CHECK CHAOS History Succecd ");
            f_SetRecordInfo();
        }
    }
    //-----------------ChaosRank => listRank of GodDress------------------------
    private void f_UpdataCountMoeny()
    {
        //newCode - show my ChaosRank Index
        ChaosBattleRankPoolDT chaosRank = Data_Pool.m_ChaosBattlePool.SelfRankInfo;
        _myRank.text = chaosRank.Rank.ToString();
        _myIntegral.text = chaosRank.ChaosScore.ToString();
        //
    }
    private void f_SetRankIntegralInfo()
    {
        //GridUtil.f_SetGridView<RankUserInfo>(_rankIntegralItemParent, _rankIntegralItem, listRankInfo, OnRankIntegralItemUpdate);
        List<BasePoolDT<long>> m_RankList = Data_Pool.m_ChaosBattlePool.RankDict[1].RankList;
        GridUtil.f_SetGridView<BasePoolDT<long>>(_rankIntegralItemParent, _rankIntegralItem, m_RankList, OnRankIntegralItemUpdate_TsuFunc);
    }
    private void f_SetRankIntegralInfo_TsuFunc()
    {

        //GridUtil.f_SetGridView<RankUserInfo>(_rankIntegralItemParent, _rankIntegralItem, listRankInfo, OnRankIntegralItemUpdate);
        //List<BasePoolDT<long>> m_RankList = Data_Pool.m_ChaosBattlePool.RankDict[1].RankList;
        List<ChaosBattleRankPoolDT> m_RankList = Data_Pool.m_ChaosBattlePool.ListChaosRank;
        int size = m_RankList.Count;
        if (size >10)
        {
            List<ChaosBattleRankPoolDT> m_RankList_Top10 = new List<ChaosBattleRankPoolDT>();
            for (int i = 0; i < 10; i++)
            {
                m_RankList_Top10.Add(m_RankList[i]);
            }
            GridUtil.f_SetGridView<ChaosBattleRankPoolDT>(_rankIntegralItemParent, _rankIntegralItem, m_RankList_Top10, OnRankIntegralItemUpdate_TsuFunc);
        }else
        GridUtil.f_SetGridView<ChaosBattleRankPoolDT>(_rankIntegralItemParent, _rankIntegralItem, m_RankList, OnRankIntegralItemUpdate_TsuFunc);
    }
    private void OnRankIntegralItemUpdate_TsuFunc(GameObject item, BasePoolDT<long> data)
    {
        UILabel name = item.transform.Find("Name").GetComponent<UILabel>();
        UILabel point = item.transform.Find("Point").GetComponent<UILabel>();
        UILabel roleName = item.transform.Find("NameRole").GetComponent<UILabel>();
        ChaosBattleRankPoolDT a = data as ChaosBattleRankPoolDT;
        name.text = a.Rank.ToString();
        roleName.text = a.PlayerName;
        point.text = a.ChaosScore.ToString();
    }
    private void f_Callback_RankList(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result != (int)eMsgOperateResult.OR_Succeed)
            MessageBox.ASSERT(string.Format(CommonTools.f_GetTransLanguage(1035), (int)result));
        f_SetRankIntegralInfo_TsuFunc();
        f_UpdataCountMoeny();
    }

    private void f_MomnetEnds() //Scrow
    {
        Vector3 constraint = mScrollView.panel.CalculateConstrainOffset(mScrollView.bounds.min, mScrollView.bounds.min);
        if (constraint.y <= 0)
        {
            UITool.f_OpenOrCloseWaitTip(true, true);
            Data_Pool.m_ChaosBattlePool.f_ExecuteAfterRankList((byte)1, true, ref m_NeedUpdate, f_Callback_RankList);
        }
    }
    //---------------------------------------------------------------------------
   
    //Show RankAward & TaskAward

    private void RankIntegralItemUpdateByInfo_ChaosRank(Transform item, NBaseSCDT dt)
    {
        ChaosBattleRankAwardDT data = (ChaosBattleRankAwardDT)dt;
        UILabel rankText = item.Find("Index").GetComponent<UILabel>();
        GameObject awardParent = item.Find("ScrollView/Grid").gameObject;
        GameObject awardItem = item.Find("ResourceCommonItem").gameObject;
        //名次
        if (data.iRankBeg == data.iRankEnd)
        {
            rankText.text = string.Format(CommonTools.f_GetTransLanguage(1506), data.iRankBeg.ToString());
        }
        else
        {
            //rankText.text = string.Format(CommonTools.f_GetTransLanguage(1507), data.iRankBeg.ToString(), data.iRankEnd > 101 ? "∞" : data.iRankEnd.ToString());
            rankText.text = string.Format(CommonTools.f_GetTransLanguage(1507), data.iRankBeg.ToString(), data.iRankEnd.ToString());
        }
        //设置奖励
        List<ResourceCommonDT> m_ResourceCommonDTList = CommonTools.f_GetListCommonDT(data.szAward);
        GridUtil.f_SetGridView<ResourceCommonDT>(awardParent, awardItem, m_ResourceCommonDTList, CommonAwarUpdate);
    }
    private void RankIntegralItemUpdateByInfo_ChaosTask(Transform item, NBaseSCDT dt)
    {
        ChaosBattleTaskDT data = (ChaosBattleTaskDT)dt;
        UILabel rankText = item.Find("Index").GetComponent<UILabel>();
        GameObject awardParent = item.Find("ScrollView/Grid").gameObject;
        GameObject awardItem = item.Find("ResourceCommonItem").gameObject;
         
        rankText.text = data.szTaskName + ":" + data.iCondition.ToString();
        //设置奖励
        List<ResourceCommonDT> m_ResourceCommonDTList = CommonTools.f_GetListCommonDT(data.szAward);
        GridUtil.f_SetGridView<ResourceCommonDT>(awardParent, awardItem, m_ResourceCommonDTList, CommonAwarUpdate);
    }
    private void f_OpenRankAwardPage_ChaosRank(GameObject go, object value1, object value2)
    {
        CommonShowAwardParam param = new CommonShowAwardParam();
        param.m_Item = f_GetObject("Item");
        param.m_SCDTList = glo_Main.GetInstance().m_SC_Pool.m_ChaosBattleRankAwardSC.f_GetAll();
        param.m_SCDTUpdate = RankIntegralItemUpdateByInfo_ChaosRank;

        ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonShowAward, UIMessageDef.UI_OPEN, param);
    }
    private void f_OpenRankAwardPage_ChaosTask(GameObject go, object value1, object value2)
    {
        CommonShowAwardParam param = new CommonShowAwardParam();
        param.m_Item = f_GetObject("Item");
        param.m_SCDTList = glo_Main.GetInstance().m_SC_Pool.m_ChaosBattleTaskSC.f_GetAll();
        param.m_SCDTUpdate = RankIntegralItemUpdateByInfo_ChaosTask;

        ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonShowAward, UIMessageDef.UI_OPEN, param);
    }
    //////-----------------------------------------------------------------------------------------------
    //Record
    private void f_SetRecordInfo()
    {
        MessageBox.ASSERT("TSULOG CHECK SIZE Chaos History: " + Data_Pool.m_ChaosBattlePool.ListChaosHistory.Count);
        List<ChaosHistoryPoolDT> listHistory = Data_Pool.m_ChaosBattlePool.ListChaosHistory;
        //listHistory.Reverse();
    //GridUtil.f_SetGridView<TurntableLotteryInfo>(_recordParent, _recordItem, Data_Pool.m_TurntablePool.m_RecordList, OnTurntableRecordItemUpdate);
    GridUtil.f_SetGridView<ChaosHistoryPoolDT>(_recordParent, _recordItem, listHistory, OnChaosHistoryRecordItemUpdate);
        _recordParent.transform.GetComponent<UIGrid>().Reposition();
        _recordParent.transform.GetComponentInParent<UIScrollView>().ResetPosition();

    }
    private void OnChaosHistoryRecordItemUpdate(GameObject item, ChaosHistoryPoolDT data)
    {

        ServerInforDT serverInfo = (ServerInforDT)glo_Main.GetInstance().m_SC_Pool.m_ServerInforSC.f_GetSC(data.ServerEnemyId);
        List<NBaseSCDT> a =  glo_Main.GetInstance().m_SC_Pool.m_ServerInforSC.f_GetAll();
        //string serverName = serverInfo != null ? serverInfo.szName : string.Empty;
        DateTime time = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Unspecified).AddSeconds(data.RecordTime + 7 * 60 * 60);
        string sztime = string.Format("{0}/{1} {2}:{3}", time.Month, time.Day, time.Hour, time.Minute);
		item.GetComponent<UILabel>().text = sztime + " [d1783d] [" + data.ServerName + "]" + "[4a4988]【" + data.UserName + "】[-]" + "[FFFFFF]khiêu chiến: " + "[d1783d]" + "[" + data.ServerEnemyName + "]" + "[4a4988]【" + data.EnemyName + "】" + ": [FFFFFF]" + (data.BattleRes != 0 ? "Chiến thắng" : "Thất bại");
        //item.GetComponent<UILabel>().text = "may mắn nhận:";
        //item.GetComponent<UILabel>().text = "may mắn nhận:";
    }
    private void f_EeventCallback_UpdateList(object value)
    {

        f_SetRecordInfo();
    }
    //
    #endregion TsuCode - newRankList

}
