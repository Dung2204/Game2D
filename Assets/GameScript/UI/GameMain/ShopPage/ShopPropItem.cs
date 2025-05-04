using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 商店购买道具item
/// </summary>
public class ShopPropItem : MonoBehaviour {
    public UILabel LabelName;//道具名称
    public UILabel LabelLimitTimes;//限购次数
    public UILabel LabelPrice;//价格
    public GameObject BtnBuy;//购买
    public UI2DSprite Icon;//道具图片
    public UISprite m_SprBoder;//边框背景
    public UILabel m_LabelCount;//道具数量
    public GameObject m_ObjDiscount;//折扣
    public GameObject m_ObjOriginalPrice;
    public UILabel m_labelOriginalPrice;
    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="propName">道具名称</param>
    /// <param name="limitTimes">限购次数</param>
    /// <param name="price">价格</param>
    /// <param name="hasBuyCount">已经购买次数</param>
    public void SetData(EM_ResourceType resourceType,int resourceId,string borderSprName,string propName,int propCount,int discount,int limitTimes,int price,int hasBuyCount)
    {
        UITool.f_SetIconSprite(Icon, resourceType, resourceId);
        m_SprBoder.spriteName = borderSprName;
        LabelName.text = propName;
        m_LabelCount.text = propCount.ToString();
        if (limitTimes > 0)
        {
            LabelLimitTimes.text = CommonTools.f_GetTransLanguage(1121) + " "  + hasBuyCount + "/" + limitTimes;
        }
        else//小于等于0表示不限购(不显示限购字)
        {
            LabelLimitTimes.text = "";
        }
        LabelPrice.text = price.ToString();
        m_ObjOriginalPrice.SetActive(discount > 0);

        int tenDisCount = discount / 10;
        int oneDisCount = discount % 10;
        if (tenDisCount == 0 && oneDisCount == 0)
        {
            m_ObjDiscount.SetActive(false);
        }
        else
        {
            m_ObjDiscount.SetActive(true);
            m_labelOriginalPrice.text = (price * 100 / discount).ToString();
            // m_ObjDiscount.GetComponentInChildren<UILabel>().text = tenDisCount + (oneDisCount == 0 ? "" : ("." + oneDisCount));
			m_ObjDiscount.GetComponentInChildren<UILabel>().text = ( 100 - discount ) + "%";
        }
    }
}
