using UnityEngine;
using System.Collections.Generic;

public class AwardTipItem : MonoBehaviour
{
    public UIPanel mPanel;
    public UILabel mTitleLabel;
    public UIGrid mAwardItemGrid;
    public GameObject mAwardItem;
    private ResourceCommonItemComponent mShowComponent;

    public TweenPosition mPosAni;

    private EventDelegate.Callback mOnFinish;

    private bool inUse = true;
    public bool m_bInUse
    {
        get
        {
            return inUse;
        }
    }
    
    public void f_Show(int panelDepth,string title, List<AwardPoolDT> awardList,EventDelegate.Callback onFinish)
    {
        gameObject.SetActive(true);
        mOnFinish = onFinish;
        mPanel.depth = panelDepth;
        mTitleLabel.text = title;
        if (mShowComponent == null)
            mShowComponent = new ResourceCommonItemComponent(mAwardItemGrid, mAwardItem);
        inUse = true;
        mShowComponent.f_Show(awardList);
        mPosAni.SetOnFinished(f_OnFinish);
        mPosAni.ResetToBeginning();
        mPosAni.PlayForward();
    }

    private void f_OnFinish()
    {
        inUse = false;
        gameObject.SetActive(false);
        if (mOnFinish != null)
        {
            mOnFinish();
        }
    }
}
