using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class ChallengeMenuPage : UIFramwork
{
    private GameObject mBtn_Arena;
    private GameObject mBtn_GrabTreasure;//群雄夺宝按钮
    private UIScrollView mMenuScrollView;
	bool ChallengeBgIsPlaying = true;
	//My Code
	GameParamDT AssetOpen;
	GameParamDT CardBattleOpen;
	int CardBattleOpenLvl = 30;
	//

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
		glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.UI_Challenge);
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mBtn_Arena = f_GetObject("Btn_Arena");
        mBtn_GrabTreasure = f_GetObject("Btn_GrabTreasure");
        mMenuScrollView = f_GetObject("MenuScrollView").GetComponent<UIScrollView>();
		//My Code
		AssetOpen = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(93) as GameParamDT);
		CardBattleOpen = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(96) as GameParamDT);
		CardBattleOpenLvl = CardBattleOpen.iParam3 * 1;
		// if(AssetOpen.iParam1 == 0)
		// {
			// if (GameObject.Find("CardBattle") != null)
			// {
				// GameObject.Find("CardBattle").SetActive(false);
			// }
			// if (GameObject.Find("GrabTreasure") != null)
			// {
				// GameObject.Find("GrabTreasure").SetActive(false);
			// }
		// }
		//
        f_RegClickEvent(mBtn_Arena, f_ArenaBtn);
        f_RegClickEvent(mBtn_GrabTreasure, f_GrabTreasureBtn);
        f_RegClickEvent("BackBtn", f_BackBtn);
        f_RegClickEvent("Btn_RunningMan", f_BtnRunningMan);
        f_RegClickEvent("Btn_Patrol", f_BtnPatrol);
        f_RegClickEvent("Btn_RebelArmy", f_Btn_RebelArmy);
        f_RegClickEvent("Btn_CrossServerBattle", f_OnBtnCrossServerBattleClick);
        f_RegClickEvent("Btn_CardBattle", f_OnBtnCardBattleClick);
        //f_RegClickEvent("Btn_TrialTower", f_OnBtnTrialTower);
        f_RegClickEvent("Btn_ChaosBattle", f_OnBtnChaosBattleClick); //TsuCode - ChaosBattle
        f_RegClickEvent("Btn_CrossArena", f_OnBtnCrossArenaClick);
        f_RegClickEvent("Btn_CrossTournament", f_OnBtnCrossTournamentClick);
		f_RegClickEvent("Btn_GrandArena", f_OnBtnGrandArenaClick);
		f_RegClickEvent("Btn_ClosePar2", f_OnBtnGrandArenaClose);
		f_RegClickEvent("BtnRight", f_OnBtnRightClick);
		f_RegClickEvent("BtnLeft", f_OnBtnLeftClick);
    }
    #region 红点提示
    protected override void InitRaddot()
    {
        base.InitRaddot();

        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.RunningManEliteLeftTimes, f_GetObject("Btn_RunningMan"), ReddotCallback_Show_Btn_RunningMan);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.TreasureCanFix, f_GetObject("Btn_GrabTreasure"), ReddotCallback_Show_Btn_GrabTreasure);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.PatrolGetAward, f_GetObject("Btn_Patrol"), ReddotCallback_Show_Btn_Patrol);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.RebelArmy, f_GetObject("Btn_RebelArmy"), ReddotCallback_Show_Btn_RebelArmy,true);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.RebelArmyAward, f_GetObject("Btn_RebelArmy"), ReddotCallback_Show_Btn_RebelArmy,true);
        UpdateReddotUI();

    }
    protected override void On_Destory()
    {
        base.On_Destory();
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.RunningManEliteLeftTimes, f_GetObject("Btn_RunningMan"));
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.TreasureCanFix, f_GetObject("Btn_GrabTreasure"));
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.PatrolGetAward, f_GetObject("Btn_Patrol"));
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.RebelArmy, f_GetObject("Btn_RebelArmy"),true);
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.RebelArmyAward, f_GetObject("Btn_RebelArmy"),true);
    }
    protected override void UpdateReddotUI()
    {
        base.UpdateReddotUI();

        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.RunningManEliteLeftTimes);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.TreasureCanFix);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.PatrolGetAward);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.RebelArmy);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.RebelArmyAward);
    }

    private void ReddotCallback_Show_Btn_RunningMan(object Obj)
    {
        int iNum = (int)Obj;
        GameObject Btn_RunningMan = f_GetObject("Btn_RunningMan");
        UITool.f_UpdateReddot(Btn_RunningMan, iNum, new Vector3(80, 150, 0), 200, 1, 1.5f);


    }

    private void ReddotCallback_Show_Btn_GrabTreasure(object Obj)
    {
        int iNum = (int)Obj;
        GameObject Btn_GrabTreasure = f_GetObject("Btn_GrabTreasure");
        UITool.f_UpdateReddot(Btn_GrabTreasure, iNum, new Vector3(10, 145, 0), 200, 1, 1.5f);
    }

    private void ReddotCallback_Show_Btn_Patrol(object obj)
    {
        int iNum = (int)obj;
        GameObject Btn_Patrol = f_GetObject("Btn_Patrol");
        UITool.f_UpdateReddot(Btn_Patrol, iNum, new Vector3(120, 250, 0), 200, 1, 1.5f);
    }
    private void ReddotCallback_Show_Btn_RebelArmy(object obj)
    {
        int iNum = (int)obj;
        GameObject Btn_Patrol = f_GetObject("Btn_RebelArmy");
		if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < UITool.f_GetSysOpenLevel(EM_NeedLevel.RebelArmyLevel))
        {
			
        }
		else
		{
			UITool.f_UpdateReddot(Btn_Patrol, iNum, new Vector3(80, 120, 0), 200, 1, 1.5f);
		}
    }
    #endregion
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        // Lấy thông tin giải đấu vấn đỉnh
        Data_Pool.m_CrossTournamentPool.f_Info();
        Data_Pool.m_CrossTournamentPool.f_GroupStageList();
        Data_Pool.m_CrossTournamentPool.f_AllKnockList();
        if (Data_Pool.m_GuidancePool.IsEnter)
        {
            if (Data_Pool.m_GuidancePool.IGuidanceID <= 2026)
            {
                f_GetObject("ScrollBar").GetComponent<UIScrollBar>().value = 0;
            }
            else
            {
                f_GetObject("ScrollBar").GetComponent<UIScrollBar>().value = 0.4f;
            }
        }
        //mMenuScrollView.ResetPosition();
        if (e != null && e is Battle2MenuProcessParam)
        {
            Battle2MenuProcessParam tParam = (Battle2MenuProcessParam)e;
            if (tParam.m_emType == EM_Battle2MenuProcess.Arena)
            {
                f_OpenArenaPage(true);
                tParam.f_UpdateParam(EM_Battle2MenuProcess.None);
                return;
            }
            else if (tParam.m_emType == EM_Battle2MenuProcess.GrabTreasure)
            {
                f_OpenTreasurePage();
                tParam.f_UpdateParam(EM_Battle2MenuProcess.None);
                return;
            }
            else if (tParam.m_emType == EM_Battle2MenuProcess.RunningMan)
            {
                ccUIHoldPool.GetInstance().f_Hold(this);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManPage, UIMessageDef.UI_OPEN);
                tParam.f_UpdateParam(EM_Battle2MenuProcess.None);
            }
            else if (tParam.m_emType == EM_Battle2MenuProcess.RunningManElite)
            {
                ccUIHoldPool.GetInstance().f_Hold(this);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManPage, UIMessageDef.UI_OPEN, tParam);
                return;
            }
            else if (tParam.m_emType == EM_Battle2MenuProcess.Patrol)
            {
                ccUIHoldPool.GetInstance().f_Hold(this);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolPage, UIMessageDef.UI_OPEN, tParam);
                return;
            }
            else if (tParam.m_emType == EM_Battle2MenuProcess.CrossServerBattle)
            {
                tParam.f_UpdateParam(EM_Battle2MenuProcess.None);
                f_OnBtnCrossServerBattleClick(null, null, null);
                return;
            }
            else if (tParam.m_emType == EM_Battle2MenuProcess.CardBattle)
            {
                tParam.f_UpdateParam(EM_Battle2MenuProcess.None);
                f_OnBtnCardBattleClick(null, null, null);
                return;
            } else if (tParam.m_emType == EM_Battle2MenuProcess.TrialTower) {
                tParam.f_UpdateParam(EM_Battle2MenuProcess.None);
                ccUIHoldPool.GetInstance().f_Hold(this);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.TrialTowerPage, UIMessageDef.UI_OPEN);
            }
            //TsuCode - ChaosBattle
            else if (tParam.m_emType == EM_Battle2MenuProcess.ChaosBattle)
            {
                tParam.f_UpdateParam(EM_Battle2MenuProcess.None);
                f_OnBtnChaosBattleClick(null, null, null);
                return;
            }
            else if (tParam.m_emType == EM_Battle2MenuProcess.ArenaCrossBattle)
            {
                tParam.f_UpdateParam(EM_Battle2MenuProcess.None);
                f_OnBtnCrossArenaClick(null, null, null);
                return;
            }
            //
        }
        f_LoadTexture();
        Data_Pool.m_PatrolPool.f_CheckPatrolReddot();
    }

    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
		if(!ChallengeBgIsPlaying)
		{
			glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.UI_Challenge);
			ChallengeBgIsPlaying = true;
			StaticValue.m_MusicName = "UI_Challenge";
		}
        f_LoadTexture();
        Data_Pool.m_PatrolPool.f_CheckPatrolReddot();
    }
	//My Code
    //private string strTexBgRoot = "UI/TextureRemove/Challenge/Tex_ChallengeMenuBg";
	private string strTexBgRoot = "UI/TextureRemove/Challenge/Tex_ChallengeMenuBg";
	//
    private string strTexArenaIconRoot = "UI/TextureRemove/Challenge/icon_huizhang_1";
    private string strTex_SnatchIconRoot = "UI/TextureRemove/Challenge/icon_huizhang_2";
    private string strTex_RunningManIconRoot = "UI/TextureRemove/Challenge/icon_huizhang_3";
    private string strTex_PatrolIconRoot = "UI/TextureRemove/Challenge/icon_huizhang_4";
    private string strTex_RebelArmyIconRoot = "UI/TextureRemove/Challenge/icon_huizhang_5";
    private string strTex_CrossServerBattleIconRoot = "UI/TextureRemove/Challenge/icon_huizhang_6";
    private string strTex_CardBattleIconRoot = "UI/TextureRemove/Challenge/icon_huizhang_7";
    private string strTex_TrialTowerIconRoot = "UI/TextureRemove/Challenge/Tex_TowerHeroes";

    private string strTexArenaIconRoot_Lock = "UI/TextureRemove/Challenge/Tex_ArenaIcon_Lock";
    private string strTex_SnatchIconRoot_Lock = "UI/TextureRemove/Challenge/Tex_SnatchIcon_Lock";
    private string strTex_RunningManIconRoot_Lock = "UI/TextureRemove/Challenge/Tex_RunningManIcon_Lock";
    private string strTex_PatrolIconRoot_Lock = "UI/TextureRemove/Challenge/Tex_PatrolIcon_Lock";
    private string strTex_RebelArmyIconRoot_Lock = "UI/TextureRemove/Challenge/Tex_RebelArmyIcon_Lock";
    private string strTex_CrossServerBattleIconRoot_Lock = "UI/TextureRemove/Challenge/Tex_CrossServerBattleIcon_Lock";
    private string strTex_CardBattleIconRoot_Lock = "UI/TextureRemove/Challenge/Tex_CardBattleIcon_Lock";
    private string strTex_TrialTowerIconRoot_Lock = "UI/TextureRemove/Challenge/Tex_TowerHeroes";
    //TsuCode - Chaosbattle
    private string strTex_ChaosBattleIconRoot = "UI/TextureRemove/Challenge/Tex_TrialTowerIcon";
    private string strTex_ArenaCrossIconRoot = "UI/TextureRemove/Challenge/Tex_ArenaCrossIcon";
    private string strTex_ChaosBattleIconRoot_Lock = "UI/TextureRemove/Challenge/Tex_ChaosBattleIcon_Lock";
    //
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        //加载背景图
		int curLv = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        UITexture Texture_BG = f_GetObject("Texture_BG").GetComponent<UITexture>();
        // if (Texture_BG.mainTexture == null)
        // {
            Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot);

            // f_GetObject("Btn_Arena").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexArenaIconRoot);
            // f_GetObject("Btn_GrabTreasure").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTex_SnatchIconRoot);
            // f_GetObject("Btn_RunningMan").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTex_RunningManIconRoot);
            // f_GetObject("Btn_Patrol").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTex_PatrolIconRoot);
            // f_GetObject("Btn_RebelArmy").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTex_RebelArmyIconRoot);
            // f_GetObject("Btn_CrossServerBattle").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTex_CrossServerBattleIconRoot);
            // f_GetObject("Btn_CardBattle").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTex_CardBattleIconRoot);
            // f_GetObject("Btn_TrialTower").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTex_TrialTowerIconRoot);
            //TsuCode
            // f_GetObject("Btn_ChaosBattle").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTex_TrialTowerIconRoot);
            //
            // if (AssetOpen.iParam1 == 1)
			// {
				//tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("Tex_ChallengeMenuBg");
				// f_GetObject("Btn_Arena").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("Tex_ArenaIcon");
				// f_GetObject("Btn_GrabTreasure").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("Tex_SnatchIcon");
				// f_GetObject("Btn_RunningMan").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("Tex_RunningManIcon");
				// f_GetObject("Btn_Patrol").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("Tex_PatrolIcon");
				// f_GetObject("Btn_RebelArmy").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("Tex_RebelArmyIcon");
				// f_GetObject("Btn_CrossServerBattle").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("Tex_CrossServerBattleIcon");
				// f_GetObject("Btn_CardBattle").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("Tex_CardBattleIcon");
				// f_GetObject("Btn_TrialTower").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTex_TrialTowerIconRoot);
				// f_GetObject("Btn_ChaosBattle").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTex_ChaosBattleIconRoot);
				// f_GetObject("Btn_CrossArena").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTex_ArenaCrossIconRoot);// can doi path
                // f_GetObject("Btn_CrossTournament").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTex_TrialTowerIconRoot);// can doi path
            //}
            Texture_BG.mainTexture = tTexture2D;
        // }

		if (Data_Pool.m_ArenaPool.f_CheckArenaLvLimit(curLv))
        {
            GameObject isLock = f_GetObject("Btn_Arena").transform.Find("isLock").gameObject;
            if (isLock)
            {
                isLock.SetActive(true);
            }
            else
            {
                UITool.f_SetTextureGray(f_GetObject("Btn_Arena").GetComponent<UITexture>(), true);
            }
        }
		if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < UITool.f_GetSysOpenLevel(EM_NeedLevel.GrabTreasureLevel))
		{
            UITool.f_SetTextureGray(f_GetObject("Btn_GrabTreasure").GetComponent<UITexture>(), true);
        }
		if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < UITool.f_GetSysOpenLevel(EM_NeedLevel.RunningManLvel))
		{
			UITool.f_SetTextureGray(f_GetObject("Btn_RunningMan").GetComponent<UITexture>(), true);
        }
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < UITool.f_GetSysOpenLevel(EM_NeedLevel.PatrolLevel))
		{
			UITool.f_SetTextureGray(f_GetObject("Btn_Patrol").GetComponent<UITexture>(), true);
        }
		if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < UITool.f_GetSysOpenLevel(EM_NeedLevel.RebelArmyLevel))
        {
			UITool.f_SetTextureGray(f_GetObject("Btn_RebelArmy").GetComponent<UITexture>(), true);
        }
		if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < UITool.f_GetSysOpenLevel(EM_NeedLevel.CrossServerBattle))
        {
            GameObject isLock = f_GetObject("Btn_CrossServerBattle").transform.Find("isLock").gameObject;
            if (isLock)
            {
                isLock.SetActive(true);
            }
            else
            {
                UITool.f_SetTextureGray(f_GetObject("Btn_CrossServerBattle").GetComponent<UITexture>(), true);
            }
        }
		if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < CardBattleOpenLvl)
        {
            GameObject isLock = f_GetObject("Btn_CardBattle").transform.Find("isLock").gameObject;
            if (isLock)
            {
                isLock.SetActive(true);
            }
            else
            {
                UITool.f_SetTextureGray(f_GetObject("Btn_CardBattle").GetComponent<UITexture>(), true);
            }
        }
		if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < UITool.f_GetSysOpenLevel(EM_NeedLevel.TritalTowerOpenLV) || Data_Pool.m_TrialTowerPool.mNowEndTime - GameSocket.GetInstance().f_GetServerTime() <= 0) 
		{
			UITool.f_SetTextureGray(f_GetObject("Btn_TrialTower").GetComponent<UITexture>(), true);
        }
		if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < UITool.f_GetSysOpenLevel(EM_NeedLevel.ChaosBattle)) 
		{
            GameObject isLock = f_GetObject("Btn_ChaosBattle").transform.Find("isLock").gameObject;
            if (isLock)
            {
                isLock.SetActive(true);
            }
            else
            {
                UITool.f_SetTextureGray(f_GetObject("Btn_ChaosBattle").GetComponent<UITexture>(), true);
            }
        }
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < UITool.f_GetSysOpenLevel(EM_NeedLevel.CrossArena) || !CommonTools.f_CheckOpenServerDay((glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_NeedLevel.CrossArena) as GameParamDT).iParam2))
        {
            GameObject isLock = f_GetObject("Btn_CrossArena").transform.Find("isLock").gameObject;
            if (isLock)
            {
                isLock.SetActive(true);
            }
            else
            {
                UITool.f_SetTextureGray(f_GetObject("Btn_CrossArena").GetComponent<UITexture>(), true);
            }
        }
        if (curLv < UITool.f_GetSysOpenLevel(EM_NeedLevel.CrossTournament))
        {
            GameObject isLock = f_GetObject("Btn_CrossTournament").transform.Find("isLock").gameObject;
            if (isLock)
            {
                isLock.SetActive(true);
            }
            else
            {
                UITool.f_SetTextureGray(f_GetObject("Btn_CrossTournament").GetComponent<UITexture>(), true);
            }
        }
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    private void f_BackBtn(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ChallengeMenuPage, UIMessageDef.UI_CLOSE);
		glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.MainBg);
        ccUIHoldPool.GetInstance().f_UnHold();
    }

    #region 竞技场相关处理

    private void f_ArenaBtn(GameObject go, object value1, object value2)
    {
        int curLv = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        if (Data_Pool.m_ArenaPool.f_CheckArenaLvLimit(curLv))
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(736), UITool.f_GetSysOpenLevel(EM_NeedLevel.ArenaLevel)));//竞技场{0}级开放
            return;
        }
		ChallengeBgIsPlaying = false;
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_SendArenaListResult;
        socketCallbackDt.m_ccCallbackFail = f_SendArenaListResult;
        Data_Pool.m_ArenaPool.f_ArenaList(socketCallbackDt);
        UITool.f_OpenOrCloseWaitTip(true);
    }

    private void f_SendArenaListResult(object value)
    {
        int result = (int)value;
        UITool.f_OpenOrCloseWaitTip(false);
        if (result == (int)eMsgOperateResult.OR_Succeed)
        {
            f_OpenArenaPage(false);
        }
        else
        {
            UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(737) + result);//竞技场初始化失败
        }
    }

    private void f_OpenArenaPage(bool updateBySelf)
    {
        //打开竞技场界面
        ccUIHoldPool.GetInstance().f_Hold(this);
        if (updateBySelf)
            ccUIManage.GetInstance().f_SendMsg(UINameConst.ArenaPageNew, UIMessageDef.UI_OPEN, updateBySelf);
        else
            ccUIManage.GetInstance().f_SendMsg(UINameConst.ArenaPageNew, UIMessageDef.UI_OPEN);
    }
    private void f_OpenTreasurePage()
    {
        //打开夺宝界面
        ccUIHoldPool.GetInstance().f_Hold(this);
        if (GrabTreasurePage.curSelectTreasure != null)
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GrabTreasurePage, UIMessageDef.UI_OPEN, GrabTreasurePage.curSelectTreasure.iId);
        else
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GrabTreasurePage, UIMessageDef.UI_OPEN);
    }

    #endregion
    #region 群雄夺宝相关
    /// <summary>
    /// 点击群雄夺宝按钮
    /// </summary>
    private void f_GrabTreasureBtn(GameObject go, object obj1, object obj2)
    {
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < UITool.f_GetSysOpenLevel(EM_NeedLevel.GrabTreasureLevel))
        {
            // UITool.Ui_Trip("群雄夺宝" + UITool.f_GetSysOpenLevel(EM_NeedLevel.GrabTreasureLevel) + "级开放");//群雄夺宝{0}级开放
            UITool.Ui_Trip(string.Format( CommonTools.f_GetTransLanguage(738),UITool.f_GetSysOpenLevel(EM_NeedLevel.GrabTreasureLevel)));
            return;
        }
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GrabTreasurePage, UIMessageDef.UI_OPEN);
    }
    #endregion

    #region 过关斩将(RunningMan)

    private void f_BtnRunningMan(GameObject go, object value1, object value2)
    {
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < UITool.f_GetSysOpenLevel(EM_NeedLevel.RunningManLvel))
        {
            //UITool.Ui_Trip("过关斩将" + UITool.f_GetSysOpenLevel(EM_NeedLevel.RunningManLvel) + "级开放");
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(740),UITool.f_GetSysOpenLevel(EM_NeedLevel.RunningManLvel)));
            return;
        }
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManPage, UIMessageDef.UI_OPEN);
    }

    #endregion

    private void f_BtnPatrol(GameObject go, object value1, object value2)
    {
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < UITool.f_GetSysOpenLevel(EM_NeedLevel.PatrolLevel))
        {
            // UITool.Ui_Trip("古都巡礼" + UITool.f_GetSysOpenLevel(EM_NeedLevel.PatrolLevel) + "级开放");
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(739),UITool.f_GetSysOpenLevel(EM_NeedLevel.PatrolLevel)));
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_PatrolInit;
        socketCallbackDt.m_ccCallbackFail = f_Callback_PatrolInit;
        Data_Pool.m_PatrolPool.f_PatrolInit(0, socketCallbackDt);
    }

    private void f_Callback_PatrolInit(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            ccUIHoldPool.GetInstance().f_Hold(this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolPage, UIMessageDef.UI_OPEN, Data_Pool.m_PatrolPool.m_SelfPatrolDt);
        }
        else
        {
            UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(741) + result);//领地巡逻数据初始化失败
        }
    }
    #region 叛军入侵
    private void f_Btn_RebelArmy(GameObject go, object obj1, object obj2)
    {
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < UITool.f_GetSysOpenLevel(EM_NeedLevel.RebelArmyLevel))
        {
            //UITool.Ui_Trip("叛军入侵" + UITool.f_GetSysOpenLevel(EM_NeedLevel.RebelArmyLevel) + "级开放");
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(742),UITool.f_GetSysOpenLevel(EM_NeedLevel.RebelArmyLevel)));
            return;
        }
        //Data_Pool.m_ReddotMessagePool.f_MsgSubtract(EM_ReddotMsgType.RebelArmy);
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RebelArmy, UIMessageDef.UI_OPEN);
    }
    #endregion

    private void f_OnBtnCrossServerBattleClick(GameObject go, object value1, object value2)
    {
        //内部判断等级限制
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CrossServerBattlePage, UIMessageDef.UI_OPEN);
    }

    private void f_OnBtnCardBattleClick(GameObject go, object value1, object value2)
    {
		if((Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < CardBattleOpenLvl))
		{
UITool.Ui_Trip("Cấp " + CardBattleOpenLvl + " mở");
		}
		else
		{
			ccUIHoldPool.GetInstance().f_Hold(this);
			ccUIManage.GetInstance().f_SendMsg(UINameConst.CardBattlePage, UIMessageDef.UI_OPEN);
		}
    }

    private void f_OnBtnTrialTower(GameObject go, object value1, object value2)
    {
        if (!UITool.f_GetIsOpensystem(EM_NeedLevel.TritalTowerOpenLV)) {
UITool.Ui_Trip("Cấp không đủ");
            return;
        }

        if (!(Data_Pool.m_TrialTowerPool.isOpen))
        {
UITool.Ui_Trip("Chưa mở");
            return;
        }

        if (!Data_Pool.m_TrialTowerPool.isEnter)
        {
            ccUIHoldPool.GetInstance().f_Hold(this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.TrialTowerRoomPage, UIMessageDef.UI_OPEN);
        }
        else {
            ccUIHoldPool.GetInstance().f_Hold(this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.TrialTowerPage, UIMessageDef.UI_OPEN);
        }
    }

    private void f_UpdateTexture()
    {
        UITool.f_SetTextureGray(f_GetObject("Btn_TrialTower").GetComponent<UITexture>(),!(Data_Pool.m_TrialTowerPool.isOpen));


    }
    //TsuCode - ChaosBattle
    private void f_OnBtnChaosBattleClick(GameObject go, object value1, object value2)
    {
        //内部判断等级限制
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ChaosBattlePage, UIMessageDef.UI_OPEN);
    }

    private void f_OnBtnCrossArenaClick(GameObject go, object value1, object value2)
    {
        GameParamDT gameParamDT = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_NeedLevel.CrossArena);

        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < UITool.f_GetSysOpenLevel(EM_NeedLevel.CrossArena) || !CommonTools.f_CheckOpenServerDay(gameParamDT.iParam2))
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(2310), gameParamDT.iParam1, gameParamDT.iParam2));
            return;
        }
        Data_Pool.m_CrossArenaPool.f_ArenaList();
        UITool.f_OpenOrCloseWaitTip(true);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_CrossArenaPool, f_SendCrossArenaListResult, this);
    }

    private void f_SendCrossArenaListResult(object value)
    {
        int result = (int)value;
        UITool.f_OpenOrCloseWaitTip(false);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_CrossArenaPool, f_SendCrossArenaListResult, this);
        if (result >= 1)
        {
            f_OpenCrossArenaPage(false);
        }
        else
        {
            UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(737) + result);//竞技场初始化失败
        }
    }

    private void f_OpenCrossArenaPage(bool updateBySelf)
    {
        //打开竞技场界面
        ccUIHoldPool.GetInstance().f_Hold(this);

        if (updateBySelf)
            ccUIManage.GetInstance().f_SendMsg(UINameConst.ArenaCrossPage, UIMessageDef.UI_OPEN, updateBySelf);
        else
            ccUIManage.GetInstance().f_SendMsg(UINameConst.ArenaCrossPage, UIMessageDef.UI_OPEN);
    }

    private void f_OnBtnCrossTournamentClick(GameObject go, object value1, object value2)
    {
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < UITool.f_GetSysOpenLevel(EM_NeedLevel.CrossTournament))
        {
            UITool.Ui_Trip("Level 55 mở!");
            return;
        }
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CrossTournamentMainPage, UIMessageDef.UI_OPEN);
        
    }

	private void f_OnBtnGrandArenaClick(GameObject go, object value1, object value2)
    {
        f_GetObject("GrandArenaPanel").SetActive(true);
    }
	private void f_OnBtnGrandArenaClose(GameObject go, object value1, object value2)
    {
        f_GetObject("GrandArenaPanel").SetActive(false);
    }
	private void f_OnBtnRightClick(GameObject go, object value1, object value2)
    {
		f_GetObject("ArenaPage1").SetActive(false);
        f_GetObject("ArenaPage2").SetActive(true);
    }
	private void f_OnBtnLeftClick(GameObject go, object value1, object value2)
    {
        f_GetObject("ArenaPage1").SetActive(true);
		f_GetObject("ArenaPage2").SetActive(false);
    }
}
