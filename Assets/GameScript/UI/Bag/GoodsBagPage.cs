using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class GoodsBagPage : UIFramwork
{

    #region 红点提示
    protected override void InitRaddot()
    {
        base.InitRaddot();
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.GoodsBagNewGoods, f_GetObject("Btn_CardPage"), ReddotCallback_Show_BtnFragment, true);
        UpdateReddotUI();
    }
    protected override void On_Destory()
    {
        base.On_Destory();
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.GoodsBagNewGoods, f_GetObject("Btn_CardPage"));
    }
    protected override void UpdateReddotUI()
    {
        base.UpdateReddotUI();
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.GoodsBagNewGoods);
    }
    private void ReddotCallback_Show_BtnFragment(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnFragment = f_GetObject("Btn_CardPage");
        UITool.f_UpdateReddot(BtnFragment, iNum, new Vector3(110, -20, 0), 74);
    }
    #endregion
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
                _GoodsList = Data_Pool.m_BaseGoodsPool.f_GetAll();
                _cardWrapComponet = new UIWrapComponent(186, 2, 650, 5, _cardItemParent, _cardItem, _GoodsList, CardItemUpdateByInfo, CardItemClickHandle);
            }
            //_Sort();
            _GoodsList.Sort((BasePoolDT<long> a, BasePoolDT<long> b) => { return ((BaseGoodsPoolDT)a).m_BaseGoodsDT.iPriority >= ((BaseGoodsPoolDT)b).m_BaseGoodsDT.iPriority ? 1 : -1; });
            return _cardWrapComponet;
        }
    }


    private List<BasePoolDT<long>> _GoodsList;
    private GameObject _btnBack;
    private UIToggle _cardToggle;

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
        f_RegClickEvent(_btnBack, BagPageBackBtnHandle);
        EventDelegate.Add(_cardToggle.onChange, CardToggleChange);
    }
    private void _UpdateBag(object obj)
    {
        mCardWrapComponet.f_UpdateView();
        UpdateViewInfo();
    }
    //卡牌选项选中处理
    private void CardToggleChange()
    {
        mCardWrapComponet.f_ResetView();
        f_GetObject("NoHave").SetActive(_GoodsList.Count == 0);
    }
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        Data_Pool.m_GuidancePool.OpenLevelPageUIName = UINameConst.GoodsBagPage;
        f_GetObject("Batch").SetActive(false);
        //Debug.LogWarning("TreasureBagPageOpen");
        //if (e != null)
        //{
        //    //根据传参数 打开不同界面
        //}
        //else
        //{
        //    _pageType = TreasureBagType.Treasure;
        //    _cardToggle.value = true;
        if (e != null && e is BaseGoodsPoolDT)//打开批量购买界面(如体力丹)
        {
            CommThing(null, e, 0);
        }
        mCardWrapComponet.f_ResetView();
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.GoodsBagNewGoods);

        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_UpdateGoodsBag, _UpdateBag, this);
        f_GetObject("NoHave").SetActive(false);
        //}
        UpdateViewInfo();
        InitMoneyUI();
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_UpdateGoodsBag);
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
        //ccUIHoldPool.GetInstance().f_UnHold();
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
        int BagNow = Data_Pool.m_BaseGoodsPool.f_GetAll().Count;
        int BagUp = Data_Pool.m_RechargePool.f_GetVipPriValue(EM_VipPrivilege.eVip_GeneralBag, UITool.f_GetNowVipLv());
        // f_GetObject("BagCapacity").transform.Find("Label").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(256)+"[{0}]{1}/{2}", BagNow > BagUp ? "ef3612" : "27cc42", BagNow, BagUp);
		f_GetObject("BagCapacity").transform.Find("Label").GetComponent<UILabel>().text = string.Format("[{0}]{1}/{2}", BagNow > BagUp ? "ef3612" : "27cc42", BagNow, BagUp);
    }

    //关闭处理
    private void BagPageBackBtnHandle(GameObject go, object obj1, object obj2)
    {
        //ccUIHoldPool.GetInstance().f_Hold(this);
        
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GoodsBagPage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold(this);
        //MessageBox.DEBUG("当前hold的界面+++++++++++++++" + ccUIHoldPool.GetInstance().f_GetHoldNum());
    }

    //卡牌Item更新
    private void CardItemUpdateByInfo(Transform item, BasePoolDT<long> dt)
    {
        BaseGoodsPoolDT GoodsDT = dt as BaseGoodsPoolDT;
        UILabel tmpName = item.Find("Name").GetComponent<UILabel>();
        UI2DSprite tmpIcon = item.Find("Icon").GetComponent<UI2DSprite>();

        ResourceCommonDT commonDT = new ResourceCommonDT();
        commonDT.f_UpdateInfo((byte)EM_ResourceType.Good, GoodsDT.m_iTempleteId, 0);
        f_RegClickEvent(tmpIcon.gameObject, UITool.f_OnItemIconClick, commonDT);

        UILabel tmpNum = item.Find("Num").GetComponent<UILabel>();
        UISprite tmpCase = item.Find("Case").GetComponent<UISprite>();
        UILabel tDesc = item.Find("Desc").GetComponent<UILabel>();

        string name = GoodsDT.m_BaseGoodsDT.szName;
        tmpCase.spriteName = UITool.f_GetImporentColorName(GoodsDT.m_BaseGoodsDT.iImportant, ref name);
        tmpName.text = name;
        tmpNum.text = string.Format("{0}", GoodsDT.m_iNum);
        tmpIcon.sprite2D = UITool.f_GetIconSprite(GoodsDT.m_BaseGoodsDT.iIcon);
        tDesc.text = GoodsDT.m_BaseGoodsDT.szReadme;

        GameObject tmpGoUrl = item.Find("GoUrl").gameObject;
        GameObject Use = item.Find("Use").gameObject;

        tmpGoUrl.SetActive(false);
        Use.SetActive(false);

        switch ((EM_ItemUse)GoodsDT.m_BaseGoodsDT.iCanUse)
        {
            case EM_ItemUse.eItemEffect_Jump:
                //tmpGoUrl.SetActive(true);
                tmpGoUrl.transform.GetChild(0).GetComponent<UILabel>().text = GoodsDT.m_BaseGoodsDT.szURL;
                UI_GoToUI(tmpGoUrl, GoodsDT.m_BaseGoodsDT.szURL);
                break;
            case EM_ItemUse.eItemEffect_BatchUse:
            case EM_ItemUse.eItemEffect_Use:
                Use.SetActive(true);
                f_RegClickEvent(Use, CommThing, GoodsDT, GoodsDT.m_iNum);
                break;
            default:
                break;
        }
    }
    private void UI_GoToUI(GameObject go, string URL)
    {
        EM_GoodsGotoPage tGotoPage = EM_GoodsGotoPage.Cardcultivate;
        //if (URL == CommonTools.f_GetTransLanguage(257))
		if (string.Equals(URL,CommonTools.f_GetTransLanguage(257)))
        {
            tGotoPage = EM_GoodsGotoPage.CardRefine;
        }
        //else if (URL == CommonTools.f_GetTransLanguage(258))
		else if (string.Equals(URL,CommonTools.f_GetTransLanguage(258)))
        {
            tGotoPage = EM_GoodsGotoPage.EquipRefine;
        }
        //else if (URL == CommonTools.f_GetTransLanguage(259))
		else if (string.Equals(URL,CommonTools.f_GetTransLanguage(259)))	
        {
            tGotoPage = EM_GoodsGotoPage.TreasureRefine;
        }
        //else if (URL == CommonTools.f_GetTransLanguage(260))
		else if (string.Equals(URL,CommonTools.f_GetTransLanguage(260)))
        {
            tGotoPage = EM_GoodsGotoPage.CardAwaken;
        }
        else if (URL == CommonTools.f_GetTransLanguage(261))
        {
            tGotoPage = EM_GoodsGotoPage.CardSky;
        }
        else if (URL == CommonTools.f_GetTransLanguage(262))
        {
            tGotoPage = EM_GoodsGotoPage.BattleFormPage;
        }
        else if (URL == CommonTools.f_GetTransLanguage(263))
        {
            tGotoPage = EM_GoodsGotoPage.Cardcultivate;
            go.SetActive(false);
        }
        else if (URL == CommonTools.f_GetTransLanguage(264))
        {
            tGotoPage = EM_GoodsGotoPage.CardUpLv;
        }
        else if (URL == CommonTools.f_GetTransLanguage(265))
        {
            tGotoPage = EM_GoodsGotoPage.Shop;
        }
        else if (URL == CommonTools.f_GetTransLanguage(266))
        {
            tGotoPage = EM_GoodsGotoPage.ValentinesDay;
        }
        else if (URL == CommonTools.f_GetTransLanguage(267))
        {
            tGotoPage = EM_GoodsGotoPage.ArtifactUp;
        }
        else if (URL == CommonTools.f_GetTransLanguage(268))
        {
            tGotoPage = EM_GoodsGotoPage.ArtifactUp;
} else if (URL == "Challenge" || URL == "Scan") {
            tGotoPage = EM_GoodsGotoPage.TrialTowerChalleng;
} else if (URL == "Withdraw") {
            tGotoPage = EM_GoodsGotoPage.SevenStarPage;
}else if(URL == "To"){
            tGotoPage = EM_GoodsGotoPage.BattleFormPage;
        }
        f_RegClickEvent(go, Btn_GotoUrl, tGotoPage);
    }

    private void Btn_GotoUrl(GameObject go, object obj1, object obj2)
    {
        EM_GoodsGotoPage tGotoPage = (EM_GoodsGotoPage)obj1;

        switch (tGotoPage)
        {
            case EM_GoodsGotoPage.CardRefine:
                CardBox tCardBox = new CardBox();
                tCardBox.m_bType = CardBox.BoxType.Refine;
                if (Data_Pool.m_CardPool.f_GetAll().Count == 1)
                    tCardBox.m_Card = Data_Pool.m_CardPool.f_GetAll()[0] as CardPoolDT;
                else
                    tCardBox.m_Card = Data_Pool.m_CardPool.f_GetAll()[1] as CardPoolDT;
                tCardBox.m_oType = CardBox.OpenType.Bag;
                if (!UITool.f_GotoPage(this, UINameConst.CardProperty, 0, tCardBox))
                {

                };
                break;
            case EM_GoodsGotoPage.CardUpLv:
                CardBox tCardBox2 = new CardBox();
                tCardBox2.m_bType = CardBox.BoxType.Inten;
                if (Data_Pool.m_CardPool.f_GetAll().Count == 1)
                    tCardBox2.m_Card = Data_Pool.m_CardPool.f_GetAll()[0] as CardPoolDT;
                else
                    tCardBox2.m_Card = Data_Pool.m_CardPool.f_GetAll()[1] as CardPoolDT;
                tCardBox2.m_oType = CardBox.OpenType.Bag;
                UITool.f_GotoPage(this, UINameConst.CardProperty, 0, tCardBox2);
                break;
            case EM_GoodsGotoPage.Cardcultivate:   //卡牌培养暂时没做
                break;
            case EM_GoodsGotoPage.CardSky:


                if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < 10)
                {
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(269));
                    return;
                }
                CardBox tCardBox3 = new CardBox();
                tCardBox3.m_bType = CardBox.BoxType.Sky;
                if (Data_Pool.m_CardPool.f_GetAll().Count == 1)
                    tCardBox3.m_Card = Data_Pool.m_CardPool.f_GetAll()[0] as CardPoolDT;
                else
                    tCardBox3.m_Card = Data_Pool.m_CardPool.f_GetAll()[1] as CardPoolDT;
                tCardBox3.m_oType = CardBox.OpenType.Bag;
                if (!UITool.f_GotoPage(this, UINameConst.CardProperty, 0, tCardBox3))
                {
                    UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(270), UITool.f_GetSysOpenLevel(EM_NeedLevel.CardSky)));
                };
                break;
            case EM_GoodsGotoPage.CardAwaken:
                CardBox tCardBox4 = new CardBox();
                tCardBox4.m_bType = CardBox.BoxType.Awaken;
                if (Data_Pool.m_CardPool.f_GetAll().Count == 1)
                    tCardBox4.m_Card = Data_Pool.m_CardPool.f_GetAll()[0] as CardPoolDT;
                else
                    tCardBox4.m_Card = Data_Pool.m_CardPool.f_GetAll()[1] as CardPoolDT;
                tCardBox4.m_oType = CardBox.OpenType.Bag;
                if (!UITool.f_GotoPage(this, UINameConst.CardProperty, 0, tCardBox4))
                {
                    UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(271), UITool.f_GetSysOpenLevel(EM_NeedLevel.CardAwaken)));
                };
                break;
            case EM_GoodsGotoPage.TreasureRefine:
                TreasureBox tTreasureBox = new TreasureBox();
                tTreasureBox.IsMastr = 0;
                tTreasureBox.IsShowChange = -1;
                for (int i = 0; i < Data_Pool.m_TreasurePool.f_GetAll().Count; i++)
                {
                    if ((Data_Pool.m_TreasurePool.f_GetAll()[i] as TreasurePoolDT).m_TreasureDT.iSite < 7 && (Data_Pool.m_TreasurePool.f_GetAll()[i] as TreasurePoolDT).m_TreasureDT.iImportant >= (int)EM_Important.Oragen)
                    {
                        tTreasureBox.tTreasurePoolDT = Data_Pool.m_TreasurePool.f_GetAll()[i] as TreasurePoolDT;
                        break;
                    }
                }
                if (tTreasureBox.tTreasurePoolDT == null)
                {
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(272));
                    return;
                }
                tTreasureBox.tType = TreasureBox.BoxType.Refine;
                if (!UITool.f_GotoPage(this, UINameConst.TreasureManage, 0, tTreasureBox))
                {
                    UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(273), UITool.f_GetSysOpenLevel(EM_NeedLevel.OpenTreasureLevel)));
                };
                break;
            case EM_GoodsGotoPage.EquipRefine:
                EquipBox tEquipBox = new EquipBox();
                tEquipBox.oType = EquipBox.OpenType.Bage;
                if (Data_Pool.m_EquipPool.f_GetAll().Count > 0)
                    tEquipBox.tEquipPoolDT = Data_Pool.m_EquipPool.f_GetAll()[0] as EquipPoolDT;
                else
                {
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(274));
                    return;
                }
                tEquipBox.tType = EquipBox.BoxTye.Refine;
                if (!UITool.f_GotoPage(this, UINameConst.EquipManage, 0, tEquipBox))
                {
                    UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(275), UITool.f_GetSysOpenLevel(EM_NeedLevel.EquipRefine)));
                };
                break;
            case EM_GoodsGotoPage.BattleFormPage:

                if (!UITool.f_GotoPage(this, UINameConst.BattleFormPage, 0))
                {
                    UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(276), UITool.f_GetSysOpenLevel(EM_NeedLevel.BattleFormLevel)));
                };
                break;
            case EM_GoodsGotoPage.Shop:
                UITool.f_GotoPage(this, UINameConst.ShopPage, 1);
                break;
            case EM_GoodsGotoPage.ValentinesDay:
                if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < 25)
                {
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(277));
                }
                else
                {
                    GameParamDT tGameParamDT = glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.ValentinesDayOpenTime) as GameParamDT;
                    int StarTime = ccMath.f_Data2Int(tGameParamDT.iParam1);
                    int EndTime = ccMath.f_Data2Int(tGameParamDT.iParam2);

                    if (GameSocket.GetInstance().f_GetServerTime() > StarTime && GameSocket.GetInstance().f_GetServerTime() < EndTime)
                    {
                        ccUIHoldPool.GetInstance().f_Hold(this);
                        ccUIManage.GetInstance().f_SendMsg(UINameConst.ValentinesDayPage, UIMessageDef.UI_OPEN);
                    }
                    else
                        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(278));
                }
                break;
            case EM_GoodsGotoPage.ArtifactUp:
            case EM_GoodsGotoPage.ArtifactRefine:
                if (Data_Pool.m_CardPool.f_GetArtifactOpenLv() > Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level))
                {
                    UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(279), Data_Pool.m_CardPool.f_GetArtifactOpenLv()));
                }
                else
                {
                    CardBox tCardBox5 = new CardBox();
                    tCardBox5.m_Card = Data_Pool.m_CardPool.mRolePoolDt;
                    tCardBox5.m_oType = CardBox.OpenType.Bag;
                    tCardBox5.m_bType = CardBox.BoxType.Artifact;
                    UITool.f_GotoPage(this, UINameConst.CardProperty, 0, tCardBox5);
                }
                break;
            case EM_GoodsGotoPage.TrialTowerSweep:
            case EM_GoodsGotoPage.TrialTowerChalleng:
                if (Data_Pool.m_TrialTowerPool.isOpen)
                {
                    if (!Data_Pool.m_TrialTowerPool.isEnter)
                    {
                        ccUIHoldPool.GetInstance().f_Hold(this);
                        ccUIManage.GetInstance().f_SendMsg(UINameConst.TrialTowerRoomPage, UIMessageDef.UI_OPEN);
                    }
                    else
                    {
                        if (!UITool.f_GetIsOpensystem(EM_NeedLevel.TritalTowerOpenLV))
                        {
UITool.Ui_Trip("Không đủ cấp độ");
                            return;
                        }
                        ccUIHoldPool.GetInstance().f_Hold(this);
                        ccUIManage.GetInstance().f_SendMsg(UINameConst.TrialTowerPage, UIMessageDef.UI_OPEN);
                    }
                }
                else {
UITool.Ui_Trip("Chưa mở");
                }

                break;

            case EM_GoodsGotoPage.SevenStarPage:
                if (!UITool.f_GetIsOpensystem(EM_NeedLevel.SevenStarOpenLv))
                {
UITool.Ui_Trip("Không đủ cấp độ");
                    return;
                }
                if (Data_Pool.m_TrialTowerPool.isOpen)
                {
                    //ccUIHoldPool.GetInstance().f_Hold(this);
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.SevenStarPage, UIMessageDef.UI_OPEN);
                }
                else {
UITool.Ui_Trip("Chưa mở");
                }

                break;
        }
    }
    //卡牌Item点击
    private void CardItemClickHandle(Transform item, BasePoolDT<long> dt)
    {
        BaseGoodsPoolDT cardDT = dt as BaseGoodsPoolDT;
print("Opened" + cardDT.m_BaseGoodsDT.szName);
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

    /// /////////////////////使用////////////////////////////////////////
    Transform _Batch;
    UILabel Batch_Name;
    UILabel Batch_OwnNum;
    GameObject Batch_Add;
    GameObject Batch_Add10;
    GameObject Batch_Minus;
    GameObject Batch_Minus10;
    UILabel Batch_Num;
    GameObject Batch_Close;
    GameObject Batch_Suc;
    UI2DSprite Batch_Icon;
    int CommNum = 1;

    void CommThing(GameObject go, object obj1, object obj2)
    {
        if ((int)obj2 == 1 || ((EM_GoodsEffect)((BaseGoodsPoolDT)obj1).m_BaseGoodsDT.iEffect) == EM_GoodsEffect.OptionalReward)
        {
            CommThingGoods(go, obj1, 1);
            return;
        }
        BaseGoodsPoolDT GoodsDT = (BaseGoodsPoolDT)obj1;
        if (null == GoodsDT)
        {
MessageBox.ASSERT("GoodsBagPage CommThing,, blank props！");
            return;
        }

        //单独判断体力和精力是否已达上限，是则不使用
        if (GoodsDT.m_iTempleteId == GameParamConst.VigorGoodId || GoodsDT.m_iTempleteId == GameParamConst.EnergyGoodId)
        {
            bool isEnergy = GoodsDT.m_iTempleteId == GameParamConst.EnergyGoodId;
            int haveNum =  isEnergy ? Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Energy) :
                Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Vigor);
            int mEnergyLimit = UITool.f_GetNowVipPrivilege((int)EM_VipPrivilege.eVip_EnergyLimit);
            int maxNum = isEnergy ? mEnergyLimit : GameParamConst.VigorMax;
            int tipsId = isEnergy ? 1951 : 1947;
            if (null != GoodsDT.m_BaseGoodsDT && haveNum + GoodsDT.m_BaseGoodsDT.iEffectData > maxNum)
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(tipsId));
                return;
            }
        }

        MutiOperateParam tParam = new MutiOperateParam("title_pilsy", EM_ResourceType.Good, GoodsDT.m_BaseGoodsDT.iId,
                   GoodsDT.m_iNum, GoodsDT.m_iNum, "", BatchUse);
        tParam.iId = GoodsDT.iId;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.MutiOperatePage, UIMessageDef.UI_OPEN, tParam);
        return;
        CommNum = 1;
        f_GetObject("Batch").SetActive(true);
        _Batch = f_GetObject("Batch").transform;
        Batch_Add = _Batch.Find("Add").gameObject;
        Batch_Add10 = _Batch.Find("Add10").gameObject;
        Batch_Minus = _Batch.Find("Minus").gameObject;
        Batch_Minus10 = _Batch.Find("Minus10").gameObject;
        Batch_Close = _Batch.Find("Close").gameObject;
        Batch_Suc = _Batch.Find("Suc").gameObject;
        _Batch = f_GetObject("Batch").transform;
        Batch_Name = _Batch.Find("Name").GetComponent<UILabel>();
        Batch_OwnNum = _Batch.Find("own/Num").GetComponent<UILabel>();
        Batch_Num = _Batch.Find("NumBg/Num").GetComponent<UILabel>();
        Batch_Icon = _Batch.Find("Icon").GetComponent<UI2DSprite>();

       
        Batch_Name.text = GoodsDT.m_BaseGoodsDT.szName;
        Batch_Icon.sprite2D = UITool.f_GetIconSprite(GoodsDT.m_BaseGoodsDT.iIcon);
        Batch_OwnNum.text = GoodsDT.m_iNum.ToString();
        Batch_Num.text = CommNum.ToString();
        f_RegClickEvent(Batch_Add, OnckileCount, 1, GoodsDT);
        f_RegClickEvent(Batch_Add10, OnckileCount, 10, GoodsDT);
        f_RegClickEvent(Batch_Minus, OnckileCount, -1, GoodsDT);
        f_RegClickEvent(Batch_Minus10, OnckileCount, -10, GoodsDT);

        f_RegClickEvent(Batch_Close, (GameObject go1, object obj11, object obj21) => { f_GetObject("Batch").SetActive(false); });
        f_RegClickEvent(Batch_Suc, CommThingGoods, GoodsDT);
    }

    void OnckileCount(GameObject go, object obj1, object obj2)
    {
        BaseGoodsPoolDT GoodsDT = (BaseGoodsPoolDT)obj2;
        CommNum += (int)obj1;
        if (CommNum <= 1)
            CommNum = 1;
        if (CommNum >= GoodsDT.m_iNum)
            CommNum = GoodsDT.m_iNum;
        Batch_Num.text = CommNum.ToString();
    }
    void OneCommThing(GameObject go, object obj1, object obj2)
    {
        CommThingGoods(go, obj1, 1);
    }
    int _OnckileGiftid = 0;    //礼包ID
    bool _IsFixedReward = false;
    void CommThingGoods(GameObject go, object obj1, object obj2)
    {
        BaseGoodsPoolDT GoodsDT = (BaseGoodsPoolDT)obj1;
        _OnckileGiftid = GoodsDT.m_BaseGoodsDT.iEffectData;
        int times;
        if (obj2 == null)
            times = CommNum;
        else
            times = (int)obj2;
        if (times == 0 || times > GoodsDT.m_iNum)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(280));
            return;
        }
        if (GoodsDT.m_BaseGoodsDT.iEffect == (int)EM_GoodsEffect.GetPhysical)
        {
            if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Energy) + GoodsDT.m_BaseGoodsDT.iEffectData > 500)
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(281));
                return;
            }
        }
        if ((EM_GoodsEffect)GoodsDT.m_BaseGoodsDT.iEffect == EM_GoodsEffect.OptionalReward)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.SelectAward, UIMessageDef.UI_OPEN, GoodsDT);
            _IsFixedReward = false;
            return;
        }
        else if ((EM_GoodsEffect)GoodsDT.m_BaseGoodsDT.iEffect == EM_GoodsEffect.FixedReward)
            _IsFixedReward = true;
        else
            _IsFixedReward = false;
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT back = new SocketCallbackDT();
        back.m_ccCallbackFail = UseFail;
        back.m_ccCallbackSuc = UseSuc;
        Data_Pool.m_BaseGoodsPool.f_Use(GoodsDT.iId, times, 0, back);
    }

    private void BatchUse(long iId, EM_ResourceType type, int resourceId, int resourceCount, int UseCount) {
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT back = new SocketCallbackDT();
        back.m_ccCallbackFail = UseFail;
        back.m_ccCallbackSuc = UseSuc;
        Data_Pool.m_BaseGoodsPool.f_Use(iId, UseCount, 0, back);
    }
    
    void UseSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(282));
        //List<AwardPoolDT> tGoods2 = Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(_OnckileGiftid, false, CommNum);
        List<AwardPoolDT> tGoods = Data_Pool.m_AwardPool.m_GetLoginAward;
        CommNum = 1;
        mCardWrapComponet.f_UpdateView();
        UpdateViewInfo();
        f_GetObject("Batch").SetActive(false);
        if (tGoods.Count >= 1)
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN, new object[] { tGoods });
    }
    void UseFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(283));
        CommNum = 1;
        mCardWrapComponet.f_UpdateView();
        UpdateViewInfo();
        f_GetObject("Batch").SetActive(false);
    }

}
