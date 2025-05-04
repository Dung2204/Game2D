using UnityEngine;
using System.Collections;

public class LegionSacrificeTypeItem : MonoBehaviour
{
    public UISprite m_SacrificeName;
    public UILabel m_SacrificeProgress;
    public UILabel m_LegionExp;
    public UILabel m_LegionContri;
    public UISprite m_CostIcon;
    public UILabel m_CostNum;
    public GameObject m_AlreadyTip;
    public GameObject m_SelectTip;

    public GameObject m_SelectBtn;

    public void f_UpdatebyInfo(int alreadyType, int curType, LegionSacrificeDT info)
    {
        m_SacrificeName.spriteName = "ji" + info.iId;
        m_SacrificeProgress.text = string.Format("{0}", info.iSacrificeNum);
        m_LegionExp.text = string.Format("{0}", info.iSacrificeExpNum);
        m_LegionContri.text = string.Format("{0}", info.iSacrificeContributeNum);
        m_CostIcon.spriteName = UITool.f_GetMoneySpriteName((EM_MoneyType)info.iCostType);
        m_CostNum.text = string.Format("{0}", info.iCostCount);
        m_CostNum.gameObject.SetActive(alreadyType != info.iId);
        m_AlreadyTip.SetActive(alreadyType == info.iId);
        m_SelectTip.SetActive(alreadyType != info.iId && curType == info.iId);
        transform.localScale = (alreadyType != info.iId && curType == info.iId) ? Vector3.one : Vector3.one * 0.9f;

    }
}
