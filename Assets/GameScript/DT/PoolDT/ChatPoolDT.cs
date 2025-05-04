using ccU3DEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ChatPoolDT : BasePoolDT<long>
{
    /// <summary>
    /// 频道
    /// </summary>
    public byte m_Chan;
    /// <summary>
    /// 发消息人ID
    /// </summary>
    public long m_Id;
    public long m_RoomId;

    public string m_Char;
    //public int32 content_len;//内容长度
    /// <summary>
    /// 聊天内容
    /// </summary>
    // public char[MAX_CHAT_BUFF] sender_Name#notice;
}
