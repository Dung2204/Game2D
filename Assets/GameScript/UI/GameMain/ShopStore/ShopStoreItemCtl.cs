using UnityEngine;
using System.Collections;
/// <summary>
/// 通用商店item控制
/// </summary>
public class ShopStoreItemCtl : MonoBehaviour
{
    public UIButton m_BtnBuy;//购买按钮
    public UILabel m_LabelName;//物品名称
    public UI2DSprite m_SprIcon;//物品图标
    public UILabel m_Labelcount;//物品数量
    public UILabel LabelHaveHint;//拥有数量提示
    public UISprite m_SprOriginalBuyType;//原价货币类型
    public UILabel  m_LabelOriginalPrice;//物品原价
    public UISprite m_SprBuyType;//购买货币类型
    public UILabel m_LabelPrice;//物品价格
    public GameObject m_ObjCover;//已经购买的遮罩
    public UISprite m_Sprborder;//物品边框
    public GameObject m_SprInField;   //已上阵
    public GameObject m_SprPartner;    //小伙伴
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
    public void SetData(string name, EM_ResourceType resourceType, int resourceId, string iconBordersprName, int count, EM_MoneyType moneyType, int price, bool hasBuy, string haveCountHint)
    {
        m_LabelName.text = name;
        UITool.f_SetIconSprite(m_SprIcon, resourceType, resourceId);
        m_Sprborder.spriteName = iconBordersprName;
        m_Labelcount.text = count.ToString();
        m_LabelPrice.text = price.ToString(); 
        LabelHaveHint.text = haveCountHint;
        m_ObjCover.SetActive(hasBuy);
        m_SprBuyType.spriteName = UITool.f_GetMoneySpriteName(moneyType); 

        //这里后面策划说没有打折，但是预设有加，所以直接active设false
        m_SprOriginalBuyType.spriteName = UITool.f_GetMoneySpriteName(moneyType);
        m_SprOriginalBuyType.gameObject.SetActive(false);

        m_SprInField.SetActive(false);
        m_SprPartner.SetActive(false);
        if (resourceType == EM_ResourceType.Card || resourceType == EM_ResourceType.CardFragment)
        {
            bool bInfield = Data_Pool.m_TeamPool.f_CheckInTeamByKeyId(resourceId);
            bool bPartner = Data_Pool.m_TeamPool.f_CheckReinforceCardForTempId(resourceId);
            m_SprInField.SetActive(bInfield);
            m_SprPartner.SetActive(bPartner);
        }

    }
}
