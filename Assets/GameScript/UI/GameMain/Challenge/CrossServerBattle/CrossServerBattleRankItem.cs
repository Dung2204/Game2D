using UnityEngine;
using System.Collections;

public class CrossServerBattleRankItem : MonoBehaviour
{
    public static string[] TipName = new string[] {
        "",
        //"UI/UITexture/RankList/Tex_RankListTip1",
        //"UI/UITexture/RankList/Tex_RankListTip2",
        //"UI/UITexture/RankList/Tex_RankListTip3"
        "Icon_rank1",
        "Icon_rank2",
        "Icon_rank3",
    };

    public UISprite mRankTex;
    public UISprite mBg;
    public UILabel mRank;
    public UILabel mTitle;
    public UILabel mServerId;
    public UILabel mPlayerName;
    public UILabel mPlayerPower;

    public void f_UpdateByInfo(bool isSelf, CrossServerBattleRankPoolDT info)
    {
        int ret = info.Rank % 2;
        if (!isSelf)
            mBg.spriteName = ret != 0 ? "Border_RebelTimeBg" : "Border_wjdk";
        string desc = isSelf ? "[cfe3ff]{0}" : "[ffffff]{0}";
        string nameDesc = info.Rank == 1 ? "[D5AF56]{0}" :
                          info.Rank == 2 ? "[569BD5]{0}" :
                          info.Rank == 3 ? "[D55F56]{0}" :
                          isSelf ? "[F1C049]{0}" :
                          "[BAB49E]{0}";

        mRank.text = info.Rank == 0 ? string.Format(desc, CommonTools.f_GetTransLanguage(1037)) : string.Format(desc, info.Rank);
        mTitle.text = string.Format(desc, info.TitleName);
        mServerId.text = string.Format(desc, info.ServerIdName);
        mPlayerName.text = string.Format(nameDesc, info.PlayerName);
        mPlayerPower.text = string.Format(desc, UITool.f_CountToChineseStr(info.PlayerPower));
        mRankTex.gameObject.SetActive(info.Rank > 0 && info.Rank <= 3);
        if (info.Rank > 0 && info.Rank <= 3)
        {
            mRankTex.spriteName = TipName[info.Rank];
            //mRankTex.mainTexture = Resources.Load<Texture>(TipName[info.Rank]);
        }
    }
}
