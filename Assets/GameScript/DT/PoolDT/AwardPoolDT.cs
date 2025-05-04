using ccU3DEngine;

public class AwardPoolDT : BasePoolDT<long>
{
    public AwardPoolDT()
    {
        mTemplate = new ResourceCommonDT();
    }

    public void f_UpdateByInfo(byte resourceType, int resourceId, int resourceNum)
    {
        mTemplate.f_UpdateInfo(resourceType, resourceId, resourceNum);
    }

    public ResourceCommonDT mTemplate
    {
        get;
        private set;
    }

    public long m_Id
    {
        get { return id; }
        set { id = value; }
    }

    private long id;    //奖励大厅id

    private byte _CenterAwardType;  //奖励类型
    public byte m_CenterAwardType
    {
        get { return _CenterAwardType; }
        set
        {
            _CenterAwardType = value;
            m_AwardCenterDT = glo_Main.GetInstance().m_SC_Pool.m_AwardCentreSC.f_GetSC((int)_CenterAwardType) as AwardCentreDT;
            if (m_AwardCenterDT == null)
            {
                m_AwardCenterDT = new AwardCentreDT();

                m_AwardCenterDT._szTitle = "";
                m_AwardCenterDT._szDesc = "";
                m_AwardCenterDT.szNote = "";
            }

        }
    }

    public int[] m_Param;     //奖励参数

    public SC_CenterAwardNode[] m_Goods;    //奖励的物品

    public AwardCentreDT m_AwardCenterDT;
}

public class ResourceCommonDT
{
    public ResourceCommonDT()
    {
        mName = string.Empty;
        mDesc = string.Empty;
        mIcon = 0;
        mImportant = (int)EM_Important.White;
    }

    /// <summary>
    /// 类型
    /// </summary>
    public int mResourceType
    {
        get;
        private set;
    }

    /// <summary>
    /// 资源Id
    /// </summary>
    public int mResourceId
    {
        get;
        private set;
    }

    /// <summary>
    /// 资源数量
    /// </summary>
    public int mResourceNum
    {
        get;
        private set;
    }

    /// <summary>
    /// 是否灰显  单独修改
    /// </summary>
    public bool isGray=false;
    public void f_UpdateInfo(byte resourceType, int resourceId, int resourceNum)
    {
        mResourceType = resourceType;
        mResourceId = resourceId;
        mResourceNum = resourceNum;
        UpdateTemplateData();
    }

    public void f_AddNum(int resourceNum)
    {
        mResourceNum += resourceNum;
    }

    public void f_SubtractNum(int resourceNum)
    {
        mResourceNum -= resourceNum;
    }

    /// <summary>
    /// 名字
    /// </summary>
    public string mName
    {
        get;
        private set;
    }

    /// <summary>
    /// 图标
    /// </summary>
    public int mIcon
    {
        get;
        private set;
    }

    /// <summary>
    /// 品质 
    /// </summary>
    public int mImportant
    {
        get;
        private set;
    }

    /// <summary>
    /// 描述
    /// </summary>
    public string mDesc
    {
        get;
        private set;
    }

    /// <summary>
    /// 更新公共数据
    /// </summary>
    private void UpdateTemplateData()
    {
        switch (mResourceType)
        {
            case (int)EM_ResourceType.Money:
                f_UpdateByMoney();
                break;
            case (int)EM_ResourceType.Good:
                f_UpdateByGood();
                break;

            case (int)EM_ResourceType.AwakenEquip:
                f_UpdateByAwakenEquip();
                break;
            case (int)EM_ResourceType.Card:
                f_UpdateByCard();
                break;
            case (int)EM_ResourceType.CardFragment:
                f_UpdateByCardFragment();
                break;
            case (int)EM_ResourceType.Equip:
                f_UpdateByEquip();
                break;
            case (int)EM_ResourceType.EquipFragment:
                f_UpdateByEquipFragment();
                break;
            case (int)EM_ResourceType.Treasure:
                f_UpdateByTreasure();
                break;
            case (int)EM_ResourceType.TreasureFragment:
                f_UpdateByTreasureFragment();
                break;
            case (int)EM_ResourceType.Fashion:
                f_UpdateByFashion();
                break;
			case (int)EM_ResourceType.GodEquip:
                f_UpdateByGodEquip();
                break;
			case (int)EM_ResourceType.GodEquipFragment:
                f_UpdateByGodEquipFragment();
                break;
            default:
				MessageBox.ASSERT("Unknown resource：Type = " + mResourceType);
                break;
        }
    }

    /// <summary>
    /// 由于没有货币表，手动填写
    /// </summary>
    private void f_UpdateByMoney()
    {
        if (mResourceId == (int)EM_UserAttr.eUserAttr_Vip)
        {
            mName = "EXP VIP";
            mIcon = 6;
            mImportant = (int)EM_Important.Oragen;
            mDesc = "EXP VIP，dùng nâng cấp VIP";
        }
        else if (mResourceId == (int)EM_UserAttr.eUserAttr_Sycee)
        {
            mName = "KNB";
            mIcon = 7;
            mImportant = (int)EM_Important.Gold;
            mDesc = "Đơn vị tiền tệ toàn năng";
        }
        else if (mResourceId == (int)EM_UserAttr.eUserAttr_Money)
        {
			mName = "Bạc";
            mIcon = 8;
            mImportant = (int)EM_Important.White;
            mDesc = "Đơn vị tiền tệ phổ biến, dùng trong hầu hết lĩnh vực";
        }
        else if (mResourceId == (int)EM_UserAttr.eUserAttr_Energy)
        {
			mName = "Thể lực";
            mIcon = 16;
            mImportant = (int)EM_Important.Oragen;
            mDesc = "Dùng để tham gia chiến trận";
        }
        else if (mResourceId == (int)EM_UserAttr.eUserAttr_Exp)
        {
            mName = "EXP";
            mIcon = 5;
            mImportant = (int)EM_Important.Oragen;
            mDesc = "Điểm kinh nghiệm có được qua chinh chiến";
        }
        else if (mResourceId == (int)EM_UserAttr.eUserAttr_TaskScore)
        {
			mName = "Điểm tích luỹ";
            mIcon = 1006;
            mImportant = (int)EM_Important.Oragen;
            mDesc = "Điểm tích luỹ ghi nhận cống hiến, dùng để nhận thưởng từ nhiệm vụ hằng ngày";
        }
        else if (mResourceId == (int)EM_UserAttr.eUserAttr_GodSoul)
        {
			mName = "Thần hồn";
            mIcon = 10;
            mImportant = (int)EM_Important.Oragen;
            mDesc = "Dùng để mua sắm tại chợ Thức Tỉnh";
        }
        else if (mResourceId == (int)EM_UserAttr.eUserAttr_Fame)
        {
			mName = "Danh vọng";
            mIcon = 11;
            mImportant = (int)EM_Important.Oragen;
            mDesc = "Danh vọng";
        }
        else if (mResourceId == (int)EM_UserAttr.eUserAttr_GeneralSoul)
        {
			mName = "Tướng hồn";
            mIcon = 13;
            mImportant = (int)EM_Important.Oragen;
            mDesc = "Dùng để mua đồ trong chợ Thần Tướng, nhận được qua thu hồi tướng";
        }
        else if (mResourceId == (int)EM_UserAttr.eUserAttr_LegionContribution)
        {
			mName = "Cống hiến";
            mIcon = 18;
            mImportant = (int)EM_Important.Oragen;
            mDesc = "Dùng để mua đồ trong chợ Quân Đoàn";
        }
        else if (mResourceId == (int)EM_UserAttr.eUserAttr_Prestige)
        {
            mName = "Uy danh";
            mIcon = 14;
            mImportant = (int)EM_Important.Oragen;
            mDesc = "Vượt qua các thử thách sẽ nhận, dùng để đổi trang bị hiếm";
        }
        else if (mResourceId == (int)EM_UserAttr.eUserAttr_Vigor)
        {
			mName = "Tinh lực";
            mIcon = 15;
            mImportant = (int)EM_Important.Oragen;
            mDesc = "Dùng để tham gia các hoạt động đặc biệt, hồi 30 mỗi ngày";
        }
        else if (mResourceId == (int)EM_UserAttr.eUserAttr_BattleFeat)
        {
			mName = "Chiến công";
            mIcon = 12;
            mImportant = (int)EM_Important.Oragen;
            mDesc = "Chiến công";
        }
        else if (mResourceId == (int)EM_UserAttr.eUserAttr_CrusadeToken)
        {
            mName = "Thảo phạt lệnh";
            mIcon = 17;
            mImportant = (int)EM_Important.Oragen;
            mDesc = "Thảo phạt lệnh";
        }
        else if (mResourceId == (int)EM_UserAttr.eUserAttr_CrossServerScore)
        {
            mName = "Điểm tích luỹ";
            mIcon = 18;
            mImportant = (int)EM_Important.Oragen;
            mDesc = "Điểm tích luỹ";
        }
        ///TsuCode -ChaosBattle
        else if (mResourceId == (int)EM_UserAttr.eUserAttr_ChaosScore)
        {
			mName = "Điểm Thí Luyện";
            mIcon = 19;
            mImportant = (int)EM_Important.Red;
            mDesc = "Điểm tích luỹ trong hoạt động Thí Luyện";
        }
        else if (mResourceId == (int)EM_UserAttr.eUserAttr_ArenaCorssMoney)
        {
            mName = "Xu Đỉnh Cao";
            mIcon = 22;
            mImportant = (int)EM_Important.Red;
            mDesc = "Dùng để mua sắm trong chợ Đấu Trường Đỉnh Cao";
        }
        else if (mResourceId == (int)EM_UserAttr.eUserAttr_TournamentPoint)
        {
            mName = "Xu Giải Đấu";
            mIcon = 23;
            mImportant = (int)EM_Important.Red;
            mDesc = "Dùng để mua sắm trong chợ Giải Đấu";
        }
        /////
        else
        {
			MessageBox.ASSERT("Đơn vị tiền chưa được xử lý frontend:Type = " + mResourceId);
            mName = UITool.f_GetGoodName((EM_ResourceType)mResourceType, mResourceId);
            mImportant = (int)EM_Important.Oragen;
            mDesc = "";
        }
    }

    private void f_UpdateByGood()
    {
        BaseGoodsDT tTemplate = (BaseGoodsDT)glo_Main.GetInstance().m_SC_Pool.m_BaseGoodsSC.f_GetSC(mResourceId);
        if (tTemplate != null)
        {
            mName = tTemplate.szName;
            mImportant = tTemplate.iImportant;
            mIcon = tTemplate.iIcon;
            mDesc = tTemplate.szReadme;
        }
    }

    private void f_UpdateByAwakenEquip()
    {
        AwakenEquipDT tTemplate = (AwakenEquipDT)glo_Main.GetInstance().m_SC_Pool.m_AwakenEquipSC.f_GetSC(mResourceId);
        if (tTemplate != null)
        {
            mName = tTemplate.szName;
            mImportant = tTemplate.iImportant;
            mIcon = tTemplate.iIcon;
            mDesc = tTemplate.szDesc;
        }
    }

    private void f_UpdateByCard()
    {
        CardDT tTemplate = (CardDT)glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(mResourceId);
        if (tTemplate != null)
        {
            mName = tTemplate.iCardType == (int)EM_CardType.RoleCard ? Data_Pool.m_UserData.m_szRoleName : tTemplate.szName;
            mImportant = tTemplate.iImportant;
            mDesc = tTemplate.szCardDesc;
            //头像ID 去模型表取
            if (tTemplate.iStatelId1 != 0)
            {
                RoleModelDT roleModle = (RoleModelDT)glo_Main.GetInstance().m_SC_Pool.m_RoleModelSC.f_GetSC(tTemplate.iStatelId1);
                mIcon = roleModle.iIcon;
            }
            else if (tTemplate.iStatelId2 != 0)
            {
                RoleModelDT roleModle = (RoleModelDT)glo_Main.GetInstance().m_SC_Pool.m_RoleModelSC.f_GetSC(tTemplate.iStatelId2);
                mIcon = roleModle.iIcon;
            }
        }
    }

    private void f_UpdateByCardFragment()
    {
        CardFragmentDT tTemplate = (CardFragmentDT)glo_Main.GetInstance().m_SC_Pool.m_CardFragmentSC.f_GetSC(mResourceId);
        CardDT cardTemplate = (CardDT)glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(mResourceId);
        if (tTemplate != null)
        {
            mName = tTemplate.szName;
            mImportant = tTemplate.iImportant;
            mDesc = tTemplate.szReadme;
            //头像ID 去模型表取
            if (cardTemplate.iStatelId1 != 0)
            {
                RoleModelDT roleModle = (RoleModelDT)glo_Main.GetInstance().m_SC_Pool.m_RoleModelSC.f_GetSC(cardTemplate.iStatelId1);
                mIcon = roleModle.iIcon;
            }
            else if (cardTemplate.iStatelId2 != 0)
            {
                RoleModelDT roleModle = (RoleModelDT)glo_Main.GetInstance().m_SC_Pool.m_RoleModelSC.f_GetSC(cardTemplate.iStatelId2);
                mIcon = roleModle.iIcon;
            }
        }
    }

    private void f_UpdateByEquip()
    {
        EquipDT tTemplate = (EquipDT)glo_Main.GetInstance().m_SC_Pool.m_EquipSC.f_GetSC(mResourceId);
        if (tTemplate != null)
        {
            mName = tTemplate.szName;
            mImportant = tTemplate.iColour;
            mIcon = tTemplate.iIcon;  //模型1id代表头像Id
            mDesc = tTemplate.szDescribe;
        }
    }

    private void f_UpdateByEquipFragment()
    {
        EquipFragmentsDT tTemplate = (EquipFragmentsDT)glo_Main.GetInstance().m_SC_Pool.m_EquipFragmentsSC.f_GetSC(mResourceId);
        if (tTemplate != null)
        {
            mName = tTemplate.szName;
            mImportant = tTemplate.iColour;
            mIcon = tTemplate.iIcon;  //模型1id代表头像Id
            mDesc = tTemplate.szDescribe;
        }
    }

    private void f_UpdateByTreasure()
    {
        TreasureDT tTemplate = (TreasureDT)glo_Main.GetInstance().m_SC_Pool.m_TreasureSC.f_GetSC(mResourceId);
        if (tTemplate != null)
        {
            mName = tTemplate.szName;
            mImportant = tTemplate.iImportant;
            mIcon = tTemplate.iIcon;
            mDesc = tTemplate.szDescribe;
        }
    }

    private void f_UpdateByTreasureFragment()
    {
        TreasureFragmentsDT tTemplate = (TreasureFragmentsDT)glo_Main.GetInstance().m_SC_Pool.m_TreasureFragmentsSC.f_GetSC(mResourceId);
        if (tTemplate != null)
        {
            mName = tTemplate.szName;
            mImportant = tTemplate.iImportant;
            mIcon = tTemplate.iIcon;
            mDesc = tTemplate.szDescribe;
        }
    }
	
    private void f_UpdateByFashion()
    {
        FashionableDressDT tTemplate = (FashionableDressDT)glo_Main.GetInstance().m_SC_Pool.m_FashionableDressSC.f_GetSC(mResourceId);
        if (tTemplate != null)
        {
            mName = tTemplate.szName;
            mImportant = tTemplate.iImportant;
            mIcon = tTemplate.iIcon;
            mDesc = tTemplate.szName;
        }
    }

    public void f_UpdateName(string name)
    {
        mName = name;
    }

    private void f_UpdateByGodEquip()
    {
        GodEquipDT tTemplate = (GodEquipDT)glo_Main.GetInstance().m_SC_Pool.m_GodEquipSC.f_GetSC(mResourceId);
        if (tTemplate != null)
        {
            mName = tTemplate.szName;
            mImportant = tTemplate.iColour;
            mIcon = tTemplate.iIcon;  //模型1id代表头像Id
            mDesc = tTemplate.szDescribe;
        }
    }

    private void f_UpdateByGodEquipFragment()
    {
        GodEquipFragmentsDT tTemplate = (GodEquipFragmentsDT)glo_Main.GetInstance().m_SC_Pool.m_GodEquipFragmentsSC.f_GetSC(mResourceId);
        if (tTemplate != null)
        {
            mName = tTemplate.szName;
            mImportant = tTemplate.iColour;
            mIcon = tTemplate.iIcon;  //模型1id代表头像Id
            mDesc = tTemplate.szDescribe;
        }
    }

}