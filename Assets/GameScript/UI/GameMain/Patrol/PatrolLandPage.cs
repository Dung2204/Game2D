using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;
using System.Text;

public class PatrolLandPage : UIFramwork
{
    //Common
    private UILabel m_LandName;
    private UILabel m_LandName2;
    private UISprite m_LandIcon;
    private Transform m_RoleParent;
    private GameObject m_Role;
    //CanAttackPanel
    private GameObject m_CanAttackPanel;
    private UILabel m_LandDesc;
    private UILabel m_ModelName;
    private UILabel m_Power;
    private GameObject m_BtnChallenge;
    private UIGrid m_AttackAwardGrid;
    private GameObject m_AwardItem;
    private ResourceCommonItemComponent _attackAwardShow;
    private ResourceCommonItemComponent m_AttackAwardShow
    {
        get
        {
            if (_attackAwardShow == null)
                _attackAwardShow = new ResourceCommonItemComponent(m_AttackAwardGrid, m_AwardItem);
            return _attackAwardShow;
        }
    }
    //CanPatrolPanel
    private GameObject m_CanPatrolPanel;
    private GameObject m_CardParent;
    private GameObject m_NullCardParent;
    private GameObject m_CommonParent;

    private UILabel m_NullCardAwardTitle;
    private UIGrid m_NullCardAwardGrid;
    private ResourceCommonItemComponent _nullCardAwardShow;
    private ResourceCommonItemComponent m_NullCardAwardShow
    {
        get
        {
            if (_nullCardAwardShow == null)
                _nullCardAwardShow = new ResourceCommonItemComponent(m_NullCardAwardGrid, m_AwardItem);
            return _nullCardAwardShow;
        }
    }
    private UILabel m_NullOtherAwardTitle;
    private UIGrid m_NullOtherAwardGrid;
    private ResourceCommonItemComponent _nullOtherAwardShow;
    private ResourceCommonItemComponent m_NullOtherAwardShow
    {
        get
        {
            if (_nullOtherAwardShow == null)
                _nullOtherAwardShow = new ResourceCommonItemComponent(m_NullOtherAwardGrid, m_AwardItem);
            return _nullOtherAwardShow;
        }
    }

    private PatrolTypeTimeItem[] m_TypeTimeItems;
    private UILabel m_PatrolTypeLabel;
    private UILabel m_PatrolTimeLabel;
    private UIScrollView m_CanPatrolAwardScrollView;
    private UIGrid m_CanPatrolAwardGrid;
    private ResourceCommonItemComponent _canPatrolAwardShow;
    private ResourceCommonItemComponent m_CanPatrolAwardShow
    {
        get
        {
            if (_canPatrolAwardShow == null)
                _canPatrolAwardShow = new ResourceCommonItemComponent(m_CanPatrolAwardGrid, m_AwardItem);
            return _canPatrolAwardShow;
        }
    }
    private UILabel m_CanPatrolRoleDesc;
    private GameObject m_BtnSelectCard;
    private GameObject m_BtnChangeCard;
    private GameObject m_BtnSelectType;
    private GameObject m_BtnBeginPatrol;
    private UISprite m_PatrolCostIcon;
    private UILabel m_PatrolCostNum;

    //PatrolingAndGetAwardPanel
    private GameObject m_PatrolingPanel;
    private UILabel m_EventDesc;
    private UILabel m_EventCDTimeShow;
    private UILabel m_PatrolTimeShow;
    private UILabel m_PatrolFinishDesc;
    private GameObject m_BtnGetAward;
    private GameObject m_BtnGetAwardGray;
    private GameObject m_BtnPacifyRiot;
    private GameObject m_PatrolingTip;
    private GameObject m_GetAwardTip;
    private GameObject m_RiotingTip;
    private UIScrollView m_EventScrollView;
    private UIGrid m_PatrolingAwardGrid;
    private ResourceCommonItemComponent _patrolingAwardShow;
    private ResourceCommonItemComponent m_PatrolingAwardShow
    {
        get
        {
            if (_patrolingAwardShow == null)
                _patrolingAwardShow = new ResourceCommonItemComponent(m_PatrolingAwardGrid, m_AwardItem);
            return _patrolingAwardShow;
        }
    }

    private long userId;
    private PatrolLandNode data;
    private ccCallback callback_CloseLandPage;

    private List<string> selectpopuptype = new List<string>();
    private List<string> selectpopuptime = new List<string>();
    private UIPopupList mPopupListTime ;
    private UIPopupList mPopupListType;
    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        m_LandName = f_GetObject("LandName").GetComponent<UILabel>();
        m_LandName2 = f_GetObject("LandName2").GetComponent<UILabel>();
        m_LandIcon = f_GetObject("LandIcon").GetComponent<UISprite>();
        f_RegClickEvent("BtnClose", f_BtnClose);
        f_RegClickEvent("CloseMask", f_BtnClose);
        m_RoleParent = f_GetObject("RoleParent").transform;
        //CanAttackPanel
        m_CanAttackPanel = f_GetObject("CanAttackPanel");
        m_LandDesc = f_GetObject("LandDesc").GetComponent<UILabel>();
        m_ModelName = f_GetObject("ModelName").GetComponent<UILabel>();
        m_Power = f_GetObject("Power").GetComponent<UILabel>();
        m_BtnChallenge = f_GetObject("BtnChallenge");
        m_AttackAwardGrid = f_GetObject("AttackAwardGrid").GetComponent<UIGrid>();
        m_AwardItem = f_GetObject("PatrolLandItem");
        f_RegClickEvent(m_BtnChallenge, f_BtnChallenge);
        //CanPatrolPanel
        m_CanPatrolPanel = f_GetObject("CanPatrolPanel");
        m_CardParent = f_GetObject("CardParent");
        m_NullCardParent = f_GetObject("NullCardParent");
        m_NullCardAwardTitle = f_GetObject("NullCardAwardTitle").GetComponent<UILabel>();
        m_NullCardAwardGrid = f_GetObject("NullCardAwardGrid").GetComponent<UIGrid>();
        m_NullOtherAwardTitle = f_GetObject("NullOtherAwardTitle").GetComponent<UILabel>();
        m_NullOtherAwardGrid = f_GetObject("NullOtherAwardGrid").GetComponent<UIGrid>();
        m_CommonParent = f_GetObject("CommonPanel");
        m_TypeTimeItems = new PatrolTypeTimeItem[3];
        for (int i = 0; i < m_TypeTimeItems.Length; i++)
        {
            m_TypeTimeItems[i] = f_GetObject("TypeTimeItem" + i).GetComponent<PatrolTypeTimeItem>();
        }
        m_PatrolTypeLabel = f_GetObject("PatrolTypeLabel").GetComponent<UILabel>();
        m_PatrolTimeLabel = f_GetObject("PatrolTimeLabel").GetComponent<UILabel>();
        m_CanPatrolAwardScrollView = f_GetObject("CanPatrolAwardScrollView").GetComponent<UIScrollView>();
        m_CanPatrolAwardGrid = f_GetObject("CanPatrolAwardGrid").GetComponent<UIGrid>();
        m_CanPatrolRoleDesc = f_GetObject("CanPatrolRoleDesc").GetComponent<UILabel>();
        m_BtnSelectCard = f_GetObject("BtnSelectCard");
        m_BtnChangeCard = f_GetObject("BtnChangeCard");
        m_BtnSelectType = f_GetObject("BtnSelectType");
        m_BtnBeginPatrol = f_GetObject("BtnBeginPatrol");
        m_PatrolCostIcon = f_GetObject("PatrolCostIcon").GetComponent<UISprite>();
        m_PatrolCostNum = f_GetObject("PatrolCostNum").GetComponent<UILabel>();
        f_RegClickEvent(m_BtnSelectCard, f_BtnSelectCard);
        f_RegClickEvent(m_BtnChangeCard, f_BtnChangeCard);
        //f_RegClickEvent(m_BtnSelectType, f_BtnSelectType);
        mPopupListType = m_BtnSelectType.transform.GetComponent<UIPopupList>();
        mPopupListTime = f_GetObject("TypeParent").transform.GetComponent<UIPopupList>();
        EventDelegate.Add(mPopupListType.onChange, f_BtnSelectType);
        EventDelegate.Add(mPopupListTime.onChange, f_BtnSelectTime);

        f_RegClickEvent(m_BtnBeginPatrol, f_BtnBeginPatrol);
        //PatrolingPanel
        m_PatrolingPanel = f_GetObject("PatrolingPanel");
        m_EventDesc = f_GetObject("EventDesc").GetComponent<UILabel>();
        m_EventCDTimeShow = f_GetObject("EventCDTimeShow").GetComponent<UILabel>();
        m_PatrolTimeShow = f_GetObject("PatrolTimeShow").GetComponent<UILabel>();
        m_PatrolFinishDesc = f_GetObject("PatrolFinishDesc").GetComponent<UILabel>();
        m_BtnGetAward = f_GetObject("BtnGetAward");
        m_BtnGetAwardGray = f_GetObject("BtnGetAwardGray");
        m_BtnPacifyRiot = f_GetObject("BtnPacifyRiot");
        m_PatrolingTip = f_GetObject("PatrolingTip");
        m_GetAwardTip = f_GetObject("GetAwardTip");
        m_RiotingTip = f_GetObject("RiotingTip");
        m_EventScrollView = f_GetObject("EventScrollView").GetComponent<UIScrollView>();
        m_PatrolingAwardGrid = f_GetObject("PatrolingAwardGrid").GetComponent<UIGrid>();
        f_RegClickEvent(m_BtnGetAward, f_BtnGetAward);
        f_RegClickEvent(m_BtnPacifyRiot, f_BtnPacifyRiot);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (e == null || !(e is object[]))
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(878));
        object[] values = (object[])e;
        userId = (long)values[0];
        data = (PatrolLandNode)values[1];
        callback_CloseLandPage = (ccCallback)values[2];
        m_LandName2.text = m_LandName.text = data.m_Template.szName;
        m_LandIcon.spriteName = string.Format("Icon_Land{0:d2}", data.m_iTemplateId);
        m_LandIcon.MakePixelPerfect();
        f_UpdateCanAttackPanel(data.m_State == EM_PatrolState.CanAttack);
        f_UpdateCanPatrolPanel(data.m_State == EM_PatrolState.CanPatrol);
        f_UpdatePatrolingPanel(data.m_State == EM_PatrolState.Patroling || data.m_State == EM_PatrolState.GetAward);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_PATROLEVENT_UPDATE, f_ProcessPatrolEventMessage, this);
        f_LoadTexture();
        selectpopuptype.Clear();
selectpopuptype.Add("Tuần tra - [0DC623]Sơ[-]");
selectpopuptype.Add("Tuần tra - [C927DC]Trung[-]");
selectpopuptype.Add("Tuần tra - [DC3827]Cao[-]");
        mPopupListType.items = selectpopuptype;

    }
    private string strTexPatrolLandBgRoot = "UI/TextureRemove/Challenge/Texture_PatrolLandBg";
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        //加载背景图
        UITexture TexPatrolLandBg = f_GetObject("TexPatrolLandBg").GetComponent<UITexture>();
        if (TexPatrolLandBg.mainTexture == null)
        {
            Texture2D tTexPatrolLandBg = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexPatrolLandBgRoot);
            TexPatrolLandBg.mainTexture = tTexPatrolLandBg;
        }
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_PATROLEVENT_UPDATE, f_ProcessPatrolEventMessage, this);
        if (callback_CloseLandPage != null)
            callback_CloseLandPage(eMsgOperateResult.OR_Succeed);
    }

    #region CanAttackPanel

    private void f_UpdateCanAttackPanel(bool isOpen)
    {
        m_CanAttackPanel.SetActive(isOpen);
        if (!isOpen)
            return;
        RoleModelDT modeDt = (RoleModelDT)glo_Main.GetInstance().m_SC_Pool.m_RoleModelSC.f_GetSC(data.m_Template.iModelId);
        UITool.f_CreateRoleByModeId(data.m_Template.iModelId, ref m_Role, m_RoleParent, 10, false);
        m_ModelName.text = modeDt != null ? modeDt.szName : string.Empty;
        m_LandDesc.text = data.m_Template.szDesc;
        m_Power.text = string.Format(CommonTools.f_GetTransLanguage(879), data.m_Template.iPower);
        m_AttackAwardShow.f_Show(Data_Pool.m_AwardPool.f_GetAwardByString(data.m_Template.szPassAward));
    }

    private void f_BtnChallenge(GameObject go, object value1, object value2)
    {
        if (userId != Data_Pool.m_UserData.m_iUserId)
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(880));
            return;
        }
        else if (data.m_State != EM_PatrolState.CanAttack)
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(881));
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_PatrolChallenge;
        socketCallbackDt.m_ccCallbackFail = f_Callback_PatrolChallenge;
        Data_Pool.m_PatrolPool.f_PatrolChallenge(data.m_iTemplateId, socketCallbackDt);
    }

    private void f_Callback_PatrolChallenge(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            //加载战斗场景
            ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolPage, UIMessageDef.UI_CLOSE);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolLandPage, UIMessageDef.UI_CLOSE);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.BattleMain);
        }
        else
        {
            UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(882) + result);
        }
    }

    #endregion

    #region CanPatrolPanel
    private int __cardId;
    private int cardId
    {
        set
        {
            __cardId = value;
            if (__cardId != 0)
            {
                CardDT cardDt = (CardDT)glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(__cardId);
                if (cardDt != null)
                    cardName = UITool.f_GetImporentForName(cardDt.iImportant, cardDt.szName);
                else
                    cardName = string.Empty;
            }
            else
                cardName = string.Empty;
        }
        get
        {
            return __cardId;
        }
    }
    private string cardName;
    private int patrolId;

    private List<NBaseSCDT> patrolTypeList;
    private int patrolType;
    private int patrolTime;
    private PatrolTypeDT curPatrolTypeDt;
    private List<int> patrolingCardIdList = new List<int>();

    private void f_UpdateCanPatrolPanel(bool isOpen)
    {
        m_CanPatrolPanel.SetActive(isOpen);
        if (!isOpen)
            return;
        m_CommonParent.SetActive(isOpen);
        cardId = data.m_iCardId;
        f_UpdateCanPatrolNullCardParent(cardId == 0);
        f_UpdateCanPatrolCardParent(cardId != 0);
        //刷新在巡逻中的卡牌Id
        patrolingCardIdList.Clear();
        for (int i = 0; i < Data_Pool.m_PatrolPool.m_SelfPatrolDt.m_PatrolLands.Length; i++)
        {
            if (Data_Pool.m_PatrolPool.m_SelfPatrolDt.m_PatrolLands[i].m_iCardId == 0)
                continue;
            patrolingCardIdList.Add(Data_Pool.m_PatrolPool.m_SelfPatrolDt.m_PatrolLands[i].m_iCardId);
        }
    }

    private void f_UpdateCanPatrolCardParent(bool isOpen)
    {
        m_CardParent.SetActive(isOpen);
        m_CommonParent.SetActive(isOpen);
        if (!isOpen)
            return;
        patrolId = data.m_iPatrolTypeId;
        if (patrolId == 0)
        {
            patrolType = (int)EM_PatrolType.Low;
            patrolTypeList = Data_Pool.m_PatrolPool.f_GetPatrolTypeByType(patrolType);
            patrolTime = ((PatrolTypeDT)patrolTypeList[patrolTypeList.Count - 1]).iTime;
            f_UpdateByPatrolType(Data_Pool.m_PatrolPool.f_GetPatrolTypeByTypeAndTime(patrolType, patrolTime));
        }
        else
        {
            f_UpdateByPatrolType((PatrolTypeDT)glo_Main.GetInstance().m_SC_Pool.m_PatrolTypeSC.f_GetSC(patrolId));
        }
        UITool.f_CreateRoleByCardId(cardId, ref m_Role, m_RoleParent, 10, 1, false, false);
        m_CanPatrolRoleDesc.text = string.Format(CommonTools.f_GetTransLanguage(883), cardName);
    }

    private void f_UpdateByPatrolType(PatrolTypeDT typeDt)
    {
        curPatrolTypeDt = typeDt;
        m_PatrolTypeLabel.text = typeDt.szDes;
        patrolType = typeDt.iType;
        patrolTime = typeDt.iTime;
m_PatrolTimeLabel.text = string.Format("Tuần tra {0} giờ", patrolTime);
        patrolTypeList = Data_Pool.m_PatrolPool.f_GetPatrolTypeByType(typeDt.iType);

        selectpopuptime.Clear();     
        for (int i = 0; i < patrolTypeList.Count; i++)
        {
            //m_TypeTimeItems[i].f_UpdateInfo(patrolTime, (PatrolTypeDT)patrolTypeList[i]);
            //f_RegClickEvent(m_TypeTimeItems[i].ClickItem, f_PatrolTypeTimeClick, m_TypeTimeItems[i].info);
            PatrolTypeDT patrolTypeDT = (PatrolTypeDT)patrolTypeList[i];
            selectpopuptime.Add(patrolTypeDT.iTime.ToString());
        }
        mPopupListTime.items = selectpopuptime;

        m_PatrolCostIcon.spriteName = UITool.f_GetMoneySpriteName((EM_MoneyType)curPatrolTypeDt.iCoinId);
        m_PatrolCostNum.text = curPatrolTypeDt.iCoinNum.ToString();
        //更新奖励
        List<AwardPoolDT> tAwardList = Data_Pool.m_AwardPool.f_GetAwardByString(data.m_Template.szPatrolAwardShow);
        AwardPoolDT tAwardItem = tAwardList.Find(delegate (AwardPoolDT tItem)
        {
            return tItem.mTemplate.mResourceType == (int)EM_ResourceType.CardFragment
            && tItem.mTemplate.mResourceId == (int)cardId;
        }
        );
        if (tAwardItem != null)
            tAwardList.Remove(tAwardItem);
        else
            tAwardItem = new AwardPoolDT();
        tAwardItem.mTemplate.f_UpdateInfo((int)EM_ResourceType.CardFragment, cardId, curPatrolTypeDt.iMinFragment);
        tAwardList.Insert(0, tAwardItem);
        m_CanPatrolAwardShow.f_Show(tAwardList);
        m_CanPatrolAwardScrollView.ResetPosition();
    }

    private void f_UpdateCanPatrolNullCardParent(bool isOpen)
    {
        m_NullCardParent.SetActive(isOpen);
        m_CommonParent.SetActive(!isOpen);
        if (!isOpen)
            return;
        if (m_Role != null)
        {
            UITool.f_DestoryStatelObject(m_Role);
            m_Role = null;
        }
        m_NullCardAwardTitle.text = string.Format(CommonTools.f_GetTransLanguage(884), data.m_Template.szName);
        m_NullOtherAwardTitle.text = string.Format(CommonTools.f_GetTransLanguage(885), data.m_Template.szName);
        List<AwardPoolDT> tAwardList = Data_Pool.m_AwardPool.f_GetAwardByString(data.m_Template.szPatrolAwardShow);
        List<AwardPoolDT> tCardAwardList = new List<AwardPoolDT>();
        for (int i = 0; i < tAwardList.Count; i++)
        {
            if (tAwardList[i].mTemplate.mResourceType == (int)EM_ResourceType.Card)
            {
                tCardAwardList.Add(tAwardList[i]);
            }
        }
        tAwardList.RemoveAll(delegate (AwardPoolDT tItem)
        {
            return tItem.mTemplate.mResourceType == (int)EM_ResourceType.Card;
        }
        );
        m_NullCardAwardShow.f_Show(tCardAwardList);
        m_NullOtherAwardShow.f_Show(tAwardList);
    }

    /// <summary>
    /// 无卡牌上阵卡牌
    /// </summary>
    /// <param name="go"></param>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    private void f_BtnSelectCard(GameObject go, object value1, object value2)
    {
        ccCallback handle = f_Callback_SelectCard;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolSelectCardPage, UIMessageDef.UI_OPEN, new object[] { patrolingCardIdList, handle });
    }

    /// <summary>
    /// 有卡牌 改变卡牌
    /// </summary>
    /// <param name="go"></param>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    private void f_BtnChangeCard(GameObject go, object value1, object value2)
    {
        ccCallback handle = f_Callback_SelectCard;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolSelectCardPage, UIMessageDef.UI_OPEN, new object[] { patrolingCardIdList, handle });
    }

    private void f_Callback_SelectCard(object value)
    {
        int result = (int)value;
        if (result == 0) // 0代表没有选择卡牌
            return;
        //更新正在巡逻的卡牌ID
        patrolingCardIdList.Remove(cardId);
        patrolingCardIdList.Add(result);
        cardId = result;
        f_UpdateCanPatrolNullCardParent(cardId == 0);
        f_UpdateCanPatrolCardParent(cardId != 0);
    }

    private void f_PatrolTypeTimeClick(GameObject go, object value1, object value2)
    {
        PatrolTypeDT tNode = (PatrolTypeDT)value1;
        if (patrolTime == tNode.iTime)
            return;
        patrolTime = tNode.iTime;
        f_UpdateByPatrolType(Data_Pool.m_PatrolPool.f_GetPatrolTypeByTypeAndTime(patrolType, patrolTime));
    }

    private void f_BtnSelectType()
    {
        int idx = selectpopuptype.IndexOf(UIPopupList.current.value);
        f_ItemClick(idx+1);

        //Debug.Log()
        //ccCallback handle = f_Callback_SureType;
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolSelectTypePage, UIMessageDef.UI_OPEN);
    }

    private void f_BtnSelectTime()
    {
        int idx = selectpopuptype.IndexOf(UIPopupList.current.value);
m_PatrolTimeLabel.text = string.Format("Patrol {0} hours", UIPopupList.current.value) ;
        patrolTime = int.Parse(UIPopupList.current.value);
        f_UpdateByPatrolType(Data_Pool.m_PatrolPool.f_GetPatrolTypeByTypeAndTime(patrolType, patrolTime));
        //f_PatrolTypeTimeClick(idx + 1);
        //ccCallback handle = f_Callback_SureType;
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolSelectTypePage, UIMessageDef.UI_OPEN, handle);
    }

    private void f_Callback_SureType(object value)
    {
        int sureType = (int)value;
        if (patrolType == sureType)
            return;
        patrolType = sureType;
        patrolTypeList = Data_Pool.m_PatrolPool.f_GetPatrolTypeByType(patrolType);
        patrolTime = ((PatrolTypeDT)patrolTypeList[patrolTypeList.Count - 1]).iTime;
        f_UpdateByPatrolType(Data_Pool.m_PatrolPool.f_GetPatrolTypeByTypeAndTime(patrolType, patrolTime));
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

        f_Callback_SureType(tmpType);
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolSelectTypePage, UIMessageDef.UI_CLOSE);

    }

    private void f_BtnBeginPatrol(GameObject go, object value1, object value2)
    {
        int costNum = curPatrolTypeDt.iCoinNum;
        string coinName = UITool.f_GetGoodName(EM_ResourceType.Money, curPatrolTypeDt.iCoinId);
        if (userId != Data_Pool.m_UserData.m_iUserId)
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(886));
            return;
        }
        else if (!UITool.f_IsEnoughMoney((EM_MoneyType)curPatrolTypeDt.iCoinId, costNum, true, true, this))
        {
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_PatrolBegin;
        socketCallbackDt.m_ccCallbackFail = f_Callback_PatrolBegin;
        Data_Pool.m_PatrolPool.f_PatrolBegin(data.m_iTemplateId, cardId, curPatrolTypeDt.iId, socketCallbackDt);
    }

    private void f_Callback_PatrolBegin(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            //成功
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(887));
            f_UpdateCanPatrolPanel(data.m_State == EM_PatrolState.CanPatrol);
            f_UpdatePatrolingPanel(data.m_State == EM_PatrolState.Patroling || data.m_State == EM_PatrolState.GetAward);
        }
        else
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(888) + result);
        }
    }

    #endregion

    #region PatrolingPanel

    private float updateTime = 0f;
    private int timeGap = 0;
    private int timeHour;
    private int timeMin;
    private int timeSecond;
    private int eventTimeGap;
    private int eventTimeMin;
    private int eventTimeSecond;
    private List<AwardPoolDT> patrolingAwardList = new List<AwardPoolDT>();

    private void f_UpdatePatrolingPanel(bool isOpen)
    {
        m_PatrolingPanel.SetActive(isOpen);
        if (!isOpen)
            return;
        m_CommonParent.SetActive(isOpen);
        UITool.f_CreateRoleByCardId(data.m_iCardId, ref m_Role, m_RoleParent, 10, 1, false, false);
        f_UpdateBySecond();
        f_UpdateByPatrolEvent();
        m_EventScrollView.ResetPosition();
    }

    private void f_UpdateBySecond()
    {
        timeGap = data.m_iEndTime - GameSocket.GetInstance().f_GetServerTime();
        if (timeGap < 0)
        {
            m_EventCDTimeShow.text = string.Empty;
            m_PatrolTimeShow.text = string.Empty;
        }
        else
        {
            eventTimeGap = timeGap % (60 * data.m_PatrolTypeTemplate.iEventCD);
            eventTimeMin = eventTimeGap / 60;
            eventTimeSecond = eventTimeGap % 60;
            timeHour = timeGap / 60 / 60;
            timeMin = timeGap / 60 % 60;
            timeSecond = timeGap % 60;
            m_EventCDTimeShow.text = string.Format(CommonTools.f_GetTransLanguage(889), eventTimeMin, eventTimeSecond);
m_PatrolTimeShow.text = string.Format("Thời gian tuần tra còn lại :[32f62d]{0:d2}:{1:d2}:{2:d2}[-]", timeHour, timeMin, timeSecond);
        }
        m_BtnGetAward.SetActive(userId == Data_Pool.m_UserData.m_iUserId && data.m_State == EM_PatrolState.GetAward);
        m_BtnGetAwardGray.SetActive(userId == Data_Pool.m_UserData.m_iUserId && data.m_State != EM_PatrolState.GetAward);
        m_BtnPacifyRiot.SetActive(userId != Data_Pool.m_UserData.m_iUserId && data.m_State == EM_PatrolState.Patroling && data.m_bIsRiot);
    }

    private void f_UpdateByPatrolEvent()
    {
        patrolingAwardList.Clear();
        PatrolEventNode tNode;
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < data.m_EventList.Count; i++)
        {
            tNode = data.m_EventList[i];
            if (i == data.m_EventList.Count - 1)
                sb.Append(tNode.m_szEventDesc);
            else
            {
                sb.Append(tNode.m_szEventDesc);
                sb.Append("\n");
            }
            if (tNode.m_AwardDt == null)
                continue;
            bool isProcess = false;
            for (int j = 0; j < patrolingAwardList.Count; j++)
            {
                if (patrolingAwardList[j].mTemplate.mResourceType == tNode.m_AwardDt.mResourceType &&
                    patrolingAwardList[j].mTemplate.mResourceId == tNode.m_AwardDt.mResourceId)
                {
                    patrolingAwardList[j].mTemplate.f_AddNum(tNode.m_AwardDt.mResourceNum);
                    isProcess = true;
                    break;
                }
            }
            if (isProcess)
                continue;
            AwardPoolDT tAwardNode = new AwardPoolDT();
            tAwardNode.f_UpdateByInfo((byte)tNode.m_AwardDt.mResourceType, tNode.m_AwardDt.mResourceId, tNode.m_AwardDt.mResourceNum);
            patrolingAwardList.Add(tAwardNode);
        }
        m_EventDesc.text = sb.ToString();
        m_PatrolingAwardShow.f_Show(patrolingAwardList);
        m_PatrolingTip.SetActive(data.m_State == EM_PatrolState.Patroling);
        m_GetAwardTip.SetActive(data.m_State == EM_PatrolState.GetAward);
        //m_RiotingTip.SetActive(data.m_State == EM_PatrolState.Patroling && data.m_bIsRiot);
        if (data.m_State == EM_PatrolState.GetAward)
        {
            CardDT tCardDt = (CardDT)glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(data.m_iCardId);
            string tName = UITool.f_GetImporentForName(tCardDt.iImportant, tCardDt.szName);
            m_PatrolFinishDesc.text = string.Format(CommonTools.f_GetTransLanguage(890), tName);
        }
        else
        {
            m_PatrolFinishDesc.text = string.Empty;
        }
    }

    //处理巡逻事件更新信息
    private void f_ProcessPatrolEventMessage(object result)
    {
        f_UpdateByPatrolEvent();
    }

    private void f_BtnGetAward(GameObject go, object value1, object value2)
    {
        if (userId != Data_Pool.m_UserData.m_iUserId)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(891));
            return;
        }
        else if (data.m_State != EM_PatrolState.GetAward)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(892));
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_PatrolGetAward;
        socketCallbackDt.m_ccCallbackFail = f_Callback_PatrolGetAward;
        Data_Pool.m_PatrolPool.f_PatrolAward(data.m_iTemplateId, socketCallbackDt);
    }

    private void f_Callback_PatrolGetAward(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN, new object[] { patrolingAwardList });
            f_UpdateCanPatrolPanel(data.m_State == EM_PatrolState.CanPatrol);
            f_UpdatePatrolingPanel(data.m_State == EM_PatrolState.Patroling || data.m_State == EM_PatrolState.GetAward);
        }
        else
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(893) + result);
        }
    }

    private void f_BtnPacifyRiot(GameObject go, object value1, object value2)
    {
        if (userId == Data_Pool.m_UserData.m_iUserId)
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(894));
            return;
        }
        else if (data.m_State != EM_PatrolState.Patroling)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(895));
            return;
        }
        else if (Data_Pool.m_PatrolPool.m_iPacifyTimes >= Data_Pool.m_PatrolPool.m_iPacifyTimesLimit)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(896));
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_PatrolPacify;
        socketCallbackDt.m_ccCallbackFail = f_Callback_PatrolPacify;
        Data_Pool.m_PatrolPool.f_PatrolPacify(userId, data.m_iTemplateId, socketCallbackDt);
    }

    private void f_Callback_PatrolPacify(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolPacifyAwardPage, UIMessageDef.UI_OPEN);
        }
        else
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(897));
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(898) + result);
        }
    }

    #endregion


    private void f_BtnClose(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolLandPage, UIMessageDef.UI_CLOSE);
    }

    protected override void f_Update()
    {
        base.f_Update();
        if (data != null && (data.m_State == EM_PatrolState.Patroling || data.m_State == EM_PatrolState.GetAward))
        {
            updateTime += Time.deltaTime;
            if (updateTime > 0.5f)
            {
                updateTime = 0f;
                f_UpdateBySecond();
            }
        }
    }
}
