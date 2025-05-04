using UnityEngine;
using System.Collections;
/// <summary>
/// 折扣商品item
/// </summary>
public class DiscountItem : MonoBehaviour {
    public UI2DSprite m_icon;//图标
    public UISprite m_border;//边框
    public UILabel m_name;//名称
    public UILabel m_num;//出售物品数量
    public UILabel m_disCount;//折扣
    public UISprite m_OripriceType;//原价购买货币类型icon
    public UILabel m_oriPrice;//原价
    public UISprite m_NowPriceType;//现价购买货币类型icon
    public UILabel m_nowPrice;//现价
    public GameObject m_BtnBuy;//购买按钮
    public GameObject m_BtnHasBuy;//已经购买按钮
    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="icon">图标</param>
    /// <param name="borderSprName">边框名字</param>
    /// <param name="name">名称</param>
    /// <param name="num">数量</param>
    /// <param name="disCount">折扣0-100</param>
    /// <param name="oriPrice">原价</param>
    /// <param name="nowPrice">现价</param>
    /// <param name="moneyType">购买货币类型</param>
    /// <param name="buyTimes">购买次数</param>
    public void SetData(EM_ResourceType resourceType,int resourceId,string borderSprName,string name,int num,int disCount,int oriPrice,int nowPrice,EM_MoneyType moneyType,int buyTimes)
    {
        UITool.f_SetIconSprite(m_icon, resourceType, resourceId);
        m_border.spriteName = borderSprName;
        m_name.text = name;
        m_num.text = num.ToString();
        int ten = disCount / 10;
        int one = disCount % 10;
        // m_disCount.text = ten + (one == 0 ? "" : "." + one);
		m_disCount.text = (100 - disCount) + "%";
        m_OripriceType.spriteName = UITool.f_GetMoneySpriteName(moneyType);
        m_oriPrice.text = oriPrice.ToString();
        m_NowPriceType.spriteName = UITool.f_GetMoneySpriteName(moneyType);
        m_nowPrice.text = nowPrice.ToString();
        m_BtnBuy.SetActive(buyTimes >= 1 ? false : true);
        m_BtnHasBuy.SetActive(buyTimes >= 1 ? true : false);
    }
}
