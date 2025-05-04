using UnityEngine;
using System.Collections;
/// <summary>
/// 游戏物品抽象类（所有物品的基类）
/// </summary>
public abstract class GoodsItem{
    /// <summary>
    /// 物品类别
    /// </summary>
    private GoodType goodType;
    /// <summary>
    /// 物品类别的get set方法
    /// </summary>
    public GoodType m_goodType
    {
        get { return goodType; }
        set { goodType = value; }
    }
    /// <summary>
    /// 物品名称
    /// </summary>
    private string strGoodsName;//物品名称
    /// <summary>
    /// 物品名称的get set方法
    /// </summary>
    public string m_strGoodsName{
        get { return strGoodsName; }
        set { strGoodsName = value; }
    }


    #region 需实现的抽象类
    /// <summary>
    /// 比较两个物品是否匹配
    /// </summary>
    /// <param name="goodItem">需要比较的物品</param>
    /// <returns></returns>
    public abstract bool f_Equal(GoodsItem goodItem);
    #endregion
}
