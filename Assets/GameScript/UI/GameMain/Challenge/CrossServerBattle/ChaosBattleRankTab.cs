using UnityEngine;
using System.Collections;

public class ChaosBattleRankTab : MonoBehaviour
{
    public UILabel m_Desc;
    public UILabel m_SelectDesc;
    public GameObject m_NormalBg;
    public GameObject m_SelectTip;

    private ChaosBattleZoneDT m_ZoneTemplate;

    public void f_Init(ChaosBattleZoneDT zoneTemplate)
    {
        m_ZoneTemplate = zoneTemplate;
        m_Desc.text = zoneTemplate.szName;
        m_SelectDesc.text = zoneTemplate.szName;
    }

    public void f_UpdateByInfo(int selectId)
    {
        m_SelectTip.SetActive(m_ZoneTemplate.iId == selectId);
    }
}
