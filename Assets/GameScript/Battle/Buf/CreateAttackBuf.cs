using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using ccU3DEngine;


#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct stRoleInfor : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }

    public long m_iId;
    public int m_iRoleType;
    public int m_iTempId;
    public int m_iPos;
    public long m_iMaxHp;
    public int m_iFanshionDressId;
    public int m_iGodEquipSkillId;
    public int m_iAnger;
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct stFightElementInfor : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }

    public long m_iId;
    public int m_iPos;
    public int m_iSide;
    public int m_iAnger;
    public int m_iMagicId;

    public override string ToString()
    {
        return "m_iId=" + m_iId + ",m_iPos=" + m_iPos + ",m_iSide=" + m_iSide + ",m_iAnger=" + m_iAnger + ",m_iMagicId=" + m_iMagicId;
    }
}

#if UNITY_IPHONE
[System.Serializable]
#endif
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct stBeAttackInfor : SockBaseDT
{
    public SockBaseDT Clone()
    {
        SockBaseDT tGoodsPoolDT = (SockBaseDT)MemberwiseClone();
        return tGoodsPoolDT;
    }

    public long  m_iId;
    public byte m_iCirt;
    public int m_iHp;

    //效果类型 1，物理伤害 2，魔法伤害 3，治疗 （伤害和治疗效果=攻击*（x1+x2）+y）
    //4，减少x怒气 5，增加x怒气 6，清除中毒和灼烧效果 7，清除不利BUFF 8，清除增益BUFF     
    public byte m_iAnger;

    public int m_iBuf1;
    //public int m_iBuf2;
    /// <summary>
    /// 0基本攻击伤害 1附加伤害 2前段BUF显示效果 3前段BUF伤害 4后段BUF伤害 5后段BUF显示效果
    /// </summary>
    public byte m_iIsBaseAttack;
}

public class RoleAttack
{
    public RoleAttack()
    {
        m_iTrajectoryC = 0;
        m_iTrajectoryH = 0;
        m_EM_AttackStep = EM_AttackStep.AttackC;
        m_iMaxHarmNum = 1;
        m_iCurHarmNum = 0;
    }

    public long m_iId;
    public int m_iMagicId;
    public int m_iStayPos;
    public int m_iAnger;
    public Vector3 m_v3AttackPos;
    //public int m_iCount;        

    public int m_iTrajectoryC;
    public int m_iTrajectoryH;

    public int m_iMaxHarmNum;
    public int m_iCurHarmNum;
    public int m_iMagicAttackPos;
    public int[] m_aHarmPercent;
    public int m_iMagicType;
    public int m_iMagicSound;
    public int m_iIsActiveGodEquipSkill;
    public int m_iIsActiveFightElementSkill;
    public stFightElementInfor m_tFightElement;

    /// <summary>
    /// 合体技背景颜色 
    /// 2，绿。3，蓝。4，紫。5，橙。6，红。7，金
    /// </summary>
    public int m_iFitMagic;

    public EM_AttackStep m_EM_AttackStep;       
    public List<stBeAttackInfor> m_aData = new List<stBeAttackInfor>();


    public bool CreateHarmData()
    {
        if (m_iMaxHarmNum == 0)
        {
            m_iMaxHarmNum = 1;
        }
        m_aHarmPercent = new int[m_iMaxHarmNum];
        
        if (m_iMaxHarmNum == 1)
        {
            m_aHarmPercent[0] = 10;
        }
        else if (m_iMaxHarmNum == 2)
        {
            m_aHarmPercent[0] = 4;
            m_aHarmPercent[1] = 6;
        }
        else if (m_iMaxHarmNum == 3)
        {
            m_aHarmPercent[0] = 2;
            m_aHarmPercent[1] = 3;
            m_aHarmPercent[2] = 5;
        }
        else if (m_iMaxHarmNum == 4)
        {
            m_aHarmPercent[0] = 1;
            m_aHarmPercent[1] = 2;
            m_aHarmPercent[2] = 3;
            m_aHarmPercent[3] = 4;
        }
        else if (m_iMaxHarmNum == 5)
        {
            m_aHarmPercent[0] = 1;
            m_aHarmPercent[1] = 1;
            m_aHarmPercent[2] = 2;
            m_aHarmPercent[3] = 2;
            m_aHarmPercent[4] = 4;
        }
        else if (m_iMaxHarmNum == 6)
        {
            m_aHarmPercent[0] = 1;
            m_aHarmPercent[1] = 1;
            m_aHarmPercent[2] = 1;
            m_aHarmPercent[3] = 2;
            m_aHarmPercent[4] = 2;
            m_aHarmPercent[5] = 3;
        }
        else if (m_iMaxHarmNum == 7)
        {
            m_aHarmPercent[0] = 1;
            m_aHarmPercent[1] = 1;
            m_aHarmPercent[2] = 1;
            m_aHarmPercent[3] = 1;
            m_aHarmPercent[4] = 1;
            m_aHarmPercent[5] = 2;
            m_aHarmPercent[6] = 3;
        }
        else if (m_iMaxHarmNum == 8)
        {
            m_aHarmPercent[0] = 1;
            m_aHarmPercent[1] = 1;
            m_aHarmPercent[2] = 1;
            m_aHarmPercent[3] = 1;
            m_aHarmPercent[4] = 1;
            m_aHarmPercent[5] = 1;
            m_aHarmPercent[6] = 2;
            m_aHarmPercent[7] = 3;
        }
        else if (m_iMaxHarmNum == 9)
        {
            m_aHarmPercent[0] = 1;
            m_aHarmPercent[1] = 1;
            m_aHarmPercent[2] = 1;
            m_aHarmPercent[3] = 1;
            m_aHarmPercent[4] = 1;
            m_aHarmPercent[5] = 1;
            m_aHarmPercent[6] = 1;
            m_aHarmPercent[7] = 1;
            m_aHarmPercent[8] = 2;
        }
        else if (m_iMaxHarmNum == 10)
        {
            m_aHarmPercent[0] = 1;
            m_aHarmPercent[1] = 1;
            m_aHarmPercent[2] = 1;
            m_aHarmPercent[3] = 1;
            m_aHarmPercent[4] = 1;
            m_aHarmPercent[5] = 1;
            m_aHarmPercent[6] = 1;
            m_aHarmPercent[7] = 1;
            m_aHarmPercent[8] = 1;
            m_aHarmPercent[9] = 1;
        }
        else
        {
            return false;
        }
        return true;
    }

    public int f_GetCurHarmPercent(bool bAutoAdd = true)
    {
        if (m_iCurHarmNum >= m_aHarmPercent.Length)
        {
MessageBox.ASSERT("Passing damage, id: " + m_iMagicId);
            return 0;
        }
        int iA = m_aHarmPercent[m_iCurHarmNum];
        if (bAutoAdd)
        {
            m_iCurHarmNum++;
        }
        return iA;
    }


    public bool f_CheckIsEnd()
    {
        if (m_iCurHarmNum >= m_aHarmPercent.Length)
        {
            return true;
        }
        return false;
    }


}

public class BattleTurn
{
    public int m_iCurTurns;
    private int _iAttackIndex;
    public List<RoleAttack> m_aTurnData = new List<RoleAttack>();

    public BattleTurn(int iCurTurns)
    {
        m_iCurTurns = iCurTurns;
        _iAttackIndex = 0;
    }    
    
    public RoleAttack f_GetCurRoleAttack()
    {
        if (_iAttackIndex == m_aTurnData.Count)
        {
            return null;
        }
        return m_aTurnData[_iAttackIndex++];
    }

    public RoleAttack f_GetLastRoleAttack()
    {
        if (_iAttackIndex == m_aTurnData.Count)
        {
            return null;
        }
        return m_aTurnData[_iAttackIndex];
    }

    /// <summary>
    /// 跳过战斗专用接口！！！（为了保证跳过时丢掉还在演示的战斗数据）
    /// </summary>
    public void f_SetLastAttackIndex() {
        _iAttackIndex--;
        if (_iAttackIndex < 0) {
            _iAttackIndex = 0;
        }
    }
}

public class CreateAttackBuf
{
    ReadBuf _ReadBuf = new ReadBuf();
    List<BattleTurn> _aBattleTurnData = new List<BattleTurn>();
    stRoleInfor[] _aRoleList = new stRoleInfor[40];
    stFightElementInfor[] _aElementList = new stFightElementInfor[4];
    int[] _totalDamage = new int[14];
    private int _iMaxTurn = 0;
    public void f_Reset()
    {
        _aBattleTurnData.Clear();
        _ReadBuf.f_Reset();
        _iMaxTurn = 0;
    }

    public bool f_CheckIsLoadSuc()
    {
        if (_iMaxTurn == 0)
        {
            return false;
        }
        return true;            
    }

    public void f_Create(byte[] aBuf)
    {
        int iCurTurns = 0;
        int iBattleResult = 0;
        int iCount = 0;
        int iTurnRoleNum;

MessageBox.DEBUG("Battle Data");
        _ReadBuf.f_Save(aBuf, aBuf.Length);
        for (int i = 0; i < 40; i++)
        {
            _aRoleList[i] = (stRoleInfor)_ReadBuf.f_ReadObj(typeof(stRoleInfor));
        }
        // 4 nguyên tố trùng kích của 2 đội
        for (int i = 0; i < 4; i++)
        {
            _aElementList[i] = (stFightElementInfor)_ReadBuf.f_ReadObj(typeof(stFightElementInfor));
        }
        _iMaxTurn = _ReadBuf.f_ReadInt();
        iBattleResult = _ReadBuf.f_ReadInt();
        // tạm thời lấy 7 tướng
        for (int i = 0; i < 14; i++)
        {
            _totalDamage[i] = _ReadBuf.f_ReadInt();
        }

        for (int i = 0; i < _iMaxTurn; i++)
        {
            iCurTurns = _ReadBuf.f_ReadInt();
            iTurnRoleNum = _ReadBuf.f_ReadInt();
            BattleTurn tBattleTurn = new BattleTurn(iCurTurns);
            for (int iRoleNum = 0; iRoleNum < iTurnRoleNum; iRoleNum++)
            {
                RoleAttack tBattleTurnBuf = new RoleAttack();
                tBattleTurnBuf.m_iId = _ReadBuf.f_ReadLong();
                tBattleTurnBuf.m_iMagicId = _ReadBuf.f_ReadInt();
                tBattleTurnBuf.m_iStayPos = _ReadBuf.f_ReadInt();
                tBattleTurnBuf.m_iAnger = _ReadBuf.f_ReadInt();
                tBattleTurnBuf.m_iIsActiveGodEquipSkill = _ReadBuf.f_ReadInt();
                tBattleTurnBuf.m_iIsActiveFightElementSkill = _ReadBuf.f_ReadInt();
                // thêm update nộ nguyên tố
                tBattleTurnBuf.m_tFightElement = (stFightElementInfor)_ReadBuf.f_ReadObj(typeof(stFightElementInfor));
                iCount = _ReadBuf.f_ReadInt();
                for (int iN = 0; iN < iCount; iN++)
                {
                    stBeAttackInfor tstBeAttackInfor = (stBeAttackInfor)_ReadBuf.f_ReadObj(typeof(stBeAttackInfor));
                    tBattleTurnBuf.m_aData.Add(tstBeAttackInfor);
                }
                tBattleTurn.m_aTurnData.Add(tBattleTurnBuf);
            }
            _aBattleTurnData.Add(tBattleTurn);
        }
MessageBox.DEBUG(iBattleResult + "half" + _aBattleTurnData.Count);
    }


    public stRoleInfor[] f_GetRoleList()
    {
        return _aRoleList;
    }
    
    public BattleTurn f_GetBattleTurn(int iTurns)
    {
        if (iTurns > _aBattleTurnData.Count)
        {
            return null;
        }
        return _aBattleTurnData[iTurns];
    }

    public int f_GetMaxTurn()
    {
        return _iMaxTurn;
    }

    public int[] f_GetDPS() {
        return _totalDamage;
    }
    /// <summary>
    /// nguyên tố trùng kích
    /// </summary>
    public stFightElementInfor[] f_GetFightElementList()
    {
        return _aElementList;
    }
}
