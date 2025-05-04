using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class HelpDataDT : NBaseSCDT {

    ///<summary>
    ///数据
    ///</summary>
    public string _szData;
    public string szData
    {
        get
        {
            return glo_Main.GetInstance().m_SC_Pool.m_TranslateConfigSC.GetTranslateTextByKey(_szData);
        }
    }
    ///<summary>
    ///特殊处理
    ///</summary>
    public string szSpecial;
	
}
