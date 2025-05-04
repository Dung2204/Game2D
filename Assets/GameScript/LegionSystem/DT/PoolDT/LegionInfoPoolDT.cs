using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class LegionInfoPoolDT : BasePoolDT<long>
{
    private long _iMasterUserId;
    private BaseProperty _LegionProperty = new BaseProperty((int)EM_LegionProperty.End);
    private string _szName;
    private string _szManifesto;
    private string _szNotice;

    public LegionInfoPoolDT()
    {
        _szName = string.Empty;
        _szManifesto = string.Empty;
        _szNotice = string.Empty;
    }

    #region 更新数据相关

    public void f_UpdateProperty(int iIndex, int iData)
    {
        _LegionProperty.f_UpdateProperty(iIndex, iData);
    }

    public void f_UpdateProperty(int iIndex, long iData)
    {
        if (iIndex == (int)EM_LegionProperty.MasterUserId)
        {
            _iMasterUserId = iData;
        }
    }

    public void f_UpdateProperty(int iIndex, string strText)
    {
        if (iIndex == (int)EM_LegionProperty.Name)
        {
            _szName = strText;
        }
        else if (iIndex == (int)EM_LegionProperty.Manifesto)
        {
            _szManifesto = strText;
        }
        else if (iIndex == (int)EM_LegionProperty.Notice)
        {
            _szNotice = strText;
        }
    }

    #endregion

    /// <summary>
    /// 获取int类型的相关属性  EM_LegionProperty
    /// </summary>
    /// <param name="iIndex"></param>
    /// <returns></returns>
    public int f_GetProperty(int iIndex)
    {
        return _LegionProperty.f_GetProperty(iIndex);
    }

    /// <summary>
    /// 军团长Id
    /// </summary>
    public long MasterUserId
    {
        get
        {
            return _iMasterUserId;
        }
    }

    /// <summary>
    /// 军团名字
    /// </summary>
    public string LegionName
    {
        get
        {
            return _szName;
        }
    }
    /// <summary>
    /// 宣言
    /// </summary>
    public string Manifesto
    {
        get
        {
            return _szManifesto;
        }
    }
    /// <summary>
    /// 公告
    /// </summary>
    public string Notice
    {
        get
        {
            return _szNotice;
        }
    }
}
