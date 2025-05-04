using ccU3DEngine;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 移魂阵
/// </summary>
public class CardChangePage : UIFramwork
{
    private string strTexBgRoot = "UI/TextureRemove/Tex_MainMenuBg/Texture_CardChangeBg";
    private GameObject _cardItemParent;//卡牌父节点
    private GameObject _cardItem;//卡牌预设
    private GameObject _btnWei;//魏国
    private GameObject _btnShu;//蜀国
    private GameObject _btnWu;//吴国
    private GameObject _btnQun;//群雄
    private Transform _leftCardModel;//变身前模型
    private Transform _rightCardModel;//变身后模型
    private GameObject _btnAddChangeCard;//添加变身后模型
    private GameObject _btnChangeCard;//变身按钮
    private UILabel _syceeText;//元宝消耗
    private UILabel _jiangHunText;//将魂消耗
    private UILabel _tipsText;//卡牌消耗文本
    private GameObject _btnHelp;//帮助
    private GameObject _cardPopPanel;//变身后武将选择界面
    private GameObject _btnCloseSeleChange;//关闭变身选择
    private GameObject _changeAfterParent;//选择卡父节点
    private GameObject _changeAfterItem;//选择卡预设
    private int _cardID;//需要转换卡牌ID
    private int _targetID;//目标卡牌
    private int _costSysceeCount;//消耗元宝数量
    private int _generalSoulCount;//消耗将魂数量
    private long[] _arrayCostCardSeverId;//卡牌消耗服务器id
    private List<BasePoolDT<long>> ScrCardList;
    private List<BasePoolDT<long>> _selectCardData;
    private List<BasePoolDT<long>> _listModelData;
    private List<BasePoolDT<long>> _cardList;//卡牌
    private UIWrapComponent Card_WrapComponet;
    private UIWrapComponent CardSelect_WrapComponet;
    private string tipsContent = string.Empty;
    private string HELP_CONTENT = string.Empty;
    //初始化信息
    protected override void f_InitMessage()
    {
        base.f_InitMessage();

        tipsContent = CommonTools.f_GetTransLanguage(1192);
        HELP_CONTENT = CommonTools.f_GetTransLanguage(1193);

        InitUI();

        f_RegClickEvent(f_GetObject("Back"), OnCloseClick);
        f_RegClickEvent(_btnWei, OnPageItemClick, EM_CardCamp.eCardWei);
        f_RegClickEvent(_btnShu, OnPageItemClick, EM_CardCamp.eCardShu);
        f_RegClickEvent(_btnWu, OnPageItemClick, EM_CardCamp.eCardWu);
        f_RegClickEvent(_btnQun, OnPageItemClick, EM_CardCamp.eCardGroupHero);
        f_RegClickEvent(_btnCloseSeleChange, OnDestorySelectCardClick);
        f_RegClickEvent(_btnChangeCard, OnChangeClick);
        f_RegClickEvent(_btnAddChangeCard, OnAddClick);
        f_RegClickEvent(_btnHelp, OnHelpClick);
    }

    //初始化ui
    private void InitUI()
    {
        _cardItemParent = f_GetObject("SeleCardItemParent");
        _cardItem = f_GetObject("SeleCardItem");
        _btnWei = f_GetObject("BtnWei");
        _btnShu = f_GetObject("BtnShu");
        _btnWu = f_GetObject("BtnWu");
        _btnQun = f_GetObject("BtnQun");
        _leftCardModel = f_GetObject("LeftCardModel").transform;
        _rightCardModel = f_GetObject("RightCardModel").transform;
        _btnAddChangeCard = f_GetObject("AddCardMagic");
        _btnChangeCard = f_GetObject("BtnChange");
        _syceeText = f_GetObject("SyceeText").GetComponent<UILabel>();
        _jiangHunText = f_GetObject("JiangHunText").GetComponent<UILabel>();
        _tipsText = f_GetObject("LabelCardXHName").GetComponent<UILabel>();
        _btnHelp = f_GetObject("Btn_Help");
        _cardPopPanel = f_GetObject("ChangeCardItem");
        _btnCloseSeleChange = f_GetObject("CardAlphe");
        _changeAfterParent = f_GetObject("CardSelectItemParent");
        _changeAfterItem = f_GetObject("CardSelectItem");
    }

    //数据初始化
    private void InitData()
    {
        _targetID = 0;
        _cardID = 0;
        _costSysceeCount = 0;
        _generalSoulCount = 0;
        _syceeText.text = "0";
        _jiangHunText.text = "0";
        _arrayCostCardSeverId = null;
        SetTapState(_btnWei, false);
        SetTapState(_btnShu, false);
        SetTapState(_btnWu, false);
        SetTapState(_btnQun, false);
        //if (_leftCardModel.Find("Model") != null)
        //    UITool.f_DestoryStatelObject(_leftCardModel.Find("Model").gameObject);
        //if (_rightCardModel.Find("Model") != null)
        //    UITool.f_DestoryStatelObject(_rightCardModel.Find("Model").gameObject);
    }

    //打开界面
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        f_LoadTexture();
        InitMoneyInfo();

        _cardList = f_GetCard();//获取可转换卡牌
        f_SetCanChangeCardInfo();//设置转换卡信息
    }

    //关闭界面
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        InitData();
        if (_leftCardModel.Find("Model") != null)
            UITool.f_DestoryStatelObject(_leftCardModel.Find("Model").gameObject);
        if (_rightCardModel.Find("Model") != null)
            UITool.f_DestoryStatelObject(_rightCardModel.Find("Model").gameObject);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
    }

    protected override void UI_HOLD(object e)
    {
        base.UI_HOLD(e);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
    }

    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        InitMoneyInfo();
    }

    
    // 加载texture
    private void f_LoadTexture()
    {
        //加载背景图
        UITexture TexBg = f_GetObject("TexBg").GetComponent<UITexture>();
        if (TexBg.mainTexture == null)
        {
            Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot);
            TexBg.mainTexture = tTexture2D;
        }
    }

    //初始化货币信息
    private void InitMoneyInfo()
    {
        List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
        listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
        listMoneyType.Add(EM_MoneyType.eUserAttr_Money);
        listMoneyType.Add(EM_MoneyType.eUserAttr_GeneralSoul);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, listMoneyType);
    }

    //获取消耗卡数量
    private int CardNum(CardPoolDT dt)
    {
        int tCardNum = 0;
        List<BasePoolDT<long>> tList = Data_Pool.m_CardPool.f_GetAllForData1(dt.m_CardDT.iId);
        CardPoolDT tCardPoolDT;
        for (int i = 0; i < tList.Count; i++)
        {
            tCardPoolDT = tList[i] as CardPoolDT;
            if (Data_Pool.m_TeamPool.f_CheckInTeamByKeyId(tCardPoolDT.iId))
                continue;
            if (Data_Pool.m_TeamPool.dicReinforceCardId.ContainsValue(tCardPoolDT))
                continue;
            //进阶等级  经验  等级  领悟等级  天命等级 领悟道具 天命经验
            if (tCardPoolDT.m_iEvolveLv == 0 && tCardPoolDT.m_iExp == 0 && tCardPoolDT.m_iLv == 1 && tCardPoolDT.m_iLvAwaken == 0 && tCardPoolDT.uSkyDestinyLv == 0 && tCardPoolDT.m_iFlagAwaken == 0 && tCardPoolDT.uSkyDestinyExp == 0)
            {
                tCardNum++;
            }

        }
        if (tCardNum - 1 < 0)
            tCardNum = 0;
        return tCardNum;
    }

    //获取当前能转换的卡牌信息
    private List<BasePoolDT<long>> f_GetCard()
    {
        List<BasePoolDT<long>> _cardListArr = Data_Pool.m_CardPool.f_GetAll();
        Data_Pool.m_CardPool.f_SortList();
        List<BasePoolDT<long>>  CardList = new List<BasePoolDT<long>>();
        List<NBaseSCDT> listDT = glo_Main.GetInstance().m_SC_Pool.m_TransmigrationCardSC.f_GetAll();
        ScrCardList = new List<BasePoolDT<long>>();
        for (int i =0;i< _cardListArr.Count; i++)
        {
            TransmigrationCardPoolDT changeDataDT = f_GetCardDT((long)(_cardListArr[i] as CardPoolDT).m_CardDT.iId);
            if (changeDataDT == null)
                continue;
            if (Data_Pool.m_TeamPool.f_CheckInTeamByKeyId((_cardListArr[i] as CardPoolDT).iId))
                continue;
            else
            {
                if (Data_Pool.m_TeamPool.dicReinforceCardId.ContainsValue(_cardListArr[i] as CardPoolDT) ||
                    ((_cardListArr[i] as CardPoolDT).m_iLv > 1) )  
                    continue;
                else
                {
                    int countCard = CardNum((_cardListArr[i] as CardPoolDT));//Data_Pool.m_CardPool.f_GetHaveNumByTemplate((_cardListArr[i] as CardPoolDT).m_CardDT.iId);
                    if (countCard >= changeDataDT.m_TransmigrationCardData.iCost)
                    {
                        CardList.Add(_cardListArr[i]);
                        ScrCardList.Add(_cardListArr[i]);
                    }
                }
            }
        }
        //排除相同卡牌
        for (int i = 0; i < CardList.Count; i++)
        {
            int sum = 0;
            CardPoolDT card1 = (CardPoolDT)CardList[i];
            for (int j = CardList.Count -1; j > i; j--)
            {
                CardPoolDT card2 = (CardPoolDT)CardList[j];
                if (card1.m_CardDT.iId == card2.m_CardDT.iId)
                {
                    sum = sum + 1;
                    CardList.RemoveAt(j);
                }
            }
        }
        return CardList;
    }


    //根据模板id获取服务器id
    private long[] f_GetSeverCardId(int templetId)
    {
        TransmigrationCardPoolDT changeDataDT = f_GetCardDT((long)templetId);
        List<BasePoolDT<long>> cardSeverIDList = new List<BasePoolDT<long>>();
        long[] severCardId = new long[changeDataDT.m_TransmigrationCardData.iCost];
        for (int i =0;i< ScrCardList.Count;i++)
        {
            CardPoolDT card = (CardPoolDT)ScrCardList[i];
            if (templetId == card.m_CardDT.iId)
            {
                BasePoolDT<long> temp = new BasePoolDT<long>();
                temp.iId = card.iId;
                if (Data_Pool.m_TeamPool.f_CheckInTeamByKeyId(card.iId))
                    continue;
                else
                    cardSeverIDList.Add(temp);
            }
        }
        for (int i =0;i< cardSeverIDList.Count;i++)
        {
            if (cardSeverIDList.Count < severCardId.Length)
                return severCardId;
            if (i+1 <= severCardId.Length )
            {
                severCardId[i] = cardSeverIDList[i].iId;
            }
        }
        return severCardId;
    }

    //根据卡牌id获得卡牌表格数据
    private TransmigrationCardPoolDT f_GetCardDT(long id)
    {
        TransmigrationCardPoolDT poolDT = Data_Pool.m_TransmigrationCardPool.f_GetForId(id) as TransmigrationCardPoolDT;
        return poolDT;
    }

    //设置可转换的卡牌链表信息
    private void f_SetCanChangeCardInfo()
    {
        _tipsText.text = tipsContent;
        if (Card_WrapComponet == null)
        {
            Card_WrapComponet = new UIWrapComponent(170, 1, 350, 8, _cardItemParent, _cardItem, _cardList, OnContentCardItemUpdate, CardClick);
        }
        Card_WrapComponet.f_UpdateList(_cardList);
        Card_WrapComponet.f_ResetView();
        
    }
    
    //卡牌信息回调
    private void OnContentCardItemUpdate(Transform item,BasePoolDT<long> dt)
    {
        CardPoolDT card = (CardPoolDT)dt;
        UI2DSprite Icon = item.Find("Icon").GetComponent<UI2DSprite>();
        UILabel Name = item.Find("Name").GetComponent<UILabel>();
        UILabel Count = item.Find("Lv").GetComponent<UILabel>();
        UISprite Important = item.Find("Important").GetComponent<UISprite>();
        GameObject spriteXiYou = item.Find("XiYouSprite").gameObject;
        spriteXiYou.SetActive(f_GetIsRare(card.m_CardDT.iId));
        string tname = card.m_CardDT.szName;
        Icon.sprite2D = UITool.f_GetIconSpriteByCardId(card);
        Name.text = tname;
        Important.spriteName = UITool.f_GetImporentColorName(card.m_CardDT.iImportant, ref tname);
        //卡牌数量
        int countCard = CardNum(card);//UITool.f_GetResourceHaveNum((int)EM_ResourceType.Card, (int)card.m_CardDT.iId);
        Count.text = string.Format("[f1bf49]{0}[e0d5b8]{1}", CommonTools.f_GetTransLanguage(1194), countCard);
    }

    //卡牌选项卡点击事件回调
    private void CardClick(Transform item, BasePoolDT<long> dt)
    {
        CardPoolDT card = (CardPoolDT)dt;
        _cardID = (int)card.m_CardDT.iId;
        TransmigrationCardPoolDT changeDataDT = f_GetCardDT((long)card.m_CardDT.iId);
        _costSysceeCount = CommonTools.f_GetListCommonDT(changeDataDT.m_TransmigrationCardData.szCost)[0].mResourceNum;
        _generalSoulCount = CommonTools.f_GetListCommonDT(changeDataDT.m_TransmigrationCardData.szCost)[1].mResourceNum;
        _syceeText.text = _costSysceeCount.ToString();
        _jiangHunText.text = _generalSoulCount.ToString();
        f_LoadModel(EM_ChangeState.LeftCard,(int)card.m_CardDT.iId);
        _arrayCostCardSeverId = f_GetSeverCardId(_cardID);
        if (_rightCardModel.Find("Model") != null)
            UITool.f_DestoryStatelObject(_rightCardModel.Find("Model").gameObject);

    }


    //加载模型
    private void f_LoadModel(EM_ChangeState state,int modelId)
    {
        if (state == EM_ChangeState.LeftCard)
        {
            if (_leftCardModel.Find("Model") != null)
                UITool.f_DestoryStatelObject(_leftCardModel.Find("Model").gameObject);
            GameObject card = UITool.f_GetStatelObject(modelId, _leftCardModel, Vector3.zero, Vector3.zero);
        }
        else if (state == EM_ChangeState.RightCard)
        {
            if (_rightCardModel.Find("Model") != null)
                UITool.f_DestoryStatelObject(_rightCardModel.Find("Model").gameObject);
            GameObject card = UITool.f_GetStatelObject(modelId, _rightCardModel, Vector3.zero, Vector3.zero);
        }  
    }

    //关闭界面点击事件
    private void OnCloseClick(GameObject go, object value1, object value2)
    {
        if (_leftCardModel.Find("Model") != null)
            UITool.f_DestoryStatelObject(_leftCardModel.Find("Model").gameObject);
        if (_rightCardModel.Find("Model") != null)
            UITool.f_DestoryStatelObject(_rightCardModel.Find("Model").gameObject);
        InitData();
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CardChangePage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);

    }


    // 点击了分页按钮
    private void OnPageItemClick(GameObject go, object obj1, object obj2)
    {
        if (_leftCardModel.childCount == 0)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1195));
            return;
        }
        EM_CardCamp cardCamp = (EM_CardCamp)obj1;//卡牌阵营
        SetTapState(_btnWei, cardCamp == EM_CardCamp.eCardWei);
        SetTapState(_btnShu, cardCamp == EM_CardCamp.eCardShu);
        SetTapState(_btnWu, cardCamp == EM_CardCamp.eCardWu);
        SetTapState(_btnQun, cardCamp == EM_CardCamp.eCardGroupHero);
        f_GetCardModelID(cardCamp);
        ShowSelectCard();
    }

    // 设置分页状态
    private void SetTapState(GameObject tapItem, bool isPress)
    {
         tapItem.transform.Find("Normal").gameObject.SetActive(!isPress);
         tapItem.transform.Find("Press").gameObject.SetActive(isPress);
    }

    //加号提示
    private void OnAddClick(GameObject go,object obj1,object obj2)
    {
        if (_rightCardModel.Find("Model") == null)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1196));
            return;
        }
    }
    //帮助事件
    private void OnHelpClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonHelpPage, UIMessageDef.UI_OPEN, 9);
    }


    //转换按钮事件
    private void OnChangeClick(GameObject go,object obj1,object obj2)
    {
        if (_leftCardModel.Find("Model") == null || (_rightCardModel.Find("Model") == null))
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1197));
            return;
        }
        TransmigrationCardPoolDT changeDataDT = f_GetCardDT((long)_cardID);
        int syceeCount = UITool.f_GetResourceHaveNum((int)EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_Sycee);
        int generalSoulCount= UITool.f_GetResourceHaveNum((int)EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_GeneralSoul);
        if ((syceeCount >= _costSysceeCount) && (generalSoulCount >= _generalSoulCount))
        {
            //满足条件 转换卡牌请求
            SocketCallbackDT callbackDT = new SocketCallbackDT();
            callbackDT.m_ccCallbackSuc = f_GetChangeCardSuc;
            callbackDT.m_ccCallbackFail = f_GetChangeCardFail;
            Data_Pool.m_TransmigrationCardPool.f_GetChangeCard(_targetID, changeDataDT.m_TransmigrationCardData.iCost, _arrayCostCardSeverId, callbackDT);
            UITool.f_OpenOrCloseWaitTip(true);
        }
        else
        {
            if(syceeCount < _costSysceeCount && (generalSoulCount < _generalSoulCount))
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1198));
            }
            else if (syceeCount < _costSysceeCount)
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1199));
            }
            else
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1200));
            }
        }
    }

    //请求转换成功
    private void f_GetChangeCardSuc(object obj)
    {
        //更新卡牌转换链表数据
        _cardList = f_GetCard();
        f_SetCanChangeCardInfo();
        //UITool.Ui_Trip("转换成功");
        UITool.f_OpenOrCloseWaitTip(false);
        ccUIHoldPool.GetInstance().f_Hold(this);
        EquipSythesis tCardSynthesis = new EquipSythesis(_targetID, 1, EquipSythesis.ResonureType.Card);
        tCardSynthesis.isShowBotton = false;
        InitData();
        if (_leftCardModel.Find("Model") != null)
            UITool.f_DestoryStatelObject(_leftCardModel.Find("Model").gameObject);
        if (_rightCardModel.Find("Model") != null)
            UITool.f_DestoryStatelObject(_rightCardModel.Find("Model").gameObject);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainEquipShowPage, UIMessageDef.UI_OPEN, tCardSynthesis);
    }

    //请求转换失败
    private void f_GetChangeCardFail(object obj)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1201) + CommonTools.f_GetTransLanguage((int)obj));
        UITool.f_OpenOrCloseWaitTip(false);
    }

    //变身前   变身后
    private enum EM_ChangeState
    {
        LeftCard,//变身前
        RightCard,//变身后
    }

    //显示选择武将界面
    private void ShowSelectCard()
    {
        _cardPopPanel.SetActive(true);
        f_SetChangeAfterInfo();
    }

    //隐藏选择武将界面
    private void OnDestorySelectCardClick(GameObject go, object obj1, object obj2)
    {
        _cardPopPanel.SetActive(false);
    }

    //变身后武将选择信息
    private void f_SetChangeAfterInfo()
    {
        TransmigrationCardPoolDT changeDataDT = f_GetCardDT((long)_cardID);
        
        if (CardSelect_WrapComponet == null)
        {
            CardSelect_WrapComponet = new UIWrapComponent(200, 1, 770, 5,
            _changeAfterParent, _changeAfterItem, _selectCardData, CardChangeSelectInfo, CardChangeSelectClick);
        }
        CardSelect_WrapComponet.f_UpdateList(_selectCardData);
        CardSelect_WrapComponet.f_ResetView();
    }

    //根据id分割不同国家的卡牌id
    private void f_GetCardModelID(EM_CardCamp state)
    {
        TransmigrationCardPoolDT changeDataDT = f_GetCardDT((long)_cardID);

        if (changeDataDT == null)
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1202));
        _selectCardData = changeDataDT.f_GetTransmigrationCampArr(state);
    }


    //变身选择设置卡牌信息回调
    private void CardChangeSelectInfo(Transform item, BasePoolDT<long> dt)
    {
        SelectCardData selectData = (SelectCardData)dt;
        CardDT card = (CardDT)selectData.cardDt;
        UI2DSprite Icon = item.Find("Icon").GetComponent<UI2DSprite>();
        UILabel Name = item.Find("Name").GetComponent<UILabel>();
        UILabel Lv = item.Find("Lv").GetComponent<UILabel>();
        UILabel SkyLife = item.Find("SkyLift").GetComponent<UILabel>();
        UISprite Important = item.Find("Important").GetComponent<UISprite>();
        GameObject rareSprite = item.Find("XiYouSprite").gameObject;
        string tname = card.szName;
        Icon.sprite2D = UITool.f_GetIconSprite(card.iStatelId1);//UITool.f_GetIconSpriteByCardId(card);
        Name.text = tname;
        Important.spriteName = UITool.f_GetImporentColorName(card.iImportant, ref tname);
        Lv.text = string.Format("{0}：{1}", "[f5bf3d]Lv[-]", "1");
        SkyLife.text = string.Format("{0}：{1}", CommonTools.f_GetTransLanguage(1203), "1");
        rareSprite.SetActive(f_GetIsRare(selectData.CardId));
    }

    //变身选择确定界面
    private void CardChangeSelectClick(Transform item, BasePoolDT<long> dt)
    {
        SelectCardData selectData = (SelectCardData)dt;
        CardDT card = (CardDT)selectData.cardDt;
        _targetID = selectData.CardId;
        OnSelectClick(item.gameObject, selectData.CardId, null);
    }

    //根据卡牌id判断卡牌是否为稀有
    private bool f_GetIsRare(int cardId)
    {
        if (cardId == 1105 || cardId == 1108 || cardId == 1210 || cardId == 1203 || 
            cardId == 1309 || cardId == 1307 || cardId == 1402 || cardId == 1408)
        {
            return true;
        }
        else
            return false;
    }

    //选择卡牌事件
    private void OnSelectClick(GameObject go,object obj1,object obj2)
    {
        int cardId = (int)obj1;
        f_LoadModel(EM_ChangeState.RightCard, cardId);
        _cardPopPanel.SetActive(false);
    }

}
