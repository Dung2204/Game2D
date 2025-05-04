using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class RunningManRankPage : UIFramwork
{
    private UIScrollView mScrollView;
    private GameObject mRankItemParent;
    private GameObject mRankItem;

    private List<BasePoolDT<long>> _rankList;
    private UIWrapComponent _rankListWrapComponent;
    private UIWrapComponent mRankListWrapComponent
    {
        get
        {
            if (_rankListWrapComponent == null)
            {
                _rankList = Data_Pool.m_RunningManPool.m_RankList;
                _rankListWrapComponent = new UIWrapComponent(105, 1, 800, 10, mRankItemParent, mRankItem, _rankList, f_RankListUpdateByInfo, null);
            }
            return _rankListWrapComponent;
        }
    }
    private GameObject mNullTip;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mScrollView = f_GetObject("ScrollView").GetComponent<UIScrollView>();
        mScrollView.onDragFinished = f_MomnetEnds;
        mRankItemParent = f_GetObject("RankItemParent");
        mRankItem = f_GetObject("RankItem");
        mNullTip = f_GetObject("NullTip");
        f_RegClickEvent("BtnCancel", f_MaskClose);
        f_RegClickEvent("BtnClose", f_MaskClose);
        f_RegClickEvent("MaskClose", f_MaskClose);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        mRankListWrapComponent.f_ResetView();
        mNullTip.SetActive(_rankList.Count == 0);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    /// <summary>
    /// 拖拉到底部处理函数
    /// </summary>
    private void f_MomnetEnds()
    {
        Debug.Log("Moment Stop");
        Vector3 constraint = mScrollView.panel.CalculateConstrainOffset(mScrollView.bounds.min, mScrollView.bounds.min);
        Debug.Log(constraint);
        if (constraint.y <= 0)
        {
            Debug.Log(CommonTools.f_GetTransLanguage(837));
            Data_Pool.m_RunningManPool.f_ExecuteAferRankList(false,f_Callback_RankList);
        }
    }

    private void f_Callback_RankList(object result)
    {
        if ((int)result != (int)eMsgOperateResult.OR_Succeed)
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(838) + result);
        mRankListWrapComponent.f_UpdateView();
    }

    private void f_RankListUpdateByInfo(Transform tf, BasePoolDT<long> dt)
    {
        RunningManRankItem tItem = tf.GetComponent<RunningManRankItem>();
        tItem.f_UpdateByInfo(dt);
    }

    private void f_MaskClose(GameObject go,object value1,object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManRankPage, UIMessageDef.UI_CLOSE);
    }
}
