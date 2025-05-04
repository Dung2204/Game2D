using System.Collections.Generic;

public class RolePropertyPool
{
    /// <summary>
    /// 基础属性+进化
    /// </summary>
    RoleProperty _BaseRoleProperty;

    /// <summary>
    /// 天赋属性
    /// </summary>
    RoleProperty _TalentRoleProperty;

    /// <summary>
    /// 领悟
    /// </summary>
    RoleProperty _AwakenRoleProperty;

    /// <summary>
    /// 领悟装备
    /// </summary>
    RoleProperty _AwakenEquipRoleProperty;

    /// <summary>
    /// 套装属性
    /// </summary>
    RoleProperty _AllEquipRoleProperty;


    /// <summary>
    /// 宝物属性
    /// </summary>
    RoleProperty _TreasureRoleProperty;

    RoleProperty _AllGodEquipRoleProperty;


    /// <summary>
    /// 缘分
    /// </summary>
    RoleProperty _FateRoleProperty;

    /// <summary>
    /// 阵法的附加属性
    /// </summary>
    RoleProperty _DestinyRoleProperty;

    CardPoolDT _CardPoolDT;
    /// <summary>
    /// 天命
    /// </summary>
    RoleProperty _SkyRoleProperty;
    /// <summary>
    /// 大师
    /// </summary>
    RoleProperty _MasterProperty;
    /// <summary>
    /// 军团技能
    /// </summary>
    RoleProperty _LegionSkillProperty;

    RoleProperty _FanshiProperty;

    /// <summary>
    /// 神器属性
    /// </summary>
    RoleProperty _ArtifactProperty;

    /// <summary>
    /// 阵法属性
    /// </summary>
    RoleProperty _TacticalProperty;
    /// <summary>
    /// 月卡加成
    /// </summary>
    RoleProperty _MonthCard;
    /// <summary>
    /// vòng sáng phe
    /// </summary>
    RoleProperty _AuraCamp;
    RoleProperty _ElementalSeasonProperty;
    /// <summary>
    /// 另外卡牌的额外加成
    /// </summary>
    OtherProperty _OtherProperty;
    public OtherProperty m_OtherProperty
    {
        get { return _OtherProperty; }
    }

    public RolePropertyPool(CardPoolDT tCardPoolDT)
    {
        _BaseRoleProperty = new RoleProperty();
        _TalentRoleProperty = new RoleProperty();
        _AwakenRoleProperty = new RoleProperty();
        _AwakenEquipRoleProperty = new RoleProperty();
        _AllEquipRoleProperty = new RoleProperty();
        _TreasureRoleProperty = new RoleProperty();
        _AllGodEquipRoleProperty = new RoleProperty();
        _FateRoleProperty = new RoleProperty();
        _DestinyRoleProperty = new RoleProperty();
        _SkyRoleProperty = new RoleProperty();
        _MasterProperty = new RoleProperty();
        _OtherProperty = new OtherProperty();
        _LegionSkillProperty = new RoleProperty();
        _FanshiProperty = new RoleProperty();
        _ArtifactProperty = new RoleProperty();
        _TacticalProperty = new RoleProperty();
        _MonthCard = new RoleProperty();
        _AuraCamp = new RoleProperty();
        _ElementalSeasonProperty = new RoleProperty();
        _CardPoolDT = tCardPoolDT;
    }


    public void f_Reset()
    {
        _BaseRoleProperty.f_Reset();
        _TalentRoleProperty.f_Reset();
        _AwakenRoleProperty.f_Reset();
        _AwakenEquipRoleProperty.f_Reset();
        _AllEquipRoleProperty.f_Reset();
        _TreasureRoleProperty.f_Reset();
        _AllGodEquipRoleProperty.f_Reset();
        _FateRoleProperty.f_Reset();
        _DestinyRoleProperty.f_Reset();
        _SkyRoleProperty.f_Reset();
        _MasterProperty.f_Reset();
        _FanshiProperty.f_Reset();
        _LegionSkillProperty.f_Reset();
        _OtherProperty.f_Init();
        _ArtifactProperty.f_Reset();
        _TacticalProperty.f_Reset();
        _MonthCard.f_Reset();
        _AuraCamp.f_Reset();
        _ElementalSeasonProperty.f_Reset();
    }

    public RoleProperty f_Disp(List<EquipPoolDT> aEquipPoolDT, int iDestinyProgress, List<TreasurePoolDT> aTreasurePoolDT = null, int SkyDestiny = 0, List<GodEquipPoolDT> aGodEquipPoolDT = null, bool[] IsProperty = null)
    {
        if (IsProperty == null)
        {
            IsProperty = new bool[(int)EM_PropertyControl.End];
            for (int i = 0; i < IsProperty.Length; i++)
                IsProperty[i] = true;
        }
        f_Reset();

        RolePropertyTools.f_GetBaseEvolevProperty(_CardPoolDT, ref _BaseRoleProperty);
        //// xử lý lực chiến theo mùa nguyên tố => tăng chỉ số cơ bản
        //RolePropertyTools.f_GetElementalSeasonProperty(_CardPoolDT, ref _BaseRoleProperty);
        if (IsProperty[(int)EM_PropertyControl.isTalent])
        {
            RolePropertyTools.f_GetTalentProperty(_CardPoolDT, ref _TalentRoleProperty, ref _OtherProperty);
        }
        if (IsProperty[(int)EM_PropertyControl.isAwaken])
        {
            RolePropertyTools.f_GetAwakenProperty(_CardPoolDT, ref _AwakenRoleProperty);
            RolePropertyTools.f_GetAwakenEquipProperty(_CardPoolDT, ref _AwakenEquipRoleProperty);
        }
        if (IsProperty[(int)EM_PropertyControl.isSkyDestiny])
        {
            if (SkyDestiny > 0)
                RolePropertyTools.f_CountSkyDesnityForSkyLevel(SkyDestiny, ref _SkyRoleProperty);
            //_SkyRoleProperty = RolePropertyTools.f_GetSkyDestiny(SkyDestiny);
        }
        if (aEquipPoolDT != null)
            RolePropertyTools.f_GetAllEquipProperty(aEquipPoolDT, ref _AllEquipRoleProperty);
        if (aTreasurePoolDT != null)
            RolePropertyTools.f_GetTreasureProperty(_CardPoolDT, aTreasurePoolDT, ref _TreasureRoleProperty);
        if (aGodEquipPoolDT != null)
            RolePropertyTools.f_GetAllGodEquipProperty(aGodEquipPoolDT, ref _AllGodEquipRoleProperty);
        if (IsProperty[(int)EM_PropertyControl.isFate])
        {
            RolePropertyTools.f_GetFateProperty(_CardPoolDT, ref _FateRoleProperty);
        }
        RolePropertyTools.f_GetDestinyProperty(_CardPoolDT, iDestinyProgress, ref _DestinyRoleProperty);
        if (IsProperty[(int)EM_PropertyControl.isArtifact])
        {
            RolePropertyTools.f_GetArtifactProperty(_CardPoolDT, ref _ArtifactProperty);
        }
#if Game
        if (IsProperty[(int)EM_PropertyControl.isLegionSkill])
        {
            RolePropertyTools.f_UpdateLegionSkillProperty(LegionMain.GetInstance().m_LegionSkillPool, ref _LegionSkillProperty);

            _BaseRoleProperty += _LegionSkillProperty;
        }

        if (IsProperty[(int)EM_PropertyControl.isEquipFinsh])
        {
            if (_CardPoolDT.m_FanshionableDressPoolDT != null)
                RolePropertyTools.f_UpdateFanshionableDress(_CardPoolDT.m_FanshionableDressPoolDT, ref _FanshiProperty);
        }
        if (IsProperty[(int)EM_PropertyControl.isTactical])
        {
            RolePropertyTools.f_UpdateTactical(_CardPoolDT, Data_Pool.m_TariningAndTacticalPool.m_TacticalInfo.maxFormatId, ref _TacticalProperty);
        }

        if (IsProperty[(int)EM_PropertyControl.isTalent])
        {
            for (int i = 0; i < Data_Pool.m_TeamPool.f_GetAll().Count; i++)
            {
                RolePropertyTools.f_AddOtherCardProperty(ref _TalentRoleProperty,
                    ((TeamPoolDT)Data_Pool.m_TeamPool.f_GetAll()[i]).m_CardPoolDT.m_RolePropertyPool.m_OtherProperty, _CardPoolDT.m_CardDT.iCardCamp);
            }
            _BaseRoleProperty += _TalentRoleProperty;
        }
        if (IsProperty[(int)EM_PropertyControl.isMonthCard])
        {

            RolePropertyTools.f_CountMonthCard(_CardPoolDT, ref _MonthCard);
        }
        if (IsProperty[(int)EM_PropertyControl.isAuraCamp])
        {

            RolePropertyTools.f_CountAuraCamp(_CardPoolDT, ref _AuraCamp);
        }
        if (IsProperty[(int)EM_PropertyControl.isElementalSeason])
        {
            RolePropertyTools.f_GetElementalSeasonProperty(_CardPoolDT, ref _ElementalSeasonProperty);
        }


#else
        if (IsProperty[(int)EM_PropertyControl.isTactical])
        {
            RolePropertyTools.f_UpdateTactical(_CardPoolDT, ref _TacticalProperty);
        }
        if (_CardPoolDT.m_FanshionableDressPoolDT != null)
            RolePropertyTools.f_UpdateFanshionableDress(_CardPoolDT.m_FanshionableDressPoolDT.m_FashionableDressDT, ref _FanshiProperty);
        if (IsProperty[(int)EM_PropertyControl.isTalent])
        {
            _BaseRoleProperty += _TalentRoleProperty;
        }
#endif
        if (IsProperty[(int)EM_PropertyControl.isArtifact])
        {
            _BaseRoleProperty += _ArtifactProperty;
        }
        if (IsProperty[(int)EM_PropertyControl.isAwaken])
        {
            _BaseRoleProperty += _AwakenRoleProperty;
            _BaseRoleProperty += _AwakenEquipRoleProperty;
        }
        if (IsProperty[(int)EM_PropertyControl.isSkyDestiny])
        {
            _BaseRoleProperty += _SkyRoleProperty;
        }
        _BaseRoleProperty += _AllEquipRoleProperty;
        _BaseRoleProperty += _TreasureRoleProperty;
        _BaseRoleProperty += _AllGodEquipRoleProperty;
        if (IsProperty[(int)EM_PropertyControl.isFate])
        {
            _BaseRoleProperty += _FateRoleProperty;
        }

        if (IsProperty[(int)EM_PropertyControl.isTactical])
        {
            _BaseRoleProperty += _TacticalProperty;
        }
        if (IsProperty[(int)EM_PropertyControl.isMonthCard])
        {
            _BaseRoleProperty += _MonthCard;
        }

        if (IsProperty[(int)EM_PropertyControl.isAuraCamp])
        {
            _BaseRoleProperty += _AuraCamp;
        }
        if (IsProperty[(int)EM_PropertyControl.isElementalSeason])
        {
            _BaseRoleProperty += _ElementalSeasonProperty;
        }


        _BaseRoleProperty += _FanshiProperty;
        _BaseRoleProperty += _DestinyRoleProperty;

        //进行血量转处理
        //_BaseRoleProperty.f_AddHp(_BaseRoleProperty.f_GetProperty((int)EM_RoleProperty.Hp));

        //if (IsProperty[(int)EM_PropertyControl.isAdd]) {
        //    RolePropertyTools.f_GetAddProperty(ref _BaseRoleProperty);
        //}

        return _BaseRoleProperty;
    }
}