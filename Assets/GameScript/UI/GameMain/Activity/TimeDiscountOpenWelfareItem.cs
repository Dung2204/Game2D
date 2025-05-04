using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 限时折扣全民福利item
/// </summary>
public class TimeDiscountOpenWelfareItem : MonoBehaviour {
    public UILabel m_LabelName;//名字
    public UILabel m_LabelCondi;//条件
    public UI2DSprite m_SprIcon;//icon
    public UISprite m_SprBorder;//边框
    public UILabel m_LabelCount;//数量
    public GameObject m_ObjHasGet;//已经领取
    public GameObject m_ObjGet;//领取
    public GameObject m_ObjWaitGet;//等待领取
    /// <summary>
    /// 设置参数
    /// </summary>
    /// <param name="Name">道具名字</param>
    /// <param name="Condi">条件</param>
    /// <param name="icon">图标</param>
    /// <param name="borderName">边框</param>
    /// <param name="Count">数量</param>
    public void SetData(string Name,int Condi,EM_ResourceType resourceType, int resourceId,string borderName,int Count)
    {
        m_LabelName.text = Name;
m_LabelCondi.text = "The number of purchases that " + Condi + " can be received";
        UITool.f_SetIconSprite(m_SprIcon,resourceType,resourceId);
        m_SprBorder.spriteName = borderName;
        m_LabelCount.text = Count.ToString();
    }
}
