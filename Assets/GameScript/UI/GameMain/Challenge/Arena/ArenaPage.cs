using ccU3DEngine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaPage : UIFramwork
{
    private const int SWEEP_MAX_NUM = 5;
    private GameObject mBtn_Back;
    private GameObject mBtn_FameShop;
    private GameObject mBtn_Lineup;
    private GameObject mBtn_RankPage;

    private UILabel mFameLabel;
    private UILabel mRankLabel;
    private UILabel mArenaCostLabel;
    private UILabel mRankAwardNull;

    private UIGrid mAwardGrid;
    private GameObject mAwardItem;
    private ResourceCommonItemComponent _awardShowComponent;
    private ResourceCommonItemComponent mAwardShowComponent
    {
        get
        {
            if(_awardShowComponent == null)
                _awardShowComponent = new ResourceCommonItemComponent(mAwardGrid, mAwardItem);
            return _awardShowComponent;
        }
    }

    private UIScrollView mArenaScrollView;
    private UISlider mArenaScrollViewSlider;
    private GameObject mArenaScrollViewArrowR;
    private GameObject mArenaScrollViewArrowL;
    private UIGrid mArenaGrid;
    private GameObject mArenaItem;
    private List<ArenaItem> mItemList;
    private const int ArenaItemMaxNum = 23;
    private List<BasePoolDT<long>> mArenaList;

    private UILabel mSelfName;
    private UILabel mSelfPower;
    private Transform mRoleParent;
    private GameObject mRole;
    private string strTexBgRoot = "UI/TextureRemove/Arena/Texture_ArenaBg";

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        mBtn_Back = f_GetObject("Btn_Back");
        mBtn_FameShop = f_GetObject("Btn_FameShop");
        mBtn_Lineup = f_GetObject("Btn_Lineup");
        mBtn_RankPage = f_GetObject("Btn_RankPage");
        f_RegClickEvent(mBtn_Back, f_BackHandle);
        f_RegClickEvent(mBtn_FameShop, f_FameShopHandle);
        f_RegClickEvent(mBtn_Lineup, f_LineupHandle);
        f_RegClickEvent(mBtn_RankPage, f_RankPageHandle);
        mFameLabel = f_GetObject("FameLabel").GetComponent<UILabel>();
        mRankLabel = f_GetObject("RankLabel").GetComponent<UILabel>();
        mArenaCostLabel = f_GetObject("ArenaCostLabel").GetComponent<UILabel>();
        mAwardGrid = f_GetObject("AwardGrid").GetComponent<UIGrid>();
        mAwardItem = f_GetObject("TaskAwardItem");
        mRankAwardNull = f_GetObject("RankAwardNull").GetComponent<UILabel>();
        mArenaScrollView = f_GetObject("ArenaScrollView").GetComponent<UIScrollView>();
        mArenaScrollViewSlider = f_GetObject("ArenaScrollViewSlider").GetComponent<UISlider>();
        mArenaScrollViewArrowR = f_GetObject("ArenaScrollViewArrowR");
        mArenaScrollViewArrowL = f_GetObject("ArenaScrollViewArrowL");
        mArenaGrid = f_GetObject("ArenaGrid").GetComponent<UIGrid>();
        mArenaItem = f_GetObject("ArenaItem");
        mItemList = new List<ArenaItem>();
        for(int i = 0; i < ArenaItemMaxNum; i++)
        {
            mItemList.Add(ArenaItem.f_Create(mArenaGrid.gameObject, mArenaItem));
        }
        mArenaCostLabel.text = string.Format(CommonTools.f_GetTransLanguage(743), GameParamConst.ArenaVigorCost);//
        mArenaGrid.onReposition = f_ArenaGridReposition;
        EventDelegate.Set(mArenaScrollViewSlider.onChange, f_ArenaScrollViewSliderChange);
        mSelfName = f_GetObject("SelfName").GetComponent<UILabel>();
        mSelfPower = f_GetObject("SelfPower").GetComponent<UILabel>();
        mRoleParent = f_GetObject("RoleParent").transform;
    }

    private void f_ArenaScrollViewSliderChange()
    {
        if(mArenaScrollViewSlider.value == 1 && mArenaScrollViewArrowR.activeSelf)
        {
            mArenaScrollViewArrowR.SetActive(false);
        }
        else if(mArenaScrollViewSlider.value < 1 && !mArenaScrollViewArrowR.activeSelf)
        {
            mArenaScrollViewArrowR.SetActive(true);
        }

        if(mArenaScrollViewSlider.value < 0.05f && mArenaScrollViewArrowL.activeSelf)
        {
            mArenaScrollViewArrowL.SetActive(false);
        }
        else if(mArenaScrollViewSlider.value > 0.05f && !mArenaScrollViewArrowL.activeSelf)
        {
            mArenaScrollViewArrowL.SetActive(true);
        }
    }



    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        Data_Pool.m_GuidancePool.OpenLevelPageUIName = UINameConst.ArenaPage;
        Data_Pool.m_GuidancePool.IsUpdateArena = false;
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
        if(!Data_Pool.m_ArenaPool.m_IsOnRank)
        {
            mRankAwardNull.text = string.Empty;
            mRankLabel.text = CommonTools.f_GetTransLanguage(744);
        }
        else
        {
            if(tList.Count > 0)
                mRankAwardNull.text = string.Empty;
            else
                mRankAwardNull.text = CommonTools.f_GetTransLanguage(745);
            mRankLabel.text = Data_Pool.m_ArenaPool.m_iRank.ToString();
        }
        if(m_InitRes)
        {
            f_ShowArenaList(e);
        }
        else
        {
            UITool.f_OpenOrCloseWaitTip(true, true);
            //异步加载资源
            StartCoroutine(f_PreloadRoleRes(e));
        }
        f_LoadTexture();
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_UPDATE_MODELINFOR, f_ProcessModelInfo, this);
    }
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        //加载背景图
        UITexture Texture_BG = f_GetObject("Texture_BG").GetComponent<UITexture>();
        if(Texture_BG.mainTexture == null)
        {
            Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot);
            Texture_BG.mainTexture = tTexture2D;
        }
    }

    private bool m_InitRes = false;
    //预加载资源
    private IEnumerator f_PreloadRoleRes(object value)
    {
        m_InitRes = false;
        CardDT ManCardDT = glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(GameParamConst.ManCardId) as CardDT;
        glo_Main.GetInstance().m_ResourceManager.f_PreloadRoleRes(((RoleModelDT)glo_Main.GetInstance().m_SC_Pool.m_RoleModelSC.f_GetSC(ManCardDT.iStatelId1)).iModel);
        CardDT WomanCardDT = glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(GameParamConst.WomanCardId) as CardDT;
        glo_Main.GetInstance().m_ResourceManager.f_PreloadRoleRes(((RoleModelDT)glo_Main.GetInstance().m_SC_Pool.m_RoleModelSC.f_GetSC(WomanCardDT.iStatelId1)).iModel);
        yield return 0;
        m_InitRes = true;
        UITool.f_OpenOrCloseWaitTip(false);
        f_ShowArenaList(value);
    }

    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        //货币
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
        if(Data_Pool.m_ArenaPool.mBreakRankInfo.m_bShowInfo)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.ArenaBreakAwardPage, UIMessageDef.UI_OPEN);
        }
        //先打开，自己请求数据后再刷新界面
        if(value != null && value is bool)
        {
            bool updateBySelf = (bool)value;
            if(updateBySelf)
                f_UpdateAfterRequest();
        }
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        if(mRole != null)
            glo_Main.GetInstance().m_ResourceManager.ResetShader(mRole);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_UPDATE_MODELINFOR, f_ProcessModelInfo, this);

    }

    /// <summary>
    /// 更新玩家列表
    /// </summary>
    /// <param name="list"></param>
    private void f_UpdateByArenaList(List<BasePoolDT<long>> list)
    {
        m_CurSelectIdx = -99;
        mArenaList = list;
        for(int i = 0; i < mItemList.Count; i++)
        {
            if(i < mArenaList.Count)
            {
                mItemList[i].f_UpdateByInfo(m_CurSelectIdx, i, (ArenaPoolDT)mArenaList[i]);
                f_RegClickEvent(mItemList[i].mSelectItem, f_ArenaSelectItem, mArenaList[i], i);
                f_RegClickEvent(mItemList[i].mChallengeBtn, f_ArenaItemChallengeClick, mArenaList[i], i);
                f_RegClickEvent(mItemList[i].mSweepBtn, f_ArenaItemSweepClick, mArenaList[i], i);
                f_RegClickEvent(mItemList[i].mRoleIcon.gameObject, f_ArenaItemChallengeClick, mArenaList[i], i);
                if(mArenaList[i].iId == Data_Pool.m_UserData.m_iUserId)
                {
                    mSelfName.text = ((ArenaPoolDT)mArenaList[i]).m_PlayerInfo.m_szName;
                    mSelfPower.text = Data_Pool.m_TeamPool.f_GetTotalBattlePower().ToString();
                    //设置模型
                    UITool.f_CreateRoleByCardId(((ArenaPoolDT)mArenaList[i]).m_PlayerInfo.m_CardId, ref mRole, mRoleParent, 1);
                    f_SetMaskShader();
                }
            }
            else
                mItemList[i].f_UpdateByInfo(m_CurSelectIdx, i, null);
        }
        mArenaGrid.repositionNow = true;
        Data_Pool.m_GuidancePool.IsUpdateArena = true;
    }

    #region Btn_Handle

    private void f_BackHandle(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ArenaPage, UIMessageDef.UI_CLOSE);
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
        mRoleParent.gameObject.SetActive(false);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_ClothArrayPage_CLOSE, f_OnClothArrayPageClose, this);
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
        if(result == (int)eMsgOperateResult.OR_Succeed)
        {
            mRoleParent.gameObject.SetActive(false);
            glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_ArenaRankPage_CLOSE, f_OnArenaRankPageClose, this);
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
    }

    #endregion

    #region ArenaItem


    private int m_CurSelectIdx = -99;
    //选择
    private void f_ArenaSelectItem(GameObject go, object value1, object value2)
    {
        ArenaPoolDT node = (ArenaPoolDT)value1;
        int selectIdx = (int)value2;
        if(node.m_PlayerInfo.iId == Data_Pool.m_UserData.m_iUserId)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(747));
            return;
        }
        else if(m_CurSelectIdx == selectIdx)
            return;
        m_CurSelectIdx = selectIdx;
        for(int i = 0; i < mItemList.Count; i++)
        {
            mItemList[i].f_UpdateSelectIdx(m_CurSelectIdx);
        }

    }

    /// <summary>
    /// 挑战某个玩家
    /// </summary>
    private void f_ArenaItemChallengeClick(GameObject go, object value1, object value2)
    {
        ArenaPoolDT node = (ArenaPoolDT)value1;
        int tCurIdx = (int)value2;
        if(m_CurSelectIdx != tCurIdx)
        {
            MessageBox.DEBUG(CommonTools.f_GetTransLanguage(748));
            return;
        }
        if(node.m_PlayerInfo.iId == Data_Pool.m_UserData.m_iUserId)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(749));
            return;
        }
        else if(!UITool.f_IsEnoughMoney(EM_MoneyType.eUserAttr_Vigor, GameParamConst.ArenaVigorCost,true,true,this))
        {
            return;
        }
        else if(Data_Pool.m_ArenaPool.f_CheckChallengeRankLimit(node.m_iRank))
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(750));
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_ChallengePlayerSuc;
        socketCallbackDt.m_ccCallbackFail = f_ChallengePlayerFail;
        //发送挑战玩家信息
        Data_Pool.m_ArenaPool.f_ArenaChallenge(node.m_iRank, socketCallbackDt, 1, 0);

    }

    /// <summary>
    /// 挑战多次 点击
    /// </summary>
    private void f_ArenaItemSweepClick(GameObject go, object value1, object value2)
    {
        ArenaPoolDT poolData = (ArenaPoolDT)value1;
        int vigorCost = SWEEP_MAX_NUM * GameParamConst.ArenaVigorCost;
        if(poolData.m_iRank < Data_Pool.m_ArenaPool.m_iRank)
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(751));
            return;
        }
        else if(!UITool.f_IsEnoughMoney(EM_MoneyType.eUserAttr_Vigor, vigorCost,true,true,this))
        {
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_ArenaSweepSuc;
        socketCallbackDt.m_ccCallbackFail = f_ArenaSweepFail;
        Data_Pool.m_ArenaPool.f_ArenaSweep((uint)poolData.m_iRank,5,1, socketCallbackDt);
    }

    private void f_ChallengePlayerSuc(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        //挑战成功切换场景
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ArenaPage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.BattleMain);
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
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ArenaSweepPage, UIMessageDef.UI_OPEN);
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
        if((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            //更新玩家列表
            f_UpdateByArenaList(Data_Pool.m_ArenaPool.f_GetArenaList());
        }
        else
        {
            UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(754) + result);
        }
    }

    private void f_OnClothArrayPageClose(object value)
    {
        mRoleParent.gameObject.SetActive(true);
        f_SetMaskShader();
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_ClothArrayPage_CLOSE, f_OnClothArrayPageClose, this);
    }

    private void f_OnArenaRankPageClose(object value)
    {
        mRoleParent.gameObject.SetActive(true);
        f_SetMaskShader();
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_ArenaRankPage_CLOSE, f_OnArenaRankPageClose, this);
    }

    private void f_SetMaskShader()
    {
        if(mRole != null)
        {
            Renderer tRenderer = mRole.GetComponent<Renderer>();
            Shader tShader = Shader.Find("Spine/SkeletonGray");
            if(tShader == null)
                MessageBox.ASSERT("Can't find shader. Name is Spine/SkeletonGray");
            tRenderer.sharedMaterial.shader = tShader;
            tRenderer.sharedMaterial.SetFloat("_GraySwitch", 1.0f);
            tRenderer.sharedMaterial.SetVector("_MaskPos", new Vector4(0, 1.43f, 0.78f, 0));
        }
    }

    private void f_ProcessModelInfo(object value)
    {
        //重新设置一下 shader
        f_SetMaskShader();
    }

}
