using UnityEngine;
using System.Collections;

public class PatrolSelectCardItem : MonoBehaviour
{
    public UI2DSprite m_CardIcon;
    public UISprite m_CardFrame;
    public UILabel m_CardName;
    public UILabel m_CardFragmentNum;

    public GameObject m_BtnSelect;
    public GameObject m_PatrolingTip;

    public void f_UpdateByInfo(CardDT cardDt,bool isPatroling)
    {
        m_CardIcon.sprite2D = UITool.f_GetIconSpriteByCardId(cardDt.iId); 
        string tName = cardDt.szName;
        m_CardFrame.spriteName = UITool.f_GetImporentColorName(cardDt.iImportant, ref tName);
        m_CardName.text = tName;
m_CardFragmentNum.text = string.Format("[F5C14C]Hiện có：[-]{0}", Data_Pool.m_CardFragmentPool.f_GetHaveNumByTemplate(cardDt.iId));
        m_BtnSelect.SetActive(!isPatroling);
        m_PatrolingTip.SetActive(isPatroling);
    }

}
