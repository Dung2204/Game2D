using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
using Spine.Unity;

public class RolePool
{
    private Dictionary<long, RoleControl> m_aRolePool = new Dictionary<long, RoleControl>();
    private List<RoleControl> m_aRoleList = new List<RoleControl>();

    public void Init()
    {
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_SHOW_FIGHT_ROLE, ShowFightRole);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_SET_FIGHT_ROLE_SKILL, SetFightRoleSkill);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_CHANGE_FIGHT_ROLE, On_ChangeFightRole);
    }
	
	public List<RoleControl> GetAllRolesControl() { return m_aRoleList; }

    public RoleControl f_GetRoleControl(long iId)
    {
        RoleControl tRoleControl = null;
        if (!m_aRolePool.TryGetValue(iId, out tRoleControl))
        {
MessageBox.ASSERT("Id information not found" + iId);
        }
        return tRoleControl;
    }

    public RoleControl f_CreatePlayerRole(long iId, EM_CloseArrayPos tEM_FormationPos, int iTempId, 
        long iMaxHp, Vector3 Pos, EM_Factions tEM_Factions, int iFanshionDressId,bool isNeedHide, bool needShowRedCard, int iAnger, int m_iGodEquipSkillId)
    {
        CardDT tCardDT = (CardDT)glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(iTempId);
        int iModelId = tCardDT.iStatelId1;
        string szModelMagic = tCardDT.szModelMagic1;
        if (iModelId == 0)
        {
            szModelMagic = tCardDT.szModelMagic2;
            iModelId = tCardDT.iStatelId2;
        }
        RoleModelDT tRoleModelDT = (RoleModelDT)glo_Main.GetInstance().m_SC_Pool.m_RoleModelSC.f_GetSC(iModelId);
        return CreateRole(iId, iMaxHp, tRoleModelDT, tEM_FormationPos, Pos, tEM_Factions, tCardDT.szModelMagic1, iFanshionDressId, isNeedHide, needShowRedCard, tCardDT.iStatelId1, tCardDT.iImportant, iAnger, m_iGodEquipSkillId, tCardDT);
    }


    public RoleControl f_CreateEnemyRole(long iId, EM_CloseArrayPos tEM_FormationPos, int iTempId, long iMaxHp,
        Vector3 Pos, EM_Factions tEM_Factions, int iFanshionDressId, bool isNeedHide, bool needShowRedCard, int iAnger, int m_iGodEquipSkillId)
    {
        MonsterDT tMonsterDT = (MonsterDT)glo_Main.GetInstance().m_SC_Pool.m_MonsterSC.f_GetSC(iTempId);
        int iModelId = tMonsterDT.iStatelId1;
        RoleModelDT tRoleModelDT = (RoleModelDT)glo_Main.GetInstance().m_SC_Pool.m_RoleModelSC.f_GetSC(iModelId);
        return CreateRole(iId, iMaxHp, tRoleModelDT, tEM_FormationPos, Pos, tEM_Factions, tMonsterDT.szModelMagic, iFanshionDressId, isNeedHide, needShowRedCard, tMonsterDT.iStatelId1, tMonsterDT.iImportant, iAnger, m_iGodEquipSkillId, tMonsterDT);
    }


    private RoleControl CreateRole(long iId, long iMaxHp, RoleModelDT tRoleModelDT, EM_CloseArrayPos tEM_FormationPos, Vector3 Pos,
        EM_Factions tEM_Factions,  string szModelMagic, int iFanshionDressId, bool isNeedHide, bool needShowRedCard, int iStatelId1, int iImportant, int iAnger, int m_iGodEquipSkillId, NBaseSCDT cardDT)
    {
        int iModelId = tRoleModelDT.iModel;
        FashionableDressDT tFashionableDressDT = null;
        if (iFanshionDressId > 0)
        {
            tFashionableDressDT = (FashionableDressDT)glo_Main.GetInstance().m_SC_Pool.m_FashionableDressSC.f_GetSC(iFanshionDressId);
            iModelId = tFashionableDressDT.iModel;
        }
        int tollgateId = StaticValue.m_CurBattleConfig.m_iTollgateId;
        RoleControl tRoleControl = RoleTools.f_CreateRole(iModelId, Pos, tEM_Factions, needShowRedCard, tollgateId != GameParamConst.PLOT_TOLLGATEID, tRoleModelDT.iId, iStatelId1, iImportant, m_iGodEquipSkillId);
        tRoleControl.f_Init(iId, iMaxHp, tRoleModelDT, szModelMagic, tEM_FormationPos, tEM_Factions, tFashionableDressDT, iAnger,cardDT);
        if (tollgateId == GameParamConst.PLOT_TOLLGATEID)
        {
            //如果是创角剧情，则添加光效
            Transform modelParent = tRoleControl.transform;
            if (modelParent != null)
            {
                GameObject magic = null;
                if (modelParent.Find("magic") != null)
                {
                    magic = modelParent.Find("magic").gameObject;

                }
                int order = modelParent.GetComponentInChildren<Renderer>().sortingOrder - 1;
                UITool.f_CreateMagicById((int)EM_MagicId.eRoleBottomLine, ref magic, modelParent, order, null);
                magic.name = "magic";
                magic.transform.GetComponent<SkeletonAnimation>().state.SetAnimation(0, "animation", true);
                magic.transform.localScale = Vector3.one * 0.5f;
                magic.SetActive(true);
                if (modelParent.Find(GameParamConst.prefabShadowName) != null)
                {
                    modelParent.Find(GameParamConst.prefabShadowName).gameObject.SetActive(false);
                }
                UITool.f_CreateEffect_Old(UIEffectName.main_shanguang_01, modelParent, Vector3.zero, 0.2f, 0, UIEffectName.UIEffectAddress2);
            }
        }

        //剧情需要，先隐藏角色，由剧情控制再显示       
        if (isNeedHide)
        {
            tRoleControl.gameObject.SetActive(false);
            tRoleControl.f_UnShowHpPanel();
        }

        if (m_aRolePool.ContainsKey(tRoleControl.m_iId))
        {
            m_aRolePool[tRoleControl.m_iId] = tRoleControl;
        }
        else
        {
            m_aRolePool.Add(tRoleControl.m_iId, tRoleControl);
        }       
        m_aRoleList.Add(tRoleControl);
		return tRoleControl;
    }

    /// <summary>
    /// 显示某个武将（剧情控制）
    /// </summary>
    /// <param name="obj"></param>
    private void ShowFightRole(object obj)
    {
        PlotDT plotDt = obj as PlotDT;
        if (null == plotDt)
        {
MessageBox.ASSERT("plotDt data null！！！");
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_SHOW_FIGHT_ROLE_END);
            return;
        }

        //分隔多个待显示角色数据
        string[] szShowFightRoleParams = plotDt.szEffectParams.Split('^');
        float showOneDelayTime = 0.5f;
        int totalShow = 0;
        for (int j = 0; j < szShowFightRoleParams.Length; j++)
        {
            string szShowFightRoleParam = szShowFightRoleParams[j];
            if (szShowFightRoleParam == "")
                continue;

            //分隔一个待显示角色数据
            string[] szShowFightRole = szShowFightRoleParam.Split(';');
            if (szShowFightRole.Length < 2)
            {
MessageBox.ASSERT("Plot character parameters are not normal，plot id：" + plotDt.iId);
                continue;
            }

            try
            {
                int camp = int.Parse(szShowFightRole[0]);
                int standIndex = int.Parse(szShowFightRole[1]);
                int fightIndex = camp * 6 + standIndex + 1;
                for (int i = 0; i < m_aRoleList.Count; i++)
                {
                    RoleControl roleControl = m_aRoleList[i];
                    if (roleControl.f_Get_FormationPos() == (EM_CloseArrayPos)standIndex && 
                        roleControl.f_Get_Factions() == (EM_Factions)camp) {

                        //显示被隐藏的模型
                        totalShow++;
                        ccTimeEvent.GetInstance().f_RegEvent(totalShow * showOneDelayTime, false, null,
                            (object obja) => {
                                //这里做延迟显示表现                               
                                roleControl.gameObject.SetActive(true);
                                roleControl.m_CharActionController.f_PlayStand();
                                roleControl.f_ShowHpPanel();
                            });                       
                    }
                }
            }
            catch (System.Exception e)
            {
MessageBox.ASSERT("Plot character parameters are not normal，plot id： " + plotDt.iId + ",Error： " + e.Message);
                break;
            }
        }

        //等延迟显示完再回调     
        ccTimeEvent.GetInstance().f_RegEvent(totalShow * showOneDelayTime, false, null,
        (object obja) => {
            //显示完还要初始化战斗速度（因为隐藏后模型的速度没有初始化到）
            int iSpeed = LocalDataManager.f_GetLocalDataIfNotExitSetData<int>(LocalDataType.Int_BattleSpeed, 1);
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_BATTLE_SPEED, iSpeed);
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_SHOW_FIGHT_ROLE_END);
        });       
    }

    /// <summary>
    /// 设定某个武将释放的技能类型（剧情控制）
    /// </summary>
    /// <param name="obj"></param>
    private void SetFightRoleSkill(object obj)
    {
        PlotDT plotDt = obj as PlotDT;
        if (null == plotDt)
        {
MessageBox.ASSERT("At SetFightRoleSkill，plotDt null！！！");
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_SET_FIGHT_ROLE_SKILL_END);
            return;
        }

        string[] szFightRoleSkillParams = plotDt.szEffectParams.Split(';');
        if (szFightRoleSkillParams.Length < 3)
        {
MessageBox.ASSERT("In SetFightRoleSkill parameter is not normal，plot id：" + plotDt.iId);
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_SET_FIGHT_ROLE_SKILL_END);
            return;
        }

        try
        {
            int camp = int.Parse(szFightRoleSkillParams[0]);
            int standIndex = int.Parse(szFightRoleSkillParams[1]);
            int skillIndex = int.Parse(szFightRoleSkillParams[2]);
            for (int i = 0; i < m_aRoleList.Count; i++)
            {
                RoleControl roleControl = m_aRoleList[i];
                if (roleControl.f_Get_FormationPos() == (EM_CloseArrayPos)standIndex &&
                    roleControl.f_Get_Factions() == (EM_Factions)camp)
                {
                    roleControl._forcePlaySkillIndex = skillIndex;
                    break;
                }
            }
        }
        catch (System.Exception e)
        {
MessageBox.ASSERT("In SetFightRoleSkill parameter is not normal，plot id： " + plotDt.iId + ",Error： " + e.Message);
        }
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_SET_FIGHT_ROLE_SKILL_END);
    }

    /// <summary>
    /// 换武将（剧情系统，约定，战斗中换了武将，该武将出现一个大招全秒对方结束战斗）
    /// </summary>
    /// <param name="obj"></param>
    private void On_ChangeFightRole(object obj)
    {
        PlotDT plotDt = obj as PlotDT;
        if (null == plotDt)
        {
MessageBox.ASSERT("At On_ChangeFightRole，plotDt null！！！");
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_CHANGE_FIGHT_ROLE_END);
            return;
        }

        RoleControl targetRoleControl = null;
        int targetRoleId = 0;
        try
        {
            string[] szTriggerParams = plotDt.szTriggerParams.Split(';');
            int camp = int.Parse(szTriggerParams[1]);
            int standIndex = int.Parse(szTriggerParams[2]);
            targetRoleId = int.Parse(plotDt.szEffectParams);
            for (int i = 0; i < m_aRoleList.Count; i++)
            {
                RoleControl roleControl = m_aRoleList[i];
                if (roleControl.f_Get_FormationPos() == (EM_CloseArrayPos)standIndex &&
                    roleControl.f_Get_Factions() == (EM_Factions)camp)
                {
                    targetRoleControl = roleControl;
                    break;
                }
            }
        }
        catch (System.Exception e)
        {
MessageBox.ASSERT("On_ChangeFightRole parameter is not normal，plot id：" + plotDt.iId + ", Error： " + e.Message);
        }

        if (null == targetRoleControl || targetRoleId <= 0)
        {
            //没有找到目标武将
            MessageBox.ASSERT("targetRoleControl null，plot id：" + plotDt.iId);
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_CHANGE_FIGHT_ROLE_END);
            return;
        }

        //销毁旧武将
        EM_CloseArrayPos pos = targetRoleControl.f_Get_FormationPos();
        EM_Factions faction = targetRoleControl.f_Get_Factions();
        long iId = targetRoleControl.m_iId;
        targetRoleControl.f_Destory();

        //替换武将
        MonsterDT monsterDT = (MonsterDT)glo_Main.GetInstance().m_SC_Pool.m_MonsterSC.f_GetSC(targetRoleId);
        f_CreateEnemyRole(iId, pos, targetRoleId, 100000000,
           BattleManager.GetInstance().GetFormationPos(pos, faction), faction, 0,false, false, monsterDT.iAnger, 0);
        RoleControl newRoleControl = f_GetRoleControl(iId);

        //一个大招把对面全秒了,构建攻击指令
        BattleTurn battleTurn = BattleManager.GetInstance().f_GetCurBattleTurn();
        RoleAttack roleAttack = new RoleAttack();
        roleAttack.m_iId = iId;
        roleAttack.m_iAnger = 4;
        roleAttack.m_iMagicId = newRoleControl._mRoleControlDT.m_aModelMagic[1];
        roleAttack.m_iStayPos = battleTurn.f_GetLastRoleAttack().m_iStayPos;
        roleAttack.m_v3AttackPos = BattleManager.GetInstance().GetAttackPos(newRoleControl, roleAttack);
        EM_Factions enemyFactions = (EM_Factions)(((int)faction + 1) % 2);
        for (int i = 0; i < m_aRoleList.Count; i++)
        {
            RoleControl roleControl = m_aRoleList[i];
            if (roleControl.f_Get_Factions() == enemyFactions)
            {
                stBeAttackInfor attackData = new stBeAttackInfor();
                attackData.m_iId = roleControl.m_iId;
                attackData.m_iHp = (int)-roleControl._mRoleControlDT.m_iHp;
                attackData.m_iIsBaseAttack = 0;
                attackData.m_iCirt = 99;
                roleAttack.m_aData.Add(attackData);
            }
        }

        //一个大招结束战斗
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.FIGHTRESUME);
        newRoleControl.f_Attacking(roleAttack,BattleManager.GetInstance().Callback_ChangeFightRole);
    }

    public void f_SetDepth2Other()
    {
        for(int i = 0; i < m_aRoleList.Count; i++)
        {
            if (RoleTools.f_CheckIsDie(m_aRoleList[i]))
            {
                continue;
            }
            //m_aRoleList[i].f_SetDepthForBeAttack();
            m_aRoleList[i].f_SetDepthForOther();
        }
    }

    public void f_SetGoHome()
    {
        for (int i = 0; i < m_aRoleList.Count; i++)
        {
            if (RoleTools.f_CheckIsDie(m_aRoleList[i]))
            {
                continue;
            }
             m_aRoleList[i].f_GoHome(null);
        }
    }


    public void f_Stand()
    {
        for (int i = 0; i < m_aRoleList.Count; i++)
        {
            if (RoleTools.f_CheckIsDie(m_aRoleList[i]))
            {
                continue;
            }
            m_aRoleList[i].f_Stand();
        }
    }

    public void f_Destory()
    {
        for (int i = 0; i < m_aRoleList.Count; i++)
        {
            if (RoleTools.f_CheckIsDie(m_aRoleList[i]))
            {
                continue;
            }
            //m_aRoleList[i].f_Destory();
			//My Code
			m_aRoleList[i].f_PlayWin();
			//
        }
    }

	public void f_Destory2()
    {
        for (int i = 0; i < m_aRoleList.Count; i++)
        {
            if (RoleTools.f_CheckIsDie(m_aRoleList[i]))
            {
                continue;
            }
            m_aRoleList[i].f_Destory();
			//My Code
			//
			//
        }
    }
}
