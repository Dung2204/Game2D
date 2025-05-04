using UnityEngine;
using System.Collections;

public class Battle2MenuProcessParam
{
    public EM_Battle2MenuProcess m_emType
    {
        private set;
        get;
    }

    public object[] m_Params
    {
        private set;
        get;
    }

    public Battle2MenuProcessParam()
    {
        m_emType = EM_Battle2MenuProcess.None;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="param">处理该类型的相关参数</param>
    public void f_UpdateParam(EM_Battle2MenuProcess type,params object[] param)
    {
        m_emType = type;
        m_Params = param;
    }
}
