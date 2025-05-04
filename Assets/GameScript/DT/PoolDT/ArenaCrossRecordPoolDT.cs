using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class ArenaCrossRecordPoolDT : BasePoolDT<long>
{
    public ArenaCrossRecordPoolDT()
    {

    }
  
    private CMsg_SC_CrossArenaRecordList mArenaCrossRecord;
    public CMsg_SC_CrossArenaRecordList m_ArenaCrossRecord
    {
        get
        {
            return mArenaCrossRecord;
        }
    }

    public void f_UpdateArenaCrossRecord(CMsg_SC_CrossArenaRecordList msg_ArenaCrossRecord)
    {
        mArenaCrossRecord = msg_ArenaCrossRecord;

        ServerInforDT serverInfo = (ServerInforDT)glo_Main.GetInstance().m_SC_Pool.m_ServerInforSC.f_GetSC(mArenaCrossRecord.serverId);
        m_ServerName = serverInfo != null ? serverInfo.szName : string.Empty;
        ServerInforDT serverInfo1 = (ServerInforDT)glo_Main.GetInstance().m_SC_Pool.m_ServerInforSC.f_GetSC(mArenaCrossRecord.serverEnemyId);
        m_ServerNameEnemy = serverInfo != null ? serverInfo.szName : string.Empty;
    }

    public string ServerName
    {
        get
        {
            return m_ServerName;
        }
    }
    private string m_ServerName;

    public string ServerNameEnemy
    {
        get
        {
            return m_ServerNameEnemy;
        }
    }
    private string m_ServerNameEnemy;
}
