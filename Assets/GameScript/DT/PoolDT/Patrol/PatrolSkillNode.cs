using UnityEngine;
using System.Collections;

public class PatrolSkillNode
{
    public PatrolSkillNode(PatrolLandDT landTemplate, int lvMax)
    {
        this.landId = landTemplate.iId;
        this.landTemplate = landTemplate;
        lv = 0;
        totalTime = 0;
        isLock = true;
        this.lvMax = lvMax;
        template = Data_Pool.m_PatrolPool.f_GetLandSkillTemplate(landId, lv);
        lvUpTemplate = Data_Pool.m_PatrolPool.f_GetLandSkillTemplate(landId, Mathf.Min(lv+1, lvMax));
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="lv"></param>
    public void f_UpdateByInfo(EM_PatrolState landState,int lv,int totalTime)
    {
        isLock = landState <= EM_PatrolState.CanAttack;
        if (lv > lvMax)
MessageBox.ASSERT("Skill level exceeds max level，landId:" + landId + "Lv:" + lv);
        if (this.lv != lv)
        {
            this.lv = Mathf.Min(lv, lvMax);
            template = Data_Pool.m_PatrolPool.f_GetLandSkillTemplate(landId, this.lv);
            lvUpTemplate = Data_Pool.m_PatrolPool.f_GetLandSkillTemplate(landId, Mathf.Min(lv+1, lvMax));
        }
        this.totalTime = totalTime;
    }

    private int landId;
    public int m_iLandId
    {
        get
        {
            return landId;
        }
    }
    
    private PatrolLandDT landTemplate;
    public PatrolLandDT m_LandTemplate
    {
        get
        {
            return landTemplate;
        }
    }

    private PatrolLandSkillDT template;
    public PatrolLandSkillDT m_Template
    {
        get
        {
            return template;
        }
    }

    private PatrolLandSkillDT lvUpTemplate;
    public PatrolLandSkillDT m_LvUpTemplate
    {
        get
        {
            return lvUpTemplate;
        }
    }

    private int lv;
    public int m_iLv
    {
        get
        {
            return lv;
        }
    }

    private int totalTime;
    public int m_iTotalTime
    {
        get
        {
            return totalTime;
        }
    }

    private int lvMax;
    public int m_iLvMax
    {
        get
        {
            return lvMax;
        }
    }

    private bool isLock;
    public bool m_bIsLock
    {
        get
        {
            return isLock;
        }
    }

}
