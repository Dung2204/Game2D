using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System;
using System.Collections.Generic;

public class CardProperty : UIFramwork
{

    BetterList<BaseGoodsPoolDT> GoodsList;    //经验石道具
    int CardStarLv;     //卡牌开始等级
    CardPoolDT _Card;
    List<BasePoolDT<long>> GoodsPool;   //道具Pool
    private UILabel Ui_Exp;
    public static bool s_isShow;
    int CardInList = 0;    //当前卡牌所在位置
    GameObject CardReson;    //卡牌模型
    int NeedGold;    //需要多少银币    一共添加了多少经验
    long[] NeedExpiId;   //强化道具的ID

    RoleProperty Pro;   //属性
    Transform Show;    //进阶属性窗口

    Stack<CardBox> stackCardBox = new Stack<CardBox>();
    CardBox.OpenType tBoxType;
    CardBox.BoxType tBoxTabType;
    CardBox.BoxType tmpSkyType;
    //特效
    GameObject Inten_CardParticle;   //卡牌的特效
    GameObject Inten_BaseGoodsEffect; //道具的特效
    Transform EffectParent;     //所有特效的父级

    GameObject Evolve_CardEffect;   //进阶卡牌特效
    GameObject Evolve_ProEffect;    //进阶属性特效

    GameObject Artifact_GoodsEffect;  //神器特效
    GameObject Artifact_GoodsUpEffect;   //神器升级特效

    bool IsRefine = true;
    bool IsUpLevel = true;
    private const int AwakeId = 106;  //觉醒丹id

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        f_InitArtifactView();
    }

    protected override void UI_OPEN(object e)
    {
        //UI_Empty();
        base.UI_OPEN(e);
        f_GetObject("UpLv").SetActive(false);
        f_GetObject("Sky").SetActive(false);
        f_GetObject("Awanken").SetActive(false);
        f_GetObject("ShowFightType").SetActive(false);
        CardBox tmp = (CardBox)e;
        stackCardBox.Push(tmp);
        tBoxType = tmp.m_oType;
        _Card = tmp.m_Card;
        tBoxTabType = tmp.m_bType;
        UpdateCardPro();
        UpdateIntroduce();
        IninAwaken();
        UI_ChangeUI(tBoxTabType);
        SetBtnShow(tmp.m_oType);
        EffectParent = f_GetObject("Particle").transform;
        CheckRedPoint();
        f_LoadTexture();
    }

    private string strTexArtifactBgRoot = "UI/TextureRemove/MainMenu/Tex_ArtifactBg";
    private string strTexArtifactEffect = "UI/TextureRemove/MainMenu/Tex_ArtifactEffect";
    private string strTexArtifact = "UI/TextureRemove/MainMenu/Tex_Artifact";


    private void f_LoadTexture()
    {
        //加载背景图
        UITexture Texture_ArtifactBg = f_GetObject("ArtifactTexBg").GetComponent<UITexture>();
        UITexture Texture_ArtifactEffect = f_GetObject("ArtifactEffect").GetComponent<UITexture>();
        UITexture Texture_Artifact = f_GetObject("Artifact").GetComponent<UITexture>();
        if (Texture_ArtifactBg.mainTexture == null)
        {
            //Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexArtifactBgRoot);
            //Texture_ArtifactBg.mainTexture = tTexture2D;
        }
        if (Texture_ArtifactEffect.mainTexture == null)
        {
            Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexArtifactEffect);
            Texture_ArtifactEffect.mainTexture = tTexture2D;
        }
        if (Texture_Artifact.mainTexture == null)
        {
            Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexArtifact);
            Texture_Artifact.mainTexture = tTexture2D;
        }

    }

    private void SetBtnShow(CardBox.OpenType tmp)
    {
        f_GetObject("GetWayBtn").SetActive(tmp == CardBox.OpenType.handbook || tmp == CardBox.OpenType.SelectAward);
        f_GetObject("FateBtn").SetActive((tmp == CardBox.OpenType.handbook || tmp == CardBox.OpenType.SelectAward) && _Card.m_CardDT.iImportant >= (int)EM_Important.Blue);
        f_GetObject("Chage").SetActive(tmp == CardBox.OpenType.battleArray);
        f_GetObject("UpBtn").SetActive(tmp == CardBox.OpenType.Bag || tmp == CardBox.OpenType.battleArray);
        f_GetObject("MForwardBtn").SetActive(tmp == CardBox.OpenType.Bag || tmp == CardBox.OpenType.battleArray);
        f_GetObject("Arrow_Lift").SetActive(tmp == CardBox.OpenType.battleArray);
        f_GetObject("Arrow_Rigt").SetActive(tmp == CardBox.OpenType.battleArray);
        f_GetObject("AwakenBtn").SetActive((tmp == CardBox.OpenType.Bag || tmp == CardBox.OpenType.battleArray) && Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) >= UITool.f_GetSysOpenLevel(EM_NeedLevel.CardAwaken));
        f_GetObject("SkyBtn").SetActive((tmp == CardBox.OpenType.Bag || tmp == CardBox.OpenType.battleArray) && Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) >= UITool.f_GetSysOpenLevel(EM_NeedLevel.CardSky));
        f_GetObject("ArtifactBtn").SetActive((tmp == CardBox.OpenType.Bag || tmp == CardBox.OpenType.battleArray) && Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) >= Data_Pool.m_CardPool.f_GetArtifactOpenLv());
        UpdateCard(tmp == CardBox.OpenType.handbook || tmp == CardBox.OpenType.SelectAward || tmp == CardBox.OpenType.CardBattlePage);

    }
    void UpdateCardPro()
    {
        bool[] tbool = { false, false, false, false, false, false, false, false, false, false, false};
        Pro = null;
        Pro = RolePropertyTools.f_Disp(_Card, null, -99, null, null, tbool);
        f_GetObject("Ce_Label").GetComponent<UILabel>().text = RolePropertyTools.f_GetBattlePower(Pro).ToString();
        Introduce_AwakenStar = f_GetObject("AwakenStar").transform;
        if (_Card.m_iLv >= 50)
        {
            Introduce_AwakenStar.gameObject.SetActive(true);
            f_GetObject("AwakenStarBg").SetActive(true);
            for (int i = 0; i < Introduce_AwakenStar.childCount; i++)
                AwakenStar[i] = Introduce_AwakenStar.GetChild(i).GetComponent<UISprite>();
            UITool.f_UpdateStarNum(AwakenStar, _Card.m_iLvAwaken / 10, "Icon_DungeonStar2");
        }
        else
        {
            Introduce_AwakenStar.gameObject.SetActive(false);
            f_GetObject("AwakenStarBg").SetActive(false);
        }
        Pro = RolePropertyTools.f_Disp(_Card, null, -99, null, null);

    }

    /// <summary>
    /// 消息
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("ExitBtn", UI_ExitBtn);
        //f_RegClickEvent("Up", Card_Upgrade);
        f_RegClickEvent("ExitSeleBtn", UI_CloseSelect);
        f_RegClickEvent("MF", Card_Evolve);
        f_RegClickEvent("Succeed_Card", UI_HideSucceed);
        f_RegClickEvent("Chage", UI_Chage);
        Transform Food = f_GetObject("Food").transform;
        for (int i = 0; i < Food.childCount; i++)
        {
            f_RegClickEvent(Food.GetChild(i).gameObject, UI_OpenSelect);
        }
        f_RegClickEvent("AddExp", UI_CardUpOne, 1);
        f_RegClickEvent("Arrow_Lift", UI_Arrlift);
        f_RegClickEvent("Arrow_Rigt", UI_ArrRigt);
        f_RegClickEvent("IntroduceBtn", UI_ChageTab, CardBox.BoxType.Intro);
        f_RegClickEvent("AwakenBtn", UI_ChageTab, CardBox.BoxType.Awaken);
        f_RegClickEvent("AwakenEquipInduce_ABg", UI_CloseAwakenEquipIndecu);
        f_RegClickEvent("UpBtn", UI_ChageTab, CardBox.BoxType.Inten);
        f_RegClickEvent("MForwardBtn", UI_ChageTab, CardBox.BoxType.Refine);
        f_RegClickEvent("Up5", UI_CardUpFive, 5);
        f_RegClickEvent("SkyBtn", UI_ChageTab, CardBox.BoxType.Sky);
        f_RegClickEvent("GetWayBtn", UI_ChageTab, CardBox.BoxType.GetWay);
        f_RegClickEvent("FateBtn", UI_ChageTab, CardBox.BoxType.Fate);
        f_RegClickEvent("ArtifactBtn", UI_ChageTab, CardBox.BoxType.Artifact);
        f_RegClickEvent("Sky_SkyBtn", Btn_Sky);
        f_RegClickEvent("CardFight_Type", UpdateCardFithtLabel);
        f_RegClickEvent(f_GetObject("ShowFightType").transform.Find("Sprite").gameObject, CloseCardFightLabel);
    }
    #region 红点提示
    private void CheckRedPoint()
    {
        bool isCanLvUp = false;
        bool isCanEnvolve = false;
        Data_Pool.m_TeamPool.f_CheckTeamCardRedPoint(_Card, ref isCanLvUp, ref isCanEnvolve);
        //可升级红点
        UITool.f_UpdateReddot(f_GetObject("UpBtn"), isCanLvUp ? 1 : 0, new Vector3(100, 28, 0), 102);
        //可进阶红点
        UITool.f_UpdateReddot(f_GetObject("MForwardBtn"), isCanEnvolve ? 1 : 0, new Vector3(100, 28, 0), 102);
    }
    #endregion
    /// <summary>
    /// 更换
    /// </summary>
    private void UI_Chage(GameObject go, object obj1, object obj2)
    {
        if (Data_Pool.m_CardPool.f_CheckHasCardLeft())
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.SelectCardPage, UIMessageDef.UI_OPEN);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.CardProperty, UIMessageDef.UI_CLOSE);
            ccUIHoldPool.GetInstance().f_UnHold();
        }
        else
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(103));
        }
    }

    private void UI_ChageTab(GameObject go, object obj1, object obj2)
    {
        UI_ChangeUI((CardBox.BoxType)obj1);
    }

    private void UI_ChangeUI(CardBox.BoxType tBox, bool Sky = true)
    {
        if (!IsRefine || !IsUpLevel)
            return;
        if (tBoxTabType == CardBox.BoxType.Sky && tBox != CardBox.BoxType.Sky && Sky && _Card.uSkyDestinyExp != 0)
        {
            tmpSkyType = tBox;
            _SkyChangeUI(UI_TripSkyZeor);
            //PopupMenuParams tLabel = new PopupMenuParams(CommonTools.f_GetTransLanguage(104), CommonTools.f_GetTransLanguage(105), "留在此界面",null, CommonTools.f_GetTransLanguage(106), UI_TripSkyZeor);
            //ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tLabel);
            return;
        }
        isSendSky = false;
        ccTimeEvent.GetInstance().f_UnRegEvent(Time_AnalogySkyUp);
        f_GetObject("Introduce").SetActive(tBox == CardBox.BoxType.Intro);
        f_GetObject("Awanken").SetActive(tBox == CardBox.BoxType.Awaken);
        f_GetObject("UpLv").SetActive(tBox == CardBox.BoxType.Inten);
        f_GetObject("Sky").SetActive(tBox == CardBox.BoxType.Sky);
        f_GetObject("Quality").SetActive(tBox == CardBox.BoxType.Refine);
        f_GetObject("ShowPro").SetActive(tBox == CardBox.BoxType.Inten);
        f_GetObject("GetWay").SetActive(tBox == CardBox.BoxType.GetWay);
        f_GetObject("Fate").SetActive(tBox == CardBox.BoxType.Fate);
        f_ShowHideArtifactView(tBox);

        UI_ChangeBtn(f_GetObject("IntroduceBtn"), tBox == CardBox.BoxType.Intro);
        UI_ChangeBtn(f_GetObject("AwakenBtn"), tBox == CardBox.BoxType.Awaken);
        UI_ChangeBtn(f_GetObject("UpBtn"), tBox == CardBox.BoxType.Inten);
        UI_ChangeBtn(f_GetObject("MForwardBtn"), tBox == CardBox.BoxType.Refine);
        UI_ChangeBtn(f_GetObject("SkyBtn"), tBox == CardBox.BoxType.Sky);
        UI_ChangeBtn(f_GetObject("GetWayBtn"), tBox == CardBox.BoxType.GetWay);
        UI_ChangeBtn(f_GetObject("FateBtn"), tBox == CardBox.BoxType.Fate);
        UI_ChangeBtn(f_GetObject("ArtifactBtn"), tBox == CardBox.BoxType.Artifact);
        if (stackCardBox.Count > 0)
        {
            CardBox curCardBox = stackCardBox.Pop();
            curCardBox.m_bType = tBox;
            stackCardBox.Push(curCardBox);
        }
        switch (tBox)
        {
            case CardBox.BoxType.Intro:
                UpdateIntroduce();
                break;
            case CardBox.BoxType.Inten:
                UI_Empty();
                UI_OpenIntre(null, null, null);
                break;
            case CardBox.BoxType.Refine:
                UI_OpenMf(null, null, null);
                break;
            case CardBox.BoxType.Awaken:
                UpdateAwaken();
                break;
            case CardBox.BoxType.Sky:
                OpenSky(null, null, null);
                break;
            case CardBox.BoxType.GetWay:
                UI_OpenHandbook();
                break;
            case CardBox.BoxType.Fate:
                UpdateFateTab();
                break;
            case CardBox.BoxType.Artifact:
                f_UpdateArtifactView();
                break;
        }
        tBoxTabType = tBox;
    }

    private void UI_ChangeBtn(GameObject go, bool Show)
    {
        go.transform.Find("Up").gameObject.SetActive(Show);
        go.transform.Find("Down").gameObject.SetActive(!Show);
    }
    private void UI_TripSkyZeor(object obj)
    {
        UI_ChangeUI(tmpSkyType, false);
    }
    private void UI_OpenMf(GameObject go, object obj1, object obj2)
    {
        UpdateCardPro();
        //UpdataPro();
        UpdateMf();
    }
    private void UI_OpenIntre(GameObject go, object obj1, object obj2)
    {
        UpdateCardPro();
        UpdateIntre();
        UpdataPro();
    }
    private void UI_CloseAwakenEquipIndecu(GameObject go, object obj1, object obj2)
    {
        f_GetObject("AwakenEquipInduce").SetActive(false);
    }

    void UI_OpenFate(GameObject go, object obj1, object obj2)
    {
        UpdateFate();
    }
    /// <summary>
    /// 关闭进阶成功界面
    /// </summary>
    private void UI_HideSucceed(GameObject go, object obj1, object obj2)
    {
        f_GetObject("Succeed").SetActive(false);
        CardReson.SetActive(true);

        IsRefine = true;
    }
    /// <summary>
    /// 卡牌升级按钮
    /// </summary>
    //private void Card_Upgrade(GameObject go, object obj1, object obj2)
    //{
    //    SendUp();
    //}
    /// <summary>
    /// 退出当前界面
    /// </summary>
    private void UI_ExitBtn(GameObject go, object obj1, object obj2)
    {
        if (!IsRefine || !IsUpLevel)
            return;
        if (tBoxTabType == CardBox.BoxType.Sky && _Card.uSkyDestinyExp != 0 && obj1 == null)
        {
            _SkyChangeUI((object obj) =>
            {
                UI_ExitBtn(go, 1, obj2);
            });
            return;
        }

        isSendSky = false;
        ccTimeEvent.GetInstance().f_UnRegEvent(Time_AnalogySkyUp);
        if (stackCardBox.Count > 0)
        {
            CardBox curCardBox = stackCardBox.Pop();
            tBoxType = curCardBox.m_oType;
        }
        else
        {
MessageBox.ASSERT("CardProperty UI_UNHOLD, Interface data sort exception！");
        }

        if (tBoxType == CardBox.OpenType.handbook || tBoxType == CardBox.OpenType.SelectAward)
        {
            ccUIHoldPool.GetInstance().f_UnHold();
            ccUIHoldPool.GetInstance().f_UnHold();
        }
        else
        {
            ccUIHoldPool.GetInstance().f_UnHold(this);
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CardProperty, UIMessageDef.UI_CLOSE);
    }
    /// <summary>
    /// 自动添加
    /// </summary>
    private void UI_AutoAdd(GameObject go, object obj1, object obj2)
    {
        if (AddExpStone((int)obj1))
        {
            if (NeedGold >= CountNeedExp(_Card.m_iLv) - _Card.m_iExp)
            {
                int tlv = 0;
                int tNeedExp = CountNeedExp(_Card.m_iLv) - _Card.m_iExp;
                int tAddExp = NeedGold;
                while (true)
                {
                    if (tAddExp >= tNeedExp)
                    {
                        tlv++;
                        tAddExp -= tNeedExp;
                        tNeedExp = GetCardExp(tlv + _Card.m_iLv);
                    }
                    else
                        break;
                }
                f_GetObject("Last_Lv").GetComponent<UILabel>().text = "+" + tlv;
                f_GetObject("ExpStrand").GetComponent<UISlider>().value = (float)(Math.Abs(tAddExp)) / (float)tNeedExp;
            }
            else
            {
                f_GetObject("Last_Lv").GetComponent<UILabel>().text = "";
                f_GetObject("ExpStrand").GetComponent<UISlider>().value = 1;
            }

            f_GetObject("NeedGold").GetComponent<UILabel>().text = (NeedGold < Data_Pool.m_UserData.f_GetProperty((int)EM_MoneyType.eUserAttr_Money) ? CommonTools.f_GetTransLanguage(107) + NeedGold : string.Format(CommonTools.f_GetTransLanguage(107) + "[ff0000]{0}[-]", NeedGold));
            f_GetObject("Now_Exp").GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(108) + NeedGold;
        }
        if (GoodsList.size > 0)
        {
            //f_GetObject("Up").SetActive(true);
            //f_GetObject("AddExp").SetActive(false);
            SendUp((int)obj1);
        }
    }

    private void UI_CardUpFive(GameObject go, object obj1, object obj2)
    {
        if (!IsUpLevel)
            return;

        int num = (int)obj1;
        if (_Card.m_iLv + num > Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level))
            num = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) - _Card.m_iLv;
        if (num == 0)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(109));
            return;
        }
        if (_Card.m_iExp >= CountNeedExp(_Card.m_iLv))
        {
            GoodsList = new BetterList<BaseGoodsPoolDT>();
            SendUp(0);
            return;
        }
        UI_AutoAdd(go, num, obj2);
    }
    private void UI_CardUpOne(GameObject go, object obj1, object obj2)
    {
        if (!IsUpLevel)
            return;
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < _Card.m_iLv + 1)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(109));
            return;
        }
        if (_Card.m_iExp >= CountNeedExp(_Card.m_iLv))
        {
            GoodsList = new BetterList<BaseGoodsPoolDT>();
            SendUp(0);
            return;
        }
        UI_AutoAdd(go, obj1, obj2);
    }
    /// <summary>
    /// 暂时不用
    /// </summary>
    private void UI_OpenSelect(GameObject go, object obj1, object obj2)
    {
        f_GetObject("Select").SetActive(true);
    }

    /// <summary>
    /// 暂时不用
    /// </summary>
    private void UI_CloseSelect(GameObject go, object obj1, object obj2)
    {
        f_GetObject("Select").SetActive(false);
    }
    /// <summary>
    /// 箭头左
    /// </summary>
    private void UI_Arrlift(GameObject go, object obj1, object obj2)
    {
        if (!IsRefine || !IsUpLevel)
            return;
        if (tBoxTabType == CardBox.BoxType.Sky && _Card.uSkyDestinyExp != 0 && obj1 == null)
        {
            _SkyChangeUI((object obj) =>
            {
                UI_Arrlift(go, 1, obj2);
            });
            return;
        }
        //UI_ChangeUI(CardBox.BoxType.Intro);
        TeamPoolDT tTeamPoolDT;
        for (int i = 0; i < Data_Pool.m_TeamPool.f_GetAll().Count; i++)
        {
            tTeamPoolDT = (TeamPoolDT)Data_Pool.m_TeamPool.f_GetAll()[i];
            if (tTeamPoolDT.m_CardPoolDT == _Card)
            {
                CardInList = i;
                break;
            }
        }

        if (CardInList == 0)
            CardInList = Data_Pool.m_TeamPool.f_GetAll().Count - 1;
        else
            CardInList--;
        _Card = ((TeamPoolDT)Data_Pool.m_TeamPool.f_GetAll()[CardInList]).m_CardPoolDT;
        LineUpPage.CurrentSelectCardPos = ((TeamPoolDT)Data_Pool.m_TeamPool.f_GetAll()[CardInList]).m_eFormationPos;
        UI_ChageTab(go, CardBox.BoxType.Intro, null);
        UpdateCard();
        UpdateIntroduce();
    }
    private void UI_OpenIntroduce(GameObject go, object obj1, object obj2)
    {
        UpdateIntroduce();
    }
    /// <summary>
    /// 箭头右
    /// </summary>
    private void UI_ArrRigt(GameObject go, object obj1, object obj2)
    {
        if (!IsRefine || !IsUpLevel)
            return;
        if (tBoxTabType == CardBox.BoxType.Sky && _Card.uSkyDestinyExp != 0 && obj1 == null)
        {
            _SkyChangeUI((object obj) =>
            {
                UI_ArrRigt(go, 1, obj2);
            });
            return;
        }

        //UI_ChangeUI(CardBox.BoxType.Intro);
        TeamPoolDT tTeamPoolDT;
        for (int i = 0; i < Data_Pool.m_TeamPool.f_GetAll().Count; i++)
        {
            tTeamPoolDT = (TeamPoolDT)Data_Pool.m_TeamPool.f_GetAll()[i];
            if (tTeamPoolDT.m_CardPoolDT == _Card)
            {
                CardInList = i;
                break;
            }
        }
        if (CardInList == Data_Pool.m_TeamPool.f_GetAll().Count - 1)
            CardInList = 0;
        else
            CardInList++;
        _Card = ((TeamPoolDT)Data_Pool.m_TeamPool.f_GetAll()[CardInList]).m_CardPoolDT;
        LineUpPage.CurrentSelectCardPos = ((TeamPoolDT)Data_Pool.m_TeamPool.f_GetAll()[CardInList]).m_eFormationPos;
        UI_ChageTab(go, CardBox.BoxType.Intro, null);
        UpdateCard();
        UpdateIntroduce();
    }
    /// <summary>
    /// 刷新卡牌信息
    /// </summary>
    private void UpdateCard(bool isBook = false)
    {
        if (_Card.m_CardDT.iCardType == (int)EM_CardType.RoleCard)
        {
            f_GetObject("MForwardBtn").SetActive(true);
            f_GetObject("UpBtn").SetActive(false);
            f_GetObject("Chage").SetActive(false);
        }
        else
        {
            f_GetObject("MForwardBtn").SetActive(true);
            f_GetObject("UpBtn").SetActive(true);
        }
        if (isBook)
        {
            f_GetObject("MForwardBtn").SetActive(false);
            f_GetObject("UpBtn").SetActive(false);
        }
        if (CardReson != null)
            UITool.f_DestoryStatelObject(CardReson);
        f_GetObject("Btn_Grid").GetComponent<UIGrid>().enabled = true;
        CardStarLv = _Card.m_iLv;
        f_GetObject("Image_Card");
        CardReson = UITool.f_GetStatelObject(_Card, f_GetObject("Image_Card").transform, Vector3.zero, Vector3.zero, 2);
        Transform tf = CardReson.transform.Find(GameParamConst.prefabShadowName);
        if (tf)
        {
            tf.gameObject.SetActive(false);
        }
        string name = string.Empty;
        name = UITool.f_GetCardName(_Card.m_CardDT);
        if (_Card.m_iEvolveId != 0)
            name += "+" + _Card.m_iEvolveLv;
        UITool.f_GetImporentColorName(_Card.m_CardDT.iImportant, ref name);
        name += UITool.f_GetFiveElementNameById(_Card.m_CardDT.iCardEle);
        f_GetObject("Card_Explain").transform.GetChild(0).GetChild(3).GetComponent<UILabel>().text = name;
        UpdateCamp();
        UpdateFightType();
    }
    /// <summary>
    /// 更新武将国家
    /// </summary>
    void UpdateCamp()
    {
        UILabel Icon = f_GetObject("Card_Camp").GetComponent<UILabel>();
        switch ((EM_CardCamp)_Card.m_CardDT.iCardCamp)
        {
            case EM_CardCamp.eCardMain:
                Icon.text = CommonTools.f_GetTransLanguage(111);
                break;
            case EM_CardCamp.eCardWei:
                Icon.text = CommonTools.f_GetTransLanguage(112);
                break;
            case EM_CardCamp.eCardShu:
                Icon.text = CommonTools.f_GetTransLanguage(113);
                break;
            case EM_CardCamp.eCardWu:
                Icon.text = CommonTools.f_GetTransLanguage(114);
                break;
            case EM_CardCamp.eCardGroupHero:
                Icon.text = CommonTools.f_GetTransLanguage(115);
                break;
            case EM_CardCamp.eCardNo:
                break;
        }
        f_GetObject("Ce_Label").GetComponent<UILabel>().text = RolePropertyTools.f_GetBattlePower(Pro).ToString();
    }
    /// <summary>
    /// 更新武将类型
    /// </summary>
    void UpdateFightType()
    {
        UISprite FightType = f_GetObject("CardFight_Type").GetComponent<UISprite>();
        string SpriteName = "Icon_Fight";
        switch ((EM_CardFightType)_Card.m_CardDT.iCardFightType)
        {
            case EM_CardFightType.eCardPhyAtt:
                SpriteName += "PhyAtt";
                break;
            case EM_CardFightType.eCardMagAtt:
                SpriteName += "MagAtt";
                break;
            case EM_CardFightType.eCardSup:
                SpriteName += "Sup";
                break;
            case EM_CardFightType.eCardTank:
                SpriteName += "Tank";
                break;
            case EM_CardFightType.eCardKiller:
                SpriteName += "Killer";
                break;
            case EM_CardFightType.eCardPhysician:
                SpriteName += "Physician";
                break;
            case EM_CardFightType.eCardLogistics:
                SpriteName += "Logistics";
                break;
        }
        FightType.spriteName = SpriteName;
        // FightType.MakePixelPerfect();
    }
    /// <summary>
    /// 刷新信息界面
    /// </summary>
    private void UpdateIntroduce()
    {
        f_GetObject("Introduce").transform.GetChild(0).GetComponent<UIScrollView>().ResetPosition();
        UpdateCardPro();
        SkyLv = _Card.uSkyDestinyLv;
        IntroducePro_Atk = f_GetObject("IntroducePro_Atk").GetComponent<UILabel>();
        IntroducePro_Hp = f_GetObject("IntroducePro_Hp").GetComponent<UILabel>();
        IntroducePro_MagDef = f_GetObject("IntroducePro_MagDef").GetComponent<UILabel>();
        IntroducePro_PhyDef = f_GetObject("IntroducePro_PhyDef").GetComponent<UILabel>();
        IntroducePro_CardLv = f_GetObject("IntroducePro_CardLv").GetComponent<UILabel>();
        IntroducePro_CardMFLv = f_GetObject("IntroducePro_CardMFLv").GetComponent<UILabel>();
        Intro_Ability = f_GetObject("Intro_Ability").transform;
        IntroduceAbi_Abi1 = f_GetObject("IntroduceAbi_Abi1").GetComponent<UILabel>();
        IntroduceAbi_Abi2 = f_GetObject("IntroduceAbi_Abi2").GetComponent<UILabel>();
        IntroduceAbi_Abi3 = f_GetObject("IntroduceAbi_Abi3").GetComponent<UILabel>();
        IntroduceAbi_Abi4 = f_GetObject("IntroduceAbi_Abi4").GetComponent<UILabel>();
        IntroducePro_CardLv.transform.parent.gameObject.SetActive(!(tBoxType == CardBox.OpenType.handbook));
        IntroducePro_CardMFLv.transform.parent.gameObject.SetActive(!(tBoxType == CardBox.OpenType.handbook));

        if (tBoxType == CardBox.OpenType.handbook)
        {
            float line1 = -50;
            float line2 = line1 - 30;
            IntroducePro_Atk.transform.parent.localPosition = new Vector2(-7f, line1);
            IntroducePro_Hp.transform.parent.localPosition = new Vector2(440f, line1);
            IntroducePro_PhyDef.transform.parent.localPosition = new Vector2(-7f, line2);
            IntroducePro_MagDef.transform.parent.localPosition = new Vector3(440f, line2);
        }
        else
        {
            float line1 = -50;
            float line2 = line1 - 30;
            float line3 = line2 - 30;
            IntroducePro_Atk.transform.parent.localPosition = new Vector2(-7f, line2);
            IntroducePro_Hp.transform.parent.localPosition = new Vector2(440f, line2);
            IntroducePro_PhyDef.transform.parent.localPosition = new Vector2(-7f, line3);
            IntroducePro_MagDef.transform.parent.localPosition = new Vector3(440f, line3);
        }
        IntroducePro_Atk.text = Pro.f_GetProperty((int)EM_RoleProperty.Atk).ToString();
        IntroducePro_Hp.text = Pro.f_GetHp().ToString();
        IntroducePro_PhyDef.text = Pro.f_GetProperty((int)EM_RoleProperty.Def).ToString();
        IntroducePro_MagDef.text = Pro.f_GetProperty((int)EM_RoleProperty.MDef).ToString();
        IntroducePro_CardLv.text = _Card.m_iLv.ToString();
        IntroducePro_CardMFLv.text = _Card.m_iEvolveLv > 0 ? _Card.m_iEvolveLv.ToString() : "0";

        IntroduceTalent = f_GetObject("Intro_Talent").transform;
        Introduce_Talent = f_GetObject("Introduce_Talent").GetComponent<UILabel>();

        IntroduceDesc = f_GetObject("Intro_Desc").transform;
        Introduce_Desc = f_GetObject("Introduce_Desc").GetComponent<UILabel>();

        Intro_Fate = f_GetObject("Intro_Fate").transform;
        Introduce_Fate = f_GetObject("Introduce_Fate").GetComponent<UILabel>();

        MagicDT[] tmpMagic = UITool.f_GetCardMagic(_Card.m_CardDT);
        CardTalentDT[] tcardTalent = UITool.f_GetCardTalent(_Card);

        float _FountSpacing = 20;    //块中的分支间距
        float _CaseSpacing = 50;     //块与块的间距
        Intro_Ability.transform.localPosition = tBoxType == CardBox.OpenType.handbook ? new Vector2(-340, 150 + 55) : new Vector2(-340, 150);
        //技能
        float mHeight = 0;
        if (tmpMagic[0] == null)
            _SetIntroduceMagic(IntroduceAbi_Abi1, null, 0, ref mHeight);
        else
            _SetIntroduceMagic(IntroduceAbi_Abi1, string.Format("[FFD986]{0}：[-][FFEAD2]{1}", tmpMagic[0].szName, tmpMagic[0].szReadme), tmpMagic[0].iClass, ref mHeight, _FountSpacing);
        //////////////////////////////////////////////////////////////////////////
        if (tmpMagic[1] != null)
            _SetIntroduceMagic(IntroduceAbi_Abi2, string.Format("[FFD986]{0}：[-][FFEAD2]{1}", tmpMagic[1].szName, tmpMagic[1].szReadme), tmpMagic[1].iClass, ref mHeight, _FountSpacing);
        else
            _SetIntroduceMagic(IntroduceAbi_Abi2, null, 0, ref mHeight);
        //////////////////////////////////////////////////////////////////////////
        if (tmpMagic[2] != null)
            _SetIntroduceMagic(IntroduceAbi_Abi3, string.Format("[FFD986]{0}：[-][FFEAD2]{1}", tmpMagic[2].szName, tmpMagic[2].szReadme), tmpMagic[2].iClass, ref mHeight, _FountSpacing);
        else
            _SetIntroduceMagic(IntroduceAbi_Abi3, null, 0, ref mHeight);
        //////////////////////////////////////////////////////////////////////////
        if (tmpMagic[3] != null)
            _SetIntroduceMagic(IntroduceAbi_Abi4, string.Format("[FFD986]{0}：[-][FFEAD2]{1}", tmpMagic[3].szName, tmpMagic[3].szReadme), tmpMagic[3].iClass, ref mHeight, _FountSpacing);
        else
            _SetIntroduceMagic(IntroduceAbi_Abi4, null, 0, ref mHeight);


        //缘分
        Data_Pool.m_TeamPool.f_UpdateCardFate(_Card);
        Introduce_Fate.text = string.Empty;
        Intro_Fate.localPosition = new Vector2(-340, Intro_Ability.localPosition.y - mHeight - _CaseSpacing);
        Introduce_Fate.transform.gameObject.SetActive(false);
        float _FatemHeight = 0;
        for (int i = 0; i < tFateArr.Length; i++)
        {
            if (tFateArr[i] == null)
                tFateArr[i] = NGUITools.AddChild(Intro_Fate.Find("Ability1").gameObject, Introduce_Fate.transform.gameObject).GetComponent<UILabel>();
            if (_Card.m_CardFatePoolDT.m_aFateList.Count - 1 < i)
            {
                tFateArr[i].transform.gameObject.SetActive(false);
                continue;
            }
            else
                tFateArr[i].transform.gameObject.SetActive(true);
            tFateArr[i].transform.localPosition = new Vector3(Intro_Fate.localPosition.x, -_FatemHeight, 0);
            if (_Card.m_CardFatePoolDT.m_aFateIsOk[i])
            {
                tFateArr[i].text = string.Format("[FFD986][{0}][-]", _Card.m_CardFatePoolDT.m_aFateList[i].szName);
                tFateArr[i].transform.GetChild(0).GetComponent<UILabel>().text = "[FFEAD2]" + _Card.m_CardFatePoolDT.m_aFateList[i].szReadme.Substring(_Card.m_CardFatePoolDT.m_aFateList[i].szName.Length + 1);
            }
            else
            {
                tFateArr[i].text = string.Format("[9a9a9a][{0}][-]", _Card.m_CardFatePoolDT.m_aFateList[i].szName);
                tFateArr[i].transform.GetChild(0).GetComponent<UILabel>().text = "[9a9a9a]" + _Card.m_CardFatePoolDT.m_aFateList[i].szReadme.Substring(_Card.m_CardFatePoolDT.m_aFateList[i].szName.Length + 1);
            }
            tFateArr[i].transform.GetChild(0).GetComponent<UILabel>().spacingY = 3;
            _FatemHeight += tFateArr[i].transform.GetChild(0).GetComponent<UILabel>().height + _FountSpacing;
        }


        //天赋
        IntroduceTalent.localPosition = Intro_Fate.localPosition - new Vector3(0, _FatemHeight + _CaseSpacing + 20f, 0);
        Introduce_Talent.text = string.Empty;
        Introduce_Talent.transform.gameObject.SetActive(false);

        //int fornum = tcardTalent.Length > IntroduceTalent.Find("Talent").childCount ? tcardTalent.Length : IntroduceTalent.Find("Talent").childCount;
        float _TalentHeight = 0;
        for (int i = 0; i < Introduce_TalentLabel.Length; i++)
        {
            if (Introduce_TalentLabel[i] == null)
                Introduce_TalentLabel[i] = NGUITools.AddChild(IntroduceTalent.Find("Talent").gameObject, Introduce_Talent.transform.gameObject).GetComponent<UILabel>();

            if (tcardTalent.Length - 1 < i)
            {
                Introduce_TalentLabel[i].transform.gameObject.SetActive(false);
                continue;
            }
            else
                Introduce_TalentLabel[i].transform.gameObject.SetActive(true);
            Introduce_TalentLabel[i].transform.localPosition = new Vector3(Introduce_Talent.transform.localPosition.x, -_TalentHeight, 0);

            if (tcardTalent[i].iPropertyId1 != 0)
            {
                if (_Card.m_iEvolveLv > i)   //当前已经解锁的
                    CreareTalentCase(Introduce_TalentLabel[i], tcardTalent[i].iTarget, tcardTalent[i].iPropertyId1, tcardTalent[i].iPropertyNum1, tcardTalent[i].szName);
                else
                    CreareTalentCase(Introduce_TalentLabel[i], tcardTalent[i].iTarget, tcardTalent[i].iPropertyId1, tcardTalent[i].iPropertyNum1, tcardTalent[i].szName, 0, i);
            }
            if (tcardTalent[i].iPropertyId2 != 0)
            {
                if (_Card.m_iEvolveLv > i)
                {
                    CreareTalentCase(Introduce_TalentLabel[i], tcardTalent[i].iTarget, tcardTalent[i].iPropertyId1, tcardTalent[i].iPropertyNum1, tcardTalent[i].szName, 1);
                }
                else
                {
                    CreareTalentCase(Introduce_TalentLabel[i], tcardTalent[i].iTarget, tcardTalent[i].iPropertyId1, tcardTalent[i].iPropertyNum1, tcardTalent[i].szName, 1, i);
                }
            }

            Introduce_TalentLabel[i].transform.GetChild(0).GetComponent<UILabel>().spacingY = 3;
            _TalentHeight += Introduce_TalentLabel[i].transform.GetChild(0).GetComponent<UILabel>().height + _FountSpacing;
        }


        //英雄介绍
        IntroduceDesc.localPosition = IntroduceTalent.localPosition - new Vector3(0, _TalentHeight + _CaseSpacing, 0);
        Introduce_Desc.text = _Card.m_CardDT.szCardDesc;

    }

    void CreareTalentCase(UILabel tLabelParent, int Target, int ProId, int ProNum, string TalentName = null, int Proid2 = 0, int i = -1)
    {
        if (TalentName != null)
            tLabelParent.text = string.Format("[FFD986][{0}][-]", TalentName);
        UILabel tLabel = tLabelParent.transform.GetChild(0).GetComponent<UILabel>();
        tLabel.text = "[FFEAD2]" + GetTarget(Target);
        if (Proid2 != 0)
            tLabel.text += CommonTools.f_GetTransLanguage(116);
        else
            tLabel.text += string.Format("{0}+", UITool.f_GetProName((EM_RoleProperty)ProId));
        UITool.f_UpdateAddPro(ProId, tLabel, ProNum, true);
        if (i != -1)
            tLabel.text += string.Format(CommonTools.f_GetTransLanguage(117), i + 1);


    }
    string GetTarget(int Target)
    {
        switch ((EM_ProTarget)Target)
        {
            case EM_ProTarget.eMyself:
                return "";
            case EM_ProTarget.eAllCard:
                return CommonTools.f_GetTransLanguage(118);
            case EM_ProTarget.eAllWei:
                return CommonTools.f_GetTransLanguage(119);
            case EM_ProTarget.eAllShu:
                return CommonTools.f_GetTransLanguage(120);
            case EM_ProTarget.eAllWu:
                return CommonTools.f_GetTransLanguage(121);
            case EM_ProTarget.eAllQun:
                return CommonTools.f_GetTransLanguage(122);
        }
        return "";
    }
    /// <summary>
    /// 设置详细界面的技能
    /// </summary>
    void _SetIntroduceMagic(UILabel tUiLabel, string text, int MagicType, ref float mHeight, float fontsize = 26)
    {
        if (text != null)
        {
            tUiLabel.spacingY = 3;
            tUiLabel.transform.gameObject.SetActive(true);
            tUiLabel.text = text;
            tUiLabel.transform.localPosition = new Vector3(tUiLabel.transform.localPosition.x, -mHeight, 0);
            if (MagicType != 0)
            {
                UISprite tType = tUiLabel.transform.GetChild(0).GetComponent<UISprite>();
                switch ((EM_MagicType)MagicType)
                {
                    case EM_MagicType.Attack:
                        tType.spriteName = "Label_Pu";
                        break;
                    case EM_MagicType.anger:
                        tType.spriteName = "Label_Ji";
                        break;
                    case EM_MagicType.fit:
                        tType.spriteName = "Label_He";
                        break;
                    case EM_MagicType.superFit:
                        tType.spriteName = "Label_He";
                        tUiLabel.transform.gameObject.SetActive(false);
                        return;
                    default:
                        break;
                }
            }
            mHeight += tUiLabel.height + fontsize;
        }
        else
            tUiLabel.transform.gameObject.SetActive(false);
    }
    /// <summary>
    /// 更新升级界面
    /// </summary>
    private void UpdateIntre()
    {
        CarLvUpDT CardLv = (CarLvUpDT)glo_Main.GetInstance().m_SC_Pool.m_CarLvUpSC.f_GetSC(_Card.m_iLv + 1);
        f_GetObject("Up_MagDef").SetActive(CardLv != null);
        f_GetObject("Up_PhyDef").SetActive(CardLv != null);
        f_GetObject("Up_Atk").SetActive(CardLv != null);
        f_GetObject("Up_Hp").SetActive(CardLv != null);
        f_GetObject("Now_Exp").SetActive(CardLv != null);
        f_GetObject("ExpStrand").SetActive(CardLv != null);
        f_GetObject("Food").SetActive(CardLv != null);
        f_GetObject("AddExp").SetActive(CardLv != null);
        f_GetObject("Up5").SetActive(CardLv != null);
        if (CardLv == null)
        {
            f_GetObject("ShowLv").GetComponent<UILabel>().text =
          CommonTools.f_GetTransLanguage(123);
            return;
        }

        f_GetObject("Up_MagDef").GetComponent<UILabel>().text =
             "+" + _Card.m_CardDT.iAddMagDef;
        f_GetObject("Up_PhyDef").GetComponent<UILabel>().text =
            "+" + _Card.m_CardDT.iAddDef;
        f_GetObject("Up_Atk").GetComponent<UILabel>().text =
            "+" + _Card.m_CardDT.iAddAtk;
        f_GetObject("Up_Hp").GetComponent<UILabel>().text =
            "+" + _Card.m_CardDT.iAddHP;
        f_GetObject("Now_Exp").GetComponent<UILabel>().text =
            string.Format("{0}/{1}", _Card.m_iExp, NeedExp());
        f_GetObject("ExpStrand").GetComponent<UISlider>().value =
            (float)_Card.m_iExp / (float)NeedExp();
        f_GetObject("ShowLv").GetComponent<UILabel>().text =
            "LV." + _Card.m_iLv;
        for (int i = 0; i < f_GetObject("Food").transform.childCount; i++)
        {
            if (f_GetObject("Food").transform.GetChild(i).GetComponent<UISprite>().spriteName != "Icon_White_dk")
                f_GetObject("Food").transform.GetChild(i).GetComponent<UISprite>().spriteName = "Icon_White_dk";
            if (f_GetObject("Food").transform.GetChild(i).Find("Case").GetComponent<UISprite>().spriteName != "Icon_White")
                f_GetObject("Food").transform.GetChild(i).Find("Case").GetComponent<UISprite>().spriteName = "Icon_White";
            if (!f_GetObject("Food").transform.GetChild(i).Find("AddSign").gameObject.activeSelf)
                f_GetObject("Food").transform.GetChild(i).Find("AddSign").gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// 更新进阶界面
    /// </summary>
    private void UpdateMf()
    {
        UILabel EvolveNumUi = f_GetObject("Prop_Num").GetComponent<UILabel>();
        UILabel NeedEvoLv = f_GetObject("NeedLv").GetComponent<UILabel>();
        UI2DSprite Prop_Card = f_GetObject("Prop_Card").GetComponent<UI2DSprite>();
        UILabel NeedCardNum = f_GetObject("Prop_Card").transform.Find("Label").GetComponent<UILabel>();
        UILabel StarLv = f_GetObject("Pro_RLv").GetComponent<UILabel>();
        UILabel LastLv = f_GetObject("Up_RLv").GetComponent<UILabel>();
        UILabel Characteristic = f_GetObject("Characteristic_Label").GetComponent<UILabel>();
        UILabel Tianfu_Desc = f_GetObject("Tianfu_Desc").GetComponent<UILabel>();
        UILabel NeedGold = f_GetObject("MF_NeedGold").GetComponent<UILabel>();
        Transform prop = f_GetObject("Prop").transform;

        UILabel LastHp = f_GetObject("Mf_LastHp").GetComponent<UILabel>();
        UILabel LastAtk = f_GetObject("Mf_LastAtk").GetComponent<UILabel>();
        UILabel LastPhy = f_GetObject("Mf_LastPhyDef").GetComponent<UILabel>();
        UILabel LastMag = f_GetObject("Mf_LastMagDef").GetComponent<UILabel>();

        LastHp.text = string.Format("{0}:{1}", CommonTools.f_GetTransLanguage(187), Pro.f_GetHp());
        //Hp = HpLabel.text;
        LastAtk.text = string.Format("{0}:{1}", CommonTools.f_GetTransLanguage(186), Pro.f_GetProperty((int)EM_RoleProperty.Atk));
        //Atk = AtkLabel.text;
        LastPhy.text = string.Format("{0}:{1}", CommonTools.f_GetTransLanguage(188), Pro.f_GetProperty((int)EM_RoleProperty.Def));
        //PhyDef = PhyDefLabel.text;
        LastMag.text = string.Format("{0}:{1}", CommonTools.f_GetTransLanguage(189), Pro.f_GetProperty((int)EM_RoleProperty.MDef));


        CardEvolveDT CardEvolve;    //当前进阶等级
        CardEvolveDT CardEvolveLast; //下一等级
        if (_Card.m_iEvolveId == 0)
        {
            CardEvolve = new CardEvolveDT();
            CardEvolve.iEvoLv = 0;
            CardEvolveLast = (CardEvolveDT)glo_Main.GetInstance().m_SC_Pool.m_CardEvolveSC.f_GetSC(_Card.m_CardDT.iEvolveId);
        }
        else
        {
            CardEvolve = _Card.m_CardEvolveDT;
            if (CardEvolve != null)
                CardEvolveLast = (CardEvolveDT)glo_Main.GetInstance().m_SC_Pool.m_CardEvolveSC.f_GetSC(CardEvolve.iNextLvId);
            else
                CardEvolveLast = null;
        }
        StarLv.text = CommonTools.f_GetTransLanguage(184) + ":";
        if (CardEvolve != null)
            StarLv.text += CardEvolve.iEvoLv.ToString();
        else
            StarLv.text += "5";

        NeedEvoLv.gameObject.SetActive(CardEvolveLast != null);
        NeedGold.gameObject.SetActive(CardEvolveLast != null);
        f_GetObject("Prop").SetActive(CardEvolveLast != null);
        f_GetObject("MF").SetActive(CardEvolveLast != null);
        //f_GetObject("ShowPro").SetActive(CardEvolveLast != null);
        f_GetObject("Up_RLv").SetActive(CardEvolveLast != null);
        f_GetObject("Mf_Hp").SetActive(CardEvolveLast != null);
        f_GetObject("Mf_Atk").SetActive(CardEvolveLast != null);
        f_GetObject("Mf_PhyDef").SetActive(CardEvolveLast != null);
        f_GetObject("Mf_MagDef").SetActive(CardEvolveLast != null);
        Tianfu_Desc.gameObject.SetActive(CardEvolveLast != null);
        f_GetObject("MFUpMax").SetActive(CardEvolveLast == null);
        if (CardEvolveLast == null)
        {
            return;
        }
        LastLv.text = CommonTools.f_GetTransLanguage(184) + ":";
        LastLv.text += CardEvolveLast.iEvoLv.ToString();

        if (_Card.m_iEvolveId == 0)
            UpdataAddMf(false);
        else
            UpdataAddMf();

        prop.Find("Case").GetComponent<UISprite>().spriteName = UITool.f_GetImporentCase(5);
        UITool.f_SetIconSprite(prop.Find("Evolve").GetComponent<UI2DSprite>(), EM_ResourceType.Good, 100);

        int CardEvolveNum = Traverse();
        if (CardEvolveNum < CardEvolveLast.iEvolvePill)  //判断进阶丹数量
            EvolveNumUi.text = "[ff0000]" + CardEvolveNum + "/" + CardEvolveLast.iEvolvePill + "[-]";
        else
            EvolveNumUi.text = CardEvolveNum + "/" + CardEvolveLast.iEvolvePill;


        if (CardEvolveLast.iNeedLv > _Card.m_iLv)    //卡牌等级需要
            NeedEvoLv.text = CommonTools.f_GetTransLanguage(124) + CardEvolveLast.iNeedLv;
        else
            NeedEvoLv.text = "";

        prop.Find("Prop_CardName").gameObject.SetActive(CardEvolveLast.iNeedCardNum != 0);

        if (CardEvolveLast.iNeedCardNum == 0)    //需要卡牌张数
        {
            f_GetObject("Prop_Card").SetActive(false);
        }
        else
        {
            prop.Find("Prop_CardName").GetComponent<UILabel>().text = _Card.m_CardDT.szName;

            prop.Find("Prop_Card/Case").GetComponent<UISprite>().spriteName = UITool.f_GetImporentCase(_Card.m_CardDT.iImportant);
            UITool.f_SetIconSprite(Prop_Card, EM_ResourceType.Card, _Card.m_CardDT.iId);
            int tCardNum = CardNum();
            if (tCardNum >= CardEvolveLast.iNeedCardNum)
                NeedCardNum.text = "[ffffff]" + tCardNum + "/" + CardEvolveLast.iNeedCardNum + "[-]";
            else
                NeedCardNum.text = "[ff0000]" + tCardNum + "/" + CardEvolveLast.iNeedCardNum + "[-]";
            f_GetObject("Prop_Card").SetActive(true);
        }
        if (CardEvolveLast.iMoney < Data_Pool.m_UserData.f_GetProperty((int)EM_MoneyType.eUserAttr_Money))
            NeedGold.text = CommonTools.f_GetTransLanguage(125) + CardEvolveLast.iMoney;
        else
            NeedGold.text = string.Format(CommonTools.f_GetTransLanguage(126), CardEvolveLast.iMoney);
        //Characteristic.text = CardEvolveLast.szTalentName;
        Tianfu_Desc.text = string.Format(CommonTools.f_GetTransLanguage(127), ((CardTalentDT)glo_Main.GetInstance().m_SC_Pool.m_CardTalentSC.f_GetSC(CardEvolveLast.iTalentId)).szName, _Card.m_iEvolveLv + 1);
        //StarLv.text = CardEvolve.iEvoLv.ToString();
        //LastLv.text += CardEvolveLast.iEvoLv.ToString();
    }
    /// <summary>
    ///  进阶属性
    /// </summary>
    private void UpdataAddMf(bool IsEvole = true)
    {
        UILabel Mf_HP = f_GetObject("Mf_Hp").GetComponent<UILabel>();
        UILabel Mf_Atk = f_GetObject("Mf_Atk").GetComponent<UILabel>();
        UILabel Mf_Def = f_GetObject("Mf_PhyDef").GetComponent<UILabel>();
        UILabel Mf_MDef = f_GetObject("Mf_MagDef").GetComponent<UILabel>();
        Mf_HP.text = CommonTools.f_GetTransLanguage(187) + ":";
        Mf_Atk.text = CommonTools.f_GetTransLanguage(186) + ":";
        Mf_Def.text = CommonTools.f_GetTransLanguage(188) + ":";
        Mf_MDef.text = CommonTools.f_GetTransLanguage(189) + ":";
        if (!IsEvole)
        {
            Mf_HP.text +=
               Convert.ToInt32(Pro.f_GetHp() * 1.15f - 0.5f) + "";
            Mf_Atk.text +=
                Convert.ToInt32(Pro.f_GetProperty((int)EM_RoleProperty.Atk) * 1.15f - 0.5f) + "";
            Mf_Def.text +=
               Convert.ToInt32(Pro.f_GetProperty((int)EM_RoleProperty.Def) * 1.15f - 0.5f) + "";
            Mf_MDef.text +=
                Convert.ToInt32(Pro.f_GetProperty((int)EM_RoleProperty.MDef) * 1.15f - 0.5f) + "";
        }
        else
        {
            Mf_HP.text +=
              Convert.ToInt32(RolePropertyTools.CalculatePropertyStartLv1(_Card.m_CardDT.iInitHP, _Card.m_CardDT.iAddHP, _Card.m_iLv) * Math.Pow(1.15f, _Card.m_iEvolveLv + 1) - 0.5f) + "";
            Mf_Atk.text +=
                Convert.ToInt32(RolePropertyTools.CalculatePropertyStartLv1(_Card.m_CardDT.iInitAtk, _Card.m_CardDT.iAddAtk, _Card.m_iLv) * Math.Pow(1.15f, _Card.m_iEvolveLv + 1) - 0.5f) + "";
            Mf_Def.text +=
                Convert.ToInt32(RolePropertyTools.CalculatePropertyStartLv1(_Card.m_CardDT.iInitPhyDef, _Card.m_CardDT.iAddDef, _Card.m_iLv) * Math.Pow(1.15f, _Card.m_iEvolveLv + 1) - 0.5f) + "";
            Mf_MDef.text +=
                 Convert.ToInt32(RolePropertyTools.CalculatePropertyStartLv1(_Card.m_CardDT.iInitMagDef, _Card.m_CardDT.iAddMagDef, _Card.m_iLv) * Math.Pow(1.15f, _Card.m_iEvolveLv + 1) - 0.5f) + "";
        }
    }
    /// <summary>
    /// 更新属性
    /// </summary>
    private void UpdataPro()
    {
        f_GetObject("Pro_Hp").GetComponent<UILabel>().text =
            (Pro.f_GetHp()).ToString();
        //Hp = HpLabel.text;
        f_GetObject("Pro_Atk").GetComponent<UILabel>().text =
            (Pro.f_GetProperty((int)EM_RoleProperty.Atk)).ToString();
        //Atk = AtkLabel.text;
        f_GetObject("Pro_PhyDef").GetComponent<UILabel>().text =
            (Pro.f_GetProperty((int)EM_RoleProperty.Def)).ToString();
        //PhyDef = PhyDefLabel.text;
        f_GetObject("Pro_MagDef").GetComponent<UILabel>().text =
            (Pro.f_GetProperty((int)EM_RoleProperty.MDef)).ToString();
    }
    /// <summary>
    /// 清空道具
    /// </summary>
    public void UI_Empty()
    {
        NeedGold = 0;
        f_GetObject("NeedGold").GetComponent<UILabel>().text = "";
        UI2DSprite TempSprite;
        for (int i = 0; i < f_GetObject("Food").transform.childCount; i++)
        {
            TempSprite = f_GetObject("Food").transform.GetChild(i).GetChild(0).GetComponent<UI2DSprite>();
            TempSprite.sprite2D = null;
            //TempSprite.MakePixelPerfect();
            f_GetObject("Food").transform.GetChild(i).GetChild(1).GetComponent<UILabel>().text = "";
        }
        NeedExpiId = new long[4];
        GoodsList = new BetterList<BaseGoodsPoolDT>();
        //f_GetObject("ExpStrand").GetComponent<UISlider>().value =
        //    (float)_Card.m_iExp / (float)NeedExp();
        f_GetObject("Last_Lv").GetComponent<UILabel>().text = "";
    }
    /// <summary>
    /// 进阶
    /// </summary>
    private void Card_Evolve(GameObject go, object obj1, object obj2)
    {
        if (!IsRefine)
            return;
        CardEvolveDT tCardEvoDT;
        tCardEvoDT = glo_Main.GetInstance().m_SC_Pool.m_CardEvolveSC.f_GetSC(_Card.m_CardEvolveDT.iNextLvId) as CardEvolveDT;

        if (tCardEvoDT == null)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(128));
            return;
        }
        //银币不足
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_MoneyType.eUserAttr_Money) < tCardEvoDT.iMoney)
        {
            GetWayPageParam tGetWayParm = new GetWayPageParam(EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_Money, this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
            return;
        }  //进阶丹不足
        else if (Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(100) < tCardEvoDT.iEvolvePill)
        {
            GetWayPageParam tGetWayParm = new GetWayPageParam(EM_ResourceType.Good, 100, this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
            return;
        }//卡牌数量不足
        else if (CardNum() < tCardEvoDT.iNeedCardNum)
        {
            GetWayPageParam tGetWayParm = new GetWayPageParam(EM_ResourceType.Card, _Card.m_CardDT.iId, this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
            return;
        }
        else if (_Card.m_iLv < tCardEvoDT.iNeedLv)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(129));
            return;
        }
        hp = Pro.f_GetHp();
        tatk = Pro.f_GetProperty((int)EM_RoleProperty.Atk);
        def = Pro.f_GetProperty((int)EM_RoleProperty.Def);
        mdef = Pro.f_GetProperty((int)EM_RoleProperty.MDef);
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT CallBack = new SocketCallbackDT();
        CallBack.m_ccCallbackFail = EvolveFail;
        CallBack.m_ccCallbackSuc = EvolveSuc;
        Data_Pool.m_CardPool.f_Evolve(_Card, CallBack);
        IsRefine = false;
    }
    /// <summary>
    /// 进阶失败
    /// </summary>
    private void EvolveFail(object data)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult e = (eMsgOperateResult)data;
        UITool.Ui_Trip(Card_Error(e));
        //UpdataPro();
        UpdateMf();
        IsRefine = true;
    }
    int Time_CreareEvolveEffect;
    int Time_CreareEvolveSucTrip;
    /// <summary>
    /// 进阶成功
    /// </summary>
    private void EvolveSuc(object data)
    {
        Data_Pool.m_GuidancePool.m_OtherSave = true;
        UITool.f_OpenOrCloseWaitTip(false);
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(AudioEffectType.CardRefine);
        Evolve_CardEffect = UITool.f_CreateEffect_Old(UIEffectName.kapai_kpjj_01, EffectParent, new Vector3(-0.9f, 0, 0), 1f, 3f, UIEffectName.UIEffectAddress1);
        Time_CreareEvolveEffect = ccTimeEvent.GetInstance().f_RegEvent(0.5f, false, null, CreareEvolveEffect);
        Time_CreareEvolveSucTrip = ccTimeEvent.GetInstance().f_RegEvent(1.09f + 0.5f, false, null, CreareEvolveSucTrip);
        //进阶完成后更新红点
        CheckRedPoint();

        //重新计算战斗力
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.PlayerFightPowerChange);
    }

    void CreareEvolveEffect(object obj)
    {
        Evolve_ProEffect = UITool.f_CreateEffect_Old(UIEffectName.kapai_kpjj_02, EffectParent, new Vector3(0.521f, 0.13f, 0), 0.2f, 3f, UIEffectName.UIEffectAddress1);
        for (int i = 0; i < Evolve_ProEffect.transform.childCount; i++)
        {
            Evolve_ProEffect.transform.GetChild(i).GetComponent<TrailRenderer>().sortingLayerName = GameParamConst.UILayerName;
            Evolve_ProEffect.transform.GetChild(i).GetComponent<TrailRenderer>().sortingOrder = 3;
            Evolve_ProEffect.transform.GetChild(i).Find("line_01_1").GetComponent<TrailRenderer>().sortingLayerName = GameParamConst.UILayerName;
            Evolve_ProEffect.transform.GetChild(i).Find("line_01_1").GetComponent<TrailRenderer>().sortingOrder = 3;

        }
    }
    long hp;
    int tatk, def, mdef;   //进阶前来赋值(为了避免更新战斗力)
    void CreareEvolveSucTrip(object obj)
    {
        Show = f_GetObject("Succeed").transform.Find("ShowAll");  //展示的属性
        MfAddProTimeLapse(0, false);
        MfLastProTimeLapse(0, false);
        MfProTimeLapse(0, false);
        CardReson.SetActive(false);
        f_GetObject("Succeed").SetActive(true);
        GameObject Succeed_Icon = f_GetObject("Succeed_Icon");
        if (Succeed_Icon.transform.GetComponent<TweenScale>() != null)
        {
            Destroy(Succeed_Icon.transform.GetComponent<TweenScale>());
        }
        TweenScale ts = Succeed_Icon.AddComponent<TweenScale>();
        ts.from = new Vector3(0, 0, 1);
        ts.to = new Vector3(1, 1, 1);
        ts.animationCurve = AnimationCurve.EaseInOut(0, 0, 0.7f, 1);
        UpdateCard(tBoxType == CardBox.OpenType.handbook);
        //////////////////////////////////////////////////////////////////////////
        Show.Find("ShowPro/ShowHp/BG/Pro_Hp").GetComponent<UILabel>().text = hp.ToString();
        Show.Find("ShowPro/ShowAtk/BG/Pro_Atk").GetComponent<UILabel>().text = tatk.ToString();
        Show.Find("ShowPro/ShowPhyDef/BG/Pro_PhyDef").GetComponent<UILabel>().text = def.ToString();
        Show.Find("ShowPro/ShowMagDef/BG/Pro_MagDef").GetComponent<UILabel>().text = mdef.ToString();
        MfProTimeLapse(1f, true);

        UpdateCardPro();
        Show.Find("Arr/Arr2/Atk").GetComponent<UILabel>().text =
            Pro.f_GetProperty((int)EM_RoleProperty.Atk).ToString();
        Show.Find("Arr/Arr1/Hp").GetComponent<UILabel>().text =
           Pro.f_GetHp().ToString();
        Show.Find("Arr/Arr3/PhyDef").GetComponent<UILabel>().text =
            Pro.f_GetProperty((int)EM_RoleProperty.Def).ToString();
        Show.Find("Arr/Arr4/MagDef").GetComponent<UILabel>().text =
            Pro.f_GetProperty((int)EM_RoleProperty.MDef).ToString();
        MfLastProTimeLapse(1.5f, true);

        Show.Find("Arr/Arr2/Sprite/AtkMF").GetComponent<UILabel>().text =
             Pro.f_GetProperty((int)EM_RoleProperty.Atk) - tatk + "";
        Show.Find("Arr/Arr1/Sprite/HpMF").GetComponent<UILabel>().text =
             Pro.f_GetHp() - hp + "";
        Show.Find("Arr/Arr3/Sprite/PhyDefMF").GetComponent<UILabel>().text =
              Pro.f_GetProperty((int)EM_RoleProperty.Def) - def + "";
        Show.Find("Arr/Arr4/Sprite/MagDefMF").GetComponent<UILabel>().text =
             Pro.f_GetProperty((int)EM_RoleProperty.MDef) - mdef + "";
        MfAddProTimeLapse(2f, true);

        ////////////////////////////////////////////////////////////////////////// 
        //UpdataPro();
        UpdateMf();
    }
    #region 进阶成功界面
    bool MfSucShow;
    bool MfLastProShow;
    bool MfAddNumShow;
    void MfLastProTimeLapse()
    {
        Show.Find("Arr/Arr2/Atk").gameObject.SetActive(MfLastProShow);
        Show.Find("Arr/Arr1/Hp").gameObject.SetActive(MfLastProShow);
        Show.Find("Arr/Arr3/PhyDef").gameObject.SetActive(MfLastProShow);
        Show.Find("Arr/Arr4/MagDef").gameObject.SetActive(MfLastProShow);
    }
    void MfProTimeLapse()
    {
        Show.Find("ShowPro/ShowHp/BG/Pro_Hp").gameObject.SetActive(MfSucShow);
        Show.Find("ShowPro/ShowAtk/BG/Pro_Atk").gameObject.SetActive(MfSucShow);
        Show.Find("ShowPro/ShowPhyDef/BG/Pro_PhyDef").gameObject.SetActive(MfSucShow);
        Show.Find("ShowPro/ShowMagDef/BG/Pro_MagDef").gameObject.SetActive(MfSucShow);
    }
    void MfAddProTimeLapse()
    {
        Show.Find("Arr/Arr2/Sprite").gameObject.SetActive(MfAddNumShow);
        Show.Find("Arr/Arr1/Sprite").gameObject.SetActive(MfAddNumShow);
        Show.Find("Arr/Arr3/Sprite").gameObject.SetActive(MfAddNumShow);
        Show.Find("Arr/Arr4/Sprite").gameObject.SetActive(MfAddNumShow);
    }
    void MfProTimeLapse(float time, bool show)
    {
        MfSucShow = show;
        if (!show)
            MfProTimeLapse();
        else
            Invoke("MfProTimeLapse", time);
    }
    void MfLastProTimeLapse(float time, bool show)
    {
        MfLastProShow = show;
        if (!show)
            MfLastProTimeLapse();
        else
            Invoke("MfLastProTimeLapse", time);
    }
    void MfAddProTimeLapse(float time, bool show)
    {
        MfAddNumShow = show;
        if (!show)
            MfAddProTimeLapse();
        else
            Invoke("MfAddProTimeLapse", time);
    }
    void SetAlphaTweenParameter(TweenAlpha tween, UITweener.Style style, float Duration, int Show = 0)
    {
        tween.PlayForward();
        tween.style = style;
        tween.duration = Duration;
        if (Show == 0)
            tween.enabled = true;
    }
    #endregion

    int Time_UpBoxCollider;
    /// <summary>
    /// 升级成功
    /// </summary>
    private void UpSucceed(object data)
    {
        Data_Pool.m_GuidancePool.m_OtherSave = true;
        UITool.f_OpenOrCloseWaitTip(false);
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(AudioEffectType.CardUpLevel);
        if (_Card.m_iLv - CardStarLv > 0)
        {
            UITool.Ui_Trip(
                CommonTools.f_GetTransLanguage(130) + (_Card.m_iLv - CardStarLv) + CommonTools.f_GetTransLanguage(131) +
                "\n[41ee52]" + CommonTools.f_GetTransLanguage(132) + "+" + (_Card.m_CardDT.iAddHP * (_Card.m_iLv - CardStarLv)) +
                "\n" + CommonTools.f_GetTransLanguage(133) + "+" + (_Card.m_CardDT.iAddAtk * (_Card.m_iLv - CardStarLv)) +
                "\n" + CommonTools.f_GetTransLanguage(134) + "+" + (_Card.m_CardDT.iAddDef * (_Card.m_iLv - CardStarLv)) +
                "\n" + CommonTools.f_GetTransLanguage(135) + "+" + (_Card.m_CardDT.iAddMagDef * (_Card.m_iLv - CardStarLv)));
        }
        Inten_CardParticle = UITool.f_CreateEffect_Old(UIEffectName.kapai_kpsj_01, EffectParent, new Vector3(-0.85f, 0, 0), 0.2f, 3f, UIEffectName.UIEffectAddress1);
        Invoke("CreateUpGoodsEffect", 0.21f);
        UpdateCardPro();
        CardStarLv = _Card.m_iLv;
        UpdateIntre();
        Time_UpBoxCollider = ccTimeEvent.GetInstance().f_RegEvent(2f, false, null, UpBoxCollider);
        UpdataPro();
        f_GetObject("Btn_Grid").GetComponent<UIGrid>().enabled = true;
        //升级完成检查红点
        CheckRedPoint();

        //重新计算战斗力
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.PlayerFightPowerChange);
    }
    void UpBoxCollider(object obj)
    {
        IsUpLevel = true;
    }

    void CreateUpGoodsEffect()
    {
        Inten_BaseGoodsEffect = UITool.f_CreateEffect_Old(UIEffectName.kapai_kpsj_02, EffectParent, new Vector3(0.874f, -0.27f, 0), 1, 3f, UIEffectName.UIEffectAddress1);
        Transform[] Line = new Transform[4];
        Transform[] Kuang = new Transform[4];
        for (int i = 0; i < Line.Length; i++)
        {
            Line[i] = Inten_BaseGoodsEffect.transform.Find("line_0" + (i + 1));
            Kuang[i] = Inten_BaseGoodsEffect.transform.Find("kuang_0" + (i + 1));
            Kuang[i].gameObject.SetActive(NeedExpiId[i] != 0);
            Line[i].gameObject.SetActive(NeedExpiId[i] != 0);
            if (NeedExpiId[i] != 0)
            {
                Line[i].GetComponent<TrailRenderer>().sortingLayerName = GameParamConst.UILayerName;
                Line[i].localPosition = Line[i].localPosition + new Vector3(0, 0, 1);
                Line[i].GetComponent<TrailRenderer>().sortingOrder = 3;
            }
        }
        UI_Empty();

    }

    void DeleEffect(float time, GameObject obj1 = null, GameObject obj2 = null, GameObject obj3 = null)
    {
        if (obj1 != null)
            Destroy(obj1, time);
        if (obj2 != null)
            Destroy(obj2, time);
        if (obj3 != null)
            Destroy(obj3, time);
    }
    /// <summary>
    /// 升级失败
    /// </summary>
    private void UpFlid(object data)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult e = (eMsgOperateResult)data;
        UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(136), Card_Error(e)));
        f_GetObject("Up").SetActive(false);
        f_GetObject("AddExp").SetActive(true);
        UpdateIntre();
        GoodsList.Clear();
        UI_Empty();
        UpdataPro();
        IsUpLevel = true;
    }

    /// <summary>
    /// 发送升级
    /// </summary>
    private void SendUp(int tnum)
    {
        NeedExpiId = new long[4];
        int[] num = new int[4];
        int numCount = 0;
        int iint = 0;

        #region 获取ID以及数量

        for (int i = 0; i < GoodsList.size; i++)
        {
            if (NeedExpiId[numCount] == GoodsList[i].iId)
            {
                num[numCount] = ++iint;
                NeedExpiId[numCount] = GoodsList[i].iId;
            }
            else
            {
                if (i == 0)
                {
                    numCount = 0;
                    NeedExpiId[numCount] = GoodsList[i].iId;
                    i = -1;
                }
                else
                {
                    numCount++;
                    iint = 0;
                    num[numCount] = ++iint;
                    NeedExpiId[numCount] = GoodsList[i].iId;
                }
            }
        }
        #endregion

        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Money) < NeedGold)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(137));
            GetWayPageParam tGetWayParm = new GetWayPageParam(EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_Money, this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
            return;
        }
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < _Card.m_iLv + tnum)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(138));
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT SocketTemp = new SocketCallbackDT();
        SocketTemp.m_ccCallbackSuc = UpSucceed;
        SocketTemp.m_ccCallbackFail = UpFlid;
        Data_Pool.m_CardPool.f_LevelUp(_Card,
            NeedExpiId[0], num[0]
            , NeedExpiId[1], num[1]
            , NeedExpiId[2], num[2]
            , NeedExpiId[3], num[3]
            , SocketTemp);
        IsUpLevel = false;
    }

    /// <summary>
    /// 自动添加经验石
    /// </summary>
    private bool AddExpStone(int tnum)
    {
        NeedGold = 0;
        GoodsList = new BetterList<BaseGoodsPoolDT>();
        if (tnum == 0)
        {
            return false;
        }
        #region 计算所需经验
        int NeedExp = 0;
        for (int i = 0; i < tnum; i++)
        {
            NeedExp += CountNeedExp(_Card.m_iLv + i);
        }
        NeedExp -= _Card.m_iExp;
        #endregion
        Data_Pool.m_BaseGoodsPool.f_GetAll().Sort((BasePoolDT<long> a, BasePoolDT<long> b) => { return ((BaseGoodsPoolDT)a).m_BaseGoodsDT.iPriority >= ((BaseGoodsPoolDT)b).m_BaseGoodsDT.iPriority ? 1 : -1; });
        GoodsPool = Data_Pool.m_BaseGoodsPool.f_GetAll();

        Transform Food = f_GetObject("Food").transform;
        int FoodCount = 0;
        BaseGoodsPoolDT tGoods = new BaseGoodsPoolDT();
        for (int count = 0; count < GoodsPool.Count; count++)
        {
            #region 经验石
            tGoods = (BaseGoodsPoolDT)GoodsPool[count];
            //////////判断道具是否为经验石
            if ((tGoods.m_BaseGoodsDT.iEffect) == (int)EM_GoodsEffect.CardExp)
            {
                ///////遍历道具的数量
                int Tempnum = 0;
                for (int num = 1; num <= (tGoods).m_iNum; num++)
                {
                    //BaseGoodsPoolDT Temp = Goods;
                    //Temp.m_iNum = 1;
                    if ((tGoods).m_BaseGoodsDT.iEffectData - NeedExp > 0)
                    {
                        GoodsList.Add((tGoods));
                        Tempnum++;
                        NeedGold += (tGoods).m_BaseGoodsDT.iEffectData;
                        NeedExp = 0;
                        break;
                    }
                    else
                    {
                        NeedExp -= (tGoods).m_BaseGoodsDT.iEffectData;
                        GoodsList.Add((tGoods));
                        NeedGold += (tGoods).m_BaseGoodsDT.iEffectData;
                        Tempnum++;
                    }
                }


                UI2DSprite TemoSprite = Food.GetChild(FoodCount).GetChild(0).GetComponent<UI2DSprite>();
                TemoSprite.sprite2D = UITool.f_GetIconSprite(tGoods.m_BaseGoodsDT.iIcon);
                //TemoSprite.MakePixelPerfect();
                Food.GetChild(FoodCount).GetChild(1).GetComponent<UILabel>().text = Tempnum + "";
                Food.GetChild(FoodCount).Find("Case").GetComponent<UISprite>().spriteName = UITool.f_GetImporentCase(tGoods.m_BaseGoodsDT.iImportant);
                Food.GetChild(FoodCount).Find("AddSign").gameObject.SetActive(false);
                Tempnum = 0;
                FoodCount++;
                if (NeedExp == 0)
                {
                    return true;
                }
                if (FoodCount == 3)
                {
                    return true;
                }
            }
            #endregion
        }
        if (GoodsList.size == 0)
        {
            GetWayPageParam tWay = new GetWayPageParam(EM_ResourceType.Good, 127, this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tWay);
            //UITool.Ui_Trip("没有经验石");
        }
        return false;
    }

    int CountNeedExp(int Lv)
    {
        int Exp = 0;
        CarLvUpDT CardLv = (CarLvUpDT)glo_Main.GetInstance().m_SC_Pool.m_CarLvUpSC.f_GetSC(Lv + 1);
        if (CardLv == null)
            return 0;
        switch (_Card.m_CardDT.iImportant)
        {
            case 1:
                Exp = CardLv.iWhiteCard;
                break;
            case 2:
                Exp = CardLv.iGreenCard;
                break;
            case 3:
                Exp = CardLv.iBlueCard;
                break;
            case 4:
                Exp = CardLv.iPurpleCard;
                break;
            case 5:
                Exp = CardLv.iOragenCard;
                break;
            case 6:
                Exp = CardLv.iRedCard;
                break;
            //TsuCode - tuong kim
            case 7:
                Exp = CardLv.iGoldCard;
                break;
            //
            default:
                break;
        }
        return Exp;
    }

    int GetCardExp(int lv)
    {
        CarLvUpDT CardLv = (CarLvUpDT)glo_Main.GetInstance().m_SC_Pool.m_CarLvUpSC.f_GetSC(lv);
        switch ((EM_Important)_Card.m_CardDT.iImportant)
        {
            case EM_Important.White:
                return CardLv.iWhiteCard;
            case EM_Important.Green:
                return CardLv.iGreenCard;
            case EM_Important.Blue:
                return CardLv.iBlueCard;
            case EM_Important.Purple:
                return CardLv.iPurpleCard;
            case EM_Important.Oragen: return CardLv.iOragenCard;
            case EM_Important.Red: return CardLv.iRedCard;
            //TsuCode - tuong kim
            case EM_Important.Gold: return CardLv.iGoldCard;
                //
        }
        return 0;
    }
    /// <summary>
    /// 获取错误提示
    /// </summary>
    private string Card_Error(eMsgOperateResult e)
    {
        return UITool.f_GetError((int)e);
    }
    #region   UI
    /// /////////// 信息界面//////////////////////////////////
    UILabel IntroducePro_Atk;
    UILabel IntroducePro_Hp;
    UILabel IntroducePro_PhyDef;
    UILabel IntroducePro_MagDef;
    UILabel IntroducePro_CardLv;
    UILabel IntroducePro_CardMFLv;
    UILabel IntroduceAbi_Abi1;
    UILabel IntroduceAbi_Abi2;
    UILabel IntroduceAbi_Abi3;
    UILabel IntroduceAbi_Abi4;
    Transform Introduce_AwakenStar;
    UILabel[] tFateArr = new UILabel[8];
    UISprite[] AwakenStar = new UISprite[6];
    UILabel[] Introduce_TalentLabel = new UILabel[20];
    UILabel Introduce_Talent;
    UILabel Introduce_Desc;
    UILabel Introduce_Fate;
    Transform IntroduceTalent;
    Transform IntroduceDesc;
    Transform Intro_Fate;
    Transform Intro_Ability;
    /// ////////////领悟界面////////////////////////////////////////
    Transform Awaken;
    UILabel Awaken_LastStarLv;
    UILabel Awaken_StarLv;
    UILabel Awaken_NextStar;
    UISprite[] Awaken_Star = new UISprite[6];
    UILabel Awaken_Desc;
    Transform Awaken_AwakenEquip;
    UILabel Awaken_GoodsName;
    UILabel Awaken_GoodsNum;
    UILabel Awaken_CardName;
    UILabel Awaken_CardNum;
    UI2DSprite Awaken_GoodsSprite;
    UI2DSprite Awaken_CardSprite;
    UILabel Awaken_NeedMoeny;
    UILabel Awaken_BtnLabel;
    GameObject Awaken_AwakenBtn;
    ///////////////////////////////////////
    GameObject Fate_Desc;
    GameObject Fate_DescParent;

    #endregion


    #region 领悟
    private AwakenCardDT nextAwakenCard;//下一次觉醒数据
    List<AwakenEquipDT> tListAwakenEquipShow;    //用来保存打开过的
    private string getNextStarLevelHint(AwakenCardDT tmpAwakenCard)
    {
        if (tmpAwakenCard == null)
            return "";
        for (int i = tmpAwakenCard.iId; i < glo_Main.GetInstance().m_SC_Pool.m_AwakenCardSC.f_GetAll().Count; i++)
        {
            AwakenCardDT awakenCardDT = glo_Main.GetInstance().m_SC_Pool.m_AwakenCardSC.f_GetSC(i) as AwakenCardDT;
            if (awakenCardDT != null && awakenCardDT.szDesc != null && awakenCardDT.szDesc != "")
            {
                return string.Format(CommonTools.f_GetTransLanguage(139), awakenCardDT.iId / 10, awakenCardDT.iId % 10) + awakenCardDT.szDesc;
            }
        }
        return "";
    }
    /// <summary>
    /// 更新领悟界面
    /// </summary>
    void UpdateAwaken()
    {
        AwakenCardDT tmpAwakenCard = UITool.f_GetAwakenCardDT(_Card.m_iLvAwaken);
        f_GetObject("Awaken_Body").SetActive(tmpAwakenCard != null);
        Awaken_GoodsName.transform.parent.parent.gameObject.SetActive(tmpAwakenCard != null);
        Awaken_NeedMoeny.transform.parent.gameObject.SetActive(tmpAwakenCard != null);
        Awaken_LastStarLv.gameObject.SetActive(tmpAwakenCard != null);
        //Awaken_LastStarLv.transform.Find("Sprite").gameObject.SetActive(tmpAwakenCard != null);
        f_GetObject("AwakenArr").SetActive(tmpAwakenCard != null);
        Awaken_AwakenBtn.SetActive(tmpAwakenCard != null);
        Awaken_NextStar.gameObject.SetActive(tmpAwakenCard != null);
        nextAwakenCard = tmpAwakenCard;
        if (tmpAwakenCard == null)
        {
            Awaken_StarLv.text = CommonTools.f_GetTransLanguage(140);
            return;
        }
        string flagAwaken = _Card.m_iFlagAwaken.ToString();
        if (flagAwaken.Length != 4)
        {
            for (int i = flagAwaken.Length; i < 4; i++)
            {
                flagAwaken = "0" + flagAwaken;
            }
        }
        Awaken_NextStar.text = getNextStarLevelHint(tmpAwakenCard);
        AwakenEquipDT[] tmpAwakenEquip = UITool.f_GetAwakenEquipArr(_Card.m_iLvAwaken);
        int[] AwakenEquipId = new int[4];
        bool isEquip = false;
        UITool.f_UpdateStarNum(Awaken_Star, _Card.m_iLvAwaken / 10);
        Awaken_LastStarLv.text = string.Format(CommonTools.f_GetTransLanguage(141), tmpAwakenCard.iId / 10, tmpAwakenCard.iId % 10);
        int NowAwakenLv = tmpAwakenCard.iId - 1;
        Awaken_StarLv.text = string.Format(CommonTools.f_GetTransLanguage(141), NowAwakenLv / 10, NowAwakenLv % 10);
        Awaken_Desc.text = tmpAwakenCard.szDesc;
        Awaken_AwakenEquip.GetChild(2).gameObject.SetActive(tmpAwakenCard.iEquipID3 != 0);
        Awaken_AwakenEquip.GetChild(3).gameObject.SetActive(tmpAwakenCard.iEquipID4 != 0);

        Awaken_AwakenEquip.GetComponent<UIGrid>().enabled = true;
        Transform tmpEquip;
        for (int i = 0; i < Awaken_AwakenEquip.childCount; i++)
        {
            if (Awaken_AwakenEquip.GetChild(i).gameObject.activeSelf)
            {
                tmpEquip = Awaken_AwakenEquip.GetChild(i);
                tmpEquip.GetComponent<UISprite>().spriteName = UITool.f_GetImporentCase(tmpAwakenEquip[i].iImportant);
                UITool.f_Set2DSpriteGray(tmpEquip.GetChild(0).GetComponent<UI2DSprite>(), int.Parse(flagAwaken[i].ToString()) != 1);
                tmpEquip.Find("Name").GetComponent<UILabel>().text = UITool.f_GetImporentForName(tmpAwakenEquip[i].iImportant, tmpAwakenEquip[i].szName);
                if (int.Parse(flagAwaken[i].ToString()) == 1)   //以装备
                {
                    tmpEquip.GetChild(2).gameObject.SetActive(false);
                    tmpEquip.GetChild(1).GetComponent<UILabel>().text = "";
                    tmpEquip.GetChild(0).GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSprite(tmpAwakenEquip[i].iIcon);
                    AwakenEquipId[i] = tmpAwakenEquip[i].iId;
                    ResourceCommonDT ttResource = new ResourceCommonDT();
                    ttResource.f_UpdateInfo((byte)EM_ResourceType.AwakenEquip, tmpAwakenEquip[i].iId, 1);
                    f_RegClickEvent(tmpEquip.gameObject, UI_ShowAward, ttResource);
                }
                else
                {
                    tmpEquip.GetChild(2).gameObject.SetActive(true);
                    if (UITool.f_GetIsHaveAwakenEquip(tmpAwakenEquip[i]))   //可以装备
                    {
                        tmpEquip.GetChild(1).GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(142);
                        tmpEquip.GetChild(2).GetComponent<UISprite>().spriteName = "Icon_Plus";
                        UITool.f_SetSpriteGray(tmpEquip.GetChild(2).GetComponent<UISprite>(), false);
                        isEquip = true;
                        AwakenEquipId[i] = tmpAwakenEquip[i].iId;
                        f_RegClickEvent(tmpEquip.gameObject, Equip_Btn, AwakenEquipId[i], i);
                    }
                    else if (tmpAwakenEquip[i].iImportant == 1)     //品质为1的不可合成（添加获取途径功能）
                    {
                        tmpEquip.GetChild(1).GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(143);
                        UITool.f_SetSpriteGray(tmpEquip.GetChild(2).GetComponent<UISprite>(), true);
                        GetWayPageParam param = new GetWayPageParam(EM_ResourceType.AwakenEquip, tmpAwakenEquip[i].iId, this);
                        f_RegClickEvent(tmpEquip.gameObject, OnAwakeEquipGetWayBtn, param);
                    }
                    else  //背包没有   打开详细界面
                    {
                        tmpEquip.GetChild(1).GetComponent<UILabel>().text = CommonTools.f_GetTransLanguage(144);
                        UITool.f_SetSpriteGray(tmpEquip.GetChild(2).GetComponent<UISprite>(), true);
                        tmpEquip.GetChild(2).GetComponent<UISprite>().spriteName = "Icon_PlusYellow";
                        f_RegClickEvent(tmpEquip.gameObject, Equip_Btn, tmpAwakenEquip[i].iId);
                    }
                    tmpEquip.GetChild(0).GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSprite(tmpAwakenEquip[i].iIcon);
                }
            }
        }
        Awaken_GoodsName.text = CommonTools.f_GetTransLanguage(145);
        Awaken_GoodsName.transform.parent.GetComponent<UISprite>().spriteName = UITool.f_GetImporentCase((glo_Main.GetInstance().m_SC_Pool.m_BaseGoodsSC.f_GetSC(AwakeId) as BaseGoodsDT).iImportant);
        if (_Card.m_CardDT.iCardType == (int)EM_CardType.RoleCard)
            Awaken_GoodsNum.text = string.Format(UITool.f_GetGoodsNum(AwakeId) < tmpAwakenCard.iNeedGoods * 2 ? "[ff0000]{0}/{1}" : "{0}/{1}", UITool.f_GetGoodsNum(AwakeId), tmpAwakenCard.iNeedGoods * 2);
        else
            Awaken_GoodsNum.text = string.Format(UITool.f_GetGoodsNum(AwakeId) < tmpAwakenCard.iNeedGoods ? "[ff0000]{0}/{1}" : "{0}/{1}", UITool.f_GetGoodsNum(AwakeId), tmpAwakenCard.iNeedGoods);

        if (_Card.m_CardDT.iCardType != 1)
        {
            if (tmpAwakenCard.iNeedCard == 0)
                Awaken.Find("NeedNum/Card").gameObject.SetActive(false);
            else
            {
                Awaken.Find("NeedNum/Card").gameObject.SetActive(true);
                Awaken_CardNum.transform.parent.GetComponent<UISprite>().spriteName = UITool.f_GetImporentCase(_Card.m_CardDT.iImportant);
                Awaken_CardName.text = _Card.m_CardDT.szName;
                int tCardNum = CardNum();

                if (tCardNum >= tmpAwakenCard.iNeedCard)
                    Awaken_CardNum.text = string.Format("{0}/{1}", tCardNum, tmpAwakenCard.iNeedCard);
                else
                    Awaken_CardNum.text = string.Format("[ff0000]{0}/{1}", tCardNum, tmpAwakenCard.iNeedCard);
                Awaken_CardSprite.sprite2D = UITool.f_GetIconSpriteByCardId(_Card);
            }
        }
        else
            Awaken.Find("NeedNum/Card").gameObject.SetActive(false);
        UITool.f_SetIconSprite(Awaken_GoodsSprite, EM_ResourceType.Good, (int)EM_MoneyType.eCardAwakenStone);
        Awaken_NeedMoeny.text = CommonTools.f_GetTransLanguage(125);
        if (tmpAwakenCard.iNeedMoeny <= Data_Pool.m_UserData.f_GetProperty((int)EM_MoneyType.eUserAttr_Money))
            Awaken_NeedMoeny.text += tmpAwakenCard.iNeedMoeny.ToString();
        else
            Awaken_NeedMoeny.text += string.Format("[ff0000]{0}", tmpAwakenCard.iNeedMoeny);
        if (isEquip)
        {
            Awaken_BtnLabel.text = CommonTools.f_GetTransLanguage(146);
            f_RegClickEvent(Awaken_AwakenBtn, Awaken_Btn, AwakenBtnType.OneEquip, AwakenEquipId);
        }
        else
        {
            Awaken_BtnLabel.text = CommonTools.f_GetTransLanguage(147);
            f_RegClickEvent(Awaken_AwakenBtn, Awaken_Btn, AwakenBtnType.Awaken, AwakenEquipId);
        }
        isEquip = false;

        //重新计算战斗力
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.PlayerFightPowerChange, 1);
    }

    /// <summary>
    /// 点击icon显示详细信息
    /// </summary>
    void UI_ShowAward(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, obj1);
    }
    /// <summary>
    /// 没有领悟装备，点击打开获取途径
    /// </summary>
    private void OnAwakeEquipGetWayBtn(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, obj1);
    }

    /// <summary>
    /// 获取缘分需要物品名字
    /// </summary>
    string GetFateNeedGoodsName(int rType, int[] Goodsid)
    {
        string FateGoodsName = string.Empty;
        for (int i = 0; i < Goodsid.Length; i++)
        {
            if (Goodsid[i] > 0)
            {
                switch ((EM_ResourceType)rType)
                {
                    case EM_ResourceType.Card:
                        FateGoodsName += (glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(Goodsid[i]) as CardDT).szName + ",";
                        break;
                    case EM_ResourceType.Equip:
                        FateGoodsName += (glo_Main.GetInstance().m_SC_Pool.m_EquipSC.f_GetSC(Goodsid[i]) as EquipDT).szName + ",";
                        break;
                    case EM_ResourceType.Treasure:
                        FateGoodsName += (glo_Main.GetInstance().m_SC_Pool.m_TreasureSC.f_GetSC(Goodsid[i]) as TreasureDT).szName + ",";
                        break;
                }
            }
        }
        FateGoodsName.Remove(Goodsid.Length - 1, 1);
        return FateGoodsName;
    }

    /// <summary>
    /// 计算所需经验
    /// </summary>
    private int NeedExp()
    {

        CarLvUpDT carlvup =
       (CarLvUpDT)glo_Main.GetInstance().m_SC_Pool.m_CarLvUpSC.f_GetSC(_Card.m_iLv + 1);
        switch (_Card.m_CardDT.iImportant)
        {
            case 1:
                return carlvup.iWhiteCard;
            case 2:
                return carlvup.iGreenCard;
            case 3:
                return carlvup.iBlueCard;
            case 4:
                return carlvup.iPurpleCard;
            case 5:
                return carlvup.iOragenCard;
            case 6:
                return carlvup.iRedCard;
            //Tsucode - tuong kim
            case 7:
                return carlvup.iGoldCard;
                //
        }
        return 0;
    }
    private int Traverse()
    {
        if ((Data_Pool.m_BaseGoodsPool.f_GetForData5(100) as BaseGoodsPoolDT) != null)
            return (Data_Pool.m_BaseGoodsPool.f_GetForData5(100) as BaseGoodsPoolDT).m_iNum;
        else
            return 0;
    }
    private int CardNum()
    {
        int tCardNum = 0;
        List<BasePoolDT<long>> tList = Data_Pool.m_CardPool.f_GetAllForData1(_Card.m_CardDT.iId);
        CardPoolDT tCardPoolDT;
        for (int i = 0; i < tList.Count; i++)
        {
            tCardPoolDT = tList[i] as CardPoolDT;
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

    /// <summary>
    /// 初始化领悟界面
    /// </summary>
    void IninAwaken()
    {
        AwakenCardDT tmpAwankenCard = UITool.f_GetAwakenCardDT(_Card.m_iLvAwaken);
        Awaken = f_GetObject("Awanken").transform;
        for (int i = 0; i < Awaken.Find("Stars").childCount; i++)
        {
            Awaken_Star[i] = Awaken.Find("Stars").GetChild(i).GetComponent<UISprite>();
        }
        Awaken_LastStarLv = f_GetObject("Awaken_LastStarLv").GetComponent<UILabel>();
        Awaken_StarLv = f_GetObject("Awaken_StarLv").GetComponent<UILabel>();
        Awaken_NextStar = f_GetObject("Awaken_NextStar").GetComponent<UILabel>();
        Awaken_Desc = f_GetObject("Awaken_Desc").GetComponent<UILabel>();
        Awaken_AwakenEquip = Awaken.Find("Awaken_Body/AwakenEquip");
        Awaken_GoodsName = f_GetObject("Awaken_GoodsName").GetComponent<UILabel>();
        Awaken_GoodsSprite = f_GetObject("Awaken_GoodsSprite").GetComponent<UI2DSprite>();
        Awaken_GoodsNum = f_GetObject("Awaken_GoodsNum").GetComponent<UILabel>();
        Awaken_CardName = f_GetObject("Awaken_CardName").GetComponent<UILabel>();
        Awaken_CardNum = f_GetObject("Awaken_CardNum").GetComponent<UILabel>();
        Awaken_CardSprite = f_GetObject("Awaken_CardSprite").GetComponent<UI2DSprite>();
        Awaken_NeedMoeny = f_GetObject("Awaken_NeedMoeny").GetComponent<UILabel>();
        Awaken_BtnLabel = f_GetObject("Awaken_BtnLabel").GetComponent<UILabel>();
        Awaken_AwakenBtn = Awaken.Find("Btn_Awaken").gameObject;
    }
    private AwakenEquipDT[] tmpLonSave = new AwakenEquipDT[4];
    /// <summary>
    /// 领悟按钮
    /// </summary>
    void Awaken_Btn(GameObject go, object obj1, object obj2)
    {
        int[] tmpArr = (int[])obj2;
        tmpLonSave = new AwakenEquipDT[4];
        AwakenBtnType tAwaken = (AwakenBtnType)obj1;
        AwakenEquipPoolDT[] tmpLon = new AwakenEquipPoolDT[4];
        if (tAwaken == AwakenBtnType.OneEquip)
            tmpLon = UITool.f_GetAwakenEquip(tmpArr);
        //领悟道具掩码
        string flagAwaken = _Card.m_iFlagAwaken.ToString();
        if (flagAwaken.Length != 4)   //掩码转换字符串
        {
            for (int i = flagAwaken.Length; i <= 4; i++)
            {
                flagAwaken = "0" + flagAwaken;
            }
        }
        switch ((AwakenBtnType)obj1)
        {
            case AwakenBtnType.OneEquip:

                for (int i = 0; i < tmpLon.Length; i++)
                {
                    if (flagAwaken[i] == '1')
                    {
                        tmpLonSave[i] = null;
                        continue;
                    }
                    if (tmpLon[i] == null)
                    {
                        tmpLonSave[i] = null;
                        continue;
                    }

                    if (UITool.f_GetAwakenCardDT(_Card.m_iLvAwaken).iCardNeedLv > Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level))
                    {
                        tmpLonSave[i] = null;
                        UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(148), UITool.f_GetAwakenCardDT(_Card.m_iLvAwaken).iCardNeedLv));
                        return;
                    }
                    tmpLonSave[i] = tmpLon[i].m_AwakenEquipDT;
                    UITool.f_OpenOrCloseWaitTip(true);
                    SocketCallbackDT CallBack = new SocketCallbackDT();
                    CallBack.m_ccCallbackSuc = MutiEquipSuc;
                    CallBack.m_ccCallbackFail = OneEquipFill;
                    if (tmpLon[i] != null)
                        Data_Pool.m_AwakenEquipPool.f_Equip(_Card.iId, (byte)i, CallBack);
                }
                Invoke("UpdateAwaken", 0.5f);
                break;
            case AwakenBtnType.Awaken:
                if (_Card.m_CardDT.iCardType == (int)EM_CardType.RoleCard)
                {
                    if (UITool.f_GetAwakenCardDT(_Card.m_iLvAwaken).iNeedGoods * 2 > UITool.f_GetGoodsNum(AwakeId))
                    {
                        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(149));
                        GetWayPageParam tGetWayParm = new GetWayPageParam(EM_ResourceType.Good, AwakeId, this);
                        ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
                        return;
                    }
                }

                if (UITool.f_GetAwakenCardDT(_Card.m_iLvAwaken).iEquipID3 == 0)
                {
                    if (flagAwaken.Substring(0, 2).IndexOf('0') != -1)
                    {
                        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(150));
                        return;
                    }
                }
                else if (UITool.f_GetAwakenCardDT(_Card.m_iLvAwaken).iEquipID4 == 0)
                {
                    if (flagAwaken.Substring(0, 3).IndexOf('0') != -1)
                    {
                        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(150));
                        return;
                    }
                }
                else
                {

                    if (flagAwaken.IndexOf('0') != -1)
                    {
                        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(150));
                        return;
                    }
                }

                if (UITool.f_GetAwakenCardDT(_Card.m_iLvAwaken).iNeedGoods > UITool.f_GetGoodsNum(AwakeId))
                {
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(149));
                    GetWayPageParam tGetWayParm = new GetWayPageParam(EM_ResourceType.Good, AwakeId, this);
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
                    return;
                }
                if (UITool.f_GetAwakenCardDT(_Card.m_iLvAwaken).iNeedMoeny >
                    Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Money))
                {
                    UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1530));
                    GetWayPageParam tGetWayParm = new GetWayPageParam(EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_Money, this);
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
                    return;
                }
                if (_Card.m_CardDT.iCardType != 1)
                {
                    if (UITool.f_GetAwakenCardDT(_Card.m_iLvAwaken).iNeedCard >
                        CardNum())
                    {
                        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1529));
                        return;
                    }
                }
                if (UITool.f_GetAwakenCardDT(_Card.m_iLvAwaken).iCardNeedLv >
                    Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level))
                {
                    UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(148), UITool.f_GetAwakenCardDT(_Card.m_iLvAwaken).iCardNeedLv));
                    return;
                }
                SocketCallbackDT CallBack1 = new SocketCallbackDT();
                CallBack1.m_ccCallbackFail = AwakenFall;
                CallBack1.m_ccCallbackSuc = AwakenSuc;
                Data_Pool.m_CardPool.f_CardAwaken(_Card.iId, CallBack1);
                break;
            default:
                break;
        }
    }



    /// <summary>
    /// 装备按钮   (可以装备的领悟道具)
    /// </summary>
    void Equip_Btn(GameObject go, object obj1, object obj2)
    {
        int tmpId = (int)obj1;
        int idx = 999;
        if (obj2 != null)
            idx = (int)obj2;

        AwakenEquipPageParam PagePram = new AwakenEquipPageParam(tmpId, (byte)idx, obj2 != null, _Card, AwakenEquipPageParam.em_State.Induce);
        PagePram.EquipFail = OneEquipFill;
        PagePram.EquipSuc = OneEquipSuc;
        PagePram.SytheSuc = (object obj) => { UpdateAwaken(); };
        ccUIManage.GetInstance().f_SendMsg(UINameConst.AwakenEquipPage, UIMessageDef.UI_OPEN, PagePram);
        return;
        #region 备用
        f_GetObject("AwakenEquipInduce").SetActive(true);
        f_GetObject("AwakenEquipInduce").transform.Find("EquipInduce").gameObject.SetActive(true);
        f_GetObject("AwakenEquipInduce").transform.Find("EquipInduce/GetWayBtn").gameObject.SetActive(obj2 == null);
        f_GetObject("AwakenEquipInduce").transform.Find("EquipInduce/GoSytheBtn").gameObject.SetActive(obj2 == null);
        f_GetObject("AwakenEquipInduce").transform.Find("EquipInduce/EquipBtn").gameObject.SetActive(obj2 != null);
        f_GetObject("AwakenEquipInduce").transform.Find("EquipSythe").gameObject.SetActive(false);
        if (obj2 == null)
        {
            GetWayPageParam param = new GetWayPageParam(EM_ResourceType.AwakenEquip, tmpId, this);
            f_RegClickEvent(f_GetObject("AwakenEquipInduce").transform.Find("EquipInduce/GetWayBtn").gameObject, OnAwakeEquipGetWayBtn, param);
            f_RegClickEvent(f_GetObject("AwakenEquipInduce").transform.Find("EquipInduce/GoSytheBtn").gameObject, Sythe_Btn, tmpId);
        }
        AwakenEquipPoolDT tmpEquip = UITool.f_GetAwakenEquip(tmpId);
        if (tmpEquip == null)
        {
            tmpEquip = new AwakenEquipPoolDT();
            tmpEquip.m_AwakenEquipDT = glo_Main.GetInstance().m_SC_Pool.m_AwakenEquipSC.f_GetSC(tmpId) as AwakenEquipDT;
        }
        Transform tmpTran = f_GetObject("AwakenEquipInduce").transform.Find("EquipInduce");
        UI2DSprite tmpIcon = tmpTran.Find("Icon").GetComponent<UI2DSprite>();
        UILabel tmpName = tmpTran.Find("Name").GetComponent<UILabel>();
        UILabel tmpDesc = tmpTran.Find("Desc").GetComponent<UILabel>();
        UILabel tmpNum = tmpTran.Find("Num").GetComponent<UILabel>();
        UILabel tmpAtk = tmpTran.Find("Pro/Atk").GetComponent<UILabel>();
        UILabel tmpHp = tmpTran.Find("Pro/Hp").GetComponent<UILabel>();
        UILabel tmpDef = tmpTran.Find("Pro/Def").GetComponent<UILabel>();
        UISprite tmpCase = tmpTran.Find("Case").GetComponent<UISprite>();
        GameObject tmpBtn = tmpTran.Find("EquipBtn").gameObject;

        tmpCase.spriteName = UITool.f_GetImporentCase(tmpEquip.m_AwakenEquipDT.iImportant);
        tmpName.text = tmpEquip.m_AwakenEquipDT.szName;
        tmpIcon.sprite2D = UITool.f_GetIconSprite(tmpEquip.m_AwakenEquipDT.iIcon);
        tmpNum.text = string.Format("{0}:{1}", CommonTools.f_GetTransLanguage(151), tmpEquip.m_num);
        tmpDesc.text = tmpEquip.m_AwakenEquipDT.szDesc;
        tmpAtk.text = string.Format("[f1b049]{0}[-]     [f0eccb]+{1}", UITool.f_GetProName((EM_RoleProperty)tmpEquip.m_AwakenEquipDT.iAddProId1), tmpEquip.m_AwakenEquipDT.iAddPro1);
        tmpHp.text = string.Format("[f1b049]{0}[-]     [f0eccb]+{1}", UITool.f_GetProName((EM_RoleProperty)tmpEquip.m_AwakenEquipDT.iAddProId2), tmpEquip.m_AwakenEquipDT.iAddPro2);
        tmpDef.text = string.Format("[f1b049]{0}[-]     [f0eccb]+{1}", CommonTools.f_GetTransLanguage(152), tmpEquip.m_AwakenEquipDT.iAddPro3);
        if (obj2 != null)
            f_RegClickEvent(tmpBtn, EquipTo_Btn, tmpEquip, idx);
        #endregion
    }
    /// <summary>
    /// 领悟装备装备
    /// </summary>
    /// <param name="go"></param>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    void EquipTo_Btn(GameObject go, object obj1, object obj2)
    {
        int i = (int)obj2;
        if (UITool.f_GetAwakenCardDT(_Card.m_iLvAwaken).iCardNeedLv >
                  _Card.m_iLv)
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(148), UITool.f_GetAwakenCardDT(_Card.m_iLvAwaken).iCardNeedLv));
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT CallBack = new SocketCallbackDT();
        CallBack.m_ccCallbackSuc = OneEquipSuc;
        CallBack.m_ccCallbackFail = OneEquipFill;
        Data_Pool.m_AwakenEquipPool.f_Equip(_Card.iId, (byte)i, CallBack);
        f_GetObject("AwakenEquipInduce").SetActive(false);
    }
    /// <summary>
    /// 领悟合成按钮
    /// </summary>
    void Sythe_Btn(GameObject go, object obj1, object obj2)
    {
        int tmpId = (int)obj1;
        AwakenEquipDT tmpEquip = (AwakenEquipDT)glo_Main.GetInstance().m_SC_Pool.m_AwakenEquipSC.f_GetSC(tmpId);
        UpdateSytheUI(tmpEquip);
    }


    void UpdateSytheUI(AwakenEquipDT tmpEquip)
    {
        int tmpId = tmpEquip.iId;
        Transform tmpTran = f_GetObject("AwakenEquipInduce").transform.Find("EquipSythe");
        tmpTran.gameObject.SetActive(true);
        f_GetObject("AwakenEquipInduce").transform.Find("EquipInduce").gameObject.SetActive(false);
        UI2DSprite tmpIcon = tmpTran.Find("Icon").GetComponent<UI2DSprite>();
        UILabel tmpName = tmpTran.Find("Name").GetComponent<UILabel>();
        UI2DSprite tmpIcon1 = tmpTran.Find("Equip/Icon").GetComponent<UI2DSprite>();
        UILabel tmpNum = tmpTran.Find("Equip/Num").GetComponent<UILabel>();
        UISprite tmpCase = tmpTran.Find("Equip/Case").GetComponent<UISprite>();
        GameObject Sythe4 = tmpTran.Find("Sythe4").gameObject;
        GameObject Sythe3 = tmpTran.Find("Sythe3").gameObject;
        UILabel tmpMoeny = tmpTran.Find("Moeny/Num").GetComponent<UILabel>();
        GameObject SytheBtn = tmpTran.Find("SytheBtn").gameObject;


        tmpCase.spriteName = UITool.f_GetImporentCase(tmpEquip.iImportant);
        tmpIcon.sprite2D = UITool.f_GetIconSprite(tmpEquip.iIcon);
        tmpName.text = tmpEquip.szName;
        tmpIcon1.sprite2D = UITool.f_GetIconSprite(tmpEquip.iIcon);
        f_RegClickEvent(tmpIcon1.gameObject, UI_OpenNeedSytheAwakenEquipTrip, tmpEquip);

        if (UITool.f_GetAwakenEquip(tmpId) != null)
            tmpNum.text = string.Format("{0}/1", UITool.f_GetAwakenEquip(tmpId).m_num);
        else
            tmpNum.text = string.Format("{0}/1", 0);

        if (tmpEquip.iMoenyNum > Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Money))
            tmpMoeny.text = string.Format("[ff0000]{0}[-]", tmpEquip.iMoenyNum);
        else
            tmpMoeny.text = tmpEquip.iMoenyNum.ToString();

        f_RegClickEvent(SytheBtn, AwakenEquipSythe, tmpEquip);
        AwakenEquipDT tawakenEquipDT = new AwakenEquipDT();
        if (tmpEquip.iSynthesisId1 == 0)
        {
            GetWayPageParam param = new GetWayPageParam(EM_ResourceType.AwakenEquip, tmpEquip.iId, this);
            OnAwakeEquipGetWayBtn(null, param, null);
            return;
        }
        if (tmpEquip.iSynthesisId4 == 0)
        {
            for (int i = 0; Sythe3.transform.childCount > i; i++)
            {
                int NeedNum = 0;
                switch (i)
                {
                    case 0:
                        tawakenEquipDT = glo_Main.GetInstance().m_SC_Pool.m_AwakenEquipSC.f_GetSC(tmpEquip.iSynthesisId1) as AwakenEquipDT;
                        NeedNum = tmpEquip.iSynthesisNum1;
                        break;
                    case 1:
                        tawakenEquipDT = glo_Main.GetInstance().m_SC_Pool.m_AwakenEquipSC.f_GetSC(tmpEquip.iSynthesisId2) as AwakenEquipDT;
                        NeedNum = tmpEquip.iSynthesisNum2;
                        break;
                    case 2:
                        tawakenEquipDT = glo_Main.GetInstance().m_SC_Pool.m_AwakenEquipSC.f_GetSC(tmpEquip.iSynthesisId3) as AwakenEquipDT;
                        NeedNum = tmpEquip.iSynthesisNum3;
                        break;
                    default:
                        break;
                }
                if (tawakenEquipDT != null)
                    _SetAwakenShow(Sythe3.transform.GetChild(i), tawakenEquipDT, NeedNum);
            }
            Sythe3.SetActive(true);
            Sythe4.SetActive(false);
        }
        else
        {
            for (int i = 0; Sythe4.transform.childCount > i; i++)
            {
                int NeedNum = 0;
                switch (i)
                {
                    case 0:
                        tawakenEquipDT = glo_Main.GetInstance().m_SC_Pool.m_AwakenEquipSC.f_GetSC(tmpEquip.iSynthesisId1) as AwakenEquipDT;
                        NeedNum = tmpEquip.iSynthesisNum1;
                        break;
                    case 1:
                        tawakenEquipDT = glo_Main.GetInstance().m_SC_Pool.m_AwakenEquipSC.f_GetSC(tmpEquip.iSynthesisId2) as AwakenEquipDT;
                        NeedNum = tmpEquip.iSynthesisNum2;
                        break;
                    case 2:
                        tawakenEquipDT = glo_Main.GetInstance().m_SC_Pool.m_AwakenEquipSC.f_GetSC(tmpEquip.iSynthesisId3) as AwakenEquipDT;
                        NeedNum = tmpEquip.iSynthesisNum3;
                        break;
                    case 3:
                        tawakenEquipDT = glo_Main.GetInstance().m_SC_Pool.m_AwakenEquipSC.f_GetSC(tmpEquip.iSynthesisId4) as AwakenEquipDT;
                        NeedNum = tmpEquip.iSynthesisNum4;
                        break;
                    default:
                        break;
                }
                if (tawakenEquipDT != null)
                    _SetAwakenShow(Sythe4.transform.GetChild(i), tawakenEquipDT, NeedNum);
            }
            Sythe3.SetActive(false);
            Sythe4.SetActive(true);
            f_GetObject("AwakenEquipInduce").SetActive(true);
        }
    }
    void AwakenEquipSythe(GameObject go1, object obj11, object obj12)
    {
        AwakenEquipDT tmpEquip = (AwakenEquipDT)obj11;
        if (UITool.f_GetAwakenEquipPoolDT(tmpEquip.iSynthesisId1) == null ||
           UITool.f_GetAwakenEquipPoolDT(tmpEquip.iSynthesisId2) == null ||
           UITool.f_GetAwakenEquipPoolDT(tmpEquip.iSynthesisId3) == null ||
           UITool.f_GetAwakenEquipPoolDT(tmpEquip.iSynthesisId3) == null)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(153));
            return;
        }
        else if (tmpEquip.iSynthesisNum1 > UITool.f_GetAwakenEquipPoolDT(tmpEquip.iSynthesisId1).m_num ||
            tmpEquip.iSynthesisNum2 > UITool.f_GetAwakenEquipPoolDT(tmpEquip.iSynthesisId2).m_num ||
            tmpEquip.iSynthesisNum3 > UITool.f_GetAwakenEquipPoolDT(tmpEquip.iSynthesisId3).m_num ||
            tmpEquip.iSynthesisNum4 > UITool.f_GetAwakenEquipPoolDT(tmpEquip.iSynthesisId4).m_num)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(153));
            return;
        }
        if (tmpEquip.iMoenyNum > Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Money))
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(154));
            GetWayPageParam tGetWayParm = new GetWayPageParam(EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_Money, this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT tmpCallBack = new SocketCallbackDT();
        tmpCallBack.m_ccCallbackFail = (object tmp1) =>
        {
            UITool.f_OpenOrCloseWaitTip(false);

            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(155) + Card_Error((eMsgOperateResult)tmp1));
        };
        tmpCallBack.m_ccCallbackSuc = (object tmp2) =>
        {
            UpdateAwaken();
            UITool.f_OpenOrCloseWaitTip(false);
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(156));
        };
        Data_Pool.m_AwakenEquipPool.f_Sythe(tmpEquip.iId, 1, tmpCallBack);
        f_GetObject("AwakenEquipInduce").SetActive(false);
    }

    void _SetAwakenShow(Transform tTran, AwakenEquipDT tAwakenEquip, int num)
    {
        AwakenEquipDT tAwaken = tAwakenEquip;
        UI2DSprite tmp2DSprite = tTran.GetComponent<UI2DSprite>();
        UILabel tmpLbale = tTran.Find("Label").GetComponent<UILabel>();
        UISprite tmpSprite = tTran.Find("Case").GetComponent<UISprite>();
        tmp2DSprite.sprite2D = UITool.f_GetIconSprite(tAwaken.iIcon);
        if (UITool.f_GetAwakenEquipPoolDT(tAwaken.iId).iId != 0)
        {
            tmpLbale.text = UITool.f_GetAwakenEquipPoolDT(tAwaken.iId).m_num.ToString() + "/" + num;
            if (UITool.f_GetAwakenEquipPoolDT(tAwaken.iId).m_num >= num)
            {
                f_RegClickEvent(tTran.gameObject, UI_OpenNeedSytheAwakenEquipTrip, tAwaken);
            }
            else
            {
                f_RegClickEvent(tTran.gameObject, UI_OpenNeedSytheAwakenEquip, tAwaken);
            }
        }
        else
        {
            tmpLbale.text = "0/" + num;
            f_RegClickEvent(tTran.gameObject, UI_OpenNeedSytheAwakenEquip, tAwaken);
        }
        tmpSprite.spriteName = UITool.f_GetImporentCase(tAwaken.iImportant);
    }

    void UI_OpenNeedSytheAwakenEquip(GameObject go, object obj1, object obj2)
    {
        AwakenEquipDT tAwaken = (AwakenEquipDT)obj1;

        if (tAwaken.iSynthesisId1 == 0)
        {
            GetWayPageParam param = new GetWayPageParam(EM_ResourceType.AwakenEquip, tAwaken.iId, this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, param);
            return;
        }
        UpdateSytheUI(tAwaken);
    }
    void UI_OpenNeedSytheAwakenEquipTrip(GameObject go, object obj1, object obj2)
    {
        AwakenEquipDT tAwaken = (AwakenEquipDT)obj1;
        ResourceCommonDT tResourceCommonDT = new ResourceCommonDT();
        tResourceCommonDT.f_UpdateInfo((byte)EM_ResourceType.AwakenEquip, tAwaken.iId, UITool.f_GetAwakenEquipPoolDT(tAwaken.iId) == null ? 0 : UITool.f_GetAwakenEquipPoolDT(tAwaken.iId).m_num);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, tResourceCommonDT);
    }
    void MutiEquipSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        RoleProperty roleProperty = new RoleProperty();
        roleProperty.f_Reset();
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(157));
        for (int i = 0; i < tmpLonSave.Length; i++)
        {
            if (tmpLonSave[i] != null)
            {
                roleProperty.f_AddProperty((int)tmpLonSave[i].iAddProId1, tmpLonSave[i].iAddPro1);
                roleProperty.f_AddProperty((int)tmpLonSave[i].iAddProId2, tmpLonSave[i].iAddPro2);
                roleProperty.f_AddProperty((int)tmpLonSave[i].iAddProId3, tmpLonSave[i].iAddPro3);
                roleProperty.f_AddProperty((int)tmpLonSave[i].iAddProId4, tmpLonSave[i].iAddPro4);
            }
        }
        List<object> param = new List<object>();
        param.Add(new Vector2(418, 0));
        for (int i = (int)EM_RoleProperty.Atk; i < (int)EM_RoleProperty.End; i++)
        {
            if (roleProperty.f_GetProperty(i) > 0)
            {
                AddProTripPageParam addProParam = new AddProTripPageParam();
                addProParam.addProId = i;
                addProParam.addProValue = roleProperty.f_GetProperty(i);
                param.Add(addProParam);
            }
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.AddProTripPage, UIMessageDef.UI_OPEN, param);
        UpdateAwaken();
    }
    void OneEquipSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        AwakenEquipPoolDT tAwakenDT = (AwakenEquipPoolDT)obj;
        RoleProperty roleProperty = new RoleProperty();
        roleProperty.f_Reset();

        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(157));
        roleProperty.f_AddProperty((int)tAwakenDT.m_AwakenEquipDT.iAddProId1, tAwakenDT.m_AwakenEquipDT.iAddPro1);
        roleProperty.f_AddProperty((int)tAwakenDT.m_AwakenEquipDT.iAddProId2, tAwakenDT.m_AwakenEquipDT.iAddPro2);
        roleProperty.f_AddProperty((int)tAwakenDT.m_AwakenEquipDT.iAddProId3, tAwakenDT.m_AwakenEquipDT.iAddPro3);
        roleProperty.f_AddProperty((int)tAwakenDT.m_AwakenEquipDT.iAddProId4, tAwakenDT.m_AwakenEquipDT.iAddPro4);
        List<object> param = new List<object>();
        param.Add(new Vector2(418, 0));
        for (int i = (int)EM_RoleProperty.Atk; i < (int)EM_RoleProperty.End; i++)
        {
            if (roleProperty.f_GetProperty(i) > 0)
            {
                AddProTripPageParam addProParam = new AddProTripPageParam();
                addProParam.addProId = i;
                addProParam.addProValue = roleProperty.f_GetProperty(i);
                param.Add(addProParam);
            }
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.AddProTripPage, UIMessageDef.UI_OPEN, param);
        UpdateAwaken();
    }
    void OneEquipFill(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.Ui_Trip(UITool.f_GetError((int)obj));
        UpdateAwaken();
    }
    void AwakenSuc(object obj)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(UITool.GetEnumName(typeof(AudioEffectType), AudioEffectType.Awaken));

        UITool.f_OpenOrCloseWaitTip(false);
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(158));
        int NowAwakenLv = nextAwakenCard.iId - 1;
        List<object> param = new List<object>();

        AwakenCardDT nowAwakenCardDT = glo_Main.GetInstance().m_SC_Pool.m_AwakenCardSC.f_GetSC(NowAwakenLv) as AwakenCardDT;

        param.Add(new Vector2(418, 0));
        RoleProperty roleProperty = new RoleProperty();
        roleProperty.f_Reset();
        roleProperty.f_AddProperty((int)nextAwakenCard.iAddProId1, nextAwakenCard.iAddPro1);
        roleProperty.f_AddProperty((int)nextAwakenCard.iAddProId2, nextAwakenCard.iAddPro2);
        roleProperty.f_AddProperty((int)nextAwakenCard.iAddProId3, nextAwakenCard.iAddPro3);
        roleProperty.f_AddProperty((int)nextAwakenCard.iAddProId4, nextAwakenCard.iAddPro4);
        if (nowAwakenCardDT != null)
        {
            RoleProperty rolePropertyNow = new RoleProperty();
            rolePropertyNow.f_Reset();
            rolePropertyNow.f_AddProperty((int)nowAwakenCardDT.iAddProId1, nowAwakenCardDT.iAddPro1);
            rolePropertyNow.f_AddProperty((int)nowAwakenCardDT.iAddProId2, nowAwakenCardDT.iAddPro2);
            rolePropertyNow.f_AddProperty((int)nowAwakenCardDT.iAddProId3, nowAwakenCardDT.iAddPro3);
            rolePropertyNow.f_AddProperty((int)nowAwakenCardDT.iAddProId4, nowAwakenCardDT.iAddPro4);
            roleProperty -= rolePropertyNow;
        }
        for (int i = (int)EM_RoleProperty.Atk; i < (int)EM_RoleProperty.End; i++)
        {
            if (roleProperty.f_GetProperty(i) > 0)
            {
                AddProTripPageParam addProParam = new AddProTripPageParam();
                addProParam.addProId = i;
                addProParam.addProValue = roleProperty.f_GetProperty(i);
                param.Add(addProParam);
            }
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.AddProTripPage, UIMessageDef.UI_OPEN, param);
        UpdateAwaken();
        UpdateCardPro();
    }
    void AwakenFall(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(159));
        UpdateAwaken();
    }
    #endregion


    #region   天命

    private UILabel Now_Sky;
    private UILabel Now_Atk;
    private UILabel Now_Hp;
    private UILabel Now_Def;
    private UILabel Now_MDef;
    private UILabel Now_AtkName;
    private UILabel Now_HpName;
    private UILabel Now_DefName;
    private UILabel Now_MDefName;
    private Transform Now_Magic1;
    private Transform Now_Magic2;
    private int Time_AnalogySkyUp;
    private int SkyRealNum;
    private int SkyNum = 0;   //次数
    private int BeforeSkyExp;
    private int BeforeSkyLv;
    private int SkyGoodsNum;
    private int SkyGoodsNumLast;

    private UILabel Last_Sky;
    private UILabel Last_Atk;
    private UILabel Last_Hp;
    private UILabel Last_Def;
    private UILabel Last_MDef;
    private UILabel Last_AtkName;
    private UILabel Last_HpName;
    private UILabel Last_DefName;
    private UILabel Last_MDefName;
    private Transform Last_Magic1;
    private Transform Last_Magic2;

    private UILabel SkyExp;
    private UILabel GoodsNum;
    private UIToggle AutoSky;
    private UILabel ConNum;
    private UILabel Probability;
    private UI2DSprite ConProIcon;
    private UISlider SkyExpSlider;

    bool IsInitSkyUI = true;
    bool lackMoeny = true;
    bool isSendSky = false;

    int SkyLv;

    int SkyAddPro = 6;
    RoleProperty NowSkyPro;  //当前天命加成
    RoleProperty NextSkyPro; //下一等级天命加成

    SocketCallbackDT SkySocketCallback = new SocketCallbackDT();
    void InitSkyUI()
    {
        IsInitSkyUI = Now_Sky == null;
        SkySocketCallback.m_ccCallbackFail = SkyFail;
        SkySocketCallback.m_ccCallbackSuc = SkySuc;
        NowSkyPro = new RoleProperty();
        NextSkyPro = new RoleProperty();
        if (IsInitSkyUI)
        {
            Now_Sky = f_GetObject("Now_SkyLabel").GetComponent<UILabel>();
            Now_Atk = f_GetObject("Now_AtkLabel").GetComponent<UILabel>();
            Now_Hp = f_GetObject("Now_HpLabel").GetComponent<UILabel>();
            Now_Def = f_GetObject("Now_DefLabel").GetComponent<UILabel>();
            Now_MDef = f_GetObject("Now_MDefLabel").GetComponent<UILabel>();
            Now_Magic1 = f_GetObject("NowMagic").transform.GetChild(0);
            Now_Magic2 = f_GetObject("NowMagic").transform.GetChild(1);
            Now_AtkName = Now_Atk.transform.parent.GetComponent<UILabel>();
            Now_HpName = Now_Hp.transform.parent.GetComponent<UILabel>();
            Now_DefName = Now_Def.transform.parent.GetComponent<UILabel>();
            Now_MDefName = Now_MDef.transform.parent.GetComponent<UILabel>();

            Last_Sky = f_GetObject("Last_SkyLabel").GetComponent<UILabel>();
            Last_Atk = f_GetObject("Last_AtkLabel").GetComponent<UILabel>();
            Last_Hp = f_GetObject("Last_HpLabel").GetComponent<UILabel>();
            Last_Def = f_GetObject("Last_DefLabel").GetComponent<UILabel>();
            Last_MDef = f_GetObject("Last_MDefLabel").GetComponent<UILabel>();
            Last_Magic1 = f_GetObject("LastMagic").transform.GetChild(0);
            Last_Magic2 = f_GetObject("LastMagic").transform.GetChild(1);
            Last_AtkName = Last_Atk.transform.parent.GetComponent<UILabel>();
            Last_HpName = Last_Hp.transform.parent.GetComponent<UILabel>();
            Last_DefName = Last_Def.transform.parent.GetComponent<UILabel>();
            Last_MDefName = Last_MDef.transform.parent.GetComponent<UILabel>();

            SkyExp = f_GetObject("SkyExp").GetComponent<UILabel>();
            GoodsNum = f_GetObject("GoodsNum").GetComponent<UILabel>();
            AutoSky = f_GetObject("AutoSky").transform.GetChild(0).GetComponent<UIToggle>();
            ConNum = f_GetObject("Sky_ConNum").GetComponent<UILabel>();
            ConProIcon = f_GetObject("SkyIcon").GetComponent<UI2DSprite>();
            Probability = f_GetObject("Sky_ProbabilityDesc").GetComponent<UILabel>();
            SkyExpSlider = f_GetObject("SkyExpSlider").GetComponent<UISlider>();
            //f_RegClickEvent(AutoSky.gameObject, _UnRegAutoSky);
        }
        f_RegClickEvent(AutoSky.gameObject, _CleanSky);
    }

    private void _AutoSkyPaues(GameObject go, object obj1, object obj2)
    {
        isSendSky = false;
        ccTimeEvent.GetInstance().f_UnRegEvent(Time_AnalogySkyUp);
    }
    /// <summary>
    /// 刷新界面
    /// </summary>
    private void UpdateSkyUI()
    {
        int SkyLv = _Card.uSkyDestinyLv;
        int tNowSktExp = _Card.uSkyDestinyExp;
        IsInitSkyUI = Now_Sky != null;
        if (Data_Pool.m_BaseGoodsPool.f_GetForData5((int)EM_MoneyType.eCardSkyPill) != null)
            SkyGoodsNum = (Data_Pool.m_BaseGoodsPool.f_GetForData5((int)EM_MoneyType.eCardSkyPill) as BaseGoodsPoolDT).m_iNum;
        else
            SkyGoodsNum = 0;

        BaseGoodsDT tGoodsPoolDT = glo_Main.GetInstance().m_SC_Pool.m_BaseGoodsSC.f_GetSC((int)EM_MoneyType.eCardSkyPill) as BaseGoodsDT;

        if (IsInitSkyUI)
        {
            UpdateSkyNowPro(SkyLv);
            UpdateSkyLastPro(SkyLv + 1);
            UpdateSkyLeg(SkyLv, tNowSktExp);
        }
        else
        {
            InitSkyUI();
            UpdateSkyNowPro(SkyLv);
            UpdateSkyLastPro(SkyLv + 1);
            UpdateSkyLeg(SkyLv, tNowSktExp);
        }

        ConProIcon.sprite2D = UITool.f_GetIconSprite(tGoodsPoolDT.iIcon);

    }
    private void UpdateSkyNowPro(int SkyLv)
    {
        NowSkyPro.f_Reset();
        RolePropertyTools.f_CountSkyDesnityForSkyLevel(SkyLv, ref NowSkyPro);
        //int[] tMagicId = ccMath.f_String2ArrayInt(_Card.m_CardDT.szModelMagic1, GameParamConst.StringToIntSign);
        MagicDT tMagic = _Card.m_CardDT.m_aMagicDT[0];      //_GetMagicDt(tMagicId[0]);
        SkyDesnityDT SkyProDT = glo_Main.GetInstance().m_SC_Pool.m_SkyDesnitySC.f_GetSC(0) as SkyDesnityDT;
        Now_Sky.text = SkyLv.ToString();
        Now_AtkName.text = UITool.f_GetProName((EM_RoleProperty)SkyProDT.iSkyDestinyProid1);
        Now_HpName.text = UITool.f_GetProName((EM_RoleProperty)SkyProDT.iSkyDestinyProid2);
        Now_DefName.text = UITool.f_GetProName((EM_RoleProperty)SkyProDT.iSkyDestinyProid3);
        Now_MDefName.text = UITool.f_GetProName((EM_RoleProperty)SkyProDT.iSkyDestinyProid4);
        if (SkyLv < 14)
        {
            Now_Atk.text = (NowSkyPro.f_GetProperty(SkyProDT.iSkyDestinyProid1) / 100) + "%";
            Now_Hp.text = (NowSkyPro.f_GetProperty(SkyProDT.iSkyDestinyProid2) / 100) + "%";
            Now_Def.text = (NowSkyPro.f_GetProperty(SkyProDT.iSkyDestinyProid3) / 100) + "%";
            Now_MDef.text = (NowSkyPro.f_GetProperty(SkyProDT.iSkyDestinyProid4) / 100) + "%";
            //SkyProDT == null ? "0%" : "+" + tText + "%";
            Now_Magic1.GetComponent<UILabel>().text = tMagic.szName;
Now_Magic1.GetChild(0).GetComponent<UILabel>().text = string.Format("Level {0}", SkyLv);
            tMagic = _Card.m_CardDT.m_aMagicDT[1];      // _GetMagicDt(tMagicId[1]);
            Now_Magic2.GetComponent<UILabel>().text = tMagic.szName;
Now_Magic2.GetChild(0).GetComponent<UILabel>().text = string.Format("Level {0}", SkyLv);
        }
        else
        {
            Now_Magic1.GetComponent<UILabel>().text = tMagic.szName;
            Now_Magic1.GetChild(0).GetComponent<UILabel>().text = "Max";
            tMagic = _Card.m_CardDT.m_aMagicDT[1];          // _GetMagicDt(tMagicId[1]);
            Now_Magic2.GetComponent<UILabel>().text = tMagic.szName;
            Now_Magic2.GetChild(0).GetComponent<UILabel>().text = "Max";
            Now_Atk.text = Now_Hp.text = Now_Def.text = Now_MDef.text = "Max";
        }
    }

    private void UpdateSkyLastPro(int SkyLv)
    {
        NextSkyPro.f_Reset();
        RolePropertyTools.f_CountSkyDesnityForSkyLevel(SkyLv, ref NextSkyPro);
        //从第一级开始至最后级属性id不变 取第一个数据即可
        SkyDesnityDT SkyProDT = glo_Main.GetInstance().m_SC_Pool.m_SkyDesnitySC.f_GetSC(0) as SkyDesnityDT;
        //int[] tMagicId = ccMath.f_String2ArrayInt(_Card.m_CardDT.szModelMagic1, GameParamConst.StringToIntSign);
        MagicDT tMagic = _Card.m_CardDT.m_aMagicDT[0];      // _GetMagicDt(tMagicId[0]);
        int tText = SkyLv * SkyAddPro;
        Last_Sky.text = SkyLv.ToString();
        Last_AtkName.text = UITool.f_GetProName((EM_RoleProperty)SkyProDT.iSkyDestinyProid1);
        Last_HpName.text = UITool.f_GetProName((EM_RoleProperty)SkyProDT.iSkyDestinyProid2);
        Last_DefName.text = UITool.f_GetProName((EM_RoleProperty)SkyProDT.iSkyDestinyProid3);
        Last_MDefName.text = UITool.f_GetProName((EM_RoleProperty)SkyProDT.iSkyDestinyProid4);
        if (SkyLv < 14)
        {
            Last_Atk.text = (NextSkyPro.f_GetProperty(SkyProDT.iSkyDestinyProid1) / 100) + "%";
            Last_Hp.text = (NextSkyPro.f_GetProperty(SkyProDT.iSkyDestinyProid2) / 100) + "%";
            Last_Def.text = (NextSkyPro.f_GetProperty(SkyProDT.iSkyDestinyProid3) / 100) + "%";
            Last_MDef.text = (NextSkyPro.f_GetProperty(SkyProDT.iSkyDestinyProid4) / 100) + "%";
            //Last_Atk.text = Last_Hp.text = Last_Def.text = Last_MDef.text = "+" + tText + "%";

            Last_Magic1.GetComponent<UILabel>().text = tMagic.szName;
            Last_Magic1.GetChild(0).GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(160), SkyLv);
            tMagic = _Card.m_CardDT.m_aMagicDT[1];      // _GetMagicDt(tMagicId[1]);
            Last_Magic2.GetComponent<UILabel>().text = tMagic.szName;
            Last_Magic2.GetChild(0).GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(160), SkyLv);
        }
        else
        {
            Last_Magic1.GetComponent<UILabel>().text = tMagic.szName;
            Last_Magic1.GetChild(0).GetComponent<UILabel>().text = "Max";
            tMagic = _Card.m_CardDT.m_aMagicDT[1];      // _GetMagicDt(tMagicId[1]);
            Last_Magic2.GetComponent<UILabel>().text = tMagic.szName;
            Last_Magic2.GetChild(0).GetComponent<UILabel>().text = "Max";
            Last_Atk.text = Last_Hp.text = Last_Def.text = Last_MDef.text = "Max";
        }
    }

    private void UpdateSkyLeg(int skylv, int tNowSkyExp)
    {
        if (skylv < 13)
        {
            CardSkyDestinyDT tSky = glo_Main.GetInstance().m_SC_Pool.m_CardSkyDestinySC.f_GetSC(skylv + 1) as CardSkyDestinyDT;
            SkyExp.text = string.Format("{0}/{1}", tNowSkyExp, tSky.iNeedExp);
            SkyExpSlider.value = (float)tNowSkyExp / (float)tSky.iNeedExp;

            if (Data_Pool.m_BaseGoodsPool.f_GetForData5((int)EM_MoneyType.eCardSkyPill) != null)
            {
                if (!AutoSky.value)
                    ConNum.text = string.Format("{0}/{1}", tSky.iGoodsNum, (Data_Pool.m_BaseGoodsPool.f_GetForData5((int)EM_MoneyType.eCardSkyPill) as BaseGoodsPoolDT).m_iNum);
                else
                {
                    ConNum.text = string.Format("{0}/{1}", tSky.iGoodsNum, SkyGoodsNum - tSky.iGoodsNum <= 0 ? 0 : (SkyGoodsNum -= tSky.iGoodsNum));
                }
                SkyGoodsNumLast -= tSky.iGoodsNum;
                lackMoeny = SkyGoodsNum >= tSky.iGoodsNum;
            }
            else
            {
                ConNum.text = "0";
                lackMoeny = false;
            }
            if (tNowSkyExp <= tSky.iNowExp1)
                Probability.text = tSky.szProbabilityDesc1;
            else if (tNowSkyExp <= tSky.iNowExp2)
                Probability.text = tSky.szProbabilityDesc2;
            else if (tNowSkyExp <= tSky.iNowExp3)
                Probability.text = tSky.szProbabilityDesc3;
            else if (tNowSkyExp <= tSky.iNowExp4)
                Probability.text = tSky.szProbabilityDesc4;
            else
                Probability.text = tSky.szProbabilityDesc5;
        }
        else
        {

            SkyExp.text = string.Format("max");
            SkyExpSlider.value = 1;
        }

    }

    private void Btn_Sky(GameObject go, object obj1, object obj2)
    {
        SendSky(null);
    }

    private void OpenSky(GameObject go, object obj1, object obj2)
    {
        if (AutoSky != null)
            AutoSky.value = false;
        UpdateSkyUI();
    }
    private void SendSky(object obj)
    {
        if (isSendSky)
        {
            return;
        }
        if (!lackMoeny)
        {
            GetWayPageParam tGetWayPageParam = new GetWayPageParam(EM_ResourceType.Good, (int)EM_MoneyType.eCardSkyPill, this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayPageParam);
            AutoSky.value = false;
            return;
        }
        if (_Card.uSkyDestinyLv >= 13)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(161));
            AutoSky.value = false;
            return;
        }
        isSendSky = true;
        UpdateSkyUI();
        //发送协议
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_CardPool.f_CardSky(_Card.iId, SetSkyNum(), AnalogySkyUp, SkySocketCallback);
    }
    private void SkySuc(object obj)
    {
Debug.LogError("=====================Upgrade successful====");
        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void SkyFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.f_GetError((int)obj);
    }

    private void AnalogySkyUp(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        CMsc_SC_CardSkyDestiny tCradSky = (CMsc_SC_CardSkyDestiny)obj;
        SkyNum = 0;
        BeforeSkyExp = 0;
        SkyGoodsNumLast = Data_Pool.m_BaseGoodsPool.f_GetForData5((int)EM_MoneyType.eCardSkyPill) == null ? 0 : (Data_Pool.m_BaseGoodsPool.f_GetForData5((int)EM_MoneyType.eCardSkyPill) as BaseGoodsPoolDT).m_iNum;
        SkyRealNum = tCradSky.uRealNum;
        BeforeSkyExp = _Card.uSkyDestinyExp;
        BeforeSkyLv = _Card.uSkyDestinyLv;
        ccTimeEvent.GetInstance().f_UnRegEvent(Time_AnalogySkyUp);
        Time_AnalogySkyUp = ccTimeEvent.GetInstance().f_RegEvent(0.2f, true, tCradSky, AnalogySkyUI);
    }
    private void AnalogySkyUI(object obj)
    {
        if (!lackMoeny)   //道具数量
        {
            isSendSky = false;
            ccTimeEvent.GetInstance().f_UnRegEvent(Time_AnalogySkyUp);
            UpdateSkyUI();
            return;
        }
        CMsc_SC_CardSkyDestiny tCradSky = (CMsc_SC_CardSkyDestiny)obj;

        if (!AutoSky.value)
        {
            if (BeforeSkyLv < _Card.uSkyDestinyLv)
                UI_SkySucTrip(_Card.uSkyDestinyLv);
            UpdateSkyUI();
            isSendSky = false;
            ccTimeEvent.GetInstance().f_UnRegEvent(Time_AnalogySkyUp);
            glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(UITool.GetEnumName(typeof(AudioEffectType), AudioEffectType.SkyLife));
            return;
        }
        if (SkyNum >= SkyRealNum)
        {
            isSendSky = false;
            ccTimeEvent.GetInstance().f_UnRegEvent(Time_AnalogySkyUp);
            if (BeforeSkyLv < _Card.uSkyDestinyLv && tCradSky.IsSkyUp == 1)
            {
                AutoSky.value = false;
                UI_SkySucTrip(_Card.uSkyDestinyLv);
            }
            else if (AutoSky.value && tCradSky.IsSkyUp == 0)
                Data_Pool.m_CardPool.f_CardSky(_Card.iId, SetSkyNum(), AnalogySkyUp, SkySocketCallback);
            UpdateSkyUI();
            MessageBox.DEBUG(CommonTools.f_GetTransLanguage(162) + "     " + SkyNum + "    " + CommonTools.f_GetTransLanguage(163));
            return;
        }
        BeforeSkyExp += tCradSky.bExp[SkyNum];
        UpdateSkyLeg(BeforeSkyLv, BeforeSkyExp);
        SkyNum++;
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(UITool.GetEnumName(typeof(AudioEffectType), AudioEffectType.SkyLife));
    }
    private void UI_SkySucTrip(int lv)
    {
        GameObject SkyTrip = f_GetObject("SkySuc");
        SkyTrip.SetActive(true);
        GameObject BeforePro = f_GetObject("SkySuc_BeforePro");
        GameObject LastPro = f_GetObject("SkySuc_LastPro");

        SkyTrip.transform.Find("Body/Title/Label").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(164), lv);

        //int[] tMagicId = ccMath.f_String2ArrayInt(_Card.m_CardDT.szModelMagic1, GameParamConst.StringToIntSign);
        MagicDT tMagic = _Card.m_CardDT.m_aMagicDT[0];      // _GetMagicDt(tMagicId[0]);
        MagicDT tMagic2 = _Card.m_CardDT.m_aMagicDT[1];      //_GetMagicDt(tMagicId[1]);
        SkyDesnityDT SkyProDT = glo_Main.GetInstance().m_SC_Pool.m_SkyDesnitySC.f_GetSC(0) as SkyDesnityDT;
        RoleProperty RolePro = new RoleProperty();
        RolePropertyTools.f_CountSkyDesnityForSkyLevel(lv - 1, ref RolePro);

        int ProID = 0;
        for (int i = 1; i <= 4; i++)
        {
            Transform ProName = BeforePro.transform.Find("Name" + i);
            if (i == 0) ProID = SkyProDT.iSkyDestinyProid1;
            else if (i == 1) ProID = SkyProDT.iSkyDestinyProid2;
            else if (i == 2) ProID = SkyProDT.iSkyDestinyProid3;
            else if (i == 3) ProID = SkyProDT.iSkyDestinyProid4;
            ProName.GetComponent<UILabel>().text = UITool.f_GetProName((EM_RoleProperty)ProID);
            ProName.Find("Label").GetComponent<UILabel>().text = (RolePro.f_GetProperty(ProID) / 100) + "%";
        }
        RolePro.f_Reset();
        RolePropertyTools.f_CountSkyDesnityForSkyLevel(lv, ref RolePro);
        for (int i = 1; i <= 4; i++)
        {
            if (i == 0) ProID = SkyProDT.iSkyDestinyProid1;
            else if (i == 1) ProID = SkyProDT.iSkyDestinyProid2;
            else if (i == 2) ProID = SkyProDT.iSkyDestinyProid3;
            else if (i == 3) ProID = SkyProDT.iSkyDestinyProid4;
            LastPro.transform.Find("Pro" + i).GetComponent<UILabel>().text = (RolePro.f_GetProperty(ProID) / 100) + "%";
        }
        BeforePro.transform.GetChild(4).GetComponent<UILabel>().text = tMagic.szName;
        BeforePro.transform.GetChild(5).GetComponent<UILabel>().text = tMagic2.szName;
        for (int i = 0; i < 2; i++)
        {
            BeforePro.transform.GetChild(i + 4).GetChild(0).GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(160), (lv - 1));
            LastPro.transform.GetChild(i + 4).GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(160), lv);
        }




        //LastPro.transform.GetChild(i).GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(160), lv);
        //for (int i = 0; i < BeforePro.transform.childCount; i++)
        //{
        //if (BeforePro.transform.GetChild(i).name == "Ability1" || BeforePro.transform.GetChild(i).name == "Ability2")
        //{
        //    BeforePro.transform.GetChild(4).GetComponent<UILabel>().text = tMagic.szName;
        //    BeforePro.transform.GetChild(5).GetComponent<UILabel>().text = tMagic2.szName;
        //    BeforePro.transform.GetChild(i).GetChild(0).GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(160), (lv - 1));
        //    LastPro.transform.GetChild(i).GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(160), lv);
        //}
        //else
        //{
        //    BeforePro.transform.GetChild(i).GetChild(0).GetComponent<UILabel>().text = (lv - 1) * 6 + "%";
        //    LastPro.transform.GetChild(i).GetComponent<UILabel>().text = (lv * 6) + "%";
        //}
        GameObject Mark = SkyTrip.transform.Find("MarkBox").gameObject;
        f_RegClickEvent(Mark, (GameObject go, object obj1, object obj2) =>
        {
            SkyTrip.SetActive(false);
            BeforeSkyLv = _Card.uSkyDestinyLv;
            isSendSky = false;
            ccTimeEvent.GetInstance().f_UnRegEvent(Time_AnalogySkyUp);
        });

        //重新计算战斗力
        glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.PlayerFightPowerChange);
    }


    private MagicDT _GetMagicDt(int id)
    {
        return glo_Main.GetInstance().m_SC_Pool.m_MagicSC.f_GetSC(id) as MagicDT;
    }

    private int SetSkyNum()
    {
        CardSkyDestinyDT tSkyDestiny = glo_Main.GetInstance().m_SC_Pool.m_CardSkyDestinySC.f_GetSC(_Card.uSkyDestinyLv + 1) as CardSkyDestinyDT;
        if (tSkyDestiny == null)
        {
            tSkyDestiny = glo_Main.GetInstance().m_SC_Pool.m_CardSkyDestinySC.f_GetSC(_Card.uSkyDestinyLv) as CardSkyDestinyDT;
        }
        int SkyNum = (tSkyDestiny.iNowExp5 - _Card.uSkyDestinyExp) / tSkyDestiny.iOnceUp / 2;
        if (SkyNum >= 100)   //算出大于100的赋值给100
            SkyNum = 100;
        if (SkyNum <= 0)
        {
            SkyNum = 1;
        }
        //算出
        if (SkyNum * tSkyDestiny.iGoodsNum > (Data_Pool.m_BaseGoodsPool.f_GetForData5((int)EM_MoneyType.eCardSkyPill) as BaseGoodsPoolDT).m_iNum)
            SkyNum = (Data_Pool.m_BaseGoodsPool.f_GetForData5((int)EM_MoneyType.eCardSkyPill) as BaseGoodsPoolDT).m_iNum / tSkyDestiny.iGoodsNum;
        if (!AutoSky.value)
        {
            SkyNum = 1;
        }
        return SkyNum;
    }

    private void _CleanSky(GameObject go, object obj1, object obj2)
    {

        if (isSendSky)
        {
            AutoSky.value = true;
            return;
        }
        else
        {
            if (LocalDataManager.f_GetLocalData<bool>(LocalDataType.AutoSky)) { return; }

PopupMenuParams tParams = new PopupMenuParams("Nhắc nhở", "Một khi đã chọn, nó không thể hoàn tác cho đến khi thăng cấp sao hoặc Đá định mệnh được sử dụng",
                "Xác nhận", _SkyPopSuc, "Hủy", _SkyPopFill, null, null, PopupMenuParams.PopSaveParam.Sky);

            ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParams);
        }

    }



    void _SkyPopSuc(object obj)
    {
        AutoSky.value = true;
    }
    void _SkyPopFill(object obj)
    {
        AutoSky.value = false;
    }
    #endregion

    #region 图鉴打开
    /// <summary>
    /// 图鉴打开
    /// </summary>
    private void UI_OpenHandbook()
    {
        GetWayTools.ShowContent(f_GetObject("GetWayScrollview"), f_GetObject("GetWay_ItemParent"), f_GetObject("GetWay_Item"),
            new GetWayPageParam(EM_ResourceType.Card, _Card.m_CardDT.iId, null), this);
    }
    /// <summary>
    /// 页面关闭
    /// </summary>
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        stackCardBox.Clear();
        StaticValue.mGetWayToBattleParam.f_Empty();
    }
    /// <summary>
    /// UI UnHold
    /// </summary>
    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        GetWayTools.ShowContent();

        if (stackCardBox.Count > 0)
        {
            CardBox curCardBox = stackCardBox.Peek();
            tBoxTabType = curCardBox.m_bType;
            _Card = curCardBox.m_Card;
            tBoxType = curCardBox.m_oType;
        }
        else
        {
MessageBox.ASSERT("CardProperty UI_UNHOLD, Exception occurred while sorting interface data！");
        }

        //所有信息都要再更新一遍，要不卡牌养成缺物跳转到商城，再打开图鉴卡牌信息，，再返回卡牌养成时卡牌变成了图鉴卡牌信息！！！
        UpdateCardPro();
        UpdateIntroduce();
        IninAwaken();
        UI_ChangeUI(tBoxTabType);
        SetBtnShow(tBoxType);
    }
    #endregion

    #region 羁绊

    void UpdateFate()
    {
        if (Fate_Desc == null)
            Fate_Desc = f_GetObject("FateDesc");
        if (Fate_DescParent == null)
            Fate_DescParent = f_GetObject("FateDescParent");
        float _mDescHigh = 0;
        for (int i = 0; i < Fate_DescParent.transform.childCount; i++)
        {
            Destroy(Fate_DescParent.transform.GetChild(i).gameObject);
        }
        Data_Pool.m_TeamPool.f_UpdateCardFate(_Card);
        for (int i = 0; i < _Card.m_CardFatePoolDT.m_aFateList.Count; i++)
        {
            GameObject CopyFateDesc = NGUITools.AddChild(Fate_DescParent, Fate_Desc);
            CopyFateDesc.SetActive(true);
            CopyFateDesc.transform.localPosition = new Vector3(0, _mDescHigh, 0);
            CopyFateDesc.transform.Find("DescName").GetComponent<UILabel>().text =
                _Card.m_CardFatePoolDT.m_aFateList[i].szName;
            //int[] _mNeedGoods = ccMath.f_String2ArrayInt(_Card.m_CardFatePoolDT.m_aFateList[i].szGoodsId, ";");
            string[] szReadme = ccMath.f_String2ArrayString(_Card.m_CardFatePoolDT.m_aFateList[i].szReadme, "：");
            CopyFateDesc.transform.Find("Desc").GetComponent<UILabel>().text = szReadme[1];
            _mDescHigh -= (CopyFateDesc.transform.Find("Desc").GetComponent<UILabel>().height + 20);
        }
        Fate_DescParent.GetComponent<BoxCollider>().size += new Vector3(0, Mathf.Abs(_mDescHigh), 0);

    }

    List<GameObject> FateTabFateItem;
    List<GameObject> FateTabFateDesc;
    Transform FateTab_FateItem;
    Transform FateTab_FateItemParent;
    Transform FateTab_FateDesc;
    Transform FateTab_FateDescParent;
    Transform FateTabWidget;
    private void UpdateFateTab()
    {
        if (FateTab_FateItem == null)
            FateTab_FateItem = f_GetObject("Fate").transform.Find("FateCard/Item");
        if (FateTab_FateItemParent == null)
            FateTab_FateItemParent = f_GetObject("Fate").transform.Find("FateCard/FateCard_ScrollView/Grid");
        if (FateTab_FateDesc == null)
            FateTab_FateDesc = f_GetObject("Fate").transform.Find("FataTab/Body/Label");
        if (FateTab_FateDescParent == null)
            FateTab_FateDescParent = f_GetObject("Fate").transform.Find("FataTab/Body");
        if (FateTabFateItem == null)
            FateTabFateItem = new List<GameObject>();
        if (FateTabFateDesc == null)
            FateTabFateDesc = new List<GameObject>();
        if (FateTabWidget == null)
            FateTabWidget = f_GetObject("Fate").transform.Find("FataTab/FateTabWidget");
        UIGrid FateTab_FateDescParentGrid = FateTab_FateDescParent.GetComponent<UIGrid>();
        //////////////////////////////////////////////////////////////////////////   Desc
        int DescNum = 0;
        for (int i = 0; i < _Card.m_CardFatePoolDT.m_aFateList.Count; i++)
        {
            DescNum++;
            if (FateTabFateDesc.Count - 1 < DescNum)
            {
                GameObject go = NGUITools.AddChild(FateTab_FateDescParent.gameObject, FateTab_FateDesc.gameObject);
                FateTabFateDesc.Add(go);
            }
            FateTabFateDesc[i].SetActive(true);
            FateTabFateDesc[i].GetComponent<UILabel>().text = _Card.m_CardFatePoolDT.m_aFateList[i].szName;
        }
        for (int i = DescNum; i < FateTabFateDesc.Count; i++)
            FateTabFateDesc[i].SetActive(false);
        FateTab_FateDescParentGrid.enabled = true;
        //////////////////////////////////////////////////////////////////////////  Item
        int[] tArr;
        int ListIndex = 0;
        for (int i = 0; i < _Card.m_CardFatePoolDT.m_aFateList.Count; i++)
        {
            tArr = ccMath.f_String2ArrayInt(_Card.m_CardFatePoolDT.m_aFateList[i].szGoodsId, GameParamConst.StringToIntSign);
            for (int j = 0; j < tArr.Length; j++)
            {
                if (tArr[j] != 0)
                {
                    ListIndex++;
                    if (FateTabFateItem.Count - 1 < ListIndex)
                    {
                        GameObject go = NGUITools.AddChild(FateTab_FateItemParent.gameObject, FateTab_FateItem.gameObject);
                        FateTabFateItem.Add(go);
                    }
                    FateTabFateItem[ListIndex - 1].SetActive(true);
                    UpdateFateItem(FateTabFateItem[ListIndex - 1], _Card.m_CardFatePoolDT.m_aFateList[i], tArr, j);
                }
            }
        }
        for (int i = ListIndex; i < FateTabFateItem.Count; i++)
            FateTabFateItem[i].SetActive(false);
        FateTab_FateItemParent.GetComponent<UIGrid>().enabled = true;
    }
    private void UpdateFateItem(GameObject go, CardFateDataDT tFate, int[] tArr, int index)
    {
        Transform tTran = go.transform;
        UISprite Case = tTran.Find("Case").GetComponent<UISprite>();
        UILabel Desc = tTran.Find("Desc").GetComponent<UILabel>();
        UILabel Name = tTran.Find("Name").GetComponent<UILabel>();
        UI2DSprite Icon = tTran.Find("Icon").GetComponent<UI2DSprite>();
        GameObject GoWay = tTran.Find("GoWay").gameObject;
        ResourceCommonDT tResource = new ResourceCommonDT();
        tResource.f_UpdateInfo((byte)tFate.iGoodsType, tArr[index], 1);

        Case.spriteName = UITool.f_GetImporentCase(tResource.mImportant);
        Desc.text = tFate.szReadme;
        Name.text = UITool.f_GetImporentForName(tResource.mImportant, tResource.mName);// tResource.mName;
        Icon.sprite2D = UITool.f_GetIconSprite(tResource.mIcon);

        f_RegClickEvent(GoWay, FateItemGoWay, tFate.iGoodsType, tArr[index]);
    }
    private void FateItemGoWay(GameObject go, object obj1, object obj2)
    {
        GetWayPageParam tParam = new GetWayPageParam((EM_ResourceType)((int)obj1), (int)obj2, this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tParam);
    }

    #endregion


    #region 神器 Artifact

    private Transform m_ArtifactRoot;

    private UILabel m_ArtifactLv;
    private UILabel m_ArtifactHp;
    private UILabel m_ArtifactAtk;
    private UILabel m_ArtifactDef;
    private UILabel m_ArtifactMDef;

    private UI2DSprite m_ArtifactGoodIcon;
    private UISprite m_ArtifactGoodFrame;
    private UILabel m_ArtifactGoodName;
    private UILabel m_ArtifactGoodNum;

    private UILabel m_ArtifactUpgradeBtnLabel;
    private UILabel m_ArtifactLvMaxTip;

    private ArtifactDT m_LastArtifactTemplate;
    private ArtifactDT m_ArtifactShowTemplate;
    private CardArtifactPoolDT m_curArtifactPoolDt;

    private GameObject m_OutEffect;
    private GameObject m_ArtfactEffectSpine;
    //private GameObject m_ArtifactEffectSpine;

    private Vector3 m_ArtifactEffectPos;
    private Vector3 m_ArtifactEffectScale;
    private void f_InitArtifactView()
    {
        m_ArtifactRoot = f_GetObject("ArtifactRoot").transform;
        m_ArtifactLv = m_ArtifactRoot.Find("ArtifactLv").GetComponent<UILabel>();
        m_ArtifactHp = m_ArtifactRoot.Find("LabelGrid/HpTitle/ArtifactHp").GetComponent<UILabel>();
        m_ArtifactAtk = m_ArtifactRoot.Find("LabelGrid/AtkTitle/ArtifactAtk").GetComponent<UILabel>();
        m_ArtifactDef = m_ArtifactRoot.Find("LabelGrid/DefTitle/ArtifactDef").GetComponent<UILabel>();
        m_ArtifactMDef = m_ArtifactRoot.Find("LabelGrid/MDefTitle/ArtifactMDef").GetComponent<UILabel>();

        m_ArtifactGoodIcon = m_ArtifactRoot.Find("Good/GoodIcon").GetComponent<UI2DSprite>();
        m_ArtifactGoodFrame = m_ArtifactRoot.Find("Good/GoodFrame").GetComponent<UISprite>();
        m_ArtifactGoodName = m_ArtifactRoot.Find("Good/GoodName").GetComponent<UILabel>();
        m_ArtifactGoodNum = m_ArtifactRoot.Find("Good/GoodNum").GetComponent<UILabel>();
        m_ArtifactUpgradeBtnLabel = m_ArtifactRoot.Find("ArtifactUpgradeBtn/ArtifactUpgradeBtnLabel").GetComponent<UILabel>();
        m_OutEffect = f_GetObject("ArtifactEffect");
        m_ArtifactLvMaxTip = m_ArtifactRoot.Find("Good/LvMaxTip").GetComponent<UILabel>();
        f_RegClickEvent("ArtifactUpgradeBtn", f_OnArtifactUpgradeClick);
        m_ArtfactEffectSpine = f_GetObject("ArtifactSpine");

        //m_ArtifactEffectSpine = f_GetObject("ArtifactEffect");
        //m_ArtifactEffectPos = m_ArtifactEffectSpine.transform.localPosition;
        //m_ArtifactEffectScale = m_ArtifactEffectSpine.transform.localScale;
    }

    private void f_ShowHideArtifactView(CardBox.BoxType boxType)
    {
        m_ArtifactRoot.gameObject.SetActive(boxType == CardBox.BoxType.Artifact);
        f_GetObject("Card_Explain").SetActive(boxType != CardBox.BoxType.Artifact);
        f_GetObject("AwakenStar").SetActive(_Card.m_iLv >= 50 && boxType != CardBox.BoxType.Artifact);
        f_GetObject("AwakenStarBg").SetActive(_Card.m_iLv >= 50 && boxType != CardBox.BoxType.Artifact);
        f_GetObject("Chage").SetActive(_Card.m_CardDT.iCardType != (int)EM_CardType.RoleCard && tBoxType == CardBox.OpenType.battleArray && boxType != CardBox.BoxType.Artifact);
        f_GetObject("Image_Card").SetActive(boxType != CardBox.BoxType.Artifact);
        //m_ArtifactEffectSpine.SetActive(boxType == CardBox.BoxType.Artifact);
        if (Artifact_GoodsEffect != null)
            Artifact_GoodsEffect.SetActive(boxType == CardBox.BoxType.Artifact);
    }

    private void f_UpdateArtifactView()
    {
        m_curArtifactPoolDt = _Card.m_ArtifactPoolDT;
        //特效相关
        m_OutEffect.SetActive(m_curArtifactPoolDt.Lv / GameParamConst.ArtifactLvPreStep >= 3);
        //m_ArtfactEffectSpine = glo_Main.GetInstance().m_ResourceManager.f_CreateMagic(20010);
        //if(m_ArtfactEffectSpine != null)
        m_ArtfactEffectSpine.SetActive(m_curArtifactPoolDt.Lv / GameParamConst.ArtifactLvPreStep >= 5);

        if (m_curArtifactPoolDt.Lv / GameParamConst.ArtifactLvPreStep >= 1)
        {
            if (Artifact_GoodsEffect == null)
                Artifact_GoodsEffect = UITool.f_CreateEffect_Old(UIEffectName.Artifact, EffectParent, new Vector3(-0.78f, -0.08f), 1, 0, UIEffectName.UIEffectAddress3);
            if (Artifact_GoodsEffect != null)
                Artifact_GoodsEffect.SetActive(true);
        }
        else
        {
            if (Artifact_GoodsEffect != null)
                Artifact_GoodsEffect.SetActive(false);
        }


        //end
        m_ArtifactShowTemplate = (ArtifactDT)glo_Main.GetInstance().m_SC_Pool.m_ArtifactSC.f_GetSC(m_curArtifactPoolDt.Lv + 1);
        m_ArtifactUpgradeBtnLabel.text = m_ArtifactShowTemplate != null && m_ArtifactShowTemplate.iId % 10 == 0 ? CommonTools.f_GetTransLanguage(165) : CommonTools.f_GetTransLanguage(166);
        m_ArtifactLvMaxTip.text = string.Empty;
        m_ArtifactLv.text = string.Format(CommonTools.f_GetTransLanguage(175), m_curArtifactPoolDt.Lv / GameParamConst.ArtifactLvPreStep, m_curArtifactPoolDt.Lv % GameParamConst.ArtifactLvPreStep);
        int roleLv = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        int tHp = 0;
        int tAtk = 0;
        int tDef = 0;
        int tMDef = 0;
        if (m_curArtifactPoolDt.Template != null)
        {
            tHp += m_curArtifactPoolDt.Template.iHp;
            tAtk += m_curArtifactPoolDt.Template.iAtk;
            tDef += m_curArtifactPoolDt.Template.iDef;
            tMDef += m_curArtifactPoolDt.Template.iMagDef;
        }
        m_ArtifactHp.text = tHp.ToString();
        m_ArtifactAtk.text = tAtk.ToString();
        m_ArtifactDef.text = tDef.ToString();
        m_ArtifactMDef.text = tMDef.ToString();
        m_ArtifactGoodIcon.gameObject.SetActive(m_ArtifactShowTemplate != null && m_curArtifactPoolDt.Lv < Data_Pool.m_CardPool.ArtifactLvMax);
        m_ArtifactGoodFrame.gameObject.SetActive(m_ArtifactShowTemplate != null && m_curArtifactPoolDt.Lv < Data_Pool.m_CardPool.ArtifactLvMax);
        m_ArtifactGoodName.gameObject.SetActive(m_ArtifactShowTemplate != null && m_curArtifactPoolDt.Lv < Data_Pool.m_CardPool.ArtifactLvMax);
        m_ArtifactGoodNum.gameObject.SetActive(m_ArtifactShowTemplate != null && m_curArtifactPoolDt.Lv < Data_Pool.m_CardPool.ArtifactLvMax);
        if (m_ArtifactShowTemplate != null)
        {
            UITool.f_SetIconSprite(m_ArtifactGoodIcon, EM_ResourceType.Good, m_ArtifactShowTemplate.iItemId);
            BaseGoodsDT tItem = (BaseGoodsDT)glo_Main.GetInstance().m_SC_Pool.m_BaseGoodsSC.f_GetSC(m_ArtifactShowTemplate.iItemId);
            string tName = tItem.szName;
            m_ArtifactGoodFrame.spriteName = UITool.f_GetImporentColorName(tItem.iImportant, ref tName);
            m_ArtifactGoodName.text = tName;
            int tHaveNum = Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(m_ArtifactShowTemplate.iItemId);
            m_ArtifactGoodNum.text = tHaveNum >= m_ArtifactShowTemplate.iItemNum ? string.Format("{0}/{1}", tHaveNum, m_ArtifactShowTemplate.iItemNum) :
                                                                                string.Format("[ff0000]{0}[-]/{1}", tHaveNum, m_ArtifactShowTemplate.iItemNum);
        }
        if (m_ArtifactShowTemplate == null || Data_Pool.m_CardPool.f_IsTrueArtifactMaxLv(roleLv, m_curArtifactPoolDt.Lv))
        {
            m_ArtifactUpgradeBtnLabel.text = CommonTools.f_GetTransLanguage(167);
            m_ArtifactLvMaxTip.text = CommonTools.f_GetTransLanguage(168);
        }
        else if (m_curArtifactPoolDt.Lv >= Data_Pool.m_CardPool.ArtifactLvMax)
        {
            int needRoleLv = Data_Pool.m_CardPool.f_GetArtifactUpgradeNeedRoleLv(roleLv);
            m_ArtifactLvMaxTip.text = string.Format(CommonTools.f_GetTransLanguage(169), needRoleLv);
            return;
        }
        //特效相关
        //if(glo_Main.GetInstance().m_SC_Pool.m_ArtifactSC.f_GetSC(1000000) == null)
        //{
        //    if(m_ArtifactEffectSpine != null)
        //        glo_Main.GetInstance().m_ResourceManager.f_DestorySD(m_ArtifactEffectSpine);

        //    int RefineNum = m_curArtifactPoolDt.Lv / GameParamConst.ArtifactLvPreStep;
        //    m_ArtifactEffectSpine = glo_Main.GetInstance().m_ResourceManager.f_CreateMagic(20010 + RefineNum);
        //    m_ArtifactEffectSpine.transform.localScale = m_ArtifactEffectScale;
        //    m_ArtifactEffectSpine.transform.localPosition = m_ArtifactEffectPos;
        //}
        //end

    }

    private void f_OnArtifactUpgradeClick(GameObject go, object value1, object value2)
    {
        int curArtifactLvMax = Data_Pool.m_CardPool.ArtifactLvMax;
        int roleLv = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        if (curArtifactLvMax == 0)
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(170), Data_Pool.m_CardPool.f_GetArtifactOpenLv()));
            return;
        }
        else if (m_ArtifactShowTemplate == null || Data_Pool.m_CardPool.f_IsTrueArtifactMaxLv(roleLv, m_curArtifactPoolDt.Lv))
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(171));
            return;
        }
        else if (m_curArtifactPoolDt.Lv >= Data_Pool.m_CardPool.ArtifactLvMax)
        {
            int needRoleLv = Data_Pool.m_CardPool.f_GetArtifactUpgradeNeedRoleLv(roleLv);
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(172), needRoleLv));
            return;
        }
        else if (Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(m_ArtifactShowTemplate.iItemId) < m_ArtifactShowTemplate.iItemNum)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(173));
            GetWayPageParam tGetWayParm = new GetWayPageParam(EM_ResourceType.Good, m_ArtifactShowTemplate.iItemId, this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, tGetWayParm);
            return;
        }
        m_LastArtifactTemplate = (ArtifactDT)glo_Main.GetInstance().m_SC_Pool.m_ArtifactSC.f_GetSC(m_curArtifactPoolDt.Lv);
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_ArtifactUpgrade;
        socketCallbackDt.m_ccCallbackFail = f_Callback_ArtifactUpgrade;
        Data_Pool.m_CardPool.f_CardArtifactUpgrade(_Card.iId, socketCallbackDt);
    }

    private void f_Callback_ArtifactUpgrade(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        //UITool.f_CreateEffect_Old(UIEffectName.Artifact, EffectParent, new Vector3(-0.78f, -0.08f), 1, 0, UIEffectName.UIEffectAddress3);
        UITool.f_CreateEffect_Old(UIEffectName.ArtifactUpEffetc, EffectParent, new Vector3(-0.85f, -0.08f), 0.2f, 2f, UIEffectName.UIEffectAddress3);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            if (m_curArtifactPoolDt.Lv % GameParamConst.ArtifactLvPreStep == 0)
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(174));
            else
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(176));
            //添加属性展示
            ArtifactDT tCurDt = (ArtifactDT)glo_Main.GetInstance().m_SC_Pool.m_ArtifactSC.f_GetSC(m_curArtifactPoolDt.Lv);
            List<object> tParam = new List<object>();
            tParam.Add(new Vector2(418, 0));
            AddProTripPageParam addProParamHp = new AddProTripPageParam();
            addProParamHp.addProId = (int)EM_RoleProperty.Hp;
            addProParamHp.addProValue = m_LastArtifactTemplate == null ? tCurDt.iHp : tCurDt.iHp - m_LastArtifactTemplate.iHp;
            tParam.Add(addProParamHp);
            AddProTripPageParam addProParamAtk = new AddProTripPageParam();
            addProParamAtk.addProId = (int)EM_RoleProperty.Atk;
            addProParamAtk.addProValue = m_LastArtifactTemplate == null ? tCurDt.iAtk : tCurDt.iAtk - m_LastArtifactTemplate.iAtk;
            tParam.Add(addProParamAtk);
            AddProTripPageParam addProParamDef = new AddProTripPageParam();
            addProParamDef.addProId = (int)EM_RoleProperty.Def;
            addProParamDef.addProValue = m_LastArtifactTemplate == null ? tCurDt.iDef : tCurDt.iDef - m_LastArtifactTemplate.iDef;
            tParam.Add(addProParamDef);
            AddProTripPageParam addProParamMDef = new AddProTripPageParam();
            addProParamMDef.addProId = (int)EM_RoleProperty.MDef;
            addProParamMDef.addProValue = m_LastArtifactTemplate == null ? tCurDt.iMagDef : tCurDt.iMagDef - m_LastArtifactTemplate.iMagDef;
            tParam.Add(addProParamMDef);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.AddProTripPage, UIMessageDef.UI_OPEN, tParam);

            //刷新界面
            f_UpdateArtifactView();
            glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(UITool.GetEnumName(typeof(AudioEffectType), AudioEffectType.ArtifactUp));

            //重新计算战斗力
            glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.PlayerFightPowerChange);
        }
        else
        {
            UITool.Ui_Trip(string.Format("ErrorCode:{0}", (int)result));
        }
    }

    void _SkyChangeUI(ccCallback Back)
    {
PopupMenuParams tLabel = new PopupMenuParams(CommonTools.f_GetTransLanguage(104), CommonTools.f_GetTransLanguage(105), "Ở lại giao diện này", null, CommonTools.f_GetTransLanguage(AwakeId), Back);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tLabel);
    }

    #endregion


    #region 打开卡牌定位
    private void UpdateCardFithtLabel(GameObject go, object obj1, object obj2)
    {
        f_GetObject("ShowFightType").SetActive(true);
        UILabel CardFight = f_GetObject("ShowFightType").transform.Find("CardFightType").GetComponent<UILabel>();

        CardFight.text = UITool.f_GetCardFightType((EM_CardFightType)_Card.m_CardDT.iCardFightType);

    }

    private void CloseCardFightLabel(GameObject go, object obj1, object obj2)
    {
        f_GetObject("ShowFightType").SetActive(false);
    }

    #endregion
}

public static class S_CardManage
{
    public static CardPoolDT m_Card;
}


public enum AwakenBtnType
{
    OneEquip,   //一键装备
    Awaken,    //  领悟
}

public struct CardBox : Box
{
    public CardPoolDT m_Card;
    public OpenType m_oType;
    public BoxType m_bType;
    public enum BoxType
    {
        Intro,  //信息
        Inten,   //强化
        Refine,  //  精炼
        Awaken,  //   领悟
        Sky,  //天命
        GetWay,  //获取途径
        Fate,  //羁绊
        Artifact, //神器
    }
    public enum OpenType
    {
        handbook,  //图鉴
        battleArray,   //阵容
        Bag,             //背包
        SelectAward,   //选择奖励界面
        CardBattlePage, //斗将界面
    }
}
public interface Box { }
