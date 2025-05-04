using UnityEngine;
using System.Collections;

public class GetWayToBattleParam{

    public EM_GetWayToBattle em_GetWayToBattle = EM_GetWayToBattle.None;

    public EM_ResourceType m_GetWayResourceType;

    public int m_GetWayResourId;

    public void f_UpdateDataInfo(EM_GetWayToBattle em_GetWayToBattle,EM_ResourceType m_GetWayResourceType, int m_GetWayResourId)
    {
        this.em_GetWayToBattle = em_GetWayToBattle;
        this.m_GetWayResourceType = m_GetWayResourceType;
        this.m_GetWayResourId = m_GetWayResourId;
    }
    public void f_Empty()
    {
        em_GetWayToBattle = EM_GetWayToBattle.None;
    }
}
