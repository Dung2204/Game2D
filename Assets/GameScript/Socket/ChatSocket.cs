using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
using System;
using Object = System.Object;
using System.Linq;

public class ChatSocket : BaseSocket
{
    private Socket_Loop _Socket_Loop = null;
    private static ChatSocket _Instance = null;
    private System.DateTime m_fLastTime;
    public System.DateTime _LastUpdateTime;

    public int m_PintSend = 0;
    public int m_PintRecv = 0;
    public System.DateTime _LastPinUpdateTime = System.DateTime.Now;
    public static ChatSocket GetInstance()
    {
        if (null == _Instance)
        {
            _Instance = new ChatSocket();
        }

        return _Instance;
    }

    public ChatSocket()
    {
        m_strTTTT = "ChatSocket";
    }

    protected override void InitMachine()
    {
        base.InitMachine();

        Socket_Wait tSocket_Wait = new Socket_Wait(this);
        _Socket_Loop = new Socket_Loop(this);
        _SocketMachineManger = new ccMachineManager(_Socket_Loop);
        _SocketMachineManger.f_RegState(new Socket_Connect(this));
        _SocketMachineManger.f_RegState(new Socket_ChatLogin(this));
        _SocketMachineManger.f_RegState(new Socket_Wait(this));
        _SocketMachineManger.f_ChangeState(tSocket_Wait);

    }
 
    public override void f_Close()
    {
        base.f_Close();
        //Debug.LogError("=--=-------关闭chat   socket-------------");
    }

    #region 登陆相关

    public void f_Login(SC_RelationServer tSC_RelationServer)
    {
        Socket_ChatLogin tSocket_ChatLogin = (Socket_ChatLogin)_SocketMachineManger.f_GetStaticBase((int)EM_Socket.Login);
        tSocket_ChatLogin.f_UpdateLoginToken(tSC_RelationServer);
        if (!tSocket_ChatLogin.f_CheckIsLogined())
        {
            _SocketMachineManger.f_ChangeState(tSocket_ChatLogin);
        }
    }

    //private void CallBack_LoginSuc(object Obj)
    //{
    //    //MessageBox.DEBUG("登陆聊天服成功");
    //}

    #endregion

#region 内部消息处理

    public override void Destroy()
    {
        base.Destroy();
        _Instance = null;
    }
    
    public override void f_Update()
    {
        base.f_Update();
//        if((int)(System.DateTime.Now -_LastPinUpdateTime).Seconds>10)
//        {
//            _LastPinUpdateTime = System.DateTime.Now;
//            Debug.LogError("Ping sendCount:" +m_PintSend+"   recvCount:"+m_PintRecv);
//        }

    }
    
#endregion

#region 外部消息处理
    
    protected override void InitMessage()
    {
        base.InitMessage();

        //m_GMSocketMessagePool.f_AddListener(SocketCommand.GM_SCK_UNBUILD.ToString(), On_GM_SCK_UNBUILD, null);
        //stGameCommandReturn tGameCommandRet = new stGameCommandReturn();
        //f_AddListener((int)SocketCommand.CONTROL_CTG_OperateResult, tGameCommandRet, On_CMsg_GameCommandReturn);

        sMSG_CHAT2C_Notice tsMSG_Notice = new sMSG_CHAT2C_Notice();
        f_AddListener_EndString((int)ChatSocketCommand.CR_Chat, tsMSG_Notice, On_CHAT2C_Notice);

        RC_ChatRoomInfo RC_ChatRoomInfoReturn = new RC_ChatRoomInfo();
        f_RegMessage((int)ChatSocketCommand.RC_GetChatRoom, RC_ChatRoomInfoReturn, f_Callback_Return);
        //////////////////////////////////////////////////////////////////////////
        //DATA

        basicNode1 tPing = new basicNode1();
        f_AddListener((int)ChatSocketCommand.PING_Reps, tPing, On_Ping);

        stGameCommandReturn tGameCommandRet = new stGameCommandReturn();
        f_AddListener((int)SocketCommand.CONTROL_CTG_OperateResult, tGameCommandRet, On_CMsg_GameCommandReturn);

    }

    private void f_Callback_Return(object data)
    {
        RC_ChatRoomInfo rooms = (RC_ChatRoomInfo)data;
        Data_Pool.m_ChatPool.ChatRoomList = rooms.allRooms.ToList().GetRange(0, rooms.num).ToList();
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_ChatRoomUpdate);
    }

    private void On_CHAT2C_Notice(object obj, string ppSQL)
    {
        sMSG_CHAT2C_Notice node = (sMSG_CHAT2C_Notice)obj;
        Debug.Log(node.userId);
        //Data_Pool.m_GeneralPlayerPool.f_ReadInfor(node.userId, EM_ReadPlayerStep.Base,null);
        Data_Pool.m_ChatPool.f_AddChat(node,ppSQL);
    }

    #endregion

    protected void On_Ping(object Obj)
    {
        base.On_Ping(Obj);
        _LastUpdateTime = DateTime.Now;
        //m_PintRecv++;
        //Debug.LogError("==========chat ping resp=======");
    }

    #region 聊天


    /// <summary>
    /// 发送聊天信息
    /// </summary>
    /// <param name="iUserid">接收消息玩家</param>
    /// <param name="iType">频道</param>
    /// <param name="content"></param>
    public void f_SendMessage(long iUserid, EM_ChatChan tEM_ChatChan, string szContent,long roomId = 0)
    {
        CreateSocketBuf mBuf = new CreateSocketBuf();
        mBuf.f_Add((byte)tEM_ChatChan);
        mBuf.f_Add(iUserid);
        mBuf.f_Add(roomId);
        int iTextLen = 0;
        mBuf.f_AddEndString(szContent, ref iTextLen);
        byte[] bBuf = mBuf.f_GetBuf();
        f_SendBuf(ChatSocketCommand.CR_Chat, bBuf);
    }

    public void f_GetRoomMessage(long roomId = 0)
    {
        CreateSocketBuf mBuf = new CreateSocketBuf();
        mBuf.f_Add(roomId);
        byte[] bBuf = mBuf.f_GetBuf();
        f_SendBuf(ChatSocketCommand.CR_GetChatRooms, bBuf);
    }

    #endregion

    #region 时间

    public override void f_Ping()
    {
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        int ret = f_SendBuf2Force((int)ChatSocketCommand.PING, bBuf);
//        Debug.LogError("==========================chat ping");
//        if (ret >0)
//        {
//            //Debug.LogError("Chat Ping Success");
//            m_PintSend++;
//        }
        //else
            //Debug.LogError("Chat Ping failed");
    }

#endregion


}//END Class