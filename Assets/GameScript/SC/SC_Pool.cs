using System;
using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;



/// <summary>
/// 脚本管理器
/// </summary>
public class SC_Pool

{
    private bool _bLoadSuc;

    public ResourceSC m_ResourceSC;

#if Game
    public TipAniTemplateSC m_TipAniTemplateSC;
#elif BattleServer
    public ArenaRobotAttribSC m_ArenaRobotAttribSC = new ArenaRobotAttribSC();
#endif

    public AwakenCardSC m_AwakenCardSC = new AwakenCardSC();
    public CardFragmentSC m_CardFragmentSC = new CardFragmentSC();
    public CardSC m_CardSC = new CardSC();
    public CarLvUpSC m_CarLvUpSC = new CarLvUpSC();
    public MagicSC m_MagicSC = new MagicSC();
    public BaseGoodsSC m_BaseGoodsSC = new BaseGoodsSC();
    public ShopResourceSC m_ShopResourceSC = new ShopResourceSC();
    public CardEvolveSC m_CardEvolveSC = new CardEvolveSC();
    public PngAtlasSC m_PngAtlasSC = new PngAtlasSC();
    public DungeonChapterSC m_DungeonChapterSC = new DungeonChapterSC();
    public DungeonTollgateSC m_DungeonTollgateSC = new DungeonTollgateSC();
    public MonsterSC m_MonsterSC = new MonsterSC();
    public RoleModelSC m_RoleModelSC = new RoleModelSC();

    public EquipSC m_EquipSC = new EquipSC();
    public EquipConsumeSC m_EquipConsumeSC = new EquipConsumeSC();
    public EquipFragmentsSC m_EquipFragmentsSC = new EquipFragmentsSC();
    public EquipIntensifyConsumeSC m_EquipIntensifyConsumeSC = new EquipIntensifyConsumeSC();
    public MasterSC m_MasterSC = new MasterSC();
    public EquipUpStarSC m_EquipUpStarSC = new EquipUpStarSC();
    public ShopLotterySC m_ShopLotterySC = new ShopLotterySC();
    public AwardSC m_AwardSC = new AwardSC();
    public ShopGiftSC m_ShopGiftSC = new ShopGiftSC();

    public TreasureSC m_TreasureSC = new TreasureSC();
    public TreasureFragmentsSC m_TreasureFragmentsSC = new TreasureFragmentsSC();
    public TreasureRefineConsumeSC m_TreasureRefineConsumeSC = new TreasureRefineConsumeSC();
    public TreasureSkillSC m_TreasureSkillSC = new TreasureSkillSC();
    public TreasureUpConsumeSC m_TreasureUpConsumeSC = new TreasureUpConsumeSC();

    public GodEquipSC m_GodEquipSC = new GodEquipSC();
    public GodEquipConsumeSC m_GodEquipConsumeSC = new GodEquipConsumeSC();
    public GodEquipIntensifyConsumeSC m_GodEquipIntensifyConsumeSC = new GodEquipIntensifyConsumeSC();
    public GodEquipUpStarSC m_GodEquipUpStarSC = new GodEquipUpStarSC();
    public GodEquipFragmentsSC m_GodEquipFragmentsSC = new GodEquipFragmentsSC();
    public GodEquipSkillSC m_GodEquipSkillSC = new GodEquipSkillSC();

    public ShopRandSC m_ShopRandSC = new ShopRandSC();
    public ShopRandGoodsSC m_ShopRandGoodsSC = new ShopRandGoodsSC();
    public GameParamSC m_GameParamSC = new GameParamSC();
    public DungeonDialogSC m_DungeonDialogSC = new DungeonDialogSC();
    public SignedSC m_SignedSC = new SignedSC();
    public AwakenEquipSC m_AwakenEquipSC = new AwakenEquipSC();
    public TaskAchievementSC m_TaskAchievementSC = new TaskAchievementSC();
    public TaskBoxSC m_TaskBoxSC = new TaskBoxSC();
    public TaskDailySC m_TaskDailySC = new TaskDailySC();
    public VipPrivilegeSC m_VipPrivilegeSC = new VipPrivilegeSC();
    public PccaccySC m_PaySC = new PccaccySC();
    public AwardCentreSC m_AwardCentreSC = new AwardCentreSC();
    public ActLuckyLevelConfigSC m_ActLuckyLevelConfigSC = new ActLuckyLevelConfigSC();
    public ActLuckySymbolSC m_ActLuckySymbolSC = new ActLuckySymbolSC();
    public RankGiftSC m_RankGiftSC = new RankGiftSC();
    public BattleFormationsSC m_BattleFormationsSC = new BattleFormationsSC();
    public WealthManSC m_WealthManSC = new WealthManSC();
    public CardFateSC m_CardFateSC = new CardFateSC();
    public CardFateDataSC m_CardFateDataSC = new CardFateDataSC();
    public ActLoginGiftSC m_ActLoginGiftSC = new ActLoginGiftSC();
    public OpenServFundSC m_OpenServFundSC = new OpenServFundSC();
    public RebelArmySC m_RebelArmySC = new RebelArmySC();
    public RebelArmyDeploySC m_RebelArmyDeploySC = new RebelArmyDeploySC();
    public ReputationShopSC m_ReputationShopSC = new ReputationShopSC();
    public ArenaMatchSC m_ArenaMatchSC = new ArenaMatchSC();
    public ArenaRankAwardSC m_ArenaRankAwardSC = new ArenaRankAwardSC();
    public CrossArenaRankAwardSC m_CrossArenaRankAwardSC = new CrossArenaRankAwardSC();
    public CrossArenaShopSC m_CrossArenaShopSC = new CrossArenaShopSC();
    public CrossArenaTaskSC m_CrossArenaTaskSC = new CrossArenaTaskSC();
    public ExploitAwardSC m_ExploitAwardSC = new ExploitAwardSC();
    public ExploitLvSC m_ExploitLvSC = new ExploitLvSC();
    public RebelArmyRankAwardSC m_RebelArmyRankAwardSC = new RebelArmyRankAwardSC();
    public DailyPveGateSC m_DailyPveGateSC = new DailyPveGateSC();
    public DailyPveInfoSC m_DailyPveInfoSC = new DailyPveInfoSC();
    public CardTalentSC m_CardTalentSC = new CardTalentSC();
    public DiscountPropSC m_DiscountPropSC = new DiscountPropSC();
    public DiscountRechargeSC m_DiscountRechargeSC = new DiscountRechargeSC();
    public DiscountAllServSC m_DiscountAllServSC = new DiscountAllServSC();
    public SetEquipSC m_SetEquipSC = new SetEquipSC();
    public RunningManChapterSC m_RunningManChapterSC = new RunningManChapterSC();
    public RunningManTollgateSC m_RunningManTollgateSC = new RunningManTollgateSC();
    public RandNameSC m_RandNameSC = new RandNameSC();
    public RunningManBuffSC m_RunningManBuffSC = new RunningManBuffSC();
    public RunningManEliteSC m_RunningManEliteSC = new RunningManEliteSC();
    public RunningManShopSC m_RunningManShopSC = new RunningManShopSC();
    public CardSkyDestinySC m_CardSkyDestinySC = new CardSkyDestinySC();
    public GetWaySC m_GetWaySC = new GetWaySC();
    public PatrolEventSC m_PatrolEventSC = new PatrolEventSC();
    public PatrolLandSkillSC m_PatrolLandSkillSC = new PatrolLandSkillSC();
    public PatrolLandSC m_PatrolLandSC = new PatrolLandSC();
    public PatrolTypeSC m_PatrolTypeSC = new PatrolTypeSC();
    public GuidanceSC m_GuidanceSC = new GuidanceSC();
    public GuidanceDialogSC m_GuidanceDialogSC = new GuidanceDialogSC();
    public GuidanceTeamSC m_GuidanceTeamSC = new GuidanceTeamSC();
    public OnlineAwardSC m_OnlineAwardSC = new OnlineAwardSC();
    public BattleFeatShopSC m_BattleFeatShopSC = new BattleFeatShopSC();
    public DescendFortuneSC m_DescendFortuneSC = new DescendFortuneSC();
    public SevenActivityTaskSC m_SevenActivityTaskSC = new SevenActivityTaskSC();
    public EveryDayHotSaleSC m_EveryDayHotSaleSC = new EveryDayHotSaleSC();
    public HalfDiscountSC m_HalfDiscountSC = new HalfDiscountSC();

    public BufSC m_BufSC = new BufSC();
    public ServerInforSC m_ServerInforSC = new ServerInforSC();

    public RedPacketExchangeSC m_RedPacketExChangeSC = new RedPacketExchangeSC();
    public FashionableDressSC m_FashionableDressSC = new FashionableDressSC();
    public NewYearSellingSC m_NewYearSelling = new NewYearSellingSC();
    public RedPacketTaskSC m_RedPacketTaskSC = new RedPacketTaskSC();
    public NewYearStepSC m_NewYearStepSC = new NewYearStepSC();
    public NewYearSingleRechargeAwardSC m_NewYearSingleRechargeAwardSC = new NewYearSingleRechargeAwardSC();
    public NewYearMultiRechargeAwardSC m_NewYearMultiRechargeAwardSC = new NewYearMultiRechargeAwardSC();
    public NewYearSyceeConsumeSC m_NewYearSyceeConsumeSC = new NewYearSyceeConsumeSC();
    public MammonSendGiftSC m_MammonSendGiftSC = new MammonSendGiftSC();
    public NewYearSignSC m_NewYearSignSC = new NewYearSignSC();
    public NewYearDealsEveryDaySC m_NewYearDealsEveryDaySC = new NewYearDealsEveryDaySC();
    public NewYearExclusionSpinSC m_NewYearExclusionSpinSC = new NewYearExclusionSpinSC();

    public SyceeAwardSC m_SyceeAwardSC = new SyceeAwardSC();
    public ArtifactSC m_ArtifactSC = new ArtifactSC();
    public VipGiftSC m_VipGiftSC = new VipGiftSC();
    public FirstRechargeSC m_FirstRechargeSC = new FirstRechargeSC();
    public RecommendTeamSC m_RecommendTeamSC = new RecommendTeamSC();
    public GameNameParamSC m_GameNameParamSC = new GameNameParamSC();
    public WeekFundSC m_WeekFundSC = new WeekFundSC();//周基金

    public OpenSkyFortuneTimeSC m_OpenSkyFortuneTimeSC = new OpenSkyFortuneTimeSC();

    public CrossServerBattleTitleSC m_CrossServerBattleTitleSC = new CrossServerBattleTitleSC(); //跨服战头衔
    public CrossServerBattleZoneSC m_CrossServerBattleZoneSC = new CrossServerBattleZoneSC();  //跨服战战区
    public CrossServerBattleShopSC m_CrossServerBattleShopSC = new CrossServerBattleShopSC(); //跨服战商店
    public CrossServerBattleAwardSC m_CrossServerBattleAwardSC = new CrossServerBattleAwardSC(); //跨服战奖励
    //TsuCode- ChaosBattle
    public ChaosBattleTitleSC m_ChaosBattleTitleSC = new ChaosBattleTitleSC(); //跨服战头衔
    public ChaosBattleZoneSC m_ChaosBattleZoneSC = new ChaosBattleZoneSC();  //跨服战战区
    public ChaosBattleShopSC m_ChaosBattleShopSC = new ChaosBattleShopSC(); //跨服战商店
    public ChaosBattleAwardSC m_ChaosBattleAwardSC = new ChaosBattleAwardSC(); //跨服战奖励
    public ChaosBattleRankAwardSC m_ChaosBattleRankAwardSC = new ChaosBattleRankAwardSC();
    public ChaosBattleTaskSC m_ChaosBattleTaskSC = new ChaosBattleTaskSC();
    //TsuCode - AFKModule
    public AFKAwardSC m_AFKAwardSC = new AFKAwardSC();
    public AFKConfigSC m_AFKConfigSC = new AFKConfigSC();
    //----------------

    public TacticalSC m_TacticalSC = new TacticalSC();

    public TransmigrationCardSC m_TransmigrationCardSC = new TransmigrationCardSC();//卡牌转换
    public TransmigrationTreasureSC m_TransmigrationTreasureSC = new TransmigrationTreasureSC();//法宝转换
    public GodDressSC m_GodDressSC = new GodDressSC();//限时神装活动表
    public GodDressRankAwardSC m_GodDressRankAwardSC = new GodDressRankAwardSC();//限时神装排名奖励表
    public GodDressBoxSC m_GodDressBoxSC = new GodDressBoxSC();//限时神装活动宝箱表
    public FestivalExchangeSC m_FestivalExchangeSC = new FestivalExchangeSC();
    public TurntableLotterySC m_TurntableLotterySC = new TurntableLotterySC();//通天转盘
    public TurntableBoxSC m_TurntableBoxSC = new TurntableBoxSC();//通天转盘宝箱表
    public VERManageSC m_VERManageSC = new VERManageSC();

    public TranslateLanguageSC m_TranslateLanguageSC = new TranslateLanguageSC();
    public TranslateConfigSC m_TranslateConfigSC = new TranslateConfigSC();

    public PlotSC m_PlotSC = new PlotSC();
    public TrialTowerSC m_TrialTowerSC = new TrialTowerSC();
    public SevenStarBlessSC m_SevenStarBlessSC = new SevenStarBlessSC();
    public SevenStarLotterySC m_SevenStarLotterySC = new SevenStarLotterySC();
    public HelpDataSC m_HelpDataSC = new HelpDataSC();
    public SkyDesnitySC m_SkyDesnitySC = new SkyDesnitySC();
    public TurntableTimeSC m_TurntableTimeSC = new TurntableTimeSC();
    /////////////////////////////////////////////////////////////////////////////////
    //军团相关
    public LegionLevelSC m_LegionLevelSC = new LegionLevelSC();
    public LegionAwardSC m_LegionAwardSC = new LegionAwardSC();
    public LegionSacrificeSC m_LegionSacrificeSC = new LegionSacrificeSC();
    public LegionShopSC m_LegionShopSC = new LegionShopSC();
    public LegionShopTimeLimitSC m_LegionShopTimeLimitSC = new LegionShopTimeLimitSC();
    public LegionSkillSC m_LegionSkillSC = new LegionSkillSC();
    public LegionChapterSC m_LegionChapterSC = new LegionChapterSC();
    public LegionTollgateBoxSC m_LegionTollgateBoxSC = new LegionTollgateBoxSC();
    public LegionTollgateSC m_LegionTollgateSC = new LegionTollgateSC();

    public EventTimeSC m_EventTimeSC = new EventTimeSC();
    public OnlineVipAwardSC m_OnlineVipAwardSC = new OnlineVipAwardSC();
    public VoteAppSC m_VoteAppSC = new VoteAppSC();
    public AuraCampSC m_AuraCampSC = new AuraCampSC();
    public RankingPowerAwardSC m_RankingPowerAwardSC = new RankingPowerAwardSC();

    public TripleMoneySC m_TripleMoneySC = new TripleMoneySC();
    public LevelGiftSC m_LevelGiftSC = new LevelGiftSC();

    public BattlePassTaskSC m_BattlePassTaskSC = new BattlePassTaskSC();
    public BattlePassAwardSC m_BattlePassAwardSC = new BattlePassAwardSC();
    public BattlePassRankingSC m_BattlePassRankingSC = new BattlePassRankingSC();


    public PayCoinSC m_PayCoinSC = new PayCoinSC();

    // maco
    public AuraTypeSC m_AuraTypeSC = new AuraTypeSC();
    public AuraFiveElementsSC m_AuraFiveElementsSC = new AuraFiveElementsSC();
    public ElementalSeasonSC m_ElementalSeasonSC = new ElementalSeasonSC();
    public CrossTournamentShopSC m_CrossTournamentShopSC = new CrossTournamentShopSC();
    public CrossTournamentSC m_CrossTournamentSC = new CrossTournamentSC();
    public CrossTournamentKnockSC m_CrossTournamentKnockSC = new CrossTournamentKnockSC();
    public CrossTournamentQualifyingRoundSC m_CrossTournamentQualifyingRoundSC = new CrossTournamentQualifyingRoundSC();

    public ShopLotteryGodAwardSC m_ShopLotteryGenAwardSC = new ShopLotteryGodAwardSC();
    public ShopLotteryGodAwardSC m_ShopLotteryCampAwardSC = new ShopLotteryGodAwardSC();
    public CampGemShopSC m_CampGemShopSC = new CampGemShopSC();
    public LotteryLimitEventSC m_LotteryLimitEventSC = new LotteryLimitEventSC();
    public ShopEventTimeSC m_ShopEventTimeSC = new ShopEventTimeSC();
    public ShopEventTimeAwardSC m_ShopEventTimeAwardSC = new ShopEventTimeAwardSC();
    public ShopEndowAwardSC m_ShopEndowAwardSC = new ShopEndowAwardSC();
    public BufDetailSC m_BufDetailSC = new BufDetailSC();
    public ShopSeasonAwardSC m_ShopSeasonAwardSC = new ShopSeasonAwardSC();
    /////////////////////////////////////////////////////////////////////////////////

    List<NBaseSC> _aSCList = new List<NBaseSC>();
    Dictionary<string, NBaseSC> _dicSC = new Dictionary<string, NBaseSC>();
    public void f_LoadSC(byte[] bData)
    //public IEnumerator f_LoadSC(byte[] bData)
    {
        m_ResourceSC = new ResourceSC();
        //m_TipAniTemplateSC = new TipAniTemplateSC();
        _dicSC.Add("ServerInfo", m_ServerInforSC);
        _dicSC.Add("AwakenCard", m_AwakenCardSC);
        _dicSC.Add("CardFragment", m_CardFragmentSC);
        _dicSC.Add("Card", m_CardSC);
        _dicSC.Add("CarLvUp", m_CarLvUpSC);
        _dicSC.Add("Magic", m_MagicSC);
        _dicSC.Add("BaseGoods", m_BaseGoodsSC);
        _dicSC.Add("ShopResource", m_ShopResourceSC);
        _dicSC.Add("CardEvolve", m_CardEvolveSC);
        _dicSC.Add("PngAtlas", m_PngAtlasSC);
        _dicSC.Add("DungeonChapter", m_DungeonChapterSC);
        _dicSC.Add("DungeonTollgate", m_DungeonTollgateSC);
        _dicSC.Add("Monster", m_MonsterSC);
        _dicSC.Add("RoleModel", m_RoleModelSC);
        _dicSC.Add("Equip", m_EquipSC);
        _dicSC.Add("EquipConsume", m_EquipConsumeSC);
        _dicSC.Add("EquipFragments", m_EquipFragmentsSC);
        _dicSC.Add("EquipIntensifyConsume", m_EquipIntensifyConsumeSC);
        _dicSC.Add("GodEquip", m_GodEquipSC);
        _dicSC.Add("GodEquipConsume", m_GodEquipConsumeSC);
        _dicSC.Add("GodEquipIntensifyConsume", m_GodEquipIntensifyConsumeSC);
        _dicSC.Add("GodEquipUpStar", m_GodEquipUpStarSC);
        _dicSC.Add("GodEquipFragments", m_GodEquipFragmentsSC);
        _dicSC.Add("GodEquipSkill", m_GodEquipSkillSC);
        _dicSC.Add("Master", m_MasterSC);
        _dicSC.Add("EquipUpStar", m_EquipUpStarSC);
        _dicSC.Add("ShopLottery", m_ShopLotterySC);
        _dicSC.Add("Award", m_AwardSC);
        _dicSC.Add("ShopGift", m_ShopGiftSC);
        _dicSC.Add("Treasure", m_TreasureSC);
        _dicSC.Add("TreasureFragments", m_TreasureFragmentsSC);
        _dicSC.Add("TreasureRefineConsume", m_TreasureRefineConsumeSC);
        //_dicSC.Add("TreasureRefineConsume", m_TreasureRefineConsumeSC);
        _dicSC.Add("TreasureSkill", m_TreasureSkillSC);
        _dicSC.Add("TreasureUpConsume", m_TreasureUpConsumeSC);
        _dicSC.Add("ShopRand", m_ShopRandSC);
        _dicSC.Add("ShopRandGoods", m_ShopRandGoodsSC);
        _dicSC.Add("GameParam", m_GameParamSC);
        _dicSC.Add("DungeonDialog", m_DungeonDialogSC);
        _dicSC.Add("Signed", m_SignedSC);
        _dicSC.Add("AwakenEquip", m_AwakenEquipSC);
        _dicSC.Add("TaskAchievement", m_TaskAchievementSC);
        _dicSC.Add("TaskBox", m_TaskBoxSC);
        _dicSC.Add("TaskDaily", m_TaskDailySC);
        _dicSC.Add("VipPrivilege", m_VipPrivilegeSC);
        _dicSC.Add("Pay", m_PaySC);
        _dicSC.Add("AwardCentre", m_AwardCentreSC);
        _dicSC.Add("ActLuckyLevelConfig", m_ActLuckyLevelConfigSC);
        _dicSC.Add("ActLuckySymbol", m_ActLuckySymbolSC);
        _dicSC.Add("RankGift", m_RankGiftSC);
        _dicSC.Add("BattleFormations", m_BattleFormationsSC);
        _dicSC.Add("WealthMan", m_WealthManSC);
        _dicSC.Add("CardFate", m_CardFateSC);
        _dicSC.Add("CardFateData", m_CardFateDataSC);
        _dicSC.Add("ActLoginGift", m_ActLoginGiftSC);
        _dicSC.Add("OpenServFund", m_OpenServFundSC);
        _dicSC.Add("RebelArmy", m_RebelArmySC);
        _dicSC.Add("RebelArmyDeploy", m_RebelArmyDeploySC);
        _dicSC.Add("ReputationShop", m_ReputationShopSC);
        _dicSC.Add("ArenaMatch", m_ArenaMatchSC);
        _dicSC.Add("ArenaRankAward", m_ArenaRankAwardSC); 
        _dicSC.Add("CrossArenaRankAward", m_CrossArenaRankAwardSC); 
        _dicSC.Add("CrossArenaShop", m_CrossArenaShopSC);
        _dicSC.Add("CrossArenaTask", m_CrossArenaTaskSC);
        _dicSC.Add("ExploitAward", m_ExploitAwardSC);
        _dicSC.Add("ExploitLv", m_ExploitLvSC);
        _dicSC.Add("RebelArmyRankAward", m_RebelArmyRankAwardSC);
        _dicSC.Add("DailyPveGate", m_DailyPveGateSC);
        _dicSC.Add("DailyPveInfo", m_DailyPveInfoSC);
        _dicSC.Add("CardTalent", m_CardTalentSC);
        _dicSC.Add("DiscountProp", m_DiscountPropSC);
        _dicSC.Add("DiscountRecharge", m_DiscountRechargeSC);
        _dicSC.Add("DiscountAllServ", m_DiscountAllServSC);
        _dicSC.Add("SetEquip", m_SetEquipSC);
        _dicSC.Add("RunningManChapter", m_RunningManChapterSC);
        _dicSC.Add("RunningManTollgate", m_RunningManTollgateSC);
        _dicSC.Add("RandName", m_RandNameSC);
        _dicSC.Add("RunningManBuff", m_RunningManBuffSC);
        _dicSC.Add("RunningManElite", m_RunningManEliteSC);
        _dicSC.Add("RunningManShop", m_RunningManShopSC);
        _dicSC.Add("CardSkyDestiny", m_CardSkyDestinySC);
        _dicSC.Add("GetWay", m_GetWaySC);
        _dicSC.Add("TranslateConfig", m_TranslateConfigSC);
        _dicSC.Add("PatrolEvent", m_PatrolEventSC);
        _dicSC.Add("PatrolLandSkill", m_PatrolLandSkillSC);
        _dicSC.Add("PatrolLand", m_PatrolLandSC);
        _dicSC.Add("PatrolType", m_PatrolTypeSC);
        _dicSC.Add("Guidance", m_GuidanceSC);
        _dicSC.Add("GuidanceDialog", m_GuidanceDialogSC);
        _dicSC.Add("GuidanceTeam", m_GuidanceTeamSC);
        _dicSC.Add("OnlineAward", m_OnlineAwardSC);
        _dicSC.Add("BattleFeatShop", m_BattleFeatShopSC);
        _dicSC.Add("DescendFortune", m_DescendFortuneSC);
        _dicSC.Add("Buf", m_BufSC);
        _dicSC.Add("HalfDiscount", m_HalfDiscountSC);
        _dicSC.Add("SevenActivityTask", m_SevenActivityTaskSC);
        _dicSC.Add("EveryDayHotSale", m_EveryDayHotSaleSC);
        _dicSC.Add("RedPacketExchange", m_RedPacketExChangeSC);
        _dicSC.Add("FashionableDress", m_FashionableDressSC);
        _dicSC.Add("NewYearSelling", m_NewYearSelling);
        _dicSC.Add("RedPacketTask", m_RedPacketTaskSC);
        _dicSC.Add("NewYearSingleRechargeAward", m_NewYearSingleRechargeAwardSC);
        _dicSC.Add("NewYearMultiRechargeAward", m_NewYearMultiRechargeAwardSC);
        _dicSC.Add("NewYearDealsEveryDay", m_NewYearDealsEveryDaySC); 
        _dicSC.Add("NewYearExclusionSpin", m_NewYearExclusionSpinSC); 
        _dicSC.Add("NewYearSyceeConsume", m_NewYearSyceeConsumeSC);
        _dicSC.Add("MammonSendGift", m_MammonSendGiftSC);
        _dicSC.Add("NewYearSign", m_NewYearSignSC);
        _dicSC.Add("SyceeAward", m_SyceeAwardSC);
        _dicSC.Add("Artifact", m_ArtifactSC);
        _dicSC.Add("VipGift", m_VipGiftSC);
        _dicSC.Add("FirstRecharge", m_FirstRechargeSC);
        _dicSC.Add("RecommendTeam", m_RecommendTeamSC);
        _dicSC.Add("GameNameParam", m_GameNameParamSC);
        _dicSC.Add("WeekFund", m_WeekFundSC);
        _dicSC.Add("OpenSkyFortuneTime", m_OpenSkyFortuneTimeSC);
        _dicSC.Add("CrossServerBattleTitle", m_CrossServerBattleTitleSC);
        _dicSC.Add("CrossServerBattleZone", m_CrossServerBattleZoneSC);
        _dicSC.Add("CrossServerBattleShop", m_CrossServerBattleShopSC);
        _dicSC.Add("CrossServerBattleAward", m_CrossServerBattleAwardSC);
        //TsuCode - ChaosBattle
        _dicSC.Add("ChaosBattleTitle", m_ChaosBattleTitleSC);
        _dicSC.Add("ChaosBattleZone", m_ChaosBattleZoneSC);
        _dicSC.Add("ChaosBattleShop", m_ChaosBattleShopSC);
        _dicSC.Add("ChaosBattleAward", m_ChaosBattleAwardSC);
        _dicSC.Add("ChaosBattleRankAward", m_ChaosBattleRankAwardSC);
        _dicSC.Add("ChaosBattleTask", m_ChaosBattleTaskSC);
        _dicSC.Add("AFKAward", m_AFKAwardSC);
        _dicSC.Add("AFKConfig", m_AFKConfigSC);
        //-----------------------------
        _dicSC.Add("Tactical", m_TacticalSC);
        _dicSC.Add("TransmigrationCard", m_TransmigrationCardSC);
        _dicSC.Add("TransmigrationTreasure", m_TransmigrationTreasureSC);
        _dicSC.Add("GodDress", m_GodDressSC);
        _dicSC.Add("GodDressRankAward", m_GodDressRankAwardSC);
        _dicSC.Add("GodDressBox", m_GodDressBoxSC);
        _dicSC.Add("FestivalExchange", m_FestivalExchangeSC);
        _dicSC.Add("TurntableLottery", m_TurntableLotterySC);
        _dicSC.Add("TurntableBox", m_TurntableBoxSC);
        _dicSC.Add("LegionLevel", m_LegionLevelSC);
        _dicSC.Add("LegionAward", m_LegionAwardSC);
        _dicSC.Add("LegionSacrifice", m_LegionSacrificeSC);
        _dicSC.Add("LegionShop", m_LegionShopSC);
        _dicSC.Add("LegionShopTimeLimit", m_LegionShopTimeLimitSC);
        _dicSC.Add("LegionSkill", m_LegionSkillSC);
        _dicSC.Add("LegionChapter", m_LegionChapterSC);
        _dicSC.Add("LegionTollgate", m_LegionTollgateSC);
        _dicSC.Add("LegionTollgateBox", m_LegionTollgateBoxSC);
        _dicSC.Add("VERManage", m_VERManageSC);
        _dicSC.Add("TranslateLanguage", m_TranslateLanguageSC);
        _dicSC.Add("Plot", m_PlotSC);
        _dicSC.Add("TrialTower", m_TrialTowerSC);
        _dicSC.Add("SevenStarBless", m_SevenStarBlessSC);
        _dicSC.Add("SevenStarLottery", m_SevenStarLotterySC);
        _dicSC.Add("HelpData", m_HelpDataSC);
        _dicSC.Add("SkyDesnity", m_SkyDesnitySC);
        _dicSC.Add("TurntableTime", m_TurntableTimeSC);
        _dicSC.Add("EventTime", m_EventTimeSC); 
        _dicSC.Add("OnlineVipAward", m_OnlineVipAwardSC); 
        _dicSC.Add("VoteApp", m_VoteAppSC);
        _dicSC.Add("AuraCamp", m_AuraCampSC);
        _dicSC.Add("RankingPowerAward", m_RankingPowerAwardSC);

        _dicSC.Add("TripleMoney", m_TripleMoneySC);
        _dicSC.Add("LevelGift", m_LevelGiftSC);
        _dicSC.Add("BattlePassTask", m_BattlePassTaskSC); 
        _dicSC.Add("BattlePassAward", m_BattlePassAwardSC); 
        _dicSC.Add("BattlePassRanking", m_BattlePassRankingSC);
        _dicSC.Add("PayCoin", m_PayCoinSC);

        _dicSC.Add("AuraType", m_AuraTypeSC);
        _dicSC.Add("AuraFiveElements", m_AuraFiveElementsSC);
        _dicSC.Add("ElementalSeason", m_ElementalSeasonSC);
        _dicSC.Add("CrossTournamentShop", m_CrossTournamentShopSC);
        _dicSC.Add("CrossTournament", m_CrossTournamentSC);
        _dicSC.Add("CrossTournamentKnock", m_CrossTournamentKnockSC);
        _dicSC.Add("CrossTournamentQualifyingRound", m_CrossTournamentQualifyingRoundSC);
        _dicSC.Add("ShopLotteryGenAward", m_ShopLotteryGenAwardSC);
        _dicSC.Add("ShopLotteryCampAward", m_ShopLotteryCampAwardSC);
        _dicSC.Add("CampGemShop", m_CampGemShopSC);
        _dicSC.Add("LotteryLimitEvent", m_LotteryLimitEventSC);
        _dicSC.Add("ShopEventTime", m_ShopEventTimeSC);
        _dicSC.Add("ShopEventTimeAward", m_ShopEventTimeAwardSC);
        _dicSC.Add("ShopEndowAward", m_ShopEndowAwardSC);
        _dicSC.Add("BufDetail", m_BufDetailSC);
        _dicSC.Add("ShopSeasonAward", m_ShopSeasonAwardSC);
        //_dicSC.Add("VERManage", m_VERManageSC);


        //_aSCList.Add(m_AwakenCardSC);
        //_aSCList.Add(m_CardFragmentSC);
        //_aSCList.Add(m_CardSC);
        //_aSCList.Add(m_CarLvUpSC);
        //_aSCList.Add(m_MagicSC);
        //_aSCList.Add(m_BaseGoodsSC);
        //_aSCList.Add(m_ShopResourceSC);
        //_aSCList.Add(m_CardEvolveSC);
        //_aSCList.Add(m_PngAtlasSC);
        //_aSCList.Add(m_DungeonChapterSC);
        //_aSCList.Add(m_DungeonTollgateSC);
        //_aSCList.Add(m_MonsterSC);
        //_aSCList.Add(m_RoleModelSC);
        //_aSCList.Add(m_EquipSC);
        //_aSCList.Add(m_EquipConsumeSC);
        //_aSCList.Add(m_EquipFragmentsSC);
        //_aSCList.Add(m_EquipIntensifyConsumeSC);
        //_aSCList.Add(m_MasterSC);
        //_aSCList.Add(m_EquipUpStarSC);
        //_aSCList.Add(m_ShopLotterySC);
        //_aSCList.Add(m_AwardSC);
        //_aSCList.Add(m_ShopGiftSC);
        //_aSCList.Add(m_TreasureSC);
        //_aSCList.Add(m_TreasureFragmentsSC);
        //_aSCList.Add(m_TreasureRefineConsumeSC);
        //_aSCList.Add(m_TreasureSkillSC);
        //_aSCList.Add(m_TreasureUpConsumeSC);
        //_aSCList.Add(m_ShopRandSC);
        //_aSCList.Add(m_ShopRandGoodsSC);
        //_aSCList.Add(m_GameParamSC);
        //_aSCList.Add(m_DungeonDialogSC);
        //_aSCList.Add(m_SignedSC);
        //_aSCList.Add(m_AwakenEquipSC);
        //_aSCList.Add(m_TaskAchievementSC);
        //_aSCList.Add(m_TaskBoxSC);
        //_aSCList.Add(m_TaskDailySC);
        //_aSCList.Add(m_VipPrivilegeSC);
        //_aSCList.Add(m_PaySC);
        //_aSCList.Add(m_AwardCentreSC);
        //_aSCList.Add(m_ActLuckyLevelConfigSC);
        //_aSCList.Add(m_ActLuckySymbolSC);
        //_aSCList.Add(m_RankGiftSC);
        //_aSCList.Add(m_BattleFormationsSC);
        //_aSCList.Add(m_WealthManSC);
        //_aSCList.Add(m_CardFateSC);
        //_aSCList.Add(m_CardFateDataSC);
        //_aSCList.Add(m_ActLoginGiftSC);
        //_aSCList.Add(m_OpenServFundSC);
        //_aSCList.Add(m_RebelArmySC);
        //_aSCList.Add(m_RebelArmyDeploySC);
        //_aSCList.Add(m_ReputationShopSC);
        //_aSCList.Add(m_ArenaMatchSC);
        //_aSCList.Add(m_ArenaRankAwardSC);
        //_aSCList.Add(m_ExploitAwardSC);
        //_aSCList.Add(m_ExploitLvSC);
        //_aSCList.Add(m_RebelArmyRankAwardSC);
        //_aSCList.Add(m_DailyPveGateSC);
        //_aSCList.Add(m_DailyPveInfoSC);
        //_aSCList.Add(m_CardTalentSC);
        //_aSCList.Add(m_DiscountPropSC);
        //_aSCList.Add(m_DiscountRechargeSC);
        //_aSCList.Add(m_DiscountAllServSC);
        //_aSCList.Add(m_SetEquipSC);
        //_aSCList.Add(m_RunningManChapterSC);
        //_aSCList.Add(m_RunningManTollgateSC);
        //_aSCList.Add(m_RandNameSC);
        //_aSCList.Add(m_RunningManBuffSC);
        //_aSCList.Add(m_RunningManEliteSC);
        //_aSCList.Add(m_RunningManShopSC);
        //_aSCList.Add(m_CardSkyDestinySC);
        //_aSCList.Add(m_GetWaySC);
        //_aSCList.Add(m_PatrolEventSC);
        //_aSCList.Add(m_PatrolLandSkillSC);
        //_aSCList.Add(m_PatrolLandSC);
        //_aSCList.Add(m_PatrolTypeSC);
        //_aSCList.Add(m_GuidanceSC);
        //_aSCList.Add(m_GuidanceDialogSC);
        //_aSCList.Add(m_GuidanceTeamSC);
        //_aSCList.Add(m_OnlineAwardSC);
        //_aSCList.Add(m_BattleFeatShopSC);
        //_aSCList.Add(m_DescendFortuneSC);
        //_aSCList.Add(m_BufSC);
        //_aSCList.Add(m_HalfDiscountSC);
        //_aSCList.Add(m_SevenActivityTaskSC);
        //_aSCList.Add(m_EveryDayHotSaleSC);
        //_aSCList.Add(m_FashionableDressSC);
        //_aSCList.Add(m_RedPacketExChangeSC);
        //_aSCList.Add(m_NewYearSelling);
        //_aSCList.Add(m_RedPacketTaskSC);
        //_aSCList.Add(m_NewYearSingleRechargeAwardSC);
        //_aSCList.Add(m_NewYearMultiRechargeAwardSC);
        //_aSCList.Add(m_NewYearSyceeConsumeSC);
        //_aSCList.Add(m_MammonSendGiftSC);
        //_aSCList.Add(m_NewYearSignSC);
        //_aSCList.Add(m_SyceeAwardSC);
        //_aSCList.Add(m_ArtifactSC);
        //_aSCList.Add(m_VipGiftSC);
        //_aSCList.Add(m_FirstRechargeSC);
        //_aSCList.Add(m_RecommendTeamSC);
        //_aSCList.Add(m_GameNameParamSC);
        //_aSCList.Add(m_WeekFundSC);//周基金
        //_aSCList.Add(m_OpenSkyFortuneTimeSC);
        //_aSCList.Add(m_CrossServerBattleTitleSC);
        //_aSCList.Add(m_CrossServerBattleZoneSC);
        //_aSCList.Add(m_CrossServerBattleShopSC);
        //_aSCList.Add(m_TacticalSC);
        //_aSCList.Add(m_TransmigrationCardSC);//卡牌转换
        //_aSCList.Add(m_TransmigrationTreasureSC);//法宝转换
        //_aSCList.Add(m_GodDressSC);//限时神装活动表
        //_aSCList.Add(m_GodDressRankAwardSC);//限时神装排名奖励
        //_aSCList.Add(m_GodDressBoxSC);//限时神装宝箱
        //_aSCList.Add(m_FestivalExchangeSC);
        // _aSCList.Add(m_TurntableLotterySC);
        //_aSCList.Add(m_TurntableBoxSC);
        /////////////////////////////////////////////////////////////////////////////////
        //军团相关
        //_aSCList.Add(m_LegionLevelSC);
        //_aSCList.Add(m_LegionAwardSC);
        //_aSCList.Add(m_LegionSacrificeSC);
        //_aSCList.Add(m_LegionShopSC);
        //_aSCList.Add(m_LegionShopTimeLimitSC);
        //_aSCList.Add(m_LegionSkillSC);
        //_aSCList.Add(m_LegionChapterSC);
        //_aSCList.Add(m_LegionTollgateSC);
        //_aSCList.Add(m_LegionTollgateBoxSC);
        /////////////////////////////////////////////////////////////////////////////////

        //_aSCList.Add(m_VERManageSC);
        //_aSCList.Add(m_TranslateLanguageSC);


#if BattleServer
        m_ArenaRobotAttribSC.f_LoadSCForData(ArenaRobotAttribSC.m_strSC);
#endif


        _bLoadSuc = false;
        int i = 0;
MessageBox.DEBUG("Switch Commands");
        //DateTime start = DateTime.Now;
        //Debug.Log("解析脚本");

        //string ppSQL;
        ////System.Array.Copy(bData , b , 5);
        //int iHeadLen = int.Parse(System.Text.Encoding.UTF8.GetString(bData, 0, 5));
        //byte[] b = new byte[iHeadLen + 1];
        //System.Array.Copy(bData, 5, b, 0, iHeadLen);
        //string strHeadData = System.Text.Encoding.UTF8.GetString(b);
        //string[] ttt = strHeadData.Split(new string[] { "," }, System.StringSplitOptions.None);
        //int iMovePos = iHeadLen + 5;

        byte[] bUncompressData = ZipTools.ccUnCompress(bData);
        string strData = System.Text.Encoding.UTF8.GetString(bUncompressData);
        string[] strArrData = strData.Split(new string[] { "#1SD#" }, StringSplitOptions.None);
        for (int j = 0; j < strArrData.Length; j++)
        {
            string[] strSc = strArrData[j].Split(new string[] { "1#QW" }, StringSplitOptions.None);
            string sKey = strSc[0];

            if (_dicSC.ContainsKey(sKey))
            {
                _dicSC[sKey].f_LoadSCForData(strArrData[j]);
            }
            else
            {
Debug.Log(string.Format("{0} data table is not used", sKey));
            }
        }
        //Debug.Log(strData);
        //for (i = 0; i < _aSCList.Count; i++)
        //{
        //    //yield return new WaitForSeconds(4.5f/_aSCList.Count);
        //    MessageBox.DEBUG("SC " + i + " " + _aSCList[i].m_strRegDTName);
        //    int iDataLen = int.Parse(ttt[i]);
        //    ppSQL = ZipTools.aaa556(bData, iMovePos, iDataLen);
        //    _aSCList[i].f_LoadSCForData(ppSQL);
        //    iMovePos = iMovePos + iDataLen;
        //}

        _bLoadSuc = true;
        //Debug.Log("解析脚本成功" + (DateTime.Now - start));
MessageBox.DEBUG("Command conversion successful");
    }


    public bool f_CheckLoadSuc()
    {
        return _bLoadSuc;
    }


}
