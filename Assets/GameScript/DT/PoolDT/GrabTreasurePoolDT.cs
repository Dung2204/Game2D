using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 夺宝结算poolDT
/// </summary>
public class GrabTreasurePoolDT : BasePoolDT<long> {
    /// <summary>
    /// 有没有获取碎片id，如果获取了则为0
    /// </summary>
    public int treaFragID;
    /// <summary>
    /// 奖池id
    /// </summary>
    public int awardID;
    /// <summary>
    /// 奖励类型
    /// </summary>
    public byte resourceType;
    /// <summary>
    /// 奖励ID
    /// </summary>
    public int resourceId;
    /// <summary>
    /// 奖励数量
    /// </summary>
    public int resourceNum;
    /// <summary>
    /// 战斗星数
    /// </summary>
    public int star;

}
