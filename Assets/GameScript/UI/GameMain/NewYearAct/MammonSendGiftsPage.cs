using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;

public class MammonSendGiftsPage : UIFramwork
{
    private Transform _ItemParent;
    private SocketCallbackDT _GetSocketback = new SocketCallbackDT();
    private List<AwardPoolDT> _ShowAward = new List<AwardPoolDT>();

    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("GetGoodsBtn", _OnGetGoods);
        f_RegClickEvent("Alphe", _CloseGoods);
    }

    public void f_ShowView()
    {
        gameObject.SetActive(true);
        Data_Pool.m_NewYearActivityPool.m_bIsShowOnePage = true;
        _UpdateMain();
    }

    private void _UpdateMain()
    {
        _ItemParent = f_GetObject("ItemParent").transform;
        if (_ItemParent.childCount > 0)
        {
            for (int i = 0; i < _ItemParent.childCount; i++)
                GameObject.Destroy(_ItemParent.GetChild(i).gameObject);
        }
        System.DateTime tTime = ccMath.time_t2DateTime(GameSocket.GetInstance().f_GetServerTime());
        f_GetObject("GetGoodsBtn").SetActive(!Data_Pool.m_NewYearActivityPool.m_bMammonSendGiftIsGet && Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) >= 25 &&
            tTime.Hour >= 12 && tTime.Hour < 24);
        GameParamDT tGameParamIcon = UITool.f_GetGameParamDT((int)EM_GameParamType.MammonSendGiftIcon);
        GameParamDT tGameParamDropOut = UITool.f_GetGameParamDT((int)EM_GameParamType.MammonSendGiftAwardId);
        if (tGameParamIcon.iParam1 > 0)
            _CreateItem(tGameParamIcon.iParam1, tGameParamDropOut.iParam1);
        if (tGameParamIcon.iParam2 > 0)
            _CreateItem(tGameParamIcon.iParam2, tGameParamDropOut.iParam2);
        if (tGameParamIcon.iParam3 > 0)
            _CreateItem(tGameParamIcon.iParam3, tGameParamDropOut.iParam3);
        if (tGameParamIcon.iParam4 > 0)
            _CreateItem(tGameParamIcon.iParam4, tGameParamDropOut.iParam4);

        _ItemParent.GetComponent<UIGrid>().Reposition();
        _ItemParent.parent.GetComponent<UIScrollView>().ResetPosition();
    }


    private void _CreateItem(int IconID, int DropOut)
    {
        GameObject tgo = NGUITools.AddChild(_ItemParent.gameObject, f_GetObject("Item")); //  Instantiate<GameObject>(f_GetObject("Item"));
        tgo.transform.Find("Case").GetComponent<UISprite>().spriteName = UITool.f_GetImporentCase((int)EM_Important.Red);
        tgo.transform.Find("Icon").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSprite(IconID);
        if (IconID == 7)
        {
            tgo.transform.Find("Num").GetComponent<UILabel>().text = "x666";
        }
        else
        {
            tgo.transform.Find("Num").GetComponent<UILabel>().text = "";
        }
        tgo.SetActive(true);
        //f_RegClickEvent(tgo, _OnBox, DropOut);
    }
    private void _OnBox(GameObject go, object obj1, object obj2)
    {
        int DropOut = (int)obj1;

        for (int i = 0; i < f_GetObject("GoodsParent").transform.childCount; i++)
            Destroy(f_GetObject("GoodsParent").transform.GetChild(i).gameObject);

        _ShowAward = Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(DropOut);
        AwardPoolDT tAwardPoolDT;
        for (int i = 0; i < _ShowAward.Count; i++)
        {
            tAwardPoolDT = _ShowAward[i];
            ResourceCommonItem tResourceCommonItem = ResourceCommonItem.f_Create(f_GetObject("GoodsParent"), f_GetObject("ResourceCommonItem"));
            tResourceCommonItem.f_UpdateByInfo(tAwardPoolDT.mTemplate.mResourceType, tAwardPoolDT.mTemplate.mResourceId, tAwardPoolDT.mTemplate.mResourceNum);
            tResourceCommonItem.gameObject.SetActive(true);
        }
        f_GetObject("GoodsParent").GetComponent<UIGrid>().Reposition();
        f_GetObject("ShowGoods").SetActive(true);
    }

    private void _CloseGoods(GameObject go, object obj1, object obj2)
    {
        f_GetObject("ShowGoods").SetActive(false);
    }
    private void _OnGetGoods(GameObject go, object obj1, object obj2)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_AwardPool.m_GetLoginAward.Clear();
        _GetSocketback.m_ccCallbackFail = _GetFail;
        _GetSocketback.m_ccCallbackSuc = _GetSuc;
        Data_Pool.m_NewYearActivityPool.f_MammonSendGiftGet(_GetSocketback);
    }


    private void _GetSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        f_GetObject("GetGoodsBtn").SetActive(!Data_Pool.m_NewYearActivityPool.m_bMammonSendGiftIsGet && Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) >= 25);
        if (Data_Pool.m_AwardPool.m_GetLoginAward.Count > 0)
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN, new object[] { Data_Pool.m_AwardPool.m_GetLoginAward });
    }

    private void _GetFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
    }


    public void f_DestoryView()
    {
        gameObject.SetActive(false);
    }
}
