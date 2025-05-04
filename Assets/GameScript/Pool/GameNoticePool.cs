using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;

public class GameNoticePool : BasePool
{
    private int Time_StarTime;
    public GameNoticePool() : base("GameNoticePoolDT", true)
    {
        Time_StarTime = 1;
    }

    #region  Pool数据管理

    protected override void f_Init()
    {
    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
    }

    protected override void RegSocketMessage()
    {
        CMsc_SC_GameNotice tGameNotice = new CMsc_SC_GameNotice();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_GameNotice, tGameNotice, _AddData);
    }

    private void _AddData(object obj)
    {
        CMsc_SC_GameNotice tGameNotice = (CMsc_SC_GameNotice)obj;
        GameNoticePoolDT tGameNoticePoolDT = new GameNoticePoolDT();
        tGameNoticePoolDT.iId = tGameNotice.uMsgId;
        tGameNoticePoolDT.m_iStarTime = tGameNotice.uStartTime;
        tGameNoticePoolDT.m_iEndTime = tGameNotice.uOverTime;
        tGameNoticePoolDT.m_iQuitGame = tGameNotice.uQuitGame;
        tGameNoticePoolDT.m_iIsLockGame = tGameNotice.uModal;
        tGameNoticePoolDT.m_szContext = tGameNotice.szContext;
        tGameNoticePoolDT.m_szTitle = tGameNotice.szTitle;

        if (tGameNoticePoolDT.m_iStarTime > GameSocket.GetInstance().f_GetServerTime())
        {
            if (f_GetForId(tGameNoticePoolDT.iId) == null)
                f_Save(tGameNoticePoolDT);
            else
            {
                f_Clear();
                f_Save(tGameNoticePoolDT);
            }
            _StarTime();
        }
        else
        {
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_OpenGameNotice, tGameNoticePoolDT);
        }

    }

    #endregion

    GameNoticePoolDT _Message;

    public void _StarTime()
    {
        ccTimeEvent.GetInstance().f_UnRegEvent(Time_StarTime);
        Time_StarTime = ccTimeEvent.GetInstance().f_RegEvent(1f, true, null, _Star);
    }

    private void _Star(object obj)
    {
        if (StaticValue.m_curScene != EM_Scene.GameMain)
            return;

        if (f_GetAll().Count <= 0)
        {
            ccTimeEvent.GetInstance().f_UnRegEvent(Time_StarTime);
            return;
        }

        _Message = f_GetAll()[0] as GameNoticePoolDT;
        if (_Message.m_iStarTime <= GameSocket.GetInstance().f_GetServerTime())
        {
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_OpenGameNotice, _Message);
            ccTimeEvent.GetInstance().f_UnRegEvent(Time_StarTime);
            f_Delete(_Message.iId);
        }
    }
}
