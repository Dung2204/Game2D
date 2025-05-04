using UnityEngine;
using System.Collections;
using System;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 查看好友阵容pool
/// </summary>
public class ViewPlayerLineUpPool : BasePool
{
    public Dictionary<EM_FormationPos, PlayerTeamItem> mDicTeamPoolDT = new Dictionary<EM_FormationPos, PlayerTeamItem>();//保存数据
    public ViewPlayerLineUpPool() : base("TeamPoolDT", true)
    {

    }
    #region Pool数据管理

    protected override void f_Init()
    {
    }

    protected override void RegSocketMessage()
    {
        OtherCardInfo tOtherCardInfo = new OtherCardInfo();
        GameSocket.GetInstance().f_RegMessage_Int3((int)SocketCommand.SC_QueryOtherFormation, tOtherCardInfo, CallbackSC_FormationInfo);
    }
    /// <summary>
    /// 服务器回调
    /// </summary>
    /// <param name="iData1"></param>
    /// <param name="iData2"></param>
    /// <param name="iNum"></param>
    /// <param name="aData"></param>
    protected void CallbackSC_FormationInfo(int iData1, int iData2, int iData3, int iNum, ArrayList aData)
    {
        mDicTeamPoolDT.Clear();
        for(int i = 0; i < aData.Count; i++)
        {
            SockBaseDT tData = aData[i] as SockBaseDT;
            OtherCardInfo tOtherCardInfo = (OtherCardInfo)tData;
            PlayerTeamItem item = new PlayerTeamItem();
            item.mPos = (EM_FormationPos)i;
            if((EM_FormationPos)i == EM_FormationPos.eFormationPos_Main)
                item.mFashId = iData3;
            else
                item.mFashId = 0;
            item.mCardId = tOtherCardInfo.tempId;
            if(tOtherCardInfo.evolveId > 0)
            {
                item.miEvolveLv = (glo_Main.GetInstance().m_SC_Pool.m_CardEvolveSC.f_GetSC(tOtherCardInfo.evolveId) as CardEvolveDT).iEvoLv;
            }
            else
            {
                item.miEvolveLv = 0;
            }
            item.mAwakeLevel = tOtherCardInfo.lvAwaken;
            for(int j = 1; j <= 7; j++)
            {
                PlayerTeamEquipItem EquipItem = new PlayerTeamEquipItem();
                if (j == 5 || j == 6)//法宝
                {
                    OtherTreasureInfo treasureInfo = tOtherCardInfo.otherTreasureInfo[j - 5];
                    if (treasureInfo.tempId <= 0)
                    {
                        EquipItem = null;
                    }
                    else
                    {
                        EquipItem.EquipId = treasureInfo.tempId;
                        EquipItem.EquipLevel = treasureInfo.lvIntensify;
                    }
                }
                else if (j == 7)//法宝
                {
                    OtherGodEquipInfo godequipInfo = tOtherCardInfo.otherGodEquipInfo[j - 7];
                    if (godequipInfo.tempId <= 0)
                    {
                        EquipItem = null;
                    }
                    else
                    {
                        EquipItem.EquipId = godequipInfo.tempId;
                        EquipItem.EquipLevel = godequipInfo.lvIntensify;
                    }
                }
                else//装备
                {
                    OtherEquipInfo equipInfo = tOtherCardInfo.otherEquipInfo[j - 1];
                    if (equipInfo.tempId <= 0)
                    {
                        EquipItem = null;
                    }
                    else
                    {
                        EquipItem.EquipId = equipInfo.tempId;
                        EquipItem.EquipLevel = equipInfo.lvIntensify;
                    }
                }
                item.dicEquipItem.Add((EM_EquipPart)j, EquipItem);
            }
            mDicTeamPoolDT.Add((EM_FormationPos)i, item);
        }
    }

    protected override void f_Socket_AddData(SockBaseDT Obj, bool bNew)
    {
        throw new NotImplementedException();
    }

    protected override void f_Socket_UpdateData(SockBaseDT Obj)
    {
        throw new NotImplementedException();
    }
    #endregion
    #region 外部接口调用
    /// <summary>
    /// 请求玩家的阵容信息
    /// </summary>
    /// <param name="friendId">玩家id</param>
    /// <param name="tSocketCallbackDT">回调</param>
    public void f_QueryPlayerTeam(long friendId, SocketCallbackDT tSocketCallbackDT)
    {
        GameSocket.GetInstance().f_RegCommandReturn((int)SocketCommand.CS_QueryOtherFormation, tSocketCallbackDT.m_ccCallbackSuc, tSocketCallbackDT.m_ccCallbackFail);
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        tCreateSocketBuf.f_Add(friendId);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_QueryOtherFormation, bBuf);
    }
    #endregion
}
public class PlayerTeamItem
{
    public EM_FormationPos mPos;//位置
    public int mFashId;   //时装ID
    public int mCardId;//卡牌id
    public int miEvolveLv;//卡牌突破等级
    public int mAwakeLevel;//主卡领悟等级
    public Dictionary<EM_EquipPart, PlayerTeamEquipItem> dicEquipItem = new Dictionary<EM_EquipPart, PlayerTeamEquipItem>();//阵容数据
}
public class PlayerTeamEquipItem
{
    public int EquipId;//装备/法宝id
    public int EquipLevel;//装备/法宝等级
}
