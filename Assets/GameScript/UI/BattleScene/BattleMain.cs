using ccU3DEngine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using DG.Tweening;
using Spine.Unity;

public class BattleMain : UIFramwork
{
    //TsuCode
    private bool flag = false;
    //
    private GameObject[] m_aPos;

    //未开始战斗 遮挡场景
    private GameObject mTopMask;

    private GameObject mBattleStart;
    private GameObject mRoleBeforeDeath;
    private GameObject mBattleFinish;
    private int _iSpeed = 1;
    private TextControl _TextControl;
    private UIPanel _MainUIPanel;
    private UILabel mPopText;
	//My Code
	GameParamDT AssetOpen;
	GameObject TexBgKD;
	GameParamDT VipCheck;

    private GameObject mStatusView;
    private bool isStatusView = false;
    //
    //int LvCheck = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
    //int VipLvCheck = UITool.f_GetVipLv(Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Vip));
    int LvCheck = 1;
	int VipLvCheck = 1;
    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        if(StaticValue.m_isPlayMusic)
        {
            glo_Main.GetInstance().m_AdudioManager.f_UnPauseAudioMusic();
        }
        if(StaticValue.m_isPlaySound)
        {
            glo_Main.GetInstance().m_AdudioManager.f_UnPauseAudioButtle();
        }
		LvCheck = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
		VipLvCheck = UITool.f_GetNowVipLv(); //UITool.f_GetVipLv(Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Vip));
        MessageBox.DEBUG("BattleMain");
		//My Code
		AssetOpen = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(93) as GameParamDT);
		VipCheck = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(110) as GameParamDT);
		MessageBox.ASSERT("VIP NEED: " + VipCheck.iParam1 + " " + VipCheck.iParam2 + " " + VipCheck.iParam3);
		//
        InitGUI();
        InitSpeed();
        InitBg();
        MessageBox.DEBUG("BattleMain End");
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        bool isHavePlot = Data_Pool.m_DungeonPool.f_JudgeIsHavePlot();
        mBattleFinish = f_GetObject("BattleFinishBtn");
        // mBattleFinish.SetActive(!isHavePlot);
		mBattleFinish.SetActive(false);
        f_RegClickEvent(mBattleFinish, f_BattleFinishHandle);
        f_RegClickEvent("BattleSpeedBtn", On_BattleSpeedBtn);
        f_RegClickEvent("BattleSpeedBtn2", On_BattleSpeedBtn2);
        f_RegClickEvent("BattlePauseBtn", On_BattlePauseBtn);
        AddEventListener(UIMessageDef.UI_SETBATTLEUIDEPTH, CallBack_SetBattleDepth);
        _MainUIPanel = _Panel.GetComponent<UIPanel>();
        mTopMask = f_GetObject("TopMask");       
        // mTopMask.SetActive(!isHavePlot);
        mPopText = f_GetObject("PopText").GetComponent<UILabel>();
        f_RegClickEvent("StatusBtn", On_StatusBtn);
        f_RegClickEvent("BlackBG", On_StatusBtn);
        f_RegClickEvent("StatusCloseBtn", On_StatusBtn);
        mStatusView = f_GetObject("StatusView");
        isStatusView = false;
        mStatusView.SetActive(isStatusView);
        f_RegClickEvent("BtnAuraShowA", On_AuraShowBtn, 1);
        f_RegClickEvent("BtnAuraShowB", On_AuraShowBtn, 2);
    }
    private void On_AuraShowBtn(GameObject go, object obj1, object obj2)
    {
        int team = (int)obj1;
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.AuraMainPage, UIMessageDef.UI_OPEN);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.BATTLE_SHOW_AURA_DETAIL, team);
    }
    private void On_StatusBtn(GameObject go, object obj1, object obj2)
    {
        isStatusView = !isStatusView;
        mStatusView.SetActive(isStatusView);
    }

    private void InitBg()
    {
        int fiveEle = RolePropertyTools.f_GetElementalSeason();
        string BattleBg = "battle_"+ fiveEle;//"battlebg" + _GetBattleBgID(StaticValue.m_CurBattleConfig.m_eBattleType);
        _ChangeAudio(StaticValue.m_CurBattleConfig.m_eBattleType);
        GameObject BattleBG = f_GetObject("BattleBG");
        MessageBox.DEBUG("BattleBg " + BattleBg);
        // SpriteRenderer tSpriteRenderer = BattleBG.GetComponent<SpriteRenderer>();
        // Sprite tSprite = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite(BattleBg);
        Sprite tSprite = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite(BattleBg);
		MessageBox.ASSERT("BattleBG: " + tSprite);

		//My Code
		TexBgKD = GameObject.Find("TexBgKD");
		if(TexBgKD != null)
		{
			if(AssetOpen.iParam2 == 0)
			{
				TexBgKD.SetActive(false);
			}
		}
		
		f_GetObject("BattleFinishBtn").SetActive(false);
		//
        if(tSprite == null)
        {
MessageBox.ASSERT(CommonTools.f_GetTransLanguage(2125) + ", default.");
			BattleBG.GetComponent<UI2DSprite>().sprite2D = Resources.Load<Sprite>("UI/TextureRemove/Tex_BattleBg");
        }
        else
        {
			//tSpriteRenderer.sprite = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite("UI/TextureRemove/Tex_BattleBg");
			// if(AssetOpen.iParam1 == 1)
			// {
			BattleBG.GetComponent<UI2DSprite>().sprite2D = glo_Main.GetInstance().m_ResourceManager.f_CreateSprite(BattleBg);
			// }
        }
        // show mùa nguyên tố
        //int fiveEle = RolePropertyTools.f_GetElementalSeason();
        string nameEle = UITool.f_GetFiveElementNameById(fiveEle);
        UILabel LabelElement = f_GetObject("LabelElement").GetComponent<UILabel>();
        LabelElement.text = "Mùa "+ nameEle;
    }

    private int _GetBattleBgID(EM_Fight_Enum tBattleType)
    {
        switch(tBattleType)
        {
            case EM_Fight_Enum.eFight_Invalid:
                break;
            case EM_Fight_Enum.eFight_DungeonMain:
            case EM_Fight_Enum.eFight_DungeonElite:
            case EM_Fight_Enum.eFight_Legend:
                {
                    return StaticValue.m_CurBattleConfig.m_iSceneId;
                }
            case EM_Fight_Enum.eFight_DailyPve:
                return (int)EM_BattleBgId.DailyPve;
            case EM_Fight_Enum.eFight_Rebel:
                return (int)EM_BattleBgId.Rebel;
            case EM_Fight_Enum.eFight_Friend:
                break;
            case EM_Fight_Enum.eFight_Guild:
                return (int)EM_BattleBgId.LegionDungeon;
            case EM_Fight_Enum.eFight_Arena:
                return (int)EM_BattleBgId.ArenaOrGrabTreasure;
            case EM_Fight_Enum.eFight_ArenaSweep:
                break;
            case EM_Fight_Enum.eFight_CrusadeRebel:
                return (int)EM_BattleBgId.Rebel;
            case EM_Fight_Enum.eFight_Boss:
                break;
            case EM_Fight_Enum.eFight_LegionDungeon:
                break;
            case EM_Fight_Enum.eFight_GrabTreasure:
                return (int)EM_BattleBgId.DailyPve;
            case EM_Fight_Enum.eFight_GrabTreasureSweep:
                break;
            case EM_Fight_Enum.eFight_RunningMan:
                return (int)EM_BattleBgId.RunningMan;
            case EM_Fight_Enum.eFight_RunningManElite:
                return (int)EM_BattleBgId.RunningMan;
            case EM_Fight_Enum.eFight_Patrol:
                return (int)EM_BattleBgId.Patrol;
			case EM_Fight_Enum.eFight_CrossServerBattle:
                return (int)EM_BattleBgId.CrossServerBattle;
			case EM_Fight_Enum.eFight_ChaosBattle:
            case EM_Fight_Enum.eFight_ArenaCross:
                return (int)EM_BattleBgId.ChaosBattle;
        }
        return (int)EM_BattleBgId.Invalid;
    }
    private void _ChangeAudio(EM_Fight_Enum tBattleType)
    {
        switch(tBattleType)
        {

            case EM_Fight_Enum.eFight_Invalid:
            case EM_Fight_Enum.eFight_DungeonMain:
            case EM_Fight_Enum.eFight_DungeonElite:
            case EM_Fight_Enum.eFight_Legend:
            case EM_Fight_Enum.eFight_DailyPve:
                glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.BattleMusic);
                break;
            case EM_Fight_Enum.eFight_CrusadeRebel:
            case EM_Fight_Enum.eFight_RunningManElite:
            case EM_Fight_Enum.eFight_RunningMan:
            case EM_Fight_Enum.eFight_GrabTreasure:
            case EM_Fight_Enum.eFight_Rebel:
            case EM_Fight_Enum.eFight_Arena:
                glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.challenge);
                break;
            case EM_Fight_Enum.eFight_Friend:
            case EM_Fight_Enum.eFight_Guild:
            case EM_Fight_Enum.eFight_ArenaSweep:
            case EM_Fight_Enum.eFight_Boss:
            case EM_Fight_Enum.eFight_LegionDungeon:
            case EM_Fight_Enum.eFight_GrabTreasureSweep:


            case EM_Fight_Enum.eFight_Patrol:
            default:
                glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.BattleMusic);
                break;
        }
    }
	Dictionary<int, GameObject> roleDict = new Dictionary<int, GameObject>();

    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        roleDict.Clear();
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_DIALOG_BATTLESTART, f_BattleStart, this);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_DIALOG_ROLEBEFOREDEATH, f_RoleBeforeDeath, this);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_DIALOG_BATTLEFINISH, f_BattleFinish, this);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_BATTLE_TURNINFOR, On_UpdateTurns, this);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_BATTLE_ACTIVE, SkillEffect, this);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_BATTLE_START, (o) => { mBattleFinish.SetActive(false); }, this);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_BATTLE_SKILL_OPEN, (o) => {
            f_GetObject("SkillMask").SetActive(true);
            if (o != null) (o as Action).Invoke();
        }, this);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_BATTLE_SKILL_CLOSE, (o) => {
            f_GetObject("SkillMask").SetActive(false); ;
        }, this);
    }

    private void InitSpeed()
    {
        _iSpeed = LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_BattleSpeed, 1);
        DoSpeed();
    }
    /// <summary>
    /// 暂时隐藏场景和提示
    /// </summary>
    /// <param name="obj"></param>
    private void SkillEffect(object obj)
    {
        bool IsActive = (bool)obj;
        this.gameObject.SetActive(IsActive);
        f_GetObject("BattleMap").SetActive(IsActive);
        f_GetObject("AdapterBg").SetActive(IsActive);
    }
    private void On_BattleSpeedBtn(GameObject go, object obj1, object obj2)
    {
        bool IsReturn = false;
        string Sz = "";


        if(GloData.glo_StarGuidance)
        {
            if(!UITool.f_GetIsOpensystem(EM_NeedLevel.BattleNumLevel))
            {
                IsReturn = true;
                Sz = string.Format(CommonTools.f_GetTransLanguage(2126), UITool.f_GetSysOpenLevel(EM_NeedLevel.BattleNumLevel));
            }
            _iSpeed++;
            if(_iSpeed == 2)
            {
                GameParamDT tGameParamDT = UITool.f_GetGameParamDT((int)EM_GameParamType.VipX2);
                if (VipLvCheck < tGameParamDT.iParam1)
                {
                    Sz = string.Format(CommonTools.f_GetTransLanguage(2127), 2,tGameParamDT.iParam1);
                    IsReturn = true;
                }
                
            }
            else if(_iSpeed == 3)
            {
				int tNeedVip = Data_Pool.m_RechargePool.f_GetVipLvLimit(EM_VipPrivilege.eVip_BattleTrebleSpeed);
                MessageBox.ASSERT("VIP: " + tNeedVip + " " + VipLvCheck);
                // if(Data_Pool.m_RechargePool.f_GetCurLvVipPriValue(EM_VipPrivilege.eVip_BattleTrebleSpeed) < tNeedVip && !UITool.f_GetIsOpensystem(EM_NeedLevel.BattleNum3Level))
				if(VipLvCheck < tNeedVip)
                {
                    _iSpeed = 2;
                    IsReturn = true;
                    Sz = "";
                    int tNeedLv = UITool.f_GetSysOpenLevel(EM_NeedLevel.BattleNum3Level);
                    // int tNeedVip = Data_Pool.m_RechargePool.f_GetVipLvLimit(EM_VipPrivilege.eVip_BattleTrebleSpeed);
                    if(tNeedLv == 0)
                    {
                        //UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(2127), tNeedVip));
                        Sz = string.Format(CommonTools.f_GetTransLanguage(2127), 3, tNeedVip);
                        mPopText.text = Sz;
                    //mPopText.text = string.Format("[000000]3倍加速[ff0000]Vip{0}[-]开放[-]", tNeedVip);
                    }
                    else if(tNeedVip == 0)
                    {
                        //UITool.Ui_Trip(mPopText.text = string.Format(CommonTools.f_GetTransLanguage(2128), tNeedLv));
                        // Sz = string.Format(CommonTools.f_GetTransLanguage(2128), tNeedLv);
						Sz = string.Format(CommonTools.f_GetTransLanguage(2127), 3, tNeedVip);
                        mPopText.text = Sz;
                    //mPopText.text = string.Format("[000000]3倍加速[ff0000]{0}级[-]开放[-]", tNeedLv);
                    }
                    else
                    {
                        //UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(2129), tNeedLv, tNeedVip));
                        // Sz = string.Format(CommonTools.f_GetTransLanguage(2129), tNeedLv, tNeedVip);
						Sz = string.Format(CommonTools.f_GetTransLanguage(2127), 3, tNeedVip);
                        mPopText.text = Sz;
                        //mPopText.text = string.Format("[000000]3倍加速[ff0000]{0}级[-]或者\n[ff0000]Vip{1}[-]开放[-]", tNeedLv, tNeedVip);
                    }
                    //f_GetObject("PopTextBox").SetActive(true);
                    ccTimeEvent.GetInstance().f_RegEvent(1.5f, false, null, _DelayActive);
                }
            }

            if(IsReturn)
            {
                if(_TextControl == null)
                {
                    _TextControl = f_GetObject("CurTurns").GetComponent<TextControl>();
                    _TextControl.IsRole = false;
                }
                UITool.Ui_Trip(Sz);
                _iSpeed--;
                DoSpeed();
                return;
            }
        }
        else
        {
            _iSpeed++;
        }
        if(_iSpeed > 3)
        {
            _iSpeed = 1;
        }
        LocalDataManager.f_SetLocalData<int>(LocalDataType.Int_BattleSpeed, _iSpeed);
        DoSpeed();
    }
    private void _DelayActive(object obj)
    {
        f_GetObject("PopTextBox").SetActive(false);
    }
    private void DoSpeed()
    {

        UISprite SpeedText = f_GetObject("BattleSpeedBtn").GetComponent<UISprite>();
        SpeedText.spriteName = "x" + _iSpeed;
        SpeedText.MakePixelPerfect();
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_BATTLE_SPEED, _iSpeed);
    }

    private bool _bIsPauses = false;
    private void On_BattlePauseBtn(GameObject go, object obj1, object obj2)
    {
        if(_bIsPauses)
        {
            _bIsPauses = false;
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_BATTLE_SPEED, 1);
        }
        else
        {
            _bIsPauses = true;
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_BATTLE_SPEED, 0);
        }

    }

    private void On_BattleSpeedBtn2(GameObject go, object obj1, object obj2)
    {
        UISprite SpeedText = f_GetObject("BattleSpeedBtn").GetComponent<UISprite>();
        SpeedText.spriteName = "xE";
        SpeedText.MakePixelPerfect();
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_BATTLE_SPEED, 99);
    }

    private void On_UpdateTurns(object Obj)
    {
        //TurnCount
        string szTurn = (string)Obj;
		int curTurn = int.Parse(szTurn);
		if(curTurn >= 1 )
		{
			f_GetObject("BattleFinishBtn").SetActive(true);
		}
        //TsuCode
        // int turnCount = int.Parse(szTurn);
        // if (turnCount >= 3) flag = true;
        // MessageBox.ASSERT("BattleMain On_UpdateTurns :" + szTurn + "flag:"+flag);
        //
		if (StaticValue.m_CurBattleConfig.m_eBattleType == EM_Fight_Enum.eFight_CrusadeRebel)
		{
			if (VipLvCheck >= VipCheck.iParam2)
			{
				flag = true;
			}
			else
			{
				flag = false;
			}
		}
		
		if (StaticValue.m_CurBattleConfig.m_eBattleType == EM_Fight_Enum.eFight_DungeonMain ||
			StaticValue.m_CurBattleConfig.m_eBattleType == EM_Fight_Enum.eFight_DungeonElite ||
			StaticValue.m_CurBattleConfig.m_eBattleType == EM_Fight_Enum.eFight_Legend ||
			StaticValue.m_CurBattleConfig.m_eBattleType == EM_Fight_Enum.eFight_DailyPve)
		{
			if (VipLvCheck >= VipCheck.iParam1)
			{
				flag = true;
			}
			else
			{
				flag = false;
			}
		}
		else
		{
			flag = true;
		}

        int maxTurns = StaticValue.m_CurBattleConfig.m_eBattleType == EM_Fight_Enum.eFight_CrusadeRebel ? 5 : 15;
        UILabel LabelTurns = f_GetObject("LabelTurns").GetComponent<UILabel>();
        LabelTurns.text = string.Format("{0}/{1}",szTurn,maxTurns);

        string ppSQL = string.Format(CommonTools.f_GetTransLanguage(2130), ccMath.NumberToChinese(szTurn));
        if(_TextControl == null)
        {
            _TextControl = f_GetObject("CurTurns").GetComponent<TextControl>();
            _TextControl.IsRole = false;
        }
        _TextControl.f_Show(ppSQL);
    }



    private void f_BattleStart(object result)
    {
        MessageBox.ASSERT("BattleMain f_BattleStart");
        if(mTopMask.activeSelf)
            mTopMask.SetActive(false);
        if(Data_Pool.m_DialogPool.f_CheckCurNeedDialog(EM_DialogcCondition.BattleStart))
        {
            //弹出剧情对话前先重置回合数
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_BATTLE_TURNINFOR, "1");
            DialogPageParams tParam = new DialogPageParams(Data_Pool.m_DialogPool.f_GetCurNeedDialog(EM_DialogcCondition.BattleStart), EM_DialogcCondition.BattleStart, f_DialogFinish);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.DialogPage, UIMessageDef.UI_OPEN, tParam);
        }
        else
        {
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.DIALOGSTARBATTLE);
        }
    }
    /// <summary>
    /// 战斗结束结算和画面跳转处理
    /// </summary>
    /// <param name="result"></param>
    private void f_BattleFinish(object result)
    {
        CloseBattleUI();
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.FIGHTPAUSE);
        if (StaticValue.m_CurBattleConfig.m_iTollgateId == GameParamConst.PLOT_TOLLGATEID)
        {
            //剧情战斗完不结算，直接回到主界面
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
        }
        else if (StaticValue.m_CurBattleConfig.m_eBattleType == EM_Fight_Enum.eFight_Arena)
        {
            if (!Data_Pool.m_ArenaPool.mIsFinish)
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(2131));
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
            }
            else
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.ArenaFinishPage, UIMessageDef.UI_OPEN);
            }
        }
        else if (StaticValue.m_CurBattleConfig.m_eBattleType == EM_Fight_Enum.eFight_DailyPve)
        {
            if (!Data_Pool.m_DailyPveInfoPool.isFinish)
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(2132));
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
            }
            else
            {
                BattleFinishParam param = new BattleFinishParam();
                param.StarNum = Data_Pool.m_DailyPveInfoPool.Star;
                param.listAward = Data_Pool.m_DailyPveInfoPool.listAwardData;
                param.NeedShowFirstWin = false;
                ccUIManage.GetInstance().f_SendMsg(UINameConst.BattleFinishPage, UIMessageDef.UI_OPEN, param);
            }
        }
        else if (StaticValue.m_CurBattleConfig.m_eBattleType == EM_Fight_Enum.eFight_CrusadeRebel)  //叛军战斗结束
        {
            if (!Data_Pool.m_RebelArmyPool._bDungeonFinish)
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(2133));
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
            }
            else
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.RebelArmyFinish, UIMessageDef.UI_OPEN);
            }
        }
        else if (StaticValue.m_CurBattleConfig.m_eBattleType == EM_Fight_Enum.eFight_LegionDungeon)
        {
            if (!LegionMain.GetInstance().m_LegionDungeonPool.m_bChallengeFinish)
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(2134));
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
            }
            else
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionDungeonFinishPage, UIMessageDef.UI_OPEN);
            }
        }
        else if (StaticValue.m_CurBattleConfig.m_eBattleType == EM_Fight_Enum.eFight_GrabTreasure)//夺宝扫荡不需要进入战斗，只需处理扫荡战斗
        {
            if (!Data_Pool.m_GrabTreasurePool.m_GrabIsFinish)
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(2135));
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
            }
            else
            {
                GrabTreasurePoolDT poolDT = Data_Pool.m_GrabTreasurePool.m_GrabTreasurePoolDT;
                GrabTreasureFinishParam param = new GrabTreasureFinishParam();
                param.StarNum = poolDT.star;
                param.TreaFragId = poolDT.treaFragID;
                param.VigorCost = 2;
                ccUIManage.GetInstance().f_SendMsg(UINameConst.GrabTreasureFinishPage, UIMessageDef.UI_OPEN, param);
            }
        }
        else if (StaticValue.m_CurBattleConfig.m_eBattleType == EM_Fight_Enum.eFight_RunningMan)
        {
            if (!Data_Pool.m_RunningManPool.m_bChallengeFinish)
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(2136));
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
            }
            else
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManBattleFinishPage, UIMessageDef.UI_OPEN);
            }
        }
        else if (StaticValue.m_CurBattleConfig.m_eBattleType == EM_Fight_Enum.eFight_RunningManElite)
        {
            if (!Data_Pool.m_RunningManPool.m_bEliteChallengeFinish)
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(2137));
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
            }
            else
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManEliteFinishPage, UIMessageDef.UI_OPEN);
            }
        }
        else if (StaticValue.m_CurBattleConfig.m_eBattleType == EM_Fight_Enum.eFight_Patrol)
        {
            if (!Data_Pool.m_PatrolPool.m_bIsFinish)
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(2138));
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
            }
            else
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolBattleFinishPage, UIMessageDef.UI_OPEN);
            }
        }
        else if (StaticValue.m_CurBattleConfig.m_eBattleType == EM_Fight_Enum.eFight_LegionBattle)
        {
            if (!LegionMain.GetInstance().m_LegionBattlePool.m_bIsFinished)
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(2139));
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
            }
            else
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionBattleFinishPage, UIMessageDef.UI_OPEN);
            }
        }
        else if (StaticValue.m_CurBattleConfig.m_eBattleType == EM_Fight_Enum.eFight_CrossServerBattle)
        {
            if (!Data_Pool.m_CrossServerBattlePool.IsFinish)
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(2140));
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
            }
            else
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.CrossServerBattleFinishPage, UIMessageDef.UI_OPEN);
            }
        }
        //TsuCode - ChaosBattle
        else if (StaticValue.m_CurBattleConfig.m_eBattleType == EM_Fight_Enum.eFight_ChaosBattle)
        {
            if (!Data_Pool.m_ChaosBattlePool.IsFinish)
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(2140));
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
            }
            else
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.ChaosBattleFinishPage, UIMessageDef.UI_OPEN);
            }
        }
        //
        else if (StaticValue.m_CurBattleConfig.m_eBattleType == EM_Fight_Enum.eFight_CardBattle)
        {
            if (!Data_Pool.m_CardBattlePool.IsFinish)
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(2141));
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
            }
            else
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.CardBattleFinishPage, UIMessageDef.UI_OPEN);
            }
        }
        else if (StaticValue.m_CurBattleConfig.m_eBattleType == EM_Fight_Enum.eFight_TrialTower) {
            if (!Data_Pool.m_TrialTowerPool.isFinsh)
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(2141));
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
            }
            else
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.TrialTowerFinshPage, UIMessageDef.UI_OPEN);
            }
        }
        else if (StaticValue.m_CurBattleConfig.m_eBattleType == EM_Fight_Enum.eFight_ArenaCross)
        {
            if (!Data_Pool.m_CrossArenaPool.IsFinish)
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(2140));
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
            }
            else
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.ArenaCrossBattleFinishPage, UIMessageDef.UI_OPEN);
            }
        }
        else
        {
            if (!Data_Pool.m_DungeonPool.f_DungeonIsFinish())
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(2142));
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
            }
            else
            {
                DungeonFinishInfo info = Data_Pool.m_DungeonPool.mDungeonFinishInfo;
                //有对话数据 而且 战斗胜利 就播放对话
                if (Data_Pool.m_DialogPool.f_CheckCurNeedDialog(EM_DialogcCondition.BattleFinish) && info.StarNum > 0)
                {
                    DialogPageParams tParam = new DialogPageParams(Data_Pool.m_DialogPool.f_GetCurNeedDialog(EM_DialogcCondition.BattleFinish), EM_DialogcCondition.BattleFinish, f_DialogFinish);
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.DialogPage, UIMessageDef.UI_OPEN, tParam);
                }
                else
                {
                    BattleFinishParam param = new BattleFinishParam();
                    param.StarNum = info.StarNum;
                    param.listAward = Data_Pool.m_AwardPool.f_GetAwardPoolDTByType(info.StarNum, info.mAwardSource);
                    param.NeedShowFirstWin = info.NeedShowFirstWin;
                    param.EnergyCost = info.EnergyCost;

                    if (info.IsFirstWin && CheckIsNeedShowGetBattleformFrag())//显示获得阵图碎片UI
                    {
                        BattleFormFragmentParam param2 = new BattleFormFragmentParam();
                        param2.mCallback = f_ShowGetBattleformFragCallback;
                        param2.data = param;
                        ccUIManage.GetInstance().f_SendMsg(UINameConst.BattleFormFragmentPage, UIMessageDef.UI_OPEN, param2);
                    }
                    else
                    {
                        ccUIManage.GetInstance().f_SendMsg(UINameConst.BattleFinishPage, UIMessageDef.UI_OPEN, param);
                    }
                }
            }
        }

    }
    /// <summary>
    /// 显示完阵图碎片
    /// </summary>
    /// <param name="data">BattleFinishParam</param>
    private void f_ShowGetBattleformFragCallback(object data)
    {
        BattleFinishParam param = (BattleFinishParam)data;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.BattleFinishPage, UIMessageDef.UI_OPEN, param);
    }
    /// <summary>
    /// 检测是否要显示获得阵图碎片UI
    /// </summary>
    /// <returns></returns>
    private bool CheckIsNeedShowGetBattleformFrag()
    {
        //1.战斗是主线副本 2.第一次打该关卡 3.该章节处于该章最后一个关卡
        if(StaticValue.m_CurBattleConfig.m_eBattleType == EM_Fight_Enum.eFight_DungeonMain)//1.战斗是主线副本
        {
            DungeonPoolDT poolDT = Data_Pool.m_DungeonPool.f_GetForId(StaticValue.m_CurBattleConfig.m_iChapterId) as DungeonPoolDT;
            List<DungeonTollgatePoolDT> listTollPoolDT = poolDT.mTollgateList;
            if(listTollPoolDT[listTollPoolDT.Count - 1].mTollgateId == StaticValue.m_CurBattleConfig.m_iTollgateId)//3.该章节处于该章最后一个关卡
            {
                return true;
            }
        }
        return false;
    }
    private void f_RoleBeforeDeath(object result)
    {
        if(result == null || !(result is int))
        {
            //站位
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(2143));
            return;
        }
        int tValue = (int)result;
        if(Data_Pool.m_DialogPool.f_CheckCurNeedDialog(EM_DialogcCondition.RoleBeforDeath, tValue))
        {
            DialogPageParams tParam = new DialogPageParams(Data_Pool.m_DialogPool.f_GetCurNeedDialog(EM_DialogcCondition.RoleBeforDeath, tValue), EM_DialogcCondition.RoleBeforDeath, f_DialogFinish);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.DialogPage, UIMessageDef.UI_OPEN, tParam);
        }
    }

    private void f_DialogFinish(object result)
    {
        MessageBox.ASSERT("BattleMain DialogFinish");
        EM_DialogcCondition tCondition = (EM_DialogcCondition)result;
        if(tCondition == EM_DialogcCondition.BattleFinish)
        {
            if(StaticValue.m_CurBattleConfig.m_eBattleType == EM_Fight_Enum.eFight_Arena)
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.ArenaFinishPage, UIMessageDef.UI_OPEN);
            }
            else
            {
                DungeonFinishInfo info = Data_Pool.m_DungeonPool.mDungeonFinishInfo;
                BattleFinishParam param = new BattleFinishParam();
                param.StarNum = info.StarNum;
                param.listAward = Data_Pool.m_AwardPool.f_GetAwardPoolDTByType(info.StarNum, info.mAwardSource);
                param.NeedShowFirstWin = info.NeedShowFirstWin;
                param.EnergyCost = info.EnergyCost;

                if(info.IsFirstWin && CheckIsNeedShowGetBattleformFrag())//显示获得阵图碎片UI
                {
                    BattleFormFragmentParam param2 = new BattleFormFragmentParam();
                    param2.mCallback = f_ShowGetBattleformFragCallback;
                    param2.data = param;
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.BattleFormFragmentPage, UIMessageDef.UI_OPEN, param2);
                }
                else
                {
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.BattleFinishPage, UIMessageDef.UI_OPEN, param);
                }
            }

        }
    }

    private void CallBack_SetBattleDepth(object Obj)
    {
        if((bool)Obj == true)
        {
            _MainUIPanel.sortingOrder = (int)EM_BattleDepth.GameBg;
        }
        else
        {
            _MainUIPanel.sortingOrder = (int)EM_BattleDepth.UI;
        }
    }

    private void CloseBattleUI(bool bCloseHpPanel = true)
    {
        if(bCloseHpPanel)
        {
            f_GetObject("HPPanel").SetActive(false);
        }
        f_GetObject("BattleSpeedBtn").SetActive(false);
        f_GetObject("LabelTurns").SetActive(false);
        f_GetObject("BattleFinishBtn").SetActive(false);
        f_GetObject("LabelElement").SetActive(false);
        mStatusView.SetActive(false);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.AuraMainPage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.BuffDetailPage, UIMessageDef.UI_CLOSE);
    }


    #region 测试模拟处理函数

    private void f_BattleStartHandle(GameObject go, object value1, object value2)
    {
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_DIALOG_BATTLESTART);
    }

    private void f_RoleBeforeDeathHandle(GameObject go, object value1, object value2)
    {
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_DIALOG_ROLEBEFOREDEATH, 5);
    }

    private void f_BattleFinishHandle(GameObject go, object value1, object value2)
    {
        // MessageBox.ASSERT("BattleMain f_BattleFinishHandle");
        // MessageBox.ASSERT("BattleMain f_BattleFinishHandle Name"+flag);
        if (flag == true)
        {
            if (GloData.glo_StarGuidance)
            {

                if (StaticValue.m_CurBattleConfig.m_bIsHandle)
                {

                }
                //副本类型跳过 加等级限制
                else if (StaticValue.m_CurBattleConfig.m_eBattleType == EM_Fight_Enum.eFight_DungeonMain
                  || StaticValue.m_CurBattleConfig.m_eBattleType == EM_Fight_Enum.eFight_DungeonElite
                  || StaticValue.m_CurBattleConfig.m_eBattleType == EM_Fight_Enum.eFight_Legend
                  || StaticValue.m_CurBattleConfig.m_eBattleType == EM_Fight_Enum.eFight_DailyPve
                  || StaticValue.m_CurBattleConfig.m_eBattleType == EM_Fight_Enum.eFight_LegionDungeon
                  )
                {
                    //Check level character and levelVip

                    if (!UITool.f_GetIsOpensystem(EM_NeedLevel.DungeonJumpLevel)
                        && Data_Pool.m_RechargePool.f_GetCurLvVipPriValue(EM_VipPrivilege.eVip_DungeonSkip) == 0
                        )
                    {
                        MessageBox.ASSERT("BattleMain f_BattleFinishHandle :flag:" + flag);
                        int tNeedLv = UITool.f_GetSysOpenLevel(EM_NeedLevel.DungeonJumpLevel);
                        int tNeedVip = Data_Pool.m_RechargePool.f_GetVipLvLimit(EM_VipPrivilege.eVip_DungeonSkip);
						MessageBox.ASSERT("Type: " + StaticValue.m_CurBattleConfig.m_eBattleType);
						MessageBox.ASSERT("NeedType: " + EM_Fight_Enum.eFight_CrusadeRebel);
                        if (tNeedLv == 0)
                        {
                            if (StaticValue.m_CurBattleConfig.m_eBattleType == EM_Fight_Enum.eFight_CrusadeRebel)
                                UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(2144), VipCheck.iParam2));
                            else
                                UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(2145), VipCheck.iParam1));
                        }
                        else if (tNeedVip == 0)
                        {
                            if (StaticValue.m_CurBattleConfig.m_eBattleType == EM_Fight_Enum.eFight_CrusadeRebel)
                                UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(2146), tNeedLv));
                            else
                                UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(2147), tNeedLv));
                        }
                        else
                        {
                            if (StaticValue.m_CurBattleConfig.m_eBattleType == EM_Fight_Enum.eFight_CrusadeRebel)
                                UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(2148), tNeedLv, VipCheck.iParam2));
                            else
                                UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(2149), tNeedLv, VipCheck.iParam1));
                        }
                        return;
                    }

                }
                else if (StaticValue.m_CurBattleConfig.m_eBattleType == EM_Fight_Enum.eFight_CrusadeRebel)
                {
                    if (Data_Pool.m_RechargePool.f_GetCurLvVipPriValue(EM_VipPrivilege.eVip_CrusadeRebel) == 0)
                    {
                        int tNeedVip = Data_Pool.m_RechargePool.f_GetVipLvLimit(EM_VipPrivilege.eVip_CrusadeRebel);
                        UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(2218), VipCheck.iParam2));
                        return;
                    }
                }

            }
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_SHOWLASTTURN);
            Time.timeScale = 1;
            CloseBattleUI(false);
            //Tsu
        }
        else
        {
            int tNeedLv = UITool.f_GetSysOpenLevel(EM_NeedLevel.DungeonJumpLevel);
            int tNeedVip = Data_Pool.m_RechargePool.f_GetVipLvLimit(EM_VipPrivilege.eVip_DungeonSkip);
			if(StaticValue.m_CurBattleConfig.m_eBattleType == EM_Fight_Enum.eFight_CrusadeRebel)
			{
				UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(2146), VipCheck.iParam2));
			}
			else
			{
				UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(2147), VipCheck.iParam1));
			}
        }
        //
    }

    #endregion

}
