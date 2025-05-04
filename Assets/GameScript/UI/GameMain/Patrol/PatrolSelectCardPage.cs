using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class PatrolSelectCardPage : UIFramwork
{
    private GameObject m_CardParent;
    private GameObject m_CardItem;

    private UIWrapComponent _cardWrapComponent;
    private List<int> patrolingCardId;
    private ccCallback callback_SelectCard;
    private List<NBaseSCDT> cardList;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        m_CardParent = f_GetObject("CardParent");
        m_CardItem = f_GetObject("CardItem");
        f_RegClickEvent("BtnClose", f_BntClost);
        f_RegClickEvent("CloseMask", f_BntClost);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        object[] values = (object[])e;
        patrolingCardId = (List<int>)values[0];
        callback_SelectCard = (ccCallback)values[1];

        cardList = CommonTools.f_CopySCDTArray(glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetAll());
        //移除主角卡牌和玩家没有拥有的卡牌
        cardList.RemoveAll(delegate (NBaseSCDT tNode)
        {
            CardDT tItem = (CardDT)tNode;
            return tItem.iCardType == (int)EM_CardType.RoleCard || !Data_Pool.m_CardPool.f_CheckHaveForData1(tItem.iId);
        });
        //排序 巡逻状态（未巡逻->已巡逻） > (已上阵的 -> 未上阵)  > 品质(高->低) > cardId(小->大)
        cardList.Sort(delegate (NBaseSCDT tNode1, NBaseSCDT tNode2)
        {
            CardDT tItem1 = (CardDT)tNode1;
            CardDT tItem2 = (CardDT)tNode2;
            if (f_IsPatroingCardId(tItem1.iId) && !f_IsPatroingCardId(tItem2.iId))
                return 1;
            else if (!f_IsPatroingCardId(tItem1.iId) && f_IsPatroingCardId(tItem2.iId))
                return -1;
            if (!Data_Pool.m_TeamPool.f_CheckInTeamByKeyId(tItem1.iId) && Data_Pool.m_TeamPool.f_CheckInTeamByKeyId(tItem2.iId))
                return 1;
            else if (Data_Pool.m_TeamPool.f_CheckInTeamByKeyId(tItem1.iId) && !Data_Pool.m_TeamPool.f_CheckInTeamByKeyId(tItem2.iId))
                return -1;

            int tImportant = tItem2.iImportant.CompareTo(tItem1.iImportant);
            if (tImportant != 0)
                return tImportant;
            return tItem1.iId.CompareTo(tItem2.iId);
        });

        if (_cardWrapComponent == null)
        {
            _cardWrapComponent = new UIWrapComponent(180, 1,800, 8, m_CardParent, m_CardItem, cardList, f_UpdateCardItem, null);
        }
        _cardWrapComponent.f_UpdateList(cardList);
        _cardWrapComponent.f_UpdateView();
        _cardWrapComponent.f_ResetView();
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    private void f_BntClost(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolSelectCardPage, UIMessageDef.UI_CLOSE);
    }

    private bool f_IsPatroingCardId(int cardId)
    {
        for (int i = 0; i < patrolingCardId.Count; i++)
        {
            if (patrolingCardId[i] == cardId)
                return true;
        }
        return false;
    }

    private void f_UpdateCardItem(Transform tf, NBaseSCDT Dt)
    {
        PatrolSelectCardItem tItem = tf.GetComponent<PatrolSelectCardItem>();
        tItem.f_UpdateByInfo((CardDT)Dt, f_IsPatroingCardId(Dt.iId));
        f_RegClickEvent(tItem.m_BtnSelect, f_BtnSelectBtn,Dt);
    }

    private void f_BtnSelectBtn(GameObject go, object value1, object value2)
    {
        NBaseSCDT Dt = (NBaseSCDT)value1;
        int selectCardId = Dt.iId;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolSelectCardPage, UIMessageDef.UI_CLOSE);
        if (callback_SelectCard != null)
            callback_SelectCard(selectCardId);
    }
}
