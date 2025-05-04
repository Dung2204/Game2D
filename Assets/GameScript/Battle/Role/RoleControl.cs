using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
using DG.Tweening;
using System;

public class RoleControl : MonoBehaviour
{
    private BufControll _BufControll;
    public CharActionController m_CharActionController;
    private RoleAttack _RoleAttack;
    private ccCallback _Callback_Attack = null;
    public Vector3 _v3StayPos;
	public float NeedMove = 0f;
    private RoleControlDT _RoleControlDT = new RoleControlDT();
    protected ccMachineManager _AttackMachineManger = null;

    private  HpMpPanel _HpMpPanel      = null;
    public int _forcePlaySkillIndex    = -1;     //强制播放技能索引（剧情系统需要）
    private bool _bIsSkillEndWhenPause = false;  //是否在剧情中断战斗中，，收到了技能技术回调
	//My Code
	GameObject oRoleSpineAll;
	GameObject roleTransform;
    // hướng hiện tại
    private bool isLeft = false;
    public float walkSpeed = 2f;
    //
    private GodEquipSkillEff _GodEquipSkillEff = null;

#if DEBUGINFOR

    public int iScId = 0;
    public string strName = "";
    public string szMagic = "";

#endif

    public long m_iId
    {
        get
        {
            return _RoleControlDT.m_iId;
        }
    }

    public RoleControlDT _mRoleControlDT {
        get {
            return _RoleControlDT;
        }
    }

    private bool paused = false;
    public virtual bool IsPaused()
    {
        return paused;
    }

	private int _iStatelId1;
    private int _iImportant;
    private int i_GodEquipSkillId;
    GodEquipSkillDT godEquipDT = null;
    public void f_Create(Vector3 Pos, GameObject oRoleSpine, int iStatelId1, int iImportant, int m_iGodEquipSkillId)
    {
        transform.parent = BattleManager.GetInstance().transform;
        //scale 归一化
        //transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        transform.localScale = new Vector3(0.65f, 0.65f, 0.65f);
        roleTransform = transform.gameObject;
        _v3StayPos = transform.position = Pos;
        oRoleSpine.transform.parent = transform;
        oRoleSpine.transform.localPosition = Vector3.zero;
        oRoleSpine.transform.localScale = Vector3.one;
        oRoleSpine.transform.localRotation = Quaternion.Euler(0, 0, 0);
		oRoleSpineAll = oRoleSpine;

        _BufControll = new BufControll(this);

        m_CharActionController.f_Init(oRoleSpine, Callback_Event);

        _AttackMachineManger = new ccMachineManager(new AttackWait(this));
        _AttackMachineManger.f_RegState(new AttackStart(this));
        _AttackMachineManger.f_RegState(new AttackC(this));
        _AttackMachineManger.f_RegState(new AttackH(this));
        _AttackMachineManger.f_RegState(new AttackEnd(this, Callback_AttackEnd));
        _AttackMachineManger.f_RegState(new BaAttack(this));
        _AttackMachineManger.f_RegState(new AttackSkill3MV(this));
        _AttackMachineManger.f_RegState(new AttackSkill2MV(this));
        _AttackMachineManger.f_ChangeState((int)EM_AttackState.AttackWait);

        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.FIGHTPAUSE, OnPauseGame);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.FIGHTRESUME, OnResumeGame);

        f_Stand();
        _iStatelId1 = iStatelId1;
        _iImportant = iImportant;
        i_GodEquipSkillId = m_iGodEquipSkillId;
        godEquipDT = (GodEquipSkillDT)glo_Main.GetInstance().m_SC_Pool.m_GodEquipSkillSC.f_GetSC(i_GodEquipSkillId);
    }

    private void OnPauseGame(object data)
    {
        paused = true;
    }

    private void OnResumeGame(object data)
    {
        paused = false;
        if (_bIsSkillEndWhenPause)
        {
            _bIsSkillEndWhenPause = false;
            _AttackMachineManger.f_ChangeState((int)EM_AttackState.AttackEnd);
            if (_bOpenBg)
            {
                BattleManager.GetInstance().f_ResetBattleDepth(this, _RoleAttack);
                BattleManager.GetInstance().m_BattleMaskBG.f_Close();
                _bOpenBg = false;
            }
        }
    }

    public void f_Init(long iId, long iMaxHp, RoleModelDT tRoleModelDT, string szModelMagic, EM_CloseArrayPos tEM_FormationPos, EM_Factions tEM_Factions, FashionableDressDT tFashionableDressDT, int iAnger, NBaseSCDT tCardDT)
    {
		//My Code
		//int evolve = 0;
		//int DefaultAnger = iAnger;
		//
        _RoleControlDT.m_iId = iId;
        _RoleControlDT.m_RoleModelDT = tRoleModelDT;
        _RoleControlDT.m_iHp = _RoleControlDT.m_iMaxHp = iMaxHp;
        _RoleControlDT.m_FashionableDressDT = tFashionableDressDT;
        _RoleControlDT.m_CardDT = tCardDT;
        transform.name = iId.ToString();
        if (tRoleModelDT.iModel == 14011)
        {//用吕布来技能替换
            _RoleControlDT.m_aModelMagic = ccMath.f_String2ArrayInt("140111;140112;140113;140114", ";");
MessageBox.DEBUG("Replace Lu Bu" + iId + ">>14011");
        }
        else
        {
            _RoleControlDT.m_aModelMagic = ccMath.f_String2ArrayInt(szModelMagic, ";");
        }
        _RoleControlDT.m_EM_FormationPos = tEM_FormationPos;
        _RoleControlDT.m_EM_Factions = tEM_Factions;
        
        if (_RoleControlDT.m_EM_Factions == EM_Factions.eEnemy_B)
        {
            f_Face2Left();
        }
        else
        {
            f_Face2Right();
        }

		//My Code
		//if((Data_Pool.m_CardPool.f_GetForId(m_iId) as CardPoolDT) != null)
		//{
            ////evolve = (Data_Pool.m_CardPool.f_GetForId(m_iId) as CardPoolDT).m_iEvolveLv;
            //CardPoolDT cardPoolDT = Data_Pool.m_CardPool.f_GetForId(m_iId) as CardPoolDT;
            //for (int i = 0; i < cardPoolDT._CardEvolveDT.Count; i++)
            //{
                //CardEvolveDT cardEvolveDT = cardPoolDT._CardEvolveDT[i];
                //CardTalentDT tCardTalentDT = (CardTalentDT)glo_Main.GetInstance().m_SC_Pool.m_CardTalentSC.f_GetSC(cardEvolveDT.iTalentId);
                //if (tCardTalentDT.iPropertyId1 == (int)EM_RoleProperty.Anger)
                //{
                    //DefaultAnger += tCardTalentDT.iPropertyNum1;
                //}
                //if (tCardTalentDT.iPropertyId2 == (int)EM_RoleProperty.Anger)
                //{
                    //DefaultAnger += tCardTalentDT.iPropertyNum2;
                //}
            //}
        //}
        //设置血量初始值
        f_SetMP(iAnger, 0, EM_BattleMpType.Default);
#if DEBUGINFOR

        iScId = tRoleModelDT.iId;
        strName = tRoleModelDT.szName;
        szMagic = szModelMagic;

#endif
    }
    public void UpdateDirection()
    {
        // xử lý dữ lại hướng đánh hàng sát thủ
        if (_Save_Direction == 1)
        {
            f_Face2Left();
        }
        else if (_Save_Direction == 2)
        {
            f_Face2Right();
        }
        else
        {
            if (_RoleControlDT.m_EM_Factions == EM_Factions.eEnemy_B)
            {
                // xử lý xoay chiều tướng sát thủ
                if (_RoleControlDT.m_EM_FormationPos >= EM_CloseArrayPos.eCloseArray_PosOne && _RoleControlDT.m_EM_FormationPos <= EM_CloseArrayPos.eCloseArray_PosFive)
                {
                    f_Face2Right();
                }
                else f_Face2Left();
            }
            else
            {
                // xử lý xoay chiều tướng sát thủ
                if (_RoleControlDT.m_EM_FormationPos >= EM_CloseArrayPos.eCloseArray_PosOne && _RoleControlDT.m_EM_FormationPos <= EM_CloseArrayPos.eCloseArray_PosFive)
                {
                    f_Face2Left();
                }
                else
                    f_Face2Right();
            }
        }
        
        
    }
    private void CreateHP()
    {
        _HpMpPanel = glo_Main.GetInstance().m_ResourceManager.f_CreateHP();

        Vector3 Pos = transform.position;
        // Pos.y += m_CharActionController.f_GetHeight();
		if(ScreenControl.Instance.mFunctionRatio <= 0.85f)
		{
			Pos.y += (m_CharActionController.f_GetHeight() - 0.1f);
		}
		else
		{
			Pos.y += m_CharActionController.f_GetHeight();
		}
        // ko cho hien thi nua
        // Pos = new Vector3(99999, 99999);
		_HpMpPanel.transform.position = Pos;
        _HpMpPanel.transform.localScale = new Vector3(1, 1, 1);
        _HpMpPanel.f_Reset();
        if (Data_Pool.m_CardPool.f_GetForId(m_iId) != null)
            _HpMpPanel.f_SetEvoLevel((Data_Pool.m_CardPool.f_GetForId(m_iId) as CardPoolDT).m_iEvolveLv);
        else
            _HpMpPanel.f_SetEvoLevel(0);
        f_SetDepthForOther();
        _HpMpPanel._CardCashSkill.SetParent(_RoleControlDT.m_EM_Factions, _iStatelId1, _iImportant);
        _GodEquipSkillEff = glo_Main.GetInstance().m_ResourceManager.f_CreateGodEquipSkillEff();
        _GodEquipSkillEff.transform.position = Pos;
        _GodEquipSkillEff.transform.localScale = new Vector3(1, 1, 1);
        _GodEquipSkillEff.f_SetGodEquipUse(i_GodEquipSkillId, this);
        if(_RoleControlDT.m_CardDT.GetType() == typeof(CardDT))
        {
            _HpMpPanel.f_InitIconFightType((_RoleControlDT.m_CardDT as CardDT).iCardFightType);
        }
        else
        {
            _HpMpPanel.f_InitIconFightType((_RoleControlDT.m_CardDT as MonsterDT).iCardFightType);
        }
        
    }

    public EM_Factions f_Get_Factions()
    {
        return _RoleControlDT.m_EM_Factions;
    }

    public EM_CloseArrayPos f_Get_FormationPos()
    {
        return _RoleControlDT.m_EM_FormationPos;
    }
    //public EM_FormationPos f_Get_FormationPos()
    //{
    //    return _RoleControlDT.m_EM_FormationPos;
    //}

    public int f_GetModelId()
    {
        return _RoleControlDT.m_RoleModelDT.iId;
    }

    public void f_ShowHpPanel()
    {
        if (_HpMpPanel != null)
        {
            _HpMpPanel.gameObject.SetActive(true);
        }
    }

    public void f_UnShowHpPanel()
    {
        if (_HpMpPanel != null)
        {
            _HpMpPanel.ClearFlyTxt();
            _HpMpPanel.gameObject.SetActive(false);
        }
    }

    public void f_SetMP(int iDefaultMp, int iChangMp, EM_BattleMpType tEM_BattleMpType)
    {
        if (f_CheckIsDie())
        {
            return;
        }
        if (_HpMpPanel == null)
        {
            CreateHP();
        }

        bool bIsHavePlot = Data_Pool.m_DungeonPool.f_JudgeIsHavePlot();
        if (bIsHavePlot && iDefaultMp > 0)
        {
            //通知剧情系统怒气达到一定值
            PlotCheckParam plotCheckParam = new PlotCheckParam();
            plotCheckParam.triggerType = EM_PlotTriggerType.FightRoleAnger;
            plotCheckParam.triggerParams = new int[3];
            plotCheckParam.triggerParams[0] = (int)f_Get_Factions();
            plotCheckParam.triggerParams[1] = (int)f_Get_FormationPos();
            plotCheckParam.triggerParams[2] = iDefaultMp;
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_PLOT_CHECK, plotCheckParam);
        }      

		// MessageBox.ASSERT("iDefaultMp: " + iDefaultMp + " " + iChangMp);
        _HpMpPanel.f_Mp(iDefaultMp, iChangMp, tEM_BattleMpType);
        _HpMpPanel.f_InitHp((float)_RoleControlDT.m_iHp / _RoleControlDT.m_iMaxHp);
        _HpMpPanel.f_InitImageHp(_RoleControlDT.m_EM_Factions); 
    }

    public int f_GetMp()
    {
        return _HpMpPanel.f_GetMp();
    }

    public void f_BeAttack(EM_AttackType tEM_AttackType, int iHp, int iCriticalHit = 1, bool bIsLastTurn = false, int harmNum = 0)
    {
        if (f_CheckIsDie())
        {
			MessageBox.ASSERT("Ded");
            return;
        }

        //MessageBox.DEBUG(m_iId + " " + tEM_AttackType.ToString() + " f_BeAttack " + iHp);

        if (_HpMpPanel == null)
        {
            CreateHP();
            f_ShowHpPanel();
        }

		// MessageBox.ASSERT("ID: " + m_iId + " Critical: " + iCriticalHit);
        if (iCriticalHit == 99)
        {
            if (!bIsLastTurn)
            {
                f_PlayAttack();
            }
            //iHp = (int)_RoleControlDT.m_iHp;
            _HpMpPanel.f_LostHp(iHp, 0f, iCriticalHit, bIsLastTurn);
            _RoleControlDT.m_iHp = -99999;
            ccTimeEvent.GetInstance().f_RegEvent(0.1f, false, bIsLastTurn, Callback_StartDie);
            MessageBox.DEBUG("Die11111111111111111 " + m_iId);
        }
        else
        {
            if (iHp == 0 && iCriticalHit == 3)
            {
                _HpMpPanel.f_LostHp(0, 0, iCriticalHit, bIsLastTurn, harmNum);
            }
            else
            {
                if (iHp > 0)
                {
                    _RoleControlDT.m_iHp += iHp;
                    _HpMpPanel.f_AddHp(iHp, (float)_RoleControlDT.m_iHp / _RoleControlDT.m_iMaxHp, bIsLastTurn, harmNum);
                }
                else
                {
                    if (!bIsLastTurn)
                    {
                        f_PlayAttack();
                    }
                    _RoleControlDT.m_iHp += iHp;
                    if (_RoleControlDT.m_iHp < 0)
                    {
                        _RoleControlDT.m_iHp = 1;
                        //MessageBox.ASSERT("血量计算出错 " + _RoleControlDT.m_iId + " " + _RoleControlDT.m_iHp);
                    }
                    _HpMpPanel.f_LostHp(iHp, (float)_RoleControlDT.m_iHp / _RoleControlDT.m_iMaxHp, iCriticalHit, bIsLastTurn, harmNum);

                }
            }

        }



    }


    public bool f_CheckIsDie()
    {
        if (_RoleControlDT.m_iHp <= 0)
        {
            return true;
        }
        return false;
    }


    private int _iDieTimeEventId = -99;
    private float _fDieAlpha = 1;
    private void Callback_StartDie(object Obj)
    {
        if (_Callback_Attack != null)
        {
MessageBox.DEBUG("BeginBuf on death");
            Callback_AttackEnd(true);
        }
        if (_Callback_Attack1 != null)
        {
            MessageBox.DEBUG("BeginBuf on death");
            Callback_AttackEnd(true);
        }
        if (gameObject.activeSelf && _iDieTimeEventId == -99)
        {
            bool bIsLastTurn = (bool)Obj;
            MessageBox.DEBUG("1Die11111111111111111 " + m_iId);
            if (bIsLastTurn)
            {
                _fDieAlpha = 0;
                m_CharActionController.f_SetAlpha(_fDieAlpha);
            }
            else
            {
                _fDieAlpha = 1;
            }
            _iDieTimeEventId = ccTimeEvent.GetInstance().f_RegEvent(0.1f, true, null, DieMv);
            m_CharActionController.timeScale = 1;

        }
    }

    private void DieMv(object Obj)
    {
        m_CharActionController.f_SetAlpha(_fDieAlpha);
		//My Code
		int d = RoleTools.f_GetFormationPosDepth(_RoleControlDT.m_EM_FormationPos);
		m_CharActionController.f_PlayDead(d);
		//MessageBox.ASSERT("Ded " + _RoleControlDT.m_EM_FormationPos);
		
        if (_fDieAlpha <= 0)
        {
            ccTimeEvent.GetInstance().f_UnRegEvent(_iDieTimeEventId);
			//My Code
			m_CharActionController.f_PlayDead(d);
			//
            f_Destory();
        }
        else
        {
            _fDieAlpha -= 0.2f;
        }
    }

    public void f_Destory()
    {
		
        //My Code
		m_CharActionController.f_SetAlpha(0);
        //gameObject.SetActive(false);
		oRoleSpineAll.transform.Find("Aura")?.gameObject.SetActive(false);
		oRoleSpineAll.transform.Find("Shadow")?.gameObject.SetActive(false);
		roleTransform.SetActive(false);
		// MessageBox.ASSERT("A: " + oRoleSpineAll);
        if (_HpMpPanel != null)
        {
            Destroy(_HpMpPanel.gameObject);
        }
        else
        {
MessageBox.ASSERT("Empty HP Bar,id"+ _RoleAttack.m_iId + ",objName:"+gameObject.name);
        }
        //

        // m_CharActionController.f_SetAlpha(0);
        // gameObject.SetActive(false);
        // Destroy(gameObject);
        // if (_HpMpPanel != null)
        // {
        // Destroy(_HpMpPanel.gameObject);
        // }
        // else
        // {
        // MessageBox.ASSERT("Thanh HP trống,id"+ _RoleAttack.m_iId + ",objName:"+gameObject.name);
        // }
        if (_GodEquipSkillEff != null)
        {
            Destroy(_GodEquipSkillEff.gameObject);
        }
    }
	
	public void f_PlayWin()
	{
		m_CharActionController.f_PlayStand();
	}

    public void f_Face2Left()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        isLeft = true;
    }

    public void f_Face2Right()
    {
        transform.rotation = Quaternion.Euler(0, 180, 0);
        isLeft = false;
    }
    private int _Save_Direction;
    public void f_SaveFaceDirection(int direction)
    {
        _Save_Direction = direction;
        //direction : 1 f_Face2Left , 2  f_Face2Right / k xoay /
    }

    private void Update()
    {
        if (IsPaused())
        {
            return;
        }
        if (_AttackMachineManger != null)
        {
            _AttackMachineManger.f_Update();
        }
		if (_HpMpPanel != null)
        {
            Vector3 posHP = _HpMpPanel.transform.position;
            posHP.x = transform.position.x;
            _HpMpPanel.transform.position = posHP;
        }
    }

    #region 攻击相关
    private bool _bOpenBg;    
    private void Callback_Event(object Obj)
    {
        string ppSQL = (string)Obj;

        if (IsPaused())
        {
            if (ppSQL == "Skill_Complete")
            {
                _bIsSkillEndWhenPause = true;
            }
            return;
        }
        if (ppSQL == "OnPlaySound")
        {
            if (_RoleAttack != null)
            {
                string name = _RoleAttack.m_iMagicSound.ToString();
                float pitch = (float)(LocalDataManager.f_GetLocalData<int>(LocalDataType.Int_BattleSpeed));
				glo_Main.GetInstance().m_AdudioManager.f_PlayAudioDialog("skill_" + _RoleAttack.m_iMagicId, 1);
                glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMagic(name, pitch);
            }
        }
        else if (ppSQL == "OnCreateMagicBall")
        {//OnCreateMagicBall 产生弹道
            //_AttackMachineManger.f_ChangeState((int)EM_AttackState.AttackC);
            AttackC tAttackC = (AttackC)_AttackMachineManger.f_GetStaticBase((int)EM_AttackState.AttackC);
            tAttackC.f_CrearteTrajectory();
        }
        else if (ppSQL == "OnCreateMagicHarm")
        {
            ccMachineStateBase tccMachineStateBase = _AttackMachineManger.f_GetCurMachineState();
            if ((EM_AttackState)tccMachineStateBase.iId == EM_AttackState.AttackC)
            {
                AttackC tAttackC = (AttackC)tccMachineStateBase;
                tAttackC.f_CrearteTrajectory();
            }
            else if ((EM_AttackState)tccMachineStateBase.iId == EM_AttackState.AttackH)
            {
                AttackH tAttackH = (AttackH)tccMachineStateBase;
                tAttackH.f_RoleDispHarm(null);
            }
        }
        else if (ppSQL == "OnOpenBg")
        {
            Action cb = () => { if (GetMagicIndex() == 3) {DoScaleTime(m_CharActionController.GetAnimationTimeByMagicIndex(GetMagicIndex()));}};
            if (GetMagicIndex() == 3) { glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_BATTLE_SKILL_OPEN, cb);}
            BattleManager.GetInstance().f_SetBattleDepth(this, _RoleAttack);
            _bOpenBg = true;
        }
        else if (ppSQL == "OnCloseBg")
        {
            BattleManager.GetInstance().f_ResetBattleDepth(this, _RoleAttack);
            //BattleManager.GetInstance().m_BattleMaskBG.f_Close();
            if (GetMagicIndex() == 3){glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_BATTLE_SKILL_CLOSE);}
            _bOpenBg = false;
        }
        else if (ppSQL == "Skill_Complete")
        {
			if (_bOpenBg)
            {
MessageBox.ASSERT("No close message found, close manually：" + _RoleControlDT.m_RoleModelDT.iId + " " + _RoleAttack.m_iMagicId);
                BattleManager.GetInstance().f_ResetBattleDepth(this, _RoleAttack);
                BattleManager.GetInstance().m_BattleMaskBG.f_Close();
                _bOpenBg = false;
            }
            _AttackMachineManger.f_ChangeState((int)EM_AttackState.AttackEnd);
        }
        else
        {
MessageBox.ASSERT(ppSQL + " undefined character action： " + _RoleControlDT.m_RoleModelDT.iId + " " + _RoleAttack.m_iMagicId);
        }
    }

    public int GetMagicIndex()
    {
        for (int i = 0; i < _RoleControlDT.m_aModelMagic.Length; i++)
        {
            if (_RoleAttack.m_iMagicId == _RoleControlDT.m_aModelMagic[i])
            {
                if (i == 0)
                {
                    return 1;
                }
                else if (i == 1)
                {
                    return 2;
                }
                else if (i == 2)
                {
                    return 3;
                }
                else if (i == 3)
                {
                    return 4;
                }

            }
        }
        if (glo_Main.GetInstance().m_SC_Pool.m_MagicSC.f_GetSC(_RoleAttack.m_iMagicId)!= null)
        {
            return 1;
        }
        //if (_RoleAttack.m_iMagicId >= 555555)
        //    return 1;
        return -99;
    }

    private void CreateMagicData(int iIndex)
    {
        string szMagicTrajectory = "";
        if (iIndex == 1)
        {
            szMagicTrajectory = _RoleControlDT.m_RoleModelDT.szMagic1;
            if (_RoleControlDT.m_FashionableDressDT != null)
            {
                szMagicTrajectory = _RoleControlDT.m_FashionableDressDT.szMagic1;
            }
            _RoleAttack.m_iMaxHarmNum = _RoleControlDT.m_RoleModelDT.iMagicHarm1;
            _RoleAttack.m_iMagicAttackPos = _RoleControlDT.m_RoleModelDT.iAttackPos1;
            _RoleAttack.m_iMagicType = _RoleControlDT.m_RoleModelDT.iMagicType1;
            _RoleAttack.m_iFitMagic = _RoleControlDT.m_RoleModelDT.iFitMagic1;
            _RoleAttack.m_iMagicSound = _RoleControlDT.m_aModelMagic[0];
        }
        else if (iIndex == 2)
        {
            szMagicTrajectory = _RoleControlDT.m_RoleModelDT.szMagic2;
            if (_RoleControlDT.m_FashionableDressDT != null)
            {
                szMagicTrajectory = _RoleControlDT.m_FashionableDressDT.szMagic2;
            }
            _RoleAttack.m_iMaxHarmNum = _RoleControlDT.m_RoleModelDT.iMagicHarm2;
            _RoleAttack.m_iMagicAttackPos = _RoleControlDT.m_RoleModelDT.iAttackPos2;
            _RoleAttack.m_iMagicType = _RoleControlDT.m_RoleModelDT.iMagicType2;
            _RoleAttack.m_iFitMagic = _RoleControlDT.m_RoleModelDT.iFitMagic2;
            _RoleAttack.m_iMagicSound = _RoleControlDT.m_aModelMagic[1];
        }
        else if (iIndex == 3)
        {
            szMagicTrajectory = _RoleControlDT.m_RoleModelDT.szMagic3;
            if (_RoleControlDT.m_FashionableDressDT != null)
            {
                szMagicTrajectory = _RoleControlDT.m_FashionableDressDT.szMagic3;
            }
            _RoleAttack.m_iMaxHarmNum = _RoleControlDT.m_RoleModelDT.iMagicHarm3;
            _RoleAttack.m_iMagicAttackPos = _RoleControlDT.m_RoleModelDT.iAttackPos3;
            _RoleAttack.m_iMagicType = _RoleControlDT.m_RoleModelDT.iMagicType3;
            _RoleAttack.m_iFitMagic = _RoleControlDT.m_RoleModelDT.iFitMagic3;
            _RoleAttack.m_iMagicSound = _RoleControlDT.m_aModelMagic[1];
        }
        else if (iIndex == 4)
        {
            szMagicTrajectory = _RoleControlDT.m_RoleModelDT.szMagic4;
            if (_RoleControlDT.m_FashionableDressDT != null)
            {
                szMagicTrajectory = _RoleControlDT.m_FashionableDressDT.szMagic4;
            }
            _RoleAttack.m_iMaxHarmNum = _RoleControlDT.m_RoleModelDT.iMagicHarm4;
            _RoleAttack.m_iMagicAttackPos = _RoleControlDT.m_RoleModelDT.iAttackPos4;
            _RoleAttack.m_iMagicType = _RoleControlDT.m_RoleModelDT.iMagicType4;
            _RoleAttack.m_iFitMagic = _RoleControlDT.m_RoleModelDT.iFitMagic4;
            _RoleAttack.m_iMagicSound = _RoleControlDT.m_aModelMagic[1];
        }
        if (!string.IsNullOrEmpty(szMagicTrajectory))
        {//多段攻击
            int[] aTrajectory = ccMath.f_String2ArrayInt(szMagicTrajectory, ";");
            if (aTrajectory.Length == 2)
            {
                _RoleAttack.m_iTrajectoryC = aTrajectory[0];
                _RoleAttack.m_iTrajectoryH = aTrajectory[1];
            }
            else
            {
MessageBox.ASSERT("Wrong ranged attack type：" + _RoleAttack.m_iMagicId);
            }
        }
        if (!_RoleAttack.CreateHarmData())
        {
MessageBox.ASSERT("Damage unknown：" + _RoleControlDT.m_RoleModelDT.iId + " " + _RoleAttack.m_iMagicId);
        }

    }

    public void f_Attacking(RoleAttack tRoleAttack, ccCallback tccCallback)
    {
        _Callback_Attack = tccCallback;
        _RoleAttack = tRoleAttack;
MessageBox.DEBUG("Assault character " + m_iId + " " + _RoleAttack.m_iMagicId);
        f_GodEquipEffect(_RoleAttack.m_iIsActiveGodEquipSkill);

        //计算攻击位置
        int iIndex = GetMagicIndex();
        if (iIndex < 0)
        {
MessageBox.ASSERT("Error Id Skill " + _RoleControlDT.m_RoleModelDT.iId + " " + _RoleAttack.m_iMagicId);
        }
        CreateMagicData(iIndex);
        // if (iIndex == 2)
        // {
            //MessageBox.DEBUG("合击技能 " + _RoleAttack.m_iMagicId);
            // _AttackMachineManger.f_ChangeState((int)EM_AttackState.AttackSkill2MV);
        // }
        // else if (iIndex == 3)
		if (iIndex == 3)
        {
            MessageBox.DEBUG("合击技能 " + _RoleAttack.m_iMagicId);
            _AttackMachineManger.f_ChangeState((int)EM_AttackState.AttackSkill3MV);
        }
        else
        {
            _AttackMachineManger.f_ChangeState((int)EM_AttackState.AttackStart);
        }
        // update nộ nguyên tố trùng kích
        BattleManager.GetInstance().UpdateElement(_RoleAttack.m_tFightElement);
    }
    private ccCallback _Callback_Attack1 = null;
    public void f_FightElementAttacking(RoleAttack tRoleAttack, ccCallback tccCallback)
    {
        _Callback_Attack1 = tccCallback;
        _RoleAttack = tRoleAttack;
        MessageBox.DEBUG("Assault character " + m_iId + " " + _RoleAttack.m_iMagicId);
        MessageBox.DEBUG("Nguyên tố trùng kích-----------------------");
        MagicDT tMagicDT = (MagicDT)glo_Main.GetInstance().m_SC_Pool.m_MagicSC.f_GetSC(_RoleAttack.m_iIsActiveFightElementSkill);
        BattleManager.GetInstance().UpdateEffectElement(_RoleAttack.m_tFightElement);
        UITool.Ui_Trip(tMagicDT.szName);
        //CreateMagicData(1);
        //_AttackMachineManger.f_ChangeState((int)EM_AttackState.AttackStart);
        //// update nộ nguyên tố trùng kích
        //BattleManager.GetInstance().UpdateElement(_RoleAttack.m_tFightElement);


        //////Skill2MvParam
        FitSkillParam tSkill2MvParam = new FitSkillParam(tMagicDT, this, (data) =>
        {
            //Skill2MvParam tSkill2MvParam = new Skill2MvParam(tMagicDT, this, (data) => {
            CreateMagicData(1);
            _AttackMachineManger.f_ChangeState((int)EM_AttackState.AttackStart);
            // update nộ nguyên tố trùng kích
           
            BattleManager.GetInstance().UpdateElement(_RoleAttack.m_tFightElement);
        });
        ccUIManage.GetInstance().f_SendMsg(UINameConst.FitSkillPage, UIMessageDef.UI_OPEN, tSkill2MvParam);
    }

    public void DoScaleTime(float second)
    {
        StartCoroutine(ScaleTimeSkill(second));
    }

    IEnumerator ScaleTimeSkill(float second)
    {
        f_SetDepthForSpecialSkill(true);
        m_CharActionController.SetUnScaledTime(true);
        Time.timeScale = 0.05f;
        yield return new WaitForSecondsRealtime(second);
        m_CharActionController.SetUnScaledTime(false);
        f_SetDepthForSpecialSkill(false);
        Time.timeScale = 1f;
    }

    public void f_GoAttackPos()
    {
        transform.position = _RoleAttack.m_v3AttackPos;
    }

    public int f_GetMagicAttackPos()
    {
        return _RoleAttack.m_iMagicAttackPos;
    }

    public void f_GoHome(ccCallback callback)
    {
        float newX = _v3StayPos.x;
        Vector3 pos = _v3StayPos;
        UpdateDirectionByMove(transform.position.x,newX);
        int speed = LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_BattleSpeed, 1);
		m_CharActionController.f_PlayWalk(walkSpeed* speed);
        transform.DOMove(pos, System.Math.Abs((newX + 100f) - (transform.position.x + 100f)) * (walkSpeed/ speed)).OnComplete(() => { UpdateDirection(); if (callback != null) callback(null); });
        //transform.position = _v3StayPos;
    }

    public void UpdateDirectionByMove(float oldX,float newX)
    {
        if (isLeft)// hướng trái
        {
            if(oldX < newX)
            {
                f_Face2Right();
            }
        }
        else
        {
            if (oldX > newX)
            {
                f_Face2Left();
            }
        }
    }

    public void f_Stand()
    {
        MessageBox.DEBUG(m_iId + " f_Stand " + _AttackMachineManger.f_GetCurMachineState().iId);
        if (_AttackMachineManger.f_GetCurMachineState().iId == (int)EM_AttackState.AttackWait &&
            _AttackMachineManger.f_GetCurMachineState().iId == (int)EM_AttackState.BaAttack)
        {
            m_CharActionController.f_PlayStand();
        }
        else
        {
MessageBox.DEBUG("The attack hasn't ended，pause for status change");
        }
    }

    public void f_StartAttack()
    {
        int iMagicIndex = GetMagicIndex(_RoleAttack.m_iMagicId);       
        bool isHavePlot = Data_Pool.m_DungeonPool.f_JudgeIsHavePlot();
        if (isHavePlot)
        {
            //剧情系统逻辑
            if (_forcePlaySkillIndex >= 0)
            {
                //强制播放指定技能，剧情需要
                iMagicIndex = _forcePlaySkillIndex;
                _forcePlaySkillIndex = -1;
            }

            //通知剧情系统某个阵营某个站位武将技能触发            
            PlotCheckParam plotCheckParam = new PlotCheckParam();
            plotCheckParam.triggerType = EM_PlotTriggerType.FightRoleSkill;
            plotCheckParam.triggerParams = new int[3];
            plotCheckParam.triggerParams[0] = (int)f_Get_Factions();
            plotCheckParam.triggerParams[1] = (int)f_Get_FormationPos();
            plotCheckParam.triggerParams[2] = iMagicIndex;
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_PLOT_CHECK, plotCheckParam);
        }

        m_CharActionController.f_PlaySkill(iMagicIndex);
        _HpMpPanel._CardCashSkill.f_PlaySkill(iMagicIndex);
        f_SetDepthForAttack();
    }

    public void f_PlayAttack()
    {
        if (_AttackMachineManger.f_GetCurMachineState().iId == (int)EM_AttackState.AttackWait ||
            _AttackMachineManger.f_GetCurMachineState().iId == (int)EM_AttackState.BaAttack)
        {
            m_CharActionController.f_PlayBeAttack();
            //MessageBox.DEBUG("被攻击 " + _RoleControlDT.m_RoleModelDT.szName);
        }
        else
        {
            //MessageBox.ASSERT("被攻击状态错误 " + _RoleControlDT.m_RoleModelDT.szName + " " + _AttackMachineManger.f_GetCurMachineState().iId);
        }

    }

    private void Callback_AttackEnd(object Obj)
    {
        bool bIsDieAttackEnd = false;
        if (Obj != null)
        {
            bIsDieAttackEnd = (bool)Obj;
        }
        if (!bIsDieAttackEnd)
        {
            f_SetMP(_RoleAttack.m_iAnger, 0, EM_BattleMpType.Default);
            f_ShowHpPanel();
            _AttackMachineManger.f_ChangeState((int)EM_AttackState.AttackWait);
MessageBox.DEBUG("------------ Character attack ends------------");
            f_SetDepthForOther();
        }

        if (_Callback_Attack != null)
        {
            _Callback_Attack(this);
            _Callback_Attack = null;
        }else if (_Callback_Attack1 != null)
        {
            _Callback_Attack1(this);
            _Callback_Attack1 = null;
        }
        else
        {
MessageBox.DEBUG("Callback_AttackEnd failed");
        }
    }

    public int GetMagicIndex(int iMagicId)
    {
        for (int i = 0; i < _RoleControlDT.m_aModelMagic.Length; i++)
        {
            if (_RoleControlDT.m_aModelMagic[i] == iMagicId)
            {
                return i;
            }
        }
        //nguyên tố trùng kích
        if (glo_Main.GetInstance().m_SC_Pool.m_MagicSC.f_GetSC(iMagicId) != null)
        {
            return 1;
        }
        //if (iMagicId >= 555555)
        //    return 1;
        return -99;
    }

    public RoleAttack f_GetRoleAttack()
    {
        return _RoleAttack;
    }
    #endregion

    #region 深度层次设置

    public void f_SetDepthForAttack()
    {
        int iDepth = RoleTools.f_GetFormationPosDepth(_RoleControlDT.m_EM_FormationPos);
        if (_HpMpPanel != null)
        {
            _HpMpPanel.f_SetDepthForAttack(iDepth);
        }
        m_CharActionController.f_SetDepthForAttack();
        _BufControll.f_UpdateDepth();
    }

    public void f_SetDepthForSpecialSkill(bool isSkill)
    {
        m_CharActionController.f_SetSkillDepth(isSkill);
        _BufControll.f_UpdateDepth();
    }

    public void f_SetDepthForBeAttack(int aokiji)
    {
        int iDepth = RoleTools.f_GetFormationPosDepth(_RoleControlDT.m_EM_FormationPos);
        if (_HpMpPanel != null)
        {
            _HpMpPanel.f_SetDepthForBeAttack(iDepth);
        }
		//My Code code nay chi dung cho char aokiji
		// if(aokiji == 1)
		// {
			// iDepth = iDepth + 11;
			// MessageBox.ASSERT("1" + iDepth);
		// }
		//
        m_CharActionController.f_SetDepthForBeAttack(iDepth);
        _BufControll.f_UpdateDepth();
    }

    public void f_SetDepthForOther()
    {
        int iDepth = RoleTools.f_GetFormationPosDepth(_RoleControlDT.m_EM_FormationPos);
        if (_HpMpPanel != null)
        {
            _HpMpPanel.f_SetDepthForOther(iDepth);
        }
        m_CharActionController.f_SetDepthForOther(iDepth);
        _BufControll.f_UpdateDepth();
    }

    public int f_GetDepth()
    {
        return m_CharActionController.f_GetDepth();
    }

    #endregion

    #region Buf

    //private void DispBuf()
    //{
    //    for (int i = 0; i < _RoleAttack.m_aData.Count; i++)
    //    {
    //        RoleControl tBeAttackRoleControl = BattleManager.GetInstance().f_GetRoleControl(_RoleAttack.m_aData[i].m_iId);
    //        if (tBeAttackRoleControl == null)
    //        {
    //            continue;
    //        }
    //        if (_RoleAttack.m_aData[i].m_iBuf1 == -99)
    //        {
    //            tBeAttackRoleControl.f_UpdateBuf(-99, true);
    //        }
    //        else if (_RoleAttack.m_aData[i].m_iBuf1 > 0)
    //        {
    //            tBeAttackRoleControl.f_UpdateBuf(_RoleAttack.m_aData[i].m_iBuf1, true);
    //        }
    //    }
    //}

    public void f_UpdateBuf(int iBufEffect)
    {
        _BufControll.f_UpdateBuf(iBufEffect);

        //判断是否眩晕飘字，
        if ((iBufEffect % 100) / 10 > 0) {
            _HpMpPanel.f_Dizzy();
        }
    }

    public void f_ShowBuf()
    {
        _BufControll.f_ShowBuf();
    }

    public void f_UnShowBuf()
    {
        _BufControll.f_UnShowBuf();
    }

    public void f_DispBeginBuf()
    {
        RoleAttack tRoleAttack = f_GetRoleAttack();
        Dictionary<long, List<stBeAttackInfor>> dict_m_aData = new Dictionary<long, List<stBeAttackInfor>>();
        for (int i = 0; i < tRoleAttack.m_aData.Count; i++)
        {
            if ((EM_AttackType)tRoleAttack.m_aData[i].m_iIsBaseAttack == EM_AttackType.eBufBegin)
            {
                RoleControl tRoleControl = this;
                if (tRoleAttack.m_aData[i].m_iId != m_iId)
                {
                    tRoleControl = BattleManager.GetInstance().f_GetRoleControl(tRoleAttack.m_aData[i].m_iId);
                }
                tRoleControl.f_BeAttack((EM_AttackType)tRoleAttack.m_aData[i].m_iIsBaseAttack, tRoleAttack.m_aData[i].m_iHp, tRoleAttack.m_aData[i].m_iCirt);
            }
            if ((EM_AttackType)tRoleAttack.m_aData[i].m_iIsBaseAttack == EM_AttackType.eRoleBeginEffect)
            {
                RoleControl tRoleControl = this;
                if (tRoleAttack.m_aData[i].m_iId != m_iId)
                {
                    tRoleControl = BattleManager.GetInstance().f_GetRoleControl(tRoleAttack.m_aData[i].m_iId);
                }
                tRoleControl.f_UpdateBuf(tRoleAttack.m_aData[i].m_iBuf1);
            }
            if ((EM_AttackType)tRoleAttack.m_aData[i].m_iIsBaseAttack == EM_AttackType.eRoleBuffIcon)
            {
                List<stBeAttackInfor> list_stBeAttackInfors = null;// bị null chỗ này sẽ dính bug đánh loạn xạ
                dict_m_aData.TryGetValue(tRoleAttack.m_aData[i].m_iId, out list_stBeAttackInfors);
                if (list_stBeAttackInfors == null)
                {
                    list_stBeAttackInfors = new List<stBeAttackInfor>();
                    list_stBeAttackInfors.Add(tRoleAttack.m_aData[i]);
                    dict_m_aData.Add(tRoleAttack.m_aData[i].m_iId, list_stBeAttackInfors);
                }
                else
                {
                    list_stBeAttackInfors.Add(tRoleAttack.m_aData[i]);
                }
            }
            if ((EM_AttackType)tRoleAttack.m_aData[i].m_iIsBaseAttack == EM_AttackType.eRoleGodEquipEffect)
            {
                RoleControl tRoleControl = this;
                if (tRoleAttack.m_aData[i].m_iId != m_iId)
                {
                    tRoleControl = BattleManager.GetInstance().f_GetRoleControl(tRoleAttack.m_aData[i].m_iId);
                }
                tRoleControl.f_GodEquipEffect(tRoleAttack.m_aData[i].m_iBuf1);
            }

        }
        for (int i = 0; i < tRoleAttack.m_aData.Count; i++)
        {
            if ((EM_AttackType)tRoleAttack.m_aData[i].m_iIsBaseAttack == EM_AttackType.eRoleBuffIcon)
            {
                List<stBeAttackInfor> list_stBeAttackInfors = null;
                dict_m_aData.TryGetValue(tRoleAttack.m_aData[i].m_iId, out list_stBeAttackInfors);
                if (list_stBeAttackInfors != null)
                {
                    RoleControl tRoleControl = this;
                    if (tRoleAttack.m_aData[i].m_iId != m_iId)
                    {
                        tRoleControl = BattleManager.GetInstance().f_GetRoleControl(tRoleAttack.m_aData[i].m_iId);
                    }
                    tRoleControl.f_ListBuff(list_stBeAttackInfors);
                }
            }
        }
        if (f_CheckIsDie())
        {
            _AttackMachineManger.f_ChangeState((int)EM_AttackState.AttackWait);
        }
    }

    #endregion

    public void f_ListBuff(List<stBeAttackInfor> list_stBeAttackInfors)
    {
        _HpMpPanel.f_UpdateListBuffIcon(list_stBeAttackInfors);
    }

    public void f_GodEquipEffect(int m_iIsActiveGodEquipSkill)
    {
        if (m_iIsActiveGodEquipSkill != 0)
        {
            MessageBox.ASSERT("Kích hoạt thần binh");
            if (_GodEquipSkillEff == null) return;


            MagicDT MagicDT = (MagicDT)glo_Main.GetInstance().m_SC_Pool.m_MagicSC.f_GetSC(godEquipDT.iMagicId);
            if (MagicDT == null) return;
            string szName = MagicDT.szName;
            _GodEquipSkillEff.f_ShowGodEquipSkillName(MagicDT);

        }
    }

}