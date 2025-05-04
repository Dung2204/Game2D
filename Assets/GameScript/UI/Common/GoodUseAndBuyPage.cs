using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class GoodUseAndBuyPage : UIFramwork
{
    private UI2DSprite mIcon;
    private UISprite mFrame;
    private UILabel mGoodDesc;
    private UILabel mGoodName;

    private UILabel mPriceLabel;
    private UILabel mHaveNumLabel;

    private UISprite mUseBtn;
    private UISprite mBuyBtn;

    
    private int goodId;
    private BaseGoodsDT goodTemplate;

    private GoodUseAndBuyPageParam m_Param;
    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
        mIcon = f_GetObject("Icon").GetComponent<UI2DSprite>();
        mFrame = f_GetObject("Frame").GetComponent<UISprite>();
        mGoodDesc = f_GetObject("GoodDesc").GetComponent<UILabel>();
        mGoodName = f_GetObject("GoodName").GetComponent<UILabel>();

        mPriceLabel = f_GetObject("PriceLabel").GetComponent<UILabel>();
        mHaveNumLabel = f_GetObject("HaveNumLabel").GetComponent<UILabel>();

        mUseBtn = f_GetObject("UseBtn").GetComponent<UISprite>();
        mBuyBtn = f_GetObject("BuyBtn").GetComponent<UISprite>();

        f_RegClickEvent(mUseBtn.gameObject, f_UseBtn);
        f_RegClickEvent(mBuyBtn.gameObject, f_BuyBtn);
        f_RegClickEvent("CloseBtn", f_CloseBtn);
        f_RegClickEvent("CloseMask", f_CloseBtn);
    }

    protected override void InitGUI()
    {
        base.InitGUI();
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e); 
        if (e == null || !(e is GoodUseAndBuyPageParam))
MessageBox.ASSERT("Wrong parameter");
        m_Param = (GoodUseAndBuyPageParam)e;
        goodId = m_Param.m_iGoodId;
        goodTemplate = (BaseGoodsDT)glo_Main.GetInstance().m_SC_Pool.m_BaseGoodsSC.f_GetSC(goodId);
        if(goodTemplate == null)
MessageBox.ASSERT("The parameter used does not exist, Item Id:" + goodId);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_RequestShopPropListCallback;
        socketCallbackDt.m_ccCallbackFail = f_RequestShopPropListCallback;
        Data_Pool.m_ShopResourcePool.f_GetNewShop(socketCallbackDt);//请求商店列表
        f_UpdateByInfo();
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        if (m_Param != null && m_Param.m_CallbackHandle != null)
        {
            m_Param.m_CallbackHandle();
        }
    }

    private void f_RequestShopPropListCallback(object result)
    {
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            f_UpdateByInfo();
        }
        else
        {
UITool.Ui_Trip("Data Error");
MessageBox.ASSERT("Data loading failed");
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GoodUseAndBuyPage, UIMessageDef.UI_CLOSE);
        }
    }

    private void f_UpdateByInfo()
    {
        mGoodDesc.text = goodTemplate.szReadme;
        string goodName = goodTemplate.szName;
        string spriteBorderName = UITool.f_GetImporentColorName(goodTemplate.iImportant, ref goodName);
        mGoodName.text = goodName;
        mFrame.spriteName = spriteBorderName;
        UITool.f_SetIconSprite(mIcon, EM_ResourceType.Good, goodId);

        int haveNum = Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(goodId);
        UITool.f_SetSpriteGray(mUseBtn, haveNum <= 0);
        UILabel useBtnLabel = mUseBtn.GetComponentInChildren<UILabel>();
        useBtnLabel.effectColor = haveNum <= 0? new Color(0f,0f,0f) : new Color(145f/255,23f/255f,8f/255f);
        useBtnLabel.color = haveNum <= 0 ? new Color(196f / 255f, 189f / 255f, 170f / 255f) : new Color(255f / 255F, 249f / 255f, 212f / 255f);
mHaveNumLabel.text = string.Format("Có：{0}", haveNum);

        ShopResourcePoolDT shopPoolDt = (ShopResourcePoolDT)Data_Pool.m_ShopResourcePool.f_GetShopPoolDtByTypeAndId((int)EM_ResourceType.Good, goodId);
        int[] timesLimitArr = ccMath.f_String2ArrayInt(shopPoolDt.m_ShopResourceDT.szVipLimitTimes, ";");
        int[] priceArr = ccMath.f_String2ArrayInt(shopPoolDt.m_ShopResourceDT.szNewNum, ";");
        int vipLv = UITool.f_GetNowVipLv();
        int buyTimes = shopPoolDt.m_iBuyTimes;
        int buyTimesLimit = vipLv < timesLimitArr.Length ? timesLimitArr[vipLv] : timesLimitArr[timesLimitArr.Length - 1];
        int price = buyTimes < priceArr.Length ? priceArr[buyTimes] : priceArr[priceArr.Length - 1];
        int haveSycee = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee);
        UITool.f_SetSpriteGray(mBuyBtn, buyTimes >= buyTimesLimit);
        UILabel buyBtnLabel = mBuyBtn.GetComponentInChildren<UILabel>();
        buyBtnLabel.effectColor = buyTimes >= buyTimesLimit ? new Color(0f, 0f, 0f) : new Color(145f / 255, 23f / 255f, 8f / 255f);
        buyBtnLabel.color = buyTimes >= buyTimesLimit ? new Color(196f / 255f, 189f / 255f, 170f / 255f) : new Color(255f / 255F, 249f / 255f, 212f / 255f);
        mPriceLabel.text = haveSycee < price ? string.Format("[ff0000]{0}", price) : price.ToString();
    }


    private void f_CloseBtn(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg( UINameConst.GoodUseAndBuyPage, UIMessageDef.UI_CLOSE);
    }
    
    private void f_UseBtn(GameObject go, object value1, object value2)
    {
        int haveNum = Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(goodId);
        if (haveNum <= 0)
        {
UITool.Ui_Trip(string.Format("{0} đã dùng", goodTemplate.szName));
            return;
        }
        else if (goodId == GameParamConst.EnergyGoodId)
        {
            int haveEnery = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Energy);
            int mEnergyLimit = UITool.f_GetNowVipPrivilege((int)EM_VipPrivilege.eVip_EnergyLimit);
            if (haveEnery + goodTemplate.iEffectData > mEnergyLimit)
            {
UITool.Ui_Trip("Đã đạt tối đa");
                return;
            }
        }
        UITool.f_OpenOrCloseWaitTip(true);
        BaseGoodsPoolDT dt = (BaseGoodsPoolDT)Data_Pool.m_BaseGoodsPool.f_GetForData5(goodId);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_UseGood;
        socketCallbackDt.m_ccCallbackFail = f_Callback_UseGood;
        Data_Pool.m_BaseGoodsPool.f_Use(dt.iId, 1, 0,socketCallbackDt);
    }

    private void f_BuyBtn(GameObject go, object value1, object value2)
    {
        ShopResourcePoolDT shopPoolDt = (ShopResourcePoolDT)Data_Pool.m_ShopResourcePool.f_GetShopPoolDtByTypeAndId((int)EM_ResourceType.Good, goodId);
        int[] timesLimitArr = ccMath.f_String2ArrayInt(shopPoolDt.m_ShopResourceDT.szVipLimitTimes,";");
        int[] priceArr = ccMath.f_String2ArrayInt(shopPoolDt.m_ShopResourceDT.szNewNum,";");
        int vipLv = UITool.f_GetNowVipLv();
        int buyTimes = shopPoolDt.m_iBuyTimes;
        int buyTimesLimit = vipLv < timesLimitArr.Length ?timesLimitArr[vipLv]:timesLimitArr[timesLimitArr.Length - 1];
        int price = buyTimes < priceArr.Length ? priceArr[buyTimes] : priceArr[priceArr.Length - 1];
        int haveSycee = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee);
        if (buyTimes >= buyTimesLimit)
        {
UITool.Ui_Trip("Đã mua");
            return;
        }
        else if (haveSycee < price)
        {
UITool.Ui_Trip("Không đủ KNB");
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_BuyGood;
        socketCallbackDt.m_ccCallbackFail = f_Callback_BuyGood;
        Data_Pool.m_ShopResourcePool.f_Buy((int)shopPoolDt.iId, 1, socketCallbackDt);
    }

    private void f_Callback_UseGood(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        f_UpdateByInfo();
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
UITool.Ui_Trip("Dùng thành công");
        }
        else
        {
UITool.Ui_Trip("Dùng failed：code:" + result);
        }
    }

    private void f_Callback_BuyGood(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        f_UpdateByInfo();
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
UITool.Ui_Trip("Mua thành công");
        }
        else
        {
            // UITool.Ui_Trip("Mua thất bại：code:" + result);
UITool.Ui_Trip("Mua thất bại");
        }
    }
}

public class GoodUseAndBuyPageParam
{
    public GoodUseAndBuyPageParam(int goodId,EventDelegate.Callback callback)
    {
        m_iGoodId = goodId;
        m_CallbackHandle = callback;
    }

    public int m_iGoodId
    {
        private set;
        get;
    }

    public EventDelegate.Callback m_CallbackHandle
    {
        private set;
        get;
    }
}
