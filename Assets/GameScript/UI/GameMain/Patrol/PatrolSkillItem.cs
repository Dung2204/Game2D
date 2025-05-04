using UnityEngine;
using System.Collections;

public class PatrolSkillItem : MonoBehaviour
{
    public UILabel m_LandName;
    public UILabel m_LandLv;
    public UISprite m_LandIcon;
    public UILabel m_SkillDesc;
    public UILabel m_StateDesc;
    public UILabel m_LvUpCost;
    public UILabel m_LandTime;

    public GameObject m_BtnLvUp;
    public GameObject m_LockTip;
    public GameObject m_LvMaxTip;

    public void f_UpdateByInfo(PatrolSkillNode info)
    {
        m_LandName.text = info.m_LandTemplate.szName;
        m_LandLv.text = string.Format(CommonTools.f_GetTransLanguage(909), info.m_iLv);
        m_LandIcon.spriteName = string.Format("Icon_Land{0:d2}", info.m_iLandId);
        m_LandIcon.MakePixelPerfect();
        //m_LandIcon.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        m_SkillDesc.text = info.m_Template.szDesc;
        m_LandTime.text = string.Format(CommonTools.f_GetTransLanguage(910), info.m_iTotalTime);
        UITool.f_SetSpriteGray(m_LandIcon, info.m_bIsLock);
        m_BtnLvUp.SetActive(!info.m_bIsLock && info.m_iLv < info.m_iLvMax);
        m_LockTip.SetActive(info.m_bIsLock);
        m_LvMaxTip.SetActive(info.m_iLv >= info.m_iLvMax);
        if (info.m_bIsLock)
        {
            m_StateDesc.text = CommonTools.f_GetTransLanguage(911);
            m_LandLv.text = CommonTools.f_GetTransLanguage(912);
        }
        else if (info.m_iLv >= info.m_iLvMax)
        {
            m_StateDesc.text = string.Empty;
        }
        else
        {
            m_StateDesc.text = string.Format(CommonTools.f_GetTransLanguage(913), info.m_LvUpTemplate.iNeedTime);
            m_LvUpCost.text = info.m_LvUpTemplate.iCostSycee.ToString();
        }
    }
}
