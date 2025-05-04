using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 基本属性管理
/// </summary>
public class BaseProperty
{
    int iTTT;
    private int[] _aData = null;


    public BaseProperty(int iLen)
    {
       _aData = new int[iLen];
        iTTT = ccMath.f_CreateKeyId();

    }

    /// <summary>
    /// 复制数据
    /// </summary>
    /// <param name="aData">来源数组</param>
    /// <param name="iStartPos">目录的开始位置</param>
    /// <param name="iLen">复制长度</param>
    virtual public void f_Copy(int[] aData, int iStartPos, int iLen)
    {
        Array.Copy(aData, 0, _aData, iStartPos, iLen);
    }

    /// <summary>
    /// 增加属性
    /// </summary>
    /// <param name="iIndex"></param>
    /// <param name="iData"></param>
    virtual public void f_AddProperty(int iIndex, int iData)
    {
        _aData[iIndex] += iData;
    }

    /// <summary>
    /// 减少属性
    /// </summary>
    /// <param name="iIndex"></param>
    /// <param name="iData"></param>
    virtual public void f_SubtractProperty(int iIndex, int iData)
    {
        _aData[iIndex] -= iData;
    }

    /// <summary>
    /// 更新属性
    /// </summary>
    /// <param name="iIndex"></param>
    /// <param name="iData"></param>
    virtual public void f_UpdateProperty(int iIndex, int iData)
    {
        _aData[iIndex] = iData;
    }

    /// <summary>
    /// 获取属性
    /// </summary>
    /// <param name="iIndex"></param>
    /// <returns></returns>
    virtual public int f_GetProperty(int iIndex)
    {
        return _aData[iIndex];
    }

    virtual public void f_Reset()
    {
        Array.Clear(_aData, 0, (int)EM_RoleProperty.End);
    }
    
}
