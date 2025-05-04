using UnityEngine;
using System.Collections;

public class PatrolVisitFriendItem : MonoBehaviour
{
    public UI2DSprite m_Icon;
    public UISprite m_IconBorder;
    public UILabel m_NameLabel;
    public UILabel m_LevelLabel;
    public UILabel m_TimeLabel;
    public GameObject m_BtnVisit;

    public UILabel m_UnlockNum;
    public UILabel m_PatrolingNum;
    public UILabel m_RiotingNum;
    public GameObject m_RiotingTip;

    public void f_UpdateByInfo(PatrolFriendInfoPoolDT info)
    {
        m_Icon.sprite2D = UITool.f_GetIconSpriteBySexId(info.m_PlayerInfo.m_iSex);

        string tName = info.m_PlayerInfo.m_szName;
        int iFrame = info.m_PlayerInfo.m_iFrameId;
        if (info.m_PlayerInfo.m_iFrameId <= 0)
        {
            iFrame = (int)EM_Important.White;
        }
        m_IconBorder.spriteName = UITool.f_GetImporentColorName(iFrame, ref tName);
        m_NameLabel.text = tName;
        m_LevelLabel.text = info.m_PlayerInfo.m_iLv.ToString();
        m_TimeLabel.text = UITool.f_GetTimeDescFromNow(info.m_PlayerInfo.m_iOfflineTime);

        m_UnlockNum.text = string.Format(CommonTools.f_GetTransLanguage(948), info.m_iUnlockNum);
        m_PatrolingNum.text = string.Format(CommonTools.f_GetTransLanguage(949), info.m_iPatrolingNum);
        m_RiotingNum.text = string.Format(CommonTools.f_GetTransLanguage(950), info.m_iRiotingNum);
        m_RiotingTip.SetActive(info.m_iRiotingNum > 0);
    }
}
