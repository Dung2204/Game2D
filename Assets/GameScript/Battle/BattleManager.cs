using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
using DG.Tweening;
using System;
using System.Linq;

public class BattleManager : MonoBehaviour
{
    public TweenPosition m_BattlePopW;
    public TweenPosition m_BattlePopH;
    public const float speedRate = 1f; //再原有战斗速度的基础上的倍数
	public float delayTimeEachTurn = 0.01f;//0.025f;
	stRoleInfor[] aRoleList;
    float maxBackward =0.5f;
	float battleOffset =2.5f;

    private BattlePop _BattlePop;
    private bool _bShowLastTurn = false;
    private UILabel _CurTurns;
    public BattleMaskBG m_BattleMaskBG;
    public SWP_TimeGroupController m_SWP_TimeGroupController;

    private int _iTurns;
    private BattleTurn _CurBattleTurn;

    private bool _bIsHavePlot = false;
	public bool isBattleEnd = false;

    public GameObject[] m_aPos;
    //public GameObject[] m_aAttackPosA;
    //public GameObject[] m_aAttackPosB;
    public GameObject[] m_aBattleIndex;
    private RolePool _RolePool = new RolePool();
    private const int FightTimeOut = 5;  //服务器下发战斗数据超时时间
    private System.DateTime startFightTime;

    stFightElementInfor[] aElementList; // dánh sách nguyên tố trùng kích
    private FightElementPool _FightElementPool = new FightElementPool();

    private BattleAuraData m_BattleAuraDataA = new BattleAuraData();
    private BattleAuraData m_BattleAuraDataB = new BattleAuraData();

    private static BattleManager _Instance = null;
    public static BattleManager GetInstance()
    {
        if (!_Instance)
        {
            _Instance = (BattleManager)FindObjectOfType(typeof(BattleManager));
            if (!_Instance)
            {
                Debug.LogError("init BattleMain Fail");
            }
        }


        return _Instance;
    }

    void Start()
    {
        _BattlePop = new BattlePop(m_BattlePopW, m_BattlePopH);
        startFightTime = System.DateTime.Now;
        StaticValue.m_LastFightIsTimeOut = false;
        CreateGame(null);
		if (StaticValue.m_preScene == EM_Scene.Login && StaticValue.m_curScene == EM_Scene.BattleMain)
		{
            //My Code
			// GameObject camera = GameObject.Find("Camera");
			// var videoPlayer = camera.GetComponent<UnityEngine.Video.VideoPlayer>();
			// videoPlayer.Play();
			// videoPlayer.loopPointReached += EndReached;
			//
		}
    }
	
	//My Code
	// void EndReached(UnityEngine.Video.VideoPlayer vp)
    // {
        // vp.renderMode = UnityEngine.Video.VideoRenderMode.CameraFarPlane;
    // }
	//

    private void CreateGame(object Obj)
    {
        if (Data_Pool.m_BattleDataPool.f_CheckIsLoadSuc())
        {
            InitRole();
            InitMessage();
            InitSpeed();
        }
        else
        {
            if ((System.DateTime.Now - startFightTime).TotalSeconds > FightTimeOut)
            {
                //超时结束战斗
                StaticValue.m_LastFightIsTimeOut = true;
                f_BattleEnd();
                return;
            }
            ccTimeEvent.GetInstance().f_RegEvent(1f, false, null, CreateGame);
        }
    }



    private void InitMessage()
    {
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_BATTLE_SPEED, On_BattleSpeed);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_SHOWLASTTURN, On_ShowLastTurn, this);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.DIALOGSTARBATTLE, _StareNewTurn);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.BATTLE_SHOW_AURA_DETAIL, _ShowAuraDetail);
    }
    private void _ShowAuraDetail(object obj)
    {
        int team = (int)obj;
        if (team == 1)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.AuraMainPage, UIMessageDef.UI_OPEN,m_BattleAuraDataA);
        }
        else
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.AuraMainPage, UIMessageDef.UI_OPEN, m_BattleAuraDataB);
        }
    }

    public void f_Open()
    {
        gameObject.SetActive(true);
    }

    public void f_Close()
    {
        _BattlePop.f_Destory();
        On_BattleSpeed(1);
        gameObject.SetActive(true);
        _RolePool.f_Destory();
        _FightElementPool.f_Destory();
        System.GC.Collect();
    }

    public BattleTurn f_GetCurBattleTurn()
    {
        return _CurBattleTurn;
    }

    #region 播放速度控制

    private void InitSpeed()
    {
        int iSpeed = LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_BattleSpeed, 1);
        DoSpeed(iSpeed, true);
    }

    private void On_BattleSpeed(object Obj)
    {
        int iSpeed = (int)Obj;
        DoSpeed(iSpeed, false);
    }

    private void DoSpeed(float iSpeed, bool IsInit)
    {
		int rateS = 30;
        //iSpeed *= speedRate;
        //if (iSpeed == 99)
        //{
        //    m_SWP_TimeGroupController.SpeedUpGroupTime(30, IsInit);
        //}
        //else
        //{
        //    //m_SWP_TimeGroupController.SpeedUpGroupTime(iSpeed * 100, IsInit);
        //    m_SWP_TimeGroupController.SpeedUpGroupTime(iSpeed * 80, IsInit);
            
        //}
		if(LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_BattleSpeed, 1) == 1)
			rateS = 80;
		if(LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_BattleSpeed, 1) == 2)
			rateS = 50;
		if(LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_BattleSpeed, 1) == 3)
			rateS = 50;
        iSpeed *= speedRate;
        if (iSpeed == 99)
        {
            m_SWP_TimeGroupController.SpeedUpGroupTime(15, IsInit);//30 
        }
        else
        {
            if (iSpeed == 4)
            {
                m_SWP_TimeGroupController.SpeedUpGroupTime(330, IsInit);//330
            }
            else
            {
                m_SWP_TimeGroupController.SpeedUpGroupTime(iSpeed * rateS, IsInit);//80
            }

        }
    }

    #endregion

    private void InitRole()
    {
MessageBox.DEBUG("Start");
        _RolePool.Init();
        _FightElementPool.Init();
        List<RoleControl> m_aTeamA = new List<RoleControl>();
        List<RoleControl> m_aTeamB = new List<RoleControl>();
    //先判断要不要进行剧情
    _bIsHavePlot = Data_Pool.m_DungeonPool.f_JudgeIsHavePlot();
        if (_bIsHavePlot)
        {
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_INIT_CHECKPOINT_PLOT);
        }
        else
        {
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_DIALOG_BATTLESTART);
        }        

        //隐藏战斗角色数据(由剧情控制角色显示)，位处理，我方1-6，敌方7-12，，如果哪位要隐藏则该位置1
        int hideFightRoleData = Data_Pool.m_DungeonPool.f_GetHideFightRoleData();

        _iTurns = 0;
		aRoleList = Data_Pool.m_BattleDataPool.f_GetRoleList();
        for (int i = 0; i < aRoleList.Length; i++)
        {
            //EM_FormationPos tEM_FormationPos = (EM_FormationPos)aRoleList[i].m_iPos;
            EM_CloseArrayPos tEM_FormationPos = (EM_CloseArrayPos)aRoleList[i].m_iPos;
			EM_CloseArrayPos tEM_FormationPos2 = (EM_CloseArrayPos)aRoleList[i].m_iPos;
            EM_RoleType tEM_RoleType = (EM_RoleType)aRoleList[i].m_iRoleType;
            bool isNeedHide = BitTool.BitTest(hideFightRoleData, (ushort)(i + 1));
            // if (i < 6)
            // {
                // if (aRoleList[i].m_iId > 0)
                // {
                    // if (tEM_RoleType == EM_RoleType.ePlayerRole)
                        // _RolePool.f_CreatePlayerRole(aRoleList[i].m_iId, tEM_FormationPos, aRoleList[i].m_iTempId, aRoleList[i].m_iMaxHp, 
                            // GetFormationPos(tEM_FormationPos, EM_Factions.ePlayer_A), EM_Factions.ePlayer_A, aRoleList[i].m_iFanshionDressId, isNeedHide, false);
                    // else
                    // {
                        // _RolePool.f_CreateEnemyRole(aRoleList[i].m_iId, tEM_FormationPos, aRoleList[i].m_iTempId, aRoleList[i].m_iMaxHp, 
                            // GetFormationPos(tEM_FormationPos, EM_Factions.ePlayer_A), EM_Factions.ePlayer_A, aRoleList[i].m_iFanshionDressId, isNeedHide, false);
                    // }
                // }
            // }
            // else
            // {
                // if (aRoleList[i].m_iId > 0)
                // {
                    // if (tEM_RoleType == EM_RoleType.ePlayerRole)
                    // { _RolePool.f_CreatePlayerRole(aRoleList[i].m_iId, tEM_FormationPos, aRoleList[i].m_iTempId, aRoleList[i].m_iMaxHp,
                        // GetFormationPos(tEM_FormationPos, EM_Factions.eEnemy_B), EM_Factions.eEnemy_B, aRoleList[i].m_iFanshionDressId, isNeedHide, false); }
                    // else
                    // {
                        // _RolePool.f_CreateEnemyRole(aRoleList[i].m_iId, tEM_FormationPos, aRoleList[i].m_iTempId, aRoleList[i].m_iMaxHp,
                            // GetFormationPos(tEM_FormationPos, EM_Factions.eEnemy_B), EM_Factions.eEnemy_B, aRoleList[i].m_iFanshionDressId, isNeedHide, false);
                    // }
                // }
            // }
			if (i < 20)
            {
                if (aRoleList[i].m_iId > 0)
                {
                    RoleControl rs = null;
                    Vector3 formationPos = GetFormationPos(tEM_FormationPos, EM_Factions.ePlayer_A);
					Vector3 formationPos2 = GetFormationPos(tEM_FormationPos2, EM_Factions.ePlayer_A);
					MessageBox.ASSERT("Temp ID: " + aRoleList[i].m_iId + " " + aRoleList[i].m_iTempId);
                    if (tEM_RoleType == EM_RoleType.ePlayerRole)
                        rs = _RolePool.f_CreatePlayerRole(aRoleList[i].m_iId, tEM_FormationPos, aRoleList[i].m_iTempId, aRoleList[i].m_iMaxHp, formationPos, EM_Factions.ePlayer_A, aRoleList[i].m_iFanshionDressId, isNeedHide, false, aRoleList[i].m_iAnger, aRoleList[i].m_iGodEquipSkillId);
                    else if(aRoleList[i].m_iTempId == 400006)
                    {
						tEM_FormationPos2 = EM_CloseArrayPos.eCloseArray_Pos13;
						formationPos2 = GetFormationPos(tEM_FormationPos2, EM_Factions.ePlayer_A);
                        rs = _RolePool.f_CreateEnemyRole(aRoleList[i].m_iId, tEM_FormationPos2, aRoleList[i].m_iTempId, aRoleList[i].m_iMaxHp, formationPos2, EM_Factions.ePlayer_A, aRoleList[i].m_iFanshionDressId, isNeedHide, false, aRoleList[i].m_iAnger, aRoleList[i].m_iGodEquipSkillId);
					}
					else
                    {
                        rs = _RolePool.f_CreateEnemyRole(aRoleList[i].m_iId, tEM_FormationPos, aRoleList[i].m_iTempId, aRoleList[i].m_iMaxHp, formationPos, EM_Factions.ePlayer_A, aRoleList[i].m_iFanshionDressId, isNeedHide, false, aRoleList[i].m_iAnger, aRoleList[i].m_iGodEquipSkillId);
                    }
					
					if(aRoleList[i].m_iTempId == 400006)
					{
						formationPos2.x -= battleOffset;
						if(rs) rs.transform.position = formationPos2;
						m_aTeamA.Add(rs);
					}
					else
					{
						formationPos.x -= battleOffset;
						if(rs) rs.transform.position = formationPos;
						m_aTeamA.Add(rs);
					}
                }
            }
            else
            {
                if (aRoleList[i].m_iId > 0)
                {
                    RoleControl rs = null;
                    Vector3 formationPos = GetFormationPos(tEM_FormationPos, EM_Factions.eEnemy_B);
                    if (tEM_RoleType == EM_RoleType.ePlayerRole)
                    {
                        rs = _RolePool.f_CreatePlayerRole(aRoleList[i].m_iId, tEM_FormationPos, aRoleList[i].m_iTempId, aRoleList[i].m_iMaxHp, formationPos, EM_Factions.eEnemy_B, aRoleList[i].m_iFanshionDressId, isNeedHide, false, aRoleList[i].m_iAnger, aRoleList[i].m_iGodEquipSkillId);
                    }
                    else
                    {
                        rs = _RolePool.f_CreateEnemyRole(aRoleList[i].m_iId, tEM_FormationPos, aRoleList[i].m_iTempId, aRoleList[i].m_iMaxHp, formationPos, EM_Factions.eEnemy_B, aRoleList[i].m_iFanshionDressId, isNeedHide, false, aRoleList[i].m_iAnger, aRoleList[i].m_iGodEquipSkillId);
                    }
                    formationPos.x += battleOffset;
                    if(rs) rs.transform.position = formationPos;
                    m_aTeamB.Add(rs);
                }
            }
        }
        RolePropertyTools.GetDictAuraCamp(m_aTeamA, ref m_BattleAuraDataA.dict_AuraCamp);
        RolePropertyTools.GetDictAuraType(m_aTeamA, ref m_BattleAuraDataA.dict_AuraType);
        RolePropertyTools.GetDictAuraFiveEle(m_aTeamA,ref m_BattleAuraDataA.dict_AuraEle);
        RolePropertyTools.GetDictAuraCamp(m_aTeamB, ref m_BattleAuraDataB.dict_AuraCamp);
        RolePropertyTools.GetDictAuraType(m_aTeamB, ref m_BattleAuraDataB.dict_AuraType);
        RolePropertyTools.GetDictAuraFiveEle(m_aTeamB, ref m_BattleAuraDataB.dict_AuraEle);
        // nguyên tố trùng kích
        aElementList = Data_Pool.m_BattleDataPool.f_GetFightElementList();
        for (int i = 0; i < aElementList.Length; i++)
        {
            if (aElementList[i].m_iId > 0)
            {
                int iPos = aElementList[i].m_iPos;// vị trí
                EM_Factions iSide = (EM_Factions)aElementList[i].m_iSide;// team A or team b
                FightElementItem fe = _FightElementPool.f_CreateFightElement(aElementList[i]);
            }
        }
            MessageBox.DEBUG("Combat");
        _RolePool.f_SetDepth2Other();
        _bShowLastTurn = false;
    }
    
    private void _StareNewTurn(object obj)
    {
        MessageBox.DEBUG("_StareNewTurn");
        Sequence moveInSequence = DOTween.Sequence();
        moveInSequence.Pause();
        ccTimeEvent.GetInstance().f_RegEvent(1.75f,false,null,(o)=>{
            StartNewTurn();
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_BATTLE_START);
        });
		if(_iTurns < 1)
		{
			foreach (RoleControl roleControl in _RolePool.GetAllRolesControl())
			{
				roleControl.m_CharActionController.f_PlayWalk(2f);
				roleControl.transform.DOMoveX(roleControl._v3StayPos.x,1.25f).OnComplete(()=> { roleControl.m_CharActionController.f_PlayStand(); roleControl.UpdateDirection(); });
			}
		}
        moveInSequence.Play();
    }

    public Vector3 GetFormationPos(EM_CloseArrayPos tEM_FormationPos, EM_Factions tEM_Factions)
    {
        int iPos = (int)tEM_FormationPos;
        if (tEM_Factions == EM_Factions.ePlayer_A)
        {
            return m_aPos[iPos].transform.position;
        }
        return m_aPos[20 + iPos].transform.position;
    }

    //void Update()
    //{
    //    if (!_bInitOK)
    //    {
    //        return;
    //    }

    //}

    void RoleAttack(object obj = null)
    {
MessageBox.DEBUG("Ready to attack");
        if (_bShowLastTurn)
        {
            return;
        }
        if (null == _CurBattleTurn)
        {
MessageBox.ASSERT(string.Format("Data set null：{0},highest set：{1}", _iTurns, Data_Pool.m_BattleDataPool.f_GetMaxTurn()));
            StartNewTurn();
            return;
        }
        RoleAttack tRoleAttack = _CurBattleTurn.f_GetCurRoleAttack();
        if (tRoleAttack == null)
        {
            ccTimeEvent.GetInstance().f_RegEvent(_iTurns <= 1? 0.25f : 1f, false,null,(o)=> { StartNewTurn(); });
        }
        else
        {
            RoleControl tRoleControl = _RolePool.f_GetRoleControl(tRoleAttack.m_iId);
            if (_bIsHavePlot)
            {
                //通知剧情系统指定回合某个阵营某个站位武将行动前                
                PlotCheckParam plotCheckParam = new PlotCheckParam();
                plotCheckParam.triggerType = EM_PlotTriggerType.FightRoleAction;
                plotCheckParam.triggerParams = new int[4];
                plotCheckParam.triggerParams[0] = _iTurns;
                plotCheckParam.triggerParams[1] = (int)tRoleControl.f_Get_Factions();
                plotCheckParam.triggerParams[2] = (int)tRoleControl.f_Get_FormationPos();
                plotCheckParam.triggerParams[3] = (int)EM_FightRoleActionType.BeforAction;
                plotCheckParam.callback = f_RoleStartAttack;
                plotCheckParam.callbackData = tRoleAttack;
                glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_PLOT_CHECK, plotCheckParam);
            }
            else
            {
                f_RoleStartAttack(tRoleAttack);
            }
        }
    }

    /// <summary>
    /// 角色开始攻击
    /// </summary>
    /// <param name="tRoleAttack"></param>
    private void f_RoleStartAttack(object _tRoleAttack)
    {
        if (_bShowLastTurn)
        {
            return;
        }
        RoleAttack tRoleAttack = _tRoleAttack as RoleAttack;
        if (null == tRoleAttack)
        {
            RoleAttack();
MessageBox.ASSERT("f_RoleStartAttack，Character attack data null！！！");
            return;
        }

		//My Code
        RoleControl tRoleControl = _RolePool.f_GetRoleControl(tRoleAttack.m_iId);
		// if(_iTurns >= Data_Pool.m_BattleDataPool.f_GetMaxTurn())
		// {
			// tRoleControl.NeedMove = 0;
		// }
		// else
		// {
			// tRoleControl.NeedMove = moveStep;
		// }
        tRoleControl.NeedMove = (_iTurns >= Data_Pool.m_BattleDataPool.f_GetMaxTurn()) ? tRoleControl.NeedMove : moveStep;
		// tRoleControl.NeedMove = moveStep;
        if (!tRoleControl.gameObject.activeInHierarchy)
        {
            //如果武将在剧情系统中隐藏起来了，则跳过他的行动
            RoleAttack();
            return;
        }
        //tRoleAttack.m_v3AttackPos = GetAttackPos(tRoleControl, tRoleAttack);
        tRoleAttack.m_v3AttackPos = GetAttackPos(tRoleControl, tRoleAttack);
        if(tRoleAttack.m_iIsActiveFightElementSkill != 0)
        {
            tRoleControl.f_FightElementAttacking(tRoleAttack, CallBack_RoleAttckComplete);
        }
        else
        {
            tRoleControl.f_Attacking(tRoleAttack, CallBack_RoleAttckComplete);
        }
        //tRoleControl.f_Attacking(tRoleAttack, CallBack_RoleAttckComplete);
  //      float delayTimeEachRoles = 0.5f;
  //      tRoleControl.f_Attacking(tRoleAttack, CallBack_RoleAttckComplete);
  //      delayTimeEachRoles = tRoleControl.m_CharActionController.GetAnimationTimeByMagicIndex(tRoleControl.GetMagicIndex());
  //if(tRoleControl.GetMagicIndex() == 3)
  //{
  //	if(LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_BattleSpeed, 1) == 1)
  //		delayTimeEachRoles = (30f + (tRoleControl.m_CharActionController.GetAnimationTimeByMagicIndex(tRoleControl.GetMagicIndex())))/3;
  //	if(LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_BattleSpeed, 1) == 2)
  //		delayTimeEachRoles = (19f + (tRoleControl.m_CharActionController.GetAnimationTimeByMagicIndex(tRoleControl.GetMagicIndex())))/2;
  //	if(LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_BattleSpeed, 1) == 3)
  //		delayTimeEachRoles = (12f + (tRoleControl.m_CharActionController.GetAnimationTimeByMagicIndex(tRoleControl.GetMagicIndex())));
  //}
  //else if(tRoleControl.GetMagicIndex() == 2)
  //{
  //	if(LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_BattleSpeed, 1) == 1)
  //		delayTimeEachRoles = (8f + (tRoleControl.m_CharActionController.GetAnimationTimeByMagicIndex(tRoleControl.GetMagicIndex())))/3;
  //	if(LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_BattleSpeed, 1) == 2)
  //		delayTimeEachRoles = (3f + (tRoleControl.m_CharActionController.GetAnimationTimeByMagicIndex(tRoleControl.GetMagicIndex())))/2;
  //	if(LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_BattleSpeed, 1) == 3)
  //		delayTimeEachRoles = (2f + (tRoleControl.m_CharActionController.GetAnimationTimeByMagicIndex(tRoleControl.GetMagicIndex())));
  //}
  //      // else if(tRoleAttack.m_iMagicId == 100111)
  //      // {
  //      // if(LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_BattleSpeed, 1) == 1)
  //      // delayTimeEachRoles = (20f + (tRoleControl.m_CharActionController.GetAnimationTimeByMagicIndex(tRoleControl.GetMagicIndex())))/3;
  //      // if(LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_BattleSpeed, 1) == 2)
  //      // delayTimeEachRoles = (8f + (tRoleControl.m_CharActionController.GetAnimationTimeByMagicIndex(tRoleControl.GetMagicIndex())))/2;
  //      // if(LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_BattleSpeed, 1) == 3)
  //      // delayTimeEachRoles = (3f + (tRoleControl.m_CharActionController.GetAnimationTimeByMagicIndex(tRoleControl.GetMagicIndex())));
  //      // }
  //      // else if(tRoleAttack.m_iMagicId == 100112)
  //      // {
  //      // if(LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_BattleSpeed, 1) == 1)
  //      // delayTimeEachRoles = (36f + (tRoleControl.m_CharActionController.GetAnimationTimeByMagicIndex(tRoleControl.GetMagicIndex())))/3;
  //      // if(LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_BattleSpeed, 1) == 2)
  //      // delayTimeEachRoles = (14f + (tRoleControl.m_CharActionController.GetAnimationTimeByMagicIndex(tRoleControl.GetMagicIndex())))/2;
  //      // if(LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_BattleSpeed, 1) == 3)
  //      // delayTimeEachRoles = (8f + (tRoleControl.m_CharActionController.GetAnimationTimeByMagicIndex(tRoleControl.GetMagicIndex())));
  //      // }
        //else if (tRoleControl.GetMagicIndex() == 5)// nguyên tố trùng kích
        //{
        //    if (LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_BattleSpeed, 1) == 1)
        //        delayTimeEachRoles = (8f + (tRoleControl.m_CharActionController.GetAnimationTimeByMagicIndex(1))) / 3;
        //    if (LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_BattleSpeed, 1) == 2)
        //        delayTimeEachRoles = (3f + (tRoleControl.m_CharActionController.GetAnimationTimeByMagicIndex(1))) / 2;
        //    if (LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_BattleSpeed, 1) == 3)
        //        delayTimeEachRoles = (2f + (tRoleControl.m_CharActionController.GetAnimationTimeByMagicIndex(1)));
        //}
        //else
        //{
        //    if (LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_BattleSpeed, 1) == 1)
        //        delayTimeEachRoles = tRoleControl.m_CharActionController.GetAnimationTimeByMagicIndex(tRoleControl.GetMagicIndex());
        //    if (LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_BattleSpeed, 1) == 2)
        //        delayTimeEachRoles = (tRoleControl.m_CharActionController.GetAnimationTimeByMagicIndex(tRoleControl.GetMagicIndex()) / 1.5f);
        //    if (LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_BattleSpeed, 1) == 3)
        //        delayTimeEachRoles = (tRoleControl.m_CharActionController.GetAnimationTimeByMagicIndex(tRoleControl.GetMagicIndex()) / 2f);
        //}
        //// MessageBox.ASSERT("Speed: " + LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_BattleSpeed, 1));
        //      ccTimeEvent.GetInstance().f_RegEvent(delayTimeEachRoles/6, false, null, (a)=> { RoleAttack(); });
        ////
    }

    public void f_SetBattleDepth(RoleControl tRoleControl, RoleAttack tRoleAttack)
    {
        _RolePool.f_SetDepth2Other();

        tRoleControl.f_SetDepthForAttack();
        for (int i = 0; i < tRoleControl.f_GetRoleAttack().m_aData.Count; i++)
        {
            if (tRoleControl.f_GetRoleAttack().m_aData[i].m_iIsBaseAttack == 0)
            {
                RoleControl tBeAttackRoleControl = BattleManager.GetInstance().f_GetRoleControl(tRoleControl.f_GetRoleAttack().m_aData[i].m_iId);
				//My Code code nay chi dung cho char aokiji
				int aokiji = 0;
				// if(tRoleAttack.m_iMagicId == 110212)
				// {
					// aokiji = 1;
				// }
                if (!RoleTools.f_CheckIsDie(tBeAttackRoleControl))
                {
                    tBeAttackRoleControl.f_SetDepthForBeAttack(aokiji);
                }
				
            }
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.BattleMain, UIMessageDef.UI_SETBATTLEUIDEPTH, true);
    }

    public void f_ResetBattleDepth(RoleControl tRoleControl, RoleAttack tRoleAttack)
    {
        _RolePool.f_SetDepth2Other();
        ccUIManage.GetInstance().f_SendMsg(UINameConst.BattleMain, UIMessageDef.UI_SETBATTLEUIDEPTH, false);
    }

    void CallBack_RoleAttckComplete(object Obj)
    {
        MessageBox.DEBUG("CallBack_RoleAttckComplete");
        RoleControl tRoleControl = Obj as RoleControl;
        if (_bIsHavePlot && null != tRoleControl )
        {
            //通知剧情系统指定回合某个阵营某个站位武将行动后
            PlotCheckParam plotCheckParam = new PlotCheckParam();
            plotCheckParam.triggerType = EM_PlotTriggerType.FightRoleAction;
            plotCheckParam.triggerParams = new int[4];
            plotCheckParam.triggerParams[0] = _iTurns;
            plotCheckParam.triggerParams[1] = (int)tRoleControl.f_Get_Factions();
            plotCheckParam.triggerParams[2] = (int)tRoleControl.f_Get_FormationPos();
            plotCheckParam.triggerParams[3] = (int)EM_FightRoleActionType.AfterAction;
            plotCheckParam.callback = RoleAttack;
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_PLOT_CHECK, plotCheckParam);
        }
        else
        {
            RoleAttack();
        }        
    }

    void StartNewTurn()
    {
        if (_bShowLastTurn)
        {
            return;
        }
        if (_iTurns >= Data_Pool.m_BattleDataPool.f_GetMaxTurn())
        {
			foreach (RoleControl roleControl in _RolePool.GetAllRolesControl())
			{
				roleControl.m_CharActionController.f_PlayStand();
			}
            ccTimeEvent.GetInstance().f_RegEvent(2f, false, null, CallBack_BattleEnd);
        }
        else
        {
            _CurBattleTurn = Data_Pool.m_BattleDataPool.f_GetBattleTurn(_iTurns);
            _iTurns++;
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_BATTLE_TURNINFOR, _iTurns.ToString());
MessageBox.DEBUG("New Loop" + _iTurns);
            if (_bIsHavePlot)
            {
                //通知剧情系统回合数改变
                PlotCheckParam plotCheckParam = new PlotCheckParam();
                plotCheckParam.triggerType = EM_PlotTriggerType.Round;
                plotCheckParam.triggerParams = new int[1] { _iTurns };
                plotCheckParam.callback = f_DelayStartRoleAttack;
                glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_PLOT_CHECK, plotCheckParam);
            }
            else
            {
                f_DelayStartRoleAttack();
            }           
        }
    }

    float aMax = 0, eMax = 0, aCurrent = 0, eCurrent = 0;
    float moveStep = 0;
    private void f_DelayStartRoleAttack(object obj = null)
    {
        //      aMax = 0; eMax = 0; aCurrent = 0; eCurrent = 0;
        //      for (int i = 0; i < aRoleList.Length; i++)
        //      {
        //          var item = aRoleList[i];
        //          if (item.m_iId <= 0) continue;
        //          var role = _RolePool.f_GetRoleControl(item.m_iId);
        //          if (i < 6)
        //          {
        //              aMax += role._mRoleControlDT.m_iMaxHp;
        //              aCurrent += role._mRoleControlDT.m_iHp < 0 ? 0 : role._mRoleControlDT.m_iHp;
        //          }
        //          else
        //          {
        //              eMax += role._mRoleControlDT.m_iMaxHp;
        //              eCurrent += role._mRoleControlDT.m_iHp < 0 ? 0 : role._mRoleControlDT.m_iHp;
        //          }
        //      }
        //      float aRate = (aCurrent / aMax) > 1 ? 1 : (aCurrent / aMax);
        //      float eRate = (eCurrent / eMax) > 1 ? 1 : (eCurrent / eMax);
        //// MessageBox.ASSERT("Ally: " + aRate);
        //// MessageBox.ASSERT("Enemy: " + eRate);
        //      moveStep = (aRate - eRate)*maxBackward;

        //      ccTimeEvent.GetInstance().f_RegEvent(delayTimeEachTurn, false, null, (o)=> { RoleAttack(); });
        ccTimeEvent.GetInstance().f_RegEvent(1, false, null, CallBack_StartRoleAttack);
    }

    #region 战斗结束处理

    /// <summary>
    /// 战斗直接跳到最后一波结束状态
    /// </summary>
    private void On_ShowLastTurn(object Obj)
    {
        try
        {
            _bShowLastTurn = true;
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.FIGHTPAUSE);
            _RolePool.f_SetGoHome();
            _RolePool.f_SetDepth2Other();
            _RolePool.f_Stand();
            if (_CurBattleTurn != null)
                _CurBattleTurn.f_SetLastAttackIndex();
            _iTurns--;
            if (_iTurns < 0)
            {
                _iTurns = 0;
            }
            //重置当前状态
            while (true)
            {
                while (true)
                {
                    if (_CurBattleTurn == null)
                    {
                        if(_iTurns > 0)
						MessageBox.ASSERT(string.Format("Dữ liệu hiệp đấu null，Hiệp ：{0},Hiệp lớn nhất：{1}",_iTurns, Data_Pool.m_BattleDataPool.f_GetMaxTurn()));
                        break;
                    }
                    RoleAttack tRoleAttack = _CurBattleTurn.f_GetCurRoleAttack();
                    if (tRoleAttack == null)
                    {
                        break;
                    }
                    else
                    {
                        //RoleControl tRoleControl = _RolePool.f_GetRoleControl(tRoleAttack.m_iId);
                        for (int i = 0; i < tRoleAttack.m_aData.Count; i++)
                        {
                            RoleControl tBeAttackRoleControl = f_GetRoleControl(tRoleAttack.m_aData[i].m_iId);
                            EM_AttackType attackType = (EM_AttackType)tRoleAttack.m_aData[i].m_iIsBaseAttack;
                            if (attackType == EM_AttackType.eRoleBeginEffect || attackType == EM_AttackType.eRoleEndEffect)
                            {
                                continue;
                            }
                            int isCrit = 1;
                            if (tRoleAttack.m_aData[i].m_iCirt == 99)
                            {
                                isCrit = tRoleAttack.m_aData[i].m_iCirt;
                            }
                            tBeAttackRoleControl.f_BeAttack((EM_AttackType)tRoleAttack.m_aData[i].m_iIsBaseAttack, tRoleAttack.m_aData[i].m_iHp, isCrit, true);
                        }
                    }
                }
                _iTurns++;
                if (_iTurns >= Data_Pool.m_BattleDataPool.f_GetMaxTurn())
                {
                    break;
                }
                _CurBattleTurn = Data_Pool.m_BattleDataPool.f_GetBattleTurn(_iTurns);
            }
            _BattlePop.f_Destory();
			//My Code
			_RolePool.f_Destory2();
            //string Path = "UI Root/BattleMain/BattlePopW/BattlePopH/StatusView/Panel/Anchor-Left/";
            string Path = "UI Root/BattleMain/BattlePopW/BattlePopH/StatusView/";
            GameObject.Find(Path+ "ScrollB/ScrollView/GridCardCashSkillePlayer_BPanel").SetActive(false);
			GameObject.Find(Path+ "ScrollA/ScrollView/GridCardCashSkillePlayer_APanel").SetActive(false);
            GameObject.Find("GridElementA").SetActive(false);
            GameObject.Find("GridElementB").SetActive(false);
            UITool.f_OpenOrCloseWaitTip(true);
			//
            //显示最后的战斗状态
			foreach (RoleControl roleControl in _RolePool.GetAllRolesControl())
			{
				roleControl.m_CharActionController.f_PlayStand();
			}
            ccTimeEvent.GetInstance().f_RegEvent(1f, false, null, CallBack_BattleEnd);
        }
        catch (System.Exception e)
        {
string message = "Ignore failure：" + e.Message + "|" + e.StackTrace;
            MessageBox.ASSERT(message);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, message);
			foreach (RoleControl roleControl in _RolePool.GetAllRolesControl())
			{
				roleControl.m_CharActionController.f_PlayStand();
			}
            ccTimeEvent.GetInstance().f_RegEvent(1f, false, null, CallBack_BattleEnd);
        }
    }

    /// <summary>
    /// 换武将战斗结束
    /// </summary>
    /// <param name="obj"></param>
    public void Callback_ChangeFightRole(object obj)
    {
        _bShowLastTurn = true;
        _BattlePop.f_Destory();
        _RolePool.f_SetGoHome();
        _RolePool.f_SetDepth2Other();
        _RolePool.f_Stand();
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.FIGHTPAUSE);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_CHANGE_FIGHT_ROLE_END);
		foreach (RoleControl roleControl in _RolePool.GetAllRolesControl())
		{
			roleControl.m_CharActionController.f_PlayStand();
		}
        ccTimeEvent.GetInstance().f_RegEvent(1f, false, null, CallBack_BattleEnd);
    }

    public void UpdateElement(stFightElementInfor obj)
    {
        FightElementItem fightElementItem = _FightElementPool.f_GetElementControl(obj.m_iSide,(int)obj.m_iId);
        if(fightElementItem!= null)
        {
            fightElementItem.f_Mp(obj.m_iAnger);
        }
            
    }
    public void UpdateEffectElement(stFightElementInfor obj)
    {
        FightElementItem fightElementItem = _FightElementPool.f_GetElementControl(obj.m_iSide, (int)obj.m_iId);
        if (fightElementItem != null)
        {
              fightElementItem.PlayEffect();
        }

    }

    private void CallBack_BattleEnd(object Obj)
    {
		foreach (RoleControl roleControl in _RolePool.GetAllRolesControl())
		{
			roleControl.m_CharActionController.f_PlayStand();
		}
        if (_bIsHavePlot)
        {
            //通知剧情系统战斗结束改变
            PlotCheckParam plotCheckParam = new PlotCheckParam();
            plotCheckParam.triggerType = EM_PlotTriggerType.FightWin;
            plotCheckParam.callback = f_BattleEnd;
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_PLOT_CHECK, plotCheckParam);
        }
        else
        {
			UITool.f_OpenOrCloseWaitTip(false);
            f_BattleEnd();
        }   
    }

    /// <summary>
    /// 战斗结束
    /// </summary>
    private void f_BattleEnd(object obj = null)
    {
        Data_Pool.m_BattleDataPool.f_Reset();
        f_Close();
MessageBox.DEBUG("Combat End");
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.FIGHTPAUSE);
        ccTimeEvent.GetInstance().f_RegEvent(0.5f, false, null, (object obj1) => { glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_DIALOG_BATTLEFINISH); });
    }
	
    #endregion

    private void CallBack_StartRoleAttack(object Obj)
    {
        RoleAttack();
    }

    public Vector3 GetAttackPos(RoleControl tRoleControl, RoleAttack tRoleAttack)
    {
        if ((EM_BattleIndex)tRoleAttack.m_iStayPos == EM_BattleIndex.My)
        {
            return tRoleControl.transform.position;
        }
        if ((EM_BattleIndex)tRoleAttack.m_iStayPos == EM_BattleIndex.MyLeft)
        {
            return new Vector3(tRoleControl.transform.position.x-0.001f, tRoleControl.transform.position.y, tRoleControl.transform.position.z);
        }
        if ((EM_BattleIndex)tRoleAttack.m_iStayPos == EM_BattleIndex.MyRight)
        {
            return new Vector3(tRoleControl.transform.position.x + 0.001f, tRoleControl.transform.position.y, tRoleControl.transform.position.z);
        }
        if (m_aBattleIndex.Length < tRoleAttack.m_iStayPos)
        {
MessageBox.ASSERT("Invalid position " + tRoleAttack.m_iStayPos + " " + m_aBattleIndex.Length);
            return tRoleControl.transform.position;
        }
        return m_aBattleIndex[tRoleAttack.m_iStayPos].transform.position;
    }

    public RoleControl f_GetRoleControl(long iId)
    {
        return _RolePool.f_GetRoleControl(iId);
    }
}

public class BattleAuraData
{
    public Dictionary<int, int> dict_AuraCamp = new Dictionary<int, int>();
    public Dictionary<int, int> dict_AuraType = new Dictionary<int, int>();
    public Dictionary<int, int> dict_AuraEle = new Dictionary<int, int>();

    public BattleAuraData() { }
}