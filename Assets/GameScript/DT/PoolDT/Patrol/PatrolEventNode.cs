using UnityEngine;
using System.Collections;

public class PatrolEventNode
{
    public void f_UpdateInfo(int idx,uint time,byte eventId,int awardNum,int param0,int param1,int param2,int patrolTime,string cardName)
    {
        this.idx = idx;
        eventTime = time;
        this.eventId = eventId;
        this.awardNum = awardNum;
        this.param0 = param0;
        this.param1 = param1;
        this.param2 = param2;
        if (awardNum == 0 || eventId == GameParamConst.PatrolEventPacify)
        {
            awardDt = null;
            isSkillDouble = false;
            awardMultiple = 0;
        }
        else
        {
            awardDt = new ResourceCommonDT();
            awardDt.f_UpdateInfo((byte)param0, param1, awardNum);
            int eventMult = param2 % 10;
            int skillMult = param2 / 10;
            awardMultiple = 1 + eventMult + skillMult;
            isSkillDouble  = skillMult > 0;
        }
        this.patrolTime = patrolTime;
        this.cardName = cardName;
        friendId = 0;
        friendName = string.Empty;
        eventTemplate = (PatrolEventDT)glo_Main.GetInstance().m_SC_Pool.m_PatrolEventSC.f_GetSC((int)eventId);
        if (eventTemplate == null)
MessageBox.ASSERT("Server sent null Id, Id:" + eventId);
        else
            f_FillDesc(eventTemplate.szDes); 
        if (eventId == GameParamConst.PatrolEventPacify)
        {
            friendId = CommonTools.f_TwoInt2Long((uint)param0, (uint)param1);
            if (friendId == Data_Pool.m_UserData.m_iUserId)
            {
                friendName = Data_Pool.m_UserData.m_szRoleName;
                if (eventTemplate != null)
                    f_FillDesc(eventTemplate.szDes);
            }
            else
                Data_Pool.m_GeneralPlayerPool.f_ReadInfor(friendId, EM_ReadPlayerStep.Base, f_Callback_ReadInfo);
        }
    }

    private int idx;
    //索引
    public int m_iIdx
    {
        get
        {
            return idx;
        }
    }

    private uint eventTime;
    /// <summary>
    /// 事件触发时间
    /// </summary>
    public uint m_iEventTime
    {
        get
        {
            return eventTime;
        }
    }


    private int eventId;
    public int m_iEventId
    {
        get
        {
            return eventId;
        }
    }

    private PatrolEventDT eventTemplate;

    private int awardNum;

    private int param0;

    private int param1;

    private int param2;

    /// <summary>
    /// 是否技能翻倍
    /// </summary>
    private bool isSkillDouble;
    public bool m_bIsSkillDouble
    {
        get
        {
            return isSkillDouble;
        }
    }

    private int awardMultiple;
    /// <summary>
    /// 奖励倍数
    /// </summary>
    public int m_iAwardMultiple
    {
        get
        {
            return awardMultiple;
        }
    }

    private string eventDesc;
    public string m_szEventDesc
    {
        get
        {
            return eventDesc;
        }
    }

    private string cardName;

    private ResourceCommonDT awardDt;

    /// <summary>
    /// 奖励数据 有可能为空
    /// </summary>
    public ResourceCommonDT m_AwardDt
    {
        get
        {
            return awardDt;
        }
    }

    //巡逻时长
    private int patrolTime;

    private long friendId;
    private string friendName;
    public string m_szFriendName
    {
        get
        {
            return friendName;
        }
    }

    private void f_Callback_ReadInfo(object value)
    {
        BasePlayerPoolDT playerInfo = (BasePlayerPoolDT)value;
        friendName = playerInfo.m_szName;
        if (eventTemplate != null)
        {
            f_FillDesc(eventTemplate.szDes);
            //请求玩家名字后发事件刷新
            glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_PATROLEVENT_UPDATE);
        }
            
    }

    private void f_FillDesc(string source)
    {
        eventDesc = source;

        eventDesc = eventDesc.Replace(GameParamConst.PatrolReplaceCardName, cardName);

        if (eventId == GameParamConst.PatrolEventPacify)
        {
eventDesc = eventDesc.Replace(GameParamConst.PatrolReplaceAward, "Spirit x2");
        }
        else
        {
            if (awardDt != null)
eventDesc = eventDesc.Replace(GameParamConst.PatrolReplaceAward, param2 > 0 ? string.Format("{0}x{1}({2} times)", awardDt.mName, awardDt.mResourceNum,awardMultiple) :
                                                            string.Format("{0}x{1}", awardDt.mName, awardDt.mResourceNum));
            else
                eventDesc = eventDesc.Replace(GameParamConst.PatrolReplaceAward, string.Empty);
        } 
        eventDesc = eventDesc.Replace(GameParamConst.PatrolReplaceFriendName, friendName);
        
eventDesc = eventDesc.Replace(GameParamConst.PatrolReplacePatrolTime,string.Format("{0} hours",patrolTime));
        System.DateTime tEventTime = ccMath.time_t2DateTime(eventTime);
        eventDesc = string.Format("[FF8830][{0:d2}:{1:d2}:{2:d2}][-] {3}", tEventTime.Hour,tEventTime.Minute,tEventTime.Second,eventDesc);
    }
}
