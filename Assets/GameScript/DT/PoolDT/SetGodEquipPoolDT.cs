using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;

public class SetGodEquipPoolDT : BasePoolDT<long>
{
    public SetGodEquipDT m_SetEquipDT;

    /// <summary>
    /// 装备是否组成 (true已有，false未有)
    /// </summary>
    public bool[] m_aSetIsOk = new bool[4];
}
