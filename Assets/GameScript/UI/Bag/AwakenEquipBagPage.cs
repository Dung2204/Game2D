using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class AwakenEquipBagPage : UIFramwork
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
                _TreasureList = Data_Pool.m_AwakenEquipPool.f_GetAll();

                _cardWrapComponet = new UIWrapComponent(186, 2, 650, 5, _cardItemParent, _cardItem, _TreasureList, CardItemUpdateByInfo, CardItemClickHandle);
            }
            _TreasureList.Sort((BasePoolDT<long> a, BasePoolDT<long> b) => { return ((AwakenEquipPoolDT)a).m_AwakenEquipDT.iList >= ((AwakenEquipPoolDT)b).m_AwakenEquipDT.iList ? 1 : -1; });
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
    private GameObject _btnBack;
    private UIToggle _cardToggle;
    private UIToggle _fragmentToggle;
    #region 红点提示
    protected override void InitRaddot()
    {
        base.InitRaddot();
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.AwakenBagNewGoods, f_GetObject("Btn_CardPage"), ReddotCallback_Show_BtnFragment, true);
        UpdateReddotUI();
    }
    protected override void On_Destory()
    {
        base.On_Destory();
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.AwakenBagNewGoods, f_GetObject("Btn_CardPage"));
    }
    protected override void UpdateReddotUI()
    {
        base.UpdateReddotUI();
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.AwakenBagNewGoods);
    }
    private void ReddotCallback_Show_BtnFragment(object Obj)
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
        _btnBack = f_GetObject("BackBtn");
        _cardToggle = f_GetObject("Btn_CardPage").GetComponent<UIToggle>();
        _ItemInduce = f_GetObject("ItemInduce").transform;
        f_RegClickEvent(_btnBack, BagPageBackBtnHandle);
        EventDelegate.Add(_cardToggle.onChange, CardToggleChange);
        f_RegClickEvent(_ItemInduce.Find("AwakenEquipInduce_ABg").gameObject, UI_CloseInduce);
    }

    //卡牌选项选中处理
    private void CardToggleChange()
    {
        mCardWrapComponet.f_ResetView();
        f_GetObject("NoHave").SetActive(_TreasureList.Count == 0);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        //Debug.LogWarning("TreasureBagPageOpen");
        //if (e != null)
        //{
        //    //根据传参数 打开不同界面
        //}
        //else
        //{
        //    _pageType = TreasureBagType.Treasure;
        //    _cardToggle.value = true;
        mCardWrapComponet.f_ResetView();
        f_GetObject("NoHave").SetActive(false);
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.AwakenBagNewGoods);

        //}
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
        mCardWrapComponet.f_UpdateView();
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

        int moneyCount = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Money);
        string moneyText = moneyCount.ToString();
        if (moneyCount >= 100000)
            moneyText = moneyCount / 10000 + CommonTools.f_GetTransLanguage(286);//string.Format("{0:F}", moneyCount * 1.0f / 10000) + "万";
        int syceeCount = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee);
        string syceeText = syceeCount.ToString();
        if (syceeCount >= 100000)
        {
            syceeText = syceeCount / 10000 + CommonTools.f_GetTransLanguage(286);
        }
        int BagNow = Data_Pool.m_AwakenEquipPool.f_GetAll().Count;
        int BagUp = Data_Pool.m_RechargePool.f_GetVipPriValue(EM_VipPrivilege.eVip_GeneralBag, UITool.f_GetNowVipLv());
        // f_GetObject("BagCapacity").transform.Find("Label").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(287)+"[{0}]{1}/{2}", BagNow > BagUp ? "ef3612" : "27cc42", BagNow, BagUp);
		f_GetObject("BagCapacity").transform.Find("Label").GetComponent<UILabel>().text = string.Format("[{0}]{1}/{2}", BagNow > BagUp ? "ef3612" : "27cc42", BagNow, BagUp);
    }

    //关闭处理
    private void BagPageBackBtnHandle(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.AwakenEquipBagPage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
    }
    private void UI_CloseInduce(GameObject go, object obj1, object obj2s)
    {
        f_GetObject("ItemInduce").gameObject.SetActive(false);
    }
    private void UI_OpenInduce(GameObject go, object obj1, object obj2)
    {
        f_GetObject("ItemInduce").SetActive(true);
    }
    //卡牌Item更新
    private void CardItemUpdateByInfo(Transform item, BasePoolDT<long> dt)
    {
        AwakenEquipPoolDT AwakenDT = dt as AwakenEquipPoolDT;
        UILabel tmpName = item.Find("Name").GetComponent<UILabel>();
        UI2DSprite tmpIcon = item.Find("Icon").GetComponent<UI2DSprite>();
        UILabel tmpNum = item.Find("Num").GetComponent<UILabel>();
        UILabel tmpPro1 = item.Find("Pro1").GetComponent<UILabel>();
        UILabel tmpPro2 = item.Find("Pro2").GetComponent<UILabel>();
        UILabel tmpPro3 = item.Find("Pro3").GetComponent<UILabel>();
        UISprite tmpCase = item.Find("Case").GetComponent<UISprite>();
        GameObject tmpSellBtn = item.Find("SellBtn").gameObject;
        string name = AwakenDT.m_AwakenEquipDT.szName;
        tmpCase.spriteName = UITool.f_GetImporentColorName(AwakenDT.m_AwakenEquipDT.iImportant, ref name);
        tmpName.text = name;
        tmpNum.text = string.Format(CommonTools.f_GetTransLanguage(288), AwakenDT.m_num);
        tmpIcon.sprite2D = UITool.f_GetIconSprite(AwakenDT.m_AwakenEquipDT.iIcon);
        tmpPro1.text = string.Format("[f5bf3e]{0}[-]+{1}", UITool.f_GetProName((EM_RoleProperty)AwakenDT.m_AwakenEquipDT.iAddProId1), AwakenDT.m_AwakenEquipDT.iAddPro1);
        tmpPro2.text = string.Format("[f5bf3e]{0}[-]+{1}", UITool.f_GetProName((EM_RoleProperty)AwakenDT.m_AwakenEquipDT.iAddProId2), AwakenDT.m_AwakenEquipDT.iAddPro2);
        tmpPro3.text = string.Format("[f5bf3e]{0}[-]+{1}", CommonTools.f_GetTransLanguage(289), AwakenDT.m_AwakenEquipDT.iAddPro3);
        ResourceCommonDT commonDT = new ResourceCommonDT();
        commonDT.f_UpdateInfo((byte)EM_ResourceType.AwakenEquip, AwakenDT.m_iTempleteId, 0);
        f_RegClickEvent(tmpIcon.gameObject, UITool.f_OnItemIconClick, commonDT);
        f_RegClickEvent(tmpSellBtn, OnSellBtn, AwakenDT);
    }

    private Transform _ItemInduce;
    UILabel Induce_Num;
    UILabel Induce_Pro1;
    UILabel Induce_Pro2;
    UILabel Induce_Pro3;
    UILabel Induce_Name;
    UI2DSprite Induce_Icon;
    UILabel Induce_Desc;
    UISprite Induce_Case;
    private void UI_InduceUpdate(AwakenEquipPoolDT Awaken)
    {
        Transform tTran = _ItemInduce.Find("EquipInduce");
        Induce_Num = tTran.Find("Num").GetComponent<UILabel>();
        Induce_Name = tTran.Find("Name").GetComponent<UILabel>();
        Induce_Icon = tTran.Find("Icon").GetComponent<UI2DSprite>();
        Induce_Pro1 = tTran.Find("Pro/Pro1").GetComponent<UILabel>();
        Induce_Pro2 = tTran.Find("Pro/Pro2").GetComponent<UILabel>();
        Induce_Pro3 = tTran.Find("Pro/Pro3").GetComponent<UILabel>();
        Induce_Desc = tTran.Find("Desc").GetComponent<UILabel>();
        Induce_Case = tTran.Find("Case").GetComponent<UISprite>();

        Induce_Num.text = string.Format(CommonTools.f_GetTransLanguage(290), Awaken.m_num);
        Induce_Case.spriteName = UITool.f_GetImporentCase(Awaken.m_AwakenEquipDT.iImportant);
        Induce_Name.text = Awaken.m_AwakenEquipDT.szName;
        Induce_Icon.sprite2D = UITool.f_GetIconSprite(Awaken.m_AwakenEquipDT.iIcon);
        Induce_Pro1.text = string.Format("[f5bf3d]{0}[-]+{1}", UITool.f_GetProName((EM_RoleProperty)Awaken.m_AwakenEquipDT.iAddProId1), Awaken.m_AwakenEquipDT.iAddPro1);
        Induce_Pro2.text = string.Format("[f5bf3d]{0}[-]+{1}", UITool.f_GetProName((EM_RoleProperty)Awaken.m_AwakenEquipDT.iAddProId2), Awaken.m_AwakenEquipDT.iAddPro2);
        Induce_Pro3.text = string.Format("[f5bf3d]{0}[-]+{1}", CommonTools.f_GetTransLanguage(289), Awaken.m_AwakenEquipDT.iAddPro3);
        Induce_Desc.text = Awaken.m_AwakenEquipDT.szDesc;
    }
    //卡牌Item点击
    private void CardItemClickHandle(Transform item, BasePoolDT<long> dt)
    {
        AwakenEquipPoolDT AwakenDT = dt as AwakenEquipPoolDT;
        AwakenEquipPageParam PagePram = new AwakenEquipPageParam(AwakenDT.m_AwakenEquipDT.iId);
        PagePram.m_isBagOpen = true;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.AwakenEquipPage,UIMessageDef.UI_OPEN, PagePram);
        //UI_InduceUpdate(AwakenDT);
        //f_GetObject("ItemInduce").SetActive(true);
        //f_RegClickEvent(item.gameObject, UI_TreasureManage, cardDT);
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.TreasureManage, BaseUIMessageDef.UI_OPEN, cardDT);
    }
    private void UI_TreasureManage(GameObject go, object obj1, object obj2)
    {
        //通知HoldPool保存当前页
        ccUIHoldPool.GetInstance().f_Hold(this);
        //通知ccUIManager打开BuildPage页
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.TreasureManage, BaseUIMessageDef.UI_OPEN, obj1);
    }


    private void OnSellBtn(GameObject go, object obj1, object obj2)
    {
        AwakenEquipPoolDT tAwakenEquipPool = (AwakenEquipPoolDT)obj1;

        SellPageParam tSellPageParam = new SellPageParam();
        tSellPageParam.iId = 1;
        tSellPageParam.KeyId = tAwakenEquipPool.iId;
        tSellPageParam.moneyType = EM_MoneyType.eUserAttr_GodSoul;
        tSellPageParam.onConfirmBuyCallback = OnSellCallBack;
        tSellPageParam.resourceCount = tAwakenEquipPool.m_num;
        tSellPageParam.resourceID = tAwakenEquipPool.m_AwakenEquipDT.iId;
        tSellPageParam.resourceType = EM_ResourceType.AwakenEquip;
        tSellPageParam.SellNum = tAwakenEquipPool.m_AwakenEquipDT.iResolveCount;
        tSellPageParam.title = CommonTools.f_GetTransLanguage(291);

        ccUIManage.GetInstance().f_SendMsg(UINameConst.SellPage, UIMessageDef.UI_OPEN, tSellPageParam);

    }

    private void OnSellCallBack(int iId, EM_ResourceType type, int resourceId, int resourceCount, int buyCount, long KeyId)
    {
        UITool.f_OpenOrCloseWaitTip(true);

        SocketCallbackDT tSellCallBack = new SocketCallbackDT();
        tSellCallBack.m_ccCallbackSuc = SellSuc;
        tSellCallBack.m_ccCallbackFail = SellFail;
        Data_Pool.m_AwakenEquipPool.f_Sell(KeyId, buyCount, tSellCallBack);
    }

    private void SellSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(292));
        _cardWrapComponet.f_UpdateView();
    }

    private void SellFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(293));
        _cardWrapComponet.f_UpdateView();
    }
}
