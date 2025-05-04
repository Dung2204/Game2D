using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;

public class ShowVip : UIFramwork
{
    private bool isCanPay = true;
    List<int> ignoreGood = new List<int>();
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_PAY_UPDATE_VIEW, f_OnPayUpdateView, this);
        Initialize();
        f_LoadTexture();
        f_Message();
        UpadteShowVip();
        Data_Pool.m_ShopGiftPool.f_GetNewShop(null);
        if (e is EM_PageIndex)//有默认选中页
        {
            EM_PageIndex pageIndex = (EM_PageIndex)e;
            if (pageIndex == EM_PageIndex.Recharge)
                UI_OpenPay(null, null, null);
            else if (pageIndex == EM_PageIndex.RechargeCoin)
            {
                UI_OpenPayCoin(null, null, null);
            }
            else
                UI_OpenPre(null, null, null);
        }
        else
            UI_OpenPre(null, null, null);
        InitMoneyUI();

        UITool.f_OpenOrCloseWaitTip(true);
        StartCoroutine(StartCheckPay());
		glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.ChargeBg);

    }

    /// <summary>
    /// 检测能不能充值
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartCheckPay()
    {
        WWWForm form = new WWWForm();
        //form.AddField("ChanelId", glo_Main.GetInstance().m_SDKCmponent.f_GetSdkChannelType());
        //form.AddField("Ver", Application.version);
        //form.AddField("ServerId", Data_Pool.m_UserData.m_iServerId);
        //WWW www = new WWW(GloData.glo_strCheckPay, form);
        WWW www = new WWW(GloData.glo_strCheckPay); //TsuCode - newCheckPay - get 
        yield return www;
        if (www.error != null)
        {
            yield return null;
        }
        if (www.isDone && www.error == null)
        {
            if (!string.IsNullOrEmpty(www.text))
            {
                isCanPay = www.text.ToLower().StartsWith("ok");
                if(isCanPay)
                {
                    ignoreGood.Clear();
                    var strSplit = www.text.ToLower().Split('#');
                    for(int i=0;i<strSplit.Length;i++)
                    {
                        int value;
                        if(int.TryParse(strSplit[i],out value))
                        {
                            ignoreGood.Add(value);
                        }
                    }
                    UpdateHidenPay();
                }
            }
            else
            {
                isCanPay = true;
            }
        }
        MessageBox.DEBUG(CommonTools.f_GetTransLanguage(1433) + isCanPay);
        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void UpdateHidenPay()
    {
        //List<NBaseSCDT> tmpPay = glo_Main.GetInstance().m_SC_Pool.m_PaySC.f_GetAll();
        for (int i = 0; i < ignoreGood.Count; i++)
        {
            if (ignoreGood[i]< MoneyParent.childCount && MoneyParent.GetChild(ignoreGood[i]) != null)
            {
                MoneyParent.GetChild(ignoreGood[i]).gameObject.SetActive(false);
            }
        }
        UpdatePay();
        UpadteShowVip(false);
    }
    private string strTexBgRoot = "UI/TextureRemove/MainMenu/CommonBg";
    private string strTexGiftBgRoot = "UI/TextureRemove/Vip/Tex_GiftBg";
    private string strTexturePaySpecialRoot = "UI/TextureRemove/Vip/Texture_PaySpecial";
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        //加载背景图
        UITexture TexBg = f_GetObject("TexBg").GetComponent<UITexture>();
        UITexture BodyGift = Body_Gift.GetComponent<UITexture>();
        UITexture Texture_PaySpecial = Pay_Money.transform.Find("Icon/Sprite").GetComponent<UITexture>();
        if (TexBg.mainTexture == null)
        {
            Texture2D tTexBg = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot);
            TexBg.mainTexture = tTexBg;

            Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexGiftBgRoot);
            BodyGift.mainTexture = tTexture2D;
            
            Texture2D tTexturePaySpecial = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexturePaySpecialRoot);
            Texture_PaySpecial.mainTexture = tTexturePaySpecial;
        }
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_PAY_UPDATE_VIEW, f_OnPayUpdateView, this);
    }

    /// <summary>
    /// 初始化金钱UI
    /// </summary>
    private void InitMoneyUI()
    {
        List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
        listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
        //listMoneyType.Add(EM_MoneyType.eUserAttr_Money);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Coin);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, listMoneyType);
    }
    /// <summary>
    /// 绑定事件
    /// </summary>
    protected void f_Message()
    {
        f_RegClickEvent("Back_Btn", UI_CloseThis);
        f_RegClickEvent("PayBtn", UI_OpenPay);
        f_RegClickEvent("PayBtn2", UI_OpenPay);
        f_RegClickEvent("PayBtnCoin", UI_OpenPayCoin);
        f_RegClickEvent("PreBtn", UI_OpenPre);
        f_RegClickEvent("BtnHelp", f_OnHelpBtnClick);
        //TsuCode - COin - KP
        //f_RegClickEvent("BtnAddCoin", UI_BtnAddCoin);
        //
    }
    #region UI
    //GameObject Pay_Btn;
    //GameObject Pro_Btn;

    int VipLv = 0;
    int NowVipLv = 0;


    Transform Head;
    Transform PreBody;
    UILabel Head_VipLv;
    private UILabel Head_VipLvNum;
    private UILabel Head_VipIconLvNum;
    UILabel Head_Pay;
    UILabel Head_LastVip;
    UILabel Head_LastVipNum;
    UISlider Head_VipExp;
    UILabel Head_VipExpLabel;
    UILabel Body_VipLv;
    UILabel Body_DescName;
    UILabel Body_Desc;
    GameObject Body_TypeDesc;
    Transform Body_DescGrid;
    Transform Body_Gift;
    UISprite Body_GiftVipLv;
    UILabel Body_GiftMoney;
    UILabel Body_GiftNoMoney;
    Transform Body_GiftGoodsGrid;
    GameObject Body_GiftBuyBtn;

    Transform Pay;
    Transform Pay_Money;
    Transform MoneyParent;

    Transform PayCoin;
    Transform Pay_Moneycoin;
    Transform MoneyParentCoin;

    UISlider Head_TempVipExp;
    UILabel Head_TempVipExpLabel;
    UILabel Head_LbNowVip;

    #endregion

    /// <summary>
    /// 初始化
    /// </summary>
    void Initialize()
    {
        Head = f_GetObject("Head").transform;
        Head_VipLv = Head.Find("Vip/name").GetComponent<UILabel>();
        Head_VipLvNum = Head.Find("Vip/Num").GetComponent<UILabel>();
        Head_VipIconLvNum = Head.Find("VipIcon/Num").GetComponent<UILabel>();
        Head_Pay = Head.Find("Rush/Pay").GetComponent<UILabel>();
        Head_LastVip = Head.Find("Vip/ToVipLvName").GetComponent<UILabel>();
        Head_LastVipNum = Head.Find("Vip/ToVipLv").GetComponent<UILabel>();
        Head_VipExp = Head.Find("VipExp").GetComponent<UISlider>();
        Head_VipExpLabel = Head.Find("VipExp/Num").GetComponent<UILabel>();
        PreBody = f_GetObject("PreBodyBg").transform;
        //Body_VipLv = PreBody.Find("VipIcon/Lv").GetComponent<UILabel>();
        Body_DescName = PreBody.Find("Desc/DescName").GetComponent<UILabel>();
        Body_Desc = PreBody.Find("Desc/Desc").GetComponent<UILabel>();
        Body_TypeDesc = PreBody.Find("TypeBg/Induce").gameObject;
        Body_DescGrid = PreBody.Find("TypeBg/DragScorllView/DescScrollView/Desc");
        Body_Gift = PreBody.Find("Gift");
        Body_GiftVipLv = PreBody.Find("Gift/Vip/Lv").GetComponent<UISprite>();
        Body_GiftMoney = PreBody.Find("Gift/BuyMoeny/Num").GetComponent<UILabel>();
        Body_GiftNoMoney = PreBody.Find("Gift/NoBuyMoeny/Num").GetComponent<UILabel>();
        Body_GiftGoodsGrid = PreBody.Find("Gift/ScrollView/Goods");
        Body_GiftBuyBtn = PreBody.Find("Gift/BuyBtn").gameObject;



        Pay = f_GetObject("Pay").transform;
        Pay_Money = Pay.Find("MoenyView/Moeny");
        MoneyParent = Pay.Find("MoenyView/Parent");

        PayCoin = f_GetObject("PayCoin").transform;
        Pay_Moneycoin = PayCoin.Find("MoenyView/Moeny");
        MoneyParentCoin = PayCoin.Find("MoenyView/Parent");

        Head_TempVipExp = Head.Find("TempVipExp").GetComponent<UISlider>();
        Head_TempVipExpLabel = Head.Find("TempVipExp/Num").GetComponent<UILabel>();
        Head_LbNowVip = Head.Find("LbNowVip").GetComponent<UILabel>();
    }
    void UpadteShowVip(bool isBody = true)
    {
        int vipExp = (int)Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Vip);
        int tempVipExp = (int)Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_TempVip);
        int tTempVipLv = UITool.f_GetVipLv(tempVipExp);
        int tTempNeedExp = 0;
        UITool.f_GetMaxNeedExp(vipExp,ref tTempNeedExp);
        //UITool.f_GetVipLvAndNeedExp(tempVipExp, ref tTempVipLv, ref tTempNeedExp);
        Head_TempVipExp.value = (float)Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_TempVip) / (float)tTempNeedExp;
        Head_TempVipExp.transform.Find("Foreground/yellow").localScale = Head_TempVipExp.transform.Find("Foreground/texture").localScale = new Vector3(Head_TempVipExp.value, 1, 0);
        Head_TempVipExpLabel.text = string.Format("{0}/{1}", Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_TempVip), tTempNeedExp);
        Head_LbNowVip.text = string.Format("Vip: {0}", tTempVipLv);

        
        int tVipLv = 0;
        int tNeedExp = 0;
        UITool.f_GetVipLvAndNeedExp(vipExp, ref tVipLv, ref tNeedExp);
        NowVipLv = tVipLv;
        VipLv = tVipLv;
        Head_VipLv.text = string.Format(CommonTools.f_GetTransLanguage(1434), "");
        Head_VipLvNum.text = "VIP"+ tVipLv.ToString();
        Head_VipIconLvNum.text = "VIP"+ tVipLv.ToString(); 
        Head_Pay.text = string.Format(CommonTools.f_GetTransLanguage(1435), (tNeedExp - vipExp));
        Head.Find("Rush/Diamond").localPosition = new Vector3(Head_Pay.transform.localPosition.x + Head_Pay.width + 50, 0, 0);
        Head_LastVip.text = string.Format(CommonTools.f_GetTransLanguage(1436), " ");
        Head_LastVipNum.text = "VIP" + (tVipLv + 1).ToString();
        Head.Find("Rush").gameObject.SetActive(tVipLv < 15);

        Head_VipExp.value = (float)Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Vip) / (float)tNeedExp;
        Head_VipExp.transform.Find("Foreground/yellow").localScale = Head_VipExp.transform.Find("Foreground/texture").localScale = new Vector3(Head_VipExp.value, 1, 0);
        Head_VipExpLabel.text = string.Format("{0}/{1}", Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Vip), tNeedExp);
        if (tVipLv >= 15)
        {
            Head_LastVip.text = string.Format(CommonTools.f_GetTransLanguage(1437));
            Head_VipExpLabel.text = string.Format(CommonTools.f_GetTransLanguage(1438));
            Head_VipExp.value = 1;
        }
        if (isBody)
            UpdateBody(tVipLv);

        //TsuCode - Coin- KimPhieu
        int coinCount = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Coin);
        string coinText = UITool.f_CountToChineseStr2(coinCount);
        f_GetObject("LabelCoinCount").GetComponent<UILabel>().text = coinText;
        //---------------------------
    }
    void UpdateBody(int lv)
    {

        if (lv + 1 <= glo_Main.GetInstance().m_SC_Pool.m_ShopGiftSC.f_GetAll().Count)
        {
            Body_Gift.gameObject.SetActive(true);
            ShopGiftDT tmpGift = null;
            tmpGift = (ShopGiftDT)glo_Main.GetInstance().m_SC_Pool.m_ShopGiftSC.f_GetSC(lv + 1);
            Body_GiftVipLv.spriteName = string.Format("Vip_ ({0})", lv);
			f_GetObject("VIPText").GetComponent<UILabel>().text = string.Format("VIP {0}", lv);
            //Body_GiftVipLv.MakePixelPerfect();
            Body_GiftMoney.text = tmpGift.iNewNum.ToString();
            Body_GiftNoMoney.text = tmpGift.iOldNum.ToString();
            f_RegClickEvent(Body_GiftBuyBtn, BuyGift, tmpGift);
            List<AwardPoolDT> tmpAward = Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(((BaseGoodsDT)glo_Main.GetInstance().m_SC_Pool.m_BaseGoodsSC.f_GetSC(tmpGift.iTempId)).iEffectData);
            GridUtil.f_SetGridView<AwardPoolDT>(Body_GiftGoodsGrid.gameObject, Body_GiftGoodsGrid.GetChild(0).gameObject, tmpAward, UpdateGoodsItem);
            Body_GiftGoodsGrid.parent.GetComponent<UIScrollView>().ResetPosition();
           // Body_GiftGoodsGrid.GetComponent<UIGrid>().Reposition();
                //for (int i = 0; i < Body_GiftGoodsGrid.childCount; i++)
            //{
            //    try
            //    {
            //        Body_GiftGoodsGrid.GetChild(i).GetComponent<ResourceCommonItem>().f_UpdateByInfo(tmpAward[i].mTemplate.mResourceType, tmpAward[i].mTemplate.mResourceId, tmpAward[i].mTemplate.mResourceNum);
            //    }
            //    catch
            //    {
            //        Body_GiftGoodsGrid.GetChild(i).gameObject.SetActive(false);
            //    }
            //}
            Body_GiftBuyBtn.transform.Find("Label").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(1439);
            bool CanBuy = true;
            if (VipLv < lv)
            {
                Body_GiftBuyBtn.GetComponent<UIButton>().isEnabled = false;
                CanBuy = false;
            }
            else
            {
                Body_GiftBuyBtn.GetComponent<UIButton>().isEnabled = true;
                if (((ShopGiftPoolDT)Data_Pool.m_ShopGiftPool.f_GetForId(tmpGift.iId)).m_buyTimes >= 1)
                {
                    Body_GiftBuyBtn.GetComponent<UIButton>().isEnabled = false;
                    Body_GiftBuyBtn.transform.Find("Label").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(1440);
                    CanBuy = false;
                }
            }
            Body_GiftBuyBtn.transform.Find("Label").GetComponent<UILabel>().color = CanBuy ? Color.white : new Color(0.5f, 0.5f, 0.5f);
        }
        else
            Body_Gift.gameObject.SetActive(false);
        //Body_VipLv.text = lv.ToString();
        Body_DescName.text = CommonTools.f_GetTransLanguage(1441);
        Body_Desc.text = CommonTools.f_GetTransLanguage(1442);

        Body_GiftGoodsGrid.GetComponent<UIGrid>().hideInactive = true;
        Body_GiftGoodsGrid.GetComponent<UIGrid>().enabled = true;

        BetterList<string> tmp = UITool.f_GetVipDesc(lv);

        for (int i = 0; i < tmp.size; i++)
        {
            try
            {
                Body_DescGrid.GetChild(i).GetComponent<UILabel>().text = "·" + tmp[i];
            }
            catch
            {
                Transform Desc = Instantiate(Body_TypeDesc).transform;
                Desc.GetComponent<UILabel>().text = "·" + tmp[i];
                Desc.parent = Body_DescGrid;
                Desc.localScale = Vector3.one;
                Desc.gameObject.SetActive(true);
            };

        }
        if (tmp.size < Body_DescGrid.childCount)
        {
            for (int i = tmp.size; i < Body_DescGrid.childCount; i++)
            {
                Body_DescGrid.GetChild(i).gameObject.SetActive(false);
            }
        }
        Body_DescGrid.GetComponent<UIGrid>().hideInactive = true;
        Body_DescGrid.GetComponent<UIGrid>().enabled = true;
        Body_DescGrid.GetComponent<UIGrid>().Reposition();
        f_GetObject("DescScrollView").GetComponent<UIScrollView>().ResetPosition();

        //判断能否购买礼包

        f_GetObject("Arr_Lift").SetActive(true);
        f_GetObject("Arr_Right").SetActive(true);
        if (lv >= 15)
            f_GetObject("Arr_Right").SetActive(false);
        if (lv <= 0)
            f_GetObject("Arr_Lift").SetActive(false);
        f_RegClickEvent("Arr_Lift", UI_ArrLeft, lv);
        f_RegClickEvent("Arr_Right", UI_ArrRight, lv);
    }

    void UpdateGoodsItem(GameObject go, AwardPoolDT dt ) {
        go.GetComponent<ResourceCommonItem>().f_UpdateByInfo(dt.mTemplate.mResourceType, dt.mTemplate.mResourceId, dt.mTemplate.mResourceNum);
    }
    /// <summary>
    /// 更新充值界面
    /// </summary>
    void UpdatePay()
    {
        List<NBaseSCDT> tmpPay = glo_Main.GetInstance().m_SC_Pool.m_PaySC.f_GetAll();
        for (int i = 0; i < tmpPay.Count; i++)
        {
            PccaccyDT payDT = (PccaccyDT)tmpPay[i];
            try
            {
                if (MoneyParent.GetChild(i) != null)
                {
                    UpdatePayObj(MoneyParent.GetChild(i), payDT);
                }
            }
            catch
            {
                GameObject tmpRecharge = Instantiate(Pay_Money.gameObject);
                UpdatePayObj(tmpRecharge.transform, payDT);
                tmpRecharge.transform.SetParent(MoneyParent);
                tmpRecharge.SetActive(true);
                tmpRecharge.transform.localScale = Vector3.one;
            }
        }
        MoneyParent.GetComponent<UIGrid>().enabled = true;
        MoneyParent.GetComponent<UIGrid>().Reposition();
        MoneyParent.parent.GetComponent<UIScrollView>().ResetPosition();
    }

    void UpdatePaycoin()
    {
        List<NBaseSCDT> tmpPay = glo_Main.GetInstance().m_SC_Pool.m_PayCoinSC.f_GetAll();
        for (int i = 0; i < tmpPay.Count; i++)
        {
            PayCoinDT payDT = (PayCoinDT)tmpPay[i];
            try
            {
                if (MoneyParentCoin.GetChild(i) != null)
                {
                    UpdatePayObjCoin(MoneyParentCoin.GetChild(i), payDT);
                }
            }
            catch
            {
                GameObject tmpRecharge = Instantiate(Pay_Moneycoin.gameObject);
                UpdatePayObjCoin(tmpRecharge.transform, payDT);
                tmpRecharge.transform.SetParent(MoneyParentCoin);
                tmpRecharge.SetActive(true);
                tmpRecharge.transform.localScale = Vector3.one;
            }
        }
        MoneyParentCoin.GetComponent<UIGrid>().enabled = true;
        MoneyParentCoin.GetComponent<UIGrid>().Reposition();
        MoneyParentCoin.parent.GetComponent<UIScrollView>().ResetPosition();
    }

    private void UpdatePayObjCoin(Transform PayObj, PayCoinDT payDT)
    {
        PayObj.gameObject.SetActive(true);
        PayObj.Find("Desc").gameObject.SetActive(true);
        PayObj.Find("Activity").gameObject.SetActive(false);
        PayObj.Find("Recharge/Name").GetComponent<UILabel>().text = payDT.szPrice + "$";
PayObj.Find("Recharge/TitleLabel").GetComponent<UILabel>().text = payDT.iPoint + " Kim Phiếu";

        PayObj.Find("Desc").GetComponent<UILabel>().text = "";

        PayObj.Find("First").gameObject.SetActive(false);
        f_RegClickEvent(PayObj.Find("Recharge").gameObject, RechargeCoin, payDT.iId);
    }
    /// <summary>
    /// 更新充值数据
    /// </summary>
    private void UpdatePayObj(Transform PayObj, PccaccyDT payDT)
    {
        PayObj.gameObject.SetActive(true);
        if (payDT.iId == 1)//月卡25
        {
            bool isRecharge = Data_Pool.m_ActivityCommonData.m_MonthCardIsBuy25;
            PayObj.Find("Icon").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSprite(30);
            PayObj.Find("Recharge/Name").GetComponent<UILabel>().text = isRecharge ? "" : CommonTools.f_GetTransLanguage(1443);
            PayObj.Find("Recharge/TitleLabel").GetComponent<UILabel>().text = "";
            PayObj.Find("Desc").gameObject.SetActive(false);
            PayObj.Find("Activity").gameObject.SetActive(true);
            PayObj.Find("First").gameObject.SetActive(false);
            PayObj.Find("Moth25Label").gameObject.SetActive(true);
            PayObj.Find("Moth25Label").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1444), payDT.iPccaccyNum);
            GameNameParamDT gameParam = glo_Main.GetInstance().m_SC_Pool.m_GameNameParamSC.f_GetSC((int)EM_GameNameParamType.MothCard25) as GameNameParamDT;
            ResourceCommonDT commonDT = new ResourceCommonDT();
            string[] awardStr = gameParam.szParam1.Split(';');
            commonDT.f_UpdateInfo(byte.Parse(awardStr[0]), int.Parse(awardStr[1]), int.Parse(awardStr[2]));
            PayObj.Find("Moth25Label/Desc2").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1684), commonDT.mResourceNum + commonDT.mName);
            PayObj.Find("HasRecharge").gameObject.SetActive(isRecharge);
            PayObj.Find("Recharge").gameObject.SetActive(!isRecharge);

            //据说月卡会改变战斗力
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.PlayerFightPowerChange);
        }
        else if (payDT.iId == 2)//月卡50
        {
            bool isRecharge =  Data_Pool.m_ActivityCommonData.m_MonthCardIsBuy50;
            PayObj.Find("Icon").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSprite(31);
            PayObj.Find("Recharge/Name").GetComponent<UILabel>().text = isRecharge? "" : CommonTools.f_GetTransLanguage(1443);
            PayObj.Find("Recharge/TitleLabel").GetComponent<UILabel>().text = "";
            PayObj.Find("Desc").gameObject.SetActive(false);
            PayObj.Find("Activity").gameObject.SetActive(true);
            PayObj.Find("First").gameObject.SetActive(false);
            PayObj.Find("Moth50Label").gameObject.SetActive(true);
            PayObj.Find("Moth50Label").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1445), payDT.iPccaccyNum);
            GameNameParamDT gameParam = glo_Main.GetInstance().m_SC_Pool.m_GameNameParamSC.f_GetSC((int)EM_GameNameParamType.MothCard50) as GameNameParamDT;
            ResourceCommonDT commonDT = new ResourceCommonDT();
            string[] awardStr = gameParam.szParam1.Split(';');
            commonDT.f_UpdateInfo(byte.Parse(awardStr[0]), int.Parse(awardStr[1]), int.Parse(awardStr[2]));
            PayObj.Find("Moth50Label/Desc2").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1684), commonDT.mResourceNum + commonDT.mName);
            PayObj.Find("HasRecharge").gameObject.SetActive(isRecharge);
            PayObj.Find("Recharge").gameObject.SetActive(!isRecharge);

            //据说月卡会改变战斗力
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.PlayerFightPowerChange);
        }
        else
        {
            PayObj.Find("Desc").gameObject.SetActive(true);
            PayObj.Find("Activity").gameObject.SetActive(false);
PayObj.Find("Recharge/Name").GetComponent<UILabel>().text = payDT.iPccaccyNum + " Kim Phiếu";
            PayObj.Find("Recharge/TitleLabel").GetComponent<UILabel>().text = payDT.szPccaccyDesc;
            int iconId = 35;
            switch (payDT.iPccaccyNum)//6,45,68,118
            {
                case 16: iconId = 32; break;
                case 48: iconId = 33; break;
                case 93: iconId = 34; break;
                case 193: iconId = 35; break;
            }
            PayObj.Find("Icon").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSprite(iconId);
            if (((RechargePoolDT)Data_Pool.m_RechargePool.f_GetForId(payDT.iId)).times == 0)
            {
                PayObj.Find("Desc").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1448), payDT.iFirstPccaccyNum);
                PayObj.Find("First").gameObject.SetActive(true);
            }
            else
            {
                PayObj.Find("Desc").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1449), payDT.iPayCount, payDT.iPayCount);
                //if (payDT.iPresentPccaccyNum != 0)
                   
                //else
                //    PayObj.Find("Desc").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1450), payDT.iPresentPccaccyNum, payDT.iPccaccyNum);
                PayObj.Find("First").gameObject.SetActive(false);
            }
        }
        f_RegClickEvent(PayObj.Find("Recharge").gameObject, Recharge, payDT.iId);
    }
    /// <summary>
    /// 购买礼包
    /// </summary>
    void BuyGift(GameObject go, object obj1, object obj2)
    {
        ShopGiftDT tmpGift = (ShopGiftDT)obj1;
        if (tmpGift.iNewNum > Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee))
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1451));
            return;
        }

        SocketCallbackDT back = new SocketCallbackDT();
        back.m_ccCallbackFail = (object obj) =>
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1452));
            UpdateBody(NowVipLv);
        };
        back.m_ccCallbackSuc = (object obj3) =>
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1453));
            UpdateBody(NowVipLv);
        };
        Data_Pool.m_ShopGiftPool.f_Buy(tmpGift.iId, back);
    }
    /// <summary>
    /// 退出
    /// </summary>
    void UI_CloseThis(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
		glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.MainBg);
        
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShowVip, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
    }
    void UI_ArrLeft(GameObject go, object obj1, object obj2)
    {
        NowVipLv--;
        if (NowVipLv <= 0)
            NowVipLv = 0;
        UpdateBody(NowVipLv);
    }

    void UI_ArrRight(GameObject go, object obj1, object obj2)
    {
        NowVipLv++;
        if (NowVipLv >= 15)
            NowVipLv = 15;
        UpdateBody(NowVipLv);
    }

    void UI_OpenPay(GameObject go, object obj1, object obj2)
    {
        f_GetObject("PayBtn").SetActive(false);
        f_GetObject("PreBodyBg").SetActive(false);
        f_GetObject("Pay").SetActive(true);
        f_GetObject("PayCoin").SetActive(false);
        f_GetObject("PreBtn").SetActive(true);
        f_GetObject("Arr_Lift").SetActive(false);
        f_GetObject("Arr_Right").SetActive(false);
        f_GetObject("PayBtn2").SetActive(true);
        f_GetObject("PayBtnCoin").SetActive(true);
        f_GetObject("PayBtn2").transform.Find("Select").gameObject.SetActive(true);
        f_GetObject("PayBtnCoin").transform.Find("Select").gameObject.SetActive(false);
        UpdatePay();
        //更新标题
        //f_GetObject("RechargeTitle").SetActive(true);
        //f_GetObject("VipTitle").SetActive(false);
    }

    void UI_OpenPayCoin(GameObject go, object obj1, object obj2)
    {
        f_GetObject("PayBtn").SetActive(false);
        f_GetObject("PreBodyBg").SetActive(false);
        f_GetObject("Pay").SetActive(false);
        f_GetObject("PreBtn").SetActive(true);
        f_GetObject("PayCoin").SetActive(true);
        f_GetObject("Arr_Lift").SetActive(false);
        f_GetObject("Arr_Right").SetActive(false);
        f_GetObject("PayBtn2").SetActive(true);
        f_GetObject("PayBtnCoin").SetActive(true);
        f_GetObject("PayBtn2").transform.Find("Select").gameObject.SetActive(false);
        f_GetObject("PayBtnCoin").transform.Find("Select").gameObject.SetActive(true);
        UpdatePaycoin();
        //更新标题
        //f_GetObject("RechargeTitle").SetActive(true);
        //f_GetObject("VipTitle").SetActive(false);
    }

    void UI_OpenPre(GameObject go, object obj1, object obj2)
    {
        f_GetObject("PreBtn").SetActive(false);
        f_GetObject("PreBodyBg").SetActive(true);
        f_GetObject("Pay").SetActive(false);
        f_GetObject("PayCoin").SetActive(false);
        f_GetObject("PayBtn2").SetActive(false);
        f_GetObject("PayBtnCoin").SetActive(false);
        f_GetObject("PayBtn").SetActive(true);
        UpdateBody(VipLv);
        //更新标题
        //f_GetObject("RechargeTitle").SetActive(false);
        //f_GetObject("VipTitle").SetActive(true);
    }

    //TsuCode - Coin - KP
    private void UI_BtnAddCoin(GameObject go, object obj1, object obj2)
    {
        //ccUIHoldPool.GetInstance().f_Hold(this);
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.ShowVip, UIMessageDef.UI_OPEN, ShowVip.EM_PageIndex.Recharge);
UITool.Ui_Trip("Giao dịch thành công");
        string tCPOrderId = glo_Main.GetInstance().m_SDKCmponent.f_CreateCPOrderId();
        glo_Main.GetInstance().m_SDKCmponent.f_Pccaccy(0, "", 0, 0, tCPOrderId, "", 0, "", "");
    }
    //-------------------------
    private void f_OnHelpBtnClick(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonHelpPage, UIMessageDef.UI_OPEN, 24);
    }
    void Recharge(GameObject go, object obj1, object obj2)
    {
        if (!isCanPay)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1454));
            return;
        }
        int tmpid = (int)obj1;
        //SocketCallbackDT tmp = new SocketCallbackDT();
        //Data_Pool.m_RechargePool.Recharge(tmpid, tmp, RechargeSucBack);

        //TsuComment -----------------------
        //PccaccyDT tPayDt = (PccaccyDT)glo_Main.GetInstance().m_SC_Pool.m_PaySC.f_GetSC(tmpid);
        //string tPayName = UITool.f_GetPayName(tPayDt);
        //string tCPOrderId = glo_Main.GetInstance().m_SDKCmponent.f_CreateCPOrderId();
        //glo_Main.GetInstance().m_SDKCmponent.f_Pccaccy(tPayDt.iId, tPayName, tPayDt.iPccaccyNum * 1000, 1, tCPOrderId, tPayDt.iId.ToString(), tPayDt.iPccaccyNum, CommonTools.f_GetTransLanguage(1455), tPayName);
        //////////////////////////////------------------------------------//////////////////////////////////////////
    

        //TsuCode - COin - Kim Phieu
        //Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Coin)

        PccaccyDT tPayDt = (PccaccyDT)glo_Main.GetInstance().m_SC_Pool.m_PaySC.f_GetSC(tmpid);
        string tPayName = UITool.f_GetPayName(tPayDt);
        string tCPOrderId = glo_Main.GetInstance().m_SDKCmponent.f_CreateCPOrderId();

        int uCoin = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Coin);
        if (uCoin >= tPayDt.iPccaccyNum)
        {
            MessageBox.ASSERT("TsuLog: Co the mua " + uCoin + " >= " + tPayDt.iPccaccyNum);
            //glo_Main.GetInstance().m_SDKCmponent.f_Pccaccy(tPayDt.iId, tPayName, tPayDt.iPccaccyNum * 1000, 1, tCPOrderId, tPayDt.iId.ToString(), tPayDt.iPccaccyNum, CommonTools.f_GetTransLanguage(1455), tPayName);
            //a.f_Pccaccy(tPayDt.iId, tPayName, tPayDt.iPccaccyNum * 1000, 1, tCPOrderId, tPayDt.iId.ToString(), tPayDt.iPccaccyNum, CommonTools.f_GetTransLanguage(1455), tPayName);
PopupMenuParams tParam = new PopupMenuParams("Xác nhận", string.Format("Dùng {0} Kim Phiếu \n để mua gói {1}", tPayDt.iPccaccyNum, tPayDt.szPccaccyDesc), "Đồng ý", f_ConfirmRecharge, "Hủy bỏ" , null, tPayDt.iId);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParam);
        }
        else
        {
            MessageBox.ASSERT("TsuLog: Khong du kiem phieu de giao dich " + uCoin + " < " + tPayDt.iPccaccyNum);
UITool.Ui_Trip("Không đủ Kim Phiếu!");

            UI_OpenPayCoin(null, null, null);

        }

        //----------------------------------

#if REYUN
        if (!string.IsNullOrEmpty(SDKHelper.REYUN_KEY))
        {
            //支付方式(paymentType):无法获取统一写alipay  货币类型(currentType)：无法获取统一写CNY  价格(amount):单位：元
            Tracking.Instance.setryzfStart(tCPOrderId, "alipay", "CNY", (float)tPayDt.iPccaccyNum);
            MessageBox.DEBUG(string.Format("ReyunSDK SetPaymentStart,m_OrderId:{0}, amount:{1}, date:{2}", tCPOrderId, (float)tPayDt.iPccaccyNum, System.DateTime.Now.ToString("HH-mm-ss")));
        }
#endif
    }

    void RechargeCoin(GameObject go, object obj1, object obj2)
    {
        if (!isCanPay)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1454));
            return;
        }
        int tmpid = (int)obj1;

        PayCoinDT tPayDt = (PayCoinDT)glo_Main.GetInstance().m_SC_Pool.m_PayCoinSC.f_GetSC(tmpid);

        //todo  gọi sdk mua va chuyen vào tPayDt
        //glo_Main.GetInstance().m_SDKCmponent.f_ShowSDKPay(tPayDt);

    }
    private void f_ConfirmRecharge(object value)
    {
        int orderId = (int)value;

        SocketCallbackDT whitelistCallbackDt = new SocketCallbackDT();
        whitelistCallbackDt.m_ccCallbackSuc = f_OnRechargeCoin;
        whitelistCallbackDt.m_ccCallbackFail = f_OnRechargeCoin;
        Data_Pool.m_RechargePool.RechargeCoin(orderId, whitelistCallbackDt);
    }

    void f_OnPayUpdateView(object obj)
    {
      // SDKPccaccyResult tResult = (SDKPccaccyResult)obj;
        UpdatePay();
        UpadteShowVip(false);
    }
    /// <summary>
    /// 页码
    /// </summary>
    public enum EM_PageIndex
    {
        Vip = 1,
        Recharge = 2,
        RechargeCoin = 3,
    }


    #region TsuCode - Kim Phieu
    private void f_OnRechargeCoin(object result)
    {
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
UITool.Ui_Trip("Mua hàng thành công");
            int coinCount = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Coin);
            string coinText = UITool.f_CountToChineseStr2(coinCount);
            f_GetObject("LabelCoinCount").GetComponent<UILabel>().text = coinText;
            UpdatePay();
            UpadteShowVip(false);
        }
        else
        {
UITool.Ui_Trip("Mua hàng không thành công");
            MessageBox.ASSERT("Recharge whitelist failed,code:" + (int)result);
        }
    }

    #endregion TsuCode - kim phieu
}
