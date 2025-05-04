using ccU3DEngine;
using System.Collections;

public class LegionTollgateAwardPoolDT : BasePoolDT<long>
{
    public LegionTollgateAwardPoolDT(int chap_Camp,int idx)
    {
        m_iChap_Camp = chap_Camp;
        m_iIdx = idx;
        f_ResetData();
    }

    public void f_UpdateData(long getPlayer, int awardIdx,LegionTollgateBoxAward awardTemplate)
    {
        m_iGetPlayer = getPlayer;
        m_iAwardIdx = awardIdx;
        m_cAwardTemplate = awardTemplate;
    }

    /// <summary>
    /// 重置数据
    /// </summary>
    public void f_ResetData()
    {
        m_iGetPlayer = 0;
        m_iAwardIdx = 0;
        m_cAwardTemplate = null;
    }

    public int m_iChap_Camp
    {
        private set;
        get;
    }

    /// <summary>
    /// 第几个宝箱 
    /// </summary>
    public int m_iIdx
    {
        private set;
        get;
    }

    /// <summary>
    /// 已经领取的玩家Idx
    /// </summary>
    public long m_iGetPlayer
    {
        get;
        private set;
    }


    /// <summary>
    /// 对应关卡宝箱表的Idx
    /// </summary>
    public int m_iAwardIdx
    {
        get;
        private set;
    }

    public LegionTollgateBoxAward m_cAwardTemplate
    {
        private set;
        get;
    }
}
