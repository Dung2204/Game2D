using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;

//新增打开参数类，，解决缺物获取途径跳转打开图鉴后再一个获取途径，，现在的底层管理不过来，会出现白屏
//所以如果是获取途径跳转过来则屏蔽图鉴按钮
public class ShopPageParam
{
    public string stSourceUIName;                 //来源界面名称 UINameConst枚举
    public ShopPage.EM_PageIndex emPageIndex;     //酒馆选项卡枚举
    public ShopResourceDT shopResDt;              //不知道以前为什么有这个参数，，现在没找到引用
    public ShopResourcePoolDT shopResourcePoolDT; //缺物跳转，需要购买的物品对象
}

/// <summary>
/// 商店界面
/// </summary>
public class ShopPage : UIFramwork
{
    //private List<BasePoolDT<long>> _propList;//道具列表
    private UIWrapComponent _propWrapComponet;
    private List<BasePoolDT<long>> _packsList;//礼包列表
    private UIWrapComponent _packsWrapComponet;//礼包拖动组件
    private SocketCallbackDT RequestShopRecruitCallback = new SocketCallbackDT();//请求抽牌信息回调
    private SocketCallbackDT RequestShopGiftCallback = new SocketCallbackDT();//请求礼包回调信息
    private SocketCallbackDT BuyShopGiftCallback = new SocketCallbackDT();//购买礼包
    //private SocketCallbackDT RequestShopPropListCallback = new SocketCallbackDT();//请求商店道具列表回调
    //private SocketCallbackDT BuyResourceCallback = new SocketCallbackDT();//购买道具结果回调
    private ShopGiftPoolDT currentSelectGiftPoolDT;//礼包商店当前选中的礼包DT
    private List<AwardPoolDT> buyPropList = new List<AwardPoolDT>();//购买完商店资源显示奖励
    private int singlePrice;
    private ShopResourceDT OpenSelectPropDt = null;
    private int propVipLmitTimes = 0;
    private int countLeft = 0;
    private EM_PageIndex cur_PageIndex;//当前选中的页面
    private string strTexBgRoot = "UI/TextureRemove/Shop/Tex_ShopBg";
    private string strTexLightRoot = "UI/TextureRemove/Shop/Tex_LightEffect";
    private Dictionary<ActType, GameObject> dicActTypeToContentObj = new Dictionary<ActType, GameObject>();
    private bool _isPopupRateShow = false;
	
	//My Code
    GameParamDT AssetOpen;
    //
	
    /// <summary>
    /// 礼包item更新
    /// </summary>
    private void OnPacksItemUpdate(Transform item, BasePoolDT<long> dt)
    {
        ShopGiftPoolDT shopGiftPoolDT = dt as ShopGiftPoolDT;
        bool showRedPoint = false;
        string strVipHint = string.Format(CommonTools.f_GetTransLanguage(1091), shopGiftPoolDT.m_shopGiftDT.iVipLimit);
        int limitVipLevel = shopGiftPoolDT.m_shopGiftDT.iVipLimit;
        int playerSycee = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee);
		
        //1.等级已达到则不显示
        if (UITool.f_GetNowVipLv() >= limitVipLevel)
        {
            strVipHint = "";
            if (playerSycee >= shopGiftPoolDT.m_shopGiftDT.iNewNum)
                showRedPoint = true;
        }
        BaseGoodsDT baseGoodDt = glo_Main.GetInstance().m_SC_Pool.m_BaseGoodsSC.f_GetSC(shopGiftPoolDT.m_shopGiftDT.iTempId) as BaseGoodsDT;
        ShopPacksItem itemCtl = item.GetComponent<ShopPacksItem>();
        itemCtl.SetData(string.Format(CommonTools.f_GetTransLanguage(1092), shopGiftPoolDT.m_shopGiftDT.iVipLimit), shopGiftPoolDT.m_shopGiftDT.iOldNum,
            shopGiftPoolDT.m_shopGiftDT.iNewNum, baseGoodDt.szReadme, strVipHint, showRedPoint);
        f_RegClickEvent(itemCtl.BtnBuy, OnPackItemBuyClick, dt);
        f_RegClickEvent(itemCtl.BtnIconProp, OnBtnIconPropClick, dt);
    }
    /// <summary>
    /// 点击道具icon,弹出礼包内容
    /// </summary>
    private void OnBtnIconPropClick(GameObject go, object obj1, object obj2)
    {
        ShopGiftPoolDT shopGiftPoolDT = obj1 as ShopGiftPoolDT;
        BaseGoodsDT baseGoodsDT = glo_Main.GetInstance().m_SC_Pool.m_BaseGoodsSC.f_GetSC(shopGiftPoolDT.m_shopGiftDT.iTempId) as BaseGoodsDT;
        List<AwardPoolDT> _awardList = Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(baseGoodsDT.iEffectData);
        ItemShowPageParam param = new ItemShowPageParam();
        List<ItemShowGoodItem> m_listItem = new List<ItemShowGoodItem>();
        for (int i = 0; i < _awardList.Count; i++)
        {
            m_listItem.Add(new ItemShowGoodItem((EM_ResourceType)_awardList[i].mTemplate.mResourceType,
                _awardList[i].mTemplate.mResourceId, _awardList[i].mTemplate.mResourceNum));
        }
        param.m_listItem = m_listItem;
        param.m_title = CommonTools.f_GetTransLanguage(1093);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ItemsShowPage, UIMessageDef.UI_OPEN, param);
    }
    /// <summary>
    /// 点击礼包item购买
    /// </summary>
    private void OnPackItemBuyClick(GameObject go, object obj1, object obj2)
    {
        ShopGiftPoolDT shopGiftPoolDT = obj1 as ShopGiftPoolDT;
        int limitVipLevel = shopGiftPoolDT.m_shopGiftDT.iVipLimit;
        int price = shopGiftPoolDT.m_shopGiftDT.iNewNum;
        if (UITool.f_GetNowVipLv() < limitVipLevel)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1094));
            return;
        }
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee) < price)
        {
            string tipContent = string.Format(CommonTools.f_GetTransLanguage(1095));
            ccUIManage.GetInstance().f_SendMsg(UINameConst.PayTipPage, UIMessageDef.UI_OPEN, tipContent);
            return;
        }
        currentSelectGiftPoolDT = shopGiftPoolDT;
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_ShopGiftPool.f_Buy(shopGiftPoolDT.m_shopGiftDT.iId, BuyShopGiftCallback);
    }
    //道具Item更新
    //private void PropItemUpdateByInfo(Transform item, BasePoolDT<long> dt)
    //{
    //    ShopResourcePoolDT propPoolData = dt as ShopResourcePoolDT;
    //    ShopResourceDT shopResourceDt = propPoolData.m_ShopResourceDT;
    //    //玩家vip等级
    //    int playerVipLevel = UITool.f_GetNowVipLv();
    //    ResourceCommonDT resourceCommonDT = new ResourceCommonDT();
    //    resourceCommonDT.f_UpdateInfo((byte)shopResourceDt.iType, shopResourceDt.iTempId, 0);
    //    string Name = resourceCommonDT.mName;
    //    string BorderName = UITool.f_GetImporentColorName(resourceCommonDT.mImportant, ref Name);
    //    string[] discountArray = shopResourceDt.szDIscount.Split(';');
    //    string discount = propPoolData.m_iBuyTimes > (discountArray.Length - 1) ? discountArray[discountArray.Length - 1] : discountArray[propPoolData.m_iBuyTimes];
    //    item.GetComponent<ShopPropItem>().SetData((EM_ResourceType)shopResourceDt.iType, shopResourceDt.iTempId, BorderName,
    //        Name, shopResourceDt.iNum, int.Parse(discount), GetTimesByVipLevel(shopResourceDt, playerVipLevel), GetPriceByBuyTimes(shopResourceDt, propPoolData.m_iBuyTimes),
    //        propPoolData.m_iBuyTimes);
    //    f_RegClickEvent(item.GetComponent<ShopPropItem>().BtnBuy, OnShopPropItemClick, dt);
    //    f_RegClickEvent(item.GetComponent<ShopPropItem>().Icon.gameObject, OnPropIconClick, dt);
    //}
    /// <summary>
    /// 点击道具的图标
    /// </summary>
    private void OnPropIconClick(GameObject go, object obj1, object obj2)
    {
        ShopResourcePoolDT data = obj1 as ShopResourcePoolDT;
        ShopResourceDT shopResourceDt = data.m_ShopResourceDT;
        ResourceCommonDT commonData = new ResourceCommonDT();
        commonData.f_UpdateInfo((byte)shopResourceDt.iType, shopResourceDt.iTempId, shopResourceDt.iNum);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, commonData);
    }
    /// <summary>
    /// 通过玩家VIP等级获取道具限购次数
    /// </summary>
    /// <param name="shopResourceDT">策划表里的商店道具数据item</param>
    /// <param name="VipLevel">玩家vip等级</param>
    private int GetTimesByVipLevel(ShopResourceDT shopResourceDt, int VipLevel)
    {
        string[] LimiTimes = shopResourceDt.szVipLimitTimes.Split(';');
        if (VipLevel > LimiTimes.Length - 1)
        {
            Debug.LogError(CommonTools.f_GetTransLanguage(1096));
            VipLevel = LimiTimes.Length - 1;
        }
        return int.Parse(LimiTimes[VipLevel]);
    }
    /// <summary>
    /// 通过玩家已经购买次数获取道具价格
    /// </summary>
    /// <param name="shopResourceDT">策划表里的商店道具数据item</param>
    /// <param name="buyTimes">已经购买了得次数</param>
    /// <returns></returns>
    private int GetPriceByBuyTimes(ShopResourceDT shopResourceDT, int buyTimes)
    {
        string[] priceArray = shopResourceDT.szNewNum.Split(';');
        buyTimes = buyTimes > (priceArray.Length - 1) ? (priceArray.Length - 1) : buyTimes;
        int price = int.Parse(priceArray[buyTimes]);
        return price;
    }

    /// <summary>
    /// 点击item里面购买道具按钮   ------(网络接口)
    /// </summary>
    //private void OnShopPropItemClick(GameObject obj, object obj1, object obj2)
    //{
    //    ShopResourcePoolDT data = obj1 as ShopResourcePoolDT;
    //    ShopResourceDT shopResourceDt = data.m_ShopResourceDT;
    //    singlePrice = GetPriceByBuyTimes(shopResourceDt, data.m_iBuyTimes);
    //    int userVipLevel = UITool.f_GetNowVipLv();
    //    propVipLmitTimes = GetTimesByVipLevel(shopResourceDt, userVipLevel);
    //    countLeft = propVipLmitTimes == 0 ? 0 : (propVipLmitTimes - data.m_iBuyTimes);
    //    if (countLeft < 1 && propVipLmitTimes != 0)
    //    {
    //        countLeft = -1;//不可再买
    //    }
    //    BuyParam param = new BuyParam();
    //    param.iId = data.m_ShopResourceDT.iId;
    //    param.title = CommonTools.f_GetTransLanguage(1097);
    //    param.buyHint = propVipLmitTimes > 0 ? string.Format(CommonTools.f_GetTransLanguage(1098), countLeft <= 0 ? 0 : countLeft) : "";
    //    param.canBuyTimes = countLeft;
    //    param.resourceType = (EM_ResourceType)data.m_ShopResourceDT.iType;
    //    param.resourceID = data.m_ShopResourceDT.iTempId;
    //    param.resourceCount = data.m_ShopResourceDT.iNum;
    //    param.moneyType = EM_MoneyType.eUserAttr_Sycee;
    //    string[] priceArray = data.m_ShopResourceDT.szNewNum.Split(';');
    //    List<int> priceIntArray = new List<int>();
    //    for (int i = 0; i < priceArray.Length; i++)
    //    {
    //        if (i >= data.m_iBuyTimes)
    //            priceIntArray.Add(int.Parse(priceArray[i]));
    //    }
    //    if (priceIntArray.Count == 0)
    //    {
    //        priceIntArray.Add(int.Parse(priceArray[priceArray.Length - 1]));
    //    }
    //    param.price = priceIntArray;
    //    param.onConfirmBuyCallback = OnConfirmBuyCallback;
    //    ccUIManage.GetInstance().f_SendMsg(UINameConst.BuyPage, UIMessageDef.UI_OPEN, param);
    //}
    /// <summary>
    /// 页面关闭
    /// </summary>
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        CancelInvoke("CheckIsFree");
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.MainBg);
    }
    /// <summary>
    /// 界面打开
    /// </summary>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        //Vector3 posSprBg = f_GetObject("TexBackGround").transform.localPosition;
        //float ratioValue = Screen.height / StaticValue.m_DesignScreenHeight;
        //posSprBg.x = (328 * ratioValue + (Screen.width - 328 * ratioValue) / 2)/ratioValue;
        //f_GetObject("TexBackGround").transform.localPosition = posSprBg;
		AssetOpen = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(93) as GameParamDT);
        _isPopupRateShow = false;
        bool isFromGetWay = false;
        if (e != null && e is ShopPageParam)//需要默认选中页面
        {
            ShopPageParam shopPageParam = e as ShopPageParam;
            if (null == shopPageParam)
            {
                MessageBox.ASSERT("shopPageParam null！！！？");
                return;
            }
            isFromGetWay = shopPageParam.stSourceUIName == UINameConst.GetWayPage;
            //if (null != shopPageParam.shopResourcePoolDT)
            //{
            //    //后面加得缺物跳转商城弹出二级购买界面，ShopResourcePoolDT为购买物品信息
            //    ShopResourcePoolDT shopRes = shopPageParam.shopResourcePoolDT as ShopResourcePoolDT;
            //    OnBtnPropsClick(null, null, null);
            //    OnShopPropItemClick(null, shopRes, null);
            //}
            //else if (null != shopPageParam.shopResDt)
            //{
            //    //没找到地方调用，，不知道用来干嘛得
            //    OpenSelectPropDt = shopPageParam.shopResDt as ShopResourceDT;
            //    if (OpenSelectPropDt != null)
            //    {
            //        OnBtnPropsClick(null, null, null);
            //    }
            //}
            //else
            //{
                switch (shopPageParam.emPageIndex)
                {
                    case EM_PageIndex.RecruitContent:
                        OnBtnRecruitClick(null, null, null);
                        break;
                    case EM_PageIndex.RecruitContentGen:
                        OnBtnRecruitClick(null, null, null);
                        break;
                    case EM_PageIndex.PacksContent:
                        OnBtnPacksClick(null, null, null);
                        break;
                    //case EM_PageIndex.PropsContent:
                    //    OnBtnPropsClick(null, null, null);
                    //    break;
                    //case EM_PageIndex.CardShow:
                    //    OnBtnPropsClick(null, null, null);
                    //    break;
                    case EM_PageIndex.RecruitContentCamp:
                        OnBtnRecruitClick(null, null, null);
                        break;
                }
            //}
        }else if(e != null && e is int)
        {
            cur_PageIndex = (EM_PageIndex) e;
            UpdateContent(cur_PageIndex);//默认打开为抽牌界面
        }
        else
        {
            cur_PageIndex = EM_PageIndex.RecruitContent;
            UpdateContent(cur_PageIndex);//默认打开为抽牌界面
        }

        //根据来源设置图鉴显隐，，如果来源是获取途径，再打开图鉴以及点图鉴里得获取途径，，以现阶段得底层界面管理混乱地管理不过来，会白屏，所以直接隐藏
        GameObject objBtnCardShow = f_GetObject("BtnCardShow");
        // objBtnCardShow.SetActive(!isFromGetWay);
        //RecruitPage recruitPage = f_GetObject("ObjRecruitContent").GetComponent<RecruitPage>();
        //if (null != recruitPage)
        //{
        //    recruitPage.f_GetObject("Btn_NorShow").SetActive(!isFromGetWay);
        //    recruitPage.f_GetObject("Btn_GenShow").SetActive(!isFromGetWay);
        //}

		if(AssetOpen.iParam4 == 0)
		{
			objBtnCardShow.SetActive(false);
			f_GetObject("BtnRecruit").SetActive(false);
			f_GetObject("BtnRecruitGen").SetActive(false);
            f_GetObject("BtnRecruitCamp").SetActive(false);
        }
        f_GetObject("PopupRate").SetActive(_isPopupRateShow);
        //UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_ShopLotteryPool.f_GetLotteryShopInfo(RequestShopRecruitCallback);//请求商店抽牌信息
        Data_Pool.m_ShopGiftPool.f_GetNewShop(OnShowShopGiftSucCallback);//请求商店礼包信息
        //Data_Pool.m_ShopResourcePool.f_GetNewShop(RequestShopPropListCallback);//请求商店列表
        f_LoadTexture();

        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.ChargeBg);
    }
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        //加载背景图
        UITexture Tex_ShopBg = f_GetObject("Tex_ShopBg").GetComponent<UITexture>();
        if (Tex_ShopBg.mainTexture == null)
        {
            Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot);
            Tex_ShopBg.mainTexture = tTexture2D;

            UITexture TexLight = f_GetObject("TexLight").GetComponent<UITexture>();
            Texture2D tTexLight = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexLightRoot);
            TexLight.mainTexture = tTexLight;
        }
    }


    /// <summary>
    /// 检测战将和神将是否免费（1秒更新）
    /// </summary>
    private void CheckIsFree()
    {
        ShopLotteryPoolDT lotteryPoolDT = Data_Pool.m_ShopLotteryPool.f_GetForId((long)EM_RecruitType.NorAd) as ShopLotteryPoolDT;
        int secondLeft = lotteryPoolDT.shopLotteryDT.iFreeCD * 60 - (GameSocket.GetInstance().f_GetServerTime() - lotteryPoolDT.lastFreeTime);
        ShowIsRecruitFreeInfo(f_GetObject("LabelNorAdFreeLeft").transform.GetComponent<UILabel>(), secondLeft < 0 ? true : false, secondLeft);
        ShopLotteryPoolDT lotteryPoolDT2 = Data_Pool.m_ShopLotteryPool.f_GetForId((long)EM_RecruitType.GenAd) as ShopLotteryPoolDT;
        int secondLeft2 = lotteryPoolDT2.shopLotteryDT.iFreeCD * 60 - (GameSocket.GetInstance().f_GetServerTime() - lotteryPoolDT2.lastFreeTime);
        ShowIsRecruitFreeInfo(f_GetObject("LabelGenAdFreeLeft").transform.GetComponent<UILabel>(), secondLeft2 < 0 ? true : false, secondLeft2);
        bool isFree = secondLeft < 0 && (secondLeft + 1) >= 0;
        bool isFree2 = secondLeft2 < 0 && (secondLeft2 + 1) >= 0;
        if (isFree || isFree2)//更新红点和免费文本
        {
            f_GetObject("ObjRecruitContent").GetComponent<RecruitPage>().RefreshUI(this, (int)cur_PageIndex);
            Data_Pool.m_ShopLotteryPool.CheckRedDot();
        }
    }
    /// <summary>
    /// 显示战将或者神将倒计时信息
    /// </summary>
    private void ShowIsRecruitFreeInfo(UILabel hint, bool isFree, int secondLeft)
    {
        hint.text = isFree ? "" : "[1eff00]" + CommonTools.f_GetStringBySecond(secondLeft) + CommonTools.f_GetTransLanguage(1100);
    }
    #region 服务器回调
    /// <summary>
    /// 购买商店礼包成功
    /// </summary>
    private void OnGiftBuySucCallback(object obj)
    {
        _packsList = CommonTools.f_CopyPoolDTArrayToNewList(Data_Pool.m_ShopGiftPool.f_GetAll());
        RemoveGiftHasBuy(_packsList);
        _packsWrapComponet.f_UpdateList(_packsList);
        _packsWrapComponet.f_ResetView();
        f_GetObject("ObjPacksContent").transform.Find("LabelNoGiftHint").gameObject.SetActive(_packsList.Count <= 0 ? true : false);
        List<AwardPoolDT> awardList = new List<AwardPoolDT>();
        AwardPoolDT item1 = new AwardPoolDT();
        item1.f_UpdateByInfo((byte)currentSelectGiftPoolDT.m_shopGiftDT.iType, currentSelectGiftPoolDT.m_shopGiftDT.iTempId, currentSelectGiftPoolDT.m_shopGiftDT.iNum);
        awardList.Add(item1);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { awardList });
        UITool.f_OpenOrCloseWaitTip(false);
    }
    /// <summary>
    /// 购买礼包失败
    /// </summary>
    private void OnGiftBuyFailCallback(object obj)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1101));
        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void OnShowShopGiftSucCallback(object obj)
    {
        Debug.Log(CommonTools.f_GetTransLanguage(1102));
    }
    /// <summary>
    /// 请求抽牌信息成功
    /// </summary>
    private void OnShowShopLotterySucCallback(object obj)
    {
        UpdateContent(cur_PageIndex);//默认打开为抽牌界面
        InvokeRepeating("CheckIsFree", 0f, 1f);//成功之后开始检测倒计时
    }
    /// <summary>
    /// 请求抽牌信息失败
    /// </summary>
    private void OnShowShopLotteryFailCallback(object obj)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1103));
    }
    /// <summary>
    /// 请求商店资源列表成功
    /// </summary>
    /// <param name="obj"></param>
    //private void OnShowPropListSuccessCallback(object obj)
    //{
    //    UITool.f_OpenOrCloseWaitTip(false);
    //    if (OpenSelectPropDt != null)
    //    {
    //        List<BasePoolDT<long>> listData = Data_Pool.m_ShopResourcePool.f_GetAll();
    //        for (int i = 0; i < listData.Count; i++)
    //        {
    //            ShopResourcePoolDT shopResourcePoolDT = listData[i] as ShopResourcePoolDT;
    //            if (shopResourcePoolDT.m_ShopResourceDT.iId == OpenSelectPropDt.iId)
    //            {
    //                OnShopPropItemClick(null, shopResourcePoolDT, null);
    //                OpenSelectPropDt = null;
    //                break;
    //            }
    //        }
    //    }
    //}
    /// <summary>
    /// 请求商店资源列表失败
    /// </summary>
    /// <param name="obj"></param>
    private void OnShowPropListFailCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1104));
    }
    /// <summary>
    /// 购买商店资源成功
    /// </summary>
    //private void OnBuyPropSuccessCallback(object obj)
    //{
    //    //更新道具Item UI,元宝
    //    _propList = CommonTools.f_CopyPoolDTArrayToNewList(Data_Pool.m_ShopResourcePool.f_GetAll());
    //    CheckShopResourceFormat(_propList);
    //    _propWrapComponet.f_UpdateView();

    //    ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
    //        new object[] { buyPropList });
    //    UITool.f_OpenOrCloseWaitTip(false);
    //}
    /// <summary>
    /// 购买商店资源失败
    /// </summary>
    private void OnBuyPropFailCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        switch (teMsgOperateResult)
        {
            case eMsgOperateResult.eOR_TimesLimit:
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1105));
                break;
            case eMsgOperateResult.eOR_Sycee:
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1106));
                break;
            default:
                // ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1107) + teMsgOperateResult.ToString());
				ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1107));
                break;
        }
    }
    #endregion
    /// <summary>
    /// 检查商店资源数据合法性
    /// </summary>
    /// <param name="list">列表</param>
    private void CheckShopResourceFormat(List<BasePoolDT<long>> list)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            ShopResourcePoolDT data = list[i] as ShopResourcePoolDT;
            if (!data.m_ShopResourceDT.szNewNum.Contains(";"))
            {
                list.RemoveAt(i);
            }
            if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < data.m_ShopResourceDT.iShowLv)
            {
                list.RemoveAt(i);
            }
        }
    }
    /// <summary>
    /// 初始化消息事件
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnRate", OnBtnRateClick);
        f_RegClickEvent("BtnLogs", OnBtnLogsClick);
        f_RegClickEvent("BtnReturn", OnBtnReturnClick);
        f_RegClickEvent("BtnRecruit", OnBtnRecruitClick);
        f_RegClickEvent("BtnRecruitGen", OnBtnRecruitGenClick);
        f_RegClickEvent("BtnRecruitCamp", OnBtnRecruitCampClick);
        f_RegClickEvent("BtnPacks", OnBtnPacksClick);
        //f_RegClickEvent("BtnProps", OnBtnPropsClick);
        f_RegClickEvent("TexNorAdmiral", OnTexNorAdmiralClick);
        f_RegClickEvent("TexGenAdmiral", OnTexGenAdmiralClick);
        f_RegClickEvent("BtnCardShow", OnBtnCardShowClick);
        RequestShopRecruitCallback.m_ccCallbackSuc = OnShowShopLotterySucCallback;
        RequestShopRecruitCallback.m_ccCallbackFail = OnShowShopLotteryFailCallback;
        BuyShopGiftCallback.m_ccCallbackSuc = OnGiftBuySucCallback;
        BuyShopGiftCallback.m_ccCallbackFail = OnGiftBuyFailCallback;
        //BuyResourceCallback.m_ccCallbackSuc = OnBuyPropSuccessCallback;
        //BuyResourceCallback.m_ccCallbackFail = OnBuyPropFailCallback;

        //RequestShopPropListCallback.m_ccCallbackSuc = OnShowPropListSuccessCallback;
        //RequestShopPropListCallback.m_ccCallbackFail = OnShowPropListFailCallback;

        // điều chỉnh child scale
        //Vector3 posItemRoot = f_GetObject("ObjCardContent").transform.localPosition;
        //posItemRoot.x = 360 / 2 * ScreenControl.Instance.mFunctionRatio;//360为左侧菜单宽度
        //f_GetObject("ObjCardContent").transform.localPosition = posItemRoot;
    }
    #region 按钮事件
    /// <summary>
    /// 招募点击武将
    /// </summary>
    private void OnTexNorAdmiralClick(GameObject go, object obj1, object obj2)
    {
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.RecruitPage, UIMessageDef.UI_OPEN, EM_RecruitType.NorAd);
        //ccUIHoldPool.GetInstance().f_Hold(this);
    }
    /// <summary>
    /// 招募点击神将
    /// </summary>
    private void OnTexGenAdmiralClick(GameObject go, object obj1, object obj2)
    {
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.RecruitPage, UIMessageDef.UI_OPEN, EM_RecruitType.GenAd);
        //ccUIHoldPool.GetInstance().f_Hold(this);
    }
    private void OnBtnRateClick(GameObject go, object obj1, object obj2)
    {
        if (_isPopupRateShow)
        {
            _isPopupRateShow = false;
            f_GetObject("PopupRate").SetActive(_isPopupRateShow);
            return;
        }
        else
        {
            _isPopupRateShow = true;
        }
        UILabel lbRate = f_GetObject("LbRate").GetComponent<UILabel>();
        lbRate.text = "";
        if (cur_PageIndex == EM_PageIndex.RecruitContent)
        {
            lbRate.text = CommonTools.f_GetTransLanguage(2338);
        }
        else if (cur_PageIndex == EM_PageIndex.RecruitContentGen)
        {
            lbRate.text = CommonTools.f_GetTransLanguage(2339);
        }
        else
        {
            lbRate.text = CommonTools.f_GetTransLanguage(2340);
        }
        f_GetObject("PopupRate").SetActive(_isPopupRateShow);
    }
    private void OnBtnLogsClick(GameObject go, object obj1, object obj2)
    {
        if (cur_PageIndex == EM_PageIndex.RecruitContent)
        {
            ShopLotteryPoolDT lotteryNorPoolDT = Data_Pool.m_ShopLotteryPool.f_GetForId((long)EM_RecruitType.NorAd) as ShopLotteryPoolDT;

        }
        else if (cur_PageIndex == EM_PageIndex.RecruitContentGen)
        {
            ShopLotteryPoolDT lotteryGenPoolDT = Data_Pool.m_ShopLotteryPool.f_GetForId((long)EM_RecruitType.GenAd) as ShopLotteryPoolDT;
        }
        else
        {
            ShopLotteryPoolDT lotteryCampPoolDT = Data_Pool.m_ShopLotteryPool.f_GetForId((long)EM_RecruitType.CampAd) as ShopLotteryPoolDT;
        }
    }
    /// <summary>
    /// 点击卡牌图鉴
    /// </summary>
    private void OnBtnCardShowClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CardShowPage, UIMessageDef.UI_OPEN, this);
        //cur_PageIndex = EM_PageIndex.CardShow;
        //UpdateContent(cur_PageIndex);
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.CardShowPage, UIMessageDef.UI_OPEN, this);
    }
    /// <summary>
    /// 点击确认购买道具弹窗确认按钮
    /// </summary>
    //private void OnConfirmBuyCallback(int iId, EM_ResourceType type, int resourceId, int resourceCount, int buyCount)
    //{
    //    glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);


    //    int userSycee = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee);
    //    if (userSycee < singlePrice * buyCount)
    //    {
    //        string tipContent = string.Format(CommonTools.f_GetTransLanguage(1108));
    //        ccUIManage.GetInstance().f_SendMsg(UINameConst.PayTipPage, UIMessageDef.UI_OPEN, tipContent);
    //        return;
    //    }
    //    if (countLeft < 1 && propVipLmitTimes != 0)
    //    {
    //        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1109));
    //        return;
    //    }
    //    UITool.f_OpenOrCloseWaitTip(true);
    //    Data_Pool.m_ShopResourcePool.f_Buy(iId, buyCount, BuyResourceCallback);
    //    buyPropList.Clear();
    //    AwardPoolDT item1 = new AwardPoolDT();
    //    item1.f_UpdateByInfo((byte)type, resourceId, resourceCount * buyCount);
    //    buyPropList.Add(item1);
    //}
    /// <summary>
    /// 点击返回键事件
    /// </summary>
    private void OnBtnReturnClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
		GameObject DungeonTollGatePage = GameObject.Find("DungeonTollgatePageNew");
		if(DungeonTollGatePage != null)
		{
			if((DungeonTollGatePage.transform.Find("Panel")).gameObject.activeSelf)
			{
				MessageBox.ASSERT("DungeonTollGate: " + (DungeonTollGatePage.transform.Find("Panel")).gameObject.activeSelf);
			}
			else
			{
				ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
			}
		}
		else
		{
			ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
		}
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopPage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
    }
    /// <summary>
    /// 点击招募按钮触发事件
    /// </summary>
    private void OnBtnRecruitClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        cur_PageIndex = EM_PageIndex.RecruitContent;
        UpdateContent(cur_PageIndex);
        if (_isPopupRateShow)
        {
            _isPopupRateShow = false;
            f_GetObject("PopupRate").SetActive(_isPopupRateShow);
        }
    }

    private void OnBtnRecruitGenClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        cur_PageIndex = EM_PageIndex.RecruitContentGen;
        UpdateContent(cur_PageIndex);
        if (_isPopupRateShow)
        {
            _isPopupRateShow = false;
            f_GetObject("PopupRate").SetActive(_isPopupRateShow);
        }
    }
    private void OnBtnRecruitCampClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        cur_PageIndex = EM_PageIndex.RecruitContentCamp;
        UpdateContent(cur_PageIndex);
        if (_isPopupRateShow)
        {
            _isPopupRateShow = false;
            f_GetObject("PopupRate").SetActive(_isPopupRateShow);
        }
    }
    /// <summary>
    /// 点击礼包触发事件
    /// </summary>
    private void OnBtnPacksClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        _packsList = CommonTools.f_CopyPoolDTArrayToNewList(Data_Pool.m_ShopGiftPool.f_GetAll());
        RemoveGiftHasBuy(_packsList);
        if (_packsWrapComponet == null)
        {
            _packsWrapComponet = new UIWrapComponent(370, 1, 830, 7, f_GetObject("PacksItemParent"), f_GetObject("PacksItem"), _packsList, OnPacksItemUpdate, null);
        }
        _packsWrapComponet.f_UpdateList(_packsList);
        _packsWrapComponet.f_ResetView();
        f_GetObject("ObjPacksContent").transform.Find("LabelNoGiftHint").gameObject.SetActive(_packsList.Count <= 0 ? true : false);
        cur_PageIndex = EM_PageIndex.PacksContent;
        UpdateContent(EM_PageIndex.PacksContent);
        _packsWrapComponet.f_UpdateView();
    }
    /// <summary>
    /// 去除已经购买的礼包,去除表里没有的礼包
    /// </summary>
    /// <param name="listData"></param>
    private void RemoveGiftHasBuy(List<BasePoolDT<long>> listData)
    {
        int vipLevel = UITool.f_GetNowVipLv();
        for (int i = listData.Count - 1; i >= 0; i--)
        {
            ShopGiftPoolDT shopGiftPoolDT = listData[i] as ShopGiftPoolDT;
            if (shopGiftPoolDT.m_buyTimes >= 1 || shopGiftPoolDT.m_shopGiftDT.iVipLimit > vipLevel + 1)
                listData.RemoveAt(i);
        }
    }
    /// <summary>
    /// 点击道具触发事件
    /// </summary>
    //private void OnBtnPropsClick(GameObject go, object obj1, object obj2)
    //{
    //    glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
    //    _propList = Data_Pool.m_ShopResourcePool.f_GetAll();
    //    CheckShopResourceFormat(_propList);
    //    if (_propWrapComponet == null)
    //    {
    //        _propWrapComponet = new UIWrapComponent(500, 3, 450, 5, f_GetObject("PropItemParent"), f_GetObject("PropItem"), _propList, PropItemUpdateByInfo, null);
    //    }
    //    _propWrapComponet.f_ResetView();
    //    _propWrapComponet.f_UpdateView();
    //    cur_PageIndex = EM_PageIndex.PropsContent;
    //    UpdateContent(EM_PageIndex.PropsContent);
    //}
    #endregion
    /// <summary>
    /// 更新页码内容
    /// </summary>
    private void UpdateContent(EM_PageIndex pageIndex)
    {
        f_GetObject("ObjRecruitContent").SetActive(pageIndex == EM_PageIndex.RecruitContent || pageIndex == EM_PageIndex.RecruitContentGen || pageIndex == EM_PageIndex.RecruitContentCamp);
        f_GetObject("ObjPacksContent").SetActive(pageIndex == EM_PageIndex.PacksContent);
        f_GetObject("ObjPropsContent").SetActive(pageIndex == EM_PageIndex.PropsContent);
        //f_GetObject("ObjCardContent").SetActive(pageIndex == EM_PageIndex.CardShow);
        SetButtonState(f_GetObject("BtnRecruit"), pageIndex == EM_PageIndex.RecruitContent);
        SetButtonState(f_GetObject("BtnRecruitGen"), pageIndex == EM_PageIndex.RecruitContentGen);
        SetButtonState(f_GetObject("BtnPacks"), pageIndex == EM_PageIndex.PacksContent);
        SetButtonState(f_GetObject("BtnProps"), pageIndex == EM_PageIndex.PropsContent);
        //SetButtonState(f_GetObject("BtnCardShow"), pageIndex == EM_PageIndex.CardShow);
        SetButtonState(f_GetObject("BtnRecruitCamp"), pageIndex == EM_PageIndex.RecruitContentCamp);
        List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
        //listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
        if (pageIndex == EM_PageIndex.RecruitContent)//招募
        {
            f_GetObject("ObjRecruitContent").GetComponent<RecruitPage>().RefreshUI(this, (int)pageIndex);
            listMoneyType.Add(EM_MoneyType.eNorAd);
            //listMoneyType.Add(EM_MoneyType.eGenAd);
            //InitRecruitModel(f_GetObject("TexNorAdmiral"), 1409, false, new Vector3(0, -251, 0));
            //InitRecruitModel(f_GetObject("TexGenAdmiral"), 1100, true, new Vector3(0, -230, 0));
        }
        else if(pageIndex == EM_PageIndex.RecruitContentGen)
        {
            f_GetObject("ObjRecruitContent").GetComponent<RecruitPage>().RefreshUI(this, (int)pageIndex);
            listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
            listMoneyType.Add(EM_MoneyType.eGenAd);
        }
        //else if (pageIndex == EM_PageIndex.CardShow)
        //{
        //    UpdateContentData(ActType.CardShow);
        //    listMoneyType.Add(EM_MoneyType.eUserAttr_Money);
        //    listMoneyType.Add(EM_MoneyType.eUserAttr_Energy);
        //}
        else if (pageIndex == EM_PageIndex.RecruitContentCamp)
        {
            f_GetObject("ObjRecruitContent").GetComponent<RecruitPage>().RefreshUI(this, (int)pageIndex);
            listMoneyType.Add(EM_MoneyType.eGemCamp);
            listMoneyType.Add(EM_MoneyType.eCampAd);
        }
        else
        {
            listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
            listMoneyType.Add(EM_MoneyType.eUserAttr_Money);
            listMoneyType.Add(EM_MoneyType.eUserAttr_Energy);
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, listMoneyType);
        f_GetObject("BtnCardShow").SetActive(pageIndex != EM_PageIndex.PropsContent);
    }

    //private void UpdateContentData(ActType item)//----------------------case 4
    //{
    //    ActType actType = item;
    //    if (dicActTypeToContentObj.ContainsKey(ActType.CardShow))
    //        dicActTypeToContentObj[ActType.CardShow].GetComponent<CardShowPage>().f_DestoryView();
    //    switch (actType)
    //    {
    //        case ActType.CardShow:
    //            f_LoadViewContent(ActType.CardShow, "UI/UIPrefab/GameMain/CardShow/CardShowPage", f_GetObject("ObjCardContent"));
    //            dicActTypeToContentObj[ActType.CardShow].GetComponent<CardShowPage>().f_ShowView(this);
    //            break;
    //    }
    //}
    private GameObject f_LoadViewContent(ActType actType, string viewContentPath, GameObject objParent)
    {
        if (dicActTypeToContentObj.ContainsKey(actType))
            return null;
        GameObject oriObj = Resources.Load<GameObject>(viewContentPath);
        // ScreenAdaptive.Type adaptype = ScreenAdaptive.Type.Function;
        // if (oriObj.GetComponent<ScreenAdaptive>() != null)
            // adaptype = oriObj.GetComponent<ScreenAdaptive>().m_type;

        GameObject ViewContent = GameObject.Instantiate(oriObj) as GameObject;
        // Destroy(ViewContent.GetComponent<ScreenAdaptive>());
        ViewContent.transform.SetParent(objParent.transform);
        ViewContent.transform.localPosition = Vector3.zero;
        ViewContent.transform.localEulerAngles = Vector3.zero;
        ViewContent.transform.localScale = Vector3.one;
        ViewContent.gameObject.SetActive(true);
        // ViewContent.AddComponent<ScreenAdaptive>().m_type = adaptype;
        dicActTypeToContentObj.Add(actType, ViewContent);
        return ViewContent;
    }
    /// <summary>
    /// 设置模型
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="cardId"></param>
    private void InitRecruitModel(GameObject parent, int cardId, bool dir, Vector3 postion)
    {
        if (parent.transform.Find("Model") != null)
            return;
        UITool.f_GetStatelObject(cardId, parent.transform, dir ? Vector3.zero : new Vector3(0, 180, 0), postion, 6, "Model", 170);
    }
    /// <summary>
    /// 设置按钮状态
    /// </summary>
    public static void SetButtonState(GameObject buttonBg, bool isAct)
    {
        buttonBg.transform.Find("BtnBg").gameObject.SetActive(!isAct);
        buttonBg.transform.Find("BtnBgDown").gameObject.SetActive(isAct);
    }
    /// <summary>
    /// 页码
    /// </summary>
    public enum EM_PageIndex
    {
        RecruitContent = 1,
        RecruitContentGen = 2,
        PropsContent = 3,
        PacksContent = 4,
        CardShow = 5,
        RecruitContentCamp = 6,
    }

    public enum ActType
    {
        CardShow =1
    }

    #region 红点提示
    protected override void InitRaddot()
    {
        base.InitRaddot();

        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.NorAdmiralFree, f_GetObject("BtnRecruit"), ReddotCallback_Show_Btn_Recruit, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.GenAdmiralFree, f_GetObject("BtnRecruitGen"), ReddotCallback_Show_Btn_RecruitCamp, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.ShopGiftBuy, f_GetObject("BtnPacks"), ReddotCallback_Show_Btn_Gift);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.GenAdmiralFree, f_GetObject("BtnRecruitCamp"), ReddotCallback_Show_Btn_RecruitGen, true);
        UpdateReddotUI();
    }
    protected override void On_Destory()
    {
        base.On_Destory();
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.NorAdmiralFree, f_GetObject("BtnRecruit"), true);
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.GenAdmiralFree, f_GetObject("BtnRecruitGen"), true);
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.ShopGiftBuy, f_GetObject("BtnPacks"));
    }
    protected override void UpdateReddotUI()
    {
        base.UpdateReddotUI();
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.NorAdmiralFree);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.GenAdmiralFree);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.ShopGiftBuy);
    }
    private void ReddotCallback_Show_Btn_Recruit(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnRecruit = f_GetObject("BtnRecruit");
        UITool.f_UpdateReddot(BtnRecruit, iNum, new Vector3(149, 44, 0), 102);
    }
    private void ReddotCallback_Show_Btn_Gift(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnRecruit = f_GetObject("BtnPacks");
        UITool.f_UpdateReddot(BtnRecruit, iNum, new Vector3(149, 44, 0), 102);
    }

    private void ReddotCallback_Show_Btn_RecruitGen(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnRecruit = f_GetObject("BtnRecruitGen");
        UITool.f_UpdateReddot(BtnRecruit, iNum, new Vector3(149, 44, 0), 102);
    }

    private void ReddotCallback_Show_Btn_RecruitCamp(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnRecruitCamp = f_GetObject("BtnRecruitCamp");
        UITool.f_UpdateReddot(BtnRecruitCamp, iNum, new Vector3(149, 44, 0), 102);
    }
    #endregion
}
