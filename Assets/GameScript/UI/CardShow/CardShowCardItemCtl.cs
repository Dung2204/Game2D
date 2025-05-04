using UnityEngine;
using System.Collections;
/// <summary>
/// 卡牌图鉴
/// </summary>
public class CardShowCardItemCtl : MonoBehaviour {
    public UI2DSprite m_spriteIcon;
    public UILabel m_labelName;
    public UILabel m_labelCampName;
    public GameObject m_btnIcon;
    public UISprite m_SprBoder;//边框
    public UISprite m_SprBGBoder;//边框
	public UISprite FightType;
    /// <summary>
    /// 设置卡片数据
    /// </summary>
    /// <param name="icon">卡片icon</param>
    /// <param name="cardName">卡片名字</param>
    public void SetData(Sprite icon,string borderSprName,string cardName,bool isHas, string campName, string borderBGSprName, EM_CardFightType CardFightType = EM_CardFightType.eCardPhyAtt)
    {
        m_spriteIcon.sprite2D = icon;
        m_SprBoder.spriteName = borderSprName;
        m_labelName.text = cardName;
        m_labelCampName.text = campName;
        m_SprBGBoder.spriteName = borderBGSprName;
        UITool.f_Set2DSpriteGray(m_spriteIcon, !isHas);
        UITool.f_SetSpriteGray(m_SprBoder, !isHas);
        UITool.f_SetSpriteGray(m_SprBGBoder, !isHas);
		//My Code
		string SpriteName = "Icon_Fight";
        switch (CardFightType)
        {
            case EM_CardFightType.eCardPhyAtt:
                SpriteName += "PhyAtt";
                break;
            case EM_CardFightType.eCardMagAtt:
                SpriteName += "MagAtt";
                break;
            case EM_CardFightType.eCardSup:
                SpriteName += "Sup";
                break;
            case EM_CardFightType.eCardTank:
                SpriteName += "Tank";
                break;
            case EM_CardFightType.eCardKiller:
                SpriteName += "Killer";
                break;
            case EM_CardFightType.eCardPhysician:
                SpriteName += "Physician";
                break;
            case EM_CardFightType.eCardLogistics:
                SpriteName += "Logistics";
                break;
        }
		FightType.spriteName = SpriteName;
		UITool.f_SetSpriteGray(FightType, !isHas);
		//
    }
}
