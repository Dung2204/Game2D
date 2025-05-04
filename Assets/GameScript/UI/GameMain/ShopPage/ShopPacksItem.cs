using UnityEngine;
using System.Collections;
/// <summary>
/// 商城礼包item控制
/// </summary>
public class ShopPacksItem : MonoBehaviour {
    public UILabel LabelGiftName;
    public UILabel LabelOriPrice;//原价
    public UILabel LabelPrice;
    public UILabel LabelDescrip;//描述
    public UILabel LabelVipLimitHint;//提示vip等级限制
    public GameObject BtnBuy;
    public GameObject BtnIconProp;
    public GameObject ObjRedPoint;//红点
    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="name">礼包名称</param>
    /// <param name="oriPrice">原价</param>
    /// <param name="price">现价</param>
    /// <param name="descript">描述</param>
    /// <param name="vipLimitHint">vip等级提示</param>
    public void SetData(string name, int oriPrice, int price, string descript,string vipLimitHint,bool isShowRedPoint)
    {
        LabelGiftName.text = name.ToString();
        LabelOriPrice.text = oriPrice.ToString();
        LabelPrice.text = price.ToString();
        LabelDescrip.text = descript;
        LabelVipLimitHint.text = vipLimitHint;
        ObjRedPoint.SetActive(isShowRedPoint);
    }
}
