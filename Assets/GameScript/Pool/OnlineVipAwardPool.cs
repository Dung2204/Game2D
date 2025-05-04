using ccU3DEngine;
using System.Collections.Generic;
using System;

public class OnlineVipAwardPool : BasePool
{
    EventOnlineVipInfo EventOnlineVipInfo = new EventOnlineVipInfo();
    private int[] _event = new int[4];
    private List<NBaseSCDT> _EventTimeSCList;
    public Dictionary<int, List<OnlineVipAwardDT>> OnlineVipAward_Dict = new Dictionary<int, List<OnlineVipAwardDT>>();
    public OnlineVipAwardPool() : base("OnlineVipAwardPoolDT")
    {
        //OnlineVipAward_Dict = new Dictionary<int, List<OnlineVipAwardDT>>();
        //for (int i = 0; i < glo_Main.GetInstance().m_SC_Pool.m_OnlineVipAwardSC.f_GetAll().Count; i++)
        //{
        //    OnlineVipAwardDT onlineVipAwardDT = glo_Main.GetInstance().m_SC_Pool.m_OnlineVipAwardSC.f_GetAll()[i] as OnlineVipAwardDT;
        //    if (OnlineVipAward_Dict.ContainsKey(onlineVipAwardDT.iEventTime))
        //    {
        //        OnlineVipAward_Dict[onlineVipAwardDT.iEventTime].Add(onlineVipAwardDT);
        //    }
        //    else
        //    {
        //        OnlineVipAward_Dict.Add(onlineVipAwardDT.iEventTime, new  List<OnlineVipAwardDT>());
        //        OnlineVipAward_Dict[onlineVipAwardDT.iEventTime].Add(onlineVipAwardDT);
        //    }
        //}
    }

    protected override void f_Init()
    {

    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
        f_Socket_UpdateData(Obj);
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
       
    }

    public void f_QueryInfo(SocketCallbackDT tSocketCallbackDT)
    {
        if (tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_EventOnlineVipInfo, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_EventOnlineVipInfo, bBuf);
    }
    protected override void RegSocketMessage()
    {
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_EventOnlineVipInfo, EventOnlineVipInfo, f_OnlineVipInfo);

    }

    private void f_OnlineVipInfo(object obj)
    {
        EventOnlineVipInfo = (EventOnlineVipInfo)obj;
        _event[0] = EventOnlineVipInfo.idata1;
        _event[1] = EventOnlineVipInfo.idata2;
        _event[2] = EventOnlineVipInfo.idata3;
        _event[3] = EventOnlineVipInfo.idata4;
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_ONLINE_VIP_AWARD, EventOnlineVipInfo);
    }

    public bool CheckGetAward(int id)
    {
        if(_event[id -1] > 0)
        {
            return true;
        }

        return false;
    }

    public void f_GetAward(int id, SocketCallbackDT tSocketCallbackDT)
    {
        if(tSocketCallbackDT != null)
            GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_GetEventOnlineVipAward, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(id);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_GetEventOnlineVipAward, bBuf);
    }



}
