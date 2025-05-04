using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;

public class MammonSendGiftsPage2 : UIFramwork
{
    private Transform _ItemParent;
    private SocketCallbackDT _InfoSocketback = new SocketCallbackDT();
    private SocketCallbackDT _GetSocketback = new SocketCallbackDT();
    private List<AwardPoolDT> _ShowAward = new List<AwardPoolDT>();
    MammonSendGiftDT ToDayMammonSendGiftDT;


    private string _szMammonBg = "UI/TextureRemove/Activity/MammonSendBg";

    private void _Loding()
    {
        f_GetObject("MammonBg").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(_szMammonBg);
    }
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("GetGoodsBtn", _OnGetGoods);
        f_RegClickEvent("Alphe", _CloseGoods);
    }

    public void f_ShowView()
    {
        _Loding();
        gameObject.SetActive(true);
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_NewYearActivityPool.m_bIsShowOnePage = true;
        _InfoSocketback.m_ccCallbackSuc = _InfoSuc;
        _InfoSocketback.m_ccCallbackFail = _InfoFail;
        Data_Pool.m_NewYearActivityPool.f_MammonSendGiftNewInfo(_InfoSocketback);
        MammonSendGiftDT tMammonSendGiftDT;
        int MammonId = Data_Pool.m_NewYearActivityPool.m_MammonSendGiftArr.m_DTId;
        ToDayMammonSendGiftDT = glo_Main.GetInstance().m_SC_Pool.m_MammonSendGiftSC.f_GetSC(MammonId) as MammonSendGiftDT;
        int ToDay = 0;
        for(int i = 0; i < glo_Main.GetInstance().m_SC_Pool.m_MammonSendGiftSC.f_GetAll().Count; i++)
        {
            tMammonSendGiftDT = glo_Main.GetInstance().m_SC_Pool.m_MammonSendGiftSC.f_GetAll()[i] as MammonSendGiftDT;

            if(ToDayMammonSendGiftDT.iActivityType != tMammonSendGiftDT.iActivityType)
            {
                continue;
            }
            else
            {
                if(tMammonSendGiftDT.iStarTime > ToDay)
                {
                    ToDay = tMammonSendGiftDT.iStarTime;
                }
            }

        }

        //_Time = glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.MammonSendGiftsOpenTime) as GameParamDT;
        string str = ToDay.ToString();
        int year = int.Parse(str.Substring(0, 4));//20181224
        int month = int.Parse(str.Substring(4, 2));
        int day = int.Parse(str.Substring(6, 2));
        f_GetObject("OverTime").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1492), year, month, day);
    }

    private void _UpdateMain()
    {
        _ItemParent = f_GetObject("ItemParent").transform;
        if(_ItemParent.childCount > 0)
        {
            for(int i = 0; i < _ItemParent.childCount; i++)
                GameObject.Destroy(_ItemParent.GetChild(i).gameObject);
        }
        System.DateTime tTime = ccMath.time_t2DateTime(GameSocket.GetInstance().f_GetServerTime());

        


        //bool _IsGet = false;
        //for (int i = 0; i < Data_Pool.m_NewYearActivityPool.m_MammonSendGiftArr.Length; i++)
        //{
        //    if (tMammonSendGiftDt.iId == i + 1)
        //    {
        //        if (Data_Pool.m_NewYearActivityPool.m_MammonSendGiftArr[i] == 1)
        //        {
        //            _IsGet = true;
        //            break;
        //        }
        //    }
        //}

        f_GetObject("GetGoodsBtn").SetActive(!Data_Pool.m_NewYearActivityPool.m_bMammonSendGiftIsGet && Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) >= 25);
        f_GetObject("GetGood").SetActive(Data_Pool.m_NewYearActivityPool.m_bMammonSendGiftIsGet);


        List<ResourceCommonDT> listCommonDT = CommonTools.f_GetListCommonDT(ToDayMammonSendGiftDT.szAwardId);
        for(int i = 0; i < listCommonDT.Count; i++)
        {
            _CreateItem(listCommonDT[i].mIcon, listCommonDT[i].mImportant, listCommonDT[i].mResourceNum);
        }
        _ItemParent.GetComponent<UIGrid>().Reposition();
        _ItemParent.parent.GetComponent<UIScrollView>().ResetPosition();
    }


    private void _CreateItem(int IconID, int DropOut, int Num)
    {
        GameObject tgo = NGUITools.AddChild(_ItemParent.gameObject, f_GetObject("Item")); //  Instantiate<GameObject>(f_GetObject("Item"));
        tgo.transform.Find("Case").GetComponent<UISprite>().spriteName = UITool.f_GetImporentCase(DropOut);
        tgo.transform.Find("Icon").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSprite(IconID);
        tgo.transform.Find("Num").GetComponent<UILabel>().text = string.Format("x{0}", Num);
        tgo.SetActive(true);
        //f_RegClickEvent(tgo, _OnBox, DropOut);
    }
    private void _OnBox(GameObject go, object obj1, object obj2)
    {
        int DropOut = (int)obj1;

        for(int i = 0; i < f_GetObject("GoodsParent").transform.childCount; i++)
            Destroy(f_GetObject("GoodsParent").transform.GetChild(i).gameObject);

        _ShowAward = Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(DropOut);
        AwardPoolDT tAwardPoolDT;
        for(int i = 0; i < _ShowAward.Count; i++)
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
        Data_Pool.m_NewYearActivityPool.f_MammonSendGiftGet2(_GetSocketback);
    }


    private void _GetSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        _UpdateMain();
        f_GetObject("GetGoodsBtn").SetActive(!Data_Pool.m_NewYearActivityPool.m_bMammonSendGiftIsGet && Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) >= 25);
        if(Data_Pool.m_AwardPool.m_GetLoginAward.Count > 0)
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


    private void _InfoSuc(object obj)
    {
        _UpdateMain();
        UITool.f_OpenOrCloseWaitTip(false);
    }

    private void _InfoFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
    }
}
