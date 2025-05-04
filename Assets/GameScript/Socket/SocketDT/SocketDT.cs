using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public struct stIp
{
    public string m_szIp;
    public int m_iPort;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct ChatOffline : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct OpenSeverTime : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int value1;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct basicNode1 : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int value1;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct stPoolDelData : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long iId;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct stDelData : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long iId;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct stGameCommandReturn : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int iCommand;
    /// <summary>
    /// 操作结果
    /// </summary>
    public int iResult;
    //public int iData2;
    //public int iData3;
}


/// <summary>
/// 登陆封包
/// </summary>
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_CTG_AccountEnter
{
    /// <summary>
    /// 0：正常登陆 1：重新连接
    /// </summary>
    public int m_state;									// 服务器UID

    /// <summary>
    /// 账户名
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]//SocketDT.MAX_USER_NAME)]
    public string m_strAccount;                      //// 机器码		

    /// <summary>
    /// 密码
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]//SocketDT.MAX_USER_NAME)]
    public string m_strPassword;                      //// 机器码		
    //服务器ID： 默认1
    public int m_dwServerID;									// 服务器UID
    //GamePlayer标示，服务器用
    public long m_GamePlayerPoint;
}

/// <summary>
/// 创建账户封包
/// </summary>
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_CTG_AccountCreate
{
    /// <summary>
    /// 账户名
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]//SocketDT.MAX_USER_NAME)]
    public string m_strAccount;                      //// 机器码		

    /// <summary>
    /// 密码
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]//SocketDT.MAX_USER_NAME)]
    public string m_strPassword;

}


/// <summary>
/// 创建账户封包
/// </summary>
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_AccountCreateRelt : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }

    /// <summary>
    /// OR_Succeed = 0, // 成功 OR_Error_AccountRepetition = 20, // 注册：账号重复
    /// </summary>
    public int m_result;

    /// <summary>
    /// 账户名
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]//SocketDT.MAX_USER_NAME)]
    public string m_strAccount;                      //// 机器码		


}



#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_GTC_LoginRelt : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    /// <summary>
    /// 账户ID
    /// </summary>
    public long m_PlayerId;                                  // 账户id

    /// <summary>
    /// OR_Succeed = 0, // 成功 OR_Error_NoAccount = 21, // 登陆：账号不存在 OR_Error_Password = 22, // 登陆：密码错误
    /// </summary>
    public int m_result;									// 账户id
    public long userPtr;
    public int m_iServerTime;

    public int m_iServerId;  //后端约定，如果状态是排队中，则m_iServerId为排队人数

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]//SocketDT.MAX_USER_NAME)]
    public string m_strServerName;

    /// <summary>
    /// 服务器保留
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 96)]//SocketDT.MAX_USER_NAME)]
    public string uniqueLoginId;
}


#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_GTC_AccountLoginRelt : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }

    public int createTime;			// 建号时间
    public int loginDays;
    //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.I4)]       //eUserAttr_Enum.eUserAttr_INVALID - 1)]
    //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 21, ArraySubType = UnmanagedType.I4)]       //eUserAttr_Enum.eUserAttr_INVALID - 1)] //TsuCode ChaosBattle SizeConst 20->21
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 25, ArraySubType = UnmanagedType.I4)]       //TsuCode - Coin - KimPhieu
    public int[] attr;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]//SocketDT.MAX_USER_NAME)]
    public string szRoleName;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_UserAttr : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte attrEnum;
    public int iValue;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_ReturnRandRoleName : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]//SocketDT.MAX_USER_NAME)]
    public string szRoleName;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_DescendForuneRecord : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]
    public string szRoleName;   //玩家名字
    public short iTimes;        //玩家抽奖倍数
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_RedPacketExchangeInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int id;   //序号id
    public int iExchangeTimes;    //已兑换次数
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_NewYearRechargeAwardInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public short id;   //序号id
    public byte type;    //1.单充 2.累充
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_RedPacketTaskInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public short id;   //序号id
    public byte iGetCount;  //领取奖励次数
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_RedPacketTaskTypeUpdate : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte iTaskType;   //任务类型
    public int iProgress;    //进度值
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_RoleDIY : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int orCode;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct sCharacter : SocketDataBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int GetNodeType()
    {
        return m_node_type;
    }

    /// <summary>
    /// 节点类型
    /// </summary>
    public int m_node_type;//

    public int iId;
    public int m_iExp;
    public int m_iCharacterId;
    public int m_iTempleteId;
    public int m_iCityId;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_CardInit : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long id;            // 唯一Id
    public short tempId;        // 模板Id
    public short lv;            // 等级
    public int uExp;			// 当前经验
    public int evolveId;      // 进化ID
    public short lvAwaken;      // 领悟等级
    public short flagAwaken;	// 领悟掩码 十进制掩码 ABCD(1000+100+10+1)
    public short uSkyDestinyLv; // 天命等级
    public int uSkyDestinyExp;// 天命经验
    public int lvArtifact; //神器等级
}


#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_ItemInit : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long id;            // 唯一Id
    public int tempId;        // 模板Id
    public int iNum;            // 等级
}


#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_CardFragmentInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long id;            // 唯一Id
    public int tempId;        // 模板Id
    public int iNum;            // 等级
}


#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_ShopResourceInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int id;            // 唯一Id
    public ushort buyTimes;      // 累计购买次数
}


#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_ShopLotteryInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int lotteryId;            // 唯一Id
    public int totalTimes;    // 累计购买次数
    public int lastFreeTime;	// 上次免费购买时间
    public byte totalHalfTimes; //今日半价单抽次数
    public int tempTotalTimes;    // tổng lượt đã quay thay đổi khi nhận quà mốc
    public int award;	// trạng thái quà đã nhận: 1-4: các mốc quà / 0 chưa nhận
    public int itemId;    // item đã chọn trong vòng quay tùy chọn
    public int iLock;	// khóa khi đã quay ra item trên

}


#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_ShopGiftInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long buyMask;            // 十进制掩码 1111000011110000;	表示第1,2,3,4,9,10,11,12礼包已经购买
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_ShopRandInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte eRandShop;
    public short propFreshTimes;//今日道具刷新次数
    public short freeTimes;  // 免费刷新次数
    public int lastTime;   // 上次免费刷新次数恢复时间
    public int buyMask;    // 十进制掩码从高至低 如110000表示前2个商品已经购买
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I4)]
    public int[] goodsId; //物品id
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_QueryReputation : SockBaseDT
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
public struct SC_ReinforceInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.U8)]
    public long[] cardId;	//
}
#if UNITY_IPHONE
[System.Serializable]

#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_FormationInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte eFormationPos;  // 站位[0, 8)
    public int eFormationSlot;  // [0, 19)
    public long cardId;        // 卡牌id

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7, ArraySubType = UnmanagedType.U8)]
    public long[] equipId;	// 装备栏
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct OtherEquipInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int tempId;        // 模板Id
    public byte stars;          // 星数
    public short lvIntensify;   // 强化等级
    public short lvRefine;		// 精炼等级
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct OtherTreasureInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int tempId;
    public short lvIntensify;   // 强化等级
    public short lvRefine;		// 精炼等级
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct OtherGodEquipInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int tempId;        // 模板Id
    public byte stars;          // 星数
    public short lvIntensify;   // 强化等级
    public short lvRefine;		// 精炼等级
}
#if UNITY_IPHONE
[System.Serializable]

#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct OtherCardInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public short tempId;
    public short lv;
    public int evolveId;
    public short lvAwaken;
    // 道具栏
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.Struct)]
    public OtherEquipInfo[] otherEquipInfo;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.Struct)]
    public OtherTreasureInfo[] otherTreasureInfo;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1, ArraySubType = UnmanagedType.Struct)]
    public OtherGodEquipInfo[] otherGodEquipInfo;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_DungeonChapterInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int uChapterId;    // 章节Id
    public byte uTollgates;  // 通关数
    public byte uToBoxGetNum;  //已领取的关卡宝箱数
    public byte uStars;     // 总星数
    public byte uBoxFlag;	// 十进制掩码 110 表示第1、2个宝箱已经领取

}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_DailyPveFight : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte uStars;     // 总星数

}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_DailyPveFightInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int id;     // 日常副本类型id(今日已打过的)
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_BattleForm : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int id;  //阵图id
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_UserSigned : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte iType;//子类型
    public byte eSignedCount;//已签到天数
    public byte signedMask;//签到类型
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_SyceeAwardInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int node_type;//
    public long awardFlag;
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_VipGiftInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    //public byte iGet;//今日是否领取，1.已领取，0未领取
    public int bGetFlag;      //按位 领取为1 未领取为0
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_NewYearSignInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte eSignedDays;//已签到天数
    public int lastSignedTime;//上一次签到时间
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_GetPower : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int LatestTime;      //最新领取体力时间
    public byte IsGetSycee;     //是否获得元宝
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_WealthManInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte yesterdayTimeCount;//昨天迎财神次数[0,3]
    public byte todayTimeCount;//今天迎财神次数[0,3]
    public byte yesterdayBoxCount;//昨天宝箱领取次数[0,1]
    public byte todayBoxCount;//今天天宝箱领取次数[0,1]
    public int latestTime;//最新迎财神时间
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_LuckySymbol : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int LatestTime;    //最近领取时间
    public int Count;     //领取次数
    public int Status;		//领取状态
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_ActLoginGiftInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte totalDay;    //总的天数
    public byte giftFlag;    //奖励领取标志 按位 giftFlg |=(1<< 天数)
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_ActLoginGift : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte uGetDay;    //领取的哪一天奖励
    public byte giftFlag;   //奖励领取标志 按位 giftFlg |=(1<< 天数)
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_ActLoginGiftNewServ : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int uCurDay;  //当前第几天
    //public byte giftFlag; //当天是否领取
    public int giftFlag; //TsuCode
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_NewServGift : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int uCurDay;  //当前第几天
    //public byte giftFlag; //当天是否领取
    public int giftFlag; //当天是否领取
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_PhoneBidnInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte bindFlag;    // 0未绑定，1已绑定
    public byte getAwardFlag;	// 0未领取，1已领取
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_PhoneAward : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int num;  //idx:
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.Struct)]
    public SC_Award[] awards;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_QueryMonthCard : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int remainDays25;    // 剩余有效天数 >0才可以领
    public int awardTimes25;	// 今日领取次数
    public int remainDays50;    // 剩余有效天数 >0才可以领
    public int awardTimes50;	// 今日领取次数
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_FirstRechargeInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte mId;
    //TsuCode- FirstRechargeNew -NapDau
    public long day;
    public int received_1;
    public int received_2;
    public int received_3;
    public long timeAllReceived;
    //----------------------------------
}
/// <summary>
/// 周基金
/// </summary>
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_WeekFundInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte iId; //对应配置Id
    public byte bFinish;        //是否完成
    public int awardFlag;//领奖标志,按位与
}

/// <summary>
/// 领取周基金回调
/// </summary>
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_WeekFundSign : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte uDay;       //领取哪一天
    public int awardFlag;  //领奖标志,按位与
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_RestoreTime : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int RestoreTime;    //体力、精力恢复时间
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_IsUserSigned : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte iType;//Signed表里的iType类型
    public byte Signed;//每日签到是否已经签到 0表示没签到 1表示已经签到
    public byte GrandSigned;//豪华签到是否已经签到
    public byte SignedDayCount;//每日签到已签到的天数
    public byte GrandSignedDayCount;//豪华签到已签到天数
    public short RechargeCount;//当天充值金额
    public int GrandSignResetTimeStamp;//豪华签到重置时间
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_DungeonTollgateInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int uTollgateId;    // 关卡Id
    public byte uStars;  // 星数
    public byte boxTimes;     // 宝箱奖励领取次数
    public byte times;     // 挑战次数
    public byte resetTimes; //重置次数
}


#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_EquipInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long id;            // 唯一Id
    public int tempId;        // 模板Id
    public byte stars;        // 星数
    public short expStars;     //升星经验
    public short lucky;        //幸运值
    public short lvIntensify;   //强化等级
    public short lvRefine;      //精炼等级
    public int expRefine;       //精炼经验
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_EquipFragmentInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long id;            // 唯一Id
    public int tempId;        // 模板Id
    public int num;
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_EquipIntensify : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    long id;
    public byte realTimes;
    public byte critTimes;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_ClothArrayInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameParamConst.MAX_FIGHT_POS, ArraySubType = UnmanagedType.U1)]
    public byte[] eClothArrayTeamPoolID;	// 位置与pos对应
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_DungeonFinishInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte starNum; //星数
    public byte isFirstWin; //是首次通关  > 0 = true
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_DungeonAwardChanged : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public uint uChapterId;             // 章节Id
    public ushort uTollgateBoxTimes;      // 关卡宝箱领取次数10个关卡，每个关卡按位取值
    public byte uBoxFlag;               //星数奖励领取标志 3个宝箱按位
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_Award : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte resourceType;  //
    public int resourceId;
    public int resourceNum;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_TreasureInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long id;            // 唯一Id
    public int tempId;        // 模板Id
    public int Num;           //叠加数量
    public short lvIntensify;   //强化等级
    public int expIntensify;    //强化经验
    public short lvRefine;      //精炼等级
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_TreasureFragmentInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long id;            // 唯一Id
    public int tempId;        // 模板Id
    public int num;                  //数量
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_AwakenEquipInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long id;            // 唯一Id
    public int tempId;        // 模板Id
    public int num;                  //数量
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_TaskDailyData : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte eType;            // 类型 对应 EM_DailyTaskCondition
    public int uValue;        // 对应类型的参数
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_TaskBox : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int uScore;        // 积分
    public int uFlag;        // 宝箱领取十进制掩码 如11000 表示第1,2个宝箱已经领取
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_TaskMainInit : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int uTaskId;       // 当前任务Id
    public int uValue;        // 进度值
    public int uAwardTimes;	// 奖励领取次数
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_RechargeHistoryNode : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int id;            // 充值ID
    public int times;        // 充值次数
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_RechargeHistory : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.Struct)]
    public SC_RechargeHistoryNode[] nodes;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_FightRet : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int iDataLen;
    public int iBufLen;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4096, ArraySubType = UnmanagedType.U1)]
    public byte[] eClothArrayTeamPoolID;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_CenterAwardNode : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte uType;
    public int uId;
    public int uNum;
};


#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_CenterAward : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long id;
    public byte CenterAwardType;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I4)]
    public int[] Param;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.Struct)]
    public SC_CenterAwardNode[] nodes;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_TaskAchievementInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte eType; // 成就类型
    public int uValue; // 成就进度值(仅限必要)
    public int lastId; // 最近已领取成就Id
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_RelationServer : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public uint iHost;
    public ushort iPort;
    public ulong iLoginToken;
}


#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct UserView : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int uSex;      //0男1女   IConID  
    public int uJob;
    public byte uFrameId;
    public byte uVipLv;
    public byte uTitleId;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]//SocketDT.MAX_USER_NAME)]
    public string m_szName;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct UserExtra : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public short iLv;
    public int iBattlePower;
    public int offlineTime;
    public int iDungeonStars;//副本星数
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = LegionConst.LEGION_NAME_BYTE_NUM)]//SocketDT.MAX_USER_NAME)]
    public string szLegionName;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct RC_UserView : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long id;
    public UserView m_UserView;
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct RC_UserName : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long id;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]//SocketDT.MAX_USER_NAME)]
    public string m_szName;
}


#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct RC_OrangeCard : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    // public long userId;        // 玩家Id
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]
    public string szName;
    public int cardTempId;    // 卡牌类型
    public int opType;		// 操作类型
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct RC_ServerNotice : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int uMsgId;
    public int uStartTime;
    public int uOverTime;
    public int uShowDeleTime;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string szContext;
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct RC_UserExtra : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long id;
    public UserExtra m_UserExtra;
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_RankInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int Rank;    //等级
    public int TimeStamp;       //对应的时间戳
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_GrabTreasureInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public short treasureFragID;    //法宝碎片id
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.Struct)]
    public SC_GrabTreasureInfoNode[] grabTreasureInfoNodeInfo;
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_GrabTreasure : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public short treaFragId;//碎片状态，如果没有获得则为0
    public short awardId;//奖励id
    public SC_GrabTreasureNode awardNode;//奖励
    public short star;//战斗星数
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_GrabTreasureNode : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte resourceType;//奖励类型
    public int resourceId;//奖励ID
    public int resourceNum;//奖励数量
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_GrabTreasureInfoNode : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte index;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I2)]
    public short[] tempCardIDArray;
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_DiscountProp : SockBaseDT//购买回调
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int id;    //id
    public int iProc;//进度
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_DiscountRechargeInfo : SockBaseDT//限时优惠充值
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int id;    //id
    public int timeBeg; //开始时间
    public int state;//状态（1.可领取0.不可领取）
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_DiscountAllServInfo : SockBaseDT//限时优惠全民福利
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int id;//id
    public int num;//领取次数
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_DiscountAllServ : SockBaseDT//限时优惠全民福利领取
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int id;//id
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_DiscountPropInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int iProc;//进度
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.Struct)]
    public SC_DiscountPropInfoNode[] info;
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_DiscountPropInfoNode : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int id;
    public int buyTimes;
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_GiftInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int GiftId;      //礼包id
    public byte Num;       //购买数量
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_OpenServFund : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int GiftId;      //礼包id
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_OpenServOnlineAwardInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public short Id;      //id
    public byte state;      //领取状态：1.可领取，0.已领取
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_DescendFortuneInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte num;      //已经祈福的次数
    public int openServerDay;     //开服天数
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_DescendFortune : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public short times;      //祈福得到的倍数
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_QBuyOpenServFund : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte isBuy;   //是否已经购买
    public int count;    //全服购买人数
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct sMSG_CHAT2C_Notice : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tSockBaseDT = (SockBaseDT)MemberwiseClone();
        return tSockBaseDT;
    }

    public byte uChan;      
    public long userId;
    public long roomId;
    //int32 content_len;//内容长度
    //char content[MAX_CHAT_BUFF];//内容 sender_Name#notice
};

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct ChatRoomInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long roomId;
    public long lastUpdate;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I8)]
    public long[] roomMembers;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct RC_ChatRoomInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tSockBaseDT = (SockBaseDT)MemberwiseClone();
        return tSockBaseDT;
    }
    public int num;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.Struct)]
    public ChatRoomInfo[] allRooms;
};

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct RC_InitFriend : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long userId;
    public UserView m_UserView;
    public UserExtra m_UserExtra;
}


#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_GTC_ChatLoginRelt : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    /// <summary>
    /// OR_Succeed = 0, // 成功 OR_Error_NoAccount = 21, // 登陆：账号不存在 OR_Error_Password = 22, // 登陆：密码错误
    /// </summary>
    public int m_result;                                    // 账户id
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_RTC_RecomFriend : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.I8)]       //eUserAttr_Enum.eUserAttr_INVALID - 1)]
    public long[] recomUserId;   //推荐好友id
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_RTC_ReplyFriend : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long userId;
    public byte uAccept; // 0=拒绝
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_RTC_VigorHistory : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte uMask; //对应EM_VigorFlag    0=我赠送给好友的 1=赠送给我且尚未领取的	3=赠送给我且已经领取的
    public long friendId;  //好友Id
    public int uTime;   //赠送时间
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_RTC_RelationUser : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int vigorTimes; //今日好友领取
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_EquipCostHistory : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }

    public long totalMoney;   //累计强化银币
    public int totalSycee;    //  累计强化元宝 
    public int totalFragment;  //  累计强化碎片
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_Arena : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte isOnRank;	// 0未上榜
    public int uRank;  //排名
    public int uiHighstRank;
    public int uTimes; //今日挑战次数
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_RTC_ArenaListNode : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long rivalId;  //对手Id
    public int uRank; //排名
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_ArenaRet : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte isWin; //0是失败 1是胜利
    public int oldRank; //原来的排名
    public int curRank;		// 现在的排名
    public int rankBreakNum; //历史最高排名突破了多少名
    public int iRivalPower;//对方战力
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]
    public string szRivalName;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_ArenaSweepRetNode : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte isWin; //0是失败 1是胜利
    public SC_Award award;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_ArenaSweepRet : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.Struct)]
    public CMsg_SC_ArenaSweepRetNode[] nodes;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_ArenaChooseAward : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte idx;  //idx:
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.Struct)]
    public SC_Award[] awards;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_RTC_ArenaRankList : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long userId;  //玩家Id 前端用不到
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30, ArraySubType = UnmanagedType.I8)]
    public long[] rankUserId;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct RebelInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long discovererId;   //发现者ID
    public int uDeployId;   //模版ID
    public int uRebelLv;    //叛军等级
    public byte color;     //品质
    public int hpPercent;  //生命亿分比
    public int EndTime;    //逃跑时间
    public int shareTime;  //分享时间
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_CrusadeRebelInit : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte isShare;// (Nổi dậy tôi đã tìm thấy) Có được chia sẻ không

    public int uExploit;// Xứng đáng

    public int maxDmg;// Sát thương lớn nhất trong một trò chơi hiện nay được gửi từ cuối sân và chia cho W

    public byte rankExploit;// Xếp hạng khen thưởng 0: "200+" được hiển thị trên giao diện người dùng

    public byte rankDmg;	// Xếp hạng sát thương 0: "200+" hiển thị trên giao diện người dùng

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I4)]
    public int[] uAwardFlag;    //nhận trạng thái
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CrusadeRebelRank : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long userId;
    public int uDmg;
}


#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_CrusadeRebelDmgRank : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long userid;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.Struct)]
    public CrusadeRebelRank[] Info;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_RebelArmyFinish : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte isWin; //是否胜利
    public ulong uDmg;   //造成的伤害
    public uint uExploit;    //获得功勋
    public int uBattleFeat;  //获得战功
    public byte isCanShare;//是否可分享(只有第一次失败时可分享)
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_RunningMan : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public ushort hisStars;      // 历史最高星数
    public ushort hisChap;       // 历史最高章节
    public ushort his3StarsChap; // 历史最高连续全3星章节
    public ushort eliteFirstProg;// 精英首通进度

    public ushort uStars;        // 本轮获得星数
    public ushort uesdStars;     // 本轮使用星数
    public byte resetTimes;     // 今日重置次数

    public byte eliteTimes;     // 今日精英关卡挑战次数
    public byte eliteBuyTimes;	// 今日精英关卡购买次数
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_ChapInfoNode : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public ushort uChapId;   // 章节Id
    public byte uBoxTimes;  // 宝箱领取次数
    public byte uBuffIdx;   // 所兑换bff索引[0, 3] 0表示尚未兑换
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U1)]
    public byte[] uBuff;   // buff
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
    public sbyte[] iRet;    //0未挑战 >0成功 <0失败 abs星数

}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_RunningManRet : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public ushort uChap;     // 章节
    public byte idx;        // [1, 3]
    public sbyte iRet;       //  >0成功 <0失败 abs星数
    public ushort uAwardMoneyRate;// 奖励倍率(百分比)
    public ushort uAwardPrestRate;// 奖励倍率(百分比)
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_RunningManRankNode : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long userId;
    public ushort hisStars; //历史最高星数
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_RunningManRank : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long userId;        // 用于服务器转发
    public int page;			// [0, 4]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.Struct)]
    public CMsg_RunningManRankNode[] info;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_RunningManRankMySelf : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public short hisStar;            //历史最高星数
    public byte myRank;			 //自己排名
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_RunningManEliteRet : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public ushort uTollgate;
    public byte isWin;
    public byte isFirst;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_RunningManSweepRetNode : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public ushort uAwardMoneyRate; //奖励银币倍率(百分比)
    public ushort uAwardPrestRate; //奖励威名倍率(百分比)
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_RunningManShopNode : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public ushort id;//id
    public ushort buyTimes;//购买次数
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_PatrolInitNode : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int landId;        // 领地id
    public int lv;            // 领地等级
    public int totalHours;   // 累计巡逻时长
    public int cardId;       // 卡牌id
    public int patrolId;     // 巡逻id
    public int beginTime;   // 巡逻开始时间
    public byte isRiot;     // 是否正在暴动
    public byte isAward;    //奖励
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_PatrolChallengeRet : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte isWin;   //是否胜利
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_PatrolPacify : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int todayTimes;  //今天的镇压次数
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_PatrolEventInfoNode : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte idx;    //时间索引，标记唯一性
    public uint uTime; //触发时间
    public byte eventId; //事件Id
    public int awardNum; //奖励数量
    // 奖励参数
    // 对于普通事件依次是 奖励类型；奖励id；奖励加倍十进制掩码(00均未加倍 01事件加倍 10技能加倍 11均加倍)
    // 对于镇压事件依次是 好友id低32位 好友id高32位
    public int uParam0;
    public int uParam1;
    public int uParam2;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_RC_FriendPatrolNode : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long userId;
    public byte landNum;
    public byte patrolNum;
    public byte riotNum;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsc_SC_CardSkyDestiny : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long lCardid;
    public int IsSkyUp;
    public int uRealNum;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100, ArraySubType = UnmanagedType.U1)]
    public byte[] bExp;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_CardCostHistory : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int totalSkyDestinyItem;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsc_SC_NewbieStep : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte idx;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.I2)]
    public short[] value;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_UnsettledPccaccyNode : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    //充值Id
    public int payId;
    //流水号
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SDKPccaccyResult.ORDERID_MAX_LEN)]
    public string szSerial;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_PccaccyRecordInfoNode : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    //充值Id
    public int payId;
    //充值时间
    public int time;

    public int ePayFromConfig;
}


#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct taskGain : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public short taskID;   //模版表
    public byte isFinsh;  //是否完成
    public byte isGain;   //是否领取
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct hotSaleInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int taskID;   //模版表
    public short buyTime;  //购买次数
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct taskInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte taskEnum;   //任务枚举
    public int value;  //
}


#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct HalfDiscInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte DayIdx;   //天数
    public short BuyTime;  //购买次数
}


#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsc_SC_GameNotice : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int uMsgId;
    public int uStartTime;
    public int uOverTime;
    public int uModal;        // 客户端锁屏
    public int uQuitGame;     // 客户端点击确认退出游戏
    //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.U2)]
    //public char[] szTitle;
    //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512, ArraySubType = UnmanagedType.U2)]
    //public char[] szContext;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
    public string szTitle;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
    public string szContext;

}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_LegionBattleInit : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte state; //0：未开启 1：初始化 2：匹配中 3：战斗中
    public int lastApplyTime; //军团战最近报名时间戳
    public byte myChallengeTimes; //本人已挑战次数
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_FashionInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long id;
    /// <summary>
    /// 模板Id
    /// </summary>
    public int tempId;
    /// <summary>
    /// 过期时间戳
    /// </summary>
    public int limitTime;
    /// <summary>
    /// 当前装备者
    /// </summary>
    public long equiperId;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_LegionBattleGateNode : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte memberNum;
    public byte starNum;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_LegionBattleInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long enemyLegionId; //敌方军团Id
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = LegionConst.LEGION_NAME_BYTE_NUM)]
    public string szEnemyLegionName; //敌方军团名字
    public byte isSelfNowWin; //自己单前是否领先

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.Struct)]
    public CMsg_SC_LegionBattleGateNode[] selfBattleInfo;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.Struct)]
    public CMsg_SC_LegionBattleGateNode[] enemyBattleInfo;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_LegionBattleDefenceNode : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long userId;
    public byte uStar; //[0,3]
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_LegionBattleTabeNode : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte ret; // 0:未分出胜负 1:A胜利 2：胜利
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = LegionConst.LEGION_NAME_BYTE_NUM)]
    public string szEnemyLegionNameA;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = LegionConst.LEGION_NAME_BYTE_NUM)]
    public string szEnemyLegionNameB;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_NewYearSellingInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int goodId;    // 商品id
    public short times;		// 购买次数
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_NewYearGiftInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int todayTimes;		// 今日领取次数
}


#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_NewYearStepInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int uScore;    // 剩余积分
    public int uStep;		// 已走步数
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_ValentineSendRoseInit : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int uScore;    //积分
    public int uBoxFlag;  // 宝箱领取进度十进制掩码 如110表示第1, 2个宝箱已经领取
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public int[] uNum;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_HistoryConst : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int uYYYYMMDD;   //年月日
    public int uCost;    //消费记录
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_TotalConsumotion : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int m_DTId;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_MammonGiftInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int m_DTId;
    public byte m_state;  //领取状态
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct RC_RankerUserPowerNode : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long userId;
    public short rank;
    public int iPraiseNum;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct RC_RankerUserDungeonStarNode : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long userId;
    public short rank;
    public short iCurChapterId;//当前章节
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct RC_RankerUserDungeonStarSelfNode : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int iStars;  //玩家副本星数
    public short iRank;  //副本星数排行，未上榜（前50）初始化为0
    public short iCurChapterId;  //当前章节
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct RC_RankerUserLevelNode : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long userId;
    public short rank;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct RC_RankerUserLevelSelfNode : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public short iRank;  //等级排行，未上榜（前50）初始化为0
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct RC_RankerUserPowerSelfNode : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long iPower;
    public short iRank;
    public int iPraiseNum0; //玩家今天点赞次数
    public int iPraiseNum1; //玩家今天被点赞次数
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = LegionConst.LEGION_NAME_BYTE_NUM)]
    public string szLegionName; //军团名字
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct RC_RankerLegionPowerNode : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long legionId;
    public short rank;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct RC_RankerLegionPowerSelf : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long legionId;
    public short rank;
    public long iPower; //军团战力
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct RC_LegionPowerInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long legionId;     //军团id
    public byte lv;          //军团等级
    public long legionPower; //军团战力
    public byte iconId; //军团图标
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = LegionConst.LEGION_NAME_BYTE_NUM)]
    public string szLegionName; //军团名字

    //军团长信息
    public long chiefId;//军团长Id
    public byte uChiefFrameId;//军团长边框id
    public byte uChiefVipLv;//军团长vip等级
    public uint chiefSex;//军团长时装Id
    public uint chiefLv;//军团长等级
    public uint chiefPower;//军团长战力
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]
    public string chiefName; //军团长名称   
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct OneKeyFragmentInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }

    public short treaFragId;
    public short awardId;
    public SC_GrabTreasureNode awardNode;
    public byte star;

    //public short treaFragId;//碎片状态，如果没有获得则为0
    //public short awardId;//奖励id
    //public SC_GrabTreasureNode awardNode;//奖励
    //public short star;//战斗星数
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct TariningInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int TariningTime;   //今日练兵次数  
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.Struct)]
    public TariningInfoItem[] m_TariningInfo;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct TariningInfoItem : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int TimeStamp;     //练兵时间戳
    public byte awardType;
    public int awardId;
    public int Num;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct TacticalInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int maxFormatId;  //当前第几个阵法
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.U1)]
    public byte[] formatInfo;      //阵容对应的阵法ID
}

//限时活动神装信息
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_GodDressInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public short uTotalScore;//累计积分
    public short uTodayScore;//今日购买次数
}

//神装宝箱
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_GodDressBox : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U1)]
    public byte[] uBoxState;//宝箱状态 0:不可领取 1:已领取  2:可领取
}

//神装排行榜信息
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_GodDressRankInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int idx;//我的排名
    public int uScore;//我的积分
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.Struct)]
    public RankUserInfo[] m_RankUserInfo;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct RankUserInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte idx;//排名
    public int uScore;//积分
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]
    public string szRoleName;   //玩家名字
}

//通天转盘基本信息
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct TurntableLotteryInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int uType;//类型
    public int uId;//id
    public int uNum;//数量
    public int iServerId;//服务器id
    public int iTime;//中奖时间
    public byte idx;//玩家索引
    public int tempId;//模板id
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]
    public string szRoleName;   //玩家名字
}
/// <summary>
/// 转盘奖池
/// </summary>
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_TurntableRemainSycee : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int uRemainSycee;//奖池数量
}



//通天转盘宝箱SC_TurntableBoxInfo
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_TurntableBoxInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int uLotteryTime;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.U1)]
    public byte[] uState;
}
//通天转盘抽奖返还
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct TurntableLottery : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int tempId;
    public int sycee;
}



#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_CrossBattleInfor : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte iZone;        //单前跨服战区
    public byte iCrossLv;           //跨服战等级（官衔）
    public byte iCrossIntegral;         //当前特级积分（当前官衔的积分）
    public short iLeftTimes;                    //今天剩余挑战次数
    public short iBuyTimes;             //今天已购买次数
    public byte iWinningTimes;      //当前连胜场次
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_CrossBattlePlayer : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long userId;
    public int iServerId;
    public int iCrossSvId; //TsuCode - Add Server id
    public int iTempId;
    public byte uFrameId;
    public byte uVipLv;
    public short lv;
    public int power;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]
    public string szRoleName;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_CrossBattleResult : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int iOperateResult;
    public byte iResult;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_GoodInfoNode : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int goodsId; //商品ID
    public int times; //购买次数
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_CrossCardBattleInfor : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte iBattleTimes;                   //今日剩余挑战次数
    public byte iReflashTimes;                  //今天剩余刷新次数
    public byte iWinningTimes;                  //胜战绩
    public byte iLostingTimes;                  //失败战绩
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_CrossCardBattleRandCard : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = CardBattlePool.CardBattleUseableNum, ArraySubType = UnmanagedType.I4)]
    public int[] aRandCardTempId;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_CrossUser : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long userId;
    public int iServerId;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I4)]
    public int[] aFormation;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]
    public string szRoleName;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_CrossCardBattlePlayer : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.Struct)]
    public CMsg_SC_CrossUser[] aPlayer;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_CrossCardBattleFormation : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.I4)]
    public int[] aCardTempId;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_CrossCardBattleResult : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int iOperateResult;
    public byte iResult;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_CrossRankNode : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public short uRankIdx;
    public short uRankTitle;
    public short uServerId;
    public long uPower;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]
    public string szRoleName;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_FestivalExchange : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int mId;

    public int mNum;

}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_DealsEveryDayInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int mId;

    public int mGet;

}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_ExclusionroTationInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int mcount;
    public int mendtime;
    public int mgetime;

}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_ExclusionSpinInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int mId;

    public int mGet;

}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_TowerEnter : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte uState;//当前挑战状态 0：失败 1：成功
    public byte uEnter;//是否进入状态 0：未进入 1：进入
    public byte uCurTower;//上一层塔层
    public byte uHighestTower;//历史最高塔层
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_TowerChallengeRet : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte uWin;//当前挑战状态 0：失败 1：成功
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_TowerInit : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte uEnter;//是否进入状态 0：未进入 1：进入
    public byte uFirst;  //是否第一次进入 0：否 1：是
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_SevenStarInfoNode : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int utype; //类型
    public int uId;   //Id
    public int uNum;  //数量
    public int iServerId; //服务器ID
    public int iTime; //时间
    public byte idx;    //玩家索引
    public int tempId;    //模板ID
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]
    public string szRoleName;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_SevenStarInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int uBlessValue;//祝福值
    public int uAwardRemain;    //奖池中剩余
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_SevenStarBlessLevelUp : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int uBlessValue;//祝福值
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CS_SevenStarGiftAwardKey : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int uid;//七星灯特殊奖励
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_SevenStarGiftAwardKey : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int uid;//七星灯特殊奖励
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
    public string szKey;//七星灯特殊奖励Key
}


#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_SevenStarGiftAwardList : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int uId;   //Id
    public int iTime;	//时间
}


#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_SevenStarLottery : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int uId;   //Id
    public int uAwardRemain;	//奖池中剩余
}


#region TsuCode - ChaosBattle

//TsuCode
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_ChaosBattleInfor : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte iZone;        //单前跨服战区
    public byte iCrossLv;           //跨服战等级（官衔）
    public byte iCrossIntegral;         //当前特级积分（当前官衔的积分）
    public short iLeftTimes;                    //今天剩余挑战次数
    public short iBuyTimes;             //今天已购买次数
    public byte iWinningTimes;      //当前连胜场次
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_ChaosBattlePlayer : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long userId;
    public int iServerId;
    public int iTempId;
    public byte uFrameId;
    public byte uVipLv;
    public short lv;
    public int power;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]
    public string szRoleName;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_ChaosBattleResult : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int iOperateResult;
    public byte iResult;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_ChaosRankNode : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public short uRankIdx;
    public short uRankTitle;
    public short uServerId;
    public long uPower;
    public long uChaosScore; //TsuCode - ChaosScore
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]
    public string szRoleName;
}
//----------------------
//-----Record-------------------------------

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_ChaosHistory : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long userId;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]
    public string userName;
    public int serverId;
    public long enemyId;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]
    public string enemyName;
    public int serverEnemyId;
    public int battleRes;
    public int recordTime;
    public int note;

}
#endregion TsuCode - ChaosBattle

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct EventOnlineVipInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int itype;
    public int idata1;
    public int idata2;
    public int idata3;
    public int idata4;

    public int ievent;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct EventTimeInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int itype;
    public int idata1;
    public int idata2;
    public int idata3;
    public int idata4;

    public int ieventtime;
    public int ievent;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_RankingPowerUser : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int rank;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]
    public string szRoleName;
    public int ift;

}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_RankingPowerMyRank : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int rank;
    public int score;

}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_RankingPowerList : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.Struct)]
    SC_RankingPowerUser[] RankingPowerUser;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_RankingBattlePassMyRank : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int rank;
    public int eventtimeId;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_RankingBattlePassUser : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int rank;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]
    public string szRoleName;
    public int ift;
}



    #region TsuCode - AFK module
#if UNITY_IPHONE
[System.Serializable]
#endif
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_AFKInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long afkTime;
    public int isFirst; //1: is first
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_AFKGetAward : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int awardType;
    public int awardId;
    public int awardNum;
}
#endregion TsuCode - AFK module


#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_GodEquipInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long id;            // 唯一Id
    public int tempId;        // 模板Id
    public byte stars;        // 星数
    public short expStars;     //升星经验
    public short lucky;        //幸运值
    public short lvIntensify;   //强化等级
    public short lvRefine;      //精炼等级
    public int expRefine;       //精炼经验
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_GodEquipFragmentInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long id;            // 唯一Id
    public int tempId;        // 模板Id
    public int num;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_GodEquipIntensify : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    long id;
    public byte realTimes;
    public byte critTimes;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_ArenaCrossInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long userId;
    public int uRank;
    public int uMainCard;
    public UserView userView;
    public UserExtra userExtra;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_CrossArenaResult : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int iOperateResult;
    public byte iResult;
    public int iResultScore;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_CrossArenaInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public byte iCrossLv;           //¿ç·þÕ½µÈ¼¶£¨¹ÙÏÎ£©
    public short iTimes;                    //½ñÌì¿ÉÌôÕ½´ÎÊý
    public short iWinTimes;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_CrossArenaRecordList : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int id;
    public long userId;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]
    public string userName;
    public int serverId;
    public long enemyId;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]
    public string enemyName;
    public int serverEnemyId;
    public int battleRes;
    public int recordTime;
    public int myChange;
    public int enemyChange;

}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_CrossTournamentRegeditRet : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }

    public int ret;	
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct TournamentFighterCard : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    //public long iId;
    public int tempId;
    public int uLv;         // 等级
    //public int uEvolveId;       // 进化Id
    //public int uAwakenLv;       // 觉醒等级
    //public int uAwakenFlag;	// 觉醒标识
    //public int iSkyDestiny;	    // 天命等级
    //public int m_iFanshionDressId;
    //public int uLvArtifact;	       // 神器等级

}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_CrossTournamentUserInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long userId;
    public int uRank;
    public int uMainCard;
    public UserView userView;
    public short iLv;
    public int iServerId;
    public int iBattlePower;
    public long totalDamage;
    public int iPoint;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = GameParamConst.MAX_FIGHT_POS, ArraySubType = UnmanagedType.Struct)]
    public TournamentFighterCard[] fighterCard;
    //public UserExtra userExtra;

}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_CrossTournamentGroupStageInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int id;
    public long userIdA;
    public long userIdB;
    public int wday;
    public int time;
    public int index;
    public int result;

}
//#if UNITY_IPHONE
//[System.Serializable]
//#endif
//[StructLayout(LayoutKind.Sequential, Pack = 1)]
//public struct SC_CrossTournamentUserList : SockBaseDT
//{
//    public SockBaseDT Clone()
//    {
//        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
//        return tGoodsPoolDT;
//    }

//}

//#if UNITY_IPHONE
//[System.Serializable]
//#endif
//[StructLayout(LayoutKind.Sequential, Pack = 1)]
//public struct SC_CrossTournamentGroupStageList : SockBaseDT
//{
//    public SockBaseDT Clone()
//    {
//        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
//        return tGoodsPoolDT;
//    }

//}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_CrossTournamentTheInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }

    public int id;
    public int iThe;
    public long userIdA;
    public long userIdB;
    public int winA;
    public int winB;
    public int result;
    public int recordNo;
    public long uTime;
    public long damageA;
    public long damageB;
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_CrossTournamentShopInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int goodsId;  
    public int times; 
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_CrossTournamentInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public short isOpen;
    public int iType;
    public long top1;
    public long top2;
    public long top3;
    public long top4;
    public byte hasRegister;
    public long time;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CrossTournamentTheBetInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long userId;    
    public int bet;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_CrossTournamentTheBetInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int m_node_type;
    public int id;
    public int iThe;
    public long userIdA;
    public long userIdB;
    public int betCountA;
    public int betCountB;
    public int betNumA;
    public int betNumB;
    public int result;
    public long stratTime;
    public long endTime;
    public CrossTournamentTheBetInfo myBet;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SC_CrossTournamentTheBetRet : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }

    public int ret;
    public int winNo;
    public int bet;
    public int countA;
    public int countB;
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct ShopEventAward : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int awardid;           
    public int idata1;
    public int idata2;
    public int idata3;
    public int idata4;
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct ShopEventAwardInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int num;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.Struct)]
    public ShopEventAward[] awards;
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct ShopEventTimeInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int itype;
    public int idata1;
    public int idata2;
    public int idata3;
    public int idata4;

    public int ieventtime;
    public ShopEventAwardInfo info;
}
#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_BuyShopEventTime : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public int eventId;
    public int awardId;
    public int count;
    public int num;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.Struct)]
    public SC_Award[] awards;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_ShopEndowInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long endDay;
    public long endWeek;
    public long endMonth;
    public int num;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.Struct)]
    public ShopEventAward[] awards;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CMsg_SC_ShopSeasonInfo : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }
    public long endTime;
    public int num;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.Struct)]
    public ShopEventAward[] awards;
}