using ccU3DEngine;
using System.Collections.Generic;
using UnityEngine;

public class RankListPageNew : UIFramwork
{

    //我的排名信息(除了军团)
    private Transform mMyRankInfoExcept;
    private UI2DSprite mMyHeadIcon;
    private UISprite mMyHeadFrame;
    private GameObject mobjMyFightPower;
    private UILabel mLabelMyPower;
    private UILabel mLabelMyRank;
    private UILabel mLabelMyName;
    private UILabel mLabelMyGuildName;
    private UILabel mLabelMyBepraised;
    private UILabel mLabelMyRamainBepraised;
    private GameObject mobjMyStar;
    private UILabel mLabelMyStarNum;
    private UILabel mLabelMyChapterInfo;
    private UILabel mLabelMyLv;

    //我的排名信息（军团）
    private Transform mMyLegionRankInfo;
    private GameObject mMyLegionInfo;
    private GameObject mUnJoinLegion;
    private GameObject mMyLegionUnRank;
    private UI2DSprite mMyLegionHeadIcon;
    private UISprite mMyLegionHeadFrame;
    private UILabel mLabelMyLegionPower;
    private UILabel mLabelMyLegionRank;
    private UILabel mLabelMyLegionGuildName;
    private UILabel mLabelMyLegionLv;

    private GameObject[] mSelectedRankTypeBtnList;

    //滑动面板相关
    private GameObject   mRankItemParent;
    private GameObject   mRankItem;
    private UIScrollView mScrollView;
    private Transform    mRoleParent;

    //左边选中信息
    private GameObject   mRole;
    private UI2DSprite   mHeadIcon;
    private UISprite     mHeadFrame;
    private UILabel      mLvLabel;
    private UILabel      mPowerLabel;
    private UILabel      mPlayerNameLabel;
    private EM_RankListType mCurRankType = EM_RankListType.Arena;
    private BasePoolDT<long> mCurSelData;
    private GameObject mlastSelectedItem;
    private bool mFirstPage = false;

    private UIWrapComponent _rankWrapComponet;
    public UIWrapComponent mRankWrapComponet
    {
        get
        {
            if (_rankWrapComponet == null)
            {
                List<BasePoolDT<long>> _arenaList = Data_Pool.m_ArenaPool.f_GetRankList();
                _rankWrapComponet = new UIWrapComponent(190, 1, 190, 5, mRankItemParent, mRankItem, _arenaList, f_RankItemUpdateByInfo, f_OnRankItemClick);
            }
            return _rankWrapComponet;
        }
    }

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();

        //获取排行榜类型按钮
        int maxRankType = (int)EM_RankListType.End;
        string[] rankTypeNameList = new string[maxRankType];
        mSelectedRankTypeBtnList = new GameObject[maxRankType];
        rankTypeNameList[(int)EM_RankListType.Power] = "PowerTypeTab";
        rankTypeNameList[(int)EM_RankListType.Level] = "LvTypeTab";
        rankTypeNameList[(int)EM_RankListType.Legion] = "GuildTypeTab";
        rankTypeNameList[(int)EM_RankListType.Duplicate] = "DupTypeTab";
        rankTypeNameList[(int)EM_RankListType.Arena] = "ArenaTypeTab";
        rankTypeNameList[(int)EM_RankListType.RunningMan] = "RunningManTypeTab";
        Transform rankTypeParent = f_GetObject("RankTypeBtnParent").transform;
        for (int i = 0; i < maxRankType; i++) {
            GameObject rankTypeBtn = rankTypeParent.Find(rankTypeNameList[i]).gameObject;
            f_RegClickEvent(rankTypeBtn, OnClickRankTypeBtn,i);
            mSelectedRankTypeBtnList[i] = rankTypeBtn.transform.Find(rankTypeNameList[i] + "Selected").gameObject;
        }

        //我的排名信息(除了军团)
        Transform myInfoItem = f_GetObject("MyInfoItem").transform;
        mMyRankInfoExcept = myInfoItem.Find("Common");
        mMyHeadIcon      = mMyRankInfoExcept.Find("SelfIcon").GetComponent<UI2DSprite>();
        mMyHeadFrame     = mMyRankInfoExcept.Find("SelfFrame").GetComponent<UISprite>();
        mobjMyFightPower = mMyRankInfoExcept.Find("Sprite_FightPower").gameObject;
        mLabelMyPower    = mobjMyFightPower.transform.Find("SelfPowerLabel").GetComponent<UILabel>();
        mLabelMyRank     = mMyRankInfoExcept.Find("SelfRankLabel").GetComponent<UILabel>();
        mLabelMyName     = mMyRankInfoExcept.Find("SelfNameLabel").GetComponent<UILabel>();
        mLabelMyLv       = mMyRankInfoExcept.Find("SelfLabelLv").GetComponent<UILabel>();
        mobjMyStar       = mMyRankInfoExcept.Find("Sprite_Star").gameObject;
        mLabelMyStarNum  = mobjMyStar.transform.Find("StarNumLabel").GetComponent<UILabel>();
        mLabelMyChapterInfo = mobjMyStar.transform.Find("ChapterInfoLabel").GetComponent<UILabel>();
        mLabelMyGuildName   = mMyRankInfoExcept.Find("SelfLegionLabel").GetComponent<UILabel>();
        mLabelMyBepraised   = mMyRankInfoExcept.Find("SelfBepraisedTimesLabel").GetComponent<UILabel>();
        mLabelMyRamainBepraised = mMyRankInfoExcept.Find("SelfBepraisedTimesLabel/SelfPraiseTimesLabel").GetComponent<UILabel>();

        //我的排名信息（军团）
        mMyLegionRankInfo       = myInfoItem.Find("Legion");               
        mUnJoinLegion           = mMyLegionRankInfo.Find("Label_UnJoin").gameObject;
        mMyLegionUnRank         = mMyLegionRankInfo.Find("Label_UnRank").gameObject;
        mMyLegionInfo           = mMyLegionRankInfo.Find("LegionInfo").gameObject;
        mLabelMyLegionPower     = mMyLegionInfo.transform.Find("Sprite_FightPower/SelfPowerLabel").GetComponent<UILabel>();
        mMyLegionHeadIcon       = mMyLegionInfo.transform.Find("SelfIcon").GetComponent<UI2DSprite>();
        mMyLegionHeadFrame      = mMyLegionInfo.transform.Find("SelfFrame").GetComponent<UISprite>();        
        mLabelMyLegionRank      = mMyLegionInfo.transform.Find("SelfRankLabel").GetComponent<UILabel>();
        mLabelMyLegionLv        = mMyLegionInfo.transform.Find("SelfLabelLv").GetComponent<UILabel>();      
        mLabelMyLegionGuildName = mMyLegionInfo.transform.Find("SelfLegionLabel").GetComponent<UILabel>();

        //滑动面板相关
        mScrollView = f_GetObject("ScrollView").GetComponent<UIScrollView>();
        mScrollView.onDragFinished = f_OnMomnetEnds;
        mRankItemParent = f_GetObject("Grid");
        mRankItem = f_GetObject("CommonTab");

        //左边选中信息
        mRoleParent = f_GetObject("RoleParent").transform;
        mHeadIcon = f_GetObject("HeadIcon").GetComponent<UI2DSprite>();
        mHeadFrame = f_GetObject("HeadFrame").GetComponent<UISprite>();
        mLvLabel = f_GetObject("LvLabel").GetComponent<UILabel>();
        mPowerLabel = f_GetObject("PowerLabel").GetComponent<UILabel>();
        mPlayerNameLabel = f_GetObject("PlayerNameLabel").GetComponent<UILabel>();

        f_RegClickEvent("BtnBack", OnClose);
    }


    /// <summary>
    /// 打开界面
    /// </summary>
    /// <param name="e">EquipSythesis类似</param>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
		glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.UI_Rank);
        List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
        listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Money);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Energy);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, listMoneyType);

        EM_RankListType rankType = EM_RankListType.Arena;
        if (null != e)
        {
            rankType = (EM_RankListType)e;
        }
        UpdateInfoByRankType(rankType);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
    }

    /// <summary>
    /// 关闭事件
    /// </summary>
    /// <param name="go"></param>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    private void OnClose(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RankListPage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
    }

    /// <summary>
    /// 排行榜类型按钮点击事件
    /// </summary>
    /// <param name="go"></param>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    private void OnClickRankTypeBtn(GameObject go, object value1, object value2)
    {
        UpdateInfoByRankType((EM_RankListType)value1);
    }

    /// <summary>
    /// 拖拉到底部处理函数
    /// </summary>
    private void f_OnMomnetEnds()
    {
        Vector3 constraint = mScrollView.panel.CalculateConstrainOffset(mScrollView.bounds.min, mScrollView.bounds.min);
        if (constraint.y <= 0)
        {
            mFirstPage = false;            
            f_GetDataByType(mCurRankType);
        }
    }

    /// <summary>
    /// 点赞
    /// </summary>
    /// <param name="go"></param>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    private void f_OnPraiseBtnClick(GameObject go, object value1, object value2)
    {
        RankListPoolDT tNode = (RankListPoolDT)value1;
        if (tNode.AlreadyPraise)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1230));
            return;
        }
        else if (tNode.PlayerInfo == null)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1231));
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_PowerPraise;
        socketCallbackDt.m_ccCallbackFail = f_Callback_PowerPraise;
        Data_Pool.m_RankListPool.f_PowerPraise(tNode.PlayerInfo.iId, socketCallbackDt);
    }

    private void f_Callback_PowerPraise(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1232), RankListPool.PraiseAwardSyceeNum));
        }
        else
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1233) + (int)result);
        }
        mRankWrapComponet.f_UpdateView();
        f_UpdateMyRankInfo(Data_Pool.m_RankListPool.f_GetSelfPoolDt(EM_RankListType.Power));
    }

    /// <summary>
    /// 根据排行榜类型更新排行榜信息
    /// </summary>
    /// <param name="rankType"></param>
    private void UpdateInfoByRankType(EM_RankListType rankType) {
        //更新类型按钮状态
        mCurRankType = rankType;
        for (var i = EM_RankListType.Power; i < EM_RankListType.End; i++)
        {
            mSelectedRankTypeBtnList[(int)i].SetActive(i == rankType);
        }

        //根据排行榜类型更新滑动面板信息
        mFirstPage = true;
        f_GetDataByType(rankType);
    }

    /// <summary>
    /// 根据类型获取排行榜数据
    /// </summary>
    /// <param name="rankType"></param>
    private void f_GetDataByType(EM_RankListType rankType)
    {
        UITool.f_OpenOrCloseWaitTip(true, true);
        if (rankType == EM_RankListType.Arena)
        {            
            Data_Pool.m_ArenaPool.f_ExecuteAfterInitRankList(f_CallbackAfterInitRankList);
        }
        else if (rankType == EM_RankListType.Duplicate || rankType == EM_RankListType.Legion || rankType == EM_RankListType.Level || rankType == EM_RankListType.Power)
        {
            Data_Pool.m_RankListPool.f_ExecuteAfterRankList(rankType, mFirstPage, f_CallbackAfterInitRankList);
        }
        else if (rankType == EM_RankListType.RunningMan)
        {
            Data_Pool.m_RunningManPool.f_ExecuteAferRankList(mFirstPage, f_CallbackAfterInitRankList);
        }
        else
        {
MessageBox.ASSERT("Rank does not exist：" + rankType);
            return;
        }
    }

    /// <summary>
    /// 请求服务器数据回调
    /// </summary>
    /// <param name="value"></param>
    private void f_CallbackAfterInitRankList(object value)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        int result = (int)value;
        if (result == (int)eMsgOperateResult.OR_Succeed)
        { 
            if (mFirstPage) {
                //如果是第一页则更新滑动列表数据
                List<BasePoolDT<long>> _arenaList = null;
                BasePoolDT<long> myRankDt = null;
                mlastSelectedItem = null;
                if (mCurRankType == EM_RankListType.Arena)
                {
                    _arenaList = Data_Pool.m_ArenaPool.f_GetRankList();
                    myRankDt = Data_Pool.m_ArenaPool.f_GetMyRankData();
                }
                else if (mCurRankType == EM_RankListType.Duplicate || mCurRankType == EM_RankListType.Legion || mCurRankType == EM_RankListType.Level || mCurRankType == EM_RankListType.Power)
                {
                    _arenaList = Data_Pool.m_RankListPool.f_GetRankList(mCurRankType);
                    myRankDt = Data_Pool.m_RankListPool.f_GetSelfPoolDt(mCurRankType);
                }
                else if (mCurRankType == EM_RankListType.RunningMan)
                {
                    _arenaList = Data_Pool.m_RunningManPool.m_RankList;
                    myRankDt = Data_Pool.m_RunningManPool.f_GetMyRankData();
                }
                else
                {
MessageBox.ASSERT("Rank does not exist：" + mCurRankType);
                    return;
                }

                //刷新第一名信息
                if (_arenaList.Count > 0)
                {
                    f_UpdateLeftDisplayRankInfo(_arenaList[0]);
                }

                //更新列表数据
                mRankWrapComponet.f_UpdateList(_arenaList);
                mRankWrapComponet.f_ResetView();

                //根据排行榜类型更新我的排行榜信息
                f_UpdateMyRankInfo(myRankDt);
            }
            mRankWrapComponet.f_UpdateView();
        }
        else
        {
            UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(746) + value);
        }
    }

    /// <summary>
    /// 更新滑动列表
    /// </summary>
    /// <param name="tf"></param>
    /// <param name="dt"></param>
    private void f_RankItemUpdateByInfo(Transform tf, BasePoolDT<long> dt)
    {
        int rank = 0;
        BasePlayerPoolDT basePlayerDt = f_GetBasePlayerDt(dt,ref rank);
        if (null == basePlayerDt) return;

        //获取预设元素
        UI2DSprite headIcon = tf.Find("HeadIcon").GetComponent<UI2DSprite>();
        UISprite headFrame = tf.Find("HeadFrame").GetComponent<UISprite>();
        GameObject objFightPower = tf.Find("Sprite_FightPower").gameObject;
        UILabel labelPower = objFightPower.transform.Find("PowerLabel").GetComponent<UILabel>();
        UISprite spriteRank = tf.Find("Sprite_Rank").GetComponent<UISprite>();
        UILabel labelRank = tf.Find("RankLabel").GetComponent<UILabel>();
        UILabel labelName = tf.Find("NameLabel").GetComponent<UILabel>();
        UILabel labelGuildName = tf.Find("LegionLabel").GetComponent<UILabel>();
        UILabel labelBepraised = tf.Find("BepraisedTimesLabel").GetComponent<UILabel>();
        GameObject spriteBepraiseBtn = labelBepraised.transform.Find("Fabulous").gameObject;
        GameObject objSelectedFlag = tf.Find("Sprite_Select").gameObject;
        GameObject objStar = tf.Find("Sprite_Star").gameObject;
        UILabel labelStarNum = objStar.transform.Find("StarNumLabel").GetComponent<UILabel>();
        UILabel labelChapterInfo = objStar.transform.Find("ChapterInfoLabel").GetComponent<UILabel>();
        UILabel labelLv = tf.Find("LabelLv").GetComponent<UILabel>();

        //更新选中效果
        bool isSelected = dt == mCurSelData;
        objSelectedFlag.SetActive(isSelected);
        if (isSelected) mlastSelectedItem = objSelectedFlag;

        //设置排名
        spriteRank.gameObject.SetActive(rank <= 3);
        labelRank.gameObject.SetActive(rank > 3);
        if (rank > 3)
        {
            labelRank.text = rank.ToString();
        }
        else
        {
            spriteRank.spriteName = "phb_pic_" + rank;
        }
		spriteRank.MakePixelPerfect();
        //设置玩家名字
        labelName.text = basePlayerDt.m_szName;
        labelName.gameObject.SetActive(mCurRankType != EM_RankListType.Legion);

        //设置军团名字
        Vector3 legionNamePos = mCurRankType == EM_RankListType.Legion ? new Vector3(-78.5f, 45.7f) : new Vector3(95f, 45.7f);
        labelGuildName.transform.localPosition = legionNamePos;
        labelGuildName.text = basePlayerDt.m_szLegion == "" ? "" : "【" + basePlayerDt.m_szLegion + "】";
      
        //设置头像信息
        headIcon.sprite2D = UITool.f_GetIconSpriteBySexId(basePlayerDt.m_iSex);
        if(mCurRankType != EM_RankListType.Legion)
        f_RegClickEvent(headIcon.gameObject, OnPlayerIconClick, basePlayerDt);
        string temp = "";
        int iFrame = basePlayerDt.m_iFrameId;
        if (basePlayerDt.m_iFrameId <= 0)
        {
            iFrame = (int)EM_Important.White;
        }
        headFrame.spriteName = UITool.f_GetImporentColorName(iFrame, ref temp);

        //设置玩家等级
        labelLv.gameObject.SetActive(mCurRankType == EM_RankListType.Arena || mCurRankType == EM_RankListType.Level || mCurRankType == EM_RankListType.Legion);
        labelLv.text = "LV." + basePlayerDt.m_iLv;
        Vector3 lvPos = mCurRankType == EM_RankListType.Legion ? new Vector3(200, 36.5f) : new Vector3(-78.5f, -30);
        labelLv.transform.localPosition = lvPos;

        //设置战力
        objFightPower.SetActive(mCurRankType == EM_RankListType.Power || mCurRankType == EM_RankListType.Legion);
        labelPower.text = basePlayerDt.m_iBattlePower.ToString();

        //根据排行榜类型设置信息
        labelBepraised.gameObject.SetActive(mCurRankType == EM_RankListType.Power);
        objStar.SetActive(mCurRankType == EM_RankListType.Duplicate || mCurRankType == EM_RankListType.RunningMan);
        labelStarNum.text = basePlayerDt.m_iDungeonStars.ToString();
        if (mCurRankType == EM_RankListType.Power)
        {    
            //设置点赞数      
            RankListPoolDT rankListDT = dt as RankListPoolDT;
            if (null == rankListDT) return;
            labelBepraised.text = string.Format(CommonTools.f_GetTransLanguage(1225), rankListDT.PraiseTimes);
            f_RegClickEvent(spriteBepraiseBtn, f_OnPraiseBtnClick, rankListDT);
        }
        else if (mCurRankType == EM_RankListType.RunningMan)
        {
            //设置勇闯星数
            RunningManRankPoolDT runningManRankPoolDT = dt as RunningManRankPoolDT;
            if (null == runningManRankPoolDT) return;
            labelStarNum.text = runningManRankPoolDT.m_iStarNum.ToString();
            labelChapterInfo.text = "";
        }
        else if (mCurRankType == EM_RankListType.Duplicate)
        {
            //设置副本章节
            RankListPoolDT rankListDT = dt as RankListPoolDT;
            if (null == rankListDT) return;
            DungeonChapterDT ChapterTemplate = (DungeonChapterDT)glo_Main.GetInstance().m_SC_Pool.m_DungeonChapterSC.f_GetSC(rankListDT.CurChapterId);
            if (null == ChapterTemplate) return;
            labelChapterInfo.text = string.Format(CommonTools.f_GetTransLanguage(644), rankListDT.CurChapterId) + ChapterTemplate.szChapterName;
        }
        else if (mCurRankType == EM_RankListType.Legion)
        {
            //设置军团头像和等级
            RankListLegionPoolDT legionPoolDT = dt as RankListLegionPoolDT;
            if (null == legionPoolDT) return;
            headIcon.sprite2D = UITool.f_GetIconSprite(legionPoolDT.LegionInfo.f_GetProperty((int)EM_LegionProperty.Icon));
            labelLv.text = "LV." + legionPoolDT.LegionInfo.f_GetProperty((int)EM_LegionProperty.Lv);
            labelPower.text = legionPoolDT.LegionPower.ToString();
        }
    }

    /// <summary>
    /// 点击滑动列表
    /// </summary>
    /// <param name="tf"></param>
    /// <param name="dt"></param>
    private void f_OnRankItemClick(Transform tf, BasePoolDT<long> dt)
    {
        //已经选中，则弹出玩家详细信息界面
        GameObject objSelectedFlag = tf.Find("Sprite_Select").gameObject;
        if (mlastSelectedItem == objSelectedFlag)
        {
            int rank = 0;
            BasePlayerPoolDT basePlayerDt = f_GetBasePlayerDt(dt, ref rank);
            if (null == basePlayerDt) return;
            OnPlayerIconClick(null, basePlayerDt, null);
            return;
        }

        //设置选中效果
        if (null != mlastSelectedItem) mlastSelectedItem.SetActive(false);
        mlastSelectedItem = objSelectedFlag;
        mlastSelectedItem.SetActive(true);
        f_UpdateLeftDisplayRankInfo(dt);
    }

    /// <summary>
    /// 更新我的展示信息
    /// </summary>
    private void f_UpdateMyRankInfo(BasePoolDT<long> dt)
    {
        //根据是否军团设置显隐
        bool isLegion = mCurRankType == EM_RankListType.Legion;
        mMyRankInfoExcept.gameObject.SetActive(!isLegion);
        mMyLegionRankInfo.gameObject.SetActive(isLegion);        
        if (isLegion)
        {
            RankListLegionPoolDT legionPoolDT = dt as RankListLegionPoolDT;
            mMyLegionInfo.SetActive(null != legionPoolDT);
            mUnJoinLegion.SetActive(null == legionPoolDT);
            mMyLegionUnRank.SetActive(null == legionPoolDT);
            if (null == legionPoolDT) return;

            //设置排名
            int rank = legionPoolDT.Rank;
            mMyLegionUnRank.SetActive(rank <= 0);
            mLabelMyLegionRank.text = rank > 0 ? string.Format(CommonTools.f_GetTransLanguage(1506), rank) : CommonTools.f_GetTransLanguage(1037);

            //设置军团名字
            mLabelMyLegionGuildName.text = legionPoolDT.LegionInfo.LegionName == "" ? "" : "【" + legionPoolDT.LegionInfo.LegionName + "】";

            //设置头像信息           
            mMyLegionHeadIcon.sprite2D = UITool.f_GetIconSprite(legionPoolDT.LegionInfo.f_GetProperty((int)EM_LegionProperty.Icon));
            //f_RegClickEvent(mMyLegionHeadIcon.gameObject, OnPlayerIconClick, basePlayerDt);
            string temp = "";
            int iFrame = legionPoolDT.LegionInfo.f_GetProperty((int)EM_LegionProperty.Frame);
            if (iFrame <= 0)
            {
                iFrame = (int)EM_Important.White;
            }
            mMyLegionHeadFrame.spriteName = UITool.f_GetImporentColorName(iFrame, ref temp);

            //设置等级
            mLabelMyLegionLv.text = "LV." + legionPoolDT.LegionInfo.f_GetProperty((int)EM_LegionProperty.Lv);

            //设置战力
            mLabelMyLegionPower.text = legionPoolDT.LegionPower.ToString();
        }
        else
        {
            int rank = 0;
            BasePlayerPoolDT basePlayerDt = f_GetBasePlayerDt(dt, ref rank);
            if (null == basePlayerDt) return;

            //设置排名
            mLabelMyRank.text = rank > 0 ? string.Format(CommonTools.f_GetTransLanguage(1506), rank) : CommonTools.f_GetTransLanguage(1037);

            //设置玩家名字
            mLabelMyName.text = basePlayerDt.m_szName;

            //设置军团名字
            mLabelMyGuildName.text = basePlayerDt.m_szLegion == "" ? "" : "【" + basePlayerDt.m_szLegion + "】";

            //设置头像信息
            mMyHeadIcon.sprite2D = UITool.f_GetIconSpriteBySexId(basePlayerDt.m_iSex);
            f_RegClickEvent(mMyHeadIcon.gameObject, OnPlayerIconClick, basePlayerDt);
            string temp = "";
            int iFrame = basePlayerDt.m_iFrameId;
            if (basePlayerDt.m_iFrameId <= 0)
            {
                iFrame = (int)EM_Important.White;
            }
            mMyHeadFrame.spriteName = UITool.f_GetImporentColorName(iFrame, ref temp);

            //设置玩家等级
            //mLabelMyLv.gameObject.SetActive(mCurRankType == EM_RankListType.Arena || mCurRankType == EM_RankListType.Level);
            mLabelMyLv.text = "LV." + basePlayerDt.m_iLv;

            //设置战力
            mobjMyFightPower.SetActive(mCurRankType == EM_RankListType.Power);
            mLabelMyPower.text = basePlayerDt.m_iBattlePower.ToString();

            //根据排行榜类型设置信息
            mLabelMyBepraised.gameObject.SetActive(mCurRankType == EM_RankListType.Power);
            mobjMyStar.SetActive(mCurRankType == EM_RankListType.Duplicate || mCurRankType == EM_RankListType.RunningMan);
            mLabelMyStarNum.text = basePlayerDt.m_iDungeonStars.ToString();
            if (mCurRankType == EM_RankListType.Power)
            {
                RankListPoolDT rankListDT = dt as RankListPoolDT;
                if (null == rankListDT) return;
                mLabelMyBepraised.text = string.Format(CommonTools.f_GetTransLanguage(1225), rankListDT.PraiseTimes);
                mLabelMyRamainBepraised.text = CommonTools.f_GetTransLanguage(2198) +" "+ (RankListPool.SelfPraiseTimesLimit - Data_Pool.m_RankListPool.SelfPraiseTimes);
            }
            else if (mCurRankType == EM_RankListType.RunningMan)
            {
                RunningManRankPoolDT runningManRankPoolDT = dt as RunningManRankPoolDT;
                if (null == runningManRankPoolDT) return;
                mLabelMyStarNum.text = runningManRankPoolDT.m_iStarNum.ToString();
                mLabelMyChapterInfo.text = "";
            }
            else if (mCurRankType == EM_RankListType.Duplicate)
            {
                RankListPoolDT rankListDT = dt as RankListPoolDT;
                if (null == rankListDT) return;
                DungeonChapterDT ChapterTemplate = (DungeonChapterDT)glo_Main.GetInstance().m_SC_Pool.m_DungeonChapterSC.f_GetSC(rankListDT.CurChapterId);
                if (null == ChapterTemplate) return;
                mLabelMyChapterInfo.text = string.Format(CommonTools.f_GetTransLanguage(644), rankListDT.CurChapterId) + ChapterTemplate.szChapterName;
            }           
        }       
    }

    /// <summary>
    /// 更新左边的展示信息
    /// </summary>
    private void f_UpdateLeftDisplayRankInfo(BasePoolDT<long> dt) {
        mCurSelData = dt;
        int rank = 0;
        BasePlayerPoolDT basePlayerDt = f_GetBasePlayerDt(dt,ref rank);
        if (null == basePlayerDt) return;

        //设置基础信息
        mLvLabel.text = basePlayerDt.m_iLv.ToString();
        mPowerLabel.text = basePlayerDt.m_iBattlePower.ToString();
        mPlayerNameLabel.text = basePlayerDt.m_szName;

        //加载模型和头像
        UITool.f_CreateRoleByCardId(basePlayerDt.m_CardId, ref mRole,mRoleParent , 1);
        mHeadIcon.sprite2D = UITool.f_GetIconSpriteBySexId(basePlayerDt.m_iSex);
        f_RegClickEvent(mHeadIcon.gameObject, OnPlayerIconClick, basePlayerDt);
        string temp = "";
        int iFrame = basePlayerDt.m_iFrameId;
        if (basePlayerDt.m_iFrameId <= 0)
        {
            iFrame = (int)EM_Important.White;
        }
        mHeadFrame.spriteName = UITool.f_GetImporentColorName(iFrame, ref temp);
    }

    /// <summary>
    /// 根据排行榜类型获取玩家基础数据
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    private BasePlayerPoolDT f_GetBasePlayerDt(BasePoolDT<long> dt,ref int rank)
    {
        if (mCurRankType == EM_RankListType.Arena)
        {
            ArenaPoolDT arenaPoolDT = dt as ArenaPoolDT;
            if (null == arenaPoolDT) return null;
            rank = arenaPoolDT.m_iRank;
            return arenaPoolDT.m_PlayerInfo;
        }
        else if (mCurRankType == EM_RankListType.Duplicate || mCurRankType == EM_RankListType.Level || mCurRankType == EM_RankListType.Power)
        {
            RankListPoolDT rankListDT = dt as RankListPoolDT;
            if (null == rankListDT) return null;
            rank = rankListDT.Rank;
            return rankListDT.PlayerInfo;
        }
        else if (mCurRankType == EM_RankListType.Legion) 
        {
            //军团返回军团长部分信息（没有BasePlayerPoolDT完整信息，只有卡牌id,等级，战力,名称以及军团名！！！）
            RankListLegionPoolDT legionPoolDT = dt as RankListLegionPoolDT;
            if (null == legionPoolDT) return null;
            rank = legionPoolDT.Rank;
            return legionPoolDT.m_mChiefInfo;
        }
        else if (mCurRankType == EM_RankListType.RunningMan) 
        {
            RunningManRankPoolDT runningManRankPoolDT = dt as RunningManRankPoolDT;
            if (null == runningManRankPoolDT) return null;
            rank = runningManRankPoolDT.m_iRank;
            return runningManRankPoolDT.m_PlayerInfo;
        }
        return null;
    }

    /// <summary>
    /// 点击玩家头像
    /// </summary>
    private void OnPlayerIconClick(GameObject go, object obj1, object obj2)
    {
        //如果是自己则不弹出详细信息界面
        BasePlayerPoolDT tData = (BasePlayerPoolDT)obj1;
        if (tData.iId == Data_Pool.m_UserData.m_iUserId) return;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LookPlayerInfoPage, UIMessageDef.UI_OPEN, tData);
    }
}
