using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class LegionDungeonPoolDT :BasePoolDT<long>
{
    public LegionDungeonPoolDT(LegionChapterDT template)
    {
        iId = template.iId;
        _chapterId = (byte)iId;
        mChapterTemplate = template;
        _tollgatePoolDtList.Clear();
        LegionTollgatePoolDT tollgatePoolDt;
        tollgatePoolDt = new LegionTollgatePoolDT(_chapterId,template.iTollgateId1,0);
        _tollgatePoolDtList.Add(tollgatePoolDt);
        tollgatePoolDt = new LegionTollgatePoolDT(_chapterId, template.iTollgateId2, 1);
        _tollgatePoolDtList.Add(tollgatePoolDt);
        tollgatePoolDt = new LegionTollgatePoolDT(_chapterId, template.iTollgateId3, 2);
        _tollgatePoolDtList.Add(tollgatePoolDt);
        tollgatePoolDt = new LegionTollgatePoolDT(_chapterId, template.iTollgateId4, 3);
        _tollgatePoolDtList.Add(tollgatePoolDt);
    }

    private byte _chapterId;
    public byte mChapterId
    {
        get
        {
            return _chapterId;
        }
    }

    public LegionChapterDT mChapterTemplate
    {
        private set;
        get;
    }

    private List<LegionTollgatePoolDT> _tollgatePoolDtList = new List<LegionTollgatePoolDT>();

    public void f_UpdateTollgateInfo(int idx,long hp,long killerId)
    {
        if (idx < _tollgatePoolDtList.Count)
        {
            _tollgatePoolDtList[idx].f_UpdateTollgateInfo(hp, killerId);
        }
    }

    public LegionTollgatePoolDT f_GetTollgatePoolDtByIdx(int idx)
    {
        if (idx >= 0 && idx < _tollgatePoolDtList.Count)
        {
            return _tollgatePoolDtList[idx];
        }
MessageBox.ASSERT("Subscript level exceeds the allowed level");
        return null;
    }

    public int mTotalTollgateCount
    {
        get {
            return _tollgatePoolDtList.Count;
        }
    }

    public long mTotalHp
    {
        get
        {
            long result = 0;
            for (int i = 0; i < _tollgatePoolDtList.Count; i++)
            {
                result += _tollgatePoolDtList[i].mHp;
            }
            return result;
        }
    }

    public long mTotalHpMax
    {
        get
        {
            long result = 0;
            for (int i = 0; i < _tollgatePoolDtList.Count; i++)
            {
                result += _tollgatePoolDtList[i].mHpMax;
            }
            return result;
        }
    }

}

public class LegionTollgatePoolDT
{
    public LegionTollgatePoolDT(byte chapterId,int tollgateId,int idx)
    {
        mChapterId = chapterId;
        mTollgateTemplate = (LegionTollgateDT)glo_Main.GetInstance().m_SC_Pool.m_LegionTollgateSC.f_GetSC(tollgateId);
        mCamp = (EM_CardCamp)idx + 1;
        _hp = 0;
        _hpMax = 0;
        _killerId = 0;
        MonsterDT tMonster;
        tMonster = (MonsterDT)glo_Main.GetInstance().m_SC_Pool.m_MonsterSC.f_GetSC(mTollgateTemplate.iMonster1);
        if (tMonster != null)
            _hpMax += tMonster.iHp;
        tMonster = (MonsterDT)glo_Main.GetInstance().m_SC_Pool.m_MonsterSC.f_GetSC(mTollgateTemplate.iMonster2);
        if (tMonster != null)
            _hpMax += tMonster.iHp;
        tMonster = (MonsterDT)glo_Main.GetInstance().m_SC_Pool.m_MonsterSC.f_GetSC(mTollgateTemplate.iMonster3);
        if (tMonster != null)
            _hpMax += tMonster.iHp;
        tMonster = (MonsterDT)glo_Main.GetInstance().m_SC_Pool.m_MonsterSC.f_GetSC(mTollgateTemplate.iMonster4);
        if (tMonster != null)
            _hpMax += tMonster.iHp;
        tMonster = (MonsterDT)glo_Main.GetInstance().m_SC_Pool.m_MonsterSC.f_GetSC(mTollgateTemplate.iMonster5);
        if (tMonster != null)
            _hpMax += tMonster.iHp;
        tMonster = (MonsterDT)glo_Main.GetInstance().m_SC_Pool.m_MonsterSC.f_GetSC(mTollgateTemplate.iMonster6);
        if (tMonster != null)
            _hpMax += tMonster.iHp;
    }
    
    public void f_UpdateTollgateInfo(long hp, long killerId)
    {
        _hp = hp;
        _killerId = killerId;
    }

    public LegionTollgateDT mTollgateTemplate
    {
        private set;
        get;
    }
    
    public byte mChapterId
    {
        private set;
        get;
    }

    public EM_CardCamp mCamp
    {
        private set;
        get;
    }

    private long _hp;
    public long mHp
    {
        get
        {
            return _hp;
        }
    }

    private long _hpMax;
    public long mHpMax
    {
        get
        {
            return _hpMax;
        }
    }

    private long _killerId;
    public long mKillerId
    {
        get
        {
            return _killerId;
        }
    }
}
