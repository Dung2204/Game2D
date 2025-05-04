using UnityEngine;
using System.Collections;
using ccU3DEngine;

/// <summary>
/// Profab管理 注册所有UI界面
/// </summary>
public class UIProfabPool
{
    private static readonly string strGameMainSceneUIRoot = "GameMain";
    private static readonly string strLoginSceneUIRoot = "LoginMain";
    private static readonly string strBattleSceneUIRoot = "BattleMain";

    /// <summary>
    /// 公共UI的根节点
    /// </summary>
    private static readonly string strCommonUIRoot = "CommonUI";

    public static void f_Init()
    {
        //注册登录场景UI
        ccUIManage.GetInstance().f_RegProfab(UINameConst.RegisterPage, "UI/UIPrefab/Login/RegisterPage", strLoginSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.SwichAccountPage, "UI/UIPrefab/Login/SwichAccountPage", strLoginSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.ResLoadPage, "UI/UIPrefab/Login/ResLoadPage", strLoginSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.DownLoadNewApk, "UI/UIPrefab/Login/DownLoadNewApk", strLoginSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.ForceResLoadPage, "UI/UIPrefab/Login/ForceResLoadPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.LoginPage, "UI/UIPrefab/Login/LoginPage", strLoginSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.ShowLogoPage, "UI/UIPrefab/Login/ShowLogoPage", strLoginSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.NoticePanel, "UI/UIPrefab/Login/NoticePanel", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.SelectServerPage, "UI/UIPrefab/Login/SelectServerPage", strLoginSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.CreateRolePage, "UI/UIPrefab/CreateRolePage", strLoginSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.LoginQueuePage, "UI/UIPrefab/Login/LoginQueuePage", strLoginSceneUIRoot);

        //注册主场景UI
        ccUIManage.GetInstance().f_RegProfab(UINameConst.MainMenu, "UI/UIPrefab/GameMain/MainMenu", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.UserInfoPage, "UI/UIPrefab/GameMain/UserInfoPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.ConnectGMPage, "UI/UIPrefab/GameMain/ConnectGMPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.BugReportPage, "UI/UIPrefab/GameMain/BugReportPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.RenamePage, "UI/UIPrefab/GameMain/RenamePage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.CardShowPage, "UI/UIPrefab/GameMain/CardShow/CardShowPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.RecommendTeamPage, "UI/UIPrefab/GameMain/RecommendTeamPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.LineUpPage, "UI/UIPrefab/GameMain/ClothArray/LineUpPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.ClothArrayPage, "UI/UIPrefab/GameMain/ClothArray/ClothArrayPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.AuraCampPage, "UI/UIPrefab/GameMain/ClothArray/AuraCampPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.StrengthenMasterPage, "UI/UIPrefab/GameMain/ClothArray/StrengthenMasterPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.SkillIntroDetailPage, "UI/UIPrefab/GameMain/ClothArray/SkillIntroDetailPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.FateIntroDetailPage, "UI/UIPrefab/GameMain/ClothArray/FateIntroDetailPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.SelectCardPage, "UI/UIPrefab/GameMain/ClothArray/SelectCardPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.SelectEquipPage, "UI/UIPrefab/GameMain/ClothArray/SelectEquipPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.CardProperty, "UI/UIPrefab/GameMain/CardProperty/CardProperty", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.ChatManage, "UI/UIPrefab/GameMain/ChatManage/ChatManage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.ShopPage, "UI/UIPrefab/GameMain/ShopPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.RebelArmy, "UI/UIPrefab/GameMain/RebelArmy/RebelArmy", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.RebelAymyTriggen, "UI/UIPrefab/GameMain/RebelAymyTriggen/RebelAymyTriggen", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.MainLineOneKeySweepPage, "UI/UIPrefab/GameMain/Dungeon/MainLineOneKeySweepPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.MainLineTreasureBookPage, "UI/UIPrefab/GameMain/Dungeon/MainLineTreasureBookPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.FightPowerChangePage, "UI/UIPrefab/GameMain/FightPowerChangePage", strGameMainSceneUIRoot);

        ccUIManage.GetInstance().f_RegProfab(UINameConst.CardBagPage, "UI/UIPrefab/GameMain/Bag/CardBagPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.EquipBagPage, "UI/UIPrefab/GameMain/Bag/EquipBagPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.GodEquipBagPage, "UI/UIPrefab/GameMain/Bag/GodEquipBagPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.AwakenEquipBagPage, "UI/UIPrefab/GameMain/Bag/AwakenEquipBagPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.EquipManage, "UI/UIPrefab/GameMain/EquipManage/EquipManage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.GodEquipManage, "UI/UIPrefab/GameMain/GodEquipManage/GodEquipManage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.OneKeyEquipRefine, "UI/UIPrefab/GameMain/EquipManage/OneKeyEquipRefine", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.OneKeyGodEquipRefine, "UI/UIPrefab/GameMain/GodEquipManage/OneKeyGodEquipRefine", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.SelectAward, "UI/UIPrefab/GameMain/Bag/SelectAward", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.GainEquipShowPage, "UI/UIPrefab/GameMain/EquipManage/GainEquipShowPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.TreasureBagPage, "UI/UIPrefab/GameMain/Bag/TreasureBagPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.TreasureManage, "UI/UIPrefab/GameMain/TreasureManage/TreasureManage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.OneKeyTransureUpLv, "UI/UIPrefab/GameMain/TreasureManage/OneKeyTransureUpLv", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.GoodsBagPage, "UI/UIPrefab/GameMain/Bag/GoodsBagPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.DungeonChapterPageNew, "UI/UIPrefab/GameMain/Dungeon/DungeonChapterPageNew", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.DungeonTollgatePageNew, "UI/UIPrefab/GameMain/Dungeon/DungeonTollgatePageNew", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.DungeonFirstWinPage, "UI/UIPrefab/GameMain/Dungeon/DungeonFirstWinPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.BattleFormFragmentPage, "UI/UIPrefab/GameMain/Dungeon/BattleFormFragmentPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.DungeonSweepPage, "UI/UIPrefab/GameMain/Dungeon/DungeonSweepPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.SelectDifficultyPage, "UI/UIPrefab/GameMain/Dungeon/SelectDifficultyPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.ShopStorePage, "UI/UIPrefab/GameMain/ShopStore/ShopStorePage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.ShopCommonPage, "UI/UIPrefab/GameMain/ShopStore/ShopCommonPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.ShopMutiCommonPage, "UI/UIPrefab/GameMain/ShopStore/ShopMutiCommonPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.ActivityPage, "UI/UIPrefab/GameMain/Activity/ActivityPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.NewYearActPage, "UI/UIPrefab/GameMain/NewYearAct/NewYearActPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.TimeDiscountPage, "UI/UIPrefab/GameMain/Activity/TimeDiscountPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.LimitGodEquipActPage, "UI/UIPrefab/GameMain/Activity/LimitGodEquipActPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.FirstRechargePage, "UI/UIPrefab/GameMain/Activity/FirstRechargePage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.FirstRechargePageNew, "UI/UIPrefab/GameMain/Activity/FirstRecharge/FirstRechargePageNew", strGameMainSceneUIRoot);  //TsuCode - FirstRechargeNew - NapDau
        ccUIManage.GetInstance().f_RegProfab(UINameConst.RankGiftPage, "UI/UIPrefab/GameMain/Activity/RankGiftPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.SkyFortunePage, "UI/UIPrefab/GameMain/Activity/SkyFortunePage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.ShowVip, "UI/UIPrefab/GameMain/ShowVip", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.TaskPage, "UI/UIPrefab/GameMain/Task/TaskPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.Award, "UI/UIPrefab/GameMain/Award/Award", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.CommonHelpPage, "UI/UIPrefab/CommonUI/CommonHelpPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.SleepTimePage, "UI/UIPrefab/GameMain/SleepTimePage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.Recycle, "UI/UIPrefab/GameMain/Recycle/Recycle", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.BattleFormPage, "UI/UIPrefab/GameMain/BattleFormPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.FateTrip, "UI/UIPrefab/GameMain/FateTrip/FateTrip", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.LegionSacrifice, "LegionRes/UIPrefab/LegionSacrifice", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.LegionUpSuc, "LegionRes/UIPrefab/LegionUpSuc", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.Manor, "LegionRes/UIPrefab/Manor/Manor", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.SevenDayActivityPage, "UI/UIPrefab/GameMain/SevenDayActivityPage/SevenDayActivityPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.EveryDayHotSalePage, "UI/UIPrefab/GameMain/SevenDayActivityPage/EveryDayHotSalePage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.TariningPage, "UI/UIPrefab/GameMain/Tarining/TariningPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.TacticalPage, "UI/UIPrefab/GameMain/Tarining/TacticalPage", strGameMainSceneUIRoot);

        ccUIManage.GetInstance().f_RegProfab(UINameConst.CardChangePage, "UI/UIPrefab/GameMain/CardChange/CardChangePage", strGameMainSceneUIRoot);//移魂阵
        ccUIManage.GetInstance().f_RegProfab(UINameConst.TransmigrationTreasurePage, "UI/UIPrefab/GameMain/GrabTreasure/TransmigrationTreasurePage", strGameMainSceneUIRoot);//熔炼
        ccUIManage.GetInstance().f_RegProfab(UINameConst.TurntablePage, "UI/UIPrefab/GameMain/Turntable/TurntablePage", strGameMainSceneUIRoot);//熔炼

        ccUIManage.GetInstance().f_RegProfab(UINameConst.TrialTowerPage, "UI/UIPrefab/GameMain/TrialTowerPage/TrialTowerPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.TrialTowerRoomPage, "UI/UIPrefab/GameMain/TrialTowerPage/TrialTowerRoomPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.SevenStarPage, "UI/UIPrefab/GameMain/TrialTowerPage/SevenStarPage", strGameMainSceneUIRoot);
        //好友相关
        ccUIManage.GetInstance().f_RegProfab(UINameConst.FriendPage, "UI/UIPrefab/GameMain/Friend/FriendPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.AddFriendPage, "UI/UIPrefab/GameMain/Friend/AddFriendPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.RecommendFriendPage, "UI/UIPrefab/GameMain/Friend/RecommendFriendPage", strGameMainSceneUIRoot);
        //挑战菜单界面
        ccUIManage.GetInstance().f_RegProfab(UINameConst.ChallengeMenuPage, "UI/UIPrefab/GameMain/ChallengeMenuPage", strGameMainSceneUIRoot);
        //竞技场相关
        ccUIManage.GetInstance().f_RegProfab(UINameConst.ArenaPage, "UI/UIPrefab/GameMain/Arena/ArenaPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.ArenaPageNew, "UI/UIPrefab/GameMain/Arena/ArenaPageNew", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.ArenaRankPage, "UI/UIPrefab/GameMain/Arena/ArenaRankPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.ArenaSweepPage, "UI/UIPrefab/GameMain/Arena/ArenaSweepPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.HandilyChallengePage, "UI/UIPrefab/GameMain/Arena/HandilyChallengePage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.HandilyChallengeResultPage, "UI/UIPrefab/GameMain/Arena/HandilyChallengeResultPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.ArenaBreakAwardPage, "UI/UIPrefab/GameMain/Arena/ArenaBreakAwardPage", strGameMainSceneUIRoot);
        //竞技场 结算相关
        ccUIManage.GetInstance().f_RegProfab(UINameConst.ArenaFinishPage, "UI/UIPrefab/GameMain/Arena/ArenaFinishPageNew", strBattleSceneUIRoot);

        ccUIManage.GetInstance().f_RegProfab(UINameConst.ArenaCrossPage, "UI/UIPrefab/GameMain/ArenaCross/ArenaCrossPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.ArenaCrossBattleFinishPage, "UI/UIPrefab/GameMain/ArenaCross/ArenaCrossBattleFinishPage", strBattleSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.ArenaCrossRankPage, "UI/UIPrefab/GameMain/ArenaCross/ArenaCrossRankPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.ArenaCrossRecordPage, "UI/UIPrefab/GameMain/ArenaCross/ArenaCrossRecordPage", strGameMainSceneUIRoot);


        //夺宝界面相关
        ccUIManage.GetInstance().f_RegProfab(UINameConst.GrabTreasurePage, "UI/UIPrefab/GameMain/GrabTreasure/GrabTreasurePage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.SelectOpponentPage, "UI/UIPrefab/GameMain/GrabTreasure/SelectOpponentPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.GrabTreasureFinishPage, "UI/UIPrefab/BattleScene/GrabTreasureFinishPage", strBattleSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.OneKeyGrabTreasurePage, "UI/UIPrefab/GameMain/GrabTreasure/OneKeyGrabTreasurePage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.GrabSweepResultPage, "UI/UIPrefab/GameMain/GrabTreasure/GrabSweepResultPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.ChooseAwardPage, "UI/UIPrefab/BattleScene/ChooseAwardPage", strBattleSceneUIRoot);
        #region 军团相关

        ccUIManage.GetInstance().f_RegProfab(UINameConst.LegionCreatePage, "LegionRes/UIPrefab/LegionCreatePage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.LegionListPage, "LegionRes/UIPrefab/LegionListPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.LegionMenuPage, "LegionRes/UIPrefab/LegionMenuPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.LegionHallPage, "LegionRes/UIPrefab/LegionHallPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.LegionApplicantPage, "LegionRes/UIPrefab/LegionApplicantPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.LegionSkillPage, "LegionRes/UIPrefab/LegionSkillPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.SkillInfoPage, "LegionRes/UIPrefab/SkillInfoPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.LegionRedPacketPage, "LegionRes/UIPrefab/LegionRedPacketPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.LegionRedRankPage, "LegionRes/UIPrefab/LegionRedRankPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.LegionRankPage, "LegionRes/UIPrefab/LegionRankPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.LegionChapterPage, "LegionRes/UIPrefab/Dungeon/LegionChapterPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.LegionTollgatePage, "LegionRes/UIPrefab/Dungeon/LegionTollgatePage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.LegionTollgateChallengePage, "LegionRes/UIPrefab/Dungeon/LegionTollgateChallengePage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.LegionChapterAwardPage, "LegionRes/UIPrefab/Dungeon/LegionChapterAwardPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.LegionChapterResetPage, "LegionRes/UIPrefab/Dungeon/LegionChapterResetPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.LegionTollgateAwardPage, "LegionRes/UIPrefab/Dungeon/LegionTollgateAwardPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.LegionTollgateAwardPreviewPage, "LegionRes/UIPrefab/Dungeon/LegionTollgateAwardPreviewPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.LegionDungeonFinishPage, "LegionRes/UIPrefab/Dungeon/LegionDungeonFinishPage", strBattleSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.LegionDungeonBuyPage, "LegionRes/UIPrefab/Dungeon/LegionDungeonBuyPage", strGameMainSceneUIRoot);

        ccUIManage.GetInstance().f_RegProfab(UINameConst.LegionBattlePage, "LegionRes/UIPrefab/Battle/new/LegionBattlePage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.LegionBattleGatePage, "LegionRes/UIPrefab/Battle/new/LegionBattleGatePage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.LegionBattleChallengePage, "LegionRes/UIPrefab/Battle/new/LegionBattleChallengePage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.LegionBattleFinishPage, "LegionRes/UIPrefab/Battle/new/LegionBattleFinishPage", strBattleSceneUIRoot);

        ccUIManage.GetInstance().f_RegProfab(UINameConst.LegionBattleAwardPage, "LegionRes/UIPrefab/Battle/new/LegionBattleAwardPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.LegionBattleListPage, "LegionRes/UIPrefab/Battle/new/LegionBattleListPage", strGameMainSceneUIRoot);

        #endregion

        //过关斩将
        ccUIManage.GetInstance().f_RegProfab(UINameConst.RunningManPage, "UI/UIPrefab/GameMain/RunningMan/RunningManPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.RunningManChallengePage, "UI/UIPrefab/GameMain/RunningMan/RunningManChallengePage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.RunningManEliteChallengePage, "UI/UIPrefab/GameMain/RunningMan/RunningManEliteChallengePage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.RunningManBuffAddPage, "UI/UIPrefab/GameMain/RunningMan/RunningManBuffAddPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.RunningManBattleFinishPage, "UI/UIPrefab/GameMain/RunningMan/RunningManBattleFinishPage", strBattleSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.RunningManRankPage, "UI/UIPrefab/GameMain/RunningMan/RunningManRankPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.RunningManElitePage, "UI/UIPrefab/GameMain/RunningMan/RunningManElitePage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.RunningManEliteFinishPage, "UI/UIPrefab/GameMain/RunningMan/RunningManEliteFinishPage", strBattleSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.RunningManSweepPage, "UI/UIPrefab/GameMain/RunningMan/RunningManSweepPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.RunningManAwardPage, "UI/UIPrefab/GameMain/RunningMan/RunningManAwardPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.RunningManAttrPlusPage, "UI/UIPrefab/GameMain/RunningMan/RunningManAttrPlusPage", strGameMainSceneUIRoot);
        //领地巡逻
        ccUIManage.GetInstance().f_RegProfab(UINameConst.PatrolPage, "UI/UIPrefab/GameMain/PatrolPage/PatrolPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.PatrolLandPage, "UI/UIPrefab/GameMain/PatrolPage/PatrolLandPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.PatrolSkillPage, "UI/UIPrefab/GameMain/PatrolPage/PatrolSkillPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.PatrolVisitFriendPage, "UI/UIPrefab/GameMain/PatrolPage/PatrolVisitFriendPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.PatrolSelectTypePage, "UI/UIPrefab/GameMain/PatrolPage/PatrolSelectTypePage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.PatrolSelectTimePage, "UI/UIPrefab/GameMain/PatrolPage/PatrolSelectTimePage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.PatrolSelectCardPage, "UI/UIPrefab/GameMain/PatrolPage/PatrolSelectCardPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.PatrolOnekeyPage, "UI/UIPrefab/GameMain/PatrolPage/PatrolOnekeyPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.PatrolAwardOnekeyPage, "UI/UIPrefab/GameMain/PatrolPage/PatrolAwardOnekeyPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.PatrolPacifyAwardPage, "UI/UIPrefab/GameMain/PatrolPage/PatrolPacifyAwardPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.PatrolBattleFinishPage, "UI/UIPrefab/GameMain/PatrolPage/PatrolBattleFinishPage", strBattleSceneUIRoot);
        //跨服战
        ccUIManage.GetInstance().f_RegProfab(UINameConst.CrossServerBattlePage, "UI/UIPrefab/GameMain/CrossServerBattle/CrossServerBattlePage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.CrossServerBattleFinishPage, "UI/UIPrefab/GameMain/CrossServerBattle/CrossServerBattleFinishPage", strBattleSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.CrossServerBattleRankPage, "UI/UIPrefab/GameMain/CrossServerBattle/CrossServerBattleRankPage", strGameMainSceneUIRoot);
        //TsuCode - Chaosbattle
        ccUIManage.GetInstance().f_RegProfab(UINameConst.ChaosBattlePage, "UI/UIPrefab/GameMain/ChaosBattle/ChaosBattlePage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.ChaosBattleFinishPage, "UI/UIPrefab/GameMain/ChaosBattle/ChaosBattleFinishPage", strBattleSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.ChaosBattleRankPage, "UI/UIPrefab/GameMain/ChaosBattle/ChaosBattleRankPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.RoyalBattlePage, "UI/UIPrefab/GameMain/ChaosBattle/RoyalBattlePage", strGameMainSceneUIRoot);
        //TsuCode - AFKModule
        ccUIManage.GetInstance().f_RegProfab(UINameConst.AFKPage, "UI/UIPrefab/GameMain/AFK/AFKPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.AFKInfoPage, "UI/UIPrefab/GameMain/AFK/AFKInfoPage", strGameMainSceneUIRoot);
        //--------------------

        //跨服斗将
        ccUIManage.GetInstance().f_RegProfab(UINameConst.CardBattlePage, "UI/UIPrefab/GameMain/CardBattle/CardBattlePage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.CardBattleClothPage, "UI/UIPrefab/GameMain/CardBattle/CardBattleClothPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.CardBattleFinishPage, "UI/UIPrefab/GameMain/CardBattle/CardBattleFinishPage", strBattleSceneUIRoot);

        //排行榜
        ccUIManage.GetInstance().f_RegProfab(UINameConst.RankListPage, "UI/UIPrefab/GameMain/RankList/RankListPageNew", strGameMainSceneUIRoot);
        //注册战斗场景UI
        ccUIManage.GetInstance().f_RegProfab(UINameConst.BattleFinishPage, "UI/UIPrefab/BattleScene/BattleFinishPage", strBattleSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.BattleDataPag, "UI/UIPrefab/BattleScene/BattleDataPag", strBattleSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.RebelArmyFinish, "UI/UIPrefab/BattleScene/RebelArmyFinish", strBattleSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.TrialTowerFinshPage, "UI/UIPrefab/GameMain/TrialTowerPage/TrialTowerFinshPage", strBattleSceneUIRoot);
        //注册公共界面
        ccUIManage.GetInstance().f_RegProfab(UINameConst.LoadingPage, "UI/UIPrefab/CommonUI/LoadingPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.DialogPage, "UI/UIPrefab/CommonUI/Dialog/DialogPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.LookPlayerInfoPage, "UI/UIPrefab/CommonUI/LookPlayerInfoPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.BoxGetSubPage, "UI/UIPrefab/CommonUI/BoxGetSubPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.AwardGetSubPage, "UI/UIPrefab/CommonUI/AwardGetSubPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.CardPropertyDetailPage, "UI/UIPrefab/CommonUI/CardPropertyDetailPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.ResourceCommonItemDetailPage, "UI/UIPrefab/CommonUI/ResourceCommonItemDetailPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.ResourceCommonItemDetail2Page, "UI/UIPrefab/CommonUI/ResourceCommonItemDetail2Page", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.GainAwardShowPage, "UI/UIPrefab/CommonUI/GainAwardShowPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.AwardTipPage, "UI/UIPrefab/CommonUI/AwardTipPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.LabelTipPage, "UI/UIPrefab/CommonUI/Tip/LabelTipPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.LabelCenterTipPage, "UI/UIPrefab/CommonUI/Tip/LabelCenterTipPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.LabelPopupMenuPage, "UI/UIPrefab/CommonUI/Tip/LabelPopupMenuPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.PopupMenuPage, "UI/UIPrefab/CommonUI/Tip/PopupMenuPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.PopupMenuGoodsPage, "UI/UIPrefab/CommonUI/Tip/PopupMenuGoodsPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.TopPopupMenuPage, "UI/UIPrefab/CommonUI/Tip/TopPopupMenuPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.TopMoneyPage, "UI/UIPrefab/CommonUI/TopMoneyPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.ItemsShowPage, "UI/UIPrefab/CommonUI/ItemsShowPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.GetWayPage, "UI/UIPrefab/CommonUI/GetWayPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.BuyPage, "UI/UIPrefab/CommonUI/BuyPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.SellPage, "UI/UIPrefab/CommonUI/SellPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.ResetLevelWindowPage, "UI/UIPrefab/CommonUI/ResetLevelWindowPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.SelectPage, "UI/UIPrefab/CommonUI/SelectPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.MutiOperatePage, "UI/UIPrefab/CommonUI/MutiOperatePage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.ViewPlayerLineUpPage, "UI/UIPrefab/CommonUI/ViewPlayerLineUpPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.GoodUseAndBuyPage, "UI/UIPrefab/CommonUI/GoodUseAndBuyPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.WaitTipPage, "UI/UIPrefab/CommonUI/Tip/WaitTipPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.PayMaskPage, "UI/UIPrefab/CommonUI/Tip/PayMaskPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.PayTipPage, "UI/UIPrefab/CommonUI/Tip/PayTipPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.UseGoodOnekeyPage, "UI/UIPrefab/CommonUI/UseGoodOnekeyPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.BuyGoodOnekeyPage, "UI/UIPrefab/CommonUI/BuyGoodOnekeyPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.LevelUpPage, "UI/UIPrefab/CommonUI/LevelUpPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.AddProTripPage, "UI/UIPrefab/CommonUI/Tip/AddProTripPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.CommonBoxPage, "UI/UIPrefab/CommonUI/CommonBoxPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.AwakenEquipPage, "UI/UIPrefab/GameMain/Bag/AwakenEquipPage",strCommonUIRoot);
        //////////////////////////////////////////////////////////////////////////
        ccUIManage.GetInstance().f_RegProfab(UINameConst.FitSkillPage, "UI/UIPrefab/BattleScene/FitSkillPage", strBattleSceneUIRoot);

        ccUIManage.GetInstance().f_RegProfab(UINameConst.GameNoticePage, "UI/UIPrefab/GameMain/GameNotice/GameNoticePage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.ValentinesDayPage, "UI/UIPrefab/GameMain/NewYearAct/ValentinesDayPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.MallCardDisplayPage, "UI/UIPrefab/GameMain/Mall/MallCardDisplayPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.DungeonChallengePage, "UI/UIPrefab/GameMain/Dungeon/DungeonChallengePage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.CommonShowAward, "UI/UIPrefab/CommonUI/CommonShowAward", strCommonUIRoot);

        ccUIManage.GetInstance().f_RegProfab(UINameConst.OnlineVipPage, "UI/UIPrefab/GameMain/EventTime/OnlineVipPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.OnlineVoteAppPage, "UI/UIPrefab/GameMain/EventTime/OnlineVoteAppPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.RankingPowerPage, "UI/UIPrefab/GameMain/EventTime/RankingPowerPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.TripleMoneyPage, "UI/UIPrefab/GameMain/EventTime/TripleMoneyPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.LevelGiftPage, "UI/UIPrefab/GameMain/EventTime/LevelGiftPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.BattlePassPage, "UI/UIPrefab/GameMain/EventTime/BattlePassPage", strGameMainSceneUIRoot);
        // clone vòng sáng nghề
        ccUIManage.GetInstance().f_RegProfab(UINameConst.AuraTypePage, "UI/UIPrefab/GameMain/ClothArray/AuraTypePage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.AuraFiveElementsPage, "UI/UIPrefab/GameMain/ClothArray/AuraFiveElementsPage", strGameMainSceneUIRoot);

        ccUIManage.GetInstance().f_RegProfab(UINameConst.CrossTournamentMainPage, "UI/UIPrefab/GameMain/CrossTournament/CrossTournamentMainPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.CrossTournamentPage, "UI/UIPrefab/GameMain/CrossTournament/CrossTournamentPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.CrossTournamentDetailPage, "UI/UIPrefab/GameMain/CrossTournament/CrossTournamentDetailPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.CrossTournamentBetPage, "UI/UIPrefab/GameMain/CrossTournament/CrossTournamentBetPage", strGameMainSceneUIRoot);

        ccUIManage.GetInstance().f_RegProfab(UINameConst.LotteryLimitEventPage, "UI/UIPrefab/GameMain/EventTime/LotteryLimitEventPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.GainCardLimitShowPage, "UI/UIPrefab/GameMain/EventTime/GainCardLimitShowPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.SupermarketPage, "UI/UIPrefab/GameMain/Supermarket/SupermarketPage", strGameMainSceneUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.AuraMainPage, "UI/UIPrefab/GameMain/ClothArray/AuraMainPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.BuffDetailPage, "UI/UIPrefab/CommonUI/BuffDetailPage", strCommonUIRoot);
        ccUIManage.GetInstance().f_RegProfab(UINameConst.ShopPropsPage, "UI/UIPrefab/GameMain/ShopStore/ShopPropsPage", strGameMainSceneUIRoot);

        ccUIManage.GetInstance().f_RegProfab(UINameConst.Logo18Page, "UI/UIPrefab/Login/Logo18", strCommonUIRoot);
    }
}

