using UnityEngine;
using System.Collections;

public class LegionBattleAwardItem : MonoBehaviour
{
    public UILabel mAwardName;
    public UIGrid mAwardGrid;
    public GameObject mAwardItem;
    private ResourceCommonItemComponent _awardShowComponent;
    private ResourceCommonItemComponent mAwardShowComponent
    {
        get
        {
            if (_awardShowComponent == null)
                _awardShowComponent = new ResourceCommonItemComponent(mAwardGrid, mAwardItem);
            return _awardShowComponent;
        }
    }

    public void f_UpdateByInfo(LegionBattleAwardNode info)
    {
        mAwardName.text = info.m_szAwardName;
        mAwardShowComponent.f_Show(Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(info.m_iAwardId));
    }
}

public class LegionBattleAwardNode : NBaseSCDT
{
    public LegionBattleAwardNode(int idx, int awardId)
    {
        iId = idx;
        this.idx = idx;
        this.awardId = awardId;
        if (idx == 0)
        {
            awardName = CommonTools.f_GetTransLanguage(517);
        }
        else if (idx == 1)
        {
            awardName = CommonTools.f_GetTransLanguage(518);
        }
        else if (idx == 2)
        {
            awardName = CommonTools.f_GetTransLanguage(519);
        }
        else if (idx == 3)
        {
            awardName = CommonTools.f_GetTransLanguage(520);
        }
        else if (idx == 4)
        {
            awardName = CommonTools.f_GetTransLanguage(521);
        }
        else if (idx == 5)
        {
            awardName = CommonTools.f_GetTransLanguage(522);
        }
        else
        {
            awardName = string.Empty;
        }
    }

    private int idx;
    private int awardId;
    private string awardName;

    public int m_iIdx
    {
        get
        {
            return idx;
        }
    }

    public int m_iAwardId
    {
        get
        {
            return awardId;
        }
    }

    public string m_szAwardName
    {
        get
        {
            return awardName;
        }
    }
}
