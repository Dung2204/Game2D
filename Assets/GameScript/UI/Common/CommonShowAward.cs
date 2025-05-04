using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;

public class CommonShowAward : UIFramwork
{
    private CommonShowAwardParam Param;

    private UIWrapComponent _Wrap;

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (!(e is CommonShowAwardParam))
        {
MessageBox.ASSERT("CommonShowAward interface parameter error");
        }
        Param = e as CommonShowAwardParam;
        if (Param.m_PoolDTLise != null)
            _Wrap = new UIWrapComponent(140, 1, 667, 6, f_GetObject("ItemParam"), Param.m_Item, Param.m_PoolDTLise, Param.m_PoolDTUpdate, Param.m_PoolDTClick);

        else
            _Wrap = new UIWrapComponent(140, 1, 667, 6, f_GetObject("ItemParam"), Param.m_Item.gameObject, Param.m_SCDTList, Param.m_SCDTUpdate, Param.m_SCDTClick);

        _Wrap.f_UpdateView();

    }

    protected override void f_InitMessage()
    {
        base.f_InitMessage();

        f_RegClickEvent("Close", f_Close);
        f_RegClickEvent("MaskClose", f_Close);
    }

    private void f_Close(GameObject go,object obj1,object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonShowAward,UIMessageDef.UI_CLOSE);
        _Wrap.f_DeleAllChild();
        _Wrap = null;
    }
}

public class CommonShowAwardParam
{
    public GameObject m_Item;
    public List<BasePoolDT<long>> m_PoolDTLise;
    public List<NBaseSCDT> m_SCDTList;
    public ccCallBack_WrapItemUpdate m_PoolDTUpdate;
    public ccCallBack_WrapItemClick m_PoolDTClick;
    public ccCallBack_WrapItemNBaseSCDTClick m_SCDTClick;
    public ccCallBack_WrapItemNBaseSCDTUpdate m_SCDTUpdate;
}
