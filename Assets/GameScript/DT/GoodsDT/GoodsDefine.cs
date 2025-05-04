using UnityEngine;
using System.Collections;

/// <summary>
/// 物品类别定义
/// </summary>
public enum GoodType
{
    GoodEuipPieces,//装备碎片
    GoodEuip,//装备
    GoodCardPieces,//卡牌碎片
    GoodTreasurePieces,//宝物碎片
    GoodTreasure,//法宝
    GoodRouse,//领悟物品
    GoodMedicine,//药品
}
/// <summary>
/// 装备碎片物品
/// </summary>
public class GoodsEquipPieces : GoodsItem
{
    public int goodId;//物品id
    public string m_strTextureName;//装备碎片图片名字
    public GoodEquip m_syntheticQuip;//可合成的装备
    public string m_strIntro;//碎片介绍
    public GoodsEquipPieces()
    {
        m_goodType = GoodType.GoodEuipPieces;
    }
    /// <summary>
    /// 比较两个装备碎片是否相等
    /// </summary>
    /// <param name="goodItem">待比较的碎片物品</param>
    /// <returns></returns>
    public override bool f_Equal(GoodsItem goodItem)
    {
        bool isSame = true;
        if (goodItem.m_goodType != m_goodType || goodItem.m_strGoodsName != m_strGoodsName)
            return false;
        //GoodsEquipPieces goodsEquipPieces = (GoodsEquipPieces)goodItem;
        return isSame;
    }
}
/// <summary>
/// 装备物品
/// </summary>
public class GoodEquip : GoodsItem
{
    public int goodId;//物品id
    public string m_strTextureName;//装备图片名字
    public string m_strIntro;//装备介绍
    public GoodEquip()
    {
        m_goodType = GoodType.GoodEuip;
    }
    /// <summary>
    /// 比较两个装备是否相等
    /// </summary>
    /// <param name="goodItem">待比较的装备物品</param>
    /// <returns></returns>
    public override bool f_Equal(GoodsItem goodItem)
    {
        bool isSame = true;
        if (goodItem.m_goodType != m_goodType || goodItem.m_strGoodsName != m_strGoodsName)
            return false;
        GoodEquip goodEquip = (GoodEquip)goodItem;
        return isSame;
    }
}
