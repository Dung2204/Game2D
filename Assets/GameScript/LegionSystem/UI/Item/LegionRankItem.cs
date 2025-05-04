using UnityEngine;
using System.Collections;
/// <summary>
/// 军团排行榜item
/// </summary>
public class LegionRankItem : MonoBehaviour
{
    public UI2DSprite m_SprLegionIcon;//军团图标
    public UILabel m_LabelRank;//排名
    public UILabel m_LabelLegionName;//军团名字
    public UILabel m_LabelLevel;//军团等级
    public UILabel m_LabelLegionLeader;//军团长
    public UILabel m_LabelMember;//成员
    public UILabel m_DungeonProgress;//副本进度
    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="rank">排名</param>
    /// <param name="LegionName">军团名字</param>
    /// <param name="legionLevel">军团等级</param>
    /// <param name="leader">军团长</param>
    public void SetData(int legionIcon,int rank, string LegionName, int legionLevel, string leader, int curMemberCount, int maxMemberCount, string dungeonProgress)
    {
        m_SprLegionIcon.sprite2D = UITool.f_GetIconSprite(legionIcon);
        m_LabelRank.text = rank.ToString();
        m_LabelLegionName.text = LegionName;
m_LabelLevel.text = "Level" + legionLevel;
        m_LabelLegionLeader.text = leader;
m_LabelMember.text = "Member: " + curMemberCount + "/" + maxMemberCount;
        if (m_DungeonProgress != null)
            m_DungeonProgress.text = dungeonProgress;
    }
}
