using UnityEngine;
using System.Collections;

public class PatrolTypeTimeItem : MonoBehaviour
{
    public GameObject m_SelectIcon;
    public UILabel m_Desc;
    public GameObject ClickItem;

    public PatrolTypeDT info
    {
        private set;
        get;
    }

    public void f_UpdateInfo(int curTime,PatrolTypeDT info)
    {
        this.info = info;
m_Desc.text = string.Format("Patrol {0} hours", info.iTime);
        m_SelectIcon.SetActive(curTime == info.iTime);
    }
}
