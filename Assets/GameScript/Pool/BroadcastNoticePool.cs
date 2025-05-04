using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 服务器跑马灯广播消息
/// </summary>
public class BroadcastNoticePool : BasePool
{
    public BroadcastNoticePool() : base("BroadcastNoticePoolDT", true)
    {

    }
    protected override void f_Init()
    {
        
    }

    protected override void RegSocketMessage()
    {
        RC_ServerNotice rcServerNotice = new RC_ServerNotice();
        ChatSocket.GetInstance().f_RegMessage((int)ChatSocketCommand.BS_HourseLight, rcServerNotice, OnServerNoticeCardCallback);
    }
    /// <summary>
    /// 跑马灯广播消息
    /// </summary>
    /// <param name="data"></param>
    private void OnServerNoticeCardCallback(object data)
    {
        RC_ServerNotice rcServerNotice = (RC_ServerNotice)data;
        BroadcastNoticePoolDT poolDT = f_GetForId(rcServerNotice.uMsgId) as BroadcastNoticePoolDT;
        if (poolDT == null)
        {
            poolDT = new BroadcastNoticePoolDT();
            poolDT.iId = rcServerNotice.uMsgId;
            poolDT.uStartTime = rcServerNotice.uStartTime;
            poolDT.uOverTime = rcServerNotice.uOverTime;
            poolDT.uShowDeleTime = rcServerNotice.uShowDeleTime;
            poolDT.szContext = rcServerNotice.szContext;
            f_Save(poolDT);
        }
        else
        {
            poolDT.uStartTime = rcServerNotice.uStartTime;
            poolDT.uOverTime = rcServerNotice.uOverTime;
            poolDT.uShowDeleTime = rcServerNotice.uShowDeleTime;
            poolDT.szContext = rcServerNotice.szContext;
        }
        ccTimeEvent.GetInstance().f_RegEvent(poolDT.uShowDeleTime + 1f, true, null, ShowNotice);
        if (StaticValue.m_curScene == EM_Scene.GameMain)
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_ServerNoticeBroadcast);
    }
    private void _CharArrToString(char[] chararr, ref string tString)
    {
        tString = string.Empty;
        for (int i = 0; i < chararr.Length; i++)
        {
            tString += chararr[i].ToString();
        }
    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {

    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {

    }
    private void ShowNotice(object obj)
    {
        if (StaticValue.m_curScene == EM_Scene.GameMain)
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_ServerNoticeBroadcast);
    }
    #region 外部接口
    /// <summary>
    /// 是否需要显示新消息
    /// </summary>
    /// <returns></returns>
    public bool NeedShowNewMessage()
    {
        List<BasePoolDT<long>> listData = f_GetAll();
        //时间未到，不播放此条消息
        int gameserverTime = GameSocket.GetInstance().f_GetServerTime();
        for (int i = 0; i < listData.Count; i++)
        {
            BroadcastNoticePoolDT poolDT = listData[i] as BroadcastNoticePoolDT;
            if (gameserverTime > poolDT.uOverTime || gameserverTime < poolDT.uStartTime)
                continue;
            //间隔时间未到，不播放此条消息
            if (gameserverTime - poolDT.lastShowTime < poolDT.uShowDeleTime)
                continue;
            return true;
        }
        return false;
    }
    #endregion
}
