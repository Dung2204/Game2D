using ccU3DEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class ChatPool : BasePool
{
    public ChatPool()
        : base("ChatPoolDT" , true)
    {
        lRecord = new BetterList<int>();
    }


    #region  数据管理

    protected override void f_Init()
    {
    }
     
    private void HandleSystemBroadCast(object data)
    {
        
    }

    protected override void f_Socket_AddData(SockBaseDT Obj , bool bNew)
    {
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
    }

    protected override void RegSocketMessage()
    {

    }

    private void Dele_Dt(int id)
    {
        f_Delete(id);
    }

    #endregion


    public List<ChatRoomInfo> ChatRoomList;

    ////////////////外部接口//////////////////
    /// <summary>
    /// 数量
    /// </summary>
    int Count = 0;
    /// <summary>
    /// 删除
    /// </summary>
    int DeleIndex = 0;
    /// <summary>
    /// 存储上限
    /// </summary>
    int limit = 50;
    /// <summary>
    /// 打开界面就进行广播
    /// </summary>
    public bool isUIShow = false;
    /// <summary>
    /// 含有的就不用Add
    /// </summary>
    public BetterList<int> lRecord;
    public void f_AddChat(sMSG_CHAT2C_Notice tNotice , string sql)
    {
        ChatPoolDT tChat = new ChatPoolDT();
        if (Data_Pool.m_FriendPool.f_CheckIsInBlackList(tNotice.userId))
        {
            return;
        }
        tChat.iId = Count;
        tChat.m_Chan = tNotice.uChan;
        tChat.m_Id = tNotice.userId;
        tChat.m_RoomId = tNotice.roomId;
        tChat.m_Char = sql;
        switch (tNotice.uChan)
        {
            case (int)EM_ChatChan.eChan_World:
                Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.ChatWordNewMsg);
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.ChatWordNewMsg);
                break;
            case (int)EM_ChatChan.eChan_Legion:
                Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.ChatLegionNewMsg);
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.ChatLegionNewMsg);
                break;
            case (int)EM_ChatChan.eChan_Team:
                Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.ChatTeamNewMsg);
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.ChatTeamNewMsg);
                break;
            case (int)EM_ChatChan.eChan_Private:
                Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.ChatPrivateNewMsg);
                Data_Pool.m_ReddotMessagePool.f_MsgAdd(EM_ReddotMsgType.ChatPrivateNewMsg);
                break;
        }
        // handle chat system notice

        if (tChat.m_Chan == (int)EM_ChatChan.eChan_System)
        {
            string[] dataSplit = tChat.m_Char.Split('|');
            tChat.m_Char  = CommonTools.GetBroadCastMess(dataSplit[0],int.Parse(dataSplit[1]),int.Parse(dataSplit[2]));
        }

        if (f_GetAll().Count >= limit)
        {
            f_GetAll().RemoveAt(0);
        }
        f_Save(tChat);
        Count++;
        if (isUIShow)
        {
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.RECEIVEMESSAGE , tChat);
        }
    }
}
