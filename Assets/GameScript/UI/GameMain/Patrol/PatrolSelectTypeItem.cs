using UnityEngine;
using System.Collections;

public class PatrolSelectTypeItem : MonoBehaviour
{
    public UILabel m_Title;
    public UILabel m_Content;
    public GameObject m_Bg;
    
    public void f_UpdateByInfo(PatrolTypeDT info)
    {
        int vipLv = UITool.f_GetNowVipLv();
        int needVipLv = info.iNeedVip;
        m_Title.text = info.szDes;
        if (info.iNeedVip > 0)
        {
            m_Content.text = vipLv < needVipLv ? string.Format(CommonTools.f_GetTransLanguage(937), info.iEventCD, info.iNeedVip)
                                : string.Format(CommonTools.f_GetTransLanguage(938), info.iEventCD, info.iNeedVip);
        }
        else
        {
            m_Content.text = string.Format(CommonTools.f_GetTransLanguage(939), info.iEventCD);
        }                                
    }
}
