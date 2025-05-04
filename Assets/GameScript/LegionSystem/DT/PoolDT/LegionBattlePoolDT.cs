using ccU3DEngine;

public class LegionBattlePoolDT : BasePoolDT<long>
{
    public LegionBattlePoolDT(bool isSelf)
    {
        iId = isSelf ? 0 : 1;
        this.isSelf = isSelf;
        gateNodes = new LegionBattleGateNode[(int)EM_LegionGate.End];
        for (int i = 0; i < (int)EM_LegionGate.End; i++)
        {
            gateNodes[i] = new LegionBattleGateNode((EM_LegionGate)i); 
        }
    }

    /// <summary>
    /// 是否单前领先 （暂时没用）
    /// </summary>
    public byte isNowWin;

    private long legionId;
    /// <summary>
    /// 军团Id
    /// </summary>
    public long m_lLegionId
    {
        get
        {
            return legionId;
        }
    }

    private string legionName;
    /// <summary>
    /// 军团名字
    /// </summary>
    public string m_szLegionName
    {
        get
        {
            if (legionName == null)
                return string.Empty;
            return legionName;
        }
    }


    private bool isSelf;
    /// <summary>
    /// 是否是自己的军团战数据
    /// </summary>
    public bool m_bIsSelf
    {
        get
        {
            return isSelf;
        }
    }

    private int starNum;
    /// <summary>
    /// 星数
    /// </summary>
    public int m_iStarNum
    {
        get
        {
            int tStarNum = 0;
            for (int i = 0; i < gateNodes.Length; i++)
            {
                tStarNum += gateNodes[i].m_iStarNum;
            }
            return tStarNum;
        }
    }

    private LegionBattleGateNode[] gateNodes;
    public LegionBattleGateNode f_GetGateNode(EM_LegionGate gate)
    {
        int tGateIdx = (int)gate;
        if (tGateIdx >= 0 && tGateIdx < gateNodes.Length)
        {
            return gateNodes[tGateIdx];
        }
        else
        {
            MessageBox.ASSERT("try get invalid gate node");
            return gateNodes[(int)EM_LegionGate.Invalid];
        }
    }

    public void f_UpdateGateNode(EM_LegionGate gate,byte memberNum,byte starNum)
    {
        LegionBattleGateNode tNode = f_GetGateNode(gate);
        tNode.f_UpdateInfo(memberNum,starNum);
    }

    public void f_UpdateGateNodeDefenerNode(EM_LegionGate gate,int defenderIdx,long userId,byte starNum)
    {
        LegionBattleGateNode tNode = f_GetGateNode(gate);
        tNode.f_UpdateDefenderInfo(defenderIdx,userId,starNum);
    }

    public void f_UpdateLegionBaseInfo(long legionId, string legionName)
    {
        this.legionId = legionId;
        this.legionName = legionName;
    }
}

public class LegionBattleGateNode
{
    public LegionBattleGateNode(EM_LegionGate gate)
    {
        gateType = gate;
        //改了每个防守者最大值 要修改LegionConst.LEGION_BATTLE_SIGNUP_MEMBERNUM_LIMIT
        switch (gateType)
        {
            case EM_LegionGate.Inside:
gateName = "Cổng Nội Thành";
                memberMaxNum = 1;
                break;
            case EM_LegionGate.LeftGate:
gateName = "Cổng thành trái";
                memberMaxNum = 7;
                break;
            case EM_LegionGate.RightGate:
gateName = "Cổng thành phải";
                memberMaxNum = 7;
                break;
            case EM_LegionGate.MainGate:
gateName = "Cổng chính";
                memberMaxNum = 7;
                break;
            default:
                gateName = string.Empty;
                memberMaxNum = 0;
                break;
        }
        starNum = 0;
        memberNum = 0;
        starMaxNum = memberMaxNum * 3;
        //初始化城门防守者
        defenderNodes = new LegionBattleGateDefenderNode[memberMaxNum];
        for (int i = 0; i < defenderNodes.Length; i++)
        {
            defenderNodes[i] = new LegionBattleGateDefenderNode(gateType,i);
        }
    }

    /// <summary>
    /// 更新gateInfo 
    /// </summary>
    /// <param name="memberNum">成员数</param>
    /// <param name="starNum">星数</param>
    public void f_UpdateInfo(byte memberNum,byte starNum)
    {
        this.memberNum = memberNum;
        this.starNum = starNum;
    }

    /// <summary>
    /// 更新gate的DefenderInfo
    /// </summary>
    /// <param name="idx"></param>
    public void f_UpdateDefenderInfo(int defenderIdx,long userId,byte starNum)
    {
        if (defenderIdx >= 0 && defenderIdx < defenderNodes.Length)
        {
            BasePlayerPoolDT tPlayerInfo = (BasePlayerPoolDT)Data_Pool.m_GeneralPlayerPool.f_GetForId(userId);
            if (userId == Data_Pool.m_UserData.m_iUserId)
            {
                LegionTool.f_SelfPlayerInfoDispose(ref tPlayerInfo);
            }
            if (tPlayerInfo == null)
                MessageBox.ASSERT("LegionBattleDefenderNode player info not found");
            defenderNodes[defenderIdx].f_UpdateInfo(tPlayerInfo, starNum);
        }
        else
        {
            MessageBox.ASSERT("try update invalid gate defender node");
        }
    }
    private LegionBattleGateDefenderNode[] defenderNodes;
    public LegionBattleGateDefenderNode[] m_DefenderNodes
    {
        get
        {
            return defenderNodes;
        }
    }


    #region GateInfo

    private int memberNum;
    /// <summary>
    /// 可挑战成员数
    /// </summary>
    public int m_iMemberNum
    {
        get
        {
            return memberNum;
        }
    }

    private int starNum;
    /// <summary>
    /// 该城门的星数总览
    /// </summary>
    public int m_iStarNum
    {
        get
        {
            return starNum;
        }
    }

    #endregion

    #region 配置数据（前端固定写）

    private EM_LegionGate gateType;
    private string gateName;
    private int memberMaxNum;
    private int starMaxNum;

    public EM_LegionGate m_GateType
    {
        get
        {
            return gateType;
        }
    }

    public string m_szGateName
    {
        get
        {
            return gateName;
        }
    }

    public int m_iMemberMaxNum
    {
        get
        {
            return memberMaxNum;
        }
    }

    public int m_iStarMaxNum
    {
        get
        {
            return starMaxNum;
        }
    }

    #endregion
}

public class LegionBattleGateDefenderNode
{
    public LegionBattleGateDefenderNode(EM_LegionGate gate,int idx)
    {
        playerInfo = null;
        gateType = gate;
        this.idx = idx;
    }
    private EM_LegionGate gateType;
    /// <summary>
    /// 门的类型
    /// </summary>
    public EM_LegionGate m_GateType
    {
        get
        {
            return gateType;
        }
    }

    private int idx;
    /// <summary>
    /// 下标
    /// </summary>
    public int m_iIdx
    {
        get
        {
            return idx;
        }
    }

    private BasePlayerPoolDT playerInfo;
    /// <summary>
    /// 玩家信息
    /// </summary>
    public BasePlayerPoolDT m_PlayerInfo
    {
        get
        {
            return playerInfo;
        }
    }

    private int starNum;
    /// <summary>
    /// 星数
    /// </summary>
    public int m_iStarNum
    {
        get
        {
            return starNum;
        }
    }

    /// <summary>
    /// 星数最大值
    /// </summary>
    public int m_iStarMaxNum
    {
        get
        {
            return 3;
        }
    }

    public void f_UpdateInfo(BasePlayerPoolDT playerInfo,int starNum)
    {
        this.playerInfo = playerInfo;
        this.starNum = starNum;
    }
}

public class LegionBattleTableNode : BasePoolDT<long>
{
    public LegionBattleTableNode(int idx,EM_LegionTableRet ret, string legionNameA, string legionNameB)
    {
        this.idx = idx;
        this.ret = ret;
        this.legionNameA = legionNameA;
        this.legionNameB = legionNameB;
    }

    private int idx;
    public int m_iIdx
    {
        get
        {
            return idx;
        }
    }

    private EM_LegionTableRet ret;
    public EM_LegionTableRet m_Ret
    {
        get
        {
            return ret;
        }
    }

    private string legionNameA;
    public string m_szLegionNameA
    {
        get
        {
            return legionNameA;
        }
    }


    private string legionNameB;
    public string m_szLegionNameB
    {
        get
        {
            return legionNameB;
        }
    }
}
