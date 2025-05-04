using UnityEngine;

public class CardBattleTeamItem : MonoBehaviour
{
    public UI2DSprite mIcon;
    public UISprite mFrame;
    public GameObject mInClothTip;

    public GameObject DragParent;
    public GameObject RoleParent;
    public GameObject role;
    
    public void f_UpdateByInfo(CardBattleTeamPoolDT info,bool inCloth)
    {
        mIcon.sprite2D = UITool.f_GetIconSpriteByCardId(info.CardTemplateId);
        mFrame.spriteName = UITool.f_GetImporentCase(info.CardTemplate.iImportant);
        mInClothTip.SetActive(inCloth);
    }
}
