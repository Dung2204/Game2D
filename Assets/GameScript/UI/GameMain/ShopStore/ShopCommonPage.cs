using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 商店通用页面（只包含6个商品的，可随机的，不可拖动类型的商店）
/// 神将和领悟商店
/// </summary>
public class ShopCommonPage : UIFramwork
{
    private SocketCallbackDT OnRequestShopRandInfoCallback = new SocketCallbackDT();//请求商店信息回调
    private SocketCallbackDT OnBuyShopRandCallback = new SocketCallbackDT();//购买信息回调
    private SocketCallbackDT OnRefreshShopCallback = new SocketCallbackDT();//刷新回调
    private EM_ShopType em_currentShopType = EM_ShopType.Card;//当前商店类型
    private ShopRandDT currentShopRandConfig;
    private int currentBuyIndex;

    private readonly int showModeId = 11071;
    private Transform roleParent;
    private GameObject role;
    /// <summary>
    /// 页面打开
    /// </summary>
    /// <param name="e"></param>
    protected override void UI_OPEN(object e)
    {
        // glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(UITool.GetEnumName(typeof(AudioEffectType), AudioEffectType.OpenMarket));
        base.UI_OPEN(e);
        em_currentShopType = (EM_ShopType)e;
        InitOrUpdateShopUIData();
        InitUI();
        InvokeRepeating("UpdateTimesLeft", 1f, 1f);
        f_LoadTexture();
    }
    private string strTexBgRoot = "UI/TextureRemove/MainMenu/CommonBg";
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        //加载背景图
        UITexture TexBg = f_GetObject("TexBg").GetComponent<UITexture>();
        if (TexBg.mainTexture == null)
        {
            Texture2D tTexBg = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot);
            TexBg.mainTexture = tTexBg;
        }
		switch (em_currentShopType)
        {
            case EM_ShopType.Card:
				f_GetObject("ShopTitle").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(1543);
				break;
            case EM_ShopType.Awake:
				f_GetObject("ShopTitle").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(1544);
				break;
            default:
                break;
        }
    }

    private void ui_OpenMoneyUI() {
        switch (em_currentShopType)
        {
            case EM_ShopType.Card:


                break;
            case EM_ShopType.Awake:
                break;
            default:
                break;
        }
    }
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        CancelInvoke("UpdateTimesLeft");
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
    }
    /// <summary>
    /// 更新倒计时
    /// 60为临时的数据（以后待改）
    /// </summary>
    private void UpdateTimesLeft()
    {
        ShopStorePoolDT shopStorePoolDT = Data_Pool.m_ShopStorePool.f_GetForId((int)em_currentShopType) as ShopStorePoolDT;
        if (shopStorePoolDT != null)
        {
            int secondLeft = currentShopRandConfig.iFreeCD - (GameSocket.GetInstance().f_GetServerTime() - shopStorePoolDT.lastTime);
            if (shopStorePoolDT.freeTimes < currentShopRandConfig.iFreeTimes)
            {
                secondLeft = secondLeft < 0 ? 0 : secondLeft;
                if (secondLeft <= 0)
                {
                    secondLeft = 0;
                    Data_Pool.m_ShopStorePool.f_GetShopRandInfo(em_currentShopType, OnRequestShopRandInfoCallback);
                    UITool.f_OpenOrCloseWaitTip(true);
                }
                else
                    f_GetObject("LabelTimesLeft").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(410)
                            + shopStorePoolDT.freeTimes + "/" + currentShopRandConfig.iFreeTimes + " [2FE636](" + CommonTools.f_GetStringBySecond(secondLeft) + ")";
            }
            else
                f_GetObject("LabelTimesLeft").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(410)
                            + shopStorePoolDT.freeTimes + "/" + currentShopRandConfig.iFreeTimes + CommonTools.f_GetTransLanguage(411);
        }
    }
    /// <summary>
    /// 更新UI
    /// </summary>
    private void InitUI()
    {
        List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
        listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
        listMoneyType.Add(EM_MoneyType.eFreshToken);

        switch (em_currentShopType)
        {
            case EM_ShopType.Card:
                listMoneyType.Add(EM_MoneyType.eUserAttr_GeneralSoul);
                break;
            case EM_ShopType.Awake:
                listMoneyType.Add(EM_MoneyType.eUserAttr_GodSoul);
                break;
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, listMoneyType);
    }
    /// <summary>
    /// 初始化事件
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnReturn", OnBtnReturnClick);
        f_RegClickEvent("BtnFresh", OnBtnReFreshClick);

        OnRequestShopRandInfoCallback.m_ccCallbackSuc = OnRequestSuc;
        OnRequestShopRandInfoCallback.m_ccCallbackFail = OnRequestFail;
        OnBuyShopRandCallback.m_ccCallbackSuc = OnBuyShopRandSuc;
        OnBuyShopRandCallback.m_ccCallbackFail = OnBuyShopRandFail;
        OnRefreshShopCallback.m_ccCallbackSuc = OnRefreshShopSuc;
        OnRefreshShopCallback.m_ccCallbackFail = OnRefreshShopFail;

        roleParent = f_GetObject("RoleParent").transform;
        UITool.f_CreateRoleByModeId(showModeId, ref role, roleParent, 17);
    }
    #region 服务器回调
    /// <summary>
    /// 请求商店信息成功回调
    /// </summary>
    /// <param name="msg"></param>
    private void OnRequestSuc(object msg)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)msg;
        InitOrUpdateShopUIData();
        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void OnRequestFail(object msg)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)msg;
        UITool.f_OpenOrCloseWaitTip(false);
    }
    /// <summary>
    /// 购买商店物品成功回调
    /// </summary>
    /// <param name="msg"></param>
    private void OnBuyShopRandSuc(object msg)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)msg;
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "购买商店物品成功");
        InitOrUpdateShopUIData();
        InitUI();

        ShopStorePoolDT shopStorePoolDT = Data_Pool.m_ShopStorePool.f_GetForId((int)em_currentShopType) as ShopStorePoolDT;
        ShopRandGoodsDT shopRandGoodDT = shopStorePoolDT.m_shopRandItemDTArray[currentBuyIndex] as ShopRandGoodsDT;
        List<AwardPoolDT> awardList = new List<AwardPoolDT>();
        AwardPoolDT item1 = new AwardPoolDT();
        item1.f_UpdateByInfo((byte)shopRandGoodDT.iGoodsType, shopRandGoodDT.iGoodsId, shopRandGoodDT.iGoodsNum);
        awardList.Add(item1);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { awardList });
        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void OnBuyShopRandFail(object msg)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)msg;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(412) + CommonTools.f_GetTransLanguage((int)msg));
        UITool.f_OpenOrCloseWaitTip(false);
    }
    /// <summary>
    /// 刷新商店成功
    /// </summary>
    /// <param name="msg"></param>
    private void OnRefreshShopSuc(object msg)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)msg;
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "刷新商店");
        InitOrUpdateShopUIData();
        InitUI();
        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void OnRefreshShopFail(object msg)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)msg;
        if (teMsgOperateResult == eMsgOperateResult.eOR_TimesLimit)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(413));
        }
        else
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(414) + CommonTools.f_GetTransLanguage((int)msg));
        }
        UITool.f_OpenOrCloseWaitTip(false);
    }
    #endregion
    private void ShowViewContent(bool isShow)
    {
        for (int i = 0; i < 6; i++)
        {
            GameObject ItemObj = f_GetObject("Item" + i);
            ItemObj.SetActive(isShow);
        }
    }
    /// <summary>
    /// 收到商店消息后，马上初始化数据
    /// </summary>
    private void InitOrUpdateShopUIData()
    {
        ShopStorePoolDT shopStorePoolDT = Data_Pool.m_ShopStorePool.f_GetForId((int)em_currentShopType) as ShopStorePoolDT;
        ShowViewContent(shopStorePoolDT == null ? false : true);
        currentShopRandConfig = GetShopRandDTByShopType(em_currentShopType);

        ResourceCommonDT dt = new ResourceCommonDT();
        dt.f_UpdateInfo((byte)EM_ResourceType.Money, currentShopRandConfig.iCoin, currentShopRandConfig.iCoinNum);
        f_GetObject("LabelHint").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(415) + dt.mResourceNum + dt.mName + CommonTools.f_GetTransLanguage(416);

        if (shopStorePoolDT == null)
        {
            Debug.LogWarning(CommonTools.f_GetTransLanguage(417));//如果没有数据则进入刷新
            Data_Pool.m_ShopStorePool.f_GetShopRandInfo(em_currentShopType, OnRequestShopRandInfoCallback);
            UITool.f_OpenOrCloseWaitTip(true);
            return;
        }
        int secondLeft = currentShopRandConfig.iFreeCD - (GameSocket.GetInstance().f_GetServerTime() - shopStorePoolDT.lastTime);
        if (shopStorePoolDT.freeTimes < currentShopRandConfig.iFreeTimes)
        {
            f_GetObject("LabelTimesLeft").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(410)
                        + shopStorePoolDT.freeTimes + "/" + currentShopRandConfig.iFreeTimes + " [2FE636](" + CommonTools.f_GetStringBySecond(secondLeft) + ")";
        }
        else
            f_GetObject("LabelTimesLeft").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(410)
                        + shopStorePoolDT.freeTimes + "/" + currentShopRandConfig.iFreeTimes + CommonTools.f_GetTransLanguage(411);
        for (int i = 0; i < 6; i++)
        {
            ShopRandGoodsDT shopRandGoodsDt = shopStorePoolDT.m_shopRandItemDTArray[i] as ShopRandGoodsDT;
            //Debug.Log("物品ID:" + shopRandGoodsDt.iId);
            GameObject ItemObj = f_GetObject("Item" + i);
            ShopStoreItemCtl shopStoreItemCtl = ItemObj.GetComponent<ShopStoreItemCtl>();
            ResourceCommonDT resourceCommonDT = new ResourceCommonDT();
            resourceCommonDT.f_UpdateInfo((byte)shopRandGoodsDt.iGoodsType, shopRandGoodsDt.iGoodsId, 0);
            string Name = UITool.f_GetGoodName((EM_ResourceType)shopRandGoodsDt.iGoodsType, shopRandGoodsDt.iGoodsId);
            string BorderName = UITool.f_GetImporentColorName(resourceCommonDT.mImportant, ref Name);
            string hasCountHint = "";
            if (shopRandGoodsDt.iGoodsType == (int)EM_ResourceType.CardFragment)
            {
                int count = UITool.f_GetGoodNum(EM_ResourceType.CardFragment, shopRandGoodsDt.iGoodsId);
                CardFragmentDT cardFragmentDt = glo_Main.GetInstance().m_SC_Pool.m_CardFragmentSC.f_GetSC(shopRandGoodsDt.iGoodsId) as CardFragmentDT;
                hasCountHint = "（"+ CommonTools.f_GetTransLanguage(418) + count + "/" + cardFragmentDt.iNeedNum + "）";
            }
            shopStoreItemCtl.SetData(Name, (EM_ResourceType)shopRandGoodsDt.iGoodsType, shopRandGoodsDt.iGoodsId, BorderName,
                shopRandGoodsDt.iGoodsNum, (EM_MoneyType)shopRandGoodsDt.iCostId, shopRandGoodsDt.iCostNum, CheckHasBuy(i, shopStorePoolDT.buyMask), hasCountHint);
            f_RegClickEvent(shopStoreItemCtl.m_BtnBuy.gameObject, OnItemBuyClick, shopRandGoodsDt, i);
            f_RegClickEvent(shopStoreItemCtl.m_SprIcon.gameObject, OnItemIconClick, shopRandGoodsDt);
        }
        f_GetObject("LabelPropFreshTimesLeft").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(419)
            + shopStorePoolDT.propFreshTimes + "/" + GetFreshLimitTimesByVip(em_currentShopType, UITool.f_GetNowVipLv());
        if (shopStorePoolDT.freeTimes <= 0)
        {
            //f_GetObject("BtnFresh").transform.Find("Title").gameObject.SetActive(false);
            f_GetObject("BtnFresh").transform.Find("SprPriceType").gameObject.SetActive(true);
            if (GetGenPropCount(currentShopRandConfig.iItem) >= currentShopRandConfig.iItemNum)//道具货币
            {
                f_GetObject("BtnFresh").transform.Find("SprPriceType/MoneyPriceType").GetComponent<UISprite>().spriteName = UITool.f_GetMoneySpriteName(EM_MoneyType.eFreshToken);
                f_GetObject("BtnFresh").transform.Find("SprPriceType/LabelPrice").GetComponent<UILabel>().text = currentShopRandConfig.iItemNum.ToString();
            }
            else if (Data_Pool.m_UserData.f_GetProperty(currentShopRandConfig.iCoin) >= currentShopRandConfig.iCoinNum)//普通货币
            {
                f_GetObject("BtnFresh").transform.Find("SprPriceType/MoneyPriceType").GetComponent<UISprite>().spriteName = UITool.f_GetMoneySpriteName((EM_MoneyType)currentShopRandConfig.iCoin);
                f_GetObject("BtnFresh").transform.Find("SprPriceType/LabelPrice").GetComponent<UILabel>().text = currentShopRandConfig.iCoinNum.ToString();
            }
            else//如果道具和货币都不足，则显示道具UI
            {
                f_GetObject("BtnFresh").transform.Find("SprPriceType/MoneyPriceType").GetComponent<UISprite>().spriteName = UITool.f_GetMoneySpriteName(EM_MoneyType.eFreshToken);
                f_GetObject("BtnFresh").transform.Find("SprPriceType/LabelPrice").GetComponent<UILabel>().text = currentShopRandConfig.iItemNum.ToString();
            }
        }
        else
        {
            //f_GetObject("BtnFresh").transform.Find("Title").gameObject.SetActive(true);
            f_GetObject("BtnFresh").transform.Find("SprPriceType").gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 获取刷新上限
    /// </summary>
    /// <returns></returns>
    private int GetFreshLimitTimesByVip(EM_ShopType shopType, int vipLevel)
    {
        int id = 15;
        switch (shopType)
        {
            case EM_ShopType.Card: id = 16; break;
            case EM_ShopType.Awake: id = 13; break;
        }
        VipPrivilegeDT vipPrivilegeDT = (VipPrivilegeDT)glo_Main.GetInstance().m_SC_Pool.m_VipPrivilegeSC.f_GetSC(id);
        switch (vipLevel)
        {
            case 0: return vipPrivilegeDT.iLv0;
            case 1: return vipPrivilegeDT.iLv1;
            case 2: return vipPrivilegeDT.iLv2;
            case 3: return vipPrivilegeDT.iLv3;
            case 4: return vipPrivilegeDT.iLv4;
            case 5: return vipPrivilegeDT.iLv5;
            case 6: return vipPrivilegeDT.iLv6;
            case 7: return vipPrivilegeDT.iLv7;
            case 8: return vipPrivilegeDT.iLv8;
            case 9: return vipPrivilegeDT.iLv9;
            case 10: return vipPrivilegeDT.iLv10;
            case 11: return vipPrivilegeDT.iLv11;
            case 12: return vipPrivilegeDT.iLv12;
            case 13: return vipPrivilegeDT.iLv13;
            case 14: return vipPrivilegeDT.iLv14;
            case 15: return vipPrivilegeDT.iLv15;
            default: return vipPrivilegeDT.iLv15;
        }
    }
    private int GetGenPropCount(int id)
    {
        int count = 0;
        List<BasePoolDT<long>> allUserGood = Data_Pool.m_BaseGoodsPool.f_GetAll();
        for (int i = 0; i < allUserGood.Count; i++)
        {
            BaseGoodsPoolDT data = allUserGood[i] as BaseGoodsPoolDT;
            if (data.m_BaseGoodsDT.iId == id)
            {
                count = data.m_iNum;
                break;
            }
        }
        return count;
    }
    /// <summary>
    /// 检查该物品是否购买
    /// </summary>
    /// <param name="gooIndex"></param>
    /// <param name="intMask"></param>
    private bool CheckHasBuy(int goodIndex, int intMask)
    {
        char[] maskArray = intMask.ToString().ToCharArray();
        List<int> listMask = new List<int>();
        for (int i = 0; i < 6 - maskArray.Length; i++)
        {
            listMask.Add(0);
        }
        for (int j = 0; j < maskArray.Length; j++)
        {
            if (maskArray[j] == '1')
                listMask.Add(1);
            else
                listMask.Add(0);
        }
        for (int i = 0; i < listMask.Count; i++)
        {
            if (i == goodIndex)
            {
                if (listMask[i] == 1)
                    return true;
                else
                    return false;
            }
        }
        return false;
    }
    /// <summary>
    /// 点击物品图标，打开详细信息界面
    /// </summary>
    private void OnItemIconClick(GameObject go, object obj1, object obj2)
    {
        ShopRandGoodsDT data = (obj1 as ShopRandGoodsDT);
        ResourceCommonDT commonData = new ResourceCommonDT();
        commonData.f_UpdateInfo((byte)data.iGoodsType, data.iGoodsId, data.iGoodsNum);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, commonData);
    }
    /// <summary>
    /// 点击购买
    /// </summary>
    private void OnItemBuyClick(GameObject go, object obj1, object obj2)
    {
        ShopRandGoodsDT data = (obj1 as ShopRandGoodsDT);
        if (data.iGoodsType == (int)EM_ResourceType.Card) {
            if (em_currentShopType == EM_ShopType.Card)
            {
                if (Data_Pool.m_CardPool.f_GetAll().Count > Data_Pool.m_RechargePool.f_GetCurLvVipPriValue(EM_VipPrivilege.eVip_GeneralBag))
                {
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(420));
                    return;
                }
            }
        }
        int count = Data_Pool.m_UserData.f_GetProperty(data.iCostId);
        if (count < data.iCostNum)
        {
            string moneyName = UITool.f_GetGoodName(EM_ResourceType.Money, data.iCostId);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, string.Format(CommonTools.f_GetTransLanguage(421), moneyName));
            // GetWayPageParam tGetWayParm = new GetWayPageParam(EM_ResourceType.Money, data.iCostId, this);
            // ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
            return;
        }
        currentBuyIndex = (int)obj2;
        Data_Pool.m_ShopStorePool.f_ShopRandBuy(em_currentShopType, (int)obj2, OnBuyShopRandCallback);
        UITool.f_OpenOrCloseWaitTip(true);
    }
    /// <summary>
    /// 获取该商店类型的数据
    /// </summary>
    /// <param name="shopType">商店类型</param>
    /// <returns></returns>
    private ShopRandDT GetShopRandDTByShopType(EM_ShopType shopType)
    {
        List<NBaseSCDT> listAllShopRandItem = glo_Main.GetInstance().m_SC_Pool.m_ShopRandSC.f_GetAll();
        for (int i = 0; i < listAllShopRandItem.Count; i++)
        {
            ShopRandDT item = listAllShopRandItem[i] as ShopRandDT;
            if (item.iType == (int)shopType)
            {
                return item;
            }
        }
        Debug.LogError(CommonTools.f_GetTransLanguage(422));
        return null;
    }
    #region 按钮事件
    /// <summary>
    /// 点击返回按钮
    /// </summary>
    private void OnBtnReturnClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopCommonPage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
    }
    /// <summary>
    /// 点击刷新
    /// </summary>
    private void OnBtnReFreshClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        ShopStorePoolDT shopStorePoolDT = Data_Pool.m_ShopStorePool.f_GetForId((int)em_currentShopType) as ShopStorePoolDT;
        if ((Data_Pool.m_ShopStorePool.f_GetForId((int)em_currentShopType) as ShopStorePoolDT).freeTimes <= 0)
        {
            //if (shopStorePoolDT.propFreshTimes >= GetFreshLimitTimesByVip(em_currentShopType, UITool.f_GetVipLv(Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Vip))))
            //{
            //    ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "可用道具刷新次数已达上限！");
            //    return;
            //}
            if (GetGenPropCount(currentShopRandConfig.iItem) < currentShopRandConfig.iItemNum
                && Data_Pool.m_UserData.f_GetProperty(currentShopRandConfig.iCoin) < currentShopRandConfig.iCoinNum)
            {
                UITool.f_IsEnoughMoney(EM_MoneyType.eFreshToken, 0, true, true, this);
                return;
            }
        }
        Data_Pool.m_ShopStorePool.f_ShopRandRefresh(em_currentShopType, OnRefreshShopCallback);
        UITool.f_OpenOrCloseWaitTip(true);
    }
    #endregion
}
