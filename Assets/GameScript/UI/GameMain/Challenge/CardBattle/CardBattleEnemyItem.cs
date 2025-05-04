using UnityEngine;
using System.Collections;

public class CardBattleEnemyItem : MonoBehaviour
{
    public UILabel mServerName;
    public UILabel mPlayerName;
    public GameObject[] mCardItems;
    public UIGrid mGrid;
    public GameObject mBtnChallenge;

    public void f_UpdateByInfo(CardBattleEnemyPoolDT info)
    {
        mServerName.text = info.ServerName;
        mPlayerName.text = info.PlayerName;
        for (int i = 0; i < info.ClothPoolDtList.Count; i++)
        {
            if (i >= mCardItems.Length)
                break;
            if (info.ClothPoolDtList[i].CardTemplateId == 0 || info.ClothPoolDtList[i].CardTemplate == null)
            {
                mCardItems[i].SetActive(false);
                continue;
            }
            mCardItems[i].SetActive(true);
            UI2DSprite icon = mCardItems[i].transform.Find("Icon").GetComponent<UI2DSprite>();
            UISprite frame = mCardItems[i].transform.Find("Frame").GetComponent<UISprite>();
            icon.sprite2D = UITool.f_GetIconSpriteByCardId(info.ClothPoolDtList[i].CardTemplateId);
            frame.spriteName = UITool.f_GetImporentCase(info.ClothPoolDtList[i].CardTemplate.iImportant);
        }
        mGrid.repositionNow = true;
        mGrid.Reposition();
    }
}
