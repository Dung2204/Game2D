using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class PatrolOnekeyPage : UIFramwork
{
    private UIScrollView m_OnekeyScrollView;
    private GameObject m_OnekeyItemParent;
    private GameObject m_OnekeyItem; 
    private GameObject m_CostItemParent;
    private GameObject m_CostItem; 
    
    private List<int> patrolingCardIdList = new List<int>();
    private List<PatrolOnekeyData> onekeyDatas = new List<PatrolOnekeyData>();
    private List<ResourceCommonDT> costList = new List<ResourceCommonDT>();
    private PatrolOnekeyData curData;
    private ccCallback callback_PatorlOnekey;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }
    protected override void InitGUI()
    {
        base.InitGUI();
        m_OnekeyItemParent = f_GetObject("OnekeyItemParent");
        m_OnekeyItem = f_GetObject("OnekeyItem");
        m_OnekeyScrollView = f_GetObject("OnekeyScrollView").GetComponent<UIScrollView>();
        m_CostItemParent = f_GetObject("CostItemParent");
        m_CostItem = f_GetObject("CostItem");
        f_RegClickEvent("BtnClose", f_BtnClose);
        f_RegClickEvent("CloseMask", f_BtnClose);
        f_RegClickEvent("BtnBeginPatrol", f_BtnBeginOnekeyPatrol);

    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        callback_PatorlOnekey = (ccCallback)e;
        patrolingCardIdList.Clear();
        onekeyDatas.Clear();
        costList.Clear();
        PatrolLandNode[] tInitArr = Data_Pool.m_PatrolPool.m_SelfPatrolDt.m_PatrolLands;
        for (int i = 0; i < tInitArr.Length; i++)
        {
            if (tInitArr[i].m_iCardId != 0)
                patrolingCardIdList.Add(tInitArr[i].m_iCardId);
            if (tInitArr[i].m_State == EM_PatrolState.CanPatrol)
            {
                PatrolOnekeyData tNode = new PatrolOnekeyData(tInitArr[i].m_Template);
                tNode.f_UpdateCardId(tInitArr[i].m_iCardId);
                if (tInitArr[i].m_iPatrolTypeId == 0)
                {
                    List<NBaseSCDT> tList = Data_Pool.m_PatrolPool.f_GetPatrolTypeByType((int)EM_PatrolType.Low);
                    PatrolTypeDT tPatrolDt = (PatrolTypeDT)tList[tList.Count - 1];
                    tNode.f_UpdateTypeAndTime((int)EM_PatrolType.Low, tPatrolDt.iTime);
                    f_UpdateCostList(true, tNode.m_PatrolTypeDT.iCoinId, tNode.m_PatrolTypeDT.iCoinNum);
                }
                else
                {
                    tNode.f_UpdateTypeAndTime(tInitArr[i].m_PatrolTypeTemplate.iType, tInitArr[i].m_PatrolTypeTemplate.iTime);
                    f_UpdateCostList(true, tNode.m_PatrolTypeDT.iCoinId, tNode.m_PatrolTypeDT.iCoinNum);
                }   
                onekeyDatas.Add(tNode);
            }
        }
        f_UpdateByOnekeyDatas(true);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_UPDATE_USERINFOR, f_OnUpdateUserInfo, this);
        
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_UPDATE_USERINFOR, f_OnUpdateUserInfo, this);
    }

    private void f_UpdateCostList(bool isAdd, int costId, int costNum)
    {
        if (isAdd)
        {
            for (int i = 0; i < costList.Count; i++)
            {
                if (costList[i].mResourceId == costId)
                {
                    costList[i].f_AddNum(costNum);
                    return;
                }
            }
            ResourceCommonDT tNode = new ResourceCommonDT();
            tNode.f_UpdateInfo((byte)EM_ResourceType.Money, costId, costNum);
            costList.Add(tNode);
        }
        else
        {
            for (int i = 0; i < costList.Count; i++)
            {
                if (costList[i].mResourceId == costId)
                {
                    costList[i].f_SubtractNum(costNum);
                    return;
                }
            }
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(916));
        }
    }

    private void f_UpdateByOnekeyDatas(bool resetPos)
    {
        GridUtil.f_SetGridView<PatrolOnekeyData>(resetPos, m_OnekeyItemParent, m_OnekeyItem, onekeyDatas, f_UpdateOnekeyItem);
        //更新花费
        GridUtil.f_SetGridView<ResourceCommonDT>(true,m_CostItemParent,m_CostItem,costList,f_UpdateCostItem);
        m_OnekeyScrollView.ResetPosition();
    }

    private void f_UpdateCostItem(GameObject go, ResourceCommonDT Dt)
    {
        int haveNum = Data_Pool.m_UserData.f_GetProperty(Dt.mResourceId);
        UILabel tTitle = go.transform.GetChild(0).GetComponent<UILabel>();
        UISprite tIcon = go.transform.GetChild(1).GetComponent<UISprite>();
        UILabel tNum = go.transform.GetChild(2).GetComponent<UILabel>();
        tTitle.text = string.Format(CommonTools.f_GetTransLanguage(917), Dt.mName);
        tIcon.spriteName = UITool.f_GetMoneySpriteName((EM_MoneyType)Dt.mResourceId);
        tNum.text = haveNum < Dt.mResourceNum? string.Format("[ff0000]{0}[-]", Dt.mResourceNum) : Dt.mResourceNum.ToString();
    }

    private void f_UpdateOnekeyItem(GameObject go, PatrolOnekeyData Dt)
    {
        PatrolOnekeyItem tItem = go.GetComponent<PatrolOnekeyItem>();
        tItem.f_UpdateByInfo(Dt, this);
        f_RegClickEvent(tItem.m_BtnDown, f_BtnPatrolCardDown,Dt);
        f_RegClickEvent(tItem.m_BtnSelect, f_BtnSelectCard,Dt);
        f_RegClickEvent(tItem.m_BtnSelectType, f_BtnSelectType,Dt);
        f_RegClickEvent(tItem.m_BtnSelectTime, f_BtnSelectTime,Dt);
    }
    private void f_BtnSelectCard(GameObject go, object value1, object value2)
    {
        PatrolOnekeyData tData = (PatrolOnekeyData)value1;
        curData = tData;
        ccCallback handle = f_Callback_SelectCard;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolSelectCardPage, UIMessageDef.UI_OPEN,new object[] {patrolingCardIdList,handle});
    }

    private void f_Callback_SelectCard(object result)
    {
        int tCardId = (int)result;
        if (curData.m_iCardId == tCardId)
            return;
        if (curData.m_iCardId != 0)
            patrolingCardIdList.Remove(curData.m_iCardId);
        curData.f_UpdateCardId(tCardId);
        patrolingCardIdList.Add(tCardId);
        f_UpdateByOnekeyDatas(false);
    }

    private void f_BtnPatrolCardDown(GameObject go,object value1,object value2)
    {
        PatrolOnekeyData tData = (PatrolOnekeyData)value1;
        if(tData.m_iCardId != 0)
            patrolingCardIdList.Remove(tData.m_iCardId);
        tData.f_UpdateCardId(0);
        f_UpdateByOnekeyDatas(false);
    }

    private void f_BtnSelectType(GameObject go, object value1, object value2)
    {
        PatrolOnekeyData tData = (PatrolOnekeyData)value1;
        curData = tData;
        go.transform.parent.Find("Drop-down List").localPosition = new Vector3(-140, 100);
        //ccCallback handle = f_Callback_SelectType;
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolSelectTypePage, UIMessageDef.UI_OPEN, handle);

        //int idx = selectpopuptype.IndexOf(UIPopupList.current.value);
        //f_ItemClick(idx + 1);

    }

    public void f_BtnSelectType(int type, PatrolOnekeyData patrolOnekeyData)
    {
        curData = patrolOnekeyData;
        f_ItemClick(type);
    }

    private void f_ItemClick(int tmpType)
    {
        int vipLv = UITool.f_GetNowVipLv();
        int needVipLv = ((PatrolTypeDT)Data_Pool.m_PatrolPool.f_GetPatrolTypeByType(tmpType)[0]).iNeedVip;
        if (vipLv < needVipLv)
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(936), needVipLv));
            return;
        }

        f_Callback_SelectType(tmpType);
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolSelectTypePage, UIMessageDef.UI_CLOSE);

    }


    private void f_Callback_SelectType(object result)
    {
        int tType = (int)result;
        if (curData == null)
            return;
        if (curData.m_iPatrolType == tType)
            return;
        f_UpdateCostList(false, curData.m_PatrolTypeDT.iCoinId, curData.m_PatrolTypeDT.iCoinNum);
        curData.f_UpdateTypeAndTime(tType, curData.m_iPatrolTime);
        f_UpdateCostList(true, curData.m_PatrolTypeDT.iCoinId, curData.m_PatrolTypeDT.iCoinNum); 
        f_UpdateByOnekeyDatas(false);
    }

    private void f_BtnSelectTime(GameObject go, object value1, object value2)
    {
        PatrolOnekeyData tData = (PatrolOnekeyData)value1;
        curData = tData;
        go.transform.parent.Find("Drop-down List").localPosition = new Vector3(-140,100);
        //ccCallback handle = f_Callback_SelectTime;
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolSelectTimePage, UIMessageDef.UI_OPEN,new object[] { tData.m_iPatrolType,handle } );
    }

    public void f_BtnSelectTime(int time, PatrolOnekeyData patrolOnekeyData )
    {
        curData = patrolOnekeyData;
        f_Callback_SelectTime(time);
    }

    private void f_Callback_SelectTime(object result)
    {
        int tTime = (int)result;
        if (curData == null)
            return;
        if (curData.m_iPatrolTime == tTime)
            return;
        f_UpdateCostList(false, curData.m_PatrolTypeDT.iCoinId, curData.m_PatrolTypeDT.iCoinNum);
        curData.f_UpdateTypeAndTime(curData.m_iPatrolType, tTime);
        f_UpdateCostList(true, curData.m_PatrolTypeDT.iCoinId, curData.m_PatrolTypeDT.iCoinNum);
        f_UpdateByOnekeyDatas(false);
    }

    private int beginPatrolIdx = 0;
    private void f_BtnBeginOnekeyPatrol(GameObject go, object value1, object value2)
    {
        if (onekeyDatas.Count <= 0)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolOnekeyPage, UIMessageDef.UI_CLOSE);
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(918));
            return;
        }
        for (int i = 0; i < onekeyDatas.Count; i++)
        {
            if (onekeyDatas[i].m_iCardId == 0)
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(919));
                return;
            }
        }
        for (int i = 0; i < costList.Count; i++)
        {
            if (!UITool.f_IsEnoughMoney((EM_MoneyType)costList[i].mResourceId,costList[i].mResourceNum, true, true, this))
            {
                return;
            }
        }
        System.Text.StringBuilder tSb = new System.Text.StringBuilder();
        tSb.Append(CommonTools.f_GetTransLanguage(920));
        for (int i = 0; i < costList.Count; i++)
        {
            if (i == 0)
                tSb.AppendFormat(" {0} X{1} ", costList[i].mName, costList[i].mResourceNum);
            else
tSb.AppendFormat("With {0} X{1} ", costList[i].mName, costList[i].mResourceNum);
        }
        tSb.Append(CommonTools.f_GetTransLanguage(921));
        PopupMenuParams tParam = new PopupMenuParams(CommonTools.f_GetTransLanguage(922), tSb.ToString(), CommonTools.f_GetTransLanguage(923), f_SureBeginOnekeyPatrol, CommonTools.f_GetTransLanguage(924));
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParam);
    }

    private void f_SureBeginOnekeyPatrol(object result)
    {
        UITool.f_OpenOrCloseWaitTip(true);
        beginPatrolIdx = 0;
        f_SendPatrolBeginNode(onekeyDatas[beginPatrolIdx]);
    }

    private void f_Callback_BeginPatrol(object result)
    {
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            beginPatrolIdx++;
            if (beginPatrolIdx < onekeyDatas.Count)
            {
                ccTimeEvent.GetInstance().f_RegEvent(0.01f, false, onekeyDatas[beginPatrolIdx], f_DelaySendPatrolBeginNode);
            }
            else
            {
                UITool.f_OpenOrCloseWaitTip(false);
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(925));
                ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolOnekeyPage, UIMessageDef.UI_CLOSE);
                if (callback_PatorlOnekey != null)
                    callback_PatorlOnekey(eMsgOperateResult.OR_Succeed);
            }
        }
        else
        {
            UITool.f_OpenOrCloseWaitTip(false);
            UITool.UI_ShowFailContent(string.Format(CommonTools.f_GetTransLanguage(926), onekeyDatas[beginPatrolIdx].m_PatrolLandDT.iId, result));
            ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolOnekeyPage, UIMessageDef.UI_CLOSE);
            if (callback_PatorlOnekey != null)
                callback_PatorlOnekey(eMsgOperateResult.OR_Fail);
        }
    }

    private void f_SendPatrolBeginNode(PatrolOnekeyData node)
    {
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_BeginPatrol;
        socketCallbackDt.m_ccCallbackFail = f_Callback_BeginPatrol;
        Data_Pool.m_PatrolPool.f_PatrolBegin(node.m_PatrolLandDT.iId, node.m_iCardId, node.m_PatrolTypeDT.iId,socketCallbackDt);
    }

    private void f_DelaySendPatrolBeginNode(object value)
    {
        PatrolOnekeyData node = (PatrolOnekeyData)value;
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_BeginPatrol;
        socketCallbackDt.m_ccCallbackFail = f_Callback_BeginPatrol;
        Data_Pool.m_PatrolPool.f_PatrolBegin(node.m_PatrolLandDT.iId, node.m_iCardId, node.m_PatrolTypeDT.iId, socketCallbackDt);
    }

    private void f_BtnClose(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolOnekeyPage, UIMessageDef.UI_CLOSE);
    }

    private void f_OnUpdateUserInfo(object value)
    {
        //更新花费
        GridUtil.f_SetGridView<ResourceCommonDT>(true, m_CostItemParent, m_CostItem, costList, f_UpdateCostItem);
    }

}

public class PatrolOnekeyData
{
    public PatrolOnekeyData(PatrolLandDT landDt)
    {
        patrolLandDt = landDt;
        cardId = 0;
        cardDt = null;
        patrolType = 0;
        patrolTime = 0;
        patrolTypeDt = null;
    }

    public void f_UpdateCardId(int cardId)
    {
        this.cardId = cardId;
        if (cardId == 0)
            cardDt = null;
        else
            cardDt = (CardDT)glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(cardId);
    }

    public void f_UpdateTypeAndTime(int type, int time)
    {
        patrolType = type;
        patrolTime = time;
        patrolTypeDt = Data_Pool.m_PatrolPool.f_GetPatrolTypeByTypeAndTime(type, time);
    }

    private PatrolLandDT patrolLandDt;
    public PatrolLandDT m_PatrolLandDT
    {
        get
        {
            return patrolLandDt;
        }
    }

    private int patrolType;
    public int m_iPatrolType
    {
        get
        {
            return patrolType;
        }
    }
    private int patrolTime;
    public int m_iPatrolTime
    {
        get
        {
            return patrolTime;
        }
    }

    private int cardId;
    public int m_iCardId
    {
        get
        {
            return cardId;
        }
    }

    private CardDT cardDt;
    public CardDT m_CardDT
    {
        get
        {
            return cardDt;
        }
    }

    private PatrolTypeDT patrolTypeDt;
    public PatrolTypeDT m_PatrolTypeDT
    {
        get
        {
            return patrolTypeDt;
        }
    }
}
