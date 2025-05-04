using ccU3DEngine;
using System.Collections.Generic;

public class CardBattleEnemyPoolDT :BasePoolDT<long>
{
    public CardBattleEnemyPoolDT(long id,int serverId,string playerName,int[] clothArray)
    {
        iId = id;
        ServerId = serverId;
        ServerInforDT serverInfo = (ServerInforDT)glo_Main.GetInstance().m_SC_Pool.m_ServerInforSC.f_GetSC(serverId);
        ServerName = serverInfo != null? serverInfo.szChannel:string.Empty;
        PlayerName = playerName;
        ClothPoolDtList = new List<CardBattleClothPoolDT>();
        for (int i = 0; i < clothArray.Length; i++)
        {
            ClothPoolDtList.Add(new CardBattleClothPoolDT(i, clothArray[i]));
        }
    }

    public int ServerId
    {
        get;
        private set;
    }

    public string ServerName
    {
        get;
        private set;
    }

    public string PlayerName
    {
        get;
        private set;
    }

    public List<CardBattleClothPoolDT> ClothPoolDtList
    {
        get;
        private set;
    }
}
