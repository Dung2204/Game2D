using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class GodEquipBagPage : UIFramwork
{
    private GodEquipBagType _pageType;

    private GameObject _cardDragObj;
    private GameObject _cardItemParent;
    private GameObject _cardItem;
    private UIWrapComponent _cardWrapComponet;
    public UIWrapComponent mCardWrapComponet
    {
        get
        {
            _GodEquipList = Data_Pool.m_GodEquipPool.f_GetAll();

            if(_cardWrapComponet == null)
            {
                _cardWrapComponet = new UIWrapComponent(186, 2, 650, 5, _cardItemParent, _cardItem, _GodEquipList, CardItemUpdateByInfo, CardItemClickHandle);
            }
            Data_Pool.m_GodEquipPool.f_SortList();
            return _cardWrapComponet;
        }
    }
    private List<BasePoolDT<long>> _GodEquipList;
    private void _Sort()
    {
        List<BasePoolDT<long>> EquipList = new List<BasePoolDT<long>>();
        _GodEquipList.Sort((BasePoolDT<long> a, BasePoolDT<long> b) => { return ((GodEquipPoolDT)a).m_EquipDT.iColour > ((GodEquipPoolDT)a).m_EquipDT.iColour ? -1 : 1; });
        foreach (GodEquipPoolDT item in _GodEquipList)
        {
            if (UITool.f_GetHowEquip(item.iId) != "")
            {
                EquipList.Add(item);
            }
        }
        foreach (GodEquipPoolDT item in EquipList)
        {
            if (_GodEquipList.Contains(item))
            {
                _GodEquipList.Remove(item);
            }
        }
        List<BasePoolDT<long>> tmpList = new List<BasePoolDT<long>>();
        List<BasePoolDT<long>> tmpList2 = new List<BasePoolDT<long>>();
        for (int i = 0; i < _GodEquipList.Count; i++)
        {
            if (i + 1 == _GodEquipList.Count)
            {
                if (!tmpList.Contains(_GodEquipList[i]))
                    tmpList.Add(_GodEquipList[i]);
                tmpList2.AddRange(tmpList);
                break;
            }
            if (((GodEquipPoolDT)_GodEquipList[i]).m_EquipDT.iColour == ((GodEquipPoolDT)_GodEquipList[i + 1]).m_EquipDT.iColour)
            {
                if (!tmpList.Contains(_GodEquipList[i]))
                    tmpList.Add(_GodEquipList[i]);
                if (!tmpList.Contains(_GodEquipList[i + 1]))
                    tmpList.Add(_GodEquipList[i + 1]);
            }
            else
            {
                if (!tmpList.Contains(_GodEquipList[i]))
                    tmpList.Add(_GodEquipList[i]);
                tmpList.Sort((BasePoolDT<long> a, BasePoolDT<long> b) => { return ((GodEquipPoolDT)a).m_lvIntensify > ((GodEquipPoolDT)b).m_lvIntensify ? -1 : 1; });
                tmpList.Sort((BasePoolDT<long> a, BasePoolDT<long> b) => { return ((GodEquipPoolDT)a).m_lvRefine >= ((GodEquipPoolDT)b).m_lvRefine ? -1 : 1; });
                tmpList2.AddRange(tmpList);
                tmpList.Clear();
            }
        }
        EquipList.AddRange(tmpList2);
        _GodEquipList = EquipList;
    }
    private GameObject _fragmentDragObj;
    private GameObject _fragmentItemParent;
    private GameObject _fragmentItem;
    private UIWrapComponent _fragmentWrapComponet;
    // chưa
    public UIWrapComponent mFragmentWrapComponet
    {
        get
        {
            _fragmentList = Data_Pool.m_GodEquipFragmentPool.f_GetAll();
            if (_fragmentWrapComponet == null)
            {
                _fragmentWrapComponet = new UIWrapComponent(186, 2, 650, 5, _fragmentItemParent, _fragmentItem, _fragmentList, FragmentItemUpdateByInfo, FragmentItemClickHandle);
            }
            Data_Pool.m_GodEquipFragmentPool._Sort();
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
        f_RegClickEvent("Btn_CardPage", CardToggleChange, 1);
        f_RegClickEvent("Btn_FragmentPage", CardToggleChange, 2);
    }

    #region 红点提示
//chưa
    protected override void InitRaddot()
    {
        base.InitRaddot();
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.GodEquipBagNewGoods, f_GetObject("Btn_CardPage"), ReddotCallback_Show_Btn_EquipPage, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.GodEquipFragmentBagNewGoods, f_GetObject("Btn_FragmentPage"), ReddotCallback_Show_Btn_FragmentPage, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.GodEquipBag_Fragment, f_GetObject("Btn_FragmentPage"), ReddotCallback_Show_Btn_FragmentPage, true);
        UpdateReddotUI();
    }
    protected override void On_Destory()
    {
        base.On_Destory();
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.GodEquipBagNewGoods, f_GetObject("Btn_CardPage"));
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.GodEquipFragmentBagNewGoods, f_GetObject("Btn_FragmentPage"));
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.GodEquipBag_Fragment, f_GetObject("Btn_FragmentPage"));
    }
    protected override void UpdateReddotUI()
    {
        base.UpdateReddotUI();
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.GodEquipFragmentBagNewGoods);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.GodEquipBagNewGoods);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.GodEquipBag_Fragment);
    }

    private void ReddotCallback_Show_Btn_FragmentPage(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnTask = f_GetObject("Btn_FragmentPage");
        UITool.f_UpdateReddot(BtnTask, iNum, new Vector3(110, -20, 0), 74);
    }
    private void ReddotCallback_Show_Btn_EquipPage(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnTask = f_GetObject("Btn_CardPage");
        UITool.f_UpdateReddot(BtnTask, iNum, new Vector3(110, -20, 0), 74);
    }
    #endregion

    //卡牌选项选中处理
    private void CardToggleChange(GameObject go, object obj, object obj2)
    {
        int tmp = (int)obj;
        _cardDragObj.SetActive(tmp == 1);
        _fragmentDragObj.SetActive(tmp == 2);
        if (tmp == 2)
        {
            f_GetObject("BagCapacity").SetActive(false);
            _pageType = GodEquipBagType.Fragment;
            mFragmentWrapComponet.f_ResetView();
            f_GetObject("NoHaveEquipFragment").SetActive(_fragmentList.Count == 0);
            f_GetObject("NoHaveEquip").SetActive(false);
            Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.GodEquipFragmentBagNewGoods);
        }
        else
        {
            f_GetObject("BagCapacity").SetActive(true);
            _pageType = GodEquipBagType.Equip;
            mCardWrapComponet.f_ResetView();
            f_GetObject("NoHaveEquip").SetActive(_GodEquipList.Count == 0);
            f_GetObject("NoHaveEquipFragment").SetActive(false);
            //Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.GodEquipBagNewGoods);

        }
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (e != null)
        {
            //Mở các giao diện khác nhau theo các tham số
        }
        else
        {
            _pageType = GodEquipBagType.Equip;
            _cardToggle.value = true;
            //check lại nút của thần binh
            Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.GodEquipBagNewGoods);
            CardToggleChange(null, 1, null);
            mCardWrapComponet.f_ResetView();
        }
        UpdateViewInfo();
        InitMoneyUI();
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
    }

    protected override void UI_HOLD(object e)
    {
        base.UI_HOLD(e);
        if (_pageType == GodEquipBagType.Equip)
        {
            mCardWrapComponet.f_UpdateView();
        }
        else if (_pageType == GodEquipBagType.Fragment)
        {
            mFragmentWrapComponet.f_UpdateView();
        }
        UpdateViewInfo();
        Debug.LogWarning("CardBagPage__Hold");
    }

    /// <summary>
    /// unhold 的时候 刷新下界面
    /// </summary>
    /// <param name="e"></param>
    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        if (_pageType == GodEquipBagType.Equip)
        {
            mCardWrapComponet.f_UpdateView();
        }
        else if (_pageType == GodEquipBagType.Fragment)
        {
            mFragmentWrapComponet.f_UpdateView();
        }
        UpdateViewInfo();
        Debug.LogWarning("CardBagPage__UnHold");
    }

    protected override void f_InitMessage()
    {
        base.f_InitMessage();
    }

    /// <summary>
    /// 初始化金钱UI
    /// </summary>
    private void InitMoneyUI()
    {
        //List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
        //listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
        //listMoneyType.Add(EM_MoneyType.eUserAttr_Money);
        //listMoneyType.Add(EM_MoneyType.eUserAttr_Energy);
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, listMoneyType);
        f_GetObject("NoHaveEquip").SetActive(_GodEquipList.Count == 0);
        f_GetObject("NoHaveEquipFragment").SetActive(false);
    }
    /// <summary>
    /// 更新货币信息
    /// </summary>
    private void UpdateViewInfo()
    {
        int BagNow = Data_Pool.m_GodEquipPool.f_GetAll().Count;
        int BagUp = Data_Pool.m_RechargePool.f_GetVipPriValue(EM_VipPrivilege.eVip_GeneralBag, UITool.f_GetNowVipLv());
        // f_GetObject("BagCapacity").transform.Find("Label").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(228)+" [{0}]{1}/{2}", BagNow > BagUp ? "ef3612" : "27cc42", BagNow, BagUp);
		f_GetObject("BagCapacity").transform.Find("Label").GetComponent<UILabel>().text = string.Format(" [{0}]{1}/{2}", BagNow > BagUp ? "ef3612" : "27cc42", BagNow, BagUp);
    }

    //关闭处理
    private void BagPageBackBtnHandle(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GodEquipBagPage, UIMessageDef.UI_CLOSE);

        ccUIHoldPool.GetInstance().f_UnHold();
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.MainMenu, UIMessageDef.UI_OPEN);
    }

    //卡牌Item更新
    private void CardItemUpdateByInfo(Transform item, BasePoolDT<long> dt)
    {
        GodEquipPoolDT equipDT = dt as GodEquipPoolDT;
        UILabel tmpName = item.Find("Name").GetComponent<UILabel>();
        UILabel tmpQuality = item.Find("QualityTitle/Quality").GetComponent<UILabel>();
        UILabel tmpQuality2 = item.Find("QualityTitle2/Quality").GetComponent<UILabel>();
        UILabel tmpQualityName = item.Find("QualityTitle").GetComponent<UILabel>();
        UILabel tmpQualityName2 = item.Find("QualityTitle2").GetComponent<UILabel>();
        UILabel tmpLevel = item.Find("LevelTitle/Level").GetComponent<UILabel>();
        UILabel tmpRefine = item.Find("Refine").GetComponent<UILabel>();
        UILabel tmpFitOut = item.Find("FitOut").GetComponent<UILabel>();
        UI2DSprite tmpIcon = item.Find("Icon").GetComponent<UI2DSprite>();

        ResourceCommonDT commonDT = new ResourceCommonDT();
        commonDT.f_UpdateInfo((byte)EM_ResourceType.GodEquip, equipDT.m_iTempleteId, 0);
        f_RegClickEvent(tmpIcon.gameObject, UITool.f_OnItemIconClick, commonDT);

        UISprite tmpCase = item.Find("Case").GetComponent<UISprite>();
        Transform tStar3 = item.Find("star3");
        Transform tStar5 = item.Find("star5");
        string name = equipDT.m_EquipDT.szName;
        tmpCase.spriteName = UITool.f_GetImporentColorName(equipDT.m_EquipDT.iColour, ref name);
        tmpName.text = name;
        tmpQualityName.text = UITool.f_GetProName((EM_RoleProperty)equipDT.m_EquipDT.iIntenProId) + ":";
        tmpQuality.text = UITool.f_GetGodEquipPro(equipDT) + "";
        tmpLevel.text = equipDT.m_lvIntensify.ToString();
        if (equipDT.m_lvRefine != 0)
        {
            item.Find("QualityTitle2").gameObject.SetActive(true);
            tmpQualityName2.text = UITool.f_GetProName((EM_RoleProperty)equipDT.m_EquipDT.iRefinProId2) + ":";
            tmpQuality2.text = RolePropertyTools.CalculatePropertyStartLv1(0, equipDT.m_EquipDT.iRefinPro2, equipDT.m_lvRefine + 1) / 100f + "%";
            tmpRefine.text = string.Format(CommonTools.f_GetTransLanguage(229), equipDT.m_lvRefine);
        }
        else
        {
            item.Find("QualityTitle2").gameObject.SetActive(false);
            tmpRefine.text = "";
        }
        tStar5.gameObject.SetActive(false);
        tStar3.gameObject.SetActive(false);
        UISprite[] tstar;
        if (UITool.f_GetIsOpensystem(EM_NeedLevel.EquipUpStar))
        {
            switch ((EM_Important)equipDT.m_EquipDT.iColour)
            {
                case EM_Important.Red:
                case EM_Important.Gold:
                    tStar5.gameObject.SetActive(true);
                    tStar3.gameObject.SetActive(false);
                    tstar = new UISprite[5];
                    for (int i = 0; i < tStar5.childCount; i++)
                        tstar[i] = tStar5.GetChild(i).GetComponent<UISprite>();
                    UITool.f_UpdateStarNum(tstar, equipDT.m_sstars, "Icon_RMStar_4", "Icon_RMStar_3", 5,false);
                    break;
                case EM_Important.Oragen:
                    tStar5.gameObject.SetActive(false);
                    tStar3.gameObject.SetActive(true);
                    tstar = new UISprite[3];
                    for (int i = 0; i < tStar3.childCount; i++)
                        tstar[i] = tStar3.GetChild(i).GetComponent<UISprite>();
                    UITool.f_UpdateStarNum(tstar, equipDT.m_sstars, "Icon_RMStar_4", "Icon_RMStar_3", 5, true);
                    break;
                default:
                    break;
            }
        }

        CardPoolDT cardDT = UITool.f_GetWhoEquip(equipDT.iId);
        tmpFitOut.gameObject.SetActive(cardDT != null);
        if (cardDT != null)
        {
            tmpFitOut.transform.Find("HeadIcon").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSpriteByCardId(cardDT);

            int iEvolveLv = cardDT.m_iEvolveLv;
            string cardName = cardDT.m_CardDT.szName;
            string borderName = UITool.f_GetImporentColorName(cardDT.m_CardDT.iImportant, ref cardName);
            tmpFitOut.transform.Find("Border").GetComponent<UISprite>().spriteName = borderName;

            tmpFitOut.text = UITool.f_GetHowEquip(equipDT.iId);// +" "+ CommonTools.f_GetTransLanguage(230);
        }
        tmpIcon.sprite2D = UITool.f_GetIconSprite(equipDT.m_EquipDT.iIcon);
    }

    //卡牌Item点击
    private void CardItemClickHandle(Transform item, BasePoolDT<long> dt)
    {
        GodEquipPoolDT cardDT = dt as GodEquipPoolDT;
        GodEquipBox tmp = new GodEquipBox();
        tmp.tEquipPoolDT = cardDT;
        tmp.tType = GodEquipBox.BoxTye.Intro;
        tmp.oType = GodEquipBox.OpenType.Bage;
        //通知HoldPool保存当前页
        ccUIHoldPool.GetInstance().f_Hold(this);
        //通知ccUIManager打开BuildPage页
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GodEquipManage, UIMessageDef.UI_OPEN, tmp);
        MessageBox.DEBUG(string.Format(CommonTools.f_GetTransLanguage(231)+"templateId:{0} , Name:{1}", cardDT.m_iTempleteId, cardDT.m_EquipDT.szName));
    }

    //碎片Item更新
    private void FragmentItemUpdateByInfo(Transform item, BasePoolDT<long> dt)
    {
        GodEquipFragmentPoolDT fragmentDT = dt as GodEquipFragmentPoolDT;
        UILabel tmpName = item.Find("Name").GetComponent<UILabel>();
        UILabel tmpNum = item.Find("NumTitle/Num").GetComponent<UILabel>();
        UI2DSprite tmpIcon = item.Find("Icon").GetComponent<UI2DSprite>();

        ResourceCommonDT commonDT = new ResourceCommonDT();
        commonDT.f_UpdateInfo((byte)EM_ResourceType.GodEquipFragment, fragmentDT.m_iTempleteId, 0);
        f_RegClickEvent(tmpIcon.gameObject, UITool.f_OnItemIconClick, commonDT);

        GameObject tmpSynthesisBtn = item.Find("BtnSynthesis").gameObject;
        GameObject tmpGetBtn = item.Find("BtnGet").gameObject;
        GameObject tmpLackOfTip = item.Find("LackOfTip").gameObject;
        int haveNum = fragmentDT.m_iNum;
        int needNum = fragmentDT.m_EquipFragmentsDT.iBondNum;
        UISprite tmpCase = item.Find("Case").GetComponent<UISprite>();
        string name = fragmentDT.m_EquipFragmentsDT.szName;
        tmpCase.spriteName = UITool.f_GetImporentColorName(fragmentDT.m_EquipFragmentsDT.iColour, ref name);
        tmpName.text = name;
        tmpNum.text = string.Format("{0}/{1}", haveNum, needNum);
        tmpIcon.sprite2D = UITool.f_GetIconSprite(fragmentDT.m_EquipFragmentsDT.iIcon);
        tmpLackOfTip.SetActive(haveNum < needNum);
        tmpGetBtn.SetActive(haveNum < needNum);
        tmpSynthesisBtn.SetActive(haveNum >= needNum);
        f_RegClickEvent(tmpSynthesisBtn, FragmentItemSynthesisBtnHandle, dt);
        f_RegClickEvent(tmpGetBtn, FragmentItemGetBtnHandle, dt);
    }

    //碎片Item点击
    private void FragmentItemClickHandle(Transform item, BasePoolDT<long> dt)
    {
        GodEquipFragmentPoolDT fragmentDT = dt as GodEquipFragmentPoolDT;
        MessageBox.DEBUG(string.Format(CommonTools.f_GetTransLanguage(1536), fragmentDT.m_iTempleteId, fragmentDT.m_EquipFragmentsDT.szName));
    }

    private int _curSynthesisId;
    /// <summary>
    /// 碎片Item合成按钮
    /// </summary>
    private void FragmentItemSynthesisBtnHandle(GameObject go, object obj1, object obj2)
    {
        GodEquipFragmentPoolDT fragmentDT = obj1 as GodEquipFragmentPoolDT;
        MessageBox.DEBUG(string.Format(CommonTools.f_GetTransLanguage(232), fragmentDT.m_iTempleteId, fragmentDT.m_EquipFragmentsDT.szName));
        //发送合成协议
        _curSynthesisId = fragmentDT.m_iTempleteId;
        int haveNum = fragmentDT.m_iNum;
        int needNum = fragmentDT.m_EquipFragmentsDT.iBondNum;
        if (haveNum >= needNum * 2)
        {
            MutiOperateParam tMutiOperare = new MutiOperateParam("wj_hc_font_hc", EM_ResourceType.GodEquipFragment, fragmentDT.m_EquipFragmentsDT.iId, haveNum, haveNum / needNum, "", BatchSynthesis);

            ccUIManage.GetInstance().f_SendMsg(UINameConst.MutiOperatePage, UIMessageDef.UI_OPEN, tMutiOperare);
        }
        else
        {
            UITool.f_OpenOrCloseWaitTip(true);
            SocketCallbackDT callBackDT = new SocketCallbackDT();
            callBackDT.m_ccCallbackSuc = Synthesis_Suc;
            callBackDT.m_ccCallbackFail = Synthesis_Fail;
            Data_Pool.m_GodEquipFragmentPool.f_Synthesis(_curSynthesisId, 1, callBackDT);
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
        SocketCallbackDT callBackDT = new SocketCallbackDT();
        callBackDT.m_ccCallbackSuc = Synthesis_Suc;
        callBackDT.m_ccCallbackFail = Synthesis_Fail;
        Data_Pool.m_GodEquipFragmentPool.f_Synthesis(_curSynthesisId, BatchNum, callBackDT);
    }

    /// <summary>
    /// 碎片Item获取按钮
    /// </summary>
    private void FragmentItemGetBtnHandle(GameObject go, object obj1, object obj2)
    {
        GodEquipFragmentPoolDT fragmentDT = obj1 as GodEquipFragmentPoolDT;
        StaticValue.mGetWayToBattleParam.f_UpdateDataInfo(EM_GetWayToBattle.EquipBag, EM_ResourceType.GodEquipFragment, fragmentDT.m_iTempleteId);
        GetWayPageParam param = new GetWayPageParam(EM_ResourceType.GodEquipFragment, fragmentDT.m_iTempleteId, this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, param);
    }

    private void Synthesis_Suc(object node)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        SynthesisJudge++;
        eMsgOperateResult result = (eMsgOperateResult)node;
        Debug.LogWarning(CommonTools.f_GetTransLanguage(234) + result.ToString());
        ccUIHoldPool.GetInstance().f_Hold(this);
        EquipSythesis tEquipSythesis = new EquipSythesis(_curSynthesisId, BatchNum, EquipSythesis.ResonureType.GodEquip);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainEquipShowPage, UIMessageDef.UI_OPEN, tEquipSythesis);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.MutiOperatePage, UIMessageDef.UI_CLOSE);
        Data_Pool.m_GuidancePool.m_OtherSave = true;
    }

    private void Synthesis_Fail(object node)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult result = (eMsgOperateResult)node;
        Debug.LogWarning(CommonTools.f_GetTransLanguage(235) + result.ToString());
    }
    private void UI_OpenEquip(GameObject go, object obj1, object obj2)
    {

    }
    private string EquipProperty(int id)
    {
        switch (id)
        {
            case 1:
                return CommonTools.f_GetTransLanguage(236);
            case 2:
                return CommonTools.f_GetTransLanguage(237);
            case 3:
                return CommonTools.f_GetTransLanguage(238);
            case 4:
                return CommonTools.f_GetTransLanguage(239);
        }
        return null;
    }


}
