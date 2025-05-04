using ccU3DEngine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;

public class ArenaCrossPage : UIFramwork
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
    private GameObject mBtn_Record; 

    private UILabel mFameLabel;
    private UILabel mRankLabel;
    private UISprite mRankSprite;
    private UILabel mArenaCostLabel;
    private UILabel mRankAwardNull;
    public GameObject mRoleParent;
    GameObject mRole = null;

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
    private List<ArenaCrossItem> mItemList;
    private const int ArenaItemMaxNum = 24;
    private ArrayList mArenaList;

    private UILabel mSelfName;
    private UILabel mSelfPower;
    private UILabel mSelfLevel;
    private UILabel mLeftBattleTimes;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.Arena);
        InitGUI();
    }
    protected override void f_Create()
    {
        _InitReference();
        base.f_Create();
    }
    private void _InitReference()
    {
        AddGOReference("Panel/Anchor-Right/ArenaDragPageItem");
        AddGOReference("Panel/Anchor-Right/DragItem/ArenaScrollView");
        AddGOReference("Panel/Anchor-Right/DragItem/ArenaScrollView/ArenaGrid");
        AddGOReference("Panel/Anchor-Right/DragItem/ArenaScrollViewSlider");
        AddGOReference("Panel/Anchor-TopLeft/Btn_Back");
        AddGOReference("Panel/Anchor-BottomLeft/BtnGrid/Btn_FameShop");
        AddGOReference("Panel/Anchor-BottomLeft/BtnGrid/Btn_Lineup");
        AddGOReference("Panel/Anchor-BottomLeft/BtnGrid/Btn_RankPage");
        AddGOReference("Panel/Anchor-BottomLeft/BtnGrid/Btn_Help");
        AddGOReference("Panel/Anchor-BottomLeft/BtnGrid/Btn_Record");
        AddGOReference("Panel/Achor-Left/SelfInfo/AwardTitle/AwardGrid");
        AddGOReference("Panel/Achor-Left/SelfInfo/LabelParent/ArenaCostLabel");
        AddGOReference("Panel/Achor-Left/SelfInfo/LabelParent/FameLabel");
        AddGOReference("Panel/Achor-Left/SelfInfo/LabelParent/Label_MyRank");
        AddGOReference("Panel/Achor-Left/SelfInfo/AwardTitle/RankAwardNull");
        AddGOReference("Panel/Achor-Left/SelfInfo/LabelParent/SelfName");
        AddGOReference("Panel/Achor-Left/SelfInfo/LabelParent/SelfPower");
        AddGOReference("Panel/Achor-Left/SelfInfo/LabelParent/Sprite_MyRank");
        AddGOReference("Panel/Achor-Left/SelfInfo/LabelParent/SelfLevel");
        AddGOReference("Panel/Achor-Left/SelfInfo/RoleParent");
        AddGOReference("Panel/Achor-Left/SelfInfo/LeftBattleTimes");
        AddGOReference("Panel/Anchor-BottomLeft/BtnGrid/TaskAward");
        AddGOReference("Panel/Anchor-Right/Item");
    }

    protected override void InitGUI()
    {
        mBtn_Back = f_GetObject("Btn_Back");
        mBtn_FameShop = f_GetObject("Btn_FameShop");
        mBtn_Lineup = f_GetObject("Btn_Lineup");
        mBtn_RankPage = f_GetObject("Btn_RankPage");
        mBtn_Record = f_GetObject("Btn_Record");
        f_RegClickEvent(mBtn_Back, f_BackHandle);
        f_RegClickEvent("Btn_Help", f_OnHelpBtnClick);
        f_RegClickEvent(mBtn_FameShop, f_FameShopHandle);
        f_RegClickEvent(mBtn_Lineup, f_LineupHandle);
        f_RegClickEvent(mBtn_RankPage, f_RankPageHandle);
        f_RegClickEvent(mBtn_Record, f_RecordPageHandle);
        f_RegClickEvent("TaskAward", f_OpenCrossArenaTask);
        mFameLabel = f_GetObject("FameLabel").GetComponent<UILabel>();
        mRankLabel = f_GetObject("Label_MyRank").GetComponent<UILabel>();
        mRankSprite = f_GetObject("Sprite_MyRank").GetComponent<UISprite>();
        mArenaCostLabel = f_GetObject("ArenaCostLabel").GetComponent<UILabel>();
        mAwardGrid = f_GetObject("AwardGrid").GetComponent<UIGrid>();
        mAwardItem = f_GetObject("TaskAwardItem");
        mRankAwardNull = f_GetObject("RankAwardNull").GetComponent<UILabel>();
        mRoleParent = f_GetObject("RoleParent");

        //适配滑动面板，滑动item的背景布满屏幕，，为了在不同分辨率下布满屏幕且不变形，所以以大的比例同比放大（放大也不会使item重要的内容在屏幕外，背景图部分在屏幕外不关心！）
        mArenaScrollView = f_GetObject("ArenaScrollView").GetComponent<UIScrollView>();
        Vector2 UIScrollViewScale = mArenaScrollView.transform.GetComponent<UIPanel>().GetViewSize();
        GameObject arenaDragPageItemPrefab = f_GetObject("ArenaDragPageItem");
        Vector3 dragItemScale = Vector3.one;
        MessageBox.ASSERT("" + ScreenControl.Instance.mFunctionRatio);
        // if (ScreenControl.Instance.mFunctionRatio <= 0.85f)
        // {
            // UIScrollViewScale *= ScreenControl.Instance.mFunctionRatio;
            // mArenaScrollView.transform.GetComponent<UIPanel>().SetRect(0, 0, UIScrollViewScale.x, 1080);

            // mArenaGrid.cellWidth = UIScrollViewScale.x;
            // //mArenaGrid.cellHeight = UIScrollViewScale.y;
            // dragItemScale = new Vector3(arenaDragPageItemPrefab.transform.localScale.x * ScreenControl.Instance.mFunctionRatio, 1f, arenaDragPageItemPrefab.transform.localScale.z * ScreenControl.Instance.mFunctionRatio);
        // }
        // else
        // {
            // UIScrollViewScale *= adaptiveScale;
            // mArenaScrollView.transform.GetComponent<UIPanel>().SetRect(0, 0, UIScrollViewScale.x, UIScrollViewScale.y);
            mArenaGrid = f_GetObject("ArenaGrid").GetComponent<UIGrid>();
            mArenaGrid.cellWidth = UIScrollViewScale.x;
            mArenaGrid.cellHeight = 1100f;
            // dragItemScale = arenaDragPageItemPrefab.transform.localScale * adaptiveScale;
        // }



        mItemList = new List<ArenaCrossItem>();
        mArenaScrollViewSlider = f_GetObject("ArenaScrollViewSlider").GetComponent<UISlider>();
        Texture2D firstArenaTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/TextureRemove/Arena/ArenaMap1");
        Texture2D top10ArenaTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/TextureRemove/Arena/ArenaMap2");
        Texture2D normalArenaTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/TextureRemove/Arena/ArenaMap3");
            for (int i = 0; i < ArenaItemMaxNum / ItemNumsOfOneDragPage; i++)
            {
                //优化后，把以前ItemNumsOfOneDragPage个选项卡加上背景（占一个屏）作为一个整体
                GameObject arenaDragPageItem = NGUITools.AddChild(mArenaGrid.gameObject, arenaDragPageItemPrefab);
                arenaDragPageItem.name = "ArenaDragPageItem" + (i + 1);
                arenaDragPageItem.transform.localScale = dragItemScale;
                NGUITools.MarkParentAsChanged(arenaDragPageItem);
                UITexture Texture_BG = arenaDragPageItem.transform.Find("Bg").GetComponent<UITexture>();           
                Texture_BG.mainTexture = i == 0 ? firstArenaTexture2D : i < 3 ? top10ArenaTexture2D : normalArenaTexture2D ;
                for (int j = 0; j < ItemNumsOfOneDragPage; j++) {
                    GameObject arenaItem = arenaDragPageItem.transform.Find("ArenaItem"+j.ToString()).gameObject;
        if(ScreenControl.Instance.mFunctionRatio <= 0.85f)
        {
        	arenaItem.transform.localScale = new Vector3(1f,1f,1f);
        }
        if(ScreenControl.Instance.mFunctionRatio <= 0.78f)
        {
        	arenaItem.transform.localScale = new Vector3(1f,1f,1f);
        }
                    mItemList.Add(ArenaCrossItem.f_Create(arenaDragPageItem,arenaItem));
                    if (i == 0) {
                        //竞技场前两名单独一屏，设置前两名位置
                        if (j == 0)
                        {
                            arenaItem.transform.localPosition = new Vector3(615, 115);
                        }
                        else if (j == 1)
                        {
                            arenaItem.transform.localPosition = new Vector3(115, -285);
                            break;
                        }                    
                    }
                }            
            }
        mArenaCostLabel.text = string.Format(CommonTools.f_GetTransLanguage(743), GameParamConst.ArenaVigorCost);
        mArenaGrid = f_GetObject("ArenaGrid").GetComponent<UIGrid>();
        mArenaGrid.onReposition = f_ArenaGridReposition;
        mSelfName = f_GetObject("SelfName").GetComponent<UILabel>();
        mSelfPower = f_GetObject("SelfPower").GetComponent<UILabel>();
        mSelfLevel = f_GetObject("SelfLevel").GetComponent<UILabel>();
        mLeftBattleTimes = f_GetObject("LeftBattleTimes").GetComponent<UILabel>();
        

    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);

        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.Arena);

        if (!UITool.f_IsCanOpenChallengePage(UINameConst.ArenaCrossPage))
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.ArenaCrossPage, UIMessageDef.UI_CLOSE);
            return;
        }

        //Data_Pool.m_GuidancePool.OpenLevelPageUIName = UINameConst.ArenaPageNew;
        //Data_Pool.m_GuidancePool.IsUpdateArena = false;
        //PlayerPrefs.SetString(ArenaFakePlayerNameKey, "");

        //货币
        List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
        listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Vigor);
        listMoneyType.Add(EM_MoneyType.eUserAttr_ArenaCorssMoney);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, listMoneyType);

        //根据排名显示奖励
        List<AwardPoolDT> tList = Data_Pool.m_AwardPool.f_GetArenaCrossRankAward(Data_Pool.m_CrossArenaPool.m_iRank);
        mAwardShowComponent.f_Show(tList, EM_CommonItemShowType.All, EM_CommonItemClickType.AllTip, this);
        mFameLabel.text = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_ArenaCorssMoney).ToString();
        if (tList.Count > 0)
            mRankAwardNull.text = string.Empty;
        else
            mRankAwardNull.text = CommonTools.f_GetTransLanguage(745);
        int myRank = Data_Pool.m_CrossArenaPool.m_iRank;
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
        listMoneyType.Add(EM_MoneyType.eUserAttr_ArenaCorssMoney);
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
        f_UpdateByArenaList(Data_Pool.m_CrossArenaPool.f_GetArenaList());

        ////检查是否需要打开突破奖励界面
        //if (Data_Pool.m_ArenaPool.mBreakRankInfo.m_bShowInfo)
        //{
        //    ccUIManage.GetInstance().f_SendMsg(UINameConst.ArenaBreakAwardPage, UIMessageDef.UI_OPEN);
        //}

        //先打开，自己请求数据后再刷新界面
        //if (value != null && value is bool)
        //{
        //    bool updateBySelf = (bool)value;
        //    if (updateBySelf)
        //        f_UpdateAfterRequest();
        //}
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
    private void f_UpdateByArenaList(ArrayList list)
    {
        System.DateTime start = System.DateTime.Now;
        mArenaList = list;
        mScrollPercent = 1;
        MessageBox.ASSERT("Số người tham đấu trường： " + list.Count);
        //GridUtil.f_SetGridView<BasePoolDT<long>>(f_GetObject("ArenaGrid"), f_GetObject("ArenaItem"), mArenaList, OnShowItemData, true);
        //ccTimeEvent.GetInstance().f_RegEvent(1f, false, null, (object obj1) => { mArenaGrid.GetComponent<UICenterOnChild>().CenterOn(selfGo.transform); });


        //设置滑动限制，前两名占一屏，其他每四人一屏，再加mArenaGrid.cellHeight / 25偏移量
        mArenaScrollView.mCanDragLimitPos = Vector2.up * (list.Count * mArenaGrid.cellHeight / 4 - mArenaGrid.cellHeight / 2 + mArenaGrid.cellHeight / 25);
        mArenaScrollView.CirticalValue2CorrectedPos = 15;
        for (int i = 0; i < mItemList.Count; i++)
        {
            if ((i - 2) % ItemNumsOfOneDragPage == 0 || i == 0)
            {
                mItemList[i].mParent.SetActive(i < mArenaList.Count);
                if (i >= mArenaList.Count)
                    continue;
            }
            if (i < mArenaList.Count)
            {
                CMsg_ArenaCrossInfo arenaPoolDT = (CMsg_ArenaCrossInfo)mArenaList[i];
                mItemList[i].f_UpdateByInfo(i, arenaPoolDT);
                f_RegClickEvent(mItemList[i].mChallengeBtn, f_ArenaItemChallengeClick, arenaPoolDT, i);
                f_RegClickEvent(mItemList[i].mSweepBtn, f_ArenaItemSweepClick, arenaPoolDT, i);
                if (arenaPoolDT.userId == Data_Pool.m_UserData.m_iUserId)
                {
                    mSelfName.text = arenaPoolDT.userView.m_szName;
                    mSelfPower.text = Data_Pool.m_TeamPool.f_GetTotalBattlePower().ToString();
                    mSelfLevel.text = arenaPoolDT.uRank.ToString();
                    mLeftBattleTimes.text = string.Format(CommonTools.f_GetTransLanguage(1013), Data_Pool.m_CrossArenaPool.CrossArenaInfo.iTimes.ToString());
                    if (mRole != null)
                    {
                        DestroyImmediate(mRole);
                        mRole = null;
                    }
                    UITool.f_CreateRoleByCardId(arenaPoolDT.uMainCard, ref mRole, mRoleParent.transform, 3, 50, false);
                    //UITool.f_ShowBone(mRole.transform.GetComponent<SkeletonAnimation>(), 0, 1);
                    //mSelfIcon.sprite2D = UITool.f_GetCardIcon(arenaPoolDT.m_PlayerInfo.m_CardId);
                    //UITool.f_GetIconSpriteByCardId(teamPoolDT.m_CardPoolDT)
                    if (i + 1 == list.Count)
                    {
                        //我在最后一位则固定限制我刚好在最底下，要不会看到下面的站台
                        mScrollPercent = 0.86f;
                    }
                    else
                    {
                        mScrollPercent = (float)i / list.Count;
                    }
                }
            }
            else
                mItemList[i].f_UpdateByInfo(i, new CMsg_ArenaCrossInfo());
        }
        mArenaGrid.repositionNow = true;

        Data_Pool.m_GuidancePool.IsUpdateArena = true;
        MessageBox.ASSERT("Đặt thời gian giao diện đấu trường:" + (System.DateTime.Now - start).TotalSeconds);
    }

    private void OnShowItemData(GameObject go, BasePoolDT<long> basePoolDT)
    {
        //ArenaPoolDT arenaPoolDT = (ArenaPoolDT)basePoolDT;
        //ArenaCrossItem arenaItemNew = go.transform.GetComponent<ArenaCrossItem>();
        //arenaItemNew.f_UpdateByInfo(arenaPoolDT);
        //f_RegClickEvent(arenaItemNew.mChallengeBtn, f_ArenaItemChallengeClick, arenaPoolDT);
        //f_RegClickEvent(arenaItemNew.mSweepBtn, f_ArenaItemSweepClick, arenaPoolDT);
        //if (arenaPoolDT.iId == Data_Pool.m_UserData.m_iUserId)
        //{
        //    mSelfName.text = arenaPoolDT.m_PlayerInfo.m_szName;
        //    mSelfPower.text = Data_Pool.m_TeamPool.f_GetTotalBattlePower().ToString();
        //    mSelfLevel.text = arenaPoolDT.m_PlayerInfo.m_iLv.ToString();
        //}
        //if (arenaPoolDT.iId == Data_Pool.m_UserData.m_iUserId)
        //{
        //    selfGo = go;
        //}
    }




    #region Btn_Handle

    private void f_BackHandle(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ArenaCrossPage, UIMessageDef.UI_CLOSE);
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.UI_Challenge);
        ccUIHoldPool.GetInstance().f_UnHold();
    }

    private void f_FameShopHandle(GameObject go, object value1, object value2)
    {
        ccUIHoldPool.GetInstance().f_Hold(this);
        //打开商店
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopMutiCommonPage, UIMessageDef.UI_OPEN, EM_ShopMutiType.CrossArena);
    }

    private void f_LineupHandle(GameObject go, object value1, object value2)
    {
        ccUIHoldPool.GetInstance().f_Hold(this);
        //打开布阵
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ClothArrayPage, UIMessageDef.UI_OPEN);
    }

    private void f_RankPageHandle(GameObject go, object value1, object value2)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_CrossArenaRankPool, f_SendCrossArenaListResult, this);
        Data_Pool.m_CrossArenaPool.f_ExecuteAfterInitRankList();
    }

    private void f_RecordPageHandle(GameObject go, object value1, object value2)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_CrossArenaRecordPool, f_SendCrossArenaListRecordResult, this);
        Data_Pool.m_CrossArenaPool.f_GetCrossArenaRecordList();
    }

    private void f_SendCrossArenaListResult(object value)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_CrossArenaRankPool, f_SendCrossArenaListResult, this);

        int result = (int)value;
        if (result >= 1)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.ArenaCrossRankPage, UIMessageDef.UI_OPEN);
        }
        else
        {
            UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(746) + value);
        }
    }

    private void f_SendCrossArenaListRecordResult(object value)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_CrossArenaRecordPool, f_SendCrossArenaListRecordResult, this);

        ccUIManage.GetInstance().f_SendMsg(UINameConst.ArenaCrossRecordPage, UIMessageDef.UI_OPEN);  
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
        CMsg_ArenaCrossInfo node = (CMsg_ArenaCrossInfo)value1;
        //int tCurIdx = (int)value2;
        if (node.userId == Data_Pool.m_UserData.m_iUserId)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(749));
            return;
        }
        if (Data_Pool.m_CrossArenaPool.CrossArenaInfo.iTimes <= 0)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(676));
            return;
        }
        else if (Data_Pool.m_CrossArenaPool.f_CheckChallengeRankLimit(node.uRank))
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(750));
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_ChallengePlayerSuc;
        socketCallbackDt.m_ccCallbackFail = f_ChallengePlayerFail;

        //发送挑战玩家信息

        Data_Pool.m_CrossArenaPool.f_ArenaChallenge(node.uRank, socketCallbackDt, node);
    }

    /// <summary>
    /// 便捷挑战
    /// </summary>
    private void f_ArenaItemSweepClick(GameObject go, object value1, object value2)
    {
        CMsg_ArenaCrossInfo node = (CMsg_ArenaCrossInfo)value1;

        if (node.uRank < Data_Pool.m_CrossArenaPool.m_iRank)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(751));
            return;
        }
        if (Data_Pool.m_CrossArenaPool.CrossArenaInfo.iTimes <= 0)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(676));
            return;
        }

        //请求便捷挑战
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_ArenaSweepSuc;
        socketCallbackDt.m_ccCallbackFail = f_ArenaSweepFail;

        Data_Pool.m_CrossArenaPool.f_ArenaSweep(socketCallbackDt);

    }

    private void f_ChallengePlayerSuc(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ArenaCrossPage, UIMessageDef.UI_CLOSE);
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
        int syceeCount = 100;
        List<AwardPoolDT> awardList = new List<AwardPoolDT>();
        AwardPoolDT item1 = new AwardPoolDT();
        item1.f_UpdateByInfo((byte)EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_ArenaCorssMoney, syceeCount);
        awardList.Add(item1);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { awardList });

        mFameLabel.text = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_ArenaCorssMoney).ToString();
        mLeftBattleTimes.text = string.Format(CommonTools.f_GetTransLanguage(1013), Data_Pool.m_CrossArenaPool.CrossArenaInfo.iTimes.ToString());

    }

    private void f_ArenaSweepFail(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(753) + result);
    }

    #endregion


    //private void f_UpdateAfterRequest()
    //{
    //    UITool.f_OpenOrCloseWaitTip(true);
    //    SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
    //    socketCallbackDt.m_ccCallbackSuc = f_Callback_ArenaList;
    //    socketCallbackDt.m_ccCallbackFail = f_Callback_ArenaList;
    //    Data_Pool.m_CrossArenaPool.f_ArenaList();
    //}

    private void f_Callback_ArenaList(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            //更新玩家列表
            f_UpdateByArenaList(Data_Pool.m_CrossArenaPool.f_GetArenaList());
        }
        else
        {
            UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(754) + result);
        }
    }

    private void f_OpenCrossArenaTask(GameObject go, object value1, object value2)
    {
        CommonShowAwardParam param = new CommonShowAwardParam();
        param.m_Item = f_GetObject("Item");
        param.m_SCDTList = glo_Main.GetInstance().m_SC_Pool.m_CrossArenaTaskSC.f_GetAll();
        param.m_SCDTUpdate = ntegralItemUpdateByInfoTask;

        ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonShowAward, UIMessageDef.UI_OPEN, param);
    }

    private void ntegralItemUpdateByInfoTask(Transform item, NBaseSCDT dt)
    {
        CrossArenaTaskDT data = (CrossArenaTaskDT)dt;
        UILabel rankText = item.Find("Index").GetComponent<UILabel>();
        GameObject awardParent = item.Find("ScrollView/Grid").gameObject;
        GameObject awardItem = item.Find("ResourceCommonItem").gameObject;

        rankText.text = data.szTaskName.ToString();
        //设置奖励
        List<ResourceCommonDT> m_ResourceCommonDTList = CommonTools.f_GetListCommonDT(data.szAward);
        GridUtil.f_SetGridView<ResourceCommonDT>(awardParent, awardItem, m_ResourceCommonDTList, CommonAwarUpdate);
    }
    private void CommonAwarUpdate(GameObject item, ResourceCommonDT data)
    {
        item.GetComponent<ResourceCommonItem>().f_UpdateByInfo(data);
    }
}
