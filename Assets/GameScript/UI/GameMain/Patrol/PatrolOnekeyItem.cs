using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PatrolOnekeyItem : MonoBehaviour
{
    public UILabel m_LandName; 
    public UI2DSprite m_CardIcon;
    public UISprite m_CardFrame;
    public UILabel m_TipDesc;
    public UILabel m_TimeLabel;
    public UILabel m_TypeLabel;
    public UISprite m_LandIcon;

    public GameObject m_BtnSelect;
    public GameObject m_BtnDown;
    public GameObject m_BtnSelectTime;
    public GameObject m_BtnSelectType;
    
private const string NullCardTip = "Nhấn “+” để thêm";
private const string CardTip = "Tuần tra chung：{0}";
    private PatrolOnekeyPage m_patrolOnekeyPage;


    private List<string> selectpopuptype = new List<string>();
    private List<string> selectpopuptime = new List<string>();
    private UIPopupList mPopupListTime;
    private UIPopupList mPopupListType;
    private PatrolOnekeyData patrolOnekeyData;

    public void f_UpdateByInfo(PatrolOnekeyData info, PatrolOnekeyPage patrolOnekeyPage)
    {
        patrolOnekeyData = info;
        m_LandName.text = info.m_PatrolLandDT.szName;
        m_CardIcon.gameObject.SetActive(info.m_iCardId != 0);
        m_LandIcon.spriteName = "Icon_Land0" + info.m_PatrolLandDT.iId;
        if (info.m_iCardId != 0)
        {
            m_CardIcon.sprite2D = UITool.f_GetIconSpriteByCardId(info.m_iCardId);
            string tName = info.m_CardDT.szName;
            m_CardFrame.spriteName = UITool.f_GetImporentColorName(info.m_CardDT.iImportant,ref tName);
            m_TipDesc.text = string.Format(CardTip, tName);
        }
        else
        {
            m_TipDesc.text = NullCardTip;
        }

        m_TypeLabel.text = info.m_PatrolTypeDT.szDes;
        m_patrolOnekeyPage = patrolOnekeyPage;


        mPopupListType = m_BtnSelectType.transform.GetComponent<UIPopupList>();
        EventDelegate.Add(mPopupListType.onChange, f_BtnSelectType);
        selectpopuptype.Clear();
selectpopuptype.Add("Tuần Tra - [0DC623]Thường[-]");
selectpopuptype.Add("Tuần Tra - [C927DC]Trung[-]");
selectpopuptype.Add("Tuần Tra - [DC3827]Cao[-]");
        mPopupListType.items = selectpopuptype;


        mPopupListTime = m_BtnSelectTime.transform.GetComponent<UIPopupList>();
m_TimeLabel.text = string.Format("Tuần Tra {0} Giờ", info.m_PatrolTypeDT.iTime);
        selectpopuptime.Clear();
        List<NBaseSCDT> patrolTypeList = Data_Pool.m_PatrolPool.f_GetPatrolTypeByType(info.m_iPatrolType);
        for (int i = 0; i < patrolTypeList.Count; i++)
        {
            PatrolTypeDT patrolTypeDT = (PatrolTypeDT)patrolTypeList[i];
            selectpopuptime.Add(patrolTypeDT.iTime.ToString());
        }
        mPopupListTime.items = selectpopuptime;
        EventDelegate.Add(mPopupListTime.onChange, f_BtnSelectTime);

    }

    private void f_BtnSelectType()
    {
        int idx = selectpopuptype.IndexOf(UIPopupList.current.value);
        m_patrolOnekeyPage.f_BtnSelectType(idx + 1, patrolOnekeyData);
    }

    private void f_BtnSelectTime()
    {
        m_patrolOnekeyPage.f_BtnSelectTime(int.Parse(UIPopupList.current.value), patrolOnekeyData);
m_TimeLabel.text = string.Format("Tuần Tra {0} Giờ", UIPopupList.current.value);
    }
}
