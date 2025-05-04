using ccU3DEngine;
using System.Collections.Generic;

public class PatrolLandNode
{
    public PatrolLandNode(string unlockLandName, PatrolLandDT template)
    {
        this.unlockLandName = unlockLandName;
        templateId = template.iId;
        this.template = template;
        if (template.iUnlock > 0)
            state = EM_PatrolState.Lock;
        else
            state = EM_PatrolState.CanAttack;
        totalHours = 0;
        cardId = 0;
        cardName = string.Empty;
        patrolTypeId = 0;
        patrolTypeTemplate = null;
        beginTime = 0;
        endTime = 0;
        isRiot = false;
    }

    //解锁
    public void f_Unlock()
    {
        if (state == EM_PatrolState.Lock)
            state = EM_PatrolState.CanAttack;
    }

    public void f_UpdateInfo(EM_PatrolState unlockState, int lv, int totalHours, int cardId, int patrolId, int beginTime, bool isRiot,byte isAward)
    {
        this.lv = lv;
        this.totalHours = totalHours;
        this.cardId = cardId;
        CardDT cardDt = (CardDT)glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(cardId);
        cardName = cardDt != null ? cardDt.szName : string.Empty;
        if (cardId > 0 && cardDt == null)
MessageBox.ASSERT("Update champion data does not exist，Champion Id ：" + cardId);
        this.patrolTypeId = patrolId;
        patrolTypeTemplate = (PatrolTypeDT)glo_Main.GetInstance().m_SC_Pool.m_PatrolTypeSC.f_GetSC(patrolTypeId);
        if (patrolTypeId > 0 && patrolTypeTemplate == null)
MessageBox.ASSERT("Update patrol type does not exist ， Patrol Type Id ：" + patrolTypeId);
        this.beginTime = beginTime; 
        if (beginTime != 0)
            endTime = beginTime + patrolTypeTemplate.iTime * 60 * 60;
        else
            endTime = 0;
        this.isRiot = isRiot;
        if (beginTime == 0 && isAward == 0)
        {
            if (state == EM_PatrolState.GetAward)
                eventList.Clear();
            state = EM_PatrolState.CanPatrol;
        }
        else if (beginTime == 0 && isAward > 0)
        {
            state = EM_PatrolState.GetAward;
        }
        else
        {
            state = EM_PatrolState.Patroling;
        }
    }

    //时间差
    int timeGap = 0;
    int cdTime = 0;
    private ccCallback m_Callback_CheckTimeUpdate;
    /// <summary>
    /// 巡逻中的时间检查
    /// </summary>
    public bool f_PatrolingCheckTime(long userId,int serverTime,ccCallback callbackUpdate)
    {
        m_Callback_CheckTimeUpdate = callbackUpdate;
        if (beginTime > 0 && endTime > 0)
        {
            timeGap = endTime - serverTime;
            if (timeGap < 0)
            {
                return true;
            }
            cdTime =  timeGap % (m_PatrolTypeTemplate.iEventCD * 60);
            if (cdTime == 0)
            {
                Data_Pool.m_PatrolPool.f_RequestEventByServer(userId, m_iTemplateId,(byte)eventList.Count, f_Callback_PatrolEvent);
            }
        }
        return false;
    }
    private void f_Callback_PatrolEvent(object result)
    {
        if(m_Callback_CheckTimeUpdate != null)
            m_Callback_CheckTimeUpdate(result);
    }


    private string unlockLandName;
    /// <summary>
    /// 解锁关卡名字
    /// </summary>
    public string m_szUnlockLandName
    {
        get
        {
            return unlockLandName;
        }
    }

    private int templateId;
    /// <summary>
    /// 模板Id
    /// </summary>
    public int m_iTemplateId
    {
        get
        {
            return templateId;
        }
    }

    private PatrolLandDT template;
    /// <summary>
    /// 模板数据
    /// </summary>
    public PatrolLandDT m_Template
    {
        get
        {
            return template;
        }
    }

    private EM_PatrolState state;
    /// <summary>
    /// 巡逻的状态
    /// </summary>
    public EM_PatrolState m_State
    {
        get
        {
            return state;
        }
    }

    private int lv;
    /// <summary>
    /// 等级
    /// </summary>
    public int m_iLv
    {
        get
        {
            return lv;
        }
    }

    private int totalHours;
    /// <summary>
    /// 领地总巡逻时间
    /// </summary>
    public int m_iTotalHours
    {
        get
        {
            return totalHours;
        }
    }

    private int cardId;
    /// <summary>
    /// 卡牌Id
    /// </summary>
    public int m_iCardId
    {
        get
        {
            return cardId;
        }
    }

    private string cardName;
    /// <summary>
    /// 卡牌名字
    /// </summary>
    public string m_szCardName
    {
        get
        {
            return cardName;
        }
    }

    private int patrolTypeId;
    /// <summary>
    /// 巡逻类型Id
    /// </summary>
    public int m_iPatrolTypeId
    {
        get
        {
            return patrolTypeId;
        }
    }

    private PatrolTypeDT patrolTypeTemplate;
    /// <summary>
    /// 巡逻类型模板数据
    /// </summary>
    public PatrolTypeDT m_PatrolTypeTemplate
    {
        get
        {
            return patrolTypeTemplate;
        }
    }

    private int beginTime;
    /// <summary>
    /// 巡逻开始时间戳
    /// </summary>
    public int m_iBeginTime
    {
        get
        {
            return beginTime;
        }
    }

    private int endTime;
    /// <summary>
    /// 巡逻结束时间戳
    /// </summary>
    public int m_iEndTime
    {
        get
        {
            return endTime;
        }
    }

    private bool isRiot;
    /// <summary>
    /// 是否暴动  巡逻中且属于暴动状态
    /// </summary>
    public bool m_bIsRiot
    {
        get
        {
            return state == EM_PatrolState.Patroling && isRiot;
        }
    }

    #region 相关巡逻事件

    private List<PatrolEventNode> eventList = new List<PatrolEventNode>();
    public List<PatrolEventNode> m_EventList
    {
        get
        {
            return eventList;
        }
    }

    public void f_AddEvent(CMsg_SC_PatrolEventInfoNode serverData)
    {
        PatrolEventNode node = null;
        for (int i = 0; i < eventList.Count; i++)
        {
            if (serverData.idx == i)
            {
                node = eventList[i];
                break;
            }
        }
        if (node == null)
        {
            node = new PatrolEventNode();
            eventList.Add(node);
        }
        node.f_UpdateInfo(serverData.idx,serverData.uTime,serverData.eventId,serverData.awardNum,serverData.uParam0,serverData.uParam1,serverData.uParam2,
            patrolTypeTemplate != null?patrolTypeTemplate.iTime:0,cardName);    
    }
    
    #endregion

}
