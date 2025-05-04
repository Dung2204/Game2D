using UnityEngine;
using ccU3DEngine;
using System.Collections;

public class CrossTournamentKnockItem : MonoBehaviour
{
    public static CrossTournamentKnockItem f_Create(GameObject parent, GameObject item)
    {
        CrossTournamentKnockItem result = item.GetComponent<CrossTournamentKnockItem>();
        if (result == null)
            MessageBox.ASSERT("CrossTournamentKnockItem.f_Create in Item must contain CrossTournamentKnockItem");
        else
            result.f_Init(parent);
        return result;
    }

    [HideInInspector]
    public GameObject mParent;
    public GameObject mItem;
    public UI2DSprite HeadA;
    public UI2DSprite HeadB;
    public UILabel LbNameA;
    public UILabel LbNameB;
    public UILabel LbResult;
    public UISprite IconResult;

    private int timeLeft;
    private CrossTournamentThePoolDT m_Info;
    public void f_Init(GameObject parent)
    {
        mParent = parent;
        mItem.SetActive(false);
    }
    public void f_UpdateByInfo(CrossTournamentThePoolDT info,  GameObject parent)
    {
        mItem.SetActive(info != null);
        if (info == null)
            return;
        m_Info = info;
        mParent = parent;
        if(info.m_userA!= null)
        {
            string nameA = info.m_userA.m_szName + "[S" + info.m_userA.m_ServerName + "]";
            LbNameA.text = m_Info.m_Result == 1 ? nameA + "\n(Thắng)" : nameA;
            //HeadA.sprite2D = UITool.f_GetIconSpriteBySexId(info.m_userA.m_iSex);
            HeadA.sprite2D =  UITool.f_GetCardIcon(info.m_userA.Icon, "L1_");
        }
        else
        {
            LbNameA.text = "Đang Chờ";
            HeadA.sprite2D = UITool.f_GetCardIcon(999999, "L1_");
        }
        if (info.m_userB != null)
        {
            string nameB = info.m_userB.m_szName + "[S" + info.m_userB.m_ServerName + "]";
            LbNameB.text = m_Info.m_Result == 2 ? nameB + "\n(Thắng)": nameB;
            //HeadB.sprite2D = UITool.f_GetIconSpriteBySexId(info.m_userB.m_iSex);
            HeadB.sprite2D = UITool.f_GetCardIcon(info.m_userB.Icon, "L1_");
        }
        else
        {
            LbNameB.text = "Đang Chờ";
            HeadB.sprite2D = UITool.f_GetCardIcon(999999, "L1_");
        }
        
        if (info.m_Result == 0)
        {
            LbResult.text = "Chưa đấu";
            IconResult.spriteName = "fight_Icon";
        }
        else if(Data_Pool.m_CrossTournamentPool.m_Info.iType != 26) {
            timeLeft = (int)(info.m_uTime - GameSocket.GetInstance().f_GetServerTime());
            TimerControl(true);
        }
        else
        {
            LbResult.text = m_Info.m_Result == 0 ? "Chưa đấu" : "";
            IconResult.spriteName = "fight_Icon_1";
        }
        
    }
    private void TimerControl(bool isStart)
    {
        CancelInvoke("ReduceTime");
        if (isStart)
        {
            InvokeRepeating("ReduceTime", 0f, 1f);
        }
    }
    private void ReduceTime()
    {
        timeLeft--;
        if (timeLeft <= 0)
        {
            TimerControl(false);
            //LbResult.text = m_Info.m_Result == 0 ? "Chưa đấu" : (m_Info.m_Result == 1 ? m_Info.m_userA.m_szName : m_Info.m_userB.m_szName) + "(Thắng)";
            IconResult.spriteName = "fight_Icon_1";
        }
        else
        {
            LbResult.text = "Chờ KQ: " + CommonTools.f_GetStringBySecond(timeLeft);
        }
    }

}

