using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class ArenaRankPage : UIFramwork
{
    private GameObject mCloseBtn;
    private GameObject mRankItemParent;
    private GameObject mRankItem;
    private UIWrapComponent _rankWrapComponet;
    public UIWrapComponent mRankWrapComponet
    {
        get
        {
            if (_rankWrapComponet == null)
            {
                _arenaList = Data_Pool.m_ArenaPool.f_GetRankList();
                _rankWrapComponet = new UIWrapComponent(185, 1, 185, 6, mRankItemParent,mRankItem, _arenaList, f_RankItemUpdateByInfo,null);
            }
            return _rankWrapComponet;
        }
    }
    private List<BasePoolDT<long>> _arenaList;

    private UILabel mSelfRankLabel;
    private UILabel mTargetRankLabel;
    private UILabel mAwardNullLabel;
    private GameObject mAwardItem;
    private UIGrid mAwardGrid;
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

    private UIGrid mTargetAwardGrid;
    private ResourceCommonItemComponent _targetAwardShowComponent;
    private ResourceCommonItemComponent mTargetAwardShowComponent
    {
        get
        {
            if (_targetAwardShowComponent == null)
                _targetAwardShowComponent = new ResourceCommonItemComponent(mTargetAwardGrid, mAwardItem);
            return _targetAwardShowComponent;
        }
    }


    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mRankItem = f_GetObject("ArenaRankItem");
        mRankItemParent = f_GetObject("ArenaRankItemParent");
        mSelfRankLabel = f_GetObject("SelfRankLabel").GetComponent<UILabel>();
        mTargetRankLabel = f_GetObject("TargetRankLabel").GetComponent<UILabel>();
        mAwardNullLabel = f_GetObject("AwardNullLabel").GetComponent<UILabel>();
        mAwardItem = f_GetObject("TaskAwardItem");
        mAwardItem.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        mAwardGrid = f_GetObject("SelfAwardGrid").GetComponent<UIGrid>();
        mTargetAwardGrid = f_GetObject("TargetAwardGrid").GetComponent<UIGrid>();
        f_RegClickEvent("BtnClose", f_CloseBtnHandle); 
        f_RegClickEvent("BtnClose2", f_CloseBtnHandle);
        f_RegClickEvent("MaskClose", f_CloseBtnHandle);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        mRankWrapComponet.f_ResetView();
        mSelfRankLabel.text = Data_Pool.m_ArenaPool.m_iRank.ToString();
        mTargetRankLabel.text = Data_Pool.m_ArenaPool.m_iTargetRank.ToString();
        List<AwardPoolDT> tList = Data_Pool.m_AwardPool.f_GetArenaRankAward(Data_Pool.m_ArenaPool.m_iRank);
        mAwardNullLabel.text = tList.Count > 0 ? string.Empty : CommonTools.f_GetTransLanguage(759);
        mAwardShowComponent.f_Show(tList);
        mTargetAwardShowComponent.f_Show(Data_Pool.m_AwardPool.f_GetArenaRankAward(Data_Pool.m_ArenaPool.m_iTargetRank));
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_ArenaRankPage_CLOSE);
    }

    private void f_RankItemUpdateByInfo(Transform tf, BasePoolDT<long> dt)
    {
        ArenaRankItem item = tf.GetComponent<ArenaRankItem>();
        item.f_UpdateByInfo(dt);
    }

    private void f_CloseBtnHandle(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ArenaRankPage, UIMessageDef.UI_CLOSE);
    }
}