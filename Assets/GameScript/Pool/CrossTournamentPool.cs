using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossTournamentPool : BasePool
{
    public static int m_TimeDelay = 10;
    private bool m_IsShopInit = false;

    private List<BasePoolDT<long>> m_CurLvShopList;

    public List<BasePoolDT<long>> ShopList
    {
        get
        {
            return m_CurLvShopList;
        }
    }
    private int mTimes;
    public int m_iTimes
    {
        get
        {
            return mTimes;
        }
    }
    private Dictionary<long, CrossUserTournamentPoolDT> dict_UserList = new Dictionary<long, CrossUserTournamentPoolDT>();
    //private ArrayList mGroupStageList;
    private List<BasePoolDT<long>> mGroupStageList;

    private Dictionary<int, CrossTournamentThePoolDT> dict_KnockList = new Dictionary<int, CrossTournamentThePoolDT>();

    private CrossUserTournamentPoolDT _MyInfo;
    public CrossUserTournamentPoolDT m_MyInfo
    {
        get
        {
            return _MyInfo;
        }
    }

     SC_CrossTournamentInfo _Info;
    public SC_CrossTournamentInfo m_Info
    {
        get
        {
            return _Info;
        }
    }
    private CrossTournamentTheBetInfoPoolDT _BetInfo;
    public CrossTournamentTheBetInfoPoolDT m_BetInfo
    {
        get
        {
            return _BetInfo;
        }
    }

    public CrossTournamentPool() : base("CrossTournamentPoolDT")
    {
        //f_Init();
    }
    protected override void f_Init()
    {
        mTimes = 0;
        m_CurLvShopList = new List<BasePoolDT<long>>();
        List<NBaseSCDT> tInitList = glo_Main.GetInstance().m_SC_Pool.m_CrossTournamentShopSC.f_GetAll();
        for (int i = 0; i < tInitList.Count; i++)
        {
            CrossTournamentShopDT tInitNode = (CrossTournamentShopDT)tInitList[i];
            f_Save(new CrossTournamentShopPoolDT(tInitNode));
        }
        f_SortData1();
        for (int i = 0; i < 64; i++)
        {
            CrossTournamentThePoolDT item = new CrossTournamentThePoolDT(i+1);
            item.f_InitInfo();
            dict_KnockList.Add(item.m_Id, item);
        }

        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.PlayerLvUpdate, f_UpdateDataByLv);
        glo_Main.GetInstance().m_GameMessagePool.f_AddListener(MessageDef.TheNextDay, f_UpdateDataByNextDay);
        //RegSocketMessage();
    }
    private void f_UpdateDataByLv(object value)
    {
        int lv = (int)value;
        m_CurLvShopList.Clear();
        List<BasePoolDT<long>> tSourceList = f_GetAll();
        for (int i = 0; i < tSourceList.Count; i++)
        {
            CrossTournamentShopPoolDT tNode = (CrossTournamentShopPoolDT)tSourceList[i];
            if (tNode.Template.iShowLv <= lv)
            {
                m_CurLvShopList.Add(tNode);
            }
        }
    }

    private void f_UpdateDataByNextDay(object value)
    {
        f_Reset();
        List<BasePoolDT<long>> tResetList = f_GetAll();
        for (int i = 0; i < tResetList.Count; i++)
        {
            CrossTournamentShopPoolDT tNode = (CrossTournamentShopPoolDT)tResetList[i];
            tNode.f_Reset();
        }
    }
    private void f_Reset()
    {
        mTimes = 0;
    }
    protected override void RegSocketMessage()
    {
        SC_CrossTournamentInfo tSC_CrossTournamentInfo = new SC_CrossTournamentInfo();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_CrossTournamentInfo, tSC_CrossTournamentInfo, Callback_SC_Info);

        SC_CrossTournamentRegeditRet tSC_CrossTournamentRegeditRet = new SC_CrossTournamentRegeditRet();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_CrossTournamentRegedit, tSC_CrossTournamentRegeditRet, Callback_SC_Regedit);

        CMsg_CrossTournamentUserInfo tSC_CrossTournamentUserList = new CMsg_CrossTournamentUserInfo();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_CrossTournamentUserList, tSC_CrossTournamentUserList, Callback_SC_UserList);

        CMsg_CrossTournamentGroupStageInfo tSC_CrossTournamentGroupStageList = new CMsg_CrossTournamentGroupStageInfo();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_CrossTournamentGroupStageList, tSC_CrossTournamentGroupStageList, Callback_SC_GroupStageList);

        CMsg_CrossTournamentTheInfo tSC_CrossTournamentAllKnockList = new CMsg_CrossTournamentTheInfo();
        GameSocket.GetInstance().f_RegMessage_Int0((int)SocketCommand.SC_CrossTournamentThe, tSC_CrossTournamentAllKnockList, Callback_SC_AllKnockList);

        CMsg_SC_CrossTournamentShopInfo tSC_CrossTournamentShopInfo = new CMsg_SC_CrossTournamentShopInfo();
        GameSocket.GetInstance().f_RegMessage_Int1((int)SocketCommand.SC_CrossTournamentShopInfo, tSC_CrossTournamentShopInfo, f_SC_ShopInfoUpdateHandle);

        SC_CrossTournamentTheBetInfo tSC_CrossTournamentTheBetInfo = new SC_CrossTournamentTheBetInfo();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_CrossTournamentTheBetInfo, tSC_CrossTournamentTheBetInfo, Callback_SC_CrossTournamentBetInfo);

        SC_CrossTournamentTheBetRet tSC_CrossTournamentTheBetRet = new SC_CrossTournamentTheBetRet();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_CrossTournamentTheBetRet, tSC_CrossTournamentTheBetRet, Callback_SC_CrossTournamentTheBetRet);
        // lấy thông tin data
        f_Info();
    }
    private int _userTime = 0;
    public void CheckUpdateUser()
    {
        if (GameSocket.GetInstance().f_GetServerTime() - _userTime < CrossTournamentPool.m_TimeDelay)
        {
            return;
        }
        _userTime = GameSocket.GetInstance().f_GetServerTime();
        if (_Info.iType > 0 && _Info.hasRegister != 99 && _Info.time > 0) SendRegedit(1);
    }
    private int _infoTime = 0;
    public void f_Info()
    {
        if (GameSocket.GetInstance().f_GetServerTime()- _infoTime < CrossTournamentPool.m_TimeDelay)
        {
            return;
        }
        _infoTime = GameSocket.GetInstance().f_GetServerTime();
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf(); 
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CrossTournamentInfo, bBuf);
    }
   
    private void Callback_SC_Info(object Obj)
    {
        SC_CrossTournamentInfo tServerData = (SC_CrossTournamentInfo)Obj;
        _Info = tServerData;
        if (_Info.iType>0)
        {
            f_UserList();
        }
    }
    private void Callback_SC_Regedit(object Obj)
    {
        SC_CrossTournamentRegeditRet tServerData = (SC_CrossTournamentRegeditRet)Obj;
        if(tServerData.ret == 1)
        {
            _Info.hasRegister = 1;
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.CrossTournament_Regedit_SUC);
        }
        UITool.Ui_Trip(tServerData.ret == 1 ? "Đăng ký thành công" : "Đăng ký thất bại");

    }

    private void Callback_SC_UserList(int iData1, int iData2, int iNum, ArrayList aData)
    {
        for (int i = 0; i < aData.Count; i++)
        {
            CMsg_CrossTournamentUserInfo userPoolDT = (CMsg_CrossTournamentUserInfo)aData[i];
            CrossUserTournamentPoolDT temp;
            if (dict_UserList.TryGetValue(userPoolDT.userId, out temp))
            {
                temp.f_UpdateInfo(userPoolDT);
                dict_UserList[userPoolDT.userId] = temp;
            }
            else
            {
                CrossUserTournamentPoolDT newUser = new CrossUserTournamentPoolDT(userPoolDT.userId);
                newUser.f_UpdateInfo(userPoolDT);
                dict_UserList.Add(userPoolDT.userId, newUser);
            }

            if (userPoolDT.userId == Data_Pool.m_UserData.m_iUserId)
            {
                _MyInfo = dict_UserList[userPoolDT.userId];
            }
        }

    }
    private void Callback_SC_GroupStageList(int iData1, int iData2, int iNum, ArrayList aData)
    {
        mGroupStageList = new List<BasePoolDT<long>>();
        for (int i = 0; i < aData.Count; i++)
        {
            CMsg_CrossTournamentGroupStageInfo data = (CMsg_CrossTournamentGroupStageInfo)aData[i];
            CrossTournamentGroupStagePoolDT item = new CrossTournamentGroupStagePoolDT(data.id);
            item.f_UpdateInfo(data);
            mGroupStageList.Add(item);
        }
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.CrossTournament_Update_GroupStage);
    }

    private void Callback_SC_AllKnockList(int iData1, int iData2, int iNum, ArrayList aData)
    {
        for (int i = 0; i < aData.Count; i++)
        {
            CMsg_CrossTournamentTheInfo data = (CMsg_CrossTournamentTheInfo)aData[i];
            CrossTournamentThePoolDT temp;
            if (dict_KnockList.TryGetValue(data.id, out temp))
            {
                temp.f_UpdateInfo(data);
                dict_KnockList[data.id] = temp;
            }
            else
            {
                CrossTournamentThePoolDT item = new CrossTournamentThePoolDT(data.id);
                item.f_UpdateInfo(data);
                dict_KnockList.Add(data.id, item);
            }
        }
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.CrossTournament_Update_Knock);
    }

    private void Callback_SC_CrossTournamentTheBetRet(object Obj)
    {
        SC_CrossTournamentTheBetRet tServerData = (SC_CrossTournamentTheBetRet)Obj;
        if (tServerData.ret == 1)
        {
            // thành công
            _BetInfo.f_MyBet(tServerData.winNo, tServerData.bet, tServerData.countA, tServerData.countB);
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.CrossTournament_Bet_SUC);
        }
        else if (tServerData.ret == 2 || tServerData.ret == 3)
        {
            UITool.Ui_Trip("Đã hết thời gian đặc cược");
        }
        else if (tServerData.ret == 5)
        {
            UITool.Ui_Trip("Chưa đến thời gian đặt cược");

        }
        else if (tServerData.ret == 4)
        {
            UITool.Ui_Trip("Chỉ có thể đặt cược 1 lần/trận");

        }

    }

    private void Callback_SC_CrossTournamentBetInfo(object Obj)
    {
        SC_CrossTournamentTheBetInfo tServerData = (SC_CrossTournamentTheBetInfo)Obj;
        
        if(_BetInfo != null && _BetInfo.m_Id == tServerData.id && (_BetInfo.m_MyBet.m_userId != 0 && tServerData.myBet.userId == 0))
        {
            return;
        }
        _BetInfo = new CrossTournamentTheBetInfoPoolDT(tServerData.id);
        _BetInfo.f_UpdateInfo(tServerData);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.CrossTournament_Bet_Info);
    }

    public List<BasePoolDT<long>> f_GetGroupStageListByDay(int wday)
    {
        List<BasePoolDT<long>> result = new List<BasePoolDT<long>>();
        //if (wday == 0)
        //{

        //}
        for (int i = 0; i < mGroupStageList.Count; i++)
        {
            CrossTournamentGroupStagePoolDT itemDT = (CrossTournamentGroupStagePoolDT)mGroupStageList[i];
            if (itemDT.m_iWday == wday)
            {
                result.Add(itemDT);
            }

        }
        return result;
    }

    public List<BasePoolDT<long>> f_GetKnock(int iThe)
    {
        List<BasePoolDT<long>> result = new List<BasePoolDT<long>>();
        foreach(KeyValuePair<int, CrossTournamentThePoolDT> item in dict_KnockList)
        {
            CrossTournamentThePoolDT itemDT = (CrossTournamentThePoolDT)item.Value;
            if (itemDT.m_iThe == iThe)
            {
                result.Add(itemDT);
            }
        }
        //result.Sort();
        return result;
    }

    public CrossTournamentThePoolDT f_GetKnockById(int id)
    {
        CrossTournamentThePoolDT temp;
        if (dict_KnockList.TryGetValue(id, out temp))
        {
            return temp;
        }
        return temp;
    }

    public void f_SC_ShopInfoUpdateHandle(int value1, int value2, int num, System.Collections.ArrayList arrayList)
    {
        for (int i = 0; i < arrayList.Count; i++)
        {
            CMsg_SC_CrossTournamentShopInfo serverData = (CMsg_SC_CrossTournamentShopInfo)arrayList[i];
            CrossTournamentShopPoolDT shopItem = (CrossTournamentShopPoolDT)f_GetForId(serverData.goodsId);
            if (shopItem != null)
            {
                shopItem.f_UpdateInfo(serverData.times);
            }
            else
            {
                MessageBox.ASSERT(string.Format("Máy chủ gửi dữ liệu vật phẩm không tồn tại,NodeType:{0}; GoodId:{1}", value1, serverData.goodsId));
            }
        }
    }

    public void SendRegedit(int type)
    {
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(type);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CrossTournamentRegedit, bBuf);
    }
    private int _userListTime = 0;
    /// <summary>
    /// lấy danh sách user đã đăng ký
    /// </summary>
    public void f_UserList()
    {
        if (GameSocket.GetInstance().f_GetServerTime() - _userListTime < CrossTournamentPool.m_TimeDelay)
        {
            return;
        }
        _userListTime = GameSocket.GetInstance().f_GetServerTime();
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CrossTournamentUserList, bBuf);
    }
    private int _GroupStageListTime = 0;
    /// <summary>
    /// lấy all danh sách đánh vòng bảng tính điểm // công phá
    /// </summary>
    public void f_GroupStageList()
    {
        if (GameSocket.GetInstance().f_GetServerTime() - _GroupStageListTime < CrossTournamentPool.m_TimeDelay)
        {
            return;
        }
        _GroupStageListTime = GameSocket.GetInstance().f_GetServerTime();
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CrossTournamentGroupStageList, bBuf);
    }
    private int _KnockListTime = 0;
    public void f_AllKnockList()
    {
        if (GameSocket.GetInstance().f_GetServerTime() - _KnockListTime < CrossTournamentPool.m_TimeDelay)
        {
            return;
        }
        _KnockListTime = GameSocket.GetInstance().f_GetServerTime();
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CrossTournamentAllKnockList, bBuf);
    }
    private int _BetInfoTime = 0;
    public void f_TheBetInfo()
    {
        if (GameSocket.GetInstance().f_GetServerTime() - _BetInfoTime < CrossTournamentPool.m_TimeDelay)
        {
            return;
        }
        _BetInfoTime = GameSocket.GetInstance().f_GetServerTime();
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CrossTournamentTheBetInfo, bBuf);
    }
    public void f_TheBet(int betId,int winNo,int bet)
    {
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(betId);
        tCreateSocketBuf.f_Add(winNo);
        tCreateSocketBuf.f_Add(bet);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CrossTournamentTheBet, bBuf);
    }
    
    public void f_ShopInit(SocketCallbackDT socketCallbackDt)
    {
        if (m_IsShopInit)
        {
            socketCallbackDt.m_ccCallbackSuc(eMsgOperateResult.OR_Succeed);
            return;
        }
        m_IsShopInit = true;
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_CrossTournamentShop, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CrossTournamentShop, bBuf);
    }
    public void f_Buy(int shopItemId, int buyNum, SocketCallbackDT socketCallbackDt)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_CrossTournamentBuy, socketCallbackDt.m_ccCallbackSuc, socketCallbackDt.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(shopItemId);
        tCreateSocketBuf.f_Add(buyNum);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CrossTournamentBuy, bBuf);
    }

    public void SendTest()
    {
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(5);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        //GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_CrossTournamentTest, bBuf);
    }


    // get data
    public CrossUserTournamentPoolDT GetUser(long userId)
    {
        CrossUserTournamentPoolDT temp;
        if (dict_UserList.TryGetValue(userId, out temp))
        {
            return temp;
            
        }
        return null;
    }
    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
    }
}
