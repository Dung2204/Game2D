using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class CrossServerBattlePage : UIFramwork
{
    //匹配遮罩
    private GameObject mMatchMask;
    //Texture
    private UITexture mTex_CrossServerBattleBg;
    //private UITexture mTex_EnemyRoleBg;

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
    //private Transform mSelfRoleParent;
    private UILabel mSelfServerInfo;
    private UILabel mSelfPlayerName;
    private UILabel mSelfPlayerPower;
    private UILabel mSelfLevel;
    private UI2DSprite mSelfIcon;
    private GameObject mSelfRole;

    //Enemy
    private GameObject mMatchEnemy;
    private GameObject mEndMatchEnemy;
    //private Transform mEnemyRoleParent;
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


    private CrossServerBattlePool m_CurDataPool;

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
        mTex_CrossServerBattleBg = f_GetObject("Tex_CrossServerBattleBg").GetComponent<UITexture>();
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
        //mSelfRoleParent = f_GetObject("SelfRoleParent").transform;
        mSelfServerInfo = f_GetObject("SelfServerInfo").GetComponent<UILabel>();
        mSelfPlayerName = f_GetObject("SelfPlayerName").GetComponent<UILabel>();
        mSelfPlayerPower = f_GetObject("SelfPlayerPower").GetComponent<UILabel>();
        mSelfLevel = f_GetObject("SelfLevel").GetComponent<UILabel>();
        mSelfIcon = f_GetObject("SelfIcon").GetComponent<UI2DSprite>();
        //Enemy
        mMatchEnemy = f_GetObject("MatchEnemy");
        mEndMatchEnemy = f_GetObject("EndMatchEnemy");
        mEnemyIcon = f_GetObject("EnemyIcon").GetComponent<UI2DSprite>();
        //mEnemyRoleParent = f_GetObject("EnemyRoleParent").transform;
        mEnemyServerInfo = f_GetObject("EnemyServerInfo").GetComponent<UILabel>();
        mEnemyPlayerName = f_GetObject("EnemyPlayerName").GetComponent<UILabel>();
        mEnemyPlayerPower = f_GetObject("EnemyPlayerPower").GetComponent<UILabel>();
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

        //初始化星数
        mStar.SetActive(false);
        mStars = new UISprite[Data_Pool.m_CrossServerBattlePool.TitleStarMax];
        for(int i = 0; i < Data_Pool.m_CrossServerBattlePool.TitleStarMax; i++)
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
        if (!UITool.f_IsCanOpenChallengePage(UINameConst.CrossServerBattlePage))
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.CrossServerBattlePage, UIMessageDef.UI_CLOSE);
            return;
        }

        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < UITool.f_GetSysOpenLevel(EM_NeedLevel.CrossServerBattle))
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1011), UITool.f_GetSysOpenLevel(EM_NeedLevel.CrossServerBattle)));
            ccUIManage.GetInstance().f_SendMsg(UINameConst.CrossServerBattlePage, UIMessageDef.UI_CLOSE);
            ccUIHoldPool.GetInstance().f_UnHold();
            return;
        }
        f_SetUIInfo();
    }

    private void f_SetUIInfo()
    {
        m_CurDataPool = Data_Pool.m_CrossServerBattlePool;
        f_LoadTexture();
        f_UpdateLeftInfo();
        f_UpdateRightInfo();
        f_UpdateSelfRole();
        mEndMatchEnemy.SetActive(false);
        //mMatchSelf.SetActive(false);
        mMatchTip.text = "";
        mBtnMatch.SetActive(true);
        mBtnMatchGray.SetActive(false);
        mShowEnemyTime.text = string.Empty;
        mMatchMask.SetActive(false);
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_CrossServerBattleInit;
        socketCallbackDt.m_ccCallbackFail = f_Callback_CrossServerBattleInit;
        m_CurDataPool.f_CrossServerBattleInit(socketCallbackDt);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_CrossServerBattle_TimesUpdate, f_Callback_TimesUpdate, this);
        f_OpenOrCloseMoneyTopPage(true);
    }

    private void f_Callback_CrossServerBattleInit(object result)
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
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_CrossServerBattle_TimesUpdate, f_Callback_TimesUpdate, this);
        f_OpenOrCloseMoneyTopPage(false);
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

    private string strTexCrossServerBattleBgPatch = "UI/TextureRemove/Challenge/Tex_CrossServerBattleBg";
    private string strTexEnemyRoleBgPath = "UI/TextureRemove/Challenge/Tex_CrossServerBattleRoleBg";
    private void f_LoadTexture()
    {
		if(ScreenControl.Instance.mFunctionRatio <= 0.85f)
		{
			//f_GetObject("TopPanel").transform.localPosition = new Vector3(0f, -150f, 0f);
		}
        if(mTex_CrossServerBattleBg.mainTexture == null)
        {
            mTex_CrossServerBattleBg.mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexCrossServerBattleBgPatch);
        }
        //if(mTex_EnemyRoleBg.mainTexture == null)
        //{
        //    mTex_EnemyRoleBg.mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexEnemyRoleBgPath);
        //}
    }

    private void f_UpdateLeftInfo()
    {
        mScore.text = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_CrossServerScore).ToString();
        mJobTitle.text = m_CurDataPool.TitleTemplate == null ? string.Empty : m_CurDataPool.TitleTemplate.szName;
        UITool.f_UpdateStarNum(mStars, m_CurDataPool.TitleStar, "Icon_RMStar_4", "", m_CurDataPool.TitleTemplate == null ? m_CurDataPool.TitleStarMax : m_CurDataPool.TitleTemplate.iStarNum);
    }

    private void f_UpdateRightInfo()
    {
        mLeftBattleTimes.text = string.Format(CommonTools.f_GetTransLanguage(1013), m_CurDataPool.BattleLeftTimes);
mBattleZone.text = m_CurDataPool.ZoneTemplate == null ? string.Empty : string.Format("{0}, Lực chiến: {1}-{2}",
            // m_CurDataPool.ZoneTemplate.szName, UITool.f_CountToChineseStr(m_CurDataPool.ZoneTemplate.iBeginPower), m_CurDataPool.ZoneTemplate.iEndPower == 0 ? "Max" : UITool.f_CountToChineseStr(m_CurDataPool.ZoneTemplate.iEndPower));
			m_CurDataPool.ZoneTemplate.szName, (m_CurDataPool.ZoneTemplate.iBeginPower + ""), m_CurDataPool.ZoneTemplate.iEndPower == 0 ? "tối đa" : (m_CurDataPool.ZoneTemplate.iEndPower + ""));
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
        List<NBaseSCDT> titleAwardList = glo_Main.GetInstance().m_SC_Pool.m_CrossServerBattleAwardSC.f_GetAll();
        if (null == titleAwardList || titleAwardList.Count <= 0)
        {
MessageBox.ASSERT("CrossServerBattleAward data table is empty or data is empty！");
            return;
        }
        CrossServerBattleAwardDT battleAward = titleAwardList.Find((NBaseSCDT item) => {
            CrossServerBattleAwardDT dataDT = item as CrossServerBattleAwardDT;
            if (null == item) return false;
            return dataDT.iZoneId == m_CurDataPool.ZoneId && dataDT.iTitleId == m_CurDataPool.TitleId;
        }) as CrossServerBattleAwardDT;
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
        mSelfLevel.text = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level).ToString() ;
        mSelfIcon.sprite2D = UITool.f_GetIconSpriteBySexId(Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_MainCard) % 2);
    }

    private void f_UpdateEnemyRole(bool matching, CrossServerBattlePoolDT enemyInfo)
    {
        mMatchEnemy.SetActive(matching || enemyInfo.IsNull);
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
			MessageBox.ASSERT("Enemy Icon: " + enemyInfo.Sex);
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
            listMoneyType.Add(EM_MoneyType.eUserAttr_CrossServerScore);
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
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CrossServerBattlePage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
    }

    private void f_OnMatchBtnClick(GameObject go, object value1, object value2)
    {
        if(m_IsServerMatching || m_CurShowTime <= ShowMatchTime + ShowEnemyTime)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1015));
            return;
        }
        else if(Data_Pool.m_CrossServerBattlePool.BattleLeftTimes <= 0)
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
        Data_Pool.m_CrossServerBattlePool.f_MatchBattle(socketCallbackDt);
    }

    private void f_OnBuyTimesBtnClick(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionDungeonBuyPage, UIMessageDef.UI_OPEN, LegionDungeonBuyPage.EM_BuyType.CrossServerBattle);
    }

    private void f_OnShopBtnClick(GameObject go, object value1, object value2)
    {
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopMutiCommonPage, UIMessageDef.UI_OPEN, EM_ShopMutiType.CrossServerBattle);
    }

    private void f_OnRankBtnClick(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CrossServerBattleRankPage, UIMessageDef.UI_OPEN);
    }


    private string HELP_CONTENT = string.Empty;
    private void f_OnHelpBtnClick(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonHelpPage, UIMessageDef.UI_OPEN, 5);
    }

    #endregion

    private void f_Callback_TimesUpdate(object value)
    {
        mLeftBattleTimes.text = string.Format(CommonTools.f_GetTransLanguage(1013), m_CurDataPool.BattleLeftTimes);
    }

    private void f_Callback_MatchBattle(object result)
    {
        m_IsServerMatching = false;
        if((int)result == (int)eMsgOperateResult.OR_Succeed)
        {

        }
        else
        {
            if((int)result == (int)eMsgOperateResult.eOR_CrossBattleTims)
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1019));
            else
                UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1020), (int)result));
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
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CrossServerBattlePage, UIMessageDef.UI_CLOSE);
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
}
