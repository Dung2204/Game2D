using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPropsPage : UIFramwork
{
    private UIWrapComponent _propWrapComponet;
    private int singlePrice;
    private ShopResourceDT OpenSelectPropDt = null;
    private int propVipLmitTimes = 0;
    private int countLeft = 0;
    private List<BasePoolDT<long>> _propList;
    private SocketCallbackDT RequestShopPropListCallback = new SocketCallbackDT();//请求商店道具列表回调
    private SocketCallbackDT BuyResourceCallback = new SocketCallbackDT();//购买道具结果回调
    private List<AwardPoolDT> buyPropList = new List<AwardPoolDT>();//购买完商店资源显示奖励
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        _propList = Data_Pool.m_ShopResourcePool.f_GetAll();
        CheckShopResourceFormat(_propList);
        if (_propWrapComponet == null)
        {
            _propWrapComponet = new UIWrapComponent(240, 2, 740, 5, f_GetObject("PropItemParent"), f_GetObject("PropItem"), _propList, PropItemUpdateByInfo, null);
        }
        _propWrapComponet.f_ResetView();
        _propWrapComponet.f_UpdateView();
        Data_Pool.m_ShopResourcePool.f_GetNewShop(RequestShopPropListCallback);//请求商店列表
        
    }
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.MainBg);
    }
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnReturn", OnBtnReturnClick);
        BuyResourceCallback.m_ccCallbackSuc = OnBuyPropSuccessCallback;
        BuyResourceCallback.m_ccCallbackFail = OnBuyPropFailCallback;
        RequestShopPropListCallback.m_ccCallbackSuc = OnShowPropListSuccessCallback;
        RequestShopPropListCallback.m_ccCallbackFail = OnShowPropListFailCallback;

        // điều chỉnh child scale
        //Vector3 posItemRoot = f_GetObject("ObjCardContent").transform.localPosition;
        //posItemRoot.x = 360 / 2 * ScreenControl.Instance.mFunctionRatio;//360为左侧菜单宽度
        //f_GetObject("ObjCardContent").transform.localPosition = posItemRoot;
    }
    /// <summary>
    /// 请求商店资源列表成功
    /// </summary>
    /// <param name="obj"></param>
    private void OnShowPropListSuccessCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if (OpenSelectPropDt != null)
        {
            List<BasePoolDT<long>> listData = Data_Pool.m_ShopResourcePool.f_GetAll();
            for (int i = 0; i < listData.Count; i++)
            {
                ShopResourcePoolDT shopResourcePoolDT = listData[i] as ShopResourcePoolDT;
                if (shopResourcePoolDT.m_ShopResourceDT.iId == OpenSelectPropDt.iId)
                {
                    OnShopPropItemClick(null, shopResourcePoolDT, null);
                    OpenSelectPropDt = null;
                    break;
                }
            }
        }
    }
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
    private void OnBuyPropSuccessCallback(object obj)
    {
        //更新道具Item UI,元宝
        _propList = CommonTools.f_CopyPoolDTArrayToNewList(Data_Pool.m_ShopResourcePool.f_GetAll());
        CheckShopResourceFormat(_propList);
        _propWrapComponet.f_UpdateView();

        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { buyPropList });
        UITool.f_OpenOrCloseWaitTip(false);
    }
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
    private void PropItemUpdateByInfo(Transform item, BasePoolDT<long> dt)
    {
        ShopResourcePoolDT propPoolData = dt as ShopResourcePoolDT;
        ShopResourceDT shopResourceDt = propPoolData.m_ShopResourceDT;
        //玩家vip等级
        int playerVipLevel = UITool.f_GetNowVipLv();
        ResourceCommonDT resourceCommonDT = new ResourceCommonDT();
        resourceCommonDT.f_UpdateInfo((byte)shopResourceDt.iType, shopResourceDt.iTempId, 0);
        string Name = resourceCommonDT.mName;
        string BorderName = UITool.f_GetImporentColorName(resourceCommonDT.mImportant, ref Name);
        string[] discountArray = shopResourceDt.szDIscount.Split(';');
        string discount = propPoolData.m_iBuyTimes > (discountArray.Length - 1) ? discountArray[discountArray.Length - 1] : discountArray[propPoolData.m_iBuyTimes];
        item.GetComponent<ShopPropItem>().SetData((EM_ResourceType)shopResourceDt.iType, shopResourceDt.iTempId, BorderName,
            Name, shopResourceDt.iNum, int.Parse(discount), GetTimesByVipLevel(shopResourceDt, playerVipLevel), GetPriceByBuyTimes(shopResourceDt, propPoolData.m_iBuyTimes),
            propPoolData.m_iBuyTimes);
        f_RegClickEvent(item.GetComponent<ShopPropItem>().BtnBuy, OnShopPropItemClick, dt);
        f_RegClickEvent(item.GetComponent<ShopPropItem>().Icon.gameObject, OnPropIconClick, dt);
    }
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
    /// 点击item里面购买道具按钮   ------(网络接口)
    /// </summary>
    private void OnShopPropItemClick(GameObject obj, object obj1, object obj2)
    {
        ShopResourcePoolDT data = obj1 as ShopResourcePoolDT;
        ShopResourceDT shopResourceDt = data.m_ShopResourceDT;
        singlePrice = GetPriceByBuyTimes(shopResourceDt, data.m_iBuyTimes);
        int userVipLevel = UITool.f_GetNowVipLv();
        propVipLmitTimes = GetTimesByVipLevel(shopResourceDt, userVipLevel);
        countLeft = propVipLmitTimes == 0 ? 0 : (propVipLmitTimes - data.m_iBuyTimes);
        if (countLeft < 1 && propVipLmitTimes != 0)
        {
            countLeft = -1;//不可再买
        }
        BuyParam param = new BuyParam();
        param.iId = data.m_ShopResourceDT.iId;
        param.title = CommonTools.f_GetTransLanguage(1097);
        param.buyHint = propVipLmitTimes > 0 ? string.Format(CommonTools.f_GetTransLanguage(1098), countLeft <= 0 ? 0 : countLeft) : "";
        param.canBuyTimes = countLeft;
        param.resourceType = (EM_ResourceType)data.m_ShopResourceDT.iType;
        param.resourceID = data.m_ShopResourceDT.iTempId;
        param.resourceCount = data.m_ShopResourceDT.iNum;
        param.moneyType = EM_MoneyType.eUserAttr_Sycee;
        string[] priceArray = data.m_ShopResourceDT.szNewNum.Split(';');
        List<int> priceIntArray = new List<int>();
        for (int i = 0; i < priceArray.Length; i++)
        {
            if (i >= data.m_iBuyTimes)
                priceIntArray.Add(int.Parse(priceArray[i]));
        }
        if (priceIntArray.Count == 0)
        {
            priceIntArray.Add(int.Parse(priceArray[priceArray.Length - 1]));
        }
        param.price = priceIntArray;
        param.onConfirmBuyCallback = OnConfirmBuyCallback;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.BuyPage, UIMessageDef.UI_OPEN, param);
    }
    private void OnConfirmBuyCallback(int iId, EM_ResourceType type, int resourceId, int resourceCount, int buyCount)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);


        int userSycee = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee);
        if (userSycee < singlePrice * buyCount)
        {
            string tipContent = string.Format(CommonTools.f_GetTransLanguage(1108));
            ccUIManage.GetInstance().f_SendMsg(UINameConst.PayTipPage, UIMessageDef.UI_OPEN, tipContent);
            return;
        }
        if (countLeft < 1 && propVipLmitTimes != 0)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1109));
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_ShopResourcePool.f_Buy(iId, buyCount, BuyResourceCallback);
        buyPropList.Clear();
        AwardPoolDT item1 = new AwardPoolDT();
        item1.f_UpdateByInfo((byte)type, resourceId, resourceCount * buyCount);
        buyPropList.Add(item1);
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

    private void OnBtnReturnClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopPropsPage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
    }
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
}
