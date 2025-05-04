using UnityEngine;
using System.Collections;



public class ResourceDT
{
    public int iId;

    /// <summary>
    /// 文字资源TEXT = 1,
    /// 普通游戏资源GameObject = 2,
    /// 包含列表游戏资源ObjectList = 3,
    /// 二进制资源BIN = 4,
    /// 多sprite资源 = 5;
    /// </summary>
    public int iType;

    /// <summary>
    /// 优先级
    /// </summary>
    public int iPriority;

    /// <summary>
    /// 是否为本地资源
    /// </summary>
    public int iIsInResource;

    /// <summary>
    /// 存放位置
    /// </summary>
    public string strPath;

    public string Md5;

    public int[] m_aDenpendency;


    public AssetBundle m_Ab = null;

    public AssetBundleCreateRequest m_AbcRequest;

    public Object m_Obj = null;
    public MulSprite m_MulSprite = null;


    public bool f_CheckIsLoadComplete()
    {
        if (m_Obj != null || m_MulSprite != null)
        {
            return true;
        }
        return false;
    }

}
