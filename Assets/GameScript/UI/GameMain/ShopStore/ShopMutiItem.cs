using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 商店item
/// </summary>
public class ShopMutiItem : MonoBehaviour {
    public UITable NameTabe;
    public UILabel Name;
    public UISprite IconBorder;
    public UI2DSprite Icon;
    public UILabel LabelCount;
    public UILabel LabelBuyTimes;//购买次数
    public UILabel LabelBuyLimit; //购买限制
    public UILabel LabelHaveHint;//拥有数量提示
    public UISprite SprPrice1;//购买价格1
    public UILabel LabelPrice1;//购买价格1
    public UISprite SprOriginalPrice1;//购买原价格1
    public UILabel LabelOriginalPrice1;//购买原价格1
    public UISprite SprPrice2;//购买价格2
    public UILabel LabelPrice2;//购买价格2
    public UISprite SprOriginalPrice2;//购买原价格2
    public UILabel LabelOriginalPrice2;//购买原价格2
    public UISprite SprDiscount;//折价
    public UILabel  LabelDiscount;//折价
    public GameObject ObjHasBuy;//已购买
    public GameObject ObjLock;//不开放购买
    public GameObject BtnBuy;//购买按钮
    public GameObject m_SprInField;   //已上阵
    public GameObject m_SprPartner;    //小伙伴
    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="icon"></param>
    /// <param name="borderName"></param>
    /// <param name="name"></param>
    public void SetData(EM_ResourceType resourceType,int resourceId,string borderName,string name,int count,string hintStr,string buyTimes,
        EM_MoneyType moneyType1,int price1,EM_MoneyType moneyType2,int price2,int disCount,bool showHasBuy, bool isOpen, string haveCountHint)
    {
        Name.text = name;
        LabelBuyTimes.text = buyTimes;
        LabelBuyLimit.text = hintStr;
        LabelHaveHint.text = haveCountHint;
        NameTabe.repositionNow = true;
        NameTabe.Reposition();
        IconBorder.spriteName = borderName;
        UITool.f_SetIconSprite(Icon, resourceType, resourceId);
        LabelCount.text = count.ToString(); 
        SprPrice1.gameObject.SetActive(price1 > 0 ? true : false);
        if (price1 > 0)
        {
            SprPrice1.spriteName = UITool.f_GetMoneySpriteName(moneyType1);
            
            //SprPrice1.gameObject.GetComponent<UISprite>().MakePixelPerfect();
        }
        LabelPrice1.text = price1.ToString();
        SprPrice2.gameObject.SetActive(price2 > 0 ? true : false);
        if (price2 > 0)
        {
            SprPrice2.spriteName = UITool.f_GetMoneySpriteName(moneyType2);
            //SprPrice2.gameObject.GetComponent<UISprite>().MakePixelPerfect();
        }
        LabelPrice2.text = price2.ToString();
        
        if (disCount > 0 && disCount < 100)
        {
            SprDiscount.gameObject.SetActive(true);
            float disPrice = 100 - disCount;
            LabelDiscount.text = ((int)disPrice).ToString() + "%";
            SprOriginalPrice1.gameObject.SetActive(price1 > 0);
            if (price1 > 0)
            {
                LabelOriginalPrice1.text = (price1 * 100 / disCount).ToString();
                SprOriginalPrice1.spriteName = UITool.f_GetMoneySpriteName(moneyType1);
            }

            SprOriginalPrice2.gameObject.SetActive(price2 > 0);
            if (price2 > 0)
            {
                LabelOriginalPrice2.text = (price2 * 100 / disCount).ToString();
                SprOriginalPrice2.spriteName = UITool.f_GetMoneySpriteName(moneyType2);
            }
        }
        else
        {
            SprDiscount.gameObject.SetActive(false);
            SprOriginalPrice1.gameObject.SetActive(false);
            SprOriginalPrice2.gameObject.SetActive(false);
        }
        ObjHasBuy.SetActive(showHasBuy);
        ObjLock.SetActive(!isOpen);
        BtnBuy.SetActive(isOpen);

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
