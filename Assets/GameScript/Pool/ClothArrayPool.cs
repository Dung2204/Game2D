using UnityEngine;
using System.Collections;
using System;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 编队pool
/// </summary>
public class ClothArrayData : BaseProperty
{
    /// <summary>
    /// 编队位于阵容位位置绑定
    /// 对应关系
    /// 元素0对应布阵里面一手
    /// 元素1对应布阵里面二手
    /// 元素2对应布阵里面三手
    /// 元素3对应布阵里面四手
    /// 元素4对应布阵里面五手
    /// 元素5对应布阵里面六手
    /// EM_FormationPos代表在阵容里的位置，其可能的取值为：eFormationPos_Main主卡，eFormationPos_Assist1辅卡1，eFormationPos_Assist2辅卡2，eFormationPos_Assist3辅卡3
    /// eFormationPos_Assist4辅卡4，eFormationPos_Assist5辅卡5，值eFormationPos_INVALID表示该位置没有卡牌
    /// </summary>
    //public List<EM_FormationPos> m_aClothArrayTeamPoolID = new List<EM_FormationPos>();
    public Dictionary<EM_CloseArrayPos, EM_FormationPos> m_dicClothArrayToPos = new Dictionary<EM_CloseArrayPos, EM_FormationPos>();//记录编队与阵容位置对应关系
    public Dictionary<EM_CloseArrayPos, TeamPoolDT> m_dicClothArrayToTeam = new Dictionary<EM_CloseArrayPos, TeamPoolDT>();//记录编队与阵容poolDT对应关系(该位置没有卡牌则为null)
    public ClothArrayData() : base(0)
    {
        f_Init();
    }
    #region 数据管理
    public void f_Init()
    {
        RegSocketMessage();
    }

    protected void RegSocketMessage()
    {
        SC_ClothArrayInfo tSC_FormationInfo = new SC_ClothArrayInfo();
        GameSocket.GetInstance().f_RegMessage((int)SocketCommand.SC_FightPos, tSC_FormationInfo, Callback_SC_FightPos);
    }

    private void Callback_SC_FightPos(object Obj)
    {
        SC_ClothArrayInfo tServerData = (SC_ClothArrayInfo)Obj;
        byte[] serverData = tServerData.eClothArrayTeamPoolID;
        //m_aClothArrayTeamPoolID.Clear();
        m_dicClothArrayToPos.Clear();
        m_dicClothArrayToTeam.Clear();
        // xử lý mới
        // server trả 1 ds gồm 6 tướng nằm ở ô nào
        for (int i = 0; i < serverData.Length; i++)
        {
            //m_aClothArrayTeamPoolID.Add((EM_FormationPos)serverData[i]);
            // ô chưa tướng
            m_dicClothArrayToPos.Add((EM_CloseArrayPos)serverData[i], (EM_FormationPos)i);
            // ô chưa info tướng
            m_dicClothArrayToTeam.Add((EM_CloseArrayPos)serverData[i], Data_Pool.m_TeamPool.f_GetForId(i) as TeamPoolDT);
        }
        //for (int i = 0; i < serverData.Length; i++)
        //{
        //    m_aClothArrayTeamPoolID.Add((EM_FormationPos)serverData[i]);
        //    m_dicClothArrayToPos.Add((EM_CloseArrayPos)i, (EM_FormationPos)serverData[i]);
        //    m_dicClothArrayToTeam.Add((EM_CloseArrayPos)i, Data_Pool.m_TeamPool.f_GetForId(serverData[i]) as TeamPoolDT);
        //}
        Data_Pool.m_GuidancePool.m_OtherSave = true;
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_ClothArrayChanged);

        Data_Pool.m_CrossTournamentPool.CheckUpdateUser();
    }
    #endregion
    /// <summary>
    /// 传递位置信息
    /// </summary>
    /// <param name="data">传递排好的序号</param>
    public void CommitData(EM_FormationSlot[] data)
    {
        CreateSocketBuf tCreateSocketBuf = new CreateSocketBuf();
        if (data.Length != 7)
        {
Debug.Log("Problematic lineup，must be 6 cells");
        }
        tCreateSocketBuf.f_Add((byte)data[0]);
        tCreateSocketBuf.f_Add((byte)data[1]);
        tCreateSocketBuf.f_Add((byte)data[2]);
        tCreateSocketBuf.f_Add((byte)data[3]);
        tCreateSocketBuf.f_Add((byte)data[4]);
        tCreateSocketBuf.f_Add((byte)data[5]);
        tCreateSocketBuf.f_Add((byte)data[6]);
        byte[] bBuf = tCreateSocketBuf.f_GetBuf();
        GameSocket.GetInstance().f_SendBuf(SocketCommand.CS_FightPos, bBuf);
    }

    public EM_CloseArrayPos GetPosIndex(EM_FormationPos index)
    {
        foreach (KeyValuePair<EM_CloseArrayPos, EM_FormationPos> kvp in m_dicClothArrayToPos)
        {
            if (kvp.Value == index) return kvp.Key;
            
        }
        Debug.LogError("Lấy vị trí thất bại EM_FormationPos= "+index);
        return EM_CloseArrayPos.eCloseArray_PosOne;
    }
}
