using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class PatrolPage : UIFramwork
{
    private UIGrid m_ItemGrid;
    private GameObject m_ItemSource;

    //Btn
    private GameObject m_BtnSkill;
    private GameObject m_BtnGetAwardOnekey;
    private GameObject m_BtnPatrolOnekey;
    private GameObject m_BtnVisitFriend;
    private GameObject m_BtnReturnSelf;
    private UILabel m_PacifyRiotTimes;
	//My Code
	private List<BasePoolDT<long>> _friendList;
	bool haveFriend = false;
	//

    private PatrolItem[] patrolItems;
    private PatrolPoolDT data;
    const int EventInitId = -99;
    private int eventId = EventInitId;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        m_BtnSkill = f_GetObject("BtnSkill");
        m_BtnGetAwardOnekey = f_GetObject("BtnGetAwardOnekey");
        m_BtnPatrolOnekey = f_GetObject("BtnPatrolOnekey");
        m_BtnVisitFriend = f_GetObject("BtnVisitFriend");
        m_BtnReturnSelf = f_GetObject("BtnReturnSelf");
        f_RegClickEvent(m_BtnSkill, f_BtnSkill);
        f_RegClickEvent(m_BtnGetAwardOnekey, f_BtnGetAwardOnekey);
        f_RegClickEvent(m_BtnPatrolOnekey, f_BtnPatrolOnekey);
        f_RegClickEvent(m_BtnVisitFriend, f_BtnVisitFriend);
        f_RegClickEvent(m_BtnReturnSelf, f_BtnReturnSelf);
        f_RegClickEvent("BtnReturn", f_BtnReturn);
		f_RegClickEvent("Btn_Help", f_OnHelpBtnClick);
        m_PacifyRiotTimes = f_GetObject("PacifyRiotTimes").GetComponent<UILabel>();
        patrolItems = new PatrolItem[6];
        for (int i = 0; i < patrolItems.Length; i++)
        {
            patrolItems[i] = f_GetObject("PatrolItem" + i).GetComponent<PatrolItem>();
        }
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);

        //断线重连如果切去其他界面，这时候不处理，服务器消息返回再打开界面就会和其他界面重叠
        if (!UITool.f_IsCanOpenChallengePage(UINameConst.PatrolPage))
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolPage, UIMessageDef.UI_CLOSE);
            return;
        }

        if (e == null)
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(868));
        if (patrolItems.Length != Data_Pool.m_PatrolPool.m_iLandTotalNum)
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(869));
            ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolPage, UIMessageDef.UI_CLOSE);
            return;
        }
        PatrolPoolDT tData = null;
        if (e is PatrolPoolDT)
            tData = (PatrolPoolDT)e;
        else if (e is object[])
        {
            object[] tParamArr = (object[])e;
            tData = (PatrolPoolDT)tParamArr[0];
            int isOpenFriendPage = (int)tParamArr[1];
            if (isOpenFriendPage > 0)
                f_BtnVisitFriend(null, null, null);
        }
        else
        {
            tData = Data_Pool.m_PatrolPool.m_SelfPatrolDt;
            if (e is Battle2MenuProcessParam)
            {
                Battle2MenuProcessParam tParam = (Battle2MenuProcessParam)e;
                if (tParam.m_emType == EM_Battle2MenuProcess.Patrol)
                {
                    f_PatrolUpdateBySelfRequest();
                    tParam.f_UpdateParam(EM_Battle2MenuProcess.None);
                }
            }
        }
        f_OpenOrCloseMoneyPage(true);
        f_UpdateByData(tData); 
        if (eventId != EventInitId)
        {
            ccTimeEvent.GetInstance().f_UnRegEvent(eventId);
            eventId = EventInitId;
        }   
        eventId = ccTimeEvent.GetInstance().f_RegEvent(1f, true, null,f_UpdateBySecond);
        f_LoadTexture();
    }
    private string strTexBgRoot = "UI/TextureRemove/Challenge/Texture_PatrolBg";
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        //加载背景图
        UITexture Texture_BG = f_GetObject("Texture_BG").GetComponent<UITexture>();
        if (Texture_BG.mainTexture == null)
        {
            Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot);
            Texture_BG.mainTexture = tTexture2D;
        }
		
		//My Code
		_friendList = Data_Pool.m_FriendPool.f_GetDataListByType(EM_FriendListType.Friend);
		if(_friendList.Count > 0)
		{
			haveFriend = true;
		}
		//
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        f_OpenOrCloseMoneyPage(false);
        if (eventId != EventInitId)
        {
            ccTimeEvent.GetInstance().f_UnRegEvent(eventId);
            eventId = EventInitId;
        }
    }

    protected override void UI_HOLD(object e)
    {
        base.UI_HOLD(e);
    }

    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        f_UpdateByData(data);
    }

    private void f_UpdateByData(PatrolPoolDT data)
    {
        this.data = data;
        bool tHaveAward = false;
        bool tHaveCanPatrol = false;
        for (int i = 0; i < patrolItems.Length; i++)
        {
            patrolItems[i].f_UpdateByInfo(data.m_PatrolLands[i]);
            f_RegClickEvent(patrolItems[i].m_ClickItem, f_PatrolItemClick, data.m_PatrolLands[i]);
            if (data.m_PatrolLands[i].m_State == EM_PatrolState.GetAward)
                tHaveAward = true;
            if (data.m_PatrolLands[i].m_State == EM_PatrolState.CanPatrol)
                tHaveCanPatrol = true;
        }
        m_BtnSkill.SetActive(data.m_bIsSelf);
        m_BtnGetAwardOnekey.SetActive(data.m_bIsSelf && tHaveAward);
        m_BtnPatrolOnekey.SetActive(data.m_bIsSelf && !tHaveAward && tHaveCanPatrol);
        m_BtnReturnSelf.SetActive(!data.m_bIsSelf);
        m_PacifyRiotTimes.text = string.Format(CommonTools.f_GetTransLanguage(870), Data_Pool.m_PatrolPool.m_iPacifyTimesLimit - Data_Pool.m_PatrolPool.m_iPacifyTimes);
    }

    int serverTime = 0;
    int beginTime = 0;
    private void f_UpdateBySecond(object value)
    {
        serverTime = GameSocket.GetInstance().f_GetServerTime();
        for (int i = 0; i < patrolItems.Length; i++)
        {
            if (patrolItems[i].m_Info.f_PatrolingCheckTime(data.iId,serverTime, f_Callback_PatrolEvent))
                patrolItems[i].f_UpdateByInfo(patrolItems[i].m_Info);
            patrolItems[i].f_UpdateTime(patrolItems[i].m_Info.m_iEndTime - serverTime);
        }
		//My Code
		_friendList = Data_Pool.m_FriendPool.f_GetDataListByType(EM_FriendListType.Friend);
		if(_friendList.Count > 0)
		{
			haveFriend = true;
		}
		//
    }

    private void f_Callback_PatrolEvent(object result)
    {
        for (int i = 0; i < patrolItems.Length; i++)
        {
            patrolItems[i].f_UpdateByInfo(patrolItems[i].m_Info);
        }
    }

    private void f_PatrolItemClick(GameObject go, object value1, object value2)
    {
        PatrolLandNode node = (PatrolLandNode)value1;
        tmpNode = node;
        if (data.m_bIsSelf)
        {
            if (node.m_State == EM_PatrolState.Lock)
            {
                UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(871), node.m_szUnlockLandName));
                return;
            }
            else if(node.m_State == EM_PatrolState.CanAttack || node.m_State == EM_PatrolState.CanPatrol)
            {
                //打开领地界面
                ccCallback handle = f_Callback_CloseLandPage;
                ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolLandPage, UIMessageDef.UI_OPEN, new object[] { data.iId, tmpNode, handle });
                return;
            }
            UITool.f_OpenOrCloseWaitTip(true);
            Data_Pool.m_PatrolPool.f_RequestEventByServer(data.iId, node.m_iTemplateId,(byte)node.m_EventList.Count, f_Callback_OpenLandPage); 
        }
        else
        {
            if (node.m_State != EM_PatrolState.Patroling)
            {
                UITool.Ui_Trip(CommonTools.f_GetTransLanguage(872));
                return;
            }
            UITool.f_OpenOrCloseWaitTip(true);
            Data_Pool.m_PatrolPool.f_RequestEventByServer(data.iId, node.m_iTemplateId, (byte)node.m_EventList.Count, f_Callback_OpenLandPage);
        }
    }

    private PatrolLandNode tmpNode;
    private void f_Callback_OpenLandPage(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        ccCallback handle = f_Callback_CloseLandPage;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolLandPage, UIMessageDef.UI_OPEN, new object[] { data.iId, tmpNode ,handle});
    }

    private void f_Callback_CloseLandPage(object result)
    {
        f_UpdateByData(data);
    }

    private void f_BtnSkill(GameObject go, object value1, object value2)
    {
        ccCallback handle = f_Callback_PatrolSkillPage;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolSkillPage, UIMessageDef.UI_OPEN,handle);
    }


    private List<PatrolLandNode> onekeyAwardList = new List<PatrolLandNode>();
    private int onekeyAwardIdx = 0;
    private void f_BtnGetAwardOnekey(GameObject go, object value1, object value2)
    {
        onekeyAwardList.Clear();
        PatrolLandNode[] tLands = Data_Pool.m_PatrolPool.m_SelfPatrolDt.m_PatrolLands;
        for (int i = 0; i < tLands.Length; i++)
        {
            if (tLands[i].m_State == EM_PatrolState.GetAward)
            {
                onekeyAwardList.Add(tLands[i]);
            }
        }
        if (onekeyAwardList.Count <= 0)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(873));
            return;
        }
        onekeyAwardIdx = 0;
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_PatrolEventEx;
        socketCallbackDt.m_ccCallbackFail = f_Callback_PatrolEventEx;
        byte[] eventNums = new byte[data.m_PatrolLands.Length];
        for (int i = 0; i < eventNums.Length; i++)
        {
            eventNums[i] = (byte)data.m_PatrolLands[i].m_EventList.Count;
        }
        Data_Pool.m_PatrolPool.f_PatrolEventEx(0,eventNums,socketCallbackDt);
    }

    private void f_Callback_PatrolEventEx(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            ccCallback handle = f_Callback_AwardOnekeyClose;
            ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolAwardOnekeyPage, UIMessageDef.UI_OPEN, new object[] { onekeyAwardList, handle });
        }
        else
        {
            UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(874));
        } 
    }
    
    private void f_Callback_AwardOnekeyClose(object result)
    {
        f_UpdateByData(data);
    }

    private void f_BtnPatrolOnekey(GameObject go, object value1, object value2)
    {
        ccCallback handle = f_Callback_OnkeyPatrol;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolOnekeyPage, UIMessageDef.UI_OPEN,handle);
    }
	
	private void f_OnHelpBtnClick(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonHelpPage, UIMessageDef.UI_OPEN, 21);
    }
	
    private void f_BtnVisitFriend(GameObject go, object value1, object value2)
    {
        if(haveFriend == false)
		{
UITool.Ui_Trip("Hiện tại không có bạn bè");
		}
		else
		{
			UITool.f_OpenOrCloseWaitTip(true);
			Data_Pool.m_PatrolPool.f_RequestPatrolFriendInfo(f_Callback_RequestPatrolFriendInfo);
		}
    }

    private void f_Callback_RequestPatrolFriendInfo(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            ccCallback callbackHandle = f_Callback_VisitFriend;
            ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolVisitFriendPage, UIMessageDef.UI_OPEN, new object[] { data.iId, callbackHandle });
        }
        else
        {
            UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(875) + result);
        } 
    }

    private void f_BtnReturnSelf(GameObject go, object value1, object value2)
    {
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(876));
        f_UpdateByData(Data_Pool.m_PatrolPool.m_SelfPatrolDt);
    }

    private void f_BtnReturn(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PatrolPage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
    }

    private void f_Callback_VisitFriend(object value)
    {
        long friendId = (long)value;
        if (friendId == 0)
        {
            return;
        }
        if (friendId == Data_Pool.m_UserData.m_iUserId)
        {
            f_UpdateByData(Data_Pool.m_PatrolPool.m_SelfPatrolDt);
        }
        else
        {
            f_UpdateByData((PatrolPoolDT)Data_Pool.m_PatrolPool.f_GetForId(friendId));
        }
    }

    private void f_Callback_OnkeyPatrol(object result)
    {
        f_UpdateByData(Data_Pool.m_PatrolPool.m_SelfPatrolDt);
    }

    private void f_Callback_PatrolSkillPage(object result)
    {
        f_UpdateByData(data);
    }

    private void f_OpenOrCloseMoneyPage(bool isOpen)
    {
        if (isOpen)
        {
            List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
            listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
            listMoneyType.Add(EM_MoneyType.eUserAttr_Money);
            listMoneyType.Add(EM_MoneyType.eUserAttr_Vigor);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, listMoneyType);
        }
        else
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
        } 
    }

    /// <summary>
    /// 先打开界面，自己再重新请求数据更新
    /// </summary>
    /// <param name="go"></param>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    private void f_PatrolUpdateBySelfRequest()
    {
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_OnPatrolIniPatrolUpdateBySelfRequest;
        socketCallbackDt.m_ccCallbackFail = f_OnPatrolIniPatrolUpdateBySelfRequest;
        Data_Pool.m_PatrolPool.f_PatrolInit(0, socketCallbackDt);
    }

    private void f_OnPatrolIniPatrolUpdateBySelfRequest(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            f_UpdateByData(Data_Pool.m_PatrolPool.m_SelfPatrolDt);
        }
        else
        {
            UITool.UI_ShowFailContent(CommonTools.f_GetTransLanguage(877) + result);
        }
    }
}
