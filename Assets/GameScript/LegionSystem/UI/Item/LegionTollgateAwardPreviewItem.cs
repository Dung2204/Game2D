using UnityEngine;
using System.Collections;

public class LegionTollgateAwardPreviewItem : MonoBehaviour 
{
    public GameObject mAwardItem;
    public GameObject mAwardParent;
    private ResourceCommonItem mShowItem;
    public UILabel mCountShow;
    public void f_UpdateByInfo(ccU3DEngine.BasePoolDT<long> dt)
    {
        LegionTollgateAwardCountPoolDT info = (LegionTollgateAwardCountPoolDT)dt;
        if (mShowItem == null)
            mShowItem = ResourceCommonItem.f_Create(mAwardParent, mAwardItem);
        mShowItem.f_UpdateByInfo(info.m_cAwardTemplate.m_iAwardType, info.m_cAwardTemplate.m_iAwardId, info.m_cAwardTemplate.m_iAwardCount);
        int leftCount = info.m_iTotalCount - info.m_iAlreadyGetCount;
        mCountShow.text = string.Format("{0}/{1}", leftCount < 0 ? 0 : leftCount, info.m_iTotalCount);
    }
}
