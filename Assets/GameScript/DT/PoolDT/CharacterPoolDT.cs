using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;

public class CharacterPoolDT : BasePoolDT<int>
{
    
    /// <summary>
    /// 当前小人经验
    /// </summary>
    public int m_iExp;
    
    /// <summary>
    /// 对应等级脚本CharacterLvUp
    /// </summary>
    public int m_iTempleteId
    {
        get;
        set;
    }
    
    public CharacterDT m_CharacterDT;
    
}
