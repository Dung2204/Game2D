using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;

public class CardFatePoolDT : BasePoolDT<long>
{
    public CardFateDT m_CardFateDT;
    /// <summary>
    /// 卡牌对应的缘分清单
    /// </summary>
    public List<CardFateDataDT> m_aFateList = new List<CardFateDataDT>();

    /// <summary>
    /// 卡牌获得缘分清单 (true获得，false未获得)
    /// </summary>
    public List<bool> m_aFateIsOk = new List<bool>();
}