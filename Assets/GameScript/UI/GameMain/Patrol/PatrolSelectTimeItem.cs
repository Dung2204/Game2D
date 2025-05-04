using UnityEngine;
using System.Collections;

public class PatrolSelectTimeItem : MonoBehaviour
{
    public UILabel m_TimeLabel;
    public UISprite m_TipBg;
    public GameObject m_ClickItem;

    public void f_UpdateByInfo(PatrolTypeDT info)
    {
        m_TimeLabel.text = string.Format(CommonTools.f_GetTransLanguage(932), info.iTime);
        m_TipBg.spriteName = string.Format("Border_Time{0}", info.iTime);
    }
}
