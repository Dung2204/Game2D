using ccU3DEngine;
using System.Collections.Generic;
public class WeekFundPoolDT : BasePoolDT<long>
{
    public List<WeekFundAwardNode> m_AwardNodeList;
    private WeekFundDT m_WeekFundValue;
    public WeekFundDT WeekFundData
    {
        set
        {
            m_WeekFundValue = value;
            m_AwardNodeList = new List<WeekFundAwardNode>();
			m_AwardNodeList.Add(new WeekFundAwardNode() { m_Day = 1, m_DayName = "Day 1", m_SZAward = m_WeekFundValue.szAward1, m_ResourceCommonDTList = CommonTools.f_GetListCommonDT(m_WeekFundValue.szAward1)});
			m_AwardNodeList.Add(new WeekFundAwardNode() { m_Day = 2, m_DayName = "Day 2", m_SZAward = m_WeekFundValue.szAward2, m_ResourceCommonDTList = CommonTools.f_GetListCommonDT(m_WeekFundValue.szAward2)});
			m_AwardNodeList.Add(new WeekFundAwardNode() { m_Day = 3, m_DayName = "Day 3", m_SZAward = m_WeekFundValue.szAward3, m_ResourceCommonDTList = CommonTools.f_GetListCommonDT(m_WeekFundValue.szAward3)});
			m_AwardNodeList.Add(new WeekFundAwardNode() { m_Day = 4, m_DayName = "Day 4", m_SZAward = m_WeekFundValue.szAward4, m_ResourceCommonDTList = CommonTools.f_GetListCommonDT(m_WeekFundValue.szAward4)});
			m_AwardNodeList.Add(new WeekFundAwardNode() { m_Day = 5, m_DayName = "Day 5", m_SZAward = m_WeekFundValue.szAward5, m_ResourceCommonDTList = CommonTools.f_GetListCommonDT(m_WeekFundValue.szAward5)});
			m_AwardNodeList.Add(new WeekFundAwardNode() { m_Day = 6, m_DayName = "Day 6", m_SZAward = m_WeekFundValue.szAward6, m_ResourceCommonDTList = CommonTools.f_GetListCommonDT(m_WeekFundValue.szAward6)});
			m_AwardNodeList.Add(new WeekFundAwardNode() { m_Day = 7, m_DayName = "Day 7", m_SZAward = m_WeekFundValue.szAward7, m_ResourceCommonDTList = CommonTools.f_GetListCommonDT(m_WeekFundValue.szAward7)});

        }
        get
        {
            return m_WeekFundValue;
        }
    }
}

public class WeekFundAwardNode
{
    public byte m_Day;
    public string m_DayName;
    public string m_SZAward;
    public List<ResourceCommonDT> m_ResourceCommonDTList;         
}

