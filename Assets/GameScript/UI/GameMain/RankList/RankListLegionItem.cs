using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class RankListLegionItem : MonoBehaviour
{
    public static string[] TipName = new string[] {
        "",
        "UI/UITexture/RankList/Tex_RankListTip1",
        "UI/UITexture/RankList/Tex_RankListTip2",
        "UI/UITexture/RankList/Tex_RankListTip3"
    };
    public UILabel mRankLabel;
    public UI2DSprite mLegionIcon;
    public UILabel mLegionName;
    public UILabel mLegionLv;
    public UILabel mLegionPower;
    public UITexture mTexTitleTip;

    public UISprite mRankLabelThere;
    public GameObject mLeftBg;
    public GameObject mLeftThereBg;


    public void f_UpdateByInfo(BasePoolDT<long> info)
    {
        RankListLegionPoolDT tData = (RankListLegionPoolDT)info;
        //mRankLabel.text = tData.Rank.ToString();
        mLeftBg.SetActive(tData.Rank > 3);
        mLeftThereBg.SetActive(tData.Rank <= 3);
        mRankLabel.gameObject.SetActive(tData.Rank > 3);
        mRankLabelThere.gameObject.SetActive(tData.Rank <= 3);
        if(tData.Rank > 3)
        {
            mRankLabel.text = tData.Rank.ToString();
        }
        else
        {
            mRankLabelThere.spriteName = "Icon_RankListTip" + tData.Rank;
            mRankLabelThere.MakePixelPerfect();
        }

        mLegionIcon.sprite2D = UITool.f_GetIconSprite(tData.LegionInfo.f_GetProperty((int)EM_LegionProperty.Icon));
        mLegionName.text = tData.LegionInfo.LegionName;
        mLegionLv.text = string.Format("LV. {0}", tData.LegionInfo.f_GetProperty((int)EM_LegionProperty.Lv));
        mLegionPower.text = string.Format(CommonTools.f_GetTransLanguage(1226), UITool.f_CountToChineseStr(tData.LegionPower));
        mTexTitleTip.gameObject.SetActive(false);
        mRankLabel.gameObject.SetActive(tData.Rank > 3);
        //if(tData.Rank > 0 && tData.Rank <= 3)
        //{
        //    mTexTitleTip.mainTexture = Resources.Load<Texture>(TipName[tData.Rank]);
        //}
    }
}
