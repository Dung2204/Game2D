/// <summary>
/// 工会相关公共方法
/// </summary>
public class LegionTool
{
    /// <summary>
    /// 获取升级所需经验
    /// </summary>
    /// <param name="lv"></param>
    /// <returns></returns>
    public static int f_GetLvUpExpValue(int lv)
    {
        int result = 0;
        LegionLevelDT dt = (LegionLevelDT)glo_Main.GetInstance().m_SC_Pool.m_LegionLevelSC.f_GetSC(lv + 1);
        if(dt != null)
            result = dt.iExp;
        return result;
    }

    public static void f_SelfPlayerInfoDispose(ref BasePlayerPoolDT playerInfo)
    {
        //处理自己的数据
        // 0 男 1女
        //int selfCardSex = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_MainCard) % 2;
        int selfCardSex = Data_Pool.m_CardPool.mRolePoolDt.m_FanshionableDressPoolDT == null ? Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_MainCard) % 2 : Data_Pool.m_CardPool.mRolePoolDt.m_FanshionableDressPoolDT.m_iTempId;
        int vipLv = UITool.f_GetNowVipLv();
        int frameId = Data_Pool.m_CardPool.mRolePoolDt.m_CardDT.iImportant;
        int title = 0; //暂无处理
        string guildName = string.Empty;
        if (null != LegionMain.GetInstance().m_LegionInfor && null != LegionMain.GetInstance().m_LegionInfor.f_getUserLegion())
            guildName = LegionMain.GetInstance().m_LegionInfor.f_getUserLegion().LegionName;
        int offlineTime = 0;
        int lv = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        int power = Data_Pool.m_TeamPool.f_GetTotalBattlePower();
        if(playerInfo == null)
        {
            playerInfo = new BasePlayerPoolDT();
            playerInfo.iId = Data_Pool.m_UserData.m_iUserId;
            Data_Pool.m_GeneralPlayerPool.f_AddPlayer(playerInfo);
        }
        playerInfo.f_SaveBase(Data_Pool.m_UserData.m_szRoleName, selfCardSex, frameId, vipLv, title);
        playerInfo.f_SaveExtrend(guildName, lv, power, offlineTime);
    }

    public static string f_GetPosDesc(int pos)
    {
        string result;
        if(pos == (int)EM_LegionPostionEnum.eLegion_Chief)
        {
result = "Captain";
        }
        else if(pos == (int)EM_LegionPostionEnum.eLegion_Deputy)
        {
result = "Deputy Captain";
        }
        else if(pos == (int)EM_LegionPostionEnum.eLegion_Normal)
        {
result = "Member";
        }
        else
        {
result = "Substitute";
        }
        return result;
    }

    /// <summary>
    /// 获取军团职位flag 名字
    /// </summary>
    /// <param name="pos">pos</param>
    /// <returns></returns>
    public static string f_GetPosFlagName(int pos)
    {
        return pos < LegionConst.LEGION_POSTION_FLAG_NAME.Length ? LegionConst.LEGION_POSTION_FLAG_NAME[pos] : LegionConst.LEGION_POSTION_FLAG_NAME[(int)EM_LegionPostionEnum.eLegion_Normal];
    }


    /// <summary>
    /// 是否满足权限
    /// </summary>
    /// <param name="tType"></param>
    /// <param name="showTip"></param>
    /// <param name="legionPos"> == 0?用自己的职位：用传入的职位</param>
    /// <returns>true:有权限执行，false：没权限</returns>
    public static bool f_IsEnoughPermission(EM_LegionOperateType tType, bool showTip = true, int legionPos = 0)
    {
        EM_LegionPermission permission = LegionMain.GetInstance().m_LegionPlayerPool.f_CheckOperateTypePermission(tType, legionPos);
        if(permission == EM_LegionPermission.Enough)
        {
            return true;
        }
        else
        {
            if(showTip)
            {
                if(permission == EM_LegionPermission.Chief)
                {
UITool.Ui_Trip("Không đủ quyền hạn");
                }
                else if(permission == EM_LegionPermission.Deputy)
                {
UITool.Ui_Trip("Không đủ quyền hạn");
                }
            }
            return false;
        }
    }

    /// <summary>
    /// 根据阵营类型设置图标
    /// </summary>
    /// <param name="campSprite"></param>
    /// <param name="camp"></param>
    public static void f_SetCampSpriteByCamp(UISprite campSprite, EM_CardCamp camp)
    {
        switch(camp)
        {
            case EM_CardCamp.eCardWei:
                campSprite.spriteName = "Icon_Wei";
                break;
            case EM_CardCamp.eCardShu:
                campSprite.spriteName = "Icon_Shu";
                break;
            case EM_CardCamp.eCardWu:
                campSprite.spriteName = "Icon_Wu";
                break;
            case EM_CardCamp.eCardGroupHero:
                campSprite.spriteName = "Icon_Qun";
                break;
            default:
                campSprite.spriteName = string.Empty;
                break;
        }
    }

    /// <summary>
    /// 计算购买副本挑战次数的消耗
    /// </summary>
    /// <param name="curTimes">单前次数</param>
    /// <param name="buyTimes">购买次数</param>
    /// <returns></returns>
    public static int f_GetBuyDungeonTimesSyceeCost(int curTimes, int buyTimes)
    {
        int result = 0;
        for(int i = 1; i <= buyTimes; i++)
        {
            result += (curTimes + i) * LegionConst.LEGION_DUNGEON_SYCEE_PRE_TIME;
        }
        result *= 5;
        return result;
    }

    /// <summary>
    /// 计算购买军团战挑战次数的消耗 (未确定)
    /// </summary>
    /// <param name="curTimes">单前次数</param>
    /// <param name="buyTimes">购买次数</param>
    /// <returns></returns>
    public static int f_GetBuyBattleTimesSyceeCost(int curTimes, int buyTimes)
    {
        int result = 0;
        for(int i = 1; i <= buyTimes; i++)
        {
            result += (curTimes + i) * LegionConst.LEGION_DUNGEON_SYCEE_PRE_TIME;
        }
        return result;
    }
}
