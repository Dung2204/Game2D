using UnityEngine;
using ccU3DEngine;
using System.Collections.Generic;
using System;
using System.Globalization;
using Spine;
using Spine.Unity;
using System.Linq;
//Lưu ý: BtnStore là Chat
/// <summary>
/// 主界面
/// </summary>
public class MainMenu : UIFramwork
{
[Header("Hidden Icon")]
    public GameObject[] OpenIcon;
    string HasOpenFunc;
    string NextOpenFuc;
    string OpenFuncSprite;
    string NextOpenFuncSprite;
    int nextOpenLevel;
    bool MainBgIsPlaying;
	int displayRole = 0;
    //My Code
    GameObject SpriNextLv2;
    GameObject LabelNextOpen2;
    GameObject LabelNextOpenTitle2;
    GameParamDT ActivityOpen;
    GameParamDT HotActivityOpen;
    GameParamDT AssetOpen;
	GameParamDT NewsOpen;
    GameObject TexBgKD;
	GameParamDT CardBattleOpen;
	int CardBattleOpenLvl = 30;
	int resetScroll = 0;
	int eventTimeCount = 0;
	int reddotEventTimeCount = 0;
    //



    #region 初始化
    /// <summary>
    /// Awake初始化
    /// </summary>
    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.MainBg, true, 0.2f);
        MainBgIsPlaying = true;
        //My Code
        ActivityOpen = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(90) as GameParamDT);
        HotActivityOpen = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(91) as GameParamDT);
        AssetOpen = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(93) as GameParamDT);
		NewsOpen = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(108) as GameParamDT);
        //
        if (StaticValue.m_isPlayMusic)
        {
            glo_Main.GetInstance().m_AdudioManager.f_UnPauseAudioMusic();
        }
        if (StaticValue.m_isPlaySound)
        {
            glo_Main.GetInstance().m_AdudioManager.f_UnPauseAudioButtle();
        }
        _ValentinesInfo();

        _ShowGameNotice(null);
        InitGUI();

        _AwardCenter();
        ClothArrayPage.CheckAndCommitClothArray();
        Data_Pool.m_GuidancePool.f_Enter();
        f_TipUnsettledPay(null);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_UnsettledPay_Tip, f_TipUnsettledPay, this);
        BroadcastCheckServerNotice(null);//检测广播消息
        //SetIcon();
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_ONLINE_VIP_AWARD, f_OnlineVipAward, this);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_EVENT_TIME, f_CloseEventTime, this);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_OPEN_EVENT_TIME, f_OPenEventTime, this);

    }
    /// <summary>
    /// 该UIOPen不会被调用（待删除）
    /// </summary>
    protected override void UI_OPEN(object e)
    {
        _NeedCloseSound = false;
        base.UI_OPEN(e);
    }

    #endregion
    #region 注册按钮和广播消息
    /// <summary>
    /// 定义自己需要处理的消息
    /// UI消息定义放在UIMessageDef中
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        GameSocket.GetInstance().f_Ping();
        Time.timeScale = 1;
        f_RegClickEvent("BtnBackpackClose", UI_BtnBackpackClose);
        f_RegClickEvent("BtnCardShow", UI_CardShow);
        f_RegClickEvent(f_GetObject("BtnChangeCard"), UI_ChangeCardShow);
        f_RegClickEvent("BtnRecommendTeam", UI_RecommendTeam);
        f_RegClickEvent("BtnRankList", UI_BtnRankList);
        f_RegClickEvent("Btn_Backpack", UI_Btn_Backpack);
        f_RegClickEvent("BtnSprHead", UI_UserInfo);
        f_RegClickEvent("BtnClothArray", UI_ClothArray);
        f_RegClickEvent("Btn_Destiny", UI_Destiny);
        // f_RegClickEvent("BtnShop", UI_Shop, ShopPage.EM_PageIndex.PropsContent);
        f_RegClickEvent("BtnShop", UI_Store);
        f_RegClickEvent("BtnStore", UI_OpenChat);
        f_RegClickEvent("BtnActivity", UI_Activity);
        f_RegClickEvent("BtnNewYear", UI_NewYear);
        f_RegClickEvent("BtnTimeDiscount", UI_TimeDiscount);
        f_RegClickEvent("BtnRecruitCard", UI_Recruit);
        f_RegClickEvent("BtnTask", UI_Task);
        f_RegClickEvent("BtnFriend", UI_Friend);
        f_RegClickEvent("BtnVip", UI_Vip, ShowVip.EM_PageIndex.Vip);
        f_RegClickEvent("BtnFirstRecharge", UI_FirstRecharge);
        f_RegClickEvent("BtnRecharge", UI_Vip, ShowVip.EM_PageIndex.Recharge);
        f_RegClickEvent("Btn_CardBag", UI_CardBag);
        f_RegClickEvent("Btn_EquipBag", UI_EquipBag);
        f_RegClickEvent("Btn_Dungeon", UI_DungeonBtn);
        f_RegClickEvent("Btn_TreasureBag", UI_TreasureBag);
        f_RegClickEvent("Btn_AwakeBag", UI_AwakeBag);
        f_RegClickEvent("Btn_GodEquipBag", UI_GodEquipBag);
        f_RegClickEvent("BtnAwardCenter", UI_OpenAwardCenter);
        f_RegClickEvent("Btn_GoodsBag", UI_GoodsBag);
        f_RegPressEvent("BtnSlide", UI_DragSlide);
        f_RegClickEvent("Btn_Recycling", UI_OpenRecycle);
        f_RegClickEvent("Btn_Legion", UI_Region);
        f_RegClickEvent("Btn_Challenge", UI_OpenChallengeMenuPage);
        f_RegClickEvent("Seven", UI_OpenSevenDay);
        f_RegClickEvent("BtnRankGift", UI_BtnRankGift);
        f_RegClickEvent("LimitGodBtn", UI_OpenLimitGod);
        f_RegClickEvent("TurntableBtn", UI_OpenTurntable);//通天转盘
		f_RegClickEvent("Btn_ChangeRole", f_ChangeDisplayRole);

        f_RegClickEvent("BtnMore", UI_More);
        f_RegClickEvent("BtnMoreContentClose", UI_CloseMoreContent);
        f_RegClickEvent("BtnAddMoney", UI_BtnAddMoney);
        f_RegClickEvent("BtnAddSycee", UI_BtnAddSycee);
        f_RegClickEvent("BtnAddEnergy", UI_BtnAddEnergy);

        f_RegClickEvent("ValentinesDay", _ValentinesDay);

        f_RegClickEvent("Btn_Tarining", UI_OpenTariningPage);
        f_RegClickEvent("SevenStar", UI_OpenSevenStarPage);

        //TsuCode - COin - KP	
        f_RegClickEvent("BtnAddCoin", UI_BtnAddCoin);

        //TsuCode - DashBoard
        f_RegClickEvent("BtnDashBoard", ShowDashBoard);
        //TsuCode - AFKModule
        f_RegClickEvent("BtnAFK", AFKClick);
        f_RegClickEvent("GroupFb", GroupFbClick);
		//
		f_RegClickEvent("Supermarket", UI_OpenSupermarketPage);
        f_RegClickEvent("Btn_TrialTower", f_OnBtn_TrialTowerClick);
        //
        f_RegClickEvent("Btn_Castle", f_OnBtnCastleClick);
		f_RegClickEvent("Btn_ClosePar2", f_OnBtnCastleClose);
		
        InitOrUpdateUIData(null);
        InitOrUpdateModelData(null);
        InitEffect();
        //        UITool.f_GetSysOpenLevel(EM_NeedLevel.GrabTreasureLevel)
        f_GetObject("BtnNewYear").SetActive(NewYearActPage.f_IsOpenNewYearAct());
        f_GetObject("BtnTimeDiscount").SetActive(Data_Pool.m_TimeDiscountPool.f_CheckOpenTimeLimitActTime());
        //My Code
        // if(ActivityOpen.iParam1 == 0)
        // {
        // f_GetObject("BtnActivity").SetActive(false);
        // f_GetObject("BtnRecruitCard").SetActive(false);
        // f_GetObject("BtnMore").SetActive(false);
        // f_GetObject("BtnRecommendTeam").SetActive(false);
        // f_GetObject("BtnRankList").SetActive(false);
        // f_GetObject("BtnStore").SetActive(false);
        // f_GetObject("Btn_Recycling").SetActive(false);
        // }
        if (HotActivityOpen.iParam1 == 0)
        {
            f_GetObject("BtnNewYear").SetActive(false);
        }
        // if(ActivityOpen.iParam2 == 0)
        // {
        // f_GetObject("BtnFirstRecharge").SetActive(false);
        // }
        if (ActivityOpen.iParam3 == 0)
        {
            f_GetObject("Seven").SetActive(false);
        }
        f_GetObject("TopRightGrid").GetComponent<UIGrid>().Reposition();
        //

        f_OpenTurntable();
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_UPDATE_USERINFOR, InitOrUpdateUIData, this);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_UPDATE_MODELINFOR, InitOrUpdateModelData, this);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_TASKMAINUPDATE, MainTaskInit, this);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_ServerBroadcast, BroadcastCheck, this);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_ServerNoticeBroadcast, BroadcastCheckServerNotice, this);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_UpdateSevenBtn, UI_UpdateSevenBtn, this);
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.TheNextDay, NextDay);
		

        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_OpenGameNotice, _ShowGameNotice, this);

        if (StaticValue.mIsNeedShowLevelPage)
        {
            StaticValue.mIsNeedShowLevelPage = false;
            ccTimeEvent.GetInstance().f_RegEvent(0.35f, false, null, ShowLevelUp);
        }
        MainTaskInit();
        f_LoadTexture();
        f_ShowLimitBtn();
        SetIcon();
		
		AFKClick(null, null, null);

        _setBtn();
        f_GetObject("TopRightGrid").GetComponent<UIGrid>().Reposition();

        InitEventTime();
        // xử lý show GP
        GameParamDT gameParamDTShowEvent = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.NotShow) as GameParamDT);
        bool _notShow = (gameParamDTShowEvent != null && gameParamDTShowEvent.iParam1 == 1) ? false : true;
        f_GetObject("TopRightGrid").SetActive(_notShow);
    }

    private string strTexBgRoot = "UI/TextureRemove/MainMenu/Tex_MainBgMetal";
	private string strTexFgRoot = "UI/TextureRemove/MainMenu/Tex_MainFgMetal";
	private string strTexMgRoot = "UI/TextureRemove/MainMenu/Tex_FxMetal_1";
	private string strTexFxRoot = "UI/TextureRemove/MainMenu/Tex_FxMetal_2";
	private string strTexBtn1Root = "UI/TextureRemove/MainMenu/Tex_MGBtnMetal_1";
	private string strTexBtn2Root = "UI/TextureRemove/MainMenu/Tex_MGBtnMetal_2";
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        //加载背景图
        UITexture TexBg = f_GetObject("TexBg").GetComponent<UITexture>();
		UITexture TexFg = f_GetObject("TexFG").GetComponent<UITexture>();
		UITexture TexMg = f_GetObject("TexMG").GetComponent<UITexture>();
		UITexture TexFx = f_GetObject("TexFx").GetComponent<UITexture>();
		UITexture TexBtn1 = f_GetObject("MGBtn1").GetComponent<UITexture>();
		UITexture TexBtn2 = f_GetObject("MGBtn2").GetComponent<UITexture>();
		GameObject charMagic = null;
		int fiveEle = RolePropertyTools.f_GetElementalSeason();
		switch (fiveEle)
		{
			case 1:
				strTexBgRoot = "UI/TextureRemove/MainMenu/Tex_MainBgMetal";
				strTexFgRoot = "UI/TextureRemove/MainMenu/Tex_MainFgMetal";
				strTexMgRoot = "UI/TextureRemove/MainMenu/Tex_FxMetal_1";
				strTexFxRoot = "UI/TextureRemove/MainMenu/Tex_FxMetal_2";
				strTexBtn1Root = "UI/TextureRemove/MainMenu/Tex_MGBtnMetal_1";
				strTexBtn2Root = "UI/TextureRemove/MainMenu/Tex_MGBtnMetal_2";
				break;
			case 2:
				strTexBgRoot = "UI/TextureRemove/MainMenu/Tex_MainBgNature";
				strTexFgRoot = "UI/TextureRemove/MainMenu/Tex_MainFgNature";
				strTexMgRoot = "UI/TextureRemove/MainMenu/Tex_FxNature_1";
				strTexFxRoot = "UI/TextureRemove/MainMenu/Tex_FxNature_2";
				strTexBtn1Root = "UI/TextureRemove/MainMenu/Tex_MGBtnNature_1";
				strTexBtn2Root = "UI/TextureRemove/MainMenu/Tex_MGBtnNature_2";
				break;
			case 3:
				strTexBgRoot = "UI/TextureRemove/MainMenu/Tex_MainBgWater";
				strTexFgRoot = "UI/TextureRemove/MainMenu/Tex_MainFgWater";
				strTexMgRoot = "UI/TextureRemove/MainMenu/Tex_FxWater_1";
				strTexFxRoot = "UI/TextureRemove/MainMenu/Tex_FxWater_2";
				strTexBtn1Root = "UI/TextureRemove/MainMenu/Tex_MGBtnWater_1";
				strTexBtn2Root = "UI/TextureRemove/MainMenu/Tex_MGBtnWater_2";
				break;
			case 4:
				strTexBgRoot = "UI/TextureRemove/MainMenu/Tex_MainBgFire";
				strTexFgRoot = "UI/TextureRemove/MainMenu/Tex_MainFgFire";
				strTexMgRoot = "UI/TextureRemove/MainMenu/Tex_FxFire_1";
				strTexFxRoot = "UI/TextureRemove/MainMenu/Tex_FxFire_2";
				strTexBtn1Root = "UI/TextureRemove/MainMenu/Tex_MGBtnFire_1";
				strTexBtn2Root = "UI/TextureRemove/MainMenu/Tex_MGBtnFire_2";
				break;
			case 5:
				strTexBgRoot = "UI/TextureRemove/MainMenu/Tex_MainBgEarth";
				strTexFgRoot = "UI/TextureRemove/MainMenu/Tex_MainFgEarth";
				strTexMgRoot = "UI/TextureRemove/MainMenu/Tex_FxEarth_1";
				strTexFxRoot = "UI/TextureRemove/MainMenu/Tex_FxEarth_2";
				strTexBtn1Root = "UI/TextureRemove/MainMenu/Tex_MGBtnEarth_1";
				strTexBtn2Root = "UI/TextureRemove/MainMenu/Tex_MGBtnEarth_2";
				break;
		}
		// UITool.f_CreateMagicById(20030, ref charMagic, f_GetObject("Ani").transform, 6, "animation", null, true, 1);
        //if (TexBg.mainTexture == null)
        {
            //My Code
            float windowAspect = (float)Screen.width / (float)Screen.height;
            //MessageBox.ASSERT("Ratio: " + windowAspect);
            Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot);
			
            // if (AssetOpen.iParam1 == 1)
            // {
                // tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("Tex_MainMenuBg");
                // tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot);
            // }
            // if (windowAspect >= 2.1)
            // {
                // f_GetObject("TexBg").transform.localPosition = new Vector3(-410f, 100f, 0f);
            // }
            // if (windowAspect <= 1.4)
            // {
                // f_GetObject("TexBg").transform.localPosition = new Vector3(-410f, 80f, 0f);
            // }
            //
            TexBg.mainTexture = tTexture2D;
			TexFg.mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexFgRoot);
			TexMg.mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexMgRoot);
			TexFx.mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexFxRoot);
			TexBtn1.mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBtn1Root);
			TexBtn2.mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBtn2Root);
        }

        //My Code
        if (AssetOpen.iParam1 == 1)
        {
            // f_GetObject("BtnNewYear").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSprite(301);
            // f_GetObject("BtnNewYear").GetComponent<UI2DSprite>().MakePixelPerfect();
            // f_GetObject("ValentinesDay").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSprite(302);
            // f_GetObject("ValentinesDay").GetComponent<UI2DSprite>().MakePixelPerfect();
            // f_GetObject("CardBattleBtn").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_CardBattle");
            // f_GetObject("LegionBattleBtn").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_LegionBattle");
            // f_GetObject("BtnAwardCenter").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_AwardCenter");
            // f_GetObject("BtnTimeDiscount").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_TimeDiscount");
            // f_GetObject("Seven").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_ActSevenDay");
            // f_GetObject("LimitGodBtn").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_LimitGod");
            // f_GetObject("TurntableBtn").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_Turntable");
            // f_GetObject("SevenStar").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_qxmd");
            // f_GetObject("BtnActivity").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_Activity");
            // f_GetObject("BtnFirstRecharge").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_FirstRecharge");
            // f_GetObject("BtnTask").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_Task");
            // f_GetObject("BtnRecharge").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_Recharge");
            // f_GetObject("BtnRankGift").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_RankGift");
            // f_GetObject("BtnClothArray").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_LineUp");
            // f_GetObject("Btn_CardBag").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("CardBagPage");
            // f_GetObject("Btn_Backpack").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_Packet");
            // f_GetObject("Btn_Destiny").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_BattleForm");
            // f_GetObject("Btn_Recycling").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_Recycling");
            // f_GetObject("Btn_Legion").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_Legion");
            // f_GetObject("Btn_Tarining").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_Tactical");
            // f_GetObject("BtnRecruitCard").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_Recruit");
            // f_GetObject("BtnStore").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_ChatTime");
            // f_GetObject("BtnMore").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_More");
            // f_GetObject("BtnRecommendTeam").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_RecommendTeam");
            // f_GetObject("BtnRankList").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_RankList");
            // f_GetObject("BtnShop").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_ShopStoreBtn");
            // f_GetObject("BtnCardShow").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_CardShow");
            // f_GetObject("BtnFriend").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_FrientTap");
            // f_GetObject("Btn_EquipBag").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_EquipBag");
            // f_GetObject("Btn_TreasureBag").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_TreasureBag");
            // f_GetObject("Btn_GoodsBag").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_GoodBag");
            // f_GetObject("Btn_AwakeBag").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_AwakeBag");
            // f_GetObject("Btn_Dungeon").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_Dungeon");
            // f_GetObject("Btn_Challenge").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_Challenge");
            // f_GetObject("BtnAFK").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_Afk");
            f_GetObject("GroupFb").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_GroupFb");
            f_GetObject("Supermarket").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_Supermarket");
            f_GetObject("Supermarket").GetComponent<UI2DSprite>().MakePixelPerfect();
            f_GetObject("Btn_TrialTower").GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_TrialTower");
            f_GetObject("Btn_TrialTower").GetComponent<UI2DSprite>().MakePixelPerfect();
        }
		if (NewsOpen.iParam1 == 0)
        {
            f_GetObject("GroupFb").SetActive(false);
			f_GetObject("SDKDashBoard").SetActive(false);
		}
		if (AssetOpen.iParam4 == 0)
        {
			f_GetObject("BtnRecruitCard").SetActive(false);
            f_GetObject("BtnCardShow").SetActive(false);
		}
        //
    }
	
    /// <summary>
    /// 显示升级界面
    /// </summary>
    private void ShowLevelUp(object data)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LevelUpPage, UIMessageDef.UI_OPEN, StaticValue.mLevelUpPageParam);
    }
    /// <summary>
    /// 初始化特效
    /// </summary>
    private void InitEffect()
    {
        return;
        //挑战和副本特效
        Transform effectParent = f_GetObject("Btn_Dungeon").transform.parent;
        UITool.f_CreateEffect_Old(UIEffectName.main_fuben_01, effectParent, f_GetObject("Btn_Dungeon").transform.localPosition, 0.2f, 0, UIEffectName.UIEffectAddress2);
        UITool.f_CreateEffect_Old(UIEffectName.main_tiaozhan_01, effectParent, f_GetObject("Btn_Challenge").transform.localPosition + new Vector3(-3, -30, 0),
            0.21f, 0, UIEffectName.UIEffectAddress2);
        //vip特效
        Transform effectParent2 = f_GetObject("BtnVip").transform.parent;
        UITool.f_CreateEffect_Old(UIEffectName.main_vip_01, effectParent2, f_GetObject("BtnVip").transform.localPosition + new Vector3(-1, -7, 0),
            0.2f, 0, UIEffectName.UIEffectAddress2);
        //战力特效
        Transform effectParent3 = f_GetObject("SprFightingBg").transform.parent;
        UITool.f_CreateEffect_Old(UIEffectName.main_zhandouli_01, effectParent3, f_GetObject("SprFightingBg").transform.localPosition + new Vector3(-32, 20, 0),
            0.2f, 0, UIEffectName.UIEffectAddress2);
        //活动特效
        Transform effectParent4 = f_GetObject("BtnActivity").transform.parent.Find("Particle");
        UITool.f_CreateEffect_Old(UIEffectName.main_iocn_01, effectParent4, Vector3.zero, 0.2f, 0, UIEffectName.UIEffectAddress2);
        Transform effectParent5 = f_GetObject("BtnFirstRecharge").transform.parent.Find("Particle");
        UITool.f_CreateEffect_Old(UIEffectName.main_iocn_01, effectParent5, Vector3.zero, 0.2f, 0, UIEffectName.UIEffectAddress2);
        Transform effectParent6 = f_GetObject("BtnRecharge").transform.parent.Find("Particle");
        UITool.f_CreateEffect_Old(UIEffectName.main_iocn_01, effectParent6, Vector3.zero, 0.2f, 0, UIEffectName.UIEffectAddress2);


        Transform effectParent7 = f_GetObject("ValentinesDay").transform.Find("Particle");
        UITool.f_CreateEffect_Old(UIEffectName.main_iocn_01, effectParent7, Vector3.zero, 0.2f, 0, UIEffectName.UIEffectAddress2);
        Transform effectParent8 = f_GetObject("BtnNewYear").transform.Find("Particle");
        UITool.f_CreateEffect_Old(UIEffectName.main_iocn_01, effectParent8, Vector3.zero, 0.2f, 0, UIEffectName.UIEffectAddress2);
    }
	
    /// <summary>
    /// 初始化或更新模型数据
    /// </summary>
    private void InitOrUpdateModelData(object data)
    {
        //if (CardReson != null)
        //    UITool.f_DestoryStatelObject(CardReson);
        GameObject ModelPoint = f_GetObject("ModelPoint");
		//My Code
		GameObject ModelPoint_New = f_GetObject("ModelPoint_New");
		// MessageBox.ASSERT("int: " + displayRole);
		CardPoolDT cardPoolDT2 = Data_Pool.m_TeamPool.f_GetFormationPosCardPoolDT((EM_FormationPos)displayRole);
		if(ModelPoint_New.transform.Find("Model") == null)
		{
			// GameObject CardReson2 = UITool.f_GetStatelObject(cardPoolDT2, ModelPoint_New.transform, Vector3.zero, Vector3.zero, 6);
		}
		else
		{
			// UITool.f_DestoryStatelObject(ModelPoint_New.transform.Find("Model").gameObject);
			// GameObject CardReson2 = UITool.f_GetStatelObject(cardPoolDT2, ModelPoint_New.transform, Vector3.zero, Vector3.zero, 6);
		}
		
        // ModelPoint.SetActive(true);
        for (int i = 0; i < 6; i++)
        {
            EM_FormationPos formationPos = (EM_FormationPos)i;
            CardPoolDT cardPoolDT = Data_Pool.m_TeamPool.f_GetFormationPosCardPoolDT(formationPos);
            Transform CardPoint = ModelPoint.transform.Find("Model" + i);
            if (CardPoint.Find("Model") != null)
            {
                UITool.f_DestoryStatelObject(CardPoint.Find("Model").gameObject);
            }
            CardPoint.gameObject.SetActive(cardPoolDT != null);

            CardPoint.Find("NameRoot").gameObject.SetActive(cardPoolDT != null);
            CardPoint.Find("TouchEnter").gameObject.SetActive(cardPoolDT == null);
            CardPoint.Find("Particle").gameObject.SetActive(cardPoolDT != null);
            int EM_NeedLevelVlaue = (int)EM_NeedLevel.OpenTwoCardLevel - 1;
            if (i > 0)
            {
                Transform obj = CardPoint.Find("TouchEnter/Level");
                if (obj != null)
                    obj.gameObject.SetActive(false);// (!UITool.f_GetIsOpensystem((EM_NeedLevel)(9 + i)));
                CardPoint.Find("TouchEnter/LabelTouchLineUp").gameObject.SetActive(UITool.f_GetIsOpensystem((EM_NeedLevel)(EM_NeedLevelVlaue + i)));
                if (!UITool.f_GetIsOpensystem((EM_NeedLevel)(EM_NeedLevelVlaue + i)))
                {
                    f_RegPressEvent(CardPoint.Find("TouchEnter").gameObject, UI_DragSlide);
                    string openLevelHint = string.Format(CommonTools.f_GetTransLanguage(1), UITool.f_GetSysOpenLevel((EM_NeedLevel)(EM_NeedLevelVlaue + i)));//UITool.f_GetSysOpenLevel((EM_NeedLevel)(EM_NeedLevelVlaue + i)) + "级开放";
                    f_RegClickEvent(CardPoint.Find("TouchEnter").gameObject, OnModelUnOpenClick, openLevelHint);
                    //CardPoint.FindChild("TouchEnter/Level").GetComponent<UILabel>().text = UITool.f_GetSysOpenLevel((EM_NeedLevel)(9 + i)) + "级开放";
                    continue;
                }
            }
            if (cardPoolDT == null && UITool.f_GetIsOpensystem((EM_NeedLevel)(EM_NeedLevelVlaue + i)))
            {
                f_RegPressEvent(CardPoint.Find("TouchEnter").gameObject, UI_DragSlide);
                f_RegClickEvent(CardPoint.Find("TouchEnter").gameObject, OnModelClick, formationPos);
                continue;
            }
            //创建模型特效
            Transform effectParentModel = ModelPoint.transform.Find("Model" + i).transform.Find("Particle");
            if (effectParentModel.childCount == 0)
            {
                //UITool.f_CreateEffect_Old(UIEffectName.main_shangzhenweizhi_01, effectParentModel, Vector3.zero, 0.2f, 0, UIEffectName.UIEffectAddress2);
                //UITool.f_CreateEffect_Old(UIEffectName.main_shangzhenweizhi_02, effectParentModel, Vector3.zero, 0.2f, 0, UIEffectName.UIEffectAddress2);
                UITool.f_CreateEffect_Old(UIEffectName.main_shanguang_01, effectParentModel, Vector3.zero, 0.2f, 0, UIEffectName.UIEffectAddress2);
            }
            //品质为金，红，橙色才显示外光圈(外光圈都去掉，口述需求)
            //bool isShowCircle = cardPoolDT.m_CardDT.iImportant == (int)EM_Important.Gold || cardPoolDT.m_CardDT.iImportant == (int)EM_Important.Oragen
            //    || cardPoolDT.m_CardDT.iImportant == (int)EM_Important.Red;
            //effectParentModel.transform.GetChild(0).gameObject.SetActive(false);
            //effectParentModel.transform.GetChild(1).gameObject.SetActive(false);
            //领悟等级大于等于2才有星星
            bool isShowStar = cardPoolDT.m_iLvAwaken >= 2;
            effectParentModel.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(isShowStar);
            effectParentModel.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(isShowStar);
            //等级大于等于5才有竖条特效
            bool isShowVerEffect = cardPoolDT.m_iLv >= 5;
            effectParentModel.transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(isShowVerEffect);

            CardPoint.gameObject.SetActive(true);
            // Model1
            GameObject CardReson = UITool.f_GetStatelObject(cardPoolDT, CardPoint, Vector3.zero, Vector3.zero, (6 - i) * 2);
            if (null == CardReson)
            {
                continue;
            }
            Transform tf = CardReson.transform.Find(GameParamConst.prefabShadowName);
            if (tf)
            {
                tf.gameObject.SetActive(false);
            }
            BoxCollider collider = CardReson.GetComponent<BoxCollider>();
            if (collider == null)
            {
                collider = CardReson.AddComponent<BoxCollider>();
            }
            collider.size = new Vector3(0.021f, 0.021f, 0.001f) * 150;
            collider.center = new Vector3(0, 1, 0);
            f_RegPressEvent(CardReson, UI_DragSlide);
            f_RegClickEvent(CardReson, OnModelClick, formationPos);
            string cardName = cardPoolDT.m_CardDT.szName;
            if (formationPos == EM_FormationPos.eFormationPos_Main)
                cardName = Data_Pool.m_UserData.m_szRoleName;
            char[] strArray = cardName.ToCharArray();
            string RoleName = "";
            for (int j = 0; j < strArray.Length; j++)
            {
                RoleName += strArray[j];
                if (j != strArray.Length - 1)
                    RoleName += "\n";
            }
            UITool.f_GetImporentColorName(cardPoolDT.m_CardDT.iImportant, ref RoleName);
            CardPoint.Find("NameRoot/LabelName").GetComponent<UILabel>().text = RoleName;

            //ParticleScaler[] particleScalerArray = CardPoint.transform.GetComponentsInChildren<ParticleScaler>();
            //for (int j = 0; j < particleScalerArray.Length; j++)
            //{
            //    particleScalerArray[j].particleScale = 0.2f * GetScaleByModelIndex(i);
            //}
        }
        ResetModelPos();
        //2.更新战斗力
        //f_GetObject("LabelBattleFeat").GetComponent<UILabel>().text = UITool.f_CountToChineseStr(Data_Pool.m_TeamPool.f_GetTotalBattlePower()); hiển thị 10 vạn thay vì 10000, nhưng cần ghi chữ vạn bằng tiếng Trung 万 sẽ có Function lấy hình ảnh (tiếng Việt) đắp vào, 亿 = trăm triệu.
        f_GetObject("LabelBattleFeat").GetComponent<UILabel>().text = Data_Pool.m_TeamPool.f_GetTotalBattlePower() + "";

        TeamPoolDT teamPoolDT = Data_Pool.m_TeamPool.f_GetForId((int)EM_FormationPos.eFormationPos_Main) as TeamPoolDT;
        string playerName = Data_Pool.m_UserData.m_szRoleName;
        f_GetObject("SpriteHead").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSpriteByCardId(teamPoolDT.m_CardPoolDT);
        f_GetObject("BorderHeadSprite").GetComponent<UISprite>().spriteName = UITool.f_GetImporentColorName(teamPoolDT.m_CardPoolDT.m_CardDT.iImportant, ref playerName);
        f_GetObject("LabelName").GetComponent<UILabel>().text = playerName;
    }
    /// <summary>
    /// 点击模型
    /// </summary>
    private void OnModelClick(GameObject go, object obj1, object obj2)
    {
        EM_FormationPos formationPos = (EM_FormationPos)obj1;
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LineUpPage, UIMessageDef.UI_OPEN, formationPos);
    }
    /// <summary>
    /// 点击模型未开放提示
    /// </summary>
    private void OnModelUnOpenClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, (string)obj1);
    }
    private void UI_OpenChat(GameObject go, object obj1, object obj2)
    {
        //ccUIHoldPool.GetInstance().f_Hold(this);
        OnContentReverseFinish();
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ChatManage, UIMessageDef.UI_OPEN);
    }
    /// <summary>
    /// 关闭模型展示
    /// </summary>
    private void CloseModelShow()
    {
        f_GetObject("ModelPoint").SetActive(false);
    }
    /// <summary>
    /// 初始化或更新UI数据
    /// </summary>
    public void InitOrUpdateUIData(object data)
    {
        //TsuCode
        //
        int vipExpCheck = (int)Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Vip);
        int lvCheck = (int)Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        bool checkOpen = true;
        if (ActivityOpen != null)
        {
            if (ActivityOpen.iParam2 == 1)
            {
                if (StaticValue.countOpenPopupFirstRecharge == 0)
                {
                    if (vipExpCheck < 10000 && Data_Pool.m_GuidancePool.IsEnter == false)
                    {
                        UI_FirstRechargeAutoOpen(null, 0, 0);
                        StaticValue.countOpenPopupFirstRecharge++;
                        checkOpen = false;
                    }
                }
            }
        }

        // 
        if (checkOpen && UITool.CheckInMainMenu())// ko co popup nap dau nen se check goi nap han gio
        {
            StaticValue.m_LocalLevelGift = LocalDataManager.f_GetLocalData<string>(LocalDataType.LevelGift, StaticValue.m_LoginName);
            Dictionary<int, bool> dict_data = UITool.ConverStringToDictionrary(StaticValue.m_LocalLevelGift == null ? "" : StaticValue.m_LocalLevelGift);
            if (Data_Pool.m_GuidancePool.IsEnter == false)
            {
                for (int i = 0; i < dict_data.Count; i++)
                {
                    var item = dict_data.ElementAt(i);
                    bool check = item.Value;
                    if (check)
                    {
                        StaticValue.OpenPopupLevelGiftId = item.Key;
                        List<NBaseSCDT> _EventTimeSCList = Data_Pool.m_EventTimePool.GetEventTimeSCList();

                        for (int j = 0; j < _EventTimeSCList.Count; j++)
                        {
                            //CreatMainICon((EventTimeDT)_EventTimeSCList[i]);
                            //f_OPenEventTime(_EventTimeSCList[i].iId);
                            EventTimeDT ttttttttttttt = _EventTimeSCList[j] as EventTimeDT;
                            if(ttttttttttttt.szNameConst == "LevelGiftPage")
                            {
                                //OnContentReverseFinish();
                                ccUIManage.GetInstance().f_SendMsg(UINameConst.LevelGiftPage, UIMessageDef.UI_OPEN, ttttttttttttt);
                                break;
                            }
                        }


                      
                    }
                }
            }

        }
        ////
        //
        //My Code
        SpriNextLv2 = GameObject.Find("SpriNextLv2");
        TexBgKD = GameObject.Find("TexBgKD");
        LabelNextOpen2 = GameObject.Find("LabelNextOpen2");
        LabelNextOpenTitle2 = GameObject.Find("LabelNextOpenTitle2");
        //UITool.Ui_Trip("State: " + SpriNextLv2);
        UITool.f_GetCurLevelOpenSystemDes(out HasOpenFunc, out NextOpenFuc, out nextOpenLevel, out OpenFuncSprite, out NextOpenFuncSprite);
        if (TexBgKD != null && AssetOpen != null)
        {
            if (AssetOpen.iParam2 == 0)
            {
                TexBgKD.SetActive(false);
            }
        }
        if (SpriNextLv2 != null)
            SpriNextLv2.SetActive(NextOpenFuc != null);
        if (nextOpenLevel == 300 && SpriNextLv2 != null)
        {
            SpriNextLv2.SetActive(false);
        }
		
		//My Code
		CardBattleOpen = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(96) as GameParamDT);
		CardBattleOpenLvl = CardBattleOpen.iParam3 * 1;
		//

        if (NextOpenFuc != null && LabelNextOpen2 != null && LabelNextOpenTitle2 != null && SpriNextLv2 != null && nextOpenLevel != 300)
        {
            LabelNextOpen2.GetComponent<UILabel>().text = NextOpenFuc ?? "";
LabelNextOpenTitle2.GetComponent<UILabel>().text = "Cấp " + nextOpenLevel;
            SpriNextLv2.GetComponent<UISprite>().spriteName = NextOpenFuncSprite;
            if (nextOpenLevel == 2)
            {
                SpriNextLv2.GetComponent<UISprite>().spriteName = "LineUpPage6";
LabelNextOpen2.GetComponent<UILabel>().text = "Open all champion tiles";
            }
            if (nextOpenLevel == UITool.f_GetSysOpenLevel(EM_NeedLevel.CrossServerBattle))
            {
LabelNextOpen2.GetComponent<UILabel>().text = "Open Peak";
            }
            if (nextOpenLevel == CardBattleOpenLvl )
            {
LabelNextOpenTitle2.GetComponent<UILabel>().text = "Cấp " + "55";
                SpriNextLv2.GetComponent<UISprite>().spriteName = "Icon_CardBattle";
LabelNextOpen2.GetComponent<UILabel>().text = "Open Martial Arts Club";
            }
        }
        //
        f_GetObject("LabelLevel").GetComponent<UILabel>().text = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level).ToString();
        f_GetObject("LabelVip").GetComponent<UILabel>().text = UITool.f_GetNowVipLv().ToString();
        f_GetObject("LabelEnergyCount").GetComponent<UILabel>().text = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Energy).ToString();
        int moneyCount = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Money);
        string moneyText = UITool.f_CountToChineseStr(moneyCount);
        int syceeCount = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee);
        string syceeText = UITool.f_CountToChineseStr2(syceeCount);
        //2.更新战斗力
        //f_GetObject("LabelBattleFeat").GetComponent<UILabel>().text = UITool.f_CountToChineseStr(Data_Pool.m_TeamPool.f_GetTotalBattlePower()); như trên
        f_GetObject("LabelBattleFeat").GetComponent<UILabel>().text = Data_Pool.m_TeamPool.f_GetTotalBattlePower() + "";
        f_GetObject("LabelMoneyCount").GetComponent<UILabel>().text = moneyText;
        f_GetObject("LabelSyceeCount").GetComponent<UILabel>().text = syceeText;
        //TsuCode - Coin- KimPhieu	
        int coinCount = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Coin);
        string coinText = UITool.f_CountToChineseStr2(coinCount);
        f_GetObject("LabelCoinCount").GetComponent<UILabel>().text = coinText;
        //---------------------------
        TeamPoolDT teamPoolDT = Data_Pool.m_TeamPool.f_GetForId((int)EM_FormationPos.eFormationPos_Main) as TeamPoolDT;
        string playerName = Data_Pool.m_UserData.m_szRoleName;
		MessageBox.ASSERT("Team Data:" +  teamPoolDT);
        f_GetObject("SpriteHead").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSpriteByCardId(teamPoolDT.m_CardPoolDT);
        f_GetObject("BorderHeadSprite").GetComponent<UISprite>().spriteName = UITool.f_GetImporentColorName(teamPoolDT.m_CardPoolDT.m_CardDT.iImportant, ref playerName);
        f_GetObject("LabelName").GetComponent<UILabel>().text = playerName;
        int Level = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        CarLvUpDT carLvUpDT = (CarLvUpDT)glo_Main.GetInstance().m_SC_Pool.m_CarLvUpSC.f_GetSC(Level + 1);
        int exMax = carLvUpDT == null ? 1 : carLvUpDT.iCardType;
        int ex = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Exp);
        float sliderValue = carLvUpDT == null ? 1 : (ex * 1.0f / exMax);
        sliderValue = sliderValue < 0 ? 0 : sliderValue;
        f_GetObject("LabelExValue").GetComponent<UILabel>().text = (int)(sliderValue * 100) + "%";
        f_GetObject("SliderEx").GetComponent<UISlider>().value = sliderValue;

        f_GetObject("BtnRankGift").SetActive(Data_Pool.m_RankGiftPool.f_CheckGiftIsOpen());

        Data_Pool.m_TrialTowerPool.CountEndTime();
        f_GetObject("SevenStar").SetActive(GameSocket.GetInstance().f_GetServerTime() < Data_Pool.m_TrialTowerPool.mNowEndTime);



        f_GetObject("TopRightGrid").GetComponent<UIGrid>().Reposition();
    }
    /// <summary>
    /// 点击元宝+号
    /// </summary>
    private void UI_BtnAddSycee(GameObject go, object obj1, object obj2)
    {
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShowVip, UIMessageDef.UI_OPEN, ShowVip.EM_PageIndex.Recharge);
    }
    /// <summary>
    /// 点击银币+号
    /// </summary>

    //TsuCode - Coin - Kim Phieu	
    private void UI_BtnAddCoin(GameObject go, object obj1, object obj2)
    {
        //ccUIHoldPool.GetInstance().f_Hold(this);	
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.ShowVip, UIMessageDef.UI_OPEN, ShowVip.EM_PageIndex.Recharge);	
        //UITool.Ui_Trip("Đây là btn nạp kim phiếu");
        //string tCPOrderId = glo_Main.GetInstance().m_SDKCmponent.f_CreateCPOrderId();
        //glo_Main.GetInstance().m_SDKCmponent.f_Pccaccy(0, "", 0, 0, tCPOrderId, "", 0, "", "");

        //glo_Main.GetInstance().m_SDKCmponent.f_ShowSDKPay();

        //SocketCallbackDT whitelistCallbackDt = new SocketCallbackDT();
        //whitelistCallbackDt.m_ccCallbackSuc = f_OnBuyCoinSDK;
        //whitelistCallbackDt.m_ccCallbackFail = f_OnBuyCoinSDK;
        //Data_Pool.m_RechargePool.BuyCoinSDK(whitelistCallbackDt);

        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShowVip, UIMessageDef.UI_OPEN, ShowVip.EM_PageIndex.RechargeCoin);
    }

    //TsuCode - DashBoard
    private void ShowDashBoard(GameObject go, object obj1, object obj2)
    {
        //UITool.Ui_Trip("ShowDashBoard");
        glo_Main.GetInstance().m_SDKCmponent.f_ShowDashBoard();
    }
    //TsuCode - AFK module
    private void AFKClick(GameObject go, object obj1, object obj2)
    {
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.AFKInfoPage, UIMessageDef.UI_OPEN);
        SocketCallbackDT QueryCallback = new SocketCallbackDT();//请求信息回调
        QueryCallback.m_ccCallbackFail = OnAFKTimeFailCallback;
        QueryCallback.m_ccCallbackSuc = OnAFKTimeSucCallback;
        Data_Pool.m_AFKPool.f_QueryAFKTimeInfo(QueryCallback);
    }
    //-------------------------

    
    private void GroupFbClick(GameObject go, object obj1, object obj2)
    {
       
        string url = CommonTools.f_GetTransLanguage(2278);
        string url2 = CommonTools.f_GetTransLanguage(2282);

        glo_Main.GetInstance().m_SDKCmponent.f_OpenFacebook(url, url2); //TsuCode - openfacebook
        //if (Application.platform == RuntimePlatform.IPhonePlayer)
        //{
        //    Application.OpenURL(url);
        //}
        //else
        //{
        //    Application.OpenURL(url);
        //}
    }
    //-------------------------
    private void UI_BtnAddMoney(GameObject go, object obj1, object obj2)
    {
        UITool.f_IsEnoughMoney(EM_MoneyType.eUserAttr_Money, 0, true, true, this);
    }
    /// <summary>
    /// 点击体力+号
    /// </summary>SevenStar
    private void UI_BtnAddEnergy(GameObject go, object obj1, object obj2)
    {
        UITool.f_IsEnoughMoney(EM_MoneyType.eUserAttr_Energy, 0, true, true, this);
    }
    /// <summary>
    /// 点击图鉴打开图鉴界面
    /// </summary>
    private void UI_CardShow(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        OnContentReverseFinish();
        //ccUIHoldPool.GetInstance().f_Hold(this);
        MainBgIsPlaying = false;
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopPage, UIMessageDef.UI_OPEN, 5);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CardShowPage, UIMessageDef.UI_OPEN, this);
    }

    //点击移魂阵打开移魂阵界面
    private void UI_ChangeCardShow(GameObject go, object obj1, object obj2)
    {
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < UITool.f_GetSysOpenLevel(EM_NeedLevel.TransmigrationCard))
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, string.Format(CommonTools.f_GetTransLanguage(2), UITool.f_GetSysOpenLevel(EM_NeedLevel.TransmigrationCard)));
            return;
        }
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        ccUIHoldPool.GetInstance().f_Hold(this);
        OnContentReverseFinish();
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CardChangePage, UIMessageDef.UI_OPEN);
    }

    /// <summary>
    /// 点击打开推荐阵容
    /// </summary>
    private void UI_RecommendTeam(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        OnContentReverseFinish();
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RecommendTeamPage, UIMessageDef.UI_OPEN);
    }
    /// <summary>
    /// 点击打开排行榜
    /// </summary>
    private void UI_BtnRankList(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        OnContentReverseFinish();
        ccUIHoldPool.GetInstance().f_Hold(this);
        MainBgIsPlaying = false;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RankListPage, UIMessageDef.UI_OPEN);
    }
    /// <summary>
    /// 点击活动
    /// </summary>
    private void UI_Activity(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ActivityPage, UIMessageDef.UI_OPEN);
        OnContentReverseFinish();
    }
    /// <summary>
    /// 点击首充按钮
    /// </summary>
    private void UI_FirstRecharge(GameObject go, object obj1, object obj2)
    {
        OnContentReverseFinish();
        if (!Data_Pool.m_FirstRechargePool.f_CheckIsOpen())
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(3));
            return;
        }
        //TsuComment 	
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.FirstRechargePage, UIMessageDef.UI_OPEN);	
        ccUIManage.GetInstance().f_SendMsg(UINameConst.FirstRechargePageNew, UIMessageDef.UI_OPEN); //TsuCode - FirstRechargeNew - NapDau

        //ccUIHoldPool.GetInstance().f_Hold(this);
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.ActivityPage, UIMessageDef.UI_OPEN, EM_ActivityType.FirstRecharge);
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.FirstRechargePage, UIMessageDef.UI_OPEN);
    }

    //TsuCode
    public void UI_FirstRechargeAutoOpen(GameObject go, object obj1, object obj2)
    {
        OnContentReverseFinish();
        if (!Data_Pool.m_FirstRechargePool.f_CheckIsOpen())
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(3));
            return;
        }
        //TsuComment 	
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.FirstRechargePage, UIMessageDef.UI_OPEN);	
        ccUIManage.GetInstance().f_SendMsg(UINameConst.FirstRechargePageNew, UIMessageDef.UI_OPEN); //TsuCode- FirstRechargeNew - NapDau

        //ccUIHoldPool.GetInstance().f_Hold(this);
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.ActivityPage, UIMessageDef.UI_OPEN, EM_ActivityType.FirstRecharge);
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.FirstRechargePage, UIMessageDef.UI_OPEN);
    }
    //

    /// <summary>
    /// 点击新春活动
    /// </summary>
    private void UI_NewYear(GameObject go, object obj1, object obj2)
    {
        GameParamDT gameParamDTOpenLevel = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.NewYearOpenLevel) as GameParamDT);
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < gameParamDTOpenLevel.iParam1)
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(4), gameParamDTOpenLevel.iParam1));
            return;
        }

        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.NewYearActPage, UIMessageDef.UI_OPEN);
        OnContentReverseFinish();
    }
    /// <summary>
    /// 点击限时折扣
    /// </summary>
    private void UI_TimeDiscount(GameObject go, object obj1, object obj2)
    {
        OnContentReverseFinish();
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TimeDiscountPage, UIMessageDef.UI_OPEN);
    }
    /// <summary>
    /// 点击vip
    /// </summary>
    private void UI_Vip(GameObject go, object obj1, object obj2)
    {
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShowVip, UIMessageDef.UI_OPEN, obj1);
        OnContentReverseFinish();
    }
    /// <summary>
    /// 点击任务
    /// </summary>
    private void UI_Task(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        int curLv = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        if (curLv < GameParamConst.TaskLvLimit)
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(5), GameParamConst.TaskLvLimit));
            return;
        }
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TaskPage, UIMessageDef.UI_OPEN);
        OnContentReverseFinish();
    }

    /// <summary>
    /// 点击招募，也是打开商店
    /// </summary>
    private void UI_Recruit(GameObject go, object obj1, object obj2)
    {
        UI_Shop(null, null, null);
    }
    private bool isDisContent = false;//是否已经展开more/bag按钮
    private void UI_Friend(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        OnContentReverseFinish();
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.FriendPage, UIMessageDef.UI_OPEN);
    }

    private bool isShowing = false;
    /// <summary>
    /// 点击更多
    /// </summary>
    private void UI_More(GameObject go, object obj1, object obj2)
    {
        if (!isDisContent && !isShowing)
        {
            glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
            isShowing = true;
            f_GetObject("MoreContent").SetActive(true);
            Transform ContentRoot = f_GetObject("MoreContent").transform.Find("ContentRoot");
            TweenScale ts = ContentRoot.GetComponent<TweenScale>();
            ContentRoot.localScale = ts.from;
            ts.SetOnFinished(OnContentShowFinish);
            ts.PlayForward();
            ts.ResetToBeginning();
            ts.PlayForward();
        }
    }
    /// <summary>
    /// 展开内容之后
    /// </summary>
    private void OnContentShowFinish()
    {
        isDisContent = true;
        isShowing = false;
    }
    /// <summary>
    /// 更多按钮关闭事件
    /// </summary>
    private void UI_CloseMoreContent(GameObject go, object obj1, object obj2)
    {
        if (isDisContent && !isShowing)
        {
            isShowing = true;
            Transform ContentRoot = f_GetObject("MoreContent").transform.Find("ContentRoot");
            TweenScale ts = ContentRoot.GetComponent<TweenScale>();
            ContentRoot.localScale = ts.to;
            ts.SetOnFinished(OnContentReverseFinish);
            ts.PlayReverse();
            ts.ResetToBeginning();
            ts.PlayReverse();
        }
    }
    /// <summary>
    /// 更新按钮内容关闭完成
    /// </summary>
    private void OnContentReverseFinish()
    {
        f_GetObject("MoreContent").SetActive(false);
        f_GetObject("ObjBackpack").SetActive(false);
        isDisContent = false;
        isShowing = false;
    }
    /// <summary>
    /// 点击商店
    /// </summary>
    private void UI_Store(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        //ccUIHoldPool.GetInstance().f_Hold(this);
        OnContentReverseFinish();
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopStorePage, UIMessageDef.UI_OPEN);
    }
    /// <summary>
    /// 点击商城
    /// </summary>
    private void UI_Shop(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        OnContentReverseFinish();
        ccUIHoldPool.GetInstance().f_Hold(this);
        MainBgIsPlaying = false;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopPage, UIMessageDef.UI_OPEN, obj1);

    }
    /// <summary>
    /// 点击阵荣
    /// </summary>
    private void UI_ClothArray(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LineUpPage, UIMessageDef.UI_OPEN);
    }
    /// <summary>
    /// 点击天命/阵图
    /// </summary>
    private void UI_Destiny(GameObject go, object obj1, object obj2)
    {
        if (!UITool.f_GetIsOpensystem(EM_NeedLevel.BattleFormLevel))
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(6), UITool.f_GetSysOpenLevel(EM_NeedLevel.BattleFormLevel)));
            return;
        }
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.BattleFormPage, UIMessageDef.UI_OPEN);
    }
    /// <summary>
    /// 点击用户头像，关闭角色
    /// </summary>
    private void UI_UserInfo(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.UserInfoPage, UIMessageDef.UI_OPEN);
    }
    /// <summary>
    /// 背包背景按钮
    /// </summary>
    private void UI_BtnBackpackClose(GameObject go, object obj1, object obj2)
    {
        if (isDisContent && !isShowing)
        {
            isShowing = true;
            Transform ContentRoot = f_GetObject("ObjBackpack").transform.Find("ContentRoot");
            TweenScale ts = ContentRoot.GetComponent<TweenScale>();
            ContentRoot.localScale = ts.to;
            ts.SetOnFinished(OnContentReverseFinish);
            ts.PlayReverse();
            ts.ResetToBeginning();
            ts.PlayReverse();
        }
    }
    /// <summary>
    /// 点击背包按钮
    /// </summary>
    private void UI_Btn_Backpack(GameObject go, object obj1, object obj2)
    {
        bool isAct = f_GetObject("ObjBackpack").activeInHierarchy;
        f_GetObject("ObjBackpack").SetActive(!isAct);
        if (!isDisContent && !isShowing)
        {
            glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
            isShowing = true;
            f_GetObject("ObjBackpack").SetActive(true);
            Transform ContentRoot = f_GetObject("ObjBackpack").transform.Find("ContentRoot");
            TweenScale ts = ContentRoot.GetComponent<TweenScale>();
            ContentRoot.localScale = ts.from;
            ts.SetOnFinished(OnContentShowFinish);
            ts.PlayForward();
            ts.ResetToBeginning();
            ts.PlayForward();
        }
    }

    private void UI_CardBag(GameObject go, object obj1, object obj2)
    {
        //通知HoldPool保存当前页
        ccUIHoldPool.GetInstance().f_Hold(this);
        //通知ccUIManager打开背包页
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CardBagPage, UIMessageDef.UI_OPEN);
    }

    private void UI_EquipBag(GameObject go, object obj1, object obj2)
    {
        OnContentReverseFinish();
        //通知HoldPool保存当前页
        ccUIHoldPool.GetInstance().f_Hold(this);
        //通知ccUIManager打开背包页
        ccUIManage.GetInstance().f_SendMsg(UINameConst.EquipBagPage, UIMessageDef.UI_OPEN);
    }

    private void UI_TreasureBag(GameObject go, object obj1, object obj2)
    {
        if (!UITool.f_GetIsOpensystem(EM_NeedLevel.OpenTreasureLevel))
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(7), UITool.f_GetSysOpenLevel(EM_NeedLevel.OpenTreasureLevel)));
            return;
        }
        OnContentReverseFinish();
        //通知HoldPool保存当前页
        ccUIHoldPool.GetInstance().f_Hold(this);
        //通知ccUIManager打开背包页
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TreasureBagPage, UIMessageDef.UI_OPEN);
    }
    /// <summary>
    /// 点击领悟背包
    /// </summary>
    private void UI_AwakeBag(GameObject go, object obj1, object obj2)
    {
        OnContentReverseFinish();
        //通知HoldPool保存当前页
        ccUIHoldPool.GetInstance().f_Hold(this);
        ////通知ccUIManager打开背包页
        ccUIManage.GetInstance().f_SendMsg(UINameConst.AwakenEquipBagPage, UIMessageDef.UI_OPEN);
    }

    private void UI_GodEquipBag(GameObject go, object obj1, object obj2)
    {
        GameParamDT param = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(117);
        //param.iParam1 = 25;//test
        if (CommonTools.f_CheckOpenServerDay(param.iParam1))
        {
            OnContentReverseFinish();
            //通知HoldPool保存当前页
            ccUIHoldPool.GetInstance().f_Hold(this);
            //通知ccUIManager打开背包页
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GodEquipBagPage, UIMessageDef.UI_OPEN);
        }
        else
        {

            string message = string.Format(CommonTools.f_GetTransLanguage(2300), param.iParam1);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, message);
        }
    }

    private void UI_GoodsBag(GameObject go, object obj1, object obj2)
    {
        OnContentReverseFinish();
        ccUIHoldPool.GetInstance().f_Hold(this);
        //通知ccUIManager打开背包页
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GoodsBagPage, UIMessageDef.UI_OPEN);
    }
    /// <summary>
    /// 打开副本界面
    /// </summary>
    private void UI_DungeonBtn(GameObject go, object obj1, object obj2)
    {
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.DungeonChapterPageNew, UIMessageDef.UI_OPEN);
    }

    void UI_OpenAwardCenter(GameObject go, object obj1, object obj2)
    {
        //ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.Award, UIMessageDef.UI_OPEN);
    }
    /// <summary>
    /// 打开回收界面
    /// </summary>
    void UI_OpenRecycle(GameObject go, object obj1, object obj2)
    {
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.Recycle, UIMessageDef.UI_OPEN);
    }
    void UI_OpenSevenDay(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SevenDayActivityPage, UIMessageDef.UI_OPEN);
        OnContentReverseFinish();

    }
    /// <summary>
    /// 点击限时神装
    /// </summary>
    private void UI_OpenLimitGod(GameObject go, object obj1, object obj2)
    {
        SocketCallbackDT QueryCallback = new SocketCallbackDT();//请求信息回调
        QueryCallback.m_ccCallbackFail = OnGodDressFailCallback;
        QueryCallback.m_ccCallbackSuc = OnGodDressSucCallback;
        Data_Pool.m_GodDressPool.f_QueryGodDressInfo(QueryCallback);
    }
    /// 查询失败回调
    private void OnGodDressFailCallback(object obj)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(15) + CommonTools.f_GetTransLanguage((int)obj));
    }

    //查询成功回调
    private void OnGodDressSucCallback(object obj)
    {
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LimitGodEquipActPage, UIMessageDef.UI_OPEN);
    }

    /// <summary>
    /// 点击轮盘转
    /// </summary>
    private void UI_OpenTurntable(GameObject go, object obj1, object obj2)
    {
        GameParamDT _turntableParam = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.TurntableLottery);
        if (_turntableParam.iParam4 != 0)
        {
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT QueryCallback = new SocketCallbackDT();//请求信息回调
        QueryCallback.m_ccCallbackFail = OnTurntableFailCallback;
        QueryCallback.m_ccCallbackSuc = OnTurntableSucCallback;
        Data_Pool.m_TurntablePool.f_QueryTurntableInfo(QueryCallback);
    }

    //通天转盘活动开启设置
    private void f_OpenTurntable()
    {
        GameParamDT _turntableParam = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.TurntableLottery);
        GameParamDT _turntableLvParam = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.TurntableLimitLv);
        if (Data_Pool.m_TurntablePool.f_GetIsOpen() && (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) >= _turntableLvParam.iParam1))
        {
            f_GetObject("TurntableBtn").SetActive(true);
        }
        else
            f_GetObject("TurntableBtn").SetActive(false);
        f_GetObject("TopRightGrid").GetComponent<UIGrid>().Reposition();
    }

    /// 查询失败回调
    private void OnTurntableFailCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(15) + CommonTools.f_GetTransLanguage((int)obj));
    }

    //查询成功回调
    private void OnTurntableSucCallback(object obj)
    {
        //ccUIHoldPool.GetInstance().f_Hold(this);
        UITool.f_OpenOrCloseWaitTip(false);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TurntablePage, UIMessageDef.UI_OPEN);
        OnContentReverseFinish();
    }


    /// <summary>
    /// 点击等级礼包
    /// </summary>
    void UI_BtnRankGift(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RankGiftPage, UIMessageDef.UI_OPEN);
        OnContentReverseFinish();
    }
    /// <summary>
    /// 打开挑战菜单界面
    /// </summary>
    void UI_OpenChallengeMenuPage(GameObject go, object value1, object value2)
    {
        ccUIHoldPool.GetInstance().f_Hold(this);
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.UI_Challenge);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ChallengeMenuPage, UIMessageDef.UI_OPEN);
    }

	private void f_ChangeDisplayRole(GameObject go, object value1, object value2)
	{
		if(displayRole < 5)
		{
			displayRole++;
			glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_UPDATE_MODELINFOR);
		}
		else
		{
			displayRole = 0;
			glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_UPDATE_MODELINFOR);
		}
	}

    void UI_OpenTariningPage(GameObject go, object value1, object value2)
    {
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TariningPage, UIMessageDef.UI_OPEN);
    }

    private void UI_OpenSevenStarPage(GameObject go, object value1, object value2)
    {
        if (!UITool.f_GetIsOpensystem(EM_NeedLevel.SevenStarOpenLv))
        {
UITool.Ui_Trip("Cấp không đủ");
            return;
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SevenStarPage, UIMessageDef.UI_OPEN);
    }
    private void f_OnBtn_TrialTowerClick(GameObject go, object value1, object value2)
    {
        if (!UITool.f_GetIsOpensystem(EM_NeedLevel.TritalTowerOpenLV))
        {
            UITool.Ui_Trip("Cấp không đủ");
            return;
        }

        if (!(Data_Pool.m_TrialTowerPool.isOpen))
        {
            UITool.Ui_Trip("Chưa mở");
            return;
        }
        if (Data_Pool.m_TrialTowerPool.mNowEndTime - GameSocket.GetInstance().f_GetServerTime() <= 0)
        {
            UITool.Ui_Trip("Chưa mở");
            return;
        }

        if (!Data_Pool.m_TrialTowerPool.isEnter)
        {
            ccUIHoldPool.GetInstance().f_Hold(this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.TrialTowerRoomPage, UIMessageDef.UI_OPEN);
        }
        else
        {
            ccUIHoldPool.GetInstance().f_Hold(this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.TrialTowerPage, UIMessageDef.UI_OPEN);
        }
    }
    private void UI_OpenSupermarketPage(GameObject go, object value1, object value2)
    {
        //if (!UITool.f_GetIsOpensystem(EM_NeedLevel.SevenStarOpenLv))
        //{
        //    UITool.Ui_Trip("Not enough level");
        //    return;
        //}
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SupermarketPage, UIMessageDef.UI_OPEN);
    }
    #endregion
    #region 军团相关处理
    /// <summary>
    /// 打开军团界面
    /// </summary>
    void UI_Region(GameObject go, object obj1, object obj2)
    {
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < UITool.f_GetSysOpenLevel(EM_NeedLevel.LegionLevel))
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(16), UITool.f_GetSysOpenLevel(EM_NeedLevel.LegionLevel)));
            return;
        }
        if (LegionMain.GetInstance().m_LegionInfor.m_iLegionId == 0)
        {
            //还没有军团
            UITool.f_OpenOrCloseWaitTip(true);
            LegionMain.GetInstance().m_LegionInfor.f_ExecuteAfterLegionInfo(false, true, f_Callback_LegionListInfo);
        }
        else
        {
            MainBgIsPlaying = false;
            UITool.f_OpenOrCloseWaitTip(true);
            LegionMain.GetInstance().m_LegionInfor.f_ExecuteAfterLegionInfo(true, true, f_Callback_LegionSelfInfo);
        }
    }

    private void f_Callback_LegionListInfo(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            //打开军团列表界面
            ccUIHoldPool.GetInstance().f_Hold(this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionListPage, UIMessageDef.UI_OPEN);
        }
        else
        {
            UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(17) + result);
        }
    }

    private void f_Callback_LegionSelfInfo(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            ccUIHoldPool.GetInstance().f_Hold(this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionMenuPage, UIMessageDef.UI_OPEN);
        }
        else
        {
            UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(18) + result);
        }
    }

    private void f_Callback_LegionSelfInfo_InitFiniChaper(object result)
    {
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            //军团副本数据初始化，驱动红点
            SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
            socketCallbackDt.m_ccCallbackSuc = f_Callback_InitFiniChaper;
            socketCallbackDt.m_ccCallbackFail = f_Callback_InitFiniChaper;
            LegionMain.GetInstance().m_LegionDungeonPool.f_InitFiniChapter(socketCallbackDt);
        }
        else
        {
            UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(18) + result);
        }
    }

    #endregion
    #region 红点提示
    protected override void InitRaddot()
    {
        base.InitRaddot();

        Data_Pool.m_RunningManPool.f_CheckEliTimesLeft();

        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.TaskPage_Daily, f_GetObject("BtnTask"), ReddotCallback_Show_BtnTask, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.TaskPage_Achievement, f_GetObject("BtnTask"), ReddotCallback_Show_BtnTask, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.EquipBag_Fragment, f_GetObject("Btn_Backpack"), ReddotCallback_Show_Btn_Backpack, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.EquipBag_Fragment, f_GetObject("Btn_EquipBag"), ReddotCallback_Show_Btn_EquipBag, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.CardBag_Fragment, f_GetObject("Btn_CardBag"), ReddotCallback_Show_Btn_CardBag, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.GodEquipBag_Fragment, f_GetObject("Btn_Backpack"), ReddotCallback_Show_Btn_Backpack, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.GodEquipBag_Fragment, f_GetObject("Btn_GodEquipBag"), ReddotCallback_Show_Btn_GodEquipBag, true);
        BagNewGoods();

        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.BattleForm_CanAct, f_GetObject("Btn_Destiny"), ReddotCallback_Show_Btn_Destiny);
        //Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.EquipRecycle, f_GetObject("Btn_Recycling"), ReddotCallback_Show_Btn_Recycling, true);
        //Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.CardRecycle, f_GetObject("Btn_Recycling"), ReddotCallback_Show_Btn_Recycling, true);
        // Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.NorAdmiralFree, f_GetObject("BtnRecruitCard"), ReddotCallback_Show_Btn_Recruit, true);
        // Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.GenAdmiralFree, f_GetObject("BtnRecruitCard"), ReddotCallback_Show_Btn_Recruit, true);
        // Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.NorAdmiralFree, f_GetObject("BtnMore"), ReddotCallback_Show_Btn_More, true);
        // Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.GenAdmiralFree, f_GetObject("BtnMore"), ReddotCallback_Show_Btn_More, true);
        // Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.ShopGiftBuy, f_GetObject("BtnMore"), ReddotCallback_Show_Btn_More, true);
        // Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.ChatWordNewMsg, f_GetObject("BtnMore"), ReddotCallback_Show_Btn_More, true);
        // Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.ChatLegionNewMsg, f_GetObject("BtnMore"), ReddotCallback_Show_Btn_More, true);
        // Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.ChatTeamNewMsg, f_GetObject("BtnMore"), ReddotCallback_Show_Btn_More, true);
        // Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.ChatPrivateNewMsg, f_GetObject("BtnMore"), ReddotCallback_Show_Btn_More, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.FriendVigor, f_GetObject("BtnMore"), ReddotCallback_Show_Btn_More, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.FriendApply, f_GetObject("BtnMore"), ReddotCallback_Show_Btn_More, true);
        // Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.NorAdmiralFree, f_GetObject("BtnShop"), ReddotCallback_Show_Btn_Shop, true);
        // Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.GenAdmiralFree, f_GetObject("BtnShop"), ReddotCallback_Show_Btn_Shop, true);
        // Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.ShopGiftBuy, f_GetObject("BtnShop"), ReddotCallback_Show_Btn_Shop, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.FirstRecharge, f_GetObject("BtnFirstRecharge"), ReddotCallback_Show_Btn_FirstRecharge);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.RankGiftBuy, f_GetObject("BtnRankGift"), ReddotCallback_Show_Btn_RankGift);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.ChatWordNewMsg, f_GetObject("BtnStore"), ReddotCallback_Show_Btn_Chat, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.ChatLegionNewMsg, f_GetObject("BtnStore"), ReddotCallback_Show_Btn_Chat, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.ChatTeamNewMsg, f_GetObject("BtnStore"), ReddotCallback_Show_Btn_Chat, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.ChatPrivateNewMsg, f_GetObject("BtnStore"), ReddotCallback_Show_Btn_Chat, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.FriendVigor, f_GetObject("BtnFriend"), ReddotCallback_Show_Btn_Friend, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.FriendApply, f_GetObject("BtnFriend"), ReddotCallback_Show_Btn_Friend, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.RunningManEliteLeftTimes, f_GetObject("Btn_Challenge"), ReddotCallback_Show_Btn_Challenge, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.TreasureCanFix, f_GetObject("Btn_Challenge"), ReddotCallback_Show_Btn_Challenge, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.PatrolGetAward, f_GetObject("Btn_Challenge"), ReddotCallback_Show_Btn_Challenge, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.RebelArmy, f_GetObject("Btn_Challenge"), ReddotCallback_Show_Btn_Challenge, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.RebelArmyAward, f_GetObject("Btn_Challenge"), ReddotCallback_Show_Btn_Challenge, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.DailyPve, f_GetObject("Btn_Dungeon"), ReddotCallback_Show_Btn_Dungeon, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.LegendHavaTimes, f_GetObject("Btn_Dungeon"), ReddotCallback_Show_Btn_Dungeon, true);
        //活动红点
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.UserSign, f_GetObject("BtnActivity"), ReddotCallback_Show_BtnActivity, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.GrandSign, f_GetObject("BtnActivity"), ReddotCallback_Show_BtnActivity, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.BanQuet, f_GetObject("BtnActivity"), ReddotCallback_Show_BtnActivity, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.LuckySymbolFree, f_GetObject("BtnActivity"), ReddotCallback_Show_BtnActivity, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.WealthMan, f_GetObject("BtnActivity"), ReddotCallback_Show_BtnActivity, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.LoginGift, f_GetObject("BtnActivity"), ReddotCallback_Show_BtnActivity, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.LoginGiftNewServ, f_GetObject("BtnActivity"), ReddotCallback_Show_BtnActivity, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.MonthCardGet, f_GetObject("BtnActivity"), ReddotCallback_Show_BtnActivity, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.ActOpenServFund, f_GetObject("BtnActivity"), ReddotCallback_Show_BtnActivity, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.ActOpenWelfare, f_GetObject("BtnActivity"), ReddotCallback_Show_BtnActivity, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.OnlineAward, f_GetObject("BtnActivity"), ReddotCallback_Show_BtnActivity, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.SkyFortune, f_GetObject("BtnActivity"), ReddotCallback_Show_BtnActivity, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.TenSycee, f_GetObject("BtnActivity"), ReddotCallback_Show_BtnActivity, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.VipGift, f_GetObject("BtnActivity"), ReddotCallback_Show_BtnActivity, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.FirstRecharge, f_GetObject("BtnActivity"), ReddotCallback_Show_BtnActivity, true);
        //暂时没图标
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.TariningGetAward, f_GetObject("Btn_Tarining"), ReddotCallback_Show_BtnTactical, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.TacticalStdySkill, f_GetObject("Btn_Tarining"), ReddotCallback_Show_BtnTactical, true);
        //七日活动
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.SevenDayTask, f_GetObject("Seven"), ReddotCallback_Show_Seven, true);

        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.SingleRecharge, f_GetObject("BtnNewYear"), ReddotCallback_Show_BtnNewYear, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.MutiRecharge, f_GetObject("BtnNewYear"), ReddotCallback_Show_BtnNewYear, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.RedPacketTask, f_GetObject("BtnNewYear"), ReddotCallback_Show_BtnNewYear, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.RecruitGift, f_GetObject("BtnNewYear"), ReddotCallback_Show_BtnNewYear, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.RedPacketExchange, f_GetObject("BtnNewYear"), ReddotCallback_Show_BtnNewYear, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.TotalConst, f_GetObject("BtnNewYear"), ReddotCallback_Show_BtnNewYear, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.NewYearSelling, f_GetObject("BtnNewYear"), ReddotCallback_Show_BtnNewYear, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.MammonSendGifts, f_GetObject("BtnNewYear"), ReddotCallback_Show_BtnNewYear, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.NewYearSign, f_GetObject("BtnNewYear"), ReddotCallback_Show_BtnNewYear, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.DealsEveryDay, f_GetObject("BtnNewYear"), ReddotCallback_Show_BtnNewYear, true);
        UpdateReddotUI();
        CheckRedDotFormServer();
        Data_Pool.m_EquipFragmentPool.f_ReCheckRedPoint();//不需要联网
        Data_Pool.m_CardFragmentPool.f_ReCheckRedPoint();//不需要联网
        Data_Pool.m_GodEquipFragmentPool.f_ReCheckRedPoint();//不需要联网


        UITool.f_UpdateReddot(f_GetObject("BtnAwardCenter"), 1, new Vector3(30, 40, 0), 1);//领奖中心
        Data_Pool.m_FriendPool.f_CheckFriendRedDot(); //好友检查红点
    }
    private void BagNewGoods()
    {
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.EquipBagNewGoods, f_GetObject("Btn_Backpack"), ReddotCallback_Show_Btn_Backpack, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.GoodsBagNewGoods, f_GetObject("Btn_Backpack"), ReddotCallback_Show_Btn_Backpack, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.AwakenBagNewGoods, f_GetObject("Btn_Backpack"), ReddotCallback_Show_Btn_Backpack, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.TreasureBagNewGoods, f_GetObject("Btn_Backpack"), ReddotCallback_Show_Btn_Backpack, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.GodEquipBagNewGoods, f_GetObject("Btn_Backpack"), ReddotCallback_Show_Btn_Backpack, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.EquipFragmentBagNewGoods, f_GetObject("Btn_Backpack"), ReddotCallback_Show_Btn_Backpack, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.TreasureFragmentBagNewGoods, f_GetObject("Btn_Backpack"), ReddotCallback_Show_Btn_Backpack, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.GodEquipFragmentBagNewGoods, f_GetObject("Btn_Backpack"), ReddotCallback_Show_Btn_Backpack, true);

        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.EquipBagNewGoods, f_GetObject("Btn_EquipBag"), ReddotCallback_Show_Btn_EquipBag, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.EquipFragmentBagNewGoods, f_GetObject("Btn_EquipBag"), ReddotCallback_Show_Btn_EquipBag, true);

        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.TreasureBagNewGoods, f_GetObject("Btn_TreasureBag"), ReddotCallback_Show_Btn_TreasureBag, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.TreasureFragmentBagNewGoods, f_GetObject("Btn_TreasureBag"), ReddotCallback_Show_Btn_TreasureBag, true);

        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.GodEquipBagNewGoods, f_GetObject("Btn_GodEquipBag"), ReddotCallback_Show_Btn_GodEquipBag, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.GodEquipFragmentBagNewGoods, f_GetObject("Btn_GodEquipBag"), ReddotCallback_Show_Btn_GodEquipBag, true);

        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.GoodsBagNewGoods, f_GetObject("Btn_GoodsBag"), ReddotCallback_Show_Btn_GoodsBag, true);

        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.AwakenBagNewGoods, f_GetObject("Btn_AwakeBag"), ReddotCallback_Show_Btn_AwakenBag, true);

        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.CardFragmentBagNewGoods, f_GetObject("Btn_CardBag"), ReddotCallback_Show_Btn_CardBag, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.CardBagNewGoods, f_GetObject("Btn_CardBag"), ReddotCallback_Show_Btn_CardBag, true);

        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.LegionApplicantList, f_GetObject("Btn_Legion"), ReddotCallback_Show_Btn_Legion, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.LegionChapetAward, f_GetObject("Btn_Legion"), ReddotCallback_Show_Btn_Legion, true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.LegionRedPacket, f_GetObject("Btn_Legion"), ReddotCallback_Show_Btn_Legion, true);

        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.CardBagNewGoods);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.CardFragmentBagNewGoods);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.AwakenBagNewGoods);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.GoodsBagNewGoods);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.TreasureFragmentBagNewGoods);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.TreasureBagNewGoods);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.EquipFragmentBagNewGoods);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.EquipBagNewGoods);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.GodEquipBagNewGoods);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.GodEquipFragmentBagNewGoods);

    }
    /// <summary>
    /// 检查服务器端红点提示，客户端主动检查（主页面打开或UnHold时会检查）
    /// </summary>
    private void CheckRedDotFormServer()
    {
        Data_Pool.m_ShopLotteryPool.f_GetLotteryShopInfo(null);//请求商店抽牌信息, 第二次不需要联网
        Data_Pool.m_ShopGiftPool.f_GetNewShop(null);//请求商店礼包信息
        Data_Pool.m_ActivityCommonData.f_QueryFirstRecharge(null);//请求首充信息, 第二次不需要联网
        Data_Pool.m_TreasurePool.f_CheckCanFixTreasure();//检查是否有可合成的法宝，不需要联网
        Data_Pool.m_DailyPveInfoPool.f_DailyPveInfo(null);//日常副本 第二次不需要联网
        Data_Pool.m_DungeonPool.f_CheckLegionRedDot();//不需要联网
        Data_Pool.m_TaskDailyPool.f_CheckRedPoint();//不需要联网（日常任务）
        Data_Pool.m_TaskAchvPool.f_CheckRedPoint();//不需要联网（日常成就）
        Data_Pool.m_SignPool.f_RequestIsSignToday(null);//第二次不需要联网
        Data_Pool.m_ActivityCommonData.f_GetPowerInfo(null);//第二次不需要联网
        Data_Pool.m_ActivityCommonData.f_QueryLuckySymbolInfo(null);//第二次不需要联网
        Data_Pool.m_ActivityCommonData.f_QueryWealthManInfo(null);//第二次不需要联网
        Data_Pool.m_ActivityCommonData.f_QueryLoginGift(null);//第二次不需要联网
        Data_Pool.m_ActivityCommonData.f_QueryLoginGiftNewServ(null);//第二次不需要联网
        Data_Pool.m_ActivityCommonData.f_QueryMonthCard(null);//第二次不需要联网

        Data_Pool.m_BattleFormPool.f_QueryBattleForm(null);//第二次不需要联网
        Data_Pool.m_RankGiftPool.f_RequestRankGiftInfo(null);//等级礼包
        Data_Pool.m_OpenServFundPool.f_CheckRedPoint();//检查开服基金和全民福利红点
        Data_Pool.m_OnlineAwardPool.f_QueryInfo(null);//检查在线豪礼红点
        Data_Pool.m_SkyFortunePool.f_QueryInfo(null);//第二次不需要联网

        Data_Pool.m_SevenActivityTaskPool.f_GetTaskInfo(null);
        Data_Pool.m_SevenActivityTaskPool._LoadDate();
        Data_Pool.m_FriendPool.f_CheckRedPoint();//不需要联网
        //Data_Pool.m_EquipPool.f_CheckEquipRecycleRedPoint();//不需要联网
        //Data_Pool.m_CardPool.f_CheckCardRecycleRedPoint();//不需要联网

        Data_Pool.m_SingleRechargePool.f_QueryInfo(null);//第二次不需要联网
        Data_Pool.m_MutiRechargePool.f_QueryInfo(null);//第二次不需要联网
        Data_Pool.m_RedPacketTaskPool.f_QueryInfo(null);//第二次不需要联网
        Data_Pool.m_RedPacketExchangePool.f_QueryInfo(null);//第二次不需要联网
        Data_Pool.m_TotalConsumptionPool.f_TotalConsumpInfo(null);//第二次不需要联网
        Data_Pool.m_NewYearActivityPool.f_MammonSendGiftNewInfo(null);
        Data_Pool.m_NewYearSellingPool.f_NewYearSellingInfo(null);
        Data_Pool.m_NewYearSignPool.f_RequestSignInfo(null);//第二次不需要联网
        Data_Pool.m_TenSyceePool.f_RequestSyceeInfo(null);//第二次不需要联网
        Data_Pool.m_VipGiftPool.f_RequestInfo(null);//第二次不需要联网

        Data_Pool.m_RebelArmyPool.f_CrusadeRebelInit(null);
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.RebelArmy);
        Data_Pool.m_RebelArmyPool.f_CrusadeRebelList(delegate (object obj)
        {
            List<BasePoolDT<long>> tPoolDT = Data_Pool.m_RebelArmyPool.f_GetAll();
            RebelArmyPoolDT myPoolDt = tPoolDT.Find((BasePoolDT<long> item) =>
            {
                return item.iId == Data_Pool.m_UserData.m_iUserId;
            }) as RebelArmyPoolDT;
            if (null != myPoolDt && myPoolDt.m_EndTime > GameSocket.GetInstance().f_GetServerTime() && myPoolDt.hpPercent * 0.00000001f > 0)
            {
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.RebelArmy);
            }
        });

        SocketCallbackDT QueryCallback = new SocketCallbackDT();//请求信息回调
        QueryCallback.m_ccCallbackFail = OnFirstRecharFailCallback;
        QueryCallback.m_ccCallbackSuc = OnFirstRecharSucCallback;
        Data_Pool.m_FirstRechargePool.f_QueryInfo(QueryCallback);//第二次不需要联网

        Data_Pool.m_ValentinesDayPool.f_ValentinesInfo(null);

        Data_Pool.m_WeekFundPool.f_QueryWeekFundInfo(null);//周基金

        if (!Data_Pool.m_NewYearActivityPool.m_bFesticalIsOpen)
        {
            if (Data_Pool.m_NewYearActivityPool.f_CheckFestivaExchangeOpenBool())
            {
                Data_Pool.m_NewYearActivityPool.f_FestivalExchangeInfo(null);
            }
        }

        GameParamDT param = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.TariningOpenSv);
        if (CommonTools.f_CheckOpenServerDay(param.iParam1))
        {
            f_GetObject("Btn_Tarining").SetActive(Data_Pool.m_TariningAndTacticalPool._sakInfo);
            //f_GetObject("Btn_Tarining").transform.parent.GetComponent<UIGrid>().repositionNow = true;

            SocketCallbackDT tTrainingInfoSocketBack = new SocketCallbackDT();
            tTrainingInfoSocketBack.m_ccCallbackSuc = TrainingInfoSuc;
            tTrainingInfoSocketBack.m_ccCallbackFail = TrainingInfoFail;
            //UITool.f_OpenOrCloseWaitTip(true);
            Data_Pool.m_TariningAndTacticalPool.f_TrainingSendInfo(tTrainingInfoSocketBack);  //练兵场
        }
        SetIcon();
        Data_Pool.m_TariningAndTacticalPool.f_TacticalSendInfo(null);
        //检查阵容红点，不用联网
        int num = Data_Pool.m_TeamPool.f_CheckRedPoint() ? 1 : 0;
        GameObject BtnClothArray = f_GetObject("BtnClothArray");
        UITool.f_UpdateReddot(BtnClothArray, num, new Vector3(30, 40, 0), 102);

        Data_Pool.m_OnlineVipAwardPool.f_QueryInfo(null);//第二次不需要联网

        Data_Pool.m_NewYearActivityPool.f_InitDealsEveryDayPoolDT();
    }/// <summary>
     /// 查询成功回调
     /// </summary>
    private void OnFirstRecharFailCallback(object obj)
    {

    }
    private void OnFirstRecharSucCallback(object obj)
    {

        GameObject BtnFirstRecharge = f_GetObject("BtnFirstRecharge");
        BtnFirstRecharge.transform.parent.gameObject.SetActive(Data_Pool.m_FirstRechargePool.f_CheckIsOpen());
        UIGrid Grid = BtnFirstRecharge.transform.GetComponentInParent<UIGrid>();
        SetIcon();
        if (Grid != null)
            Grid.Reposition();
    }

    private void TrainingInfoSuc(object obj)
    {
        //f_GetObject("Btn_Tarining").SetActive(UITool.f_GetIsOpensystem(EM_NeedLevel.TariningLevel));
        GameParamDT param = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.TariningOpenSv);
        f_GetObject("Btn_Tarining").SetActive(CommonTools.f_CheckOpenServerDay(param.iParam1));

        //f_GetObject("Btn_Tarining").transform.parent.GetComponent<UIGrid>().repositionNow = true;

        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void TrainingInfoFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
    }
    private void TaskRedDotUI(object data)
    {
        Data_Pool.m_TaskDailyPool.f_CheckRedPoint();//不需要联网（日常任务）
        Data_Pool.m_TaskAchvPool.f_CheckRedPoint();//不需要联网（日常成就）
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.TaskPage_Daily);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.TaskPage_Achievement);
    }
	
	//My Code
	void Update()
	{
		if(resetScroll < 2)
		{
			glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.MainBg, true, 0.2f);
			resetScroll++;
		}
	}
	//
	
    protected override void UpdateReddotUI()
    {
        base.UpdateReddotUI();

        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.TaskPage_Daily);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.TaskPage_Achievement);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.CardBag_Fragment);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.EquipBag_Fragment);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.GodEquipBag_Fragment);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.BattleForm_CanAct);
        //Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.EquipRecycle);
        //Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.CardRecycle);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.BattleForm_CanAct);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.NorAdmiralFree);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.GenAdmiralFree);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.ShopGiftBuy);
        //Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.FirstRecharge);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.RankGiftBuy);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.ChatWordNewMsg);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.ChatLegionNewMsg);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.ChatTeamNewMsg);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.ChatPrivateNewMsg);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.FriendVigor);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.FriendApply);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.RunningManEliteLeftTimes);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.PatrolGetAward);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.RebelArmy);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.RebelArmyAward);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.TreasureCanFix);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.DailyPve);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.LegendHavaTimes);

        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.UserSign);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.GrandSign);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.BanQuet);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.LuckySymbolFree);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.WealthMan);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.LoginGift);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.LoginGiftNewServ);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.MonthCardGet);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.ActOpenServFund);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.ActOpenWelfare);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.OnlineAward);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.SkyFortune);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.SevenDayTask);


        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.SingleRecharge);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.MutiRecharge);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.RedPacketTask);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.RedPacketExchange);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.TotalConst);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.NewYearSign);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.DealsEveryDay);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.TenSycee);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.VipGift);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.FirstRecharge);

        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.LegionApplicantList);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.LegionChapetAward);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.LegionRedPacket);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.TariningGetAward);
    }
    private void ReddotCallback_Show_BtnTask(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnTask = f_GetObject("BtnTask");
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < GameParamConst.TaskLvLimit)
            iNum = 0;
        UITool.f_UpdateReddot(BtnTask, iNum, new Vector3(30, 40, 0), 102);
    }

    private void ReddotCallback_Show_Btn_Backpack(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnTask = f_GetObject("Btn_Backpack");
        UITool.f_UpdateReddot(BtnTask, iNum, new Vector3(30, 40, 0), 102);
    }

    private void ReddotCallback_Show_Btn_EquipBag(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnTask = f_GetObject("Btn_EquipBag");
        UITool.f_UpdateReddot(BtnTask, iNum, new Vector3(30, 40, 0), 102);
    }
    private void ReddotCallback_Show_Btn_TreasureBag(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnTask = f_GetObject("Btn_TreasureBag");
        UITool.f_UpdateReddot(BtnTask, iNum, new Vector3(30, 40, 0), 102);
    }

    private void ReddotCallback_Show_Btn_GodEquipBag(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnTask = f_GetObject("Btn_GodEquipBag");
        UITool.f_UpdateReddot(BtnTask, iNum, new Vector3(30, 40, 0), 102);
    }

    private void ReddotCallback_Show_Btn_GoodsBag(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnTask = f_GetObject("Btn_GoodsBag");
        UITool.f_UpdateReddot(BtnTask, iNum, new Vector3(30, 40, 0), 102);
    }
    private void ReddotCallback_Show_Btn_AwakenBag(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnTask = f_GetObject("Btn_AwakeBag");
        UITool.f_UpdateReddot(BtnTask, iNum, new Vector3(30, 40, 0), 102);
    }
    private void ReddotCallback_Show_Btn_CardBag(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnCardBag = f_GetObject("Btn_CardBag");
        UITool.f_UpdateReddot(BtnCardBag, iNum, new Vector3(30, 40, 0), 102);
    }

    private void ReddotCallback_Show_Btn_Destiny(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnDestiny = f_GetObject("Btn_Destiny");
        UITool.f_UpdateReddot(BtnDestiny, iNum, new Vector3(30, 40, 0), 102);
    }
    private void ReddotCallback_Show_Btn_Recycling(object Obj)
    {
        int iNum = (int)Obj;
        GameObject Btn_Recycling = f_GetObject("Btn_Recycling");
        UITool.f_UpdateReddot(Btn_Recycling, iNum, new Vector3(30, 40, 0), 102);
    }
    private void ReddotCallback_Show_Btn_Recruit(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnRecruitCard = f_GetObject("BtnRecruitCard");
        UITool.f_UpdateReddot(BtnRecruitCard, iNum, new Vector3(30, 40, 0), 102);
    }
    private void ReddotCallback_Show_Btn_More(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnMore = f_GetObject("BtnMore");
        UITool.f_UpdateReddot(BtnMore, iNum, new Vector3(30, 40, 0), 102);
    }
    // private void ReddotCallback_Show_Btn_Shop(object Obj)
    // {
    // int iNum = (int)Obj;
    // GameObject BtnShop = f_GetObject("BtnShop");
    // UITool.f_UpdateReddot(BtnShop, iNum, new Vector3(55, 55, 0), 102);
    // }
    private void ReddotCallback_Show_Btn_Chat(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnChat = f_GetObject("BtnStore");
        UITool.f_UpdateReddot(BtnChat, iNum, new Vector3(30, 40, 0), 102);
    }
    private void ReddotCallback_Show_Btn_FirstRecharge(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnFirstRecharge = f_GetObject("BtnFirstRecharge");
        UITool.f_UpdateReddot(BtnFirstRecharge, iNum, new Vector3(30, 40, 0), 102);
    }
    private void ReddotCallback_Show_Btn_RankGift(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnRankGift = f_GetObject("BtnRankGift");
        UITool.f_UpdateReddot(BtnRankGift, iNum, new Vector3(30, 40, 0), 102);
    }
    private void ReddotCallback_Show_Btn_Friend(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnMore = f_GetObject("BtnFriend");
        UITool.f_UpdateReddot(BtnMore, iNum, new Vector3(30, 40, 0), 102);
    }

    private void ReddotCallback_Show_Btn_Challenge(object Obj)
    {
        int iNum = (int)Obj;
        GameObject Btn_Challenge = f_GetObject("Btn_Challenge");
        UITool.f_UpdateReddot(Btn_Challenge, iNum, new Vector3(110, 30, 0), 102);
    }
    private void ReddotCallback_Show_Btn_Dungeon(object Obj)
    {
        int iNum = (int)Obj;
        if (iNum <= 0)
        {
            bool MainDungeonRedPoint = Data_Pool.m_DungeonPool.f_CheckMainDungeonBoxGetRedPoint();
            bool EliteDungeonRedPoint = Data_Pool.m_DungeonPool.f_CheckEliteDungeonBoxGetRedPoint();
            if (MainDungeonRedPoint || EliteDungeonRedPoint)
                iNum = 1;
        }
        GameObject Btn_Dungeon = f_GetObject("Btn_Dungeon");
        UITool.f_UpdateReddot(Btn_Dungeon, iNum, new Vector3(110, 30, 0), 102);
    }
    private void ReddotCallback_Show_BtnActivity(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnActivity = f_GetObject("BtnActivity");
        UITool.f_UpdateReddot(BtnActivity, iNum, new Vector3(30, 40, 0), 102);
    }
    private void ReddotCallback_Show_BtnTactical(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnActivity = f_GetObject("Btn_Tarining");
        UITool.f_UpdateReddot(BtnActivity, iNum, new Vector3(30, 40, 0), 102);
    }
    private void ReddotCallback_Show_BtnNewYear(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnNewYear = f_GetObject("BtnNewYear");
        UITool.f_UpdateReddot(BtnNewYear, iNum, new Vector3(30, 40, 0), 102);
    }

    //军团红点
    private void ReddotCallback_Show_Btn_Legion(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnLegion = f_GetObject("Btn_Legion");
        UITool.f_UpdateReddot(BtnLegion, iNum, new Vector3(80, 60, 0), 102);
    }

    private void ReddotCallback_Show_Seven(object obj)
    {
        int iNum = (int)obj;
        GameObject BtnSeven = f_GetObject("Seven");
        UITool.f_UpdateReddot(BtnSeven, iNum, new Vector3(30, 40, 0), 102);
    }

    #endregion

    #region 初始UI, 手工调用
    /// <summary>
    /// 初始UI, 手工调用
    /// </summary>
    protected override void InitGUI()
    {
        m_LegionBattleBtn = f_GetObject("LegionBattleBtn");
        m_LegionBattleBtn.SetActive(false);
        f_RegClickEvent(m_LegionBattleBtn, f_OnLegionBattleBtnClick);
        m_CardBattleBtn = f_GetObject("CardBattleBtn");
        f_UpdateCardBattleBtnState();
        f_RegClickEvent(m_CardBattleBtn, f_OnCardBattleBtnClick);
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_DungeonPool.f_DungeonLegend();
        Data_Pool.m_TaskDailyPool.f_ExecuteAfterInit(TaskRedDotUI);
        Data_Pool.m_TaskAchvPool.f_ExecuteAfterInit(TaskRedDotUI);
        Data_Pool.m_FriendPool.f_ExecuteAfterInit(EM_FriendListType.Friend, f_ExecuteAfterFriendInit);
        Data_Pool.m_FriendPool.f_ExecuteAfterInit(EM_FriendListType.Apply, f_ExecuteAfterFriendApplyInit);
        Data_Pool.m_FriendPool.f_ExecuteAfterInit(EM_FriendListType.Blacklist, f_ExecuteAfterBlacklistInit);
        Data_Pool.m_FriendPool.f_ExecuteAfterInit(EM_FriendListType.Vigor, f_ExecuteAfterVigorInit);
        LegionMain.GetInstance().m_LegionInfor.f_ExecuteAfterLegionInit(f_ExecuteAfterLegionInit);
        Data_Pool.m_RunningManPool.f_ExecuteAfterRunningManInit(f_ExecuteAfterRunningManInit);
        Data_Pool.m_RunningManShopPool.f_ExecuteAferShopInit(f_ExecuteAfterRunningMamShopInit);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_GetNewShop;
        socketCallbackDt.m_ccCallbackFail = f_Callback_GetNewShop;
        Data_Pool.m_ShopResourcePool.f_GetNewShop(socketCallbackDt);
        f_Battle2MenuProcess();
        Data_Pool.m_SevenActivityTaskPool.f_GetOpenTime(_SevenOpen, null);
        Data_Pool.m_PatrolPool.f_InitReddotData(f_ExecuteAfterInitPatrolReddotData);
    }

    //限时神装按钮显示
    private void f_ShowLimitBtn()
    {
        f_GetObject("LimitGodBtn").SetActive(Data_Pool.m_GodDressPool.f_CheckLimitGodOpen());
        f_GetObject("TopRightGrid").GetComponent<UIGrid>().Reposition();
    }

    private void f_ExecuteAfterInitPatrolReddotData(object result)
    {
        Data_Pool.m_PatrolPool.f_CheckPatrolReddot();
    }

    private void _SevenOpen(object obj)
    {
        f_GetObject("Seven").SetActive((Data_Pool.m_SevenActivityTaskPool.OpenSeverTime + 864000) - GameSocket.GetInstance().f_GetServerTime() >= 0);
        SetIcon();
        //My Code
        if (ActivityOpen != null)
        {
            if (ActivityOpen.iParam3 == 0)
            {
                f_GetObject("Seven").SetActive(false);
            }
            f_GetObject("TopRightGrid").GetComponent<UIGrid>().Reposition();
        }
        //
    }
    #endregion
    #region Enter主界面做的处理

    private void f_Callback_GetNewShop(object result)
    {
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
            MessageBox.DEBUG(CommonTools.f_GetTransLanguage(19));
        else
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(20));
    }

    public void f_ExecuteAfterDailyTaskInit(object result)
    {

    }

    public void f_ExecuteAfterAchvTaskInit(object resutl)
    {

    }

    public void f_ExecuteAfterFriendInit(object result)
    {
        if ((int)result != (int)eMsgOperateResult.OR_Succeed)
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(21) + result);
        else
            MessageBox.DEBUG(CommonTools.f_GetTransLanguage(22));
    }

    public void f_ExecuteAfterFriendApplyInit(object result)
    {
        if ((int)result != (int)eMsgOperateResult.OR_Succeed)
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(23) + result);
        else
            MessageBox.DEBUG(CommonTools.f_GetTransLanguage(24));
    }

    public void f_ExecuteAfterBlacklistInit(object result)
    {
        if ((int)result != (int)eMsgOperateResult.OR_Succeed)
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(25) + result);
        else
            MessageBox.DEBUG(CommonTools.f_GetTransLanguage(26));
    }

    public void f_ExecuteAfterVigorInit(object result)
    {
        if ((int)result != (int)eMsgOperateResult.OR_Succeed)
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(27) + result);
        else
            MessageBox.DEBUG(CommonTools.f_GetTransLanguage(28));
    }

    private void f_ExecuteAfterLegionInit(object result)
    {
        if ((int)result != (int)eMsgOperateResult.OR_Succeed)
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(29) + result);
        else
        {
            MessageBox.DEBUG(CommonTools.f_GetTransLanguage(30));
            if (LegionMain.GetInstance().m_LegionInfor.m_iLegionId == 0)
                //申请军团列表
                LegionMain.GetInstance().m_LegionInfor.f_ExecuteAfterApplyList(f_ExecuteAfterApplyList);
            else
            {
                if (LegionTool.f_IsEnoughPermission(EM_LegionOperateType.ApplicantList, false))
                {
                    //成员管理的申请列表
                    LegionMain.GetInstance().m_LegionPlayerPool.f_ExecuteAfterApplicantList(f_ExecuteAfterApplicantList);
                }
                //军团副本数据初始化，驱动红点
                LegionMain.GetInstance().m_LegionInfor.f_ExecuteAfterLegionInfo(true, true, f_Callback_LegionSelfInfo_InitFiniChaper);
                //军团红包数据初始化，驱动红点
                SocketCallbackDT socketCallbackDt1 = new SocketCallbackDT();
                socketCallbackDt1.m_ccCallbackSuc = f_Callback_GetRedpacketInfo;
                socketCallbackDt1.m_ccCallbackFail = f_Callback_GetRedpacketInfo;
                LegionMain.GetInstance().m_LegionRedPacketPool.f_GetRedPacketInfo(socketCallbackDt1);
                //更新军团战按钮状态
                f_UpdateLegionBattleBtnState();
            }
        }
    }

    private void f_ExecuteAfterApplyList(object result)
    {
        if ((int)result != (int)eMsgOperateResult.OR_Succeed)
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(31) + result);
        else
            MessageBox.DEBUG(CommonTools.f_GetTransLanguage(32));
    }

    private void f_ExecuteAfterApplicantList(object result)
    {
        if ((int)result != (int)eMsgOperateResult.OR_Succeed)
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(33) + result);
        else
            MessageBox.DEBUG(CommonTools.f_GetTransLanguage(34));
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.LegionApplicantList);
    }

    private void f_Callback_InitFiniChaper(object result)
    {
        if ((int)result != (int)eMsgOperateResult.OR_Succeed)
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(35) + result);
        else
            MessageBox.DEBUG(CommonTools.f_GetTransLanguage(36));
    }

    private void f_Callback_GetRedpacketInfo(object result)
    {
        if ((int)result != (int)eMsgOperateResult.OR_Succeed)
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(37) + result);
        else
            MessageBox.DEBUG(CommonTools.f_GetTransLanguage(38));
    }
    private void f_ExecuteAfterRunningManInit(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result != (int)eMsgOperateResult.OR_Succeed)
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(39) + result);
        else
            MessageBox.DEBUG(CommonTools.f_GetTransLanguage(40));
    }

    private void f_ExecuteAfterRunningMamShopInit(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result != (int)eMsgOperateResult.OR_Succeed)
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(41) + result);
        else
            MessageBox.DEBUG(CommonTools.f_GetTransLanguage(42));
    }

    #endregion
    #region UI_UNHOLD/UI_HOLD 战斗场景切换主场景处理(Battle2MenuProcess) 
    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        //获取途径记忆功能
        SpriNextLv2 = GameObject.Find("SpriNextLv2");
        LabelNextOpen2 = GameObject.Find("LabelNextOpen2");
        LabelNextOpenTitle2 = GameObject.Find("LabelNextOpenTitle2");
        TexBgKD = GameObject.Find("TexBgKD");
        UITool.f_GetCurLevelOpenSystemDes(out HasOpenFunc, out NextOpenFuc, out nextOpenLevel, out OpenFuncSprite, out NextOpenFuncSprite);

        if (TexBgKD != null)
        {
            if (AssetOpen.iParam2 == 0)
            {
                TexBgKD.SetActive(false);
            }
        }
        if (!MainBgIsPlaying)
        {
            glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.MainBg);
            MainBgIsPlaying = true;
        }
        if (SpriNextLv2 != null)
            SpriNextLv2.SetActive(NextOpenFuc != null);
        if (nextOpenLevel == 300 && SpriNextLv2 != null)
            SpriNextLv2.SetActive(false);

        //My Code
		CardBattleOpen = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(96) as GameParamDT);
		CardBattleOpenLvl = CardBattleOpen.iParam3 * 1;
		//

        if (NextOpenFuc != null && LabelNextOpen2 != null && LabelNextOpenTitle2 != null && SpriNextLv2 != null && nextOpenLevel != 300)
        {
            LabelNextOpen2.GetComponent<UILabel>().text = NextOpenFuc ?? "";
LabelNextOpenTitle2.GetComponent<UILabel>().text = "Level " + nextOpenLevel;
            SpriNextLv2.GetComponent<UISprite>().spriteName = NextOpenFuncSprite;
            if (nextOpenLevel == 2)
            {
                SpriNextLv2.GetComponent<UISprite>().spriteName = "LineUpPage6";
LabelNextOpen2.GetComponent<UILabel>().text = "Open all champion tiles";
            }
            if (nextOpenLevel == UITool.f_GetSysOpenLevel(EM_NeedLevel.CrossServerBattle))
            {
LabelNextOpen2.GetComponent<UILabel>().text = "Open Peak";
            }
            if (nextOpenLevel == CardBattleOpenLvl )
            {
LabelNextOpenTitle2.GetComponent<UILabel>().text = "Grade " + "55";
                SpriNextLv2.GetComponent<UISprite>().spriteName = "Icon_CardBattle";
LabelNextOpen2.GetComponent<UILabel>().text = "Open Martial Arts Club";
            }
        }
        //


        if (StaticValue.mGetWayToBattleParam.em_GetWayToBattle != EM_GetWayToBattle.None)
        {
            GetWayPageParam param = new GetWayPageParam(StaticValue.mGetWayToBattleParam.m_GetWayResourceType, StaticValue.mGetWayToBattleParam.m_GetWayResourId, this);
            switch (StaticValue.mGetWayToBattleParam.em_GetWayToBattle)
            {
                case EM_GetWayToBattle.CardBag:
                    UI_CardBag(null, null, null);
                    break;
                case EM_GetWayToBattle.EquipBag:
                    UI_EquipBag(null, null, null);
                    break;
                case EM_GetWayToBattle.LineUpPage:
                    UI_ClothArray(null, null, null);
                    break;
            }
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GetWayPage, UIMessageDef.UI_OPEN, param);
        }
        f_UpdateLegionBattleBtnState();
        f_UpdateCardBattleBtnState();

        f_GetObject("TopRightGrid").GetComponent<UIGrid>().Reposition();

        UpdateReddotUI();
        CheckRedDotFormServer();
        _ValentinesInfo();
        _setBtn();
        f_GetObject("TopRightGrid").GetComponent<UIGrid>().Reposition();

        f_OpenTurntable();
        if (Data_Pool.m_BroadcastPool.f_Count() > 0)
            DestoryBroadcastRootChild();
        BroadcastCheck(null);//检测广播消息
        if (Data_Pool.m_BroadcastNoticePool.NeedShowNewMessage())
            DestoryBroadcastRootChildServerNotice();
        BroadcastCheckServerNotice(null);//检测广播消息

        Data_Pool.m_PatrolPool.f_CheckPatrolReddot();
        //重新注册主界面更新数据和模型消息
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_UPDATE_USERINFOR, InitOrUpdateUIDataHolded, this);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_UPDATE_MODELINFOR, InitOrUpdateModelDataHolded, this);

        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_UPDATE_USERINFOR, InitOrUpdateUIData, this);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_UPDATE_MODELINFOR, InitOrUpdateModelData, this);

        //if (mNeedUpdataUIData)
        InitOrUpdateUIData(null);
        if (mNeedUpdataModelData)
            InitOrUpdateModelData(null);
        SetIcon();
        f_ShowLimitBtn();
        InitEventTime();
    }
    private bool mNeedUpdataUIData = false;
    private bool mNeedUpdataModelData = false;
    /// <summary>
    /// 主界面处于关闭界面时UI有没有更新
    /// </summary>
    private void InitOrUpdateUIDataHolded(object data)
    {
        mNeedUpdataUIData = true;
    }
    /// <summary>
    /// 主界面处于关闭界面时模型有没有更新
    /// </summary>
    private void InitOrUpdateModelDataHolded(object data)
    {
        mNeedUpdataModelData = true;
    }
    /// <summary>
    /// 页面hold住
    /// </summary>
    protected override void UI_HOLD(object e)
    {
        base.UI_HOLD(e);
        mNeedUpdataUIData = false;
        mNeedUpdataModelData = false;

        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_UPDATE_USERINFOR, InitOrUpdateUIDataHolded, this);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_UPDATE_MODELINFOR, InitOrUpdateModelDataHolded, this);

        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_UPDATE_USERINFOR, InitOrUpdateUIData, this);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_UPDATE_MODELINFOR, InitOrUpdateModelData, this);
    }
    public void f_Battle2MenuProcess()
    {
        if (StaticValue.mIsNeedShowLevelPage)
        {
            Data_Pool.m_GuidancePool.f_ChangeGuidanceType(EM_GuidanceType.UpLevel);
        }
        ccTimeEvent.GetInstance().f_RegEvent(0.8f, false, null, (object obj) =>
        {
            if (StaticValue.m_LastFightIsTimeOut && !Data_Pool.m_GuidancePool.IsEnter)
            {
                //上场战斗如果超时退出则弹出提示
PopupMenuParams tParams = new PopupMenuParams("Nhắc nhở", CommonTools.f_GetTransLanguage(2254), "Xác nhận");
                ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParams);
                StaticValue.m_LastFightIsTimeOut = false;
            }
        });

        Battle2MenuProcessParam tParam = StaticValue.m_Battle2MenuProcessParam;
        switch (tParam.m_emType)
        {
            case EM_Battle2MenuProcess.None:
                _NewYaerMain();
                break;
            case EM_Battle2MenuProcess.Dungeon:
                ccUIHoldPool.GetInstance().f_Hold(this);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.DungeonChapterPageNew, UIMessageDef.UI_OPEN, tParam);

                Data_Pool.m_GuidancePool.f_ChangeGuidanceType(EM_GuidanceType.OutDungeon);
                break;
            case EM_Battle2MenuProcess.Arena:
                ccUIHoldPool.GetInstance().f_Hold(this);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.ChallengeMenuPage, UIMessageDef.UI_OPEN, StaticValue.m_Battle2MenuProcessParam);
                break;
            case EM_Battle2MenuProcess.CrusadeRebel:
                ccUIHoldPool.GetInstance().f_Hold(this);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.RebelArmy, UIMessageDef.UI_OPEN, true);
                tParam.f_UpdateParam(EM_Battle2MenuProcess.None);
                break;
            case EM_Battle2MenuProcess.GrabTreasure:
                ccUIHoldPool.GetInstance().f_Hold(this);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.ChallengeMenuPage, UIMessageDef.UI_OPEN, tParam);
                break;
            case EM_Battle2MenuProcess.RunningMan:
                ccUIHoldPool.GetInstance().f_Hold(this);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.ChallengeMenuPage, UIMessageDef.UI_OPEN, tParam);
                break;
            case EM_Battle2MenuProcess.RunningManElite:
                ccUIHoldPool.GetInstance().f_Hold(this);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.ChallengeMenuPage, UIMessageDef.UI_OPEN, tParam);
                break;
            case EM_Battle2MenuProcess.Patrol:
                ccUIHoldPool.GetInstance().f_Hold(this);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.ChallengeMenuPage, UIMessageDef.UI_OPEN, tParam);
                break;
            case EM_Battle2MenuProcess.LegionDungeon:
                if (LegionMain.GetInstance().m_LegionInfor.m_iLegionId == 0)
                {
                    StaticValue.m_Battle2MenuProcessParam.f_UpdateParam(EM_Battle2MenuProcess.None);
                }
                else
                {
                    ccUIHoldPool.GetInstance().f_Hold(this);
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionMenuPage, UIMessageDef.UI_OPEN, tParam);
                }
                break;
            case EM_Battle2MenuProcess.LegionBattle:
                if (LegionMain.GetInstance().m_LegionInfor.m_iLegionId == 0)
                {
                    StaticValue.m_Battle2MenuProcessParam.f_UpdateParam(EM_Battle2MenuProcess.None);
                }
                else
                {
                    ccUIHoldPool.GetInstance().f_Hold(this);
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionMenuPage, UIMessageDef.UI_OPEN, tParam);
                }
                break;
            case EM_Battle2MenuProcess.CrossServerBattle:
                ccUIHoldPool.GetInstance().f_Hold(this);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.ChallengeMenuPage, UIMessageDef.UI_OPEN, tParam);
                break;
            //TsuCode - ChaosBattle
            case EM_Battle2MenuProcess.ChaosBattle:
                ccUIHoldPool.GetInstance().f_Hold(this);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.ChallengeMenuPage, UIMessageDef.UI_OPEN, tParam);
                break;
            //
            case EM_Battle2MenuProcess.CardBattle:
                ccUIHoldPool.GetInstance().f_Hold(this);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.ChallengeMenuPage, UIMessageDef.UI_OPEN, tParam);
                break;
            case EM_Battle2MenuProcess.BattleLose:
                if ((EM_BattleLoseProcess)tParam.m_Params[0] == EM_BattleLoseProcess.GetMoreCard)
                {
                    UI_Recruit(null, null, null);
                }
                else if ((EM_BattleLoseProcess)tParam.m_Params[0] == EM_BattleLoseProcess.CardIntensify)
                {
                    UI_CardBag(null, null, null);
                }
                else if ((EM_BattleLoseProcess)tParam.m_Params[0] == EM_BattleLoseProcess.EquipIntensify)
                {
                    UI_EquipBag(null, null, null);
                }
                else if ((EM_BattleLoseProcess)tParam.m_Params[0] == EM_BattleLoseProcess.LineupChange)
                {
                    UI_ClothArray(null, null, null);
                }
                else
                    MessageBox.ASSERT(CommonTools.f_GetTransLanguage(43));
                tParam.f_UpdateParam(EM_Battle2MenuProcess.None);
                break;
            case EM_Battle2MenuProcess.TrialTower:
                ccUIHoldPool.GetInstance().f_Hold(this);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.ChallengeMenuPage, UIMessageDef.UI_OPEN, tParam);
                break;
            case EM_Battle2MenuProcess.ArenaCrossBattle:
                ccUIHoldPool.GetInstance().f_Hold(this);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.ChallengeMenuPage, UIMessageDef.UI_OPEN, tParam);
                break;
            default:
                break;
        }
    }

    #endregion
    #region 拖拽舞台旋转模型相关
    /// <summary>
    /// 拖拽是否开始
    /// </summary>
    private bool isDragStart = false;
    /// <summary>
    /// 是否正在重设坐标（拖拽结束时重新移动到正确位置)
    /// </summary>
    private bool isRePositionModel = false;
    /// <summary>
    /// 记录拖拽的开始点击点
    /// </summary>
    private Vector3 StartTouchPos = Vector3.zero;
    private float StartTouchPosX;
    private bool sliderRight;//旋转方向(是否向右)
    /// <summary>
    /// 是否发生拖拽模型事件（移动小于一定距离是不发生拖拽的）
    /// </summary>
    private bool isRealDrag = false;
    /// <summary>
    /// 拖动舞台，移动模型
    /// </summary>
    /// <param name="go"></param>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    public void UI_DragSlide(GameObject go, object obj1, object obj2)
    {
        if (isRePositionModel)//正在重设模型坐标，不接受新的
            return;
        if ((bool)obj1)
        {
            if (!isDragStart)
            {
                isDragStart = true;
                isRealDrag = false;
                StartTouchPos = Input.mousePosition;
                StartTouchPosX = StartTouchPos.x;
            }
        }
        else
        {
            isDragStart = false;
            if (isRealDrag)
            {
                sliderRight = (Input.mousePosition.x - StartTouchPosX) > 0 ? true : false;
                isRePositionModel = true;//重置模型
            }
        }
    }
    /// <summary>
    /// 获取对应位置模型缩放大小
    /// </summary>
    /// <param name="index">模型序号</param>
    /// <returns></returns>
    private float GetScaleByModelIndex(int index)
    {
        switch (index)
        {
            case 0: return 1;
            case 1: return 0.9f;//0.66,0.33,0.2
            case 2: return 0.9f;//80-85
            case 3: return 0.7f;//70-75
            case 4: return 0.7f;
            case 5: return 0.63f;//60
            default:
                MessageBox.ASSERT(CommonTools.f_GetTransLanguage(44));
                return 1;
        }
    }
    /// <summary>
    /// 使用此Update, 可以控制使用普通的Update还是FixedUpdate
    /// </summary>
    protected override void f_Update()
    {
        base.f_Update();
        //拖拽开始
        if (isDragStart)
        {
            DragingState();
        }
        else
        {
            //拖拽结束后重设模型坐标位置
            if (isRePositionModel)
            {
                RePositionModelState();
            }
        }
    }
    /// <summary>
    /// 拖拽状态
    /// </summary>
    private void DragingState()
    {
        Vector3 DragPos = Input.mousePosition;
        Vector3 mouseOffsetPos = DragPos - StartTouchPos;
        if (Mathf.Abs(mouseOffsetPos.x) > 12f)//拖动
        {
            isRealDrag = true;
            GameObject ModelPoint = f_GetObject("ModelPoint");
            // ModelPoint.SetActive(true);
            bool isSliderRight = mouseOffsetPos.x > 0 ? true : false;
            float sliderOneDistance = 1000;//滑动一节需要移动的x距离
            float sliderProgress = Mathf.Abs(mouseOffsetPos.x) * 1.0f / sliderOneDistance;
            for (int i = 0; i < 6; i++)
            {
                Transform CardPoint = ModelPoint.transform.Find("Model" + i);
                int nextSliderIndex = GetNexSliderIndex(i, isSliderRight);
                Vector2 oriPos = GetPositionByIndex(i);
                Vector2 nextPos = GetPositionByIndex(nextSliderIndex);
                Vector2 offsetPos = (nextPos - oriPos) * sliderProgress;
                float offsetScale = (GetScaleByModelIndex(nextSliderIndex) - GetScaleByModelIndex(i)) * sliderProgress;
                Vector2 afterPos = oriPos + offsetPos;
                afterPos = CalculateEllipsePos(i, isSliderRight, afterPos, sliderProgress);
                if (float.IsNaN(afterPos.x) || float.IsNaN(afterPos.y))
                {
                    continue;
                }
                else
                    CardPoint.transform.localPosition = afterPos;
                float afterScale = GetScaleByModelIndex(i) + offsetScale;
                CardPoint.transform.localScale = new Vector3(afterScale, afterScale, 1);
            }
            //越界判断
            CheckPosCross(isSliderRight);
        }
    }
    /// <summary>
    /// 拖拽结束重设模型位置状态
    /// </summary>
    private void RePositionModelState()
    {
        GameObject ModelPoint = f_GetObject("ModelPoint");
        for (int i = 0; i < 6; i++)
        {
            Transform CardPoint = ModelPoint.transform.Find("Model" + i);
            int nextSliderIndex = GetNexSliderIndex(i, sliderRight);
            Vector2 oriPos = GetPositionByIndex(i);
            Vector2 nextPos = GetPositionByIndex(nextSliderIndex);
            Vector2 addPos = (nextPos - oriPos) * 0.05f;

            Vector2 cardPos = new Vector2(CardPoint.transform.localPosition.x, CardPoint.transform.localPosition.y);
            Vector2 afterPos = cardPos + addPos;
            float sliderProgress = GetAutoRotateProgress(i, sliderRight, afterPos, nextPos, oriPos);
            afterPos = CalculateEllipsePos(i, sliderRight, afterPos, sliderProgress);
            if (float.IsNaN(afterPos.x) || float.IsNaN(afterPos.y))
            {
                continue;
            }
            else
                CardPoint.transform.localPosition = afterPos;
            float afterScale = GetScaleByModelIndex(i) + (GetScaleByModelIndex(nextSliderIndex) - GetScaleByModelIndex(i)) * sliderProgress;
            CardPoint.transform.localScale = new Vector3(afterScale, afterScale, 1);
            //ParticleScaler[] particleScalerArray = CardPoint.transform.GetComponentsInChildren<ParticleScaler>();
            //for (int j = 0; j < particleScalerArray.Length; j++)
            //{
            //    particleScalerArray[j].particleScale = 0.2f * afterScale;
            //}
        }
        CheckResetModelPos(sliderRight);
    }
    /// <summary>
    /// 计算椭圆位置
    /// </summary>
    /// <param name="i">序号</param>
    /// <param name="isSliderRight">是否右滑</param>
    /// <param name="afterPos">待设置的卡牌的位置</param>
    /// <param name="sliderProgress">拖拽进度</param>
    /// <returns></returns>
    private Vector2 CalculateEllipsePos(int i, bool isSliderRight, Vector2 afterPos, float sliderProgress)
    {
        //经计算得出该椭圆公式：x^2/a^2+y^2/b^2 = 1 (其中a = 620,b = 145)
        int a = 620;
        int b = 145;
        if (isSliderRight)
        {
            if (i == 1 || i == 0 || i == 4 || i == 5)
            {
                afterPos.x *= Mathf.Pow(sliderProgress, 1 / 5);
                afterPos.y = Mathf.Sqrt((1 - Mathf.Pow(afterPos.x, 2) / Mathf.Pow(a, 2)) * Mathf.Pow(b, 2));
                afterPos.y *= (i == 1 || i == 0) ? -1 : 1;
            }
            else
            {
                afterPos.y *= Mathf.Pow(sliderProgress, 1 / 5);
                afterPos.x = Mathf.Sqrt((1 - Mathf.Pow(afterPos.y, 2) / Mathf.Pow(b, 2)) * Mathf.Pow(a, 2));
                afterPos.x *= i == 3 ? -1 : 1;
            }
        }
        else
        {
            if (i == 2 || i == 0 || i == 3 || i == 5)
            {
                afterPos.x *= Mathf.Pow(sliderProgress, 1 / 5);
                afterPos.y = Mathf.Sqrt((1 - Mathf.Pow(afterPos.x, 2) / Mathf.Pow(a, 2)) * Mathf.Pow(b, 2));
                afterPos.y *= (i == 2 || i == 0) ? -1 : 1;
            }
            else
            {
                afterPos.y *= Mathf.Pow(sliderProgress, 1 / 5);
                afterPos.x = Mathf.Sqrt((1 - Mathf.Pow(afterPos.y, 2) / Mathf.Pow(b, 2)) * Mathf.Pow(a, 2));
                afterPos.x *= i == 1 ? -1 : 1;
            }
        }
        return afterPos;
    }
    /// <summary>
    /// 获取自动旋转进度
    /// </summary>
    /// <param name="i">序号</param>
    /// <param name="sliderRight">是否右滑</param>
    /// <param name="afterPos">待设置的卡牌的位置</param>
    /// <param name="nextPos">下一个序号的位置</param>
    /// <param name="oriPos">上一个序号的位置</param>
    /// <returns></returns>
    private float GetAutoRotateProgress(int i, bool sliderRight, Vector2 afterPos, Vector2 nextPos, Vector2 oriPos)
    {
        if (sliderRight)
        {
            if (i == 1 || i == 0 || i == 4 || i == 5)
                return (afterPos.x - oriPos.x) / (nextPos.x - oriPos.x);
            else
                return (afterPos.y - oriPos.y) / (nextPos.y - oriPos.y);
        }
        else
        {
            if (i == 2 || i == 0 || i == 3 || i == 5)
                return (afterPos.x - oriPos.x) / (nextPos.x - oriPos.x);
            else
                return (afterPos.y - oriPos.y) / (nextPos.y - oriPos.y);
        }
    }

    private void ShowModelEffect()
    {
        GameObject ModelPoint = f_GetObject("ModelPoint");
        for (int i = 0; i < 6; i++)
        {
            Transform CardPoint = ModelPoint.transform.Find("Model" + i);
            Transform model = CardPoint.Find("Model");
            if (model != null)
            {
                GameObject magic = null;
                if (model.Find("magic") != null)
                {
                    magic = model.Find("magic").gameObject;

                }
                int order = model.GetComponent<Renderer>().sortingOrder - 1;
                UITool.f_CreateMagicById((int)EM_MagicId.eRoleBottomLine, ref magic, model, order, null);
                magic.name = "magic";
                magic.transform.GetComponent<SkeletonAnimation>().state.SetAnimation(0, "animation", true);
                magic.transform.localScale = new Vector3(60 / model.localScale.x, 60 / model.transform.localScale.y,
                    60 / model.transform.localScale.z);
                magic.SetActive(true);
            }

        }
    }

    /// <summary>
    /// 重设模型位置坐标和缩放
    /// </summary>
    private void ResetModelPos()
    {
        GameObject ModelPoint = f_GetObject("ModelPoint");
        isRePositionModel = false;
        for (int i = 0; i < 6; i++)
        {
            Transform CardPoint = ModelPoint.transform.Find("Model" + i);
            Vector2 pos = GetPositionByIndex(i);
            CardPoint.localPosition = new Vector3(pos.x, pos.y, 0);
            float scale = GetScaleByModelIndex(i);
            CardPoint.localScale = new Vector3(scale, scale, 1);
        }
        ResetModelSortingLayerOrder();
        ShowModelEffect();
    }
    private void ResetModelSortingLayerOrder()
    {
        GameObject ModelPoint = f_GetObject("ModelPoint");
        isRePositionModel = false;
        for (int i = 0; i < 6; i++)
        {
            Transform CardPoint = ModelPoint.transform.Find("Model" + i);
            if (CardPoint.Find("Model") != null)
            {
                CardPoint.Find("Model").GetComponent<Renderer>().sortingOrder = (6 - i) * 2;
                CardPoint.Find("NameRoot").GetComponent<UIPanel>().sortingOrder = (6 - i) * 2 + 1;
            }
            CardPoint.Find("TouchEnter").GetComponent<UIPanel>().sortingOrder = (6 - i) * 2 + 1;
        }
    }
    /// <summary>
    /// 检测模型位置
    /// 拖拽结束后重设模型坐标位置
    /// </summary>
    /// <param name="isSliderRight"></param>
    private void CheckResetModelPos(bool isSliderRight)
    {
        GameObject ModelPoint = f_GetObject("ModelPoint");
        Transform CardPoint0 = ModelPoint.transform.Find("Model0");
        Vector2 desPos = GetPositionByIndex(isSliderRight ? 2 : 1);
        if (isSliderRight)
        {
            if (CardPoint0.transform.localPosition.x >= desPos.x)
            {
                CheckPosCross(isSliderRight);
                ResetModelPos();
            }
        }
        else
        {
            if (CardPoint0.transform.localPosition.x <= desPos.x)
            {
                CheckPosCross(isSliderRight);
                ResetModelPos();
            }
        }
    }
    /// <summary>
    /// 判断越界的情况
    /// </summary>
    /// <param name="isSliderRight">是否往右滑动</param>
    /// <param name="isForceChange">是否强制改变名称</param>
    private void CheckPosCross(bool isSliderRight)
    {
        GameObject ModelPoint = f_GetObject("ModelPoint");
        Transform CardPoint0 = ModelPoint.transform.Find("Model0");
        int nextSliderIndex = GetNexSliderIndex(0, isSliderRight);
        Vector2 nextPos = GetPositionByIndex(nextSliderIndex);
        if (isSliderRight)
        {
            if (CardPoint0.transform.localPosition.x >= nextPos.x)
            {
                //重置初始化鼠标坐标，重置命名
                StartTouchPos = Input.mousePosition;
                ModelPoint.transform.Find("Model1").gameObject.name = "Model0";
                ModelPoint.transform.Find("Model3").gameObject.name = "Model1";
                ModelPoint.transform.Find("Model5").gameObject.name = "Model3";
                ModelPoint.transform.Find("Model4").gameObject.name = "Model5";
                ModelPoint.transform.Find("Model2").gameObject.name = "Model4";
                CardPoint0.gameObject.name = "Model2";
                ResetModelSortingLayerOrder();
            }
        }
        else
        {
            if (CardPoint0.transform.localPosition.x <= nextPos.x)
            {
                //重置初始化鼠标坐标，重置命名
                StartTouchPos = Input.mousePosition;
                ModelPoint.transform.Find("Model2").gameObject.name = "Model0";
                ModelPoint.transform.Find("Model4").gameObject.name = "Model2";
                ModelPoint.transform.Find("Model5").gameObject.name = "Model4";
                ModelPoint.transform.Find("Model3").gameObject.name = "Model5";
                ModelPoint.transform.Find("Model1").gameObject.name = "Model3";
                CardPoint0.gameObject.name = "Model1";
                ResetModelSortingLayerOrder();
            }
        }
    }
    /// <summary>
    /// 获取滑动的下一个序号
    /// </summary>
    /// <param name="currentIndex">当前序号</param>
    /// <param name="isSliderRight">是否往右滑动</param>
    /// <returns></returns>
    private int GetNexSliderIndex(int currentIndex, bool isSliderRight)
    {
        switch (currentIndex)
        {
            case 0: return isSliderRight ? 2 : 1;
            case 1: return isSliderRight ? 0 : 3;
            case 3: return isSliderRight ? 1 : 5;
            case 5: return isSliderRight ? 3 : 4;
            case 4: return isSliderRight ? 5 : 2;
            case 2: return isSliderRight ? 4 : 0;
            default:
                MessageBox.ASSERT(CommonTools.f_GetTransLanguage(45));
                return 0;
        }
    }
    /// <summary>
    /// 获取对应模型序号的坐标
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private Vector2 GetPositionByIndex(int index)
    {
        switch (index)
        {
            case 0: return new Vector2(0, -145);
            case 1: return new Vector2(-570, -57.047261f);//57.047261
            case 2: return new Vector2(570, -57.047261f);
            case 3: return new Vector2(-345, 120.477602f);//120.477602
            case 4: return new Vector2(345, 120.477602f);
            case 5: return new Vector2(0, 145);
            default:
                MessageBox.ASSERT(CommonTools.f_GetTransLanguage(44));
                return new Vector2(0, 0);
        }
    }
    #endregion
    #region 领奖中心
    void _AwardCenter()
    {
        Special(null);
        Data_Pool.m_AwardPool.f_AwardCenter(Special);

    }
    void Special(object obj)
    {
        if (Data_Pool.m_AwardPool.f_GetAll() == null)
            return;
        f_GetObject("BtnAwardCenter").gameObject.SetActive(Data_Pool.m_AwardPool.f_GetAll().Count > 0);
        f_GetObject("TopRightGrid").GetComponent<UIGrid>().Reposition();
    }
    #endregion

    private int timeEventId;
    /// <summary>
    /// 如果存在没处理充值单 就弹出提示
    /// </summary>
    private void f_TipUnsettledPay(object value)
    {
        timeEventId = 0;
        if (Data_Pool.m_GuidancePool.IsEnter)
        {
            timeEventId = ccTimeEvent.GetInstance().f_RegEvent(10, true, null, (object obj) =>
            {
                if (Data_Pool.m_GuidancePool.IsEnter) return;
                glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_UnsettledPay_Tip);
                ccTimeEvent.GetInstance().f_UnRegEvent(timeEventId);
            });
            return;
        }
        if (Data_Pool.m_RechargePool.UnsettledPayList.Count > 0)
        {
            PopupMenuParams tParam = new PopupMenuParams(CommonTools.f_GetTransLanguage(45), string.Format(CommonTools.f_GetTransLanguage(46), Data_Pool.m_RechargePool.UnsettledPayList.Count), CommonTools.f_GetTransLanguage(47), f_ProcessUnsettledPay);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParam);
        }
    }

    private void SetIcon()
    {
        return;
        string[] tIcon = new string[] {
           "0", "0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0"
        };
        NBaseSCDT tNBaseSCDT = new NBaseSCDT();
#if VER1
        tNBaseSCDT= glo_Main.GetInstance().m_SC_Pool.m_VERManageSC.f_GetSC(101);
#elif VER2
        tNBaseSCDT= glo_Main.GetInstance().m_SC_Pool.m_VERManageSC.f_GetSC(102);
#elif VER3
        tNBaseSCDT= glo_Main.GetInstance().m_SC_Pool.m_VERManageSC.f_GetSC(103);
#elif VER4
        tNBaseSCDT= glo_Main.GetInstance().m_SC_Pool.m_VERManageSC.f_GetSC(104);
#elif VER5
        tNBaseSCDT= glo_Main.GetInstance().m_SC_Pool.m_VERManageSC.f_GetSC(105);
#elif VER6
        tNBaseSCDT= glo_Main.GetInstance().m_SC_Pool.m_VERManageSC.f_GetSC(106);
#elif VER7
        tNBaseSCDT= glo_Main.GetInstance().m_SC_Pool.m_VERManageSC.f_GetSC(107);
#elif VER8
        tNBaseSCDT= glo_Main.GetInstance().m_SC_Pool.m_VERManageSC.f_GetSC(108);
#elif VER9
        tNBaseSCDT= glo_Main.GetInstance().m_SC_Pool.m_VERManageSC.f_GetSC(109);
#endif
        VERManageDT tVERManageDT = tNBaseSCDT as VERManageDT;
        if (tVERManageDT != null)
            if (tVERManageDT.szOpenIcon != string.Empty)
                tIcon = tVERManageDT.szOpenIcon.Split(';');

        for (int i = 0; i < OpenIcon.Length; i++)
        {
            if (i >= tIcon.Length)
            {
                break;
            }

            if (tIcon[i] == "0")
                OpenIcon[i].SetActive(false);
            else
                OpenIcon[i].SetActive(true);
        }

    }

    /// <summary>
    /// 处理 未处理充值单
    /// </summary>
    /// <param name="value"></param>
    private void f_ProcessUnsettledPay(object value)
    {
        Data_Pool.m_RechargePool.f_StartProcessUnsettledPay();
    }


    private void _ShowGameNotice(object obj)
    {
        if (obj == null)
        {
            if (Data_Pool.m_GameNoticePool.f_GetAll().Count > 0)
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.GameNoticePage, UIMessageDef.UI_OPEN, Data_Pool.m_GameNoticePool.f_GetAll()[0]);
            }
        }
        else
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GameNoticePage, UIMessageDef.UI_OPEN, obj);
        }

    }

    private void NextDay(object obj)
    {
        Data_Pool.m_NewYearActivityPool.m_bFesticalIsOpen = false;
        _AwardCenter();
    }
    #region 主线任务相关
    /// <summary>
    /// 主线任务初始化
    /// </summary>
    public void MainTaskInit(object data = null)
    {
        int playerLevel = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        Debug.Log(playerLevel);
        TaskDailyDT NextTaskDaily = f_GetNextMainTaskId(Data_Pool.m_TaskDailyPool.mMainTaskId);
        if (Data_Pool.m_TaskDailyPool.mMainTaskId <= 0)//主线任务未初始化(首次进入)
        {
            FirstEnterTask();
        }
        else
        {
            TaskDailyDT mCurTaskDaily = glo_Main.GetInstance().m_SC_Pool.m_TaskDailySC.f_GetSC(Data_Pool.m_TaskDailyPool.mMainTaskId) as TaskDailyDT;
            f_GetObject("MainTaskContent").SetActive(true);

            f_GetObject("LabelCurTaskText").GetComponent<UILabel>().text = mCurTaskDaily.szDesc.Replace(GameParamConst.ReplaceFlag, mCurTaskDaily.iConditionParam.ToString())
                + "(" + Data_Pool.m_TaskDailyPool.mMainTaskValue + "/" + mCurTaskDaily.iConditionParam + ")";

            if (Data_Pool.m_TaskDailyPool.mMainTaskValue >= mCurTaskDaily.iConditionParam)//已完成任务
            {
                TaskComplete(mCurTaskDaily, NextTaskDaily);
            }
            else//未完成任务
            {
                TaskUnComplete(mCurTaskDaily, NextTaskDaily);
            }
        }
    }
    private void TaskUnComplete(TaskDailyDT mCurTaskDaily, TaskDailyDT NextTaskDaily)//未完成任务
    {
        int playerLevel = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        bool isOpen = playerLevel >= mCurTaskDaily.iOpenLv;
        if (isOpen)//等级已达到
        {
            SetMainTaskState(EM_MainTaskState.Doing, mCurTaskDaily.iAwardMoney);
            f_RegClickEvent("BtnTaskGoto", UI_BtnTaskGoto, mCurTaskDaily);
        }
        else
        {
            f_GetObject("LabelMainTaskLockHint").GetComponent<UILabel>().text = "（" + string.Format(CommonTools.f_GetTransLanguage(8), NextTaskDaily.iOpenLv) + "）";
            SetMainTaskState(EM_MainTaskState.Lock, NextTaskDaily.iOpenLv);
        }
    }
    private TaskDailyDT mCurTaskDailyTemp;
    private void TaskComplete(TaskDailyDT mCurTaskDaily, TaskDailyDT NextTaskDaily)//已完成任务
    {
        int playerLevel = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        if (Data_Pool.m_TaskDailyPool.mMainTaskAwardTimes <= 0)//未领取奖励
        {
            SetMainTaskState(EM_MainTaskState.CanGetAward, mCurTaskDaily.iAwardMoney);
            mCurTaskDailyTemp = mCurTaskDaily;
            f_RegClickEvent("BtnTaskGet", UI_BtnTaskGet);
        }
        else//已领取奖励，初始化下一个副本
        {
            if (NextTaskDaily == null)//所有任务已完成
            {
                f_GetObject("MainTaskContent").SetActive(false);
                return;
            }
            bool isOpen = playerLevel >= NextTaskDaily.iOpenLv;
            if (isOpen)//等级已达到
            {
                SetMainTaskState(EM_MainTaskState.Doing, NextTaskDaily.iAwardMoney);
                f_InitMainTask(NextTaskDaily.iId);
            }
            else
            {
                f_GetObject("LabelCurTaskText").GetComponent<UILabel>().text = NextTaskDaily.szDesc.Replace(GameParamConst.ReplaceFlag, NextTaskDaily.iConditionParam.ToString())
                    + "(0/" + NextTaskDaily.iConditionParam + ")";
                f_GetObject("LabelMainTaskLockHint").GetComponent<UILabel>().text = "（" + string.Format(CommonTools.f_GetTransLanguage(8), NextTaskDaily.iOpenLv) + "）";
                SetMainTaskState(EM_MainTaskState.Lock, NextTaskDaily.iAwardMoney, NextTaskDaily.iOpenLv);
            }
        }
    }
    private void FirstEnterTask()//首次初始化任务
    {
        int playerLevel = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        TaskDailyDT NextTaskDaily = f_GetNextMainTaskId(Data_Pool.m_TaskDailyPool.mMainTaskId);
        f_GetObject("MainTaskContent").SetActive(true);
        f_GetObject("LabelCurTaskText").GetComponent<UILabel>().text = NextTaskDaily.szDesc.Replace(GameParamConst.ReplaceFlag, NextTaskDaily.iConditionParam.ToString())
            + "(0/" + NextTaskDaily.iConditionParam + ")";
        bool isOpen = playerLevel >= NextTaskDaily.iOpenLv;
        if (isOpen)//等级已达到
        {
            SetMainTaskState(EM_MainTaskState.Doing, NextTaskDaily.iAwardMoney);
            f_InitMainTask(NextTaskDaily.iId);
        }
        else
        {
            f_GetObject("LabelMainTaskLockHint").GetComponent<UILabel>().text = "（" + string.Format(CommonTools.f_GetTransLanguage(8), NextTaskDaily.iOpenLv) + "）";
            SetMainTaskState(EM_MainTaskState.Lock, NextTaskDaily.iAwardMoney, NextTaskDaily.iOpenLv);
        }
    }
    /// <summary>
    /// 主线任务前往
    /// </summary>
    public void UI_BtnTaskGoto(GameObject go, object obj1, object obj2)
    {
        TaskDailyDT mCurTaskDaily = (TaskDailyDT)obj1;
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "点击前往");
        UITool.f_DailyGoto(this, (EM_DailyTaskCondition)mCurTaskDaily.iCondition);
    }

    /// <summary>
    /// 主线任务领取奖励
    /// </summary>
    public void UI_BtnTaskGet(GameObject go, object obj1, object obj2)
    {
        SocketCallbackDT callbackDT = new SocketCallbackDT();
        callbackDT.m_ccCallbackSuc = f_TaskMainGetAwardSuc;
        callbackDT.m_ccCallbackFail = f_TaskMainGetAwardFail;
        Data_Pool.m_TaskDailyPool.f_TaskTaskMainAward(callbackDT);
    }
    private void SetMainTaskState(EM_MainTaskState emMainTaskState, int awardMoneyCount, int openLevel = 0)
    {
        f_GetObject("LabelMainTaskLockHint").SetActive(emMainTaskState == EM_MainTaskState.Lock);
        f_GetObject("BtnTaskGoto").SetActive(emMainTaskState == EM_MainTaskState.Doing);
        f_GetObject("BtnTaskGet").SetActive(emMainTaskState == EM_MainTaskState.CanGetAward);
        f_GetObject("LabelMainTaskLockHint").GetComponent<UILabel>().text = "（" + string.Format(CommonTools.f_GetTransLanguage(8), openLevel) + "）";

        ResourceCommonDT dt = new ResourceCommonDT();
        dt.f_UpdateInfo((byte)EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_Money, awardMoneyCount);
        string goodName = dt.mName;
        f_GetObject("MainTaskAward").transform.Find("IconBorder").GetComponent<UISprite>().spriteName = UITool.f_GetImporentColorName(dt.mImportant, ref goodName);
        f_GetObject("MainTaskAward").transform.Find("LabelCount").GetComponent<UILabel>().text = dt.mResourceNum.ToString();
        UITool.f_SetIconSprite(f_GetObject("MainTaskAward").transform.Find("Icon").GetComponent<UI2DSprite>(), EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_Money);

    }
    private void f_TaskMainGetAwardSuc(object data)
    {
        List<AwardPoolDT> awardList = new List<AwardPoolDT>();
        AwardPoolDT item1 = new AwardPoolDT();
        item1.f_UpdateByInfo((byte)EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_Money, mCurTaskDailyTemp.iAwardMoney);
        awardList.Add(item1);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { awardList });
    }
    private void f_TaskMainGetAwardFail(object data)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(48));
    }
    private void f_InitMainTask(int taskId)
    {
        SocketCallbackDT callbackDT = new SocketCallbackDT();
        callbackDT.m_ccCallbackSuc = f_TaskMainInitSuc;
        callbackDT.m_ccCallbackFail = f_TaskMainInitFail;
        Data_Pool.m_TaskDailyPool.f_TaskMainInit(taskId, callbackDT);
    }
    private void f_TaskMainInitSuc(object data)
    {
        MainTaskInit();
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "主线任务初始化成功");
    }
	
    private void f_TaskMainInitFail(object data)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(49));
    }
    /// <summary>
    /// 返回主线任务的下一个任务
    /// </summary>
    /// <param name="curMainTaskId"></param>
    /// <returns></returns>
    private TaskDailyDT f_GetNextMainTaskId(int curMainTaskId)
    {
        List<NBaseSCDT> tmp = glo_Main.GetInstance().m_SC_Pool.m_TaskDailySC.f_GetAll();
        bool FindTaskId = false;
        for (int i = 0; i < tmp.Count; i++)
        {
            TaskDailyDT tmpItem = tmp[i] as TaskDailyDT;
            if (tmpItem.iTaskType == 2)//主线任务
            {
                if (curMainTaskId <= 0)//主线任务未初始化,返回第一个
                    return tmpItem;
                else
                {
                    if (tmpItem.iId == curMainTaskId)
                        FindTaskId = true;
                    else if (FindTaskId)
                        return tmpItem;
                }
            }
        }
        return null;
    }
    #endregion
    #region 服务器广播消息相关
    private bool isOpenBroadcastText = false;
    private void PlayReverseTime(object data)
    {
        if (null == f_GetObject("BroadcastTextRoot"))
        {
            return;
        }
        Transform BroadcastTextRoot = f_GetObject("BroadcastTextRoot").transform;

        if (BroadcastTextRoot.childCount <= 0 && isOpenBroadcastText)
        {
            isOpenBroadcastText = false;
            Transform BroadcastBg = f_GetObject("BroadcastBg").transform;
            TweenScale ts = BroadcastBg.GetComponent<TweenScale>();
            ts.enabled = true;
            BroadcastBg.localScale = ts.to;
            ts.PlayReverse();
            ts.ResetToBeginning();
            ts.PlayReverse();
        }
    }
    private void DestoryBroadcastRootChild()
    {
        Transform BroadcastTextRoot = f_GetObject("BroadcastTextRoot").transform;
        for (int i = BroadcastTextRoot.childCount - 1; i >= 0; i--)//如有新消息，清楚之前的数据
        {
            GameObject.DestroyImmediate(BroadcastTextRoot.transform.GetChild(i).gameObject);
        }
    }
    /// <summary>
    /// 检测有没有广播详细
    /// </summary>
    private void BroadcastCheck(object data)
    {
        if (this.f_IsOpen())
        {
            if (Data_Pool.m_BroadcastPool.f_Count() > 0)
            {
                if (null == f_GetObject("BroadcastTextRoot"))
                {
                    return;
                }
                Transform BroadcastTextRoot = f_GetObject("BroadcastTextRoot").transform;
                float borderHalfLength = 570f;//聊天背景一半的长度
                float playEndTime = 0f;//显示结束时间
                List<BasePoolDT<long>> listData = Data_Pool.m_BroadcastPool.f_GetAll();
                TweenPosition tpMaxTime = null;
                for (int i = 0; i < listData.Count; i++)
                {
                    BroadcastPoolDT poolDT = listData[i] as BroadcastPoolDT;
                    float childPosition = 0;
                    if (BroadcastTextRoot.childCount > 0)
                    {
                        Transform lastChild = BroadcastTextRoot.GetChild(BroadcastTextRoot.childCount - 1);
                        float lastTransormLength = lastChild.GetComponent<UILabel>().width;
                        childPosition = lastChild.localPosition.x + lastTransormLength;
                    }
                    float NewTestPos = childPosition + 80;

                    //1.设置文字
                    //BasePlayerPoolDT basePlayerPoolDT = Data_Pool.m_GeneralPlayerPool.f_GetForId(poolDT.userid) as BasePlayerPoolDT;
                    //if (basePlayerPoolDT == null)
                    //    continue;

                    NewTestPos = NewTestPos < borderHalfLength ? borderHalfLength : NewTestPos;
                    GameObject textNewTextObj = GameObject.Instantiate(f_GetObject("BroadcastText")) as GameObject;
                    textNewTextObj.transform.SetParent(BroadcastTextRoot.transform);
                    textNewTextObj.transform.localPosition = new Vector3(NewTestPos, 0, 0);
                    textNewTextObj.transform.localEulerAngles = Vector3.zero;
                    textNewTextObj.transform.localScale = Vector3.one;
                    textNewTextObj.gameObject.SetActive(true);
                    // move to CommonTool
                    //if (poolDT.opType == (int)SocketCommand.CS_TurntableLottery)
                    //{
                    //    textNewTextObj.GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(2231), poolDT.szName, poolDT.cardTempId);
                    //}
                    //else
                    //{
                    //    ResourceCommonDT commonDT = new ResourceCommonDT();
                    //    commonDT.f_UpdateInfo((byte)EM_ResourceType.Card, poolDT.cardTempId, 1);
                    //    string cardName = commonDT.mName;
                    //    UITool.f_GetImporentColorName(commonDT.mImportant, ref cardName);
                    //    textNewTextObj.GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(9), poolDT.szName, UITool.GetGetWayBySocketId(poolDT.opType), UITool.f_GetImportantColorName((EM_Important)commonDT.mImportant), cardName);
                    //}

                    textNewTextObj.GetComponent<UILabel>().text = CommonTools.GetBroadCastMess(poolDT.szName, (int)poolDT.opType, poolDT.cardTempId);

                    TweenPosition tp = textNewTextObj.GetComponent<TweenPosition>();
                    tp.from = textNewTextObj.transform.localPosition;
                    tp.to = new Vector3(-borderHalfLength - textNewTextObj.GetComponent<UILabel>().width, 0, 0);
                    tp.duration = (NewTestPos + Mathf.Abs(tp.to.x)) * 1.0f / 100;
                    EventDelegate ed = new EventDelegate(this, "OnDestroyTextSellf");
                    ed.parameters[0].obj = textNewTextObj;
                    tp.AddOnFinished(ed);
                    if (tp.duration > playEndTime)
                    {
                        playEndTime = tp.duration;
                        tpMaxTime = tp;
                    }
                }
                if (tpMaxTime != null)
                    tpMaxTime.AddOnFinished(OnTextTweenEndCallback);
                if (!isOpenBroadcastText)
                {
                    isOpenBroadcastText = true;
                    Transform BroadcastBg = f_GetObject("BroadcastBg").transform;
                    TweenScale ts = BroadcastBg.GetComponent<TweenScale>();
                    ts.enabled = true;
                    BroadcastBg.localScale = ts.from;
                    ts.PlayForward();
                    ts.ResetToBeginning();
                    ts.PlayForward();
                }
                Data_Pool.m_BroadcastPool.f_Clear();
            }
            PlayReverseTime(null);
        }
    }

    private void UI_UpdateSevenBtn(object obj)
    {
        f_GetObject("Seven").SetActive(false);
    }
    private void OnDestroyTextSellf(GameObject obj)
    {
        GameObject.DestroyImmediate(obj);
    }
    private void OnTextTweenEndCallback()
    {
        PlayReverseTime(null);
    }

    #endregion

    #region 服务器跑马灯广播消息相关
    private bool isOpenBroadcastTextServerNotice = false;
    private void PlayReverseTimeServerNotice(object data)
    {
        if (null == f_GetObject("ServerBroadcastTextRoot"))
        {
            return;
        }
        Transform BroadcastTextRoot = f_GetObject("ServerBroadcastTextRoot").transform;

        if (BroadcastTextRoot.childCount <= 0 && isOpenBroadcastTextServerNotice)
        {
            isOpenBroadcastTextServerNotice = false;
            Transform BroadcastBg = f_GetObject("ServerBroadcastBg").transform;
            TweenScale ts = BroadcastBg.GetComponent<TweenScale>();
            ts.enabled = true;
            BroadcastBg.localScale = ts.to;
            ts.PlayReverse();
            ts.ResetToBeginning();
            ts.PlayReverse();
        }
    }
    private void DestoryBroadcastRootChildServerNotice()
    {
        Transform BroadcastTextRoot = f_GetObject("ServerBroadcastTextRoot").transform;
        for (int i = BroadcastTextRoot.childCount - 1; i >= 0; i--)//如有新消息，清楚之前的数据
        {
            GameObject.DestroyImmediate(BroadcastTextRoot.transform.GetChild(i).gameObject);
        }
    }
    /// <summary>
    /// 检测有没有广播详细
    /// </summary>
    private void BroadcastCheckServerNotice(object data)
    {
        if (this.f_IsOpen())
        {
            if (Data_Pool.m_BroadcastNoticePool.NeedShowNewMessage())
            {
                if (null == f_GetObject("ServerBroadcastTextRoot"))
                {
                    return;
                }
                Transform BroadcastTextRoot = f_GetObject("ServerBroadcastTextRoot").transform;
                float borderHalfLength = 570f;//聊天背景一半的长度
                float playEndTime = 0f;//显示结束时间
                List<BasePoolDT<long>> listData = Data_Pool.m_BroadcastNoticePool.f_GetAll();
                TweenPosition tpMaxTime = null;
                for (int i = 0; i < listData.Count; i++)
                {
                    BroadcastNoticePoolDT poolDT = listData[i] as BroadcastNoticePoolDT;
                    float childPosition = 0;
                    if (BroadcastTextRoot.childCount > 0)
                    {
                        Transform lastChild = BroadcastTextRoot.GetChild(BroadcastTextRoot.childCount - 1);
                        float lastTransormLength = lastChild.GetComponent<UILabel>().width;
                        childPosition = lastChild.localPosition.x + lastTransormLength;
                    }
                    float NewTestPos = childPosition + 80;

                    //时间未到，不播放此条消息
                    int gameserverTime = GameSocket.GetInstance().f_GetServerTime();
                    if (gameserverTime > poolDT.uOverTime || gameserverTime < poolDT.uStartTime)
                        continue;
                    //间隔时间未到，不播放此条消息
                    if (gameserverTime - poolDT.lastShowTime < poolDT.uShowDeleTime)
                        continue;
                    poolDT.lastShowTime = gameserverTime;

                    NewTestPos = NewTestPos < borderHalfLength ? borderHalfLength : NewTestPos;
                    GameObject textNewTextObj = GameObject.Instantiate(f_GetObject("ServerBroadcastText")) as GameObject;
                    textNewTextObj.transform.SetParent(BroadcastTextRoot.transform);
                    textNewTextObj.transform.localPosition = new Vector3(NewTestPos, 0, 0);
                    textNewTextObj.transform.localEulerAngles = Vector3.zero;
                    textNewTextObj.transform.localScale = Vector3.one;
                    textNewTextObj.gameObject.SetActive(true);

                    textNewTextObj.GetComponent<UILabel>().text = poolDT.szContext;

                    TweenPosition tp = textNewTextObj.GetComponent<TweenPosition>();
                    tp.from = textNewTextObj.transform.localPosition;
                    tp.to = new Vector3(-borderHalfLength - textNewTextObj.GetComponent<UILabel>().width, 0, 0);
                    tp.duration = (NewTestPos + Mathf.Abs(tp.to.x)) * 1.0f / 100;
                    EventDelegate ed = new EventDelegate(this, "OnDestroyTextSellfServerNotice");
                    ed.parameters[0].obj = textNewTextObj;
                    tp.AddOnFinished(ed);
                    if (tp.duration > playEndTime)
                    {
                        playEndTime = tp.duration;
                        tpMaxTime = tp;
                    }
                    break;
                }
                if (tpMaxTime != null)
                    tpMaxTime.AddOnFinished(OnTextTweenEndCallbackServerNotice);
                if (!isOpenBroadcastTextServerNotice)
                {
                    isOpenBroadcastTextServerNotice = true;
                    Transform BroadcastBg = f_GetObject("ServerBroadcastBg").transform;
                    TweenScale ts = BroadcastBg.GetComponent<TweenScale>();
                    ts.enabled = true;
                    BroadcastBg.localScale = ts.from;
                    ts.PlayForward();
                    ts.ResetToBeginning();
                    ts.PlayForward();
                }
            }
            PlayReverseTimeServerNotice(null);
        }
    }
    private void OnDestroyTextSellfServerNotice(GameObject obj)
    {
        GameObject.DestroyImmediate(obj);
    }
    private void OnTextTweenEndCallbackServerNotice()
    {
        PlayReverseTimeServerNotice(null);
    }
    #endregion


    #region  新年活动相关

    private void _setBtn()
    {
        int curLv = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);

        GameParamDT gameParamDTOpenLevel = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.NewYearOpenLevel) as GameParamDT);
        f_GetObject("BtnNewYear").SetActive(NewYearActPage.f_IsOpenNewYearAct() &&
                                            (curLv >= gameParamDTOpenLevel.iParam1));

        f_GetObject("Btn_Destiny").SetActive(UITool.f_GetIsOpensystem(EM_NeedLevel.BattleFormLevel));

        GameParamDT gameParam = glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_NeedLevel.LegionLevel) as GameParamDT;
        int showLv = null == gameParam ? 999999 : gameParam.iParam2;
        int myLv = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        f_GetObject("Btn_Legion").SetActive(myLv >= showLv);


        f_GetObject("BtnTask").SetActive(curLv >= GameParamConst.TaskLvLimit);
        //f_GetObject("Btn_Destiny").transform.parent.GetComponent<UIGrid>().repositionNow = true;
        //My Code
        // if(ActivityOpen.iParam1 == 0)
        // {
        // f_GetObject("BtnActivity").SetActive(false);
        // f_GetObject("BtnTask").SetActive(false);
        // f_GetObject("Btn_Legion").SetActive(false);
        // }
        if (HotActivityOpen.iParam1 == 0)
        {
            f_GetObject("BtnNewYear").SetActive(false);
        }
        // if(ActivityOpen.iParam2 == 0)
        // {
        // f_GetObject("BtnFirstRecharge").SetActive(false);
        // }
        f_GetObject("TopRightGrid").GetComponent<UIGrid>().Reposition();
        //
        GameParamDT gameParamDTAFK= (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(102) as GameParamDT);
        UITool.f_Set2DSpriteGray(f_GetObject("BtnAFK"), curLv < gameParamDTAFK.iParam2);
    }

    /// <summary>
    /// 新年活动主入口
    /// </summary>
    private void _NewYaerMain()
    {

        //if(!Data_Pool.m_NewYearActivityPool.m_bMammonIsOpenTime)
        //{
        //    return;
        //}

        _NewYearInfo();
    }
    private void _NewYearInfo()
    {
        if (!Data_Pool.m_NewYearActivityPool.m_bIsShowOnePage)
        {
            GameParamDT gameParamDTOpenLevel = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.NewYearOpenLevel) as GameParamDT);
            if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < gameParamDTOpenLevel.iParam1)
                return;
            if (Data_Pool.m_GuidancePool.IsEnter)
                return;

            MammonSendGiftDT tMammonSendGiftDT;
            DateTime tDateTime = ccMath.time_t2DateTime(GameSocket.GetInstance().f_GetServerTime());

            for (int i = 0; i < glo_Main.GetInstance().m_SC_Pool.m_MammonSendGiftSC.f_GetAll().Count; i++)
            {
                tMammonSendGiftDT = glo_Main.GetInstance().m_SC_Pool.m_MammonSendGiftSC.f_GetAll()[i] as MammonSendGiftDT;

                DateTime tActivityTime = ccMath.time_t2DateTime(ccMath.f_Data2Int(tMammonSendGiftDT.iStarTime));

                if (tActivityTime.Year == tDateTime.Year && tActivityTime.Month == tDateTime.Month && tActivityTime.Day == tDateTime.Day)
                {
                    UITool.f_OpenOrCloseWaitTip(true);
                    SocketCallbackDT tSocketCallbackDT = new SocketCallbackDT();
                    tSocketCallbackDT.m_ccCallbackSuc = _MammonSendGiftInfo;
                    tSocketCallbackDT.m_ccCallbackFail = _MammoinSendGiftFail;
                    Data_Pool.m_NewYearActivityPool.f_MammonSendGiftNewInfo(tSocketCallbackDT);
                    break;
                }
            }
        }

    }

    private void _MammonSendGiftInfo(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if (Data_Pool.m_NewYearActivityPool.m_MammonSendGiftArr.m_DTId == 0)
        {
            return;
        }
        //        if (!Data_Pool.m_NewYearActivityPool.m_bMammonSendGiftIsGet && !Data_Pool.m_NewYearActivityPool.m_bIsShowOnePage)
        //        {
        //            ccUIHoldPool.GetInstance().f_Hold(this);
        //            ccUIManage.GetInstance().f_SendMsg(UINameConst.NewYearActPage, UIMessageDef.UI_OPEN, EM_NewYearActType.MammonSendGiftsNew);
        //        }
    }

    private void _MammoinSendGiftFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(50) + obj.ToString());
    }

    #endregion

    #region  情人节活动

    private void _ValentinesInfo()
    {

        GameParamDT tGameParamDT = glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.ValentinesDayOpenTime) as GameParamDT;

        if (tGameParamDT == null)
        {
            f_GetObject("ValentinesDay").SetActive(false);
            MessageBox.DEBUG(CommonTools.f_GetTransLanguage(51));
            return;
        }
        int StarTime = ccMath.f_Data2Int(tGameParamDT.iParam1);
        int EndTime = ccMath.f_Data2Int(tGameParamDT.iParam2);

        if (GameSocket.GetInstance().f_GetServerTime() > StarTime && GameSocket.GetInstance().f_GetServerTime() < EndTime)
            f_GetObject("ValentinesDay").SetActive(true);
        else
            f_GetObject("ValentinesDay").SetActive(false);
    }
    private void _ValentinesDay(GameObject go, object obj1, object obj2)
    {

        GameParamDT gameParamDTOpenLevel = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.NewYearOpenLevel) as GameParamDT);
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < gameParamDTOpenLevel.iParam1)
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(4), gameParamDTOpenLevel.iParam1));
            return;
        }
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ValentinesDayPage, UIMessageDef.UI_OPEN);
    }
    #endregion

    #region 军团战相关

    private GameObject m_LegionBattleBtn;

    private void f_UpdateLegionBattleBtnState()
    {
        m_LegionBattleBtn.SetActive(LegionMain.GetInstance().m_LegionInfor.m_iLegionId != 0 && LegionMain.GetInstance().m_LegionBattlePool.f_IsInBattleTime());
    }

    public void f_OnLegionBattleBtnClick(GameObject go, object value1, object value2)
    {
        if (LegionMain.GetInstance().m_LegionInfor.m_iLegionId == 0)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(13));
            f_UpdateLegionBattleBtnState();
            return;
        }
        else if (!LegionMain.GetInstance().m_LegionBattlePool.f_IsInBattleTime())
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(14));
            f_UpdateLegionBattleBtnState();
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        LegionMain.GetInstance().m_LegionPlayerPool.f_ExecuteAfterLegionMemInit(f_ExecuteOpenLegionBalltePageAfterLegionMemInit);
    }

    private void f_ExecuteOpenLegionBalltePageAfterLegionMemInit(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            ccUIHoldPool.GetInstance().f_Hold(this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionBattlePage, UIMessageDef.UI_OPEN);
        }
        else
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(52) + result);
        }
    }

    #endregion

    #region 斗将主界面按钮
    private GameObject m_CardBattleBtn;

    private void f_OnCardBattleBtnClick(GameObject go, object value1, object value2)
    {
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CardBattlePage, UIMessageDef.UI_OPEN);
    }

    private void f_UpdateCardBattleBtnState()
    {
        CardBattlePool.EM_CardBattleState state = Data_Pool.m_CardBattlePool.f_GetState();
        m_CardBattleBtn.SetActive(state != CardBattlePool.EM_CardBattleState.Invalid);
    }
    #endregion

    #region 马甲包

    //private void SetBtn()
    //{

    //    string[] OpenIcon;



    //    //OpenIcon = UITool.f_GetGameParamDT(101).iParam1;
    //    //统一管理开启图标
    //    bool IsOpen = CommonTools.f_GetGameParamOpen(EM_GameParamType.V_ActivityOpen);

    //    f_GetObject("Activity").SetActive(IsOpen); //Huodong

    //    f_GetObject("FirstRecharge").SetActive(IsOpen);//首冲
    //    f_GetObject("BtnRankGift").SetActive(IsOpen);//等级礼包
    //    f_GetObject("CardBattleBtn").SetActive(IsOpen);//跨服斗将
    //    f_GetObject("BtnRankList").SetActive(IsOpen);//排行榜
    //}

    #endregion

    #region event time
    private void InitEventTime() {
        List<NBaseSCDT> _EventTimeSCList = Data_Pool.m_EventTimePool.GetEventTimeSCList();
		eventTimeCount = _EventTimeSCList.Count;

        for (int i = 0; i < _EventTimeSCList.Count; i++)
        {
            //CreatMainICon((EventTimeDT)_EventTimeSCList[i]);
            f_OPenEventTime(_EventTimeSCList[i].iId);
        }       
    }

    private void CreatMainICon(EventTimeDT eventTimeDT)
    {
        EventTimeDT ttttttttttttt = eventTimeDT;
        if (Data_Pool.m_EventTimePool.f_CheckOpenEventTime(ttttttttttttt.iId))
            return;
        GameObject Icon = glo_Main.GetInstance().m_ResourceManager.f_CreateEventTimeSystem(ttttttttttttt.szSystemId);
        switch (ttttttttttttt.iPosition)
        {
            case 1:
                Icon.transform.parent = f_GetObject("TopRightGrid").transform;
                f_GetObject("TopRightGrid").GetComponent<UIGrid>().Reposition();
                break;
            case 2:
                Icon.transform.parent = f_GetObject("TopRightGrid").transform;
                f_GetObject("TopRightGrid").GetComponent<UIGrid>().Reposition();
                break;
            case 3:
                Icon.transform.parent = f_GetObject("TopRightGrid").transform;
                f_GetObject("TopRightGrid").GetComponent<UIGrid>().Reposition();
                break;
        }
        Icon.transform.localScale = new Vector3(1, 1, 1);
        Icon.name = ttttttttttttt.iId.ToString();
		
		//My Code
		int nowTime = GameSocket.GetInstance().f_GetServerTime();
        int pastTime =  LocalDataManager.f_GetLocalData<int>(LocalDataType.EventTimeReddotReset);
		
		if(pastTime == null)
		{
			Icon.transform.Find("Reddot").gameObject.SetActive(true);
			reddotEventTimeCount++;
			if(reddotEventTimeCount == eventTimeCount)
			{
				LocalDataManager.f_SetLocalData<int>(LocalDataType.EventTimeReddotReset, nowTime);
				pastTime = LocalDataManager.f_GetLocalData<int>(LocalDataType.EventTimeReddotReset);
				reddotEventTimeCount = 0;
			}
		}
		else
		{
            if (!CommonTools.f_CheckSameDay(pastTime, GameSocket.GetInstance().f_GetServerTime()))
            {
                Icon.transform.Find("Reddot").gameObject.SetActive(true);
                reddotEventTimeCount++;
                if (reddotEventTimeCount == eventTimeCount)
                {
                    LocalDataManager.f_SetLocalData<int>(LocalDataType.EventTimeReddotReset, nowTime);
                    reddotEventTimeCount = 0;
                }
            }

		}
		
		// MessageBox.ASSERT("PastTime: " + pastYear + pastMonth + pastDay);
		// MessageBox.ASSERT("NowTime: " + nowYear + nowMonth + nowDay);
		MessageBox.ASSERT("PastTime: " + pastTime);
		MessageBox.ASSERT("NowTime: " + nowTime);
		//
		
        f_RegClickEvent(Icon, UI_EventTime, ttttttttttttt);
        Icon.GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite(ttttttttttttt.szIcon);
		Icon.GetComponent<UI2DSprite>().MakePixelPerfect();
        if (ttttttttttttt.szNameConst == "LevelGiftPage")
        {
            UITool.f_UpdateReddot(Icon, 1, new Vector3(30, 40, 0), 102);
        }
    }

    private void UI_EventTime(GameObject go, object obj1, object obj2)
    {

        EventTimeDT eventTimeDT = (EventTimeDT)obj1;
        //通知ccUIManager打开背包页
        if (eventTimeDT.iHold > 0)
        {
            ccUIHoldPool.GetInstance().f_Hold(this);
        }
        if (eventTimeDT.szNameConst == "RankingPowerPage" || eventTimeDT.szNameConst == "RankingGodEquipPage" || eventTimeDT.szNameConst == "RankingTariningPage")
        {
            ccUIManage.GetInstance().f_SendMsg("RankingPowerPage", UIMessageDef.UI_OPEN, obj1);
        }
        else
        {
            ccUIManage.GetInstance().f_SendMsg(eventTimeDT.szNameConst, UIMessageDef.UI_OPEN, obj1);
        }
		
		Transform Icon = GameObject.Find(eventTimeDT.iId.ToString()).transform;
		Icon.Find("Reddot").gameObject.SetActive(false);
		
       
        OnContentReverseFinish();
    }

    private  bool initIconOnlineVipAward;
    public UILabel m_TimeLeft;
    private int timeLeft;
    private void f_OnlineVipAward(object value)
    {
        EventOnlineVipInfo eventOnlineVipInfo = (EventOnlineVipInfo)value;
        Debug.Log("1111");
        if (eventOnlineVipInfo.ievent > 0)
        {
            if (!initIconOnlineVipAward)
            {
                GameObject Icon = glo_Main.GetInstance().m_ResourceManager.f_CreateEventOnlineVipSys();
                Icon.transform.parent = f_GetObject("TopRightGrid3").transform;
                f_GetObject("TopRightGrid3").GetComponent<UIGrid>().Reposition();
                Icon.transform.localScale = new Vector3(1, 1, 1);
                // Icon.GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("Icon_OnlineVipAward");
                Icon.name = "Icon_OnlineVipAward";
                f_RegClickEvent(Icon, UI_OnlineVipAwardOnClick);
                initIconOnlineVipAward = true;
            }
            GameObject Icon_OnlineVipAward = f_GetObject("TopRightGrid3").transform.Find("Icon_OnlineVipAward").gameObject;
            m_TimeLeft = Icon_OnlineVipAward.transform.Find("Labeltime").GetComponent<UILabel>();
            TimerControl(true);
            int timeSec = Data_Pool.m_OnlineAwardPool.m_timeSecondToday;
            for (int i = 0; i < glo_Main.GetInstance().m_SC_Pool.m_OnlineVipAwardSC.f_GetAll().Count; i++)
            {
                OnlineVipAwardDT onlineVipAwardDT = glo_Main.GetInstance().m_SC_Pool.m_OnlineVipAwardSC.f_GetAll()[i] as OnlineVipAwardDT;

                bool check = Data_Pool.m_OnlineVipAwardPool.CheckGetAward(onlineVipAwardDT.iId);
                int time = onlineVipAwardDT.iTime * 60;

                if (!check)
                {
                    timeLeft = time - timeSec;
                    Icon_OnlineVipAward.transform.Find("Labeltext").GetComponent<UILabel>().text = onlineVipAwardDT.szDescribe;
                    TimerControl(true);
                    break;
                }
            }

            if(eventOnlineVipInfo.idata1 > 0 && eventOnlineVipInfo.idata2 > 0)
            {
                Icon_OnlineVipAward.SetActive(false);
            }

        }
    }

    private void UI_OnlineVipAwardOnClick(GameObject go, object obj1, object obj2)
    {
        OnContentReverseFinish();
        //通知ccUIManager打开背包页
        ccUIManage.GetInstance().f_SendMsg(UINameConst.OnlineVipPage, UIMessageDef.UI_OPEN, obj1);
    }
	
	private void f_OnBtnCastleClick(GameObject go, object value1, object value2)
    {
        f_GetObject("CastlePanel").SetActive(true);
    }
    
    
    private void f_OnBtnCastleClose(GameObject go, object value1, object value2)
    {
        f_GetObject("CastlePanel").SetActive(false);
    }

    private void TimerControl(bool isStart)
    {
        CancelInvoke("ReduceTime");
        if (isStart)
        {
            InvokeRepeating("ReduceTime", 0f, 1f);
        }
    }

    private void ReduceTime()
    {
        timeLeft--;
        if (timeLeft <= 0)
        {
            TimerControl(false);
            m_TimeLeft.text = CommonTools.f_GetTransLanguage(795);
        }
        else
        {
            m_TimeLeft.text = "" + CommonTools.f_GetStringBySecond(timeLeft);
        }
    }

    private void f_CloseEventTime(object value)
    {
        int  id = (int)value;
        Transform Icon_Event = null;
        if(Icon_Event == null)
           Icon_Event = f_GetObject("TopRightGrid").transform.Find(id.ToString());
        if (Icon_Event == null)
            Icon_Event = f_GetObject("TopRightGrid").transform.Find(id.ToString());
        if (Icon_Event == null)
            Icon_Event = f_GetObject("TopRightGrid").transform.Find(id.ToString());

        if (Icon_Event == null) return;
        Icon_Event.gameObject.SetActive(false);
        f_GetObject("TopRightGrid").GetComponent<UIGrid>().Reposition();
        f_GetObject("TopRightGrid").GetComponent<UIGrid>().Reposition();
        f_GetObject("TopRightGrid").GetComponent<UIGrid>().Reposition();
    }

    private void f_OPenEventTime(object value)
    {
        int id = (int)value;
        Transform Icon_Event = null;
        if (Icon_Event == null)
            Icon_Event = f_GetObject("TopRightGrid").transform.Find(id.ToString());
        if (Icon_Event == null)
            Icon_Event = f_GetObject("TopRightGrid").transform.Find(id.ToString());
        if (Icon_Event == null)
            Icon_Event = f_GetObject("TopRightGrid").transform.Find(id.ToString());

        EventTimeDT ttttttttttttt = glo_Main.GetInstance().m_SC_Pool.m_EventTimeSC.f_GetSC(id) as EventTimeDT;
        if (Icon_Event == null)
        {
            CreatMainICon(ttttttttttttt);
        }
        else
        {
            if (Data_Pool.m_EventTimePool.f_CheckOpenEventTime(ttttttttttttt.iId))
                return;
            Icon_Event.gameObject.SetActive(true);
        }

        if(ttttttttttttt.szNameConst == "BattlePassPage")
        {
            if (Icon_Event == null)
                Icon_Event = f_GetObject("TopRightGrid").transform.Find(id.ToString());
            if (Icon_Event == null)
                Icon_Event = f_GetObject("TopRightGrid").transform.Find(id.ToString());
            if (Icon_Event == null)
                Icon_Event = f_GetObject("TopRightGrid").transform.Find(id.ToString());
            if (Data_Pool.m_EventTimePool.CheckReddotBattlePassTask(id))
            {
                Icon_Event.transform.Find("Reddot").gameObject.SetActive(true);
            }
            else if (Data_Pool.m_EventTimePool.CheckReddotBattlePassAward(id, 99999))
            {
                Icon_Event.transform.Find("Reddot").gameObject.SetActive(true);
            }
            else
            {
                Icon_Event.transform.Find("Reddot").gameObject.SetActive(false);
            }
        }


        f_GetObject("TopRightGrid").GetComponent<UIGrid>().Reposition();
        f_GetObject("TopRightGrid").GetComponent<UIGrid>().Reposition();
        f_GetObject("TopRightGrid").GetComponent<UIGrid>().Reposition();
    }

    #endregion


    #region TsuCode - AFK module
    private void OnAFKTimeSucCallback(object obj)
    {
        MessageBox.ASSERT("TSUlog AFK check " + Data_Pool.m_AFKPool.afkTime);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.AFKInfoPage, UIMessageDef.UI_OPEN);
        //if (obj != null)
        //    Data_Pool.m_AFKPool.afkTime = (long)obj;
    }
    private void OnAFKTimeFailCallback(object obj)
    {
        MessageBox.ASSERT("TSUlog AFK CS_AFKInfoTIme FAIl");

    }


    #endregion TsuCode - AFK module

    #region TsuCode - Kim Phieu
    private void f_OnBuyCoinSDK(object result)
    {
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
UITool.Ui_Trip("Mua hàng thành công");
            int coinCount = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Coin);
            string coinText = UITool.f_CountToChineseStr2(coinCount);
            f_GetObject("LabelCoinCount").GetComponent<UILabel>().text = coinText;
        }
        else
        {
UITool.Ui_Trip("Mua hàng không thành công");
            MessageBox.ASSERT("Recharge whitelist failed,code:" + (int)result);
        }
    }

    #endregion TsuCode - kim phieu

}
