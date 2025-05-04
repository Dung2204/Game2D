using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BattleDataPool
{
    private CreateAttackBuf _CreateAttackBuf = new CreateAttackBuf();

    public BattleDataPool()
    {
        RegSocketMessage();
    }


    void RegSocketMessage()
    {
        SC_FightRet tSC_FightRet = new SC_FightRet();
        GameSocket.GetInstance().f_AddListener_Buf_Int2_V2((int)SocketCommand.SC_FightRet, tSC_FightRet, Callback_SocketData_BattleData);

    }

    void Callback_SocketData_BattleData(int iData1, int iData2, int iLen, byte[] aBuf)
    {
        byte[] aZipBuf = ZipTools.aaa557788(aBuf, iData1);
        _CreateAttackBuf.f_Reset();
        //数据包
        _CreateAttackBuf.f_Create(aZipBuf);

    }

    public stRoleInfor[] f_GetRoleList()
    {
        return _CreateAttackBuf.f_GetRoleList();
    }
       
    public BattleTurn f_GetBattleTurn(int iTurns)
    {
        return _CreateAttackBuf.f_GetBattleTurn(iTurns);
    }

    public int f_GetMaxTurn()
    {
        return _CreateAttackBuf.f_GetMaxTurn();
    }
    
    public void f_Reset()
    {
        _CreateAttackBuf.f_Reset();
    }

    public bool f_CheckIsLoadSuc()
    {
        return _CreateAttackBuf.f_CheckIsLoadSuc();
    }

    public int[] f_GetDPS()
    {
        return _CreateAttackBuf.f_GetDPS();
    }

    public stFightElementInfor[] f_GetFightElementList()
    {
        return _CreateAttackBuf.f_GetFightElementList();
    }
}