using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;

public class EveryDayHotSalePage : UIFramwork
{
    private UIWrapComponent _ItemParent;
    private int DayNum = 0;

    List<BasePoolDT<long>> EveryDayList;
    public UIWrapComponent ItemParent
    {
        get
        {
            if (_ItemParent == null)
            {
                _ItemParent = new UIWrapComponent(295, 2, 375, 2, f_GetObject("ItemParent"), f_GetObject("Item"), EveryDayList, _UpdateItem, null);
            }
            EveryDayList = Data_Pool.m_EveryDayHotSalePool.f_GetAllForData1(DayNum);
            _ItemParent.f_SetHide(true);
            return _ItemParent;
        }
    }
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("AlpheBg", _Btn_MaskClose);
        f_RegClickEvent("BtnClose", _Btn_MaskClose);
    }
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        DayNum = (int)e;
        int iTime1 = (Data_Pool.m_SevenActivityTaskPool.OpenSeverTime + 604800);
        System.DateTime t = ccMath.time_t2DateTime(iTime1);
        f_GetObject("EndTime").GetComponent<UILabel>().text = string.Format("[6CD83D]{0}/{1}/{2} 0:00", t.Day + 1, t.Month, t.Year);

        SocketCallbackDT tSocket = new SocketCallbackDT();
        tSocket.m_ccCallbackFail = _UpdateEverDay;
        tSocket.m_ccCallbackSuc = _UpdateEverDay;
        Data_Pool.m_EveryDayHotSalePool.f_EveryDayInfo(tSocket);

    }
    private void _UpdateEverDay(object obj)
    {
        if ((int)obj == (int)eMsgOperateResult.OR_Succeed)
        {
            ItemParent.f_UpdateList(EveryDayList);
            ItemParent.f_UpdateView(); ItemParent.f_ResetView();
        }
        else
        {
MessageBox.DEBUG("Initialization failed");
        }


    }


    #region 按钮事件
    private void _Btn_Buy(GameObject go, object obj1, object obj2)
    {
        EveryDayHotSalePoolDT tPoolDT = (EveryDayHotSalePoolDT)obj1;
        if (Data_Pool.m_UserData.f_GetProperty((int)tPoolDT.m_EveryDayHotSaleDT.iConsumeType) < tPoolDT.m_EveryDayHotSaleDT.iConsumeNum)
        {
UITool.Ui_Trip("Không đủ");
            return;
        }
        if (tPoolDT.m_EveryDayHotSaleDT.iBuyMax < tPoolDT.BuyTime)
        {
UITool.Ui_Trip("Đã đạt đến giới hạn");
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT tback = new SocketCallbackDT();
        tback.m_ccCallbackFail = BuyCallBack;
        tback.m_ccCallbackSuc = BuyCallBack;
        Data_Pool.m_AwardPool.m_GetLoginAward.Clear();
        Data_Pool.m_EveryDayHotSalePool.f_EveryDayBuy((short)(tPoolDT.ITempleteId), tback);
    }
    private void BuyCallBack(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)obj == (int)eMsgOperateResult.OR_Succeed)
        {
UITool.Ui_Trip("Mua hàng thành công");
            ItemParent.f_UpdateList(EveryDayList);
            ItemParent.f_UpdateView();
            List<AwardPoolDT> tGoods = Data_Pool.m_AwardPool.m_GetLoginAward;
            if (tGoods.Count >= 1)
                ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN, new object[] { tGoods });

        }
        else
        {
            // UITool.Ui_Trip("Mua thất bại" + obj.ToString());
UITool.Ui_Trip("mua thất bại");
        }
    }
    private void _Btn_MaskClose(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.EveryDayHotSalePage, UIMessageDef.UI_CLOSE);
    }

    #endregion

    private void _UpdateItem(Transform Item, BasePoolDT<long> EveryPoolDT)
    {
        Item.gameObject.SetActive(true);
        EveryDayHotSalePoolDT tPoolDT = EveryPoolDT as EveryDayHotSalePoolDT;
        Transform GoodsItem = Item.Find("Goods");
        //Transform Money = Item.Find("Money");
        UILabel MoneyNum = Item.Find("BuyBtn/MoneyNum").GetComponent<UILabel>();
        UILabel SurplusTime = Item.Find("SurplusTime").GetComponent<UILabel>();
        GameObject BuyBtn = Item.Find("BuyBtn").gameObject;
        BuyBtn.SetActive(tPoolDT.m_EveryDayHotSaleDT.iBuyMax > tPoolDT.BuyTime);
        GoodsItem.GetComponent<ResourceCommonItem>().f_UpdateByInfo(tPoolDT.m_EveryDayHotSaleDT.iGoodsType, tPoolDT.m_EveryDayHotSaleDT.iGoodsId, tPoolDT.m_EveryDayHotSaleDT.iGoodsNum);
        // Money.GetComponent<ResourceCommonItem>().f_UpdateByInfo((int)EM_ResourceType.Money, tPoolDT.m_EveryDayHotSaleDT.iConsumeType, tPoolDT.m_EveryDayHotSaleDT.iConsumeNum);
        SurplusTime.text = string.Format("{0}/{1}", tPoolDT.BuyTime, tPoolDT.m_EveryDayHotSaleDT.iBuyMax);
        MoneyNum.text = tPoolDT.m_EveryDayHotSaleDT.iConsumeNum.ToString();
        f_RegClickEvent(BuyBtn, _Btn_Buy, tPoolDT);
    }
}
