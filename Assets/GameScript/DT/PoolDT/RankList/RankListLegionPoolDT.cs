using ccU3DEngine;

public class RankListLegionPoolDT : BasePoolDT<long>
{
    private int m_Rank;
    /// <summary>
    /// 排名
    /// </summary>
    public int Rank
    {
        get
        {
            return m_Rank;
        }
    }

    private LegionInfoPoolDT m_LegionInfo;
    /// <summary>
    /// 军团信息
    /// </summary>
    public LegionInfoPoolDT LegionInfo
    {
        get
        {
            return m_LegionInfo;
        }
    }

    private long m_LegionPower;
    /// <summary>
    /// 军团总战斗力
    /// </summary>
    public long LegionPower
    {
        get
        {
            return m_LegionPower;
        }
    }

    private BasePlayerPoolDT mChiefInfo;
    /// <summary>
    /// 军团长信息（没有BasePlayerPoolDT完整信息，只有卡牌id,等级，战力，名称以及军团名！！！）
    /// </summary>
    public BasePlayerPoolDT m_mChiefInfo
    {
        get
        {
            return mChiefInfo;
        }
    }
  
    /// <summary>
    /// 更新军团长信息
    /// </summary>
    /// <param name="chiefSex"></param>
    /// <param name="chiefLv"></param>
    /// <param name="chiefPower"></param>
    /// <param name="chiefName"></param>
    public void f_UpdateChiefInfo(int chiefSex,int chiefLv,int chiefPower,string chiefName,long chiefId,byte uChiefFrameId,byte uChiefVipLv)
    {
        if (null == mChiefInfo)
        {
            mChiefInfo = new BasePlayerPoolDT();
        }
        string szLegionName = "";
        if (null != m_LegionInfo) szLegionName = m_LegionInfo.LegionName;
        mChiefInfo.f_SaveBase(chiefName, chiefSex, uChiefFrameId, uChiefVipLv, 0);
        mChiefInfo.f_SaveExtrend(szLegionName, chiefLv, chiefPower, 0);
        mChiefInfo.iId = chiefId;
    }

    public void f_UpdateRank(int rank)
    {
        m_Rank = rank;
    }

    public void f_UpdateByInfo(LegionInfoPoolDT legionInfo,long legionPower)
    {
        m_LegionInfo = legionInfo;
        m_LegionPower = legionPower;
    }

}
