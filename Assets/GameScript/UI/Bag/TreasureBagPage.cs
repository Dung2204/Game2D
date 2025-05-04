using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class TreasureBagPage : UIFramwork

{
    private TreasureBagType _pageType;
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
                _TreasureList = Data_Pool.m_TreasurePool.f_GetAll();
                _cardWrapComponet = new UIWrapComponent(186, 2, 650, 5, _cardItemParent, _cardItem, _TreasureList, CardItemUpdateByInfo, CardItemClickHandle);
            }
            Data_Pool.m_TreasurePool.f_SortList();
            return _cardWrapComponet;
        }
    }


    private List<BasePoolDT<long>> _TreasureList;
    private void _Sort()
    {
        List<BasePoolDT<long>> EquipList = new List<BasePoolDT<long>>();
        _TreasureList.Sort((BasePoolDT<long> a, BasePoolDT<long> b) => { return ((TreasurePoolDT)a).m_TreasureDT.iList > ((TreasurePoolDT)b).m_TreasureDT.iList ? -1 : 1; });
        foreach (TreasurePoolDT item in _TreasureList)
        {
            if (UITool.f_GetHowEquip(item.iId) != "")
            {
                _TreasureList.Remove(item);
                EquipList.Add(item);
            }
        }
        List<BasePoolDT<long>> tmpList = new List<BasePoolDT<long>>();
        List<BasePoolDT<long>> tmpList2 = new List<BasePoolDT<long>>();
        for (int i = 0; i < _TreasureList.Count; i++)
        {
            if (i + 1 == _TreasureList.Count)
            {
                if (!tmpList.Contains(_TreasureList[i]))
                {
                    tmpList.Add(_TreasureList[i]);
                    tmpList2.AddRange(tmpList);
                }
                break;
            }
            if (((TreasurePoolDT)_TreasureList[i]).m_TreasureDT.iList == ((TreasurePoolDT)_TreasureList[i + 1]).m_TreasureDT.iList)
            {

                if (!tmpList.Contains(_TreasureList[i]))
                    tmpList.Add(_TreasureList[i]);
                if (!tmpList.Contains(_TreasureList[i + 1]))
                    tmpList.Add(_TreasureList[i + 1]);
            }
            else
            {
                tmpList.Sort((BasePoolDT<long> a, BasePoolDT<long> b) => { return ((TreasurePoolDT)a).m_lvIntensify > ((TreasurePoolDT)b).m_lvIntensify ? -1 : 1; });
                tmpList.Sort((BasePoolDT<long> a, BasePoolDT<long> b) => { return ((TreasurePoolDT)a).m_lvRefine >= ((TreasurePoolDT)b).m_lvRefine ? -1 : 1; });
                tmpList2.AddRange(tmpList);
                tmpList.Clear();
            }
        }
        EquipList.AddRange(tmpList2);
        _TreasureList = EquipList;
    }
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
                _fragmentList = Data_Pool.m_TreasureFragmentPool.f_GetAll();
                _fragmentWrapComponet = new UIWrapComponent(186, 2, 650, 5, _fragmentItemParent, _fragmentItem, _fragmentList, FragmentItemUpdateByInfo, FragmentItemClickHandle);
            }
            Data_Pool.m_TreasureFragmentPool._Sort();
            return _fragmentWrapComponet;
        }
    }
    private List<BasePoolDT<long>> _fragmentList;
    
    private GameObject _btnBack;
    private UIToggle _cardToggle;
    private UIToggle _fragmentToggle;
    #region 红点提示
    protected override void InitRaddot()
    {
        base.InitRaddot();
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.TreasureBagNewGoods, f_GetObject("Btn_FragmentPage"), ReddotCallback_Show_BtnFragment, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.TreasureFragmentBagNewGoods, f_GetObject("Btn_CardPage"), ReddotCallback_Show_BtnCard, true);
        UpdateReddotUI();
    }
    protected override void On_Destory()
    {
        base.On_Destory();
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.TreasureBagNewGoods, f_GetObject("Btn_FragmentPage"));
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.TreasureFragmentBagNewGoods, f_GetObject("Btn_FragmentPage"));
    }
    protected override void UpdateReddotUI()
    {
        base.UpdateReddotUI();
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.TreasureBagNewGoods);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.TreasureFragmentBagNewGoods);
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
            _pageType = TreasureBagType.Treasure;
            mCardWrapComponet.f_ResetView();
            f_GetObject("NoHaveMain").SetActive(_TreasureList.Count == 0);
            f_GetObject("NoHaveFrament").SetActive(false);
        }
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.TreasureBagNewGoods);
    }

    //碎片选项选中处理
    private void FragmentToggleChange()
    {
        _fragmentDragObj.SetActive(_fragmentToggle.value);
        if (_fragmentToggle.value)
        {
            f_GetObject("BagCapacity").SetActive(false);
            _pageType = TreasureBagType.Fragment;
            mFragmentWrapComponet.f_ResetView();
            f_GetObject("NoHaveFrament").SetActive(_fragmentList.Count == 0);
            f_GetObject("NoHaveMain").SetActive(false);
        }
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.TreasureFragmentBagNewGoods);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        Debug.LogWarning("TreasureBagPageOpen");
        if (e != null)
        {
            //根据传参数 打开不同界面
            _pageType = (TreasureBagType)e;
            if (_pageType == TreasureBagType.Treasure)
            {
                Data_Pool.m_ReddotMessagePool.f_MsgSubtract(EM_ReddotMsgType.TreasureBagNewGoods);
                _cardToggle.value = true;
                mCardWrapComponet.f_ResetView();
            }
            else if (_pageType == TreasureBagType.Fragment)
            {
                _fragmentToggle.value = true;
                mFragmentWrapComponet.f_ResetView();
                Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.TreasureFragmentBagNewGoods);
            }
        }
        else
        {
            _pageType = TreasureBagType.Treasure;
            _cardToggle.value = true;
            mCardWrapComponet.f_ResetView();
        }
        f_GetObject("NoHaveMain").SetActive(_TreasureList.Count == 0);
        f_GetObject("NoHaveFrament").SetActive(false);
        UpdateViewInfo();
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
        Debug.LogWarning("CardBagPage__Hold");
    }

    /// <summary>
    /// unhold 的时候 刷新下界面
    /// </summary>
    /// <param name="e"></param>
    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        if (_pageType == TreasureBagType.Treasure)
        {
            mCardWrapComponet.f_UpdateView();
        }
        else if (_pageType == TreasureBagType.Fragment)
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
        List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
        listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Money);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Energy);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, listMoneyType);
    }
    /// <summary>
    /// 更新货币信息
    /// </summary>
    private void UpdateViewInfo()
    {
        int BagNow = Data_Pool.m_TreasurePool.f_GetAll().Count;
        int BagUp = Data_Pool.m_RechargePool.f_GetVipPriValue(EM_VipPrivilege.eVip_GeneralBag, UITool.f_GetNowVipLv());
        // f_GetObject("BagCapacity").transform.Find("Label").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(228)+"[{0}]{1}/{2}", BagNow > BagUp ? "ef3612" : "27cc42", BagNow, BagUp);
		f_GetObject("BagCapacity").transform.Find("Label").GetComponent<UILabel>().text = string.Format("[{0}]{1}/{2}", BagNow > BagUp ? "ef3612" : "27cc42", BagNow, BagUp);
    }

    //关闭处理
    private void BagPageBackBtnHandle(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TreasureBagPage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
    }

    //卡牌Item更新
    private void CardItemUpdateByInfo(Transform item, BasePoolDT<long> dt)
    {
        TreasurePoolDT TreasureDT = dt as TreasurePoolDT;
        UILabel tmpName = item.Find("Name").GetComponent<UILabel>();
        UILabel tmpQuality = item.Find("QualityTitle/Quality").GetComponent<UILabel>();
        UILabel tmpQualityName = item.Find("QualityTitle").GetComponent<UILabel>();
        UILabel tmpQuality2 = item.Find("QualityTitle2/Quality").GetComponent<UILabel>();
        Transform tmpQuality2Tran = item.Find("QualityTitle2");
        UILabel tmpQualityName2 = item.Find("QualityTitle2").GetComponent<UILabel>();
        UILabel tmpLevel = item.Find("LevelTitle/Level").GetComponent<UILabel>();
        UILabel tmpRefine = item.Find("Refine").GetComponent<UILabel>();
        UILabel tmpFitOut = item.Find("FitOut").GetComponent<UILabel>();
        UI2DSprite tmpIcon = item.Find("Icon").GetComponent<UI2DSprite>();

        ResourceCommonDT commonDT = new ResourceCommonDT();
        commonDT.f_UpdateInfo((byte)EM_ResourceType.Treasure, TreasureDT.m_iTempleteId, 0);
        f_RegClickEvent(tmpIcon.gameObject, UITool.f_OnItemIconClick, commonDT);

        UILabel tmpNum = item.Find("Num").GetComponent<UILabel>();
        UISprite tmpCase = item.Find("Case").GetComponent<UISprite>();
        string name = TreasureDT.m_TreasureDT.szName;
        tmpCase.spriteName = UITool.f_GetImporentColorName(TreasureDT.m_TreasureDT.iImportant, ref name);
        tmpName.text = name;
        tmpQualityName.text = UITool.f_GetProName((EM_RoleProperty)TreasureDT.m_TreasureDT.iIntenProId1);
        tmpQualityName2.text = UITool.f_GetProName((EM_RoleProperty)TreasureDT.m_TreasureDT.iIntenProId2);
        if (UITool.f_GetTreasurePro(TreasureDT)[1] == 0)
            tmpQuality2Tran.gameObject.SetActive(false);
        else
            tmpQuality2Tran.gameObject.SetActive(true);
        tmpQuality.text = UITool.f_GetTreasurePro(TreasureDT)[0].ToString();
        tmpQuality2.text = UITool.f_GetTreasurePro(TreasureDT)[1] / 100f + "%";
        tmpLevel.text = TreasureDT.m_lvIntensify.ToString();
        if (TreasureDT.m_lvRefine != 0)
            tmpRefine.text = string.Format(CommonTools.f_GetTransLanguage(244), TreasureDT.m_lvRefine);
        else
            tmpRefine.text = "";
        tmpFitOut.text = UITool.f_GetHowEquip(TreasureDT.iId) == "" ? "" : UITool.f_GetHowEquip(TreasureDT.iId) + CommonTools.f_GetTransLanguage(230);
        tmpIcon.sprite2D = UITool.f_GetIconSprite(TreasureDT.m_TreasureDT.iIcon);
        if (TreasureDT.m_Num > 1)
            tmpNum.text = string.Format(CommonTools.f_GetTransLanguage(245), TreasureDT.m_Num);
        else
            tmpNum.text = "";
    }

    //卡牌Item点击
    private void CardItemClickHandle(Transform item, BasePoolDT<long> dt)
    {
        TreasurePoolDT cardDT = dt as TreasurePoolDT;
        TreasureBox tmp = new TreasureBox();
        tmp.IsShowChange = 1;
        tmp.tTreasurePoolDT = cardDT;
        tmp.tType = TreasureBox.BoxType.Intro;
        //f_RegClickEvent(item.gameObject, UI_TreasureManage, cardDT);
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TreasureManage, BaseUIMessageDef.UI_OPEN, tmp);
    }

    //碎片Item更新
    private void FragmentItemUpdateByInfo(Transform item, BasePoolDT<long> dt)
    {
        TreasureFragmentPoolDT fragmentDT = dt as TreasureFragmentPoolDT;
        UILabel tmpName = item.Find("Name").GetComponent<UILabel>();
        UILabel tmpNum = item.Find("NumTitle/Num").GetComponent<UILabel>();
        UI2DSprite tmpIcon = item.Find("Icon").GetComponent<UI2DSprite>();

        ResourceCommonDT commonDT = new ResourceCommonDT();
        commonDT.f_UpdateInfo((byte)EM_ResourceType.TreasureFragment, fragmentDT.m_iTempleteId, 0);
        f_RegClickEvent(tmpIcon.gameObject, UITool.f_OnItemIconClick, commonDT);

        GameObject tmpSynthesisBtn = item.Find("BtnSynthesis").gameObject;
        UISprite tmpCase = item.Find("Case").GetComponent<UISprite>();
        string name = fragmentDT.m_TreasureFragmentsDT.szName;
        tmpCase.spriteName = UITool.f_GetImporentColorName(fragmentDT.m_TreasureFragmentsDT.iImportant, ref name);
        tmpName.text = name;
        tmpNum.text = string.Format("{0}", fragmentDT.m_num);
        tmpIcon.sprite2D = UITool.f_GetIconSprite(fragmentDT.m_TreasureFragmentsDT.iIcon);
        tmpSynthesisBtn.SetActive(true);
        f_RegClickEvent(tmpSynthesisBtn, Ui_OpenSythes, dt);
    }

    //碎片Item点击
    private void FragmentItemClickHandle(Transform item, BasePoolDT<long> dt)
    {
        TreasureFragmentPoolDT fragmentDT = dt as TreasureFragmentPoolDT;
    }

    private int _curSynthesisId;
    /// <summary>
    /// 碎片Item合成按钮
    /// </summary>
    private void FragmentItemSynthesisBtnHandle(GameObject go, object obj1, object obj2)
    {
        TreasureFragmentPoolDT fragmentDT = obj1 as TreasureFragmentPoolDT;
        TreasureFragmentsDT[] tmpArr = (TreasureFragmentsDT[])obj2;
        int i = 0;
        foreach (TreasureFragmentsDT item in tmpArr)
        {
            if (UITool.f_GetTreasureFragmentNum(item.iId)[i] < 1)
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(246));
                return;
            }
            i++;
        }
        //发送合成协议
        UITool.f_OpenOrCloseWaitTip(true);
        _curSynthesisId = fragmentDT.m_iTempleteId;
        SocketCallbackDT callBackDT = new SocketCallbackDT();
        callBackDT.m_ccCallbackSuc = Synthesis_Suc;
        callBackDT.m_ccCallbackFail = Synthesis_Fail;
        Data_Pool.m_TreasureFragmentPool.f_Synthesis(fragmentDT.m_TreasureFragmentsDT.iTreasureId, callBackDT);
    }

    private void Synthesis_Suc(object node)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(247));
        f_GetObject("TreasureSythes").SetActive(false);
        _fragmentItemParent.SetActive(true);
        mFragmentWrapComponet.f_UpdateView();
        //ccUIHoldPool.GetInstance().f_Hold(this);
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.GainCardShowPage, UIMessageDef.UI_OPEN, _curSynthesisId);
    }

    private void Synthesis_Fail(object node)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult result = (eMsgOperateResult)node;
        Debug.LogWarning(CommonTools.f_GetTransLanguage(248) + result.ToString());
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(249));
    }
    private void UI_TreasureManage(GameObject go, object obj1, object obj2)
    {
        //通知HoldPool保存当前页
        ccUIHoldPool.GetInstance().f_Hold(this);
        //通知ccUIManager打开BuildPage页
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TreasureManage, BaseUIMessageDef.UI_OPEN, obj1);
    }




    //////////////法宝合成界面////////////////////
    private void UI_CloseSythes(GameObject go, object obj1, object obj2) { f_GetObject("TreasureSythes").SetActive(false); _fragmentItemParent.SetActive(true); }
    private void Ui_OpenSythes(GameObject go, object obj1, object obj2)
    {

        ccUIManage.GetInstance().f_SendMsg(UINameConst.GrabTreasurePage, UIMessageDef.UI_OPEN, obj1);
        ccUIHoldPool.GetInstance().f_Hold(this);
        return;
        _fragmentItemParent.SetActive(false);
        f_GetObject("TreasureSythes").SetActive(true);
        Transform Sythes;
        Transform Sythes_Frament3;
        Transform Sythes_Frament4;
        Transform Sythes_Frament5;
        Transform Sythes_Frament6;
        UI2DSprite Sythes_TreaIcon;
        GameObject Sythes_SythesBtn;
        Sythes = f_GetObject("TreasureSythes").transform.Find("SythesBG");
        Sythes_Frament3 = Sythes.Find("Sythes3");
        Sythes_Frament4 = Sythes.Find("Sythes4");
        Sythes_Frament5 = Sythes.Find("Sythes5");
        Sythes_Frament6 = Sythes.Find("Sythes6");
        Sythes_TreaIcon = Sythes.Find("Treasure").GetComponent<UI2DSprite>();
        Sythes_SythesBtn = Sythes.Find("SythesBtn").gameObject;
        Sythes_Frament3.gameObject.SetActive(false);
        Sythes_Frament4.gameObject.SetActive(false);
        Sythes_Frament5.gameObject.SetActive(false);
        Sythes_Frament6.gameObject.SetActive(false);
        TreasureFragmentPoolDT fragmentDT = obj1 as TreasureFragmentPoolDT;
        Transform tmpTrans = null;
        TreasureFragmentsDT[] tmpTreasure = UITool.f_GetTreasureFragmentDT(fragmentDT.m_TreasureFragmentsDT.iId);
        switch (tmpTreasure.Length)
        {
            case 3:
                Sythes_Frament3.gameObject.SetActive(true);
                tmpTrans = Sythes_Frament3;
                break;
            case 4:
                Sythes_Frament4.gameObject.SetActive(true);
                tmpTrans = Sythes_Frament4;
                break;
            case 5:
                Sythes_Frament5.gameObject.SetActive(true);
                tmpTrans = Sythes_Frament5;
                break;
            case 6:
                Sythes_Frament6.gameObject.SetActive(true);
                tmpTrans = Sythes_Frament6;
                break;
        }
        for (int i = 0; i < tmpTrans.childCount; i++)
        {
            tmpTrans.GetChild(i).GetChild(0).GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSprite(tmpTreasure[i].iIcon);
            tmpTrans.GetChild(i).GetChild(1).GetComponent<UILabel>().text = UITool.f_GetTreasureFragmentNum(tmpTreasure[i].iId)[i].ToString();
        }

        Sythes_TreaIcon.sprite2D = UITool.f_GetIconSprite(fragmentDT.m_TreasureFragmentsDT.iIcon);
        f_RegClickEvent(Sythes_SythesBtn, FragmentItemSynthesisBtnHandle, fragmentDT, tmpTreasure);
        f_RegClickEvent(Sythes.gameObject, UI_CloseSythes);
    }




}
