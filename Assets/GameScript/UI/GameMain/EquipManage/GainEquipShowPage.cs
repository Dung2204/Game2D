using ccU3DEngine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GainEquipShowPage : UIFramwork
{

    private GameObject _btnBack;
    private UILabel _gainTip;   //名字
    UISprite influence;    //势力
    UILabel _Num;         //数量
    UILabel TipColor;     //获得什么颜色
    UI2DSprite _EquipShow;  //显示装备
    UI2DSprite _GodEquipShow;  //

    Transform _card;      //获得卡牌的panl
    private EquipDT _EquipDT;   //装备DT
    private GodEquipDT _GodEquipDT;   //
    CardDT _CardDT;                //卡牌DT
    UI2DSprite _Treasure2DSpr;
    TreasureDT _treasureDT;     //法宝DT
    private EquipSythesis tequip;  //合成回调
    UILabel _Type;                   //装备显示部位 武将显示什么型


    //动画
    UIPlayTween TweenManage;
    EventDelegate tEvent = new EventDelegate();

    private GameObject OneRole;
    private GameObject OneSongRole;
    //特效
    private Transform ParticleParent;
    private GameObject BottomParticle;   //底部特效     
    private GameObject OnParticle;//顶部特效

    private int m_DepthIndex = 20;
    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
        ShowEffectOver = false;

    }

    protected override void InitGUI()
    {
        base.InitGUI();
        influence = f_GetObject("influence").GetComponent<UISprite>();
        _btnBack = f_GetObject("Texture_BG");
        _gainTip = f_GetObject("GainTip").GetComponent<UILabel>();
        _card = f_GetObject("Card").transform;
        _Num = f_GetObject("GoodsNum").GetComponent<UILabel>();
        TipColor = f_GetObject("TipColor").GetComponent<UILabel>();
        _EquipShow = f_GetObject("Equip").GetComponent<UI2DSprite>();
        _Treasure2DSpr = f_GetObject("Treasure").GetComponent<UI2DSprite>();
        _GodEquipShow = f_GetObject("GodEquip").GetComponent<UI2DSprite>();

        //_Type = f_GetObject("Type").GetComponent<UILabel>();
        ParticleParent = f_GetObject("Particle").transform;
        TweenManage = f_GetObject("TweenManage").GetComponent<UIPlayTween>();
        LetterNode = f_GetObject("LetterNode").transform;
        f_RegClickEvent(_btnBack, BackBtnHandle);
        f_RegClickEvent("Btn_Again", Btn_Again);
        f_RegClickEvent(f_GetObject("Btn_Exit"), BackBtnHandle);
    }
    private bool ShowEffectOver = false;
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        _btnBack.gameObject.SetActive(true);
        f_GetObject("ObjShowLater").SetActive(false);
        tequip = (EquipSythesis)e;
        f_GetObject("SongParent").SetActive(false);
        f_GetObject("OneSongParent").SetActive(false);
        UpdateMainUI();
        if (!tequip.IsTenRecruit)
            OneGain();
        else
            TenGain();

        f_GetObject("TenNode").SetActive(tequip.IsTenRecruit);
        // List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
        // listMoneyType.Add(EM_MoneyType.eNorAd);
        // listMoneyType.Add(EM_MoneyType.eGenAd);
        // listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
        // ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, listMoneyType);
        f_LoadTexture();
    }
    private string strTexBgRoot = "UI/TextureRemove/Common/Texture_GainBg";
    /// <summary>
    /// 加载texture
    /// </summary>
    /// 
    private void f_LoadTexture()
    {
        //加载背景图
        UITexture Texture_BG = f_GetObject("Texture_BG").GetComponent<UITexture>();
        //UITexture Texture_BGCopy = f_GetObject("Texture_BGCopy").GetComponent<UITexture>();
		//My Code
		float windowAspect = (float)Screen.width /  (float) Screen.height ;
		MessageBox.ASSERT("" + windowAspect);
		if(windowAspect <= 1.55)
		{
			f_GetObject("Anchor-Center").transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
		}
        //
        EM_RecruitType lotteryId = Data_Pool.m_ShopLotteryPool.lotteryId;
        Texture_BG.mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(GetBGByType(lotteryId));
        if (Texture_BG.mainTexture == null)
        {
            Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot);
            Texture_BG.mainTexture = tTexture2D;
            //Texture_BGCopy.mainTexture = tTexture2D;
        }

    }
    private string GetBGByType(EM_RecruitType type)
    {
        string bg = "UI/TextureRemove/Common/Texture_GainBg";
        switch (type)
        {
            case EM_RecruitType.NorAd:
                bg = "UI/TextureRemove/Shop/BG_Nor";
                break;
            case EM_RecruitType.GenAd:
                bg = "UI/TextureRemove/Shop/BG_Gen";
                break;
            case EM_RecruitType.CampAd:
                bg = "UI/TextureRemove/Shop/BG_Camp";
                break;
        }
        return bg;
    }
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        if (BottomParticle != null)
        {
            Destroy(BottomParticle);
        }
        if (OnParticle != null)
        {
            Destroy(OnParticle);
        }

        CancelInvoke("CaeateEquipModel");
        CancelInvoke("CreateCardModel");
        CancelInvoke("CaeateTreasureModel");
        CancelInvoke("OnShowCardEffectOver");
        CancelInvoke("CreateItem");

        //ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
        //if (RoleArr != null)
        //{
        //    for (int i = 0; i < RoleArr.Length; i++)
        //    {
        //        UITool.f_DestoryStatelObject(RoleArr[i]);
        //    }
        //}
        //if (OneSongRole != null)
        //    UITool.f_DestoryStatelObject(OneSongRole);
        //if (OneRole != null)
        //    UITool.f_DestoryStatelObject(OneRole);
    }
    private void OneGain()
    {
        int templateId = tequip.id;
        switch (tequip.m_Type)
        {
            case EquipSythesis.ResonureType.Equip:
                _EquipDT = (EquipDT)glo_Main.GetInstance().m_SC_Pool.m_EquipSC.f_GetSC(templateId - 1000);
                UpdateMainGui();
                UpdateEquipUI();
                break;
            case EquipSythesis.ResonureType.Card:
                if (tequip.m_ListGetCard.Count > 0)
                {
                    _CardDT = glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(tequip.m_ListGetCard[0]) as CardDT;
                }
                else
                {
                    _CardDT = glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(tequip.id) as CardDT;
                }

                UpdateMainGui();
                UpdateCardUI();
                break;
            case EquipSythesis.ResonureType.Treasure:
                _treasureDT = glo_Main.GetInstance().m_SC_Pool.m_TreasureSC.f_GetSC(templateId) as TreasureDT;
                UpdateMainGui();
                UpdateTreasureUI();
                break;
            case EquipSythesis.ResonureType.GodEquip:
                _GodEquipDT = (GodEquipDT)glo_Main.GetInstance().m_SC_Pool.m_GodEquipSC.f_GetSC(templateId - 1000);
                UpdateMainGui();
                UpdateGodEquipUI();
                break;
            default:
                break;
        }
    }



    private void BackBtnHandle(GameObject go, object value1, object value2)
    {
        //        if (!ShowEffectOver)//等待特效播放完才能关闭
        //           return; 
        if (!showOneGain)
        {
            if (tequip.m_Type == EquipSythesis.ResonureType.Card && !tequip.IsTenRecruit)
            {
                CancelInvoke("CreateCardModel");
                CreateCardModel();
                return;
            }
            else if (tequip.m_Type == EquipSythesis.ResonureType.Equip && !tequip.IsTenRecruit)
            {
                CancelInvoke("CaeateEquipModel");
                CaeateEquipModel();
                return;
            }
            else if (tequip.m_Type == EquipSythesis.ResonureType.Treasure && !tequip.IsTenRecruit)
            {
                CancelInvoke("CaeateTreasureModel");
                CaeateTreasureModel();
                return;
            }
        }
        showOneGain = false;
        if (tequip.IsTenRecruit)
        {
            if (Count < tequip.m_ListGetCard.Count - 1)
            {
                //ccTimeEvent.GetInstance().f_UnRegEvent(Time_CreateRole);
                //_CreateRole(null);
                return;
            }
        }

        if (tequip.m_Type == EquipSythesis.ResonureType.Card && !tequip.IsTenRecruit)
        {
            if (_card.childCount > 0)
            {
                //UITool.f_DestoryStatelObject(_card.GetChild(0).gameObject);
                Destroy(_card.GetChild(0).gameObject);
            }
        }
        if (ParticleParent.Find("GainEffect") != null)
            Destroy(ParticleParent.Find("GainEffect").gameObject);
        if (tequip.IsTenRecruit)
        {
            for (int i = 0; i < RoleArr.Length; i++)
            {
                if (RoleArr[i] != null)
                {
                    Destroy(RoleArr[i]);
                }
                    //UITool.f_DestoryStatelObject(RoleArr[i]);
                RoleArr[i] = null;
                if (i <= 9)
                    TenNode.Find(i.ToString()).gameObject.SetActive(false);
            }

        }
        if (OneSongRole!= null)
        {
            Destroy(OneSongRole);
        }

        RemoveLetter();

        ccTimeEvent.GetInstance().f_UnRegEvent(Time_CreateRole);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainEquipShowPage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
    }

    private void RemoveLetter()
    {
        for (int i = 0; i < 11; i++)
        {
            LetterNode.Find(i.ToString()).gameObject.SetActive(false);
        }
    }

    void Btn_Again(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainEquipShowPage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
        showOneGain = false;
        if (tequip.m_OnCloseCallback != null)
        {
            tequip.m_OnCloseCallback(tequip.mCallbackParam);
            tequip.m_OnCloseCallback = null;
        }
    }

    void UpdateMainGui()
    {
        //if (_card.Find("Model") != null)
        //    UITool.f_DestoryStatelObject(_card.Find("Model").gameObject);
        if (_card.childCount > 0)
        {
            //UITool.f_DestoryStatelObject(_card.GetChild(0).gameObject);
            Destroy(_card.GetChild(0).gameObject);
        }
        if (ParticleParent.Find("GainEffect") != null)
            Destroy(ParticleParent.Find("GainEffect").gameObject);
        influence.gameObject.SetActive(false);
        f_GetObject("Btn_Again").SetActive(false);
        f_GetObject("Btn_Exit").SetActive(false);
        f_GetObject("influenceBg").SetActive(false);
        switch (tequip.m_Type)
        {
            case EquipSythesis.ResonureType.Equip:
                _PlayAudio(_EquipDT.iColour);
                BottomParticle = UITool.f_CreateEffect_Old(UIEffectName.kapai_kpsphc_01, ParticleParent, Vector3.zero, 0.15f, 3f, UIEffectName.UIEffectAddress1);
                break;
            case EquipSythesis.ResonureType.Card:
                _PlayAudio(_CardDT.iImportant);
                InitLetter();
                break;
            case EquipSythesis.ResonureType.Treasure:
                _PlayAudio(_treasureDT.iImportant);
                BottomParticle = UITool.f_CreateEffect_Old(UIEffectName.kapai_kpsphc_01, ParticleParent, Vector3.zero, 0.15f, 3f, UIEffectName.UIEffectAddress1);
                break;
            case EquipSythesis.ResonureType.GodEquip:
                _PlayAudio(_GodEquipDT.iColour);
                BottomParticle = UITool.f_CreateEffect_Old(UIEffectName.kapai_kpsphc_01, ParticleParent, Vector3.zero, 0.15f, 3f, UIEffectName.UIEffectAddress1);
                break;
            default:
                break;
        }      
    }

    void _PlayAudio(int Imporent)
    {
        switch ((EM_Important)Imporent)
        {
            case EM_Important.White:
            case EM_Important.Green:
            case EM_Important.Blue:
                glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(AudioEffectType.Gain_Whith);
                break;
            case EM_Important.Purple:
                glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(AudioEffectType.Gain_Purple);
                break;
            case EM_Important.Oragen:
                glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(AudioEffectType.Gain_Orange);
                break;
            case EM_Important.Red:
                glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(AudioEffectType.Gain_Red);
                break;
            case EM_Important.Gold:
				glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(AudioEffectType.Gain_Red);
                break;
        }
    }
    void UpdateMainUI()
    {
        _Num.text = tequip.num > 1 ? "X" + tequip.num : "";
        _card.gameObject.SetActive(tequip.m_Type == EquipSythesis.ResonureType.Card);
        _EquipShow.gameObject.SetActive(tequip.m_Type == EquipSythesis.ResonureType.Equip);
        _Treasure2DSpr.gameObject.SetActive(tequip.m_Type == EquipSythesis.ResonureType.Treasure);
        _GodEquipShow.gameObject.SetActive(tequip.m_Type == EquipSythesis.ResonureType.GodEquip);

    }
    void UpdateEquipUI()
    {
        //_gainTip.text = string.Format("{0}", _EquipDT.szName);
        influence.spriteName = "";
        _EquipShow.sprite2D = null;
        _EquipShow.transform.Find("IconBorder").GetComponent<UISprite>().spriteName = "";
        Invoke("CaeateEquipModel", 1.1f);

        TipColor.gameObject.SetActive(false);
        _EquipShow.transform.Find("TipColor").GetComponent<UILabel>().text = UITool.f_GetImporentForName(_EquipDT.iColour, _EquipDT.szName);
        //_Type.text = UITool.f_GetEquipPart(_EquipDT.iSite);
    }

    void CaeateEquipModel()
    {
        CreateOnTheCardEffect(_EquipDT.iColour);
        TweenScale EquipTween = _EquipShow.gameObject.AddComponent<TweenScale>();
        EquipTween.from = Vector3.zero; EquipTween.to = Vector3.one;
        EquipTween.duration = 0.2f;
        tEvent = new EventDelegate(EquipTweenOver);
        EquipTween.onFinished.Clear();
        EquipTween.onFinished.Add(tEvent);
        UITool.f_SetIconSprite(_EquipShow, EM_ResourceType.Equip, _EquipDT.iId);
        _EquipShow.transform.Find("IconBorder").GetComponent<UISprite>().spriteName = UITool.f_GetImporentCase(_EquipDT.iColour);
        showOneGain = true;
    }


    void UpdateCardUI()
    {
        _gainTip.text = string.Format("{0}", _CardDT.szName);
        influence.spriteName = UITool.f_GetCardCampForUISpriteName(_CardDT.iCardCamp);
        influence.MakePixelPerfect();
        Invoke("CreateCardModel", 2.2f);
        influence.gameObject.SetActive(true);
        f_GetObject("influenceBg").SetActive(true);
        f_GetObject("Btn_Again").SetActive(tequip.isShowBotton);
        f_GetObject("Btn_Exit").SetActive(tequip.isShowBotton);
        _gainTip.text = UITool.f_GetImporentForName(_CardDT.iImportant, _CardDT.szName);
        int Times = 0;
        string TipLabel = string.Empty;
        int LabelId = 0;
        //TipColor.gameObject.SetActive(tequip.isShowBotton);
        TipColor.gameObject.SetActive(false);
        string spriteName = string.Empty;
        string num = string.Empty;
        // đóng quay 10 tặng 1
        //switch (Data_Pool.m_ShopLotteryPool.lotteryId)
        //{
        //    case EM_RecruitType.NorAd:
        //        Times = ((ShopLotteryPoolDT)Data_Pool.m_ShopLotteryPool.f_GetForId((long)EM_RecruitType.NorAd)).totalTimes % 10;
        //        TipLabel = CommonTools.f_GetTransLanguage(2157);
        //        LabelId = 2158;
        //        break;
        //    case EM_RecruitType.GenAd:
        //        Times = ((ShopLotteryPoolDT)Data_Pool.m_ShopLotteryPool.f_GetForId((long)EM_RecruitType.GenAd)).totalTimes % 10;
        //        TipLabel = CommonTools.f_GetTransLanguage(2156);
        //        LabelId = 2159;
        //        break;
        //    default:
        //        break;
        //}
        //if (Times != 0)
        //    TipColor.text = string.Format(TipLabel, 10 - Times);
        //else
        //    TipColor.text = CommonTools.f_GetTransLanguage(LabelId);
        if (tequip.isShowBotton)
        {
            ShopLotteryPoolDT lotteryPoolDT = Data_Pool.m_ShopLotteryPool.f_GetForId((long)Data_Pool.m_ShopLotteryPool.lotteryId) as ShopLotteryPoolDT;
            ResourceCommonDT OneCost1 = CommonTools.f_GetCommonResourceByResourceStr(lotteryPoolDT.shopLotteryDT.szOnceCost1);
            int GodosNum = Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(GetReruitPropId(Data_Pool.m_ShopLotteryPool.lotteryId));

            if (GodosNum > 1)
            {
                spriteName = UITool.f_GetMoneySpriteName((EM_MoneyType)OneCost1.mResourceId);
                num = "[24ff00]" + OneCost1.mResourceNum;
            }
            else if (GodosNum < 1 && lotteryPoolDT.shopLotteryDT.szOnceCost2 != "0")
            {
                spriteName = "Icon_Sycee";
                OneCost1 = CommonTools.f_GetCommonResourceByResourceStr(lotteryPoolDT.shopLotteryDT.szOnceCost2);
                if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee) >= OneCost1.mResourceNum)
                    num = "[24ff00]" + OneCost1.mResourceNum;
                else
                    num = "[ec2626]" + OneCost1.mResourceNum;
            }
            else if (GodosNum < 1 && lotteryPoolDT.shopLotteryDT.szOnceCost2 == "0")
            {
                spriteName = UITool.f_GetMoneySpriteName((EM_MoneyType)OneCost1.mResourceId);
                num = "[ec2626]" + OneCost1.mResourceNum;
            }
        }
        MoneyUI(spriteName, num);
        //_Type.text = UITool.f_GetCardFightType(_CardDT.iCardFightType);
    }

    private void MoneyUI(string spriteName, string num)
    {
        Transform tran = f_GetObject("Money").transform;
        UISprite MoneyIcon = tran.Find("MoneyIcon").GetComponent<UISprite>();
        UILabel Num = tran.Find("Num").GetComponent<UILabel>();
        MoneyIcon.spriteName = spriteName;
        Num.text = num;
    }
    private int GetReruitPropId(EM_RecruitType recruitType)
    {
        switch (recruitType)
        {
            case EM_RecruitType.NorAd://战将令206，神将令207
                return 206;
            case EM_RecruitType.GenAd:
                return 207;
            case EM_RecruitType.CampAd:
                return 503;
        }
        return 206;
    }

    private bool showOneGain = false;
    void CreateCardModel()
    {
        if (BottomParticle != null)
        {
            Destroy(BottomParticle);
        }
        if (OnParticle != null)
        {
            Destroy(OnParticle);
        }
        RemoveLetter();
        //OneRole = UITool.f_GetStatelObject(_CardDT.iId, _card, Vector3.zero, new Vector3(0, -200, 0));
        //OneRole.transform.localScale = Vector3.one * 200;
        //OneRole.GetComponent<Renderer>().sortingLayerName = "Default";
        //OneRole.GetComponent<Renderer>().sortingOrder = 20;
        OneRole = Instantiate(f_GetObject("ItemCard"), _card);
        string Name = UITool.f_ReplaceName(_CardDT.szName, " ", "\n");
        string BorderName = UITool.f_GetImporentCase(_CardDT.iImportant);
        string CampName = UITool.f_GetCampName(_CardDT.iCardCamp);
        string BorderBGName = UITool.f_GetImporentCase(_CardDT.iImportant);
        OneRole.SetActive(true);
        OneRole.GetComponent<CardShowCardItemCtl>().SetData(UITool.f_GetGainCardIcon(_CardDT.iStatelId1, "L3_"), BorderName, Name, true, CampName, BorderBGName);

        TweenScale cardTween = _card.gameObject.AddComponent<TweenScale>();
        cardTween.from = Vector3.zero; cardTween.to = Vector3.one;
        cardTween.duration = 0.07f;
        cardTween.AddOnFinished(CardTwwenOver);
        //TweenEventClear(CardTwwenOver);
        //CreateOnTheCardEffect(_CardDT.iImportant);

        GameObject eff = UITool.f_CreateEffect_Old(GetEffectName((EM_Important)_CardDT.iImportant), OneRole.transform, Vector3.zero, 0.15f, 60f, UIEffectName.UIEffectAddress1);
        eff.name = "GainEffect";
        if (_CardDT.iImportant >= (int)EM_Important.Oragen)
        {
            _OpenMallCardDisplayPage(_CardDT, false);
        }

        showOneGain = true;
    }

    void UpdateTreasureUI()
    {
        _gainTip.text = string.Format("{0}", _treasureDT.szName);
        //influence.text = "";
        _Treasure2DSpr.sprite2D = null;
        _Treasure2DSpr.transform.Find("IconBorder").GetComponent<UISprite>().spriteName = "";
        Invoke("CaeateTreasureModel", 1.1f);

        _gainTip.text = UITool.f_GetImporentForName(_treasureDT.iImportant, _treasureDT.szName);
        TipColor.gameObject.SetActive(false);
        _Treasure2DSpr.transform.Find("TipColor").GetComponent<UILabel>().text = UITool.f_GetImporentForName(_treasureDT.iImportant, _treasureDT.szName);
        //_Type.text = UITool.f_GetEquipPart(_treasureDT.iSite);
    }

    void UpdateGodEquipUI()
    {
        _gainTip.text = string.Format("{0}", _GodEquipDT.szName);
        //influence.spriteName = "";
        _GodEquipShow.sprite2D = null;
        _GodEquipShow.transform.Find("IconBorder").GetComponent<UISprite>().spriteName = "";
        Invoke("CaeateGodEquipModel", 1.1f);

        _gainTip.text = UITool.f_GetImporentForName(_GodEquipDT.iColour, _GodEquipDT.szName);
        //_Type.text = UITool.f_GetEquipPart(_EquipDT.iSite);
        TipColor.gameObject.SetActive(false);
        _GodEquipShow.transform.Find("TipColor").GetComponent<UILabel>().text = UITool.f_GetImporentForName(_GodEquipDT.iColour, _GodEquipDT.szName);

    }

    void CaeateTreasureModel()
    {
        CreateOnTheCardEffect(_treasureDT.iImportant);
        TweenScale TreasureTween = _Treasure2DSpr.gameObject.AddComponent<TweenScale>();
        TreasureTween.from = Vector3.zero; TreasureTween.to = Vector3.one;
        TreasureTween.duration = 0.2f;
        tEvent = new EventDelegate(EquipTreasureOver);
        TreasureTween.onFinished.Clear();
        TreasureTween.onFinished.Add(tEvent);
        UITool.f_SetIconSprite(_Treasure2DSpr, EM_ResourceType.Treasure, _treasureDT.iId);
        _Treasure2DSpr.transform.Find("IconBorder").GetComponent<UISprite>().spriteName = UITool.f_GetImporentCase(_treasureDT.iImportant);
        showOneGain = true;
    }

    void CaeateGodEquipModel()
    {
        CreateOnTheCardEffect(_GodEquipDT.iColour);
        TweenScale EquipTween = _GodEquipShow.gameObject.AddComponent<TweenScale>();
        EquipTween.from = Vector3.zero; EquipTween.to = Vector3.one;
        EquipTween.duration = 0.2f;
        tEvent = new EventDelegate(GodEquipTweenOver);
        EquipTween.onFinished.Clear();
        EquipTween.onFinished.Add(tEvent);
        UITool.f_SetIconSprite(_GodEquipShow, EM_ResourceType.GodEquip, _GodEquipDT.iId);
        _GodEquipShow.transform.Find("IconBorder").GetComponent<UISprite>().spriteName = UITool.f_GetImporentCase(_GodEquipDT.iColour);
        showOneGain = true;
    }

    void CreateOnTheCardEffect(int iImportant)
    {
        string EffectName = string.Empty;
        switch ((EM_Important)iImportant)
        {
            case EM_Important.White:
				EffectName = UIEffectName.kapai_kpsphc_bai_01;
				break;
            case EM_Important.Green:
				EffectName = UIEffectName.kapai_kpsphc_bai_01;
				break;
            case EM_Important.Blue:
                EffectName = UIEffectName.kapai_kpsphc_bai_01;
                break;
            case EM_Important.Purple:
                EffectName = UIEffectName.kapai_kpsphc_zi_01;
                break;
            case EM_Important.Oragen:
                EffectName = UIEffectName.kapai_kpsphc_cheng_01;
                break;
            case EM_Important.Red:
                EffectName = UIEffectName.kapai_kpsphc_hong_01;
                break;
            case EM_Important.Gold:
				EffectName = UIEffectName.kapai_kpsphc_hong_01;
                break;
        }
        OnParticle = UITool.f_CreateEffect_Old(EffectName, ParticleParent, Vector3.zero, 0.15f, 100f, UIEffectName.UIEffectAddress1);
        OnParticle.name = "GainEffect";
        Invoke("OnShowCardEffectOver", 0.35f);
    }
    void TweenEventClear(EventDelegate.Callback call)
    {
        tEvent = new EventDelegate(call);
        TweenManage.onFinished.Clear();
        TweenManage.onFinished.Add(tEvent);
        TweenManage.Play(true);
    }
    void OnShowCardEffectOver()
    {
        if (tequip.IsTenRecruit)
        {
            Invoke("CreateItem", 0.3f);
        }
        else
        {
            ShowEffectOver = true;
        }
        _btnBack.gameObject.SetActive(true);
    }
    void CardTwwenOver()
    {
        if (tequip.m_ListGetCard.Count >= 2)
        {
            f_GetObject("OneSongParent").SetActive(true);
            if (OneSongRole != null)
            {
                Destroy(OneSongRole);
            }
            CardDT _TempSongCardDT = glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(tequip.m_ListGetCard[1]) as CardDT;
            OneSongRole = Instantiate(f_GetObject("ItemCard"), f_GetObject("OneSongModel").transform);
            string Name = UITool.f_ReplaceName(_TempSongCardDT.szName, " ", "\n");
            string BorderName = UITool.f_GetImporentCase(_TempSongCardDT.iImportant);
            string CampName = UITool.f_GetCampName(_TempSongCardDT.iCardCamp);
            string BorderBGName = UITool.f_GetImporentCase(_TempSongCardDT.iImportant);
            OneSongRole.SetActive(true);
            OneSongRole.GetComponent<CardShowCardItemCtl>().SetData(UITool.f_GetGainCardIcon(_TempSongCardDT.iStatelId1, "L3_"), BorderName, Name, true, CampName, BorderBGName);

            GameObject eff = UITool.f_CreateEffect_Old(GetEffectName((EM_Important)_TempSongCardDT.iImportant), OneSongRole.transform, Vector3.zero, 0.15f, 60f, UIEffectName.UIEffectAddress1);
            eff.name = "GainEffect";
            //UITool.f_CreateRoleByCardId(tequip.m_ListGetCard[1], ref OneSongRole, f_GetObject("OneSongModel").transform, 101);
            //OneSongRole.transform.localScale = Vector3.one * 120;
        }
        Destroy(_card.gameObject.GetComponent<TweenScale>());
        //UpdateMainUI();
        f_GetObject("ObjShowLater").SetActive(true);
    }
    void EquipTweenOver()
    {
        Destroy(_EquipShow.gameObject.GetComponent<TweenScale>());
        //UpdateMainUI();
        f_GetObject("ObjShowLater").SetActive(true);
        ShowEffectOver = true;
        _btnBack.gameObject.SetActive(true);
    }
    void EquipTreasureOver()
    {
        Destroy(_Treasure2DSpr.gameObject.GetComponent<TweenScale>());
        //UpdateMainUI();
        f_GetObject("ObjShowLater").SetActive(true);
        ShowEffectOver = true;
        _btnBack.gameObject.SetActive(true);
    }

    void GodEquipTweenOver()
    {
        Destroy(_GodEquipShow.gameObject.GetComponent<TweenScale>());
        //UpdateMainUI();
        f_GetObject("ObjShowLater").SetActive(true);
        ShowEffectOver = true;
        _btnBack.gameObject.SetActive(true);
    }

    #region 十连抽 
    private GameObject TenLeft;
    private GameObject TenRight;
    private GameObject TenHead;
    List<GameObject> HeadList;
    private Transform TenNode;
    private GameObject[] RoleArr;

    private float CellWdith = 170;    // 行间距
    private float CellHeigt = 190;    //高间距
    private int MaxLineShow = 2;   //一行最多显示几个
    private int MaxTeamShow = 6;    //左右最多显示几个
    private int Count;    //计算用的
    private int Index;   //播放的第几个
    private int Time_CreateRole = -99;

    private Transform LetterNode;

    private void InitTenUI()
    {
        if (RoleArr == null)
        {
            RoleArr = new GameObject[10];
        }
        if (TenNode == null)
        {
            TenNode = f_GetObject("TenNode").transform;
        }
        RemoveLetter();
        Count = 0;
        _CreateRole(null);
        //Time_CreateRole = ccTimeEvent.GetInstance().f_RegEvent(0.3f, true, null, _CreateRole, true);



        return;
        if (TenLeft == null)
        {
            TenLeft = f_GetObject("TenLeft");
            TenRight = f_GetObject("TenRight");
            TenHead = f_GetObject("ResourceCommonItem");
        }

        Count = 0;
        Index = 0;
    }

    private void TenGain()
    {
        InitLetter();
        if (RoleArr == null)
        {
            RoleArr = new GameObject[11];
        }
        if (TenNode == null)
        {
            TenNode = f_GetObject("TenNode").transform;
        }
        Count = 0;
        Invoke("InitTenUI", 2.2f);
       
		//glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(AudioEffectType.TenRaffle); Tiếng X10
        //CreateItem();
    }

    private void InitLetter()
    {
        for (int i = 0; i < tequip.m_ListGetCard.Count; i++)
        {
            LetterNode.Find(i.ToString()).gameObject.SetActive(true);
            CardDT tCardDt = glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(tequip.m_ListGetCard[i]) as CardDT;
            
            OnParticle = UITool.f_CreateEffect_Old(GetEffectName((EM_Important)tCardDt.iImportant), LetterNode.Find(i.ToString()).gameObject.transform, Vector3.zero, 0.15f, 3f, UIEffectName.UIEffectAddress1);
            OnParticle.name = "GainEffect";
        }
		if(tequip.m_ListGetCard.Count == 0)
		{
			int y = 1;
			LetterNode.Find(y.ToString()).gameObject.SetActive(true);
            //CardDT tCardDt = glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(tequip.m_ListGetCard[y]) as CardDT;
            
            OnParticle = UITool.f_CreateEffect_Old(GetEffectName((EM_Important)1), LetterNode.Find(y.ToString()).gameObject.transform, Vector3.zero, 0.15f, 3f, UIEffectName.UIEffectAddress1);
            OnParticle.name = "GainEffect";
		}
        LetterNode.GetComponent<UIGrid>().Reposition();
    }

    private string GetEffectName(EM_Important eM_Important)
    {
        string EffectName = string.Empty;
        switch (eM_Important)
        {
            case EM_Important.White:
                EffectName = UIEffectName.kapai_kpsphc_bai_01;
                break;
            case EM_Important.Green:
                EffectName = UIEffectName.kapai_kpsphc_bai_01;
                break;
            case EM_Important.Blue:
                EffectName = UIEffectName.kapai_kpsphc_bai_01;
                break;
            case EM_Important.Purple:
                EffectName = UIEffectName.kapai_kpsphc_zi_01;
                break;
            case EM_Important.Oragen:
                EffectName = UIEffectName.kapai_kpsphc_cheng_01;
                break;
            case EM_Important.Red:
                EffectName = UIEffectName.kapai_kpsphc_hong_01;
                break;
            case EM_Important.Gold:
                EffectName = UIEffectName.kapai_kpsphc_hong_01;
                break;
        }
        return EffectName;
    }
    private void CreateItem()
    {
        if (Count >= tequip.m_ListGetCard.Count)
        {
            ShowEffectOver = true;
            return;
        }
        GameObject Headgo;
        if (HeadList.Count <= Count)
        {
            Headgo = GameObject.Instantiate(TenHead);
            HeadList.Add(Headgo);
        }
        else
            Headgo = HeadList[Count];


        Transform HeadTransform = Headgo.transform;
        ResourceCommonItem tResourceCommonItem = Headgo.GetComponent<ResourceCommonItem>();
        tResourceCommonItem.f_UpdateByInfo((int)EM_ResourceType.Card, tequip.m_ListGetCard[Count], 0);

        if (Count < MaxTeamShow)   //设置父级  
        {
            HeadTransform.SetParent(TenLeft.transform);
            Index = Count;
        }
        else
        {
            HeadTransform.SetParent(TenRight.transform);
            Index = Count - MaxTeamShow;
        }
        //设置坐标
        HeadTransform.localScale = Vector3.one;
        HeadTransform.localPosition = new Vector3((Index % MaxLineShow) * CellWdith, -(Mathf.FloorToInt(Index * 0.5f) * CellHeigt));


        _SetTweenAlpha(Headgo.GetComponent<TweenAlpha>());
        _SetTweenAlpha(tResourceCommonItem.mIcon.transform.GetComponent<TweenAlpha>());
        _SetTweenAlpha(tResourceCommonItem.mName.transform.GetComponent<TweenAlpha>());
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(AudioEffectType.TenRaffle);
        Headgo.GetComponent<TweenAlpha>().onFinished.Clear();

        Headgo.SetActive(true);

        string EffectCase = string.Empty;
        switch ((EM_Important)Headgo.GetComponent<ResourceCommonItem>().mData.mImportant)
        {
            case EM_Important.White:
                break;
            case EM_Important.Green:
                EffectCase = UIEffectName.GainCard_Green;
                break;
            case EM_Important.Blue:
                EffectCase = UIEffectName.GainCard_Blue;
                break;
            case EM_Important.Purple:
                EffectCase = UIEffectName.GainCard_Purple;
                break;
            case EM_Important.Oragen:
                EffectCase = UIEffectName.GainCard_Oragen;
                break;
            case EM_Important.Red:
				EffectCase = UIEffectName.GainCard_Oragen;
                break;
            case EM_Important.Gold:
				EffectCase = UIEffectName.GainCard_Oragen;
                break;
        }
        if (Headgo.transform.Find("GainEffect") != null)
        {
            Destroy(Headgo.transform.Find("GainEffect").gameObject);
        }
        GameObject Effect = UITool.f_CreateEffect(UIEffectName.UIEffectAddress3, EffectCase, Headgo.transform, 0, 0, false, 1.5f);
        Effect.name = "GainEffect";


        //if (Headgo.GetComponent<ResourceCommonItem>().mData.mImportant == (int)EM_Important.Purple)
        //{
        //    if (Headgo.transform.FindChild("GainEffect") == null)
        //    {

        //    }
        //    Headgo.transform.FindChild("GainEffect").gameObject.SetActive(true);
        //}
        //else
        //{
        //    if (Headgo.transform.FindChild("GainEffect") != null)
        //    {
        //        Headgo.transform.FindChild("GainEffect").gameObject.SetActive(false);
        //    }
        //}
        EM_Important tmpImportant = EM_Important.White;
        switch (Data_Pool.m_ShopLotteryPool.lotteryId)
        {
            case EM_RecruitType.NorAd:
                tmpImportant = EM_Important.Purple;
                break;
            case EM_RecruitType.GenAd:
                tmpImportant = EM_Important.Oragen;
                break;
            default:
                MessageBox.DEBUG(CommonTools.f_GetTransLanguage(1135));
                break;
        }
        if (Headgo.GetComponent<ResourceCommonItem>().mData.mImportant >= (int)tmpImportant || Count >= tequip.m_ListGetCard.Count - 1)
        {
            _CardDT = glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(tequip.m_ListGetCard[Count]) as CardDT;
            UpdateMainGui();
            UpdateCardUI();
            Count++;
            return;
        }

        EventDelegate tmpEventDelegate = new EventDelegate(CreateItem);
        Headgo.GetComponent<TweenAlpha>().onFinished.Add(tmpEventDelegate);
        Count++;
    }
    private void _SetTweenAlpha(TweenAlpha Tween)
    {
        Tween.ResetToBeginning();
        Tween.PlayForward();
    }
    
    private void _CreateRole(object obj)
    {
        //if (Count >= tequip.m_ListGetCard.Count - 1)
        //{
        //    ShowEffectOver = true;
        //    ccTimeEvent.GetInstance().f_UnRegEvent(Time_CreateRole);

        //    f_GetObject("SongParent").SetActive(true);
        //    GameObject SongCard = Instantiate(f_GetObject("ItemCard"), f_GetObject("SongModel").transform);
        //    CardDT _TempSongCardDT = glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(tequip.m_ListGetCard[10]) as CardDT;
        //    string SongName = UITool.f_ReplaceName(_TempSongCardDT.szName, " ", "\n");
        //    string SongBorderName = UITool.f_GetImporentCase(_TempSongCardDT.iImportant);
        //    string SongCampName = UITool.f_GetCampName(_TempSongCardDT.iCardCamp);
        //    string SongBorderBGName = UITool.f_GetImporentCase(_TempSongCardDT.iImportant);
        //    SongCard.SetActive(true);
        //    SongCard.GetComponent<CardShowCardItemCtl>().SetData(UITool.f_GetGainCardIcon(_TempSongCardDT.iStatelId1, "L3_"), SongBorderName, SongName, true, SongCampName, SongBorderBGName);
        //    GameObject songeff = UITool.f_CreateEffect_Old(GetEffectName((EM_Important)_TempSongCardDT.iImportant), SongCard.transform, Vector3.zero, 0.15f, 60f, UIEffectName.UIEffectAddress1);
        //    songeff.name = "GainEffect";
        //    RoleArr[10] = SongCard;
        //    //UITool.f_CreateRoleByCardId(tequip.m_ListGetCard[10], ref RoleArr[10], f_GetObject("SongModel").transform, 101);
        //    //RoleArr[10].transform.localScale = Vector3.one * 80;
        //    CardDT ttCardDt = glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(tequip.m_ListGetCard[Count]) as CardDT;
        //    if (ttCardDt != null)
        //        _OpenMallCardDisplayPage(ttCardDt, false);
        //    glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(AudioEffectType.TenRaffle2);
        //    return;
        //}

        if (Count >= tequip.m_ListGetCard.Count)
        {
            return;
        }
        Transform RoleParent = TenNode.GetChild(Count);
        RoleParent.gameObject.SetActive(true);
        CardDT _TempCardDT = glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(tequip.m_ListGetCard[Count]) as CardDT;
        GameObject OneCard = Instantiate(f_GetObject("ItemCard"), RoleParent);
        string Name = UITool.f_ReplaceName(_TempCardDT.szName, " ", "\n");
        string BorderName = UITool.f_GetImporentCase(_TempCardDT.iImportant);
        string CampName = UITool.f_GetCampName(_TempCardDT.iCardCamp);
        string BorderBGName = UITool.f_GetImporentCase(_TempCardDT.iImportant);
        OneCard.SetActive(true);
        OneCard.GetComponent<CardShowCardItemCtl>().SetData(UITool.f_GetGainCardIcon(_TempCardDT.iStatelId1, "L3_"), BorderName, Name, true, CampName, BorderBGName);
        GameObject eff = UITool.f_CreateEffect_Old(GetEffectName((EM_Important)_TempCardDT.iImportant), OneCard.transform, Vector3.zero, 0.15f, 60f, UIEffectName.UIEffectAddress1);
        eff.name = "GainEffect";
        OneCard.GetComponent<UIPanel>().sortingOrder = m_DepthIndex + Count;
        RoleArr[Count] = OneCard;
        UILabel NameLabel = TenNode.GetChild(Count).Find("Name").GetComponent<UILabel>();
        //glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(AudioEffectType.TenRaffle);
        //CardDT tCardDt = glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(tequip.m_ListGetCard[Count]) as CardDT;
        //UITool.f_CreateRoleByCardId(tequip.m_ListGetCard[Count], ref RoleArr[Count], RoleParent, 101);
        //RoleArr[Count].transform.localScale = Vector3.one * 80;
        //NameLabel.text = UITool.f_GetImporentForName(_TempCardDT.iImportant, _TempCardDT.szName);
        NameLabel.text = "";
        Count++;
        if (_TempCardDT.iImportant >= (int)EM_Important.Oragen)
        {
            _OpenMallCardDisplayPage(_TempCardDT);
            glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(AudioEffectType.TenRaffle2);
        }
        else
        {
            ccTimeEvent.GetInstance().f_UnRegEvent(Time_CreateRole);
            Time_CreateRole = -99;
            Time_CreateRole = ccTimeEvent.GetInstance().f_RegEvent(0.7f, true, null, _CreateRole);
        }
    }

    private void _OpenMallCardDisplayPage(CardDT tCard, bool End = true)
    {
        EquipSythesis tEquipSythesis = new EquipSythesis(tCard.iId, 1, EquipSythesis.ResonureType.Card);
        if (End)
        {
            tEquipSythesis.m_OnCloseCallback = _MallCardClose;
            tEquipSythesis.mCallbackParam = Count;
        }
        //ccTimeEvent.GetInstance().f_Pause();
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.MallCardDisplayPage, UIMessageDef.UI_OPEN, tEquipSythesis);
    }

    private void _MallCardClose(object obj)
    {
        Count = (int)obj;
        //ccTimeEvent.GetInstance().f_UnRegEvent(Time_CreateRole);
        _CreateRole(null);
        //ccTimeEvent.GetInstance().f_Resume();
    }

    #endregion


}

public class EquipSythesis
{
    public int id;
    public int num;
    public ResonureType m_Type;
    public ccCallback m_OnCloseCallback = null;//关闭页面时回调
    public object mCallbackParam = null;//回调参数

    public List<int> m_ListGetCard = new List<int>();
    public bool IsTenRecruit;
    public int TenRecruitIndex;  //十抽展示的索引
    public bool isShowBotton;
    public EquipSythesis(int id, int num, ResonureType t)
    {
        this.id = id;
        this.num = num;
        m_Type = t;
        IsTenRecruit = false;
        isShowBotton = true;

    }
    public EquipSythesis(List<int> listGetCard)
    {
        m_ListGetCard = listGetCard;
        IsTenRecruit = listGetCard.Count >= 10;
        if (!IsTenRecruit)
        {
            isShowBotton = true;
        }

        m_Type = ResonureType.Card;
        num = 1;
        TenRecruitIndex = 0;
    }
    public enum ResonureType
    {
        Equip,
        Card,
        Treasure,
        GodEquip
    }
}
