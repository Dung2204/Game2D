using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
public class OtherProperty
{
    private int[] _Target;
    private int[] _AddPropertyId;
    private int[] _AddPropertyNum;
    private int _iIndex;
    private bool _bIsAddProperty;

    public bool m_bIsAddProperty
    {
        get
        {
            return _bIsAddProperty;
        }
    }

    public int m_iIndex
    {
        get { return _iIndex; }
    }

    public OtherProperty()
    {
        _Target = new int[10];
        _AddPropertyId = new int[10];
        _AddPropertyNum = new int[10];
    }
    public void f_Init()
    {
        _bIsAddProperty = false;
        _iIndex = 0;
        Array.Clear(_Target, 0, 10);
        Array.Clear(_AddPropertyId, 0, 10);
        Array.Clear(_AddPropertyNum, 0, 10);
    }

    public void f_AddOtherProperty(int iTarget, int iPropertyId, int iPropertyNum)
    {
        _bIsAddProperty = true;
        _Target[_iIndex] = iTarget;
        _AddPropertyId[_iIndex] = iPropertyId;
        _AddPropertyNum[_iIndex] = iPropertyNum;
        _iIndex++;
    }
    public int f_GetTarget(int index)
    {
        return _Target[index];
    }
    public int f_GetPropertyId(int index)
    {
        return _AddPropertyId[index];
    }
    public int f_GetPropertyNum(int index)
    {
        return _AddPropertyNum[index];
    }
}
