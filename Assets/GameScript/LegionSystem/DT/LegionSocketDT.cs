using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_LongNode : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tSockBaseDT = (SockBaseDT)MemberwiseClone();
        return tSockBaseDT;
    }
    public long value1;   //long
};

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_ByteNode : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tSockBaseDT = (SockBaseDT)MemberwiseClone();
        return tSockBaseDT;
    }
    public byte value1;   //byte
};

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_HpKillerNode : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tSockBaseDT = (SockBaseDT)MemberwiseClone();
        return tSockBaseDT;
    }
    public long hp;     //血量
    public long killer; //击杀者
};

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_AwardInfoNode : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tSockBaseDT = (SockBaseDT)MemberwiseClone();
        return tSockBaseDT;
    }
    public byte idx; //索引
    public long userId;     //玩家id
    public byte awardIdx; //奖励Idx[1,X];
};

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_RTC_LegionInfo : SockBaseDT
{
    //使用 f_AddListener_EndStrin
    public SockBaseDT Clone()
    {
        SockBaseDT tSockBaseDT = (SockBaseDT)MemberwiseClone();
        return tSockBaseDT;
    }
    public long id;            // 军团id
    public long chiefId;       // 军团长id
    public int foundTime;     // 创建时间
    public byte iconId;         // 图标id
    public byte frameId;        // 边框id
    public byte lv;             // 等级
    public int exp;           // 经验
    public byte memNum;         // 成员数
    //public int buffLen;           //内容长度
    //char buff[20 + 40 + 80];    //名称 + 宣言 + 公告
};

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_GTC_UserLegionInit : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tSockBaseDT = (SockBaseDT)MemberwiseClone();
        return tSockBaseDT;
    }
    public long legionId;            // 军团id
    public byte pos; //自己的军团职位
    public int ioTime;   // 加入或者离开的时间
    public byte SacrificeType;    //祭天类型
    public short SacrificeAward;    //奖励领取进度
    public byte uDungeonAwardChaper;	// 第x章奖励领完
};

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_RTC_UserLegion : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tSockBaseDT = (SockBaseDT)MemberwiseClone();
        return tSockBaseDT;
    }
    public long userId;    //玩家Id
    public long legionId;  //军团Id
    public byte uPos;      //职位Id
    public int todayContri; //今日军团贡献
    public int totalContri; //累计军团贡献
};

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_LegionInit : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tSockBaseDT = (SockBaseDT)MemberwiseClone();
        return tSockBaseDT;
    }
    public byte uSacrifice; // 今日祭天进度
    public byte level;      //等级
    public int exp;         //经验
};
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_LegionLimitSkillInfo : SockBaseDT//个人军团信息
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.I1)]
    public byte[] lvLimitArray;
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_LegionPersionSkillInfo : SockBaseDT//个人军团信息
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte skillId;//技能id
    public byte lv;//玩家个人等级信息
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct RC_LegionLvRankList : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long legionId;//军团id
    public short rank;//军团排名
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]//SocketDT.MAX_USER_NAME)]
    public string m_szLeaderName;//军团长名称
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct RC_LegionPveRankList : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long legionId;//军团id
    public short rank;//军团排名
    public short LegionTollgate;//副本进度id
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]//SocketDT.MAX_USER_NAME)]
    public string m_szLeaderName;//军团长名称
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_LegionAwardRedPacket : SockBaseDT //发红包
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte money;	//钱
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_LegionRedPacketRank : SockBaseDT //军团红包排行榜
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long userId;
    public int monCount;//累计发放或抢到元宝数量
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_LegionRedPacketInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long id;//红包id
    public byte type;//类型，10,30,50元
    public long userId;//发红包人id
    public byte proc;//领取进度
    public short hasGet;//已经被领取金额
    public byte isGet;//自己是否领取过该红包
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_LegionReceiveRedPacket : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long id;//红包id
    public short getSycee;//得到的元宝
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_QueryLegionShop : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public short id;//id
    public int buyTimes;//购买次数
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_LegionShopTimeLimitInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public short id;
    public int pbuyTime;//个人购买次数
    public int abuyTime;//军团购买次数
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_GTC_LegionDungeonInitChapter : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte chapId;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.Struct)]
    public CMsg_HpKillerNode[] hpKillerNode;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_GTC_LegionDungeonChallengeRet : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long uDmg;     //造成伤害
    public int uContri;  //获得军团贡献
    public int uExp; //击杀后获得军团经验
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_GTC_LegionDungeonTimes : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte uTimes;     //今日已挑战次数
    public byte uBuyTimes;  //今日已购买次数
}

