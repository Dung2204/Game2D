using ccU3DEngine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaPageNew : UIFramwork
{
    [HideInInspector]
    public static string ArenaFakePlayerPowerKey = "ArenaFakePlayerPower";
    [HideInInspector]
    public static string ArenaFakePlayerNameKey = "ArenaFakePlayerName";

    private const int NewGuideId = 2004; //竞技场新手引导id
    private const int SWEEP_MAX_NUM = 5;
    private const int ItemNumsOfOneDragPage = 4; //一页（屏）包含多少个选项卡（优化后，把以前n个选项卡加上背景作为一个整体）
    private GameObject mBtn_Back;
    private GameObject mBtn_FameShop;
    private GameObject mBtn_Lineup;
    private GameObject mBtn_RankPage;

    private UILabel mFameLabel;
    private UILabel mRankLabel;
    private UISprite mRankSprite;
    private UILabel mArenaCostLabel;
    private UILabel mRankAwardNull;

    private UIGrid mAwardGrid;
    private GameObject mAwardItem;
    private ResourceCommonItemComponent _awardShowComponent;
    private ResourceCommonItemComponent mAwardShowComponent
    {
        get
        {
            if (_awardShowComponent == null)
                _awardShowComponent = new ResourceCommonItemComponent(mAwardGrid, mAwardItem);
            return _awardShowComponent;
        }
    }

    private float adaptiveScale = ScreenControl.Instance.mScaleRatio;
    private UIScrollView mArenaScrollView;
    private float mScrollPercent;
    private UISlider mArenaScrollViewSlider;
    private UIGrid mArenaGrid;
    private List<ArenaItemNew> mItemList;
    private const int ArenaItemMaxNum = 24;
    private List<BasePoolDT<long>> mArenaList;

    private UILabel mSelfName;
    private UILabel mSelfPower;
    private UILabel mSelfLevel;
    private UI2DSprite mSelfIcon;
    private string strTexBgRoot = "UI/TextureRemove/Arena/Texture_ArenaBg";

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.Arena);
        InitGUI();
    }

    protected override void InitGUI()
    {
        mBtn_Back = f_GetObject("Btn_Back");
        mBtn_FameShop = f_GetObject("Btn_FameShop");
        mBtn_Lineup = f_GetObject("Btn_Lineup");
        mBtn_RankPage = f_GetObject("Btn_RankPage");
        f_RegClickEvent(mBtn_Back, f_BackHandle);
        f_RegClickEvent("Btn_Help", f_OnHelpBtnClick);
        f_RegClickEvent(mBtn_FameShop, f_FameShopHandle);
        f_RegClickEvent(mBtn_Lineup, f_LineupHandle);
        f_RegClickEvent(mBtn_RankPage, f_RankPageHandle);
        mFameLabel = f_GetObject("FameLabel").GetComponent<UILabel>();
        mRankLabel = f_GetObject("Label_MyRank").GetComponent<UILabel>();
        mRankSprite = f_GetObject("Sprite_MyRank").GetComponent<UISprite>();
        mArenaCostLabel = f_GetObject("ArenaCostLabel").GetComponent<UILabel>();
        mAwardGrid = f_GetObject("AwardGrid").GetComponent<UIGrid>();
        mAwardItem = f_GetObject("TaskAwardItem");
        mRankAwardNull = f_GetObject("RankAwardNull").GetComponent<UILabel>();

        //适配滑动面板，滑动item的背景布满屏幕，，为了在不同分辨率下布满屏幕且不变形，所以以大的比例同比放大（放大也不会使item重要的内容在屏幕外，背景图部分在屏幕外不关心！）
        mArenaScrollView = f_GetObject("ArenaScrollView").GetComponent<UIScrollView>();
        Vector2 UIScrollViewScale = mArenaScrollView.transform.GetComponent<UIPanel>().GetViewSize();
        GameObject arenaDragPageItemPrefab = f_GetObject("ArenaDragPageItem");
        Vector3 dragItemScale = Vector3.one;
        //MessageBox.ASSERT("" + ScreenControl.Instance.mFunctionRatio);
        //      if (ScreenControl.Instance.mFunctionRatio <= 0.85f)
        //      {
        //          UIScrollViewScale *= ScreenControl.Instance.mFunctionRatio;
        //          mArenaScrollView.transform.GetComponent<UIPanel>().SetRect(0, 0, UIScrollViewScale.x, 1080);

        //          mArenaGrid.cellWidth = UIScrollViewScale.x;
        //          // mArenaGrid.cellHeight = UIScrollViewScale.y;
        //          dragItemScale = new Vector3(arenaDragPageItemPrefab.transform.localScale.x * ScreenControl.Instance.mFunctionRatio, 1f, arenaDragPageItemPrefab.transform.localScale.z * ScreenControl.Instance.mFunctionRatio);
        //      }
        //      else
        //      {
        //          UIScrollViewScale *= adaptiveScale;
        //          mArenaScrollView.transform.GetComponent<UIPanel>().SetRect(0, 0, UIScrollViewScale.x, UIScrollViewScale.y);
        //          mArenaGrid = f_GetObject("ArenaGrid").GetComponent<UIGrid>();
        //          mArenaGrid.cellWidth = UIScrollViewScale.x;
        //          mArenaGrid.cellHeight = UIScrollViewScale.y;
        //          dragItemScale = arenaDragPageItemPrefab.transform.localScale * adaptiveScale;
        //      }



        mItemList = new List<ArenaItemNew>();
        mArenaScrollViewSlider = f_GetObject("ArenaScrollViewSlider").GetComponent<UISlider>();
        Texture2D firstArenaTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/TextureRemove/Arena/ArenaMap1");
        Texture2D top10ArenaTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/TextureRemove/Arena/ArenaMap2");
        Texture2D normalArenaTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/TextureRemove/Arena/ArenaMap3");
        //    for (int i = 0; i < ArenaItemMaxNum / ItemNumsOfOneDragPage; i++)
        //    {
        //        //优化后，把以前ItemNumsOfOneDragPage个选项卡加上背景（占一个屏）作为一个整体
        //        GameObject arenaDragPageItem = NGUITools.AddChild(mArenaGrid.gameObject, arenaDragPageItemPrefab);
        //        arenaDragPageItem.name = "ArenaDragPageItem" + (i + 1);
        //        arenaDragPageItem.transform.localScale = dragItemScale;
        //        NGUITools.MarkParentAsChanged(arenaDragPageItem);
        //        UITexture Texture_BG = arenaDragPageItem.transform.Find("Bg").GetComponent<UITexture>();           
        //        Texture_BG.mainTexture = i == 0 ? firstArenaTexture2D : i < 3 ? top10ArenaTexture2D : normalArenaTexture2D ;
        //        for (int j = 0; j < ItemNumsOfOneDragPage; j++) {
        //            GameObject arenaItem = arenaDragPageItem.transform.Find("ArenaItem"+j.ToString()).gameObject;
        //if(ScreenControl.Instance.mFunctionRatio <= 0.85f)
        //{
        //	arenaItem.transform.localScale = new Vector3(1.1f,1f,1f);
        //}
        //if(ScreenControl.Instance.mFunctionRatio <= 0.78f)
        //{
        //	arenaItem.transform.localScale = new Vector3(1.2f,1f,1f);
        //}
        //            mItemList.Add(ArenaItemNew.f_Create(arenaDragPageItem,arenaItem));
        //            if (i == 0) {
        //                //竞技场前两名单独一屏，设置前两名位置
        //                if (j == 0)
        //                {
        //                    arenaItem.transform.localPosition = new Vector3(615, 115);
        //                }
        //                else if (j == 1)
        //                {
        //                    arenaItem.transform.localPosition = new Vector3(115, -285);
        //                    break;
        //                }                    
        //            }
        //        }            
        //    }
        mArenaCostLabel.text = string.Format(CommonTools.f_GetTransLanguage(743), GameParamConst.ArenaVigorCost);
        mArenaGrid = f_GetObject("ArenaGrid").GetComponent<UIGrid>();
        mArenaGrid.onReposition = f_ArenaGridReposition;
        mSelfName = f_GetObject("SelfName").GetComponent<UILabel>();
        mSelfPower = f_GetObject("SelfPower").GetComponent<UILabel>();
        mSelfLevel = f_GetObject("SelfLevel").GetComponent<UILabel>();
        mSelfIcon = f_GetObject("SelfIcon").GetComponent<UI2DSprite>();


    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);

        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.Arena);
        //断线重连如果切去其他界面，这时候不处理，服务器消息返回再打开界面就会和其他界面重叠
        if (!UITool.f_IsCanOpenChallengePage(UINameConst.ArenaPageNew))
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.ArenaPageNew, UIMessageDef.UI_CLOSE);
            return;
        }

        Data_Pool.m_GuidancePool.OpenLevelPageUIName = UINameConst.ArenaPageNew;
        Data_Pool.m_GuidancePool.IsUpdateArena = false;
        PlayerPrefs.SetString(ArenaFakePlayerNameKey, "");

        //货币
        List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
        listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Vigor);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Fame);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, listMoneyType);

        //根据排名显示奖励
        List<AwardPoolDT> tList = Data_Pool.m_AwardPool.f_GetArenaRankAward(Data_Pool.m_ArenaPool.m_iRank);
        mAwardShowComponent.f_Show(tList, EM_CommonItemShowType.All, EM_CommonItemClickType.AllTip, this);
        mFameLabel.text = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Fame).ToString();
        if (!Data_Pool.m_ArenaPool.m_IsOnRank)
        {
            mRankAwardNull.text = string.Empty;
            mRankLabel.text = CommonTools.f_GetTransLanguage(744);
        }
        else
        {
            if (tList.Count > 0)
                mRankAwardNull.text = string.Empty;
            else
                mRankAwardNull.text = CommonTools.f_GetTransLanguage(745);
            int myRank = Data_Pool.m_ArenaPool.m_iRank;
            if (myRank <= 3)
            {
                mRankLabel.gameObject.SetActive(false);
                mRankSprite.gameObject.SetActive(true);
                mRankSprite.spriteName = string.Format("Tex_{0}st", myRank);
                mRankSprite.MakePixelPerfect();
            }
            else
            {
                mRankLabel.gameObject.SetActive(true);
                mRankSprite.gameObject.SetActive(false);
                mRankLabel.text = myRank.ToString();
            }

        }
        f_ShowArenaList(e);
    }

    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        //货币
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.Arena);
        List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
        listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Vigor);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Fame);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, listMoneyType);
    }

    protected override void UI_HOLD(object e)
    {
        base.UI_HOLD(e);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
    }

    private void f_ShowArenaList(object value)
    {
        //更新玩家列表
        f_UpdateByArenaList(Data_Pool.m_ArenaPool.f_GetArenaList());

        //检查是否需要打开突破奖励界面
        if (Data_Pool.m_ArenaPool.mBreakRankInfo.m_bShowInfo)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.ArenaBreakAwardPage, UIMessageDef.UI_OPEN);
        }

        //先打开，自己请求数据后再刷新界面
        if (value != null && value is bool)
        {
            bool updateBySelf = (bool)value;
            if (updateBySelf)
                f_UpdateAfterRequest();
        }
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);

    }

    /// <summary>
    /// 更新玩家列表
    /// </summary>
    /// <param name="list"></param>
    private GameObject selfGo;
    private void f_UpdateByArenaList(List<BasePoolDT<long>> list)
    {
        System.DateTime start = System.DateTime.Now;
        mArenaList = list;
        mScrollPercent = 1;
MessageBox.ASSERT("Number of participants in the arena： " + list.Count);
        GridUtil.f_SetGridView<BasePoolDT<long>>(f_GetObject("ArenaGrid"), f_GetObject("ArenaItem"), mArenaList, OnShowItemData, true);
        ccTimeEvent.GetInstance().f_RegEvent(1f, false, null, (object obj1) => { mArenaGrid.GetComponent<UICenterOnChild>().CenterOn(selfGo.transform); });


        //设置滑动限制，前两名占一屏，其他每四人一屏，再加mArenaGrid.cellHeight / 25偏移量
        //mArenaScrollView.mCanDragLimitPos = Vector2.up * (list.Count * mArenaGrid.cellHeight / 4 - mArenaGrid.cellHeight / 2 + mArenaGrid.cellHeight / 25);
        //mArenaScrollView.CirticalValue2CorrectedPos = 15;
        //for (int i = 0; i < mItemList.Count; i++)
        //{
        //    if ((i - 2) % ItemNumsOfOneDragPage == 0 || i == 0)
        //    {
        //        mItemList[i].mParent.SetActive(i < mArenaList.Count);
        //        if (i >= mArenaList.Count)
        //            continue;
        //    } 
        //    if (i < mArenaList.Count)
        //    {
        //        ArenaPoolDT arenaPoolDT = (ArenaPoolDT)mArenaList[i];
        //        mItemList[i].f_UpdateByInfo( i, arenaPoolDT);
        //        f_RegClickEvent(mItemList[i].mChallengeBtn, f_ArenaItemChallengeClick, mArenaList[i], i);
        //        f_RegClickEvent(mItemList[i].mSweepBtn, f_ArenaItemSweepClick, mArenaList[i], i);
        //        if(mArenaList[i].iId == Data_Pool.m_UserData.m_iUserId)
        //        {
        //            mSelfName.text = arenaPoolDT.m_PlayerInfo.m_szName;
        //            mSelfPower.text = Data_Pool.m_TeamPool.f_GetTotalBattlePower().ToString();
        //            mSelfLevel.text = arenaPoolDT.m_PlayerInfo.m_iLv.ToString();
        //            mSelfIcon.sprite2D = UITool.f_GetCardIcon(arenaPoolDT.m_PlayerInfo.m_CardId);
        //            //UITool.f_GetIconSpriteByCardId(teamPoolDT.m_CardPoolDT)
        //            if (i + 1 == list.Count)
        //            {
        //                //我在最后一位则固定限制我刚好在最底下，要不会看到下面的站台
        //                mScrollPercent = 0.86f;
        //            }
        //            else
        //            {
        //                mScrollPercent = (float)i / list.Count;
        //            }                   
        //        }
        //    }
        //    else
        //        mItemList[i].f_UpdateByInfo(i, null);
        //}
        //mArenaGrid.repositionNow = true;
        Data_Pool.m_GuidancePool.IsUpdateArena = true;
MessageBox.ASSERT("Set arena interface time:" + (System.DateTime.Now - start).TotalSeconds);
    }

    private void OnShowItemData(GameObject go, BasePoolDT<long> basePoolDT)
    {
        ArenaPoolDT arenaPoolDT = (ArenaPoolDT)basePoolDT;
        ArenaItemNew arenaItemNew = go.transform.GetComponent<ArenaItemNew>();
        arenaItemNew.f_UpdateByInfo(arenaPoolDT);
        f_RegClickEvent(arenaItemNew.mChallengeBtn, f_ArenaItemChallengeClick, arenaPoolDT);
        f_RegClickEvent(arenaItemNew.mSweepBtn, f_ArenaItemSweepClick, arenaPoolDT);
        if (arenaPoolDT.iId == Data_Pool.m_UserData.m_iUserId)
        {
            mSelfName.text = arenaPoolDT.m_PlayerInfo.m_szName;
            mSelfPower.text = Data_Pool.m_TeamPool.f_GetTotalBattlePower().ToString();
            mSelfLevel.text = arenaPoolDT.m_PlayerInfo.m_iLv.ToString();
            mSelfIcon.sprite2D = UITool.f_GetIconSpriteBySexId(arenaPoolDT.m_PlayerInfo.m_iSex);
        }
        if (arenaPoolDT.iId == Data_Pool.m_UserData.m_iUserId)
        {
            selfGo = go;
        }
    }




    #region Btn_Handle

    private void f_BackHandle(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ArenaPageNew, UIMessageDef.UI_CLOSE);
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.UI_Challenge);
        ccUIHoldPool.GetInstance().f_UnHold();
    }

    private void f_FameShopHandle(GameObject go, object value1, object value2)
    {
        ccUIHoldPool.GetInstance().f_Hold(this);
        //打开商店
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopMutiCommonPage, UIMessageDef.UI_OPEN, EM_ShopMutiType.Reputation);
    }

    private void f_LineupHandle(GameObject go, object value1, object value2)
    {
        //打开布阵
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ClothArrayPage, UIMessageDef.UI_OPEN);
    }

    private void f_RankPageHandle(GameObject go, object value1, object value2)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_ArenaPool.f_ExecuteAfterInitRankList(f_CallbackAfterInitRankList);
    }

    private void f_CallbackAfterInitRankList(object value)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        int result = (int)value;
        if (result == (int)eMsgOperateResult.OR_Succeed)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.ArenaRankPage, UIMessageDef.UI_OPEN);
        }
        else
        {
            UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(746) + value);
        }
    }

    private void f_ArenaGridReposition()
    {
        mArenaScrollView.ResetPosition();
        mArenaScrollViewSlider.value = mScrollPercent;
    }

    #endregion

    #region ArenaItem
    /// <summary>
    /// 挑战某个玩家
    /// </summary>
    private void f_ArenaItemChallengeClick(GameObject go, object value1, object value2)
    {
        ArenaPoolDT node = (ArenaPoolDT)value1;
        //int tCurIdx = (int)value2;
        if (node.m_PlayerInfo.iId == Data_Pool.m_UserData.m_iUserId && Data_Pool.m_GuidancePool.IGuidanceID != NewGuideId)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(749));
            return;
        }
        else if (!UITool.f_IsEnoughMoney(EM_MoneyType.eUserAttr_Vigor, GameParamConst.ArenaVigorCost, true, true, this))
        {
            return;
        }
        else if (Data_Pool.m_ArenaPool.f_CheckChallengeRankLimit(node.m_iRank))
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(750));
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_ChallengePlayerSuc;
        socketCallbackDt.m_ccCallbackFail = f_ChallengePlayerFail;

        //发送挑战玩家信息
        if (Data_Pool.m_GuidancePool.IGuidanceID == NewGuideId)
        {
            Data_Pool.m_ArenaPool.f_ArenaChallenge(node.m_iRank, socketCallbackDt, node.m_PlayerInfo.m_iLv, node.m_PlayerInfo.m_iSex, 1);
        }
        else
        {
            Data_Pool.m_ArenaPool.f_ArenaChallenge(node.m_iRank, socketCallbackDt, node.m_PlayerInfo.m_iLv, node.m_PlayerInfo.m_iSex);
        }
    }

    /// <summary>
    /// 便捷挑战
    /// </summary>
    private void f_ArenaItemSweepClick(GameObject go, object value1, object value2)
    {
        ArenaPoolDT poolData = (ArenaPoolDT)value1;
        if (poolData.m_iRank < Data_Pool.m_ArenaPool.m_iRank)
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(751));
            return;
        }

        //设置便捷挑战界面参数
        HandilyChallengeParam parm = new HandilyChallengeParam();
        parm.mcallbackParam = (uint)poolData.m_iRank;
        parm.mCalcCanChallengeTimes = (int count, bool isAutoCost) => {
            //计算可挑战次数
            int hasNum = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Vigor);
            int vigorCost = count * GameParamConst.ArenaVigorCost;
            if (isAutoCost)
            {
                int vigorNum = Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(GameParamConst.VigorGoodId);
                BaseGoodsDT goodTemplate = (BaseGoodsDT)glo_Main.GetInstance().m_SC_Pool.m_BaseGoodsSC.f_GetSC(GameParamConst.VigorGoodId);
                hasNum += (goodTemplate.iEffectData * vigorNum);
            }
            if (hasNum >= vigorCost)
            {
                return count;
            }
            else
            {
                return hasNum / GameParamConst.ArenaVigorCost;
            }
        };
        parm.mCheckDesc = CommonTools.f_GetTransLanguage(2179);
        parm.mCostCountPerTime = GameParamConst.ArenaVigorCost;
        parm.mCostType = EM_MoneyType.eUserAttr_Vigor;
        parm.mHandilyChallengeCallback = (object callbackParam, int count, bool isAutoCost) =>
        {
            //便捷挑战界面选好挑战次数回调
            if (count <= 0)
            {
                int haveNum = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Vigor);
                if (haveNum < GameParamConst.ArenaVigorCost)
                {
                    UITool.f_IsEnoughMoney(EM_MoneyType.eUserAttr_Vigor, 0, true, true, this);
                }
                else
                {
                    //关闭便捷挑战界面
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.HandilyChallengePage, UIMessageDef.UI_CLOSE);
                }
                return;
            }
            //关闭便捷挑战界面
            ccUIManage.GetInstance().f_SendMsg(UINameConst.HandilyChallengePage, UIMessageDef.UI_CLOSE);
            //请求便捷挑战
            UITool.f_OpenOrCloseWaitTip(true);
            SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
            socketCallbackDt.m_ccCallbackSuc = f_ArenaSweepSuc;
            socketCallbackDt.m_ccCallbackFail = f_ArenaSweepFail;
            int autoCostVigor = isAutoCost ? 1 : 0;
            uint rank = (uint)callbackParam;
            byte challengeTimes = (byte)count;
            byte isAutoCostVigor = (byte)autoCostVigor;
            Data_Pool.m_ArenaPool.f_ArenaSweep((uint)callbackParam, (byte)count, (byte)autoCostVigor, socketCallbackDt);
        };

        //打开便捷挑战界面
        ccUIManage.GetInstance().f_SendMsg(UINameConst.HandilyChallengePage, UIMessageDef.UI_OPEN, parm);
    }

    private void f_ChallengePlayerSuc(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        //挑战成功切换场景
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ArenaPageNew, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.BattleMain);
    }

    private void f_OnHelpBtnClick(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonHelpPage, UIMessageDef.UI_OPEN, 14);
    }

    private void f_ChallengePlayerFail(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        //弹出错误
        UITool.UI_ShowFailContent(string.Format(CommonTools.f_GetTransLanguage(752), result));
    }

    private void f_ArenaSweepSuc(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        //打开扫荡界面（展示扫荡结果）
        ccUIManage.GetInstance().f_SendMsg(UINameConst.HandilyChallengeResultPage, UIMessageDef.UI_OPEN, result);
        //更新声望数量
        mFameLabel.text = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Fame).ToString();
    }

    private void f_ArenaSweepFail(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(753) + result);
    }

    #endregion


    private void f_UpdateAfterRequest()
    {
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_ArenaList;
        socketCallbackDt.m_ccCallbackFail = f_Callback_ArenaList;
        Data_Pool.m_ArenaPool.f_ArenaList(socketCallbackDt);
    }

    private void f_Callback_ArenaList(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            //更新玩家列表
            f_UpdateByArenaList(Data_Pool.m_ArenaPool.f_GetArenaList());
        }
        else
        {
            UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(754) + result);
        }
    }
}
