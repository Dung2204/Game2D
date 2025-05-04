using UnityEngine;
using System.Collections;
/// <summary>
/// 红包排行榜item
/// </summary>
public class LegionRedRankItem : MonoBehaviour
{
    public UI2DSprite m_SprIcon;//玩家头像
    public UISprite m_Frame; //玩家品质框
    public UILabel m_LabelVip;//vip等级
    public UILabel m_LabelRank;//排名
    public UILabel m_LabelPlayerName;//玩家名字
    public UILabel m_LabelLevel;//玩家等级
    public UILabel m_LabelSendCount;//发放数量或抢到元宝数量
    public UILabel m_LabelLegionName;//军团名字
    public UISprite m_RankSprite;
    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="rank">排名</param>
    /// <param name="playerName">玩家名字</param>
    /// <param name="playerLevel">玩家等级</param>
    /// <param name="sendCount">发送（抢到）元宝数量</param>
    /// <param name="legionName">军团名字</param>
    public void SetData(int icon, int frameId, int vipLevel, int rank, string playerName, int playerLevel, int sendCount, string legionName, int FashID = 0)
    {
        string tName = playerName;
        m_Frame.spriteName = UITool.f_GetImporentColorName(frameId, ref tName);
        m_LabelPlayerName.text = tName;
		if(FashID != 0)
		{
			m_SprIcon.sprite2D = UITool.f_GetIconSprite(FashID);
		}
		else
		{
			m_SprIcon.sprite2D = UITool.f_GetIconSpriteByCardId(icon);
		}
        m_LabelVip.text = vipLevel.ToString();
        string RankSprite = "[FEFEE6]";//设置排名颜
        switch (rank)
        {
            case 1: RankSprite = "Border_huz4"; break;
            case 2: RankSprite = "Border_huz5"; break;
            case 3: RankSprite = "Border_huz6"; break;
            default: RankSprite = ""; break;
        }
m_LabelRank.text = "Rank" + rank + "";
        m_LabelRank.gameObject.SetActive(RankSprite == "");
        m_RankSprite.gameObject.SetActive(RankSprite != "");
m_LabelLevel.text = "Level" + playerLevel + "";
        m_LabelSendCount.text = sendCount.ToString();
m_LabelLegionName.text = "[F1B049FF]Legion：[-][bdb19b] " + legionName;

        m_RankSprite.spriteName = RankSprite;
    }
}
