using UnityEngine;
using System.Collections;
/// <summary>
/// 红包item
/// </summary>
public class RedPacketItem : MonoBehaviour {
    public UISprite m_PackIcon;//头像icon
    public UILabel m_PlayerName;//玩家名字
    public UILabel m_RedPacketName;//红包名字
    public UISlider m_SliderProgress;//进度
    public UILabel m_Labelrogress;//进度标签
    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="iconId">头像icon</param>
    /// <param name="userName">玩家姓名</param>
    /// <param name="packetName">红包名称</param>
    public void SetData(string strPackSprName,string userName,string packetName,int curCount,int totalCount,int hasSycee,int totalSycee)
    {
       // m_PackIcon.spriteName = strPackSprName;
        m_PlayerName.text = userName;
        m_RedPacketName.text = packetName+ "（" + curCount + "/"+ totalCount +"）";//100元宝（3/10）
        m_SliderProgress.value = hasSycee * 1.0f / totalSycee;
        m_Labelrogress.text = "("+ hasSycee + " / "+ totalSycee +")";
    }
}
