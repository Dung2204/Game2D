using UnityEngine;
using System.Collections;
/// <summary>
/// 变长商店固定item
/// </summary>
public class ShopMutiFixedItem : MonoBehaviour
{
    public UITable NameTabe;

    public UIButton m_BtnBuy;//购买按钮
    public UILabel m_LabelName;//物品名称
    public UI2DSprite m_SprIcon;//物品图标
    public UILabel m_Labelcount;//物品数量
    public UISprite m_SprBuyType;//购买货币类型
    public UILabel m_LabelPrice;//物品价格
    public UISprite m_SprOriginalBuyType;//原价货币类型
    public UILabel m_LabelOriPrice;//原价
    public UILabel m_LabelDiscount;//折扣
    public UILabel m_LabelBuyProgress;//购买进度
    public UILabel m_LabelLeft;//物品剩余数量
    public UILabel m_LabelTimesLeft;//可以购买的次数
    public GameObject m_ObjCover;//已经购买的遮罩
    public GameObject m_ObjHasBuy;//已购买遮罩
    public UISprite m_Sprborder;//物品边框

    //-------------------待修改
    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="Icon"></param>
    /// <param name="count"></param>
    /// <param name="priceType"></param>
    /// <param name="price"></param>
    /// <param name="hasBuy"></param>
    public void SetData(string name, EM_ResourceType resourceType, int resourceId, string iconBordersprName, int count, EM_MoneyType moneyType,
        int price,int oriPrice,int disCount,int buyTimes,int maxBuyTimes,int canBuyTimes, bool hasBuy)
    {
        m_LabelName.text = name;
        UITool.f_SetIconSprite(m_SprIcon, resourceType, resourceId);
        m_Sprborder.spriteName = iconBordersprName;
        m_Labelcount.text = count.ToString();
        m_LabelPrice.text = price.ToString();
        m_LabelOriPrice.text = CommonTools.f_GetTransLanguage(2252) + oriPrice.ToString();
        int tenDis = disCount / 10;
        int oneDis = disCount % 10;
        m_LabelDiscount.text = tenDis + (oneDis > 0 ? ("." + oneDis) : "");
        m_LabelBuyProgress.text = "(" + buyTimes + "/" + maxBuyTimes + ")";
m_LabelLeft.text = "Remaining purchases" + (maxBuyTimes - buyTimes) + "";
m_LabelTimesLeft.text = "Available" + canBuyTimes + "";
        m_SprBuyType.spriteName = UITool.f_GetMoneySpriteName(moneyType);
        m_SprOriginalBuyType.spriteName = UITool.f_GetMoneySpriteName(moneyType);
        //m_SprBuyType.MakePixelPerfect();
        m_ObjHasBuy.SetActive(hasBuy);
        m_BtnBuy.gameObject.SetActive(!hasBuy);
        m_ObjCover.SetActive((maxBuyTimes - buyTimes) <= 0 ? true : false);
        NameTabe.Reposition();
    }
}
