using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class CrossServerBattleRankPage : UIFramwork
{
    private UIScrollView mTabScrollView;
    private UIGrid mTabGrid;
    private GameObject mTabItem;
    private CrossServerBattleRankTab[] mTabs;

    private UIScrollView mScrollView;
    private GameObject mRankItemParent;
    private GameObject mRankItem;
     
    private UIWrapComponent _rankListWrapComponent;
    private UIWrapComponent m_RankListWrapComponent
    {
        get
        {
            if (_rankListWrapComponent == null)
            {
                m_RankList = Data_Pool.m_CrossServerBattlePool.RankDict[(byte)m_CurZoneId].RankList;
                _rankListWrapComponent = new UIWrapComponent(65, 1, 800, 10, mRankItemParent, mRankItem, m_RankList, f_RankListUpdateByInfo, null);
            }
            return _rankListWrapComponent;
        }
    }

    private GameObject mNullDataTip;
    private CrossServerBattleRankItem mSelfRankItem;

    private int m_CurZoneId;
    private int m_DefaultZoneId = 0;
    private bool m_FirstPage = false;
    private bool m_NeedUpdate = false;
    private List<BasePoolDT<long>> m_RankList;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mTabScrollView = f_GetObject("TabScrollView").GetComponent<UIScrollView>();
        mTabGrid = f_GetObject("TabGrid").GetComponent<UIGrid>();
        mTabItem = f_GetObject("TabItem");
        mScrollView = f_GetObject("ScrollView").GetComponent<UIScrollView>();
        mRankItemParent = f_GetObject("RankItemParent");
        mRankItem = f_GetObject("RankItem");
        mNullDataTip = f_GetObject("NullDataTip");
        mSelfRankItem = f_GetObject("SelfRankItem").GetComponent<CrossServerBattleRankItem>();
        mTabs = new CrossServerBattleRankTab[Data_Pool.m_CrossServerBattlePool.RankDict.Keys.Count];
        int tabIdx = 0;
        foreach (byte key in Data_Pool.m_CrossServerBattlePool.RankDict.Keys)
        {
            if (m_DefaultZoneId == 0)
                m_DefaultZoneId = Data_Pool.m_CrossServerBattlePool.RankDict[key].ZoneTemplate.iId;
            GameObject go = NGUITools.AddChild(mTabGrid.gameObject, mTabItem);
            go.SetActive(true);
            mTabs[tabIdx] = go.GetComponent<CrossServerBattleRankTab>();
            mTabs[tabIdx].f_Init(Data_Pool.m_CrossServerBattlePool.RankDict[key].ZoneTemplate);
            f_RegClickEvent(mTabs[tabIdx].m_NormalBg, f_OnZoneTabClick, Data_Pool.m_CrossServerBattlePool.RankDict[key].ZoneTemplate.iId);
            tabIdx++;
        }
        mTabGrid.repositionNow = true;
        mTabGrid.Reposition();
        mTabScrollView.ResetPosition();
        mScrollView.onDragFinished = f_MomnetEnds;

        f_RegClickEvent("BtnClose", f_OnBtnCloseClick);
        f_RegClickEvent("BlackBG", f_OnBtnCloseClick);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        mTabScrollView.ResetPosition();
        m_CurZoneId = 0;
        f_OnZoneTabClick(null, m_DefaultZoneId,null);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    private void f_OnZoneTabClick(GameObject go, object value1, object value2)
    {
        int zoneId = (int)value1;
        if (m_CurZoneId == zoneId)
            return;
        m_CurZoneId = zoneId;
        for (int i = 0; i < mTabs.Length; i++)
        {
            mTabs[i].f_UpdateByInfo(zoneId);
        }
        m_FirstPage = true;
        UITool.f_OpenOrCloseWaitTip(true,true); 
        Data_Pool.m_CrossServerBattlePool.f_ExecuteAfterRankList((byte)m_CurZoneId, m_FirstPage, ref m_NeedUpdate, f_Callback_RankList);
    }

    private void f_OnBtnCloseClick(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CrossServerBattleRankPage, UIMessageDef.UI_CLOSE);
    }

    /// <summary>
    /// 拖拉到底部处理函数
    /// </summary>
    private void f_MomnetEnds()
    {
        Vector3 constraint = mScrollView.panel.CalculateConstrainOffset(mScrollView.bounds.min, mScrollView.bounds.min);
        if (constraint.y <= 0)
        {
            m_FirstPage = false;
            UITool.f_OpenOrCloseWaitTip(true, true);
            Data_Pool.m_CrossServerBattlePool.f_ExecuteAfterRankList((byte)m_CurZoneId, m_FirstPage, ref m_NeedUpdate, f_Callback_RankList);
        }
    }

    private void f_Callback_RankList(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result != (int)eMsgOperateResult.OR_Succeed)
            MessageBox.ASSERT(string.Format(CommonTools.f_GetTransLanguage(1035), (int)result));
        if (m_FirstPage)
        {
            m_RankList = Data_Pool.m_CrossServerBattlePool.RankDict[(byte)m_CurZoneId].RankList;
            m_RankListWrapComponent.f_UpdateList(m_RankList);
            m_RankListWrapComponent.f_ResetView();
            mNullDataTip.SetActive(m_RankList.Count == 0);
            f_UpdateSelfItem();
        }
        else if (m_NeedUpdate)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1036));
            m_RankList = Data_Pool.m_CrossServerBattlePool.RankDict[(byte)m_CurZoneId].RankList;
            m_RankListWrapComponent.f_UpdateList(m_RankList);
            m_RankListWrapComponent.f_ResetView();
            mNullDataTip.SetActive(m_RankList.Count == 0);
            f_UpdateSelfItem();
        }
        else
        {
            m_RankListWrapComponent.f_UpdateView();
        }
    }

    private void f_RankListUpdateByInfo(Transform tf, BasePoolDT<long> node)
    {
        CrossServerBattleRankItem tItem = tf.GetComponent<CrossServerBattleRankItem>();
        tItem.f_UpdateByInfo(false, (CrossServerBattleRankPoolDT)node);
    }

    private void f_UpdateSelfItem()
    {
        mSelfRankItem.f_UpdateByInfo(true, Data_Pool.m_CrossServerBattlePool.SelfRankInfo);
    }
}
