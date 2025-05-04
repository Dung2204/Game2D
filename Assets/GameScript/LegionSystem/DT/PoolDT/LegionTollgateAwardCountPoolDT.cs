using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class LegionTollgateAwardCountPoolDT : BasePoolDT<long>
{
    public LegionTollgateAwardCountPoolDT(int chap_Camp,int awardIdx,LegionTollgateBoxAward awardTemplate)
    {
        m_iChap_Camp = chap_Camp;
        m_iAwardIdx = awardIdx;
        f_AlreadyGetCountInit();
        m_cAwardTemplate = awardTemplate;
    }

    public int m_iChap_Camp
    {
        private set;
        get;
    }

    /// <summary>
    /// 等于数组下标+1
    /// </summary>
    public int m_iAwardIdx
    {
        get;
        private set;
    }

    public int m_iTotalCount
    {
        get
        {
            return m_cAwardTemplate.m_iGenCount;
        }
    }

    public int m_iAlreadyGetCount
    {
        get;
        private set;
    }

    public LegionTollgateBoxAward m_cAwardTemplate
    {
        get;
        private set;
    }

    public void f_AlreadyGetCountAdd()
    {
        m_iAlreadyGetCount++;
    }

    public void f_AlreadyGetCountInit()
    {
        m_iAlreadyGetCount = 0;
    }

}
