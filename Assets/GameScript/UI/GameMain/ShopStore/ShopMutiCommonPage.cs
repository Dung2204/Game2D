using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 商店通用页面（不只有6个，可以拖动和分页的页面）
/// 包含军团商店限时分页
/// </summary>
public class ShopMutiCommonPage : UIFramwork
{
    private SocketCallbackDT OnRequestShopRandInfoCallback = new SocketCallbackDT();//请求商店信息回调
    private SocketCallbackDT OnBuyShopRandCallback = new SocketCallbackDT();//购买信息回调
    private EM_ShopMutiType em_currentShopType = EM_ShopMutiType.Reputation;//当前商店类型
    private int PageIndex = 1;//页码
    private List<BasePoolDT<long>> listContentData = new List<BasePoolDT<long>>();//数据
    private UIWrapComponent _contentWrapComponent = null;//内容滑动
    private List<AwardPoolDT> buyPropList = new List<AwardPoolDT>();//购买的道具列表，用于购买成功时展示
	//My Code
	private int resetScroll = 0;
	//
	
    /// <summary>
    /// 页面打开
    /// </summary>
    /// <param name="e">商店类型</param>
    protected override void UI_OPEN(object e)
    {
        // glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(UITool.GetEnumName(typeof(AudioEffectType), AudioEffectType.OpenMarket));
        base.UI_OPEN(e);
        OnBtnShopClick(f_GetObject("BtnShop"), null, null);//默认选中商店按钮
        em_currentShopType = (EM_ShopMutiType)e;
        InitOrUpdateShopUIData(PageIndex, true, OnRequestShopRandInfoCallback);
        UITool.f_LoadTexture(f_GetObject("TexBg").GetComponent<UITexture>(), strTexBgRoot);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
		
		switch (em_currentShopType)
        {
            case EM_ShopMutiType.Reputation:
				f_GetObject("ShopTitle").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(1545);
				break;
            case EM_ShopMutiType.Legion:
				f_GetObject("ShopTitle").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(1548);
				break;
			case EM_ShopMutiType.RunningMan:
				f_GetObject("ShopTitle").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(1547);
				break;
			case EM_ShopMutiType.BattleFeatShop:
				f_GetObject("ShopTitle").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(1546);
				break;
			case EM_ShopMutiType.CrossServerBattle:
				f_GetObject("ShopTitle").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(1549);
				break;
			case EM_ShopMutiType.ChaosBattle:
f_GetObject("ShopTitle").GetComponent<UILabel>().text = "Trainning Shop";
				break;
            case EM_ShopMutiType.CrossTournament:
                f_GetObject("ShopTitle").GetComponent<UILabel>().text = "Tiệm Vấn Đỉnh";
                break;
            default:
                break;
        }
    }
    private string strTexBgRoot = "UI/TextureRemove/MainMenu/CommonBg";//背景图路径
    /// <summary>
    /// 页面关闭
    /// </summary>
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
    }
    /// <summary>
    /// 显示右上角货币
    /// </summary>
    private void ShowTopMoneyPage(params EM_MoneyType[] listMoneyType)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, new List<EM_MoneyType>(listMoneyType));
    }
    /// <summary>
    /// 设置UI信息
    /// </summary>
    /// <param name="strHint">显示的提示信息</param>
    /// <param name="moneyType1">UI显示货币1，没有为none</param>
    /// <param name="moneyType2">UI显示货币2，没有为none</param>
    private void SetUIDataInfo(string strHint,EM_MoneyType moneyType1,EM_MoneyType moneyType2,List<EM_ShopMutiPageTap> listShowPageTap)
    {
        f_GetObject("LabelHint").GetComponent<UILabel>().text = strHint;
        f_GetObject("Prop1").SetActive(moneyType1 != EM_MoneyType.eUserAttr_None);
        if (moneyType1 != EM_MoneyType.eUserAttr_None)
        {
            EM_ResourceType resourceType = ((int)moneyType1 < (int)EM_UserAttr.End) ? EM_ResourceType.Money : EM_ResourceType.Good;
            ResourceCommonDT tShowItem = new ResourceCommonDT();
            tShowItem.f_UpdateInfo((byte)resourceType, (int)moneyType1, 0);
            f_GetObject("Prop1").GetComponent<UILabel>().text = string.Format("{0}:", tShowItem.mName);
            f_GetObject("SprProp1").GetComponent<UISprite>().spriteName = UITool.f_GetMoneySpriteName(moneyType1);
            f_GetObject("SprProp1").GetComponentInChildren<UILabel>().text = UITool.f_GetGoodNum(resourceType, (int)moneyType1).ToString();
        }
        f_GetObject("Prop2").SetActive(moneyType2 != EM_MoneyType.eUserAttr_None);
        if (moneyType2 != EM_MoneyType.eUserAttr_None)
        {
            EM_ResourceType resourceType = ((int)moneyType2 < (int)EM_UserAttr.End) ? EM_ResourceType.Money : EM_ResourceType.Good;
            ResourceCommonDT tShowItem = new ResourceCommonDT();
            tShowItem.f_UpdateInfo((byte)resourceType, (int)moneyType2, 0);
            f_GetObject("Prop2").GetComponent<UILabel>().text = string.Format("{0}:", tShowItem.mName);
            f_GetObject("SprProp2").GetComponent<UISprite>().spriteName = UITool.f_GetMoneySpriteName(moneyType2);
            f_GetObject("SprProp2").GetComponentInChildren<UILabel>().text = UITool.f_GetGoodNum(resourceType, (int)moneyType2).ToString();
        }
        f_GetObject("BtnShop").SetActive(listShowPageTap.Contains(EM_ShopMutiPageTap.BtnShop));
        f_GetObject("BtnAward").SetActive(listShowPageTap.Contains(EM_ShopMutiPageTap.BtnAward));
        f_GetObject("BtnLimit").SetActive(false);
        f_GetObject("BtnPurpleEquip").SetActive(listShowPageTap.Contains(EM_ShopMutiPageTap.BtnPurpleEquip));
        f_GetObject("BtnRedEquip").SetActive(listShowPageTap.Contains(EM_ShopMutiPageTap.BtnRedEquip));
        f_GetObject("TabGrid").GetComponent<UIGrid>().Reposition();
    }
    /// <summary>
    /// 初始化事件
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnReturn", OnBtnReturnClick);
        f_RegClickEvent("BtnShop", OnBtnShopClick);
        f_RegClickEvent("BtnAward", OnBtnAwardClick);
        f_RegClickEvent("BtnLimit", OnBtnLimitClick);
        f_RegClickEvent("BtnPurpleEquip", OnBtnPurpleEquip);
        f_RegClickEvent("BtnRedEquip", OnBtnRedEquip);
        OnRequestShopRandInfoCallback.m_ccCallbackSuc = OnRequestSuc;
        OnRequestShopRandInfoCallback.m_ccCallbackFail = OnRequestFail;
        OnBuyShopRandCallback.m_ccCallbackSuc = OnBuyShopRandSuc;
        OnBuyShopRandCallback.m_ccCallbackFail = OnBuyShopRandFail;
        //GameObject role = null;
        //UITool.f_CreateRoleByModeId(11071, ref role, f_GetObject("RoleParent").transform, 18);
    }
    #region 服务器回调
    /// <summary>
    /// 请求商店信息成功回调
    /// </summary>
    private void OnRequestSuc(object msg)
    {
        InitOrUpdateShopUIData(PageIndex);
        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void OnRequestFail(object msg)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)msg;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(386));
        UITool.f_OpenOrCloseWaitTip(false);
    }
    /// <summary>
    /// 购买商店物品成功回调
    /// </summary>
    private void OnBuyShopRandSuc(object msg)
    {
        InitOrUpdateShopUIData(PageIndex, false);
        //展示获取奖励
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { buyPropList });
        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void OnBuyShopRandFail(object msg)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)msg;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(387) + CommonTools.f_GetTransLanguage((int)msg));
        UITool.f_OpenOrCloseWaitTip(false);
    }
    #endregion
    /// <summary>
    /// 设置对应页码的内容数据
    /// </summary>
    private void InitOrUpdateShopUIData(int pageIndex, bool resetView = true, SocketCallbackDT RequestShopInfoCallbackDT = null)
    {
        int NeedGoodsIndex = 0;
        f_GetObject("ChangeContent").SetActive(true);
        f_GetObject("FixedContent").SetActive(false);
        int playerLevel = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        switch (em_currentShopType)//---------------------------case1（需要根据商店类型显示）
        {
            case EM_ShopMutiType.Reputation:
                if (RequestShopInfoCallbackDT != null)
                {
                    Data_Pool.m_shopReputationPool.f_GetShopRandInfo(RequestShopInfoCallbackDT);
                    UITool.f_OpenOrCloseWaitTip(true);
                }
                string strHint = CommonTools.f_GetTransLanguage(388);
                SetUIDataInfo(strHint, EM_MoneyType.eRedBattleToken, EM_MoneyType.eUserAttr_Fame, new List<EM_ShopMutiPageTap>() { EM_ShopMutiPageTap.BtnShop, EM_ShopMutiPageTap.BtnAward });
                ShowTopMoneyPage(EM_MoneyType.eUserAttr_Sycee, EM_MoneyType.eUserAttr_Money, EM_MoneyType.eUserAttr_Fame);
                listContentData = CommonTools.f_CopyPoolDTArrayToNewList(Data_Pool.m_shopReputationPool.f_GetAll());
                for (int i = listContentData.Count - 1; i >= 0; i--)
                {
                    ShopReputationPoolDT shopReputationPoolDT = listContentData[i] as ShopReputationPoolDT;
                    if (shopReputationPoolDT.m_ReputationShopDT.iStoreTabNo != pageIndex || shopReputationPoolDT.m_ReputationShopDT.iStoreOpenLevel > playerLevel)
                        listContentData.RemoveAt(i);
                }
                if (Data_Pool.m_GuidancePool.IsEnter)
                {
                    NeedGoodsIndex = listContentData.FindIndex((BasePoolDT<long> a) =>
                     {
                         ShopReputationPoolDT shopReputationPoolDT = a as ShopReputationPoolDT;
                         return shopReputationPoolDT.m_ReputationShopDT.iSailResID == 100;
                     });
                }
                break;
            case EM_ShopMutiType.Legion:
                if (RequestShopInfoCallbackDT != null)
                {
                    LegionMain.GetInstance().m_ShopLegionPool.f_GetShopRandInfo(RequestShopInfoCallbackDT);
                    UITool.f_OpenOrCloseWaitTip(true);
                }
                string strHint2 = pageIndex == 2 ? CommonTools.f_GetTransLanguage(389) : CommonTools.f_GetTransLanguage(1552);
                //Begin check 军团奖励商店数据
                bool legionAwardIdxIsInvalid = true;
                List<BasePoolDT<long>> checkList = LegionMain.GetInstance().m_ShopLegionPool.f_GetAll();
                for (int i = 0; i < checkList.Count; i++)
                {
                    ShopLegionPoolDT checkNode = (ShopLegionPoolDT)checkList[i];
                    if (checkNode.m_LegionShopDT.iStoreTabNo == 3 && checkNode.buyTimes < checkNode.m_LegionShopDT.iCanBuyTimes && playerLevel >= checkNode.m_LegionShopDT.iStoreOpenLevel)
                    {
                        legionAwardIdxIsInvalid = false;
                        break;
                    }
                }
                //如果军团奖励已被购买完 就切回军团商店页签
                if (pageIndex == 3 && legionAwardIdxIsInvalid)
                {
                    pageIndex = 1;
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(390));
                }
                //end check
                SetUIDataInfo(strHint2, EM_MoneyType.eUserAttr_LegionContribution, EM_MoneyType.eUserAttr_None,
                    legionAwardIdxIsInvalid? 
                    new List<EM_ShopMutiPageTap>() { EM_ShopMutiPageTap.BtnShop, EM_ShopMutiPageTap.BtnLimit} : 
                    new List<EM_ShopMutiPageTap>() { EM_ShopMutiPageTap.BtnShop, EM_ShopMutiPageTap.BtnLimit, EM_ShopMutiPageTap.BtnAward });
                ShowTopMoneyPage(EM_MoneyType.eUserAttr_Sycee, EM_MoneyType.eUserAttr_Money, EM_MoneyType.eUserAttr_LegionContribution);
                if (pageIndex == 2)//限时商店
                    SetLegionLimitShopContent();
                else
                {
                    listContentData = CommonTools.f_CopyPoolDTArrayToNewList(LegionMain.GetInstance().m_ShopLegionPool.f_GetAll());
                    for (int i = listContentData.Count - 1; i >= 0; i--)
                    {
                        ShopLegionPoolDT shopLegionPoolDT = listContentData[i] as ShopLegionPoolDT;
                        if (pageIndex == 3) //军团奖励商店 已购买的删除，全部都购买完成就删除分页
                        {
                            //去除切页编号不一致，且等级不够的元素
                            if (shopLegionPoolDT.m_LegionShopDT.iStoreTabNo != pageIndex || shopLegionPoolDT.m_LegionShopDT.iStoreOpenLevel > playerLevel || shopLegionPoolDT.buyTimes >= shopLegionPoolDT.m_LegionShopDT.iCanBuyTimes)
                                listContentData.RemoveAt(i);
                        }
                        else
                        {
                            //去除切页编号不一致，且等级不够的元素
                            if (shopLegionPoolDT.m_LegionShopDT.iStoreTabNo != pageIndex || shopLegionPoolDT.m_LegionShopDT.iStoreOpenLevel > playerLevel)
                                listContentData.RemoveAt(i);
                        } 
                    }
                }
                break;
            case EM_ShopMutiType.RunningMan:
                string strHint3 = CommonTools.f_GetTransLanguage(391);
                SetUIDataInfo(strHint3, EM_MoneyType.eRedEquipElite, EM_MoneyType.eUserAttr_Prestige, new List<EM_ShopMutiPageTap>() { EM_ShopMutiPageTap.BtnShop, EM_ShopMutiPageTap.BtnPurpleEquip, EM_ShopMutiPageTap.BtnRedEquip, EM_ShopMutiPageTap.BtnAward });
                ShowTopMoneyPage(EM_MoneyType.eUserAttr_Sycee, EM_MoneyType.eUserAttr_Money, EM_MoneyType.eUserAttr_Prestige);
                listContentData = CommonTools.f_CopyPoolDTArrayToNewList(Data_Pool.m_RunningManShopPool.f_GetAll());
                //移除
                int removeNum = listContentData.RemoveAll(delegate (BasePoolDT<long> node)
                {
                    RunningManShopPoolDT tItem = (RunningManShopPoolDT)node;
                    return tItem.m_Template.iStoreTabNo != pageIndex || tItem.m_Template.iStoreOpenLevel > playerLevel;
                });
                break;
            case EM_ShopMutiType.BattleFeatShop:
                string strHint4 = CommonTools.f_GetTransLanguage(392);
                SetUIDataInfo(strHint4, EM_MoneyType.eRedBattleToken, EM_MoneyType.eUserAttr_BattleFeat, new List<EM_ShopMutiPageTap>() { EM_ShopMutiPageTap.BtnShop });
                ShowTopMoneyPage(EM_MoneyType.eUserAttr_Sycee, EM_MoneyType.eUserAttr_Money, EM_MoneyType.eUserAttr_BattleFeat);
                listContentData = CommonTools.f_CopyPoolDTArrayToNewList(Data_Pool.m_BattleFeatShopPool.f_GetAll());
                listContentData.RemoveAll(delegate (BasePoolDT<long> node)
                {
                    BattleFeatShopPoolDT tPoolDT = (BattleFeatShopPoolDT)node;
                    return tPoolDT.m_BattleFeatShopDT.iOpenValue > playerLevel;
                });
                break;
            case EM_ShopMutiType.CrossServerBattle:
                if (RequestShopInfoCallbackDT != null)
                {
                    UITool.f_OpenOrCloseWaitTip(true);
                    Data_Pool.m_CrossServerBattleShopPool.f_ShopInit(RequestShopInfoCallbackDT); 
                }
                string strHint5 = CommonTools.f_GetTransLanguage(393);
                SetUIDataInfo(strHint5,EM_MoneyType.eUserAttr_CrossServerScore, EM_MoneyType.eUserAttr_None, new List<EM_ShopMutiPageTap>() { EM_ShopMutiPageTap.BtnShop });
                ShowTopMoneyPage(EM_MoneyType.eUserAttr_Sycee, EM_MoneyType.eUserAttr_Money, EM_MoneyType.eUserAttr_CrossServerScore);
                listContentData = Data_Pool.m_CrossServerBattleShopPool.ShopList;
                break;
            //TsuCode - ChaosBattle
            case EM_ShopMutiType.ChaosBattle:
                if (RequestShopInfoCallbackDT != null)
                {
                    UITool.f_OpenOrCloseWaitTip(true);
                    Data_Pool.m_ChaosBattleShopPool.f_ShopInit(RequestShopInfoCallbackDT);
                }
                string strHint6 = "[522915FF]Chào mừng đến [9E0323FF]Chợ Thí      Luyện[-] hãy mua tùy thích[-]";
                SetUIDataInfo(strHint6, EM_MoneyType.eUserAttr_ChaosScore, EM_MoneyType.eUserAttr_None, new List<EM_ShopMutiPageTap>() { EM_ShopMutiPageTap.BtnShop });
                ShowTopMoneyPage(EM_MoneyType.eUserAttr_Sycee, EM_MoneyType.eUserAttr_Money, EM_MoneyType.eUserAttr_ChaosScore);
                listContentData = Data_Pool.m_ChaosBattleShopPool.ShopList;
                break;
            case EM_ShopMutiType.CrossArena:
                if (RequestShopInfoCallbackDT != null)
                {
                    UITool.f_OpenOrCloseWaitTip(true);
                    Data_Pool.m_CrossArenaPool.f_ShopInit(RequestShopInfoCallbackDT);
                }
                string strHint7 = "[522915FF]Chào mừng đến [9E0323FF]Tiệm Đấu Trường Đỉnh Caon[-] hãy mua tùy thích[-]";
                SetUIDataInfo(strHint7, EM_MoneyType.eUserAttr_ArenaCorssMoney, EM_MoneyType.eUserAttr_None, new List<EM_ShopMutiPageTap>() { EM_ShopMutiPageTap.BtnShop });
                ShowTopMoneyPage(EM_MoneyType.eUserAttr_Sycee, EM_MoneyType.eUserAttr_Money, EM_MoneyType.eUserAttr_ArenaCorssMoney);
                listContentData = Data_Pool.m_CrossArenaPool.ShopList;
                break;
            case EM_ShopMutiType.CrossTournament:
                if (RequestShopInfoCallbackDT != null)
                {
                    UITool.f_OpenOrCloseWaitTip(true);
                    Data_Pool.m_CrossTournamentPool.f_ShopInit(RequestShopInfoCallbackDT);
                }
                string strHint8 = "[522915FF]Chào mừng đến [9E0323FF]Tiệm Vấn Đỉnh[-] hãy mua tùy thích[-]";
                SetUIDataInfo(strHint8, EM_MoneyType.eUserAttr_TournamentPoint, EM_MoneyType.eUserAttr_None, new List<EM_ShopMutiPageTap>() { EM_ShopMutiPageTap.BtnShop });
                ShowTopMoneyPage(EM_MoneyType.eUserAttr_Sycee, EM_MoneyType.eUserAttr_Money, EM_MoneyType.eUserAttr_TournamentPoint);
                listContentData = Data_Pool.m_CrossTournamentPool.ShopList;
                break;
            case EM_ShopMutiType.CampGemShop:
                string strHint11 = "";
                SetUIDataInfo(strHint11, EM_MoneyType.eGemCamp, EM_MoneyType.eUserAttr_None, new List<EM_ShopMutiPageTap>() { EM_ShopMutiPageTap.BtnShop });
                ShowTopMoneyPage(EM_MoneyType.eUserAttr_Sycee, EM_MoneyType.eUserAttr_Money, EM_MoneyType.eGemCamp);
                listContentData = CommonTools.f_CopyPoolDTArrayToNewList(Data_Pool.m_CampGemShopPool.f_GetAll());
                break;
                //

        }
        if (_contentWrapComponent == null)
        {
            _contentWrapComponent = new UIWrapComponent(230, 2, 750, 5, f_GetObject("UIGridParent"), f_GetObject("ShopMutiItem"), listContentData, OnItemUpdate, null);
        }
        _contentWrapComponent.f_UpdateList(listContentData);
        if (resetView)
		{
            _contentWrapComponent.f_ResetView();
			f_GetObject("UIGridParent").transform.parent.GetComponent<UIScrollView>().ResetPosition();
		}
        _contentWrapComponent.f_UpdateView();
        if (Data_Pool.m_GuidancePool.IsEnter)
        {
            f_GetObject("UIGridParent").transform.parent.GetComponent<UIScrollView>().ResetPosition();
            UIScrollBar tScrollview;
            switch (em_currentShopType)
            {
                case EM_ShopMutiType.Reputation:
                    tScrollview = f_GetObject("ScrollBar").GetComponent<UIScrollBar>();
                    tScrollview.value = 1;
                    break;
                case EM_ShopMutiType.Legion:
                    break;
                case EM_ShopMutiType.RunningMan:
                    break;
                case EM_ShopMutiType.BattleFeatShop:
                    break;
                case EM_ShopMutiType.CrossServerBattle:
                    break;
                //TsuCode - ChaosBattle
                case EM_ShopMutiType.ChaosBattle:
                    //f_GetObject("ShopTitle").GetComponent<UILabel>().text = "Tiệm Thí Luyện";
                    break;
                case EM_ShopMutiType.CrossArena:
                    //f_GetObject("ShopTitle").GetComponent<UILabel>().text = " Tiệm Đấu Trường Đỉnh Cao";
                    break;
                case EM_ShopMutiType.CrossTournament:
                    //f_GetObject("ShopTitle").GetComponent<UILabel>().text = " Tiệm Vấn Đỉnh";
                    break;
            }
        }
    }
	
	void Update()
	{
		if(resetScroll < 10)
		{
			f_GetObject("UIGridParent").transform.parent.GetComponent<UIScrollView>().ResetPosition();
			resetScroll++;
		}
	}
	
    #region 军团限时商店相关
    /// <summary>
    /// 设置军团限时商店数据
    /// </summary>
    private void SetLegionLimitShopContent()
    {
        f_GetObject("ChangeContent").SetActive(false);
        f_GetObject("FixedContent").SetActive(true);
        SocketCallbackDT callBack = new SocketCallbackDT();
        callBack.m_ccCallbackSuc = OnQueryLegionSucCallback;
        callBack.m_ccCallbackFail = (object result)=> { };
        LegionMain.GetInstance().m_ShopTimeLimitPool.f_GetShopBuyTimesInfo(callBack);
    }
    /// <summary>
    /// 军团限时商店查询成功
    /// </summary>
    private void OnQueryLegionSucCallback(object result)
    {
        List<BasePoolDT<long>> listData = LegionMain.GetInstance().m_ShopTimeLimitPool.f_GetAll();
        for (int i = 0; i < 6; i++)
        {
            ShopTimeLimitPoolDT poolDT = listData[i] as ShopTimeLimitPoolDT;
            ShopMutiFixedItem item = f_GetObject("Item" + i).GetComponent<ShopMutiFixedItem>();
            item.gameObject.SetActive(true);
            LegionShopTimeLimitDT dt = poolDT.m_LegionShopTimeLimitDT;
            ResourceCommonDT commonDT = new ResourceCommonDT();
            commonDT.f_UpdateInfo((byte)dt.iSailResTypeID, dt.iSailResID, dt.iSailResCount);
            string Name = commonDT.mName;
            string borderSpriteName = UITool.f_GetImporentColorName(commonDT.mImportant, ref Name);
            int canBuyTimes = dt.iCanBuyTimes - poolDT.m_myBuyTimes;
            bool HasBuy = canBuyTimes <= 0 ? true : false;
            item.SetData(Name, (EM_ResourceType)commonDT.mResourceType, commonDT.mResourceId, borderSpriteName, commonDT.mResourceNum,
                (EM_MoneyType)dt.iBuyResID, dt.iBuyDiscount, dt.iBuyResCount, dt.iDiscountIntensity, poolDT.m_abuyTimes, dt.iBuyTimesOfTroops, canBuyTimes, HasBuy);
            f_RegClickEvent(item.m_BtnBuy.gameObject, OnLimitShopBuyItem, i, poolDT);
            f_RegClickEvent(item.m_SprIcon.gameObject, UITool.f_OnItemIconClick, commonDT);
        }
    }
    /// <summary>
    /// 军团限时商店购买
    /// </summary>
    private void OnLimitShopBuyItem(GameObject go, object obj1, object obj2)
    {
        ShopTimeLimitPoolDT poolDT = obj2 as ShopTimeLimitPoolDT;
        SocketCallbackDT callback = new SocketCallbackDT();
        callback.m_ccCallbackSuc = BuyLimitShopSuc;
        callback.m_ccCallbackFail = BuyLimitShopFail;
        if (UITool.f_GetGoodNum((EM_ResourceType)poolDT.m_LegionShopTimeLimitDT.iBuyResTypeID, poolDT.m_LegionShopTimeLimitDT.iBuyResID) < poolDT.m_LegionShopTimeLimitDT.iBuyDiscount)
        {
            string moneyName = UITool.f_GetGoodName((EM_ResourceType)poolDT.m_LegionShopTimeLimitDT.iBuyResTypeID, poolDT.m_LegionShopTimeLimitDT.iBuyResID);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, string.Format(CommonTools.f_GetTransLanguage(394), moneyName));
            return;
        }
        LegionMain.GetInstance().m_ShopTimeLimitPool.f_ShopBuy(poolDT.m_LegionShopTimeLimitDT.iId, callback);
        buyPropList.Clear();
        AwardPoolDT item1 = new AwardPoolDT();
        item1.f_UpdateByInfo((byte)poolDT.m_LegionShopTimeLimitDT.iSailResTypeID, poolDT.m_LegionShopTimeLimitDT.iSailResID, poolDT.m_LegionShopTimeLimitDT.iSailResCount);
        buyPropList.Add(item1);
    }
    /// <summary>
    /// 军团购买限时商品成功
    /// </summary>
    private void BuyLimitShopSuc(object obj)
    {
        OnQueryLegionSucCallback(null);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN, new object[] { buyPropList });
    }
    /// <summary>
    /// 军团购买限时商品失败
    /// </summary>
    private void BuyLimitShopFail(object obj)
    {
        OnBtnLimitClick(null, null, null);
        if ((int)obj == (int)eMsgOperateResult.eOR_TimesLimit)//次数不足
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(395));
        }
        else if ((int)obj == (int)eMsgOperateResult.eOR_SellOut)//商店资源已被其他人买完
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(396));
        }
        else
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(397) + (int)obj);
        }
    }
    #endregion
    private string CheckBuyTime(int FreshType, int buytimes, int maxTimes)
    {
        string HintStr = "";
        if (maxTimes <= 0 || maxTimes >= 999999)
            return "";
        switch (FreshType)
        {
            case 0://不刷新
                if (maxTimes > 0 && buytimes < maxTimes)
                    HintStr += string.Format(CommonTools.f_GetTransLanguage(398), (maxTimes - buytimes));
                break;
            case 1://每日刷新
                if (maxTimes > 0 && buytimes < maxTimes)
                    HintStr += string.Format(CommonTools.f_GetTransLanguage(399), (maxTimes - buytimes)); 
                break;
            case 2://每周刷新
                if (maxTimes > 0 && buytimes < maxTimes)
                    HintStr += string.Format(CommonTools.f_GetTransLanguage(400), (maxTimes - buytimes)); 
                break;
        }
        return HintStr;
    }
    private void SetItemData(Transform item, BasePoolDT<long> poolDT, ResourceCommonDT sailResCommonDT,EM_MoneyType moneyType1, int price1, EM_MoneyType moneyType2, int price2, 
        int disCount, int hasBuyTimes, int maxCanBuyTimes, int openType, int openValue, int freshType = 0)
    {
        string Name = sailResCommonDT.mName;
        string BorderName = UITool.f_GetImporentColorName(sailResCommonDT.mImportant,ref Name);
        string buyTimes = string.Format("（{0}/{1}）", hasBuyTimes, maxCanBuyTimes);
        buyTimes = (maxCanBuyTimes <= 0 || maxCanBuyTimes >= 999999) ? "" : buyTimes;//0表示不限购
        string hasCountHint = "";
        if (buyTimes == "" && sailResCommonDT.mResourceType == (int)EM_ResourceType.CardFragment)//不限购而且为出售类型为卡牌碎片
        {
            int count = UITool.f_GetGoodNum(EM_ResourceType.CardFragment, sailResCommonDT.mResourceId);
            CardFragmentDT cardFragmentDt = glo_Main.GetInstance().m_SC_Pool.m_CardFragmentSC.f_GetSC(sailResCommonDT.mResourceId) as CardFragmentDT;
            hasCountHint = "（"+CommonTools.f_GetTransLanguage(401) + count + "/" + cardFragmentDt.iNeedNum + "）";
        }
        if (buyTimes == "" && sailResCommonDT.mResourceType == (int)EM_ResourceType.EquipFragment)
        {
            int count = UITool.f_GetGoodNum(EM_ResourceType.EquipFragment, sailResCommonDT.mResourceId);
            EquipFragmentsDT equipFragmentDt = glo_Main.GetInstance().m_SC_Pool.m_EquipFragmentsSC.f_GetSC(sailResCommonDT.mResourceId) as EquipFragmentsDT;
            hasCountHint = "（"+CommonTools.f_GetTransLanguage(401) + count + "/" + equipFragmentDt.iBondNum + "）";
        }
        //1.先检查是否开启 2.再检查是否售空
        string HintStr = "";//开启条件提示
        bool isOpen = UITool.f_CheckOpen(openType, openValue, ref HintStr);
        bool isSailOut = false;//是否售空
        if (isOpen)
        {
            HintStr = "";
            if (maxCanBuyTimes > 0 && maxCanBuyTimes < 999999 && hasBuyTimes >= maxCanBuyTimes)
                isSailOut = true;
            else
                HintStr = CheckBuyTime(freshType, hasBuyTimes, maxCanBuyTimes);
        }
        item.GetComponent<ShopMutiItem>().SetData((EM_ResourceType)sailResCommonDT.mResourceType, sailResCommonDT.mResourceId, BorderName,
            Name, sailResCommonDT.mResourceNum, HintStr, buyTimes, moneyType1, price1, moneyType2, price2, disCount, isSailOut, isOpen, hasCountHint);
        f_RegClickEvent(item.Find("BtnBuy").gameObject, OnItemBuyClick, poolDT);
        f_RegClickEvent(item.GetComponent<ShopMutiItem>().Icon.gameObject, UITool.f_OnItemIconClick, sailResCommonDT);
    }
    /// <summary>
    /// item更新
    /// </summary>
    private void OnItemUpdate(Transform item, BasePoolDT<long> dt)
    {
        switch (em_currentShopType)//---------------------------case2（需要根据商店类型显示）
        {
            case EM_ShopMutiType.Reputation:
                ShopReputationPoolDT poolDT = dt as ShopReputationPoolDT;
                ResourceCommonDT resourceCommonDT = new ResourceCommonDT();
                resourceCommonDT.f_UpdateInfo((byte)poolDT.m_ReputationShopDT.iSailResTypeID, poolDT.m_ReputationShopDT.iSailResID, poolDT.m_ReputationShopDT.iSailResCount);
                if (resourceCommonDT.mResourceId == (int)EM_MoneyType.eCardRefineStone)
                    item.name = "JinJie" + poolDT.m_ReputationShopDT.iSailResCount;
                else
                    item.name = "0";
                SetItemData(item, dt, resourceCommonDT, (EM_MoneyType)poolDT.m_ReputationShopDT.iTokenResID1, poolDT.m_ReputationShopDT.iTokenResCount1,
                    (EM_MoneyType)poolDT.m_ReputationShopDT.iTokenResID2, poolDT.m_ReputationShopDT.iTokenResCount2,
                    poolDT.m_ReputationShopDT.iDiscount, poolDT.buyTimes, poolDT.m_ReputationShopDT.iCanBuyTimes,
                    poolDT.m_ReputationShopDT.iOpenType, poolDT.m_ReputationShopDT.iOpenValue, poolDT.m_ReputationShopDT.iRefreshType);
                break;
            case EM_ShopMutiType.Legion:
                ShopLegionPoolDT poolDT2 = dt as ShopLegionPoolDT;
                ResourceCommonDT resourceCommonDT2 = new ResourceCommonDT();
                resourceCommonDT2.f_UpdateInfo((byte)poolDT2.m_LegionShopDT.iSailResTypeID, poolDT2.m_LegionShopDT.iSailResID, poolDT2.m_LegionShopDT.iSailResCount);
                SetItemData(item, dt, resourceCommonDT2, (EM_MoneyType)poolDT2.m_LegionShopDT.iTokenResID1, poolDT2.m_LegionShopDT.iTokenResCount1,
                    (EM_MoneyType)poolDT2.m_LegionShopDT.iTokenResID2, poolDT2.m_LegionShopDT.iTokenResCount2,
                    poolDT2.m_LegionShopDT.iDiscount, poolDT2.buyTimes, poolDT2.m_LegionShopDT.iCanBuyTimes,
                    poolDT2.m_LegionShopDT.iOpenType, poolDT2.m_LegionShopDT.iOpenValue, poolDT2.m_LegionShopDT.iRefreshType);
                break;
            case EM_ShopMutiType.RunningMan:
                RunningManShopPoolDT poolDT3 = dt as RunningManShopPoolDT;
                ResourceCommonDT resourceCommonDT3 = new ResourceCommonDT();
                resourceCommonDT3.f_UpdateInfo((byte)poolDT3.m_Template.iSailResTypeID, poolDT3.m_Template.iSailResID, poolDT3.m_Template.iSailResCount);
                SetItemData(item, dt, resourceCommonDT3, (EM_MoneyType)poolDT3.m_Template.iTokenResID1, poolDT3.m_Template.iTokenResCount1,
                    (EM_MoneyType)poolDT3.m_Template.iTokenResID2, poolDT3.m_Template.iTokenResCount2,
                    poolDT3.m_Template.iDiscount, poolDT3.m_iBuyTimes, poolDT3.m_Template.iCanBuyTimes,
                    poolDT3.m_Template.iOpenType, poolDT3.m_Template.iOpenValue, poolDT3.m_Template.iRefreshType);
                break;

            case EM_ShopMutiType.BattleFeatShop:
                BattleFeatShopPoolDT poolDT4 = dt as BattleFeatShopPoolDT;
                ResourceCommonDT resourceCommonDT4 = new ResourceCommonDT();
                resourceCommonDT4.f_UpdateInfo((byte)poolDT4.m_BattleFeatShopDT.iSailResTypeID, poolDT4.m_BattleFeatShopDT.iSailResID, poolDT4.m_BattleFeatShopDT.iSailResCount);
                string Name4 = resourceCommonDT4.mName;
                string BorderName4 = UITool.f_GetImporentColorName(resourceCommonDT4.mImportant, ref Name4);
                if (resourceCommonDT4.mResourceId == 108)
                {
                    item.name = "MingXingShi" + poolDT4.m_BattleFeatShopDT.iSailResCount;
                }
                SetItemData(item, dt, resourceCommonDT4, (EM_MoneyType)poolDT4.m_BattleFeatShopDT.iTokenResID1, poolDT4.m_BattleFeatShopDT.iTokenResCount1,
                   (EM_MoneyType)poolDT4.m_BattleFeatShopDT.iTokenResID2, poolDT4.m_BattleFeatShopDT.iTokenResCount2,
                    0, 0, 0, 1, 0, 0);
                break;
            case EM_ShopMutiType.CrossServerBattle:
                CrossServerBattleShopPoolDT poolDT5 = dt as CrossServerBattleShopPoolDT;
                ResourceCommonDT resourceCommonDT5 = new ResourceCommonDT();
                resourceCommonDT5.f_UpdateInfo((byte)poolDT5.Template.iResourceType, poolDT5.Template.iResourceId, poolDT5.Template.iResourceNum);
                string Name5 = resourceCommonDT5.mName;
                string BorderName5 = UITool.f_GetImporentColorName(resourceCommonDT5.mImportant, ref Name5);
                SetItemData(item, dt, resourceCommonDT5, EM_MoneyType.eUserAttr_CrossServerScore, poolDT5.Template.iNeedScore,
                   0, 0, 0, poolDT5.BuyTimes, poolDT5.Template.iBuyTimesLimit, 1, 0);
                break;
            //TsuCode - ChaosBattle
            case EM_ShopMutiType.ChaosBattle:
                ChaosBattleShopPoolDT poolDT6 = dt as ChaosBattleShopPoolDT;
                ResourceCommonDT resourceCommonDT6 = new ResourceCommonDT();
                resourceCommonDT6.f_UpdateInfo((byte)poolDT6.Template.iResourceType, poolDT6.Template.iResourceId, poolDT6.Template.iResourceNum);
                string Name6 = resourceCommonDT6.mName;
                string BorderName6 = UITool.f_GetImporentColorName(resourceCommonDT6.mImportant, ref Name6);
                SetItemData(item, dt, resourceCommonDT6, EM_MoneyType.eUserAttr_ChaosScore, poolDT6.Template.iNeedScore,
                   0, 0, 0, poolDT6.BuyTimes, poolDT6.Template.iBuyTimesLimit, 1, 0);
                break;
            case EM_ShopMutiType.CrossArena:
                CrossArenaShopPoolDT poolDT7 = dt as CrossArenaShopPoolDT;
                ResourceCommonDT resourceCommonDT7 = new ResourceCommonDT();
                resourceCommonDT7.f_UpdateInfo((byte)poolDT7.Template.iResourceType, poolDT7.Template.iResourceId, poolDT7.Template.iResourceNum);
                string Name7 = resourceCommonDT7.mName;
                string BorderName7 = UITool.f_GetImporentColorName(resourceCommonDT7.mImportant, ref Name7);
                SetItemData(item, dt, resourceCommonDT7, EM_MoneyType.eUserAttr_ChaosScore, poolDT7.Template.iNeedScore,
                   0, 0, 0, poolDT7.BuyTimes, poolDT7.Template.iBuyTimesLimit, 1, 0);
                break;
            case EM_ShopMutiType.CrossTournament:
                CrossTournamentShopPoolDT poolDT8 = dt as CrossTournamentShopPoolDT;
                ResourceCommonDT resourceCommonDT8 = new ResourceCommonDT();
                resourceCommonDT8.f_UpdateInfo((byte)poolDT8.Template.iResourceType, poolDT8.Template.iResourceId, poolDT8.Template.iResourceNum);
                string Name8 = resourceCommonDT8.mName;
                string BorderName8 = UITool.f_GetImporentColorName(resourceCommonDT8.mImportant, ref Name8);
                SetItemData(item, dt, resourceCommonDT8, EM_MoneyType.eUserAttr_TournamentPoint, poolDT8.Template.iNeedScore,
                   0, 0, 0, poolDT8.BuyTimes, poolDT8.Template.iBuyTimesLimit, 1, 0);
                break;
            case EM_ShopMutiType.CampGemShop:
                CampGemShopPoolDT poolDT11 = dt as CampGemShopPoolDT;
                ResourceCommonDT resourceCommonDT11 = new ResourceCommonDT();
                resourceCommonDT11.f_UpdateInfo((byte)poolDT11.m_CampGemShopDT.iResourceType, poolDT11.m_CampGemShopDT.iResourceId, poolDT11.m_CampGemShopDT.iResourceNum);
                string Name11 = resourceCommonDT11.mName;
                string BorderName11 = UITool.f_GetImporentColorName(resourceCommonDT11.mImportant, ref Name11);
                SetItemData(item, dt, resourceCommonDT11, (EM_MoneyType)poolDT11.m_CampGemShopDT.iScoreId, poolDT11.m_CampGemShopDT.iScoreNum, 0, 0, 0, 0, 0, 1, 0, 0);
                break;
                //
        }
    }
    /// <summary>
    /// 点击购买
    /// </summary>
    private void OnItemBuyClick(GameObject go, object obj1, object obj2)
    {
        buyPropList.Clear();
        AwardPoolDT item1 = new AwardPoolDT();
        switch (em_currentShopType)//---------------------------case3（需要根据商店类型显示）
        {
            case EM_ShopMutiType.Reputation:
                ShopReputationPoolDT poolDT = obj1 as ShopReputationPoolDT;
                if (UITool.f_CheckEnoughWaste((byte)poolDT.m_ReputationShopDT.iTokenResTableID1, poolDT.m_ReputationShopDT.iTokenResID1, poolDT.m_ReputationShopDT.iTokenResCount1,
                    (byte)poolDT.m_ReputationShopDT.iTokenResTableID2, poolDT.m_ReputationShopDT.iTokenResID2, poolDT.m_ReputationShopDT.iTokenResCount2, this))
                {
                    item1.f_UpdateByInfo((byte)poolDT.m_ReputationShopDT.iSailResTypeID, poolDT.m_ReputationShopDT.iSailResID, poolDT.m_ReputationShopDT.iSailResCount);
                    Data_Pool.m_shopReputationPool.f_ShopRandBuy((int)poolDT.iId, OnBuyShopRandCallback);
                    UITool.f_OpenOrCloseWaitTip(true);
                }
                break;
            case EM_ShopMutiType.Legion:
                ShopLegionPoolDT poolDT2 = obj1 as ShopLegionPoolDT;
                if (UITool.f_CheckEnoughWaste((byte)poolDT2.m_LegionShopDT.iTokenResTableID1, poolDT2.m_LegionShopDT.iTokenResID1, poolDT2.m_LegionShopDT.iTokenResCount1,
                    (byte)poolDT2.m_LegionShopDT.iTokenResTableID2, poolDT2.m_LegionShopDT.iTokenResID2, poolDT2.m_LegionShopDT.iTokenResCount2, this))
                {
                    item1.f_UpdateByInfo((byte)poolDT2.m_LegionShopDT.iSailResTypeID, poolDT2.m_LegionShopDT.iSailResID, poolDT2.m_LegionShopDT.iSailResCount);
                    LegionMain.GetInstance().m_ShopLegionPool.f_ShopRandBuy((int)poolDT2.iId, OnBuyShopRandCallback);
                    UITool.f_OpenOrCloseWaitTip(true);
                }
                break;
            case EM_ShopMutiType.RunningMan:
                RunningManShopPoolDT poolDT3 = obj1 as RunningManShopPoolDT;
                if (UITool.f_CheckEnoughWaste((byte)poolDT3.m_Template.iTokenResTableID1, poolDT3.m_Template.iTokenResID1, poolDT3.m_Template.iTokenResCount1,
                   (byte)poolDT3.m_Template.iTokenResTableID2, poolDT3.m_Template.iTokenResID2, poolDT3.m_Template.iTokenResCount2, this))
                {
                    item1.f_UpdateByInfo((byte)poolDT3.m_Template.iSailResTypeID, poolDT3.m_Template.iSailResID, poolDT3.m_Template.iSailResCount);
                    Data_Pool.m_RunningManShopPool.f_RunningManShopBuy((ushort)poolDT3.iId, 1, OnBuyShopRandCallback);
                    UITool.f_OpenOrCloseWaitTip(true);
                }
                break;
            case EM_ShopMutiType.BattleFeatShop:
                BattleFeatShopPoolDT poolDT4 = obj1 as BattleFeatShopPoolDT;
                if (UITool.f_CheckEnoughWaste((byte)poolDT4.m_BattleFeatShopDT.iTokenResTableID1, poolDT4.m_BattleFeatShopDT.iTokenResID1, poolDT4.m_BattleFeatShopDT.iTokenResCount1,
                   (byte)poolDT4.m_BattleFeatShopDT.iTokenResTableID2, poolDT4.m_BattleFeatShopDT.iTokenResID2, poolDT4.m_BattleFeatShopDT.iTokenResCount2, this))
                {
                    BuyParam tBuyParam = new BuyParam();
                    tBuyParam.canBuyTimes = 0;
                    tBuyParam.iId = poolDT4.m_BattleFeatShopDT.iId;
                    tBuyParam.resourceID = poolDT4.m_BattleFeatShopDT.iSailResID;
                    tBuyParam.resourceType = (EM_ResourceType)poolDT4.m_BattleFeatShopDT.iSailResTypeID;
                    tBuyParam.resourceCount = poolDT4.m_BattleFeatShopDT.iSailResCount;
                    tBuyParam.moneyType = EM_MoneyType.eUserAttr_BattleFeat;
                    tBuyParam.resourceCount = poolDT4.m_BattleFeatShopDT.iSailResCount;
                    tBuyParam.onConfirmBuyCallback = OnBtnReturnBuyParam;
                    tBuyParam.price = new List<int>();
                    tBuyParam.price.Add(poolDT4.m_BattleFeatShopDT.iTokenResCount1);
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.BuyPage, UIMessageDef.UI_OPEN, tBuyParam);
                }
                break;
            case EM_ShopMutiType.CrossServerBattle:
                CrossServerBattleShopPoolDT poolDT5 = obj1 as CrossServerBattleShopPoolDT;
                if (UITool.f_CheckEnoughWaste((byte)EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_CrossServerScore, poolDT5.Template.iNeedScore, 0, 0, 0,this))
                {
                    UITool.f_OpenOrCloseWaitTip(true);
                    item1.f_UpdateByInfo((byte)poolDT5.Template.iResourceType, poolDT5.Template.iResourceId, poolDT5.Template.iResourceNum);
                    Data_Pool.m_CrossServerBattleShopPool.f_Buy((int)poolDT5.iId, 1, OnBuyShopRandCallback);
                }
                break;
            //TsuCode - ChaosBattle
            case EM_ShopMutiType.ChaosBattle:
                ChaosBattleShopPoolDT poolDT6 = obj1 as ChaosBattleShopPoolDT;
                MessageBox.ASSERT("TsuLog Get ChaosPoint: " + Data_Pool.m_UserData.f_GetProperty(21));
                if (UITool.f_CheckEnoughWaste((byte)EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_ChaosScore, poolDT6.Template.iNeedScore, 0, 0, 0, this))
                {
                    UITool.f_OpenOrCloseWaitTip(true);
                    item1.f_UpdateByInfo((byte)poolDT6.Template.iResourceType, poolDT6.Template.iResourceId, poolDT6.Template.iResourceNum);
                    Data_Pool.m_ChaosBattleShopPool.f_Buy((int)poolDT6.iId, 1, OnBuyShopRandCallback);
                }
                break;
            //
            case EM_ShopMutiType.CrossArena:
                CrossArenaShopPoolDT poolDT7 = obj1 as CrossArenaShopPoolDT;
                MessageBox.ASSERT("TsuLog Get CrossArena: " + Data_Pool.m_UserData.f_GetProperty(22));
                if (UITool.f_CheckEnoughWaste((byte)EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_ArenaCorssMoney, poolDT7.Template.iNeedScore, 0, 0, 0, this))
                {
                    UITool.f_OpenOrCloseWaitTip(true);
                    item1.f_UpdateByInfo((byte)poolDT7.Template.iResourceType, poolDT7.Template.iResourceId, poolDT7.Template.iResourceNum);
                    Data_Pool.m_CrossArenaPool.f_Buy((int)poolDT7.iId, 1, OnBuyShopRandCallback);
                }
                break;
            case EM_ShopMutiType.CrossTournament:
                CrossTournamentShopPoolDT poolDT8 = obj1 as CrossTournamentShopPoolDT;
                MessageBox.ASSERT("TsuLog Get CrossArena: " + Data_Pool.m_UserData.f_GetProperty((int)EM_MoneyType.eUserAttr_TournamentPoint));
                if (UITool.f_CheckEnoughWaste((byte)EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_TournamentPoint, poolDT8.Template.iNeedScore, 0, 0, 0, this))
                {
                    UITool.f_OpenOrCloseWaitTip(true);
                    item1.f_UpdateByInfo((byte)poolDT8.Template.iResourceType, poolDT8.Template.iResourceId, poolDT8.Template.iResourceNum);
                    Data_Pool.m_CrossTournamentPool.f_Buy((int)poolDT8.iId, 1, OnBuyShopRandCallback);
                }
                break;
            case EM_ShopMutiType.CampGemShop:
                CampGemShopPoolDT poolDT11 = obj1 as CampGemShopPoolDT;
                if (UITool.f_CheckEnoughWaste((byte)poolDT11.m_CampGemShopDT.iScoreType, poolDT11.m_CampGemShopDT.iScoreId, poolDT11.m_CampGemShopDT.iScoreNum,
                   0, 0, 0, this))
                {
                    UITool.f_OpenOrCloseWaitTip(true);
                    item1.f_UpdateByInfo((byte)poolDT11.m_CampGemShopDT.iResourceType, poolDT11.m_CampGemShopDT.iResourceId, poolDT11.m_CampGemShopDT.iResourceNum);
                    Data_Pool.m_CampGemShopPool.f_Buy((int)poolDT11.iId, 1, OnBuyShopRandCallback);
                }
                break;
        }
        if (em_currentShopType != EM_ShopMutiType.BattleFeatShop)
            buyPropList.Add(item1);
    }
    #region 按钮事件
    /// <summary>
    /// 批量购买回调
    /// </summary>
    /// <param name="iId">原始传参</param>
    /// <param name="type">购买的资源类型</param>
    /// <param name="resourceId">购买的资源id</param>
    /// <param name="resourceCount">购买的资源数量</param>
    /// <param name="buyCount">购买的数量</param>
    private void OnBtnReturnBuyParam(int iId, EM_ResourceType type, int resourceId, int resourceCount, int buyCount)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_BattleFeatShopPool.f_BattleFeatShopBuy(iId, buyCount, OnBuyShopRandCallback);
        AwardPoolDT item1 = new AwardPoolDT();
        item1.f_UpdateByInfo((byte)type, resourceId, resourceCount * buyCount);
        buyPropList.Add(item1);
    }
    /// <summary>
    /// 点击返回按钮
    /// </summary>
    private void OnBtnReturnClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal); 
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopMutiCommonPage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
    }
    /// <summary>
    /// 设置是否按下
    /// </summary>
    /// <param name="button">分页按钮</param>
    /// <param name="isPressed">是否处于按下状态</param>
    public void SetBtnState(GameObject button, bool isPressed)
    {
        button.transform.Find("BtnNormal").gameObject.SetActive(!isPressed);
        button.transform.Find("BtnPress").gameObject.SetActive(isPressed);
    }
    /// <summary>
    /// 设置分页显示按钮，其他处于未按下状态
    /// </summary>
    /// <param name="shopMutiPageTap">被按下的分页按钮</param>
    private void SetTapItemClick(EM_ShopMutiPageTap shopMutiPageTap)
    {
        SetBtnState(f_GetObject("BtnShop"), shopMutiPageTap == EM_ShopMutiPageTap.BtnShop);
        SetBtnState(f_GetObject("BtnAward"), shopMutiPageTap == EM_ShopMutiPageTap.BtnAward);
        SetBtnState(f_GetObject("BtnLimit"), shopMutiPageTap == EM_ShopMutiPageTap.BtnLimit);
        SetBtnState(f_GetObject("BtnPurpleEquip"), shopMutiPageTap == EM_ShopMutiPageTap.BtnPurpleEquip);
        SetBtnState(f_GetObject("BtnRedEquip"), shopMutiPageTap == EM_ShopMutiPageTap.BtnRedEquip);
    }
    /// <summary>
    /// 点击商店分页
    /// </summary>
    private void OnBtnShopClick(GameObject go, object obj1, object obj2)
    {
        SetTapItemClick(EM_ShopMutiPageTap.BtnShop);
        PageIndex = 1;
        InitOrUpdateShopUIData(PageIndex);
    }
    /// <summary>
    /// 点击奖励分页
    /// </summary>
    private void OnBtnAwardClick(GameObject go, object obj1, object obj2)
    {
        SetTapItemClick(EM_ShopMutiPageTap.BtnAward);
        if (em_currentShopType == EM_ShopMutiType.Legion)
            PageIndex = 3;
        else if (em_currentShopType == EM_ShopMutiType.RunningMan)
            PageIndex = 4;
        else
            PageIndex = 2;
        InitOrUpdateShopUIData(PageIndex);
    }
    /// <summary>
    /// 点击限时（军团商店专用）
    /// </summary>
    private void OnBtnLimitClick(GameObject go, object obj1, object obj2)
    {
        SetTapItemClick(EM_ShopMutiPageTap.BtnLimit);
        PageIndex = 2;
        InitOrUpdateShopUIData(PageIndex);
    }
    /// <summary>
    /// 点击紫装备(过关斩将：神装商店专用)
    /// </summary>
    private void OnBtnPurpleEquip(GameObject go, object obj1, object obj2)
    {
        SetTapItemClick(EM_ShopMutiPageTap.BtnPurpleEquip);
        PageIndex = 2;
        InitOrUpdateShopUIData(PageIndex);
    }
    /// <summary>
    /// 点击红装备(过关斩将：神装商店专用)
    /// </summary>
    private void OnBtnRedEquip(GameObject go, object obj1, object obj2)
    {
        SetTapItemClick(EM_ShopMutiPageTap.BtnRedEquip);
        PageIndex = 3;
        InitOrUpdateShopUIData(PageIndex);
    }
    #endregion

    private void f_ProcessByMsg_ForceClose(object value)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopMutiCommonPage, UIMessageDef.UI_CLOSE);
    }
}
