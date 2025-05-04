using UnityEngine;
using System.Collections;

public class PatrolItem : MonoBehaviour
{
    public UILabel m_NameLabel;
    public UISprite m_LandIcon;
    public GameObject m_RoitTip;
    public GameObject m_RoitTipAni;
    public GameObject m_FightTip;
    public GameObject m_AwardTip;
    public GameObject m_AddTip;
    public GameObject m_LockTip;
    public UILabel m_Lv;
    public UILabel m_TimeLabel;
    public GameObject m_ClickItem;

    private PatrolLandNode info;
    public PatrolLandNode m_Info
    {
        get
        {
            return info;
        }
    }

    public void f_UpdateByInfo(PatrolLandNode info)
    {
        this.info = info;
        m_NameLabel.text = UITool.f_ReplaceName(info.m_Template.szName, " ", "\n");
        m_RoitTip.SetActive(info.m_State == EM_PatrolState.Patroling && info.m_bIsRiot);
        //m_RoitTipAni.SetActive(info.m_State == EM_PatrolState.Patroling && info.m_bIsRiot);
        m_FightTip.SetActive(info.m_State == EM_PatrolState.CanAttack);
        m_AwardTip.SetActive(info.m_State == EM_PatrolState.GetAward);
        // m_AddTip.SetActive(info.m_State == EM_PatrolState.CanPatrol && info.m_iCardId == 0);
		m_AddTip.SetActive(info.m_State == EM_PatrolState.CanPatrol);
        m_LockTip.SetActive(info.m_State == EM_PatrolState.Lock);
        UITool.f_SetSpriteGray(m_LandIcon, info.m_State == EM_PatrolState.Lock);
        m_Lv.gameObject.SetActive(info.m_iLv > 0);
        m_Lv.text = info.m_iLv == 0? string.Empty:info.m_iLv.ToString();
        if (info.m_iEndTime > 0)
        {
            m_TimeLabel.gameObject.SetActive(true);
            f_UpdateTime(info.m_iEndTime - GameSocket.GetInstance().f_GetServerTime());
        } 
        else
            m_TimeLabel.gameObject.SetActive(false);
    }

    public void f_Disable()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 时间差 秒
    /// </summary>
    /// <param name="timeGap"></param>
    public void f_UpdateTime(int timeGap)
    {
        if (timeGap < 0)
        {
            m_TimeLabel.text = string.Empty;
            return;
        } 
        int hour = timeGap / 60 / 60;
        int minute = timeGap / 60 % 60;
        int second = timeGap % 60;
        m_TimeLabel.text = string.Format("{0:d2}:{1:d2}:{2:d2}", hour, minute, second);
    }
}
