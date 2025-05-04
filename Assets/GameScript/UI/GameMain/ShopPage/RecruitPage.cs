using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
using System;
using System.Linq;
/// <summary>
/// 武将招募界面
/// </summary>
public class RecruitPage : UIFramwork
{
    private SocketCallbackDT ReqeustBuyShopLotteryCallback = new SocketCallbackDT();//购买抽牌回调
    private SocketCallbackDT ReqeustChooseShopLotteryCallback = new SocketCallbackDT();// chọn item
    private SocketCallbackDT ReqeustGetAwardShopLotteryCallback = new SocketCallbackDT();// nhận quà
    private EM_RecruitType m_CurRecruitType;
    private int mGenSelectKey;
    private UIWrapComponent m_cardWrapContent = null;
    private GameObject awardGrid;
    private GameObject awardItem;
    private GameObject Btn0;
    private GameObject Btn1;
    private GameObject Btn2;
    private GameObject Btn3;
    private GameObject Btn4;
    private GameObject RootBtn;
    private List<GameObject> _PosList;
    private EM_CardCamp m_EM_CardCampSelect;
    private UIWrapComponent _awardShowComponent;
    private UIWrapComponent mAwardShowComponent
    {
        get
        {
            if (_awardShowComponent == null)
            {
                ShopLotteryPoolDT lotteryGenPoolDT = Data_Pool.m_ShopLotteryPool.f_GetForId((long)EM_RecruitType.GenAd) as ShopLotteryPoolDT;
                List<BasePoolDT<long>> list = lotteryGenPoolDT.f_GetList(EM_CardCamp.eCardMain);
                _awardShowComponent = new UIWrapComponent(180, 7, 150, 3, awardGrid, awardItem, list, CardItemUpdateByInfo, CardItemClick);
            }
            _awardShowComponent.f_ResetView();
            //awardGrid.GetComponent<UIGrid>().Reposition();
            return _awardShowComponent;
        }
    }
    private void CardItemUpdateByInfo(Transform item, BasePoolDT<long> dt)
    {
        ShopLotteryItem tShopLotteryItem = dt as ShopLotteryItem;
        ResourceCommonDT mData = new ResourceCommonDT();
        mData.f_UpdateInfo((byte)4, tShopLotteryItem.value, 1);

        UI2DSprite mIcon = item.Find("Icon").GetComponent<UI2DSprite>();
        UISprite IconBorder = item.Find("IconBorder").GetComponent<UISprite>();
        UILabel Name = item.Find("Name").GetComponent<UILabel>();

        UITool.f_SetIconSprite(mIcon, (EM_ResourceType)mData.mResourceType, mData.mResourceId);
        string name = mData.mName;
        IconBorder.spriteName = UITool.f_GetImporentColorName(mData.mImportant, ref name);
        Name.text = name;
        item.Find("Select").gameObject.SetActive(tShopLotteryItem.key == mGenSelectKey);
    }
    private void CardItemClick(Transform item, BasePoolDT<long> dt)
    {
        ShopLotteryItem tShopLotteryItem = dt as ShopLotteryItem;
        mGenSelectKey = tShopLotteryItem.key;
        mAwardShowComponent.f_UpdateView();
        //mAwardShowComponent.f_ResetView(); //f_ViewGotoRealIdx
        //awardGrid.GetComponent<UIGrid>().Reposition();
    }

    private ccUIBase m_UiBase = null;
    private EM_PageIndex eM_PageIndex = EM_PageIndex.RecruitContent;
    /// <summary>
    /// 更新UI
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
    public void RefreshUI(ccUIBase baseUI, int pageIndex)
    {
        awardGrid = f_GetObject("AwardGrid");
        awardItem = f_GetObject("ResourceCommonItem");
        showChooseConent = false;
        f_GetObject("GenChooseConent").SetActive(showChooseConent);
        f_GetObject("CampChooseConent").SetActive(showChooseConent);
        m_UiBase = baseUI;
        eM_PageIndex = (EM_PageIndex)pageIndex;
        SetUIState(f_GetObject("TexNorAdmiral"), eM_PageIndex == EM_PageIndex.RecruitContent);
        SetUIState(f_GetObject("TexGenAdmiral"), eM_PageIndex == EM_PageIndex.RecruitContentGen);
        SetUIState(f_GetObject("TexCampAdmiral"), eM_PageIndex == EM_PageIndex.RecruitContentCamp);
        CheckIsFree();
        InitShopLotteryInfo(f_GetObject("BtnBuyOneNorAdmiral"), f_GetObject("BtnBuyTenNorAdmiral"), EM_RecruitType.NorAd);
        InitShopLotteryInfo(f_GetObject("BtnBuyOneGenAdmiral"), f_GetObject("BtnBuyTenGenAdmiral"), EM_RecruitType.GenAd);
        InitShopLotteryInfo(f_GetObject("BtnBuyOneCampAdmiral"), f_GetObject("BtnBuyTenCampAdmiral"), EM_RecruitType.CampAd);
        ShopLotteryPoolDT lotteryNorPoolDT = Data_Pool.m_ShopLotteryPool.f_GetForId((long)EM_RecruitType.NorAd) as ShopLotteryPoolDT;
        //int times1 = (10 - (lotteryNorPoolDT.totalTimes % 10));
        //f_GetObject("LabelSendAdHint").GetComponent<UILabel>().text = (times1 == 10 ? CommonTools.f_GetTransLanguage(1110) : CommonTools.f_GetTransLanguage(1111)) + CommonTools.f_GetTransLanguage(1112) + times1 + CommonTools.f_GetTransLanguage(1113);
        ShopLotteryPoolDT lotteryGenPoolDT = Data_Pool.m_ShopLotteryPool.f_GetForId((long)EM_RecruitType.GenAd) as ShopLotteryPoolDT;
        //int times2 = (10 - (lotteryGenPoolDT.totalTimes % 10));
        //f_GetObject("LabelSendGenHint").GetComponent<UILabel>().text = (times2 == 10 ? CommonTools.f_GetTransLanguage(1110) : CommonTools.f_GetTransLanguage(1111)) + CommonTools.f_GetTransLanguage(1112) + times2 + CommonTools.f_GetTransLanguage(1114);

        UpdateRewardContent();
        ShopLotteryPoolDT lotteryCampPoolDT = Data_Pool.m_ShopLotteryPool.f_GetForId((long)EM_RecruitType.CampAd) as ShopLotteryPoolDT;
        UI2DSprite genIcon = f_GetObject("GenChooseShow").transform.Find("Icon").GetComponent<UI2DSprite>();
        UISprite campIcon = f_GetObject("CampChooseShow").transform.Find("Icon").GetComponent<UISprite>();
        UISprite BtnGenChooseIcon = f_GetObject("BtnGenChoose").transform.Find("Icon").GetComponent<UISprite>();
        if (lotteryGenPoolDT.itemId > 0)
        {
            genIcon.sprite2D = UITool.f_GetIconSpriteByCardId(lotteryGenPoolDT.GetValueById(lotteryGenPoolDT.itemId));//UITool.f_GetIconSpriteByCardId(lotteryGenPoolDT.itemId);
            genIcon.gameObject.SetActive(true);
            BtnGenChooseIcon.spriteName = "btn6";
        }
        else
        {
            genIcon.gameObject.SetActive(false);
            BtnGenChooseIcon.spriteName = "btn5";
        }
        UISprite BtnCampChooseIcon = f_GetObject("BtnCampChoose").transform.Find("Icon").GetComponent<UISprite>();
        if (lotteryCampPoolDT.itemId > 0)
        {
            campIcon.spriteName = "IconCamp_" + lotteryCampPoolDT.GetId(lotteryCampPoolDT.itemId); 
            campIcon.gameObject.SetActive(true);
            BtnCampChooseIcon.spriteName = "btn6";
        }
        else
        {
            campIcon.gameObject.SetActive(false);
            BtnCampChooseIcon.spriteName = "btn5";
        }
        UpdatePosBtn();
    }

    public void UpdateRewardContent()
    {
        f_GetObject("RewardContentGen").SetActive(true);
        ShopLotteryPoolDT lotteryGenPoolDT = Data_Pool.m_ShopLotteryPool.f_GetForId((long)EM_RecruitType.GenAd) as ShopLotteryPoolDT;
        ShopLotteryGodAwardDT lotteryGodAwardDT = glo_Main.GetInstance().m_SC_Pool.m_ShopLotteryGenAwardSC.f_GetSC(lotteryGenPoolDT.award + 1) as ShopLotteryGodAwardDT;
        if(lotteryGodAwardDT!= null)
        {
            f_GetObject("LabelGenTip").GetComponent<UILabel>().text = "Triệu hồi " + lotteryGenPoolDT.tempTotalTimes + "/"+ lotteryGodAwardDT.iNum + " được nhận quà thêm";
        }
        else
        {
            f_GetObject("RewardContentGen").SetActive(false);
        }
        f_GetObject("RewardContentCamp").SetActive(true);
        ShopLotteryPoolDT lotteryCampPoolDT = Data_Pool.m_ShopLotteryPool.f_GetForId((long)EM_RecruitType.CampAd) as ShopLotteryPoolDT;
        ShopLotteryGodAwardDT lotteryCampAwardDT = glo_Main.GetInstance().m_SC_Pool.m_ShopLotteryCampAwardSC.f_GetSC(lotteryCampPoolDT.award + 1) as ShopLotteryGodAwardDT;
        if (lotteryCampAwardDT != null)
        {
            f_GetObject("LabelCampTip").GetComponent<UILabel>().text = "Triệu hồi " + lotteryCampPoolDT.tempTotalTimes + "/" + lotteryCampAwardDT.iNum + " được nhận quà thêm";
        }
        else
        {
            f_GetObject("RewardContentCamp").SetActive(false);
        }
    }

    public static void SetUIState(GameObject buttonBg, bool isAct)
    {
        buttonBg.SetActive(isAct);
    }
    /// <summary>
    /// 获取购买次数
    /// </summary>
    /// <returns></returns>
    private string GetTimes(int times)
    {
        switch (times)
        {
            case 1: return CommonTools.f_GetTransLanguage(1597);
            case 2: return CommonTools.f_GetTransLanguage(1598);
            case 3: return CommonTools.f_GetTransLanguage(1599);
            case 4: return CommonTools.f_GetTransLanguage(1600);
            case 5: return CommonTools.f_GetTransLanguage(1601);
            case 6: return CommonTools.f_GetTransLanguage(1602);
            case 7: return CommonTools.f_GetTransLanguage(1603);
            case 8: return CommonTools.f_GetTransLanguage(1604);
            case 9: return CommonTools.f_GetTransLanguage(1605);
            case 10: return CommonTools.f_GetTransLanguage(1606);
        }
        return "";
    }
    private void InitUI()
    {
        Btn0 = f_GetObject("Btn0");
        Btn1 = f_GetObject("Btn1");
        Btn2 = f_GetObject("Btn2");
        Btn3 = f_GetObject("Btn3");
        Btn4 = f_GetObject("Btn4");
        RootBtn = f_GetObject("RootBtn");
        _PosList = new List<GameObject>();
        for(int i = 0; i < 7; i++)
        {
            _PosList.Add(RootBtn.transform.Find("Pos_"+i).gameObject);
        }
    }
    /// <summary>
    /// 初始化消息
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        InitUI();
        f_RegClickEvent("BtnBuyOneNorAdmiral", OnBtnBuyOneClick, EM_RecruitType.NorAd);
        f_RegClickEvent("BtnBuyTenNorAdmiral", OnBtnBuyTenClick, EM_RecruitType.NorAd);
        f_RegClickEvent("BtnBuyOneGenAdmiral", OnBtnBuyOneClick, EM_RecruitType.GenAd);
        f_RegClickEvent("BtnBuyTenGenAdmiral", OnBtnBuyTenClick, EM_RecruitType.GenAd);
        f_RegClickEvent("TextGetCardBg", OnTouchGetCardPage);
        f_RegClickEvent("Btn_NorShow", Btn_NorShow);
        f_RegClickEvent("Btn_GenShow", Btn_GenShow);
        f_RegClickEvent("Btn_CampShow", Btn_CampShow);
        ReqeustBuyShopLotteryCallback.m_ccCallbackSuc = OnRequestShopLotteryBuySucCallback;
        ReqeustBuyShopLotteryCallback.m_ccCallbackFail = OnRequestShopLotteryBuyFailCallback;

        f_RegClickEvent("BtnBuyOneCampAdmiral", OnBtnBuyCampClick, EM_RecruitType.CampAd,1);
        f_RegClickEvent("BtnBuyTenCampAdmiral", OnBtnBuyCampClick, EM_RecruitType.CampAd,2);
        f_RegClickEvent("BtnGenGetAward", OnBtnGetAwardClick,EM_RecruitType.GenAd);
        f_RegClickEvent("BtnGenChoose", OnBtnChooseShowClick, EM_RecruitType.GenAd);
        f_RegClickEvent("BtnCampGetAward", OnBtnGetAwardClick, EM_RecruitType.CampAd);
        f_RegClickEvent("BtnCampChoose", OnBtnChooseShowClick, EM_RecruitType.CampAd);
        ReqeustChooseShopLotteryCallback.m_ccCallbackSuc = OnRequestChooseLotteryBuySucCallback;
        ReqeustChooseShopLotteryCallback.m_ccCallbackFail = OnRequestChooseLotteryBuyFailCallback;
        ReqeustGetAwardShopLotteryCallback.m_ccCallbackSuc = OnRequestGetAwardLotteryBuySucCallback;
        ReqeustGetAwardShopLotteryCallback.m_ccCallbackFail = OnRequestGetAwardLotteryBuyFailCallback;

        f_RegClickEvent("BtnWei", OnBtnCampChooseClick, EM_RecruitType.CampAd, EM_CardCamp.eCardWei);
        f_RegClickEvent("BtnShu", OnBtnCampChooseClick, EM_RecruitType.CampAd, EM_CardCamp.eCardShu);
        f_RegClickEvent("BtnWu", OnBtnCampChooseClick, EM_RecruitType.CampAd, EM_CardCamp.eCardWu);
        f_RegClickEvent("BtnGroupHero", OnBtnCampChooseClick, EM_RecruitType.CampAd, EM_CardCamp.eCardGroupHero);

        f_RegClickEvent("Btn_GenClose", OnBtnGenChooseCloseClick, EM_RecruitType.GenAd);
        f_RegClickEvent("MaskClose", OnBtnGenChooseCloseClick, EM_RecruitType.GenAd);
        f_RegClickEvent("Btn_GenChoose", OnBtnGenChooseClick, EM_RecruitType.GenAd);
        f_RegClickEvent("Btn_CampShop", OnBtnCampShopClick, EM_RecruitType.GenAd);

        f_RegClickEvent(Btn0, OnTapItemClick, EM_CardCamp.eCardMain);
        f_RegClickEvent(Btn1, OnTapItemClick, EM_CardCamp.eCardWei);
        f_RegClickEvent(Btn2, OnTapItemClick, EM_CardCamp.eCardShu);
        f_RegClickEvent(Btn3, OnTapItemClick, EM_CardCamp.eCardWu);
        f_RegClickEvent(Btn4, OnTapItemClick, EM_CardCamp.eCardGroupHero);
    }
    private void OnTapItemClick(GameObject go, object obj1, object obj2)
    {
        EM_CardCamp cardCamp = (EM_CardCamp)obj1;
        SetTapState(Btn0, cardCamp == EM_CardCamp.eCardMain);
        SetTapState(Btn1, cardCamp == EM_CardCamp.eCardWei);
        SetTapState(Btn2, cardCamp == EM_CardCamp.eCardShu);
        SetTapState(Btn3, cardCamp == EM_CardCamp.eCardWu);
        SetTapState(Btn4, cardCamp == EM_CardCamp.eCardGroupHero);
        ShowCampContent(cardCamp);
    }
    private void SetTapState(GameObject go,bool isSelect)
    {
        go.transform.Find("Press").gameObject.SetActive(isSelect);
    }
    private void ShowCampContent(EM_CardCamp camp)
    {
        m_EM_CardCampSelect = camp;
        mGenSelectKey = -1;
        ShopLotteryPoolDT lotteryGenPoolDT = Data_Pool.m_ShopLotteryPool.f_GetForId((long)EM_RecruitType.GenAd) as ShopLotteryPoolDT;
        List<BasePoolDT<long>> list = lotteryGenPoolDT.f_GetList(m_EM_CardCampSelect);
        mAwardShowComponent.f_UpdateList(list);
        mAwardShowComponent.f_ResetView();
    }
    private void UpdatePosBtn()
    {
        int[] indexs = new int[4];
        ShopLotteryPoolDT lotteryCampPoolDT = Data_Pool.m_ShopLotteryPool.f_GetForId((long)EM_RecruitType.CampAd) as ShopLotteryPoolDT;
        EM_CardCamp camp = (EM_CardCamp)lotteryCampPoolDT.GetId(lotteryCampPoolDT.itemId);
        switch (camp)
        {
            case EM_CardCamp.eCardWei:
                indexs = new int[4] { 3, 4, 5, 6};
                break;
            case EM_CardCamp.eCardShu:
                indexs = new int[4] { 2, 3, 4, 5 };
                break;
            case EM_CardCamp.eCardWu:
                indexs = new int[4] { 1, 2, 3, 4 };
                break;
            case EM_CardCamp.eCardGroupHero:
                indexs = new int[4] { 0, 1, 2, 3 };
                break;
            default:
                indexs = new int[4] { 3, 4, 5, 6 };
                break;
        }
        f_GetObject("BtnWei").transform.position = _PosList.ElementAt(indexs[0]).transform.position;
        f_GetObject("BtnShu").transform.position = _PosList.ElementAt(indexs[1]).transform.position;
        f_GetObject("BtnWu").transform.position = _PosList.ElementAt(indexs[2]).transform.position;
        f_GetObject("BtnGroupHero").transform.position = _PosList.ElementAt(indexs[3]).transform.position;
    }
    /// <summary>
    /// 获取招募将令id
    /// </summary>
    /// <param name="recruitType"></param>
    private int GetReruitPropId(EM_RecruitType recruitType)
    {
        switch (recruitType)
        {
            case EM_RecruitType.NorAd://战将令206，神将令207
                return 206;
            case EM_RecruitType.GenAd:
                return 207;
            case EM_RecruitType.CampAd:
                return 501;
        }
        return 206;
    }
    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopPage, UIMessageDef.UI_OPEN);
    }
    /// <summary>
    /// 显示获得的卡牌
    /// </summary>
    /// <param name="listGetCard"></param>
    private void ShowGetCardList(List<int> listGetCard)
    {
        ccUIBase uiShop = ccUIManage.GetInstance().f_GetUIHandler(UINameConst.ShopPage);
        ccUIHoldPool.GetInstance().f_Hold(uiShop);
        EquipSythesis tSythesis = new EquipSythesis(listGetCard);
        if (Data_Pool.m_ShopLotteryPool.lotteryId == EM_RecruitType.CampAd)
        {
            tSythesis.m_OnCloseCallback = (object obj) =>
            {
                OnBtnBuyCampClick(null, Data_Pool.m_ShopLotteryPool.lotteryId, 1) ;
            };
        }
        else
        {
            tSythesis.m_OnCloseCallback = (object obj) =>
            {
                OnBtnBuyOneClick(null, Data_Pool.m_ShopLotteryPool.lotteryId, null);
            };
        }
        
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainEquipShowPage, UIMessageDef.UI_OPEN, tSythesis);
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopPage, UIMessageDef.UI_CLOSE);
        //GameObject TextGetCardBg = f_GetObject("TextGetCardBg");
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
        //TextGetCardBg.SetActive(true);
        //TextGetCardBg.transform.FindChild("CardMore").gameObject.SetActive(true);
        //TextGetCardBg.transform.FindChild("CardMore/GetHint").GetComponent<UILabel>().text = "恭喜你获得" + listGetCard.Count + "位神将！";
        //List<BasePoolDT<long>> listCardData = new List<BasePoolDT<long>>();
        //for (int i = 0; i < listGetCard.Count; i++)
        //{
        //    BasePoolDT<long> item = new BasePoolDT<long>();
        //    item.iId = listGetCard[i];
        //    listCardData.Add(item);
        //}
        //if (m_cardWrapContent == null)
        //    m_cardWrapContent = new UIWrapComponent(230, 5, 270, 5, f_GetObject("CardItemParent"), f_GetObject("CardItem"), listCardData, ItemUpdateByInfo, null);
        //m_cardWrapContent.f_UpdateList(listCardData);
        //m_cardWrapContent.f_ResetView();

    }
    
    /// <summary>
    /// 当招募到2个时连显示2个
    /// </summary>
    /// <param name="data"></param>
    private void OnShowCardOneCallback(object data)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_Hold(this);
        EquipSythesis tSythesis = new EquipSythesis((int)data, 1, EquipSythesis.ResonureType.Card);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainEquipShowPage, UIMessageDef.UI_OPEN, tSythesis);
    }
    /// <summary>
    /// 卡牌item更新
    /// </summary>
    /// <param name="item"></param>
    /// <param name="dt"></param>
    private void ItemUpdateByInfo(Transform item, BasePoolDT<long> dt)
    {
        CardDT cardDT = (glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC((int)dt.iId) as CardDT);
        item.GetComponent<RecruitCardItem>().ShowData(cardDT);
    }
    #region 请求购买牌
    private void OnRequestShopLotteryBuySucCallback(object obj)
    {
        Data_Pool.m_GuidancePool.m_OtherSave = true;
        RefreshUI(m_UiBase,(int) eM_PageIndex);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        Debug.Log(teMsgOperateResult.ToString());
        List<int> listGetCard = Data_Pool.m_CardPool.f_GetlistLastAddCardTempID();
        ShowGetCardList(listGetCard);
        UITool.f_OpenOrCloseWaitTip(false);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_UPDATE_USERINFOR);
    }
    private void OnRequestShopLotteryBuyFailCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        if (teMsgOperateResult == eMsgOperateResult.eOR_NotyetFreeTime)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1115));
            RefreshUI(m_UiBase, (int) eM_PageIndex);

        }
        else
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1116) + CommonTools.f_GetTransLanguage((int)obj));
        }
    }
    #endregion
    #region 初始化商店抽牌数据

    /// <summary>
    /// 检测战将和神将是否免费
    /// </summary>
    private void CheckIsFree()
    {
        ShopLotteryPoolDT lotteryPoolDT = Data_Pool.m_ShopLotteryPool.f_GetForId((long)EM_RecruitType.NorAd) as ShopLotteryPoolDT;
        int secondLeft = lotteryPoolDT.shopLotteryDT.iFreeCD * 60 - (GameSocket.GetInstance().f_GetServerTime() - lotteryPoolDT.lastFreeTime);
        bool isFree = secondLeft < 0 ? true : false;
        f_GetObject("BtnBuyOneNorAdmiral").transform.Find("IconProp").gameObject.SetActive(!isFree);
        f_GetObject("BtnBuyOneNorAdmiral").transform.Find("LabelFree").gameObject.SetActive(isFree);

        ShopLotteryPoolDT lotteryPoolDT2 = Data_Pool.m_ShopLotteryPool.f_GetForId((long)EM_RecruitType.GenAd) as ShopLotteryPoolDT;
        int secondLeft2 = lotteryPoolDT2.shopLotteryDT.iFreeCD * 60 - (GameSocket.GetInstance().f_GetServerTime() - lotteryPoolDT2.lastFreeTime);
        bool isFree2 = secondLeft2 < 0 ? true : false;
        f_GetObject("BtnBuyOneGenAdmiral").transform.Find("IconProp").gameObject.SetActive(!isFree2);
        f_GetObject("BtnBuyOneGenAdmiral").transform.Find("LabelFree").gameObject.SetActive(isFree2);
    }
    /// <summary>
    /// 初始化商店抽牌数据
    /// </summary>
    /// <param name="obj"></param>
    private void InitShopLotteryInfo(GameObject BtnBuyOne, GameObject BtnBuyTen, EM_RecruitType recruitType)
    {
        ShopLotteryPoolDT lotteryPoolDT = Data_Pool.m_ShopLotteryPool.f_GetForId((long)recruitType) as ShopLotteryPoolDT;
        int AdCount = Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(GetReruitPropId(recruitType));
        //购买一次(当改道具数量小于1且又可以使用元宝购买时，显示元宝购买)
        UISprite IconSyceeBuyOne = BtnBuyOne.transform.Find("IconProp/IconSycee").GetComponent<UISprite>();
        if (AdCount < 1 && lotteryPoolDT.shopLotteryDT.szOnceCost2 != "0")
        {
            ResourceCommonDT OneCost2 = CommonTools.f_GetCommonResourceByResourceStr(lotteryPoolDT.shopLotteryDT.szOnceCost2);//元宝购买 ----------------待修改
            IconSyceeBuyOne.spriteName = "Icon_Sycee";// UITool.f_GetMoneySpriteName((EM_MoneyType)OneCost2.mResourceId);
            int num = OneCost2.mResourceNum;
            string ShowNum = num.ToString();
            BtnBuyOne.transform.Find("IconProp/Label").GetComponent<UILabel>().gameObject.SetActive(true);
            if (recruitType == EM_RecruitType.GenAd && lotteryPoolDT.totalHalfTimes <= 0)//抽次免费
            {
                num = OneCost2.mResourceNum / 2;
                ShowNum = num + CommonTools.f_GetTransLanguage(1117);
                BtnBuyOne.transform.Find("IconProp/Label").GetComponent<UILabel>().gameObject.SetActive(false);
            }
            if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee) < num)
                ShowNum = "[ec2626]" + ShowNum;
            else
                ShowNum = "[24ff00]" + ShowNum;
            BtnBuyOne.transform.Find("IconProp/LabelCount").GetComponent<UILabel>().text = ShowNum;
            
        }
        else
        {
            ResourceCommonDT OneCost1 = CommonTools.f_GetCommonResourceByResourceStr(lotteryPoolDT.shopLotteryDT.szOnceCost1);//道具购买
            IconSyceeBuyOne.spriteName = UITool.f_GetMoneySpriteName((EM_MoneyType)OneCost1.mResourceId);
            if (AdCount >= OneCost1.mResourceNum)
                BtnBuyOne.transform.Find("IconProp/LabelCount").GetComponent<UILabel>().text = "[24ff00]" + OneCost1.mResourceNum.ToString();
            else
                BtnBuyOne.transform.Find("IconProp/LabelCount").GetComponent<UILabel>().text = "[ec2626]" + OneCost1.mResourceNum.ToString();
            BtnBuyOne.transform.Find("IconProp/Label").GetComponent<UILabel>().gameObject.SetActive(true);
        }
        IconSyceeBuyOne.ResetAnchors();
        //IconSyceeBuyOne.MakePixelPerfect();
        IconSyceeBuyOne.transform.localScale = Vector3.one;
        //购买十次
        //购买十次次(当改道具数量小于1且又可以使用元宝购买时，显示元宝购买)
        UISprite IconSyceeBuyTen = BtnBuyTen.transform.Find("IconProp/IconSycee").GetComponent<UISprite>();
        if (AdCount < 10 && lotteryPoolDT.shopLotteryDT.szTenCost2 != "0")
        {
            ResourceCommonDT TenCost2 = CommonTools.f_GetCommonResourceByResourceStr(lotteryPoolDT.shopLotteryDT.szTenCost2); ;//元宝购买
            IconSyceeBuyTen.spriteName = "Icon_Sycee";// UITool.f_GetMoneySpriteName((EM_MoneyType)TenCost2.mResourceId);
            if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee) < TenCost2.mResourceNum)
                BtnBuyTen.transform.Find("IconProp/LabelCount").GetComponent<UILabel>().text = "[ec2626]" + TenCost2.mResourceNum.ToString();
            else
                BtnBuyTen.transform.Find("IconProp/LabelCount").GetComponent<UILabel>().text = "[24ff00]" + TenCost2.mResourceNum.ToString();
        }
        else
        {
            ResourceCommonDT TenCost1 = CommonTools.f_GetCommonResourceByResourceStr(lotteryPoolDT.shopLotteryDT.szTenCost1);//道具购买
            IconSyceeBuyTen.spriteName = UITool.f_GetMoneySpriteName((EM_MoneyType)TenCost1.mResourceId);
            if (AdCount >= TenCost1.mResourceNum)
                BtnBuyTen.transform.Find("IconProp/LabelCount").GetComponent<UILabel>().text = "[24ff00]" + TenCost1.mResourceNum.ToString();
            else
                BtnBuyTen.transform.Find("IconProp/LabelCount").GetComponent<UILabel>().text = "[ec2626]" + TenCost1.mResourceNum.ToString();
        }
        IconSyceeBuyTen.ResetAnchors();
        //IconSyceeBuyTen.MakePixelPerfect();
        IconSyceeBuyTen.transform.localScale = Vector3.one;
    }
    #endregion
    #region 按钮事件
    /// <summary>
    /// 点击 显示获取抽牌页面
    /// </summary>
    private void OnTouchGetCardPage(GameObject go, object obj1, object obj2)
    {
        f_GetObject("TextGetCardBg").SetActive(false);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN);
    }
    /// <summary>
    /// 购买一次按钮事件
    /// </summary>
    private void OnBtnBuyOneClick(GameObject go, object obj1, object obj2)
    {
        if (Data_Pool.m_CardPool.f_GetAll().Count > Data_Pool.m_RechargePool.f_GetCurLvVipPriValue(EM_VipPrivilege.eVip_GeneralBag))
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1118));
            return;
        }

        m_CurRecruitType = (EM_RecruitType)obj1;
        Data_Pool.m_CardPool.f_ClearlistLastAddCardTempID();
        ShopLotteryPoolDT lotteryPoolDT = Data_Pool.m_ShopLotteryPool.f_GetForId((long)m_CurRecruitType) as ShopLotteryPoolDT;
        ShopLotteryPool.BuyMode buyPara = new ShopLotteryPool.BuyMode();
        buyPara.em_buyMode = ShopLotteryPool.EM_BuyMode.BuyOne;
        int AdCount = Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(GetReruitPropId(m_CurRecruitType));
        if (AdCount < 1 && lotteryPoolDT.shopLotteryDT.szOnceCost2.Contains(";"))
        {
            buyPara.em_UserType = ShopLotteryPool.EM_UseType.Sycee;
        }
        else
        {
            buyPara.em_UserType = ShopLotteryPool.EM_UseType.Good;
        }
        //bool是否免费判断
        if ((GameSocket.GetInstance().f_GetServerTime() - lotteryPoolDT.lastFreeTime) > (lotteryPoolDT.shopLotteryDT.iFreeCD * 60))
        {
            Debug.Log(CommonTools.f_GetTransLanguage(1119));
            UITool.f_OpenOrCloseWaitTip(true);
            Data_Pool.m_ShopLotteryPool.f_Buy(true, m_CurRecruitType, buyPara, ReqeustBuyShopLotteryCallback);
            return;
        }
        switch (buyPara.em_UserType)
        {
            case ShopLotteryPool.EM_UseType.Good:
                if (AdCount < int.Parse(lotteryPoolDT.shopLotteryDT.szOnceCost1.Split(';')[2]))
                {
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, UITool.f_GetGoodName(EM_ResourceType.Good, GetReruitPropId(m_CurRecruitType)) + "not enough！");
                    return;
                }
                break;
            case ShopLotteryPool.EM_UseType.Sycee:
                ResourceCommonDT OneCost2 = CommonTools.f_GetCommonResourceByResourceStr(lotteryPoolDT.shopLotteryDT.szOnceCost2);
                int syceeCount = OneCost2.mResourceNum;
                if (m_CurRecruitType == EM_RecruitType.GenAd && lotteryPoolDT.totalHalfTimes <= 0)//半价抽牌
                    syceeCount /= 2;
                if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee) < syceeCount)
                {
                    string tipContent = string.Format(CommonTools.f_GetTransLanguage(1120));
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.PayTipPage, UIMessageDef.UI_OPEN, tipContent);
                    return;
                }
                break;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_ShopLotteryPool.f_Buy(false, m_CurRecruitType, buyPara, ReqeustBuyShopLotteryCallback);
    }
    /// <summary>
    /// 购买十次按钮事件
    /// </summary>
    private void OnBtnBuyTenClick(GameObject go, object obj1, object obj2)
    {
        if (Data_Pool.m_CardPool.f_GetAll().Count > Data_Pool.m_RechargePool.f_GetCurLvVipPriValue(EM_VipPrivilege.eVip_GeneralBag))
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1118));
            return;
        }
        m_CurRecruitType = (EM_RecruitType)obj1;
        Data_Pool.m_CardPool.f_ClearlistLastAddCardTempID();
        ShopLotteryPoolDT lotteryPoolDT = Data_Pool.m_ShopLotteryPool.f_GetForId((long)m_CurRecruitType) as ShopLotteryPoolDT;
        ShopLotteryPool.BuyMode buyPara = new ShopLotteryPool.BuyMode();
        buyPara.em_buyMode = ShopLotteryPool.EM_BuyMode.BuyTen;
        int AdCount = Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(GetReruitPropId(m_CurRecruitType));
        if (AdCount < 10 && lotteryPoolDT.shopLotteryDT.szTenCost2.Contains(";"))
        {
            buyPara.em_UserType = ShopLotteryPool.EM_UseType.Sycee;
        }
        else
        {
            buyPara.em_UserType = ShopLotteryPool.EM_UseType.Good;
        }
        switch (buyPara.em_UserType)
        {
            case ShopLotteryPool.EM_UseType.Good:
                if (AdCount < int.Parse(lotteryPoolDT.shopLotteryDT.szTenCost1.Split(';')[2]))
                {
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, UITool.f_GetGoodName(EM_ResourceType.Good, GetReruitPropId(m_CurRecruitType)) + "not enough！");
                    return;
                }
                break;
            case ShopLotteryPool.EM_UseType.Sycee:
                if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee) < int.Parse(lotteryPoolDT.shopLotteryDT.szTenCost2.Split(';')[2]))
                {
                    string tipContent = string.Format(CommonTools.f_GetTransLanguage(1120));
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.PayTipPage, UIMessageDef.UI_OPEN, tipContent);
                    return;
                }
                break;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_ShopLotteryPool.f_Buy(false, m_CurRecruitType, buyPara, ReqeustBuyShopLotteryCallback);
    }

    private void Btn_NorShow(GameObject go, object obj1, object obj2)
    {
        CardShowParam tCardShowParam = new CardShowParam(m_UiBase, EM_RecruitType.NorAd);

        ccUIManage.GetInstance().f_SendMsg(UINameConst.CardShowPage, UIMessageDef.UI_OPEN, tCardShowParam);
    }
    private void Btn_GenShow(GameObject go, object obj1, object obj2)
    {
        CardShowParam tCardShowParam = new CardShowParam(m_UiBase, EM_RecruitType.GenAd);

        ccUIManage.GetInstance().f_SendMsg(UINameConst.CardShowPage, UIMessageDef.UI_OPEN, tCardShowParam);

    }

    private void Btn_CampShow(GameObject go, object obj1, object obj2)
    {
        CardShowParam tCardShowParam = new CardShowParam(m_UiBase, EM_RecruitType.CampAd);

        ccUIManage.GetInstance().f_SendMsg(UINameConst.CardShowPage, UIMessageDef.UI_OPEN, tCardShowParam);

    }
    #endregion

    #region 红点提示
    protected override void InitRaddot()
    {
        base.InitRaddot();

        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.NorAdmiralFree, f_GetObject("BtnBuyOneNorAdmiral"), ReddotCallback_Show_Btn_NorAdmiral);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.GenAdmiralFree, f_GetObject("BtnBuyOneGenAdmiral"), ReddotCallback_Show_Btn_GenAdmiral);
        UpdateReddotUI();
    }
    protected override void UpdateReddotUI()
    {
        base.UpdateReddotUI();
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.NorAdmiralFree);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.GenAdmiralFree);
    }
    protected override void On_Destory()
    {
        base.On_Destory();
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.NorAdmiralFree, f_GetObject("BtnBuyOneNorAdmiral"));
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.GenAdmiralFree, f_GetObject("BtnBuyOneGenAdmiral"));
    }
    private void ReddotCallback_Show_Btn_NorAdmiral(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnRecruit = f_GetObject("BtnBuyOneNorAdmiral");
        UITool.f_UpdateReddot(BtnRecruit, iNum, new Vector3(150, 40, 0), 102);
    }
    private void ReddotCallback_Show_Btn_GenAdmiral(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnRecruit = f_GetObject("BtnBuyOneGenAdmiral");
        UITool.f_UpdateReddot(BtnRecruit, iNum, new Vector3(150, 40, 0), 102);
    }
    #endregion
    private void OnBtnBuyCampClick(GameObject go, object obj1, object obj2)
    {
        m_CurRecruitType = (EM_RecruitType)obj1;
        int typ = (int)obj2;
        Data_Pool.m_CardPool.f_ClearlistLastAddCardTempID();
        ShopLotteryPoolDT lotteryGenPoolDT = Data_Pool.m_ShopLotteryPool.f_GetForId((long)EM_RecruitType.GenAd) as ShopLotteryPoolDT;
        ShopLotteryPoolDT lotteryPoolDT = Data_Pool.m_ShopLotteryPool.f_GetForId((long)m_CurRecruitType) as ShopLotteryPoolDT;
        if (lotteryGenPoolDT.totalTimes < lotteryPoolDT.shopLotteryDT.iOpenNum)
        {
            //UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1118));
            UITool.Ui_Trip("Yêu cầu Chiêu mộ cao cấp " + lotteryPoolDT.shopLotteryDT.iOpenNum + " lần");
            return;
        }
        // xử lý check điều kiện quay
        ShopLotteryPool.BuyMode buyPara = new ShopLotteryPool.BuyMode();
        int AdCount = Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(GetReruitPropId(m_CurRecruitType));
        
        if (typ == 1)
        {
            buyPara.em_buyMode = ShopLotteryPool.EM_BuyMode.BuyOne;
            if (AdCount < 1 && lotteryPoolDT.shopLotteryDT.szOnceCost2.Contains(";"))
            {
                buyPara.em_UserType = ShopLotteryPool.EM_UseType.Sycee;
            }
            else
            {
                buyPara.em_UserType = ShopLotteryPool.EM_UseType.Good;
            }
        }
        else
        {
            buyPara.em_buyMode = ShopLotteryPool.EM_BuyMode.BuyTen;
            if (AdCount < 10 && lotteryPoolDT.shopLotteryDT.szTenCost2.Contains(";"))
            {
                buyPara.em_UserType = ShopLotteryPool.EM_UseType.Sycee;
            }
            else
            {
                buyPara.em_UserType = ShopLotteryPool.EM_UseType.Good;
            }
        }
        
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_ShopLotteryPool.f_CampBuy(m_CurRecruitType, buyPara, ReqeustBuyShopLotteryCallback);
    }
    private void OnBtnGenChooseCloseClick(GameObject go, object obj1, object obj2)
    {
        m_CurRecruitType = (EM_RecruitType)obj1;
        showChooseConent = false;
        f_GetObject("GenChooseConent").SetActive(showChooseConent);
    }
    private void OnBtnGenChooseClick(GameObject go, object obj1, object obj2)
    {
        m_CurRecruitType = (EM_RecruitType)obj1;
        int index = mGenSelectKey;
        if (index <= 0) return;
        //sele
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_ShopLotteryPool.f_Choose(m_CurRecruitType, index, ReqeustChooseShopLotteryCallback);

    }
    private void OnBtnCampChooseClick(GameObject go, object obj1, object obj2)
    {
        m_CurRecruitType = (EM_RecruitType)obj1;
        int camp = (int)obj2;
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_ShopLotteryPool.f_Choose(m_CurRecruitType, camp, ReqeustChooseShopLotteryCallback);
        
    }
    private void OnBtnGetAwardClick(GameObject go, object obj1, object obj2)
    {
        m_CurRecruitType = (EM_RecruitType)obj1;
        ShopLotteryPoolDT lotteryPoolDT = Data_Pool.m_ShopLotteryPool.f_GetForId((long)m_CurRecruitType) as ShopLotteryPoolDT;
        // kiểm tra trước khi gửi
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_ShopLotteryPool.f_GetAward(m_CurRecruitType, ReqeustGetAwardShopLotteryCallback);
    }
    private bool showChooseConent = false;

    private void OnBtnChooseShowClick(GameObject go, object obj1, object obj2)
    {
        m_CurRecruitType = (EM_RecruitType)obj1;
        ShopLotteryPoolDT lotteryPoolDT = Data_Pool.m_ShopLotteryPool.f_GetForId((long)m_CurRecruitType) as ShopLotteryPoolDT;
        ShopLotteryPoolDT lotteryGenPoolDT = Data_Pool.m_ShopLotteryPool.f_GetForId((long)EM_RecruitType.GenAd) as ShopLotteryPoolDT;
        if (lotteryPoolDT.shopLotteryDT.iChoose == 0) // vòng quay không cho phép quay tùy chọn
        {
            return;
        }
        if (lotteryGenPoolDT.totalTimes < lotteryPoolDT.shopLotteryDT.iOpenNum) // yêu cầu quay 50 lần 
        {
            UITool.Ui_Trip("Yêu cầu Chiêu mộ cao cấp " + lotteryPoolDT.shopLotteryDT.iOpenNum + " lần");
            return;
        }
        if (lotteryPoolDT.iLock > 0 && m_CurRecruitType == EM_RecruitType.GenAd)
        {
            UITool.Ui_Trip("Mỗi ngày chỉ có thể chọn 1 lần");
            return;
        }
        showChooseConent = !showChooseConent;
        if (m_CurRecruitType == EM_RecruitType.GenAd)
        {
            f_GetObject("GenChooseConent").SetActive(showChooseConent);
            // xử lý show choose
            mGenSelectKey = lotteryPoolDT.itemId;
            OnTapItemClick(null, EM_CardCamp.eCardMain, null);
            mAwardShowComponent.f_ResetView();
        }
        else
        {
            f_GetObject("CampChooseConent").SetActive(showChooseConent);
            UpdatePosBtn();
        }
        
    }
    private void OnRequestChooseLotteryBuySucCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        RefreshUI(m_UiBase, (int)eM_PageIndex);
        f_GetObject("CampChooseConent").SetActive(false);
        UITool.Ui_Trip("Thành công");
    }
    private void OnRequestChooseLotteryBuyFailCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        RefreshUI(m_UiBase, (int)eM_PageIndex);
        f_GetObject("CampChooseConent").SetActive(false);
        UITool.Ui_Trip("Thất bại");
    }
    private void OnRequestGetAwardLotteryBuySucCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UpdateRewardContent();
        UITool.Ui_Trip("Thành công");
    }
    private void OnRequestGetAwardLotteryBuyFailCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UpdateRewardContent();
        UITool.Ui_Trip("Thất bại");
    }

    private void OnBtnCampShopClick(GameObject go, object obj1, object obj2)
    {
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopMutiCommonPage, UIMessageDef.UI_OPEN, EM_ShopMutiType.CampGemShop);
    }
}
