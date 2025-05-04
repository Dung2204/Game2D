using UnityEngine;
using System.Collections;
using System;
using ccU3DEngine;

/// <summary>
/// 全局玩家信息Pool
/// </summary>
public class GeneralPlayerPool : BasePool
{
    private ccCallback _ReadInforCallback = null;
    private long _iRequestUsreId = 0;

    public GeneralPlayerPool() : base("GeneralPlayerPoolDT")
    {

    }

    protected override void f_Init()
    {
    }

    protected override void RegSocketMessage()
    {
        RC_UserView tRC_UserView = new RC_UserView();
        ChatSocket.GetInstance().f_AddListener((int)ChatSocketCommand.RC_UserView, tRC_UserView, Callback_SocketData_RC_UserView);

        RC_UserExtra tRC_UserExtra = new RC_UserExtra();
        ChatSocket.GetInstance().f_AddListener((int)ChatSocketCommand.RC_UserExtra, tRC_UserExtra, Callback_SocketData_RC_UpdateExtra);

        RC_UserName tRC_UserName = new RC_UserName();
        ChatSocket.GetInstance().f_AddListener((int)ChatSocketCommand.RC_UserName, tRC_UserName, f_ChangePlayerName);

    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
        throw new NotImplementedException();
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
        throw new NotImplementedException();
    }


    private void Callback_SocketData_RC_UserView(object Obj)
    {
        RC_UserView tServerData = (RC_UserView)Obj;
        BasePlayerPoolDT tPoolDataDT = new BasePlayerPoolDT();

        tPoolDataDT.iId = tServerData.id;
        tPoolDataDT.f_SaveBase(tServerData.m_UserView.m_szName, tServerData.m_UserView.uSex, tServerData.m_UserView.uFrameId, tServerData.m_UserView.uVipLv, tServerData.m_UserView.uTitleId);

        f_AddPlayer(tPoolDataDT);
    }

    private void Callback_SocketData_RC_UpdateExtra(object Obj)
    {
        RC_UserExtra tServerData = (RC_UserExtra)Obj;
        BasePlayerPoolDT tPoolDataDT = (BasePlayerPoolDT)f_GetForId(tServerData.id);

        tPoolDataDT.f_SaveExtrend(tServerData.m_UserExtra.szLegionName, tServerData.m_UserExtra.iLv,
            tServerData.m_UserExtra.iBattlePower, tServerData.m_UserExtra.offlineTime);
        tPoolDataDT.f_UpdateDungeonStar(tServerData.m_UserExtra.iDungeonStars);
        CheckIsRegRequest(tPoolDataDT);
    }

    private void f_ChangePlayerName(object obj) {
        RC_UserName tRC_UserName = (RC_UserName)obj;
        BasePlayerPoolDT tBasePlayerPoolDT= f_GetForId(tRC_UserName.id) as BasePlayerPoolDT;

        if (tBasePlayerPoolDT!=null) {
            tBasePlayerPoolDT.f_ChangeName(tRC_UserName.m_szName);
        }
    }

    private void CheckIsRegRequest(BasePlayerPoolDT tBasePlayerPoolDT)
    {
        if (_ReadInforCallback != null)
        {
            if (tBasePlayerPoolDT.iId == _iRequestUsreId)
            {
                _ReadInforCallback(tBasePlayerPoolDT);
                _ReadInforCallback = null;
                _iRequestUsreId = 0;
            }
        }
    }

    #region 对外接口

    /// <summary>
    /// 增加玩家信息
    /// </summary>
    /// <param name="tBasePlayerPoolDT"></param>
    public void f_AddPlayer(BasePlayerPoolDT tBasePlayerPoolDT)
    {
        BasePlayerPoolDT tPoolDataDT = (BasePlayerPoolDT)f_GetForId(tBasePlayerPoolDT.iId);
        if (tPoolDataDT == null)
        {
            f_Save(tBasePlayerPoolDT);
        }
        else
        {
            tPoolDataDT.f_SaveBase(tBasePlayerPoolDT.m_szName, tBasePlayerPoolDT.m_iSex, tBasePlayerPoolDT.m_iFrameId, tBasePlayerPoolDT.m_iVip, tBasePlayerPoolDT.m_iTitle);
        }
        CheckIsRegRequest(tBasePlayerPoolDT);
    }

    /// <summary>
    /// 请求玩家的拓展信息
    /// </summary>
    /// <param name="iId"></param>
    /// <param name="tEM_ReadPlayerStep"></param>
    /// <param name="tccCallback"></param>
    public void f_ReadInfor(long iId, EM_ReadPlayerStep tEM_ReadPlayerStep, ccCallback tccCallback)
    {
        BasePlayerPoolDT tPoolDataDT = (BasePlayerPoolDT)f_GetForId(iId);
        byte bType = (byte)tEM_ReadPlayerStep;
        if (tPoolDataDT == null)
        {
            //MessageBox.ASSERT("无此玩家信息");
            _ReadInforCallback = tccCallback;
            _iRequestUsreId = iId;
            CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
            tCreateSocketBuf.f_Add(iId);
            tCreateSocketBuf.f_Add(bType);
            ChatSocket.GetInstance().f_SendBuf(ChatSocketCommand.CR_UserExtra, tCreateSocketBuf.f_GetBuf());
            return;
        }
        if (tPoolDataDT.m_eReadPlayerStep >= tEM_ReadPlayerStep)
        {
            tccCallback(tPoolDataDT);
        }
        else
        {//向服务器请求对应附加信息
            _ReadInforCallback = tccCallback;
            _iRequestUsreId = iId;

            CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
            tCreateSocketBuf.f_Add(iId);
            tCreateSocketBuf.f_Add(bType);
            ChatSocket.GetInstance().f_SendBuf(ChatSocketCommand.CR_UserExtra, tCreateSocketBuf.f_GetBuf());
        }
    }
    #endregion

}