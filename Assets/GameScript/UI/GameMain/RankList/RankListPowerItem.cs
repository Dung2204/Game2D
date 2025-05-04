using ccU3DEngine;
using UnityEngine;

public class RankListPowerItem : MonoBehaviour
{
    public static string[] TipName = new string[] {
        "",
        "UI/UITexture/RankList/Tex_RankListTip1",
        "UI/UITexture/RankList/Tex_RankListTip2",
        "UI/UITexture/RankList/Tex_RankListTip3"
    };


    public UILabel mRankLabel;
    public UI2DSprite mIcon;
    public UISprite mIconFrame;
    public UILabel mPlayerName;
    public UILabel mPowerLabel;
    public UILabel mLegionLabel;
    public UITexture mTexTitleTip;

    public GameObject mPraiseBtn;
    public GameObject mAlreadyPraiseTip;
    public UILabel mPraiseTimes;
    
    public UISprite mRankLabelThere;
    public GameObject mBorderLeftThere;
    public GameObject mBorderLeft;


    public void f_UpdateByInfo(BasePoolDT<long> info)
    {
        RankListPoolDT tData = (RankListPoolDT)info;
        //mRankLabel.text = tData.Rank.ToString();

        mBorderLeftThere.SetActive(tData.Rank <= 3);
        mBorderLeft.SetActive(tData.Rank > 3);
        mRankLabelThere.gameObject.SetActive(tData.Rank <= 3);
        mRankLabel.gameObject.SetActive(tData.Rank > 3);
        if(tData.Rank <= 3)
        {
            mRankLabelThere.spriteName = "Icon_RankListTip" + tData.Rank;
            mRankLabelThere.MakePixelPerfect();
        }
        else
        {
            mRankLabel.text = tData.Rank.ToString();
        }

        mIcon.sprite2D = UITool.f_GetIconSpriteBySexId(tData.PlayerInfo.m_iSex);
        string tName = tData.PlayerInfo.m_szName;
        mIconFrame.spriteName = UITool.f_GetImporentColorName(tData.PlayerInfo.m_iFrameId, ref tName);
        mPlayerName.text = tName;
        mPowerLabel.text = string.Format(CommonTools.f_GetTransLanguage(1224), UITool.f_CountToChineseStr(tData.PlayerInfo.m_iBattlePower));
        mLegionLabel.text = tData.PlayerInfo.m_szLegion;

        mPraiseBtn.SetActive(!tData.AlreadyPraise);
        mAlreadyPraiseTip.SetActive(tData.AlreadyPraise);
        mPraiseTimes.text = string.Format(CommonTools.f_GetTransLanguage(1225), tData.PraiseTimes);
        mTexTitleTip.gameObject.SetActive(false);
        mRankLabel.gameObject.SetActive(tData.Rank > 3);
        //if(tData.Rank > 0 && tData.Rank <= 3)
        //{
        //    mTexTitleTip.mainTexture = Resources.Load<Texture>(TipName[tData.Rank]);
        //}
    }
}
