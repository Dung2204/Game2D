using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class PatrolPoolDT : BasePoolDT<long>
{
    public PatrolPoolDT(long userId)
    {
        iId = userId;
        isSelf = userId == Data_Pool.m_UserData.m_iUserId;
        List<NBaseSCDT> landInitList = glo_Main.GetInstance().m_SC_Pool.m_PatrolLandSC.f_GetAll();
        Data_Pool.m_PatrolPool.f_UpdateLandTotalNum(landInitList.Count);
        patrolLands = new PatrolLandNode[landInitList.Count];
        PatrolLandDT tItem;
        PatrolLandDT tUnlockItem;
        for (int i = 0; i < landInitList.Count; i++)
        {
            tUnlockItem = null;
            tItem = (PatrolLandDT)landInitList[i];
            if (tItem.iUnlock > 0)
                tUnlockItem = (PatrolLandDT)glo_Main.GetInstance().m_SC_Pool.m_PatrolLandSC.f_GetSC(tItem.iUnlock);
            patrolLands[i] = new PatrolLandNode(tUnlockItem == null ? string.Empty :tUnlockItem.szName,tItem);
        }
    }
    private bool isSelf;

    /// <summary>
    /// 是否是玩家自己的巡逻数据
    /// </summary>
    public bool m_bIsSelf
    {
        get
        {
            return isSelf;
        }
    }

    PatrolLandNode[] patrolLands;
    public PatrolLandNode[] m_PatrolLands
    {
        get
        {
            return patrolLands;
        }
    }

    public void f_UpdateLandInfo(int landId,int lv,int totalHours,int cardId,int patrolId,int beginTime,byte isRiot,byte isAward)
    {
        PatrolLandNode tUnlockItem;
        for (int i = 0; i < patrolLands.Length; i++)
        {
            tUnlockItem = null;
            if (patrolLands[i].m_iTemplateId == landId)
            {
                tUnlockItem = f_GetPatrolLandNodeByUnlock(patrolLands[i].m_iTemplateId);
                if (tUnlockItem != null)
                    tUnlockItem.f_Unlock();
                if(patrolLands[i].m_Template.iUnlock > 0)
                {
                    tUnlockItem = f_GetPatrolLandNode(patrolLands[i].m_Template.iUnlock);
                }
                EM_PatrolState unlockState = tUnlockItem == null ? EM_PatrolState.Lock : tUnlockItem.m_State;
                patrolLands[i].f_UpdateInfo(unlockState,lv,totalHours,cardId,patrolId,beginTime,isRiot > 0,isAward);
                return;
            }
        }
MessageBox.ASSERT("Update patrol data does not exist ,LandId:" + landId);
    }

    public void f_UpdateLandEventInfo(int landId, CMsg_SC_PatrolEventInfoNode eventInfo)
    {
        PatrolLandNode tNode = f_GetPatrolLandNode(landId);
        if (tNode == null)
MessageBox.ASSERT("No corresponding land data found, Id:" + landId);
        tNode.f_AddEvent(eventInfo);
    }

    public PatrolLandNode f_GetPatrolLandNode(int landId)
    {
        for (int i = 0; i < patrolLands.Length; i++)
        {
            if (patrolLands[i].m_iTemplateId == landId)
            {
                return patrolLands[i];
            }
        }
        return null;
    }

    public PatrolLandNode f_GetPatrolLandNodeByUnlock(int unlockId)
    {
        for (int i = 0; i < patrolLands.Length; i++)
        {
            if (patrolLands[i].m_Template != null && patrolLands[i].m_Template.iUnlock == unlockId)
                return patrolLands[i];
        }
        return null;
    }
}
