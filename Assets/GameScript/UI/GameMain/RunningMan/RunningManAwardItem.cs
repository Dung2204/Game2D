using UnityEngine;
using System.Collections.Generic;

public class RunningManAwardItem : MonoBehaviour
{
    public UILabel mTitleLabel;
    public UIGrid mAwardGrid;
    public GameObject mAwardItem;

    private ResourceCommonItemComponent mShowComponent;

    public void f_UpdateByInfo(int idx, int awardId)
    {
        List<AwardPoolDT> awardList = Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(awardId, true);
        mTitleLabel.text = string.Format(CommonTools.f_GetTransLanguage(836), (idx + 1) * GameParamConst.RMTollgateNumPreChap);
        if (mShowComponent == null)
            mShowComponent = new ResourceCommonItemComponent(mAwardGrid, mAwardItem);
        mShowComponent.f_Show(awardList);
    }
}
