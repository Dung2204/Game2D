using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class CardBagPage : UIFramwork
{
    private CardBagType _pageType;

    private GameObject _cardDragObj;
    private GameObject _cardItemParent;
    private GameObject _cardItem;
    private UIWrapComponent _cardWrapComponet;
    public UIWrapComponent mCardWrapComponet
    {
        get
        {
            if (_cardWrapComponet == null)
            {
                _cardList = Data_Pool.m_CardPool.f_GetAll();
                _cardWrapComponet = new UIWrapComponent(186, 2, 650, 5, _cardItemParent, _cardItem, _cardList, CardItemUpdateByInfo, CardItemClickHandle);
            }
            Data_Pool.m_CardPool.f_SortList();
            return _cardWrapComponet;
        }
    }
    private List<BasePoolDT<long>> _cardList;

    private GameObject _fragmentDragObj;
    private GameObject _fragmentItemParent;
    private GameObject _fragmentItem;
    private UIWrapComponent _fragmentWrapComponet;
    public UIWrapComponent mFragmentWrapComponet
    {
        get
        {

            if (_fragmentWrapComponet == null)
            {
                _fragmentList = Data_Pool.m_CardFragmentPool.f_GetAll();
                _fragmentWrapComponet = new UIWrapComponent(186, 2, 650, 5, _fragmentItemParent, _fragmentItem, _fragmentList, FragmentItemUpdateByInfo, FragmentItemClickHandle);
            }
            Data_Pool.m_CardFragmentPool.f_SortList();
            return _fragmentWrapComponet;
        }
    }
    private List<BasePoolDT<long>> _fragmentList;
    private GameObject _btnBack;
    private UIToggle _cardToggle;
    private UIToggle _fragmentToggle;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        _cardDragObj = f_GetObject("CardDragObj");
        _cardItemParent = f_GetObject("CardItemParent");
        _cardItem = f_GetObject("CardItem");
        _fragmentDragObj = f_GetObject("FragmentDragObj");
        _fragmentItemParent = f_GetObject("FragmentItemParent");
        _fragmentItem = f_GetObject("FragmentItem");
        _btnBack = f_GetObject("BackBtn");
        _cardToggle = f_GetObject("Btn_CardPage").GetComponent<UIToggle>();
        _fragmentToggle = f_GetObject("Btn_FragmentPage").GetComponent<UIToggle>();
        f_RegClickEvent(_btnBack, BagPageBackBtnHandle);
        EventDelegate.Add(_cardToggle.onChange, CardToggleChange);
        EventDelegate.Add(_fragmentToggle.onChange, FragmentToggleChange);
    }

    //卡牌选项选中处理
    private void CardToggleChange()
    {
        _cardDragObj.SetActive(_cardToggle.value);
        if (_cardToggle.value)
        {
            f_GetObject("BagCapacity").SetActive(true);
            _pageType = CardBagType.Card;
            mCardWrapComponet.f_ResetView();
            f_GetObject("NoHaveCard").SetActive(_cardList.Count == 0);
            f_GetObject("NoHaveFragment").SetActive(false);
        }

    }

    //碎片选项选中处理
    private void FragmentToggleChange()
    {
        _fragmentDragObj.SetActive(_fragmentToggle.value);
        if (_fragmentToggle.value)
        {
            f_GetObject("BagCapacity").SetActive(false);
            _pageType = CardBagType.Fragment;
            mFragmentWrapComponet.f_ResetView();
            f_GetObject("NoHaveFragment").SetActive(_fragmentList.Count == 0);
            f_GetObject("NoHaveCard").SetActive(false);
        }
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.CardFragmentBagNewGoods);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        Debug.LogWarning("CardBagPageOpen");
        if (e != null)
        {
            //根据传参数 打开不同界面
        }
        else
        {
            _pageType = CardBagType.Card;
            _cardToggle.value = true;
            Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.CardBagNewGoods);
            mCardWrapComponet.f_UpdateList(Data_Pool.m_CardPool.f_GetAll());
            mCardWrapComponet.f_ResetView();
        }
        f_GetObject("NoHaveCard").SetActive(false);
        f_GetObject("NoHaveFragment").SetActive(false);
        UpdateInfo();
        InitMoneyUI();
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
    }

    protected override void UI_HOLD(object e)
    {
        base.UI_HOLD(e);
    }

    /// <summary>
    /// unhold 的时候 刷新下界面
    /// </summary>
    /// <param name="e"></param>
    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        if (_pageType == CardBagType.Card)
        {
            mCardWrapComponet.f_UpdateView();
        }
        else if (_pageType == CardBagType.Fragment)
        {
            mFragmentWrapComponet.f_UpdateView();
        }
        UpdateInfo();
    }

    protected override void f_InitMessage()
    {
        base.f_InitMessage();
    }
    #region 红点提示
    protected override void InitRaddot()
    {
        base.InitRaddot();
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.CardFragmentBagNewGoods, f_GetObject("Btn_FragmentPage"), ReddotCallback_Show_BtnFragment, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.CardBagNewGoods, f_GetObject("Btn_CardPage"), ReddotCallback_Show_BtnCard, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.CardBag_Fragment, f_GetObject("Btn_FragmentPage"), ReddotCallback_Show_BtnFragment, true);
        UpdateReddotUI();
    }
    protected override void On_Destory()
    {
        base.On_Destory();
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.CardBagNewGoods, f_GetObject("Btn_CardPage"));
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.CardFragmentBagNewGoods, f_GetObject("Btn_FragmentPage"));
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.CardBag_Fragment, f_GetObject("Btn_FragmentPage"));
    }
    protected override void UpdateReddotUI()
    {
        base.UpdateReddotUI();
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.CardFragmentBagNewGoods);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.CardBagNewGoods);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.CardBag_Fragment);
    }
    private void ReddotCallback_Show_BtnFragment(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnFragment = f_GetObject("Btn_FragmentPage");
        UITool.f_UpdateReddot(BtnFragment, iNum, new Vector3(110, -20, 0), 74);
    }
    private void ReddotCallback_Show_BtnCard(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnFragment = f_GetObject("Btn_CardPage");
        UITool.f_UpdateReddot(BtnFragment, iNum, new Vector3(110, -20, 0), 74);
    }
    #endregion

    /// <summary>
    /// 初始化金钱UI
    /// </summary>
    private void InitMoneyUI()
    {
        List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
        listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Money);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Energy);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, listMoneyType);
    }
    /// <summary>
    /// 更新货币信息
    /// </summary>
    private void UpdateInfo()
    {
        int BagNow = Data_Pool.m_CardPool.f_GetAll().Count;
        int BagUp = Data_Pool.m_RechargePool.f_GetVipPriValue(EM_VipPrivilege.eVip_GeneralBag, UITool.f_GetNowVipLv());
        // f_GetObject("BagCapacity").transform.Find("Label").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(211)+"[{0}]{1}/{2}", BagNow > BagUp ? "ef3612" : "27cc42", BagNow, BagUp);
		f_GetObject("BagCapacity").transform.Find("Label").GetComponent<UILabel>().text = string.Format("[{0}]{1}/{2}", BagNow > BagUp ? "ef3612" : "27cc42", BagNow, BagUp);
    }

    //关闭处理
    private void BagPageBackBtnHandle(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CardBagPage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
    }

    //卡牌Item更新
    private void CardItemUpdateByInfo(Transform item, BasePoolDT<long> dt)
    {
        CardPoolDT cardDT = dt as CardPoolDT;
        if (cardDT.m_iTempleteId == (int)EM_CardType.CaiWenji)
        {
            item.name = "Caiwenji";
        }
        else if (cardDT.m_iTempleteId == (int)EM_CardType.HuangYueYing)
        {
            item.name = "Huangyueying";
        }
        else if (cardDT.m_CardDT.iCardType == (int)EM_CardType.RoleCard)
        {
            item.name = "Zhu";
        }


        UILabel tmpName = item.Find("Name").GetComponent<UILabel>();
        UILabel tmpQuality = item.Find("QualityTitle/Quality").GetComponent<UILabel>(); //天命   
        UILabel tmpLevel = item.Find("LevelTitle/Level").GetComponent<UILabel>();    //等级
        UILabel tmpAwaken = item.Find("AwakenLv/Level").GetComponent<UILabel>();    //领悟等级
        UISprite tmpCase = item.Find("Case").GetComponent<UISprite>();
        UI2DSprite tmpIcon = item.Find("Icon").GetComponent<UI2DSprite>();

        ResourceCommonDT commonDT = new ResourceCommonDT();
        commonDT.f_UpdateInfo((byte)EM_ResourceType.Card, cardDT.m_iTempleteId, 0);
        f_RegClickEvent(tmpIcon.gameObject, UITool.f_OnItemIconClick, commonDT);

        UISprite tmpTab = item.Find("Tab").GetComponent<UISprite>();
        Transform tStarParent = item.Find("star");
        GameObject tInTeam = item.Find("InTeam").gameObject;
        int evolveLv = UITool.f_GetEvolveLv(cardDT.m_iEvolveId);
        string name = string.Empty;
        if (evolveLv > 0)
            name = string.Format("{0}+{1}", UITool.f_GetCardName(cardDT.m_CardDT), evolveLv);
        else
            name = UITool.f_GetCardName(cardDT.m_CardDT);
        tmpCase.spriteName = UITool.f_GetImporentColorName(cardDT.m_CardDT.iImportant, ref name);
        tmpName.text = name;
        tmpQuality.text = cardDT.uSkyDestinyLv.ToString();
        tmpLevel.text = cardDT.m_iLv.ToString();
        tmpIcon.sprite2D = UITool.f_GetIconSpriteByCardId(cardDT);
        tInTeam.SetActive(Data_Pool.m_TeamPool.f_CheckInTeamByKeyId(dt.iId));
        bool isOpenAwaken = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) >= UITool.f_GetSysOpenLevel(EM_NeedLevel.CardAwaken);
        tmpAwaken.transform.parent.gameObject.SetActive(isOpenAwaken);
        if (isOpenAwaken)    //开启领悟
        {
            tStarParent.gameObject.SetActive(true);
            if (cardDT.m_iLvAwaken > 0)
                tmpAwaken.text = string.Format(CommonTools.f_GetTransLanguage(212), cardDT.m_iLvAwaken / 10, cardDT.m_iLvAwaken % 10);
            else
                tmpAwaken.text = string.Format(CommonTools.f_GetTransLanguage(218), "0");
            UISprite[] tStar = new UISprite[6];
            for (int i = 0; i < item.Find("star").childCount; i++)
                tStar[i] = tStarParent.GetChild(i).GetComponent<UISprite>();
            UITool.f_UpdateStarNum(tStar, cardDT.m_iLvAwaken / 10, "Icon_RMStar_4", "Icon_RMStar_3");
        }
        else
            tStarParent.gameObject.SetActive(false);

        if (Data_Pool.m_TeamPool.f_CheckInTeamByKeyId(cardDT.iId))   //当前出战卡牌, 
            tmpTab.spriteName = "icon_yisz";
        else if (Data_Pool.m_TeamPool.dicReinforceCardId.ContainsValue(cardDT))
            tmpTab.spriteName = "icon_xihb";
        else
            tmpTab.spriteName = "";
    }

    //卡牌Item点击
    private void CardItemClickHandle(Transform item, BasePoolDT<long> dt)
    {
        CardPoolDT cardDT = dt as CardPoolDT;
        MessageBox.DEBUG(string.Format(CommonTools.f_GetTransLanguage(213)+"templateId:{0} , Name:{1}", cardDT.m_iTempleteId, cardDT.m_CardDT.szName));
        CardBox tmp = new CardBox();
        tmp.m_Card = cardDT;
        tmp.m_bType = CardBox.BoxType.Intro;
        tmp.m_oType = CardBox.OpenType.Bag;
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CardProperty, UIMessageDef.UI_OPEN, tmp);
    }

    //碎片Item更新
    private void FragmentItemUpdateByInfo(Transform item, BasePoolDT<long> dt)
    {
        CardFragmentPoolDT fragmentDT = dt as CardFragmentPoolDT;
        UILabel tmpName = item.Find("Name").GetComponent<UILabel>();
        UILabel tmpNum = item.Find("NumTitle/Num").GetComponent<UILabel>();
        UI2DSprite tmpIcon = item.Find("Icon").GetComponent<UI2DSprite>();

        ResourceCommonDT commonDT = new ResourceCommonDT();
        commonDT.f_UpdateInfo((byte)EM_ResourceType.CardFragment, fragmentDT.m_iTempleteId, 0);
        f_RegClickEvent(tmpIcon.gameObject, UITool.f_OnItemIconClick, commonDT);

        UISprite tmpCase = item.Find("Case").GetComponent<UISprite>();
        GameObject tmpSynthesisBtn = item.Find("BtnSynthesis").gameObject;
        GameObject tmpGetBtn = item.Find("BtnGet").gameObject;
        GameObject tmpLackOfTip = item.Find("LackOfTip").gameObject;
        int haveNum = fragmentDT.m_iNum;
        int needNum = fragmentDT.m_CardFragmentDT.iNeedNum;
        string name = fragmentDT.m_CardFragmentDT.szName;
        tmpCase.spriteName = UITool.f_GetImporentColorName(fragmentDT.m_CardFragmentDT.iImportant, ref name);
        tmpName.text = name;
        tmpNum.text = string.Format("{0}/{1}", haveNum, needNum);
        tmpIcon.sprite2D = UITool.f_GetIconSpriteByCardId(fragmentDT.m_CardFragmentDT.iNewCardId);
        tmpLackOfTip.SetActive(haveNum < needNum);
        tmpGetBtn.SetActive(haveNum < needNum);
        tmpSynthesisBtn.SetActive(haveNum >= needNum);
        f_RegClickEvent(tmpSynthesisBtn, FragmentItemSynthesisBtnHandle, dt);
        f_RegClickEvent(tmpGetBtn, FragmentItemGetBtnHandle, dt);
    }

    //碎片Item点击
    private void FragmentItemClickHandle(Transform item, BasePoolDT<long> dt)
    {
        CardFragmentPoolDT fragmentDT = dt as CardFragmentPoolDT;
        MessageBox.DEBUG(string.Format(CommonTools.f_GetTransLanguage(214)+",templateId:{0} Name:{1}", fragmentDT.m_iTempleteId, fragmentDT.m_CardFragmentDT.szName));
    }

    private int _curSynthesisId;
    /// <summary>
    /// 碎片Item合成按钮
    /// </summary>
    private void FragmentItemSynthesisBtnHandle(GameObject go, object obj1, object obj2)
    {
        CardFragmentPoolDT fragmentDT = obj1 as CardFragmentPoolDT;
        MessageBox.DEBUG(string.Format(CommonTools.f_GetTransLanguage(215)+"templateId:{0} , Name:{1}", fragmentDT.m_iTempleteId, fragmentDT.m_CardFragmentDT.szName));
        //发送合成协议
        _curSynthesisId = fragmentDT.m_iTempleteId;
        int haveNum = fragmentDT.m_iNum;
        int needNum = fragmentDT.m_CardFragmentDT.iNeedNum;
        if (haveNum >= needNum * 2)
        {
            MutiOperateParam tMutiOperare = new MutiOperateParam("wj_hc_font_hc", EM_ResourceType.CardFragment, fragmentDT.m_CardFragmentDT.iId, haveNum, haveNum / needNum, "", BatchSynthesis);

            ccUIManage.GetInstance().f_SendMsg(UINameConst.MutiOperatePage, UIMessageDef.UI_OPEN, tMutiOperare);
        }
        else
        {
            UITool.f_OpenOrCloseWaitTip(true);
            SocketCallbackDT callBackDT = new SocketCallbackDT();
            callBackDT.m_ccCallbackSuc = Synthesis_Suc;
            callBackDT.m_ccCallbackFail = Synthesis_Fail;
            Data_Pool.m_CardFragmentPool.f_Synthesis(_curSynthesisId, 1, callBackDT);
        }
    }
    private int SynthesisJudge = 0;
    private int BatchNum = 0;
    /// <summary>
    /// 批量合成
    /// </summary>
    public void BatchSynthesis(long iId, EM_ResourceType type, int resourceId, int resourceCount, int UseCount)
    {
        SynthesisJudge = 0;
        BatchNum = UseCount;
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT callBackDT = new SocketCallbackDT();
        callBackDT.m_ccCallbackSuc = Synthesis_Suc;
        callBackDT.m_ccCallbackFail = Synthesis_Fail;
        Data_Pool.m_CardFragmentPool.f_Synthesis(_curSynthesisId, UseCount, callBackDT);


    }
    /// <summary>
    /// 碎片Item获取按钮
    /// </summary>
    private void FragmentItemGetBtnHandle(GameObject go, object obj1, object obj2)
    {
        CardFragmentPoolDT fragmentDT = obj1 as CardFragmentPoolDT;
        StaticValue.mGetWayToBattleParam.f_UpdateDataInfo(EM_GetWayToBattle.CardBag, EM_ResourceType.CardFragment, fragmentDT.m_iTempleteId);
        GetWayPageParam param = new GetWayPageParam(EM_ResourceType.CardFragment, fragmentDT.m_iTempleteId, this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, param);
    }

    private void Synthesis_Suc(object node)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        SynthesisJudge++;
        eMsgOperateResult result = (eMsgOperateResult)node;
        ccUIHoldPool.GetInstance().f_Hold(this);
        EquipSythesis tCardSynthesis = new EquipSythesis(_curSynthesisId, BatchNum, EquipSythesis.ResonureType.Card);
        tCardSynthesis.isShowBotton = false;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainEquipShowPage, UIMessageDef.UI_OPEN, tCardSynthesis);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.MutiOperatePage, UIMessageDef.UI_CLOSE);

    }

    private void Synthesis_Fail(object node)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult result = (eMsgOperateResult)node;
        MessageBox.ASSERT(CommonTools.f_GetTransLanguage(217) + result.ToString());
    }
    private void UI_OpenCard(GameObject go, object obj1, object obj2)
    {
        //通知HoldPool保存当前页
        ccUIHoldPool.GetInstance().f_Hold(this);
        //通知ccUIManager打开BuildPage页
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CardProperty, UIMessageDef.UI_OPEN, obj1);
    }
}
