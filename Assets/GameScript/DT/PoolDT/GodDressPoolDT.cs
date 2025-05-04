using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;
public class GodDressPoolDT : BasePoolDT<long>
{
    public short m_TodayCurCount;//当前次数
    public short m_CurAccumIntegral;//累计积分
    public int m_MyRank;//我的排名
    public int m_Integral;//我的积分
    public List<BasePoolDT<long>> m_GodRankData;
    private GodDressDT m_GodDataValue;//活动表数据
    public GodDressDT m_GodDressDT
    {
        set
        {
            m_GodDataValue = value;
            GodDressRankAwardDT data = (GodDressRankAwardDT)glo_Main.GetInstance().m_SC_Pool.m_GodDressRankAwardSC.f_GetSC(m_GodDataValue.iRankAwardID);
            if (data==null) {
MessageBox.ASSERT("Bonus data table is empty");
                return;
            }
            m_GodRankData = new List<BasePoolDT<long>>();
            m_GodRankData.Add(new GodRankData() { m_BeginRank = data.iRankBeg1, m_End_Rank = data.iRankEnd1, m_ResourceCommonDTList = CommonTools.f_GetListCommonDT(data.szAward1) });
            m_GodRankData.Add(new GodRankData() { m_BeginRank = data.iRankBeg2, m_End_Rank = data.iRankEnd2, m_ResourceCommonDTList = CommonTools.f_GetListCommonDT(data.szAward2) });
            m_GodRankData.Add(new GodRankData() { m_BeginRank = data.iRankBeg3, m_End_Rank = data.iRankEnd3, m_ResourceCommonDTList = CommonTools.f_GetListCommonDT(data.szAward3) });
            m_GodRankData.Add(new GodRankData() { m_BeginRank = data.iRankBeg4, m_End_Rank = data.iRankEnd4, m_ResourceCommonDTList = CommonTools.f_GetListCommonDT(data.szAward4) });
            m_GodRankData.Add(new GodRankData() { m_BeginRank = data.iRankBeg5, m_End_Rank = data.iRankEnd5, m_ResourceCommonDTList = CommonTools.f_GetListCommonDT(data.szAward5) });
            m_GodRankData.Add(new GodRankData() { m_BeginRank = data.iRankBeg6, m_End_Rank = data.iRankEnd6, m_ResourceCommonDTList = CommonTools.f_GetListCommonDT(data.szAward6) });
            m_GodRankData.Add(new GodRankData() { m_BeginRank = data.iRankBeg7, m_End_Rank = data.iRankEnd7, m_ResourceCommonDTList = CommonTools.f_GetListCommonDT(data.szAward7) });
            m_GodRankData.Add(new GodRankData() { m_BeginRank = data.iRankBeg8, m_End_Rank = data.iRankEnd8, m_ResourceCommonDTList = CommonTools.f_GetListCommonDT(data.szAward8) });
            m_GodRankData.Add(new GodRankData() { m_BeginRank = data.iRankBeg9, m_End_Rank = data.iRankEnd9, m_ResourceCommonDTList = CommonTools.f_GetListCommonDT(data.szAward9) });
            m_GodRankData.Add(new GodRankData() { m_BeginRank = data.iRankBeg10, m_End_Rank = data.iRankEnd10, m_ResourceCommonDTList = CommonTools.f_GetListCommonDT(data.szAward10) });
        }
        get
        {
            return m_GodDataValue;
        }
    }
}

//排名奖励
public class GodRankData: BasePoolDT<long>
{
    public int m_BeginRank;
    public int m_End_Rank;
    public List<ResourceCommonDT> m_ResourceCommonDTList;
}
