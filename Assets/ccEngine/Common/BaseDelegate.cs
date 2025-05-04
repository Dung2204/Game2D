using UnityEngine;
using ccU3DEngine;
/// <summary>
/// Delegate统一定义接口
/// </summary>

public delegate void ccCallback_IS(int iData1, string szData);
public delegate void ccCallback_IIS(int iData1, int iData2, string szData);


public delegate void ccCallBack_WrapItemUpdate(Transform item, BasePoolDT<long> dt);
public delegate void ccCallBack_WrapItemClick(Transform item, BasePoolDT<long> dt);

public delegate void ccCallBack_WrapItemNBaseSCDTUpdate(Transform item, NBaseSCDT dt);
public delegate void ccCallBack_WrapItemNBaseSCDTClick(Transform item, NBaseSCDT dt);

public delegate void ccCallBack_TextDelay(GameObject go, string text);
