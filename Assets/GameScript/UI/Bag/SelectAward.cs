using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;

public class SelectAward : UIFramwork
{
    BaseGoodsPoolDT tGoods;
    List<AwardPoolDT> tAwardDT;
    AwardPoolDT tselectAward;
    GameObject Item;
    GameObject GoodsItemParent;
    UILabel ui_Num;

    private int SelectNum;
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        tGoods = (BaseGoodsPoolDT)e;
        SelectNum = 1;
        tAwardDT = Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(tGoods.m_BaseGoodsDT.iEffectData);

        f_GetObject("BoxName").GetComponent<UILabel>().text = tGoods.m_BaseGoodsDT.szName;
f_GetObject("OwnNum").GetComponent<UILabel>().text = string.Format("Yes: {0}", tGoods.m_iNum);
        ui_Num = f_GetObject("Num").GetComponent<UILabel>();
        ui_Num.text = SelectNum.ToString();
        InitUI();
    }
    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
MessageBox.DEBUG("++++++++++++++++++++++++++++++++++++++++++++ +Select UNHOLD reward");
    }
    GameObject[] ItemGoArr;

    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent(f_GetObject("SucBtn"), UI_Suc);
        f_RegClickEvent(f_GetObject("Sprite"), (GameObject go, object obj1, object obj2) => { UI_Close(UINameConst.SelectAward); });

        f_RegClickEvent("Add", AddOrMinusNum, 1);
        f_RegClickEvent("Add10", AddOrMinusNum, 10);
        f_RegClickEvent("Minus", AddOrMinusNum, -1);
        f_RegClickEvent("Minus10", AddOrMinusNum, -10);
    }
    void InitUI()
    {
        Item = f_GetObject("IconAndNumItem");
        ItemGoArr = new GameObject[tAwardDT.Count];
        GoodsItemParent = f_GetObject("GoodsItemParent");
        for(int i = 0; i < tAwardDT.Count; i++)
        {
            if(GoodsItemParent.transform.childCount > i)
            {
                GoodsItemParent.transform.GetChild(i).GetComponent<ResourceCommonItem>().f_UpdateByInfo(tAwardDT[i].mTemplate.mResourceType, tAwardDT[i].mTemplate.mResourceId, tAwardDT[i].mTemplate.mResourceNum, EM_CommonItemShowType.All, (EM_CommonItemClickType)tAwardDT[i].mTemplate.mResourceType, this);
                GoodsItemParent.transform.GetChild(i).Find("Case").GetComponent<UIToggle>().value = false;
            }
            else
            {
                ResourceCommonItem tttt = ResourceCommonItem.f_Create(GoodsItemParent, Item);
                tttt.f_UpdateByInfo(tAwardDT[i].mTemplate.mResourceType, tAwardDT[i].mTemplate.mResourceId, tAwardDT[i].mTemplate.mResourceNum, EM_CommonItemShowType.All, (EM_CommonItemClickType)tAwardDT[i].mTemplate.mResourceType, this);
            }
        }
        if(tAwardDT.Count < GoodsItemParent.transform.childCount)
            for(int i = tAwardDT.Count; i < GoodsItemParent.transform.childCount; i++)
                GoodsItemParent.transform.GetChild(i).gameObject.SetActive(false);
        for(int i = 0; i < GoodsItemParent.transform.childCount; i++)
            GoodsItemParent.transform.GetChild(i).Find("Case").GetComponent<UIToggle>().value = false;

        GoodsItemParent.GetComponent<UIGrid>().enabled = true;
        GoodsItemParent.transform.parent.GetComponent<UIScrollView>().UpdatePosition();
    }

    void UI_Suc(GameObject go, object obj1, object obj2)
    {
        int num = 0;
        for(int i = 0; i < GoodsItemParent.transform.childCount; i++)
        {
            if(GoodsItemParent.transform.GetChild(i).Find("Case").GetComponent<UIToggle>().value)
            {
                num++;
                UITool.f_OpenOrCloseWaitTip(true);
                SocketCallbackDT tsockercall = new SocketCallbackDT();
                tsockercall.m_ccCallbackSuc = _UseSuc;
                tsockercall.m_ccCallbackFail = _UseFail;
                Data_Pool.m_BaseGoodsPool.f_Use((long)tGoods.iId, (int)SelectNum, (int)i + 1, tsockercall);
                tselectAward = tAwardDT[i];
            }
        }
        if(num != 1)
UITool.Ui_Trip("Chưa chọn vật phẩm");
    }
    void _UseSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        List<AwardPoolDT> tGoods = Data_Pool.m_AwardPool.m_GetLoginAward;
        UI_Close(null);
        if(tGoods.Count >= 1)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN, new object[] { tGoods });
        }
    }
    void _UseFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);

    }
    void UI_Close(object obj1)
    {
        //if (obj1 != null)
        //ccUIHoldPool.GetInstance().f_UnHold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SelectAward, UIMessageDef.UI_CLOSE);
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.GoodsBagPage, UIMessageDef.UI_UNHOLD);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_UpdateGoodsBag);
    }


    void AddOrMinusNum(GameObject go, object obj1, object obj2)
    {
        int Num = (int)obj1;
        if(SelectNum + Num <= 0)
        {
            SelectNum = 1;
        }
        else if(SelectNum + Num > tGoods.m_iNum)
        {
            SelectNum = tGoods.m_iNum;
        }
        else
            SelectNum += Num;

        ui_Num.text = SelectNum.ToString();
    }
}
