using UnityEngine;
using System.Collections;
using ccU3DEngine;

/// <summary>
/// 自定义二维
/// </summary>
public sealed class Vector2Map
{
    public Vector2Map()
    {

    }

    public Vector2Map(int iX, int iY)
    {
        x = iX;
        y = iY;
    }

    public static Vector2Map zero
    {
        get
        {
            return new Vector2Map(0, 0);
        }
    }


    public int x;
    public int y;
}


public class stGridItemData
{
    public int m_iData;

    public NBaseSCDT m_ItemData;
    public GameObject m_TmpObj;
}