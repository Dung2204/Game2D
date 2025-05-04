using UnityEngine;
using System.Collections;

public class RunningManRankItem : MonoBehaviour
{
    private readonly string[] RankIconName = new string[4] {"null",
                                                            "Icon_RankOne",
                                                            "Icon_RankTwo",
                                                            "Icon_RankThree"};
    private readonly string[] RankBGName = new string[2] { "Border_RankBG1", "Border_RankBG2"};

    public UISprite mBg;
    public UISprite mRankTip;
    public UILabel mRankLabel;
    public UILabel mNameLabel;
    public UILabel mStarLabel;
    public GameObject mObjRankTop;

    public void f_UpdateByInfo(ccU3DEngine.BasePoolDT<long> info)
    {
        RunningManRankPoolDT poolDt = (RunningManRankPoolDT)info;
        mNameLabel.text = poolDt.m_PlayerInfo.m_szName;
        mStarLabel.text = string.Format(CommonTools.f_GetTransLanguage(839), poolDt.m_iStarNum);
        if (poolDt.m_iRank <= 3)
        {
            mRankTip.spriteName = RankIconName[poolDt.m_iRank];
            mRankTip.MakePixelPerfect();
            mRankLabel.text = string.Empty;
            mObjRankTop.SetActive(true);
            mBg.spriteName = RankBGName[0];
        }
        else
        {
            mRankTip.spriteName = RankIconName[0];
            mRankLabel.text = poolDt.m_iRank.ToString();
            mObjRankTop.SetActive(false);
            mBg.spriteName = RankBGName[1];
        }
    }
}
